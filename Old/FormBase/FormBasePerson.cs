using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using unvell.ReoGrid;
using WMS.DataAccess;
using System.Threading;
using System.Data.SqlClient;

namespace WMS.UI.FormBase
{
    public partial class FormBasePerson : Form
    {
        private PagerWidget<PersonView> pagerWidget = null;
        private SearchWidget<PersonView> searchWidget = null;
        private WMSEntities wmsEntities = new WMSEntities();
        public FormBasePerson()
        {
            InitializeComponent();
        }

        private void InitComponents()
        {
            string[] visibleColumnNames = (from kn in BasePersonMetaData.KeyNames
                                           where kn.Visible == true
                                           select kn.Name).ToArray();

            //初始化
            //this.toolStripComboBoxSelect.Items.Add("无");
            //this.toolStripComboBoxSelect.Items.AddRange(visibleColumnNames);
            //this.toolStripComboBoxSelect.SelectedIndex = 0;


            this.pagerWidget = new PagerWidget<PersonView>(this.reoGridControlPerson, BasePersonMetaData.KeyNames);
            this.panelPager.Controls.Add(pagerWidget);
            pagerWidget.Show();

            this.searchWidget = new SearchWidget<PersonView>(BasePersonMetaData.KeyNames, this.pagerWidget);
            this.panelSearchWidget.Controls.Add(searchWidget);
        }

        private void FormBasePerson_Load(object sender, EventArgs e)
        {
            InitComponents();
            this.Search();
        }

        private void Search()
        {
            this.pagerWidget.ClearCondition();
            this.pagerWidget.ClearStaticCondition();

            if (this.checkBoxOnlyThisProAndWare.Checked == true)
            {
                this.pagerWidget.AddStaticCondition("ProjectID", Convert.ToString(GlobalData.ProjectID));
                this.pagerWidget.AddStaticCondition("WarehouseID", Convert.ToString(GlobalData.WarehouseID));
            }
            //if (this.toolStripComboBoxSelect.SelectedIndex != 0)
            //{
            //    this.pagerWidget.AddCondition(this.toolStripComboBoxSelect.SelectedItem.ToString(), this.toolStripTextBoxSelect.Text);
            //}
            this.searchWidget.Search();
        }

        //private void toolStripButtonSelect_Click(object sender, EventArgs e)
        //{
        //    this.pagerWidget.ClearCondition();
        //    if (this.toolStripComboBoxSelect.SelectedIndex != 0)
        //    {
        //        this.pagerWidget.AddCondition(this.toolStripComboBoxSelect.SelectedItem.ToString(), this.toolStripTextBoxSelect.Text);
        //    }
        //    this.Search();
        //}

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            var formBasePersonModify = new FormBasePersonModify();
            formBasePersonModify.SetMode(FormMode.ADD);

            formBasePersonModify.SetAddFinishedCallback((addedID) =>
            {
                this.pagerWidget.Search(false, addedID);
                var worksheet = this.reoGridControlPerson.Worksheets[0];
            });
            formBasePersonModify.Show();
        }

        
        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlPerson.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int personID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                var formBasePersonModify = new FormBasePersonModify(personID);
                formBasePersonModify.SetModifyFinishedCallback((addedID) =>
                {
                    this.pagerWidget.Search(false, addedID);
                });
                formBasePersonModify.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlPerson.Worksheets[0];
            List<int> deleteIDs = new List<int>();
            for (int i = 0; i < worksheet.SelectionRange.Rows; i++)
            {
                try
                {
                    int curID = int.Parse(worksheet[i + worksheet.SelectionRange.Row, 0].ToString());
                    deleteIDs.Add(curID);
                }
                catch
                {
                    continue;
                }
            }
            if (deleteIDs.Count == 0)
            {
                MessageBox.Show("请选择您要删除的记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("您真的要删除这些记录吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            this.labelStatus.Text = "正在删除...";

            try
            {
                foreach (int id in deleteIDs)
                {

                    var call = (from kn in wmsEntities.JobTicketItem
                                                 where kn.PersonID == id
                                                 select kn.ID).ToArray();
                    if (call.Length > 0)
                    {
                        MessageBox.Show("删除失败，选择的人员信息被作业单条目引用，需要删除相应作业单信息才能删除。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }

                }

            }
            catch
            {
                MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                foreach (int id in deleteIDs)
                {

                    var call = (from kn in wmsEntities.PutOutStorageTicket
                                where kn.PersonID == id
                                select kn.ID).ToArray();
                    if (call.Length > 0)
                    {
                        MessageBox.Show("删除失败，选择的人员信息被出库单引用，需要删除相应出库单信息才能删除。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }

                }

            }
            catch
            {
                MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                foreach (int id in deleteIDs)
                {

                    var call = (from kn in wmsEntities.PutOutStorageTicketItem
                                where kn.JobPersonID == id
                                 ||kn.ConfirmPersonID==id
                                select kn.ID).ToArray();
                    if (call.Length > 0)
                    {
                        MessageBox.Show("删除失败，选择的人员信息被出库单零件条目引用，需要删除相应出库单信息才能删除。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }

                }

            }
            catch
            {
                MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                foreach (int id in deleteIDs)
                {

                    var receiptTicketcall = (from kn in wmsEntities.ReceiptTicket
                                where kn.PersonID == id
                                select kn.ID).ToArray();
                    var putOutStorageTicketItemcall = (from kn in wmsEntities.PutOutStorageTicketItem
                                where kn.JobPersonID == id
                                 || kn.ConfirmPersonID == id
                                select kn.ID).ToArray();
                    if (receiptTicketcall.Length > 0|| putOutStorageTicketItemcall.Length>0)
                    {
                        MessageBox.Show("删除失败，选择的人员信息被收货单引用，需要删除相应收货单信息才能删除。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }

                }

            }
            catch
            {
                MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                foreach (int id in deleteIDs)
                {

                    var submissionTicketcall = (from kn in wmsEntities.SubmissionTicket
                                where kn.PersonID == id
                                select kn.ID).ToArray();
                    var submissionTicketItemcall = (from kn in wmsEntities.SubmissionTicketItem
                                where kn.JobPersonID == id
                                ||kn.ConfirmPersonID==id
                                select kn.ID).ToArray();
                    if (submissionTicketcall.Length > 0|| submissionTicketItemcall.Length>0)
                    {
                        MessageBox.Show("删除失败，选择的人员信息被送检单引用，需要删除相应送检单信息才能删除。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }

                }

            }
            catch
            {
                MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                foreach (int id in deleteIDs)
                {

                    var putawayTicketcall = (from kn in wmsEntities.PutawayTicket
                                                where kn.PersonID == id
                                                select kn.ID).ToArray();
                    var putawayTicketItemcall = (from kn in wmsEntities.PutawayTicketItem
                                                    where kn.JobPersonID == id
                                                    || kn.ConfirmPersonID == id
                                                    select kn.ID).ToArray();
                    if (putawayTicketcall.Length > 0 || putawayTicketItemcall.Length > 0)
                    {
                        MessageBox.Show("删除失败，选择的人员信息被上架单引用，需要删除相应上架单信息才能删除。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }

                }

            }
            catch
            {
                MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                foreach (int id in deleteIDs)
                {

                    var call = (from kn in wmsEntities.StockInfoCheckTicket
                                where kn.PersonID == id
                                select kn.ID).ToArray();
                    if (call.Length > 0)
                    {
                        MessageBox.Show("删除失败，选择的人员信息被库存盘点信息引用，需要删除相应库存盘点信息才能删除。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }

                }

            }
            catch
            {
                MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            new Thread(new ThreadStart(() =>
            {
                try
                {
                    foreach (int id in deleteIDs)
                    {
                        this.wmsEntities.Database.ExecuteSqlCommand("DELETE FROM Person WHERE ID = @personID", new SqlParameter("personID", id));
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.wmsEntities.SaveChanges();
                this.Invoke(new Action(() =>
                {
                    this.Search();
                    MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
            })).Start();
        }

        //private void toolStripTextBoxSelect_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == 13)
        //    {
        //        this.toolStripButtonSelect.PerformClick();
        //    }
        //}

        //private void toolStripComboBoxSelect_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (this.toolStripComboBoxSelect.SelectedIndex == 0)
        //    {
        //        this.toolStripTextBoxSelect.Text = "";
        //        this.toolStripTextBoxSelect.Enabled = false;
        //        this.toolStripTextBoxSelect.BackColor = Color.LightGray;

        //    }
        //    else
        //    {
        //        this.toolStripTextBoxSelect.Enabled = true;
        //        this.toolStripTextBoxSelect.BackColor = Color.White;
        //    }
        //}

        private void checkBoxOnlyThisProAndWare_CheckedChanged(object sender, EventArgs e)
        {
            this.Search();
        }
    }
}

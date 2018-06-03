using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.DataAccess;
using unvell.ReoGrid;
using System.Threading;
using System.Data.SqlClient;

namespace WMS.UI
{
    public partial class FormJobTicket : Form
    {
        WMSEntities wmsEntities = new WMSEntities();
        private int userID = -1;
        private int projectID = -1;
        private int warehouseID = -1;

        private PagerWidget<JobTicketView> pagerWidget = null;
        private SearchWidget<JobTicketView> searchWidget = null;

        private Action<string,string> toPutOutStorageTicketCallback = null;

        public void SetToPutOutStorageTicketCallback(Action<string,string> callback)
        {
            this.toPutOutStorageTicketCallback = callback;
        }

        public FormJobTicket(int userID,int projectID,int warehouseID)
        {
            InitializeComponent();
            this.userID = userID;
            this.projectID = projectID;
            this.warehouseID = warehouseID;
            InitComponents();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private void FormJobTicket_Load(object sender, EventArgs e)
        {
            this.Search();
        }

        private void InitComponents()
        {
            string[] visibleColumnNames = (from kn in JobTicketViewMetaData.KeyNames
                                           where kn.Visible == true
                                           select kn.Name).ToArray();

            this.pagerWidget = new PagerWidget<JobTicketView>(this.reoGridControlMain, JobTicketViewMetaData.KeyNames, this.projectID, this.warehouseID);
            this.panelPagerWidget.Controls.Add(this.pagerWidget);
            this.pagerWidget.Show();

            this.searchWidget = new SearchWidget<JobTicketView>(JobTicketViewMetaData.KeyNames, pagerWidget);
            this.panelSearchWidget.Controls.Add(searchWidget);
        }

        public void SetSearchCondition(string key,string value)
        {
            this.searchWidget.SetSearchCondition(key, value);
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            if(ids.Length != 1)
            {
                MessageBox.Show("请选择一项进行查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            FormJobTicketItem formJobTicketItem = new FormJobTicketItem(ids[0]);
            formJobTicketItem.SetJobTicketStateChangedCallback(new Action(() =>
            {
                this.Invoke(new Action(()=>
                {
                    this.Search(true);
                }));
            }));
            formJobTicketItem.Show();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            if(ids.Length == 0)
            {
                MessageBox.Show("请选择要删除的项目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            WMSEntities wmsEntities = new WMSEntities();
            //如果被出库单引用了，不能删除
            try
            {
                StringBuilder sbIDArray = new StringBuilder();
                foreach (int id in ids)
                {
                    sbIDArray.Append(id + ",");
                }
                sbIDArray.Length--;
                int countRef = wmsEntities.Database.SqlQuery<int>(string.Format("SELECT COUNT(*) FROM PutOutStorageTicket WHERE JobTicketID IN ({0})", sbIDArray.ToString())).Single();
                if (countRef > 0)
                {
                    MessageBox.Show("删除失败，作业单被出库单引用，请先删除出库单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch
            {
                MessageBox.Show("操作失败，请检查您的网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("确定要删除选中项吗？\n重要提示：\n删除后所有零件的分配翻包数量将会退回发货单中，无视实际翻包数量！\n请谨慎操作！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question)!= DialogResult.Yes)
            {
                return;
            }
            this.labelStatus.Text = "正在删除";
            new Thread(new ThreadStart(()=>
            {
                try
                {
                    List<int> shipmentTicketIDs = new List<int>();
                    foreach (int id in ids)
                    {
                        JobTicket jobTicket = (from j in wmsEntities.JobTicket where j.ID == id select j).FirstOrDefault();
                        if (jobTicket.State != JobTicketViewMetaData.STRING_STATE_UNFINISHED)
                        {
                            MessageBox.Show("删除失败，只能删除未完成翻包的作业单！\n如果需要强行删除，请使用修改功能将单据状态改为未完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        foreach (JobTicketItem jobTicketItem in jobTicket.JobTicketItem)
                        {
                            ShipmentTicketItem shipmentTicketItem = (from s in wmsEntities.ShipmentTicketItem where s.ID == jobTicketItem.ShipmentTicketItemID select s).FirstOrDefault();
                            if (shipmentTicketItem == null) continue;
                            //如果删除作业单，则发货单直接把计划翻包数量的全部数量加回来，无视实际翻包数量
                            shipmentTicketItem.ScheduledJobAmount -= ((jobTicketItem.ScheduledAmount ?? 0) * (jobTicketItem.UnitAmount ?? 1)) / (shipmentTicketItem.UnitAmount ?? 1);
                        }
                        if (jobTicket.ShipmentTicketID != null)
                        {
                            shipmentTicketIDs.Add(jobTicket.ShipmentTicketID.Value);
                        }
                        wmsEntities.Database.ExecuteSqlCommand(string.Format("DELETE FROM JobTicket WHERE ID = {0}", id));
                    }
                    wmsEntities.SaveChanges();
                    foreach(int shipmentTicketID in shipmentTicketIDs)
                    {
                        ShipmentTicketUtilities.UpdateShipmentTicketStateSync(shipmentTicketID,wmsEntities);
                    }
                }
                catch
                {
                    MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Invoke(new Action(()=>
                {
                    this.Search(true);
                }));
            })).Start();
        }

        private void buttonGeneratePutOutStorageTicket_Click(object sender, EventArgs e)
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            WMSEntities wmsEntities = new WMSEntities();

            foreach (int jobTicketID in ids)
            {
                JobTicket jobTicket = (from j in wmsEntities.JobTicket where j.ID == jobTicketID select j).FirstOrDefault();
                if (jobTicket == null)
                {
                    MessageBox.Show("选中作业单不存在，可能已被删除，请重新查询！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (jobTicket.State != JobTicketViewMetaData.STRING_STATE_ALL_FINISHED)
                {
                    if (MessageBox.Show("选中作业单未全部完成，确定生成出库单吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        return;
                    }
                }
                FormPutOutStorageTicketNew form = new FormPutOutStorageTicketNew(jobTicketID, this.userID, this.projectID, this.warehouseID);
                form.SetToPutOutStorageTicketCallback(this.toPutOutStorageTicketCallback);
                form.Show();
            }
        }

        private void buttonAlter_Click(object sender, EventArgs e)
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            if(ids.Length != 1)
            {
                MessageBox.Show("请选择一项进行修改","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            FormJobTicketModify formJobTicketModify = new FormJobTicketModify(this.userID,ids[0]);
            formJobTicketModify.SetModifyFinishedCallback(new Action(()=>
            {
                this.Search(true);
            }));
            formJobTicketModify.Show();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            this.Search();
        }

        private void Search(bool savePage = false,int selectID = -1)
        {
            this.searchWidget.Search();
        }

        private void textBoxSearchValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.Search();
            }
        }

        private void buttonToPutOutStorageTicket_Click(object sender, EventArgs e)
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            if (ids.Length != 1)
            {
                MessageBox.Show("请选择一项进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int id = ids[0];
            new Thread(() =>
            {
                try
                {
                    WMSEntities wmsEntities = new WMSEntities();
                    JobTicket jobTicket = (from s in wmsEntities.JobTicket
                                                     where s.ID == id
                                                     select s).FirstOrDefault();
                    if (jobTicket == null)
                    {
                        MessageBox.Show("作业单不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    this.toPutOutStorageTicketCallback("JobTicketJobTicketNo",jobTicket.JobTicketNo);
                }
                catch (Exception)
                {
                    MessageBox.Show("查询失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }).Start();

        }

        private void toolStripTop_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonGeneratePutOutStorageTicket_MouseDown(object sender, MouseEventArgs e)
        {
            //右键全部生成出库单
            if (e.Button == MouseButtons.Right)
            {
                int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
                if (ids.Length == 0)
                {
                    MessageBox.Show("请选择要生成出库单的项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("确定为所有选中作业单满额生成出库单吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
                WMSEntities wmsEntities = new WMSEntities();
                if(PutOutStorageTicketUitilities.GeneratePutOutStorageTicketFullSync(ids, wmsEntities))
                {
                    MessageBox.Show("生成成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    return;
                }
            }
        }
    }
}

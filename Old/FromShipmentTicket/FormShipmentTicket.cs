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
using System.Reflection;
using System.Data.SqlClient;

namespace WMS.UI
{
    public partial class FormShipmentTicket : Form
    {
        PagerWidget<ShipmentTicketView> pagerWidget = null;
        SearchWidget<ShipmentTicketView> searchWidget = null;
        WMSEntities wmsEntities = new WMSEntities();
        int userID = -1;
        int projectID = -1;
        int warehouseID = -1;

        private Action<string,string> toJobTicketCallback = null; //参数：查询条件，查询值

        public void SetToJobTicketCallback(Action<string,string> callback)
        {
            this.toJobTicketCallback = callback;
        }

        public FormShipmentTicket(int userID,int projectID,int warehouseID)
        {
            InitializeComponent();
            this.userID = userID;
            this.projectID = projectID;
            this.warehouseID = warehouseID;
        }

        private void FormShipmentTicket_Load(object sender, EventArgs e)
        {
            InitComponents();
            this.Search();
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

        private void InitComponents()
        {
            //设置人员选择默认岗位为发货员
            FormSelectPerson.DefaultPosition = FormBase.Position.SHIPMENT;

            string[] visibleColumnNames = (from kn in ShipmentTicketViewMetaData.KeyNames
                                           where kn.Visible == true
                                           select kn.Name).ToArray();
            this.pagerWidget = new PagerWidget<ShipmentTicketView>(this.reoGridControlMain, ShipmentTicketViewMetaData.KeyNames, this.projectID, this.warehouseID);
            this.panelPagerWidget.Controls.Add(pagerWidget);
            this.pagerWidget.Show();

            this.searchWidget = new SearchWidget<ShipmentTicketView>(ShipmentTicketViewMetaData.KeyNames, this.pagerWidget);
            this.panelSearchWidget.Controls.Add(searchWidget);
        }

        private void Search(bool savePage = false,int selectID = -1)
        {
            this.searchWidget.Search(savePage, selectID);
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlMain.Worksheets[0];
            try
            {
                int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
                if(ids.Length != 1)
                {
                    MessageBox.Show("请选择一项进行查看！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int shipmentTicketID = ids[0];
                var formShipmentTicketItem = new FormShipmentTicketItem(shipmentTicketID,this.projectID,this.warehouseID);
                formShipmentTicketItem.SetShipmentTicketStateChangedCallback(() =>
                {
                    this.Invoke(new Action(() =>
                    {
                        this.Search(true);
                    }));
                });
                formShipmentTicketItem.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormShipmentTicketModify(this.projectID,this.warehouseID,this.userID);
            form.SetMode(FormMode.ADD);
            form.SetAddFinishedCallback((id,openTicket) =>
            {
                this.Search(false,id);
                if (openTicket == false) return;
                var formShipmentTicketItem = new FormShipmentTicketItem(id, this.projectID, this.warehouseID);
                formShipmentTicketItem.SetShipmentTicketStateChangedCallback(() =>
                {
                    this.Invoke(new Action(()=>
                    {
                        this.Search();
                    }));
                });
                formShipmentTicketItem.Show();

            });
            form.Show();
        }

        private void buttonAlter_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlMain.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int shipmentTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                var formShipmentTicketModify = new FormShipmentTicketModify(this.projectID,this.warehouseID,this.userID,shipmentTicketID);
                formShipmentTicketModify.SetModifyFinishedCallback((id) =>
                {
                    this.Search(true,id);
                });
                formShipmentTicketModify.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int[] deleteIDs = this.GetSelectedIDs();
            if (deleteIDs.Length == 0)
            {
                MessageBox.Show("请选择您要删除的记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            WMSEntities wmsEntities = new WMSEntities();
            //如果被作业单引用了，不能删除
            try
            {
                StringBuilder sbIDArray = new StringBuilder();
                foreach(int id in deleteIDs)
                {
                    sbIDArray.Append(id + ",");
                }
                sbIDArray.Length--;
                int countRef = wmsEntities.Database.SqlQuery<int>(string.Format("SELECT COUNT(*) FROM JobTicket WHERE ShipmentTicketID IN ({0})",sbIDArray.ToString())).Single();
                if (countRef > 0)
                {
                    MessageBox.Show("删除失败，发货单被作业单引用，请先删除相应作业单！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch
            {
                MessageBox.Show("操作失败，请检查您的网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (MessageBox.Show("您真的要删除这些记录吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            this.labelStatus.Text = "正在删除...";


            new Thread(new ThreadStart(() =>
            {
                try
                {
                    foreach (int ticketID in deleteIDs)
                    {
                        int[] itemIDs = (from s in wmsEntities.ShipmentTicketItem where s.ShipmentTicketID == ticketID select s.ID).ToArray();
                        if (ShipmentTicketUtilities.DeleteItemsSync(itemIDs,out string errorMessage) == false)
                        {
                            MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        wmsEntities.Database.ExecuteSqlCommand("DELETE FROM ShipmentTicket WHERE ID = @shipmentTicketID", new SqlParameter("shipmentTicketID", ticketID));
                    }
                    wmsEntities.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("删除失败，请检查网络连接","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Invoke(new Action(() => { this.Search(true); }));
            })).Start();
        }

        private void buttonGenerateJobTicket_Click(object sender, EventArgs e)
        {
            int[] ids = this.GetSelectedIDs();
            foreach (int shipmentTicketID in ids)
            {
                FormJobTicketNew form = new FormJobTicketNew(shipmentTicketID, this.userID, this.projectID, this.warehouseID);
                form.SetToJobTicketCallback(this.toJobTicketCallback);
                form.Show();
            }
        }

        private int[] GetSelectedIDs()
        {
            var worksheet = this.reoGridControlMain.Worksheets[0];
            List<int> ids = new List<int>();
            for (int i = 0; i < worksheet.SelectionRange.Rows; i++)
            {
                try
                {
                    int curID = int.Parse(worksheet[i + worksheet.SelectionRange.Row, 0].ToString());
                    ids.Add(curID);
                }
                catch
                {
                    continue;
                }
            }
            return ids.ToArray();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            this.Search();
        }

        private void textBoxSearchValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                this.Search();
            }
        }

        private void toolStripButtonToJobTicket_Click(object sender, EventArgs e)
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            if(ids.Length != 1)
            {
                MessageBox.Show("请选择一项进行操作","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int id = ids[0];
            new Thread(()=>
            {
                try
                {
                    WMSEntities wmsEntities = new WMSEntities();
                    ShipmentTicket shipmentTicket = (from s in wmsEntities.ShipmentTicket
                                                     where s.ID == id
                                                     select s).FirstOrDefault();
                    if (shipmentTicket == null)
                    {
                        MessageBox.Show("发货单不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    this.toJobTicketCallback("ShipmentTicketNo",shipmentTicket.No);
                }
                catch (Exception)
                {
                    MessageBox.Show("查询失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }).Start();

        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
 
        }

        private void buttonGenerateJobTicket_MouseDown(object sender, MouseEventArgs e)
        {
            //按下右键，选中发货单全部生成作业单
            if(e.Button == MouseButtons.Right)
            {
                int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
                if (ids.Length == 0)
                {
                    MessageBox.Show("请选择要生成翻包作业单的项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("确定为所有选中发货单满额生成翻包作业单吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) 
                {
                    return;
                }
                WMSEntities wmsEntities = new WMSEntities();
                DateTime createTime = DateTime.Now;
                bool hasSucceededItem = false;
                foreach (int id in ids)
                {
                    hasSucceededItem |= JobTicketUtilities.GenerateJobTicketFullSync(id, wmsEntities, createTime);
                }
                if (hasSucceededItem == false)
                {
                    return;
                }
                wmsEntities.SaveChanges();
                MessageBox.Show("生成成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }
    }
}

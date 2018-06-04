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

namespace WMS.UI
{
    public partial class FormStockInfoCheckTicket : Form
    {
        private WMSEntities wmsEntities = new WMSEntities();
        int projectID = -1;
        int warehouseID = -1;
        int userID = -1;
        private int  personid=-1;
        private int checkid = -1;
        private PagerWidget<StockInfoCheckTicketView > pagerWidget = null;
        SearchWidget<StockInfoCheckTicketView> searchWidget = null;
        public FormStockInfoCheckTicket(int projectID, int warehouseID,int userID)
        {
            this.projectID = projectID;
            this.warehouseID = warehouseID;
            this.userID = userID;
            InitializeComponent();
        }
         
        private void FormStockInfoCheckTicket_Load(object sender, EventArgs e)
        {
            InitComponents();
            this.searchWidget.Search();
        }
        private void InitComponents()
        {
            this.wmsEntities.Database.Connection.Open();

            string[] visibleColumnNames = (from kn in StockInfoCheckTicketViewMetaData.KeyNames
                                           where kn.Visible == true
                                           select kn.Name).ToArray();

            //初始化查询框
            //this.comboBoxSearchCondition.Items.Add("无");
            //this.comboBoxSearchCondition.Items.AddRange(visibleColumnNames);
            //this.comboBoxSearchCondition.SelectedIndex = 0;

            //初始化分页控件
            this.pagerWidget = new PagerWidget<StockInfoCheckTicketView >(this.reoGridControlMain , StockInfoCheckTicketViewMetaData.KeyNames, this.projectID, this.warehouseID);
            this.paperpanel.Controls.Add(pagerWidget);
            pagerWidget.Show();
            this.searchWidget = new SearchWidget<StockInfoCheckTicketView>(StockInfoCheckTicketViewMetaData.KeyNames, this.pagerWidget);
            this.panelSearchWidget.Controls.Add(searchWidget);
        }

       

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            //this.pagerWidget.ClearCondition();
            //if (this.comboBoxSearchCondition.SelectedIndex != 0)
            //{
            //    this.pagerWidget.AddCondition(this.comboBoxSearchCondition.SelectedItem.ToString(), this.textBoxSearchValue.Text);
            //}
            //this.pagerWidget.Search();
        }

        

       

        private void textBoxSearchValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == 13)
            //{
            //    if (this.comboBoxSearchCondition.SelectedIndex != 0)
            //    {
            //        this.pagerWidget.AddCondition(this.comboBoxSearchCondition.SelectedItem.ToString(), this.textBoxSearchValue.Text);
            //    }
            //    this.pagerWidget.Search();
            //}
        }

        private void comboBoxSearchCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.comboBoxSearchCondition.SelectedIndex == 0)
            //{
            //    this.textBoxSearchValue.Text = "";
            //    this.textBoxSearchValue.Enabled = false;
               
            //}
            //else
            //{
            //    this.textBoxSearchValue.Enabled = true;
            //}
        }

        private void buttonAdd_Click_1(object sender, EventArgs e)
        {

            var form = new FormStockInfoCheckTicketModify(this.projectID, this.warehouseID,this.userID);
            form.SetMode(FormMode.ADD);
            form.SetAddFinishedCallback((AddID) =>
            {

                this.searchWidget.Search(false  ,AddID  );
                MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information );
              

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
                int stockInfoCheckID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                var a1 = new FormStockInfoCheckTicketModify (this.projectID, this.warehouseID,this.userID,stockInfoCheckID);
                a1 .SetMode(FormMode.ALTER);
                a1.SetModifyFinishedCallback((AlterID) =>
                {
                    this.pagerWidget.Search(false ,AlterID  );

                    MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information );
                   
                });
                a1.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void buttonDelete_Click_1(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlMain.Worksheets[0];
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


            new Thread(new ThreadStart(() =>
            {
                foreach (int id in deleteIDs)
                {
                    this.wmsEntities.Database.ExecuteSqlCommand("DELETE FROM StockInfoCheckTicket WHERE ID = @stockCheckID", new SqlParameter("stockCheckID", id));
                }
                this.wmsEntities.SaveChanges();
                this.Invoke(new Action(() =>
                {
                    this.searchWidget.Search();
                    this.labelStatus.Text = "搜索完成";
                }));
            })).Start();
        }

        

        private void button_additeam_Click(object sender, EventArgs e)
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);

            if (ids.Length != 1)
            {
                MessageBox.Show("请选择一项");
                return;
            }


            else if ((ids.Length == 1))
            {
                
                

                int stockiofocheckid = ids[0];
                 wmsEntities = new WMSEntities();
                var personid =(from kn in wmsEntities .StockInfoCheckTicketView where 
                                   kn.ID ==stockiofocheckid
                                select kn.PersonID).FirstOrDefault() ;

                this.personid = Convert .ToInt32 ( personid);

                var a1 = new FormStockInfoCheckTicketComponentModify(this.projectID , this.warehouseID ,this.userID ,this.personid , stockiofocheckid);
                StockInfoCheckTicket stockInfoCheck1 = null;
                try
                {
                    wmsEntities = new WMSEntities();
                    stockInfoCheck1 = (from s in wmsEntities.StockInfoCheckTicket
                                       where s.ID == stockiofocheckid
                                       select s).FirstOrDefault();
                }
                catch
                {
                    MessageBox.Show("加载盘点单数据失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    this.Close();
                    return;
                }
                if (stockInfoCheck1 == null)
                {
                    MessageBox.Show("要查看的项目已不存在，请确认后操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }





                a1.SetAddFinishedCallback((CheckID) =>
                {

                    this.checkid = CheckID;
                    //更改盘点信息
                  

                    StockInfoCheckTicket stockInfoCheck = null;
                    try
                    {
                        wmsEntities = new DataAccess.WMSEntities();
                        stockInfoCheck = (from s in wmsEntities.StockInfoCheckTicket
                                          where s.ID == this.checkid
                                          select s).FirstOrDefault();
                    }
                    catch
                    {
                        MessageBox.Show("加载盘点单数据失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        this.Close();
                        return;
                    }
                    if (stockInfoCheck == null)
                    {
                        MessageBox.Show("要修改的项目已不存在，请确认后操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }
                    stockInfoCheck.LastUpdateUserID = Convert.ToString(userID);
                    stockInfoCheck.LastUpdateTime = DateTime.Now;
                    try
                    {

                        wmsEntities.SaveChanges();

                    }
                    catch
                    {
                        MessageBox.Show("修改盘点单操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                this.pagerWidget.Search(false ,checkid);

                });
                a1.SetMode(FormMode.CHECK);
                a1.Show();
            }

           



        }

        private void reoGridControlMain_Click(object sender, EventArgs e)
        {

        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlMain .Worksheets[0];
            StandardFormPreviewExcel formPreview = new StandardFormPreviewExcel("盘点单预览");
            if (formPreview.SetPatternTable(@"Excel\StockInfoCheckTicket.xlsx") == false)
            {
                this.Close();
                return;
            }
            WMSEntities wmsEntities = new WMSEntities();
            if (worksheet.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项进行预览", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int stockInfoCheckID;
            try
            {
                stockInfoCheckID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
            }
            catch
            {
                MessageBox.Show("请选择一项导出", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }





           
            StockInfoCheckTicket stockInfoCheck1 = null;
            try
            {
                wmsEntities = new WMSEntities();
                stockInfoCheck1 = (from s in wmsEntities.StockInfoCheckTicket
                                   where s.ID == stockInfoCheckID
                                   select s).FirstOrDefault();
            }
            catch
            {
                MessageBox.Show("加载盘点单数据失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                this.Close();
                return;
            }
            if (stockInfoCheck1 == null)
            {
                MessageBox.Show("要查看的项目已不存在，请确认后操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            StockInfoCheckTicketView StockInfoCheckTicketView = (from stv in wmsEntities.StockInfoCheckTicketView  
                                                   where stv.ID == stockInfoCheckID
                                                   select stv).FirstOrDefault();

            StockInfoCheckTicketItemView [] StockInfoCheckTicketItemView =
                (from p in wmsEntities.StockInfoCheckTicketItemView 
                 where p.StockInfoCheckTicketID  == StockInfoCheckTicketView.ID
                 select p).ToArray();
            if (StockInfoCheckTicketView == null)
            {
                MessageBox.Show("盘点单不存在，请重新查询！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
            if (StockInfoCheckTicketView != null)
            {
                formPreview.AddData("StockInfoCheckTicket", StockInfoCheckTicketView);
            }
            formPreview.AddData("StockInfoCheckTicketItem", StockInfoCheckTicketItemView);
            
            
            formPreview.Show();
        }
    }
}

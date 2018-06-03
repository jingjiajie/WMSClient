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
using unvell.ReoGrid.DataFormat;

namespace WMS.UI
{
    public partial class SupplierStorageInfo : Form
    {
        private int supplierid;
        private int projectID = -1;
        private int warehouseID = -1;
        private int check_history = 0;
        private WMSEntities wmsEntities = new WMSEntities();
        private PagerWidget<SupplierStorageInfoView> pagerWidget = null;
        SearchWidget<SupplierStorageInfoView> searchWidget = null;

        public SupplierStorageInfo(int supplierid=-1,int check_history=0)
        {
            InitializeComponent();
            this.supplierid = supplierid;
            this.check_history = check_history;
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
        private void InitializeComponent1()
        {
            try
            {
                this.wmsEntities.Database.Connection.Open();

                //string[] visibleColumnNames = (from kn in SupplierStorageInfoMetaData.KeyNames
                //                               where kn.Visible == true
                //                               select kn.Name).ToArray();

                ////初始化查询框
                //this.toolStripComboBoxSelect.Items.Add("无");
                //this.toolStripComboBoxSelect.Items.AddRange(visibleColumnNames);
                //this.toolStripComboBoxSelect.SelectedIndex = 0;
            }
            catch
            {
                MessageBox.Show("加载失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }
            //初始化分页控件
            this.pagerWidget = new PagerWidget<SupplierStorageInfoView>(this.reoGridControlUser, SupplierStorageInfoMetaData.KeyNames, this.projectID, this.warehouseID);
            this.panelPager.Controls.Add(pagerWidget);
            pagerWidget.Show();
            this.searchWidget = new SearchWidget<SupplierStorageInfoView>(SupplierStorageInfoMetaData.KeyNames, this.pagerWidget);
            this.panelSearchWidget.Controls.Add(searchWidget);
        }

        private void FormSupplierAnnualInfo_Load(object sender, EventArgs e)
        {
          InitializeComponent1();
            this.pagerWidget.ClearCondition();
            //if (this.check_history == 1)
            //{
              
            //    //    this.pagerWidget.AddCondition("实际执行供应商合同ID", Convert.ToString(this.supplierid));
            ////    this.pagerWidget.Search();

            //}
            //else
            {
                this.pagerWidget.AddStaticCondition("供应商ID", Convert.ToString(this.supplierid));
                this.pagerWidget.Search();
            }
        }

        private void toolStripComboBoxSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.toolStripComboBoxSelect.SelectedIndex == 0)
            //{
            //    this.toolStripTextBoxSelect.Text = "";
            //    this.toolStripTextBoxSelect.Enabled = false;
                
            //}
            //else
            //{
            //    this.toolStripTextBoxSelect.Enabled = true;
            //}
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            var a1 = new SupplierStorageInfoModify(this.supplierid );

            a1.SetMode(FormMode.ADD);

            a1.SetAddFinishedCallback((AddID) =>
            {
                this.pagerWidget.Search(false, AddID);

            });
            a1.Show();
        }







        private void toolStripButtonSelect_Click(object sender, EventArgs e)
        {
            //this.pagerWidget.ClearCondition();
            //this.pagerWidget.AddCondition("供应商ID", Convert.ToString(this.supplierid));
            //if (this.toolStripComboBoxSelect.SelectedIndex != 0)
            //{
            //    this.pagerWidget.AddCondition(this.toolStripComboBoxSelect.SelectedItem.ToString(), this.toolStripTextBoxSelect.Text);
            //}
            // this.pagerWidget.Search();
           
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlUser.Worksheets[0];
            try
            {

                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int SupplierStorageInfoID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                var a1 = new SupplierStorageInfoModify (this.supplierid ,SupplierStorageInfoID);
                a1.SetMode(FormMode.ALTER);
                a1.SetModifyFinishedCallback((AlterID) =>
                {
                    this.pagerWidget.Search(false, AlterID);
                });


                a1.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlUser.Worksheets[0];
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
                WMSEntities wmsEntities = new WMSEntities();
                try
                {
                    
                        var recipettickets = (from kn in wmsEntities.ReceiptTicket
                                              where kn.SupplierID == this.supplierid 
                                              select kn).ToArray();
                        if(recipettickets.Length >0)
                        {
                            MessageBox.Show("删除失败，请先删除与本库存信息相关的收货单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;                            
                        }


                        //var ShipmentTicket = (from kn in wmsEntities.ShipmentTicket
                        //                      where kn.SupplierID == this.supplierid 
                        //                      select kn).ToArray();
                        //if (recipettickets.Length > 0)
                        //{
                        //    MessageBox.Show("删除失败，请先删除与本库存信息相关的发货单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //    return;
                        //}
                    
                  
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
                        wmsEntities.Database.ExecuteSqlCommand("DELETE FROM SupplierStorageInfo WHERE ID = @SupplierStorageInfoID", new SqlParameter("SupplierStorageInfoID", id));
                    }
                    wmsEntities.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Invoke(new Action(() =>
                {
                    this.pagerWidget.Search();
                }));

            })).Start();
        }
    }
}

﻿using System;
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
    public partial class FormStockInfo : Form
    {
        private WMSEntities wmsEntities = new WMSEntities();
        private int userID = -1;
        private int projectID = -1;
        private int warehouseID = -1;
        private int supplierid = -1;
        private PagerWidget<StockInfoView> pagerWidget = null;

        public FormStockInfo(int userID,int projectID,int warehouseID,int supplierid)
        {
            InitializeComponent();
            this.userID = userID;
            this.projectID = projectID;
            this.warehouseID = warehouseID;
            this.supplierid = supplierid;
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

        private void FormStockInfo_Load(object sender, EventArgs e)
        {
            InitComponents();
            try
            {
                WMSEntities wmsEntities = new WMSEntities();
                User user = (from u in wmsEntities.User where u.ID == this.userID select u).FirstOrDefault();
                if (user == null)
                {
                    MessageBox.Show("登录失效，请重新登录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //如果登录失效，不能查出任何数据。
                    this.pagerWidget.AddStaticCondition("SupplierID", "-1");
                    return;
                }
                if(user.Supplier != null)
                {
                    this.pagerWidget.AddStaticCondition("SupplierID", user.SupplierID.ToString());
                }
            }
            catch
            {
                MessageBox.Show("加载失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //如果加载失败，不能查出任何数据。
                this.pagerWidget.AddStaticCondition("SupplierID", "-1");
                return;
            }
            if(this.supplierid !=-1)
            {
                this.buttonAlter.Enabled = false;
            }
            this.pagerWidget.Search();
        }


        private void InitComponents()
        {
            string[] visibleColumnNames = (from kn in StockInfoViewMetaData.KeyNames
                                           where kn.Visible == true
                                           select kn.Name).ToArray();

            //初始化查询框
            this.comboBoxSearchCondition.Items.Add("无");
            this.comboBoxSearchCondition.Items.AddRange(visibleColumnNames);
            this.comboBoxSearchCondition.SelectedIndex = 0;

            //初始化分页控件
            this.pagerWidget = new PagerWidget<StockInfoView>(this.reoGridControlMain, StockInfoViewMetaData.KeyNames, this.projectID, this.warehouseID);
            this.panelPager.Controls.Add(pagerWidget);
            pagerWidget.Show();
        }

        private void reoGridControlMain_Click(object sender, EventArgs e)
        {

        }
        
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            this.pagerWidget.ClearCondition();
            if(this.comboBoxSearchCondition.SelectedIndex != 0)
            {
                this.pagerWidget.AddCondition(this.comboBoxSearchCondition.SelectedItem.ToString(), this.textBoxSearchValue.Text);
            }
            this.pagerWidget.Search();
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
                int stockInfoID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                var formStockInfoModify = new FormStockInfoModify(stockInfoID);
                formStockInfoModify.SetModifyFinishedCallback(()=>
                {
                    this.pagerWidget.Search();
                });
                formStockInfoModify.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormStockInfoModify();
            form.SetMode(FormMode.ADD);
            form.SetAddFinishedCallback(() =>
            {
                this.pagerWidget.Search();
            });
            form.Show();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
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
            if(deleteIDs.Count == 0)
            {
                MessageBox.Show("请选择您要删除的记录","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("您真的要删除这些记录吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            this.labelStatus.Text = "正在删除...";


            new Thread(new ThreadStart(()=>
            {
                foreach(int id in deleteIDs)
                {
                    this.wmsEntities.Database.ExecuteSqlCommand("DELETE FROM StockInfo WHERE ID = @stockInfoID", new SqlParameter("stockInfoID", id));
                }
                this.wmsEntities.SaveChanges();
                this.Invoke(new Action(()=>
                {
                    this.pagerWidget.Search();
                }));
            })).Start();
        }

        private void textBoxSearchValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.buttonSearch.PerformClick();
            }
        }

        private void comboBoxSearchCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxSearchCondition.SelectedIndex == 0)
            {
                this.textBoxSearchValue.Text = "";
                this.textBoxSearchValue.Enabled = false;
            }
            else
            {
                this.textBoxSearchValue.Enabled = true;
            }
        }
    }
}

using FrontWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI.FormBasicInfos
{
    public partial class FormPackage : Form
    {
        public FormPackage()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
            this.model1.RowRemoved += this.model_RowRemoved;
            this.model1.Refreshed += this.model_Refreshed;
        }

        private void FormPackage_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            this.searchView1.Search();
        }

        private void model_Refreshed(object sender, ModelRefreshedEventArgs e)
        {
            this.updateBasicAndReoGridView();
        }

        private void updateBasicAndReoGridView()
        {

            if (this.model1.RowCount == 0)
            {
                this.basicView1.Enabled = false;
                this.reoGridView1.Enabled = false;
            }
            else
            {
                this.basicView1.Enabled = true;
                this.reoGridView1.Enabled = true;
            }

        }

        private void model_RowRemoved(object sender, ModelRowRemovedEventArgs e)
        {
            this.updateBasicAndReoGridView();
        }


        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.basicView1.Enabled = true;
            this.reoGridView1.Enabled = true;
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                 { "warehouseId",GlobalData.Warehouse["id"]},
                 { "warehouseName",GlobalData.Warehouse["name"]},
                 { "enabled",1},
            });
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
            }
        }

        private void buttonItems_Click(object sender, EventArgs e)
        {
            try { 
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项发货套餐单查看物料条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row })[0];
            new FormPackageItem(rowData).Show();
            }
            catch
            {
                MessageBox.Show("无任何信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
}
    }
}

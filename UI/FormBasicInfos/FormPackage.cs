using FrontWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace WMS.UI.FormBasicInfos
{
    public partial class FormPackage : Form
    {
        public FormPackage()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private void FormPackage_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            this.searchView1.Search();
        }


        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            string s = Interaction.InputBox("请输入需要添加的行数", "提示", "1", -1, -1);  //-1表示在屏幕的中间         
            int row = 1;
            try
            {
                row = Convert.ToInt32(s);
            }
            catch
            {
                MessageBox.Show("请输入正确的数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            for (int i = 0; i < row; i++)
            {
                this.model1.InsertRow(0, new Dictionary<string, object>()
            {
            });
            }

        }

        private int WarehouseIdDefaultValue()
        {
            return (int)GlobalData.Warehouse["id"];
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

                Condition condWarehouse = new Condition().AddCondition("warehouseId", GlobalData.Warehouse["id"]);
                GlobalData.AllPackage = RestClient.Get<List<IDictionary<string, object>>>(
                  $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/package/{condWarehouse.ToString()}");
            }
        }

        private void buttonItems_Click(object sender, EventArgs e)
        {
            try { 
                if (this.model1.SelectionRange == null || this.model1.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项发货套餐单查看物料条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row })[0];
                if (rowData["id"] == null)
                {
                    MessageBox.Show("请先保存单据再查看条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
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

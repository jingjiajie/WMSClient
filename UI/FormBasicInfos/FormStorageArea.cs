using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontWork;
using Microsoft.VisualBasic;

namespace WMS.UI.FormBasicInfos
{
    public partial class FormStorageArea : Form
    {
        public FormStorageArea()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
            this.model1.RowRemoved += this.model_RowRemoved;
            this.model1.Refreshed += this.model_Refreshed;
        }

        private void FormStorageArea_Load(object sender, EventArgs e)
        {
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
            this.updateBasicAndReoGridView();
        }

        private int WarehouseIdDefaultValue()
        {
            return (int)GlobalData.Warehouse["id"];
        }

        private void model_RowRemoved(object sender, ModelRowRemovedEventArgs e)
        {
            this.updateBasicAndReoGridView();
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

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            //this.basicView1.Enabled = true;
            //this.reoGridView1.Enabled = true;
            //this.model1.InsertRow(0, new Dictionary<string, object>()
            //{                         
            //});
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

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
            if (this.model1.RowCount == 0)
            {
                this.basicView1.Enabled = false;
                this.reoGridView1.Enabled = false;
            }
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
                Condition condWarehouse = new Condition().AddCondition("warehouseId", GlobalData.Warehouse["id"]);
                GlobalData.AllStorageAreas = RestClient.Get<List<IDictionary<string, object>>>(
                   $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/storage_area/{condWarehouse.ToString()}");
            }
        }

        private string EnableForwardMapper([Data]int state)
        {
            switch (state)
            {
                case 0: return "禁用";
                case 1: return "启用";
                default: return "未知状态";
            }
        }

        private int EnableBackwardMapper([Data]string enable)
        {
            switch (enable)
            {
                case "禁用": return 0;
                case "启用": return 1;
                default: return -1;
            }
        }
    }
}

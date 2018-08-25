using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontWork;

namespace WMS.UI.FromSalary
{
    public partial class FormSalaryItem : Form
    {
        private int salaryTypeId;
        public FormSalaryItem(int salaryTypeId)
        {
            MethodListenerContainer.Register("FormSalaryItem", this);
            InitializeComponent();
            this.salaryTypeId = salaryTypeId;
        }

         private string TypeForwardMapper([Data]int state)
        {
            switch (state)
            {
                case 0: return "固定工资";
                case 1: return "计件工资";
                case 2: return "公式计算";
                default: return "未知状态";
            }
        }

        private int TypeBackwardMapper([Data]string enable)
        {
            switch (enable)
            {
                case "固定工资": return 0;
                case "计件工资": return 1;
                case "公式计算": return 2;
                default: return -1;
            }
        }

        private string GiveOutForwardMapper([Data]int state)
        {
            switch (state)
            {
                case 0: return "否";
                case 1: return "是";
                default: return "未知状态";
            }
        }

        private int GiveOutBackwardMapper([Data]string enable)
        {
            switch (enable)
            {
                case "否": return 0;
                case "是": return 1;
                default: return -1;
            }
        }

        private void FormSalaryItem_Load(object sender, EventArgs e)
        {
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            this.searchView1.AddStaticCondition("salaryTypeId", this.salaryTypeId);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            //this.model1.SelectionRangeChanged += this.model_SelectionRangeChanged;
            this.searchView1.Search();         
        }

        private void model_SelectionRangeChanged(object sender, ModelSelectionRangeChangedEventArgs e)
        {
            if (this.model1.RowCount == 0) { return; }
            if (this.model1.SelectionRange.Rows != 1)
            {
                this.basicView1.Mode = "default";
                this.reoGridView1.Mode = "default";
            }
            else
            {           
                if ((int)this.model1[this.model1.SelectionRange.Row, "type"] == 1)
                {
                    this.basicView1.Mode = "count";
                    this.reoGridView1.Mode = "count";
                }
                else if ((int)this.model1[this.model1.SelectionRange.Row, "type"] == 0)
                {
                    this.basicView1.Mode = "default";
                    this.reoGridView1.Mode = "default";
                }
            }
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "warehouseName",GlobalData.Warehouse["name"]},
                { "salaryTypeId",salaryTypeId}
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
                Condition condWarehouse = new Condition().AddCondition("warehouseId", GlobalData.Warehouse["id"]);
                GlobalData.AllSalaryItem = RestClient.Get<List<IDictionary<string, object>>>(
                   $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/salary_item/{condWarehouse.ToString()}");
            }
        }

        private void SalaryTypeNameEditEnded(int row, string salaryTypeName)
        {
            IDictionary<string, object> foundSalaryType =
                GlobalData.AllSalaryType.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == salaryTypeName;
                });
            if (foundSalaryType == null)
            {
                MessageBox.Show($"薪金类型名称\"{salaryTypeName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "salaryTypeId"] = foundSalaryType["id"];              
            }
        }
    }
}

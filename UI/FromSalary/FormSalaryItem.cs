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
        public FormSalaryItem()
        {
            MethodListenerContainer.Register("FormSalaryItem", this);
            InitializeComponent();
        }

        private string TypeForwardMapper(int state)
        {
            switch (state)
            {
                case 0: return "固定工资";
                case 1: return "计件工资";
                default: return "未知状态";
            }
        }

        private int TypeBackwardMapper(string enable)
        {
            switch (enable)
            {
                case "固定工资": return 0;
                case "计件工资": return 1;
                default: return -1;
            }
        }

        private void FormSalaryItem_Load(object sender, EventArgs e)
        {
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);         
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();         
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "warehouseName",GlobalData.Warehouse["name"]}
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

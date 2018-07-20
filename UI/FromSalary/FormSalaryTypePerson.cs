using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI.FromSalary
{
    public partial class FormSalaryTypePerson : Form
    {
        private int salaryTypeId;
        public FormSalaryTypePerson(int salaryTypeId)
        {
            InitializeComponent();
            this.salaryTypeId = salaryTypeId;
        }


        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {              
                { "salaryTypeId",salaryTypeId}
            });
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
            this.searchView1.Search();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
                /*
                Condition condWarehouse = new Condition().AddCondition("warehouseId", GlobalData.Warehouse["id"]);
                GlobalData.AllSalaryPeriod = RestClient.Get<List<IDictionary<string, object>>>(
             $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/salary_period/{condWarehouse.AddOrder("createTime", OrderItemOrder.DESC).ToString()}");*/

            }
        }
    }
}

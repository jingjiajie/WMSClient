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
    public partial class FormSalaryPeriod : Form
    {
        public FormSalaryPeriod()
        {
            MethodListenerContainer.Register("FormSalaryPeriod", this);
            InitializeComponent();
            //this.formManager = formManager;
        }
        private SingletonManager<Form> formManager;

        private void FormSalaryPeriod_Load(object sender, EventArgs e)
        {
            //this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
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
                { "warehouseName",GlobalData.Warehouse["name"]},
                { "endTime",DateTime.Now}
            });
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();            
            Condition condWarehouse = new Condition();
            GlobalData.AllSalaryPeriod = RestClient.Get<List<IDictionary<string, object>>>(
         $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/salary_period/{condWarehouse.AddOrder("endTime", OrderItemOrder.DESC).ToString()}");

            if (FormPersonSalary.formPersonSalary != null)
            {
                FormPersonSalary.formPersonSalary.RefreshSalaryPeriod();
            }
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
                Condition condWarehouse = new Condition();
                GlobalData.AllSalaryPeriod = RestClient.Get<List<IDictionary<string, object>>>(
             $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/salary_period/{condWarehouse.AddOrder("endTime", OrderItemOrder.DESC).ToString()}");

                if (FormPersonSalary.formPersonSalary != null)
                {
                    FormPersonSalary.formPersonSalary.RefreshSalaryPeriod();
                }
                //this.GetPersonSalary();
            }
        }

        //private void GetPersonSalary() {
        //FormPersonSalary form= (FormPersonSalary)formManager.Get("FormPersonSalary");
        //form.RefreshSalaryPeriod();
        //}
    }
}

using System;
using FrontWork;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI.FormAcccount
{
    public partial class FormTaxItem : Form
    {
        private IDictionary<string, object> tax = null;
        public FormTaxItem(IDictionary<string, object> tax)
        {
            this.tax = tax;
            MethodListenerContainer.Register(this);
            this.searchView1.AddStaticCondition("taxId", this.tax["id"]);
            InitializeComponent();
        }

        private void FormTaxItem_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private string TypeForwardMapper(int state)
        {
            switch (state)
            {
                case 0: return "定额收税";
                case 1: return "比例税率";
                default: return "未知状态";
            }
        }

        private int TypeBackwardMapper(string type)
        {
            switch (type)
            {
                case "定额收税": return 0;
                case "比例税率": return 1;
                default: return -1;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "taxId",this.tax["id"]}
            });
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }

        private void buttonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();

             //   Condition condWarehouse = new Condition().AddCondition("warehouseId", GlobalData.Warehouse["id"]);
             //   GlobalData.AllSalaryPeriod = RestClient.Get<List<IDictionary<string, object>>>(
             //$"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/account_title/{condWarehouse.ToString()}"); ;
            }
        }


    }
}

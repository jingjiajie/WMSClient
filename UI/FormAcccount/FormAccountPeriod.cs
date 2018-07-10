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
    public partial class FormAccountPeriod : Form
    {
        public FormAccountPeriod()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private string endedForwardMapper(int ended)
        {
            switch (ended)
            {
                case 0: return "否";
                case 1: return "是";
                default: return "未知状态";
            }
        }

        private int endedBackwardMapper(string ended)
        {
            switch (ended)
            {
                case "否": return 0;
                case "是": return 1;
                default: return -1;
            }
        }
        private void FormAccountPeriod_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "warehouseId",GlobalData.Warehouse["id"]}
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

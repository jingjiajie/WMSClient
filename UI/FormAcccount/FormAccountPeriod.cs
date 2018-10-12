using System;
using FrontWork;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace WMS.UI.FormAcccount
{
    public partial class FormAccountPeriod : Form
    {
        private Action addFinishedCallback = null;
        public FormAccountPeriod()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private string endedForwardMapper([Data]int ended)
        {
            switch (ended)
            {
                case 0: return "否";
                case 1: return "是";
                default: return "未知状态";
            }
        }

        private int endedBackwardMapper([Data]string ended)
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
                { "warehouseId",GlobalData.Warehouse["id"]}
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
                Condition condWarehouse = new Condition().AddCondition("warehouseId", GlobalData.Warehouse["id"]);
                GlobalData.AllAccountPeriod = RestClient.Get<List<IDictionary<string, object>>>(
               $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/account_period/{condWarehouse.AddOrder("startTime", OrderItemOrder.DESC).ToString()}");

                try
                {
                    GlobalData.AccountPeriod = RestClient.Get<List<IDictionary<string, object>>>(
                       $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/account_period/{condWarehouse.AddCondition("ended", 0).AddOrder("startTime", OrderItemOrder.DESC).ToString()}")[0];
                }
                catch { GlobalData.AccountPeriod = null; }
                this.searchView1.Search();
            }
        }

        public void SetAddFinishedCallback(Action callback)
        {
            this.addFinishedCallback = callback;
        }

        private void FormDeliverOrderItemClosed(object sender, FormClosedEventArgs e)
        {
            if (this.addFinishedCallback != null)
            {
                this.addFinishedCallback();
            }
        }

        private void toolStripCarryOver_Click(object sender, EventArgs e)
        {
            var a1 = new FormTime();
            a1.SetAddFinishedCallback(() =>
            {
                this.searchView1.Search();
            });
            a1.Show();
           
        }
    }
}

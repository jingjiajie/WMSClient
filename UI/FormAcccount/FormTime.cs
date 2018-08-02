using FrontWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI.FormAcccount
{
    public partial class FormTime : Form
    {
        private Action addFinishedCallback = null;
        public FormTime()
        {
            InitializeComponent();
        }

        private void FormTime_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            Utilities.BindBlueButton(this.buttonADD);
            this.model1.InsertRow(0, null);
            this.model1[this.model1.SelectionRange.Row, "startTime"] = DateTime.Now;
        }

        public void SetAddFinishedCallback(Action callback)
        {
            this.addFinishedCallback = callback;
        }

        private void buttonADD_Click_1(object sender, EventArgs e)
        {
            this.buttonADD.Focus();
            var startTime = this.model1[this.model1.SelectionRange.Row, "startTime"];
            try
            {
                string body = "{\"warehouseId\":\"" + GlobalData.Warehouse["id"] + "\",\"personId\":\"" + GlobalData.Person["id"] + "\",\"startTime\":\"" + startTime + "\"}";
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/account_period/carry_over";
                RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);
                MessageBox.Show("结转成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Condition condWarehouse = new Condition().AddCondition("warehouseId", GlobalData.Warehouse["id"]);
                GlobalData.AllAccountPeriod = RestClient.Get<List<IDictionary<string, object>>>(
               $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/account_period/{condWarehouse.AddOrder("startTime", OrderItemOrder.DESC).ToString()}");

                try
                {
                    GlobalData.AccountPeriod = RestClient.Get<List<IDictionary<string, object>>>(
                       $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/account_period/{condWarehouse.AddCondition("ended", 0).AddOrder("startTime", OrderItemOrder.DESC).ToString()}")[0];
                }
                catch { GlobalData.AccountPeriod = null; }
            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show("结转失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.Close();
        }

        private void FormTimeClosed(object sender, FormClosedEventArgs e)
        {
            if (this.addFinishedCallback != null)
            {
                this.addFinishedCallback();
            }
        }
    }
}

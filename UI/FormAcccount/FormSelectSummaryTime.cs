using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace WMS.UI.FormAcccount
{
    public partial class FormSelectSummaryTime : Form
    {
        private Action addFinishedCallback = null;
        public FormSelectSummaryTime()
        {
            InitializeComponent();
        }

        private void FormSelectSummaryTime_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            Utilities.BindBlueButton(this.buttonADD);
            this.model1.InsertRow(0, null);
            this.model1[this.model1.SelectionRange.Row, "endTime"] = DateTime.Now;
        }

        public void SetAddFinishedCallback(Action callback)
        {
            this.addFinishedCallback = callback;
        }

        private void buttonADD_Click(object sender, EventArgs e)
        {
            this.buttonADD.Focus();
            var startTime = this.model1[this.model1.SelectionRange.Row, "startTime"];
            var endTime = this.model1[this.model1.SelectionRange.Row, "endTime"];
            try
            {
                string body = "{\"warehouseId\":\"" + GlobalData.Warehouse["id"] + "\",\"endTime\":\"" + endTime + "\",\"startTime\":\"" + startTime + "\"}";
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/account_record/summary_all_title";
                var returnSummaryList=RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);

                    StringBuilder remindBody = new StringBuilder();
                    foreach (IDictionary<string, object> accountTitleSummary in returnSummaryList)
                    {
                        //remindBody = remindBody
                        //        .Append("科目名称：“").Append(accountTitleSummary["curAccountTitleName"])
                        //        .Append("”，余额：“").Append(accountTitleSummary["balance"])
                        //        .Append("”存在赤字！请核准账目记录！\r\n");

                    }
                    

                

                

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

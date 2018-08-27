using FrontWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Net;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI.FormAcccount
{
    public partial class FormTransferAccount : Form
    {
        private int outAccountTitleId = -1;
        private int inAccountTitleId = -1;
        private Action addFinishedCallback = null;
        public FormTransferAccount()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private void FormTransferAccount_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            Utilities.BindBlueButton(this.buttonADD);
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
            });
        }

        private void buttonADD_Click(object sender, EventArgs e)
        {
            this.buttonADD.Focus();
            var rowData = this.model1.GetSelectedRow();
            if (rowData["changeAmount"] == null) {
                MessageBox.Show($"发生金额不能为空，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.outAccountTitleId ==this.inAccountTitleId) {
                MessageBox.Show($"目标科目和原科目不能相同，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

            var changeAmount = rowData["changeAmount"];
            var voucherInfo = rowData["voucherInfo"];


            if (this.outAccountTitleId != -1&&this.inAccountTitleId!=-1)
            {
                try
                {
                    string body = "{\"warehouseId\":\"" + GlobalData.Warehouse["id"] + "\",\"personId\":\"" + GlobalData.Person["id"] + "\",\"accountPeriodId\":\"" + GlobalData.AccountPeriod["id"] + "\",\"outaccountTitleId\":\"" + this.outAccountTitleId + "\",\"inaccountTitleId\":\"" + this.inAccountTitleId + "\",\"changeAmount\":\"" + changeAmount + "\",\"voucherInfo\":\"" + voucherInfo + "\"}";
                    string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/account_record/real_transfer_account";
                    var remindData = RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);
                    this.Close();
                }
                catch (WebException ex)
                {
                    string message = ex.Message;
                    if (ex.Response != null)
                    {
                        message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    }
                    MessageBox.Show(("转账操作") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            
        }

        private void outAccountTitleNameEditEnded(int row, string outAccountTitleName)
        {
            var foundAccountTitles = (from s in GlobalData.AllAccountTitle
                                 where s["name"]?.ToString() == outAccountTitleName
                                 select s).ToArray();
            if (foundAccountTitles.Length != 1) goto FAILED;
            this.outAccountTitleId = (int)foundAccountTitles[0]["id"];
            return;

            FAILED:
            MessageBox.Show($"科目：\"{outAccountTitleName}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void inAccountTitleNameEditEnded(int row, string inAccountTitleName)
        {
            var foundAccountTitles = (from s in GlobalData.AllAccountTitle
                                      where s["name"]?.ToString() == inAccountTitleName
                                      select s).ToArray();
            if (foundAccountTitles.Length != 1) goto FAILED;
            this.inAccountTitleId = (int)foundAccountTitles[0]["id"];
            return;

            FAILED:
            MessageBox.Show($"科目：\"{inAccountTitleName}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        public void SetAddFinishedCallback(Action callback)
        {
            this.addFinishedCallback = callback;
        }

        private void FormTransferAccountClosed(object sender, FormClosedEventArgs e)
        {
            if (this.addFinishedCallback != null)
            {
                this.addFinishedCallback();
            }
        }



    }
}

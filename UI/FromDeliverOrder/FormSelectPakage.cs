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

namespace WMS.UI.FromDeliverOrder
{   

    public partial class FormSelectPakage : Form
    {
        private int packageId = -1;
        private Action addFinishedCallback = null;
        public FormSelectPakage()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private void basicView1_Load(object sender, EventArgs e)
        {

        }

        private void configuration1_Load(object sender, EventArgs e)
        {

        }

        private void basicView1_Load_1(object sender, EventArgs e)
        {

        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            if (this.packageId != -1)
            {
                try
                {
                    string body = "{\"warehouseId\":\"" + GlobalData.Warehouse["id"] + "\",\"personId\":\"" + GlobalData.Person["id"] + "\",\"packageId\":\"" + this.packageId + "\"}";
                    string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/delivery_order/delivery_by_package";
                    RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);
                    MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();

                }
                catch (WebException ex)
                {
                    string message = ex.Message;
                    if (ex.Response != null)
                    {
                        message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    }
                    MessageBox.Show(("按套餐添加出库单") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void PackageNameEditEnded(int row, string pakageName)
        {
            var foundPackages = (from s in GlobalData.AllPackage
                                         where s["name"]?.ToString() == pakageName
                                select s).ToArray();
            if (foundPackages.Length != 1) goto FAILED;
            this.packageId= (int)foundPackages[0]["id"];
            return;

            FAILED:
            MessageBox.Show($"套餐\"{pakageName}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        public void SetAddFinishedCallback(Action callback)
        {
            this.addFinishedCallback = callback;
        }

        private void FormSelectPakageClosed(object sender, FormClosedEventArgs e)
        {
            if (this.addFinishedCallback != null)
            {
                this.addFinishedCallback();
            }
        }

        private void FormSelectPakage_Load(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "warehouseName",GlobalData.Warehouse["name"]},
            });
        }
    }
}

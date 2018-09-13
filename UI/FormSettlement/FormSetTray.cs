using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;

namespace WMS.UI.FormSettlement
{
    public partial class FormSetTray : Form
    {
        string lengthKey;
        string widthKey;
        CommonData commonDataLength = new CommonData();
        CommonData commonDataWidth = new CommonData();

        private FormMode mode = FormMode.ALTER;
        public FormSetTray()
        {
            InitializeComponent();
        }

        private void FormSetTray_Load(object sender, EventArgs e)
        {
            this.lengthKey = "Tray_Length_" + GlobalData.Warehouse["id"];
            this.widthKey = "Tray_Width_" + GlobalData.Warehouse["id"];
            this.CenterToScreen();
            this.Search();

        }


        private void buttonADD_Click(object sender, EventArgs e)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            commonDataLength.key = this.lengthKey;
            commonDataLength.value = this.textBoxLength.Text;
          
            commonDataWidth.key = this.widthKey;
            commonDataWidth.value = this.textBoxWidth.Text;
            string body = serializer.Serialize(new CommonData[] {commonDataLength,commonDataWidth});
            try
            {
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/tray";
                if (this.mode == FormMode.ALTER)
                {
                    RestClient.RequestPost<string>(url, body, "PUT");
                    MessageBox.Show("设置托位大小成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (this.mode == FormMode.ADD)
                {
                    
                    RestClient.RequestPost<string>(url, body, "POST");
                    MessageBox.Show("设置托位大小成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show(("设置托位大小失败") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Search()
        {
            Condition condition = new Condition();
            condition.AddCondition("key", new object[] { this.lengthKey, this.widthKey }, ConditionItemRelation.IN);    
            string cond = condition.ToString();
            try
            {
                string url = $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/tray/{condition.ToString()}";
                CommonData[] trayDates= RestClient.RequestPost<CommonData[]>(url,null, "GET");
                if (trayDates.Length == 2)
                {
                    if (trayDates[0].key == this.lengthKey && trayDates[1].key == this.widthKey)
                    {
                        this.textBoxLength.Text = trayDates[0].value;
                        this.textBoxWidth.Text = trayDates[1].value;
                        this.mode = FormMode.ALTER;
                    }
                    else if (trayDates[1].key == this.lengthKey && trayDates[0].key == this.widthKey)
                    {
                        this.textBoxLength.Text = trayDates[1].value;
                        this.textBoxWidth.Text = trayDates[0].value;
                        this.mode = FormMode.ALTER;
                    }
                    else
                    {
                        MessageBox.Show(("托位数据出错！"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    this.textBoxLength.Text = "";
                    this.textBoxWidth.Text = "";
                    this.mode = FormMode.ADD;
                }
            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show(("获取托位信息失败") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public enum FormMode
        {
            ADD, ALTER
        }
    }
}

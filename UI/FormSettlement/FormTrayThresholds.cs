using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace WMS.UI.FormSettlement
{
    public partial class FormTrayThresholds : Form
    {
        string lengthKey;
        string widthKey;
        CommonData commonDataLength = new CommonData();
        CommonData commonDataWidth = new CommonData();

        private FormMode mode = FormMode.ALTER;
        public FormTrayThresholds()
        {
            InitializeComponent();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "warehouseName",GlobalData.Warehouse["name"]}
            });
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save()) { this.searchView1.Search(); }
            this.setTray();
        }

        private void FormTrayThresholds_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();

            //另一部分
            this.lengthKey = "Tray_Length_" + GlobalData.Warehouse["id"];
            this.widthKey = "Tray_Width_" + GlobalData.Warehouse["id"];
            this.CenterToScreen();
            this.Search();
        }

        private void setTray()
        {
            if (!this.validateTextBox(textBoxLength.Text))
            { MessageBox.Show("请输入正确的托位长度！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!this.validateTextBox(textBoxWidth.Text))
            { MessageBox.Show("请输入正确的托位宽度！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!this.validateDate(textBoxLength.Text))
            { MessageBox.Show("托位长度不能小于0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!this.validateDate(textBoxWidth.Text))
            { MessageBox.Show("托位宽度不能小于0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            commonDataLength.key = this.lengthKey;
            commonDataLength.value = this.textBoxLength.Text;

            commonDataWidth.key = this.widthKey;
            commonDataWidth.value = this.textBoxWidth.Text;
            string body = serializer.Serialize(new CommonData[] { commonDataLength, commonDataWidth });
            try
            {
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/tray/";
                if (this.mode == FormMode.ALTER)
                {
                    RestClient.RequestPost<int[]>(url, body, "PUT");
                    //MessageBox.Show("设置托位大小成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (this.mode == FormMode.ADD)
                {

                    RestClient.RequestPost<int[]>(url, body, "POST");
                    //MessageBox.Show("设置托位大小成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        this.commonDataLength.id = trayDates[0].id;
                        this.commonDataWidth.id = trayDates[1].id;
                    }
                    else if (trayDates[1].key == this.lengthKey && trayDates[0].key == this.widthKey)
                    {
                        this.textBoxLength.Text = trayDates[1].value;
                        this.textBoxWidth.Text = trayDates[0].value;
                        this.mode = FormMode.ALTER;
                        this.commonDataLength.id = trayDates[1].id;
                        this.commonDataWidth.id = trayDates[0].id;
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
                    this.commonDataLength.id = 0;
                    this.commonDataWidth.id = 0;
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

        private bool validateTextBox(string text)
        {
            int num;
            if (!int.TryParse(text.Trim(), out num))
            {              
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool validateDate(string text)
        {
            int num;
            int.TryParse(text.Trim(), out num);
            if (num <= 0) {
                return false;
            }
            else
            { return true; }
        }



        public enum FormMode
        {
            ADD, ALTER
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontWork;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;


namespace WMS.UI.FromSalary
{
    public partial class FormPayNoteItem : Form
    {
        private int payNoteId;
        private int periodId;
        private int taxId;
        public FormPayNoteItem(int payNoteId,int periodId,int taxId)
        {
            MethodListenerContainer.Register("FormPayNoteItem", this);
            InitializeComponent();
            this.payNoteId = payNoteId;
            this.periodId = periodId;
            this.taxId = taxId;
        }

        private void FormPayNoteItem_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            Utilities.BindBlueButton(this.buttonCalculateAllTax);
            Utilities.BindBlueButton(this.buttonCclcultateItemsTax);
            Utilities.BindBlueButton(this.buttonRealPayAll);
            Utilities.BindBlueButton(this.buttonRealPayItems);
            this.searchView1.AddStaticCondition("payNoteId", payNoteId);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {                             
                { "payNoteId",payNoteId}
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
                this.searchView1.Search();
            }
        }

        private void PersonNameEditEnded([Row]int row, [Data] string personName)
        {
            IDictionary<string, object> foundPerson =
                GlobalData.AllPersons.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == personName;
                });
            if (foundPerson == null)
            {
                MessageBox.Show($"人员\"{personName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {             
                this.model1[row, "personId"] = foundPerson["id"];
            }
        }

        private string StateForwardMapper([Data]int state)
        {
            switch (state)
            {
                case 0: return "待计算应付";
                case 1: return "已计算应付";
                case 2: return "已付款";
                default: return "未知状态";
            }
        }

        private int StateBackwardMapper([Data]string enable)
        {
            switch (enable)
            {
                case "待计算应付": return 0;
                case "已计算应付": return 1;
                case "已付款": return 2;
                default: return -1;
            }
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {          
            try
            {
                string body = $"{{\"warehouseId\":\"{GlobalData.Warehouse["id"]}\",\"payNoteId\":\"{payNoteId}\"}}";
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/pay_note_item/add_all_item";
                RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);
                MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.searchView1.Search();               
            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show(("添加失败") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ButtonCalculateItemsTax_Click(object sender, EventArgs e)
        {
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项税务条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row });
            StringBuilder payNoteIds = new StringBuilder();
            payNoteIds.Append("[");
            foreach (var a in rowData) {
                payNoteIds.Append( a["id"]);
                payNoteIds.Append(",");               
            }
            payNoteIds.Remove(payNoteIds.Length - 1, 1);
            payNoteIds.Append("]");     
            try
            {
                string body = $"{{\"taxId\":\"{this.taxId}\",\"payNoteId\":\"{payNoteId}\",\"payNoteItemId\":{payNoteIds.ToString()}}}";
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/pay_note_item/calculate_tax";
                RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);
                MessageBox.Show("计算成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.searchView1.Search();
            }
            catch (WebException ex)
            {                    
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show(("计算失败") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                  
            }
        }

        private void buttonCalculateAllTax_Click(object sender, EventArgs e)
        {         
            try
            {
                string body = $"{{\"taxId\":\"{this.taxId}\",\"payNoteId\":\"{payNoteId}\"}}";
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/pay_note_item/calculate_tax";
                RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);
                MessageBox.Show("计算成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.searchView1.Search();
            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show(("计算失败") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}

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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace WMS.UI.FromSalary
{

    public partial class FormPayNoteItem : Form
    {
        public static int WAITING_FOR_CALCULATE_PAY = 0;
        public static int CALCULATED_PAY = 1;
        public static int PAYED = 2;

        private int payNoteId;
        private int periodId;
        private int taxId;
        private int payNoteState;
        public FormPayNoteItem(int payNoteId,int periodId,int taxId,int payNoteState)
        {
            MethodListenerContainer.Register("FormPayNoteItem", this);
            InitializeComponent();
            this.payNoteId = payNoteId;
            this.periodId = periodId;
            this.taxId = taxId;
            this.payNoteState = payNoteState;
           // this.model1.SelectionRangeChanged += this.model_SelectionRangeChanged;
        }

        private void model_SelectionRangeChanged(object sender, ModelSelectionRangeChangedEventArgs e)
        {
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row });
            if ((int)rowData[0]["state"] == 0)
            {
                this.model1.Mode = "default";
                this.basicView1.Mode = "default";
                this.reoGridView1.Mode = "default";
                //this.synchronizer.Mode = "default";
                this.buttonCclcultateItemsTax.Enabled =true;
                this.buttonCalculateAllTax.Enabled = true;
                this.buttonRealPayAll.Enabled = true;
                this.buttonRealPayItems.Enabled =true;
                this.ButtonAllPerson.Enabled = true;
                this.toolStripButtonAdd.Enabled =true;
                this.toolStripButtonDelete.Enabled = true;
            }
            else if ((int)rowData[0]["state"] == 1)
            {
                this.basicView1.Mode = "pay";
                this.reoGridView1.Mode = "pay";
                this.model1.Mode = "pay";
                //this.synchronizer.Mode = "pay";
                this.buttonCalculateAllTax.Enabled = false;
                this.buttonCclcultateItemsTax.Enabled = false;
                this.ButtonAllPerson.Enabled = false;
                this.toolStripButtonAdd.Enabled = false;
                this.toolStripButtonDelete.Enabled = false;
            }
            else {
                this.basicView1.Mode = "payed";
                this.reoGridView1.Mode = "payed";
                this.model1.Mode = "payed";
                //this.synchronizer.Mode = "payed";
                this.buttonCclcultateItemsTax.Enabled = false;
                this.buttonCalculateAllTax.Enabled = false;
                this.buttonRealPayAll.Enabled = false;
                this.buttonRealPayItems.Enabled = false;
                this.ButtonAllPerson.Enabled = false;
                this.toolStripButtonAdd.Enabled = false;
                this.toolStripButtonDelete.Enabled = false;
            }
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
            this.updateState();

        }

        private void updateState() {
            if (this.payNoteState == FormPayNote.CONFIRM_PAY)
            {
                this.basicView1.Mode = "pay";
                this.reoGridView1.Mode = "pay";
                this.model1.Mode = "pay";
                //this.synchronizer.Mode = "pay";
                this.buttonCalculateAllTax.Enabled = false;
                this.buttonCclcultateItemsTax.Enabled = false;
                this.ButtonAllPerson.Enabled = false;
                this.toolStripButtonAdd.Enabled = false;
                this.toolStripButtonDelete.Enabled = false;
            }
            else if (this.payNoteState == FormPayNote.CONFIRM_REAL_PAY) {
                this.basicView1.Mode = "payed";
                this.reoGridView1.Mode = "payed";
                this.model1.Mode = "payed";
                //this.synchronizer.Mode = "payed";
                this.buttonCclcultateItemsTax.Enabled = false;
                this.buttonCalculateAllTax.Enabled = false;
                this.buttonRealPayAll.Enabled = false;
                this.buttonRealPayItems.Enabled = false;
                this.ButtonAllPerson.Enabled = false;
                this.toolStripButtonAdd.Enabled = false;
                this.toolStripButtonDelete.Enabled = false;
            }

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
                MessageBox.Show(("添加") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public int[] getSelectRowIds() {
            List<int> selectIds = new List<int>();
            for (int i = 0; i < this.model1.SelectionRange.Rows; i++)
            {
                selectIds.Add(this.model1.SelectionRange.Row + i);
            }
            return selectIds.ToArray();    
        }

        private void ButtonCalculateItemsTax_Click(object sender, EventArgs e)
        {
            if (this.model1.SelectionRange.Rows == 0)
            {
                MessageBox.Show("请选择薪金单条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(this.getSelectRowIds());
       
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
                return;
            }
            if (this.judgeAllFinish(payNoteId, CALCULATED_PAY)) {
                if (MessageBox.Show("本单全部条目已经计算税费，是否直接同步到应付总账？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                AccountSynchronize accountSynchronize = new AccountSynchronize();
                accountSynchronize.payNoteId = payNoteId;
                accountSynchronize.personId = ((int)GlobalData.Person["id"]);
                accountSynchronize.warehouseId = ((int)GlobalData.Warehouse["id"]);
                string json = (new JavaScriptSerializer()).Serialize(accountSynchronize);
                try
                {

                    string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/pay_note/confirm_to_account_title";
                    RestClient.RequestPost<List<IDictionary<string, object>>>(url,json);
                    MessageBox.Show("应付同步到总账成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.payNoteState = FormPayNote.CONFIRM_PAY;
                    this.updateState();
                    this.searchView1.Search();
                }
                catch (WebException ex)
                {
                    string message = ex.Message;
                    if (ex.Response != null)
                    {
                        message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    }
                    MessageBox.Show(("应付同步到总账") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
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
                MessageBox.Show(("计算") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.judgeAllFinish(payNoteId, CALCULATED_PAY))
            {
                if (MessageBox.Show("本单全部条目已经计算税费，是否直接同步到应付总账？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                AccountSynchronize accountSynchronize = new AccountSynchronize();
                accountSynchronize.payNoteId = payNoteId;
                accountSynchronize.personId = ((int)GlobalData.Person["id"]);
                accountSynchronize.warehouseId = ((int)GlobalData.Warehouse["id"]);
                string json = (new JavaScriptSerializer()).Serialize(accountSynchronize);
                try
                {

                    string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/pay_note/confirm_to_account_title";
                    RestClient.RequestPost<List<IDictionary<string, object>>>(url,json);
                    MessageBox.Show("应付同步到总账成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.searchView1.Search();
                    this.payNoteState = FormPayNote.CONFIRM_PAY;
                    this.updateState();
                }
                catch (WebException ex)
                {
                    string message = ex.Message;
                    if (ex.Response != null)
                    {
                        message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    }
                    MessageBox.Show(("应付同步到总账") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void buttonRealPayItems_Click(object sender, EventArgs e)
        {

            if (this.model1.SelectionRange.Rows == 0)
            {
                MessageBox.Show("请选择薪金单条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(this.getSelectRowIds()); 
            string json = (new JavaScriptSerializer()).Serialize(rowData);

            try
            {
                string body =json ;
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/pay_note_item/real_pay_part_items";
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
                MessageBox.Show(("计算") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.judgeAllFinish(payNoteId,PAYED))
            {
                if (MessageBox.Show("本单全部条目已经支付，是否直接同步到实付总账？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                AccountSynchronize accountSynchronize = new AccountSynchronize();
                accountSynchronize.payNoteId = payNoteId;
                accountSynchronize.personId = ((int)GlobalData.Person["id"]);
                accountSynchronize.warehouseId = ((int)GlobalData.Warehouse["id"]);
                string json1 = (new JavaScriptSerializer()).Serialize(accountSynchronize);
                try
                {

                    string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/pay_note/real_pay_to_account_title";
                    RestClient.RequestPost<List<IDictionary<string, object>>>(url,json1);
                    MessageBox.Show("实付同步到总账成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.searchView1.Search();
                    this.payNoteState = FormPayNote.CONFIRM_REAL_PAY;
                    this.updateState();
                }
                catch (WebException ex)
                {
                    string message = ex.Message;
                    if (ex.Response != null)
                    {
                        message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    }
                    MessageBox.Show(("实付同步到总账失败") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void buttonRealPayAll_Click(object sender, EventArgs e)
        {
            try
            {
                string body = $"{{\"taxId\":\"{this.taxId}\",\"payNoteId\":\"{payNoteId}\"}}";
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/pay_note_item/real_pay_all";
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
                MessageBox.Show(("计算") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.judgeAllFinish(payNoteId, PAYED))
            {
                if (MessageBox.Show("本单全部条目已经支付，是否直接同步到实付总账？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                AccountSynchronize accountSynchronize = new AccountSynchronize();
                accountSynchronize.payNoteId = payNoteId;
                accountSynchronize.personId = ((int)GlobalData.Person["id"]);
                accountSynchronize.warehouseId = ((int)GlobalData.Warehouse["id"]);
                string json = (new JavaScriptSerializer()).Serialize(accountSynchronize);
                try
                {

                    string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/pay_note/real_pay_to_account_title";
                    RestClient.RequestPost<List<IDictionary<string, object>>>(url,json);
                    MessageBox.Show("实付同步到总账成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.searchView1.Search();
                    this.payNoteState = FormPayNote.CONFIRM_REAL_PAY;
                    this.updateState();
                }
                catch (WebException ex)
                {
                    string message = ex.Message;
                    if (ex.Response != null)
                    {
                        message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    }
                    MessageBox.Show(("实付同步到总账") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private bool judgeAllFinish(int payNoteId, int state) {

            try
            {             
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/pay_note_item/judge_all_finish/"+payNoteId+"/"+state;
                return RestClient.RequestPost<bool>(url);          

            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                return false;
            } 
        }
    }
}

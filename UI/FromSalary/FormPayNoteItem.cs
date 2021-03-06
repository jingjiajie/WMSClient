﻿using System;
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
        private Action addFinishedCallback = null;

        private int payNoteId;
        private int periodId;
        private int taxId;
        public int payNoteState;
        private string payNoteNo;
        public FormPayNoteItem(int payNoteId,int periodId,int taxId,int payNoteState,string payNoteNo)
        {
            MethodListenerContainer.Register("FormPayNoteItem", this);
            InitializeComponent();
            this.payNoteId = payNoteId;
            this.periodId = periodId;
            this.taxId = taxId;
            this.payNoteState = payNoteState;
            this.payNoteNo = payNoteNo;
            this.model1.SelectionRangeChanged += this.model_SelectionRangeChanged;
        }

        public void SetAddFinishedCallback(Action callback)
        {
            this.addFinishedCallback = callback;
        }

        private void model_SelectionRangeChanged(object sender, ModelSelectionRangeChangedEventArgs e)
        {
            this.UpdateItemState();
        }

        private void FormPayNoteItem_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            Utilities.BindBlueButton(this.buttonCalculateAllTax);
            Utilities.BindBlueButton(this.buttonCclcultateItemsTax);
            this.searchView1.AddStaticCondition("payNoteId", payNoteId);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
            this.UpdateState();
            this.UpdateItemState();
        }

        private void UpdateItemState()
        {
            if (this.model1.SelectionRange == null) { return; }
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row });
            if (rowData == null) { return; }
            if (rowData[0]["id"] == null) {
                this.model1.Mode = "add";
                this.basicView1.Mode = "add";
                this.reoGridView1.Mode = "add";
                this.buttonCclcultateItemsTax.Enabled = false;
                this.buttonCalculateAllTax.Enabled = false;
                this.ButtonAllPerson.Enabled = false;
                this.toolStripButtonAdd.Enabled = true;
                this.toolStripButtonDelete.Enabled = true;
                return;
            }
            if ((int)rowData[0]["state"] == 0)
            {
                this.model1.Mode = "default";
                this.basicView1.Mode = "default";
                this.reoGridView1.Mode = "default";           
                this.buttonCclcultateItemsTax.Enabled = true;
                this.buttonCalculateAllTax.Enabled = true;
                this.ButtonAllPerson.Enabled = true;
                this.toolStripButtonAdd.Enabled = true;
                this.toolStripButtonDelete.Enabled = true;
            }
            else if ((int)rowData[0]["state"] == 2)
            {
                if (this.payNoteState == FormPayNote.CONFIRM_REAL_PAY)
                {
                    //条目已确认实付
                    this.basicView1.Mode = "payed";
                    this.reoGridView1.Mode = "payed";
                    this.model1.Mode = "payed";
                    this.buttonCclcultateItemsTax.Enabled = false;
                    this.buttonCalculateAllTax.Enabled = false;
                    this.ButtonAllPerson.Enabled = false;
                    this.toolStripButtonAdd.Enabled = false;
                    this.toolStripButtonDelete.Enabled = false;
                }
                else if (this.payNoteState == FormPayNote.WAITING_FOR_CONFIRM)
                {
                    //条目已确认实付
                    this.basicView1.Mode = "pre-pay";
                    this.reoGridView1.Mode = "pre-pay";
                    this.model1.Mode = "pre-pay";
                    this.buttonCclcultateItemsTax.Enabled = true;
                    this.buttonCalculateAllTax.Enabled = true;
                    this.toolStripButtonAdd.Enabled = false;
                    this.toolStripButtonDelete.Enabled = false;
                }
            }

        }


        public void UpdateState() {
            if (this.payNoteState == FormPayNote.CONFIRM_REAL_PAY) {
                this.basicView1.Mode = "payed";
                this.reoGridView1.Mode = "payed";
                this.model1.Mode = "payed";               
                this.buttonCclcultateItemsTax.Enabled = false;
                this.buttonCalculateAllTax.Enabled = false;
                this.ButtonAllPerson.Enabled = false;
                this.toolStripButtonAdd.Enabled = false;
                this.toolStripButtonDelete.Enabled = false;
            }
            else
            {
                this.basicView1.Mode = "default";
                this.reoGridView1.Mode = "default";
                this.model1.Mode = "default";
                //this.buttonCclcultateItemsTax.Enabled = true;
                this.buttonCalculateAllTax.Enabled = true;
                this.ButtonAllPerson.Enabled = true;
                this.toolStripButtonAdd.Enabled = true;
                this.toolStripButtonDelete.Enabled = true;              
            }

        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {                             
                { "payNoteId",payNoteId},
                { "state",WAITING_FOR_CALCULATE_PAY},
                { "paidAmount",0}
            });
            this.UpdateItemState();
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
                this.UpdateItemState();
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
                List<int> personIdExist = this.GetPersonId();
                if (personIdExist.Contains((int)foundPerson["id"])) {                
                    MessageBox.Show($"人员\"{personName}\"已经在此单中存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.model1[row, "personName"] = "";
                    this.model1[row, "personId"] = null;
                    return;
                }
                this.model1[row, "personId"] = foundPerson["id"];
            }
        }

        private void PreTaxAmountEditEnded([Row]int row, [Data] double preTaxAmount)
        {
            double? taxAmount = (double?)this.model1[row, "taxAmount"];
            if (taxAmount.HasValue == false)
            {
              return;
            }
            else
            {
              this.model1[row, "afterTaxAmount"] = preTaxAmount-taxAmount;
            }
        }

        private void TaxAmountEditEnded([Row]int row, [Data] double taxAmount)
        {
            double? preTaxAmount = (double?)this.model1[row, "preTaxAmount"];
            if (preTaxAmount.HasValue == false || preTaxAmount == 0)
            {
                return;
            }
            else
            {
                this.model1[row, "afterTaxAmount"] = preTaxAmount - taxAmount;
            }
        }



        private string StateForwardMapper([Data]int state)
        {
            switch (state)
            {
                case 0: return "待计算应付";
                case 1: return "已计算应付";
                case 2: return "已计算";
                default: return "未知状态";
            }
        }

        private int StateBackwardMapper([Data]string enable)
        {
            switch (enable)
            {
                case "待计算应付": return 0;
                case "已计算应付": return 1;
                case "已计算": return 2;
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
            try
            {
                if (this.model1.SelectionRange.Rows == 0)
                {
                    MessageBox.Show("请选择薪金单条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch { return; }
            var rowData = this.model1.GetRows(this.getSelectRowIds());
            bool dateNull = true;
            StringBuilder payNoteIds = new StringBuilder();
            payNoteIds.Append("[");
            foreach (var a in rowData) {
                if (a["id"] == null) { continue; }
                dateNull = false;
                payNoteIds.Append((int)a["id"]);
                payNoteIds.Append(",");               
            }
            if (dateNull) { MessageBox.Show("计算失败：薪金发放单中无条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);return; }
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
                MessageBox.Show(("计算") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.judgeAllFinish(payNoteId, PAYED)) {
                if (MessageBox.Show("本单全部条目已经计算税费，是否直接同步到总账？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                AccountSynchronize accountSynchronize = new AccountSynchronize();
                accountSynchronize.payNoteId = payNoteId;
                accountSynchronize.personId = ((int)GlobalData.Person["id"]);
                accountSynchronize.warehouseId = ((int)GlobalData.Warehouse["id"]);
                accountSynchronize.comment = "薪金自动同步到总账";
                accountSynchronize.voucherInfo = this.payNoteNo;
                if (GlobalData.AccountPeriod == null) { MessageBox.Show("当前会计期间为空，请先检查会计期间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                accountSynchronize.accountPeriodId = (int)GlobalData.AccountPeriod["id"];
                string json = (new JavaScriptSerializer()).Serialize(accountSynchronize);
                try
                {

                    string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/pay_note/real_pay_to_account_title";
                    RestClient.RequestPost<List<IDictionary<string, object>>>(url,json);
                    MessageBox.Show("应付同步到总账成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    addFinishedCallback?.Invoke();
                    this.payNoteState = FormPayNote.CONFIRM_REAL_PAY;
                    this.UpdateState();
                    this.searchView1.Search();
                }
                catch (WebException ex)
                {
                    string message = ex.Message;
                    if (ex.Response != null)
                    {
                        message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    }
                    MessageBox.Show(("薪金同步到总账") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void buttonCalculateAllTax_Click(object sender, EventArgs e)
        {
            if (this.model1.RowCount == 0) { return; }
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
            if (this.judgeAllFinish(payNoteId, PAYED))
            {
                if (MessageBox.Show("本单全部条目已经计算税费，是否直接同步到总账？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                AccountSynchronize accountSynchronize = new AccountSynchronize();
                accountSynchronize.payNoteId = payNoteId;
                accountSynchronize.personId = ((int)GlobalData.Person["id"]);
                accountSynchronize.warehouseId = ((int)GlobalData.Warehouse["id"]);
                accountSynchronize.comment = "薪金自动同步到总账";
                accountSynchronize.voucherInfo = this.payNoteNo;
                if (GlobalData.AccountPeriod == null) { MessageBox.Show("当前会计期间为空，请先检查会计期间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                accountSynchronize.accountPeriodId = (int)GlobalData.AccountPeriod["id"];
                string json = (new JavaScriptSerializer()).Serialize(accountSynchronize);
                try
                {

                    string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/pay_note/real_pay_to_account_title";
                    RestClient.RequestPost<List<IDictionary<string, object>>>(url,json);
                    MessageBox.Show("薪金同步到总账成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    addFinishedCallback?.Invoke();
                    this.searchView1.Search();
                    this.payNoteState = FormPayNote.CONFIRM_REAL_PAY;
                    this.UpdateState();
                }
                catch (WebException ex)
                {
                    string message = ex.Message;
                    if (ex.Response != null)
                    {
                        message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    }
                    MessageBox.Show(("薪金同步到总账") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void buttonRealPayItems_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.model1.SelectionRange.Rows == 0)
                {
                    MessageBox.Show("请选择薪金单条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch { return; }


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
                MessageBox.Show(("确认") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.judgeAllFinish(payNoteId,PAYED))
            {
                if (MessageBox.Show("本单全部条目已经支付，是否直接同步到实付总账？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                AccountSynchronize accountSynchronize = new AccountSynchronize();
                accountSynchronize.payNoteId = payNoteId;
                accountSynchronize.personId = ((int)GlobalData.Person["id"]);
                accountSynchronize.warehouseId = ((int)GlobalData.Warehouse["id"]);
                accountSynchronize.voucherInfo = this.payNoteNo;
                accountSynchronize.comment = "实付自动同步到总账";
                if (GlobalData.AccountPeriod == null) { MessageBox.Show("当前会计期间为空，请先检查会计期间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                accountSynchronize.accountPeriodId = (int)GlobalData.AccountPeriod["id"];
                string json1 = (new JavaScriptSerializer()).Serialize(accountSynchronize);
                try
                {

                    string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/pay_note/real_pay_to_account_title";
                    RestClient.RequestPost<List<IDictionary<string, object>>>(url,json1);
                    MessageBox.Show("实付同步到总账成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    addFinishedCallback?.Invoke();
                    this.searchView1.Search();
                    this.payNoteState = FormPayNote.CONFIRM_REAL_PAY;
                    this.UpdateState();
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
            if (this.model1.RowCount == 0) { return; }
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
                MessageBox.Show(("确认") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (this.judgeAllFinish(payNoteId, PAYED))
            {
                if (MessageBox.Show("本单全部条目已经支付，是否直接同步到实付总账？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                AccountSynchronize accountSynchronize = new AccountSynchronize();
                accountSynchronize.payNoteId = payNoteId;
                accountSynchronize.personId = ((int)GlobalData.Person["id"]);
                accountSynchronize.warehouseId = ((int)GlobalData.Warehouse["id"]);
                accountSynchronize.voucherInfo = this.payNoteNo;
                accountSynchronize.comment = "实付自动同步到总账";
                if (GlobalData.AccountPeriod == null) { MessageBox.Show("当前会计期间为空，请先检查会计期间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                accountSynchronize.accountPeriodId = (int)GlobalData.AccountPeriod["id"];
                string json = (new JavaScriptSerializer()).Serialize(accountSynchronize);
                try
                {

                    string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/pay_note/real_pay_to_account_title";
                    RestClient.RequestPost<List<IDictionary<string, object>>>(url,json);
                    MessageBox.Show("实付同步到总账成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    addFinishedCallback?.Invoke();
                    this.searchView1.Search();
                    this.payNoteState = FormPayNote.CONFIRM_REAL_PAY;
                    this.UpdateState();
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

        private void FormPayNoteItem_FormClosed(object sender, FormClosedEventArgs e)
        {
            addFinishedCallback?.Invoke();
        }

        private List<int> GetPersonId()
        {
            List<int> personId = new List<int>();
            for (int i = 0; i < this.model1.RowCount; i++)
            {
                if (this.model1.GetRow(i)["id"] != null)
                {
                    personId.Add((int)this.model1.GetRow(i)["personId"]);
                }       
            }
            return personId;      
        }
    }
}

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
    public partial class FormPayNote : Form
    {
        public FormPayNote()
        {
            MethodListenerContainer.Register("FormPayNote", this);
            InitializeComponent();
        }
        private FormPayNoteItem form ;

        public const  int WAITING_FOR_CONFIRM = 0;
        public const  int CONFIRM_PAY = 1;
        public const  int CONFIRM_REAL_PAY = 2;

    
        private void FormPayNote_Load(object sender, EventArgs e)
        {

            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
            this.model1.SelectionRangeChanged += this.model_SelectionRangeChanged;
            this.model1.RowRemoved += this.model_RowRemoved;
            this.model1.Refreshed += this.model_Refreshed;
            Utilities.BindBlueButton(this.buttonAccountRealPay);
            this.UpdateBasicAndReoGridView();
            this.RefreshState();
        }

        private void model_Refreshed(object sender, ModelRefreshedEventArgs e)
        {
            this.UpdateBasicAndReoGridView();
            this.RefreshState();
        }

        private void UpdateBasicAndReoGridView()
        {

            if (this.model1.RowCount == 0)
            {
                this.buttonAccountRealPay.Enabled= false;
                //Utilities.ButtonEffectsCancel(this.buttonAccountPay);
                //Utilities.ButtonEffectsCancel(this.buttonAccountRealPay);
            }
            else
            {
                this.buttonAccountRealPay.Enabled = true;
            }
        }

        private void RefreshState()
        {
            var rowData = this.model1.GetSelectedRow();
            if (rowData == null) { return; }
            if (rowData["id"] == null)
            {
                this.buttonAccountRealPay.Enabled = false;
                this.toolStripButtonDelete.Enabled = true;
                this.ChangeConfigMode("creat");
                return;
            }
            if ((int)rowData["state"] == 0)
            {          
                this.buttonAccountRealPay.Enabled = true;
                this.toolStripButtonDelete.Enabled = true;
                this.ChangeConfigMode("default");
            }
            else if ((int)rowData["state"] == 1)
            {      
                this.buttonAccountRealPay.Enabled = true;
                this.toolStripButtonDelete.Enabled = false;
                this.ChangeConfigMode("payed");
            }
            else
            {
                this.buttonAccountRealPay.Enabled = false;
                this.toolStripButtonDelete.Enabled = false;
                this.ChangeConfigMode("payed");
            }                           
        }

        private void model_RowRemoved(object sender, ModelRowRemovedEventArgs e)
        {
            this.UpdateBasicAndReoGridView();
        }

        private void model_SelectionRangeChanged(object sender, ModelSelectionRangeChangedEventArgs e)
        {
            if (this.model1.RowCount == 0) { return; }
            if (this.model1.SelectionRange.Rows != 1)
            {
                this.buttonAccountRealPay.Enabled = false;
                return;
            }
            this.RefreshState();
            //var rowData = this.model1.GetSelectedRow();

            //    if ((int)rowData["state"] == 0)
            //    {
            //        this.buttonAccountPay.Enabled = true;
            //        this.buttonAccountRealPay.Enabled = true;
            //    this.ChangeConfigMode("default");
            //    }
            //    else if ((int)rowData["state"] == 1)
            //    {
            //        this.buttonAccountPay.Enabled = false;
            //        this.buttonAccountRealPay.Enabled = true;
            //    this.ChangeConfigMode("payed");
            //    }
            //    else
            //    {
            //        this.buttonAccountPay.Enabled = false;
            //        this.buttonAccountRealPay.Enabled = false;
            //    this.ChangeConfigMode("payed");
            //    }
        }

        private void ChangeConfigMode(string mode)
        {
            this.model1.Mode =mode;
            this.basicView1.Mode = mode;
            this.reoGridView1.Mode = mode;
            //this.synchronizer.Mode = mode;
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "createPersonId",GlobalData.Person["id"]},
                { "createPersonName",GlobalData.Person["name"]},
                { "createTime",DateTime.Now},
                { "warehouseName",GlobalData.Warehouse["name"]}
            });
            this.RefreshState();
            //this.updateBasicAndReoGridView();
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
                this.RefreshState();
            }
        }

        private void AccountTitleExpenseNameEditEnded([Row]int row, [Data]string accountTitleName)
        {
            IDictionary<string, object> foundAccountTitle =
                GlobalData.AllAccountTitle.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == accountTitleName;
                });
            if (foundAccountTitle == null)
            {
                MessageBox.Show($"科目\"{accountTitleName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "accountTitleExpenseNo"] = foundAccountTitle["no"];
                this.model1[row, "accountTitleExpenseId"] = foundAccountTitle["id"];
            }
        }

        private void AccountTitleExpenseNoEditEnded([Row]int row, [Data]string accountTitleNo)
        {
            IDictionary<string, object> foundAccountTitle =
                GlobalData.AllAccountTitle.Find((s) =>
                {
                    if (s["no"] == null) return false;
                    return s["no"].ToString() == accountTitleNo;
                });
            if (foundAccountTitle == null)
            {
                MessageBox.Show($"科目\"{accountTitleNo}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "accountTitleExpenseName"] = foundAccountTitle["name"];
                this.model1[row, "accountTitleExpenseId"] = foundAccountTitle["id"];
            }
        }

        private void AccountTitlePayableNameEditEnded([Row]int row, [Data]string accountTitleName)
        {
            IDictionary<string, object> foundAccountTitle =
                GlobalData.AllAccountTitle.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == accountTitleName;
                });
            if (foundAccountTitle == null)
            {
                MessageBox.Show($"科目\"{accountTitleName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "accountTitlePayableNo"] = foundAccountTitle["no"];
                this.model1[row, "accountTitlePayableId"] = foundAccountTitle["id"];
            }
        }

        private void AccountTitlePayableNoEditEnded([Row]int row,[Data] string accountTitleNo)
        {
            IDictionary<string, object> foundAccountTitle =
                GlobalData.AllAccountTitle.Find((s) =>
                {
                    if (s["no"] == null) return false;
                    return s["no"].ToString() == accountTitleNo;
                });
            if (foundAccountTitle == null)
            {
                MessageBox.Show($"科目\"{accountTitleNo}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "accountTitlePayableName"] = foundAccountTitle["name"];
                this.model1[row, "accountTitlePayableId"] = foundAccountTitle["id"];
            }
        }

        private void AccountTitlePropertyNameEditEnded([Row]int row,[Data] string accountTitleName)            
        {
            IDictionary<string, object> foundAccountTitle =
                GlobalData.AllAccountTitle.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == accountTitleName;
                });
            if (foundAccountTitle == null)
            {
                MessageBox.Show($"科目\"{accountTitleName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "accountTitlePropertyNo"] = foundAccountTitle["no"];
                this.model1[row, "accountTitlePropertyId"] = foundAccountTitle["id"];
            }
        }

        private void AccountTitlePropertyNoEditEnded([Row]int row,[Data] string accountTitleNo)
        {
            IDictionary<string, object> foundAccountTitle =
                GlobalData.AllAccountTitle.Find((s) =>
                {
                    if (s["no"] == null) return false;
                    return s["no"].ToString() == accountTitleNo;
                });
            if (foundAccountTitle == null)
            {
                MessageBox.Show($"科目\"{accountTitleNo}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "accountTitlePropertyName"] = foundAccountTitle["name"];
                this.model1[row, "accountTitlePropertyId"] = foundAccountTitle["id"];
            }
        }

        private void SalaryPeriodNameEditEnded([Row]int row,[Data] string periodName)
        {
            IDictionary<string, object> foundSalaryPeriod =
                GlobalData.AllSalaryPeriod.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == periodName;
                });
            if (foundSalaryPeriod == null)
            {
                MessageBox.Show($"期间\"{periodName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "salaryPeriodName"] = foundSalaryPeriod["name"];
                this.model1[row, "salaryPeriodId"] = foundSalaryPeriod["id"];
            }
        }

        private void SalaryTypeNameEditEnded([Row]int row, [Data] string typeName)
        {
            IDictionary<string, object> foundSalaryType =
                GlobalData.AllSalaryType.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == typeName;
                });
            if (foundSalaryType == null)
            {
                MessageBox.Show($"类型\"{typeName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {               
                this.model1[row, "salaryTypeId"] = foundSalaryType["id"];
            }
        }

        private void buttonItems_Click(object sender, EventArgs e)
        {
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项税务条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row })[0];       
            FormPayNoteTax form = new FormPayNoteTax((int)rowData["id"],(string)rowData["no"]);
            form.Show();
        }

        private string StateForwardMapper([Data]int state)
        {
            switch (state)
            {
                case 0: return "待确认";
                case 1: return "已确认应付";
                case 2: return "已确认实付";
                default: return "未知状态";
            }
        }

        private int StateBackwardMapper([Data]string enable)
        {
            switch (enable)
            {
                case "待确认": return 0;
                case "已确认应付": return 1;
                case "已确认实付": return 2;
                default: return -1;
            }
        }

        private void TaxNameEditEnded([Row]int row, [Data] string taxName)
        {
            IDictionary<string, object> foundTax =
                GlobalData.AllTax.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == taxName;
                });
            if (foundTax == null)
            {
                MessageBox.Show($"税务\"{taxName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "taxNo"] = foundTax["no"];
                this.model1[row, "taxId"] = foundTax["id"];
            }
        }

        private void TaxNoEditEnded([Row]int row, [Data] string taxNo)
        {
            IDictionary<string, object> foundTax =
                GlobalData.AllTax.Find((s) =>
                {
                    if (s["no"] == null) return false;
                    return s["no"].ToString() == taxNo;
                });
            if (foundTax == null)
            {
                MessageBox.Show($"税务\"{taxNo}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "taxName"] = foundTax["name"];
                this.model1[row, "taxId"] = foundTax["id"];
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.model1.RowCount == 0) { return; }
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项薪资条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row })[0];
            if (rowData["id"] ==null) { return; }
            FormPayNoteItem form = new FormPayNoteItem((int)rowData["id"], (int)rowData["salaryPeriodId"], (int)rowData["taxId"],(int)rowData["state"],(string)rowData["no"]);
            this.form = form;
            form.SetAddFinishedCallback(() =>
            {
                this.searchView1.Search();
                this.RefreshState();
                this.form = null;
            });
            form.Show();      
        }

        public int[] getSelectRowIds()
        {
            List<int> selectIds = new List<int>();
            for (int i = 0; i < this.model1.SelectionRange.Rows; i++)
            {
                selectIds.Add(this.model1.SelectionRange.Row + i);
            }
            return selectIds.ToArray();
        }

        private void buttonAccountPay_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.model1.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择薪金单！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch { return; }
            var rowData = this.model1.GetRows(getSelectRowIds());        
            AccountSynchronize accountSynchronize=new AccountSynchronize();
            if (rowData[0]["id"] == null || rowData[0]["no"] == null) { return; }
            accountSynchronize.payNoteId=((int)rowData[0]["id"]);
            accountSynchronize.personId=((int)GlobalData.Person["id"]);
            accountSynchronize.warehouseId=((int)GlobalData.Warehouse["id"]);
            accountSynchronize.voucherInfo= (string)rowData[0]["no"];
            accountSynchronize.comment = "应付自动同步到总账";
            if (GlobalData.AccountPeriod == null) { MessageBox.Show("当前会计期间为空，请先检查会计期间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);return; }
            accountSynchronize.accountPeriodId =(int) GlobalData.AccountPeriod["id"];
            string jsonstr = JsonConvert.SerializeObject(accountSynchronize);       
            string json = (new JavaScriptSerializer()).Serialize(accountSynchronize);      
            try
            {

                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/pay_note/confirm_to_account_title";                
                RestClient.RequestPost<List<IDictionary<string, object>>>(url,json);
                MessageBox.Show("应付同步到总账成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.searchView1.Search();
                this.RefreshState();
                if (form != null)
                {
                    this.form.payNoteState = FormPayNote.CONFIRM_PAY;
                    this.form.UpdateState();
                }
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



        private void buttonAccountRealPay_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.model1.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项薪金单！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch { return; }
            var rowData = this.model1.GetRows(getSelectRowIds());
            AccountSynchronize accountSynchronize = new AccountSynchronize();
            if (rowData[0]["id"] == null || rowData[0]["no"] == null) { return; }
            accountSynchronize.payNoteId = ((int)rowData[0]["id"]);
            accountSynchronize.personId = ((int)GlobalData.Person["id"]);
            accountSynchronize.warehouseId = ((int)GlobalData.Warehouse["id"]);
            accountSynchronize.voucherInfo = (string)rowData[0]["no"];
            accountSynchronize.comment = "实付自动同步到总账";
            if (GlobalData.AccountPeriod == null) { MessageBox.Show("当前会计期间为空，请先检查会计期间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            accountSynchronize.accountPeriodId = (int)GlobalData.AccountPeriod["id"];
            string jsonstr = JsonConvert.SerializeObject(accountSynchronize);
            string json = (new JavaScriptSerializer()).Serialize(accountSynchronize);
            try
            {

                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/pay_note/real_pay_to_account_title";
                RestClient.RequestPost<List<IDictionary<string, object>>>(url,json);
                MessageBox.Show("实付同步到总账成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.searchView1.Search();
                this.RefreshState();
                if (form != null)
                {
                    this.form.payNoteState = FormPayNote.CONFIRM_REAL_PAY;
                    this.form.UpdateState();
                }
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
}

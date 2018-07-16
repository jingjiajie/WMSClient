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
    public partial class FormPayNote : Form
    {
        public FormPayNote()
        {
            MethodListenerContainer.Register("FormPayNote", this);
            InitializeComponent();
        }

        public const  int WAITING_FOR_CONFIRM = 0;
        public const  int CONFIRM_PAY = 1;
        public const  int CONFIRM_REAL_PAY = 2;

        private void FormPayNote_Load(object sender, EventArgs e)
        {
            Utilities.BindBlueButton(this.buttonAccountPay);
            Utilities.BindBlueButton(this.buttonAccountRealPay);
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
            this.model1.SelectionRangeChanged += this.model_SelectionRangeChanged;
        }

        private void model_SelectionRangeChanged(object sender, ModelSelectionRangeChangedEventArgs e)
        {
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row });
            try
            {
                if ((int)rowData[0]["state"] == 0)
                {
                    this.buttonAccountPay.Enabled = true;
                    this.buttonAccountRealPay.Enabled = true;
                }
                else if ((int)rowData[0]["state"] == 1)
                {
                    this.buttonAccountPay.Enabled = false;
                    this.buttonAccountRealPay.Enabled = true;
                }
                else
                {
                    this.buttonAccountPay.Enabled = false;
                    this.buttonAccountRealPay.Enabled = false;
                }
            }
            catch { }
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
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项税务条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row })[0];
            FormPayNoteItem form = new FormPayNoteItem((int)rowData["id"], (int)rowData["salaryPeriodId"], (int)rowData["taxId"],(int)rowData["state"]);
            form.ShowDialog();
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
            if (this.model1.SelectionRange.Rows !=1 )
            {
                MessageBox.Show("请选择薪金单！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(getSelectRowIds());
            try
            {

                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/pay_note/confirm_to_account_title/"+rowData[0]["id"].ToString()+"/"+GlobalData.Person["id"];            
                RestClient.RequestPost<List<IDictionary<string, object>>>(url);
                MessageBox.Show("应付同步到总账成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.searchView1.Search();
            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show(("应付同步到总账失败") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void buttonAccountRealPay_Click(object sender, EventArgs e)
        {
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择薪金单！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(getSelectRowIds());
            try
            {

                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/pay_note/real_pay_to_account_title/" + rowData[0]["id"].ToString() + "/" + GlobalData.Person["id"];
                RestClient.RequestPost<List<IDictionary<string, object>>>(url);
                MessageBox.Show("应付同步到总账成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.searchView1.Search();
            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show(("应付同步到总账失败") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}

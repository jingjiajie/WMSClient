using FrontWork;
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
using System.Web.Script.Serialization;

namespace WMS.UI.FormSettlement
{
    public partial class FormSettlementNote : Form
    {
        public FormSettlementNote()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private void FormSettlementNote_Load(object sender, EventArgs e)
        {
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "createPersonId",GlobalData.Person["id"]},
                { "createPersonName",GlobalData.Person["name"]},
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "createTime",DateTime.Now},
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

        private void buttonItems_Click(object sender, EventArgs e)
        {
            if (this.model1.SelectionRange == null) { return; }
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项汇总单查看结算单条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row })[0];
            var a1 = new FormSettlementNoteItem(rowData);
            a1.SetAddFinishedCallback(() =>
            {
                this.searchView1.Search();
            });
            a1.Show();
        }

        private void AccountTitleIncomeNameEditEnded(int row, string accountTitleName)
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
                this.model1[row, "accountTitleIncomeId"] = foundAccountTitle["id"];
                this.model1[row, "accountTitleIncomeNo"] = foundAccountTitle["no"];
            }

        }
        private void AccountTitleIncomeNoEditEnded(int row, string accountTitleNo)
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
                this.model1[row, "accountTitleIncomeId"] = foundAccountTitle["id"];
                this.model1[row, "accountTitleIncomeName"] = foundAccountTitle["name"];
            }
        }

        private void AccountTitleReceivableNameEditEnded(int row, string accountTitleName)
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
                this.model1[row, "accountTitleReceivableId"] = foundAccountTitle["id"];
                this.model1[row, "accountTitleReceivableNo"] = foundAccountTitle["no"];
            }

        }
        private void AccountTitleReceivableNoEditEnded(int row, string accountTitleNo)
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
                this.model1[row, "accountTitleReceivableId"] = foundAccountTitle["id"];
                this.model1[row, "accountTitleReceivableName"] = foundAccountTitle["name"];
            }
        }

        private void AccountTitlePropertyNameEditEnded(int row, string accountTitleName)
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
                this.model1[row, "accountTitlePropertyId"] = foundAccountTitle["id"];
                this.model1[row, "accountTitlePropertyNo"] = foundAccountTitle["no"];
            }

        }

        private void AccountTitlePropertyNoEditEnded(int row, string accountTitleNo)
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
                this.model1[row, "accountTitlePropertyId"] = foundAccountTitle["id"];
                this.model1[row, "accountTitlePropertyName"] = foundAccountTitle["name"];
            }
        }

        private void SummaryNoteNoEditEnded(int row, string summaryNoteNo)
        {
            IDictionary<string, object> foundSummaryNote =
                GlobalData.AllSummaryNote.Find((s) =>
                {
                    if (s["no"] == null) return false;
                    return s["no"].ToString() == summaryNoteNo;
                });
            if (foundSummaryNote == null)
            {
                MessageBox.Show($"汇总单\"{summaryNoteNo}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "summaryNoteId"] = foundSummaryNote["id"];
            }
        }
        

        private string StateForwardMapper([Data]int state)
        {
            switch (state)
            {
                case 0: return "待确认";
                case 1: return "已同步总账应收款";
                case 2: return "已同步总账实付款";
                default: return "未知状态";
            }
        }

        private int StateBackwardMapper([Data]string state)
        {
            switch (state)
            {
                case "待确认": return 0;
                case "已同步总账应收款": return 1;
                case "已同步总账实付款": return 2;
                default: return -1;
            }
        }

        //同步应收款
        private void toolStripButtonReceivables_Click(object sender, EventArgs e)
        {
            //获取选中行ID，过滤掉新建的行（ID为0的）
            int[] selectedIDs = this.model1.GetSelectedRows<int>("id").Except(new int[] { 0 }).ToArray();
            if (selectedIDs.Length == 0)
            {
                MessageBox.Show("请选择一项进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string strIDs = serializer.Serialize(selectedIDs);
            LedgerSynchronous ledgerSynchronous = new LedgerSynchronous();
            ledgerSynchronous.accountPeriodId = (int)GlobalData.AccountPeriod["id"];
            ledgerSynchronous.personId = (int)GlobalData.Person["id"];
            ledgerSynchronous.settlementNoteIds = selectedIDs;
            string body1 = serializer.Serialize(ledgerSynchronous);
            string body = "{\"settlementNoteIds\":\"" + "{" +strIDs+"}" + "\",\"personId\":\"" + GlobalData.Person["id"] + "\",\"accountPeriodId\":\"" + GlobalData.AccountPeriod["id"] + "\"}";
            try
            {
                string operatioName = "synchronous_receivables";
                RestClient.RequestPost<string>(Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/settlement_note/" + operatioName, body1, "POST");
                this.searchView1.Search();
                MessageBox.Show("同步结算单应收款操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show(("同步结算单应收款操作") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //同步实收款
        private void toolStripButtonReceipts_Click(object sender, EventArgs e)
        {
            //获取选中行ID，过滤掉新建的行（ID为0的）
            int[] selectedIDs = this.model1.GetSelectedRows<int>("id").Except(new int[] { 0 }).ToArray();
            if (selectedIDs.Length == 0)
            {
                MessageBox.Show("请选择一项进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string strIDs = serializer.Serialize(selectedIDs);
            LedgerSynchronous ledgerSynchronous = new LedgerSynchronous();
            ledgerSynchronous.accountPeriodId = (int)GlobalData.AccountPeriod["id"];
            ledgerSynchronous.personId = (int)GlobalData.Person["id"];
            ledgerSynchronous.settlementNoteIds = selectedIDs;
            string body1 = serializer.Serialize(ledgerSynchronous);
            string body = "{\"settlementNoteIds\":\"" + strIDs + "\",\"personId\":\"" + GlobalData.Person["id"] + "\",\"accountPeriodId\":\"" + GlobalData.AccountPeriod["id"] + "\"}";
            try
            {
                string operatioName = "synchronous_receipt";
                RestClient.RequestPost<string>(Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/settlement_note/" + operatioName, body1, "POST");
                this.searchView1.Search();
                MessageBox.Show("同步结算单实收款操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show(("同步结算单实收款操作") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

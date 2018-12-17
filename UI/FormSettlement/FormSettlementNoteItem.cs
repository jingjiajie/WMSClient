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

namespace WMS.UI.FormSettlement
{
    public partial class FormSettlementNoteItem : Form
    {
        private IDictionary<string, object> settlementNote = null;
        private Action addFinishedCallback = null;
        int noteState = -1;
        public FormSettlementNoteItem(IDictionary<string, object> settlementNote)
        {
            this.settlementNote = settlementNote;
         
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private void RefreshMode()
        {
            if ((int)this.model1.GetRows(new int[] { this.model1.SelectionRange.Row })[0]["state"] == 1
                ||this.noteState==1)
            {
                this.basicView1.Mode = "default1";
                this.reoGridView1.Mode = "default1";
            }
            else if(this.noteState == 2)
            {
                this.basicView1.Mode = "default2";
                this.reoGridView1.Mode = "default2";
            }
            else
            {
                this.basicView1.Mode = "default";
                this.reoGridView1.Mode = "default";
            }
        }

        private void model1_SelectionRangeChanged(object sender, ModelSelectionRangeChangedEventArgs e)
        {
            this.RefreshMode();
        }

        private void model1_Refreshed(object sender, ModelRefreshedEventArgs e)
        {
            this.RefreshMode();
        }

        private void FormSettlementNoteItem_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();

            this.searchView1.AddStaticCondition("settlementNoteId", this.settlementNote["id"]);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();

            //this.noteState = (int)this.settlementNote["state"];
            //if (noteState == 1)
            //{
            //    this.model1.Mode = "default1";
            //    this.basicView1.Mode = "default1";
            //    this.reoGridView1.Mode = "default1";
            //}
            //if (noteState == 2)
            //{
            //    this.model1.Mode = "default2";
            //    this.basicView1.Mode = "default2";
            //    this.reoGridView1.Mode = "default2";
            //}

            this.RefreshMode();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
            }
        }

        private void StorageChargeEditEnded([Row]int row, [Data] string storageCharge)
        {
            double? logisticFee = (double?)this.model1[row, "logisticFee"];
            if (logisticFee.HasValue == false)
            {
                this.model1[row, "logisticFee"] = 0;
                this.model1[row, "planPayment"] = storageCharge;

            }
            else
            {
                try
                {
                    this.model1[row, "planPayment"] = logisticFee.Value + Double.Parse(storageCharge);
                }
                catch {
                }
            }
        }

        private void logisticFeeEditEnded([Row]int row, [Data] string logisticFee)
        {
            double? storageCharge = (double?)this.model1[row, "storageCharge"];
            if (storageCharge.HasValue == false)
            {
                this.model1[row, "storageCharge"] = 0;
                this.model1[row, "planPayment"] = logisticFee;

            }
            else {
                try
                {
                    this.model1[row, "planPayment"] = storageCharge.Value + Double.Parse(logisticFee);
                }
                catch
                {
                }
            }
        }

        private string PlanPaymentForwardMapper([Data]double amount, [Row] int row)
        {
            double? storageCharge = (double?)this.model1[row, "storageCharge"];
            double? logisticFee = (double?)this.model1[row, "logisticFee"];
            return Utilities.DoubleToString(storageCharge.Value + logisticFee.Value);
            
        }

        //供应商名称编辑完成，根据名称自动搜索ID和No
        private void SupplierNameEditEnded([Row]int row, [Data] string supplierName)
        {
            IDictionary<string, object> foundSupplier =
                GlobalData.AllSuppliers.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == supplierName && s["warehouseId"] != GlobalData.Warehouse["id"];
                });
            if (foundSupplier == null)
            {
                MessageBox.Show($"供应商\"{supplierName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "supplierId"] = foundSupplier["id"];
                this.model1[row, "no"] = foundSupplier["no"];
            }
        }

        //供应商代号编辑完成，根据名称自动搜索ID和No
        private void SupplierNoEditEnded([Row]int row, [Data] string supplierNo)
        {
            IDictionary<string, object> foundSupplier =
                GlobalData.AllSuppliers.Find((s) =>
                {
                    if (s["no"] == null) return false;
                    return s["name"].ToString() == supplierNo && s["warehouseId"] != GlobalData.Warehouse["id"];
                });
            if (foundSupplier == null)
            {
                MessageBox.Show($"供应商\"{supplierNo}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "supplierId"] = foundSupplier["id"];
                this.model1[row, "supplierName"] = foundSupplier["name"];
            }
        }

        public void SetAddFinishedCallback(Action callback)
        {
            this.addFinishedCallback = callback;
        }

        private void FormPutAwayClosed(object sender, FormClosedEventArgs e)
        {
            if (this.addFinishedCallback != null)
            {
                this.addFinishedCallback();
            }
        }

        private string StateForwardMapper([Data]int state)
        {
            switch (state)
            {
                case 0: return "待供货商确认";
                case 1: return "供货商已确认";
                default: return "未知状态";
            }
        }

        private int StateBackwardMapper([Data]string state)
        {
            switch (state)
            {
                case "待供货商确认": return 0;
                case "供货商已确认": return 1;
                default: return -1;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认保存当前修改并确认选中条目吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.synchronizer.Save();
            //获取选中行ID，过滤掉新建的行（ID为0的）
            int[] selectedIDs = this.model1.GetSelectedRows<int>("id").Except(new int[] { 0 }).ToArray();
            if (selectedIDs.Length == 0)
            {
                MessageBox.Show("请选择一项进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string strIDs = serializer.Serialize(selectedIDs);
            try
            {
                string operatioName = "normalConfirm";
                RestClient.RequestPost<string>(Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/settlement_note_item/" + operatioName, strIDs, "POST");
                this.searchView1.Search();
                MessageBox.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show(("选中条目确认") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using FrontWork;
using System.Net;
using System.Web.Script.Serialization;

namespace WMS.UI.FormSettlement
{
    public partial class FormSummaryNoteItem : Form
    {
        private IDictionary<string, object> summaryNote = null;
        public FormSummaryNoteItem(IDictionary<string, object> summaryNote)
        {
            MethodListenerContainer.Register(this);
            this.summaryNote = summaryNote;
            InitializeComponent();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "summaryNoteId",this.summaryNote["id"]},             
                { "state",0},
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
        }

        private string StateForwardMapper([Data]int state)
        {
            switch (state)
            {
                case 0: return "等待供应商确认";
                case 1: return "供应商已确认";
                default: return "未知状态";
            }
        }

        private int StateBackwardMapper([Data]string state)
        {
            switch (state)
            {
                case "等待供应商确认": return 0;
                case "供应商已确认": return 1;
                default: return -1;
            }
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
                this.model1[row, "supplierNo"] = foundSupplier["no"];
            }
        }

        //供应商代号编辑完成，根据名称自动搜索ID和No
        private void SupplierNoEditEnded([Row]int row, [Data] string supplierNo)
        {
            IDictionary<string, object> foundSupplier =
                GlobalData.AllSuppliers.Find((s) =>
                {
                    if (s["no"] == null) return false;
                    return s["no"].ToString() == supplierNo && s["warehouseId"] != GlobalData.Warehouse["id"];
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

        private void FormSummaryNoteItem_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            this.searchView1.AddStaticCondition("summaryNoteId", this.summaryNote["id"]);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);          
            this.searchView1.Search();
        }

        private void buttonItems_Click(object sender, EventArgs e)
        {
            if (this.model1.SelectionRange == null) { return; }
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项汇总单条目查看详情！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row })[0];
            var a1 = new FormSummaryDetails(rowData);
            a1.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                AddAllItem addAllItem = new AddAllItem();
                addAllItem.summaryNoteId =(int) summaryNote["id"];
                addAllItem.warehouseId = (int)GlobalData.Warehouse["id"];
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/summary_note/generate_summary/";
                string body = serializer.Serialize(addAllItem);
                RestClient.RequestPost<string>(url,body,"POST");
                this.searchView1.Search();
                MessageBox.Show("添加所有条目成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show(("添加所有条目") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    public class summaryNoteItemState
    {
        public const int WAITTING_FOR_SUPPLIER_CONFIRM = 0;
        public const int SUPPLIER_CONFIRMED = 1;
    }

}

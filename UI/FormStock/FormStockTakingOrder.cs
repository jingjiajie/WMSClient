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
using WMS.UI.FormStock;
using Microsoft.VisualBasic;

namespace WMS.UI.FormStockTaking
{
    public partial class FormStockTakingOrder : Form
    {
        public FormStockTakingOrder()
        {
            InitializeComponent();
            this.model1.CellUpdated += this.model_CellUpdated;
            this.model1.RowRemoved += this.model_RowRemoved;
            this.model1.Refreshed += this.model_Refreshed;
        }
        private void model_CellUpdated(object sender, ModelCellUpdatedEventArgs e)
        {
            foreach (var cell in e.UpdatedCells)
            {
                if (cell.FieldName.StartsWith("lastUpdate")) return;
                this.model1[cell.Row, "lastUpdatePersonId"] = GlobalData.Person["id"];
                this.model1[cell.Row, "lastUpdatePersonName"] = GlobalData.Person["name"];
                this.model1[cell.Row, "lastUpdateTime"] = DateTime.Now;
            }
        }

        private void FormStockTakingOrder_Load(object sender, EventArgs e)
        {
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
            this.updateBasicAndReoGridView();
        }

        private void model_Refreshed(object sender, ModelRefreshedEventArgs e)
        {
            this.updateBasicAndReoGridView();
        }

        private void updateBasicAndReoGridView()
        {

          
        }

        //private List<int> rowChange = new List<int>();
        private void model_RowRemoved(object sender, ModelRowRemovedEventArgs e)
        {
            this.updateBasicAndReoGridView();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "warehouseName",GlobalData.Warehouse["name"]},
                { "createPersonId",GlobalData.Person["id"]},
                { "createPersonName",GlobalData.Person["name"]},
                { "lastUpdatePersonId",GlobalData.Person["id"]},
                { "lastUpdatePersonName",GlobalData.Person["name"]},
                { "lastUpdateTime",DateTime.Now},              
                { "createTime",DateTime.Now},
            });
            this.basicView1.Enabled = true;
            this.reoGridView1.Enabled = true;
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

        private void buttonItems_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.model1.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项盘点单查看盘点单条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row })[0];
                var a1 = new FormStockTakingOrderItem(rowData);
                a1.SetAddFinishedCallback(() =>
                {
                    this.searchView1.Search();
                    //this.updateBasicAndReoGridView();
                });
                a1.Show();
            }
            catch {
                MessageBox.Show("无任何数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
        }
      
        private void buttonPreview_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.model1.SelectionRange == null)
                {
                    MessageBox.Show("请选择要预览的盘点单！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                List<int> ids = new List<int>();
                for (int i = 0; i < this.model1.SelectionRange.Rows; i++)
                {
                    int curRow = this.model1.SelectionRange.Row + i;
                    if (this.model1[curRow, "id"] == null) continue;
                    ids.Add((int)this.model1[curRow, "id"]);
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string strIDs = serializer.Serialize(ids);
                var previewData = RestClient.Get<List<IDictionary<string, object>>>(Defines.ServerURL + "/warehouse/WMS_Template/stocktaking_order/preview/" + strIDs);
                if (previewData == null) return;
                StandardFormPreviewExcel formPreviewExcel = new StandardFormPreviewExcel("盘点单预览");
                foreach (IDictionary<string, object> entryAndItem in previewData)
                {
                    IDictionary<string, object> stockTakingOrder = (IDictionary<string, object>)entryAndItem["stockTakingOrderView"];
                    object[] stockTakingOrderItem = (object[])entryAndItem["stockTakingOrderItems"];
                    string no = (string)stockTakingOrder["no"];
                    if (!formPreviewExcel.AddPatternTable("Excel/StockInfoCheckTicket.xlsx", no)) return;
                    formPreviewExcel.AddData("StockInfoCheckTicket", stockTakingOrder, no);
                    formPreviewExcel.AddData("StockInfoCheckTicketItem", stockTakingOrderItem, no);
                }
                formPreviewExcel.Show();
            }
            catch
            {
                MessageBox.Show("无任何数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string body = "";
                if (MessageBox.Show("是否以当前时间进行盘点？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    body = "{\"stockTakingOrderId\":\"" + 0 + "\",\"warehouseId\":\"" + GlobalData.Warehouse["id"] + "\",\"personId\":\"" + GlobalData.Person["id"] + "\",\"checkTime\":\"" + DateTime.Now + "\"}";                         
                }
                else
                {
                    string s = Interaction.InputBox("请输入盘点时间", "提示", "1", -1, -1);  //-1表示在屏幕的中间
                }
                body = "{\"stockTakingOrderId\":\"" + 0 + "\",\"warehouseId\":\"" + GlobalData.Warehouse["id"] + "\",\"personId\":\"" + GlobalData.Person["id"] + "\",\"checkTime\":\"" + DateTime.Now + "\"}";
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/stocktaking_order_item/add_all";
                RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);
                MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.searchView1.Search();
                this.updateBasicAndReoGridView();
            }
            catch
            {
                MessageBox.Show("添加失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.updateBasicAndReoGridView();
            }
        }
    }
}

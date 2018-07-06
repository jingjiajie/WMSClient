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

namespace WMS.UI.FromDeliverOrder
{
    public partial class FormDeliverOrder : Form
    {
        private IDictionary<string, object> stockTakingOrder = null;
        private Action addFinishedCallback = null;
        public FormDeliverOrder()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
            this.model1.CellUpdated += this.model_CellUpdated;
            this.model1.RowRemoved += this.model_RowRemoved;
            this.model1.SelectionRangeChanged += this.model_SelectionRangeChanged;
        }

        private void model_SelectionRangeChanged(object sender, ModelSelectionRangeChangedEventArgs e)
        {
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row });
            if ((int)rowData[0]["state"] == 3)
            {
                this.model1.Mode = "default1";
                this.basicView1.Mode = "default1";
                this.reoGridView1.Mode = "default1";
                this.synchronizer.Mode = "default1";
            }
            else {
                this.model1.Mode = "default";
                this.basicView1.Mode = "default";
                this.reoGridView1.Mode = "default";
                this.synchronizer.Mode = "default";
            }
        }

        private void updateBasicAndReoGridView()
        {

            if (this.model1.RowCount == 0)
            {
                this.basicView1.Enabled = false;
                this.reoGridView1.Enabled = false;
            }
            else
            {
                this.basicView1.Enabled = true;
                this.reoGridView1.Enabled = true;
            }

        }

        private void model_RowRemoved(object sender, ModelRowRemovedEventArgs e)
        {
            this.updateBasicAndReoGridView();
        }

        //查看条目
        private void buttonOpen_Click(object sender, EventArgs e)
        {
            try{
                if (this.model1.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项入库单查看物料条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row })[0];
                var a1 = new FormDeliverOrderItem(rowData);
                a1.SetAddFinishedCallback(() =>
                {
                    this.searchView1.Search();
                });
                a1.Show();
            }
            catch
            {
                MessageBox.Show("无任何信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void buttonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
            }
        }

        private string StateForwardMapper(int state)
        {
            //0待入库 1送检中 2.全部入库 3.部分入库
            switch (state)
            {
                case 0: return "待装车";
                case 1: return "装车中";
                case 2: return "整单装车";
                case 3: return "发运在途";
                case 4: return "核减完成";
                default: return "未知状态";
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }
        //回单
        private void buttonReturnSupply_Click(object sender, EventArgs e)
        {

        }

        private void FormDeliveryOrder_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            this.searchView1.Search();
            this.updateBasicAndReoGridView();
        }
        private void model_CellUpdated(object sender, ModelCellUpdatedEventArgs e)
        {
            foreach (var cell in e.UpdatedCells)
            {
                if (cell.ColumnName.StartsWith("lastUpdate")) return;
                this.model1[cell.Row, "lastUpdatePersonId"] = GlobalData.Person["id"];
                this.model1[cell.Row, "lastUpdatePersonName"] = GlobalData.Person["name"];
                this.model1[cell.Row, "lastUpdateTime"] = DateTime.Now;
            }
        }
        //进入备货
        private void buttonTransferOrder_Click(object sender, EventArgs e)
        {
            new FormDeliveryOrderReady().Show();
        }

        //完成发货
        private void buttonDeliver_Click(object sender, EventArgs e)
        {
            //获取选中行ID，过滤掉新建的行（ID为0的）
            int[] selectedIDs = this.model1.GetSelectedRows<int>("id").Except(new int[] { 0 }).ToArray();
            if (selectedIDs.Length == 0)
            {
                MessageBox.Show("请选择一项进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row });
            for (int i = 0; i < this.model1.SelectionRange.Rows; i++)
            {
                //if (rowData[i]["driverName"] == null)
                //{
                //    MessageBox.Show("请输入相应司机名称以继续发运操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}
                
                //if (rowData[i]["liscensePlateNumber"] == null)
                //{
                //    MessageBox.Show("请输入相应车牌号以继续发运操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}
                if ((int)rowData[i]["state"] == 4)
                {
                    MessageBox.Show("选中出库单已经核减无法进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if ((int)rowData[i]["state"] == 3)
                {
                    MessageBox.Show("选中出库单已经发运无法进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if ((int)rowData[i]["state"] != 2)
                {
                    MessageBox.Show("选中出库单未完成装车无法进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            this.model1[this.model1.SelectionRange.Row, "state"] = 3;
            if (this.synchronizer.Save())
            {
               // MessageBox.Show("发运成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.searchView1.Search();
            }

            ////获取选中行ID，过滤掉新建的行（ID为0的）
            //int[] selectedIDs = this.model1.GetSelectedRows<int>("id").Except(new int[] { 0 }).ToArray();
            //if (selectedIDs.Length == 0)
            //{
            //    MessageBox.Show("请选择一项进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string strIDs = serializer.Serialize(selectedIDs);
            //string body = "{\"deliveryOrderIds\":\"" + strIDs + "\",\"personId\":\"" + GlobalData.Person["id"] + "\",\"driverName\":\"" + GlobalData.Person["id"] + "\",\"liscensePlateNumber\":\"" + GlobalData.Person["id"] + "\"}";
            //try
            //{
            //    string operatioName = "delivery_finish";
            //    RestClient.RequestPost<string>(Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/delivery_order/" + operatioName, strIDs, "POST");
            //    this.searchView1.Search();
            //    MessageBox.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch (WebException ex)
            //{
            //    string message = ex.Message;
            //    if (ex.Response != null)
            //    {
            //        message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            //    }
            //    MessageBox.Show(("批量完成发运") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.basicView1.Enabled = true;
            this.reoGridView1.Enabled = true;
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "createPersonId",GlobalData.Person["id"]},
                { "createPersonName",GlobalData.Person["name"]},
                { "createTime",DateTime.Now},
                { "state",0}
            });
        }

        private void model1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripAutoTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                string body = "{\"warehouseId\":\"" + GlobalData.Warehouse["id"] + "\",\"personId\":\"" + GlobalData.Person["id"] + "\"}";
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/delivery_order/transfer_auto";
                RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);
                MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.searchView1.Search();

            }
            catch(WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show("添加失败！"+ message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            if (this.model1.SelectionRange == null)
            {
                MessageBox.Show("请选择要预览的出库单！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            var previewData = RestClient.Get<List<IDictionary<string, object>>>(Defines.ServerURL + "/warehouse/WMS_Template/delivery_order/preview/" + strIDs);
            if (previewData == null) return;
            FormDeliveryOrderChooseExcelType form = new FormDeliveryOrderChooseExcelType(previewData);
            form.Show();
            //StandardFormPreviewExcel formPreviewExcel = new StandardFormPreviewExcel("出库单预览");
            //foreach (IDictionary<string, object> orderAndItem in previewData)
            //{
            //    IDictionary<string, object> deliveryOrder = (IDictionary<string, object>)orderAndItem["deliveryOrder"];
            //    object[] deliveryOrderItems = (object[])orderAndItem["deliveryOrderItems"];
            //    string no = (string)deliveryOrder["no"];
            //    if (!formPreviewExcel.AddPatternTable("Excel/patternPutOutStorageTicketNormal.xlsx", no)) return;
            //    formPreviewExcel.AddData("deliveryOrder", deliveryOrder, no);
            //    formPreviewExcel.AddData("deliveryOrderItems", deliveryOrderItems, no);
            //}
            //formPreviewExcel.Show();
        }

        private void toolStripButtonDecrease_Click(object sender, EventArgs e)
        {
            //获取选中行ID，过滤掉新建的行（ID为0的）
            int[] selectedIDs = this.model1.GetSelectedRows<int>("id").Except(new int[] { 0 }).ToArray();
            if (selectedIDs.Length == 0)
            {
                MessageBox.Show("请选择一项进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row });
            for (int i = 0; i < this.model1.SelectionRange.Rows; i++)
            {
                if ((int)rowData[i]["state"] == 4)
                {
                    MessageBox.Show("选中出库单已经核减无法进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if ((int)rowData[i]["state"] != 3)
                {
                    MessageBox.Show("选中出库单未发运无法进行核减操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (rowData[i]["returnNoteNo"] == null)
                {
                    MessageBox.Show("请输入相应回单号以继续核减操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            this.model1[this.model1.SelectionRange.Row, "state"] =4;
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string strIDs = serializer.Serialize(selectedIDs);
            //try
            //{
            //    string operatioName = "decrease_in_accounting";
            //    RestClient.RequestPost<string>(Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/delivery_order/" + operatioName, strIDs, "POST");
            //    this.searchView1.Search();
            //    MessageBox.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch (WebException ex)
            //{
            //    string message = ex.Message;
            //    if (ex.Response != null)
            //    {
            //        message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            //    }
            //    MessageBox.Show(("批量完成移库单条目") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
        }

        private void toolStripButtonDeliveyPakage_Click(object sender, EventArgs e)
        {
            var a1 = new FormSelectPakage();
            a1.SetAddFinishedCallback(() =>
            {
                this.searchView1.Search();
            });
            a1.Show();
        }

        private void ReturnNoteNoEditEnded(int row)
        {
            this.model1[row, "returnNoteTime"] = DateTime.Now;
        }
    }
}

using FrontWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace WMS.UI.FormTransferOrder
{
    public partial class FormTransferOrder : Form
    {
        public FormTransferOrder()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
            this.model1.CellUpdated += this.model_CellUpdated;
        }


        private void model_Refreshed(object sender, ModelRefreshedEventArgs e)
        {
            this.updateBasicAndReoGridView();
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

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.model1.SelectionRange == null || this.model1.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项移库单查看物料条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row })[0];
                var a1 = new FormTransferOrderItem(rowData);
                if (rowData["id"] == null)
                {
                    MessageBox.Show("请先保存单据再查看条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                a1.SetAddFinishedCallback(() =>
                {
                    this.searchView1.Search();
                });
                a1.Show();

            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show("查看失败！" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
            }
        }

        private string StateForwardMapper([Data]int state)
        {
            switch (state)
            {
                case 0: return "待移库";
                case 1: return "部分移库";
                case 2: return "移库完成";
                default: return "未知状态";
            }
        }

        private void FormTransferOrder_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            this.searchView1.AddStaticCondition("type",1);
            this.searchView1.Search();
            this.updateBasicAndReoGridView();
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            if (this.model1.SelectionRange == null)
            {
                MessageBox.Show("请选择要预览的备货单！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            var previewData = RestClient.Get<List<IDictionary<string, object>>>(Defines.ServerURL + "/warehouse/WMS_Template/transfer_order/preview/" + strIDs);
            if (previewData == null) return;
            StandardFormPreviewExcel formPreviewExcel = new StandardFormPreviewExcel("移库单预览");
            foreach (IDictionary<string, object> entryAndItem in previewData)
            {
                IDictionary<string, object> transferOrder = (IDictionary<string, object>)entryAndItem["transferOrder"];
                object[] transferOrderItems = (object[])entryAndItem["transferOrderItems"];
                string no = (string)transferOrder["no"];
                if (!formPreviewExcel.AddPatternTable("Excel/TransferOrderNote.xlsx", no)) return;
                formPreviewExcel.AddData("transferOrder", transferOrder, no);
                formPreviewExcel.AddData("transferOrderItems", transferOrderItems, no);
            }
            formPreviewExcel.Show();
        }

        private int warehouseIdDefaultValue()
        {
            return (int)GlobalData.Warehouse["id"];
        }

        private int createPersonIdDefaultValue()
        {
            return (int)GlobalData.Person["id"];
        }

        private string createPersonNameDefaultValue()
        {
            return (string)GlobalData.Person["name"];
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.basicView1.Enabled = true;
            this.reoGridView1.Enabled = true;
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "createPersonName",GlobalData.Person["name"]}
            });
        }

        private void toolStripAutoTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                string body = "{\"warehouseId\":\"" + GlobalData.Warehouse["id"] + "\",\"personId\":\"" + GlobalData.Person["id"] + "\",\"transferType\":\"" + 1 + "\"}";
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/delivery_order/transfer_auto";
                var remindData=RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);
                if (remindData.Count==0)
                {
                    MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.searchView1.Search();
                }
                else
                {
                    this.searchView1.Search();
                    StringBuilder remindBody = new StringBuilder();
                    foreach (IDictionary<string, object> transferOrderItem in remindData)
                    {
                        if ((int)transferOrderItem["state"] == 0)
                        {
                            remindBody = remindBody
                                    .Append("供货商名称：“").Append(transferOrderItem["supplierName"])
                                    .Append("”，代号：“").Append(transferOrderItem["supplierNo"])
                                    .Append("”，物料“").Append(transferOrderItem["materialName"]).Append("”，代号：“").Append(transferOrderItem["materialNo"])
                                    .Append("”，系列：“").Append(transferOrderItem["materialProductLine"])
                                    .Append("”（单位：“").Append(transferOrderItem["sourceUnit"]).Append("”，单位数量：“").Append(transferOrderItem["sourceUnitAmount"])
                                    .Append("”检测状态：“合格”），在源库位：“").Append(transferOrderItem["sourceStorageLocationName"])
                                    .Append("”上不存在库存信息！请核准库存！\r\n");
                        }
                        if ((int)transferOrderItem["state"] == 1)
                        {
                            remindBody = remindBody
                                    .Append("供货商名称：“").Append(transferOrderItem["supplierName"])
                                    .Append("”，代号：“").Append(transferOrderItem["supplierNo"])
                                    .Append("”，物料“").Append(transferOrderItem["materialName"]).Append("”，代号：“").Append(transferOrderItem["materialNo"])
                                    .Append("”，系列：“").Append(transferOrderItem["materialProductLine"])
                                     .Append("”（单位：“").Append(transferOrderItem["unit"]).Append("”，单位数量：“").Append(transferOrderItem["unitAmount"])
                                    .Append("”检测状态：“合格”），在目标库位：“").Append(transferOrderItem["targetStorageLocationName"])
                                    .Append("”上库存充足！无需备货操作！\r\n\r\n");
                        }
                        if ((int)transferOrderItem["state"] == 2)
                        {
                            remindBody = remindBody
                                    .Append("供货商名称：“").Append(transferOrderItem["supplierName"])
                                    .Append("”，代号：“").Append(transferOrderItem["supplierNo"])
                                    .Append("”，物料“").Append(transferOrderItem["materialName"]).Append("”，代号：“").Append(transferOrderItem["materialNo"])
                                    .Append("”，系列：“").Append(transferOrderItem["materialProductLine"])
                                    .Append("”（单位：“").Append(transferOrderItem["sourceUnit"]).Append("”，单位数量：“").Append(transferOrderItem["unitAmount"])
                                    .Append("”检测状态：“合格”），在源库位：“").Append(transferOrderItem["sourceStorageLocationName"])
                                    .Append("”上库存可用数量不足！需要库存数量：“").Append(transferOrderItem["scheduledAmount"]).Append("”，现有库存：“")
                                    .Append(transferOrderItem["realAmount"]).Append("”\r\n\r\n");
                        }
                    }
                    new FormRemind(remindBody.ToString()).Show();
                    
                }
                

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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }

        private void SupplierNoEditEnded([Row]int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "supplierNo"]?.ToString())) return;
            this.model1[row, "supplierName"] = "";
            this.FindSupplierID(row);
        }

        private void SupplierNameEditEnded([Row]int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "supplierName"]?.ToString())) return;
            this.model1[row, "supplierNo"] = "";
            this.FindSupplierID(row);
        }
        private void FindSupplierID(int row)
        {
            this.model1[row, "supplierId"] = 0;//先清除供货商ID
            string supplierNo = this.model1[row, "supplierNo"]?.ToString() ?? "";
            string supplierName = this.model1[row, "supplierName"]?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(supplierNo) && string.IsNullOrWhiteSpace(supplierName)) return;

            var foundSuppliers = (from s in GlobalData.AllSuppliers
                                  where (string.IsNullOrWhiteSpace(supplierNo) ? true : (s["no"]?.ToString() ?? "") == supplierNo)
                                  && (string.IsNullOrWhiteSpace(supplierName) ? true : (s["name"]?.ToString() ?? "") == supplierName)
                                  select s).ToArray();
            if (foundSuppliers.Length != 1) goto FAILED;
            int supplierID = (int)foundSuppliers[0]["id"];
            this.model1[row, "supplierId"] = foundSuppliers[0]["id"];
            this.model1[row, "supplierNo"] = foundSuppliers[0]["no"];
            this.model1[row, "supplierName"] = foundSuppliers[0]["name"];
            return;

            FAILED:
            MessageBox.Show("供应商不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void RefreshMode()
        {
            int?[] selectedIDs = this.model1.GetSelectedRows<int?>("id");
            if (selectedIDs.Length == 0 || selectedIDs[0].HasValue == false)
            {
                this.basicView1.Mode = "default";
                this.reoGridView1.Mode = "default";
            }
            else
            {
                this.basicView1.Mode = "supplier-not-editable";
                this.reoGridView1.Mode = "supplier-not-editable";
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
    }
}

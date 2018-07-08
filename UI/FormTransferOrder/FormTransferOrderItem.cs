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
using System.Windows.Forms;
using System.Web.Script.Serialization;

namespace WMS.UI.FormTransferOrder
{
    public partial class FormTransferOrderItem : Form
    {
        private IDictionary<string, object> transferOrder = null;
        private Action addFinishedCallback = null;
        public FormTransferOrderItem(IDictionary<string, object> transferOrder)
        {
            MethodListenerContainer.Register(this);
            this.transferOrder = transferOrder;
            InitializeComponent();
            this.searchView1.AddStaticCondition("transferOrderId", this.transferOrder["id"]);
            //this.model1.RowRemoved += this.model_RowRemoved;
            //this.model1.Refreshed += this.model_Refreshed;
        }

        //private void model_Refreshed(object sender, ModelRefreshedEventArgs e)
        //{
        //    this.updateBasicAndReoGridView();
        //}

        //private void updateBasicAndReoGridView()
        //{

        //    if (this.model1.RowCount == 0)
        //    {
        //        this.basicView1.Enabled = false;
        //        this.reoGridView1.Enabled = false;
        //    }
        //    else
        //    {
        //        this.basicView1.Enabled = true;
        //        this.reoGridView1.Enabled = true;
        //    }

        //}

        //private void model_RowRemoved(object sender, ModelRowRemovedEventArgs e)
        //{
        //    this.updateBasicAndReoGridView();
        //}

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
        private int transferOrderIdDefaultValue()
        {
            return (int)this.transferOrder["id"];
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            //this.basicView1.Enabled = true;
            //this.reoGridView1.Enabled = true;
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "personId",GlobalData.Person["id"]},
                { "personName",GlobalData.Person["name"]},
                { "transferOrderNo", this.transferOrder["no"]},
                { "operateTime", DateTime.Now}
                //{ "supplierId",this.transferOrder["supplierId"]},
                //{ "supplierNo",this.transferOrder["supplierNo"]},
                //{ "supplierName",this.transferOrder["supplierName"]}
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

        private string AmountForwardMapper(double amount, int row)
        {
            double? unitAmount = (double?)this.model1[row, "unitAmount"];
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                return amount.ToString();
            }
            else
            {
                return Utilities.DoubleToString(amount / unitAmount.Value);
            }
        }

        private double AmountBackwardMapper(string strAmount, int row)
        {
            if (!Double.TryParse(strAmount, out double amount))
            {
                MessageBox.Show($"\"{strAmount}\"不是合法的数字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
            double? unitAmount = (double?)this.model1[row, "unitAmount"];
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                return amount;
            }
            else
            {
                return amount * unitAmount.Value;
            }
        }

        private void UnitAmountEditEnded(int row)
        {
            this.model1.RefreshView(row);
        }

        //待工整单完成
        private void buttonFinishAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认保存当前修改并整单完成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.synchronizer.Save();
            try
            {
                string body = "{\"personId\":\"" + GlobalData.Person["id"] + "\",\"transferOrderId\":\"" +this.transferOrder["id"] + "\"}";
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/transfer_order/transfer_finish";
                RestClient.RequestPost<List<IDictionary<string, object>>>(url, body, "PUT");
                MessageBox.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.searchView1.Search();

            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show(("整单完成移库单条目") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //部分完成
        private void buttonFinish_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认保存当前修改并完成选中条目吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.synchronizer.Save();
            this.TransferDone();
        }

        private string StateForwardMapper(int state)
        {
            
            switch (state)
            {
                case 0: return "待备货";
                case 1: return "部分备货";
                case 2: return "备货完成";
                default: return "未知状态";
            }
        }

        private void TransferDone()
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
            try
            {
                string operatioName = "transfer_some/" + (int)GlobalData.Person["id"];
                RestClient.RequestPost<string>(Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/transfer_order/" + operatioName, strIDs, "PUT");
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
                MessageBox.Show(( "批量完成移库单条目") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FormTransferOrderItem_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            Utilities.BindBlueButton(this.buttonFinish);
            Utilities.BindBlueButton(this.buttonFinishAll);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
            //this.updateBasicAndReoGridView();
        }

        private void SourceStorageLocationNoEditEnded(int row, string sourceStorageLocationNo)
        {
            this.model1[row, "sourceStorageLocationId"] = 0;//先清除库位ID
            if (string.IsNullOrWhiteSpace(sourceStorageLocationNo)) return;
            var foundStorageLocations = (from s in GlobalData.AllStorageLocations
                                         where s["no"]?.ToString() == sourceStorageLocationNo
                                         select s).ToArray();
            if (foundStorageLocations.Length != 1) goto FAILED;
            this.model1[row, "sourceStorageLocationId"] = (int)foundStorageLocations[0]["id"];
            this.model1[row, "sourceStorageLocationName"] = foundStorageLocations[0]["name"];
            return;

            FAILED:
            MessageBox.Show($"库位\"{sourceStorageLocationNo}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void SourceStorageLocationNameEditEnded(int row, string sourceStorageLocationName)
        {
            this.model1[row, "sourceStorageLocationId"] = 0;//先清除库位ID
            if (string.IsNullOrWhiteSpace(sourceStorageLocationName)) return;
            var foundStorageLocations = (from s in GlobalData.AllStorageLocations
                                         where s["name"]?.ToString() == sourceStorageLocationName
                                         select s).ToArray();
            if (foundStorageLocations.Length != 1) goto FAILED;
            this.model1[row, "sourceStorageLocationId"] = (int)foundStorageLocations[0]["id"];
            this.model1[row, "sourceStorageLocationNo"] = foundStorageLocations[0]["no"];
            return;

            FAILED:
            MessageBox.Show($"库位\"{sourceStorageLocationName}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void TargetStorageLocationNoEditEnded(int row, string targetStorageLocationNo)
        {
            this.model1[row, "targetStorageLocationId"] = 0;//先清除库位ID
            if (string.IsNullOrWhiteSpace(targetStorageLocationNo)) return;
            var foundStorageLocations = (from s in GlobalData.AllStorageLocations
                                         where s["no"]?.ToString() == targetStorageLocationNo
                                         select s).ToArray();
            if (foundStorageLocations.Length != 1) goto FAILED;
            this.model1[row, "targetStorageLocationId"] = (int)foundStorageLocations[0]["id"];
            this.model1[row, "targetStorageLocationName"] = foundStorageLocations[0]["name"];
            return;

            FAILED:
            MessageBox.Show($"库位\"{targetStorageLocationNo}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void TargetStorageLocationNameEditEnded(int row, string targetStorageLocationName)
        {
            this.model1[row, "targetStorageLocationId"] = 0;//先清除库位ID
            if (string.IsNullOrWhiteSpace(targetStorageLocationName)) return;
            var foundStorageLocations = (from s in GlobalData.AllStorageLocations
                                         where s["name"]?.ToString() == targetStorageLocationName
                                         select s).ToArray();
            if (foundStorageLocations.Length != 1) goto FAILED;
            this.model1[row, "targetStorageLocationId"] = (int)foundStorageLocations[0]["id"];
            this.model1[row, "targetStorageLocationNo"] = foundStorageLocations[0]["no"];
            return;

            FAILED:
            MessageBox.Show($"库位\"{targetStorageLocationName}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        //===========为了实现一个看起来天经地义的交互逻辑=========

        //private void SupplierNoEditEnded(int row)
        //{
        //    if (string.IsNullOrWhiteSpace(this.model1[row, "supplierNo"]?.ToString())) return;
        //    this.model1[row, "supplierName"] = "";
        //    this.FindSupplierID(row);
        //    this.TryGetSupplyID(row);
        //}

        //private void SupplierNameEditEnded(int row)
        //{
        //    if (string.IsNullOrWhiteSpace(this.model1[row, "supplierName"]?.ToString())) return;
        //    this.model1[row, "supplierNo"] = "";
        //    this.FindSupplierID(row);
        //    this.TryGetSupplyID(row);
        //}

        private void MaterialNoEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "materialNo"]?.ToString())) return;
            this.FindMaterialID(row);
            this.TryGetSupplyID(row);
        }

        private void MaterialNameEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "materialName"]?.ToString())) return;
            this.FindMaterialID(row);
            this.TryGetSupplyID(row);
        }

        private void MaterialProductLineEditEnded(int row)
        {
            this.FindMaterialID(row);
            this.TryGetSupplyID(row);
        }

        private void FindMaterialID(int row)
        {
            this.model1[row, "materialId"] = 0; //先清除物料ID
            string materialNo = this.model1[row, "materialNo"]?.ToString() ?? "";
            string materialName = this.model1[row, "materialName"]?.ToString() ?? "";
            string materialProductLine = this.model1[row, "materialProductLine"]?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(materialNo) && string.IsNullOrWhiteSpace(materialName)) return;
            var foundMaterials = (from m in GlobalData.AllMaterials
                                  where (string.IsNullOrWhiteSpace(materialNo) ? true : (m["no"]?.ToString() ?? "") == materialNo)
                                  && (string.IsNullOrWhiteSpace(materialName) ? true : (m["name"]?.ToString() ?? "") == materialName)
                                     && materialProductLine == (m["productLine"]?.ToString() ?? "")
                                  select m).ToArray();
            if (foundMaterials.Length != 1)
            {
                goto FAILED;
            }
            this.model1[row, "materialId"] = foundMaterials[0]["id"];
            this.model1[row, "materialNo"] = foundMaterials[0]["no"];
            this.model1[row, "materialName"] = foundMaterials[0]["name"];
            this.model1[row, "materialProductLine"] = foundMaterials[0]["productLine"];
            return;

            FAILED:
            if (string.IsNullOrWhiteSpace(materialProductLine)) return;
            MessageBox.Show("物料不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
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

        private void TryGetSupplyID(int row)
        {
            this.model1[row, "supplyId"] = 0; //先清除供货ID
            int supplierId = (int?)this.model1[row, "supplierId"] ?? 0;
            int materialId = (int?)this.model1[row, "materialId"] ?? 0;
            if (supplierId == 0 || materialId == 0) return;
            var foundSupplies = (from s in GlobalData.AllSupplies
                                 where (int)s["supplierId"] == supplierId
                                 && (int)s["materialId"] == materialId
                                 select s).ToArray();
            //如果找到供货信息，则把供货设置的默认入库信息拷贝到相应字段上
            if (foundSupplies.Length == 1)
            {
                this.model1[row, "supplyId"] = foundSupplies[0]["id"];
                if (foundSupplies[0]["defaultDeliveryAmount"] != null)
                {
                    this.FillDefaultValue(row, "scheduledAmount", foundSupplies[0]["defaultDeliveryAmount"]);
                }
                //默认填写出库单实际数量一步到位XD
                //this.FillDefaultValue(row, "realAmount", foundSupplies[0]["defaultDeliveryAmount"]);

                this.FillDefaultValue(row, "unit", foundSupplies[0]["defaultDeliveryUnit"]);
                this.FillDefaultValue(row, "unitAmount", foundSupplies[0]["defaultDeliveryUnitAmount"]);               
                //在备货完成的库位里发货
                this.FillDefaultValue(row, "sourceStorageLocationId", foundSupplies[0]["defaultDeliveryStorageLocationId"]);
                this.FillDefaultValue(row, "sourceStorageLocationNo", foundSupplies[0]["defaultDeliveryStorageLocationNo"]);
                this.FillDefaultValue(row, "sourceStorageLocationName", foundSupplies[0]["defaultDeliveryStorageLocationName"]);
                //在备货完成的库位里发货
                this.FillDefaultValue(row, "targetStorageLocationId", foundSupplies[0]["defaultPrepareTargetStorageLocationId"]);
                this.FillDefaultValue(row, "targetStorageLocationNo", foundSupplies[0]["defaultPrepareTargetStorageLocationNo"]);
                this.FillDefaultValue(row, "targetStorageLocationName", foundSupplies[0]["defaultPrepareTargetStorageLocationName"]);

            }
        }

        //物料名称输入联想
        private object[] MaterialNameAssociation(string str)
        {

            string materialNo = this.model1[this.model1.SelectionRange.Row, "materialNo"]?.ToString() ?? "";
            int[] selectedIDs = this.model1.GetSelectedRows<int>("supplierId").Except(new int[] { 0 }).ToArray();
            if (selectedIDs.Length == 0)
            {
                var a = (from s in GlobalData.AllSupplies
                         where s["materialName"] != null &&
                         s["materialName"].ToString().StartsWith(str)
                         && s["warehouseId"] != GlobalData.Warehouse["id"]
                         && (string.IsNullOrWhiteSpace(materialNo) ? true : (s["materialNo"]?.ToString() ?? "") == materialNo)
                         select s["materialName"]).ToArray();
                return a.GroupBy(p => p).Select(p => p.Key).ToArray();
            }
            else
            {
                var a = (from s in GlobalData.AllSupplies
                         where s["materialName"] != null &&
                         s["materialName"].ToString().StartsWith(str) &&
                         (int)s["supplierId"] == selectedIDs[0]
                         && s["warehouseId"] != GlobalData.Warehouse["id"]
                         && (string.IsNullOrWhiteSpace(materialNo) ? true : (s["materialNo"]?.ToString() ?? "") == materialNo)
                         select s["materialName"]).ToArray();
                return a.GroupBy(p => p).Select(p => p.Key).ToArray();
            }
        }

        //物料代号输入联想
        private object[] MaterialNoAssociation(string str)
        {
            string materialName = this.model1[this.model1.SelectionRange.Row, "materialName"]?.ToString() ?? "";

            int[] selectedIDs = this.model1.GetSelectedRows<int>("supplierId").Except(new int[] { 0 }).ToArray();
            if (selectedIDs.Length == 0)
            {
                var a = (from s in GlobalData.AllSupplies
                         where s["materialNo"] != null &&
                         s["materialNo"].ToString().StartsWith(str)
                         && s["warehouseId"] != GlobalData.Warehouse["id"]
                         && (string.IsNullOrWhiteSpace(materialName) ? true : (s["materialName"]?.ToString() ?? "") == materialName)
                         select s["materialNo"]).ToArray();
                return a.GroupBy(p => p).Select(p => p.Key).ToArray();
            }
            else
            {
                var a = (from s in GlobalData.AllSupplies
                         where s["materialNo"] != null &&
                         s["materialNo"].ToString().StartsWith(str) &&
                         (int)s["supplierId"] == selectedIDs[0]
                         && s["warehouseId"] != GlobalData.Warehouse["id"]
                         && (string.IsNullOrWhiteSpace(materialName) ? true : (s["materialName"]?.ToString() ?? "") == materialName)

                         select s["materialNo"]).ToArray();
                return a.GroupBy(p => p).Select(p => p.Key).ToArray();
            }
        }

        //物料系列输入联想
        private object[] MaterialProductLineAssociation(string str)
        {
            int[] selectedIDs = this.model1.GetSelectedRows<int>("supplierId").Except(new int[] { 0 }).ToArray();
            if (selectedIDs.Length == 0)
            {
                var a = (from s in GlobalData.AllSupplies
                         where s["materialProductLine"] != null &&
                         s["materialProductLine"].ToString().StartsWith(str)
                         && s["warehouseId"] != GlobalData.Warehouse["id"]
                         select s["materialProductLine"]).ToArray();
                return a.GroupBy(p => p).Select(p => p.Key).ToArray();
            }
            else
            {
                var a = (from s in GlobalData.AllSupplies
                         where s["materialProductLine"] != null &&
                         s["materialProductLine"].ToString().StartsWith(str) &&
                         (int)s["supplierId"] == selectedIDs[0]
                         && s["warehouseId"] != GlobalData.Warehouse["id"]
                         select s["materialProductLine"]).ToArray();
                return a.GroupBy(p => p).Select(p => p.Key).ToArray();
            }

        }
        private void FillDefaultValue(int row, string fieldName, object value)
        {
            this.model1[row, fieldName] = value;
        }

        private void toolStripButtonAdd_Click_1(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "personId",GlobalData.Person["id"]},
                { "personName",GlobalData.Person["name"]},
                { "transferOrderNo", this.transferOrder["no"]},
                { "operateTime", DateTime.Now},
            });
        }

        public void SetAddFinishedCallback(Action callback)
        {
            this.addFinishedCallback = callback;
        }

        private void FormTransferOrderItemClosed(object sender, FormClosedEventArgs e)
        {
            if (this.addFinishedCallback != null)
            {
                this.addFinishedCallback();
            }
        }

        private void toolStripTop_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        //=============天经地义的交互逻辑到这里结束===============
    }
}

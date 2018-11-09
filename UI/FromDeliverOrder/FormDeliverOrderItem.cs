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

namespace WMS.UI.FromDeliverOrder
{
    public partial class FormDeliverOrderItem : Form
    {
        private IDictionary<string, object> deliveryOrder = null;
        private Action addFinishedCallback = null;
        public FormDeliverOrderItem(IDictionary<string, object> deliveryOrder)
        {
            MethodListenerContainer.Register(this);
            this.deliveryOrder = deliveryOrder;
            InitializeComponent();
            this.searchView1.AddStaticCondition("deliveryOrderId", this.deliveryOrder["id"]);
            int OrderState=(int)this.deliveryOrder["state"];
            if (OrderState == 3 || OrderState == 4) {
                this.toolStripButtonAdd.Enabled = false;
                this.toolStripButtonAlter.Enabled = false;
                this.toolStripButtonDelete.Enabled = false;
                this.toolStripButtonDeliveyPakage.Enabled = false;
            }
            this.model1.RowRemoved += this.model_RowRemoved;
            this.model1.Refreshed += this.model_Refreshed;
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
                this.reoGridView2.Enabled = false;
            }
            else
            {
                this.basicView1.Enabled = true;
                this.reoGridView2.Enabled = true;
            }

        }

        private void model_RowRemoved(object sender, ModelRowRemovedEventArgs e)
        {
            this.updateBasicAndReoGridView();
        }

        private int deliveryOrderIdDefaultValue()
        {
            return (int)this.deliveryOrder["id"];
        }
        
        private int deliveryOrderTypeDefaultValue()
        {
            return (int)this.deliveryOrder["type"];
        }

        private string deliveryOrderNoDefaultValue()
        {
            return (string)this.deliveryOrder["no"];
        }

        private int PersonIdDefaultValue()
        {
            return (int)GlobalData.Person["id"];
        }

        private string PersonNameDefaultValue()
        {
            return (string)GlobalData.Person["name"];
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.basicView1.Enabled = true;
            this.reoGridView2.Enabled = true;
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {

            });
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
            this.updateBasicAndReoGridView();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
            }
        }

        private string AmountForwardMapper([Data]double amount, [Row] int row)
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

        private double AmountBackwardMapper([Data]string strAmount, [Row]int row)
        {
            if (!Double.TryParse(strAmount, out double amount))
            {
                MessageBox.Show($"\"{strAmount}\"不是合法的数字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
            if (row == -1)
            {
                return amount;
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

        private void UnitAmountEditEnded([Row]int row)
        {
            this.model1.RefreshView(row);
        }

        private string TypeForwardMapper([Data]int type)
        {
            switch (type)
            {
                case 0: return "合格品出库";
                case 1: return "不良品出库";
                default: return "未知状态";
            }
        }

        private int TypeBackwardMapper([Data]string type)
        {
            switch (type)
            {
                case "合格品出库": return 0;
                case "不良品出库": return 1;
                default: return -1;
            }
        }

        private void FormDeliverOrderItem_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            Utilities.BindBlueButton(this.buttonAllLoad);
            Utilities.BindBlueButton(this.buttonLoad);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
            this.updateBasicAndReoGridView();
        }

        private void buttonAllLoad_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认保存当前修改并整单完成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.synchronizer.Save();

            int[] selectedIDs = { (int)this.deliveryOrder["id"] } ;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string strIDs = serializer.Serialize(selectedIDs);
            try
            {
                string operatioName = "loading_all";
                RestClient.RequestPost<string>(Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/delivery_order_item/" + operatioName, strIDs, "POST");
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
                MessageBox.Show(("所有条目完成") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private string StateForwardMapper([Data]int state)
        {
            //0待入库 1送检中 2.全部入库 3.部分入库
            switch (state)
            {
                case 0: return "待装车";
                case 1: return "装车中";
                case 2: return "装车完成";
                default: return "未知状态";
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认保存当前修改并完成选中条目装车吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
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
                string operatioName = "loading_some";
                RestClient.RequestPost<string>(Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/delivery_order_item/" + operatioName, strIDs, "POST");
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
                MessageBox.Show(("选中条目装车") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void SetAddFinishedCallback(Action callback)
        {
            this.addFinishedCallback = callback;
        }

        private void FormDeliverOrderItemClosed(object sender, FormClosedEventArgs e)
        {
            if (this.addFinishedCallback != null)
            {
                this.addFinishedCallback();
            }
        }

        private void toolStripButtonDeliveyPakage_Click(object sender, EventArgs e)
        {
            var a1 = new FormSelectPakage((int)this.deliveryOrder["id"]);
            a1.SetAddFinishedCallback(() =>
            {
                this.searchView1.Search();
            });
            a1.Show();
        }

        [MethodListener]
        public class FormDeliverOrderItemMethodListener
        {
            private void PersonEditEnded([Model] IModel model, [Row] int row, [Data] string personName)
            {
                model[row, "personId"] = 0;//先清除ID
                if (string.IsNullOrWhiteSpace(personName)) return;
                var foundPersons = (from s in GlobalData.AllPersons
                                    where s["name"]?.ToString() == personName
                                    select s).ToArray();
                if (foundPersons.Length != 1) goto FAILED;
                model[row, "personId"] = (int)foundPersons[0]["id"];
                model[row, "personName"] = foundPersons[0]["name"];
                return;

                FAILED:
                MessageBox.Show($"人员\"{personName}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            public string[] SupplySerialNoAssociation([Model] IModel model, [Row] int row, [Data] string input)
            {
                return (from s in GlobalData.AllSupplies
                        where s["serialNo"] != null
                        && s["serialNo"].ToString().StartsWith(input)
                        && (int)s["supplierId"] == (int)model[row, "supplierId"]
                        && s["warehouseId"].Equals(GlobalData.Warehouse["id"])
                        select s["serialNo"]?.ToString()).Distinct().ToArray();
            }

            public void SupplySerialNoEditEnded([Model] IModel model, [Row] int row)
            {
                string supplySerialNo = model[row, "supplySerialNo"]?.ToString() ?? "";
                if (string.IsNullOrWhiteSpace(supplySerialNo)) return;
                var foundSupplies = (from m in GlobalData.AllSupplies
                                     where supplySerialNo == (string)m["serialNo"]
                                     select m).ToArray();
                if (foundSupplies.Length != 1)
                {
                    model.UpdateCellState(row, "supplySerialNo", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "供货不存在！")));
                    return;
                }
                this.FillSupplyFields(model, row, foundSupplies[0]);
            }

            private void FillSupplyFields(IModel model, int row, IDictionary<string, object> supply)
            {
                model[row, "supplyId"] = supply["id"];
                model[row, "supplySerialNo"] = supply["serialNo"];
                model[row, "materialId"] = supply["materialId"];
                model[row, "materialNo"] = supply["materialNo"];
                model[row, "materialName"] = supply["materialName"];
                model[row, "materialProductLine"] = supply["materialProductLine"];
                model[row, "supplierId"] = supply["supplierId"];
                model[row, "supplierNo"] = supply["supplierNo"];
                model[row, "supplierName"] = supply["supplierName"];
                model[row, "scheduledAmount"] = supply["defaultDeliveryAmount"];
                model[row, "unit"] = supply["defaultDeliveryUnit"];
                model[row, "unitAmount"] = supply["defaultDeliveryUnitAmount"];

                string sourceStorageLocationNo = supply["defaultPrepareTargetStorageLocationNo"] as string;
                
                model[row, "sourceStorageLocationName"] = null;

                model[row, "sourceStorageLocationNo"] = sourceStorageLocationNo;

                this.FindStorageLocation(model, row, "sourceStorageLocation", FindStorageLocationBy.NO, sourceStorageLocationNo, false); model.UpdateCellState(row, "supplySerialNo", new ModelCellState(ValidationState.OK));
                model.RefreshView(row);
                return;
            }

            private void SourceStorageLocationNoEditEnded([Model] IModel model, [Row] int row, [Data] string storageLocationNo)
            {
                this.FindStorageLocation(model, row, "sourceStorageLocation", FindStorageLocationBy.NO, storageLocationNo);
            }

            private void SourceStorageLocationNameEditEnded([Model] IModel model, [Row] int row, [Data] string storageLocationName)
            {
                this.FindStorageLocation(model, row, "sourceStorageLocation", FindStorageLocationBy.NAME, storageLocationName);
            }

            enum FindStorageLocationBy
            {
                NAME, NO
            }

            private void FindStorageLocation(IModel model, int row, string storageLocationFieldName, FindStorageLocationBy byField, string value, bool warning = true)
            {
                model[row, storageLocationFieldName + "Id"] = 0;//先清除库位ID

                var foundStorageLocations = (from s in GlobalData.AllStorageLocations
                                             where s[byField == FindStorageLocationBy.NAME ? "name" : "no"]?.ToString() == value
                                             select s).ToArray();
                if (foundStorageLocations.Length != 1) goto FAILED;
                model[row, storageLocationFieldName + "Id"] = (int)foundStorageLocations[0]["id"];
                if (byField == FindStorageLocationBy.NAME)
                {
                    model[row, storageLocationFieldName + "No"] = foundStorageLocations[0]["no"];
                }
                else
                {
                    model[row, storageLocationFieldName + "Name"] = foundStorageLocations[0]["name"];
                }
                model.UpdateCellState(row, storageLocationFieldName + "Name", new ModelCellState(new ValidationState(ValidationStateType.OK)));
                model.UpdateCellState(row, storageLocationFieldName + "No", new ModelCellState(new ValidationState(ValidationStateType.OK)));
                return;

                FAILED:
                model.UpdateCellState(row, storageLocationFieldName + "Name", new ModelCellState(new ValidationState(ValidationStateType.WARNING, $"库位\"{value}\"不存在！")));
                model.UpdateCellState(row, storageLocationFieldName + "No", new ModelCellState(new ValidationState(ValidationStateType.WARNING, $"库位\"{value}\"不存在！")));
                return;
            }

            private void FindSupplyByMaterialAndSupplier(IModel model, int row)
            {
                model[row, "supplyId"] = 0; //先清除供货ID
                int supplierId = (int?)model[row, "supplierId"] ?? 0;
                int materialId = (int?)model[row, "materialId"] ?? 0;
                if (supplierId == 0 || materialId == 0) return;
                var foundSupplies = (from s in GlobalData.AllSupplies
                                     where (int)s["supplierId"] == supplierId
                                     && (int)s["materialId"] == materialId
                                     select s).ToArray();
                //如果找到供货信息，则把供货设置的默认入库信息拷贝到相应字段上
                if (foundSupplies.Length == 1)
                {
                    this.FillSupplyFields(model, row, foundSupplies[0]);
                    model.RefreshView(row);
                }
            }

            //物料名称输入联想
            private object[] MaterialNameAssociation([Model] IModel model, [Data] string str)
            {

                string materialNo = model[model.SelectionRange.Row, "materialNo"]?.ToString() ?? "";
                int[] selectedIDs = model.GetSelectedRows<int>("supplierId").Except(new int[] { 0 }).ToArray();
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
            private object[] MaterialNoAssociation([Model] IModel model, [Data] string str)
            {
                string materialName = model[model.SelectionRange.Row, "materialName"]?.ToString() ?? "";

                int[] selectedIDs = model.GetSelectedRows<int>("supplierId").Except(new int[] { 0 }).ToArray();
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
            private object[] MaterialProductLineAssociation([Model] IModel model, [Data] string str)
            {
                int[] selectedIDs = model.GetSelectedRows<int>("supplierId").Except(new int[] { 0 }).ToArray();
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

            private void SupplierNoEditEnded([Model] IModel model, [Row] int row)
            {
                if (string.IsNullOrWhiteSpace(model[row, "supplierNo"]?.ToString())) return;
                model[row, "supplierName"] = "";
                this.FindSupplierID(model, row);
                this.FindSupplyByMaterialAndSupplier(model, row);
            }

            private void SupplierNameEditEnded([Model] IModel model, [Row] int row)
            {
                if (string.IsNullOrWhiteSpace(model[row, "supplierName"]?.ToString())) return;
                model[row, "supplierNo"] = "";
                this.FindSupplierID(model, row);
                this.FindSupplyByMaterialAndSupplier(model, row);
            }

            private void TryFindSupplyByMaterialOnly(IModel model, int row)
            {
                model[row, "supplyId"] = 0; //先清除供货ID
                int materialId = (int?)model[row, "materialId"] ?? 0;
                if (materialId == 0) return;
                var foundSupplies = (from s in GlobalData.AllSupplies
                                     where (int)s["materialId"] == materialId
                                     select s).ToArray();
                //如果找到供货信息，则把供货设置的默认入库信息拷贝到相应字段上
                if (foundSupplies.Length == 1)
                {
                    this.FillSupplyFields(model, row, foundSupplies[0]);
                    model.RefreshView(row);
                }
            }

            private void MaterialNoEditEnded([Model] IModel model, [Row] int row)
            {
                if (string.IsNullOrWhiteSpace(model[row, "materialNo"]?.ToString())) return;
                //this.model[row, "materialName"] = "";
                this.FindMaterialID(model, row);
                this.FindSupplyByMaterialAndSupplier(model, row);
                if (((int?)model[row, "supplyId"] ?? 0) == 0 && ((int?)model[row, "supplierId"] ?? 0) == 0)
                {
                    this.TryFindSupplyByMaterialOnly(model, row);
                }
            }

            private void MaterialNameEditEnded([Model] IModel model, [Row] int row)
            {
                if (string.IsNullOrWhiteSpace(model[row, "materialName"]?.ToString())) return;
                // this.model[row, "materialNo"] = "";
                this.FindMaterialID(model, row);
                this.FindSupplyByMaterialAndSupplier(model, row);
                if (((int?)model[row, "supplyId"] ?? 0) == 0 && ((int?)model[row, "supplierId"] ?? 0) == 0)
                {
                    this.TryFindSupplyByMaterialOnly(model, row);
                }
            }

            private void MaterialProductLineEditEnded([Model] IModel model, [Row] int row)
            {
                this.FindMaterialID(model, row);
                this.FindSupplyByMaterialAndSupplier(model, row);
                if (((int?)model[row, "supplyId"] ?? 0) == 0 && ((int?)model[row, "supplierId"] ?? 0) == 0)
                {
                    this.TryFindSupplyByMaterialOnly(model, row);
                }
            }

            private void FindMaterialID(IModel model, int row)
            {
                model[row, "materialId"] = 0; //先清除物料ID
                string materialNo = model[row, "materialNo"]?.ToString() ?? "";
                string materialName = model[row, "materialName"]?.ToString() ?? "";
                string materialProductLine = model[row, "materialProductLine"]?.ToString() ?? "";
                if (string.IsNullOrWhiteSpace(materialNo) && string.IsNullOrWhiteSpace(materialName)) return;
                var foundMaterials = (from m in GlobalData.AllMaterials
                                      where (string.IsNullOrWhiteSpace(materialNo) ? true : (m["no"]?.ToString() ?? "") == materialNo)
                                      && (string.IsNullOrWhiteSpace(materialName) ? true : (m["name"]?.ToString() ?? "") == materialName)
                                      && (string.IsNullOrWhiteSpace(materialProductLine) ? true : materialProductLine == (m["productLine"]?.ToString() ?? ""))
                                      select m).ToArray();
                if (foundMaterials.Length != 1)
                {
                    goto FAILED;
                }
                model[row, "materialId"] = foundMaterials[0]["id"];
                model[row, "materialNo"] = foundMaterials[0]["no"];
                model[row, "materialName"] = foundMaterials[0]["name"];
                model[row, "materialProductLine"] = foundMaterials[0]["productLine"];
                return;

                FAILED:
                if (string.IsNullOrWhiteSpace(materialProductLine)) return;
                MessageBox.Show("物料不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            private void FindSupplierID(IModel model, int row)
            {
                model[row, "supplierId"] = 0;//先清除供货商ID
                string supplierNo = model[row, "supplierNo"]?.ToString() ?? "";
                string supplierName = model[row, "supplierName"]?.ToString() ?? "";
                if (string.IsNullOrWhiteSpace(supplierNo) && string.IsNullOrWhiteSpace(supplierName)) return;

                var foundSuppliers = (from s in GlobalData.AllSuppliers
                                      where (string.IsNullOrWhiteSpace(supplierNo) ? true : (s["no"]?.ToString() ?? "") == supplierNo)
                                      && (string.IsNullOrWhiteSpace(supplierName) ? true : (s["name"]?.ToString() ?? "") == supplierName)
                                      select s).ToArray();
                if (foundSuppliers.Length != 1) goto FAILED;
                int supplierID = (int)foundSuppliers[0]["id"];
                model[row, "supplierId"] = foundSuppliers[0]["id"];
                model[row, "supplierNo"] = foundSuppliers[0]["no"];
                model[row, "supplierName"] = foundSuppliers[0]["name"];
                return;

                FAILED:
                MessageBox.Show("供应商不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}

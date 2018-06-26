using FrontWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI.FormBasicInfos
{
    public partial class FormSafetyStock : Form
    {
        //private int materialId = -1;
        //private int supplierId = -1;
        private int stockType = -1;

        public FormSafetyStock(int stockType)
        {
            this.stockType = stockType;
            MethodListenerContainer.Register(this);
            InitializeComponent();
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

        //private List<int> rowChange = new List<int>();
        private void model_RowRemoved(object sender, ModelRowRemovedEventArgs e)
        {
            this.updateBasicAndReoGridView();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.basicView1.Enabled = true;
            this.reoGridView2.Enabled = true;
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "warehouseName",GlobalData.Warehouse["name"]},
                { "type",this.stockType},
            });
        }

        private string EnableForwardMapper(int state)
        {
            switch (state)
            {
                case 0: return "收货上架";
                case 1: return "备货";
                case 2: return "其他";
                default: return "未知状态";
            }
        }

        private int EnableBackwardMapper(string enable)
        {
            switch (enable)
            {
                case "收货上架": return 0;
                case "备货": return 1;
                case "其他": return 2;
                default: return -1;
            }
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

        private void FormSafetyStock_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            this.searchView1.AddStaticCondition("type", this.stockType);
            this.searchView1.Search();
            this.updateBasicAndReoGridView();
        }
        //关于目标库位
        private void TargetStorageLocationNoEditEnded(int row, string targetStorageLocationNo)
        {
            if ((string)this.model1[row, "sourceStorageLocationNo"] == targetStorageLocationNo)
            {
                MessageBox.Show($"库位与移出库位不能相同，库位代号 ：\"{targetStorageLocationNo}\"，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
            if ((string)this.model1[row, "sourceStorageLocationName"] == targetStorageLocationName)
            {
                MessageBox.Show($"库位与移出库位不能相同，库位名称 ：\"{targetStorageLocationName}\"，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
        //关于移出库位
        private void SourceStorageLocationNoEditEnded(int row, string sourceStorageLocationNo)
        {
            if ((string)this.model1[row, "targetStorageLocationNo"] == sourceStorageLocationNo)
            {
                MessageBox.Show($"库位与移出库位不能相同，库位代号 ：\"{sourceStorageLocationNo}\"，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
            if ((string)this.model1[row, "targetStorageLocationName"] == sourceStorageLocationName)
            {
                MessageBox.Show($"库位与移出库位不能相同，库位名称 ：\"{sourceStorageLocationName}\"，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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

        //===========为了实现一个看起来天经地义的交互逻辑=========

        private void SupplierNoEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "supplierNo"]?.ToString())) return;
            this.model1[row, "supplierName"] = "";
            this.FindSupplierID(row);
            this.FindMaterialID(row);
            this.TryGetSupplyID(row);
        }

        private void SupplierNameEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "supplierName"]?.ToString())) return;
            this.model1[row, "supplierNo"] = "";
            this.FindSupplierID(row);
            this.FindMaterialID(row);
            this.TryGetSupplyID(row);
        }

        private void MaterialNoEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "materialNo"]?.ToString())) return;
            this.FindSupplierID(row);
            this.FindMaterialID(row);
            this.TryGetSupplyID(row);
        }

        private void MaterialNameEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "materialName"]?.ToString())) return;
            this.FindMaterialID(row);
            this.FindSupplierID(row);
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
            if (string.IsNullOrWhiteSpace(materialProductLine)) return;
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
            return;

            FAILED:
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
                int a = (int)foundSupplies[0]["id"];
                this.model1[row, "supplyId"] = foundSupplies[0]["id"];
                this.FillDefaultValue(row, "amount", foundSupplies[0]["defaultDeliveryAmount"]);
                this.FillDefaultValue(row, "unit", foundSupplies[0]["defaultDeliveryUnit"]);
                this.FillDefaultValue(row, "unitAmount", foundSupplies[0]["defaultDeliveryUnitAmount"]);
                if (this.stockType == 1) {
                    this.FillDefaultValue(row, "targetStorageLocationId", foundSupplies[0]["defaultDeliveryStorageLocationId"]);
                    this.FillDefaultValue(row, "targetStorageLocationNo", foundSupplies[0]["defaultDeliveryStorageLocationNo"]);
                    this.FillDefaultValue(row, "targetStorageLocationName", foundSupplies[0]["defaultDeliveryStorageLocationName"]);
                    this.FillDefaultValue(row, "sourceStorageLocationId", foundSupplies[0]["defaultQualifiedStorageLocationId"]);
                    this.FillDefaultValue(row, "sourceStorageLocationNo", foundSupplies[0]["defaultQualifiedStorageLocationNo"]);
                    this.FillDefaultValue(row, "sourceStorageLocationName", foundSupplies[0]["defaultQualifiedStorageLocationName"]);
                }
                if (this.stockType == 0) {
                    this.FillDefaultValue(row, "targetStorageLocationId", foundSupplies[0]["defaultPrepareTargetStorageLocationId"]);
                    this.FillDefaultValue(row, "targetStorageLocationNo", foundSupplies[0]["defaultPrepareTargetStorageLocationNo"]);
                    this.FillDefaultValue(row, "targetStorageLocationName", foundSupplies[0]["defaultPrepareTargetStorageLocationName"]);
                    this.FillDefaultValue(row, "sourceStorageLocationId", foundSupplies[0]["defaultDeliveryStorageLocationId"]);
                    this.FillDefaultValue(row, "sourceStorageLocationNo", foundSupplies[0]["defaultDeliveryStorageLocationNo"]);
                    this.FillDefaultValue(row, "sourceStorageLocationName", foundSupplies[0]["defaultDeliveryStorageLocationName"]);
                }
                
            }
            else
            {
                MessageBox.Show("供应信息不存在，请重新选择物料——供货商！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void FillDefaultValue(int row, string fieldName, object value)
        {
            this.model1[row, fieldName] = value;
        }

        //=============天经地义的交互逻辑到这里结束===============

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
    }
}

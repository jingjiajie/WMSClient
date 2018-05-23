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
        public FormSafetyStock()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "warehouseName",GlobalData.Warehouse["name"]},
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

        private void FormSafetyStock_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }
        //关于目标库位
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
        //关于移出库位
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

        //===========为了实现一个看起来天经地义的交互逻辑=========

        private void SupplierNoEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "supplierNo"]?.ToString())) return;
            this.model1[row, "supplierName"] = "";
            this.FindSupplierID(row);
            this.TryGetSupplyID(row);
        }

        private void SupplierNameEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "supplierName"]?.ToString())) return;
            this.model1[row, "supplierNo"] = "";
            this.FindSupplierID(row);
            this.TryGetSupplyID(row);
        }

        private void MaterialNoEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "materialNo"]?.ToString())) return;
            this.model1[row, "materialName"] = "";
            this.FindMaterialID(row);
            this.TryGetSupplyID(row);
        }

        private void MaterialNameEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "materialName"]?.ToString())) return;
            this.model1[row, "materialNo"] = "";
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
            if (string.IsNullOrWhiteSpace(materialNo) && string.IsNullOrWhiteSpace(materialName)) return;
            var foundMaterials = (from m in GlobalData.AllMaterials
                                  where (string.IsNullOrWhiteSpace(materialNo) ? true : (m["no"]?.ToString() ?? "") == materialNo)
                                  && (string.IsNullOrWhiteSpace(materialName) ? true : (m["name"]?.ToString() ?? "") == materialName)
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
                this.model1[row, "supplyId"] = foundSupplies[0]["id"];
                this.FillDefaultValue(row, "amount", foundSupplies[0]["defaultDeliveryAmount"]);
                this.FillDefaultValue(row, "unit", foundSupplies[0]["defaultDeliveryUnit"]);
                this.FillDefaultValue(row, "unitAmount", foundSupplies[0]["defaultDeliveryUnitAmount"]);
                this.FillDefaultValue(row, "targetStorageLocationId", foundSupplies[0]["defaultPrepareTargetStorageLocationId"]);
                this.FillDefaultValue(row, "targetStorageLocationNo", foundSupplies[0]["defaultPrepareTargetStorageLocationNo"]);
                this.FillDefaultValue(row, "targetStorageLocationName", foundSupplies[0]["defaultPrepareTargetStorageLocationName"]);
                this.FillDefaultValue(row, "sourceStorageLocationId", foundSupplies[0]["defaultDeliveryStorageLocationId"]);
                this.FillDefaultValue(row, "sourceStorageLocationNo", foundSupplies[0]["defaultDeliveryStorageLocationNo"]);
                this.FillDefaultValue(row, "sourceStorageLocationName", foundSupplies[0]["defaultDeliveryStorageLocationName"]);
            }
        }

        private void FillDefaultValue(int row, string fieldName, object value)
        {
            this.model1[row, fieldName] = value;
        }

        //=============天经地义的交互逻辑到这里结束===============
    }
}

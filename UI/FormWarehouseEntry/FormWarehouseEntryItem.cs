﻿using FrontWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI
{
    public partial class FormWarehouseEntryItem : Form
    {
        private IDictionary<string, object> warehouseEntry = null;

        public FormWarehouseEntryItem(IDictionary<string,object> warehouseEntry)
        {
            MethodListenerContainer.Register(this);
            this.warehouseEntry = warehouseEntry;
            InitializeComponent();
            this.searchView1.AddStaticCondition("warehouseEntryId", this.warehouseEntry["id"]);
        }

        //添加按钮点击事件
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            this.model.InsertRow(0, null);
        }

        //删除按钮点击事件
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model.RemoveSelectedRows();
        }

        //保存按钮点击事件
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
            }
        }

        private void FormWarehouseEntry_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url",Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private void buttonInspect_Click(object sender, EventArgs e)
        {
            var selectionRange = this.model.SelectionRange;
            if(selectionRange == null)
            {
                MessageBox.Show("请选择要生成送检单的入库单！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var warehouseEntries = this.model.GetRows(Util.Range(selectionRange.Row, selectionRange.Row + selectionRange.Rows));
            new FormWarehouseEntryInspect(warehouseEntries).Show();
        }

        private int WarehouseEntryIDDefaultValue()
        {
            return (int)this.warehouseEntry["id"];
        }

        private void StorageLocationNoEditEnded(int row,string storageLocationNo)
        {
            this.model[row, "storageLocationId"] = 0;//先清除库位ID
            if (string.IsNullOrWhiteSpace(storageLocationNo)) return;
            var foundStorageLocations = (from s in GlobalData.AllStorageLocations
                                  where s["no"]?.ToString() == storageLocationNo
                                  select s).ToArray();
            if (foundStorageLocations.Length != 1) goto FAILED;
            this.model[row, "storageLocationId"] = (int)foundStorageLocations[0]["id"];
            this.model[row, "storageLocationName"] = foundStorageLocations[0]["name"];
            return;

            FAILED:
            MessageBox.Show($"库位\"{storageLocationNo}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void StorageLocationNameEditEnded(int row, string storageLocationName)
        {
            this.model[row, "storageLocationId"] = 0;//先清除库位ID
            if (string.IsNullOrWhiteSpace(storageLocationName)) return;
            var foundStorageLocations = (from s in GlobalData.AllStorageLocations
                                         where s["name"]?.ToString() == storageLocationName
                                         select s).ToArray();
            if (foundStorageLocations.Length != 1) goto FAILED;
            this.model[row, "storageLocationId"] = (int)foundStorageLocations[0]["id"];
            this.model[row, "storageLocationNo"] = foundStorageLocations[0]["no"];
            return;

            FAILED:
            MessageBox.Show($"库位\"{storageLocationName}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        //===========为了实现一个看起来天经地义的交互逻辑=========

        private void SupplierNoEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model[row, "materialNo"]?.ToString())) return;
            this.model[row, "supplierName"] = "";
            this.FindSupplierID(row);
            this.TryGetSupplyID(row);
        }

        private void SupplierNameEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model[row, "materialName"]?.ToString())) return;
            this.model[row, "supplierNo"] = "";
            this.FindSupplierID(row);
            this.TryGetSupplyID(row);
        }

        private void MaterialNoEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model[row, "materialNo"]?.ToString())) return;
            this.model[row, "materialName"] = "";
            this.FindMaterialID(row);
            this.TryGetSupplyID(row);
        }

        private void MaterialNameEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model[row, "materialName"]?.ToString())) return;
            this.model[row, "materialNo"] = "";
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
            this.model[row, "materialId"] = 0; //先清除物料ID
            string materialNo = this.model[row, "materialNo"]?.ToString() ?? "";
            string materialName = this.model[row, "materialName"]?.ToString() ?? "";
            string materialProductLine = this.model[row, "materialProductLine"]?.ToString() ?? "";
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
            this.model[row, "materialId"] = foundMaterials[0]["id"];
            this.model[row, "materialNo"] = foundMaterials[0]["no"];
            this.model[row, "materialName"] = foundMaterials[0]["name"];
            return;

            FAILED:
            MessageBox.Show("物料不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void FindSupplierID(int row)
        {
            this.model[row, "supplierId"] = 0;//先清除供货商ID
            string supplierNo = this.model[row, "supplierNo"]?.ToString() ?? "";
            string supplierName = this.model[row, "supplierName"]?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(supplierNo) && string.IsNullOrWhiteSpace(supplierName)) return;
            
            var foundSuppliers = (from s in GlobalData.AllSuppliers
                                  where (string.IsNullOrWhiteSpace(supplierNo) ? true : (s["no"]?.ToString() ?? "") == supplierNo)
                                  && (string.IsNullOrWhiteSpace(supplierName) ? true : (s["name"]?.ToString() ?? "") == supplierName)
                                  select s).ToArray();
            if (foundSuppliers.Length != 1) goto FAILED;
            int supplierID = (int)foundSuppliers[0]["id"];
            this.model[row, "supplierId"] = foundSuppliers[0]["id"];
            this.model[row, "supplierNo"] = foundSuppliers[0]["no"];
            this.model[row, "supplierName"] = foundSuppliers[0]["name"];
            return;

            FAILED:
            MessageBox.Show("供应商不存在，请重新填写！","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void TryGetSupplyID(int row)
        {
            this.model[row, "supplyId"] = 0; //先清除供货ID
            int supplierId = (int?)this.model[row, "supplierId"] ?? 0;
            int materialId = (int?)this.model[row, "materialId"] ?? 0;
            if (supplierId == 0 || materialId == 0) return;
            var foundSupplies = (from s in GlobalData.AllSupplies
                                 where (int)s["supplierId"] == supplierId 
                                 && (int)s["materialId"] == materialId
                                 select s).ToArray();
            //如果找到供货信息，则把供货设置的默认入库信息拷贝到相应字段上
            if (foundSupplies.Length == 1)
            {
                this.model[row, "supplyId"] = foundSupplies[0]["id"];
                this.FillDefaultValue(row, "expectedAmount", foundSupplies[0]["defaultEntryAmount"]);
                this.FillDefaultValue(row, "realAmount", foundSupplies[0]["defaultEntryAmount"]);
                this.FillDefaultValue(row, "unit", foundSupplies[0]["defaultEntryUnit"]);
                this.FillDefaultValue(row, "unitAmount", foundSupplies[0]["defaultEntryUnitAmount"]);
                this.FillDefaultValue(row, "refuseUnit", foundSupplies[0]["defaultEntryUnit"]);
                this.FillDefaultValue(row, "refuseUnitAmount", foundSupplies[0]["defaultEntryUnitAmount"]);
                this.FillDefaultValue(row, "storageLocationId", foundSupplies[0]["defaultEntryStorageLocationId"]);
                this.FillDefaultValue(row, "storageLocationNo", foundSupplies[0]["defaultInspectionStorageLocationNo"]);
                this.FillDefaultValue(row, "storageLocationName", foundSupplies[0]["defaultEntryStorageLocationName"]);
            }
        }

        private void FillDefaultValue(int row, string fieldName, object value)
        {
            this.model[row, fieldName] = value;
        }

        //=============天经地义的交互逻辑到这里结束===============
    }
}

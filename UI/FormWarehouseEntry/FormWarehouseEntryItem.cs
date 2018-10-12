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
        const int STATE_WAIT_FOR_PUT_IN = 0;

        private IDictionary<string, object> warehouseEntry = null;

        public FormWarehouseEntryItem(IDictionary<string,object> warehouseEntry)
        {
            MethodListenerContainer.Register("FormWarehouseEntryItem", this);
            this.warehouseEntry = warehouseEntry;
            InitializeComponent();
            this.searchView1.AddStaticCondition("warehouseEntryId", this.warehouseEntry["id"]);
            if ((int)warehouseEntry["state"] != STATE_WAIT_FOR_PUT_IN)
            {
                this.buttonAdd.Enabled = false;
                this.buttonDelete.Enabled = false;
            }
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

        private int SupplierIDDefaultValue()
        {
            return (int)this.warehouseEntry["supplierId"];
        }

        private string SupplierNameDefaultValue()
        {
            return (string)this.warehouseEntry["supplierName"];
        }

        private void FormWarehouseEntry_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url",Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private string AmountForwardMapper(double amount,int row)
        {
            double? unitAmount = (double?)this.model[row, "unitAmount"];
            if(unitAmount.HasValue == false || unitAmount == 0)
            {
                return amount.ToString();
            }else
            {
                return Utilities.DoubleToString(amount / unitAmount.Value);
            }
        }

        private double AmountBackwardMapper(string strAmount, int row)
        {
            if(!Double.TryParse(strAmount,out double amount))
            {
                MessageBox.Show($"\"{strAmount}\"不是合法的数字","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
            double? unitAmount = (double?)this.model[row, "unitAmount"];
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
            this.model.RefreshView(row);
        }

        private void PersonEditEnded(int row, string personName)
        {
            this.model[row, "personId"] = 0;//先清除ID
            if (string.IsNullOrWhiteSpace(personName)) return;
            var foundPersons = (from s in GlobalData.AllPersons
                                where s["name"]?.ToString() == personName
                                select s).ToArray();
            if (foundPersons.Length != 1) goto FAILED;
            this.model[row, "personId"] = (int)foundPersons[0]["id"];
            this.model[row, "personName"] = foundPersons[0]["name"];
            return;

            FAILED:
            MessageBox.Show($"人员\"{personName}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        //private void buttonInspect_Click(object sender, EventArgs e)
        //{
        //    var selectionRange = this.model.SelectionRange;
        //    if(selectionRange == null)
        //    {
        //        MessageBox.Show("请选择要生成送检单的入库单！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }
        //    var warehouseEntries = this.model.GetRows(Util.Range(selectionRange.Row, selectionRange.Row + selectionRange.Rows));
        //    new FormWarehouseEntryInspect(warehouseEntries).Show();
        //}

        private int WarehouseEntryIDDefaultValue()
        {
            return (int)this.warehouseEntry["id"];
        }

        private string StateForwardMapper(int state)
        {
            switch (state)
            {
                case 0: return "待入库";
                case 1: return "送检中";
                case 2: return "正品入库";
                case 3: return "不良品入库";
                default: return "未知状态";
            }
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


        private void QualifiedStorageLocationNoEditEnded(int row, string storageLocationNo)
        {
            this.model[row, "qualifiedStorageLocationId"] = 0;//先清除库位ID
            if (string.IsNullOrWhiteSpace(storageLocationNo)) return;
            var foundStorageLocations = (from s in GlobalData.AllStorageLocations
                                         where s["no"]?.ToString() == storageLocationNo
                                         select s).ToArray();
            if (foundStorageLocations.Length != 1) goto FAILED;
            this.model[row, "qualifiedStorageLocationId"] = (int)foundStorageLocations[0]["id"];
            this.model[row, "qualifiedStorageLocationName"] = foundStorageLocations[0]["name"];
            return;

            FAILED:
            MessageBox.Show($"库位\"{storageLocationNo}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void QualifiedStorageLocationNameEditEnded(int row, string storageLocationName)
        {
            this.model[row, "qualifiedStorageLocationId"] = 0;//先清除库位ID
            if (string.IsNullOrWhiteSpace(storageLocationName)) return;
            var foundStorageLocations = (from s in GlobalData.AllStorageLocations
                                         where s["name"]?.ToString() == storageLocationName
                                         select s).ToArray();
            if (foundStorageLocations.Length != 1) goto FAILED;
            this.model[row, "qualifiedStorageLocationId"] = (int)foundStorageLocations[0]["id"];
            this.model[row, "qualifiedStorageLocationNo"] = foundStorageLocations[0]["no"];
            return;

            FAILED:
            MessageBox.Show($"库位\"{storageLocationName}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void UnqualifiedStorageLocationNoEditEnded(int row, string storageLocationNo)
        {
            this.model[row, "unqualifiedStorageLocationId"] = 0;//先清除库位ID
            if (string.IsNullOrWhiteSpace(storageLocationNo)) return;
            var foundStorageLocations = (from s in GlobalData.AllStorageLocations
                                         where s["no"]?.ToString() == storageLocationNo
                                         select s).ToArray();
            if (foundStorageLocations.Length != 1) goto FAILED;
            this.model[row, "unqualifiedStorageLocationId"] = (int)foundStorageLocations[0]["id"];
            this.model[row, "unqualifiedStorageLocationName"] = foundStorageLocations[0]["name"];
            return;

            FAILED:
            MessageBox.Show($"库位\"{storageLocationNo}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void UnqualifiedStorageLocationNameEditEnded(int row, string storageLocationName)
        {
            this.model[row, "unqualifiedStorageLocationId"] = 0;//先清除库位ID
            if (string.IsNullOrWhiteSpace(storageLocationName)) return;
            var foundStorageLocations = (from s in GlobalData.AllStorageLocations
                                         where s["name"]?.ToString() == storageLocationName
                                         select s).ToArray();
            if (foundStorageLocations.Length != 1) goto FAILED;
            this.model[row, "unqualifiedStorageLocationId"] = (int)foundStorageLocations[0]["id"];
            this.model[row, "unqualifiedStorageLocationNo"] = foundStorageLocations[0]["no"];
            return;

            FAILED:
            MessageBox.Show($"库位\"{storageLocationName}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        //===========为了实现一个看起来天经地义的交互逻辑=========

        private void SupplierNoEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model[row, "supplierNo"]?.ToString())) return;
            this.model[row, "supplierName"] = "";
            this.FindSupplierID(row);
            this.TryGetSupplyID(row);
        }

        private void SupplierNameEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model[row, "supplierName"]?.ToString())) return;
            this.model[row, "supplierNo"] = "";
            this.FindSupplierID(row);
            this.TryGetSupplyID(row);
        }

        private void MaterialNoEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model[row, "materialNo"]?.ToString())) return;
            //this.model[row, "materialName"] = "";
            this.FindMaterialID(row);
            this.TryGetSupplyID(row);
        }

        private void MaterialNameEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model[row, "materialName"]?.ToString())) return;
           // this.model[row, "materialNo"] = "";
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
            var foundMaterials = (from m in GlobalData.AllMaterials
                                  where (string.IsNullOrWhiteSpace(materialNo) ? true : (m["no"]?.ToString() ?? "") == materialNo)
                                  && (string.IsNullOrWhiteSpace(materialName) ? true : (m["name"]?.ToString() ?? "") == materialName)
                                  && (string.IsNullOrWhiteSpace(materialProductLine) ? true : materialProductLine == (m["productLine"]?.ToString() ?? ""))
                                  select m).ToArray();
            if (foundMaterials.Length != 1)
            {
                goto FAILED;
            }
            this.model[row, "materialId"] = foundMaterials[0]["id"];
            this.model[row, "materialNo"] = foundMaterials[0]["no"];
            this.model[row, "materialName"] = foundMaterials[0]["name"];
            this.model[row, "materialProductLine"] = foundMaterials[0]["productLine"];
            return;

            FAILED:
            if (string.IsNullOrWhiteSpace(materialProductLine)) return;
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
                this.FillValueIfEmpty(row, "expectedAmount", foundSupplies[0]["defaultEntryAmount"]);
                this.FillValueIfEmpty(row, "realAmount", foundSupplies[0]["defaultEntryAmount"]);
                this.FillValueIfEmpty(row, "unit", foundSupplies[0]["defaultEntryUnit"]);
                this.FillValueIfEmpty(row, "unitAmount", foundSupplies[0]["defaultEntryUnitAmount"]);
                this.FillValueIfEmpty(row, "refuseUnit", foundSupplies[0]["defaultEntryUnit"]);
                this.FillValueIfEmpty(row, "refuseUnitAmount", foundSupplies[0]["defaultEntryUnitAmount"]);
                if (((int?)this.model[row, "storageLocationId"] ?? 0) == 0)
                {
                    this.model[row, "storageLocationNo"] = foundSupplies[0]["defaultEntryStorageLocationNo"];
                }
                if (((int?)this.model[row, "qualifiedStorageLocationId"] ?? 0) == 0)
                {
                    this.model[row, "qualifiedStorageLocationNo"] = foundSupplies[0]["defaultQualifiedStorageLocationNo"];
                }
                if (((int?)this.model[row, "unqualifiedStorageLocationId"] ?? 0) == 0)
                {
                    this.model[row, "unqualifiedStorageLocationNo"] = foundSupplies[0]["defaultUnqualifiedStorageLocationNo"];
                }
                this.model.RefreshView(row);
            }
        }

        private void FillValueIfEmpty(int row, string fieldName, object value)
        {
            if (string.IsNullOrWhiteSpace(this.model[row, fieldName]?.ToString()))
            {
                this.model[row, fieldName] = value;
            }
        }

        //=============天经地义的交互逻辑到这里结束===============

        private void model_SelectionRangeChanged(object sender, ModelSelectionRangeChangedEventArgs e)
        {
            this.RefreshMode();
        }

        private void model_Refreshed(object sender, ModelRefreshedEventArgs e)
        {
            this.RefreshMode();
        }

        private void RefreshMode()
        {
            var rows = this.model.GetSelectedRows();
            if (rows.Length == 0) return;
            if ((int)rows[0]["state"] == STATE_WAIT_FOR_PUT_IN)
            {
                this.basicView1.Mode = "default";
                this.reoGridView1.Mode = "default";
            }
            else
            {
                this.basicView1.Mode = "put_in";
                this.reoGridView1.Mode = "put_in";
            }
        }

        //物料名称输入联想
        private object[] MaterialNameAssociation(string str)
        {

            string materialNo = this.model[this.model.SelectionRange.Row, "materialNo"]?.ToString() ?? "";
            int[] selectedIDs = this.model.GetSelectedRows<int>("supplierId").Except(new int[] { 0 }).ToArray();
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
            string materialName = this.model[this.model.SelectionRange.Row, "materialName"]?.ToString() ?? "";

            int[] selectedIDs = this.model.GetSelectedRows<int>("supplierId").Except(new int[] { 0 }).ToArray();
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
            int[] selectedIDs = this.model.GetSelectedRows<int>("supplierId").Except(new int[] { 0 }).ToArray();
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

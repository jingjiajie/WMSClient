using FrontWork;
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

        private string AmountForwardMapper([Data] double amount,[Row] int row)
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

        private double AmountBackwardMapper([Data]double amount, [Row]int row)
        {
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

        private int StateBackwardMapper([Data]string state)
        {
            switch (state)
            {
                case "待入库": return 0;
                case "送检中": return 1;
                case "正品入库": return 2;
                case "不良品入库": return 3;
                default: return -1;
            }
        }


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
    }

    [MethodListener]
    public class FormWarehouseEntryItemMethodListener
    {
        public string[] SupplySerialNoAssociation([Model] IModel model, [Row] int row, [Data] string input)
        {
            return (from s in GlobalData.AllSupplies
                    where s["serialNo"] != null
                    && s["serialNo"].ToString().StartsWith(input)
                    && (int)s["supplierId"] == (int)model[row,"supplierId"]
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

        private void FillSupplyFields(IModel model, int row, IDictionary<string,object> supply)
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
            model[row, "expectedAmount"] = supply["defaultEntryAmount"];
            model[row, "unit"] = supply["defaultEntryUnit"];
            model[row, "unitAmount"] = supply["defaultEntryUnitAmount"];
            string defaultEntryStorageLocationNo = supply["defaultEntryStorageLocationNo"] as string;
            string defaultQualifiedStorageLocationNo = supply["defaultQualifiedStorageLocationNo"] as string;
            string defaultUnqualifiedStorageLocationNo = supply["defaultUnqualifiedStorageLocationNo"] as string;
            model[row, "storageLocationName"] = null;
            model[row, "qualifiedStorageLocationName"] = null;
            model[row, "unqualifiedStorageLocationName"] = null;
            model[row, "storageLocationNo"] = defaultEntryStorageLocationNo;
            model[row, "qualifiedStorageLocationNo"] = defaultQualifiedStorageLocationNo;
            model[row, "unqualifiedStorageLocationNo"] = defaultUnqualifiedStorageLocationNo;
            this.FindStorageLocation(model, row, "storageLocation", FindStorageLocationBy.NO, defaultEntryStorageLocationNo, false);
            this.FindStorageLocation(model, row, "qualifiedStorageLocation", FindStorageLocationBy.NO, defaultQualifiedStorageLocationNo, false);
            this.FindStorageLocation(model, row, "unqualifiedStorageLocation", FindStorageLocationBy.NO, defaultUnqualifiedStorageLocationNo, false);
            model.UpdateCellState(row, "supplySerialNo", new ModelCellState(ValidationState.OK));
            model.RefreshView(row);
            return;
        }


        private void StorageLocationNoEditEnded([Model] IModel model, [Row] int row, [Data] string storageLocationNo)
        {
            this.FindStorageLocation(model, row, "storageLocation", FindStorageLocationBy.NO, storageLocationNo);
        }

        private void StorageLocationNameEditEnded([Model] IModel model,[Row] int row,[Data] string storageLocationName)
        {
            this.FindStorageLocation(model, row, "storageLocation", FindStorageLocationBy.NAME, storageLocationName);
        }

        private void QualifiedStorageLocationNoEditEnded([Model] IModel model, [Row] int row, [Data] string storageLocationNo)
        {
            this.FindStorageLocation(model, row, "qualifiedStorageLocation", FindStorageLocationBy.NO, storageLocationNo);
        }

        private void QualifiedStorageLocationNameEditEnded([Model] IModel model, [Row] int row, [Data] string storageLocationName)
        {
            this.FindStorageLocation(model, row, "qualifiedStorageLocation", FindStorageLocationBy.NAME, storageLocationName);
        }

        private void UnqualifiedStorageLocationNoEditEnded([Model] IModel model, [Row] int row, [Data] string storageLocationNo)
        {
            this.FindStorageLocation(model, row, "unqualifiedStorageLocation", FindStorageLocationBy.NO, storageLocationNo);
        }

        private void UnqualifiedStorageLocationNameEditEnded([Model] IModel model, [Row] int row, [Data] string storageLocationName)
        {
            this.FindStorageLocation(model, row, "unqualifiedStorageLocation", FindStorageLocationBy.NAME, storageLocationName);
        }

        enum FindStorageLocationBy{
            NAME, NO
        }

        private void FindStorageLocation(IModel model, int row, string storageLocationFieldName, FindStorageLocationBy byField , string value, bool warning = true)
        {
            model[row, storageLocationFieldName + "Id"] = null;//先清除库位ID
            
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
        private object[] MaterialNameAssociation([Model] IModel model,[Data] string str)
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
        private object[] MaterialNoAssociation([Model] IModel model,[Data] string str)
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
            this.FindSupplierID(model,row);
            this.FindSupplyByMaterialAndSupplier(model, row);
        }

        private void SupplierNameEditEnded([Model] IModel model, [Row] int row)
        {
            if (string.IsNullOrWhiteSpace(model[row, "supplierName"]?.ToString())) return;
            model[row, "supplierNo"] = "";
            this.FindSupplierID(model, row);
            this.FindSupplyByMaterialAndSupplier(model, row);
        }

        private void MaterialNoEditEnded([Model] IModel model, [Row] int row)
        {
            if (string.IsNullOrWhiteSpace(model[row, "materialNo"]?.ToString())) return;
            //this.model[row, "materialName"] = "";
            this.FindMaterialID(model, row);
            this.FindSupplyByMaterialAndSupplier(model, row);
        }

        private void MaterialNameEditEnded([Model] IModel model, [Row] int row)
        {
            if (string.IsNullOrWhiteSpace(model[row, "materialName"]?.ToString())) return;
            // this.model[row, "materialNo"] = "";
            this.FindMaterialID(model, row);
            this.FindSupplyByMaterialAndSupplier(model, row);
        }

        private void MaterialProductLineEditEnded([Model] IModel model, [Row] int row)
        {
            this.FindMaterialID(model, row);
            this.FindSupplyByMaterialAndSupplier(model, row);
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

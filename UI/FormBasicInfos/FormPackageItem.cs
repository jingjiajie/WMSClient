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
    public partial class FormPackageItem : Form
    {
        private IDictionary<string, object> package = null;
        public FormPackageItem(IDictionary<string, object> package)
        {
            MethodListenerContainer.Register(this);
            this.package = package;
            InitializeComponent();
            this.searchView1.AddStaticCondition("packageId", this.package["id"]);
            this.model.RowRemoved += this.model_RowRemoved;
            this.model.Refreshed += this.model_Refreshed;
        }

        private void model_Refreshed(object sender, ModelRefreshedEventArgs e)
        {
            this.updateBasicAndReoGridView();
        }

        private void updateBasicAndReoGridView()
        {

            if (this.model.RowCount == 0)
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

        //private List<int> rowChange = new List<int>();
        private void model_RowRemoved(object sender, ModelRowRemovedEventArgs e)
        {
            this.updateBasicAndReoGridView();
        }

        //添加按钮点击事件
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            this.basicView1.Enabled = true;
            this.reoGridView1.Enabled = true;
            this.model.InsertRow(0, new Dictionary<string, object>()
            {
                 { "packageId",this.package["id"]},
                 { "packageName",this.package["name"]},
                 { "enabled",1},
            });
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
            this.synchronizer.Save();
        }

        private void FormPackageItem_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private void defaultDeliveryStorageLocationNameEditEnded(int row, string defaultDeliveryStorageLocationName)
        {
            IDictionary<string, object> foundStorageLocation =
                GlobalData.AllStorageLocations.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == defaultDeliveryStorageLocationName;
                });
            if (foundStorageLocation == null)
            {
                MessageBox.Show($"库位\"{defaultDeliveryStorageLocationName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model[row, "defaultDeliveryStorageLocationId"] = foundStorageLocation["id"];
            }
        }

        //写了一个小时发感觉不太对然后发现有这个东西
        //丧心病狂的交互逻辑
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
            this.FindMaterialID(row);
            this.TryGetSupplyID(row);
        }

        private void MaterialNameEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model[row, "materialName"]?.ToString())) return;
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
            MessageBox.Show("供应商不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                this.FillDefaultValue(row, "defaultDeliveryAmount", foundSupplies[0]["defaultDeliveryAmount"]);
                this.FillDefaultValue(row, "defaultDeliveryUnit", foundSupplies[0]["defaultDeliveryUnit"]);
                this.FillDefaultValue(row, "defaultDeliveryUnitAmount", foundSupplies[0]["defaultDeliveryUnitAmount"]);

            }
        }

        private void FillDefaultValue(int row, string fieldName, object value)
        {
            this.model[row, fieldName] = value;
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

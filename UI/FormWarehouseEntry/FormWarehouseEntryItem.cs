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
            this.model.InsertRow(0,new Dictionary<string, object>()
            {
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "createPersonId",GlobalData.Person["id"]},
                { "createPersonName",GlobalData.Person["name"]},
                { "createTime",DateTime.Now}
            });
        }

        //删除按钮点击事件
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            this.model.RemoveSelectedRows();
        }

        //保存按钮点击事件
        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.synchronizer.Save();
        }

        private void FormWarehouseEntry_Load(object sender, EventArgs e)
        {
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

        //===========为了实现一个看起来天经地义的交互逻辑=========

        private IList<string> editedSupplyFields = new List<string>();
        private void SetSupplyFieldEdited(string fieldName)
        {
            if(editedSupplyFields.Contains(fieldName) == false)
            {
                editedSupplyFields.Add(fieldName);
            }
        }

        private void SupplierNoEditEnded(int row)
        {
            this.SetSupplyFieldEdited("supplierNo");
            this.TryGetSupplyID(row);
        }

        private void SupplierNameEditEnded(int row)
        {
            this.SetSupplyFieldEdited("supplierName");
            this.TryGetSupplyID(row);
        }

        private void MaterialNoEditEnded(int row)
        {
            this.SetSupplyFieldEdited("materialNo");
            this.TryGetSupplyID(row);
        }

        private void MaterialNameEditEnded(int row)
        {
            this.SetSupplyFieldEdited("materialName");
            this.TryGetSupplyID(row);
        }

        private void MaterialProductLineEditEnded(int row)
        {
            this.SetSupplyFieldEdited("materialProductLine");
            this.TryGetSupplyID(row);
        }

        private void TryGetSupplyID(int row)
        {
            string materialNo = this.model[row, "materialNo"]?.ToString() ?? "";
            string materialName = this.model[row, "materialName"]?.ToString() ?? "";
            string materialProductLine = this.model[row, "materialProductLine"]?.ToString() ?? "";
            string supplierNo = this.model[row, "supplierNo"]?.ToString() ?? "";
            string supplierName = this.model[row, "supplierName"]?.ToString() ?? "";
            bool strictMaterialNo = false;
            bool strictMaterialName = false;
            bool strictSupplierNo = false;
            bool strictSupplierName = false;
            if (string.IsNullOrWhiteSpace(materialNo) && string.IsNullOrWhiteSpace(materialName)) goto FAILED;
            if (string.IsNullOrWhiteSpace(materialProductLine)) goto FAILED;
            if (string.IsNullOrWhiteSpace(supplierNo) && string.IsNullOrWhiteSpace(supplierName)) goto FAILED;
            if (editedSupplyFields.Contains("materialNo")) strictMaterialNo = true;
            if (editedSupplyFields.Contains("materialName")) strictMaterialName = true;
            if (editedSupplyFields.Contains("supplierNo")) strictSupplierNo= true;
            if (editedSupplyFields.Contains("materialName")) strictMaterialName = true;
            var foundMaterials = (from m in GlobalData.AllMaterials
                                  where (string.IsNullOrWhiteSpace(materialNo) ? !strictMaterialNo : (m["no"].ToString() ?? "") == materialNo)
                                  && (string.IsNullOrWhiteSpace(materialName) ? !strictMaterialName : (m["name"].ToString() ?? "") == materialNo)
                                  && materialProductLine == (m["productLine"]?.ToString() ?? "")
                                  select m).ToArray();
            if (foundMaterials.Length != 1)
            {
                goto FAILED;
            }
            var foundSuppliers = (from s in GlobalData.AllSuppliers
                                  where (string.IsNullOrWhiteSpace(supplierNo) ?  !strictSupplierNo : (s["no"]?.ToString() ?? "") == supplierNo)
                                  && (string.IsNullOrWhiteSpace(supplierName) ? !strictSupplierName : (s["name"]?.ToString() ?? "") == supplierName)
                                  select s).ToArray();
            if (foundSuppliers.Length != 1) goto FAILED;
            int materialID = (int)foundMaterials[0]["id"];
            int supplierID = (int)foundSuppliers[0]["id"];
            var foundSupplies = (from s in GlobalData.AllSupplies
                                 where (int)s["materialId"] == materialID
                                 && (int)s["supplierId"] == supplierID
                                 select s).ToArray();
            if (foundSupplies.Length != 1) goto FAILED;
            this.model[row, "supplyId"] = foundSupplies[0]["id"];
            this.model[row, "supplierNo"] = foundSuppliers[0]["no"];
            this.model[row, "supplierName"] = foundSuppliers[0]["name"];
            this.model[row, "materialNo"] = foundMaterials[0]["no"];
            this.model[row, "materialName"] = foundMaterials[0]["name"];
            this.editedSupplyFields.Clear();
            return;

            FAILED:
            this.model[row, "supplyId"] = 0;
            return;
        }

        //=============天经地义的交互逻辑到这里结束===============
    }
}

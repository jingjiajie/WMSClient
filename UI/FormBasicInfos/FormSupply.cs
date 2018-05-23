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
    public partial class FormSupply : Form
    {
        public FormSupply()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
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
        private void FormSupply_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "createPersonId",GlobalData.Person["id"]},
                { "createPersonName",GlobalData.Person["name"]},
                { "warehouseName",GlobalData.Warehouse["name"]},
                { "createTime",DateTime.Now},
                { "enabled",1}
            });
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            this.synchronizer.Save();
        }

        private void MaterialNameEditEnded(int row, string materialName)
        {
            IDictionary<string, object> foundMaterial =
                GlobalData.AllMaterials.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == materialName;
                });
            if (foundMaterial == null)
            {
                MessageBox.Show($"物料\"{materialName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "materialId"] = foundMaterial["id"];
                this.model1[row, "materialName"] = foundMaterial["name"];
            }
        }
        //供应商名称编辑完成，根据名称自动搜索ID和No
        private void SupplierNameEditEnded(int row, string supplierName)
        {
            IDictionary<string, object> foundSupplier =
                GlobalData.AllSuppliers.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == supplierName;
                });
            if (foundSupplier == null)
            {
                MessageBox.Show($"供应商\"{supplierName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "supplierId"] = foundSupplier["id"];
            }
        }

        private void DefaultEntryStorageLocationNameEditEnded(int row, string defaultEntryStorageLocationName)
        {
            IDictionary<string, object> foundStorageLocation =
                GlobalData.AllStorageLocations.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == defaultEntryStorageLocationName;
                });
            if (foundStorageLocation == null)
            {
                MessageBox.Show($"库位\"{defaultEntryStorageLocationName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "defaultEntryStorageLocationId"] = foundStorageLocation["id"];
            }
        }
        private void defaultInspectionStorageLocationNameEditEnded(int row, string defaultInspectionStorageLocationName)
        {
            IDictionary<string, object> foundStorageLocation =
                GlobalData.AllStorageLocations.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == defaultInspectionStorageLocationName;
                });
            if (foundStorageLocation == null)
            {
                MessageBox.Show($"库位\"{defaultInspectionStorageLocationName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "defaultInspectionStorageLocationId"] = foundStorageLocation["id"];
            }
        }
        private void defaultQualifiedStorageLocationNameEditEnded(int row, string defaultQualifiedStorageLocationName)
        {
            IDictionary<string, object> foundStorageLocation =
                GlobalData.AllStorageLocations.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == defaultQualifiedStorageLocationName;
                });
            if (foundStorageLocation == null)
            {
                MessageBox.Show($"库位\"{defaultQualifiedStorageLocationName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "defaultQualifiedStorageLocationId"] = foundStorageLocation["id"];
            }
        }
        private void defaultUnqualifiedStorageLocationNameEditEnded(int row, string defaultUnqualifiedStorageLocationName)
        {
            IDictionary<string, object> foundStorageLocation =
                GlobalData.AllStorageLocations.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == defaultUnqualifiedStorageLocationName;
                });
            if (foundStorageLocation == null)
            {
                MessageBox.Show($"库位\"{defaultUnqualifiedStorageLocationName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "defaultUnqualifiedStorageLocationId"] = foundStorageLocation["id"];
            }
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
                this.model1[row, "defaultDeliveryStorageLocationId"] = foundStorageLocation["id"];
            }
        }
        private void defaultPrepareTargetStorageLocationNameEditEnded(int row, string defaultPrepareTargetStorageLocationName)
        {
            IDictionary<string, object> foundStorageLocation =
                GlobalData.AllStorageLocations.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == defaultPrepareTargetStorageLocationName;
                });
            if (foundStorageLocation == null)
            {
                MessageBox.Show($"库位\"{defaultPrepareTargetStorageLocationName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "defaultPrepareTargetStorageLocationId"] = foundStorageLocation["id"];
            }
        }

    }
     
}

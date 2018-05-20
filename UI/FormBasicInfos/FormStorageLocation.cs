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
    public partial class FormStorageLocation : Form
    {
        public FormStorageLocation()
        {
            InitializeComponent();
        }

        private void WarehouseNameEditEnded(int row, string warehouseName)
        {
            IDictionary<string, object> foundWarehouse =
                GlobalData.AllWarehouses.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == warehouseName;
                });
            if (foundWarehouse == null)
            {
                MessageBox.Show($"仓库\"{warehouseName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "warehouseId"] = foundWarehouse["id"];
            }
        }


        private void StorageAreaNameEditEnded(int row, string storageAreaName)
        {
            IDictionary<string, object> foundStorageArea =
                GlobalData.AllWarehouses.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == storageAreaName;
                });
            if (foundStorageArea == null)
            {
                MessageBox.Show($"仓库\"{storageAreaName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "storageAreaId"] = foundStorageArea["id"];
                this.model1[row, "storgeAreaName"] = foundStorageArea["name"];
            }
        }

        private void FormStorageLocation_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, null);
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            this.model1.RemoveSelectedRows();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            this.synchronizer.Save();
        }
    }
}

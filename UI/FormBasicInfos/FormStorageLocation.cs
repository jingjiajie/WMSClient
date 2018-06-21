using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontWork;

namespace WMS.UI.FormBasicInfos
{
    public partial class FormStorageLocation : Form
    {
        public FormStorageLocation()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private void StorageAreaNameEditEnded(int row, string storageAreaName)
        {          
            IDictionary<string, object> foundStorageArea =
                GlobalData.AllStorageAreas.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == storageAreaName;
                });
            if (foundStorageArea == null)
            {
                MessageBox.Show($"库区\"{storageAreaName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "storageAreaId"] = foundStorageArea["id"];
                this.model1[row, "storageAreaNo"] = foundStorageArea["no"];
            }
        }

        private void StorageAreaNoEditEnded(int row, string storageAreaName)
        {
            IDictionary<string, object> foundStorageArea =
                GlobalData.AllStorageAreas.Find((s) =>
                {
                    if (s["no"] == null) return false;
                    return s["no"].ToString() == storageAreaName;
                });
            if (foundStorageArea == null)
            {
                MessageBox.Show($"库区编号\"{storageAreaName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "storageAreaId"] = foundStorageArea["id"];
                this.model1[row, "storageAreaName"] = foundStorageArea["name"];
            }
        }


        private void FormStorageLocation_Load(object sender, EventArgs e)
        {
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
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
                Condition condWarehouse = new Condition().AddCondition("warehouseId", GlobalData.Warehouse["id"]);
                GlobalData.AllStorageLocations = RestClient.Get<List<IDictionary<string, object>>>(
                   $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/storage_location/{condWarehouse.ToString()}");
            }
        }

        private string EnableForwardMapper(int state)
        {
            switch (state)
            {
                case 0: return "禁用";
                case 1: return "启用";
                default: return "未知状态";
            }
        }

        private int EnableBackwardMapper(string enable)
        {
            switch (enable)
            {
                case "禁用": return 0;
                case "启用": return 1;
                default: return -1;
            }
        }
    }
}

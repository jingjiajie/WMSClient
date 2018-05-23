using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontWork;

namespace WMS.UI.FormStockTaking
{
    public partial class FormStockTakingOrderItem : Form
    {
        private IDictionary<string, object> stockTakingOrder = null;

        public FormStockTakingOrderItem(IDictionary<string, object> srockTakingOrder)
        {
            MethodListenerContainer.Register(this);
            this.stockTakingOrder = srockTakingOrder;
            InitializeComponent();
            this.searchView1.AddStaticCondition("stockTakingOrder", this.stockTakingOrder["id"]);
        }

        private void FormStockTakingOrderItem_Load(object sender, EventArgs e)
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
                { "createPersonId",GlobalData.Person["id"]},             
                { "createPersonName",GlobalData.Person["name"]}
            });
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            this.model1.RemoveSelectedRows();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            this.synchronizer.Save();
        }


      

        //库位
        private void StorageLocationNoEditEnded(int row, string storageLocationNo)
        {
            IDictionary<string, object> foundStorageLocation =
                GlobalData.AllStorageLocations.Find((s) =>
                {
                    if (s["no"] == null) return false;
                    return s["no"].ToString() == storageLocationNo;
                });
            if (foundStorageLocation == null)
            {
                MessageBox.Show($"库位\"{storageLocationNo}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "storageLocationId"] = foundStorageLocation["id"];
                this.model1[row, "storageLocationName"] = foundStorageLocation["name"];
            }
        }

        private void StorageLocationNameEditEnded(int row, string storageLocationName)
        {
            IDictionary<string, object> foundStorageLocation =
                GlobalData.AllStorageLocations.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == storageLocationName;
                });
            if (foundStorageLocation == null)
            {
                MessageBox.Show($"库位\"{storageLocationName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "storageLocationId"] = foundStorageLocation["id"];
                this.model1[row, "storageLocationNo"] = foundStorageLocation["no"];
            }
        }

        //物料
        private void MaterialNoEditEnded(int row, string materialNo)
        {
            IDictionary<string, object> foundMaterial =
                GlobalData.AllStorageLocations.Find((s) =>
                {
                    if (s["no"] == null) return false;
                    return s["no"].ToString() == materialNo;
                });
            if (foundMaterial == null)
            {
                MessageBox.Show($"物料\"{materialNo}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "materialName"] = foundMaterial["name"];
                this.model1[row, "materialProductLine"] = foundMaterial["productLine"];
            }
        }

    }
}

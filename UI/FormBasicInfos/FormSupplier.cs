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
    public partial class FormSupplier : Form
    {
        public FormSupplier()
        {            
            InitializeComponent();
           this.model1.CellUpdated+= this.model_CellUpdated;
        }

        private void model_CellUpdated(object sender, ModelCellUpdatedEventArgs e)
        {            
            foreach (var cell in e.UpdatedCells)
            {          
                if (cell.ColumnName.StartsWith("lastUpdate")) return;
                this.model1[cell.Row, "lastUpdatePersonId"] = GlobalData.Person["id"];
                this.model1[cell.Row, "lastUpdatePersonName"] = GlobalData.Person["name"];
                this.model1[cell.Row, "lastUpdateTime"] = DateTime.Now;
              //  this.model1[cell.Row, "enabled"] = 1;   //TODO 需要设置吗 
            }
        }


        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "createPersonId",GlobalData.Person["id"]},
                { "createPersonName",GlobalData.Person["name"]},
                { "createTime",DateTime.Now},
                { "enabled",1}
            });
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            this.synchronizer.Save();
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            this.model1.RemoveSelectedRows();
        }

        private void configuration1_Load(object sender, EventArgs e)
        {

        }

        //
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
                this.model1[row, "warehouseName"] = foundWarehouse["name"];
            }
        }

        private void FormSupplier_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }
    }
}

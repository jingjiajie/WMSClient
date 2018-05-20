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
    public partial class FormMaterial : Form
    {
        public FormMaterial()
        {
            InitializeComponent();
        }

        private void FormMaterial_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.jsonRESTSynchronizer1.SetRequestParameter("$url", Defines.ServerURL);
            this.jsonRESTSynchronizer1.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private void model_CellUpdated(object sender, ModelCellUpdatedEventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "warehouseId",GlobalData.Warehouse["id"]},//todo 物料跟仓库绑定了？
            });
        }


        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, null);
        }

        private void configuration1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            this.model1.RemoveSelectedRows();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            this.jsonRESTSynchronizer1.Save();
        }

        //仓库名称编辑完成，根据名称自动搜索ID
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
    }
}

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
    public partial class FormWarehouseEntry : Form
    {
        private List<IDictionary<string, object>> allSuppliers = new List<IDictionary<string, object>>();

        public FormWarehouseEntry()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
            this.model1.CellUpdated += this.model_CellUpdated;
            //缓存所有供应商信息，联想用
            this.RefreshAllSuppliers();
        }

        private void model_CellUpdated(object sender, ModelCellUpdatedEventArgs e)
        {
            foreach(var cell in e.UpdatedCells)
            {
                if (cell.ColumnName.StartsWith("lastUpdate")) return;
                this.model1[cell.Row, "lastUpdatePersonId"] = GlobalData.Person["id"];
                this.model1[cell.Row, "lastUpdatePersonName"] = GlobalData.Person["name"];
                this.model1[cell.Row, "lastUpdateTime"] = DateTime.Now;
            }
        }

        public List<IDictionary<string, object>> AllSuppliers { get => allSuppliers; set => allSuppliers = value; }

        private void RefreshAllSuppliers()
        {
            this.allSuppliers = RestClient.Get<List<IDictionary<string, object>>>(
               $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/supplier/{{}}");
            if (this.allSuppliers == null) this.Close();
        }

        //添加按钮点击事件
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0,new Dictionary<string, object>()
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
            this.model1.RemoveSelectedRows();
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
        }

        //供应商名称输入联想
        private object[] SupplierNameAssociation(string str)
        {
            return (from s in this.allSuppliers
                    where s["name"] != null && s["name"].ToString().StartsWith(str)
                    select s["name"]).ToArray();
        }

        //供应商代号输入联想
        private object[] SupplierNoAssociation(string str)
        {
            return (from s in this.allSuppliers
                    where s["no"] != null && s["no"].ToString().StartsWith(str)
                    select s["no"]).ToArray();
        }
        
        //供应商名称编辑完成，根据名称自动搜索ID和No
        private void SupplierNameEditEnded(int row, string supplierName)
        {
            IDictionary<string, object> foundSupplier =
                this.AllSuppliers.Find((s) =>
                    {
                        if (s["name"] == null) return false;
                        return s["name"].ToString() == supplierName;
                    });
            if(foundSupplier == null)
            {
                MessageBox.Show($"供应商\"{supplierName}\"不存在，请重新填写","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "supplierId"] = foundSupplier["id"];
                this.model1[row, "supplierNo"] = foundSupplier["no"];
            }
        }

        //供应商代号编辑完成，根据名称自动搜索ID和名称
        private void SupplierNoEditEnded(int row, string supplierName)
        {
            IDictionary<string, object> foundSupplier =
                this.AllSuppliers.Find((s) =>
                {
                    if (s["no"] == null) return false;
                    return s["no"].ToString() == supplierName;
                });
            if (foundSupplier == null)
            {
                MessageBox.Show($"供应商\"{supplierName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "supplierId"] = foundSupplier["id"];
                this.model1[row, "supplierNo"] = foundSupplier["no"];
            }
        }

    }
}

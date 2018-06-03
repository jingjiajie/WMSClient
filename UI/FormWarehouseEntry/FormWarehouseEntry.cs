using FrontWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace WMS.UI
{
    public partial class FormWarehouseEntry : Form
    {
        Action<int[]> ToInspectionNoteCallback = null;
        public FormWarehouseEntry(Action<int[]> toInspectionNoteCallback)
        {
            this.ToInspectionNoteCallback = toInspectionNoteCallback;
            MethodListenerContainer.Register(this);
            InitializeComponent();
            this.model1.CellUpdated += this.model_CellUpdated;
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
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }

        //保存按钮点击事件
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
            }
        }

        private void FormWarehouseEntry_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url",Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            this.searchView1.Search();
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
                GlobalData.AllSuppliers.Find((s) =>
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
                this.model1[row, "supplierName"] = foundSupplier["name"];
            }
        }

        private void buttonInspect_Click(object sender, EventArgs e)
        {
            var selectionRange = this.model1.SelectionRange;
            if(selectionRange == null)
            {
                MessageBox.Show("请选择要生成送检单的入库单！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var warehouseEntries = this.model1.GetRows(Util.Range(selectionRange.Row, selectionRange.Row + selectionRange.Rows));
            new FormWarehouseEntryInspect(warehouseEntries, this.ToInspectionNoteCallback, () => this.searchView1.Search()).Show();
        }

        private void buttonItems_Click(object sender, EventArgs e)
        {
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项入库单查看物料条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row })[0];
            new FormWarehouseEntryItem(rowData).Show();
        }

        private string StateForwardMapper(int state)
        {
            //0待入库 1送检中 2.全部入库 3.部分入库
            switch (state)
            {
                case 0:return "待入库";
                case 1:return "送检中";
                case 2:return "全部入库";
                case 3:return "部分入库";
                default:return "未知状态";
            }
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            if(this.model1.SelectionRange == null)
            {
                MessageBox.Show("请选择要预览的入库单！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            List<int> ids = new List<int>();
            for(int i = 0; i < this.model1.SelectionRange.Rows; i++)
            {
                int curRow = this.model1.SelectionRange.Row + i;
                if (this.model1[curRow, "id"] == null) continue;
                ids.Add((int)this.model1[curRow, "id"]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string strIDs = serializer.Serialize(ids);
            var previewData = RestClient.Get<List<IDictionary<string,object>>>(Defines.ServerURL + "/warehouse/WMS_Template/warehouse_entry/preview/" + strIDs);
            if (previewData == null) return;
            StandardFormPreviewExcel formPreviewExcel = new StandardFormPreviewExcel("入库单预览");
            foreach (IDictionary<string,object> entryAndItem in previewData)
            {
                IDictionary<string, object> warehouseEntry = (IDictionary<string, object>)entryAndItem["warehouseEntry"];
                object[] warehouseEntryItems = (object[])entryAndItem["warehouseEntryItems"];
                string no = (string)warehouseEntry["no"];
                if (!formPreviewExcel.AddPatternTable("Excel/WarehouseEntry.xlsx", no)) return;
                formPreviewExcel.AddData("warehouseEntry",warehouseEntry,no);
                formPreviewExcel.AddData("warehouseEntryItems", warehouseEntryItems,no);
            }
            formPreviewExcel.Show();
        }

        private void toolStripTop_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void PutIn(bool qualified)
        {
            //获取选中行ID，过滤掉新建的行（ID为0的）
            int[] selectedIDs = this.model1.GetSelectedRows<int>("id").Except(new int[] { 0 }).ToArray();
            if (selectedIDs.Length == 0)
            {
                MessageBox.Show("请选择一项进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string strIDs = serializer.Serialize(selectedIDs);
            try
            {
                string operatioName = qualified ? "receive" : "reject";
                RestClient.RequestPost<string>(Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/warehouse_entry/" + operatioName, strIDs, "POST");
                this.searchView1.Search();
                MessageBox.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show((qualified ? "直接正品入库" : "直接不良品入库") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonReceive_Click(object sender, EventArgs e)
        {
            this.PutIn(true);
        }

        private void buttonReject_Click(object sender, EventArgs e)
        {
            this.PutIn(false);
        }
    }
}

using FrontWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.Script.Serialization;

namespace WMS.UI.FormBasicInfos
{
    public partial class FormSupplier : Form
    {
        public FormSupplier()
        {
           MethodListenerContainer.Register("FormSupplier", this);
           InitializeComponent();
           this.model1.CellUpdated+= this.model_CellUpdated;

        }

        //private List<int> rowChange = new List<int>();

        private void model_CellUpdated(object sender, ModelCellUpdatedEventArgs e)
        {            
            foreach (var cell in e.UpdatedCells)
            {          
                if (cell.ColumnName.StartsWith("lastUpdate")) return;
                this.model1[cell.Row, "lastUpdatePersonId"] = GlobalData.Person["id"];
                this.model1[cell.Row, "lastUpdatePersonName"] = GlobalData.Person["name"];
                this.model1[cell.Row, "lastUpdateTime"] = DateTime.Now;                
                //if (!rowChange.Contains(cell.Row))
                //{
               //     rowChange.Add(cell.Row);
                //}                
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

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "createPersonId",GlobalData.Person["id"]},
                { "createPersonName",GlobalData.Person["name"]},
                { "warehouseName",GlobalData.Warehouse["name"]},
                { "createTime",DateTime.Now}
            });        
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            bool update = false;

            for (int i = 0; i < this.model1.RowCount - 1; i++)
            {                             
                if (this.model1.GetRowSynchronizationState(i) == SynchronizationState.UPDATED)
                {
                    update = true;
                    break;
                }
            }
            if (update == true)
            {
                if (MessageBox.Show("是否保留历史信息？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.synchronizer.UpdateAPI.SetRequestParameter("$save", "history_save");
                }
                else
                {
                    this.synchronizer.UpdateAPI.SetRequestParameter("$save", "");
                }
            }
           
            if (this.synchronizer.Save())
                {
                    this.searchView1.Search();
                    Condition condWarehouse = new Condition().AddCondition("warehouseId", GlobalData.Warehouse["id"]);
                    GlobalData.AllSuppliers = RestClient.Get<List<IDictionary<string, object>>>(
                       $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/supplier/{condWarehouse.ToString()}/new");
                }
                // }
                /* else
                 //{
                     int[] row = rowChange.ToArray();
                     var rowData = this.model1.GetRows(row);
                     string json=(new JavaScriptSerializer()).Serialize(rowData);

                 }*/
            }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }

        private void FormSupplier_Load(object sender, EventArgs e)
        {
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            this.synchronizer.FindAPI.SetRequestParameter("$history","new");
            this.synchronizer.GetCountAPI.SetRequestParameter("$history", "new");
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private void ButtonFindHistory_Click(object sender, EventArgs e)
        {                 
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项查看历史信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row })[0];
            FormSupplierHistory form = new FormSupplierHistory((int)rowData["id"]);
            form.Show();
            //this.searchView1.Search();         
        }

        private void toolStripButtonFindAll_Click(object sender, EventArgs e)
        {
            this.synchronizer.FindAPI.SetRequestParameter("$history", "new");
            this.synchronizer.GetCountAPI.SetRequestParameter("$history", "new");
            this.basicView1.Enabled = true;
            this.searchView1.Visible = true;
            this.toolStripButtonAdd.Visible = true;
            this.toolStripButtonAlter.Visible = true;
            this.toolStripButtonDelete.Visible = true;
            this.ButtonFindHistory.Visible = true;
            this.toolStripButtonFindAll.Visible = false;
            this.searchView1.ClearStaticCondition("newestSupplierId");
            this.searchView1.Search();
        }
    }
}

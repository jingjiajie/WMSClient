using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontWork;

namespace WMS.UI
{
    public partial class FormWarehouseEntryInspect : Form
    {
        public FormWarehouseEntryInspect(IDictionary<String,object>[] warehouseEntries)
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.modelWarehouseEntry.AddRows(warehouseEntries);
        }

        private void FormWarehouseEntryInspect_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        private int WarehouseIdDefaultValue()
        {
            return (int)GlobalData.Warehouse["id"];
        }

        private int CreatePersonIdDefaultValue()
        {
            return (int)GlobalData.Person["id"];
        }

        private DateTime? InspectionTimeDefaultValue()
        {
            int row = (int)this.modelWarehouseEntry.SelectionRange.Row;
            DateTime? inventoryDate = this.modelWarehouseEntry[row, "inventoryDate"] as DateTime?;
            return inventoryDate;
        }

        private void modelWarehouseEntry_SelectionRangeChanged(object sender, FrontWork.ModelSelectionRangeChangedEventArgs e)
        {
            int selectedWarehouseEntryID = (int)this.modelWarehouseEntry[this.modelWarehouseEntry.SelectionRange.Row, "id"];
            this.modelBoxInspectionNoteItems.CurrentModelName = selectedWarehouseEntryID.ToString();
            Condition condition = new Condition()
                .AddCondition("warehouseEntryId", selectedWarehouseEntryID);
            this.synchronizer.SetRequestParameter("$condStr", condition.ToString());
            if (this.synchronizer.Find() == false)
            {
                MessageBox.Show("加载物料条目失败，请检查网络连接！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.ModelInspectionNoteAddOrSelect(selectedWarehouseEntryID);
        }

        /// <summary>
        /// 如果包含入库单对应送检单，则选中行，否则新建行
        /// </summary>
        /// <param name="warehouseEntryID">入库单ID</param>
        private void ModelInspectionNoteAddOrSelect(int warehouseEntryID)
        {
            DataTable dataTable = this.modelInspectionNotes.GetDataTable();
            for(int row = 0; row < dataTable.Rows.Count; row++)
            {
                int curID = (int)dataTable.Rows[row]["warehouseEntryId"];
                if(curID == warehouseEntryID)
                {
                    this.modelInspectionNotes.SelectionRange = new Range(row,0,1,this.modelInspectionNotes.ColumnCount);
                    return;
                }
            }
            this.modelInspectionNotes.AddRow(new Dictionary<string, object>()
            {
                { "warehouseEntryId",warehouseEntryID}
            });
        }
    }
}

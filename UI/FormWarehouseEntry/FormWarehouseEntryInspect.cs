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

            Condition condition = new Condition()
    .AddCondition("warehouseEntryId", warehouseEntries.Select((item) => item["id"]).ToArray(), ConditionItemRelation.IN);
            this.synchronizer.SetRequestParameter("$condStr", condition.ToString());
            if (this.synchronizer.Find() == false)
            {
                MessageBox.Show("加载物料条目失败，请检查网络连接！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.modelBoxInspectionNoteItems.GroupBy("warehouseEntryNo");
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

        private string CreatePersonNameDefaultValue()
        {
            return (string)GlobalData.Person["name"];
        }

        private DateTime? InspectionTimeDefaultValue()
        {
            int row = (int)this.modelWarehouseEntry.SelectionRange.Row;
            DateTime? createTime = this.modelWarehouseEntry[row, "createTime"] as DateTime?;
            return createTime;
        }

        private void modelWarehouseEntry_SelectionRangeChanged(object sender, FrontWork.ModelSelectionRangeChangedEventArgs e)
        {
            string selectedWarehouseEntryNo = (string)this.modelWarehouseEntry[this.modelWarehouseEntry.SelectionRange.Row, "no"];
            this.modelBoxInspectionNoteItems.CurrentModelName = selectedWarehouseEntryNo;

            this.ModelInspectionNoteAddOrSelect(selectedWarehouseEntryNo);
        }

        /// <summary>
        /// 如果包含入库单对应送检单，则选中行，否则新建行
        /// </summary>
        /// <param name="warehouseEntryID">入库单ID</param>
        private void ModelInspectionNoteAddOrSelect(string warehouseEntryNo)
        {
            DataTable dataTable = this.modelInspectionNotes.GetDataTable();
            for(int row = 0; row < dataTable.Rows.Count; row++)
            {
                string curNo = (string)dataTable.Rows[row]["warehouseEntryNo"];
                if (curNo == warehouseEntryNo)
                {
                    this.modelInspectionNotes.SelectionRange = new Range(row,0,1,this.modelInspectionNotes.ColumnCount);
                    return;
                }
            }
            this.modelInspectionNotes.AddRow(new Dictionary<string, object>()
            {
                { "warehouseEntryNo",warehouseEntryNo}
            });
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            for (int row = 0; row < this.modelWarehouseEntry.RowCount; row++)
            {
                int warehouseEntryID = (int)this.modelWarehouseEntry[row, "id"];

            }
        }

        private void configurationWarehouseEntry_Load(object sender, EventArgs e)
        {

        }
    }
}

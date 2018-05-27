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
        private IDictionary<string, object>[] warehouseEntries = null;

        public FormWarehouseEntryInspect(IDictionary<String,object>[] warehouseEntries)
        {
            MethodListenerContainer.Register(this);
            this.warehouseEntries = warehouseEntries;
            InitializeComponent();
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);

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
            string selectedWarehouseEntryNo = (string)this.modelBoxInspectionNoteItems.CurrentModelName;
            var warehouseEntry = this.warehouseEntries.Where((item) => (string)item["no"] == selectedWarehouseEntryNo).First();
            DateTime? createTime = warehouseEntry["createTime"] as DateTime?;
            return createTime;
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

        }

        private void configurationWarehouseEntry_Load(object sender, EventArgs e)
        {

        }
    }
}

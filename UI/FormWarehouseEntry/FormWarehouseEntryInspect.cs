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
    public partial class FormWarehouseEntryInspect : Form
    {
        public FormWarehouseEntryInspect(IDictionary<String,object>[] warehouseEntries)
        {
            InitializeComponent();
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.modelWarehouseEntry.AddRows(warehouseEntries);
        }

        private void FormWarehouseEntryInspect_Load(object sender, EventArgs e)
        {

        }

        private int WarehouseIdDefaultValue()
        {
            return (int)GlobalData.Warehouse["Id"];
        }

        private int CreatePersonIdDefaultValue()
        {
            return (int)GlobalData.Person["Id"];
        }

        private DateTime InspectionTimeDefaultValue()
        {
            int row = (int)this.modelWarehouseEntry.SelectionRange.Row;
            DateTime inventoryDate = (DateTime)this.modelWarehouseEntry[row, "inventoryDate"];
            return inventoryDate;
        }

        private void modelWarehouseEntry_SelectionRangeChanged(object sender, FrontWork.ModelSelectionRangeChangedEventArgs e)
        {
            int selectedWarehouseEntryID = (int)this.modelWarehouseEntry[this.modelWarehouseEntry.SelectionRange.Row, "id"];
            this.modelBoxInspectionNoteItems.CurrentModelName = selectedWarehouseEntryID.ToString();
            Condition condition = new Condition()
                .AddCondition("warehouseEntryId", selectedWarehouseEntryID);
            this.synchronizer.SetRequestParameter("$condStr", condition.ToString());
            this.synchronizer.Find();
        }
    }
}

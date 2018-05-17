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
            this.modelWarehouseEntryNo.SelectionRangeChanged += ModelWarehouseEntryNo_SelectionRangeChanged;
            this.modelWarehouseEntryNo.AddRows(warehouseEntries);
        }

        private void ModelWarehouseEntryNo_SelectionRangeChanged(object sender, FrontWork.ModelSelectionRangeChangedEventArgs e)
        {
            int selectedWarehouseEntryID = (int)this.modelWarehouseEntryNo[this.modelWarehouseEntryNo.SelectionRange.Row, "id"];
            this.modelBoxInspectionNoteItems.CurrentModelName = selectedWarehouseEntryID.ToString();
            Condition condition = new Condition()
                .AddCondition("warehouseEntryId", selectedWarehouseEntryID);
            this.synchronizer.SetRequestParameter("$condStr",condition.ToString());
            this.synchronizer.Find();
        }

        private void FormWarehouseEntryInspect_Load(object sender, EventArgs e)
        {

        }
    }
}

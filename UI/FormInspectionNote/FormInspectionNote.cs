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
    public partial class FormInspectionNote : Form
    {
        private int[] initialSelectedIDs = null;
        public FormInspectionNote(int[] initialSelectedIDs = null)
        {
            MethodListenerContainer.Register(this);
            this.initialSelectedIDs = initialSelectedIDs;
            InitializeComponent();
        }

        private void FormInspectionNote_Load(object sender, EventArgs e)
        {
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            this.searchView1.Search();
            if (initialSelectedIDs != null)
            {
                this.model1.SelectRowsByValues("id", initialSelectedIDs);
            }
        }

        private string StateForwardMapper(int state)
        {
            switch (state)
            {
                case 0: return "送检中";
                case 1: return "部分送检完成";
                case 2: return "全部送检完成";
                default: return "未知状态";
            }
        }

        private void buttonItems_Click(object sender, EventArgs e)
        {
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项送检单查看物料条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row })[0];
            new FormInspectionNoteItem(rowData).Show();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
            }
        }
    }
}

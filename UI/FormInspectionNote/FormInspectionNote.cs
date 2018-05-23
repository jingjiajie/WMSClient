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
        public FormInspectionNote()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private void FormInspectionNote_Load(object sender, EventArgs e)
        {
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            this.searchView1.Search();
        }

        private string StateForwardMapper(int state)
        {
            switch (state)
            {
                case 0: return "送检中";
                case 1: return "部分送检完成";
                case 2: return "全部送检完成";
                default: throw new Exception("状态错误:" + state);
            }
        }

        private void buttonItems_Click(object sender, EventArgs e)
        {
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项送检单查看物料条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(new long[] { this.model1.SelectionRange.Row })[0];
            new FormInspectionNoteItem(rowData).Show();
        }
    }
}

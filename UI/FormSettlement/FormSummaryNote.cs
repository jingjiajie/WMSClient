using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontWork;

namespace WMS.UI.FormSettlement
{
    public partial class FormSummaryNote : Form
    {
        public FormSummaryNote()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {             
                { "createPersonId",GlobalData.Person["id"]},
                { "createPersonName",GlobalData.Person["name"]},
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "warehouseName",GlobalData.Warehouse["name"]},
                { "createTime",DateTime.Now},
            });
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save()) { this.searchView1.Search(); }
        }

        private void buttonItems_Click(object sender, EventArgs e)
        {
            if (this.model1.SelectionRange==null) { return; }
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项汇总单查看盘点单条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row })[0];
            var a1 = new FormSummaryNoteItem(rowData);
            a1.Show();
        }

        private void FormSummaryNote_Load(object sender, EventArgs e)
        {
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FormTrayThresholds form = new FormTrayThresholds();
            form.Show();
        }
    }
}

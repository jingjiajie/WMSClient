using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI.FromSalary
{
    public partial class FormPayNoteItem : Form
    {
        private int payNoteId;
        public FormPayNoteItem(int payNoteId)
        {
            InitializeComponent();
            this.payNoteId = payNoteId;
        }

        private void FormPayNoteItem_Load(object sender, EventArgs e)
        {
            this.searchView1.AddStaticCondition("payNoteId", payNoteId);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {               
                { "personId",GlobalData.Person["id"]},
                { "personName",GlobalData.Person["name"]},
                { "payNoteId",payNoteId},
            });
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
            }
        }
    }
}

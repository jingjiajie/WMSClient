using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI.FormAcccount
{
    public partial class FormTax : Form
    {
        public FormTax()
        {
            InitializeComponent();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
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
                GlobalData.AllTax = RestClient.Get<List<IDictionary<string, object>>>(
            $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/tax/{{}}");
            }
        }

        private void FormTax_Load(object sender, EventArgs e)
        {
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private void buttonItems_Click(object sender, EventArgs e)
        {
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项税务查看税务详情页！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var tax = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row })[0];
            if (tax["id"] == null)
            {
                MessageBox.Show("请先保存单据再查看税务详情页！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            new FormTaxItem(tax).Show();
        }
    }
}

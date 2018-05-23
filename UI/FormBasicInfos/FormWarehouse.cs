using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI.FormBasicInfos
{
    public partial class FormWarehouse : Form
    {
        public FormWarehouse()
        {
            InitializeComponent();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, null);

        }

        private void FormWarehouse_QueryAccessibilityHelp(object sender, QueryAccessibilityHelpEventArgs e)
        {

        }



        private void FormWarehouse_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
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
                GlobalData.AllSuppliers = RestClient.Get<List<IDictionary<string, object>>>(
                   $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/warehouse/{{}}");
            }
        }
    }
}

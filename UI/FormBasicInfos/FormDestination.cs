using FrontWork;
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
    public partial class FormDestination : Form
    {
        public FormDestination()
        {
            MethodListenerContainer.Register(this);
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
            if (this.jsonRESTSynchronizer1.Save())
            {
                this.searchView1.Search();
                Condition condition = new Condition();
                GlobalData.AllDestinations = RestClient.Get<List<IDictionary<string, object>>>(
                   $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/destination/{condition.ToString()}");
            }
        }

        private void FormDestination_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.jsonRESTSynchronizer1.SetRequestParameter("$url", Defines.ServerURL);
            this.jsonRESTSynchronizer1.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }
    }
}

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
    public partial class FormMaterial : Form
    {
        public FormMaterial()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private void FormMaterial_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.jsonRESTSynchronizer1.SetRequestParameter("$url", Defines.ServerURL);
            this.jsonRESTSynchronizer1.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            this.searchView1.Search();
        }

        private void model_CellUpdated(object sender, ModelCellUpdatedEventArgs e)
        {

        }


        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "warehouseName",GlobalData.Warehouse["name"]},
                { "enabled",1}
            });
        }

        private void configuration1_Load(object sender, EventArgs e)
        {

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
            }
        }

    }
}

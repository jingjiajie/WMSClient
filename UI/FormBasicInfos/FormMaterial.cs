using FrontWork;
using Microsoft.VisualBasic;
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

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            string s = Interaction.InputBox("请输入需要添加的行数", "提示", "1", -1, -1);  //-1表示在屏幕的中间         
            int row = 1;
            try
            {
                row = Convert.ToInt32(s);
            }
            catch
            {
                MessageBox.Show("请输入正确的数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            for (int i = 0; i < row; i++)
            {
                this.model1.InsertRow(0, new Dictionary<string, object>()
                {
                    { "warehouseId",GlobalData.Warehouse["id"]}
                });
            }
        }

        private int WarehouseIdDefaultValue()
        {
            return (int)GlobalData.Warehouse["id"];
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
                Condition condWarehouse = new Condition().AddCondition("warehouseId", GlobalData.Warehouse["id"]);
                GlobalData.AllMaterials = RestClient.Get<List<IDictionary<string, object>>>(
                   $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/material/{condWarehouse.ToString()}");
                GlobalData.AllSupplies = RestClient.Get<List<IDictionary<string, object>>>(
              $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/supply/{condWarehouse.ToString()}");
            }
        }

    }
}

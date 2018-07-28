using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontWork;

namespace WMS.UI.FormBasicInfos
{
    public partial class FormWarehouse : Form
    {
        private ComboBox comboBoxWarehouse;
        private Panel panelRight;
        private TreeView treeViewLeft;
        public FormWarehouse(ComboBox comboBoxWarehouse, Panel panelRight, TreeView treeViewLeft)
        {
            MethodListenerContainer.Register("FormWarehouse", this);
            InitializeComponent();
            this.model1.RowRemoved += this.model_RowRemoved;
            this.model1.Refreshed += this.model_Refreshed;
            this.comboBoxWarehouse = comboBoxWarehouse;
            this.panelRight = panelRight;
            this.treeViewLeft = treeViewLeft;
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {          
                { "enable",1}
            });
        }

        private void FormWarehouse_QueryAccessibilityHelp(object sender, QueryAccessibilityHelpEventArgs e)
        {

        }

        private void model_Refreshed(object sender, ModelRefreshedEventArgs e)
        {
            this.updateBasicAndReoGridView();
        }

        private void updateBasicAndReoGridView()
        {

            if (this.model1.RowCount == 0)
            {
                this.basicView1.Enabled = false;
                this.reoGridView1.Enabled = false;
            }
            else
            {
                this.basicView1.Enabled = true;
                this.reoGridView1.Enabled = true;
            }

        }

        //private List<int> rowChange = new List<int>();
        private void model_RowRemoved(object sender, ModelRowRemovedEventArgs e)
        {
            this.updateBasicAndReoGridView();
        }



        private void FormWarehouse_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
            this.updateBasicAndReoGridView();
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row });
            foreach (var date in rowData)
            {              
                if (((int?)date["id"]??-1) == (int)GlobalData.Warehouse["id"])
                {
                    MessageBox.Show("无法删除正在查看的仓库!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            }
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
            GlobalData.AllWarehouses = RestClient.Get<List<IDictionary<string, object>>>(Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/warehouse/" +
            new Condition().AddOrder("name"));
            this.comboBoxWarehouse.Items.Clear();
            this.comboBoxWarehouse.Items.AddRange((from item in GlobalData.AllWarehouses
                                                   select new ComboBoxItem(item["name"]?.ToString(), item)).ToArray());
            for (int i = 0; i < this.comboBoxWarehouse.Items.Count; i++)
            {
                if ((int)GlobalData.AllWarehouses[i]["id"] == (int)GlobalData.Warehouse["id"])
                {
                    this.comboBoxWarehouse.Text = (string)GlobalData.AllWarehouses[i]["name"];
                }
            }
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {

            if (this.synchronizer.Save())
            {
                this.searchView1.Search();           
                GlobalData.AllWarehouses = RestClient.Get<List<IDictionary<string, object>>>(Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/warehouse/" +
                new Condition().AddOrder("name"));
                this.comboBoxWarehouse.Items.Clear();           
                this.comboBoxWarehouse.Items.AddRange((from item in GlobalData.AllWarehouses
                                                       select new ComboBoxItem(item["name"]?.ToString(), item)).ToArray());             
                for (int i = 0; i < this.comboBoxWarehouse.Items.Count; i++)
                {                  
                    if ((int)GlobalData.AllWarehouses[i]["id"] == (int)GlobalData.Warehouse["id"])
                    {                       
                        this.comboBoxWarehouse.Text = (string)GlobalData.AllWarehouses[i]["name"];                     
                    }
                }

            }
        }



        private string EnableForwardMapper([Data]int state)
        {
            switch (state)
            {
                case 0: return "禁用";
                case 1: return "启用";
                default: return "未知状态";
            }
        }

        private int EnableBackwardMapper([Data]string enable)
        {
            switch (enable)
            {
                case "禁用": return 0;
                case "启用": return 1;
                default: return -1;
            }
        }

    }
}

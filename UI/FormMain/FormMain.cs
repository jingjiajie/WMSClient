using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using WMS.UI.FormBasicInfos;
using WMS.UI.FormStockTaking;
using WMS.UI.FormStock;
using WMS.UI.FromDeliverOrder;
using WMS;

namespace WMS.UI
{
    public partial class FormMain : Form
    {
        private Action formClosedCallback;
        private FormInspectionNote formInspectionNoteInstance = new FormInspectionNote();

        public FormMain()
        {
            InitializeComponent();
        }

        public void SetFormClosedCallback(Action callback)
        {
            this.formClosedCallback = callback;
        }

        private void RefreshTreeView()
        {
            TreeNode[] treeNodes = new TreeNode[]
            {
                MakeTreeNode("基本信息",null,new TreeNode[]{
                    MakeTreeNode("用户管理", new FormPerson()),
                    MakeTreeNode("供应商管理", new FormSupplier()),
                    MakeTreeNode("物料管理", new FormMaterial()),
                    MakeTreeNode("仓库管理", new FormWarehouse(this.comboBoxWarehouse,this.panelRight,this.treeViewLeft)),
                    MakeTreeNode("库区管理", new FormStorageArea()),
                    MakeTreeNode("库位管理", new FormStorageLocation()),
                    MakeTreeNode("供货管理", new FormSupply()),
                    MakeTreeNode("发货套餐管理", new FormPackage()),
                    MakeTreeNode("上架库存设置", new FormSafetyStock(0)),
                    MakeTreeNode("备货库存设置", new FormSafetyStock(1))
                    }),
                MakeTreeNode("入库管理", null, new TreeNode[]{
                    MakeTreeNode("入库单管理", new FormWarehouseEntry(ToInspectionNoteSelectIDsCallback, ToInspectionNoteSearchNoCallback)),
                    MakeTreeNode("送检单管理", formInspectionNoteInstance),
                    MakeTreeNode("上架单管理", new FormPutAwayNote())
                    }),
                MakeTreeNode("发货管理", null, new TreeNode[]{
                    MakeTreeNode("备货作业单管理", new FormTransferOrder.FormTransferOrder()),
                    MakeTreeNode("出库单管理", new FormDeliverOrder())

                    }),
                MakeTreeNode("库存管理", null, new TreeNode[]{
                    MakeTreeNode("库存批次", new FormStockRecord()),
                    MakeTreeNode("库存盘点", new FormStockTakingOrder()),
                    MakeTreeNode("移位记录", new FormTransferRecord())
                    }),
                 MakeTreeNode("薪金管理",null ,new TreeNode[]{
                    MakeTreeNode("薪金类别", new FromSalary.FormSalaryType()),
                    MakeTreeNode("薪金期间", new FromSalary.FormSalaryPeriod()),
                    MakeTreeNode("人员薪金", new FromSalary.FormPersonSalary()),
                    MakeTreeNode("薪资发放单", new FromSalary.FormPayNote())
                    }),
                 MakeTreeNode("总账管理", null, new TreeNode[]{
                    MakeTreeNode("科目管理", new FormAcccount.FormAccountTitle()),
                    MakeTreeNode("税务管理", new FormAcccount.FormTax()),
                    MakeTreeNode("账目记录", new FormAcccount.FormAccountRecord()),
                    MakeTreeNode("会计期间", new FormAcccount.FormAccountPeriod())
                    }),
                 MakeTreeNode("结算管理",null, new TreeNode[]{
                   MakeTreeNode("汇总单管理", new FormSettlement.FormSummaryNote()),
                   MakeTreeNode("结算单管理", new FormSettlement.FormSettlementNote()),
                   MakeTreeNode("发票管理",  new FormSettlement.FormInvoice()),
                   MakeTreeNode("价格管理",  new FormSettlement.FormPrice())
                    })
            };

            this.treeViewLeft.Nodes.Clear();
            TreeNode[] nodes = treeNodes.ToArray();
            this.treeViewLeft.Nodes.AddRange(nodes);
        }

        private static TreeNode MakeTreeNode(string text, Form form , TreeNode[] subNodes = null)
        {
            TreeNode node = new TreeNode()
            {
                Text = text,
                Tag = new TreeNodeTag(form)
            };
            if (subNodes == null)
            {
                return node;
            }
            foreach (TreeNode subNode in subNodes)
            {
                node.Nodes.Add(subNode);
            }
            return node;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //刷新左边树形框
            this.RefreshTreeView();

            //刷新顶部
            this.labelUsername.Text = GlobalData.Person["name"].ToString();
            this.labelAuth.Text = GlobalData.Person["role"].ToString() + " :";

            //窗体大小根据显示屏改变
            int DeskWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int DeskHeight = Screen.PrimaryScreen.WorkingArea.Height;
            this.Width = Convert.ToInt32(DeskWidth * 0.8);
            this.Height = Convert.ToInt32(DeskHeight * 0.8);

            //刷新仓库
            this.comboBoxWarehouse.Items.AddRange((from item in GlobalData.AllWarehouses
                                                   select new ComboBoxItem(item["name"]?.ToString(),item)).ToArray());
            for(int i = 0; i < this.comboBoxWarehouse.Items.Count; i++)
            {
                if(GlobalData.AllWarehouses[i] == GlobalData.Warehouse)
                {
                    this.comboBoxWarehouse.SelectedIndexChanged -= this.comboBoxWarehouse_SelectedIndexChanged;
                    this.comboBoxWarehouse.SelectedIndex = i;
                    this.comboBoxWarehouse.SelectedIndexChanged += this.comboBoxWarehouse_SelectedIndexChanged;
                }
            }
        }

        private void treeViewLeft_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNodeTag tag = treeViewLeft.SelectedNode.Tag as TreeNodeTag;
            if (tag.Form != null)
            {
                this.panelRight.Hide();
                this.panelRight.SuspendLayout();
                this.LoadSubWindow(tag.Form);
                this.panelRight.ResumeLayout();
                this.panelRight.Show();
            }
        }

        private void LoadSubWindow(Form form)
        {
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;//窗口大小
            form.FormBorderStyle = FormBorderStyle.None;//没有标题栏
            this.panelRight.Controls.Clear();//清空
            this.panelRight.Controls.Add(form);
            form.Show();
        }


        private void ToInspectionNoteSelectIDsCallback(int[] selectedIDs)
        {
            this.SetTreeViewSelectedNodeByText("送检单管理");
            this.LoadSubWindow(formInspectionNoteInstance);
            formInspectionNoteInstance.SearchAndSelectByIDs(selectedIDs);
        }

        private void ToInspectionNoteSearchNoCallback(string searchNo)
        {
            this.SetTreeViewSelectedNodeByText("送检单管理");
            this.LoadSubWindow(formInspectionNoteInstance);
            formInspectionNoteInstance.SearchByWarehouseEntryNo(searchNo);
        }
        
        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.formClosedCallback?.Invoke();
        }

        private void comboBoxWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalData.Warehouse = ((ComboBoxItem)this.comboBoxWarehouse.SelectedItem).Value as IDictionary<string,object>;
            Condition condSalaryPeriod = new Condition().AddCondition("warehouseId", GlobalData.Warehouse["id"]);
            this.panelRight.Controls.Clear();
            this.treeViewLeft.SelectedNode = null;
            Condition condWarehouse = new Condition().AddCondition("warehouseId", GlobalData.Warehouse["id"]);
            GlobalData.AllSuppliers = RestClient.Get<List<IDictionary<string, object>>>(
               $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/supplier/{condWarehouse.ToString()}/new");

            GlobalData.AllMaterials = RestClient.Get<List<IDictionary<string, object>>>(
               $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/material/{condWarehouse.ToString()}");

            GlobalData.AllSupplies = RestClient.Get<List<IDictionary<string, object>>>(
                $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/supply/{condWarehouse.ToString()}");

            GlobalData.AllStorageLocations = RestClient.Get<List<IDictionary<string, object>>>(
               $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/storage_location/{condWarehouse.ToString()}");

            GlobalData.AllStorageAreas = RestClient.Get<List<IDictionary<string, object>>>(
               $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/storage_area/{condWarehouse.ToString()}");

            GlobalData.AllPersons = RestClient.Get<List<IDictionary<string, object>>>(
               $"{Defines.ServerURL}/ledger/{GlobalData.AccountBook}/person/{{}}");

            GlobalData.AllPackage = RestClient.Get<List<IDictionary<string, object>>>(
              $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/package/{condWarehouse.ToString()}");

            GlobalData.AllSalaryItem = RestClient.Get<List<IDictionary<string, object>>>(
            $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/salary_item/{condWarehouse.ToString()}");

            GlobalData.AllSalaryType = RestClient.Get<List<IDictionary<string, object>>>(
             $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/salary_type/{condWarehouse.ToString()}");

            GlobalData.AllSummaryNote = RestClient.Get<List<IDictionary<string, object>>>(
             $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/summary_note/{condWarehouse.ToString()}");

            GlobalData.AllSalaryPeriod = RestClient.Get<List<IDictionary<string, object>>>(
            $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/salary_period/{condSalaryPeriod.AddOrder("endTime", OrderItemOrder.DESC).ToString()}");

            GlobalData.AllAccountPeriod = RestClient.Get<List<IDictionary<string, object>>>(
              $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/account_period/{condWarehouse.AddOrder("startTime", OrderItemOrder.DESC).ToString()}");

            try
            {
                GlobalData.AccountPeriod = RestClient.Get<List<IDictionary<string, object>>>(
                   $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/account_period/{condWarehouse.AddCondition("ended", 0).AddOrder("startTime", OrderItemOrder.DESC).ToString()}")[0];
            }
            catch { GlobalData.AccountPeriod = null; }
            GlobalData.REMAINDENABLE = true;
            //刷新左边树形框
            this.RefreshTreeView();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确定退出吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void SetTreeViewSelectedNodeByText(string text)
        {
            TreeNode node = this.FindTreeNodeByText(this.treeViewLeft.Nodes, text);
            if (node == null)
            {
                throw new Exception("树形框中不包含节点：" + text);
            }
            this.treeViewLeft.AfterSelect -= this.treeViewLeft_AfterSelect;
            this.treeViewLeft.SelectedNode = node;
            this.treeViewLeft.AfterSelect += this.treeViewLeft_AfterSelect;
        }

        private TreeNode FindTreeNodeByText(TreeNodeCollection nodes, string text)
        {
            if (nodes.Count == 0)
            {
                return null;
            }
            foreach (TreeNode curNode in nodes)
            {
                if (curNode.Text == text)
                {
                    return curNode;
                }
                TreeNode foundNode = FindTreeNodeByText(curNode.Nodes, text);
                if (foundNode != null)
                {
                    return foundNode;
                }
            }
            return null;
        }




        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            //if (this.WindowState == FormWindowState.Minimized)
            //{

            //    FormSupplyRemind.HideForm();
            //}
            //else if (this.WindowState == FormWindowState.Maximized && this.button2.Visible == false)
            //{
            //    //FormSupplyRemind.RemindStockinfo();
            //    FormSupplyRemind.ShowForm();
            //}
        }


        private void button1_Click(object sender, EventArgs e)
        {
           ////FormSupplyRemind.RemindStockinfo();
           //FormSupplyRemind.RemindStockinfoClick();
           ////this.button2.Visible = false;
           //this.Run = true;
           //this.Run1 = true;         
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_q;
        }

    }
}

class TreeNodeTag
{
    public TreeNodeTag(Form form)
    {
        Form = form;
    }

    public Form Form { get; set; }
}
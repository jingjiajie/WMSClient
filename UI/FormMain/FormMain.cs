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

namespace WMS.UI
{
    public partial class FormMain : Form
    {
        private Action formClosedCallback;

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
                MakeTreeNode("基本信息",new TreeNode[]{
                    MakeTreeNode("用户管理"),
                    MakeTreeNode("供应商管理"),
                    MakeTreeNode("物料管理"),
                    MakeTreeNode("仓库管理"),
                    MakeTreeNode("库区管理"),
                    MakeTreeNode("库位管理"),
                    MakeTreeNode("供货管理"),
                    MakeTreeNode("发货套餐管理"),
                    MakeTreeNode("上架库存设置"),
                    MakeTreeNode("备货库存设置")                    
                    }),
                MakeTreeNode("入库管理",new TreeNode[]{
                    MakeTreeNode("入库单管理"),
                    MakeTreeNode("送检单管理"),
                    MakeTreeNode("上架单管理")
                    }),
                MakeTreeNode("发货管理",new TreeNode[]{
                    MakeTreeNode("备货作业单管理"),
                    MakeTreeNode("出库单管理")
                    //MakeTreeNode("工作任务单管理"),

                    }),
                MakeTreeNode("库存管理",new TreeNode[]{
                    MakeTreeNode("库存批次"),
                    MakeTreeNode("库存盘点"),
                    MakeTreeNode("移位记录")
                    }),
                 MakeTreeNode("薪金管理",new TreeNode[]{
                    MakeTreeNode("薪金类别"),
                    MakeTreeNode("薪金期间"),              
                    MakeTreeNode("人员薪金"),
                    MakeTreeNode("薪资发放单")
                    }),
                 MakeTreeNode("总账管理",new TreeNode[]{
                    MakeTreeNode("科目管理"),
                    MakeTreeNode("税务管理"),
                    MakeTreeNode("账目记录"),
                    MakeTreeNode("会计期间")
                    }),
                 MakeTreeNode("结算管理",new TreeNode[]{
                   MakeTreeNode("汇总单管理"),
                   MakeTreeNode("结算单管理"),
                   MakeTreeNode("发票管理")
                    })
            };

            this.treeViewLeft.Nodes.Clear();
            TreeNode[] nodes = treeNodes.ToArray();
            this.treeViewLeft.Nodes.AddRange(nodes);
        }

        ////检测用户是否有相应功能的权限
        //private bool HasAuthority(string funcName)
        //{
        //    var searchResult = (from fa in FormMainMetaData.FunctionAuthorities
        //                        where fa.FunctionName == funcName
        //                        select fa.Authorities).FirstOrDefault();
        //    if (searchResult == null)
        //    {
        //        return true;
        //    }
        //    Authority[] authorities = searchResult;
        //    foreach (Authority authority in authorities)
        //    {
        //        if (((int)authority & this.user.Authority) == (int)authority)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        ////获取有权限的所有子节点
        //private TreeNode GetAuthenticatedSubTreeNodes(TreeNode node)
        //{
        //    if (HasAuthority(node.Text) == false)
        //    {
        //        return null;
        //    }

        //    TreeNode newNode = (TreeNode)node.Clone();
        //    newNode.Nodes.Clear();

        //    foreach (TreeNode curNode in node.Nodes)
        //    {
        //        if (HasAuthority(curNode.Text))
        //        {
        //            newNode.Nodes.Add(GetAuthenticatedSubTreeNodes(curNode));
        //        }
        //    }
        //    return newNode;
        //}

        private static TreeNode MakeTreeNode(string text, TreeNode[] subNodes = null)
        {
            TreeNode node = new TreeNode() { Text = text };
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
            //new Thread(() =>
            //{
            //    RestClient.Get<List<IDictionary<string,object>>>
            //    //下拉栏显示仓库
            //    var allWarehouses = (from s in wms.Warehouse select s).ToArray();
            //    var allProjects = (from s in wms.Project select s).ToArray();
            //    if (this.IsDisposed)
            //    {
            //        return;
            //    }
            //    this.Invoke(new Action(() =>
            //    {
            //        this.comboBoxWarehouse.Items.AddRange((from w in allWarehouses select new ComboBoxItem(w.Name, w)).ToArray());

            //        for (int i = 0; i < this.comboBoxWarehouse.Items.Count; i++)
            //        {
            //            if (((Warehouse)(((ComboBoxItem)this.comboBoxWarehouse.Items[i]).Value)).ID == this.warehouse.ID)
            //            {
            //                this.comboBoxWarehouse.SelectedIndex = i;
            //                break;
            //            }
            //        }
            //        this.comboBoxProject.Items.AddRange((from p in allProjects select new ComboBoxItem(p.Name, p)).ToArray());
            //        for (int i = 0; i < this.comboBoxWarehouse.Items.Count; i++)
            //        {
            //            if (((Project)(((ComboBoxItem)this.comboBoxProject.Items[i]).Value)).ID == this.project.ID)
            //            {
            //                this.comboBoxProject.SelectedIndex = i;
            //                break;
            //            }
            //        }
            //    }));
            //}).Start();
        }

        private void treeViewLeft_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.panelRight.Hide();
            this.panelRight.SuspendLayout();
            switch (treeViewLeft.SelectedNode.Text)
            {
                case "用户管理":
                    this.LoadSubWindow(new FormPerson());
                    break;
                case "入库单管理":
                    this.LoadSubWindow(new FormWarehouseEntry(ToInspectionNoteSelectIDsCallback, ToInspectionNoteSearchNoCallback));
                    break;
                case "送检单管理":
                    this.LoadSubWindow(new FormInspectionNote());
                    break;
                case "上架单管理":
                    this.LoadSubWindow(new FormPutAwayNote());
                    break;
                case "供应商管理":
                    this.LoadSubWindow(new FormSupplier());
                    break;
                case "供货管理":
                    this.LoadSubWindow(new FormSupply());
                    break;
                case "物料管理":
                    this.LoadSubWindow(new FormMaterial());
                    break;
                case "备货库存设置":
                    this.LoadSubWindow(new FormSafetyStock(1));
                    break;
                case "上架库存设置":
                    this.LoadSubWindow(new FormSafetyStock(0));
                    break;
                case "发货套餐管理":
                    this.LoadSubWindow(new FormPackage());
                    break;
                case "仓库管理":
                    this.LoadSubWindow(new FormWarehouse(this.comboBoxWarehouse,this.panelRight,this.treeViewLeft));
                    break;
                case "库区管理":
                    this.LoadSubWindow(new FormStorageArea());
                    break;
                case "库位管理":
                    this.LoadSubWindow(new FormStorageLocation());
                    break;
                case "库存盘点":
                    this.LoadSubWindow(new FormStockTakingOrder());
                    break;
                case "库存批次":
                    this.LoadSubWindow(new FormStockRecord());
                    break;
                case "移位记录":
                    this.LoadSubWindow(new FormTransferRecord());
                    break;
                case "出库单管理":
                    this.LoadSubWindow(new FormDeliverOrder());
                    break;
                case "备货作业单管理":
                    this.LoadSubWindow(new FormTransferOrder.FormTransferOrder());
                    break;
                case "薪金类别":
                    this.LoadSubWindow(new FromSalary.FormSalaryType());
                    break;
                case "薪金期间":
                    this.LoadSubWindow(new FromSalary.FormSalaryPeriod());
                    break;
                 case "人员薪金":
                    this.LoadSubWindow(new FromSalary.FormPersonSalary());
                    break;
                case "薪资发放单":
                    this.LoadSubWindow(new FromSalary.FormPayNote());
                    break;
                case "科目管理":
                    this.LoadSubWindow(new FormAcccount.FormAccountTitle());
                    break;
                case "税务管理":
                    this.LoadSubWindow(new FormAcccount.FormTax());
                    break;
                case "账目记录":
                    this.LoadSubWindow(new FormAcccount.FormAccountRecord());
                    break;
                case "会计期间":
                    this.LoadSubWindow(new FormAcccount.FormAccountPeriod());
                    break;
                case "汇总单管理":
                    this.LoadSubWindow(new FormSettlement.FormSummaryNote());
                    break;
                case "结算单管理":
                    this.LoadSubWindow(new FormSettlement.FormSettlementNote());
                    break;
                case "发票管理":
                    this.LoadSubWindow(new FormSettlement.FormInvoice());
                    break;
            }
            this.panelRight.ResumeLayout();
            this.panelRight.Show();
        }

        private void ToInspectionNoteSelectIDsCallback(int[] selectedIDs)
        {
            this.SetTreeViewSelectedNodeByText("送检单管理");
            this.LoadSubWindow(new FormInspectionNote(selectedIDs));
        }

        private void ToInspectionNoteSearchNoCallback(string searchNo)
        {
            this.SetTreeViewSelectedNodeByText("送检单管理");
            this.LoadSubWindow(new FormInspectionNote(null,searchNo));
        }

        //private void ToJobTicketCallback(string condition, string value)
        //{
        //    if (this.IsDisposed) return;
        //    this.Invoke(new Action(() =>
        //    {
        //        FormJobTicket formJobTicket = new FormJobTicket(this.user.ID, this.project.ID, this.warehouse.ID);//实例化子窗口
        //        formJobTicket.SetToPutOutStorageTicketCallback(this.ToPutOutStorageTicketCallback);
        //        formJobTicket.SetSearchCondition(condition, value);
        //        this.LoadSubWindow(formJobTicket);
        //        this.SetTreeViewSelectedNodeByText("翻包作业单管理");
        //    }));
        //}

        //private void ToPutOutStorageTicketCallback(string condition, string jobTicketNo)
        //{
        //    if (this.IsDisposed) return;
        //    this.Invoke(new Action(() =>
        //    {
        //        FormPutOutStorageTicket formPutOutStorageTicket = new FormPutOutStorageTicket(this.user.ID, this.project.ID, this.warehouse.ID);//实例化子窗口
        //        formPutOutStorageTicket.SetSearchCondition(condition, jobTicketNo);
        //        this.LoadSubWindow(formPutOutStorageTicket);
        //        this.SetTreeViewSelectedNodeByText("出库单管理");
        //    }));
        //}

        private void LoadSubWindow(Form form)
        {
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;//窗口大小
            form.FormBorderStyle = FormBorderStyle.None;//没有标题栏
            this.panelRight.Controls.Clear();//清空
            this.panelRight.Controls.Add(form);
            form.Show();
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.formClosedCallback?.Invoke();
        }

        private void comboBoxProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.project = ((ComboBoxItem)this.comboBoxProject.SelectedItem).Value as Project;
            //GlobalData.ProjectID = this.project.ID;
            //this.panelRight.Controls.Clear();
            //if (this.Run1 ==true  )
            //{
            //     FormSupplyRemind.RemindStockinfo();
                 
            //}
            //this.treeViewLeft.SelectedNode = null;
            //this.Run1 = true;
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
             $"{Defines.ServerURL}/settlement/{GlobalData.AccountBook}/summary_note/{condWarehouse.ToString()}");

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

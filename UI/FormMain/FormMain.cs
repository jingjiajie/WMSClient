using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
using System.Windows.Forms;
using WMS.UI.FormReceipt;
using WMS.UI.FormBase;
using WMS.DataAccess;
using System.Diagnostics;
using WMS.UI.FormBasicInfos;

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
                    MakeTreeNode("供货管理"),
                    MakeTreeNode("物料管理"),
                    MakeTreeNode("人员管理"),
                    MakeTreeNode("库区管理"),
                    MakeTreeNode(" 库位管理")
                    }),
                MakeTreeNode("入库管理",new TreeNode[]{
                    MakeTreeNode("入库单管理"),
                    MakeTreeNode("送检单管理"),
                    MakeTreeNode("上架单管理")
                    }),
                MakeTreeNode("发货管理",new TreeNode[]{
                    MakeTreeNode("工作任务单管理"),
                    MakeTreeNode("翻包作业单管理"),
                    MakeTreeNode("出库单管理"),
                    }),
                MakeTreeNode("库存管理",new TreeNode[]{
                    MakeTreeNode("库存批次"),
                    MakeTreeNode("库存盘点"),
                    }),
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
                    this.LoadSubWindow(new FormWarehouseEntry());
                    break;
                case "送检单管理":
                    this.LoadSubWindow(new FormInspectionNote());
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
                case "库区管理":
                    this.LoadSubWindow(new FormStorageArea());
                    break;
                case "库位管理":
                    this.LoadSubWindow(new FormStorageLocation);
                    break;
            }            
            //if (treeViewLeft.SelectedNode.Text == "供应商管理")
            //{
            //    this.panelRight.Controls.Clear();//清空
            //    FormBaseSupplier l = new FormBaseSupplier(user.Authority, this.supplierid, this.user.ID);//实例化子窗口
            //    l.TopLevel = false;
            //    l.Dock = System.Windows.Forms.DockStyle.Fill;//窗口大小
            //    l.FormBorderStyle = FormBorderStyle.None;//没有标题栏
            //    this.panelRight.Controls.Add(l);
            //    l.Show();
            //}
            //if (treeViewLeft.SelectedNode.Text == "供货管理")
            //{
            //    this.panelRight.Controls.Clear();//清空
            //    FormBaseSupply l = new FormBaseSupply(user.Authority, this.supplierid, this.project.ID, this.warehouse.ID, this.user.ID);//实例化子窗口
            //    l.TopLevel = false;
            //    l.Dock = System.Windows.Forms.DockStyle.Fill;//窗口大小
            //    l.FormBorderStyle = FormBorderStyle.None;//没有标题栏
            //    this.panelRight.Controls.Add(l);
            //    l.Show();
            //}
            //if (treeViewLeft.SelectedNode.Text == "零件管理")
            //{
            //    this.panelRight.Controls.Clear();//清空
            //    FormBaseComponent l = new FormBaseComponent(user.Authority, this.supplierid, this.project.ID, this.warehouse.ID, this.user.ID);//实例化子窗口
            //    l.TopLevel = false;
            //    l.Dock = System.Windows.Forms.DockStyle.Fill;//窗口大小
            //    l.FormBorderStyle = FormBorderStyle.None;//没有标题栏
            //    this.panelRight.Controls.Add(l);
            //    l.Show();
            //}
            //if (treeViewLeft.SelectedNode.Text == "其他")
            //{
            //    this.setitem = 0;
            //    this.LoadSubWindow(new FormOtherInfo(this.setitem));
            //}
            //if (treeViewLeft.SelectedNode.Text == "项目管理")
            //{
            //    this.panelRight.Controls.Clear();//清空
            //    this.setitem = 1;
            //    FormBaseProject l = new FormBaseProject();//实例化子窗口
            //    l.TopLevel = false;
            //    l.Dock = System.Windows.Forms.DockStyle.Fill;//窗口大小
            //    l.FormBorderStyle = FormBorderStyle.None;//没有标题栏
            //    this.panelRight.Controls.Add(l);
            //    l.Show();
            //}
            //if (treeViewLeft.SelectedNode.Text == "人员管理")
            //{
            //    this.panelRight.Controls.Clear();//清空
            //    FormBase.FormBasePerson l = new FormBase.FormBasePerson();//实例化子窗口
            //    l.TopLevel = false;
            //    l.Dock = System.Windows.Forms.DockStyle.Fill;//窗口大小
            //    l.FormBorderStyle = FormBorderStyle.None;//没有标题栏
            //    this.panelRight.Controls.Add(l);
            //    l.Show();
            //}
            //if (treeViewLeft.SelectedNode.Text == "到货单管理")
            //{
            //    this.panelRight.Controls.Clear();//清空
            //    FormReceiptArrival l = new FormReceiptArrival(this.project.ID, this.warehouse.ID, this.user.ID, this.supplierid);//实例化子窗口
            //    l.SetActionTo(0, new Action<string, string>((string key, string value) =>
            //    {
            //        this.panelRight.Controls.Clear();
            //        FormSubmissionManage s = new FormSubmissionManage(this.project.ID, this.warehouse.ID, this.user.ID, key, value);
            //        s.setActionTo(new Action<string, string>((string key1, string value1) =>
            //        {
            //            this.panelRight.Controls.Clear();
            //            FormReceiptShelves t = new FormReceiptShelves(this.project.ID, this.warehouse.ID, this.user.ID, key1, value1);
            //            t.TopLevel = false;
            //            t.Dock = System.Windows.Forms.DockStyle.Fill;
            //            t.FormBorderStyle = FormBorderStyle.None;
            //            //s.Dock = System.Windows.Forms.DockStyle.Fill;
            //            this.panelRight.Controls.Add(t);

            //            t.Show();
            //            SetTreeViewSelectedNodeByText("上架单管理");
            //            Utilities.SendMessage(this.panelRight.Handle, Utilities.WM_SETREDRAW, 1, 0);
            //        }));

            //        s.TopLevel = false;
            //        s.Dock = System.Windows.Forms.DockStyle.Fill;
            //        this.panelRight.Controls.Clear();//清空
            //        s.FormBorderStyle = FormBorderStyle.None;
            //        this.panelRight.Controls.Add(s);
            //        s.Show();
            //        SetTreeViewSelectedNodeByText("送检单管理");
            //        //treeViewLeft.SelectedNode = treeViewLeft.Nodes.Find("送检单管理", true)[0];
            //        Utilities.SendMessage(this.panelRight.Handle, Utilities.WM_SETREDRAW, 1, 0);
            //    }));
            //    l.SetActionTo(1, new Action<string, string>((key, value) =>
            //    {
            //        this.panelRight.Controls.Clear();
            //        FormReceiptShelves s = new FormReceiptShelves(this.project.ID, this.warehouse.ID, this.user.ID, key, value);
            //        s.TopLevel = false;
            //        s.Dock = System.Windows.Forms.DockStyle.Fill;
            //        s.FormBorderStyle = FormBorderStyle.None;
            //        //s.Dock = System.Windows.Forms.DockStyle.Fill;
            //        this.panelRight.Controls.Add(s);

            //        s.SetToPutaway(new Action<string, string>((key1, value1) =>
            //        {
            //            this.panelRight.Controls.Clear();
            //            FormShelvesItem a = new FormShelvesItem(this.project.ID, this.warehouse.ID, this.user.ID, key1, value1);
            //            a.TopLevel = false;
            //            a.Dock = System.Windows.Forms.DockStyle.Fill;
            //            a.FormBorderStyle = FormBorderStyle.None;
            //            //s.Dock = System.Windows.Forms.DockStyle.Fill;
            //            this.panelRight.Controls.Add(a);
            //            a.Show();
            //            //this.treeViewLeft.SelectedNode = treeViewLeft.Nodes.Find("上架零件管理", true)[0];
            //            Utilities.SendMessage(this.panelRight.Handle, Utilities.WM_SETREDRAW, 1, 0);
            //        }));

            //        s.Show();
            //        SetTreeViewSelectedNodeByText("上架单管理");
            //        Utilities.SendMessage(this.panelRight.Handle, Utilities.WM_SETREDRAW, 1, 0);
            //    }));
            //    l.TopLevel = false;
            //    l.Dock = System.Windows.Forms.DockStyle.Fill;//窗口大小
            //    l.FormBorderStyle = FormBorderStyle.None;//没有标题栏
            //    this.panelRight.Controls.Add(l);
            //    l.Show();
            //}

            //if (treeViewLeft.SelectedNode.Text == "上架单管理")
            //{
            //    this.panelRight.Controls.Clear();//清空
            //    FormReceiptShelves l = new FormReceiptShelves(this.project.ID, this.warehouse.ID, this.user.ID);//实例化子窗口
            //    l.TopLevel = false;
            //    l.Dock = System.Windows.Forms.DockStyle.Fill;//窗口大小
            //    l.FormBorderStyle = FormBorderStyle.None;//没有标题栏
            //    l.SetToPutaway(new Action<string, string>((key, value) =>
            //    {
            //        this.panelRight.Controls.Clear();
            //        FormShelvesItem s = new FormShelvesItem(this.project.ID, this.warehouse.ID, this.user.ID, key, value);
            //        s.TopLevel = false;
            //        s.Dock = System.Windows.Forms.DockStyle.Fill;
            //        s.FormBorderStyle = FormBorderStyle.None;
            //        //s.Dock = System.Windows.Forms.DockStyle.Fill;
            //        this.panelRight.Controls.Add(s);
            //        s.Show();
            //        SetTreeViewSelectedNodeByText("上架零件管理");
            //        //this.treeViewLeft.SelectedNode = treeViewLeft.Nodes.Find("上架零件管理", true)[0];
            //        Utilities.SendMessage(this.panelRight.Handle, Utilities.WM_SETREDRAW, 1, 0);
            //    }));
            //    this.panelRight.Controls.Add(l);
            //    l.Show();
            //}

            //if (treeViewLeft.SelectedNode.Text == "上架零件管理")
            //{
            //    this.panelRight.Controls.Clear();
            //    FormShelvesItem l = new FormShelvesItem(this.project.ID, this.warehouse.ID, this.user.ID, null, null);
            //    l.TopLevel = false;
            //    l.Dock = System.Windows.Forms.DockStyle.Fill;
            //    l.FormBorderStyle = FormBorderStyle.None;
            //    this.panelRight.Controls.Add(l);
            //    l.Show();
            //}

            //if (treeViewLeft.SelectedNode.Text == "工作任务单管理")
            //{
            //    this.panelRight.Controls.Clear();//清空
            //    FormShipmentTicket formShipmentTicket = new FormShipmentTicket(this.user.ID, this.project.ID, this.warehouse.ID);//实例化子窗口
            //    formShipmentTicket.SetToJobTicketCallback(this.ToJobTicketCallback);
            //    formShipmentTicket.TopLevel = false;
            //    formShipmentTicket.Dock = System.Windows.Forms.DockStyle.Fill;//窗口大小
            //    formShipmentTicket.FormBorderStyle = FormBorderStyle.None;//没有标题栏
            //    this.panelRight.Controls.Add(formShipmentTicket);
            //    formShipmentTicket.Show();
            //}
            //if (treeViewLeft.SelectedNode.Text == "翻包作业单管理")
            //{
            //    this.panelRight.Controls.Clear();//清空
            //    FormJobTicket l = new FormJobTicket(this.user.ID, this.project.ID, this.warehouse.ID);//实例化子窗口
            //    l.SetToPutOutStorageTicketCallback(this.ToPutOutStorageTicketCallback);
            //    l.TopLevel = false;
            //    l.Dock = System.Windows.Forms.DockStyle.Fill;//窗口大小
            //    l.FormBorderStyle = FormBorderStyle.None;//没有标题栏
            //    this.panelRight.Controls.Add(l);
            //    l.Show();
            //}
            //if (treeViewLeft.SelectedNode.Text == "出库单管理")
            //{
            //    this.panelRight.Controls.Clear();//清空
            //    FormPutOutStorageTicket l = new FormPutOutStorageTicket(this.user.ID, this.project.ID, this.warehouse.ID);//实例化子窗口
            //    l.TopLevel = false;
            //    l.Dock = System.Windows.Forms.DockStyle.Fill;//窗口大小
            //    l.FormBorderStyle = FormBorderStyle.None;//没有标题栏
            //    this.panelRight.Controls.Add(l);
            //    l.Show();
            //}
            //if (treeViewLeft.SelectedNode.Text == "库存批次")
            //{
            //    this.panelRight.Controls.Clear();//清空
            //    var formBaseStock = new FormStockInfo(this.user.ID, this.project.ID, this.warehouse.ID);//实例化子窗口
            //    formBaseStock.TopLevel = false;
            //    formBaseStock.Dock = DockStyle.Fill;//窗口大小
            //    formBaseStock.FormBorderStyle = FormBorderStyle.None;//没有标题栏
            //    this.panelRight.Controls.Add(formBaseStock);
            //    formBaseStock.Show();
            //}
            //if (treeViewLeft.SelectedNode.Text == "库存盘点")
            //{
            //    this.panelRight.Controls.Clear();//清空
            //    var formBaseStock = new FormStockInfoCheckTicket(this.project.ID, this.warehouse.ID, this.user.ID);//实例化子窗口
            //    formBaseStock.TopLevel = false;
            //    formBaseStock.Dock = DockStyle.Fill;//窗口大小
            //    formBaseStock.FormBorderStyle = FormBorderStyle.None;//没有标题栏
            //    this.panelRight.Controls.Add(formBaseStock);
            //    formBaseStock.Show();
            //}
            //if (treeViewLeft.SelectedNode.Text == "送检单管理")
            //{
            //    this.panelRight.Controls.Clear();//清空
            //    var formSubmissionManage = new FormSubmissionManage(this.project.ID, this.warehouse.ID, this.user.ID);//实例化子窗口
            //    formSubmissionManage.setActionTo(new Action<string, string>((string key, string value) =>
            //    {
            //        this.panelRight.Controls.Clear();
            //        FormReceiptShelves t = new FormReceiptShelves(this.project.ID, this.warehouse.ID, this.user.ID, key, value);
            //        t.TopLevel = false;
            //        t.Dock = System.Windows.Forms.DockStyle.Fill;
            //        t.FormBorderStyle = FormBorderStyle.None;
            //        //s.Dock = System.Windows.Forms.DockStyle.Fill;
            //        this.panelRight.Controls.Add(t);

            //        t.Show();
            //        SetTreeViewSelectedNodeByText("上架单管理");
            //        Utilities.SendMessage(this.panelRight.Handle, Utilities.WM_SETREDRAW, 1, 0);
            //    }));
            //    formSubmissionManage.TopLevel = false;
            //    formSubmissionManage.Dock = DockStyle.Fill;//窗口大小
            //    formSubmissionManage.FormBorderStyle = FormBorderStyle.None;//没有标题栏
            //    this.panelRight.Controls.Add(formSubmissionManage);
            //    formSubmissionManage.Show();
            //}
            this.panelRight.ResumeLayout();
            this.panelRight.Show();
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
            //this.warehouse = ((ComboBoxItem)this.comboBoxWarehouse.SelectedItem).Value as Warehouse;
            //GlobalData.WarehouseID = this.warehouse.ID;
            //this.panelRight.Controls.Clear();
            //if (this.Run ==true  )
            //{
            //    FormSupplyRemind.RemindStockinfo();
               
            //}
            //this.Run = true;
            //this.treeViewLeft.SelectedNode = null;
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

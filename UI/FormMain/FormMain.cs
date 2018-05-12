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
        private User user = null;
        private Project project = null;
        private Warehouse warehouse = null;
        private WMSEntities wmsEntities = new WMSEntities();
        private int supplierid = -1;
        private string contractstate = "";
        private int setitem;
        string remindtext = "";
        private bool contract_effect = true;
        private bool startend = true;
        private  bool Run = false;
        private  bool Run1 = false;
        private DateTime contract_enddate;
        private DateTime contract_startdate;
        private int reminedays;
        StringBuilder  stringBuilder = new StringBuilder();
        //bool show = false;
        //FormSupplyRemind a1 = null;


        private Action formClosedCallback;

        public FormMain(User user, Project project, Warehouse warehouse)
        {
            InitializeComponent();
            this.user = user;
            this.project = project;
            this.warehouse = warehouse;

            if (user.SupplierID != null)
            {
                this.button2.Visible = false;
                this.supplierid = Convert.ToInt32(user.SupplierID);
                remind();


            }
            else if (user.SupplierID == null)
            {
                supplierid = -1;
            }
        }



        private void remind()
        {

            WMSEntities wmsEntities = new WMSEntities();
            ComponentView component = null;
            int days;
            StringBuilder sb = new StringBuilder();

            //合同天数
            Supplier Supplier = new Supplier();
            try
            {

                Supplier = (from u in this.wmsEntities.Supplier
                            where u.ID == supplierid
                            select u).FirstOrDefault();

                contract_enddate = Convert.ToDateTime(Supplier.EndingTime);



                contract_startdate = Convert.ToDateTime(Supplier.StartingTime);

                if (Supplier.EndingTime == null || Supplier.EndingTime == null)
                {
                    startend = false;
                }


                this.contractstate = Supplier.ContractState;
            }
            catch (Exception ex)
            {

                MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            days = (contract_enddate - DateTime.Now).Days;
            if (contract_startdate > DateTime.Now)
            {
                contract_effect = false;
            }

            //库存
            int[] warringdays = { 3, 5, 10 };



            try
            {

                var ComponentName = (from u in wmsEntities.StockInfoView
                                     where u.SupplierID == supplierid
                                     select u.ComponentName).ToArray();


                var ShipmentAreaAmount = (from u in wmsEntities.StockInfoView
                                          where u.SupplierID ==
                                          this.supplierid
                                          select u.ShipmentAreaAmount).ToArray();





                var OverflowAreaAmount = (from u in wmsEntities.StockInfoView
                                          where u.SupplierID == supplierid
                                          select u.OverflowAreaAmount).ToArray();



                for (int i = 0; i < ComponentName.Length; i++)

                {
                    if (ComponentName[i] == null)
                    {
                        continue;
                    }

                    for (int j = i + 1; j < ComponentName.Length; j++)
                    {
                        if (ComponentName[i] == ComponentName[j])
                        {


                            ComponentName[j] = null;
                            ShipmentAreaAmount[i] = Convert.ToDecimal(ShipmentAreaAmount[i]) + Convert.ToDecimal(ShipmentAreaAmount[j]);
                            OverflowAreaAmount[i] = Convert.ToDecimal(OverflowAreaAmount[i]) + Convert.ToDecimal(OverflowAreaAmount[j]);
                        }

                    }

                }


                int singlecaramount;
                int dailyproduction;
                for (int i = 0; i < ComponentName.Length; i++)

                {

                    if (ComponentName[i] == null)
                    {
                        continue;
                    }

                    string ComponentNamei = ComponentName[i];

                    if (ShipmentAreaAmount[i] == null)
                    {
                        continue;
                    }


                    if (OverflowAreaAmount[i] == null)
                    {
                        continue;
                    }

                    try
                    {
                        component = (from u in wmsEntities.ComponentView
                                     where u.Name == ComponentNamei
                                     select u).FirstOrDefault();
                        if (component == null)
                        {

                            continue;
                        }


                        {
                            if (component.SingleCarUsageAmount == null || component.SingleCarUsageAmount == 0)
                            {
                                continue;
                            }

                            singlecaramount = Convert.ToInt32(component.SingleCarUsageAmount);

                            if (component.DailyProduction == null || component.DailyProduction == 0)
                            {
                                continue;
                            }

                            dailyproduction = Convert.ToInt32(component.DailyProduction);


                            reminedays = Convert.ToInt32((ShipmentAreaAmount[i]) + OverflowAreaAmount[i]) / (singlecaramount * dailyproduction);

                            if (reminedays < 10 || reminedays == 10)
                            {
                                if (reminedays == 10)

                                {
                                    sb.Append("您的库存" + ComponentName[i] + "只有10天可生产，请您补货" + "\r\n" + "\r\n");
                                }
                                else if (reminedays == 9)
                                {


                                    sb.Append("您的库存" + ComponentName[i] + "已经不足10天生产，请您补货" + "\r\n" + "\r\n");
                                }

                                else if (reminedays == 8)
                                {


                                    sb.Append("您的库存" + ComponentName[i] + "已经不足10天生产，请您补货" + "\r\n" + "\r\n");
                                }
                                else if (reminedays == 7)
                                {


                                    sb.Append("您的库存" + ComponentName[i] + "已经不足10天生产，请您补货" + "\r\n" + "\r\n");
                                }
                                else if (reminedays == 6)
                                {


                                    sb.Append("您的库存" + ComponentName[i] + "已经不足10天生产，请您补货" + "\r\n" + "\r\n");
                                }

                                else if (reminedays == 5)
                                {


                                    sb.Append("您的库存" + ComponentName[i] + "只有5天可生产，请您补货" + "\r\n" + "\r\n");
                                }

                                else if (reminedays == 4)
                                {


                                    sb.Append("您的库存" + ComponentName[i] + "已经不足5天生产，请您补货" + "\r\n" + "\r\n");
                                }

                                else if (reminedays == 3)
                                {


                                    sb.Append("您的库存" + ComponentName[i] + "只有3天可生产，请您补货" + "\r\n" + "\r\n");
                                }
                                else if (reminedays == 2)
                                {


                                    sb.Append("您的库存" + ComponentName[i] + "已经不足3天生产，请您补货" + "\r\n" + "\r\n");
                                }
                                else if (reminedays == 1)
                                {


                                    sb.Append("您的库存" + ComponentName[i] + "已经不足3天生产，请您补货" + "\r\n" + "\r\n");
                                }
                                else if (reminedays == 0)
                                {


                                    sb.Append("您的库存" + ComponentName[i] + "已经不足3天生产，请您补货" + "\r\n" + "\r\n");
                                }
                            }

                        }
                    }
                    catch
                    {

                        MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                this.remindtext = sb.ToString();

            }
            catch (Exception ex)
            {

                MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //供应商合同日期为空 合同状态为空
            if (Supplier.StartingTime == null)

            {
                this.contract_effect = true;
            }


            if (Supplier.EndingTime == null)

            {

                days = 1;
            }





            ///

            if (days < 0 || remindtext != "" || this.contractstate == "待审核" || contract_effect == false)//||reminedays ==10||reminedays <10 )

            {
                FormSupplierRemind a1 = new FormSupplierRemind(days, this.remindtext, this.contractstate, startend, this.contract_effect);

                a1.Show();
            }
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
                    MakeTreeNode("零件管理"),
                    MakeTreeNode("人员管理"),
                    MakeTreeNode("其他")
                    }),
                MakeTreeNode("收货管理",new TreeNode[]{
                    MakeTreeNode("到货单管理"),
                    MakeTreeNode("送检单管理"),
                    MakeTreeNode("上架单管理"),
                    MakeTreeNode("上架零件管理"),
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
            TreeNode[] nodes = (from node in (from node in treeNodes
                                              where HasAuthority(node.Text)
                                              select GetAuthenticatedSubTreeNodes(node))
                                where node.Nodes.Count > 0
                                select node).ToArray();
            this.treeViewLeft.Nodes.AddRange(nodes);
        }

        //检测用户是否有相应功能的权限
        private bool HasAuthority(string funcName)
        {
            var searchResult = (from fa in FormMainMetaData.FunctionAuthorities
                                where fa.FunctionName == funcName
                                select fa.Authorities).FirstOrDefault();
            if (searchResult == null)
            {
                return true;
            }
            Authority[] authorities = searchResult;
            foreach (Authority authority in authorities)
            {
                if (((int)authority & this.user.Authority) == (int)authority)
                {
                    return true;
                }
            }
            return false;
        }

        //获取有权限的所有子节点
        private TreeNode GetAuthenticatedSubTreeNodes(TreeNode node)
        {
            if (HasAuthority(node.Text) == false)
            {
                return null;
            }

            TreeNode newNode = (TreeNode)node.Clone();
            newNode.Nodes.Clear();

            foreach (TreeNode curNode in node.Nodes)
            {
                if (HasAuthority(curNode.Text))
                {
                    newNode.Nodes.Add(GetAuthenticatedSubTreeNodes(curNode));
                }
            }
            return newNode;
        }

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

            if (this.supplierid == -1)
            {

                

                FormSupplyRemind.SetFormHidedCallback(() =>
                {
                    this.button2.Visible = true ;
                });
                FormSupplyRemind.SetFormShowCallback(() =>
                {
                    this.button2.Visible = false ;
                });
                FormSupplyRemind.RemindStockinfo();
            }
          
            else if (this.supplierid != -1)
            {
                this.button2.Visible = false;
            }            



            //刷新左边树形框
            this.RefreshTreeView();

            //刷新顶部
            this.labelUsername.Text = this.user.Username;
            this.labelAuth.Text = this.user.AuthorityName + " :";

            //窗体大小根据显示屏改变
            int DeskWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int DeskHeight = Screen.PrimaryScreen.WorkingArea.Height;
            this.Width = Convert.ToInt32(DeskWidth * 0.8);
            this.Height = Convert.ToInt32(DeskHeight * 0.8);

            new Thread(() =>
            {
                //下拉栏显示仓库
                WMSEntities wms = new WMSEntities();
                var allWarehouses = (from s in wms.Warehouse select s).ToArray();
                var allProjects = (from s in wms.Project select s).ToArray();
                if (this.IsDisposed)
                {
                    return;
                }
                this.Invoke(new Action(() =>
                {
                    this.comboBoxWarehouse.Items.AddRange((from w in allWarehouses select new ComboBoxItem(w.Name, w)).ToArray());

                    for (int i = 0; i < this.comboBoxWarehouse.Items.Count; i++)
                    {
                        if (((Warehouse)(((ComboBoxItem)this.comboBoxWarehouse.Items[i]).Value)).ID == this.warehouse.ID)
                        {
                            this.comboBoxWarehouse.SelectedIndex = i;
                            break;
                        }
                    }
                    this.comboBoxProject.Items.AddRange((from p in allProjects select new ComboBoxItem(p.Name, p)).ToArray());
                    for (int i = 0; i < this.comboBoxWarehouse.Items.Count; i++)
                    {
                        if (((Project)(((ComboBoxItem)this.comboBoxProject.Items[i]).Value)).ID == this.project.ID)
                        {
                            this.comboBoxProject.SelectedIndex = i;
                            break;
                        }
                    }
                }));
            }).Start();
           
 
            

        }

        private void treeViewLeft_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.panelRight.Hide();
            this.panelRight.SuspendLayout();
            if (treeViewLeft.SelectedNode.Text == "用户管理")
            {
                this.panelRight.Controls.Clear();//清空
                FormPerson formPerson = new FormPerson();
                formPerson.TopLevel = false;
                formPerson.Dock = DockStyle.Fill;//窗口大小
                formPerson.FormBorderStyle = FormBorderStyle.None;//没有标题栏
                this.panelRight.Controls.Add(formPerson);
                formPerson.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "供应商管理")
            {
                this.panelRight.Controls.Clear();//清空
                FormBaseSupplier l = new FormBaseSupplier(user.Authority, this.supplierid, this.user.ID);//实例化子窗口
                l.TopLevel = false;
                l.Dock = System.Windows.Forms.DockStyle.Fill;//窗口大小
                l.FormBorderStyle = FormBorderStyle.None;//没有标题栏
                this.panelRight.Controls.Add(l);
                l.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "供货管理")
            {
                this.panelRight.Controls.Clear();//清空
                FormBaseSupply l = new FormBaseSupply(user.Authority, this.supplierid, this.project.ID, this.warehouse.ID, this.user.ID);//实例化子窗口
                l.TopLevel = false;
                l.Dock = System.Windows.Forms.DockStyle.Fill;//窗口大小
                l.FormBorderStyle = FormBorderStyle.None;//没有标题栏
                this.panelRight.Controls.Add(l);
                l.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "零件管理")
            {
                this.panelRight.Controls.Clear();//清空
                FormBaseComponent l = new FormBaseComponent(user.Authority, this.supplierid, this.project.ID, this.warehouse.ID, this.user.ID);//实例化子窗口
                l.TopLevel = false;
                l.Dock = System.Windows.Forms.DockStyle.Fill;//窗口大小
                l.FormBorderStyle = FormBorderStyle.None;//没有标题栏
                this.panelRight.Controls.Add(l);
                l.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "其他")
            {
                this.setitem = 0;
                this.LoadSubWindow(new FormOtherInfo(this.setitem));
            }
            if (treeViewLeft.SelectedNode.Text == "项目管理")
            {
                this.panelRight.Controls.Clear();//清空
                this.setitem = 1;
                FormBaseProject l = new FormBaseProject();//实例化子窗口
                l.TopLevel = false;
                l.Dock = System.Windows.Forms.DockStyle.Fill;//窗口大小
                l.FormBorderStyle = FormBorderStyle.None;//没有标题栏
                this.panelRight.Controls.Add(l);
                l.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "人员管理")
            {
                this.panelRight.Controls.Clear();//清空
                FormBase.FormBasePerson l = new FormBase.FormBasePerson();//实例化子窗口
                l.TopLevel = false;
                l.Dock = System.Windows.Forms.DockStyle.Fill;//窗口大小
                l.FormBorderStyle = FormBorderStyle.None;//没有标题栏
                this.panelRight.Controls.Add(l);
                l.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "到货单管理")
            {
                this.panelRight.Controls.Clear();//清空
                FormReceiptArrival l = new FormReceiptArrival(this.project.ID, this.warehouse.ID, this.user.ID, this.supplierid);//实例化子窗口
                l.SetActionTo(0, new Action<string, string>((string key, string value) =>
                {
                    this.panelRight.Controls.Clear();
                    FormSubmissionManage s = new FormSubmissionManage(this.project.ID, this.warehouse.ID, this.user.ID, key, value);
                    s.setActionTo(new Action<string, string>((string key1, string value1) =>
                    {
                        this.panelRight.Controls.Clear();
                        FormReceiptShelves t = new FormReceiptShelves(this.project.ID, this.warehouse.ID, this.user.ID, key1, value1);
                        t.TopLevel = false;
                        t.Dock = System.Windows.Forms.DockStyle.Fill;
                        t.FormBorderStyle = FormBorderStyle.None;
                        //s.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.panelRight.Controls.Add(t);

                        t.Show();
                        SetTreeViewSelectedNodeByText("上架单管理");
                        Utilities.SendMessage(this.panelRight.Handle, Utilities.WM_SETREDRAW, 1, 0);
                    }));

                    s.TopLevel = false;
                    s.Dock = System.Windows.Forms.DockStyle.Fill;
                    this.panelRight.Controls.Clear();//清空
                    s.FormBorderStyle = FormBorderStyle.None;
                    this.panelRight.Controls.Add(s);
                    s.Show();
                    SetTreeViewSelectedNodeByText("送检单管理");
                    //treeViewLeft.SelectedNode = treeViewLeft.Nodes.Find("送检单管理", true)[0];
                    Utilities.SendMessage(this.panelRight.Handle, Utilities.WM_SETREDRAW, 1, 0);
                }));
                l.SetActionTo(1, new Action<string, string>((key, value) =>
                {
                    this.panelRight.Controls.Clear();
                    FormReceiptShelves s = new FormReceiptShelves(this.project.ID, this.warehouse.ID, this.user.ID, key, value);
                    s.TopLevel = false;
                    s.Dock = System.Windows.Forms.DockStyle.Fill;
                    s.FormBorderStyle = FormBorderStyle.None;
                    //s.Dock = System.Windows.Forms.DockStyle.Fill;
                    this.panelRight.Controls.Add(s);

                    s.SetToPutaway(new Action<string, string>((key1, value1) =>
                    {
                        this.panelRight.Controls.Clear();
                        FormShelvesItem a = new FormShelvesItem(this.project.ID, this.warehouse.ID, this.user.ID, key1, value1);
                        a.TopLevel = false;
                        a.Dock = System.Windows.Forms.DockStyle.Fill;
                        a.FormBorderStyle = FormBorderStyle.None;
                        //s.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.panelRight.Controls.Add(a);
                        a.Show();
                        //this.treeViewLeft.SelectedNode = treeViewLeft.Nodes.Find("上架零件管理", true)[0];
                        Utilities.SendMessage(this.panelRight.Handle, Utilities.WM_SETREDRAW, 1, 0);
                    }));

                    s.Show();
                    SetTreeViewSelectedNodeByText("上架单管理");
                    Utilities.SendMessage(this.panelRight.Handle, Utilities.WM_SETREDRAW, 1, 0);
                }));
                l.TopLevel = false;
                l.Dock = System.Windows.Forms.DockStyle.Fill;//窗口大小
                l.FormBorderStyle = FormBorderStyle.None;//没有标题栏
                this.panelRight.Controls.Add(l);
                l.Show();
            }

            if (treeViewLeft.SelectedNode.Text == "上架单管理")
            {
                this.panelRight.Controls.Clear();//清空
                FormReceiptShelves l = new FormReceiptShelves(this.project.ID, this.warehouse.ID, this.user.ID);//实例化子窗口
                l.TopLevel = false;
                l.Dock = System.Windows.Forms.DockStyle.Fill;//窗口大小
                l.FormBorderStyle = FormBorderStyle.None;//没有标题栏
                l.SetToPutaway(new Action<string, string>((key, value) =>
                {
                    this.panelRight.Controls.Clear();
                    FormShelvesItem s = new FormShelvesItem(this.project.ID, this.warehouse.ID, this.user.ID, key, value);
                    s.TopLevel = false;
                    s.Dock = System.Windows.Forms.DockStyle.Fill;
                    s.FormBorderStyle = FormBorderStyle.None;
                    //s.Dock = System.Windows.Forms.DockStyle.Fill;
                    this.panelRight.Controls.Add(s);
                    s.Show();
                    SetTreeViewSelectedNodeByText("上架零件管理");
                    //this.treeViewLeft.SelectedNode = treeViewLeft.Nodes.Find("上架零件管理", true)[0];
                    Utilities.SendMessage(this.panelRight.Handle, Utilities.WM_SETREDRAW, 1, 0);
                }));
                this.panelRight.Controls.Add(l);
                l.Show();
            }

            if (treeViewLeft.SelectedNode.Text == "上架零件管理")
            {
                this.panelRight.Controls.Clear();
                FormShelvesItem l = new FormShelvesItem(this.project.ID, this.warehouse.ID, this.user.ID, null, null);
                l.TopLevel = false;
                l.Dock = System.Windows.Forms.DockStyle.Fill;
                l.FormBorderStyle = FormBorderStyle.None;
                this.panelRight.Controls.Add(l);
                l.Show();
            }

            if (treeViewLeft.SelectedNode.Text == "工作任务单管理")
            {
                this.panelRight.Controls.Clear();//清空
                FormShipmentTicket formShipmentTicket = new FormShipmentTicket(this.user.ID, this.project.ID, this.warehouse.ID);//实例化子窗口
                formShipmentTicket.SetToJobTicketCallback(this.ToJobTicketCallback);
                formShipmentTicket.TopLevel = false;
                formShipmentTicket.Dock = System.Windows.Forms.DockStyle.Fill;//窗口大小
                formShipmentTicket.FormBorderStyle = FormBorderStyle.None;//没有标题栏
                this.panelRight.Controls.Add(formShipmentTicket);
                formShipmentTicket.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "翻包作业单管理")
            {
                this.panelRight.Controls.Clear();//清空
                FormJobTicket l = new FormJobTicket(this.user.ID, this.project.ID, this.warehouse.ID);//实例化子窗口
                l.SetToPutOutStorageTicketCallback(this.ToPutOutStorageTicketCallback);
                l.TopLevel = false;
                l.Dock = System.Windows.Forms.DockStyle.Fill;//窗口大小
                l.FormBorderStyle = FormBorderStyle.None;//没有标题栏
                this.panelRight.Controls.Add(l);
                l.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "出库单管理")
            {
                this.panelRight.Controls.Clear();//清空
                FormPutOutStorageTicket l = new FormPutOutStorageTicket(this.user.ID, this.project.ID, this.warehouse.ID);//实例化子窗口
                l.TopLevel = false;
                l.Dock = System.Windows.Forms.DockStyle.Fill;//窗口大小
                l.FormBorderStyle = FormBorderStyle.None;//没有标题栏
                this.panelRight.Controls.Add(l);
                l.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "库存批次")
            {
                this.panelRight.Controls.Clear();//清空
                var formBaseStock = new FormStockInfo(this.user.ID, this.project.ID, this.warehouse.ID);//实例化子窗口
                formBaseStock.TopLevel = false;
                formBaseStock.Dock = DockStyle.Fill;//窗口大小
                formBaseStock.FormBorderStyle = FormBorderStyle.None;//没有标题栏
                this.panelRight.Controls.Add(formBaseStock);
                formBaseStock.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "库存盘点")
            {
                this.panelRight.Controls.Clear();//清空
                var formBaseStock = new FormStockInfoCheckTicket(this.project.ID, this.warehouse.ID, this.user.ID);//实例化子窗口
                formBaseStock.TopLevel = false;
                formBaseStock.Dock = DockStyle.Fill;//窗口大小
                formBaseStock.FormBorderStyle = FormBorderStyle.None;//没有标题栏
                this.panelRight.Controls.Add(formBaseStock);
                formBaseStock.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "送检单管理")
            {
                this.panelRight.Controls.Clear();//清空
                var formSubmissionManage = new FormSubmissionManage(this.project.ID, this.warehouse.ID, this.user.ID);//实例化子窗口
                formSubmissionManage.setActionTo(new Action<string, string>((string key, string value) =>
                {
                    this.panelRight.Controls.Clear();
                    FormReceiptShelves t = new FormReceiptShelves(this.project.ID, this.warehouse.ID, this.user.ID, key, value);
                    t.TopLevel = false;
                    t.Dock = System.Windows.Forms.DockStyle.Fill;
                    t.FormBorderStyle = FormBorderStyle.None;
                    //s.Dock = System.Windows.Forms.DockStyle.Fill;
                    this.panelRight.Controls.Add(t);

                    t.Show();
                    SetTreeViewSelectedNodeByText("上架单管理");
                    Utilities.SendMessage(this.panelRight.Handle, Utilities.WM_SETREDRAW, 1, 0);
                }));
                formSubmissionManage.TopLevel = false;
                formSubmissionManage.Dock = DockStyle.Fill;//窗口大小
                formSubmissionManage.FormBorderStyle = FormBorderStyle.None;//没有标题栏
                this.panelRight.Controls.Add(formSubmissionManage);
                formSubmissionManage.Show();
            }
            this.panelRight.ResumeLayout();
            this.panelRight.Show();
        }

        private void ToJobTicketCallback(string condition, string value)
        {
            if (this.IsDisposed) return;
            this.Invoke(new Action(() =>
            {
                FormJobTicket formJobTicket = new FormJobTicket(this.user.ID, this.project.ID, this.warehouse.ID);//实例化子窗口
                formJobTicket.SetToPutOutStorageTicketCallback(this.ToPutOutStorageTicketCallback);
                formJobTicket.SetSearchCondition(condition, value);
                this.LoadSubWindow(formJobTicket);
                this.SetTreeViewSelectedNodeByText("翻包作业单管理");
            }));
        }

        private void ToPutOutStorageTicketCallback(string condition, string jobTicketNo)
        {
            if (this.IsDisposed) return;
            this.Invoke(new Action(() =>
            {
                FormPutOutStorageTicket formPutOutStorageTicket = new FormPutOutStorageTicket(this.user.ID, this.project.ID, this.warehouse.ID);//实例化子窗口
                formPutOutStorageTicket.SetSearchCondition(condition, jobTicketNo);
                this.LoadSubWindow(formPutOutStorageTicket);
                this.SetTreeViewSelectedNodeByText("出库单管理");
            }));
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

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.formClosedCallback?.Invoke();
        }

        private void comboBoxProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.project = ((ComboBoxItem)this.comboBoxProject.SelectedItem).Value as Project;
            GlobalData.ProjectID = this.project.ID;
            this.panelRight.Controls.Clear();
            if (this.Run1 ==true  )
            {
                 FormSupplyRemind.RemindStockinfo();
                 
            }
            this.treeViewLeft.SelectedNode = null;
            this.Run1 = true;
        }

        private void comboBoxWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.warehouse = ((ComboBoxItem)this.comboBoxWarehouse.SelectedItem).Value as Warehouse;
            GlobalData.WarehouseID = this.warehouse.ID;
            this.panelRight.Controls.Clear();
            if (this.Run ==true  )
            {
                FormSupplyRemind.RemindStockinfo();
               
            }
            this.Run = true;
            this.treeViewLeft.SelectedNode = null;
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
            if (this.WindowState == FormWindowState.Minimized)
            {

                FormSupplyRemind.HideForm();
            }
            else if (this.WindowState == FormWindowState.Maximized && this.button2.Visible == false)
            {
                //FormSupplyRemind.RemindStockinfo();
                FormSupplyRemind.ShowForm();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
           //FormSupplyRemind.RemindStockinfo();
           FormSupplyRemind.RemindStockinfoClick();
           //this.button2.Visible = false;
           this.Run = true;
           this.Run1 = true;         
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

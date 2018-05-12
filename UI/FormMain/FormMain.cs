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

            //��ͬ����
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

                MessageBox.Show("����ʧ�ܣ�������������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            days = (contract_enddate - DateTime.Now).Days;
            if (contract_startdate > DateTime.Now)
            {
                contract_effect = false;
            }

            //���
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
                                    sb.Append("���Ŀ��" + ComponentName[i] + "ֻ��10�����������������" + "\r\n" + "\r\n");
                                }
                                else if (reminedays == 9)
                                {


                                    sb.Append("���Ŀ��" + ComponentName[i] + "�Ѿ�����10����������������" + "\r\n" + "\r\n");
                                }

                                else if (reminedays == 8)
                                {


                                    sb.Append("���Ŀ��" + ComponentName[i] + "�Ѿ�����10����������������" + "\r\n" + "\r\n");
                                }
                                else if (reminedays == 7)
                                {


                                    sb.Append("���Ŀ��" + ComponentName[i] + "�Ѿ�����10����������������" + "\r\n" + "\r\n");
                                }
                                else if (reminedays == 6)
                                {


                                    sb.Append("���Ŀ��" + ComponentName[i] + "�Ѿ�����10����������������" + "\r\n" + "\r\n");
                                }

                                else if (reminedays == 5)
                                {


                                    sb.Append("���Ŀ��" + ComponentName[i] + "ֻ��5�����������������" + "\r\n" + "\r\n");
                                }

                                else if (reminedays == 4)
                                {


                                    sb.Append("���Ŀ��" + ComponentName[i] + "�Ѿ�����5����������������" + "\r\n" + "\r\n");
                                }

                                else if (reminedays == 3)
                                {


                                    sb.Append("���Ŀ��" + ComponentName[i] + "ֻ��3�����������������" + "\r\n" + "\r\n");
                                }
                                else if (reminedays == 2)
                                {


                                    sb.Append("���Ŀ��" + ComponentName[i] + "�Ѿ�����3����������������" + "\r\n" + "\r\n");
                                }
                                else if (reminedays == 1)
                                {


                                    sb.Append("���Ŀ��" + ComponentName[i] + "�Ѿ�����3����������������" + "\r\n" + "\r\n");
                                }
                                else if (reminedays == 0)
                                {


                                    sb.Append("���Ŀ��" + ComponentName[i] + "�Ѿ�����3����������������" + "\r\n" + "\r\n");
                                }
                            }

                        }
                    }
                    catch
                    {

                        MessageBox.Show("����ʧ�ܣ�������������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                this.remindtext = sb.ToString();

            }
            catch (Exception ex)
            {

                MessageBox.Show("����ʧ�ܣ�������������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //��Ӧ�̺�ͬ����Ϊ�� ��ͬ״̬Ϊ��
            if (Supplier.StartingTime == null)

            {
                this.contract_effect = true;
            }


            if (Supplier.EndingTime == null)

            {

                days = 1;
            }





            ///

            if (days < 0 || remindtext != "" || this.contractstate == "�����" || contract_effect == false)//||reminedays ==10||reminedays <10 )

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
                MakeTreeNode("������Ϣ",new TreeNode[]{
                    MakeTreeNode("�û�����"),
                    MakeTreeNode("��Ӧ�̹���"),
                    MakeTreeNode("��������"),
                    MakeTreeNode("�������"),
                    MakeTreeNode("��Ա����"),
                    MakeTreeNode("����")
                    }),
                MakeTreeNode("�ջ�����",new TreeNode[]{
                    MakeTreeNode("����������"),
                    MakeTreeNode("�ͼ쵥����"),
                    MakeTreeNode("�ϼܵ�����"),
                    MakeTreeNode("�ϼ��������"),
                    }),
                MakeTreeNode("��������",new TreeNode[]{
                    MakeTreeNode("�������񵥹���"),
                    MakeTreeNode("������ҵ������"),
                    MakeTreeNode("���ⵥ����"),
                    }),
                MakeTreeNode("������",new TreeNode[]{
                    MakeTreeNode("�������"),
                    MakeTreeNode("����̵�"),
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

        //����û��Ƿ�����Ӧ���ܵ�Ȩ��
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

        //��ȡ��Ȩ�޵������ӽڵ�
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



            //ˢ��������ο�
            this.RefreshTreeView();

            //ˢ�¶���
            this.labelUsername.Text = this.user.Username;
            this.labelAuth.Text = this.user.AuthorityName + " :";

            //�����С������ʾ���ı�
            int DeskWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int DeskHeight = Screen.PrimaryScreen.WorkingArea.Height;
            this.Width = Convert.ToInt32(DeskWidth * 0.8);
            this.Height = Convert.ToInt32(DeskHeight * 0.8);

            new Thread(() =>
            {
                //��������ʾ�ֿ�
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
            if (treeViewLeft.SelectedNode.Text == "�û�����")
            {
                this.panelRight.Controls.Clear();//���
                FormPerson formPerson = new FormPerson();
                formPerson.TopLevel = false;
                formPerson.Dock = DockStyle.Fill;//���ڴ�С
                formPerson.FormBorderStyle = FormBorderStyle.None;//û�б�����
                this.panelRight.Controls.Add(formPerson);
                formPerson.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "��Ӧ�̹���")
            {
                this.panelRight.Controls.Clear();//���
                FormBaseSupplier l = new FormBaseSupplier(user.Authority, this.supplierid, this.user.ID);//ʵ�����Ӵ���
                l.TopLevel = false;
                l.Dock = System.Windows.Forms.DockStyle.Fill;//���ڴ�С
                l.FormBorderStyle = FormBorderStyle.None;//û�б�����
                this.panelRight.Controls.Add(l);
                l.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "��������")
            {
                this.panelRight.Controls.Clear();//���
                FormBaseSupply l = new FormBaseSupply(user.Authority, this.supplierid, this.project.ID, this.warehouse.ID, this.user.ID);//ʵ�����Ӵ���
                l.TopLevel = false;
                l.Dock = System.Windows.Forms.DockStyle.Fill;//���ڴ�С
                l.FormBorderStyle = FormBorderStyle.None;//û�б�����
                this.panelRight.Controls.Add(l);
                l.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "�������")
            {
                this.panelRight.Controls.Clear();//���
                FormBaseComponent l = new FormBaseComponent(user.Authority, this.supplierid, this.project.ID, this.warehouse.ID, this.user.ID);//ʵ�����Ӵ���
                l.TopLevel = false;
                l.Dock = System.Windows.Forms.DockStyle.Fill;//���ڴ�С
                l.FormBorderStyle = FormBorderStyle.None;//û�б�����
                this.panelRight.Controls.Add(l);
                l.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "����")
            {
                this.setitem = 0;
                this.LoadSubWindow(new FormOtherInfo(this.setitem));
            }
            if (treeViewLeft.SelectedNode.Text == "��Ŀ����")
            {
                this.panelRight.Controls.Clear();//���
                this.setitem = 1;
                FormBaseProject l = new FormBaseProject();//ʵ�����Ӵ���
                l.TopLevel = false;
                l.Dock = System.Windows.Forms.DockStyle.Fill;//���ڴ�С
                l.FormBorderStyle = FormBorderStyle.None;//û�б�����
                this.panelRight.Controls.Add(l);
                l.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "��Ա����")
            {
                this.panelRight.Controls.Clear();//���
                FormBase.FormBasePerson l = new FormBase.FormBasePerson();//ʵ�����Ӵ���
                l.TopLevel = false;
                l.Dock = System.Windows.Forms.DockStyle.Fill;//���ڴ�С
                l.FormBorderStyle = FormBorderStyle.None;//û�б�����
                this.panelRight.Controls.Add(l);
                l.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "����������")
            {
                this.panelRight.Controls.Clear();//���
                FormReceiptArrival l = new FormReceiptArrival(this.project.ID, this.warehouse.ID, this.user.ID, this.supplierid);//ʵ�����Ӵ���
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
                        SetTreeViewSelectedNodeByText("�ϼܵ�����");
                        Utilities.SendMessage(this.panelRight.Handle, Utilities.WM_SETREDRAW, 1, 0);
                    }));

                    s.TopLevel = false;
                    s.Dock = System.Windows.Forms.DockStyle.Fill;
                    this.panelRight.Controls.Clear();//���
                    s.FormBorderStyle = FormBorderStyle.None;
                    this.panelRight.Controls.Add(s);
                    s.Show();
                    SetTreeViewSelectedNodeByText("�ͼ쵥����");
                    //treeViewLeft.SelectedNode = treeViewLeft.Nodes.Find("�ͼ쵥����", true)[0];
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
                        //this.treeViewLeft.SelectedNode = treeViewLeft.Nodes.Find("�ϼ��������", true)[0];
                        Utilities.SendMessage(this.panelRight.Handle, Utilities.WM_SETREDRAW, 1, 0);
                    }));

                    s.Show();
                    SetTreeViewSelectedNodeByText("�ϼܵ�����");
                    Utilities.SendMessage(this.panelRight.Handle, Utilities.WM_SETREDRAW, 1, 0);
                }));
                l.TopLevel = false;
                l.Dock = System.Windows.Forms.DockStyle.Fill;//���ڴ�С
                l.FormBorderStyle = FormBorderStyle.None;//û�б�����
                this.panelRight.Controls.Add(l);
                l.Show();
            }

            if (treeViewLeft.SelectedNode.Text == "�ϼܵ�����")
            {
                this.panelRight.Controls.Clear();//���
                FormReceiptShelves l = new FormReceiptShelves(this.project.ID, this.warehouse.ID, this.user.ID);//ʵ�����Ӵ���
                l.TopLevel = false;
                l.Dock = System.Windows.Forms.DockStyle.Fill;//���ڴ�С
                l.FormBorderStyle = FormBorderStyle.None;//û�б�����
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
                    SetTreeViewSelectedNodeByText("�ϼ��������");
                    //this.treeViewLeft.SelectedNode = treeViewLeft.Nodes.Find("�ϼ��������", true)[0];
                    Utilities.SendMessage(this.panelRight.Handle, Utilities.WM_SETREDRAW, 1, 0);
                }));
                this.panelRight.Controls.Add(l);
                l.Show();
            }

            if (treeViewLeft.SelectedNode.Text == "�ϼ��������")
            {
                this.panelRight.Controls.Clear();
                FormShelvesItem l = new FormShelvesItem(this.project.ID, this.warehouse.ID, this.user.ID, null, null);
                l.TopLevel = false;
                l.Dock = System.Windows.Forms.DockStyle.Fill;
                l.FormBorderStyle = FormBorderStyle.None;
                this.panelRight.Controls.Add(l);
                l.Show();
            }

            if (treeViewLeft.SelectedNode.Text == "�������񵥹���")
            {
                this.panelRight.Controls.Clear();//���
                FormShipmentTicket formShipmentTicket = new FormShipmentTicket(this.user.ID, this.project.ID, this.warehouse.ID);//ʵ�����Ӵ���
                formShipmentTicket.SetToJobTicketCallback(this.ToJobTicketCallback);
                formShipmentTicket.TopLevel = false;
                formShipmentTicket.Dock = System.Windows.Forms.DockStyle.Fill;//���ڴ�С
                formShipmentTicket.FormBorderStyle = FormBorderStyle.None;//û�б�����
                this.panelRight.Controls.Add(formShipmentTicket);
                formShipmentTicket.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "������ҵ������")
            {
                this.panelRight.Controls.Clear();//���
                FormJobTicket l = new FormJobTicket(this.user.ID, this.project.ID, this.warehouse.ID);//ʵ�����Ӵ���
                l.SetToPutOutStorageTicketCallback(this.ToPutOutStorageTicketCallback);
                l.TopLevel = false;
                l.Dock = System.Windows.Forms.DockStyle.Fill;//���ڴ�С
                l.FormBorderStyle = FormBorderStyle.None;//û�б�����
                this.panelRight.Controls.Add(l);
                l.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "���ⵥ����")
            {
                this.panelRight.Controls.Clear();//���
                FormPutOutStorageTicket l = new FormPutOutStorageTicket(this.user.ID, this.project.ID, this.warehouse.ID);//ʵ�����Ӵ���
                l.TopLevel = false;
                l.Dock = System.Windows.Forms.DockStyle.Fill;//���ڴ�С
                l.FormBorderStyle = FormBorderStyle.None;//û�б�����
                this.panelRight.Controls.Add(l);
                l.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "�������")
            {
                this.panelRight.Controls.Clear();//���
                var formBaseStock = new FormStockInfo(this.user.ID, this.project.ID, this.warehouse.ID);//ʵ�����Ӵ���
                formBaseStock.TopLevel = false;
                formBaseStock.Dock = DockStyle.Fill;//���ڴ�С
                formBaseStock.FormBorderStyle = FormBorderStyle.None;//û�б�����
                this.panelRight.Controls.Add(formBaseStock);
                formBaseStock.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "����̵�")
            {
                this.panelRight.Controls.Clear();//���
                var formBaseStock = new FormStockInfoCheckTicket(this.project.ID, this.warehouse.ID, this.user.ID);//ʵ�����Ӵ���
                formBaseStock.TopLevel = false;
                formBaseStock.Dock = DockStyle.Fill;//���ڴ�С
                formBaseStock.FormBorderStyle = FormBorderStyle.None;//û�б�����
                this.panelRight.Controls.Add(formBaseStock);
                formBaseStock.Show();
            }
            if (treeViewLeft.SelectedNode.Text == "�ͼ쵥����")
            {
                this.panelRight.Controls.Clear();//���
                var formSubmissionManage = new FormSubmissionManage(this.project.ID, this.warehouse.ID, this.user.ID);//ʵ�����Ӵ���
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
                    SetTreeViewSelectedNodeByText("�ϼܵ�����");
                    Utilities.SendMessage(this.panelRight.Handle, Utilities.WM_SETREDRAW, 1, 0);
                }));
                formSubmissionManage.TopLevel = false;
                formSubmissionManage.Dock = DockStyle.Fill;//���ڴ�С
                formSubmissionManage.FormBorderStyle = FormBorderStyle.None;//û�б�����
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
                FormJobTicket formJobTicket = new FormJobTicket(this.user.ID, this.project.ID, this.warehouse.ID);//ʵ�����Ӵ���
                formJobTicket.SetToPutOutStorageTicketCallback(this.ToPutOutStorageTicketCallback);
                formJobTicket.SetSearchCondition(condition, value);
                this.LoadSubWindow(formJobTicket);
                this.SetTreeViewSelectedNodeByText("������ҵ������");
            }));
        }

        private void ToPutOutStorageTicketCallback(string condition, string jobTicketNo)
        {
            if (this.IsDisposed) return;
            this.Invoke(new Action(() =>
            {
                FormPutOutStorageTicket formPutOutStorageTicket = new FormPutOutStorageTicket(this.user.ID, this.project.ID, this.warehouse.ID);//ʵ�����Ӵ���
                formPutOutStorageTicket.SetSearchCondition(condition, jobTicketNo);
                this.LoadSubWindow(formPutOutStorageTicket);
                this.SetTreeViewSelectedNodeByText("���ⵥ����");
            }));
        }

        private void LoadSubWindow(Form form)
        {
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;//���ڴ�С
            form.FormBorderStyle = FormBorderStyle.None;//û�б�����
            this.panelRight.Controls.Clear();//���
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
            if (MessageBox.Show("ȷ���˳���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void SetTreeViewSelectedNodeByText(string text)
        {
            TreeNode node = this.FindTreeNodeByText(this.treeViewLeft.Nodes, text);
            if (node == null)
            {
                throw new Exception("���ο��в������ڵ㣺" + text);
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

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
                MakeTreeNode("������Ϣ",new TreeNode[]{
                    MakeTreeNode("�û�����"),
                    MakeTreeNode("��Ӧ�̹���"),
                    MakeTreeNode("���Ϲ���"),
                    MakeTreeNode("�ֿ����"),
                    MakeTreeNode("��������"),
                    MakeTreeNode("��λ����"),
                    MakeTreeNode("��������"),
                    MakeTreeNode("�����ײ͹���"),
                    MakeTreeNode("�ϼܿ������"),
                    MakeTreeNode("�����������")                    
                    }),
                MakeTreeNode("������",new TreeNode[]{
                    MakeTreeNode("��ⵥ����"),
                    MakeTreeNode("�ͼ쵥����"),
                    MakeTreeNode("�ϼܵ�����")
                    }),
                MakeTreeNode("��������",new TreeNode[]{
                    MakeTreeNode("������ҵ������"),
                    MakeTreeNode("���ⵥ����")
                    //MakeTreeNode("�������񵥹���"),

                    }),
                MakeTreeNode("������",new TreeNode[]{
                    MakeTreeNode("�������"),
                    MakeTreeNode("����̵�"),
                    MakeTreeNode("��λ��¼")
                    }),
                 MakeTreeNode("н�����",new TreeNode[]{
                    MakeTreeNode("н�����"),
                    MakeTreeNode("н���ڼ�"),              
                    MakeTreeNode("��Աн��"),
                    MakeTreeNode("н�ʷ��ŵ�")
                    }),
                 MakeTreeNode("���˹���",new TreeNode[]{
                    MakeTreeNode("��Ŀ����"),
                    MakeTreeNode("˰�����"),
                    MakeTreeNode("��Ŀ��¼"),
                    MakeTreeNode("����ڼ�")
                    }),
                 MakeTreeNode("�������",new TreeNode[]{
                   MakeTreeNode("���ܵ�����"),
                   MakeTreeNode("���㵥����"),
                   MakeTreeNode("��Ʊ����")
                    })
            };

            this.treeViewLeft.Nodes.Clear();
            TreeNode[] nodes = treeNodes.ToArray();
            this.treeViewLeft.Nodes.AddRange(nodes);
        }

        ////����û��Ƿ�����Ӧ���ܵ�Ȩ��
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

        ////��ȡ��Ȩ�޵������ӽڵ�
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
            //ˢ��������ο�
            this.RefreshTreeView();

            //ˢ�¶���
            this.labelUsername.Text = GlobalData.Person["name"].ToString();
            this.labelAuth.Text = GlobalData.Person["role"].ToString() + " :";

            //�����С������ʾ���ı�
            int DeskWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int DeskHeight = Screen.PrimaryScreen.WorkingArea.Height;
            this.Width = Convert.ToInt32(DeskWidth * 0.8);
            this.Height = Convert.ToInt32(DeskHeight * 0.8);

            //ˢ�²ֿ�
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
            //    //��������ʾ�ֿ�
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
                case "�û�����":
                    this.LoadSubWindow(new FormPerson());
                    break;
                case "��ⵥ����":
                    this.LoadSubWindow(new FormWarehouseEntry(ToInspectionNoteSelectIDsCallback, ToInspectionNoteSearchNoCallback));
                    break;
                case "�ͼ쵥����":
                    this.LoadSubWindow(new FormInspectionNote());
                    break;
                case "�ϼܵ�����":
                    this.LoadSubWindow(new FormPutAwayNote());
                    break;
                case "��Ӧ�̹���":
                    this.LoadSubWindow(new FormSupplier());
                    break;
                case "��������":
                    this.LoadSubWindow(new FormSupply());
                    break;
                case "���Ϲ���":
                    this.LoadSubWindow(new FormMaterial());
                    break;
                case "�����������":
                    this.LoadSubWindow(new FormSafetyStock(1));
                    break;
                case "�ϼܿ������":
                    this.LoadSubWindow(new FormSafetyStock(0));
                    break;
                case "�����ײ͹���":
                    this.LoadSubWindow(new FormPackage());
                    break;
                case "�ֿ����":
                    this.LoadSubWindow(new FormWarehouse(this.comboBoxWarehouse,this.panelRight,this.treeViewLeft));
                    break;
                case "��������":
                    this.LoadSubWindow(new FormStorageArea());
                    break;
                case "��λ����":
                    this.LoadSubWindow(new FormStorageLocation());
                    break;
                case "����̵�":
                    this.LoadSubWindow(new FormStockTakingOrder());
                    break;
                case "�������":
                    this.LoadSubWindow(new FormStockRecord());
                    break;
                case "��λ��¼":
                    this.LoadSubWindow(new FormTransferRecord());
                    break;
                case "���ⵥ����":
                    this.LoadSubWindow(new FormDeliverOrder());
                    break;
                case "������ҵ������":
                    this.LoadSubWindow(new FormTransferOrder.FormTransferOrder());
                    break;
                case "н�����":
                    this.LoadSubWindow(new FromSalary.FormSalaryType());
                    break;
                case "н���ڼ�":
                    this.LoadSubWindow(new FromSalary.FormSalaryPeriod());
                    break;
                 case "��Աн��":
                    this.LoadSubWindow(new FromSalary.FormPersonSalary());
                    break;
                case "н�ʷ��ŵ�":
                    this.LoadSubWindow(new FromSalary.FormPayNote());
                    break;
                case "��Ŀ����":
                    this.LoadSubWindow(new FormAcccount.FormAccountTitle());
                    break;
                case "˰�����":
                    this.LoadSubWindow(new FormAcccount.FormTax());
                    break;
                case "��Ŀ��¼":
                    this.LoadSubWindow(new FormAcccount.FormAccountRecord());
                    break;
                case "����ڼ�":
                    this.LoadSubWindow(new FormAcccount.FormAccountPeriod());
                    break;
                case "���ܵ�����":
                    this.LoadSubWindow(new FormSettlement.FormSummaryNote());
                    break;
                case "���㵥����":
                    this.LoadSubWindow(new FormSettlement.FormSettlementNote());
                    break;
                case "��Ʊ����":
                    this.LoadSubWindow(new FormSettlement.FormInvoice());
                    break;
            }
            this.panelRight.ResumeLayout();
            this.panelRight.Show();
        }

        private void ToInspectionNoteSelectIDsCallback(int[] selectedIDs)
        {
            this.SetTreeViewSelectedNodeByText("�ͼ쵥����");
            this.LoadSubWindow(new FormInspectionNote(selectedIDs));
        }

        private void ToInspectionNoteSearchNoCallback(string searchNo)
        {
            this.SetTreeViewSelectedNodeByText("�ͼ쵥����");
            this.LoadSubWindow(new FormInspectionNote(null,searchNo));
        }

        //private void ToJobTicketCallback(string condition, string value)
        //{
        //    if (this.IsDisposed) return;
        //    this.Invoke(new Action(() =>
        //    {
        //        FormJobTicket formJobTicket = new FormJobTicket(this.user.ID, this.project.ID, this.warehouse.ID);//ʵ�����Ӵ���
        //        formJobTicket.SetToPutOutStorageTicketCallback(this.ToPutOutStorageTicketCallback);
        //        formJobTicket.SetSearchCondition(condition, value);
        //        this.LoadSubWindow(formJobTicket);
        //        this.SetTreeViewSelectedNodeByText("������ҵ������");
        //    }));
        //}

        //private void ToPutOutStorageTicketCallback(string condition, string jobTicketNo)
        //{
        //    if (this.IsDisposed) return;
        //    this.Invoke(new Action(() =>
        //    {
        //        FormPutOutStorageTicket formPutOutStorageTicket = new FormPutOutStorageTicket(this.user.ID, this.project.ID, this.warehouse.ID);//ʵ�����Ӵ���
        //        formPutOutStorageTicket.SetSearchCondition(condition, jobTicketNo);
        //        this.LoadSubWindow(formPutOutStorageTicket);
        //        this.SetTreeViewSelectedNodeByText("���ⵥ����");
        //    }));
        //}

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

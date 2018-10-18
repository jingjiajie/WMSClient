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
        private SingletonManager<Form> formManager = new SingletonManager<Form>();
        private Action formClosedCallback;

        public FormMain()
        {
            InitializeComponent();
        }

        public void SetFormClosedCallback(Action callback)
        {
            this.formClosedCallback = callback;
        }

        private void InitTreeView()
        {
            TreeNode[] treeNodes = new TreeNode[]
            {
                MakeTreeNode("基本信息",null,new TreeNode[]{
                    MakeTreeNode("用户管理", "FormPerson"),
                    MakeTreeNode("供应商管理", "FormSupplier"),
                    MakeTreeNode("物料管理", "FormMaterial"),
                    MakeTreeNode("仓库管理", "FormWarehouse"),
                    MakeTreeNode("库区管理", "FormStorageArea"),
                    MakeTreeNode("库位管理", "FormStorageLocation"),
                    MakeTreeNode("供货管理", "FormSupply"),
                    MakeTreeNode("发货套餐管理", "FormPackage"),
                    MakeTreeNode("上架库存设置", "FormSafetyStock0"),
                    MakeTreeNode("备货库存设置","FormSafetyStock1")
                    }),
                MakeTreeNode("入库管理", null, new TreeNode[]{
                    MakeTreeNode("入库单管理", "FormWarehouseEntry"),
                    MakeTreeNode("送检单管理", "FormInspectionNote"),
                    MakeTreeNode("上架单管理", "FormPutAwayNote")
                    }),
                MakeTreeNode("发货管理", null, new TreeNode[]{
                    MakeTreeNode("备货作业单管理", "FormTransferOrder"),
                    MakeTreeNode("出库单管理", "FormDeliverOrder")

                    }),
                MakeTreeNode("库存管理", null, new TreeNode[]{
                    MakeTreeNode("库存批次", "FormStockRecord"),
                    MakeTreeNode("库存盘点","FormStockTakingOrder"),
                    MakeTreeNode("移位记录", "FormTransferRecord")
                    }),
                 MakeTreeNode("薪金管理",null ,new TreeNode[]{
                    MakeTreeNode("薪金类别","FormSalaryType"),
                    MakeTreeNode("薪金期间", "FormSalaryPeriod"),
                    MakeTreeNode("人员薪金", "FormPersonSalary"),
                    MakeTreeNode("薪资发放单", "FormPayNote")
                    }),
                 MakeTreeNode("总账管理", null, new TreeNode[]{
                    MakeTreeNode("科目管理","FormAccountTitle"),
                    MakeTreeNode("税务管理","FormTax"),
                    MakeTreeNode("账目记录", "FormAccountRecord"),
                    MakeTreeNode("会计期间", "FormAccountPeriod")
                    }),
                 MakeTreeNode("结算管理",null, new TreeNode[]{
                   MakeTreeNode("汇总单管理","FormSummaryNote"),
                   MakeTreeNode("结算单管理","FormSettlementNote"),
                   MakeTreeNode("发票管理", "FormInvoice"),
                   MakeTreeNode("价格管理", "FormPrice")
                    })
            };

            this.treeViewLeft.Nodes.Clear();
            TreeNode[] nodes = treeNodes.ToArray();
            this.treeViewLeft.Nodes.AddRange(nodes);
        }

        private void InitFormManager(SingletonManager<Form> formManager)
        {
            formManager.ClearInstances();
            formManager.Set("FormPerson", () => new FormPerson());
            formManager.Set("FormSupplier", () => new FormSupplier());
            formManager.Set("FormMaterial", () => new FormMaterial());
            formManager.Set("FormWarehouse", () => new FormWarehouse(this.comboBoxWarehouse, this.panelRight, this.treeViewLeft));
            formManager.Set("FormStorageArea", () => new FormStorageArea());
            formManager.Set("FormStorageLocation", () => new FormStorageLocation());
            formManager.Set("FormSupply", () => new FormSupply());
            formManager.Set("FormPackage", () => new FormPackage());
            formManager.Set("FormSafetyStock0", () => new FormSafetyStock(0));
            formManager.Set("FormSafetyStock1", () => new FormSafetyStock(1));
            formManager.Set("FormWarehouseEntry", () => new FormWarehouseEntry(ToInspectionNoteSelectIDsCallback, ToInspectionNoteSearchNoCallback));
            formManager.Set("FormInspectionNote", () => new FormInspectionNote());
            formManager.Set("FormPutAwayNote", () => new FormPutAwayNote());
            formManager.Set("FormTransferOrder", () => new FormTransferOrder.FormTransferOrder());
            formManager.Set("FormDeliverOrder", () => new FormDeliverOrder());
            formManager.Set("FormStockRecord", () => new FormStockRecord());
            formManager.Set("FormStockTakingOrder", () => new FormStockTakingOrder());
            formManager.Set("FormTransferRecord", () => new FormTransferRecord());
            formManager.Set("FormSalaryType", () => new FromSalary.FormSalaryType());
            formManager.Set("FormSalaryPeriod", () => new FromSalary.FormSalaryPeriod());
            formManager.Set("FormPersonSalary", () => new FromSalary.FormPersonSalary());
            formManager.Set("FormPayNote", () => new FromSalary.FormPayNote());
            formManager.Set("FormAccountTitle", () => new FormAcccount.FormAccountTitle());
            formManager.Set("FormTax", () => new FormAcccount.FormTax());
            formManager.Set("FormAccountRecord", () => new FormAcccount.FormAccountRecord());
            formManager.Set("FormAccountPeriod", () => new FormAcccount.FormAccountPeriod());
            formManager.Set("FormSummaryNote", () => new FormSettlement.FormSummaryNote());
            formManager.Set("FormSettlementNote", () => new FormSettlement.FormSettlementNote());
            formManager.Set("FormInvoice", () => new FormSettlement.FormInvoice());
            formManager.Set("FormPrice", () => new FormSettlement.FormPrice());
        }

        private static TreeNode MakeTreeNode(string text, string formName, TreeNode[] subNodes = null)
        {
            TreeNode node = new TreeNode()
            {
                Text = text,
                Tag = new TreeNodeTag(formName)
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
            this.InitTreeView();
            //刷新窗口管理器
            this.InitFormManager(this.formManager);

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
            if (tag.FormName != null)
            {
                this.panelRight.Hide();
                this.panelRight.SuspendLayout();
                this.LoadSubWindow(tag.FormName);
                this.panelRight.ResumeLayout();
                this.panelRight.Show();
            }
        }

        private void LoadSubWindow(string formName)
        {
            Form form = this.formManager.Get(formName);
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
            FormInspectionNote formInspectionNote = (FormInspectionNote)this.formManager.Get("FormInspectionNote");
            this.LoadSubWindow("FormInspectionNote");
            formInspectionNote.SearchAndSelectByIDs(selectedIDs);
        }

        private void ToInspectionNoteSearchNoCallback(string searchNo)
        {
            this.SetTreeViewSelectedNodeByText("送检单管理");
            FormInspectionNote formInspectionNote = (FormInspectionNote)this.formManager.Get("FormInspectionNote");
            this.LoadSubWindow("FormInspectionNote");
            formInspectionNote.SearchByWarehouseEntryNo(searchNo);
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
            GlobalData.AllDate = RestClient.Get<IDictionary<string, object[]>>(
           Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/refreshGlobalDate" + "/" + GlobalData.Warehouse["id"]);
            foreach (KeyValuePair<string, Object[]> allData in GlobalData.AllDate)
            {
                string key = allData.Key;
                Object[] data = allData.Value;
                var list = (from x in data.ToList() select (IDictionary<string, object>)x).ToList();
                switch (key)
                {
                    case "AllSuppliers":
                        GlobalData.AllSuppliers = list;
                        break;
                    case "AllMaterial":
                        GlobalData.AllMaterials = list;
                        break;
                    case "AllSupply":
                        GlobalData.AllSupplies = list;
                        break;
                    case "AllStoaregLocation":
                        GlobalData.AllStorageLocations = list;
                        break;
                    case "AllStorageArea":
                        GlobalData.AllStorageAreas = list;
                        break;
                    case "AllPersons":
                        GlobalData.AllPersons = list;
                        break;
                    case "AllPackage":
                        GlobalData.AllPackage = list;
                        break;
                    case "AllSalaryItem":
                        GlobalData.AllSalaryItem = list;
                        break;
                    case "AllSalaryType":
                        GlobalData.AllSalaryType = list;
                        break;
                    case "AllSummaryNote":
                        GlobalData.AllSummaryNote = list;
                        break;
                    case "AllSalaryPeriod":
                        GlobalData.AllSalaryPeriod = list;
                        break;
                    case "AllAccountTitle":
                        GlobalData.AllAccountTitle = list;
                        break;
                    case "AllAccountTitleTrue":
                        GlobalData.AllAccountTitleTure = list;
                        break;
                    case "AllTax":
                        GlobalData.AllTax = list;
                        break;
                    case "AllAccountPeriod":
                        GlobalData.AllAccountPeriod = list;
                        break;
                    case "AccountPeriod":
                        if (list.Count == 1)
                        {
                            GlobalData.AccountPeriod = list[0];
                        }
                        else
                        {
                            GlobalData.AccountPeriod = null;
                        }
                        break;
                }
            }
            //Condition condWarehouse = new Condition().AddCondition("warehouseId", GlobalData.Warehouse["id"]);
            //GlobalData.AllSuppliers = RestClient.Get<List<IDictionary<string, object>>>(
            //   $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/supplier/{condWarehouse.ToString()}/new");

            //GlobalData.AllMaterials = RestClient.Get<List<IDictionary<string, object>>>(
            //   $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/material/{condWarehouse.ToString()}");

            //GlobalData.AllSupplies = RestClient.Get<List<IDictionary<string, object>>>(
            //    $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/supply/{condWarehouse.ToString()}");

            //GlobalData.AllStorageLocations = RestClient.Get<List<IDictionary<string, object>>>(
            //   $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/storage_location/{condWarehouse.ToString()}");

            //GlobalData.AllStorageAreas = RestClient.Get<List<IDictionary<string, object>>>(
            //   $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/storage_area/{condWarehouse.ToString()}");

            //GlobalData.AllPersons = RestClient.Get<List<IDictionary<string, object>>>(
            //   $"{Defines.ServerURL}/ledger/{GlobalData.AccountBook}/person/{{}}");

            //GlobalData.AllPackage = RestClient.Get<List<IDictionary<string, object>>>(
            //  $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/package/{condWarehouse.ToString()}");

            //GlobalData.AllSalaryItem = RestClient.Get<List<IDictionary<string, object>>>(
            //$"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/salary_item/{condWarehouse.ToString()}");

            //GlobalData.AllSalaryType = RestClient.Get<List<IDictionary<string, object>>>(
            // $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/salary_type/{condWarehouse.ToString()}");

            //GlobalData.AllSummaryNote = RestClient.Get<List<IDictionary<string, object>>>(
            // $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/summary_note/{condWarehouse.ToString()}");

            //GlobalData.AllSalaryPeriod = RestClient.Get<List<IDictionary<string, object>>>(
            //$"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/salary_period/{condSalaryPeriod.AddOrder("endTime", OrderItemOrder.DESC).ToString()}");

            //GlobalData.AllAccountPeriod = RestClient.Get<List<IDictionary<string, object>>>(
            //  $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/account_period/{condWarehouse.AddOrder("startTime", OrderItemOrder.DESC).ToString()}");

            //try
            //{
            //    GlobalData.AccountPeriod = RestClient.Get<List<IDictionary<string, object>>>(
            //       $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/account_period/{condWarehouse.AddCondition("ended", 0).AddOrder("startTime", OrderItemOrder.DESC).ToString()}")[0];
            //}
            //catch { GlobalData.AccountPeriod = null; }
            GlobalData.REMAINDENABLE = true;
            //清除窗口管理器缓存的窗口实例
            this.formManager.ClearInstances();
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
    public string FormName;

    public TreeNodeTag(string formName)
    {
        FormName = formName;
    }
}

class SingletonManager<T> where T:class
{
    class ObjectInfo
    {
        public T Obj { get; set; }
        public Func<T> FuncCreateObject { get; set; }
    }

    private IDictionary<string, ObjectInfo> objs = new Dictionary<string, ObjectInfo>();
    public void Set(string name, Func<T> funcCreateObj)
    {
        objs[name] = new ObjectInfo()
        {
            FuncCreateObject = funcCreateObj
        };
    }

    public T Get(string name)
    {
        if(this.objs.ContainsKey(name) == false)
        {
            return default(T);
        }
        ObjectInfo info = this.objs[name];
        if (info.Obj != null)
        {
            return info.Obj;
        }
        else
        {
            info.Obj = info.FuncCreateObject();
            return info.Obj;
        }
    }

    public void ClearInstances()
    {
        foreach (var item in this.objs)
        {
            item.Value.Obj = default(T);
        }
    }
}
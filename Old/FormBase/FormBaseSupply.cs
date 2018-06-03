using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using unvell.ReoGrid;
using WMS.DataAccess;
using System.Threading;
using System.Data.SqlClient;
using System.Reflection;
using System.Data.Entity;

namespace WMS.UI
{
    public partial class FormBaseSupply : Form
    {        
        private WMSEntities wmsEntities = new WMSEntities();
        private int authority;
        private int authority_self = (int)Authority.BASE_COMPONENT;
        int supplierID = -1;
        private int check_history = 0;
        int projectID = -1;
        int warehouseID = -1;
        int userID = -1;
        string textSearchValue = null;
        object itemComboBoxSelect = 0;
        private Supplier supplier = null;
        private string  contractst;   //合同状态
        private int contract_change = 1;
        private int setitem = -1;
        private PagerWidget<SupplyView> pagerWidget = null;
        private SearchWidget<SupplyView> searchWidget = null;

        public FormBaseSupply(int authority, int supplierID, int projectID, int warehouseID, int userID)
        {
            InitializeComponent();
            this.authority = authority;
            this.supplierID = supplierID;
            this.projectID = projectID;
            this.warehouseID = warehouseID;
            this.userID = userID;
        }
        private void InitSupplys()
        {
            string[] visibleColumnNames = (from kn in SupplyViewMetaData.supplykeyNames
                                           where kn.Visible == true
                                           select kn.Name).ToArray();

            //初始化


            this.pagerWidget = new PagerWidget<SupplyView>(this.reoGridControlSupply, SupplyViewMetaData.supplykeyNames, this.projectID, this.warehouseID);
            this.panelPager.Controls.Add(pagerWidget);
            this.pagerWidget.AddOrderBy("ID");
            pagerWidget.Show();

            this.searchWidget = new SearchWidget<SupplyView>(SupplyViewMetaData.KeyNames, this.pagerWidget);
            this.pagerWidget.AddStaticCondition("IsHistory", "0");
            //this.searchWidget.SetOrderByCondition("CreateTime",false);
            this.panelSearchWidget.Controls.Add(searchWidget);


        }

        private void FormBaseSupply_Load(object sender, EventArgs e)
        {
            if(supplierID !=-1)//改成-1了，刚发现数据库里供应商ID可能为0
            { this.toolStripButtonAdd.Enabled = false;
                this.toolStripButtonDelete.Enabled = false;
                this.toolStripButtonAlter.Enabled  = false;
            }

            if ((this.authority & authority_self) != authority_self)
            {
                
                Supplier supplier = (from u in this.wmsEntities.Supplier
                                     where u.ID == supplierID
                                     select u)  .FirstOrDefault ();
                this.supplier = supplier;
                this.contractst = (supplier.ContractState);
                this.toolStripButtonAdd.Enabled = false;
                this.toolStripButtonDelete.Enabled = false;
                this.buttonImport.Enabled = false;

                if (this.contractst == "待审核")
                {
                    this.toolStripButtonAlter.Enabled = true ;

                }

                InitSupplys();

                this.pagerWidget.AddCondition("ID", Convert.ToString(supplierID));
                this.pagerWidget.AddCondition("IsHistory", "0");
                this.pagerWidget.Search();

            }
            if ((this.authority & authority_self) == authority_self)
            {
                InitSupplys();
                this.pagerWidget.AddCondition("IsHistory", "0");
                this.pagerWidget.Search();
            }
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
        private void reoGridControlUser_Click(object sender, EventArgs e)
        {

        }

        //private void buttonSearch_Click(object sender, EventArgs e)
        //{

        //    if (this.buttonSearch.Text == "全部信息")
        //    {
        //        this.buttonSearch.Text = "查询";
        //        this.buttonSearch.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
        //        this.toolStripButtonAdd.Enabled = true;
        //        this.toolStripButtonAlter.Enabled = true;
        //        this.toolStripComboBoxSelect.Enabled = true;
        //        this.buttonImport.Enabled = true;
        //        this.buttonHistorySearch.Visible = true;

        //        this.toolStripComboBoxSelect.SelectedItem = itemComboBoxSelect;
        //        this.textBoxSearchValue.Text = textSearchValue;
        //        if (this.contractst == "待审核")
        //        {
        //            this.toolStripButtonAdd.Enabled = false ;
                  
        //            this.toolStripComboBoxSelect.Enabled = true;
        //            this.buttonImport.Enabled = false ;
        //        }
        //        else if(this.contractst == "已过审")
        //        {
        //            this.toolStripButtonAdd.Enabled = false;
        //            this.toolStripButtonAlter.Enabled = false;
        //            this.toolStripComboBoxSelect.Enabled = true;
        //            this.buttonImport.Enabled = false;
        //        }
        //    }

        //    this.pagerWidget.ClearCondition();
        //    this.pagerWidget.AddCondition("IsHistory", "0");

        //    if (this.toolStripComboBoxSelect.SelectedIndex != 0)
        //    {
        //        this.pagerWidget.AddCondition(this.toolStripComboBoxSelect.SelectedItem.ToString(), this.textBoxSearchValue.Text);
        //    }
        //    if ((this.authority & authority_self) != authority_self)
        //    {
        //        this.pagerWidget.AddCondition("ID", Convert.ToString(supplierID));
        //        this.check_history = 0;
        //        this.pagerWidget.Search();
        //    }
        //    if ((this.authority & authority_self) == authority_self)
        //    {
        //        this.check_history = 0;
        //        this.pagerWidget.Search();
        //    }
        //}


        private void buttonHistorySearch_Click(object sender, EventArgs e)
        {
            if (check_history == 1)
            {
                this.buttonHistorySearch.Text = "查看历史信息";
                check_history = 0;
                this.toolStripButtonAdd.Enabled = true;
                this.toolStripButtonAlter.Enabled = true;
                this.buttonImport.Enabled = true;

                this.pagerWidget.ClearCondition();
                this.pagerWidget.ClearStaticCondition();
                this.pagerWidget.AddStaticCondition("IsHistory", "0");
                this.pagerWidget.Search();
            }
            else
            {
                this.toolStripButtonAdd.Enabled = false;
                this.toolStripButtonAlter.Enabled = false;
                this.buttonImport.Enabled = false;
                this.buttonHistorySearch.Text = "显示全部信息";

                this.pagerWidget.ClearCondition();
                this.pagerWidget.ClearStaticCondition();
                var worksheet = this.reoGridControlSupply.Worksheets[0];
                try
                {
                    if (worksheet.SelectionRange.Rows != 1)
                    {
                        throw new Exception();
                    }
                    int componenID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                    this.pagerWidget.AddCondition("NewestSupplyID", Convert.ToString(componenID));
                }

                catch
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.pagerWidget.AddStaticCondition("IsHistory", "1");

                //if (this.toolStripComboBoxSelect.SelectedIndex != 0)
                //{

                //    this.pagerWidget.AddCondition(this.toolStripComboBoxSelect.SelectedItem.ToString(), this.textBoxSearchValue.Text);
                //}
                if ((this.authority & authority_self) != authority_self)
                {
                    this.pagerWidget.AddCondition("SupplierID", Convert.ToString(supplierID));
                    this.check_history = 1;
                    this.pagerWidget.Search();
                }
                if ((this.authority & authority_self) == authority_self)
                {
                    this.check_history = 1;
                    this.pagerWidget.Search();
                }
            }
        }


        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormSupplyModify(this.projectID, this.warehouseID, this.supplierID, this.userID);
            form.SetMode(FormMode.ADD);
            form.SetAddFinishedCallback((addedID) =>
            {
                this.pagerWidget.Search(false,addedID);
            });
            form.Show();

        }//添加
  

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            wmsEntities = new WMSEntities();
            Worksheet worksheet = this.reoGridControlSupply.Worksheets[0];
            if (worksheet.SelectionRange.Rows == 1)
            {
                try
                {
                    int componenID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                    var formSupplyModify = new FormSupplyModify(this.projectID, this.warehouseID, this.supplierID, this.userID, componenID);
                    formSupplyModify.SetModifyFinishedCallback((addedID) =>
                    {
                        this.pagerWidget.Search(false, addedID);
                    });
                    formSupplyModify.Show();
                }
                catch
                {
                    MessageBox.Show("当前选中空白条目，请重新选择要修改的条目进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }

            if (worksheet.SelectionRange.Rows != 1)
            {
                Object alterData = worksheet[worksheet.SelectionRange.Row, 0, worksheet.SelectionRange.Rows,1];
                var alterDatas = new SupplyView[worksheet.SelectionRange.Rows];
                for (int i = 0; i < worksheet.SelectionRange.Rows; i++)
                {
                    try
                    {
                        int curSupplyID = int.Parse(worksheet[worksheet.SelectionRange.Row + i, 0].ToString());
                        var curSupply = (from u in wmsEntities.SupplyView
                                         where
                                         u.ID == curSupplyID
                                         select u).FirstOrDefault();
                        if (curSupply == null)
                        {
                            MessageBox.Show("供货信息不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        alterDatas[i] = curSupply;
                        //alterDatas[i] = worksheet[worksheet.SelectionRange.Row+i, 0,1, worksheet.SelectionRange.Cols];
                    }
                    catch
                    {
                        MessageBox.Show("当前选中空白条目，请重新选择要修改的条目进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }


            int newImportID = -1;
                List<int> Removei = new List<int>();
                //创建导入窗口
                StandardImportForm<Supply> formImport =
                    new StandardImportForm<Supply>
                    (
                        //参数1：KeyName
                        SupplyViewMetaData.importsupplykeyNames,
                        (results, unimportedColumns) => //参数2：导入数据二次处理回调函数
                    {


                            string[][] supplierNamesCount = (from kn in unimportedColumns
                                                             where kn.Key == "SupplierName"
                                                             select kn.Value).ToArray();
                            string[][] componentNamesCount = (from kn in unimportedColumns
                                                              where kn.Key == "ComponentName"
                                                              select kn.Value).ToArray();

                            DialogResult messageBoxResult = DialogResult.No;//设置对话框的返回值
                        DialogResult allMessageBoxResult = DialogResult.No;
                        bool allSave = false;
                        bool allIgnore = true;
                        bool choose = false;
                        string[] suppliernames;
                            suppliernames = supplierNamesCount[0];
                            string[] componentnames;
                            componentnames = componentNamesCount[0];
                            for (int i = 0; i < results.Count; i++)
                            {
                                Supply supply1 = null;
                                string supplierName;
                                string componentName;
                                string no;
                                no = results[i].No;
                                supplierName = suppliernames[i];
                                componentName = componentnames[i];
                                int importcomponenID = -1;
                                int importsupplierID = -1;

                                if (supplierName.Length == 0)
                                {
                                    MessageBox.Show("第" + (i + 1) + "行供货商名称不存在，请重新确认后输入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                    return false;
                                }

                                if (componentName.Length == 0)
                                {
                                    MessageBox.Show("第" + (i + 1) + "行零件名称不存在，请重新确认后输入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                    return false;
                                }
                                if (results[i].No.Length == 0)
                                {
                                    MessageBox.Show("第" + (i + 1) + "行代号不能为空！请重新确认后输入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                    return false;
                                }

                            //检查导入列表中是否重名
                            for (int j = i + 1; j < results.Count; j++)
                                {
                                    if (results[i].No == results[j].No)
                                    {
                                        MessageBox.Show("您输入的代号：" + results[i].No + "条目在导入列表中重复", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                        return false;
                                    }
                                }
                            //检查数据库中同名
                            try
                                {
                                    try
                                    {
                                        Supplier supplierID = (from s in this.wmsEntities.Supplier where s.Name == supplierName && s.IsHistory == 0 select s).FirstOrDefault();
                                        importsupplierID = supplierID.ID;
                                    }
                                    catch
                                    {
                                        MessageBox.Show("您输入的第" + (i + 1) + "行供货商名称：" + supplierName + "不存在，请重新确认后输入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                        return false;
                                    }


                                    try
                                    {
                                        DataAccess.Component componenID1 = (from s in this.wmsEntities.Component where s.Name == componentName select s).FirstOrDefault();
                                        importcomponenID = componenID1.ID;
                                    }
                                    catch
                                    {
                                        MessageBox.Show("您输入的第" + (i + 1) + "行零件名称：" + componentName + "不存在，请重新确认后输入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                        return false;
                                    }

                                    var sameNameSupply = (from u in wmsEntities.Supply
                                                          where
                                                      u.No == no
                                                      && u.ProjectID == this.projectID && u.WarehouseID == this.warehouseID
                                                      && u.IsHistory == 0
                                                          select u).ToArray();
                                    if (sameNameSupply.Length > 0)
                                    {
                                    if (allSave != true&& choose == false)
                                    {
                                        allMessageBoxResult = MessageBox.Show("已存在相同代号供货条目,是否要全部保留历史信息?", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation,

                                        MessageBoxDefaultButton.Button2);
                                        if (allMessageBoxResult == DialogResult.Yes)
                                        {
                                            allSave = true;
                                            allIgnore = true;
                                        }
                                        if (allMessageBoxResult == DialogResult.No)
                                        {
                                            allSave = false;
                                            allIgnore = true;
                                        }
                                        if (allMessageBoxResult == DialogResult.Cancel)
                                        {
                                            allSave = false;
                                            allIgnore = false;
                                        }
                                            choose = true;
                                    }

                                    if (allSave == true)
                                    {
                                        messageBoxResult = DialogResult.Yes;
                                    }
                                    else
                                    {
                                        messageBoxResult = DialogResult.No;
                                    }
                                    if (allIgnore != true)
                                    {

                                        messageBoxResult = MessageBox.Show("已存在相同代号供货条目：" + no + ",是否要保留历史信息?", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation,

                                            MessageBoxDefaultButton.Button2);
                                    }
                                    if (messageBoxResult == DialogResult.Cancel)
                                        {
                                            return false;
                                        }
                                        else
                                        {
                                            Removei.Add(i);
                                        }
                                        Supply supply = null;
                                        if (messageBoxResult == DialogResult.Yes)
                                        {


                                            try
                                            {
                                                supply = (from s in this.wmsEntities.Supply
                                                          where s.No == no
                                                           && s.ProjectID == this.projectID && s.WarehouseID == this.warehouseID
                                                           && s.IsHistory == 0
                                                          select s).FirstOrDefault();
                                            }
                                            catch
                                            {
                                                MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                return false;
                                            }
                                            if (supply == null)
                                            {
                                                MessageBox.Show("历史零件信息不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                return false;
                                            }
                                            newImportID = supply.ID;
                                            this.wmsEntities.Supply.Add(supply);

                                            try
                                            {
                                                supply.ID = -1;
                                                supply.IsHistory = 1;
                                                supply.NewestSupplyID = newImportID;
                                                supply.LastUpdateUserID = this.userID;
                                                supply.LastUpdateTime = DateTime.Now;
                                                wmsEntities.SaveChanges();
                                            }
                                            catch
                                            {
                                                MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                return false;
                                            }

                                            var supplystorge = (from u in wmsEntities.Supply
                                                                where u.NewestSupplyID == newImportID
                                                                select u).ToArray();


                                            if (supplystorge.Length > 0&&allSave!=true)
                                            {
                                                MessageBox.Show("历史信息保留成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                        }
                                        try
                                        {
                                            supply1 = (from s in this.wmsEntities.Supply
                                                       where s.No == no
                                                        && s.ProjectID == this.projectID && s.WarehouseID == this.warehouseID
                                                        && s.IsHistory == 0
                                                       select s).FirstOrDefault();
                                        }
                                        catch
                                        {
                                            MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return false;
                                        }
                                        if (supply1 == null)
                                        {
                                            MessageBox.Show("历史零件信息不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return false;
                                        }
                                        PropertyInfo[] proAs = results[i].GetType().GetProperties();
                                        PropertyInfo[] proBs = supply1.GetType().GetProperties();

                                        for (int k = 0; k < proAs.Length; k++)
                                        {
                                            for (int j = 0; j < proBs.Length; j++)
                                            {
                                                if (proAs[k].Name == proBs[j].Name && proBs[j].Name != "ID" && proBs[j].Name != "SupplierID" && proBs[j].Name != "ComponentID" && proBs[j].Name != "Component")

                                                {
                                                    object a = proAs[k].GetValue(results[i], null);
                                                    proBs[j].SetValue(supply1, a, null);
                                                }
                                            }
                                        }
                                        supply1.SupplierID = importsupplierID;
                                        supply1.ComponentID = importcomponenID;

                                        supply1.IsHistory = 0;
                                        supply1.CreateTime = DateTime.Now;
                                        supply1.CreateUserID = this.userID;
                                        supply1.LastUpdateTime = DateTime.Now;
                                        supply1.LastUpdateUserID = this.userID;
                                        supply1.WarehouseID = this.warehouseID;
                                        supply1.ProjectID = this.projectID;
                                        wmsEntities.SaveChanges();

                                    }

                                }
                                catch
                                {

                                    MessageBox.Show("操作失败，请检查网络连接4", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return false;
                                }


                                results[i].SupplierID = importsupplierID;
                                results[i].ComponentID = importcomponenID;

                                results[i].IsHistory = 0;
                                results[i].CreateTime = DateTime.Now;
                                results[i].CreateUserID = this.userID;
                                results[i].LastUpdateTime = DateTime.Now;
                                results[i].LastUpdateUserID = this.userID;
                                results[i].WarehouseID = this.warehouseID;
                                results[i].ProjectID = this.projectID;


                            //历史信息设为0
                            results[i].IsHistory = 0;


                            }
                            int length = Removei.ToArray().Length;
                        try
                        {
                            for (int b = 0; b < length; b++)
                            {
                                results.RemoveAt(Removei.ToArray()[b] - b);
                            }
                        }
                        catch
                        {
                            return false;
                        }
                            return true;

                        },
                        () => //参数3：导入完成回调函数
                    {
                            this.pagerWidget.Search();
                        }
                    );

                //显示导入窗口s
                formImport.PushData(alterDatas, SupplyViewMetaData.keyConvert);
                formImport.Text = "修改供货信息";
                formImport.Show();

            }


        }//修改

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlSupply.Worksheets[0];
            List<int> deleteIDs = new List<int>();
            for (int i = 0; i < worksheet.SelectionRange.Rows; i++)
            {
                try
                {
                    int curID = int.Parse(worksheet[i + worksheet.SelectionRange.Row, 0].ToString());
                    deleteIDs.Add(curID);
                }
                catch
                {
                    continue;
                }
            }
            if (deleteIDs.Count == 0)
            {
                MessageBox.Show("请选择您要删除的记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("您真的要删除这些记录吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            this.labelStatus.Text = "正在删除...";

            try
            {
                foreach (int id in deleteIDs)
                {

                    var receiptTicketItemcall = (from kn in wmsEntities.ReceiptTicketItem
                                      where kn.SupplyID == id
                                      
                                      select kn.ID).ToArray();
                    if (receiptTicketItemcall.Length > 0)
                    {
                        MessageBox.Show("删除失败，选择的供货信息被收货单引用，需要删除相应收货单信息才能删除。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }

                }

            }
            catch
            {
                MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        foreach (int id in deleteIDs)
                        {

                            var componen_historyid = (from kn in wmsEntities.Supply
                                                      where kn.NewestSupplyID == id
                                                      &&kn.ProjectID ==this.projectID 
                                                      &&kn.WarehouseID ==this.warehouseID 
                                                      select kn.ID).ToArray();
                            if (componen_historyid.Length > 0)
                            {
                                try
                                {
                                    foreach (int NewestSupplyid in componen_historyid)
                                    {
                                        wmsEntities.Database.ExecuteSqlCommand("DELETE FROM Supply WHERE ID = @componentID", new SqlParameter("componentID", NewestSupplyid));


                                    }
                                    wmsEntities.SaveChanges();
                                }
                                catch
                                {
                                    MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }

                        }

                    }
                    catch
                    {
                        MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    try
                    {
                        foreach (int id in deleteIDs)
                        {
                            this.wmsEntities.Database.ExecuteSqlCommand("DELETE FROM Supply WHERE ID = @componenID", new SqlParameter("componenID", id));
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    this.wmsEntities.SaveChanges();
                    this.Invoke(new Action(() =>
                    {
                        this.pagerWidget.Search();
                        MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                })).Start();


        }//删除

        //private void textBoxSearchValue_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == 13)
        //    {
        //        this.pagerWidget.Search();
        //    }
        //}

        //private void toolStripComboBoxSelect_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (this.toolStripComboBoxSelect.SelectedIndex == 0)
        //    {
        //        this.textBoxSearchValue.Text = "";
        //        this.textBoxSearchValue.Enabled = false;
        //        this.textBoxSearchValue.BackColor = Color.LightGray;
        //    }
        //    else
        //    {
        //        this.textBoxSearchValue.Enabled = true;
        //        this.textBoxSearchValue.BackColor = Color.White;
        //    }
        //}

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButtonSupplySingleBoxTranPackingInfo_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlSupply.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int ID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                this.setitem = 0;
                var form = new ComponentSingleBoxTranPackingInfoModify(this.userID, this.setitem,ID);
                if (check_history == 1)
                {
                    form.SetMode(FormMode.CHECK);
                }
                else
                {
                    form.SetMode(FormMode.ALTER);
                }
                form.SetModifyFinishedCallback((addedID) =>
                {
                    this.pagerWidget.Search(false, addedID);
                });
                form.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void toolStripButtonSupplyOuterPackingSize_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlSupply.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int componenID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                this.setitem = 0;
                var form = new ComponentOuterPackingSizeModify(this.userID, this.setitem, componenID);
                if (check_history == 1)
                {
                    form.SetMode(FormMode.CHECK);
                }
                else
                {
                    form.SetMode(FormMode.ALTER);
                }
                form.SetModifyFinishedCallback((addedID) =>
                {
                    this.pagerWidget.Search(false, addedID);
                });
                form.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void toolStripButtonSupplyShipmentInfo_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlSupply.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int componenID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                this.setitem = 0;
                var form = new ComponentShipmentInfoModify(this.userID,this.setitem, componenID);

                if (check_history == 1)
                {
                    form.SetMode(FormMode.CHECK);
                }
                else
                {
                    form.SetMode(FormMode.ALTER);
                }
                form.SetModifyFinishedCallback((addedID) =>
                {
                    this.pagerWidget.Search(false, addedID);
                });
                form.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            int newImportID = -1;
            List<int> Removei = new List<int>();
            //创建导入窗口
            StandardImportForm<Supply> formImport =
                new StandardImportForm<Supply>
                (
                    //参数1：KeyName
                    SupplyViewMetaData.importsupplykeyNames,
                    (results, unimportedColumns) => //参数2：导入数据二次处理回调函数
                    {


                        string[][] supplierNamesCount = (from kn in unimportedColumns
                                                       where kn.Key == "SupplierName"
                                                       select kn.Value).ToArray();
                        string[][] componentNamesCount = (from kn in unimportedColumns
                                                         where kn.Key == "ComponentName"
                                                         select kn.Value).ToArray();

                        DialogResult messageBoxResult = DialogResult.No;//设置对话框的返回值
                        string[] suppliernames ;
                        suppliernames = supplierNamesCount[0];
                        string[] componentnames;
                        componentnames = componentNamesCount[0];
                        for (int i = 0; i < results.Count; i++)
                        {
                            Supply supply1 = null;
                            string supplierName;
                            string componentName;
                            string no;
                            no = results[i].No;
                            supplierName = suppliernames[i];
                            componentName = componentnames[i];
                            int importcomponenID=-1;
                            int importsupplierID=-1;

                                if (supplierName.Length == 0)
                                {
                                    MessageBox.Show("第" + (i + 1) + "行供货商名称不存在，请重新确认后输入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                    return false;
                                }

                                if (componentName.Length == 0)
                                {
                                    MessageBox.Show("第" + (i + 1) + "行零件名称不存在，请重新确认后输入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                    return false;
                                }
                            if (results[i].No.Length == 0)
                            {
                                MessageBox.Show("第" + (i + 1) + "行代号不能为空！请重新确认后输入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                return false;
                            }

                            //检查导入列表中是否重名
                            for (int j = i + 1; j < results.Count; j++)
                            {
                                if (results[i].No == results[j].No)
                                {
                                    MessageBox.Show("您输入的代号：" + results[i].No + "条目在导入列表中重复", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                    return false;
                                }
                            }
                            //检查数据库中同名
                            try
                            {
                                try
                                    {
                                        Supplier supplierID = (from s in this.wmsEntities.Supplier where s.Name == supplierName && s.IsHistory == 0 select s).FirstOrDefault();
                                        importsupplierID = supplierID.ID;
                                    }
                                    catch
                                    {
                                        MessageBox.Show("您输入的第" + (i + 1) + "行供货商名称：" + supplierName + "不存在，请重新确认后输入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                        return false;
                                    }


                                    try
                                    {
                                        DataAccess.Component componenID = (from s in this.wmsEntities.Component where s.Name == componentName select s).FirstOrDefault();
                                        importcomponenID = componenID.ID;
                                    }
                                    catch
                                    {
                                        MessageBox.Show("您输入的第" + (i + 1) + "行零件名称：" + componentName + "不存在，请重新确认后输入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                        return false;
                                    }

                                var sameNameSupply = (from u in wmsEntities.Supply
                                                     where 
                                                     u.No == no
                                                     && u.ProjectID == this.projectID && u.WarehouseID == this.warehouseID
                                                     &&u.IsHistory==0
                                                     select u).ToArray();
                                if (sameNameSupply.Length > 0)
                                {
                                    
                                    messageBoxResult = MessageBox.Show("已存在相同代号供货条目：" + no + ",是否要保留历史信息?", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation,

                                    MessageBoxDefaultButton.Button2);
                                    if (messageBoxResult == DialogResult.Cancel)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        Removei.Add(i);
                                    }
                                        Supply supply = null;
                                    if (messageBoxResult == DialogResult.Yes)
                                    {


                                        try
                                        {
                                            supply = (from s in this.wmsEntities.Supply
                                                      where s.No == no
                                                       && s.ProjectID == this.projectID && s.WarehouseID == this.warehouseID
                                                       && s.IsHistory == 0
                                                      select s).FirstOrDefault();
                                        }
                                        catch
                                        {
                                            MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return false;
                                        }
                                        if (supply == null)
                                        {
                                            MessageBox.Show("历史零件信息不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return false;
                                        }
                                        newImportID = supply.ID;
                                        this.wmsEntities.Supply.Add(supply);

                                        try
                                        {
                                            supply.ID = -1;
                                            supply.IsHistory = 1;
                                            supply.NewestSupplyID = newImportID;
                                            supply.LastUpdateUserID = this.userID;
                                            supply.LastUpdateTime = DateTime.Now;
                                            wmsEntities.SaveChanges();
                                        }
                                        catch
                                        {
                                            MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return false;
                                        }

                                        var supplystorge = (from u in wmsEntities.Supply
                                                            where u.NewestSupplyID == newImportID
                                                            select u).ToArray();


                                        if (supplystorge.Length > 0)
                                        {
                                            MessageBox.Show("历史信息保留成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                    try
                                    {
                                        supply1 = (from s in this.wmsEntities.Supply
                                                   where s.No == no
                                                    && s.ProjectID == this.projectID && s.WarehouseID == this.warehouseID
                                                    && s.IsHistory == 0
                                                   select s).FirstOrDefault();
                                    }
                                    catch
                                    {
                                        MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                    if (supply1 == null)
                                    {
                                        MessageBox.Show("历史零件信息不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                    PropertyInfo[] proAs = results[i].GetType().GetProperties();
                                    PropertyInfo[] proBs = supply1.GetType().GetProperties();

                                    for (int k = 0; k < proAs.Length; k++)
                                    {
                                        for (int j = 0; j < proBs.Length; j++)
                                        {
                                            if (proAs[k].Name == proBs[j].Name && proBs[j].Name != "ID" && proBs[j].Name != "SupplierID" && proBs[j].Name != "ComponentID" && proBs[j].Name != "Component")

                                            {
                                                object a = proAs[k].GetValue(results[i], null);
                                                proBs[j].SetValue(supply1, a, null);
                                            }
                                        }
                                    }

                                    supply1.SupplierID = importsupplierID;
                                    supply1.ComponentID = importcomponenID;

                                    supply1.IsHistory = 0;
                                    supply1.CreateTime = DateTime.Now;
                                    supply1.CreateUserID = this.userID;
                                    supply1.LastUpdateTime = DateTime.Now;
                                    supply1.LastUpdateUserID = this.userID;
                                    supply1.WarehouseID = this.warehouseID;
                                    supply1.ProjectID = this.projectID;
                                    wmsEntities.SaveChanges();

                                }

                            }
                            catch
                            {

                                MessageBox.Show("操作失败，请检查网络连接4", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }


                            DataAccess.Component curComponen = (from s in this.wmsEntities.Component where s.Name == componentName select s).FirstOrDefault();
                            PropertyInfo[] proA = curComponen.GetType().GetProperties();
                            PropertyInfo[] proB = results[i].GetType().GetProperties();
                            for (int l = 0; l < proA.Length; l++)
                            {
                                for (int j = 0; j < proB.Length; j++)
                                {
                                    if (proA[l].Name == "Default" + proB[j].Name)
                                    {
                                        object a = proA[l].GetValue(curComponen, null);
                                        proB[j].SetValue(results[i], a, null);
                                    }
                                }
                            }


                            results[i].SupplierID = importsupplierID;
                            results[i].ComponentID = importcomponenID;

                            results[i].IsHistory = 0;
                            results[i].CreateTime = DateTime.Now;
                            results[i].CreateUserID = this.userID;
                            results[i].LastUpdateTime = DateTime.Now;
                            results[i].LastUpdateUserID = this.userID;
                            results[i].WarehouseID = this.warehouseID;
                            results[i].ProjectID = this.projectID;


                            //历史信息设为0
                            results[i].IsHistory = 0;


                        }
                        int length = Removei.ToArray().Length;
                        try
                        {
                            for (int b = 0; b < length; b++)
                            {
                                results.RemoveAt(Removei.ToArray()[b] - b);
                            }
                        }
                        catch
                        {
                            return false;
                        }
                        return true;

                    },
                    () => //参数3：导入完成回调函数
                    {
                        this.searchWidget.Search();
                    }
                );

            //显示导入窗口
            formImport.Text = "导入供货信息";
            formImport.Show();
        }
    }
    }

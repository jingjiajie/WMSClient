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
using unvell.ReoGrid.DataFormat;
using System.Reflection;


namespace WMS.UI
{
    public partial class FormBaseSupplier : Form
    {
        private WMSEntities wmsEntities = new WMSEntities();
        private int authority;
        private int authority_supplier = Convert.ToInt32(Authority.BASE_SUPPLIER);
        private int authority_supplierself = Convert.ToInt32(Authority.BASE_SUPPLIER_SUPPLIER_SELFONLY);
        private PagerWidget<SupplierView > pagerWidget = null;
        SearchWidget<SupplierView> searchWidget = null;
        private Supplier supplier = null;
        private string  contractst="";
        private int check_history = 0;
        private int userid = -1;
        private int projectID = -1;
        private int warehouseID = -1;
        private Button toolStripButton1 = null;
        private ComboBox toolStripComboBoxSelect = null;
        private MaskedTextBox toolStripTextBoxSelect = null;
        private ComboBox comboBoxOrderByCondition = null;


        private int id=-1;
       
        public FormBaseSupplier(int authority,int supplierid,int userid)
        {
            InitializeComponent();
            this.authority = authority;
            this.id = supplierid;
            this.userid = userid;
        }

        private void FormBaseSupplier_Load(object sender, EventArgs e)
        {
            try
            {
                if ((this.authority & authority_supplier) != authority_supplier)
                {
                    Supplier supplier = (from u in this.wmsEntities.Supplier
                                         where u.ID == id
                                        select u).FirstOrDefault();
                    this.contractst = supplier.ContractState;
                    this.contractst = supplier.ContractState;
                    this.toolStripButtonAdd.Enabled = false;
                    this.toolStripButtonDelete.Enabled = false;
                    this.buttonCheck.Enabled = false;
                    this.buttonImport.Enabled = false;
                    //this.toolStripButton1.Enabled = false;
                    //this.toolStripButtonSelect.Enabled = false;
                    if (this.contractst == "待审核")
                    {
                        this.toolStripButtonAlter.Enabled = true;

                    }
                    else if (this.contractst == "已过审")
                    {
                        this.toolStripButtonAlter.Enabled = false;
                    }
                    InitSupplier();
                    this.pagerWidget.AddStaticCondition("ID", Convert.ToString(id));
                    this.pagerWidget.AddStaticCondition("IsHistory", "0");
                    this.searchWidget.Search();
                }
                if ((this.authority & authority_supplier) == authority_supplier)
                {
                    InitSupplier();
                    this.pagerWidget.AddStaticCondition("IsHistory", "0");
                    this.searchWidget.Search() ;
                }
            }
            catch
            {
                MessageBox.Show("加载失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
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

        private void InitSupplier ()
        {
            try
            {
                this.wmsEntities.Database.Connection.Open();

                //string[] visibleColumnNames = (from kn in SupplierMetaData.KeyNames
                //                               where kn.Visible == true
                //                               select kn.Name).ToArray();
                //初始化查询
                //this.toolStripComboBoxSelect.Items.Add("无");
                //this.toolStripComboBoxSelect.Items.AddRange(visibleColumnNames);
                //this.toolStripComboBoxSelect.SelectedIndex = 0;
            }
            catch
            {
                MessageBox.Show("加载失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }
            //初始化分页控件

            this.pagerWidget = new PagerWidget<SupplierView>(this.reoGridControlUser, SupplierMetaData.KeyNames, this.projectID, this.warehouseID);
            this.panelPager.Controls.Add(pagerWidget);
            pagerWidget.Show();
            this.searchWidget = new SearchWidget<SupplierView>(SupplierMetaData.KeyNames, this.pagerWidget);
            this.panelSearchWidget.Controls.Add(searchWidget);
            Button toolStripButton1 = (Button)this.Controls.Find("buttonSearch", true)[0];
            this.toolStripButton1 = toolStripButton1;
            ComboBox toolStripComboBoxSelect = (ComboBox)this.Controls.Find("comboBoxSearchCondition", true)[0];
            this.toolStripComboBoxSelect = toolStripComboBoxSelect;
            MaskedTextBox  toolStripTextBoxSelect = (MaskedTextBox)this.Controls.Find("textBoxSearchCondition", true)[0];
            this.toolStripTextBoxSelect = toolStripTextBoxSelect;
            ComboBox comboBoxOrderByCondition= (ComboBox)this.Controls.Find("comboBoxOrderByCondition", true)[0];
            this.comboBoxOrderByCondition = comboBoxOrderByCondition;
        }
       
        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            var a1  = new FormSupplierModify(-1,this.contractst,this.userid );
            
            a1.SetMode(FormMode.ADD);

            a1.SetAddFinishedCallback((AddID ) =>
            {
                this.searchWidget.Search(false ,AddID );
                this.labelStatus.Text = "供应商信息";

            });
            a1.Show();  
        }



        private void toolStripButtonSelect_Click(object sender, EventArgs e)
        {
            if (this.check_history == 0)
            {               
                this.pagerWidget.ClearCondition();
                this.pagerWidget.ClearStaticCondition();
                this.pagerWidget.AddCondition("IsHistory", "1");
                this.toolStripButtonSelect.Text = "全部信息";
                this.toolStripComboBoxSelect.SelectedIndex = 0;
                this.buttonCheck.Enabled = false;
                this.toolStripButtonAdd.Enabled = false;
                this.toolStripButtonAlter.Enabled = false;
                this.buttonImport.Enabled = false;
                this.toolStripButton1.Enabled = false;
                comboBoxOrderByCondition.Enabled = false;
                this.toolStripComboBoxSelect.Enabled = false;
                var worksheet = this.reoGridControlUser.Worksheets[0];
                try
                {
                    if (worksheet.SelectionRange.Rows != 1)
                    {
                        throw new Exception();

                    }
                    int supplierID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                    this.pagerWidget.AddCondition("NewestSupplierID", Convert.ToString(supplierID));
                }
                catch
                {
                    MessageBox.Show("请选择一项进行查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    this.toolStripButtonAdd.Enabled = true;
                    this.toolStripButtonAlter.Enabled = true;
                    this.buttonCheck.Enabled = true;
                    this.buttonImport.Enabled = true;                   
                    this.toolStripButton1.Enabled = true;
                   
                    return;
                }

                if ((this.authority & authority_supplier) != authority_supplier)
                {
                    this.pagerWidget.AddCondition("ID", Convert.ToString(id));
                    this.check_history = 1;
                    this.pagerWidget.Search();
                    this.labelStatus.Text = "供应商信息";
                }
                if ((this.authority & authority_supplier) == authority_supplier)
                {
                    this.check_history = 1;
                    this.pagerWidget.Search();
                    this.labelStatus.Text = "供应商信息";
                }

            }
            else if(this.check_history ==1)
            {
                //增加静态条件
                if ((this.authority & authority_supplier) != authority_supplier)
                {
                    this.pagerWidget.AddStaticCondition("ID", Convert.ToString(id));
                    this.pagerWidget.AddStaticCondition("IsHistory", "0");
                }
                if ((this.authority & authority_supplier) == authority_supplier)
                {
                    this.pagerWidget.AddStaticCondition("IsHistory", "0");
                }
                this.pagerWidget.ClearCondition();                
                if (this.toolStripButtonSelect.Text == "全部信息" && (this.authority & authority_supplier) == authority_supplier)
                {
                    this.toolStripButtonAdd.Enabled = true;
                    this.buttonCheck.Enabled = true;
                    this.toolStripComboBoxSelect.Enabled = true;
                    this.buttonImport.Enabled = true;
                   
                    this.toolStripButtonSelect.Visible = true;
                }
                this.toolStripButtonSelect.Text = "查询历史信息";
                this.toolStripButton1.Enabled = true;
                this.comboBoxOrderByCondition .Enabled =true ;
                this.toolStripComboBoxSelect.Enabled = true;
                if (this.contractst == "待审核" || this.contractst == "")
                {
                    this.toolStripButtonAlter.Enabled = true;
                }
                this.pagerWidget.AddCondition("是否历史信息", "0");
                if ((this.authority & authority_supplier) != authority_supplier)
                {
                    this.pagerWidget.AddCondition("ID", Convert.ToString(id));
                    this.check_history = 0;
                    this.pagerWidget.Search();
                    this.labelStatus.Text = "供应商信息";
                }
                if ((this.authority & authority_supplier) == authority_supplier)
                {
                    this.check_history = 0;
                    this.pagerWidget.Search();
                    this.labelStatus.Text = "供应商信息";
                }
            }
            


        }

        private void Search()
        {
        //    string key = null;
        //    string value = null;


        //    if (this.toolStripComboBoxSelect.SelectedIndex != 0)
        //    {
        //        key = (from kn in SupplierMetaData.KeyNames
        //               where kn.Name == this.toolStripComboBoxSelect.SelectedItem.ToString()
        //               select kn.Key).First();
        //        value = this.toolStripTextBoxSelect.Text;
        //    }
        //    this.labelStatus.Text = "正在搜索中...";
        //    var worksheet = this.reoGridControlUser.Worksheets[0];
        //    worksheet[0, 0] = "加载中...";


        //    new Thread(new ThreadStart(() =>
        //    {
        //        WMSEntities wmsEntities = new WMSEntities();
        //        SupplierView[] SupplierView = null;
        //        string sql = "SELECT * FROM SupplierView WHERE 1=1 ";
        //        List<SqlParameter> parameters = new List<SqlParameter>();
        //        if ((this.authority & authority_supplier) == authority_supplier)
        //        {
        //            if (key != null && value != null) //查询条件不为null则增加查询条件
        //            {
        //                sql += "AND " + key + " = @value ";
        //                parameters.Add(new SqlParameter("value", value));
        //            }
        //            sql += " ORDER BY ID DESC"; //倒序排序
        //            try
        //            {
        //                SupplierView = wmsEntities.Database.SqlQuery<SupplierView>(sql, parameters.ToArray()).ToArray();
                        
        //            }
                    
        //            catch (EntityCommandExecutionException)
        //            {
        //                MessageBox.Show("查询失败，请检查输入条件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                return;
        //            }
        //            catch (Exception)
        //            {
        //                MessageBox.Show("查询失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                return;
        //            }
        //        }



        //        if ((this.authority & authority_supplier) != authority_supplier)
        //        {
        //            if (id != -1)
        //            {
        //                sql += "AND ID = @ID ";
        //                parameters.Add(new SqlParameter("ID", id));
        //            }
        //            if (key != null && value != null) //查询条件不为null则增加查询条件
        //            {
        //                sql += "AND " + key + " = @value ";
        //                parameters.Add(new SqlParameter("value", value));
        //            }
        //            sql += " ORDER BY ID DESC"; //倒序排序
        //            SupplierView = wmsEntities.Database.SqlQuery<SupplierView>(sql, parameters.ToArray()).ToArray();

        //        }






        //        this.reoGridControlUser.Invoke(new Action(() =>
        //        {
        //            this.labelStatus.Text = "搜索完成";
        //            var worksheet1 = this.reoGridControlUser.Worksheets[0];
        //            worksheet1.DeleteRangeData(RangePosition.EntireRange);

        //            if (SupplierView.Length == 0)
        //            {
        //                worksheet1[0, 1] = "没有查询到符合条件的记录";
        //            }
        //            for (int i = 0; i < SupplierView.Length; i++)
        //            {
        //                SupplierView curComponent = SupplierView[i];
        //                object[] columns = Utilities.GetValuesByPropertieNames(curComponent, (from kn in SupplierMetaData.KeyNames


        //                                                                                      select kn.Key).ToArray());

        //                for (int j = 0; j < worksheet1.Columns; j++)
        //                {

        //                    worksheet1[i, j] = columns[j] == null ? "" : columns[j].ToString();
        //                    worksheet1.SetRangeDataFormat(RangePosition.EntireRange, CellDataFormatFlag.Text, null);
        //                }
        //            }

        //        }));
        //    }

        //)).Start();
        }
        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlUser.Worksheets[0];
            if (worksheet.SelectionRange.Rows == 1)
            {
                try
                {
                    //if (worksheet.SelectionRange.Rows == 1)
                    //{
                    //    throw new Exception();
                    //}
                    int supplierID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                    var a1 = new FormSupplierModify(supplierID, this.contractst, this.userid);
                    a1.SetModifyFinishedCallback((AlterID) =>
                    {
                        this.searchWidget.Search(false, AlterID);
                        this.labelStatus.Text = "供应商信息";
                    });
                    a1.Show();
                }
                catch
                {
                    MessageBox.Show("当前选中空白条目，请重新选择要修改的条目进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }


            if (worksheet.SelectionRange.Rows != 1)
            {
                Object alterData = worksheet[worksheet.SelectionRange.Row, 0, worksheet.SelectionRange.Rows, 1];
                var alterDatas = new SupplierView [worksheet.SelectionRange.Rows];
                for (int i = 0; i < worksheet.SelectionRange.Rows; i++)
                {
                    try
                    {
                        int curSupplyID = int.Parse(worksheet[worksheet.SelectionRange.Row + i, 0].ToString());
                        var curSupply = (from u in wmsEntities.SupplierView  
                                         where
                                         u.ID == curSupplyID
                                         select u).FirstOrDefault();
                        if (curSupply == null)
                        {
                            MessageBox.Show("供应商信息不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                int supplierID_Import = 0;
                wmsEntities = new WMSEntities();
                List<int> Removei = new List<int>();

                //创建导入窗口
                StandardImportForm<Supplier> formImport =
                    new StandardImportForm<Supplier>
                    (
                        SupplierMetaData.KeyNames, //参数1：KeyName
                        (results, unimportedColumns) => //参数2：导入数据二次处理回调函数
                        {
                            DialogResult allMsgBoxResult = DialogResult.No;//设置对话框的返回值
                            bool allMsgBoxResultChoose = false;
                            for (int a = 0; a < results.Count; a++)
                            {
                                string suppliernameimport;
                                suppliernameimport = results[a].Name;
                                //检查导入列表中是否重名
                                for (int j = a + 1; j < results.Count; j++)
                                {
                                    if (suppliernameimport == results[j].Name)
                                    {
                                        MessageBox.Show("您输入的用户名" + suppliernameimport + "在导入列表中重复", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                        return false;

                                    }
                                }
                                //判断合同
                                if (results[a].StartingTime > results[a].EndingTime)
                                {
                                    MessageBox.Show("操作失败，供应商" + suppliernameimport + "的合同生效日期不能大于截止日期", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return false;
                                }
                                string contractstate = results[a].ContractState;
                                if (contractstate != "待审核" && contractstate != "已过审" && contractstate != "")
                                {
                                    MessageBox.Show("操作失败，供应商" + suppliernameimport + "的合同状态请改为待审核、已过审或空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return false;
                                }
                                //判断代号
                                if (results[a].No == "")
                                {
                                    MessageBox.Show("操作失败，供应商" + suppliernameimport + "的代号不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return false;
                                }
                            }


                            //判断是否全部替换
                            for (int a = 0; a < results.Count; a++)
                            {
                                string suppliernameimport;

                                suppliernameimport = results[a].Name;

                                var sameNameUsers = (from u in wmsEntities.Supplier
                                                     where u.Name == suppliernameimport &&
                                                     u.IsHistory == 0
                                                     select u).FirstOrDefault();
                                try
                                {
                                    if (sameNameUsers != null&& allMsgBoxResultChoose==false  )
                                    {
                                        allMsgBoxResult = MessageBox.Show("已存在同名供应商是否保留全部历史信息", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation,
                                        MessageBoxDefaultButton.Button2);
                                        allMsgBoxResultChoose = true;
                                    }
                                }
                                catch
                                {
                                    MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return false;
                                }

                            }

                            //
                            for (int i = 0; i < results.Count; i++)
                            {
                            DialogResult MsgBoxResult = DialogResult.No;//设置对话框的返回值
                            Supplier supplier1 = new Supplier();

                                string suppliernameimport;

                                suppliernameimport = results[i].Name;


                             //检查数据库中是否与非历史信息同名

                                try
                                {

                                    var sameNameUsers = (from u in wmsEntities.Supplier
                                                         where u.Name == suppliernameimport &&
                                                         u.IsHistory == 0
                                                         select u).FirstOrDefault();
                                    if (sameNameUsers != null)
                                    {
                                        Removei.Add(i);
                                        supplierID_Import = sameNameUsers.ID;
                                        if (allMsgBoxResult == DialogResult.Cancel)
                                        {
                                        MsgBoxResult = MessageBox.Show("已存在同名供应商：" + suppliernameimport + "是否导入并将原信息保留为历史信息", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                        MessageBoxDefaultButton.Button2);
                                        }
                                        else 
                                        {
                                            MsgBoxResult = allMsgBoxResult ;
                                        }
                                                                                
                                        if (MsgBoxResult == DialogResult.Yes)//如果对话框的返回值是YES（按"Y"按钮）且历史信息在本次修改中还没保存过
                                        {
                                            wmsEntities.Supplier.Add(sameNameUsers);
                                            try
                                            {
                                                sameNameUsers.NewestSupplierID = supplierID_Import;
                                                sameNameUsers.ID = -1;
                                                sameNameUsers.IsHistory = 1;
                                                sameNameUsers.LastUpdateUserID = this.userid;
                                                sameNameUsers.LastUpdateTime = DateTime.Now;
                                                wmsEntities.SaveChanges();
                                            }
                                            catch
                                            {
                                                MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                return false;
                                            }
                                            try
                                            {
                                                var supplierstorge = (from u in wmsEntities.SupplierStorageInfo
                                                                      where u.SupplierID == supplierID_Import
                                                                      select u).ToArray();

                                                for (int a = 0; a < supplierstorge.Length; a++)
                                                {
                                                    supplierstorge[a].ExecuteSupplierID = supplierID_Import;
                                                    wmsEntities.SaveChanges();

                                                }
                                            }
                                            catch
                                            {
                                                MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                continue;
                                            }
                                        }


                                        try
                                        {

                                            supplier1 = (from u in wmsEntities.Supplier
                                                         where u.ID == supplierID_Import &&
                                                         u.IsHistory == 0
                                                         select u).FirstOrDefault();
                                        }
                                        catch
                                        {
                                            MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return false;
                                        }
                                        PropertyInfo[] proAs = results[i].GetType().GetProperties();
                                        PropertyInfo[] proBs = supplier1.GetType().GetProperties();

                                        for (int k = 0; k < proAs.Length; k++)
                                        {
                                            for (int j = 0; j < proBs.Length; j++)
                                            {
                                                if (proAs[k].Name == proBs[j].Name && proBs[j].Name != "ID" && proBs[j].Name != "RecipientName" && proBs[j].Name != "Supplier1" & proBs[j].Name != "Supplier2"
                                                && proBs[j].Name != "SupplierStorageInfo" && proBs[j].Name != "SupplierStorageInfo1")

                                                {
                                                    object a = proAs[k].GetValue(results[i], null);
                                                    proBs[j].SetValue(supplier1, a, null);
                                                }
                                            }
                                        }
                                        try
                                        {
                                            supplier1.IsHistory = 0;
                                            supplier1.CreateTime = DateTime.Now;
                                            supplier1.CreateUserID = this.userid;
                                            supplier1.LastUpdateTime = DateTime.Now;
                                            supplier1.LastUpdateUserID = this.userid;
                                            wmsEntities.SaveChanges();
                                        }
                                        catch
                                        {

                                            MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return false;
                                        }

                                    }

                                }
                                catch
                                {

                                    MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return false;
                                }
                                results[i].CreateTime = DateTime.Now;
                                results[i].CreateUserID = this.userid;
                                results[i].LastUpdateTime = DateTime.Now;
                                results[i].LastUpdateUserID = this.userid;
                            //历史信息设为0
                            results[i].IsHistory = 0;

                            }
                            int length = Removei.ToArray().Length;
                            for (int b = 0; b < length; b++)
                            {
                                results.RemoveAt(Removei.ToArray()[b] - b);
                            }
                            return true;
                        },
                        
                        () => //参数3：导入完成回调函数                    
                        {
                            this.pagerWidget.Search();
                        }
                    );

                //显示导入窗口
                formImport.Show();
                formImport.PushData(alterDatas, SupplierMetaData.keyConvert);
                formImport.Text = "修改供货信息";


                 






            }

        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlUser.Worksheets[0];
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


            new Thread(new ThreadStart(() =>
            {
                WMSEntities wmsEntities = new WMSEntities();

                //供应商存货删除
                try
                {
                    foreach (int id in deleteIDs)
                    {

                        try
                        {

                            var recipettickets = (from kn in wmsEntities.ReceiptTicket
                                                  where kn.SupplierID == id
                                                  select kn).ToArray();
                            if (recipettickets.Length > 0)
                            {
                                MessageBox.Show("删除失败，请先删除与本供应商信息相关的收货单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            wmsEntities = new WMSEntities();
                            var Supply = (from kn in wmsEntities.Supply 
                                                  where kn.SupplierID == id
                                                  select kn).ToArray();
                            if (Supply.Length > 0)
                            {
                                MessageBox.Show("删除失败，请先删除与本供应商信息相关的供货信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                        }
                        catch
                        {
                            MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;

                        }

                        var supplierstorgeid = (from kn in wmsEntities.SupplierStorageInfo
                                                where kn.SupplierID == id
                                                select kn.ID).ToArray();
                        if (supplierstorgeid.Length > 0)
                        {
                            try
                            {
                                foreach (int supplierstorgeid1 in supplierstorgeid)
                                {
                                    wmsEntities.Database.ExecuteSqlCommand("DELETE FROM SupplierStorageInfo WHERE ID = @supplierID", new SqlParameter("supplierID", supplierstorgeid1));


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

                        var supplier_historyid = (from kn in wmsEntities.Supplier
                                                  where kn.NewestSupplierID == id
                                                  select kn.ID).ToArray();
                        if (supplier_historyid.Length > 0)
                        {
                            try
                            {
                                foreach (int NewestSupplierid in supplier_historyid)
                                {
                                    wmsEntities.Database.ExecuteSqlCommand("DELETE FROM Supplier WHERE ID = @supplierID", new SqlParameter("supplierID", NewestSupplierid));


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
                        wmsEntities.Database.ExecuteSqlCommand("DELETE FROM Supplier WHERE ID = @supplierID", new SqlParameter("supplierID", id));
                    }

                    wmsEntities.SaveChanges();

                }
                catch
                {
                    MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Invoke(new Action(() =>
                {
                    this.searchWidget.Search();
                    this.labelStatus.Text = "供应商信息";
                    
                }));               
            })).Start();
        }
        

       

        private void toolStripComboBoxSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.toolStripComboBoxSelect.SelectedIndex == 0)
            {
                this.toolStripTextBoxSelect.Text = "";
                this.toolStripTextBoxSelect.Enabled = false;
                //this.Search();
            }
            else
            {
                this.toolStripTextBoxSelect.Enabled = true;
            }
        }

        private void toolStripTextBoxSelect_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13)
            {
                this.searchWidget .Search();
            }

        }

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            //var a1 = new FormSupplierAnnualInfo(1);
            //a1.Show();
            var worksheet = this.reoGridControlUser.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int supplierID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());

                var a1 = new SupplierStorageInfo(supplierID,this.check_history );
                a1.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if(this.toolStripButton1.Text =="全部信息"&& (this.authority & authority_supplier) == authority_supplier)
            {                
                this.toolStripButtonAdd .Enabled = true;                
                this.buttonCheck.Enabled = true;
                this.toolStripComboBoxSelect.Enabled = true;
                this.buttonImport.Enabled = true;
            }

            this.toolStripButton1.Text = "查询";
            this.toolStripButtonSelect.Visible = true;
            if (this.contractst == "待审核"||this.contractst =="")                
            {
                this.toolStripButtonAlter.Enabled = true;
            }
            this.pagerWidget.AddStaticCondition("是否历史信息", "0");

            //if (this.toolStripComboBoxSelect.SelectedIndex != 0)
            //{
            //    if(this.toolStripButton1.Text == "查询" && this.toolStripTextBoxSelect.Text ==string.Empty)
            //    {
            //        MessageBox.Show("请输入要查询的关键词", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning );
            //        return;
            //    }
            //    this.pagerWidget .SetSearchCondition(this.toolStripComboBoxSelect.SelectedItem.ToString(), this.toolStripTextBoxSelect.Text);
            //}

            if ((this.authority & authority_supplier) != authority_supplier)
            {
                this.pagerWidget.AddStaticCondition("ID", Convert.ToString(id));
                this.check_history = 0;
                this.searchWidget.Search();
                this.labelStatus.Text = "供应商信息";
            }
            if ((this.authority & authority_supplier) == authority_supplier)
            {
                this.check_history = 0;
                this.searchWidget.Search();
                this.labelStatus.Text = "供应商信息";
            }

        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            int supplierID_Import=0;
            wmsEntities = new WMSEntities();
            List<int> Removei = new List<int>();

            //创建导入窗口
            StandardImportForm<Supplier > formImport =
                new StandardImportForm<Supplier>
                (
                    
                    SupplierMetaData.KeyNames, //参数1：KeyName
                    (results, unimportedColumns) => //参数2：导入数据二次处理回调函数
                    {
                        DialogResult allMsgBoxResult = DialogResult.No;//设置对话框的返回值
                        bool allMsgBoxResultChoose = false;
                        for (int a = 0; a < results.Count; a++)
                        {
                            string suppliernameimport;
                            suppliernameimport = results[a].Name;
                            //检查导入列表中是否重名
                            for (int j = a + 1; j < results.Count; j++)
                            {
                                if (suppliernameimport == results[j].Name)
                                {
                                    MessageBox.Show("您输入的用户名" + suppliernameimport + "在导入列表中重复", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                    return false;

                                }
                            }
                            //判断合同
                            if (results[a].StartingTime > results[a].EndingTime)
                            {
                                MessageBox.Show("操作失败，供应商" + suppliernameimport + "的合同生效日期不能大于截止日期", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                            string contractstate = results[a].ContractState;
                            if (contractstate != "待审核" && contractstate != "已过审" && contractstate != "")
                            {
                                MessageBox.Show("操作失败，供应商" + suppliernameimport + "的合同状态请改为待审核、已过审或空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                            //判断代号
                            if (results[a].No == "")
                            {
                                MessageBox.Show("操作失败，供应商" + suppliernameimport + "的代号不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                        }


                        //判断是否全部替换
                        for (int a = 0; a < results.Count; a++)
                        {
                            string suppliernameimport;

                            suppliernameimport = results[a].Name;

                            var sameNameUsers = (from u in wmsEntities.Supplier
                                                 where u.Name == suppliernameimport &&
                                                 u.IsHistory == 0
                                                 select u).FirstOrDefault();
                            try
                            {
                                if (sameNameUsers != null && allMsgBoxResultChoose == false)
                                {
                                    allMsgBoxResult = MessageBox.Show("已存在同名供应商是否保留全部历史信息", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation,
                                    MessageBoxDefaultButton.Button2);
                                    allMsgBoxResultChoose = true;
                                }
                            }
                            catch
                            {
                                MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }

                        }

                        //
                        for (int i = 0; i < results.Count; i++)
                        {
                            DialogResult MsgBoxResult = DialogResult.No;//设置对话框的返回值
                            Supplier supplier1 = new Supplier();

                            string suppliernameimport;

                            suppliernameimport = results[i].Name;


                            //检查数据库中是否与非历史信息同名

                            try
                            {

                                var sameNameUsers = (from u in wmsEntities.Supplier
                                                     where u.Name == suppliernameimport &&
                                                     u.IsHistory == 0
                                                     select u).FirstOrDefault();
                                if (sameNameUsers != null)
                                {
                                    Removei.Add(i);
                                    supplierID_Import = sameNameUsers.ID;
                                    if (allMsgBoxResult == DialogResult.Cancel)
                                    {
                                        MsgBoxResult = MessageBox.Show("已存在同名供应商：" + suppliernameimport + "是否导入并将原信息保留为历史信息", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                                        MessageBoxDefaultButton.Button2);
                                    }
                                    else
                                    {
                                        MsgBoxResult = allMsgBoxResult;
                                    }

                                    if (MsgBoxResult == DialogResult.Yes)//如果对话框的返回值是YES（按"Y"按钮）且历史信息在本次修改中还没保存过
                                    {
                                        wmsEntities.Supplier.Add(sameNameUsers);
                                        try
                                        {
                                            sameNameUsers.NewestSupplierID = supplierID_Import;
                                            sameNameUsers.ID = -1;
                                            sameNameUsers.IsHistory = 1;
                                            sameNameUsers.LastUpdateUserID = this.userid;
                                            sameNameUsers.LastUpdateTime = DateTime.Now;
                                            wmsEntities.SaveChanges();
                                        }
                                        catch
                                        {
                                            MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            return false;
                                        }
                                        try
                                        {
                                            var supplierstorge = (from u in wmsEntities.SupplierStorageInfo
                                                                  where u.SupplierID == supplierID_Import
                                                                  select u).ToArray();

                                            for (int a = 0; a < supplierstorge.Length; a++)
                                            {
                                                supplierstorge[a].ExecuteSupplierID = supplierID_Import;
                                                wmsEntities.SaveChanges();

                                            }
                                        }
                                        catch
                                        {
                                            MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            continue;
                                        }
                                    }


                                    try
                                    {

                                        supplier1 = (from u in wmsEntities.Supplier
                                                     where u.ID == supplierID_Import &&
                                                     u.IsHistory == 0
                                                     select u).FirstOrDefault();
                                    }
                                    catch
                                    {
                                        MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                    PropertyInfo[] proAs = results[i].GetType().GetProperties();
                                    PropertyInfo[] proBs = supplier1.GetType().GetProperties();

                                    for (int k = 0; k < proAs.Length; k++)
                                    {
                                        for (int j = 0; j < proBs.Length; j++)
                                        {
                                            if (proAs[k].Name == proBs[j].Name && proBs[j].Name != "ID" && proBs[j].Name != "RecipientName" && proBs[j].Name != "Supplier1" & proBs[j].Name != "Supplier2"
                                            && proBs[j].Name != "SupplierStorageInfo" && proBs[j].Name != "SupplierStorageInfo1")

                                            {
                                                object a = proAs[k].GetValue(results[i], null);
                                                proBs[j].SetValue(supplier1, a, null);
                                            }
                                        }
                                    }
                                    try
                                    {
                                        supplier1.IsHistory = 0;
                                        supplier1.CreateTime = DateTime.Now;
                                        supplier1.CreateUserID = this.userid;
                                        supplier1.LastUpdateTime = DateTime.Now;
                                        supplier1.LastUpdateUserID = this.userid;
                                        wmsEntities.SaveChanges();
                                    }
                                    catch
                                    {

                                        MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }

                                }

                            }
                            catch
                            {

                                MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                            results[i].CreateTime = DateTime.Now;
                            results[i].CreateUserID = this.userid;
                            results[i].LastUpdateTime = DateTime.Now;
                            results[i].LastUpdateUserID = this.userid;
                            //历史信息设为0
                            results[i].IsHistory = 0;

                        }
                        int length = Removei.ToArray().Length;
                        for (int b = 0; b < length; b++)
                        {
                            results.RemoveAt(Removei.ToArray()[b] - b);
                        }
                        return true;
                    }, 
                    () => //参数3：导入完成回调函数
                    {
                        this.pagerWidget.Search();

                    }
                );

            //显示导入窗口
            formImport.Show();
            //FormSupplyRemind a1 = new FormSupplyRemind();
            //a1.remindSupply();
            //a1.remindStock();
            //a1.TextDeliver();
        }

        private void panelSearchWidget_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    

}

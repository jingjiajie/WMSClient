using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using unvell.ReoGrid;
using WMS.UI.FormReceipt;
using WMS.DataAccess;
using System.Threading;
using System.Data.SqlClient;
namespace WMS.UI
{
    public partial class FormReceiptItems : Form
    {
        private int userID;
        private bool realReceiptAmountRefreshMark;
        private int receiptTicketItemID;
        private FormMode formMode;
        private int receiptTicketID;
        private int componentID;
        Func<int> JobPersonIDGetter = null;
        Func<int> ConfirmPersonIDGetter = null;
        private int jobPersonID = -1;
        private int confirmPersonID = -1;
        Action callBack = null;
        const string WAIT_CHECK = "待检";
        const string CHECK = "送检中";
        SubmissionTicket submissionTicket;
        private int projectID = -1;
        private int warehouseID = -1;
        ReceiptTicketView ReceiptTicketView;
        PagerWidget<ReceiptTicketItemView> pagerWidget;
        private StandardImportForm<ReceiptTicketItem> standardImportForm = null;

        public FormReceiptItems()
        {
            InitializeComponent();
        }

        public void SetCallback(Action action)
        {
            callBack = action;
        }

        public FormReceiptItems(FormMode formMode, int receiptTicketID, int userID)
        {
            InitializeComponent();
            this.formMode = formMode;
            this.receiptTicketID = receiptTicketID;
            this.realReceiptAmountRefreshMark = false;
            this.userID = userID;
            FormSelectPerson.DefaultPosition = FormBase.Position.RECEIPT;
        }

        private bool SubmissionTicketIsExist()
        {
            WMSEntities wmsEntities = new WMSEntities();
            SubmissionTicket[] submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ReceiptTicketID == receiptTicketID && st.State != "作废" select st).ToArray();
            if (submissionTicket.Length == 0)
            {
                return false;
            }
            else
            {
                this.submissionTicket = submissionTicket[0];
                return true;
            }
        }

        private void InitComponents()
        {
            //初始化
            //string[] columnNames = (from kn in ReceiptMetaData.itemsKeyName select kn.Name).ToArray();
            //初始化表格
            var worksheet = this.reoGridControlReceiptItems.Worksheets[0];
            worksheet.SelectionMode = WorksheetSelectionMode.Row;
            for (int i = 0; i < ReceiptMetaData.itemsKeyName.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = ReceiptMetaData.itemsKeyName[i].Name;
                worksheet.ColumnHeaders[i].IsVisible = ReceiptMetaData.itemsKeyName[i].Visible;
            }
            //worksheet.ColumnHeaders[columnNames.Length].Text = "是否送检";
            worksheet.Columns = ReceiptMetaData.itemsKeyName.Length;
        }

        private void FormReceiptArrivalItems_Load(object sender, EventArgs e)
        {
            InitComponents();
            InitPanel();
            WMSEntities wmsEntities = new WMSEntities();

            ReceiptTicketView receiptTicketView = (from rt in wmsEntities.ReceiptTicketView where rt.ID == receiptTicketID select rt).FirstOrDefault();
            if (receiptTicketView == null)
            {
                MessageBox.Show("该收货单已被删除，请刷新后查看！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.ReceiptTicketView = receiptTicketView;
            this.projectID = receiptTicketView.ProjectID;
            this.warehouseID = receiptTicketView.WarehouseID;
            pagerWidget = new PagerWidget<ReceiptTicketItemView>(this.reoGridControlReceiptItems, ReceiptMetaData.itemsKeyName, receiptTicketView.ProjectID, receiptTicketView.WarehouseID);
            this.panel2.Controls.Add(pagerWidget);
            pagerWidget.Show();
            JobPersonIDGetter = Utilities.BindTextBoxSelect<FormSelectPerson, Person>(this, "textBoxJobPersonName", "Name");
            ConfirmPersonIDGetter = Utilities.BindTextBoxSelect<FormSelectPerson, Person>(this, "textBoxConfirmPersonName", "Name");
            TextBox textBoxComponentNo = (TextBox)this.Controls.Find("textBoxSupplyNo", true)[0];
            TextBox textBoxComponentName = (TextBox)this.Controls.Find("textBoxComponentName", true)[0];
            textBoxComponentNo.Click += TextBoxComponentNo_Click;
            textBoxComponentName.Click += TextBoxComponentNo_Click;


            this.Controls.Find("textBoxState", true)[0].Text = receiptTicketView.State;
            if (receiptTicketView.State != "待收货")
            {
                //MessageBox.Show("该收货单状态为" + receiptTicketView.State + ",无法修改收货单条目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.buttonAdd.Enabled = false;
                this.buttonDelete.Enabled = false;
                //this.buttonModify.Enabled = false;
                this.buttonImport.Enabled = false;
            }
            if (receiptTicketView.HasPutawayTicket == "部分生成上架单" || receiptTicketView.HasPutawayTicket == "全部生成上架单")
            {
                MessageBox.Show("该收货单已经生成上架单，无法修改收货单条目数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.buttonAdd.Enabled = false;
                this.buttonDelete.Enabled = false;
                //this.buttonModify.Enabled = false;
                this.buttonImport.Enabled = false;


            }

            Search();
            TextBox textBoxRealReceiptUnitCount = (TextBox)Controls.Find("textBoxRealReceiptUnitCount", true)[0];
            TextBox textBoxRealReceiptAmount = (TextBox)Controls.Find("textBoxRealReceiptAmount", true)[0];
            //TextBox textBoxRefuseUnitCount = (TextBox)Controls.Find("textBoxRefuseUnitCount", true)[0];
            //textBoxRealReceiptUnitCount.TextChanged += textBoxRealReceiptUnitCount_TextChanged;
            //TextBox textBoxExpectedUnitCount = (TextBox)Controls.Find("textBoxExpectedUnitCount", true)[0];
            TextBox textBoxExpectedAmount = (TextBox)Controls.Find("textBoxExpectedAmount", true)[0];
            TextBox textBoxUnitAmount = (TextBox)Controls.Find("textBoxUnitAmount", true)[0];
            textBoxExpectedAmount.TextChanged += textBoxExpectedAmount_TextChanged;
            textBoxRealReceiptAmount.TextChanged += textBoxRealReceiptAmount_TextChanged;
            textBoxUnitAmount.TextChanged += textBoxUnitAmount_TextChanged;
            User user = (from u in wmsEntities.User where u.ID == this.userID select u).FirstOrDefault();
            if (user != null)
            {
                Supplier supplier = user.Supplier;
                if (supplier != null)
                {
                    if (supplier.EndingTime < DateTime.Now)
                    {
                        this.buttonAdd.Enabled = false;
                        this.buttonDelete.Enabled = false;
                        this.buttonModify.Enabled = false;
                        this.buttonImport.Enabled = false;
                    }
                }
            }
        }

        private void textBoxUnitAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox textBoxRealReceiptUnitCount = (TextBox)this.Controls.Find("textBoxRealReceiptUnitCount", true)[0];
                TextBox textBoxUnitAmount = (TextBox)sender;
                //TextBox textBoxReceiviptAmount = (TextBox)this.Controls.Find("textBoxReceiviptAmount", true)[0];
                TextBox textBoxRealReceiptAmount = (TextBox)this.Controls.Find("textBoxRealReceiptAmount", true)[0];
                string strRealReceiptUnitCount = textBoxRealReceiptUnitCount.Text;
                string strUnitAmount = textBoxUnitAmount.Text;
                string strRealReceiptAmount = textBoxRealReceiptAmount.Text;
                decimal realReceiptAmount;
                decimal realReceiptUnitCount = -1;
                decimal unitAmount;
                //decimal receiviptAmount = -1;
                //decimal realReceiptAmount;
                if (decimal.TryParse(strRealReceiptAmount, out realReceiptAmount) == true && decimal.TryParse(strUnitAmount, out unitAmount) == true)
                {
                    realReceiptUnitCount = realReceiptAmount / unitAmount;
                    //receiviptAmount = realReceiptUnitCount * unitAmount;
                }
                if (realReceiptUnitCount >= 0)
                {
                    textBoxRealReceiptUnitCount.Text = realReceiptUnitCount.ToString();
                }
            }
            catch
            {

            }
        }

        private void textBoxRealReceiptAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox textBoxUnitAmount = (TextBox)this.Controls.Find("textBoxUnitAmount", true)[0];
                TextBox textBoxRealReceiptAmount = (TextBox)sender;
                //TextBox textBoxReceiviptAmount = (TextBox)this.Controls.Find("textBoxReceiviptAmount", true)[0];
                TextBox textBoxRealReceiptUnitCount = (TextBox)this.Controls.Find("textBoxRealReceiptUnitCount", true)[0];
                string strRealReceiptUnitCount = textBoxRealReceiptUnitCount.Text;
                string strUnitAmount = textBoxUnitAmount.Text;
                string strRealReceiptAmount = textBoxRealReceiptAmount.Text;
                decimal realReceiptUnitCount = -1;
                decimal unitAmount;
                decimal realReceiptAmount;
                //decimal receiviptAmount = -1;
                if (decimal.TryParse(strRealReceiptAmount, out realReceiptAmount) == true && decimal.TryParse(strUnitAmount, out unitAmount) == true)
                {
                    realReceiptUnitCount = realReceiptAmount / unitAmount;
                }
                if (realReceiptUnitCount >= 0)
                {
                    //textBoxReceiviptAmount.Text = realReceiptUnitCount.ToString();
                    textBoxRealReceiptUnitCount.Text = realReceiptUnitCount.ToString();
                }
            }
            catch
            {

            }
        }

        private void textBoxExpectedAmount_TextChanged(object sender, EventArgs e)
        {
            if (this.realReceiptAmountRefreshMark == true)
            {
                try
                {
                    TextBox textBoxExpectAmount = (TextBox)sender;
                    TextBox textBoxRealReceiptAmount = (TextBox)this.Controls.Find("textBoxRealReceiptAmount", true)[0];
                    TextBox textBoxRefuseAmount = (TextBox)this.Controls.Find("textBoxRefuseAmount", true)[0];
                    TextBox textBoxRefuseUnitAmount = (TextBox)this.Controls.Find("textBoxRefuseUnitAmount", true)[0];
                    TextBox textBoxUnitAmount = (TextBox)this.Controls.Find("textBoxUnitAmount", true)[0];
                    string strExpectAmount = textBoxExpectAmount.Text;
                    string strRealReceiptAmount = textBoxRealReceiptAmount.Text;
                    string strRefuseAmount = textBoxRefuseAmount.Text;
                    string strRefuseUnitAmount = textBoxRefuseUnitAmount.Text;
                    string strUnitAmount = textBoxUnitAmount.Text;
                    decimal expectAmount;
                    //decimal realReceiptUnitCount;
                    decimal realReceiptAmount;
                    //decimal refuseUnitCount;
                    decimal refuseAmount;
                    decimal unitAmount;
                    if (decimal.TryParse(strExpectAmount, out expectAmount) && decimal.TryParse(strRefuseAmount, out refuseAmount) && decimal.TryParse(strUnitAmount, out unitAmount) == true)
                    {
                        if (unitAmount > 0 && expectAmount >= 0 && refuseAmount >= 0)
                        {
                            //realReceiptUnitCount = (expectAmount * unitAmount - refuseUnitCount * refuseUnitAmount) / unitAmount;
                            realReceiptAmount = expectAmount - refuseAmount;
                            if (realReceiptAmount >= 0)
                            {
                                textBoxRealReceiptAmount.Text = realReceiptAmount.ToString();
                            }
                            else
                            {
                                textBoxRealReceiptAmount.Text = "0";
                            }
                        }
                    }
                }
                catch
                {

                }
            }
        }

        /*
        private void textBoxRealReceiptUnitCount_TextChanged(object sender, EventArgs e)
        {
            TextBox textBoxRealReceiptUnitCount = (TextBox)sender;
            TextBox textBoxExpectUnitCount = (TextBox)this.Controls.Find("textBoxExpectedUnitCount", true)[0];
            TextBox textBoxRefuseUnitCount = (TextBox)this.Controls.Find("textBoxRefuseUnitCount", true)[0];
            TextBox textBoxRefuseUnitAmount = (TextBox)this.Controls.Find("textBoxRefuseUnitAmount", true)[0];
            TextBox textBoxUnitAmount = (TextBox)this.Controls.Find("textBoxUnitAmount", true)[0];
            string strExpectUnitCount = textBoxExpectUnitCount.Text;
            string strRealReceiptUnitCount = textBoxRealReceiptUnitCount.Text;
            string strRefuseUnitCount = textBoxRefuseUnitCount.Text;
            string strRefuseUnitAmount = textBoxRefuseUnitAmount.Text;
            string strUnitAmount = textBoxUnitAmount.Text;
            decimal expectUnitCount;
            decimal realReceiptUnitCount;
            decimal refuseUnitCount;
            decimal refuseUnitAmount;
            decimal unitAmount;
            if (decimal.TryParse(strExpectUnitCount, out expectUnitCount) && decimal.TryParse(strRealReceiptUnitCount, out realReceiptUnitCount) && decimal.TryParse(strRefuseUnitAmount, out refuseUnitAmount) == true && decimal.TryParse(strUnitAmount, out unitAmount) == true)
            {
                if (unitAmount > 0 && expectUnitCount >= 0 && realReceiptUnitCount >= 0 && refuseUnitAmount >= 0)
                {
                    refuseUnitCount = (expectUnitCount - realReceiptUnitCount) / unitAmount;
                    if (strRefuseUnitCount == "")
                    {
                        textBoxRefuseUnitCount.Text = refuseUnitCount.ToString();
                    }
                }
            }
        }*/

        private void Search(bool savePage = false, int selectID = -1)
        {
            this.pagerWidget.ClearCondition();
            this.pagerWidget.AddCondition("ReceiptTicketID", this.receiptTicketID.ToString());

            this.pagerWidget.Search(savePage, selectID, new Action<ReceiptTicketItemView[]>((r) =>
            {
                this.RefreshTextBoxes();
            }));
        }

        private void TextBoxComponentNo_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities1 = new WMSEntities();
            ReceiptTicket receiptTicket1 = (from rt in wmsEntities1.ReceiptTicket where rt.ID == this.receiptTicketID select rt).FirstOrDefault();
            if (receiptTicket1 == null)
            {
                MessageBox.Show("该收货单已被删除，请刷新后重试", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (receiptTicket1.SupplierID == null)
            {
                MessageBox.Show("请为该收货单选择供应商", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            FormSearch formSearch = new FormSearch((int)receiptTicket1.SupplierID, receiptTicket1.ProjectID, receiptTicket1.WarehouseID);

            formSearch.SetSelectFinishCallback(new Action<int>((id) =>
            {
                WMSEntities wmsEntities = new WMSEntities();
                this.componentID = id;
                ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == this.receiptTicketID select rt).FirstOrDefault();
                WMS.DataAccess.Supply supply = (from c in wmsEntities.Supply where c.ID == id select c).FirstOrDefault();
                var worksheet = this.reoGridControlReceiptItems.Worksheets[0];
                
                if (supply == null)
                {
                    MessageBox.Show("没有找到该零件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (receiptTicket == null)
                {
                    MessageBox.Show("没有找到该收货单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    
                    if (receiptTicket.State == "待收货")
                    {
                        this.ClearTextBoxes();
                       
                        this.realReceiptAmountRefreshMark = true;
                        this.Controls.Find("textBoxState", true)[0].Text = receiptTicket.State;
                        this.Controls.Find("textBoxUnit", true)[0].Text = supply.DefaultReceiptUnit;
                        this.Controls.Find("textBoxUnitAmount", true)[0].Text = supply.DefaultReceiptUnitAmount.ToString();
                        

                        //this.Controls.Find("textBoxManufactureDate", true)[0].Text = supply
                        this.Controls.Find("textBoxInventoryDate", true)[0].Text = DateTime.Now.ToString();
                        if (supply.ValidPeriod != null)
                        {
                            this.Controls.Find("textBoxExpiryDate", true)[0].Text = DateTime.Now.AddDays((double)supply.ValidPeriod).ToString();
                        }
                        //this.Controls.Find("textBoxWrongComponentUnit", true)[0].Text = "个";
                        //this.Controls.Find("textBoxWrongComponentUnitAmount", true)[0].Text = "1";
                        this.Controls.Find("textBoxRefuseAmount", true)[0].Text = "0";
                        this.Controls.Find("textBoxRefuseUnit", true)[0].Text = supply.DefaultReceiptUnit;
                        this.Controls.Find("textBoxRefuseUnitAmount", true)[0].Text = supply.DefaultReceiptUnitAmount.ToString();

                        //this.Controls.Find("textBoxDisqualifiedUnit", true)[0].Text = "个";
                        //this.Controls.Find("textBoxDisqualifiedUnitAmount", true)[0].Text = "1";
                    }
                    this.Controls.Find("textBoxComponentName", true)[0].Text = supply.Component.Name;
                    this.Controls.Find("textBoxSupplyNo", true)[0].Text = supply.No;
                }

            }));
            formSearch.ShowDialog();
        }

        private void InitPanel()
        {
            WMSEntities wmsEntities = new WMSEntities();
            //this.Controls.Clear();
            Utilities.CreateEditPanel(this.tableLayoutPanelProperties, ReceiptMetaData.itemsKeyName);
            //this.control
            //this.RefreshTextBoxes();
            this.reoGridControlReceiptItems.Worksheets[0].SelectionRangeChanged += worksheet_SelectionRangeChanged;


            //TextBox textBoxComponentName = (TextBox)this.Controls.Find("textBoxComponentName", true)[0];
            //textBoxComponentName.Click += textBoxComponentName_Click;
            //textBoxComponentName.ReadOnly = true;
            //textBoxComponentName.BackColor = Color.White;
        }

        private void worksheet_SelectionRangeChanged(object sender, unvell.ReoGrid.Events.RangeEventArgs e)
        {
            try
            {
                this.RefreshTextBoxes();
                TextBox textBox = this.Controls.Find("textBoxInventoryDate", true)[0] as TextBox;
                if (textBox.Text == null)
                {
                    textBox.Text = DateTime.Now.ToString();
                }

            }
            catch
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
        }

        private void RefreshTextBoxes()
        {
            this.ClearTextBoxes();
            this.realReceiptAmountRefreshMark = false;
            var worksheet = this.reoGridControlReceiptItems.Worksheets[0];
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlReceiptItems);
            if (ids.Length == 0)
            {
                this.receiptTicketItemID = -1;
                this.buttonAdd.Text = "增加零件条目";
                return;
            }
            int id = ids[0];
            WMSEntities wmsEntities = new WMSEntities();
            ReceiptTicketItemView receiptTicketItemView =
                        (from s in wmsEntities.ReceiptTicketItemView
                         where s.ID == id
                         select s).FirstOrDefault();
            if (receiptTicketItemView == null)
            {
                MessageBox.Show("系统错误，未找到相应收货单项目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.buttonAdd.Text = "增加零件条目";
                return;
            }
            if (receiptTicketItemView.HasPutwayAmount != null && receiptTicketItemView.HasPutwayAmount != 0)
            {

                this.Controls.Find("textBoxExpectedAmount", true)[0].Enabled = false;
                this.Controls.Find("textBoxRealReceiptAmount", true)[0].Enabled = false;
                this.Controls.Find("textBoxRealReceiptUnitCount", true)[0].Enabled = false;
                this.Controls.Find("textBoxUnit", true)[0].Enabled = false;
                this.Controls.Find("textBoxUnitAmount", true)[0].Enabled = false;
                this.Controls.Find("textBoxRefuseAmount", true)[0].Enabled = false;
                this.Controls.Find("textBoxRefuseUnit", true)[0].Enabled = false;
                this.Controls.Find("textBoxRefuseUnitAmount", true)[0].Enabled = false;

            }
            else
            {
                this.Controls.Find("textBoxExpectedAmount", true)[0].Enabled = true;
                this.Controls.Find("textBoxRealReceiptAmount", true)[0].Enabled = true;
                this.Controls.Find("textBoxRealReceiptUnitCount", true)[0].Enabled = true;
                this.Controls.Find("textBoxUnit", true)[0].Enabled = true;
                this.Controls.Find("textBoxUnitAmount", true)[0].Enabled = true;
                this.Controls.Find("textBoxRefuseAmount", true)[0].Enabled = true;
                this.Controls.Find("textBoxRefuseUnit", true)[0].Enabled = true;
                this.Controls.Find("textBoxRefuseUnitAmount", true)[0].Enabled = true;
            }
            //this.buttonAdd.Text = "复制零件条目";
            this.receiptTicketItemID = int.Parse(receiptTicketItemView.ID.ToString());
            if (receiptTicketItemView.SupplyID != null)
            {
                this.componentID = (int)receiptTicketItemView.SupplyID;
            }
            this.jobPersonID = receiptTicketItemView.JobPersonID == null ? -1 : (int)receiptTicketItemView.JobPersonID;
            this.confirmPersonID = receiptTicketItemView.ConfirmPersonID == null ? -1 : (int)receiptTicketItemView.ConfirmPersonID;

            Utilities.CopyPropertiesToTextBoxes(receiptTicketItemView, this);
            Utilities.CopyPropertiesToComboBoxes(receiptTicketItemView, this);

            return;
        }

        private void ClearTextBoxes()
        {
            foreach (Control control in this.tableLayoutPanelProperties.Controls)
            {
                if (control is TextBox)
                {
                    TextBox textBox = control as TextBox;
                    textBox.Text = "";
                }
            }
        }

        /*
        private void Search()
        {
            this.labelStatus.Text = "搜索中...";

            new Thread(new ThreadStart(() =>
            {
                var wmsEntities = new WMSEntities();
                ReceiptTicketItemView[] receiptTicketItemViews = null;
                try
                {
                    receiptTicketItemViews = wmsEntities.Database.SqlQuery<ReceiptTicketItemView>("SELECT * FROM ReceiptTicketItemView WHERE ReceiptTicketID = @receiptTicketID ORDER BY ID DESC", new SqlParameter("receiptTicketID", this.receiptTicketID)).ToArray();
                }
                catch
                {
                    MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    return;
                }
                this.reoGridControlReceiptItems.Invoke(new Action(() =>
                {
                    this.labelStatus.Text = "搜索完成";
                    var worksheet = this.reoGridControlReceiptItems.Worksheets[0];
                    worksheet.DeleteRangeData(RangePosition.EntireRange);
                    for (int i = 0; i < receiptTicketItemViews.Length; i++)
                    {

                        ReceiptTicketItemView curReceiptTicketItemView = receiptTicketItemViews[i];
                        object[] columns = Utilities.GetValuesByPropertieNames(curReceiptTicketItemView, (from kn in ReceiptMetaData.itemsKeyName select kn.Key).ToArray());
                        for (int j = 0; j < worksheet.Columns; j++)
                        {
                            if (columns[j] == null)
                            {
                                worksheet[i, j] = columns[j];
                            }
                            else
                            {
                                if (columns[j] is DateTime)
                                {
                                    worksheet[i, j] = columns[j].ToString();
                                }
                                else
                                {
                                    worksheet[i, j] = columns[j];
                                }
                            }
                        }
                        //worksheet[i, worksheet.Columns-1] = new CheckBox();
                        this.RefreshTextBoxes();
                    }
                }));
                if (receiptTicketItemViews.Length == 0)
                {
                    int m = ReceiptUtilities.GetFirstColumnIndex(ReceiptMetaData.receiptNameKeys);

                    //this.reoGridControl1.Worksheets[0][6, 8] = "32323";
                    this.reoGridControlReceiptItems.Worksheets[0][0, m] = "没有查询到符合条件的记录";
                }


            })).Start();

        }*/

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            ReceiptTicketItem receiptTicketItem = new ReceiptTicketItem();

            string errorInfo;
            if (Utilities.CopyTextBoxTextsToProperties(this, receiptTicketItem, ReceiptMetaData.itemsKeyName, out errorInfo) == false)
            {
                MessageBox.Show(errorInfo, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (receiptTicketItem.InventoryDate == null)
                {
                    MessageBox.Show("存货日期不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                wmsEntities.ReceiptTicketItem.Add(receiptTicketItem);

                try
                {
                    ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == this.receiptTicketID select rt).FirstOrDefault();
                    if (receiptTicket == null)
                    {
                        MessageBox.Show("该收货单不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    ReceiptTicketItem receiptTicketItem1 = (from rti in wmsEntities.ReceiptTicketItem where rti.ReceiptTicketID == this.receiptTicketID && rti.SupplyID == this.componentID select rti).FirstOrDefault();
                    if (receiptTicketItem1 != null)
                    {
                        MessageBox.Show("该收货单中已包含该零件，不能重复添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    receiptTicketItem.SupplyID = this.componentID;
                    receiptTicketItem.ReceiptTicketID = this.receiptTicketID;
                    receiptTicketItem.JobPersonID = JobPersonIDGetter();
                    receiptTicketItem.ConfirmPersonID = ConfirmPersonIDGetter();
                    //ReceiptTicketItemView 
                    wmsEntities.SaveChanges();
                    //receiptTicketItem.ExpectedAmount = receiptTicketItem.ExpectedUnitCount * receiptTicketItem.UnitAmount;
                    receiptTicketItem.ExpectedUnitCount = receiptTicketItem.ExpectedAmount / receiptTicketItem.UnitAmount;
                    if (receiptTicketItem.RealReceiptUnitCount == null)
                    {
                        receiptTicketItem.RealReceiptUnitCount = receiptTicketItem.RealReceiptAmount / receiptTicketItem.UnitAmount;
                    }
                    //receiptTicketItem.RealReceiptAmount = receiptTicketItem.RealReceiptUnitCount * receiptTicketItem.UnitAmount;
                    //receiptTicketItem.ExpectedUnitCount = receiptTicketItem.ExpectedAmount / receiptTicketItem.UnitAmount;
                    //receiptTicketItem.RealReceiptUnitCount = receiptTicketItem.RealReceiptAmount / receiptTicketItem.UnitAmount;
                    //receiptTicketItem.DisqualifiedAmount = receiptTicketItem.DisqualifiedUnitAmount * receiptTicketItem.DisqualifiedUnitCount;
                    //receiptTicketItem.RefuseAmount = receiptTicketItem.RefuseUnitAmount * receiptTicketItem.RefuseUnitCount;
                    receiptTicketItem.RefuseUnitCount = receiptTicketItem.RefuseAmount / receiptTicketItem.RefuseUnitAmount;
                    receiptTicketItem.UnitCount = receiptTicketItem.RealReceiptUnitCount;
                    //receiptTicketItem.WrongComponentAmount = receiptTicketItem.WrongComponentUnitAmount * receiptTicketItem.WrongComponentUnitCount;
                    //receiptTicketItem.ReceiviptAmount = receiptTicketItem.RealReceiptAmount - receiptTicketItem.DisqualifiedAmount - receiptTicketItem.RefuseAmount - receiptTicketItem.WrongComponentAmount;
                    if (receiptTicketItem.ReceiviptAmount < 0)
                    {
                        MessageBox.Show("错件数、自检不良数、拒收数总和不能大于实际收货数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    //receiptTicketItem.ShortageAmount = receiptTicketItem.ExpectedAmount - receiptTicketItem.RealReceiptAmount;
                    //receiptTicketItem.UnitCount = receiptTicketItem.ReceiviptAmount / receiptTicketItem.UnitAmount;
                    receiptTicketItem.ReceiviptAmount = receiptTicketItem.RealReceiptAmount;
                    Supply supply = (from s in wmsEntities.Supply where s.ID == receiptTicketItem.SupplyID select s).FirstOrDefault();
                    if (supply != null)
                    {
                        supply.ReceiveTimes++;
                    }
                    StockInfo stockInfo = new StockInfo();
                    stockInfo.ProjectID = receiptTicket.ProjectID;
                    stockInfo.WarehouseID = receiptTicket.WarehouseID;
                    stockInfo.ReceiptTicketItemID = receiptTicketItem.ID;
                    stockInfo.ReceiptTicketNo = receiptTicketItem.ReceiptTicket.No;
                    stockInfo.OverflowAreaAmount = 0;
                    stockInfo.ShipmentAreaAmount = 0;
                    stockInfo.SubmissionAmount = 0;
                    stockInfo.RejectAreaAmount = 0;
                    stockInfo.SubmissionAmount = 0;
                    stockInfo.ReceiptAreaAmount = 0;

                    if (receiptTicketItem.ReceiviptAmount != null)
                    {
                        stockInfo.ReceiptAreaAmount = receiptTicketItem.ReceiviptAmount;
                    }
                    stockInfo.ExpiryDate = receiptTicketItem.ExpiryDate;
                    stockInfo.SupplyID = receiptTicketItem.SupplyID;
                    stockInfo.InventoryDate = receiptTicketItem.InventoryDate;
                    stockInfo.ManufactureDate = receiptTicketItem.ManufactureDate;

                    //else if (receiptTicketItem.State == "已收货")
                    //{
                    //    stockInfo.OverflowAreaAmount = 0;
                    //    stockInfo.ShipmentAreaAmount = 0;
                    //    stockInfo.SubmissionAmount = 0;
                    //    //stockInfo.RejectAreaAmount;
                    //    stockInfo.SubmissionAmount = 0;
                    //    stockInfo.ReceiptAreaAmount = 0;
                    //    if (receiptTicketItem.ReceiviptAmount != null)
                    //    {
                    //        stockInfo.OverflowAreaAmount = receiptTicketItem.ReceiviptAmount;
                    //    }
                    //}

                    wmsEntities.StockInfo.Add(stockInfo);

                    wmsEntities.SaveChanges();
                    MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Search();
                }
                catch
                {
                    MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    return;
                }

            }
        }

        private void buttonModify_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControlReceiptItems.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int receiptItemID;
                try
                {
                    receiptItemID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                }
                catch
                {
                    MessageBox.Show("请选择一项进行修改！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //int oldRejectAreaAmount;
                ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == this.receiptTicketID select rt).FirstOrDefault();
                if (receiptTicket != null)
                {

                    int oldReceiptAreaAmount;
                    ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == receiptItemID select rti).FirstOrDefault();
                    if (receiptTicketItem == null)
                    {
                        MessageBox.Show("找不到该收货单条目，可能已被删除，请刷新后查看！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    
                    Supply supply = (from s in wmsEntities.Supply where s.ID == receiptTicketItem.SupplyID select s).FirstOrDefault();
                    ReceiptTicketItem receiptTicketItem1 = (from rti in wmsEntities.ReceiptTicketItem where rti.ReceiptTicketID == this.receiptTicketID && rti.SupplyID == this.componentID select rti).FirstOrDefault();
                    if (receiptTicketItem1 != null)
                    {
                        if (receiptTicketItem1.ID != receiptTicketItem.ID)
                        {
                            MessageBox.Show("该收货单中已包含该零件，不能重复添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    if (supply != null)
                    {
                        supply.ReceiveTimes--;
                    }
                    oldReceiptAreaAmount = receiptTicketItem.ReceiviptAmount == null ? 0 : (int)receiptTicketItem.ReceiviptAmount;
                    //oldRejectAreaAmount = receiptTicketItem.DisqualifiedAmount == null ? 0 : (int)receiptTicketItem.DisqualifiedAmount;
                    string errorInfo;
                    if (Utilities.CopyTextBoxTextsToProperties(this, receiptTicketItem, ReceiptMetaData.itemsKeyName, out errorInfo) == false)
                    {
                        MessageBox.Show(errorInfo, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {

                        if (this.ConfirmPersonIDGetter() != -1)
                        {
                            receiptTicketItem.ConfirmPersonID = this.ConfirmPersonIDGetter();
                        }
                        if (this.JobPersonIDGetter() != -1)
                        {
                            receiptTicketItem.JobPersonID = this.JobPersonIDGetter();
                        }

                        //receiptTicketItem.ReceiviptAmount = receiptTicketItem.UnitAmount * receiptTicketItem.UnitCount;
                        new Thread(() =>
                        {
                            try
                            {
                                //receiptTicketItem.RealReceiptAmount = receiptTicketItem.RealReceiptUnitCount * receiptTicketItem.UnitAmount;

                                receiptTicketItem.UnitCount = receiptTicketItem.RealReceiptUnitCount;
                                receiptTicketItem.ReceiviptAmount = receiptTicketItem.RealReceiptAmount;
                                //receiptTicketItem.DisqualifiedAmount = receiptTicketItem.DisqualifiedUnitAmount * receiptTicketItem.DisqualifiedUnitCount;
                                //receiptTicketItem.RefuseAmount = receiptTicketItem.RefuseUnitAmount * receiptTicketItem.RefuseUnitCount;
                                receiptTicketItem.RefuseUnitCount = receiptTicketItem.RefuseAmount / receiptTicketItem.RefuseUnitAmount;
                                receiptTicketItem.UnitCount = receiptTicketItem.RealReceiptUnitCount;
                                //receiptTicketItem.ExpectedAmount = receiptTicketItem.ExpectedUnitCount * receiptTicketItem.UnitAmount;
                                //receiptTicketItem.ReceiviptAmount = receiptTicketItem.RealReceiptAmount;
                                //receiptTicketItem.WrongComponentAmount = receiptTicketItem.WrongComponentUnitAmount * receiptTicketItem.WrongComponentUnitCount;
                                //receiptTicketItem.ReceiviptAmount = receiptTicketItem.RealReceiptAmount - receiptTicketItem.RefuseAmount;
                                if (receiptTicketItem.ReceiviptAmount < 0)
                                {

                                    MessageBox.Show("实际收货数量不能小于0", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                if (receiptTicketItem.RefuseAmount < 0)
                                {
                                    MessageBox.Show("拒收数量不能小于0", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                if (receiptTicketItem.ExpectedAmount < 0)
                                {
                                    MessageBox.Show("订单数量不能小于0", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                //receiptTicketItem.ShortageAmount = receiptTicketItem.ExpectedAmount - receiptTicketItem.RealReceiptAmount;
                                //receiptTicketItem.UnitCount = receiptTicketItem.ReceiviptAmount / receiptTicketItem.UnitAmount;
                                if (receiptTicketItem.HasPutwayAmount == null || receiptTicketItem.HasPutwayAmount == 0)
                                {
                                    receiptTicketItem.SupplyID = this.componentID;
                                    supply = (from s in wmsEntities.Supply where s.ID == receiptTicketItem.SupplyID select s).FirstOrDefault();
                                    if (supply != null)
                                    {
                                        supply.ReceiveTimes++;
                                    }
                                    StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == receiptTicketItem.ID select si).FirstOrDefault();
                                    if (stockInfo == null)
                                    {
                                        //MessageBox.Show("该库存信息已被删除");
                                    }
                                    else
                                    {
                                        stockInfo.SupplyID = receiptTicketItem.SupplyID;
                                        if (receiptTicketItem.State == "待收货")
                                        {
                                            stockInfo.ReceiptAreaAmount = receiptTicketItem.ReceiviptAmount;
                                            //stockInfo.RejectAreaAmount = receiptTicketItem.DisqualifiedAmount;
                                        }
                                        else if (receiptTicketItem.State == "已收货")
                                        {
                                            SubmissionTicketItem submissionTicketItem = (from sti in wmsEntities.SubmissionTicketItem where sti.ReceiptTicketItemID == receiptTicketItem.ID select sti).FirstOrDefault();
                                            if (submissionTicketItem != null)
                                            {
                                                stockInfo.OverflowAreaAmount = receiptTicketItem.RealReceiptAmount + (submissionTicketItem.ReturnAmount == null ? 0 : (decimal)submissionTicketItem.ReturnAmount) - (submissionTicketItem.SubmissionAmount == null ? 0 : (decimal)submissionTicketItem.SubmissionAmount);
                                                //stockInfo.RejectAreaAmount = receiptTicketItem.DisqualifiedAmount + submissionTicketItem.RejectAmount;
                                                submissionTicketItem.ArriveAmount = receiptTicketItem.RealReceiptAmount;
                                            }
                                            else
                                            {
                                                stockInfo.OverflowAreaAmount = receiptTicketItem.ReceiviptAmount;
                                                // stockInfo.RejectAreaAmount = receiptTicketItem.DisqualifiedAmount;
                                            }
                                            stockInfo.ExpiryDate = receiptTicketItem.ExpiryDate;
                                            stockInfo.SupplyID = receiptTicketItem.SupplyID;
                                            stockInfo.ManufactureDate = receiptTicketItem.ManufactureDate;
                                            stockInfo.InventoryDate = receiptTicketItem.InventoryDate;
                                        }
                                        else if (receiptTicketItem.State == "拒收")
                                        {
                                            if (receiptTicketItem.SubmissionTicketItem.Count != 0)
                                            {
                                                SubmissionTicketItem submissionTicketItem = receiptTicketItem.SubmissionTicketItem.FirstOrDefault();
                                                if (submissionTicketItem != null)
                                                {
                                                    submissionTicketItem.ArriveAmount = receiptTicketItem.RealReceiptAmount;
                                                    stockInfo.RejectAreaAmount = receiptTicketItem.RealReceiptAmount + submissionTicketItem.ReturnAmount - submissionTicketItem.SubmissionAmount;
                                                    if (stockInfo.RejectAreaAmount < 0)
                                                    {
                                                        MessageBox.Show("收货数量不能小于送检数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                        return;
                                                    }
                                                }
                                            }
                                        }
                                        else if (receiptTicketItem.State == "送检中")
                                        {
                                            SubmissionTicketItem submissionTicketItem = (from sti in wmsEntities.SubmissionTicketItem where sti.ReceiptTicketItemID == receiptTicketItem.ID select sti).FirstOrDefault();
                                            if (submissionTicketItem != null)
                                            {
                                                submissionTicketItem.ArriveAmount = receiptTicketItem.ReceiviptAmount;
                                                stockInfo.ReceiptAreaAmount = receiptTicketItem.ReceiviptAmount - (submissionTicketItem.SubmissionAmount == null ? 0 : (decimal)submissionTicketItem.SubmissionAmount);
                                                if (stockInfo.ReceiptAreaAmount < 0)
                                                {
                                                    MessageBox.Show("实际收货数量不能小于送检数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                    return;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            receiptTicketItem.ReceiviptAmount = oldReceiptAreaAmount;
                                            //receiptTicketItem.DisqualifiedAmount = oldRejectAreaAmount;
                                        }
                                    }
                                    wmsEntities.SaveChanges();
                                    MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Search();
                                }
                                else
                                {
                                    KeyName[] keyName = (from kn in ReceiptMetaData.itemsKeyName where kn.Key != "ExpectedAmount" || kn.Key != "RealReceiptAmount" || kn.Key != "RealReceiptUnitCount" || kn.Key != "Unit" || kn.Key != "UnitAmount" || kn.Key != "RefuseAmount" || kn.Key != "RefuseUnitCount" || kn.Key != "RefuseUnitAmount" select kn).ToArray();
                                    string errorInfo2;
                                    if (Utilities.CopyTextBoxTextsToProperties(this, receiptTicketItem, keyName, out errorInfo2) == false)
                                    {
                                        MessageBox.Show(errorInfo2, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }
                                    receiptTicketItem.SupplyID = this.componentID;
                                    StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == receiptTicketItem.ID select si).FirstOrDefault();
                                    //stockInfo.SupplyID = receiptTicketItem.SupplyID;
                                    if (stockInfo != null)
                                    {
                                        stockInfo.SupplyID = this.componentID;
                                    }
                                    wmsEntities.SaveChanges();
                                    MessageBox.Show("成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    this.Search();
                                }

                            }
                            catch
                            {
                                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                                return;
                            }
                        }).Start();
                    }
                }
                //else
                //{
                //    try
                //    {
                //        ReceiptTicketItem receiptTicketItem = (from rt in wmsEntities.ReceiptTicketItem where rt.ID == receiptItemID select rt).FirstOrDefault();
                //        if (receiptTicketItem == null)
                //        {
                //            MessageBox.Show("该收货单可能已被删除，请刷新后查看！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //            return;
                //        }
                //        
                //    }
                //    catch
                //    {
                //        MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                //        return;
                //    }
                //}
                //}

            }
            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
        }



        private void buttonAddItem_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControlReceiptItems.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int receiptItemID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == receiptItemID select rti).Single();
                if (receiptTicketItem.State == "送检中")
                {
                    MessageBox.Show("该条目已送检", "提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                else
                {
                    SubmissionTicketItem submissionTicketItem = ReceiptUtilities.ReceiptTicketItemToSubmissionTicketItem(receiptTicketItem, submissionTicket.ID);
                    wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicketItem SET State = '送检中' WHERE ID = @receiptTicketItemID", new SqlParameter("receiptTicketItemID", receiptTicketItem.ID));
                    wmsEntities.SubmissionTicketItem.Add(submissionTicketItem);
                    wmsEntities.SaveChanges();
                    MessageBox.Show("成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControlReceiptItems.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int receiptItemID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                new Thread(() =>
                {
                    try
                    {
                        ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == receiptItemID select rti).FirstOrDefault();
                        if (receiptTicketItem != null)
                        {
                            Supply supply = (from s in wmsEntities.Supply where s.ID == receiptTicketItem.SupplyID select s).FirstOrDefault();
                            if (supply != null)
                            {
                                supply.ReceiveTimes--;
                            }
                            wmsEntities.SaveChanges();
                            wmsEntities.Database.ExecuteSqlCommand("DELETE FROM StockInfo WHERE ReceiptTicketItemID = @receiptTicketItemID", new SqlParameter("receiptTicketItemID", receiptItemID));
                            wmsEntities.Database.ExecuteSqlCommand("DELETE FROM ReceiptTicketItem WHERE ID = @receiptTicketItemID", new SqlParameter("receiptTicketItemID", receiptItemID));
                            MessageBox.Show("删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("该条目已被删除，无法再次删除，请刷新后查看！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                    catch
                    {
                        MessageBox.Show("该收货单零件已送检或收货，无法删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    this.Search();
                }).Start();
            }
            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void reoGridControlReceiptItems_Click(object sender, EventArgs e)
        {

        }

        private void textBoxNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxSupplierName_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }
        private void buttonAdd_MouseEnter(object sender, EventArgs e)
        {
            buttonAdd.BackgroundImage = WMS.UI.Properties.Resources.bottonW_s;
        }

        private void buttonAdd_MouseLeave(object sender, EventArgs e)
        {
            buttonAdd.BackgroundImage = WMS.UI.Properties.Resources.bottonW_q;
        }

        private void buttonAdd_MouseDown(object sender, MouseEventArgs e)
        {
            buttonAdd.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }



        private void buttonDelete_MouseEnter(object sender, EventArgs e)
        {
            buttonDelete.BackgroundImage = WMS.UI.Properties.Resources.bottonW_s;
        }

        private void buttonDelete_MouseLeave(object sender, EventArgs e)
        {
            buttonDelete.BackgroundImage = WMS.UI.Properties.Resources.bottonW_q;
        }

        private void buttonDelete_MouseDown(object sender, MouseEventArgs e)
        {
            buttonDelete.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private void buttonModify_MouseEnter(object sender, EventArgs e)
        {
            buttonModify.BackgroundImage = WMS.UI.Properties.Resources.bottonW_s;
        }

        private void buttonModify_MouseLeave(object sender, EventArgs e)
        {
            buttonModify.BackgroundImage = WMS.UI.Properties.Resources.bottonW_q;
        }

        private void buttonModify_MouseDown(object sender, MouseEventArgs e)
        {
            buttonModify.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }



        private void buttonImport_MouseEnter(object sender, EventArgs e)
        {
            buttonImport.BackgroundImage = WMS.UI.Properties.Resources.bottonW_s;
        }

        private void buttonImport_MouseLeave(object sender, EventArgs e)
        {
            buttonImport.BackgroundImage = WMS.UI.Properties.Resources.bottonW_q;
        }

        private void buttonImport_MouseDown(object sender, MouseEventArgs e)
        {
            buttonImport.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }



        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void buttonImport_Click(object sender, EventArgs e)
        {
            this.standardImportForm = new StandardImportForm<ReceiptTicketItem>(ReceiptMetaData.itemsKeyName, importItemHandler, importFinishedCallback, "导入收货单条目");
            this.standardImportForm.AddDefaultValue("RealReceiptAmount", "SELECT @ExpectedAmount WHERE @focus = 'ExpectedAmount'", true, true);
            this.standardImportForm.AddDefaultValue("RealReceiptUnitCount", "SELECT CAST(@RealReceiptAmount AS DECIMAL(18,3))/CAST(@UnitAmount AS DECIMAL(18,3))", true, true);
            //this.standardImportForm.AddDefaultValue("RealReceiptAmount", "SELECT CAST(@RealReceiptUnitCount AS DECIMAL) * CAST(@UnitAmount AS DECIMAL) WHERE @focus = 'RealReceiptUnitCount'", true, true);
            this.standardImportForm.AddDefaultValue("Unit", string.Format("SELECT DefaultReceiptUnit FROM Supply WHERE [No] = @Component AND ProjectID = {0} AND WarehouseID = {1} AND isHistory = 0;", this.projectID, this.warehouseID));
            this.standardImportForm.AddDefaultValue("UnitAmount", string.Format("SELECT DefaultReceiptUnitAmount FROM Supply WHERE [No] = @Component AND ProjectID = {0} AND WarehouseID = {1} AND isHistory = 0;", this.projectID, this.warehouseID));
            this.standardImportForm.AddDefaultValue("Unit", string.Format("SELECT DefaultReceiptUnit FROM SupplyView WHERE ComponentName = @Component AND ProjectID = {0} AND WarehouseID = {1} AND isHistory = 0;", this.projectID, this.warehouseID));
            this.standardImportForm.AddDefaultValue("UnitAmount", string.Format("SELECT DefaultReceiptUnitAmount FROM SupplyView WHERE ComponentName = @Component AND ProjectID = {0} AND WarehouseID = {1} AND isHistory = 0;", this.projectID, this.warehouseID));
            this.standardImportForm.AddDefaultValue("RefuseAmount", "SELECT '0'");
            this.standardImportForm.AddDefaultValue("RefuseUnit", string.Format("SELECT DefaultReceiptUnit FROM Supply WHERE [No] = @Component AND ProjectID = {0} AND WarehouseID = {1} AND isHistory = 0;", this.projectID, this.warehouseID));
            //this.standardImportForm.AddDefaultValue("RefuseUnit","SELECT '个'");
            this.standardImportForm.AddDefaultValue("RefuseUnitAmount", string.Format("SELECT DefaultReceiptUnitAmount FROM Supply WHERE [No] = @Component AND ProjectID = {0} AND WarehouseID = {1} AND isHistory = 0;", this.projectID, this.warehouseID));
            this.standardImportForm.AddDefaultValue("InventoryDate", string.Format("SELECT '{0}'", DateTime.Now.ToString()));

            standardImportForm.AddAssociation("Component", string.Format("SELECT No,SupplierName FROM SupplyView WHERE ProjectID={0} AND WarehouseID = {1} AND IsHistory=0 AND No LIKE '%'+@value+'%'; ", this.projectID, this.warehouseID));
            standardImportForm.AddAssociation("Component", string.Format("SELECT DISTINCT ComponentName FROM SupplyView WHERE ProjectID={0} AND WarehouseID = {1} AND IsHistory=0 AND ComponentName LIKE '%'+@value+'%'; ", this.projectID, this.warehouseID));
            standardImportForm.AddAssociation("JobPersonName", string.Format("SELECT Name FROM Person WHERE Name LIKE '%'+@value+'%'"));
            standardImportForm.AddAssociation("ConfirmPersonName", string.Format("SELECT Name FROM Person WHERE Name LIKE '%'+@value+'%'"));
            //this.standardImportForm.AddDefaultValue("ExpiryDate", string.Format("SELECT CAST(@InventoryDate as datetime) FROM Supply WHERE [No] = @Component AND ProjectID = {0} AND WarehouseID = {1};", this.projectID, this.warehouseID));
            this.standardImportForm.Show();
        }

        private bool importItemHandler(List<ReceiptTicketItem> results, Dictionary<string, string[]> unimportedColumns)
        {
            WMSEntities wmsEntities = new WMSEntities();
            ReceiptTicket receiptTicket = (from r in wmsEntities.ReceiptTicket
                                           where r.ID == this.receiptTicketID
                                           select r).FirstOrDefault();
            if (receiptTicket == null)
            {
                MessageBox.Show("导入失败：收货单不存在，可能已被删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            string[] supplyNoNames = (from s in unimportedColumns where s.Key == "Component" select s.Value).FirstOrDefault();
            string[] jobPersonNames = (from s in unimportedColumns where s.Key == "JobPersonName" select s.Value).FirstOrDefault();
            string[] confirmPersonNames = (from s in unimportedColumns where s.Key == "ConfirmPersonName" select s.Value).FirstOrDefault();
            List<int> supplyIDs = new List<int>();
            for (int i = 0; i < results.Count; i++)
            {
                string supplyNoName = supplyNoNames[i];
                if (Utilities.GetSupplyOrComponentAmbiguous(supplyNoName, out DataAccess.Component component, out Supply supply, out string errorMessage, receiptTicket.SupplierID.Value, wmsEntities) == false)
                {
                    MessageBox.Show(string.Format("行{0}：{1}", i + 1, errorMessage), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                //如果输入的不是supply，那一定是Component
                if (supply == null)
                {
                    supply = (from s in wmsEntities.Supply
                              where s.SupplierID == receiptTicket.SupplierID
                              && s.ComponentID == component.ID
                              select s).FirstOrDefault();
                }
                if (supply != null)
                {
                    results[i].SupplyID = supply.ID;
                    supply.ReceiveTimes++;
                }
                //if (supplyNoName == null)
                //{
                //    MessageBox.Show("第" + (i + 1).ToString() + "行中，没有填写零件编号/名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return false;
                //}
                ////精确搜索
                //SupplyView[] supplyViewNo = (from sv in wmsEntities.SupplyView where sv.No == supplyNoName && sv.SupplierID == this.ReceiptTicketView.SupplierID select sv).ToArray();
                //if (supplyViewNo.Length == 0)
                //{
                //    //模糊搜索
                //    supplyViewNo = (from sv in wmsEntities.SupplyView where sv.No.Contains(supplyNoName) && sv.SupplierID == this.ReceiptTicketView.SupplierID select sv).ToArray();
                //}
                //SupplyView[] supplyViewName = (from sv in wmsEntities.SupplyView where sv.ComponentName == supplyNoName && sv.SupplierID == this.ReceiptTicketView.SupplierID select sv).ToArray();
                //if (supplyViewNo.Length == 0)
                //{
                //    supplyViewName = (from sv in wmsEntities.SupplyView where sv.ComponentName.Contains(supplyNoName) && sv.SupplierID == this.ReceiptTicketView.SupplierID select sv).ToArray();
                //}
                //if (supplyViewNo.Length + supplyViewName.Length > 1)
                //{
                //    string errorMsg = "";
                //    int n = 0;
                //    foreach (SupplyView sv in supplyViewNo)
                //    {
                //        errorMsg += (sv.No.ToString() + "\n");
                //    }
                //    foreach(SupplyView sv in supplyViewName)
                //    {
                //        errorMsg += (sv.ComponentName.ToString() + "\n");
                //    }
                //    if (n > 10)
                //    {
                //        MessageBox.Show("第" + (i + 1) + "行中，零件不明确，符合条件的零件有" + n + "条！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    }
                //    else
                //    {
                //        MessageBox.Show("第" + (i + 1) + "行中，零件不明确，您是否要搜索:\n" + errorMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    }
                //    return false;
                //}
                //if (supplyViewNo == null || supplyViewNo.Length == 0)
                //{
                //    //SupplyView[] supplyViewName = (from sv in wmsEntities.SupplyView where sv.ComponentName == supplyNoName && sv.SupplierID == this.ReceiptTicketView.SupplierID select sv).ToArray();

                //    if (supplyViewName.Length == 0)
                //    {
                //        MessageBox.Show("第" + (i + 1).ToString() + "行中，无法找到该供应商提供的该零件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        return false;
                //    }
                //    else if (supplyViewName.Length > 1)
                //    {
                //        MessageBox.Show("第" + (i + 1).ToString() + "行中，有多个零件重名，请输入零件编号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        return false;
                //    }
                //    else
                //    {
                //        results[i].SupplyID = supplyViewName[0].ID;
                //    }
                //}
                //else
                //{
                //    results[i].SupplyID = supplyViewNo[0].ID;
                //}
                //if (results[i].SupplyID == null)
                //{
                //    MessageBox.Show("第" + (i + 1) + "行，无法找到该零件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return false;
                //}
                ReceiptTicketItem result = results[i];
                int supplyIDCur = (int)results[i].SupplyID;
                int count = (from s in supplyIDs where s == supplyIDCur select s).ToArray().Length;
                supplyIDs.Add((int)results[i].SupplyID);
                ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ReceiptTicketID == this.receiptTicketID && rti.SupplyID == supplyIDCur select rti).FirstOrDefault();
                if (count != 0 || receiptTicketItem != null)
                {
                    MessageBox.Show("第" + (i + 1).ToString() + "行，零件在该收货单被添加过，不能重复添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (result.UnitAmount <= 0 || result.RefuseUnitAmount <= 0)
                {
                    MessageBox.Show("第" + (i + 1) + "行中，单位数量和拒收单位数量必须大于0", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                string jobPersonName = jobPersonNames[i];
                string confirmPersonName = confirmPersonNames[i];
                if (jobPersonName == "" || jobPersonName == null)
                {
                    results[i].JobPersonID = -1;
                }
                else
                {
                    PersonView personView = (from p in wmsEntities.PersonView where p.Name == jobPersonName select p).FirstOrDefault();
                    if (personView == null)
                    {
                        MessageBox.Show("第" + (i + 1).ToString() + "中，没有“" + jobPersonName + "”这个人", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    results[i].JobPersonID = personView.ID;
                }
                if (confirmPersonName == "" || confirmPersonName == null)
                {
                    results[i].ConfirmPersonID = -1;
                }
                else
                {
                    PersonView personView = (from p in wmsEntities.PersonView where p.Name == confirmPersonName select p).FirstOrDefault();
                    if (personView == null)
                    {
                        MessageBox.Show("第" + (i + 1).ToString() + "中，没有“" + confirmPersonName + "”这个人", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    results[i].ConfirmPersonID = personView.ID;
                }
                if (results[i].InventoryDate == null)
                {
                    results[i].InventoryDate = DateTime.Now;
                }
                results[i].ReceiptTicketID = this.receiptTicketID;
                results[i].State = "待收货";
                //results[i].RefuseAmount = results[i].RefuseUnitAmount * results[i].RefuseUnitCount;
                results[i].RefuseUnitCount = results[i].RefuseAmount / results[i].RefuseUnitAmount;
                //results[i].RealReceiptAmount = results[i].RealReceiptUnitCount * results[i].UnitAmount;
                //results[i].ExpectedAmount = results[i].ExpectedUnitCount * results[i].UnitAmount;
                results[i].ExpectedUnitCount = results[i].ExpectedAmount / results[i].UnitAmount;
                results[i].UnitCount = results[i].RealReceiptUnitCount;
                results[i].ReceiviptAmount = results[i].RealReceiptAmount;
                //if (results[i].RefuseAmount > results[i].ExpectedAmount)
                //{
                //    MessageBox.Show("第" + (i + 1).ToString() + "行中，拒收数量不能大于期待数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return false;
                //}
                //if (results[i].ReceiviptAmount > results[i].ExpectedAmount)
                //{
                //    MessageBox.Show("第" + (i + 1).ToString() + "行中， 实际收货数量大于期待数量!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return false;
                //}

                wmsEntities.ReceiptTicketItem.Add(results[i]);
            }

            wmsEntities.SaveChanges();
            for (int i = 0; i < results.Count; i++)
            {
                StockInfo stockInfo = new StockInfo();
                stockInfo.ExpiryDate = results[i].ExpiryDate;
                stockInfo.SupplyID = results[i].SupplyID;
                stockInfo.ProjectID = this.projectID;
                stockInfo.WarehouseID = this.warehouseID;
                stockInfo.InventoryDate = results[i].InventoryDate;
                stockInfo.ManufactureDate = results[i].ManufactureDate;
                stockInfo.ReceiptTicketItemID = results[i].ID;
                stockInfo.ReceiptTicketNo = this.ReceiptTicketView.No;
                stockInfo.OverflowAreaAmount = 0;
                stockInfo.RejectAreaAmount = 0;
                stockInfo.SubmissionAmount = 0;
                stockInfo.ShipmentAreaAmount = 0;
                stockInfo.ReceiptAreaAmount = 0;
                if (results[i].ReceiviptAmount != null)
                {
                    stockInfo.ReceiptAreaAmount = results[i].ReceiviptAmount;
                }

                wmsEntities.StockInfo.Add(stockInfo);
            }
            wmsEntities.SaveChanges();

            this.Search();

            MessageBox.Show("导入成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.standardImportForm.Close();


            return false;
        }

        private void importFinishedCallback()
        {


        }

        private void reoGridControlReceiptItems_Click_1(object sender, EventArgs e)
        {

        }

        private void FormReceiptItems_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
    /*
                        StockInfo stockInfo = new StockInfo();
                        stockInfo.ExpiryDate = results[i].ExpiryDate;
                        stockInfo.SupplyID = results[i].SupplyID;
                        stockInfo.ProjectID = this.projectID;
                        stockInfo.WarehouseID = this.warehouseID;
                        stockInfo.InventoryDate = results[i].InventoryDate;
                        stockInfo.ManufactureDate = results[i].ManufactureDate;
                        stockInfo.ReceiptTicketItemID = results[i].ID;
                        stockInfo.ReceiptTicketNo = this.ReceiptTicketView.No;
                        stockInfo.OverflowAreaAmount = 0;
                        stockInfo.RejectAreaAmount = 0;
                        stockInfo.ShipmentAreaAmount = 0;
                        stockInfo.ReceiptAreaAmount = 0;
                        if (results[i].ReceiviptAmount != null)
                        {
                            stockInfo.ReceiptAreaAmount = results[i].ReceiviptAmount;
                        }

                        wmsEntities.StockInfo.Add(stockInfo);*/
}

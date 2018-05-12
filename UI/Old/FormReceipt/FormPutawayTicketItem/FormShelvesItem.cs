using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.DataAccess;
using unvell.ReoGrid;
using System.Threading;
using System.Data.SqlClient;

namespace WMS.UI.FormReceipt
{
    public partial class FormShelvesItem : Form
    {
        private int projectID;
        private int warehouseID;
        private int userID;
        private int putawayTicketItemID = -1;
        private int putawayTicketID;
        private string key = null;
        private string value = null;
        WMSEntities wmsEntities = new WMSEntities();
        private Action CallBack = null;
        private Func<int> JobPersonIDGetter = null;
        private Func<int> ConfirmIDGetter = null;
        private int jobPersonID = -1;
        private int confirmPersonID = -1;
        private PagerWidget<PutawayTicketItemView> pagerWidget;
        public FormShelvesItem()
        {
            InitializeComponent();
            InitComponents();
        }

        public FormShelvesItem(int putawayTicketID) : this()
        {
            this.putawayTicketID = putawayTicketID;
        }

        public FormShelvesItem(int projectID, int warehouseID, int userID, string key, string value) : this()
        {
            this.projectID = projectID;
            this.warehouseID = warehouseID;
            this.userID = userID;
            this.key = key;
            this.value = value;
            FormSelectPerson.DefaultPosition = FormBase.Position.RECEIPT;
        }

        private void SetTableLayOut()
        {
            this.buttonAll.Visible = true;

            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));

            this.tableLayoutPanelProperties.ColumnCount = 6;
            this.tableLayoutPanelProperties.RowCount = 4;
            for (int i = 0; i < 3; i++)
            {
                this.tableLayoutPanelProperties.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11F));
                this.tableLayoutPanelProperties.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));

            }
            for (int i = 0; i < 6; i++)
            {
                this.tableLayoutPanelProperties.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            }
            for (int i = 0; i < 5; i++)
            {
                this.tableLayoutPanelProperties.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            }

        }


        public void SetCallBack(Action action)
        {
            this.CallBack = action;
        }

        private void FormShelvesItem_Load(object sender, EventArgs e)
        {
            if (this.key != null && this.value != null)
            {
                this.SetTableLayOut();
            }
            new Thread(() =>
            {
                if (this.IsDisposed) return;
                this.Invoke(new Action(() =>
                {
                    this.tableLayoutPanelProperties.Visible = false;
                    InitPanel();
                    this.tableLayoutPanelProperties.Visible = true;
                    TextBox textBoxPutawayAmount = (TextBox)this.Controls.Find("textBoxPutawayAmount", true)[0];
                    textBoxPutawayAmount.TextChanged += textBoxPuawayAmount_TextChanged;
                }));
            }).Start();
            WMSEntities wmsEntities = new WMSEntities();
            pagerWidget = new PagerWidget<PutawayTicketItemView>(this.reoGridControlPutaway, ReceiptMetaData.putawayTicketItemKeyName, projectID, warehouseID);
            this.panel4.Controls.Add(pagerWidget);
            pagerWidget.ClearCondition();
            pagerWidget.AddOrderBy("StockInfoShipmentAreaAmount / (ComponentDailyProduction * ComponentSingleCarUsageAmount)");
            pagerWidget.AddOrderBy("ReceiptTicketItemInventoryDate");
            Search();
            this.pagerWidget.Show();

            //this.RefreshTextBoxes();
        }

        private void textBoxPuawayAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox textBoxPutawayAmount = (TextBox)sender;
                TextBox textBoxUnitAmount = (TextBox)this.Controls.Find("textBoxUnitAmount", true)[0];
                TextBox textBoxPutawayUnitCount = (TextBox)this.Controls.Find("textBoxUnitCount", true)[0];
                string strPutawayAmount = textBoxPutawayAmount.Text;
                string strUnitAmount = textBoxUnitAmount.Text;
                string strPutawayUnitCount = textBoxPutawayUnitCount.Text;
                decimal putawayAmount;
                decimal unitAmount;
                decimal putawayUnitCount = 0;
                if (decimal.TryParse(strPutawayAmount, out putawayAmount) == true && decimal.TryParse(strUnitAmount, out unitAmount) == true)
                {
                    putawayUnitCount = putawayAmount / unitAmount;
                }
                if (putawayUnitCount >= 0)
                {
                    textBoxPutawayUnitCount.Text = Utilities.DecimalToString(putawayUnitCount);
                }
            }
            catch
            {

            }
        }

        private void Search(bool savePage = false, int selectID = -1)
        {
            this.pagerWidget.ClearCondition();
            if (this.key != null && this.value != null)
            {
                pagerWidget.AddCondition(this.key, this.value);
                pagerWidget.Search();
            }
            else
            {
                //pagerWidget.AddCondition("状态", "待上架");
                //pagerWidget.AddCondition("状态", "部分上架");
            }

            //if (this.putawayTicketID != -1)
            //{
            //    pagerWidget.AddCondition("上架单ID", this.putawayTicketID.ToString());
            //}

            this.pagerWidget.Search(savePage, selectID, (putawayTicket) => { this.RefreshTextBoxes(); });
            //this.pagerWidget.Show();
        }



        private void InitPanel()
        {
            //WMSEntities wmsEntities = new WMSEntities();
            //this.Controls.Clear();
            Utilities.CreateEditPanel(this.tableLayoutPanelProperties, ReceiptMetaData.putawayTicketItemKeyName);
            this.ConfirmIDGetter = Utilities.BindTextBoxSelect<FormSelectPerson, Person>(this, "textBoxConfirmPersonName", "Name");
            this.JobPersonIDGetter = Utilities.BindTextBoxSelect<FormSelectPerson, Person>(this, "textBoxJobPersonName", "Name");
            //Label label = new Label();
            //label.Text = "上架数量";
            //this.tableLayoutPanelProperties.Controls.Add(label);
            //TextBox textboxPutawayAmount = new TextBox();
            //textboxPutawayAmount.Name = "textboxPutawayAmount";
            //this.tableLayoutPanelProperties.Controls.Add(textboxPutawayAmount);
            //textboxPutawayAmount.Name;
            this.reoGridControlPutaway.Worksheets[0].SelectionRangeChanged += worksheet_SelectionRangeChanged;

            //TextBox textBoxComponentName = (TextBox)this.Controls.Find("textBoxComponentName", true)[0];
            //textBoxComponentName.Click += textBoxComponentName_Click;
            //textBoxComponentName.ReadOnly = true;
            //textBoxComponentName.BackColor = Color.White;
        }

        private void worksheet_SelectionRangeChanged(object sender, unvell.ReoGrid.Events.RangeEventArgs e)
        {
            this.RefreshTextBoxes();
        }

        private void RefreshTextBoxes()
        {
            //WMSEntities wmsEntities = new WMSEntities();
            this.ClearTextBoxes();
            var worksheet = this.reoGridControlPutaway.Worksheets[0];
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlPutaway);
            if (ids.Length == 0)
            {
                this.putawayTicketItemID = -1;
                return;
            }
            int id = ids[0];
            WMSEntities wmsEntities = new WMSEntities();
            PutawayTicketItemView putawayTicketItemView =
                        (from s in wmsEntities.PutawayTicketItemView
                         where s.ID == id
                         select s).FirstOrDefault();
            this.jobPersonID = putawayTicketItemView.JobPersonID == null ? -1 : (int)putawayTicketItemView.JobPersonID;
            this.confirmPersonID = putawayTicketItemView.ConfirmPersonID == null ? -1 : (int)putawayTicketItemView.ConfirmPersonID;
            if (putawayTicketItemView == null)
            {
                MessageBox.Show("系统错误，未找到相应上架单项目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.putawayTicketItemID = int.Parse(putawayTicketItemView.ID.ToString());
            Utilities.CopyPropertiesToTextBoxes(putawayTicketItemView, this);
            Utilities.CopyPropertiesToComboBoxes(putawayTicketItemView, this);
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

        private void InitComponents()
        {
            //初始化
            //string[] columnNames = (from kn in ReceiptMetaData.putawayTicketItemKeyName select kn.Name).ToArray();
            //初始化表格
            var worksheet = this.reoGridControlPutaway.Worksheets[0];
            worksheet.SelectionMode = WorksheetSelectionMode.Row;
            for (int i = 0; i < ReceiptMetaData.putawayTicketItemKeyName.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = ReceiptMetaData.putawayTicketItemKeyName[i].Name;
                worksheet.ColumnHeaders[i].IsVisible = ReceiptMetaData.putawayTicketItemKeyName[i].Visible;
            }
            worksheet.Columns = ReceiptMetaData.putawayTicketItemKeyName.Length;
        }
        /*
        private void Search()
        {
            this.labelStatus.Text = "搜索中...";
            try
            {
                new Thread(new ThreadStart(() =>
                {
                    var wmsEntities = new WMSEntities();
                //ReceiptTicketView[] receiptTicketViews = null;
                PutawayTicketItemView[] putawayTicketItemView = null;
                //
                //try
                //{
                //    putawayTicketItemView = wmsEntities.Database.SqlQuery<PutawayTicketItemView>("SELECT * FROM PutawayTicketItemView WHERE PutawayTicketID=@putawayTicketID", new SqlParameter("putawayTicketID", putawayTicketID)).ToArray();
                //}
                //catch
                //{
                //    MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                //    return;
                //}

                    PutawayTicketItemView[] putawayTicketItemViews1;
                    if (this.key != null && this.value != null)
                    {
                        putawayTicketItemViews1 = wmsEntities.Database.SqlQuery<PutawayTicketItemView>(string.Format("SELECT * FROM PutawayTicketItemView WHERE {0}=@value AND PutawayTicketWarehouseID = @warehouseID AND PutawayTicketProjectID = @projectID", key), new SqlParameter[] { new SqlParameter("value", value), new SqlParameter("warehouseID", this.warehouseID), new SqlParameter("projectID", this.projectID) }).ToArray();
                    }
                    else
                    {
                        putawayTicketItemViews1 = wmsEntities.Database.SqlQuery<PutawayTicketItemView>("SELECT * FROM PutawayTicketItemView WHERE PutawayTicketWarehouseID = @warehouseID AND PutawayTicketProjectID = @projectID", new SqlParameter[] { new SqlParameter("warehouseID", this.warehouseID), new SqlParameter("projectID", this.projectID) }).ToArray();
                    }
                    putawayTicketItemView =
                    (from ptiv in putawayTicketItemViews1
                     where ptiv.PutawayTicketProjectID == this.projectID && ptiv.PutawayTicketWarehouseID == this.warehouseID
                     orderby ptiv.StockInfoShipmentAreaAmount / (ptiv.ComponentDailyProduction * ptiv.ComponentSingleCarUsageAmount) ascending,
                     ptiv.ReceiptTicketItemInventoryDate ascending
                     select ptiv).ToArray();
                    S
                //
                //string sql = (from ptiv in wmsEntities.PutawayTicketItemView
                //              where ptiv.PutawayTicketProjectID == this.projectID && ptiv.PutawayTicketWarehouseID == this.warehouseID
                //              orderby ptiv.StockInfoShipmentAreaAmount / (ptiv.ComponentDailyProduction * ptiv.ComponentSingleCarUsageAmount) ascending,
                //              ptiv.ReceiptTicketItemInventoryDate ascending
                //              select ptiv).ToString();
                //Console.WriteLine(sql);
                    this.reoGridControlPutaway.Invoke(new Action(() =>
                    {
                        this.labelStatus.Text = "搜索完成";
                        var worksheet = this.reoGridControlPutaway.Worksheets[0];
                        worksheet.DeleteRangeData(RangePosition.EntireRange);
                        int n = 0;
                        for (int i = 0; i < putawayTicketItemView.Length; i++)
                        {
                            if (putawayTicketItemView[i].State == "作废")
                            {
                                continue;
                            }
                            PutawayTicketItemView curputawayTicketItemView = putawayTicketItemView[i];
                            object[] columns = Utilities.GetValuesByPropertieNames(curputawayTicketItemView, (from kn in ReceiptMetaData.putawayTicketItemKeyName select kn.Key).ToArray());
                            for (int j = 0; j < worksheet.Columns; j++)
                            {
                                worksheet[n, j] = columns[j];
                            }
                            n++;
                        }
                    }));
                    this.Invoke(new Action(() => { this.RefreshTextBoxes(); }));
                })).Start();
            }
            catch
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }

        }*/

        private void buttonModify_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlPutaway.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int putawayTicketItemID;
                try
                {
                    putawayTicketItemID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                }
                catch
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                PutawayTicketItem putawayTicketItem = (from pti in wmsEntities.PutawayTicketItem where pti.ID == putawayTicketItemID select pti).FirstOrDefault();
                if (putawayTicketItem == null)
                {
                    MessageBox.Show("修改失败，该收货单可能已被删除，请刷新后查看！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                decimal oldPutawayAmount = 0;
                if (putawayTicketItem.PutawayAmount == null)
                {
                    if (putawayTicketItem.PutawayAmount == null)
                    {
                        putawayTicketItem.PutawayAmount = 0;
                    }
                }

                oldPutawayAmount = (decimal)putawayTicketItem.PutawayAmount;
                if (putawayTicketItem == null)
                {
                    MessageBox.Show("此上架单条目不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    string errorInfo;
                    if (Utilities.CopyTextBoxTextsToProperties(this, putawayTicketItem, ReceiptMetaData.putawayTicketItemKeyName, out errorInfo) == false)
                    {
                        MessageBox.Show(errorInfo, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        if (Utilities.CopyComboBoxsToProperties(this, putawayTicketItem, ReceiptMetaData.putawayTicketItemKeyName) == false)
                        {
                            MessageBox.Show("下拉框获取失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (putawayTicketItem.PutawayAmount == null)
                        {
                            MessageBox.Show("实际上架数量不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == putawayTicketItem.ReceiptTicketItemID select rti).FirstOrDefault();
                        if (putawayTicketItem.UnitAmount == null)
                        {
                            if (receiptTicketItem != null)
                            {
                                putawayTicketItem.UnitAmount = receiptTicketItem.UnitAmount;
                            }

                        }
                        if (putawayTicketItem.PutawayAmount > putawayTicketItem.ScheduledMoveCount)
                        {
                            MessageBox.Show("实际上架数量不能大于计划上架数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }


                        //putawayTicketItem.PutawayAmount = putawayTicketItem.UnitAmount * putawayTicketItem.MoveCount;
                        putawayTicketItem.MoveCount = putawayTicketItem.PutawayAmount / putawayTicketItem.UnitAmount;
                        StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == putawayTicketItem.ReceiptTicketItemID select si).FirstOrDefault();
                        if (stockInfo != null)
                        {
                            stockInfo.OverflowAreaAmount -= putawayTicketItem.PutawayAmount - oldPutawayAmount;
                            stockInfo.ShipmentAreaAmount += putawayTicketItem.PutawayAmount - oldPutawayAmount;
                        }


                        if (putawayTicketItem.ScheduledMoveCount == putawayTicketItem.PutawayAmount)
                        {
                            putawayTicketItem.State = "已上架";
                        }
                        else if (putawayTicketItem.PutawayAmount == 0)
                        {
                            putawayTicketItem.State = "待上架";
                        }
                        else
                        {
                            putawayTicketItem.State = "部分上架";
                        }

                        wmsEntities.SaveChanges();
                        this.modifyMode(putawayTicketItem.PutawayTicketID);
                        MessageBox.Show("上架成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Search();


                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
            /*
            decimal oldPutawayAmount = 0;
            var worksheet = this.reoGridControlPutaway.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int putawayTicketItemID;
                try
                {
                    putawayTicketItemID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                }
                catch
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                PutawayTicketItem putawayTicketItem = (from pti in wmsEntities.PutawayTicketItem where pti.ID == putawayTicketItemID select pti).FirstOrDefault();
                if (putawayTicketItem.ScheduledMoveCount != null)
                {
                    oldPutawayAmount = (decimal)putawayTicketItem.ScheduledMoveCount;
                    //return;
                }
                if (putawayTicketItem == null)
                {
                    MessageBox.Show("错误 无法修改此条目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string errorInfo;
                    if (Utilities.CopyTextBoxTextsToProperties(this, putawayTicketItem, ReceiptMetaData.putawayTicketItemKeyName, out errorInfo) == false)
                    {
                        MessageBox.Show(errorInfo, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        if (Utilities.CopyComboBoxsToProperties(this, putawayTicketItem,ReceiptMetaData.putawayTicketItemKeyName) == false)
                        {
                            MessageBox.Show("下拉框获取失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == putawayTicketItem.ReceiptTicketItemID select rti).FirstOrDefault();
                        if (receiptTicketItem != null)
                        {
                            receiptTicketItem.HasPutwayAmount += putawayTicketItem.ScheduledMoveCount - oldPutawayAmount;
                        }
                        if (this.JobPersonIDGetter() != -1)
                        {
                            this.jobPersonID = JobPersonIDGetter();
                        }
                        if (this.ConfirmIDGetter() != -1)
                        {
                            this.confirmPersonID = ConfirmIDGetter();
                        }

                        putawayTicketItem.JobPersonID = this.jobPersonID;
                        putawayTicketItem.ConfirmPersonID = this.confirmPersonID;
                        new Thread(() =>
                        {

                            wmsEntities.SaveChanges();
                            this.Invoke(new Action(() =>
                            {
                                this.pagerWidget.Search();
                            }));
                            MessageBox.Show("成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);


                        }).Start();
                    }
                }
            }

            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }*/
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlPutaway.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int putawayTicketItemID;
                try
                {
                    putawayTicketItemID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                }
                catch
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                PutawayTicketItem putawayTicketItem = (from pti in wmsEntities.PutawayTicketItem where pti.ID == putawayTicketItemID select pti).FirstOrDefault();
                if (putawayTicketItem == null)
                {
                    MessageBox.Show("错误 无法修改此条目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (putawayTicketItem.State != "待上架")
                    {
                        MessageBox.Show("改上架单状态为" + putawayTicketItem.State + "，无法删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == putawayTicketItem.ReceiptTicketItemID select rti).FirstOrDefault();
                    if (receiptTicketItem != null)
                    {
                        receiptTicketItem.HasPutwayAmount -= putawayTicketItem.ScheduledMoveCount;
                    }
                    new Thread(() =>
                    {
                        wmsEntities.SaveChanges();
                        PutawayTicket putawayTicket = putawayTicketItem.PutawayTicket;
                        int n = 0;
                        int m = 0;
                        foreach (PutawayTicketItem pti in putawayTicket.PutawayTicketItem)
                        {
                            ReceiptTicketItem rtii = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == pti.ReceiptTicketItemID select rti).FirstOrDefault();
                            if (rtii != null)
                            {
                                if (pti.ScheduledMoveCount == rtii.HasPutwayAmount)
                                {
                                    n++;
                                }
                                if (rtii.HasPutwayAmount == 0)
                                {
                                    m++;
                                }
                            }

                        }

                        if (m == putawayTicket.PutawayTicketItem.Count)
                        {
                            ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == putawayTicket.ReceiptTicketID select rt).FirstOrDefault();
                            if (receiptTicket != null)
                            {
                                receiptTicket.HasPutawayTicket = "没有生成上架单";
                            }
                        }
                        else
                        {
                            ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == putawayTicket.ReceiptTicketID select rt).FirstOrDefault();
                            if (receiptTicket != null)
                            {
                                receiptTicket.HasPutawayTicket = "部分生成上架单";
                            }
                            wmsEntities.SaveChanges();
                            wmsEntities.Database.ExecuteSqlCommand("DELETE FROM PutawayTicketItem WHERE ID = @putawayTicketItemID", new SqlParameter("putawayTicketItemID", putawayTicketItem.ID));
                        }
                        this.Invoke(new Action(() =>
                        {
                            this.pagerWidget.Search();
                        }));
                        MessageBox.Show("成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }).Start();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
        }

        private void modifyMode(int putawayTicketID)
        {
            WMSEntities wmsEntities = new WMSEntities();
            PutawayTicket putawayTicket = (from pt in wmsEntities.PutawayTicket where pt.ID == putawayTicketID select pt).FirstOrDefault();
            if (putawayTicket == null)
            {
                return;
            }
            int all = (from pti in wmsEntities.PutawayTicketItem where pti.PutawayTicketID == putawayTicketID select pti).ToArray().Length;
            int yes = (from pti in wmsEntities.PutawayTicketItem where pti.PutawayTicketID == putawayTicketID && pti.State == "已上架" select pti).ToArray().Length;
            int part = (from pti in wmsEntities.PutawayTicketItem where pti.PutawayTicketID == putawayTicketID && pti.State == "部分上架" select pti).ToArray().Length;
            int no = (from pti in wmsEntities.PutawayTicketItem where pti.PutawayTicketID == putawayTicketID && pti.State == "待上架" select pti).ToArray().Length;
            if (all == yes)
            {
                putawayTicket.State = "已上架";
            }
            else if (all == no)
            {
                putawayTicket.State = "待上架";
            }
            else
            {
                putawayTicket.State = "部分上架";
            }

            wmsEntities.SaveChanges();
        }

        private void buttonFInished_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlPutaway.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int putawayTicketItemID;
                try
                {
                    putawayTicketItemID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                }
                catch
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                PutawayTicketItem putawayTicketItem = (from pti in wmsEntities.PutawayTicketItem where pti.ID == putawayTicketItemID select pti).FirstOrDefault();
                decimal oldPutawayAmount = 0;
                if (putawayTicketItem.PutawayAmount == null)
                {
                    if (putawayTicketItem.PutawayAmount == null)
                    {
                        putawayTicketItem.PutawayAmount = 0;
                    }
                }

                oldPutawayAmount = (decimal)putawayTicketItem.PutawayAmount;
                if (putawayTicketItem == null)
                {
                    MessageBox.Show("此上架单条目不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (this.Controls.Find("textBoxOperateTime", true)[0].Text == "")
                    {
                        this.Controls.Find("textBoxOperateTime", true)[0].Text = DateTime.Now.ToString();
                    }
                    string errorInfo;
                    if (Utilities.CopyTextBoxTextsToProperties(this, putawayTicketItem, ReceiptMetaData.putawayTicketItemKeyName, out errorInfo) == false)
                    {
                        MessageBox.Show(errorInfo, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        if (Utilities.CopyComboBoxsToProperties(this, putawayTicketItem, ReceiptMetaData.putawayTicketItemKeyName) == false)
                        {
                            MessageBox.Show("下拉框获取失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (putawayTicketItem.PutawayAmount == null)
                        {
                            MessageBox.Show("实际上架数量不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == putawayTicketItem.ReceiptTicketItemID select rti).FirstOrDefault();
                        if (putawayTicketItem.UnitAmount == null)
                        {
                            if (receiptTicketItem != null)
                            {
                                putawayTicketItem.UnitAmount = receiptTicketItem.UnitAmount;
                            }

                        }
                        if (putawayTicketItem.PutawayAmount > putawayTicketItem.ScheduledMoveCount)
                        {
                            MessageBox.Show("实际上架数量不能大于计划上架数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }


                        //putawayTicketItem.PutawayAmount = putawayTicketItem.UnitAmount * putawayTicketItem.MoveCount;
                        putawayTicketItem.MoveCount = putawayTicketItem.PutawayAmount / putawayTicketItem.UnitAmount;
                        StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == putawayTicketItem.ReceiptTicketItemID select si).FirstOrDefault();
                        if (stockInfo != null)
                        {
                            stockInfo.OverflowAreaAmount -= putawayTicketItem.PutawayAmount - oldPutawayAmount;
                            stockInfo.ShipmentAreaAmount += putawayTicketItem.PutawayAmount - oldPutawayAmount;
                        }


                        if (putawayTicketItem.ScheduledMoveCount == putawayTicketItem.PutawayAmount)
                        {
                            putawayTicketItem.State = "已上架";
                        }
                        else if (putawayTicketItem.PutawayAmount == 0)
                        {
                            putawayTicketItem.State = "待上架";
                        }
                        else
                        {
                            putawayTicketItem.State = "部分上架";
                        }

                        wmsEntities.SaveChanges();
                        this.modifyMode(putawayTicketItem.PutawayTicketID);
                        MessageBox.Show("上架成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Search();
                        if (CallBack != null)
                        {
                            CallBack();
                        }


                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            int putawayTicketID;
            if (int.TryParse(this.value, out putawayTicketID) == false)
            {
                MessageBox.Show("system error!", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (this.putawayTicketID == -1)
            {
                return;
            }
            if (MessageBox.Show("是否将此上架单中所有上架单条目上架？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            PutawayTicket putawayTicket = (from pt in wmsEntities.PutawayTicket where pt.ID == putawayTicketID select pt).FirstOrDefault();
            if (putawayTicket == null)
            {
                MessageBox.Show("该上架单已被删除，请刷新后查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            PutawayTicketItem[] putawayTicketItems = putawayTicket.PutawayTicketItem.ToArray();
            int n = 0;
            foreach(PutawayTicketItem pti in putawayTicketItems)
            {
                if (pti.State == "已上架")
                {
                    n++;
                }
            }
            if (n == putawayTicketItems.Length)
            {
                MessageBox.Show("该上架单已全部上架！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            foreach (PutawayTicketItem pti in putawayTicketItems)
            {
                decimal oldPutawayAmount = (pti.PutawayAmount == null ? 0 : (decimal)pti.PutawayAmount);
                pti.PutawayAmount = pti.ScheduledMoveCount;
                //pti.PutawayAmount = pti.UnitAmount * pti.MoveCount;
                pti.UnitCount = pti.PutawayAmount / pti.UnitAmount;
                pti.OperateTime = DateTime.Now.ToString();
                pti.State = "已上架";
                //ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == pti.ReceiptTicketItemID select rti).FirstOrDefault();

                StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == pti.ReceiptTicketItemID select si).FirstOrDefault();

                if (stockInfo != null)
                {
                    stockInfo.OverflowAreaAmount -= pti.PutawayAmount - oldPutawayAmount;
                    stockInfo.ShipmentAreaAmount += pti.PutawayAmount - oldPutawayAmount;
                }
            }
            putawayTicket.State = "已上架";

            wmsEntities.SaveChanges();
            MessageBox.Show("上架成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Search();
            CallBack();


            /*
            var worksheet = this.reoGridControlPutaway.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int putawayTicketItemID;
                try
                {
                    putawayTicketItemID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                }
                catch
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                PutawayTicketItem putawayTicketItem = (from pti in wmsEntities.PutawayTicketItem where pti.ID == putawayTicketItemID select pti).FirstOrDefault();
                new Thread(() =>
                {
                    if (putawayTicketItem == null)
                    {
                        MessageBox.Show("找不到该上架单，可能已被删除");
                        return;
                    }
                    else
                    {
                        putawayTicketItem.State = "待上架";
                        wmsEntities.SaveChanges();
                        int count = wmsEntities.Database.SqlQuery<int>(
                            "SELECT COUNT(*) FROM PutawayTicketItem " +
                        "WHERE PutawayTicketID = @putawayTicketID AND State <> '待上架'",
                        new SqlParameter("putawayTicketID", putawayTicketItem.PutawayTicketID)).FirstOrDefault();

                        if (count == 0)
                        {
                            wmsEntities.Database.ExecuteSqlCommand(
                                "UPDATE PutawayTicket SET State = '待上架' " +
                                "WHERE ID = @putawayTicketID",
                                new SqlParameter("putawayTicketID", putawayTicketItem.PutawayTicketID));
                        }
                        else
                        {
                            wmsEntities.Database.ExecuteSqlCommand(
                                 "UPDATE PutawayTicket SET State = '部分上架' " +
                                 "WHERE ID = @putawayTicketID",
                                 new SqlParameter("putawayTicketID", putawayTicketItem.PutawayTicketID));
                        }
                    }
                    StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == putawayTicketItem.ReceiptTicketItemID select si).FirstOrDefault();
                    if (stockInfo != null)
                    {
                        if (stockInfo.OverflowAreaAmount != null)
                        {
                            int amount = (int)stockInfo.OverflowAreaAmount;
                            if (stockInfo.ReceiptAreaAmount != null)
                            {
                                stockInfo.ReceiptAreaAmount += amount;
                            }
                            else
                            {
                                stockInfo.ReceiptAreaAmount = amount;
                            }
                            stockInfo.OverflowAreaAmount = 0;
                        }
                    }
                    wmsEntities.SaveChanges();
                    //MessageBox.Show("成功");
                    this.Invoke(new Action(() =>
                    {
                        this.pagerWidget.Search();
                        CallBack();
                    }));
                }).Start();
            }

            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }*/
        }

        private void buttonDelete_MouseEnter(object sender, EventArgs e)
        {
            //buttonDelete.BackgroundImage = WMS.UI.Properties.Resources.bottonW_s;
        }

        private void buttonDelete_MouseLeave(object sender, EventArgs e)
        {
            //buttonDelete.BackgroundImage = WMS.UI.Properties.Resources.bottonW_q;
        }

        private void buttonDelete_MouseDown(object sender, MouseEventArgs e)
        {
            //buttonDelete.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
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


        private void buttonFInished_MouseEnter(object sender, EventArgs e)
        {
            buttonFInished.BackgroundImage = WMS.UI.Properties.Resources.bottonB1_s;
        }

        private void buttonFinish_MouseLeave(object sender, EventArgs e)
        {
            buttonFInished.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        }

        private void buttonFInished_MouseDown(object sender, MouseEventArgs e)
        {
            buttonFInished.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private void buttonCancel_MouseEnter(object sender, EventArgs e)
        {
            buttonAll.BackgroundImage = WMS.UI.Properties.Resources.bottonB1_s;
        }

        private void buttonCancel_MouseLeave(object sender, EventArgs e)
        {
            buttonAll.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        }

        private void buttonCancel_MouseDown(object sender, MouseEventArgs e)
        {
            buttonAll.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanelProperties_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using unvell.ReoGrid;
using unvell.ReoGrid.Events;
using System.Threading;
using WMS.DataAccess;
using System.Data.SqlClient;
using unvell.ReoGrid.CellTypes;

namespace WMS.UI.FormReceipt
{
    public partial class FormReceiptSubmissionNew : Form
    {
        private int receiptTicketID;
        private FormMode formMode;
        private int submissionTicketID;
        private int checkBoxColumn = 1;
        private Action CallBack = null;
        private int countRow;
        private int userID;
        private Func<int> PersonIDGetter = null;
        private Func<int> SubmissionPersonIDGetter = null;
        private Func<int> ReceivePersonIDGetter = null;
        private Func<int> DeliverSubmissionPersonIDGetter = null;
        private KeyName[] ItemKeyName;
        public FormReceiptSubmissionNew()
        {
            InitializeComponent();
        }

        /// <summary>
        /// formMode == ADD receiptTicketID ; formMode = ALTER submissionTicketID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="formMode"></param>

        public FormReceiptSubmissionNew(int ID, int userID, FormMode formMode)
        {
            InitializeComponent();
            this.formMode = formMode;
            this.userID = userID;
            if (formMode == FormMode.ADD)
            {
                this.receiptTicketID = ID;
            }
            else
            {
                this.submissionTicketID = ID;
            }
        }

        private void FormReceiptSubmissionNew_Load(object sender, EventArgs e)
        {
            this.ItemKeyName = (from kn in ReceiptMetaData.itemsKeyName where kn.Key != "Component" select kn).ToArray();
            InitComponents();
            Search();
            InitPanel();
            WMSEntities wmsEntities = new WMSEntities();
            User user = (from u in wmsEntities.User where u.ID == userID select u).FirstOrDefault();
            ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == this.receiptTicketID select rt).FirstOrDefault();
            if (user != null)
            {
                this.Controls.Find("textBoxCreateUserUsername", true)[0].Text = user.Username;
                this.Controls.Find("textBoxLastUpdateUserUsername", true)[0].Text = user.Username;
            }
            if (receiptTicket != null)
            {
                this.Controls.Find("textBoxReceiptTicketNo", true)[0].Text = receiptTicket.No;
                this.Controls.Find("textBoxSubmissionDate", true)[0].Text = receiptTicket.ReceiptDate.ToString();
            }
            this.Controls.Find("textBoxState", true)[0].Text = "待检";

            this.Controls.Find("textBoxCreateTime", true)[0].Text = DateTime.Now.ToString();
            this.Controls.Find("textBoxLastUpdateTime", true)[0].Text = DateTime.Now.ToString();
        }

        private void InitComponents()
        {
            //初始化
            //string[] columnNames = (from kn in ReceiptMetaData.itemsKeyName select kn.Name).ToArray();
            //初始化表格
            var worksheet = this.reoGridControlUser.Worksheets[0];
            //worksheet.SelectionMode = WorksheetSelectionMode.Cell;
            int n = 0;
            for (int i = 0; i < ItemKeyName.Length + 1; i++)
            {
                if (i == this.checkBoxColumn)
                {
                    worksheet.ColumnHeaders[i].Text = "送检数量";
                }
                else
                {
                    worksheet.ColumnHeaders[i].Text = ItemKeyName[n].Name;
                    worksheet.ColumnHeaders[i].IsVisible = ItemKeyName[n].Visible;
                    n++;
                }
            }
            //worksheet.ColumnHeaders[columnNames.Length].Text = "是否送检";
            worksheet.Columns = ItemKeyName.Length + 1;
            worksheet.CellMouseEnter += ClickOnCell;
            //worksheet.AfterCellEdit += AfterCellEdit;
        }
        /*
        private void AfterCellEdit(object sender, CellAfterEditEventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            int row = e.Cell.Position.Row;
            int id = int.Parse(this.reoGridControlUser.Worksheets[0][row, 0].ToString());
            ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == id select rti).FirstOrDefault();
            if (receiptTicketItem != null)
            {
                int count;
                if (int.TryParse(this.reoGridControlUser.Worksheets[0][row, this.checkBoxColumn].ToString(), out count) == false)
                {
                    MessageBox.Show("请输入数字！");
                    return;
                }
                if (receiptTicketItem.ReceiviptAmount != null)
                {
                    if (count > receiptTicketItem.ReceiviptAmount)
                    {
                        MessageBox.Show("送检数量不应大于收货数量");
                        this.reoGridControlUser.Worksheets[0][e.Cell.Position] = "";
                    }
                }
            }
        }*/

        private void ClickOnCell(object sender, CellMouseEventArgs e)
        {
            var worksheet = this.reoGridControlUser.Worksheets[0];
            var position = e.CellPosition;
            if ((position.Col != this.checkBoxColumn && position.Row < countRow) || position.Row >= countRow)
            {
                worksheet.CreateAndGetCell(position).IsReadOnly = true;
            }
            else
            {

            }
        }


        private void InitPanel()
        {
            Utilities.CreateEditPanel(this.tableLayoutPanel2, ReceiptMetaData.submissionTicketKeyName);
            this.DeliverSubmissionPersonIDGetter = Utilities.BindTextBoxSelect<FormSelectPerson, Person>(this, "textBoxDeliverSubmissionPersonName", "Name");
            this.SubmissionPersonIDGetter = Utilities.BindTextBoxSelect<FormSelectPerson, Person>(this, "textBoxSubmissionPersonName", "Name");
            this.ReceivePersonIDGetter = Utilities.BindTextBoxSelect<FormSelectPerson, Person>(this, "textBoxReceivePersonName", "Name");
            this.PersonIDGetter = Utilities.BindTextBoxSelect<FormSelectPerson, Person>(this, "textBoxPersonName", "Name");
            if (this.formMode == FormMode.ADD)
            {

            }
            else
            {
                WMSEntities wmsEntities = new WMSEntities();
                SubmissionTicketView submissionTicketView = (from stv in wmsEntities.SubmissionTicketView where stv.ID == this.submissionTicketID select stv).FirstOrDefault();
                if (submissionTicketView == null)
                {
                    MessageBox.Show("找不到此送检单");
                    return;
                }
                Utilities.CopyPropertiesToTextBoxes(submissionTicketView, this);
                Utilities.CopyPropertiesToComboBoxes(submissionTicketView, this);
            }
        }

        private void Search()
        {
            //this.labelStatus.Text = "搜索中...";

            new Thread(new ThreadStart(() =>
            {
                var wmsEntities = new WMSEntities();
                ReceiptTicketItemView[] receiptTicketItemViews = null;
                receiptTicketItemViews = wmsEntities.Database.SqlQuery<ReceiptTicketItemView>("SELECT * FROM ReceiptTicketItemView WHERE ReceiptTicketID = @receiptTicketID", new SqlParameter("receiptTicketID", this.receiptTicketID)).ToArray();
                this.countRow = receiptTicketItemViews.Length;
                this.reoGridControlUser.Invoke(new Action(() =>
                {
                    //this.labelStatus.Text = "搜索完成";
                    //this.reoGridControlUser.Readonly = false;
                    var worksheet = this.reoGridControlUser.Worksheets[0];
                    worksheet.DeleteRangeData(RangePosition.EntireRange);
                    //int n = 0;
                    for (int i = 0; i < receiptTicketItemViews.Length; i++)
                    {
                        ReceiptTicketItemView curReceiptTicketItemView = receiptTicketItemViews[i];
                        object[] columns = Utilities.GetValuesByPropertieNames(curReceiptTicketItemView, (from kn in ItemKeyName select kn.Key).ToArray());

                        int m = 0;
                        for (int j = 0; j < worksheet.Columns - 1; j++)
                        {
                            if (j == this.checkBoxColumn)
                            {
                                //CheckBoxCell checkboxCell;
                                //TextBox textbox = new TextBox();
                                //DataGridViewTextBoxCell;
                                //worksheet[i, m]
                                //Cell cell = 
                                worksheet.CreateAndGetCell(i, m).Style.BackColor = Color.AliceBlue;
                                worksheet[i, j] = curReceiptTicketItemView.DefaultSubmissionAmount;
                            }
                            else
                            {
                                //worksheet.CreateAndGetCell(i, m).IsReadOnly = true;
                                worksheet[i, j] = columns[m];
                                m++;
                            }
                        }
                    }
                }));

            })).Start();
        }


        public void SetCallBack(Action action)
        {
            this.CallBack = action;
        }

        private SortedDictionary<int, decimal> SelectReceiptTicketItem()
        {
            /*
            //List<ReceiptTicketItem> receiptTicketItems = new List<ReceiptTicketItem>();
            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControlUser.Worksheets[0];
            List<int> ids = new List<int>();
            bool result;
            for (int i = 0; i < this.countRow; i++)
            {
                result = (worksheet[i, this.checkBoxColumn] as bool?) ?? false;
                if (result == true)
                {
                    int id;
                    if (int.TryParse(worksheet[i, 0].ToString(), out id) == false)
                    {
                        MessageBox.Show(worksheet[i, 0].ToString() + "加入失败");
                    }
                    else
                    {
                        ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == id select rti).FirstOrDefault();
                        if (receiptTicketItem.State == "已收货" || receiptTicketItem.State == "送检中")
                        {
                            MessageBox.Show(receiptTicketItem.ID + " " + receiptTicketItem.State);
                            continue;
                        }
                        else
                        {
                            ids.Add(receiptTicketItem.ID);
                        }
                    }
                }
            }*/
            List<int> ids = new List<int>();
            SortedDictionary<int, decimal> idsAndSubmissionAmount = new SortedDictionary<int, decimal>();
            var worksheet = this.reoGridControlUser.Worksheets[0];
            decimal submissionAmount;
            for (int i = 0; i < this.countRow; i++)
            {
                int id;
                string strSubmissionAmount = worksheet[i, this.checkBoxColumn] == null ? null : worksheet[i, this.checkBoxColumn].ToString();


                if (strSubmissionAmount == null)
                {
                    MessageBox.Show("送检数量不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                else
                {

                    if (decimal.TryParse(strSubmissionAmount, out submissionAmount) && int.TryParse(worksheet[i, 0].ToString(), out id))
                    {

                        idsAndSubmissionAmount.Add(id, submissionAmount);
                    }
                    else
                    {
                        MessageBox.Show("送检数量不能为空且必须为数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return null;
                    }
                }
                /*
                if (result == true)
                {
                    int id;
                    if (int.TryParse(worksheet[i, 0].ToString(), out id) == false)
                    {
                        MessageBox.Show(worksheet[i, 0].ToString() + "加入失败");
                    }
                    else
                    {
                        ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == id select rti).FirstOrDefault();
                        if (receiptTicketItem.State == "已收货" || receiptTicketItem.State == "送检中")
                        {
                            MessageBox.Show(receiptTicketItem.ID + " " + receiptTicketItem.State);
                            continue;
                        }
                        else
                        {
                            ids.Add(receiptTicketItem.ID);
                        }
                    }*/
            }

            return idsAndSubmissionAmount;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlUser.Worksheets[0];
            worksheet.FocusPos = new CellPosition(0, 0);
            WMSEntities wmsEntities = new WMSEntities();
            ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == this.receiptTicketID select rt).FirstOrDefault();
            if (receiptTicket == null)
            {
                MessageBox.Show("该收货单已被删除，送检失败");
                return;
            }
            else
            {
                if (receiptTicket.State != "待收货")
                {
                    MessageBox.Show("该收货单状态为" + receiptTicket.State + "，无法送检！");
                }
            }
            SortedDictionary<int, decimal> idsAndSubmissionAmount = SelectReceiptTicketItem();
            if (idsAndSubmissionAmount == null)
            {
                return;
            }
            Dictionary<ReceiptTicketItem, decimal> receiptTicketItemsAndSubmissionAmount = new Dictionary<ReceiptTicketItem, decimal>();
            foreach (KeyValuePair<int, decimal> kv in idsAndSubmissionAmount)
            {
                ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == kv.Key select rti).FirstOrDefault();
                if (receiptTicketItem != null)
                {
                    if (receiptTicketItem.ReceiviptAmount < kv.Value)
                    {
                        MessageBox.Show("送检数量不应大于收货数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (kv.Value < 0)
                    {
                        MessageBox.Show("送检数量不应为负数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    receiptTicketItemsAndSubmissionAmount.Add(receiptTicketItem, kv.Value);
                }
            }
            if (this.formMode == FormMode.ADD)
            {
                SubmissionTicket submissionTicket = new SubmissionTicket();
                string errorInfo;
                if (Utilities.CopyTextBoxTextsToProperties(this, submissionTicket, ReceiptMetaData.submissionTicketKeyName, out errorInfo) == false)
                {
                    MessageBox.Show(errorInfo);
                    return;
                }
                else
                {
                    Utilities.CopyComboBoxsToProperties(this, submissionTicket, ReceiptMetaData.submissionTicketKeyName);
                    submissionTicket.PersonID = this.PersonIDGetter();
                    submissionTicket.ReceivePersonID = this.ReceivePersonIDGetter();
                    submissionTicket.SubmissionPersonID = this.SubmissionPersonIDGetter();
                    submissionTicket.DeliverSubmissionPersonID = this.DeliverSubmissionPersonIDGetter();
                    //ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == this.receiptTicketID select rt).FirstOrDefault();

                    submissionTicket.SubmissionDate = receiptTicket.ReceiptDate;
                    submissionTicket.CreateTime = DateTime.Now;
                    submissionTicket.ReceiptTicketID = this.receiptTicketID;
                    submissionTicket.CreateUserID = this.userID;
                    submissionTicket.LastUpdateTime = DateTime.Now;
                    submissionTicket.LastUpdateUserID = this.userID;
                    submissionTicket.ProjectID = receiptTicket.ProjectID;
                    submissionTicket.WarehouseID = receiptTicket.WarehouseID;
                    submissionTicket.State = "待检";
                    //////////////////////////// Begin
                    if (string.IsNullOrWhiteSpace(submissionTicket.No))
                    {
                        if (submissionTicket.CreateTime.HasValue == false)
                        {
                            MessageBox.Show("单号生成失败（未知创建日期）！请手动填写单号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        DateTime createDay = new DateTime(submissionTicket.CreateTime.Value.Year, submissionTicket.CreateTime.Value.Month, submissionTicket.CreateTime.Value.Day);
                        DateTime nextDay = createDay.AddDays(1);
                        int maxRankOfToday = Utilities.GetMaxTicketRankOfDay((from s in wmsEntities.SubmissionTicket
                                                                              where s.CreateTime >= createDay && s.CreateTime < nextDay
                                                                              select s.No).ToArray());
                        if (maxRankOfToday == -1)
                        {
                            MessageBox.Show("单号生成失败！请手动填写单号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        submissionTicket.No = Utilities.GenerateTicketNo("J", submissionTicket.CreateTime.Value, maxRankOfToday + 1);
                    }
                    /////////////////////////////////////////////////////////////// End
                    wmsEntities.SubmissionTicket.Add(submissionTicket);


                    try
                    {
                        wmsEntities.SaveChanges();
                        //submissionTicket.No = Utilities.GenerateTicketNo()

                        wmsEntities.SaveChanges();
                        foreach (KeyValuePair<ReceiptTicketItem, decimal> vp in receiptTicketItemsAndSubmissionAmount)
                        {
                            SubmissionTicketItem submissionTicketItem = new SubmissionTicketItem();
                            StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == vp.Key.ID select si).FirstOrDefault();
                            if (stockInfo == null)
                            {
                                MessageBox.Show("找不到对应的库存信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {

                                if (stockInfo.ReceiptAreaAmount != null)
                                {
                                    submissionTicketItem.ArriveAmount = stockInfo.ReceiptAreaAmount;
                                }
                                stockInfo.SubmissionAmount = vp.Value;
                                stockInfo.ReceiptAreaAmount -= vp.Value;
                                submissionTicketItem.ArriveAmount = vp.Key.ReceiviptAmount;
                                submissionTicketItem.ReceiptTicketItemID = vp.Key.ID;

                                submissionTicketItem.State = "待检";
                                vp.Key.State = "送检中";
                                submissionTicketItem.SubmissionAmount = vp.Value;
                                submissionTicketItem.SubmissionTicketID = submissionTicket.ID;
                                wmsEntities.SubmissionTicketItem.Add(submissionTicketItem);

                            }
                        }
                        receiptTicket.HasSubmission = 1;
                        receiptTicket.State = "送检中";
                        wmsEntities.SaveChanges();
                        /*
                        int count = wmsEntities.Database.SqlQuery<int>(
                            "SELECT COUNT(*) FROM ReceiptTicketItem " +
                            "WHERE ReceiptTicketID = @receiptTicketID AND State <> '送检中'",
                            new SqlParameter("receiptTicketID", receiptTicketID)).FirstOrDefault();
                        if (count == 0)
                        {
                            wmsEntities.Database.ExecuteSqlCommand(
                                "UPDATE ReceiptTicket SET State='送检中' " +
                                "WHERE ID = @receiptTicketID",
                                new SqlParameter("receiptTicketID", receiptTicketID));
                        }
                        else
                        {
                            wmsEntities.Database.ExecuteSqlCommand(
                                "UPDATE ReceiptTicket SET State='部分送检中' " +
                                "WHERE ID = @receiptTicketID",
                                new SqlParameter("receiptTicketID", receiptTicketID));
                        }
                        */

                        this.Search();
                        CallBack();
                        this.Hide();

                        MessageBox.Show("收货单条目送检成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        return;
                    }

                }
            }
            else
            {

            }
        }


        private void OK_MouseEnter(object sender, EventArgs e)
        {
            buttonOK.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        }

        private void OK_MouseLeave(object sender, EventArgs e)
        {
            buttonOK.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_q;
        }

        private void OK_MouseDown(object sender, MouseEventArgs e)
        {
            buttonOK.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private void buttonOK_MouseMove(object sender, MouseEventArgs e)
        {
            this.buttonOK.Focus();
            this.reoGridControlUser.Worksheets[0].FocusPos = new CellPosition(0, 0);
            var worksheet = this.reoGridControlUser.Worksheets[0];
            worksheet.EndEdit(new EndEditReason());
            //worksheet.CreateAndGetCell(worksheet.GetEditingCell()).EndEdit();
            /*
            for (int i = 0; i < this.countRow; i++)
            {
                worksheet.Cells[i, this.checkBoxColumn].EndEdit();
            }
            */
            //worksheet.EditingCell.EndEdit();
        }
    }
}

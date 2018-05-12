using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using unvell.ReoGrid;
using System.Threading;
using WMS.DataAccess;
using unvell.ReoGrid.CellTypes;
using System.Data.SqlClient;

namespace WMS.UI.FormReceipt
{
    public partial class FormAddSubmissionItem : Form
    {
        private int countRow;
        private int checkColumn;
        private int receiptTicketID;
        private int submissionTicketID;
        private Action CallBack = null;
        public FormAddSubmissionItem()
        {
            InitializeComponent();
        }

        public FormAddSubmissionItem(int receiptTicketID, int submissionTicketID)
        {
            InitializeComponent();
            this.receiptTicketID = receiptTicketID;
            this.submissionTicketID = submissionTicketID;
        }

        public void SetCallBack(Action action)
        {
            this.CallBack = action;
        }

        private void FormAddSubmissionItem_Load(object sender, EventArgs e)
        {
            InitComponents();
            Search();
        }

        private void InitComponents()
        {
            //初始化
            string[] columnNames = (from kn in ReceiptMetaData.itemsKeyName select kn.Name).ToArray();
            //初始化表格
            var worksheet = this.reoGridControlUser.Worksheets[0];
            worksheet.SelectionMode = WorksheetSelectionMode.Row;
            for (int i = 0; i < columnNames.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = columnNames[i];
                worksheet.ColumnHeaders[i].IsVisible = ReceiptMetaData.itemsKeyName[i].Visible;
            }
            worksheet.ColumnHeaders[columnNames.Length].Text = "是否送检";
            worksheet.Columns = columnNames.Length + 1;
        }

        private void Search()
        {
            //this.labelStatus.Text = "搜索中...";

            new Thread(new ThreadStart(() =>
            {
                var wmsEntities = new WMSEntities();
                ReceiptTicketItemView[] receiptTicketItemViews = null;
                receiptTicketItemViews = wmsEntities.Database.SqlQuery<ReceiptTicketItemView>(String.Format("SELECT * FROM ReceiptTicketItemView WHERE ReceiptTicketID = {0}", this.receiptTicketID)).ToArray();
                this.countRow = receiptTicketItemViews.Length;
                this.reoGridControlUser.Invoke(new Action(() =>
                {
                    //this.labelStatus.Text = "搜索完成";
                    var worksheet = this.reoGridControlUser.Worksheets[0];
                    worksheet.DeleteRangeData(RangePosition.EntireRange);
                    int n = 0;
                    for (int i = 0; i < receiptTicketItemViews.Length; i++)
                    {
                        ReceiptTicketItemView curReceiptTicketItemView = receiptTicketItemViews[i];
                        object[] columns = Utilities.GetValuesByPropertieNames(curReceiptTicketItemView, (from kn in ReceiptMetaData.itemsKeyName select kn.Key).ToArray());
                        if (curReceiptTicketItemView.State == "作废")
                        {
                            continue;
                        }
                        for (int j = 0; j < worksheet.Columns - 1; j++)
                        {
                            worksheet[n, j] = columns[j];
                        }
                        CheckBoxCell checkboxCell;
                        worksheet[n, worksheet.Columns - 1] = new object[] { checkboxCell = new CheckBoxCell() };
                        this.checkColumn = worksheet.Columns - 1;
                        n++;
                    }
                }));

            })).Start();

        }

        private void buttonSubmission_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControlUser.Worksheets[0];
            List<ReceiptTicketItem> ids = new List<ReceiptTicketItem>();
            bool result;
            for (int i = 0; i < this.countRow; i++)
            {
                result = (worksheet[i, this.checkColumn] as bool?) ?? false;
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
                            return;
                        }
                        else
                        {
                            ids.Add(receiptTicketItem);
                        }
                    }
                }
            }
            if (ids.Count == 0)
            {
                MessageBox.Show("请选择一项送检");
                return;
            }
            else
            {
                foreach (ReceiptTicketItem i in ids)
                {
                    //PutawayTicketItem putawayTicketItem = (from pti in wmsEntities.PutawayTicketItem where pti.ID == i && pti.State != "已收货" && pti.State != "送检中" select pti).FirstOrDefault();
                    if (i == null)
                    {
                        MessageBox.Show(i + "不能添加此条目送检");
                    }
                    else
                    {
                        SubmissionTicketItem submissionTicketItem = ReceiptUtilities.ReceiptTicketItemToSubmissionTicketItem(i, this.submissionTicketID);
                        wmsEntities.SubmissionTicketItem.Add(submissionTicketItem);
                        i.State = "送检中";
                    }
                }
                new Thread(() =>
                {
                    try
                    {
                        wmsEntities.SaveChanges();
                        int count = wmsEntities.Database.SqlQuery<int>("SELECT COUNT(*) FROM ReceiptTicketItem WHERE State <> '送检中' AND ReceiptTicketID = @receiptTicketID", new SqlParameter("receiptTicketID", this.receiptTicketID)).FirstOrDefault();
                        if (count == 0)
                        {
                            wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State = '送检中' WHERE ID = @receiptTicketID", new SqlParameter("receiptTicketID", this.receiptTicketID));
                        }
                        wmsEntities.SaveChanges();
                    }
                    catch
                    {
                        MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        return;
                    }
                    this.Invoke(new Action(() =>
                    {
                        this.Search();
                        CallBack();
                    }));
                    MessageBox.Show("添加成功");
                }).Start();
                
            }
        }
    }
}

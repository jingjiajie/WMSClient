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
    public partial class FormReceiptItem : Form
    {
        private int receiptTicketID;
        private int countRow;
        private int checkColumn;
        private Action CallBack = null;
        public FormReceiptItem()
        {
            InitializeComponent();
        }

        public FormReceiptItem(int receiptTicketID)
        {
            InitializeComponent();
            this.receiptTicketID = receiptTicketID;
        }

        public void SetCallBack(Action action)
        {
            this.CallBack = action;
        }

        private void InitComponents()
        {
            //初始化
            string[] columnNames = (from kn in ReceiptMetaData.itemsKeyName where kn.Visible == true select kn.Name).ToArray();
            //初始化表格
            var worksheet = this.reoGridControlUser.Worksheets[0];
            worksheet.SelectionMode = WorksheetSelectionMode.Row;
            for (int i = 0; i < ReceiptMetaData.itemsKeyName.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = ReceiptMetaData.itemsKeyName[i].Name;
                worksheet.ColumnHeaders[i].IsVisible = ReceiptMetaData.itemsKeyName[i].Visible;
            }
            worksheet.ColumnHeaders[ReceiptMetaData.itemsKeyName.Length].Text = "是否收货";
            worksheet.Columns = ReceiptMetaData.itemsKeyName.Length + 1;
        }

        private void FormReceiptItem_Load(object sender, EventArgs e)
        {
            InitComponents();
            this.Search();
        }

        private void Search()
        {
            //this.labelStatus.Text = "搜索中...";

            new Thread(new ThreadStart(() =>
            {
                var wmsEntities = new WMSEntities();
                ReceiptTicketItemView[] receiptTicketItemViews = null;
                try
                {
                    receiptTicketItemViews = wmsEntities.Database.SqlQuery<ReceiptTicketItemView>(
                        "SELECT * FROM ReceiptTicketItemView " +
                        "WHERE ReceiptTicketID = @receiptTicketID",
                        new SqlParameter("receiptTicketID", this.receiptTicketID)).ToArray();
                }
                catch
                {
                    MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    return;
                }
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

        private void ButtonReceipt_Click(object sender, EventArgs e)
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
                        if (receiptTicketItem != null)
                        {
                            ids.Add(receiptTicketItem);
                        }
                        else
                        {
                            MessageBox.Show("收货单条目被删除或不存在");
                        }
                    }
                }
            }
            if (ids.Count == 0)
            {
                MessageBox.Show("请选择一项收货");
                return;
            }
            else
            {
                foreach (ReceiptTicketItem i in ids)
                {
                    //PutawayTicketItem putawayTicketItem = (from pti in wmsEntities.PutawayTicketItem where pti.ID == i && pti.State != "已收货" && pti.State != "送检中" select pti).FirstOrDefault();
                    if (i == null)
                    {
                        MessageBox.Show(i + "不能添加此条目收货");
                    }
                    else
                    {
                        i.State = "已收货";
                    }
                }
                new Thread(() =>
                {
                    try
                    {
                        wmsEntities.SaveChanges();
                        int count1 = wmsEntities.Database.SqlQuery<int>("SELECT COUNT(*) FROM ReceiptTicketItem " +
                            "WHERE ReceiptTicketID = @receiptTicketID AND State <> '已收货'",
                            new SqlParameter("receiptTicketID", this.receiptTicketID)).FirstOrDefault();
                        if (count1 == 0)
                        {
                            wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State = '已收货' " +
                                "WHERE ID = @receiptTicketID",
                                new SqlParameter("receiptTicketID", this.receiptTicketID));
                        }
                        else
                        {
                            wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State = '部分收货' " +
                                "WHERE ID = @receiptTicketID",
                                new SqlParameter("receiptTicketID", this.receiptTicketID));
                        }
                        this.Invoke(new Action(() =>
                        {
                            this.Search();
                        }));
                        MessageBox.Show("添加成功");
                    }
                    catch
                    {
                        MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        return;
                    }
                    CallBack();
                }).Start();
            }
        }

        private void buttonNoReceipt_Click(object sender, EventArgs e)
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
                        if (receiptTicketItem != null)
                        {
                            ids.Add(receiptTicketItem);
                        }
                        else
                        {
                            MessageBox.Show("收货单条目被删除或不存在");
                        }
                    }
                }
            }
            if (ids.Count == 0)
            {
                MessageBox.Show("请选择一项拒收");
                return;
            }
            else
            {
                foreach (ReceiptTicketItem i in ids)
                {
                    //PutawayTicketItem putawayTicketItem = (from pti in wmsEntities.PutawayTicketItem where pti.ID == i && pti.State != "已收货" && pti.State != "送检中" select pti).FirstOrDefault();
                    if (i == null)
                    {
                        MessageBox.Show(i + "不能添加此条目收货");
                    }
                    else
                    {
                        i.State = "拒收";
                    }
                }
                new Thread(() =>
                {
                    try
                    {
                        wmsEntities.SaveChanges();
                        int count1 = wmsEntities.Database.SqlQuery<int>("SELECT COUNT(*) FROM ReceiptTicketItem " +
                            "WHERE ReceiptTicketID = @receiptTicketID AND State = '已收货'",
                            new SqlParameter("receiptTicketID", this.receiptTicketID)).FirstOrDefault();
                        int count2 = wmsEntities.Database.SqlQuery<int>("SELECT COUNT(*) FROM ReceiptTicketItem " +
                            "WHERE ReceiptTicketID = @receiptTicketID AND State <> '拒收'",
                            new SqlParameter("receiptTicketID", this.receiptTicketID)).FirstOrDefault();
                        if (count1 == 0)
                        {
                            if (count2 == 0)
                            {
                                wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State = '拒收' " +
                                "WHERE ID = @receiptTicketID",
                                new SqlParameter("receiptTicketID", this.receiptTicketID));
                            }
                            else
                            {
                                wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State = '部分拒收' " +
                                "WHERE ID = @receiptTicketID",
                                new SqlParameter("receiptTicketID", this.receiptTicketID));
                            }
                        }
                        else
                        {
                            wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State = '部分收货' " +
                                "WHERE ID = @receiptTicketID",
                                new SqlParameter("receiptTicketID", this.receiptTicketID));
                        }
                        this.Invoke(new Action(() =>
                        {
                            this.Search();
                        }));
                        MessageBox.Show("添加成功");
                    }
                    catch
                    {
                        MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        return;
                    }
                    CallBack();
                }).Start();
            }
        }
    }
}

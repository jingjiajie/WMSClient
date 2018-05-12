using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using unvell.ReoGrid;
using unvell.ReoGrid.CellTypes;
using System.Windows.Forms;
using System.Threading;
using WMS.DataAccess;
using System.Data.SqlClient;

namespace WMS.UI.FormReceipt
{
    public partial class FormAddPutawayItem : Form
    {
        private int putawayTicketID;
        private int receiptTicketID;
        private int warehouseID;
        private int projectID;
        private int checkColumn;
        private int countRow;
        private WMSEntities wmsEntities = new WMSEntities();
        public FormAddPutawayItem()
        {
            InitializeComponent();
        }
        public FormAddPutawayItem(int putawayTicketID, int receiptTicketID, int warehouseID, int projectID)
        {
            InitializeComponent();
            this.putawayTicketID = putawayTicketID;
            this.receiptTicketID = receiptTicketID;
            this.warehouseID = warehouseID;
            this.projectID = projectID;
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
                        for (int j = 0; j < worksheet.Columns-1; j++)
                        {
                            worksheet[n, j] = columns[j];
                        }
                        CheckBoxCell checkboxCell;
                        worksheet[n, worksheet.Columns-1] = new object[] { checkboxCell = new CheckBoxCell() };
                        this.checkColumn = worksheet.Columns-1;
                        n++;
                    }
                }));

            })).Start();

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
            worksheet.ColumnHeaders[ReceiptMetaData.itemsKeyName.Length].Text = "是否上架";
            worksheet.Columns = ReceiptMetaData.itemsKeyName.Length+1;
        }

        private void FormAddPutawayItem_Load(object sender, EventArgs e)
        {
            InitComponents();
            Search();
        }

        private void buttonPutaway_Click(object sender, EventArgs e)
        {
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
                        if (receiptTicketItem.State != "已收货")
                        {
                            MessageBox.Show(receiptTicketItem.ID + " " + receiptTicketItem.State + ",请先收货");
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
                MessageBox.Show("请选择一项上架");
                return;
            }
            else
            {
                foreach (ReceiptTicketItem i in ids)
                {
                    //PutawayTicketItem putawayTicketItem = (from pti in wmsEntities.PutawayTicketItem where pti.ID == i && pti.State != "已收货" && pti.State != "送检中" select pti).FirstOrDefault();
                    if (i == null)
                    {
                        MessageBox.Show(i + "不能添加此条目上架");
                    }
                    else
                    {
                        PutawayTicketItem putawayTicketItem = new PutawayTicketItem();
                        putawayTicketItem.ReceiptTicketItemID = i.ID;
                        putawayTicketItem.PutawayTicketID = this.putawayTicketID;
                        putawayTicketItem.State = "待上架";
                        //putawayTicketItem.DisplacementPositionNo = "No";
                        //putawayTicketItem.TargetStorageLocation = "Location";
                        
                        wmsEntities.PutawayTicketItem.Add(putawayTicketItem);
                        i.State = "已收货";
                    }
                }
                new Thread(() =>
                {
                    try
                    {
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
                    }));
                    MessageBox.Show("添加成功");
                }).Start();
            }
            /*
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
                        
                        PutawayTicketItem putawayTicketItem = new PutawayTicketItem();
                        ReceiptTicketItemView receiptTicketItemView = (from rti in this.wmsEntities.ReceiptTicketItemView where rti.ID == id select rti).FirstOrDefault();
                        if (receiptTicketItemView != null)
                        {
                            if (receiptTicketItemView.State == "送检中")
                            {
                                MessageBox.Show("此条目已送检，不能收货");
                            }
                            else
                            {
                                putawayTicketItem.ReceiptTicketItemID = id;
                                putawayTicketItem.PutawayTicketID = this.putawayTicketID;
                                putawayTicketItem.State = "待上架";
                                putawayTicketItem.DisplacementPositionNo = "No";
                                putawayTicketItem.TargetStorageLocation = "Location";
                                wmsEntities.PutawayTicketItem.Add(putawayTicketItem);
                            }
                        }
                        else
                        {
                            MessageBox.Show(id.ToString() + "无此送检单条目");
                            return;
                        }
                    }
                }
            }
            new Thread(() =>
            {

                wmsEntities.SaveChanges();
                this.Invoke(new Action(() =>
                {
                    this.Search();
                }));
                MessageBox.Show("添加成功");
            }).Start();*/
        }

        private void buttonReceiptCancel_Click(object sender, EventArgs e)
        {
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
                        if (receiptTicketItem.State != "已收货")
                        {
                            MessageBox.Show(receiptTicketItem.ID + "没有收货");
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
                MessageBox.Show("请选择一项取消收货");
                return;
            }
            else
            {
                foreach (ReceiptTicketItem i in ids)
                {
                    //PutawayTicketItem putawayTicketItem = (from pti in wmsEntities.PutawayTicketItem where pti.ID == i && pti.State != "已收货" && pti.State != "送检中" select pti).FirstOrDefault();
                    if (i == null)
                    {
                        MessageBox.Show(i + "不能取消");
                    }
                    else
                    {
                        PutawayTicketItem putawayTicketItem = (from pti in wmsEntities.PutawayTicketItem where pti.ReceiptTicketItemID == i.ID select pti).FirstOrDefault();
                        if (putawayTicketItem == null)
                        {
                            MessageBox.Show("不能取消");
                        }
                        else
                        {
                            putawayTicketItem.State = "作废";
                        }
                        i.State = "待收货";
                    }
                }
                new Thread(() =>
                {
                    try
                    {
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
                    }));
                    MessageBox.Show("已取消");
                }).Start();
            }
            /*
            var worksheet = this.reoGridControlUser.Worksheets[0];
            bool result;
            for (int i = 0; i < this.countRow; i++)
            {
                result = (worksheet[i, this.checkColumn] as bool?) ?? false;
                if (result == true)
                {
                    int id;
                    if (int.TryParse(worksheet[i, 0].ToString(), out id) == false)
                    {
                        MessageBox.Show(worksheet[i, 0].ToString() + "取消失败");
                    }
                    else
                    {
                        ReceiptTicketItem receiptTicketItem = (from rti in this.wmsEntities.ReceiptTicketItem where rti.ID == id && rti.State != "作废" select rti).FirstOrDefault();
                        PutawayTicketItem putawayTicketItem = (from pti in wmsEntities.PutawayTicketItem where pti.ReceiptTicketItemID == id && pti.State != "作废" select pti).FirstOrDefault();
                        if (putawayTicketItem != null)
                        {
                            putawayTicketItem.State = "作废";
                            receiptTicketItem.State = "待检";
                        }
                        else
                        {
                            MessageBox.Show(id.ToString() +"收货单条目没有上架");
                            return;
                        }
                    }
                }
            }
            new Thread(() =>
            {

                wmsEntities.SaveChanges();
                this.Invoke(new Action(() =>
                {
                    this.Search();
                }));
                MessageBox.Show("取消成功");
            }).Start();
            */
        }
    }
}

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
using WMS.UI;
using System.Threading;
using System.Data.SqlClient;

namespace WMS.UI.FormReceipt
{
    public partial class FormReceiptShelves : Form
    {
        private FormMode formMode;
        private int receiptTicketID;
        private WMSEntities wmsEntities = new WMSEntities();
        private int userID;
        private int warehouseID;
        private int projectID;
        private string key;
        private string value;
        private Action<string, string> ToPutaway = null;
        private PagerWidget<PutawayTicketView> pagerWidget;
        private SearchWidget<PutawayTicketView> searchWidget;
        public FormReceiptShelves()
        {
            InitializeComponent();
        }

        public FormReceiptShelves(int projectID, int warehouseID, int userID)
        {
            InitializeComponent();
            this.projectID = projectID;
            this.warehouseID = warehouseID;
            this.userID = userID;
            this.value = null;
            this.key = null;
        }

        public void SetToPutaway(Action<string, string> action)
        {
            this.ToPutaway = action;
        }

        public FormReceiptShelves(FormMode formMode, int receiptTicketID)
        {
            InitializeComponent();
            this.formMode = formMode;
            this.receiptTicketID = receiptTicketID;
        }

        public FormReceiptShelves(int projectID, int warehouseID, int userID, string key, string value)
        {
            InitializeComponent();
            this.projectID = projectID;
            this.warehouseID = warehouseID;
            this.userID = userID;
            this.key = key;
            this.value = value;
            FormSelectPerson.DefaultPosition = FormBase.Position.RECEIPT;
        }

        private void FormReceiptShelves_Load(object sender, EventArgs e)
        {
            InitComponents();
            if (key != null)
            {
                string name = (from n in ReceiptMetaData.putawayTicketKeyName where n.Key == key select n.Name).FirstOrDefault();
                //this.toolStripComboBoxSelect.SelectedItem = name;
                //this.toolStripComboBoxSelect.SelectedIndex = this.toolStripComboBoxSelect.Items.IndexOf(name);
                //this.toolStripTextBoxSelect.Text = value;
            }
            pagerWidget = new PagerWidget<PutawayTicketView>(this.reoGridControlUser, ReceiptMetaData.putawayTicketKeyName, projectID, warehouseID);
            searchWidget = new SearchWidget<PutawayTicketView>(ReceiptMetaData.putawayTicketKeyName, pagerWidget);
            this.panel1.Controls.Add(pagerWidget);
            this.panel2.Controls.Add(searchWidget);
            if (this.key != null && this.value != null)
            {
                searchWidget.SetSearchCondition(key, value);
            }
            Search();
            pagerWidget.Show();
        }

        private void Search(bool savePage = false, int selectID = -1)
        {
            //this.pagerWidget.ClearCondition();
            //if (this.toolStripComboBoxSelect.SelectedIndex != 0)
            //{
            //    this.pagerWidget.AddCondition(this.toolStripComboBoxSelect.SelectedItem.ToString(), this.toolStripTextBoxSelect.Text);
            //}
            this.searchWidget.Search(savePage, selectID);
        }

        private void InitComponents()
        {
            //初始化
            //this.toolStripComboBoxSelect.Items.Add("无");
            string[] columnNames = (from kn in ReceiptMetaData.putawayTicketKeyName where kn.Visible == true select kn.Name).ToArray();
            //this.toolStripComboBoxSelect.Items.AddRange(columnNames);
            //this.toolStripComboBoxSelect.SelectedIndex = 0;

            //初始化表格
            var worksheet = this.reoGridControlUser.Worksheets[0];
            worksheet.SelectionMode = WorksheetSelectionMode.Row;
            for (int i = 0; i < ReceiptMetaData.putawayTicketKeyName.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = ReceiptMetaData.putawayTicketKeyName[i].Name;
                worksheet.ColumnHeaders[i].IsVisible = ReceiptMetaData.putawayTicketKeyName[i].Visible;
            }
            worksheet.Columns = ReceiptMetaData.putawayTicketKeyName.Length;
        }

        private void Search(string key, string value)
        {
            this.lableStatus.Text = "搜索中...";

            new Thread(new ThreadStart(() =>
            {
                var wmsEntities = new WMSEntities();
                PutawayTicketView[] putawayTicketView = null;
                if (key == null || value == null)        //搜索所有
                {
                    try
                    {
                        putawayTicketView = wmsEntities.Database.SqlQuery<PutawayTicketView>("SELECT * FROM PutawayTicketView WHERE WarehouseID = @warehouseID AND ProjectID = @projectID ORDER BY ID DESC", new SqlParameter[] { new SqlParameter("warehouseID", this.warehouseID), new SqlParameter("projectID", this.projectID) }).ToArray();
                    }
                    catch
                    {
                        MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    double tmp;
                    //if (Double.TryParse(value, out tmp) == false) //不是数字则加上单引号
                    //{
                    //    value = "'" + value + "'";
                    //}
                    try
                    {
                        putawayTicketView = wmsEntities.Database.SqlQuery<PutawayTicketView>(String.Format("SELECT * FROM PutawayTicketView WHERE {0} = @key AND WarehouseID = @warehouseID AND ProjectID = @projectID ORDER BY ID DESC", key), new SqlParameter[] { new SqlParameter("@key", value), new SqlParameter("@warehouseID", this.warehouseID), new SqlParameter("@projectID", this.projectID) }).ToArray();
                    }
                    catch (EntityException)
                    {
                        MessageBox.Show("查询的值不合法，请输入正确的值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        return;
                    }
                }
                this.reoGridControlUser.Invoke(new Action(() =>
                {
                    this.lableStatus.Text = "搜索完成";
                    var worksheet = this.reoGridControlUser.Worksheets[0];
                    worksheet.DeleteRangeData(RangePosition.EntireRange);
                    int n = 0;
                    for (int i = 0; i < putawayTicketView.Length; i++)
                    {

                        PutawayTicketView curReceiptTicketView = putawayTicketView[i];
                        if (curReceiptTicketView.State == "作废")
                        {
                            continue;
                        }
                        object[] columns = Utilities.GetValuesByPropertieNames(curReceiptTicketView, (from kn in ReceiptMetaData.putawayTicketKeyName select kn.Key).ToArray());
                        for (int j = 0; j < worksheet.Columns; j++)
                        {
                            if (columns[j] == null)
                            {
                                worksheet[n, j] = columns[j];
                            }
                            else
                            {
                                worksheet[n, j] = columns[j].ToString();
                            }
                        }
                        n++;

                    }
                }));
                if (putawayTicketView.Length == 0)
                {
                    int m = ReceiptUtilities.GetFirstColumnIndex(ReceiptMetaData.submissionTicketKeyName);

                    //this.reoGridControl1.Worksheets[0][6, 8] = "32323";
                    this.reoGridControlUser.Worksheets[0][0, m] = "没有查询到符合条件的记录";
                }

            })).Start();

        }

        private void toolStripButtonItem_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlUser.Worksheets[0];
            try
            {
                WMSEntities wmsEntities = new WMSEntities();
                if (worksheet.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int putawayTicketID;
                try
                {
                    putawayTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                }
                catch
                {
                    MessageBox.Show("请选择一项进行查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                PutawayTicket putawayTicket = (from rt in wmsEntities.PutawayTicket where rt.ID == putawayTicketID select rt).FirstOrDefault();
                if (putawayTicket == null)
                {
                    MessageBox.Show("该收货单不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    string key = "上架单ID";
                    string name = (from r in ReceiptMetaData.receiptNameKeys where r.Key == key select r.Name).FirstOrDefault();
                    string value = putawayTicket.ID.ToString();
                    //ToPutaway(key, value);
                    FormShelvesItem formShelvesItem = new FormShelvesItem(this.projectID, this.warehouseID, this.userID, "上架单ID", value);
                    formShelvesItem.SetCallBack(new Action(()=> 
                    {
                        this.Search();
                    }));
                    formShelvesItem.Show();
                }
            }

            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
            /*
            var worksheet = this.reoGridControlUser.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项进行查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int putawayTicketID;
                try
                {
                    putawayTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                }
                catch
                {
                    MessageBox.Show("请选择一项进行查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                FormShelvesItem formShelvesItem = new FormShelvesItem(putawayTicketID);
                formShelvesItem.SetCallBack(new Action(() =>
                {
                    this.Search(null, null);
                }));
                formShelvesItem.Show();
            }
            
            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }    */
        }

        //private void toolStripButtonSelect_Click(object sender, EventArgs e)
        //{
        //    if (this.toolStripComboBoxSelect.SelectedIndex == 0)
        //    {
        //        Search();
        //    }
        //    else
        //    {
        //        string condition = this.toolStripComboBoxSelect.Text;
        //        string key = "";
        //        foreach (KeyName kn in ReceiptMetaData.putawayTicketKeyName)
        //        {
        //            if (condition == kn.Name)
        //            {
        //                key = kn.Key;
        //                break;
        //            }
        //        }
        //        string value = this.toolStripTextBoxSelect.Text;
        //        if (key != null || value != null)
        //        {
        //            pagerWidget.AddCondition(key, value);
        //        }
        //        pagerWidget.Search();
        //    }
        //}

        //private void toolStripComboBoxSelect_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (this.toolStripComboBoxSelect.SelectedIndex == 0)
        //    {
        //        this.toolStripTextBoxSelect.Text = "";
        //        this.toolStripTextBoxSelect.Enabled = false;
        //    }
        //    else
        //    {
        //        this.toolStripTextBoxSelect.Text = "";
        //        this.toolStripTextBoxSelect.Enabled = true;
        //    }
        //}

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlUser.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项进行查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int putawayTicketID;
                try
                {
                    putawayTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                }
                catch
                {
                    MessageBox.Show("请选择一项进行查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }                //FormShelvesItem formShelvesItem = new FormShelvesItem(putawayTicketID);
                FormPutawayModify formPutawayModify = new FormPutawayModify(putawayTicketID);
                formPutawayModify.SetCallBack(new Action(() =>
                {
                    this.Search();
                }));
                formPutawayModify.Show();
            }

            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
            this.Search();
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

            WMSEntities wmsEntities = new WMSEntities();
            foreach (int id in deleteIDs)
            {
                //if (MessageBox.Show("确定删除该上架单？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                PutawayTicket putawayTicket = (from pt in wmsEntities.PutawayTicket where pt.ID == id select pt).FirstOrDefault();
                if (putawayTicket == null)
                {
                    MessageBox.Show("该上架单已被删除，请刷新查看!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    return;
                }
                //if (putawayTicket.State != "待上架")
                //{
                //    MessageBox.Show("该上架单已有部分或者全部上架，无法删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                //    continue;
                //}
                ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == putawayTicket.ReceiptTicketID select rt).FirstOrDefault();
                PutawayTicketItem[] putawayTicketItems = putawayTicket.PutawayTicketItem.ToArray();
                int n = 0;
                foreach (PutawayTicketItem pti in putawayTicketItems)
                {
                    ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == pti.ReceiptTicketItemID select rti).FirstOrDefault();
                    if (receiptTicketItem != null)
                    {

                        receiptTicketItem.HasPutwayAmount -= (pti.ScheduledMoveCount == null ? 0 : pti.ScheduledMoveCount) - (pti.PutawayAmount == null ? 0 : pti.PutawayAmount);
                        //receiptTicketItem.HasPutwayAmount += pti.PutawayAmount;
                        if (receiptTicketItem.HasPutwayAmount == 0)
                        {
                            n++;
                        }
                    }
                }
                if (n == putawayTicketItems.Length)
                {
                    receiptTicket.HasPutawayTicket = "未生成上架单";
                }
                else
                {
                    receiptTicket.HasPutawayTicket = "部分生成上架单";
                }
                try
                {
                    wmsEntities.Database.ExecuteSqlCommand("DELETE FROM PutawayTicket WHERE ID = @putawayTicketID", new SqlParameter("putawayTicketID", id));
                    
                }
                catch
                {
                    MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    return;
                }
                //}
            }
            MessageBox.Show("成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            try
            {
                new Thread(() =>
                {
                    wmsEntities.SaveChanges();
                    this.Invoke(new Action(() =>
                    {
                        if (this.key != null || this.value != null)
                        {
                            pagerWidget.AddCondition(this.key, this.value);
                        }
                        pagerWidget.Search();
                    }));
                }).Start();
            }
            catch
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
            //var worksheet = this.reoGridControlUser.Worksheets[0];
            //try
            //{
            /*
            if (worksheet.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项进行查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int putawayTicketID;
            try
            {
                putawayTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
            }
            catch
            {
                MessageBox.Show("请选择一项进行查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }  */
            //FormShelvesItem formShelvesItem = new FormShelvesItem(putawayTicketID);
            /*
            if (MessageBox.Show("确定删除该上架单？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                PutawayTicket putawayTicket = (from pt in wmsEntities.PutawayTicket where pt.ID == putawayTicketID select pt).FirstOrDefault();
                if (putawayTicket == null)
                {
                    MessageBox.Show("该上架单已被删除，请刷新查看!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    return;
                }
                if (putawayTicket.State != "待上架")
                {
                    MessageBox.Show("该上架单已有部分或者全部上架，无法删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    return;
                }
                ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == putawayTicket.ReceiptTicketID select rt).FirstOrDefault();
                PutawayTicketItem[] putawayTicketItems = putawayTicket.PutawayTicketItem.ToArray();
                int n = 0;
                foreach (PutawayTicketItem pti in putawayTicketItems)
                {
                    ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == pti.ReceiptTicketItemID select rti).FirstOrDefault();
                    if (receiptTicketItem != null)
                    {
                        receiptTicketItem.HasPutwayAmount -= pti.ScheduledMoveCount;
                        if (receiptTicketItem.HasPutwayAmount == 0)
                        {
                            n++;
                        }
                    }
                }
                if (n == putawayTicketItems.Length)
                {
                    receiptTicket.HasPutawayTicket = "未生成上架单";
                }
                else
                {
                    receiptTicket.HasPutawayTicket = "部分生成上架单";
                }
                try
                {
                    new Thread(() =>
                    {
                        wmsEntities.Database.ExecuteSqlCommand("DELETE FROM PutawayTicket WHERE ID = @putawayTicketID", new SqlParameter("putawayTicketID", putawayTicketID));
                        wmsEntities.SaveChanges();
                        MessageBox.Show("成功");
                        this.Invoke(new Action(() =>
                        {
                            if (this.key != null || this.value != null)
                            {
                                pagerWidget.AddCondition(this.key, this.value);
                            }
                            pagerWidget.Search();
                        }));
                    }).Start();
                }
                catch (EntityException)
                {
                    MessageBox.Show("该上架单已被删除!");
                }
                catch (Exception)
                {
                    MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    return;
                }
            }
        }
        catch
        {
            MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            return;
        }*/
        }

        private void toolStripButtonPrint_Click(object sender, EventArgs e)
        {

        }

        //private void toolStripComboBoxSelect_SelectedIndexChanged_1(object sender, EventArgs e)
        //{
        //    if (this.toolStripComboBoxSelect.SelectedIndex == 0)
        //    {
        //        this.toolStripTextBoxSelect.Text = "";
        //        this.toolStripTextBoxSelect.Enabled = false;
        //    }
        //    else
        //    {
        //        this.toolStripTextBoxSelect.Text = "";
        //        this.toolStripTextBoxSelect.Enabled = true;
        //    }
        //}

        private void toolStripButtonDistributeCancel_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlUser.Worksheets[0];
            StandardFormPreviewExcel formPreview = new StandardFormPreviewExcel("上架单预览");
            WMSEntities wmsEntities = new WMSEntities();
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlUser);
            if (ids.Length == 0)
            {
                MessageBox.Show("请选择一项预览", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            foreach (int id in ids)
            {
                try
                {
                    PutawayTicketView putawayTicketView = (from ptv in wmsEntities.PutawayTicketView where ptv.ID == id select ptv).FirstOrDefault();
                    PutawayTicketItemView[] putawayTicketItemView = (from ptiv in wmsEntities.PutawayTicketItemView where ptiv.PutawayTicketID == putawayTicketView.ID select ptiv).ToArray<PutawayTicketItemView>();
                    string worksheetName = id.ToString();
                    if (putawayTicketView == null)
                    {
                        MessageBox.Show("上架单不存在，请重新查询！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    //ReceiptTicketView receiptTicketView = (from rtv in wmsEntities.ReceiptTicketView where rtv.ID == submissionTicketView.ReceiptTicketID select rtv).FirstOrDefault();
                    if (formPreview.AddPatternTable(@"Excel\PutawayTicket.xlsx", worksheetName) == false)
                    {
                        this.Close();
                        return;
                    }
                    if (putawayTicketView != null)
                    {
                        formPreview.AddData("PutawayTicket", putawayTicketView, worksheetName);
                    }
                    formPreview.AddData("PutawayTicketItem", putawayTicketItemView, worksheetName);
                    formPreview.SetPrintScale(1.0F, worksheetName);
                }
                catch
                {
                    MessageBox.Show("搜索失败，请检查网络连接！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            formPreview.SetPrintedCallback(new Action(()=>
            {
                WMSEntities wmsEntities2 = new WMSEntities();
                foreach(int id in ids)
                {
                    PutawayTicket putawayTicket = (from pt in wmsEntities2.PutawayTicket where pt.ID == id select pt).FirstOrDefault();
                    if (putawayTicket != null)
                    {
                        if (putawayTicket.PrintTimes == null)
                        {
                            putawayTicket.PrintTimes = 0;
                        }
                        putawayTicket.PrintTimes++;
                    }
                }
                new Thread(()=> 
                {
                    wmsEntities2.SaveChanges();
                    Search();
                }).Start();
            }));
            
            //formPreview.AddData("SubmissionTicketItem", submissionTicketItemView);
            formPreview.Show();
        }

        //private void toolStripTextBoxSelect_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        this.toolStripButtonSelect.PerformClick();
        //    }
        //}
    }
}

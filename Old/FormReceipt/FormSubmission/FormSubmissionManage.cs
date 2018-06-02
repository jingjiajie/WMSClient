using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.UI.FormReceipt;
using unvell.ReoGrid;
using System.Threading;
using System.Data.SqlClient;
using WMS.DataAccess;


namespace WMS.UI
{
    public partial class FormSubmissionManage : Form
    {
        private int projectID;
        private int warehouseID;
        private int userID;

        private string key;
        private string value;
        PagerWidget<SubmissionTicketView> pagerWidget;
        SearchWidget<SubmissionTicketView> searchWidget;
        Action<string, string> ToPutaway = null;
        public FormSubmissionManage()
        {
            InitializeComponent();
        }

        public FormSubmissionManage(int projectID, int warehouseID, int userID)
        {
            InitializeComponent();
            this.projectID = projectID;
            this.warehouseID = warehouseID;
            this.userID = userID;
            this.key = null;
            this.value = null;
        }

        public FormSubmissionManage(int projectID, int warehouseID, int userID, string key, string value)
        {
            InitializeComponent();
            this.projectID = projectID;
            this.warehouseID = warehouseID;
            this.userID = userID;
            this.key = key;
            this.value = value;
            FormSelectPerson.DefaultPosition = FormBase.Position.RECEIPT;
        }

        private void FormSubmissionManage_Load(object sender, EventArgs e)
        {
            InitComponents();
            if (key != null)
            {
                string name = (from n in ReceiptMetaData.submissionTicketKeyName where n.Key == key select n.Name).FirstOrDefault();
                //this.comboBoxSelect.SelectedItem = name;
                //this.comboBoxSelect.SelectedIndex = this.comboBoxSelect.Items.IndexOf(name);

            }
            pagerWidget = new PagerWidget<SubmissionTicketView>(this.reoGridControl1, ReceiptMetaData.submissionTicketKeyName, projectID, warehouseID);
            
            searchWidget = new SearchWidget<SubmissionTicketView>(ReceiptMetaData.submissionTicketKeyName, pagerWidget);
            if (key != null && value != null)
            {
                //pagerWidget.AddCondition(key, value);
                searchWidget.SetSearchCondition(key, value);
            }
            this.panel2.Controls.Add(searchWidget);
            //this.textBoxSelect.Text = value;
            this.panel1.Controls.Add(this.pagerWidget);
            pagerWidget.Show();
            this.Search();
            //Search(key, value);
        }

        public void setActionTo(Action<string, string> action)
        {
            this.ToPutaway = action;
        }

        private void Search(bool savePage = false, int selectID = -1)
        {
            //this.pagerWidget.ClearCondition();
            
            //if (this.comboBoxSelect.SelectedIndex != 0)
            //{
            //    this.pagerWidget.AddCondition(this.comboBoxSelect.SelectedItem.ToString(), this.textBoxSelect.Text);
            //}
            //this.pagerWidget.Search(savePage, selectID);
            this.searchWidget.Search(savePage, selectID);
        }

        private void InitComponents()
        {
            //初始化
            //this.comboBoxSelect.Items.Add("无");
            string[] columnNames = (from kn in ReceiptMetaData.submissionTicketKeyName where kn.Visible == true select kn.Name).ToArray();
            //this.comboBoxSelect.Items.AddRange(columnNames);
            //this.comboBoxSelect.SelectedIndex = 0;

            //初始化表格
            var worksheet = this.reoGridControl1.Worksheets[0];
            worksheet.SelectionMode = WorksheetSelectionMode.Row;
            for (int i = 0; i < ReceiptMetaData.submissionTicketKeyName.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = ReceiptMetaData.submissionTicketKeyName[i].Name;
                worksheet.ColumnHeaders[i].IsVisible = ReceiptMetaData.submissionTicketKeyName[i].Visible;
            }
            worksheet.Columns = ReceiptMetaData.submissionTicketKeyName.Length;
        }

        private void Search(string key, string value)
        {
            this.toolStripStatusLabel2.Text = "搜索中...";

            new Thread(new ThreadStart(() =>
            {
                var wmsEntities = new WMSEntities();
                //ReceiptTicketView[] receiptTicketViews = null;
                SubmissionTicketView[] submissionTicketView = null;
                if (key == null || value == null)        //搜索所有
                {
                    try
                    {
                        submissionTicketView = wmsEntities.Database.SqlQuery<SubmissionTicketView>("SELECT * FROM SubmissionTicketView WHERE WarehouseID = @warehouseID AND ProjectID = @projectID ORDER BY ID DESC", new SqlParameter[] { new SqlParameter("warehouseID", this.warehouseID), new SqlParameter("projectID", this.projectID) }).ToArray();
                    }
                    catch
                    {
                        MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    //double tmp;
                    //if (Double.TryParse(value, out tmp) == false) //不是数字则加上单引号
                    //{
                    //    value = "'" + value + "'";
                    //}
                    try
                    {
                        submissionTicketView = wmsEntities.Database.SqlQuery<SubmissionTicketView>(String.Format("SELECT * FROM SubmissionTicketView WHERE {0} = @key AND WarehouseID = @warehouseID AND ProjectID = @projectID ORDER BY ID DESC", key), new SqlParameter[] { new SqlParameter("@key", value), new SqlParameter("@warehouseID", this.warehouseID), new SqlParameter("@projectID", this.projectID) }).ToArray();
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

                this.reoGridControl1.Invoke(new Action(() =>
                {
                    this.toolStripStatusLabel2.Text = "搜索完成";
                    var worksheet = this.reoGridControl1.Worksheets[0];
                    worksheet.DeleteRangeData(RangePosition.EntireRange);
                    int n = 0;
                    for (int i = 0; i < submissionTicketView.Length; i++)
                    {
                        if (submissionTicketView[i].State == "作废")
                        {
                            continue;
                        }
                        SubmissionTicketView curSubmissionTicketView = submissionTicketView[i];
                        object[] columns = Utilities.GetValuesByPropertieNames(curSubmissionTicketView, (from kn in ReceiptMetaData.submissionTicketKeyName select kn.Key).ToArray());
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
                    if (submissionTicketView.Length == 0)
                    {
                        int m = ReceiptUtilities.GetFirstColumnIndex(ReceiptMetaData.submissionTicketKeyName);

                        //this.reoGridControl1.Worksheets[0][6, 8] = "32323";
                        this.reoGridControl1.Worksheets[0][0, m] = "没有查询到符合条件的记录";
                    }
                }));

            })).Start();

        }

        private void reoGridControlUser_Click(object sender, EventArgs e)
        {

        }

        //private void buttonSelect_Click(object sender, EventArgs e)
        //{
        //    if (comboBoxSelect.SelectedIndex == 0)
        //    {
        //        Search();
        //    }
        //    else
        //    {
        //        string condition = this.comboBoxSelect.Text;
        //        string key = "";
        //        foreach (KeyName kn in ReceiptMetaData.submissionTicketKeyName)
        //        {
        //            if (condition == kn.Name)
        //            {
        //                key = kn.Key;
        //                break;
        //            }
        //        }
        //        string value = this.textBoxSelect.Text;
        //        if (key != null && value != null)
        //        {
        //            this.pagerWidget.AddCondition(key, value);
        //        }
        //        Search();
        //    }
        //}

        private void buttonPass_Click(object sender, EventArgs e)
        {

            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControl1.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketID select st).FirstOrDefault();
                if (submissionTicket == null)
                {
                    MessageBox.Show("找不到该送检单");
                    return;
                }
                ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == submissionTicket.ReceiptTicketID select rt).FirstOrDefault();
                if (receiptTicket == null)
                {
                    MessageBox.Show("对应收货单已被删除，无法收货");
                    return;
                }

                //receiptTicket.State = "已收货";
                ReceiptTicketItem[] receiptTicketItems = receiptTicket.ReceiptTicketItem.ToArray();
                int n = 0;
                foreach (ReceiptTicketItem rti in receiptTicketItems)
                {
                    if (rti.RefuseAmount > 0)
                    {
                        rti.State = "部分收货";
                    }
                    else
                    {
                        rti.State = "已收货";
                        n++;
                    }
                    StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == rti.ID select si).FirstOrDefault();
                    if (stockInfo != null)
                    {
                        stockInfo.OverflowAreaAmount += stockInfo.ReceiptAreaAmount;
                        stockInfo.ReceiptAreaAmount -= stockInfo.ReceiptAreaAmount;
                    }
                }
                if (n == receiptTicketItems.Length)
                {
                    receiptTicket.State = "已收货";
                }
                else
                {
                    receiptTicket.State = "部分收货";
                }
                new Thread(() =>
                {
                    wmsEntities.SaveChanges();
                    MessageBox.Show("成功");
                    this.Invoke(new Action(() =>
                    {
                        if (this.key == null || this.value == null)
                        {
                            this.Search();
                        }
                        else
                        {
                            this.Search(this.key, this.value);
                        }
                    }));
                }).Start();
                /*
                SubmissionTicketItem[] submissionTicketItem = (from sti in wmsEntities.SubmissionTicketItem where sti.SubmissionTicketID == submissionTicketID select sti).ToArray();
                foreach (SubmissionTicketItem sti in submissionTicketItem)
                {
                    sti.State = "合格";
                    ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == sti.ReceiptTicketItemID select rti).FirstOrDefault();
                    if (receiptTicketItem != null)
                    {
                        receiptTicketItem.State = "过检";
                    }
                }
                submissionTicket.State = "合格";
                ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == submissionTicket.ReceiptTicketID select rt).FirstOrDefault();
                new Thread(() =>
                {
                    wmsEntities.SaveChanges();
                    if (receiptTicket != null)
                    {
                        int count = wmsEntities.Database.SqlQuery<int>(
                        "SELECT COUNT(*) FROM ReceiptTicketItem " +
                        "WHERE State <> '过检' AND ReceiptTicketID = @receiptTicketID",
                        new SqlParameter("receiptTicketID", receiptTicket.ID)).FirstOrDefault();
                        if (count == 0)
                        {
                            wmsEntities.Database.ExecuteSqlCommand(
                                "UPDATE ReceiptTicket SET State = '过检' " +
                                "WHERE ID = @receiptTicketID",
                                new SqlParameter("receiptTicketID", receiptTicket.ID));
                        }
                        else
                        {
                            int count2 = wmsEntities.Database.SqlQuery<int>(
                                "SELECT COUNT(*) FROM ReceiptTicketItem " +
                                "WHERE State = '已收货' AND ReceiptTicketID = @receiptTicketID",
                                new SqlParameter("receiptTicketID", receiptTicket.ID)).FirstOrDefault();
                            if (count2 == 0)
                            {
                                wmsEntities.Database.ExecuteSqlCommand(
                                    "UPDATE ReceiptTicket SET State = '部分过检' " +
                                    "WHERE ID = @receiptTicketID",
                                    new SqlParameter("receiptTicketID", receiptTicket.ID));
                            }
                        }
                        if (MessageBox.Show("是否同时收货?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            foreach (SubmissionTicketItem sti in submissionTicketItem)
                            {
                            //sti.State = "合格";
                                ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == sti.ReceiptTicketItemID select rti).FirstOrDefault();
                                if (receiptTicketItem != null)
                                {
                                    receiptTicketItem.State = "已收货";
                                    StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == receiptTicketItem.ID select si).FirstOrDefault();
                                    //StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == receiptTicketItem.ID select si).FirstOrDefault();
                                    if (stockInfo != null)
                                    {/*TODO
                                        if (stockInfo.SubmissionAreaAmount != null)
                                        {
                                            int amount = (int)stockInfo.SubmissionAreaAmount;
                                            stockInfo.ReceiptAreaAmount += amount;
                                            stockInfo.SubmissionAreaAmount = 0;
                                        }
                                    }
                                }
                            }
                            wmsEntities.SaveChanges();
                            int count2 = wmsEntities.Database.SqlQuery<int>(
                                "SELECT COUNT(*) FROM ReceiptTicketItem " +
                                "WHERE State <> '已收货' AND ReceiptTicketID = @receiptTicketID",
                                new SqlParameter("receiptTicketID", receiptTicket.ID)).FirstOrDefault();
                            if (count2 == 0)
                            {
                                wmsEntities.Database.ExecuteSqlCommand(
                                    "UPDATE ReceiptTicket SET State = '已收货' " +
                                    "WHERE ID = @receiptTicketID",
                                    new SqlParameter("receiptTicketID", receiptTicket.ID));
                            }
                            else
                            {
                                wmsEntities.Database.ExecuteSqlCommand(
                                    "UPDATE ReceiptTicket SET State = '部分收货' " +
                                    "WHERE ID = @receiptTicketID",
                                    new SqlParameter("receiptTicketID", receiptTicket.ID));
                            }
                        }
                    }
                    this.Invoke(new Action(() =>
                    {
                        this.Search(null, null);
                    }));
                }).Start();*/
            }
            catch
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }

            /*
            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControl1.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new EntityCommandExecutionException();
                }
                int submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketID select st).Single();
                if (submissionTicket.State == "合格")
                {
                    MessageBox.Show("该送检单状态已置为合格");
                }
                else
                {
                    wmsEntities.Database.ExecuteSqlCommand("UPDATE SubmissionTicket SET State='合格' WHERE ID=@submissionTicketID", new SqlParameter("submissionTicketID", submissionTicketID));
                    wmsEntities.Database.ExecuteSqlCommand("UPDATE SubmissionTicketItem SET State='合格' WHERE SubmissionTicketID=@submissionTicketID", new SqlParameter("submissionTicketID", submissionTicketID));
                    wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State='过检' WHERE ID=@receiptTicket", new SqlParameter("receiptTicket", submissionTicket.ReceiptTicketID));
                    wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicketItem SET State='过检' WHERE ReceiptTicketID=@receiptTicket", new SqlParameter("receiptTicket", submissionTicket.ReceiptTicketID));

                    if (MessageBox.Show("是否同时收货？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == submissionTicket.ReceiptTicketID select rt).FirstOrDefault();
                        if (receiptTicket != null)
                        {
                            if (receiptTicket.State != "已收货")
                            {
                                wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State='已收货' WHERE ID=@receiptTicket", new SqlParameter("receiptTicket", submissionTicket.ReceiptTicketID));
                                wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicketItem SET State='已收货' WHERE ReceiptTicketID=@receiptTicket", new SqlParameter("receiptTicket", submissionTicket.ReceiptTicketID));
                            }
                            
                        }
                    }
                }
            }
            catch(EntityCommandExecutionException)
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
            this.Search(null, null);
            */
        }

        private void buttonNoPass_Click(object sender, EventArgs e)
        {


            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControl1.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketID select st).FirstOrDefault();
                if (submissionTicket == null)
                {
                    MessageBox.Show("找不到该送检单");
                    return;
                }
                ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == submissionTicket.ReceiptTicketID select rt).FirstOrDefault();
                if (receiptTicket == null)
                {
                    MessageBox.Show("对应收货单已被删除，无法拒收");
                    return;
                }
                if (receiptTicket.HasPutawayTicket == "全部生成上架单" || receiptTicket.HasPutawayTicket == "部分生成上架单")
                {
                    MessageBox.Show("该送检单所对应的收货单已经生成上架单，无法拒收");
                    return;
                }
                receiptTicket.State = "拒收";
                ReceiptTicketItem[] receiptTicketItems = receiptTicket.ReceiptTicketItem.ToArray();
                foreach (ReceiptTicketItem rti in receiptTicketItems)
                {
                    StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == rti.ID select si).FirstOrDefault();
                    rti.RefuseAmount += rti.ReceiviptAmount;
                    rti.UnitCount = 0;
                    rti.ReceiviptAmount = 0;
                    rti.State = "拒收";
                    if (stockInfo != null)
                    {
                        stockInfo.ReceiptAreaAmount = 0;
                    }
                }
                new Thread(() =>
                {
                    wmsEntities.SaveChanges();
                    MessageBox.Show("成功");
                    this.Invoke(new Action(() =>
                    {
                        if (this.key == null || this.value == null)
                        {
                            this.Search();
                        }
                        else
                        {
                            this.Search(this.key, this.value);
                        }
                    }));
                }).Start();
            }
            catch
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
        }

        private void buttonItem_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControl1.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int submissionTicketID;
                try
                {
                    submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                }
                catch
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                FormSubmissionItem formSubmissionItem = new FormSubmissionItem(submissionTicketID);
                formSubmissionItem.SetCallBack(new Action(() =>
                {
                    this.Search();
                }));
                formSubmissionItem.Show();
            }
            catch (EntityCommandExecutionException)
            {
                MessageBox.Show("请选择一项进行查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
            this.Search();
        }

        private void buttonItems_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControl1.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int submissionTicketID;
                try
                {
                    submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                }
                catch
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                FormAddSubmissionTicket formAddSubmissionTicket = new FormAddSubmissionTicket(submissionTicketID, this.userID, FormMode.ALTER);
                formAddSubmissionTicket.SetCallBack(new Action(() =>
                {
                    this.Search();
                }));
                formAddSubmissionTicket.Show();
                /*
                FormReceiptArrivalCheck formReceiptArrivalCheck = new FormReceiptArrivalCheck(submissionTicketID, this.userID);
                formReceiptArrivalCheck.SetFinishedAction(()=> {
                    Search(null, null);
                });
                formReceiptArrivalCheck.Show();
                */
            }

            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
            this.Search();
        }

        //private void comboBoxSelect_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (this.comboBoxSelect.SelectedIndex == 0)
        //    {
        //        this.textBoxSelect.Text = "";
        //        this.textBoxSelect.Enabled = false;
        //    }
        //    else
        //    {
        //        this.textBoxSelect.Text = "";
        //        this.textBoxSelect.Enabled = true;
        //    }
        //}

        private void toolStripTop_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {


            if (MessageBox.Show("确认删除，并取消送检？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                Search();
                return;
            }
            var worksheet = this.reoGridControl1.Worksheets[0];
            WMSEntities wmsEntities = new WMSEntities();
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int submissionTicketID;
                try
                {
                    submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                }
                catch
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketID select st).FirstOrDefault();
                if (submissionTicket == null)
                {
                    MessageBox.Show("此送检单已被删除");
                    this.Search();
                    return;
                }
                else
                {
                    if (submissionTicket.State != "待检")
                    {
                        MessageBox.Show("该送检单状态为" + submissionTicket.State + "，不能删除送检单！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    new Thread(() =>
                    {
                        try
                        {
                            SubmissionTicketItem[] submissionTicketItems = (from sti in wmsEntities.SubmissionTicketItem where sti.SubmissionTicketID == submissionTicketID select sti).ToArray();

                            foreach (SubmissionTicketItem sti in submissionTicketItems)
                            {
                                ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == sti.ReceiptTicketItemID select rti).FirstOrDefault();
                                if (receiptTicketItem != null)
                                {
                                    if (receiptTicketItem.ReceiptTicket.State == "送检中" || receiptTicketItem.ReceiptTicket.State == "过检" || receiptTicketItem.ReceiptTicket.State == "未过检")
                                    {
                                        receiptTicketItem.State = "待收货";
                                        StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == receiptTicketItem.ID select si).FirstOrDefault();
                                        if (stockInfo != null)
                                        {
                                            stockInfo.ReceiptAreaAmount += stockInfo.SubmissionAmount;
                                            stockInfo.SubmissionAmount -= stockInfo.SubmissionAmount;
                                        }
                                        receiptTicketItem.State = "待收货";
                                    }
                                }
                            }
                            ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == submissionTicket.ReceiptTicketID select rt).FirstOrDefault();

                            if (receiptTicket.State == "送检中" || receiptTicket.State == "过检" || receiptTicket.State == "未过检")
                            {
                                if (receiptTicket != null)
                                {
                                    receiptTicket.State = "待收货";
                                    receiptTicket.HasSubmission = 0;
                                }
                            }
                            else
                            {
                                MessageBox.Show("该送检单对应收货单已收货或上架，无法改变收货单状态，但删除送检单成功。");
                            }
                            wmsEntities.Database.ExecuteSqlCommand("DELETE FROM SubmissionTicket WHERE ID = @submissionTicketID", new SqlParameter("submissionTicketID", submissionTicket.ID));
                            wmsEntities.SaveChanges();
                            this.Invoke(new Action(() =>
                            {
                                this.Search();
                            }));

                            //wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicketItem SET State = '待收货' WHERE ID = @receiptTicketItemID", new SqlParameter("receiptTicketItemID", sti.ReceiptTicketItemID));


                        }
                        catch
                        {
                            MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                            return;
                        }
                    }).Start();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
            Search();
        }




        private void ButtonOutput_Click(object sender, EventArgs e)
        {
            try
            {
                WMSEntities wmsEntities = new WMSEntities();
                int[] ids = Utilities.GetSelectedIDs(this.reoGridControl1);
                if (ids.Length == 0)
                {
                    MessageBox.Show("请选择一项预览", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                FormSubmissionChooseExcelType formSubmissionChooseExcelType = new FormSubmissionChooseExcelType(ids);
                formSubmissionChooseExcelType.Show();
            }
            catch
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }

            
            
            //this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControl1.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                WMSEntities wmsEntities = new WMSEntities();
                int submissionTicketID;
                try
                {
                    submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                }
                catch
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //var formReceiptTicketIems = new FormReceiptItems(FormMode.ALTER, receiptTicketID);

                SubmissionTicket submissionTicket = (from rt in wmsEntities.SubmissionTicket where rt.ID == submissionTicketID select rt).FirstOrDefault();
                if (submissionTicket == null)
                {
                    MessageBox.Show("该收货单已被删除，请刷新后查看！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //if (submissionTicket.State == "待检")
                //{
                //    MessageBox.Show("该送检单状态为待检，不能生成上架单!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                //    return;
                //}
                /*
                if (receiptTicket.HasPutawayTicket == "是")
                {
                    MessageBox.Show("该收货单已经生成上架单，点击查看对应上架单按钮查看！");
                    return;
                }*/
                //if (submissionTicket.State != "合格")
                //{
                //    MessageBox.Show("该送检单状态为" + submissionTicket.State + "，不能生成上架单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}
                ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == submissionTicket.ReceiptTicketID select rt).FirstOrDefault();
                if (receiptTicket == null)
                {
                    MessageBox.Show("该收货单已被删除，不能上架", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                FormPutawayNew formPutawayNew = new FormPutawayNew(receiptTicket.ID, this.userID, FormMode.ADD);
                formPutawayNew.SetCallBack(new Action(() =>
                {
                    this.Search();
                }));

                formPutawayNew.Show();
               
            }

            catch (Exception)
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControl1.Worksheets[0];
            //try
            //{
                WMSEntities wmsEntities = new WMSEntities();
                if (worksheet.SelectionRange.Rows != 1)
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int submissionTicketID;
                try
                {
                    submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                }
                catch
                {
                    MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                SubmissionTicketView submissionTicketView = (from rt in wmsEntities.SubmissionTicketView where rt.ID == submissionTicketID select rt).FirstOrDefault();
                if (submissionTicketView == null)
                {
                    MessageBox.Show("该送检单不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    string key = "ReceiptTicketNo";
                    string name = (from r in ReceiptMetaData.receiptNameKeys where r.Key == key select r.Name).FirstOrDefault();
                    string value = submissionTicketView.ReceiptTicketNo;
                    ToPutaway(key, value);
                }
            //}

            //catch (Exception)
            //{
            //    MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            //    return;
            //}
        }

        //private void textBoxSelect_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        this.buttonSelect.PerformClick();
        //    }
        //}
    }
}

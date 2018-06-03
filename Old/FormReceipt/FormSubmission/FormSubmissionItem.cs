using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.UI;
using unvell.ReoGrid;
using WMS.DataAccess;
using System.Threading;
using System.Data.SqlClient;

namespace WMS.UI.FormReceipt
{
    public partial class FormSubmissionItem : Form
    {
        private WMSEntities wmsEntities = new WMSEntities();
        private Action CallBack;
        private int submissionTicketID;
        private Func<int> JobPersonIDGetter = null;
        private Func<int> ConfirmPersonIDGetter = null;
        public FormSubmissionItem()
        {
            InitializeComponent();
        }

        public FormSubmissionItem(int submissionTicketID)
        {
            InitializeComponent();
            this.submissionTicketID = submissionTicketID;
        }

        public void SetCallBack(Action action)
        {
            this.CallBack = action;
        }

        private void FormSubmissionItem_Load(object sender, EventArgs e)
        {
            InitComponents();
            InitPanel();
            Search();
        }

        private void InitPanel()
        {
            WMSEntities wmsEntities = new WMSEntities();
            //this.Controls.Clear();
            Utilities.CreateEditPanel(this.tableLayoutPanelProperties, ReceiptMetaData.submissionTicketItemKeyName);
            this.JobPersonIDGetter = Utilities.BindTextBoxSelect<FormSelectPerson, Person>(this, "textBoxJobPersonName", "Name");
            this.ConfirmPersonIDGetter = Utilities.BindTextBoxSelect<FormSelectPerson, Person>(this, "textBoxConfirmPersonName", "Name");
            this.reoGridControlSubmissionItems.Worksheets[0].SelectionRangeChanged += worksheet_SelectionRangeChanged;
            this.Controls.Find("comboBoxState", true)[0].Enabled = false;
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
            this.ClearTextBoxes();
            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControlSubmissionItems.Worksheets[0];
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlSubmissionItems);
            if (ids.Length == 0)
            {
                this.submissionTicketID = -1;
                return;
            }
            int id = ids[0];
            SubmissionTicketItemView submissionTicketItemView = (from s in wmsEntities.SubmissionTicketItemView
                                                                 where s.ID == id
                                                                 select s).FirstOrDefault();
            if (submissionTicketItemView == null)
            {
                MessageBox.Show("系统错误，未找到相应送检单项目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == submissionTicketItemView.ReceiptTicketItemID select si).FirstOrDefault();
            ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == submissionTicketItemView.ReceiptTicketItemID select rti).FirstOrDefault();
            if (receiptTicketItem != null)
            {
                if (receiptTicketItem.HasPutwayAmount != null && receiptTicketItem.HasPutwayAmount != 0)
                {
                    this.buttonFinished.Enabled = false;
                    this.buttonModify.Enabled = false;
                }
                else
                {
                    this.buttonFinished.Enabled = true;
                    this.buttonModify.Enabled = true;
                }
            }
            this.submissionTicketID = int.Parse(submissionTicketItemView.SubmissionTicketID.ToString());
            Utilities.CopyPropertiesToTextBoxes(submissionTicketItemView, this);
            Utilities.CopyPropertiesToComboBoxes(submissionTicketItemView, this);
            if (this.Controls.Find("textBoxReturnAmount", true)[0].Text == "")
            {
                this.Controls.Find("textBoxReturnAmount", true)[0].Text = (submissionTicketItemView.SubmissionAmount == null ? 0 : (decimal)submissionTicketItemView.SubmissionAmount).ToString();
            }
            if (this.Controls.Find("textBoxRejectAmount", true)[0].Text == "0")
            {
                this.Controls.Find("textBoxRejectAmount", true)[0].Text = "0";
            }
            //if (submissionTicketItemView.RefuseAmount == null)
            //{
            //    this.Controls.Find("textBoxRefuseAmount", true)[0].Text = "0";
            //}
            this.Controls.Find("textBoxArriveAmount", true)[0].Enabled = false;
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
            //string[] columnNames = (from kn in ReceiptMetaData.submissionTicketItemKeyName select kn.Name).ToArray();
            //初始化表格
            var worksheet = this.reoGridControlSubmissionItems.Worksheets[0];
            worksheet.SelectionMode = WorksheetSelectionMode.Row;
            for (int i = 0; i < ReceiptMetaData.submissionTicketItemKeyName.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = ReceiptMetaData.submissionTicketItemKeyName[i].Name;
                worksheet.ColumnHeaders[i].IsVisible = ReceiptMetaData.submissionTicketItemKeyName[i].Visible;
            }
            worksheet.Columns = ReceiptMetaData.submissionTicketItemKeyName.Length;
        }

        private void Search()
        {
            this.labelStatus.Text = "搜索中...";

            new Thread(new ThreadStart(() =>
            {
                var wmsEntities = new WMSEntities();
                //ReceiptTicketView[] receiptTicketViews = null;
                SubmissionTicketItemView[] submissionTicketItemView = null;
                try
                {
                    submissionTicketItemView = wmsEntities.Database.SqlQuery<SubmissionTicketItemView>("SELECT * FROM SubmissionTicketItemView WHERE SubmissionTicketID=@submissionTicketID", new SqlParameter("submissionTicketID", submissionTicketID)).ToArray();
                }
                catch
                {
                    MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    return;
                }
                this.reoGridControlSubmissionItems.Invoke(new Action(() =>
                {
                    this.labelStatus.Text = "搜索完成";
                    var worksheet = this.reoGridControlSubmissionItems.Worksheets[0];
                    worksheet.DeleteRangeData(RangePosition.EntireRange);
                    worksheet.Rows = submissionTicketItemView.Length < 10 ? 10 : submissionTicketItemView.Length;
                    int n = 0;
                    for (int i = 0; i < submissionTicketItemView.Length; i++)
                    {
                        if (submissionTicketItemView[i].State == "作废")
                        {
                            continue;
                        }
                        SubmissionTicketItemView curSubmissionTicketItemView = submissionTicketItemView[i];
                        object[] columns = Utilities.GetValuesByPropertieNames(curSubmissionTicketItemView, (from kn in ReceiptMetaData.submissionTicketItemKeyName select kn.Key).ToArray());
                        for (int j = 0; j < worksheet.Columns; j++)
                        {
                            worksheet[n, j] = columns[j];
                        }
                        n++;
                    }
                }));
                while (!this.IsHandleCreated) {; }
                this.Invoke(new Action(this.RefreshTextBoxes));
            })).Start();

        }

        private void modifyAmount(decimal oldBackAmount, decimal oldRejectAmount, decimal oldSubmissionAmount, decimal oldRefuseAmount, int submissionTicketItemID)
        {
            WMSEntities wmsEntities = new WMSEntities();
            SubmissionTicketItem submissionTicketItem = (from sti in wmsEntities.SubmissionTicketItem where sti.ID == submissionTicketItemID select sti).FirstOrDefault();
            if (submissionTicketItem == null)
            {
                return;
            }
            StockInfo stockInfo = (from si in wmsEntities.StockInfo
                                   where si.ReceiptTicketItemID == submissionTicketItem.ReceiptTicketItemID
                                   select si).FirstOrDefault();
            ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == submissionTicketItem.ReceiptTicketItemID select rti).FirstOrDefault();
            if (stockInfo == null && receiptTicketItem != null)
            {
                return;
            }
            else
            {
                stockInfo.SubmissionAmount += submissionTicketItem.SubmissionAmount - oldSubmissionAmount;
                stockInfo.ReceiptAreaAmount -= submissionTicketItem.SubmissionAmount - oldSubmissionAmount;
                stockInfo.ReceiptAreaAmount += (submissionTicketItem.ReturnAmount - submissionTicketItem.RejectAmount) - (oldBackAmount - oldRejectAmount);
                stockInfo.SubmissionAmount -= submissionTicketItem.ReturnAmount - oldBackAmount;
                stockInfo.RejectAreaAmount += submissionTicketItem.RejectAmount - oldRejectAmount;
                //receiptTicketItem.RefuseAmount += submissionTicketItem.RefuseAmount - oldRefuseAmount;

            }

            wmsEntities.SaveChanges();
        }

        private void buttonItemPass_Click(object sender, EventArgs e)
        {
            this.Controls.Find("ComboBoxState", true)[0].Text = "合格";
            this.buttonModify_Click(new object(), new EventArgs());
            /*
            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControlSubmissionItems.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new EntityCommandExecutionException();
                }
                int submissionTicketItemID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                //SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketID select st).Single();
                SubmissionTicketItem submissionTicketItem = (from sti in wmsEntities.SubmissionTicketItem
                                                             where sti.ID == submissionTicketItemID
                                                             select sti).FirstOrDefault();
                submissionTicketItem.State = "合格";
                //wmsEntities.SaveChanges();
                SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket
                                                     where st.ID == submissionTicketItem.SubmissionTicketID
                                                     select st).FirstOrDefault();
                ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket
                                               where rt.ID == submissionTicket.ReceiptTicketID
                                               select rt).FirstOrDefault();
                if (receiptTicket == null)
                {
                    MessageBox.Show("没有相应的收货单");
                    return;
                }
                new Thread(() =>
                {

                    wmsEntities.SaveChanges();
                    //int count1 = wmsEntities.Database.SqlQuery<int>("SELECT COUNT(*) FROM ReceiptTicketItem " +
                    //"WHERE ReceiptTicketID = @receiptTicketID AND State <> '过检'",
                    //new SqlParameter("receiptTicketID", submissionTicket.ReceiptTicketID)
                    //).FirstOrDefault();
                    int count1 = wmsEntities.Database.SqlQuery<int>("SELECT COUNT(*) FROM SubmissionTicketItem " +
                        "WHERE SubmissionTicketID = @submissionTicketID AND State <> '合格'",
                        new SqlParameter("submissionTicketID", submissionTicket.ID)).FirstOrDefault();
                    //int count2 = wmsEntities.Database.SqlQuery<int>("SELECT COUNT(*) FROM ReceiptTicketItem WHERE ReceiptTicketID = @receiptTicketID", new SqlParameter("receiptTicketID", submissionTicket.ReceiptTicketID)).FirstOrDefault();
                    if (count1 == 0)
                    {
                        wmsEntities.Database.ExecuteSqlCommand(
                            "UPDATE SubmissionTicket SET State='合格' " +
                            "WHERE ID=@submissionTicketID",
                            new SqlParameter("submissionTicketID", submissionTicket.ID));
                        if (receiptTicket.State == "已收货" || receiptTicket.State == "部分收货")
                        {
                        }
                        else
                        {
                            wmsEntities.Database.ExecuteSqlCommand(
                           "UPDATE ReceiptTicket SET State='过检' " +
                           "WHERE ID=@receiptTicketID",
                           new SqlParameter("receiptTicketID", submissionTicket.ReceiptTicketID));
                        }
                    }
                    else
                    {
                        wmsEntities.Database.ExecuteSqlCommand("UPDATE SubmissionTicket SET State='部分合格' WHERE ID=@submissionTicketID", new SqlParameter("submissionTicketID", submissionTicket.ID));
                        if (receiptTicket.State == "已收货" || receiptTicket.State == "部分收货")
                        {
                        }
                        else
                        {
                            wmsEntities.Database.ExecuteSqlCommand(
                                "UPDATE ReceiptTicket SET State='部分过检' " +
                                "WHERE ID=@receiptTicketID",
                                new SqlParameter("receiptTicketID", submissionTicket.ReceiptTicketID));
                        }
                    }
                }).Start();

                if (MessageBox.Show("是否同时收货？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    new Thread(() =>
                    {

                        wmsEntities.Database.ExecuteSqlCommand(
                            "UPDATE ReceiptTicketItem SET State='已收货' " +
                        "WHERE ID=@receiptTicketID",
                        new SqlParameter("receiptTicketID", submissionTicketItem.ReceiptTicketItemID));
                        int count2 = wmsEntities.Database.SqlQuery<int>("SELECT COUNT(*) FROM ReceiptTicketItem " +
                        "WHERE ReceiptTicketID = @receiptTicketID AND State <> '已收货'",
                        new SqlParameter("receiptTicketID", submissionTicket.ReceiptTicketID)
                        ).FirstOrDefault();
                        if (count2 == 0)
                        {
                            wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State='已收货' WHERE ID=@receiptTicketID", new SqlParameter("receiptTicketID", submissionTicket.ReceiptTicketID));
                        }
                        else
                        {
                            wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State='部分收货' WHERE ID=@receiptTicketID", new SqlParameter("receiptTicketID", submissionTicket.ReceiptTicketID));
                        }

                        StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == submissionTicketItem.ReceiptTicketItemID select si).FirstOrDefault();
                        if (stockInfo != null)
                        {TODO
                            if (stockInfo.SubmissionAreaAmount != null)
                            {
                                int amountSubmission = (int)stockInfo.SubmissionAreaAmount;
                                stockInfo.ReceiptAreaAmount += amountSubmission;
                                stockInfo.SubmissionAreaAmount = 0;
                            }
                        }
                        wmsEntities.SaveChanges();
                        MessageBox.Show("成功");
                        Search();
                    }).Start();
                }
                else
                {
                    new Thread(() =>
                    {
                        wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicketItem SET State='过检' WHERE ID=@receiptTicketID", new SqlParameter("receiptTicketID", submissionTicketItem.ReceiptTicketItemID));
                        wmsEntities.SaveChanges();
                        MessageBox.Show("成功");
                        this.Invoke(new Action(() =>
                        {
                            this.Search();
                        }));
                    }).Start();
                }
                new Thread(() =>
                {
                    wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicketItem SET State='过检' WHERE ID=@receiptTicketID", new SqlParameter("receiptTicketID", submissionTicketItem.ReceiptTicketItemID));
                    wmsEntities.SaveChanges();
                    MessageBox.Show("成功");
                    this.Invoke(new Action(() =>
                    {
                        this.Search();
                    }));
                }).Start();
                CallBack();

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
            this.RefreshTextBoxes();
            */
        }

        private void buttonItemNoPass_Click(object sender, EventArgs e)
        {
            this.Controls.Find("ComboBoxState", true)[0].Text = "不合格";
            this.buttonModify_Click(new object(), new EventArgs());
            /*
            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControlSubmissionItems.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new EntityCommandExecutionException();
                }
                int submissionTicketItemID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                SubmissionTicketItem submissionTicketItem = (from sti in wmsEntities.SubmissionTicketItem where sti.ID == submissionTicketItemID select sti).FirstOrDefault();
                submissionTicketItem.State = "不合格";
                SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketItem.SubmissionTicketID select st).FirstOrDefault();
                ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == submissionTicketItem.ReceiptTicketItemID select rti).FirstOrDefault();
                ReceiptTicket receiptTicket = null;
                if (receiptTicketItem != null)
                {
                    receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == receiptTicketItem.ReceiptTicketID select rt).FirstOrDefault();
                    receiptTicketItem.State = "未过检";
                }
                new Thread(() =>
                {
                    wmsEntities.SaveChanges();
                    int count = wmsEntities.Database.SqlQuery<int>(
                        "SELECT COUNT(*) FROM SubmissionTicketItem " +
                        "WHERE SubmissionTicketID = @submissionTicketID AND State != '不合格'",
                        new SqlParameter("submissionTicketID", submissionTicket.ID)).FirstOrDefault();
                    if (count == 0)
                    {
                        submissionTicket.State = "不合格";
                    }
                    else
                    {
                        submissionTicket.State = "部分合格";
                    }

                    if (receiptTicketItem != null && receiptTicket != null)
                    {
                        int count1 = wmsEntities.Database.SqlQuery<int>(
                            "SELECT COUNT(*) FROM ReceiptTicketItem " +
                            "WHERE ReceiptTicketID = @receiptTicketID AND State = '已收货'",
                            new SqlParameter("receiptTicketID", receiptTicketItem.ReceiptTicketID)).FirstOrDefault();
                        if (count1 != 0)
                        {
                            receiptTicket.State = "部分收货";
                        }
                        else
                        {
                            int count2 = wmsEntities.Database.SqlQuery<int>(
                                "SELECT COUNT(*) FROM ReceiptTicketItem " +
                                "WHERE ReceiptTicketID = @receiptTicketID AND State = '过检'",
                                new SqlParameter("receiptTicketID", receiptTicketItem.ReceiptTicketID)).FirstOrDefault();
                            if (count2 != 0)
                            {
                                receiptTicket.State = "部分过检";
                            }
                            else
                            {
                                int count3 = wmsEntities.Database.SqlQuery<int>(
                                "SELECT COUNT(*) FROM ReceiptTicketItem " +
                                "WHERE ReceiptTicketID = @receiptTicketID AND State = '待收货'",
                                new SqlParameter("receiptTicketID", receiptTicketItem.ReceiptTicketID)).FirstOrDefault();
                                if (count3 != 0)
                                {
                                    receiptTicket.State = "部分未过检";
                                }
                                else
                                {
                                    receiptTicket.State = "未过检";
                                }
                            }
                        }

                    }
                    wmsEntities.SaveChanges();
                    /*
                    if (MessageBox.Show("是否将该收货单条目设为拒收？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (receiptTicketItem != null)
                        {
                            StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == receiptTicketItem.ID select si).FirstOrDefault();
                            if (stockInfo != null)
                            {TODO
                                if (stockInfo.SubmissionAreaAmount != null)
                                {
                                    int amount = (int)stockInfo.SubmissionAreaAmount;
                                    stockInfo.ReceiptAreaAmount += amount;
                                    stockInfo.SubmissionAreaAmount = 0;
                                }
                            }
                            receiptTicketItem.State = "拒收";
                            wmsEntities.SaveChanges();
                            int count1 = wmsEntities.Database.SqlQuery<int>(
                            "SELECT COUNT(*) FROM ReceiptTicketItem " +
                            "WHERE ReceiptTicketID = @receiptTicketID AND State = '已收货'",
                            new SqlParameter("receiptTicketID", receiptTicketItem.ReceiptTicketID)).FirstOrDefault();
                            ReceiptTicket receiptTicket2 = (from rt in wmsEntities.ReceiptTicket where rt.ID == receiptTicketItem.ReceiptTicketID select rt).FirstOrDefault();
                            if (count1 != 0)
                            {
                                receiptTicket2.State = "部分收货";
                            }
                            else
                            {
                                int count2 = wmsEntities.Database.SqlQuery<int>(
                                    "SELECT COUNT(*) FROM ReceiptTicketItem " +
                                    "WHERE ReceiptTicketID = @receiptTicketID AND State <> '拒收'",
                                    new SqlParameter("receiptTicketID", receiptTicket2.ID)).FirstOrDefault();
                                if (count2 == 0)
                                {
                                    receiptTicket2.State = "拒收";
                                }
                                else
                                {
                                    receiptTicket2.State = "部分拒收";
                                }
                            }
                        }
                    }
                    wmsEntities.SaveChanges();
                    this.Search();
                    CallBack();
                }).Start();*/
        }
        /*
        int count = wmsEntities.Database.SqlQuery<int>(
            "SELECT COUNT(*) FROM ReceiptTicketItem " +
            "WHERE ReceiptTicketID = @receiptTicketID AND State <> @state", 
            new SqlParameter[] 
            { new SqlParameter("receiptTicketID", submissionTicket.ReceiptTicketID),
                new SqlParameter("state", "合格") }).FirstOrDefault();
        //int count2 = wmsEntities.Database.SqlQuery<int>("SELECT COUNT(*) FROM ReceiptTicketItem WHERE ReceiptTicketID = @receiptTicketID", new SqlParameter("receiptTicketID", submissionTicket.ReceiptTicketID)).FirstOrDefault();
        if (count != 0)
        {
            wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State='不合格' WHERE ID=@receiptTicketID", new SqlParameter("receiptTicketID", submissionTicket.ReceiptTicketID));
        }
        else if (count != 0)
        {
            wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State='部分合格' WHERE ID=@receiptTicketID", new SqlParameter("receiptTicketID", submissionTicket.ReceiptTicketID));
        }
        if (MessageBox.Show("是否将该收货单条目设为拒收？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicketItem SET State='拒收' WHERE ID=@receiptTicketID", new SqlParameter("receiptTicketID", submissionTicketItem.ReceiptTicketItemID));
            if (count != 0)
            {
                wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State='收货' WHERE ID=@receiptTicketID", new SqlParameter("receiptTicketID", submissionTicket.ReceiptTicketID));
            }
            else
            {
                wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State='部分收货' WHERE ID=@receiptTicketID", new SqlParameter("receiptTicketID", submissionTicket.ReceiptTicketID));
            }
            StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == submissionTicketItem.ReceiptTicketItemID select si).FirstOrDefault();
            if (stockInfo != null)
            {
                if (stockInfo.SubmissionAreaAmount != null)
                {
                    int amountSubmission = (int)stockInfo.SubmissionAreaAmount;
                    stockInfo.ReceiptAreaAmount = amountSubmission;
                    stockInfo.SubmissionAreaAmount = 0;
                }
            }
            new Thread(() =>
            {
                wmsEntities.SaveChanges();
                MessageBox.Show("成功");
                this.Invoke(new Action(() =>
                {
                    this.Search();
                }));
            }).Start();
        }
        else
        {
            wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicketItem SET State='送检中' WHERE ID=@receiptTicketID", new SqlParameter("receiptTicketID", submissionTicketItem.ReceiptTicketItemID));
            new Thread(() =>
            {
                wmsEntities.SaveChanges();
                MessageBox.Show("成功");
                Search();
            }).Start();
        }
        //wmsEntities.Database.ExecuteSqlCommand(String.Format("UPDATE SubmissionTicketItem SET State='不合格' WHERE ID={0}", submissionTicketItemID));
        //wmsEntities.Database.ExecuteSqlCommand(String.Format("UPDATE SubmissionTicket SET State='部分合格' WHERE ID={0}", submissionTicketID));
        //wmsEntities.Database.ExecuteSqlCommand(String.Format("UPDATE ReceiptTicket SET State='部分合格' WHERE ID={0}", submissionTicketID));
    }
    *//*
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
        this.RefreshTextBoxes();
        CallBack();*/


        private string modifyState(int submissionTicketID)
        {
            WMSEntities wmsEntities = new WMSEntities();
            SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketID select st).FirstOrDefault();
            ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == submissionTicket.ReceiptTicketID select rt).FirstOrDefault();
            if (submissionTicket != null)
            {
                SubmissionTicketItem[] submissionTicketItem = submissionTicket.SubmissionTicketItem.ToArray();
                int pass = (from sti in submissionTicketItem where sti.State == "合格" select sti).ToArray().Count();
                int noPass = (from sti in submissionTicketItem where sti.State == "不合格" select sti).ToArray().Count();
                int waitCheck = (from sti in submissionTicketItem where sti.State == "待检" select sti).ToArray().Count();
                if (waitCheck != 0)
                {
                    submissionTicket.State = "待检";
                    if (receiptTicket != null)
                    {
                        receiptTicket.State = "送检中";
                    }
                }
                else if (pass == 0)
                {
                    submissionTicket.State = "不合格";
                    if (receiptTicket != null)
                    {
                        receiptTicket.State = "未过检";
                    }
                }
                else if (noPass == 0)
                {
                    submissionTicket.State = "合格";
                    if (receiptTicket != null)
                    {
                        receiptTicket.State = "过检";
                    }
                }
                else
                {
                    submissionTicket.State = "部分合格";
                    if (receiptTicket != null)
                    {
                        receiptTicket.State = "部分过检";
                    }
                }
                wmsEntities.SaveChanges();
                return submissionTicket.State;
            }
            else
            {
                return null;
            }
        }

        private void buttonModify_Click(object sender, EventArgs e)
        {

            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlSubmissionItems);
            if (ids.Length != 1)
            {
                MessageBox.Show("请选择一项进行修改！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int id = ids[0];
            decimal oldBackAmount;
            decimal oldRejectAmount;
            decimal oldSubmissionAmount;
            decimal oldRefuseAmount;
            var submissionTicketItem = (from s in this.wmsEntities.SubmissionTicketItem where s.ID == id select s).FirstOrDefault();

            if (submissionTicketItem == null)
            {
                MessageBox.Show("未找到此送检单单条目信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            submissionTicketItem.SubmissionTicketID = this.submissionTicketID;
            oldBackAmount = submissionTicketItem.ReturnAmount == null ? 0 : (decimal)submissionTicketItem.ReturnAmount;
            oldSubmissionAmount = submissionTicketItem.SubmissionAmount == null ? 0 : (decimal)submissionTicketItem.SubmissionAmount;
            oldRejectAmount = submissionTicketItem.RejectAmount == null ? 0 : (decimal)submissionTicketItem.RejectAmount;
            oldRefuseAmount = submissionTicketItem.RefuseAmount == null ? 0 : (decimal)submissionTicketItem.RefuseAmount;
            if (Utilities.CopyTextBoxTextsToProperties(this, submissionTicketItem, ReceiptMetaData.submissionTicketItemKeyName, out string errorMessage) == false)
            {
                MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Utilities.CopyComboBoxsToProperties(this, submissionTicketItem, ReceiptMetaData.submissionTicketItemKeyName);
            if (this.JobPersonIDGetter() != -1)
            {
                submissionTicketItem.JobPersonID = JobPersonIDGetter();
            }
            if (this.ConfirmPersonIDGetter() != -1)
            {
                submissionTicketItem.ConfirmPersonID = ConfirmPersonIDGetter();
            }
            if (submissionTicketItem.SubmissionAmount < submissionTicketItem.RejectAmount)
            {
                MessageBox.Show("不合格数量不能大于送检数量", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                return;
            }
            if (submissionTicketItem.SubmissionAmount < submissionTicketItem.ReturnAmount)
            {
                MessageBox.Show("返回数量不能大于送检数量", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                return;
            }
            /*
            if(submissionTicketItem.SubmissionAmount < submissionTicketItem.RejectAmount + submissionTicketItem.ReturnAmount)
            {
                MessageBox.Show("返回数量为返回发货区数量，不合格品数量会被记入不良品区");
                return;
            }*/
            ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == submissionTicketItem.ReceiptTicketItemID select rti).FirstOrDefault();
            string sql = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == submissionTicketItem.ReceiptTicketItemID select rti).ToString();
            if (receiptTicketItem != null)
            {
                if (submissionTicketItem.State == "合格")
                {
                    receiptTicketItem.State = "过检";
                }
                else if (submissionTicketItem.State == "不合格")
                {
                    receiptTicketItem.State = "未过检";
                }
                else
                {
                    receiptTicketItem.State = "送检中";
                }
            }
            if (submissionTicketItem.RejectAmount == null)
            {
                submissionTicketItem.RejectAmount = 0;
            }
            if (submissionTicketItem.ReturnAmount == null)
            {
                submissionTicketItem.ReturnAmount = submissionTicketItem.SubmissionAmount;
            }
            /*
            StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ID == receiptTicketItem.ID select si).FirstOrDefault();
            if (stockInfo != null)
            {
                stockInfo.ReceiptAreaAmount += submissionTicketItem.ReturnAmount - oldBackAmount;
                stockInfo.SubmissionAmount -= submissionTicketItem.ReturnAmount - oldBackAmount;
                stockInfo.RejectAreaAmount += submissionTicketItem.RejectAmount - oldRejectAmount;
            }*/
            new Thread(() =>
            {
                this.wmsEntities.SaveChanges();
                this.modifyState(this.submissionTicketID);
                this.modifyAmount(oldBackAmount, oldRejectAmount, oldSubmissionAmount, oldRefuseAmount, submissionTicketItem.ID);
                string state = this.modifyState(submissionTicketID);
                this.Invoke(new Action(() =>
                {
                    this.Search();
                    CallBack();
                    this.RefreshTextBoxes();
                }));

                /*
                if (state == "合格")
                {
                    if (MessageBox.Show("是否同时收货（移货到溢货区）", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketID select st).FirstOrDefault();
                        if (submissionTicket == null)
                        {
                            MessageBox.Show("收货失败，没有找到此送检单");
                            return;
                        }
                        ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == submissionTicket.ReceiptTicketID select rt).FirstOrDefault();
                        if (receiptTicket == null)
                        {
                            MessageBox.Show("收货失败，没有找到此收货单");
                            return;
                        }
                        ReceiptTicketItem[] receiptTicketItems = receiptTicket.ReceiptTicketItem.ToArray();
                        //List<StockInfo> stockInfos = null;
                        foreach(ReceiptTicketItem rti in receiptTicketItems)
                        {
                            StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == rti.ID select si).FirstOrDefault();
                            if (stockInfo != null)
                            {
                                stockInfo.OverflowAreaAmount = stockInfo.ReceiptAreaAmount - submissionTicketItem.RejectAmount;
                                stockInfo.ReceiptAreaAmount -= stockInfo.ReceiptAreaAmount;
                            }
                            rti.State = "已收货";
                        }
                        receiptTicket.State = "已收货";
                        wmsEntities.SaveChanges();
                    }
                }
                else if (state == "不合格")
                {
                    if (MessageBox.Show("是否拒收（更改状态为拒收，货物在收货区停滞）", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketID select st).FirstOrDefault();
                        if (submissionTicket == null)
                        {
                            MessageBox.Show("拒收失败，没有找到此送检单");
                            return;
                        }
                        ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == submissionTicket.ReceiptTicketID select rt).FirstOrDefault();
                        if (receiptTicket == null)
                        {
                            MessageBox.Show("拒收失败，没有找到此收货单");
                            return;
                        }
                        receiptTicket.State = "拒收";
                        foreach(ReceiptTicketItem rti in receiptTicket.ReceiptTicketItem)
                        {
                            rti.State = "拒收";
                        }
                        wmsEntities.SaveChanges();
                    }
                }
                else if (state == "部分合格")
                {
                    DialogResult dialogResult = MessageBox.Show("收货或者拒收？", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketID select st).FirstOrDefault();
                        if (submissionTicket == null)
                        {
                            MessageBox.Show("收货失败，没有找到此送检单");
                            return;
                        }
                        ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == submissionTicket.ReceiptTicketID select rt).FirstOrDefault();
                        if (receiptTicket == null)
                        {
                            MessageBox.Show("收货失败，没有找到此收货单");
                            return;
                        }
                        ReceiptTicketItem[] receiptTicketItems = receiptTicket.ReceiptTicketItem.ToArray();
                        //List<StockInfo> stockInfos = null;
                        foreach (ReceiptTicketItem rti in receiptTicketItems)
                        {
                            StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == rti.ID select si).FirstOrDefault();
                            if (stockInfo != null)
                            {
                                stockInfo.OverflowAreaAmount = stockInfo.ReceiptAreaAmount;
                                stockInfo.ReceiptAreaAmount -= stockInfo.ReceiptAreaAmount;
                            }
                            rti.State = "已收货";
                        }
                        receiptTicket.State = "已收货";
                        wmsEntities.SaveChanges();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketID select st).FirstOrDefault();
                        if (submissionTicket == null)
                        {
                            MessageBox.Show("拒收失败，没有找到此送检单");
                            return;
                        }
                        ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == submissionTicket.ReceiptTicketID select rt).FirstOrDefault();
                        if (receiptTicket == null)
                        {
                            MessageBox.Show("拒收失败，没有找到此收货单");
                            return;
                        }
                        receiptTicket.State = "拒收";
                        foreach (ReceiptTicketItem rti in receiptTicket.ReceiptTicketItem)
                        {
                            rti.State = "拒收";
                        }
                        wmsEntities.SaveChanges();
                    }
                }*/
                this.Invoke(new Action(() =>
                {
                    CallBack();
                    this.RefreshTextBoxes();
                }));
                MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }).Start();


        }

        private void buttonDeleteItem_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlSubmissionItems.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new EntityCommandExecutionException();
                }
                int submissionTicketItemID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                wmsEntities.Database.ExecuteSqlCommand("UPDATE SubmissionTicketItem SET State='作废' WHERE ID=@submissionTicketItemID", new SqlParameter("submissionTicketItemID", submissionTicketItemID));
                wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State='部分送检' WHERE ID=@submissionTicketItemID", new SqlParameter("submissionTicketItemID", submissionTicketID));
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
            this.RefreshTextBoxes();
        }

        private void buttonItemPass_MouseEnter(object sender, EventArgs e)
        {
            //buttonItemPass.BackgroundImage = WMS.UI.Properties.Resources.bottonW_s;
        }

        private void buttonItemPass_MouseLeave(object sender, EventArgs e)
        {
            //buttonItemPass.BackgroundImage = WMS.UI.Properties.Resources.bottonW_q;
        }

        private void buttonItemPass_MouseDown(object sender, MouseEventArgs e)
        {
            //buttonItemPass.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }



        private void buttonItemNoPass_MouseEnter(object sender, EventArgs e)
        {
            //buttonItemNoPass.BackgroundImage = WMS.UI.Properties.Resources.bottonW_s;
        }

        private void buttonItemNoPass_MouseLeave(object sender, EventArgs e)
        {
            //buttonItemNoPass.BackgroundImage = WMS.UI.Properties.Resources.bottonW_q;
        }

        private void buttonItemNoPass_MouseDown(object sender, MouseEventArgs e)
        {
            //buttonItemNoPass.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonAllPass_Click(object sender, EventArgs e)
        {
            
            WMSEntities wmsEntities = new WMSEntities();
            SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == this.submissionTicketID select st).FirstOrDefault();
            if (submissionTicket == null)
            {
                MessageBox.Show("该送检单已被删除，请刷新后查看！");
                return;
            }
            SubmissionTicketItem[] submissionTicketItems = submissionTicket.SubmissionTicketItem.ToArray();
            foreach (SubmissionTicketItem sti in submissionTicketItems)
            {
                if (sti.State == "合格")
                {
                    continue;
                }
               // Console.WriteLine(sti.State);
                sti.State = "合格";
                sti.ReturnAmount = sti.SubmissionAmount;
                sti.RejectAmount = 0;
                ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == sti.ReceiptTicketItemID select rti).FirstOrDefault();
                if (receiptTicketItem != null)
                {
                    if (receiptTicketItem.State == "已收货")
                    {
                        continue;
                    }
                    StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == sti.ReceiptTicketItemID select si).FirstOrDefault();
                    if (stockInfo != null)
                    {
                       
                        stockInfo.OverflowAreaAmount = receiptTicketItem.ReceiviptAmount + sti.ReturnAmount - sti.SubmissionAmount;
                        stockInfo.ReceiptAreaAmount = 0;
                        stockInfo.RejectAreaAmount = 0;
                        //stockInfo.ReceiptAreaAmount = 0;
                        stockInfo.SubmissionAmount = sti.SubmissionAmount - sti.ReturnAmount;
                        //stockInfo.RejectAreaAmount = receiptTicketItem.DisqualifiedAmount;
                    }
                    if (receiptTicketItem.RefuseAmount == 0)
                    {
                        receiptTicketItem.State = "已收货";
                    }
                    else
                    {
                        receiptTicketItem.State = "部分收货";
                    }
                }
            }
            submissionTicket.State = "合格";
            ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == submissionTicket.ReceiptTicketID select rt).FirstOrDefault();
            if (receiptTicket != null)
            {
                receiptTicket.State = "已收货";
            }
            new Thread(() =>
            {
                wmsEntities.SaveChanges();
                if (receiptTicket != null)
                {
                    int receipt = (from rti in receiptTicket.ReceiptTicketItem where rti.State == "已收货" select rti).ToArray().Length;
                    int reject = (from rti in receiptTicket.ReceiptTicketItem where rti.State == "拒收" select rti).ToArray().Length;
                    if (receiptTicket.ReceiptTicketItem.Count == receipt)
                    {
                        receiptTicket.State = "已收货";
                    }
                    else if (receiptTicket.ReceiptTicketItem.Count == reject)
                    {
                        receiptTicket.State = "拒收";
                    }
                    else
                    {
                        receiptTicket.State = "部分收货";
                    }
                }
                wmsEntities.SaveChanges();
                this.Invoke(new Action(() =>
                {
                    this.Search();
                    CallBack();
                }));
                MessageBox.Show("成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }).Start();
        }

        private void buttonAllNoPass_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketID select st).FirstOrDefault();
            if (submissionTicket == null)
            {
                MessageBox.Show("该送检单已被删除，请刷新后查看！");
                return;
            }
            SubmissionTicketItem[] submissionTicketItems = submissionTicket.SubmissionTicketItem.ToArray();
            foreach (SubmissionTicketItem sti in submissionTicketItems)
            {
                //sti.ArriveAmount = sti.SubmissionAmount;
                if (sti.RejectAmount == null)
                {
                    sti.RejectAmount = 0;
                }
                if (sti.ReturnAmount == null)
                {
                    sti.ReturnAmount = sti.SubmissionAmount;
                }
                sti.State = "不合格";
                sti.ReceiptTicketItem.State = "未过检";
                StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == sti.ReceiptTicketItemID select si).FirstOrDefault();
                if (stockInfo != null)
                {
                    stockInfo.ReceiptAreaAmount += stockInfo.SubmissionAmount;
                    stockInfo.SubmissionAmount -= stockInfo.SubmissionAmount;
                }
            }
            submissionTicket.State = "不合格";
            ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == submissionTicket.ReceiptTicketID select rt).FirstOrDefault();
            if (receiptTicket != null)
            {
                receiptTicket.State = "未过检";
            }
            new Thread(() =>
            {
                wmsEntities.SaveChanges();

                MessageBox.Show("成功");
                this.Invoke(new Action(() =>
                {
                    this.Search();
                    CallBack();
                }));
            }).Start();
        }

        private void buttonFinished_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlSubmissionItems);
            if (ids.Length != 1)
            {
                MessageBox.Show("请选择一项进行修改！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int id = ids[0];
            SubmissionTicketItem submissionTicketItem = (from sti in wmsEntities.SubmissionTicketItem where sti.ID == id select sti).FirstOrDefault();
            if (submissionTicketItem == null)
            {
                MessageBox.Show("没有找到该送检单，可能已被删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string errorInfo;
            if (Utilities.CopyTextBoxTextsToProperties(this, submissionTicketItem, ReceiptMetaData.submissionTicketItemKeyName, out errorInfo) == false)
            {
                MessageBox.Show(errorInfo, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (submissionTicketItem.ReturnAmount == null)
            {
                MessageBox.Show("送检返回数量不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (submissionTicketItem.RejectAmount == null)
            {
                MessageBox.Show("不合格数量不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (submissionTicketItem.SubmissionAmount < submissionTicketItem.ReturnAmount)
            {
                MessageBox.Show("返回数量不能大于送检数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (submissionTicketItem.SubmissionAmount < submissionTicketItem.RejectAmount)
            {
                MessageBox.Show("不合格数量不能大于送检数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            

            ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == submissionTicketItem.ReceiptTicketItemID select rti).FirstOrDefault();
            if (submissionTicketItem.RejectAmount == 0)
            {
                submissionTicketItem.State = "合格";
                if (receiptTicketItem != null)
                {
                    StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == submissionTicketItem.ReceiptTicketItemID select si).FirstOrDefault();
                    if (stockInfo != null)
                    {
                        /*
                        stockInfo.ReceiptAreaAmount += submissionTicketItem.ReturnAmount + stockInfo.RejectAreaAmount - receiptTicketItem.DisqualifiedAmount;
                        stockInfo.SubmissionAmount = submissionTicketItem.SubmissionAmount - submissionTicketItem.ReturnAmount;
                        stockInfo.OverflowAreaAmount += stockInfo.ReceiptAreaAmount;
                        stockInfo.ReceiptAreaAmount -= stockInfo.ReceiptAreaAmount;
                        stockInfo.RejectAreaAmount = receiptTicketItem.DisqualifiedAmount + submissionTicketItem.RejectAmount;*/
                        stockInfo.OverflowAreaAmount = receiptTicketItem.ReceiviptAmount + submissionTicketItem.ReturnAmount - submissionTicketItem.SubmissionAmount;
                        stockInfo.ReceiptAreaAmount = 0;
                        stockInfo.RejectAreaAmount = 0;
                        stockInfo.SubmissionAmount = submissionTicketItem.SubmissionAmount - submissionTicketItem.ReturnAmount;
                        //stockInfo.RejectAreaAmount = receiptTicketItem.DisqualifiedAmount;
                    }
                    if (receiptTicketItem.RefuseAmount == 0)
                    {
                        receiptTicketItem.State = "已收货";
                    }
                    else
                    {
                        receiptTicketItem.State = "部分收货";
                    }
                }
            }
            else if (submissionTicketItem.RejectAmount == submissionTicketItem.SubmissionAmount)
            {
                submissionTicketItem.State = "不合格";
                if (receiptTicketItem != null)
                {
                    StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == submissionTicketItem.ReceiptTicketItemID select si).FirstOrDefault();
                    if (stockInfo != null)
                    {
                        /*
                        stockInfo.ReceiptAreaAmount += submissionTicketItem.ReturnAmount + stockInfo.RejectAreaAmount - receiptTicketItem.DisqualifiedAmount;
                        stockInfo.SubmissionAmount = submissionTicketItem.SubmissionAmount - submissionTicketItem.ReturnAmount;
                        stockInfo.RejectAreaAmount = stockInfo.ReceiptAreaAmount + 
                        stockInfo.ReceiptAreaAmount -= stockInfo.ReceiptAreaAmount;*/
                        stockInfo.OverflowAreaAmount = 0;
                        stockInfo.ReceiptAreaAmount = 0;
                        stockInfo.SubmissionAmount = submissionTicketItem.SubmissionAmount - submissionTicketItem.ReturnAmount;
                        stockInfo.RejectAreaAmount = receiptTicketItem.ReceiviptAmount - submissionTicketItem.SubmissionAmount + submissionTicketItem.ReturnAmount;
                    }
                    receiptTicketItem.State = "拒收";
                }
            }
            else
            {
                submissionTicketItem.State = "部分合格";
                if (receiptTicketItem != null)
                {
                    StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == submissionTicketItem.ReceiptTicketItemID select si).FirstOrDefault();
                    if (stockInfo != null)
                    {
                        /*
                        stockInfo.ReceiptAreaAmount += submissionTicketItem.ReturnAmount + stockInfo.RejectAreaAmount - receiptTicketItem.DisqualifiedAmount;
                        stockInfo.SubmissionAmount = submissionTicketItem.SubmissionAmount - submissionTicketItem.ReturnAmount;
                        stockInfo.RejectAreaAmount += stockInfo.ReceiptAreaAmount; 
                        stockInfo.ReceiptAreaAmount -= stockInfo.ReceiptAreaAmount;*/
                        stockInfo.OverflowAreaAmount = 0;
                        stockInfo.ReceiptAreaAmount = 0;
                        stockInfo.SubmissionAmount = submissionTicketItem.SubmissionAmount - submissionTicketItem.ReturnAmount;
                        stockInfo.RejectAreaAmount = receiptTicketItem.ReceiviptAmount - submissionTicketItem.SubmissionAmount + submissionTicketItem.ReturnAmount;
                    }
                    receiptTicketItem.State = "拒收";
                }
            }

            new Thread(()=> 
            {
                wmsEntities.SaveChanges();
                SubmissionTicket submissionTicket = submissionTicketItem.SubmissionTicket;
                ReceiptTicket receiptTicket = receiptTicketItem.ReceiptTicket;
                if (submissionTicket != null)
                {
                    int allPass = (from sti in submissionTicket.SubmissionTicketItem where sti.State == "合格" select sti).ToArray().Length;
                    int partPass = (from sti in submissionTicket.SubmissionTicketItem where sti.State == "部分合格" select sti).ToArray().Length;
                    int noPass = (from sti in submissionTicket.SubmissionTicketItem where sti.State == "不合格" select sti).ToArray().Length;

                    if (submissionTicket.SubmissionTicketItem.Count == allPass)
                    {
                        submissionTicket.State = "合格";
                        
                    }
                    else if (submissionTicket.SubmissionTicketItem.Count == noPass)
                    {
                        submissionTicket.State = "不合格";
                    }
                    else
                    {
                        submissionTicket.State = "部分合格";
                    }
                }
                if (receiptTicket != null)
                {
                    int receipt = (from rti in receiptTicket.ReceiptTicketItem where rti.State == "已收货" select rti).ToArray().Length;
                    int reject = (from rti in receiptTicket.ReceiptTicketItem where rti.State == "拒收" select rti).ToArray().Length;
                    if (receiptTicket.ReceiptTicketItem.Count == receipt)
                    {
                        receiptTicket.State = "已收货";
                    }
                    else if (receiptTicket.ReceiptTicketItem.Count == reject)
                    {
                        receiptTicket.State = "拒收";
                    }
                    else
                    {
                        receiptTicket.State = "部分收货";
                    }

                }

                wmsEntities.SaveChanges();
                MessageBox.Show("送检单状态检测完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (this.IsDisposed) return;
                this.Invoke(new Action(() => 
                {
                    this.Search();
                    CallBack();
                }));
            }).Start();
        }

        private void buttonFinished_MouseEnter(object sender, EventArgs e)
        {
            buttonFinished.BackgroundImage = WMS.UI.Properties.Resources.bottonB1_s;
        }

        private void buttonFinished_MouseLeave(object sender, EventArgs e)
        {
            buttonFinished.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        }

        private void buttonFinished_MouseDown(object sender, MouseEventArgs e)
        {
            buttonFinished.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }


        private void buttonAllPass_MouseDown(object sender, MouseEventArgs e)
        {
            buttonAllPass.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private void buttonAllPass_MouseEnter(object sender, EventArgs e)
        {
            buttonAllPass.BackgroundImage = WMS.UI.Properties.Resources.bottonB1_s;
        }

        private void buttonAllPass_MouseLeave(object sender, EventArgs e)
        {
            buttonAllPass.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        }



        //private void buttonAllNoPass_MouseDown(object sender, MouseEventArgs e)
        //{
        //    buttonAllNoPass.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        //}

        //private void buttonAllNoPass_MouseEnter(object sender, EventArgs e)
        //{
        //    buttonAllNoPass.BackgroundImage = WMS.UI.Properties.Resources.bottonB1_s;
        //}

        //private void buttonAllNoPass_MouseLeave(object sender, EventArgs e)
        //{
        //    buttonAllNoPass.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        //}
    }
}

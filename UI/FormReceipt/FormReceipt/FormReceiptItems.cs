﻿using System;
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
        private int receiptTicketItemID;
        private FormMode formMode;
        private int receiptTicketID;
        private int componentID;
        Action callBack = null;
        const string WAIT_CHECK = "待检";
        const string CHECK = "送检中";
        SubmissionTicket submissionTicket;
        public FormReceiptItems()
        {
            InitializeComponent();
        }

        public void SetCallback(Action action)
        {
            callBack = action;
        }

        public FormReceiptItems(FormMode formMode, int receiptTicketID)
        {
            InitializeComponent();
            this.formMode = formMode;
            this.receiptTicketID = receiptTicketID;
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
            TextBox textBoxComponentNo = (TextBox)this.Controls.Find("textBoxComponentNo", true)[0];
            textBoxComponentNo.Click += TextBoxComponentNo_Click;
            WMSEntities wmsEntities = new WMSEntities();
            ReceiptTicketView receiptTicketView = (from rt in wmsEntities.ReceiptTicketView where rt.ID == receiptTicketID select rt).FirstOrDefault();
            if (receiptTicketView == null)
            {
                MessageBox.Show("找不到该收货单!");
                return;
            }
            this.Controls.Find("textBoxState", true)[0].Text = receiptTicketView.State;
            Search();
        }

        private void TextBoxComponentNo_Click(object sender, EventArgs e)
        {
            FormSearch formSearch = new FormSearch();
            formSearch.SetSelectFinishCallback(new Action<int>((id) => 
            {
                WMSEntities wmsEntities = new WMSEntities();
                this.componentID = id;
                WMS.DataAccess.Component component = (from c in wmsEntities.Component where c.ID == id select c).FirstOrDefault();
                if (component == null)
                {
                    MessageBox.Show("没有找到该零件");
                }
                else
                {
                    this.Controls.Find("textBoxComponentName", true)[0].Text = component.Name;
                    this.Controls.Find("textBoxComponentNo", true)[0].Text = component.No;
                }
            }));
            formSearch.Show();
        }

        private void InitPanel()
        {
            WMSEntities wmsEntities = new WMSEntities();
            //this.Controls.Clear();
            Utilities.CreateEditPanel(this.tableLayoutPanelProperties, ReceiptMetaData.itemsKeyName);
            //this.RefreshTextBoxes();
            this.reoGridControlReceiptItems.Worksheets[0].SelectionRangeChanged += worksheet_SelectionRangeChanged;
            
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
            var worksheet = this.reoGridControlReceiptItems.Worksheets[0];
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlReceiptItems);
            if (ids.Length == 0)
            {
                this.receiptTicketItemID = -1;
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
                return;
            }
            this.receiptTicketItemID = int.Parse(receiptTicketItemView.ID.ToString());
            if (receiptTicketItemView.ComponentID != null)
            {
                this.componentID = (int)receiptTicketItemView.ComponentID;
            }
            Utilities.CopyPropertiesToTextBoxes(receiptTicketItemView, this);
            Utilities.CopyPropertiesToComboBoxes(receiptTicketItemView, this);
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


        private void Search()
        {
            this.labelStatus.Text = "搜索中...";

            new Thread(new ThreadStart(() =>
            {
                var wmsEntities = new WMSEntities();
                ReceiptTicketItemView[] receiptTicketItemViews = null;
                try
                {
                    receiptTicketItemViews = wmsEntities.Database.SqlQuery<ReceiptTicketItemView>("SELECT * FROM ReceiptTicketItemView WHERE ReceiptTicketID = @receiptTicketID ORDER BY ID DESC", new SqlParameter("receiptTicketID" ,this.receiptTicketID)).ToArray();
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
                                worksheet[i, j] = columns[j].ToString();
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
                    this.reoGridControlReceiptItems.Worksheets[0][0, m] = "无查询结果";
                }


            })).Start();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            ReceiptTicketItem receiptTicketItem = new ReceiptTicketItem();

            string errorInfo;
            if (Utilities.CopyTextBoxTextsToProperties(this, receiptTicketItem,ReceiptMetaData.itemsKeyName, out errorInfo) == false)
            {
                MessageBox.Show(errorInfo);
            }
            else
            {
                wmsEntities.ReceiptTicketItem.Add(receiptTicketItem);
                new Thread(() =>
                {
                    try
                    {
                        ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == this.receiptTicketID select rt).FirstOrDefault();
                        if (receiptTicket == null)
                        {
                            MessageBox.Show("该收货单不存在");
                            return;
                        }
                        receiptTicketItem.ComponentID = this.componentID;
                        receiptTicketItem.ReceiptTicketID = this.receiptTicketID;
                        
                        wmsEntities.SaveChanges();

                        
                        StockInfo stockInfo = new StockInfo();
                        stockInfo.ProjectID = receiptTicket.ProjectID;
                        stockInfo.WarehouseID = receiptTicket.Warehouse;
                        stockInfo.ReceiptTicketItemID = receiptTicketItem.ID;
                        if (receiptTicketItem.State == "待送检" || receiptTicketItem.State == "拒收")
                        {
                            stockInfo.OverflowAreaAmount = 0;
                            stockInfo.ShipmentAreaAmount = 0;
                            stockInfo.SubmissionAmount = 0;
                            stockInfo.RejectAreaAmount = receiptTicketItem.DisqualifiedAmount == null ? 0 : receiptTicketItem.DisqualifiedAmount;
                            stockInfo.SubmissionAmount = 0;
                            stockInfo.ReceiptAreaAmount = 0;
                            if (receiptTicketItem.ReceiviptAmount != null)
                            {
                                stockInfo.ReceiptAreaAmount = receiptTicketItem.ReceiviptAmount;
                            }
                        }
                        else if (receiptTicketItem.State == "已收货")
                        {
                            stockInfo.OverflowAreaAmount = 0;
                            stockInfo.ShipmentAreaAmount = 0;
                            stockInfo.SubmissionAmount = 0;
                            stockInfo.RejectAreaAmount = receiptTicketItem.DisqualifiedAmount == null ? 0 : receiptTicketItem.DisqualifiedAmount;
                            stockInfo.SubmissionAmount = 0;
                            stockInfo.ReceiptAreaAmount = 0;
                            if (receiptTicketItem.ReceiviptAmount != null)
                            {
                                stockInfo.OverflowAreaAmount = receiptTicketItem.ReceiviptAmount;
                            }
                        }
                        
                        wmsEntities.StockInfo.Add(stockInfo);

                        wmsEntities.SaveChanges();
                        this.Search();
                    }
                    catch
                    {
                        MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        return;
                    }
                }).Start();
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
                    throw new EntityCommandExecutionException();
                }
                int receiptItemID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                int oldRejectAreaAmount;
                int oldReceiptAreaAmount;
                ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == receiptItemID select rti).FirstOrDefault();
                oldReceiptAreaAmount = receiptTicketItem.ReceiviptAmount == null ? 0 : (int)receiptTicketItem.ReceiviptAmount;
                oldRejectAreaAmount = receiptTicketItem.DisqualifiedAmount == null ? 0 : (int)receiptTicketItem.DisqualifiedAmount;
                string errorInfo;
                if(Utilities.CopyTextBoxTextsToProperties(this, receiptTicketItem, ReceiptMetaData.itemsKeyName, out errorInfo) == false)
                {
                    MessageBox.Show(errorInfo);
                    return;
                }
                else
                {
                    new Thread(() =>
                    {
                        try
                        {
                            receiptTicketItem.ComponentID = this.componentID;
                            StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == receiptTicketItem.ID select si).FirstOrDefault();
                            if (stockInfo == null)
                            {
                                //MessageBox.Show("该库存信息已被删除");
                            }
                            else
                            {
                                if (receiptTicketItem.State == "待送检" || receiptTicketItem.State == "拒收")
                                {
                                    stockInfo.ReceiptAreaAmount = receiptTicketItem.ReceiviptAmount;
                                    stockInfo.RejectAreaAmount = receiptTicketItem.DisqualifiedAmount;
                                }
                                else if (receiptTicketItem.State == "已收货")
                                {
                                    SubmissionTicketItem submissionTicketItem = (from sti in wmsEntities.SubmissionTicketItem where sti.ReceiptTicketItemID == receiptTicketItem.ID select sti).FirstOrDefault();
                                    if (submissionTicketItem != null)
                                    {
                                        stockInfo.OverflowAreaAmount = receiptTicketItem.ReceiviptAmount - submissionTicketItem.ReturnAmount;
                                        stockInfo.RejectAreaAmount = receiptTicketItem.DisqualifiedAmount + submissionTicketItem.RejectAmount;
                                    }
                                    else
                                    {
                                        stockInfo.OverflowAreaAmount = receiptTicketItem.ReceiviptAmount;
                                        stockInfo.RejectAreaAmount = receiptTicketItem.DisqualifiedAmount;
                                    }
                                }
                                else
                                {
                                    receiptTicketItem.ReceiviptAmount = oldReceiptAreaAmount;
                                    receiptTicketItem.DisqualifiedAmount = oldRejectAreaAmount;
                                }
                            }
                            wmsEntities.SaveChanges();
                            
                            this.Search();
                        }
                        catch
                        {
                            MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                            return;
                        }
                    }).Start();
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
        }

        private void buttonItemCheck_Click(object sender, EventArgs e)
        {
            FormReceiptArrivalCheck formReceiptArrivalCheck = new FormReceiptArrivalCheck(receiptTicketID, 1,AllOrPartial.PARTIAL);
            formReceiptArrivalCheck.SetFinishedAction(() =>
            {
                this.Close();
                FormReceiptItems formReceiptItems2 = new FormReceiptItems(FormMode.ALTER, this.receiptTicketID);
                formReceiptItems2.Show();
            });
            formReceiptArrivalCheck.Show();
        }
        private void receiptItemToSubmissionItem(ReceiptTicketItem receiptTicketItem)
        {
            if (submissionTicket == null)
            {
                FormReceiptArrivalCheck formReceiptArrivalCheck = new FormReceiptArrivalCheck(this.receiptTicketID, 1,AllOrPartial.PARTIAL);
                formReceiptArrivalCheck.Show();
                this.submissionTicket = formReceiptArrivalCheck.submissionTicket;
                //submissionTicket = new SubmissionTicket();
            }
            SubmissionTicketItem submissionTicketItem = new SubmissionTicketItem();
            //submissionTicketItem.
            FormSubmissionTicketItemModify formSubmissionTicketItemModify = new FormSubmissionTicketItemModify(submissionTicket.ID, receiptTicketItem);
            formSubmissionTicketItemModify.Show();
        }

        private void buttonAddItem_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControlReceiptItems.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new EntityCommandExecutionException();
                }
                int receiptItemID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                ReceiptTicketItem receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ID == receiptItemID select rti).Single();
                if (receiptTicketItem.State == "送检中")
                {
                    MessageBox.Show("该条目已送检");
                }
                else
                { 
                    SubmissionTicketItem submissionTicketItem = ReceiptUtilities.ReceiptTicketItemToSubmissionTicketItem(receiptTicketItem, submissionTicket.ID);
                    wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicketItem SET State = '送检中' WHERE ID = @receiptTicketItemID", new SqlParameter("receiptTicketItemID", receiptTicketItem.ID));
                    wmsEntities.SubmissionTicketItem.Add(submissionTicketItem);
                    wmsEntities.SaveChanges();
                    MessageBox.Show("成功");
                }
            }
            catch(EntityCommandExecutionException)
            {
                MessageBox.Show("请选择一项送检", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
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
                    throw new EntityCommandExecutionException();
                }
                int receiptItemID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                new Thread(() => 
                {
                    try
                    {
                        wmsEntities.Database.ExecuteSqlCommand("DELETE FROM StockInfo WHERE ReceiptTicketItemID = @receiptTicketItemID", new SqlParameter("receiptTicketItemID", receiptItemID));
                        wmsEntities.Database.ExecuteSqlCommand("DELETE FROM ReceiptTicketItem WHERE ID = @receiptTicketItemID", new SqlParameter("receiptTicketItemID", receiptItemID));
                    }
                    catch
                    {
                        MessageBox.Show("该收货单零件已送检或收货，无法删除");
                        
                    }
                        this.Search();
                }).Start();
            }
            catch (EntityCommandExecutionException)
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
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

    }
}
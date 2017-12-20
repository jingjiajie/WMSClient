﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.DataAccess;
using System.Data.SqlClient;
using WMS.UI.FormReceipt;
using System.Threading;
using unvell.ReoGrid;

namespace WMS.UI.FormReceipt
{
    public partial class FormReceiptArrivalCheck : Form
    {
        public SubmissionTicket submissionTicket;
        WMSEntities wmsEntities = new WMSEntities();
        private int submissionTicketID;
        private int receiptTicketID;
        private FormMode formMode;
        Action finishedAction = null;
        private AllOrPartial allOrPartial;
        public FormReceiptArrivalCheck()
        {
            InitializeComponent();
        }
        public FormReceiptArrivalCheck(int receiptTicketID, AllOrPartial allOrPartial)
        {
            InitializeComponent();
            this.receiptTicketID = receiptTicketID;
            this.allOrPartial = allOrPartial;
            this.formMode = FormMode.ADD;
        }

        public FormReceiptArrivalCheck(int receiptTicketID)
        {
            InitializeComponent();
            this.receiptTicketID = receiptTicketID;
            //this.submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketID && st.State != "作废" select st).Single();
            this.formMode = FormMode.ALTER;
        }

        public void SetFinishedAction(Action action)
        {
            this.finishedAction = action;
        }

        private void FormReceiptArrivalCheck_Load(object sender, EventArgs e)
        {
            InitComponents();
            InitPanel();

            Search();
        }


        private void InitPanel()
        {
            WMSEntities wmsEntities = new WMSEntities();
            //this.Controls.Clear();
            Utilities.CreateEditPanel(this.tableLayoutPanelProperties, ReceiptMetaData.submissionTicketKeyName);
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
            this.ClearTextBoxes();
            var worksheet = this.reoGridControlPutaway.Worksheets[0];
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlPutaway);
            if (ids.Length == 0)
            {
                this.submissionTicketID = -1;
                return;
            }
            int id = ids[0];
            
            SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == id select st).FirstOrDefault();

            if (submissionTicket == null)
            {
                MessageBox.Show("系统错误，未找到相应上架单项目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.submissionTicketID = int.Parse(submissionTicket.ID.ToString());
           
            Utilities.CopyPropertiesToTextBoxes(submissionTicket, this);
            //Utilities.CopyPropertiesToComboBoxes(shipmentTicketItemView, this);
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
            //this.Controls.Find("textBoxID", true)[0].Text = "0";
            TextBox textBoxState = (TextBox)this.Controls.Find("textBoxState", true)[0];
            if (textBoxState.Text == null)
            {
                textBoxState.Text = "待检";
            }
            TextBox textBoxReceiptTicketID = (TextBox)this.Controls.Find("textBoxReceiptTicketID", true)[0];
            textBoxReceiptTicketID.Text = this.receiptTicketID.ToString();
        }

        private void InitComponents()
        {
            //初始化
            string[] columnNames = (from kn in ReceiptMetaData.submissionTicketKeyName select kn.Name).ToArray();
            //初始化表格
            var worksheet = this.reoGridControlPutaway.Worksheets[0];
            worksheet.SelectionMode = WorksheetSelectionMode.Row;
            for (int i = 0; i < columnNames.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = columnNames[i];
                worksheet.ColumnHeaders[i].IsVisible = ReceiptMetaData.submissionTicketKeyName[i].Visible;
            }
            worksheet.Columns = columnNames.Length;
        }

        private void Search()
        {
            this.labelStatus.Text = "搜索中...";

            new Thread(new ThreadStart(() =>
            {
                var wmsEntities = new WMSEntities();
                //ReceiptTicketView[] receiptTicketViews = null;
                SubmissionTicketView[] submissionTicketView = null;

                submissionTicketView = wmsEntities.Database.SqlQuery<SubmissionTicketView>(String.Format("SELECT * FROM SubmissionTicketView WHERE ReceiptTicketID={0}", receiptTicketID)).ToArray();

                this.reoGridControlPutaway.Invoke(new Action(() =>
                {
                    this.labelStatus.Text = "搜索完成";
                    var worksheet = this.reoGridControlPutaway.Worksheets[0];
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
                            worksheet[n, j] = columns[j];
                        }
                        n++;
                    }
                }));
                this.Invoke(new Action(this.RefreshTextBoxes));
            })).Start();

        }


        private void button1_Click(object sender, EventArgs e)
        {
            /*
            if (this.formMode == FormMode.ADD)
            {
                SubmissionTicket submissionTicket = new SubmissionTicket();
                WMSEntities wmsEntities = new WMSEntities();
                string errorInfo;
                if (Utilities.CopyTextBoxTextsToProperties(this, submissionTicket, ReceiptMetaData.submissionTicketKeyName, out errorInfo) == false)
                {
                    MessageBox.Show(errorInfo);
                }
                //wmsEntities.ReceiptTicket.Add(receiptTicket);
                else
                {
                    wmsEntities.SubmissionTicket.Add(submissionTicket);
                    wmsEntities.SaveChanges();
                    if (this.allOrPartial == AllOrPartial.ALL)
                    {
                        wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State = '送检中' WHERE ID = @receiptTicketID", new SqlParameter("receiptTicketID", receiptTicketID));

                        ReceiptTicketItem[] receiptTicketItem = (from rt in wmsEntities.ReceiptTicketItem where rt.ReceiptTicketID == receiptTicketID select rt).ToArray();
                        int i = 0;
                        ReceiptUtilities receiptUtilities = new ReceiptUtilities();
                        foreach (ReceiptTicketItem rti in receiptTicketItem)
                        {
                            SubmissionTicketItem submissionTicketItem = ReceiptUtilities.ReceiptTicketItemToSubmissionTicketItem(rti, submissionTicket.ID);
                            wmsEntities.SubmissionTicketItem.Add(submissionTicketItem);
                        }
                        wmsEntities.SaveChanges();
                    }
                    else
                    {
                        wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State = '部分送检中' WHERE ID = @receiptTicketID", new SqlParameter("receiptTicketID", receiptTicketID));

                    }
                }
            }
            else if (this.formMode == FormMode.ALTER)
            {
                string errorInfo;
                if (Utilities.CopyTextBoxTextsToProperties(this, this.submissionTicket, ReceiptMetaData.submissionTicketKeyName, out errorInfo) == false)
                {
                    MessageBox.Show(errorInfo);
                    return;
                }
                else
                {
                    new Thread(() =>
                    {
                        wmsEntities.SaveChanges();
                    }).Start();
                }
            }
            //this.submissionTicket = submissionTicket;
            MessageBox.Show("Successful!");
            this.finishedAction();
            this.Close();
            */

        }

        private void buttonAdd_Click(object sender, EventArgs e)
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
                new Thread(() =>
                {
                    submissionTicket.State = "待检";
                    wmsEntities.SubmissionTicket.Add(submissionTicket);
                    wmsEntities.SaveChanges();
                    this.Invoke(new Action(() => Search()));
                    MessageBox.Show("成功");
                }).Start();

            }
        }

        private void buttonAddItem_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlPutaway.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                FormAddSubmissionItem formAddSubmissionItem = new FormAddSubmissionItem(this.receiptTicketID, submissionTicketID);
                formAddSubmissionItem.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void buttonModify_Click(object sender, EventArgs e)
        {

            var worksheet = this.reoGridControlPutaway.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == submissionTicketID select st).FirstOrDefault();
                if (submissionTicket == null)
                {
                    MessageBox.Show("错误 无法修改此条目");
                }
                else
                {
                    string errorInfo;
                    if (Utilities.CopyTextBoxTextsToProperties(this, submissionTicket, ReceiptMetaData.submissionTicketKeyName, out errorInfo) == false)
                    {
                        MessageBox.Show(errorInfo);
                        return;
                    }
                    else
                    {
                        new Thread(() =>
                        {
                            wmsEntities.SaveChanges();
                            this.Invoke(new Action(() =>
                            {
                                Search();
                            }));
                            MessageBox.Show("成功");
                        }).Start();
                    }
                }
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}

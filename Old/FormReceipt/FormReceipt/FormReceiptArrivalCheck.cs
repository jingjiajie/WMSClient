using System;
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
        private int userID;
        private FormMode formMode;
        Action nextCallBack = null;
        Action finishedAction = null;
        private AllOrPartial allOrPartial;
        public FormReceiptArrivalCheck()
        {
            InitializeComponent();
        }
        public FormReceiptArrivalCheck(int receiptTicketID, int userID ,AllOrPartial allOrPartial)
        {
            InitializeComponent();
            this.receiptTicketID = receiptTicketID;
            this.allOrPartial = allOrPartial;
            this.userID = userID;
            this.formMode = FormMode.ADD;
        }

        public void SetNextCallBack(Action action)
        {
            this.nextCallBack = action;
        }

        public FormReceiptArrivalCheck(int receiptTicketID, int userID)
        {
            InitializeComponent();
            this.receiptTicketID = receiptTicketID;
            this.userID = userID;
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
            this.RefreshTextBoxes();
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
            //TextBox textBoxReceiptTicketID = (TextBox)this.Controls.Find("textBoxReceiptTicketID", true)[0];
            //textBoxReceiptTicketID.Text = this.receiptTicketID.ToString();
        }

        private void InitComponents()
        {
            //初始化
            //string[] columnNames = (from kn in ReceiptMetaData.submissionTicketKeyName select kn.Name).ToArray();
            //初始化表格
            var worksheet = this.reoGridControlPutaway.Worksheets[0];
            worksheet.SelectionMode = WorksheetSelectionMode.Row;
            for (int i = 0; i < ReceiptMetaData.submissionTicketKeyName.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = ReceiptMetaData.submissionTicketKeyName[i].Name;
                worksheet.ColumnHeaders[i].IsVisible = ReceiptMetaData.submissionTicketKeyName[i].Visible;
            }
            worksheet.Columns = ReceiptMetaData.submissionTicketKeyName.Length;
        }

        private void Search()
        {
            this.labelStatus.Text = "搜索中...";

            new Thread(new ThreadStart(() =>
            {
                var wmsEntities = new WMSEntities();
                //ReceiptTicketView[] receiptTicketViews = null;
                SubmissionTicketView[] submissionTicketView = null;
                try
                {
                    submissionTicketView = wmsEntities.Database.SqlQuery<SubmissionTicketView>(String.Format("SELECT * FROM SubmissionTicketView WHERE ReceiptTicketID={0}", receiptTicketID)).ToArray();
                }
                catch
                {
                    MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    return;
                }
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
                    ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == this.receiptTicketID select rt).FirstOrDefault();
                    if(receiptTicket != null)
                    {
                        submissionTicket.ProjectID = receiptTicket.ProjectID;
                        submissionTicket.WarehouseID = receiptTicket.WarehouseID;
                    }
                    submissionTicket.CreateUserID = this.userID;
                    submissionTicket.LastUpdateUserID = this.userID;
                    submissionTicket.LastUpdateTime = DateTime.Now;
                    submissionTicket.CreateTime = DateTime.Now;
                    submissionTicket.State = "待检";
                    submissionTicket.ReceiptTicketID = receiptTicketID;
                    wmsEntities.SubmissionTicket.Add(submissionTicket);
                    wmsEntities.SaveChanges();
                    submissionTicket.No = Utilities.GenerateNo("J", submissionTicket.ID);
                    try
                    {
                        wmsEntities.SaveChanges();
                        this.Invoke(new Action(() => Search()));
                        MessageBox.Show("成功");
                    }
                    catch
                    {
                        MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        return;
                    }
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
                    throw new EntityCommandExecutionException();
                }
                int submissionTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                FormAddSubmissionItem formAddSubmissionItem = new FormAddSubmissionItem(this.receiptTicketID, submissionTicketID);
                formAddSubmissionItem.SetCallBack(this.nextCallBack);
                formAddSubmissionItem.Show();
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

        private void buttonModify_Click(object sender, EventArgs e)
        {

            var worksheet = this.reoGridControlPutaway.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new EntityCommandExecutionException();
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



        private void buttonAddItem_MouseEnter(object sender, EventArgs e)
        {
            buttonAddItem.BackgroundImage = WMS.UI.Properties.Resources.bottonW_s;
        }

        private void buttonAddItem_MouseLeave(object sender, EventArgs e)
        {
            buttonAddItem.BackgroundImage = WMS.UI.Properties.Resources.bottonW_q;
        }

        private void buttonAddItem_MouseDown(object sender, MouseEventArgs e)
        {
            buttonAddItem.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
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

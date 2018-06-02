using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.DataAccess;
using System.Threading;
using System.Data.SqlClient;

namespace WMS.UI.FormReceipt
{
    public partial class FormAddSubmissionTicket : Form
    {
        private int receiptTicketID;
        private int submissionTicketID;
        private Action CallBack;
        private int userID;
        private FormMode formMode;
        private WMSEntities wmsEntities = new WMSEntities();
        private SubmissionTicket submissionTicket;
        private Func<int> PersonIDGetter = null;
        private Func<int> ReceivePersonIDGetter = null;
        private Func<int> SubmissionPersonIDGetter = null;
        private Func<int> DeliverPersonIDGetter = null;
        private int personID = -1;
        private int receivePersonID = -1;
        private int submissionPersonID = -1;
        private int DeliverPersonID = -1;
        public FormAddSubmissionTicket()
        {
            InitializeComponent();
        }

        public FormAddSubmissionTicket(int ID, int userID, FormMode formMode)
        {
            InitializeComponent();
            //this.receiptTicketID = receiptTicketID;
            this.userID = userID;
            this.formMode = formMode;
            if (formMode == FormMode.ADD)
            {
                this.receiptTicketID = ID;
            }
            else if (formMode == FormMode.ALTER)
            {
                this.submissionTicketID = ID;
            }
        }

        public void SetCallBack(Action action)
        {
            this.CallBack = action;
        }

        private void FormAddSubmissionTicket_Load(object sender, EventArgs e)
        {
            Utilities.CreateEditPanel(this.tableLayoutPanelTextBoxes, ReceiptMetaData.submissionTicketKeyName);
            this.PersonIDGetter = Utilities.BindTextBoxSelect<FormSelectPerson, Person>(this, "textBoxPersonName", "Name");
            this.DeliverPersonIDGetter = Utilities.BindTextBoxSelect<FormSelectPerson, Person>(this, "textBoxDeliverSubmissionPersonName", "Name");
            this.SubmissionPersonIDGetter = Utilities.BindTextBoxSelect<FormSelectPerson, Person>(this, "textBoxSubmissionPersonName", "Name");
            this.ReceivePersonIDGetter = Utilities.BindTextBoxSelect<FormSelectPerson, Person>(this, "textBoxReceivePersonName", "Name");
            //this.Controls.Find("textBoxID", true)[0].Text = "0";
            //TextBox textBoxReceiptTicketID = (TextBox)this.Controls.Find("textBoxReceiptTicketID", true)[0];
            //textBoxReceiptTicketID.Text = receiptTicketID.ToString();
            //textBoxReceiptTicketID.Enabled = false;
            if (this.formMode == FormMode.ADD)
            {
                TextBox textBoxState = (TextBox)this.Controls.Find("textBoxState", true)[0];
                textBoxState.Text = "待检";
                textBoxState.Enabled = false;
            }
            else
            {
                this.submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == this.submissionTicketID select st).FirstOrDefault();
                SubmissionTicketView submissionTicketView;
                submissionTicketView = (from st in wmsEntities.SubmissionTicketView where st.ID == this.submissionTicketID select st).FirstOrDefault();
                if (submissionTicketView == null)
                {
                    MessageBox.Show("该送检单已被删除！");
                    CallBack();
                    this.Close();
                }
                else
                {
                    this.personID = submissionTicket.PersonID == null ? -1 : (int)submissionTicket.PersonID;
                    Utilities.CopyPropertiesToTextBoxes(submissionTicketView, this);
                    Utilities.CopyPropertiesToComboBoxes(submissionTicketView, this);
                }
            }
            
           
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            SubmissionTicket submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == this.submissionTicketID select st).FirstOrDefault();
            if (submissionTicket == null)
            {
                MessageBox.Show("该送检单已被删除");
                return;
            }
            string errorInfo;
            if (Utilities.CopyTextBoxTextsToProperties(this, submissionTicket, ReceiptMetaData.submissionTicketKeyName, out errorInfo) == false || Utilities.CopyComboBoxsToProperties(this, submissionTicket, ReceiptMetaData.submissionTicketKeyName) == false)
            {
                MessageBox.Show(errorInfo + "获取数据数据失败");
                return;
            }
            //this.personID = this.PersonIDGetter();
            //submissionTicket.PersonID = this.personID == -1 ? this.PersonIDGetter() : this.personID;
            if (this.PersonIDGetter() != -1)
            {
                submissionTicket.PersonID = PersonIDGetter();
            }
            if (this.DeliverPersonIDGetter() != -1)
            {
                submissionTicket.DeliverSubmissionPersonID = DeliverPersonIDGetter();
            }
            if (this.SubmissionPersonIDGetter() != -1)
            {
                submissionTicket.SubmissionPersonID = SubmissionPersonIDGetter();
            }
            if (this.ReceivePersonIDGetter() != -1)
            {
                submissionTicket.ReceivePersonID = ReceivePersonIDGetter();
            }
            submissionTicket.LastUpdateUserID = this.userID;
            submissionTicket.LastUpdateTime = DateTime.Now;
            new Thread(() => 
            {
                wmsEntities.SaveChanges();
                MessageBox.Show("修改成功");
                this.Invoke(new Action(() =>
                {
                    CallBack();
                    this.Hide();
                }));
            }).Start();
            /*
            WMSEntities wmsEntities = new WMSEntities();
            if (this.formMode == FormMode.ADD)
            {
                this.submissionTicket = new SubmissionTicket();
            }
            else
            {
                this.submissionTicket = (from st in wmsEntities.SubmissionTicket where st.ID == this.submissionTicketID select st).FirstOrDefault();
                if (submissionTicket == null)
                {
                    MessageBox.Show("该送检单已被删除");
                    return;
                }
            }
            string errorInfo;
            if (Utilities.CopyTextBoxTextsToProperties(this, submissionTicket, ReceiptMetaData.submissionTicketKeyName, out errorInfo) == false)
            {
                MessageBox.Show(errorInfo);
                return;
            }
            else
            {
                Utilities.CopyComboBoxsToProperties(this, submissionTicket, ReceiptMetaData.submissionTicketKeyName);
                submissionTicket.PersonID = this.personID == -1 ? this.PersonIDGetter() : this.personID;

                if (this.formMode == FormMode.ADD)
                {
                    ReceiptTicket receiptTicket = (from rt in wmsEntities.ReceiptTicket where rt.ID == receiptTicketID select rt).FirstOrDefault();
                    if (receiptTicket != null)
                    {
                        submissionTicket.ProjectID = receiptTicket.ProjectID;
                        submissionTicket.WarehouseID = receiptTicket.Warehouse;
                        submissionTicket.ReceiptTicketID = receiptTicket.ID;
                    }
                    submissionTicket.CreateTime = DateTime.Now;
                    submissionTicket.CreateUserID = this.userID;
                    submissionTicket.LastUpdateTime = DateTime.Now;
                    submissionTicket.LastUpdateUserID = this.userID;
                    wmsEntities.SubmissionTicket.Add(submissionTicket);
                }
                else if (this.formMode == FormMode.ALTER)
                {
                    submissionTicket.PersonID = this.personID == -1 ? this.PersonIDGetter() : this.personID;
                    submissionTicket.LastUpdateUserID = this.userID;
                    submissionTicket.LastUpdateTime = DateTime.Now;
                    new Thread(()=> 
                    {
                        try
                        {
                            this.wmsEntities.SaveChanges();
                            MessageBox.Show("修改成功");
                        }
                        catch
                        {
                            MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                            return;
                        }
                        CallBack();
                    }).Start();
                    this.Close();
                    return;
                }
            }
            ReceiptTicketItem[] receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ReceiptTicketID == this.receiptTicketID select rti).ToArray();
            if (receiptTicketItem.Length == 0)
            {
                MessageBox.Show("该收货单未添加收货单条目");
            }
            else
            {
                foreach(ReceiptTicketItem rti in receiptTicketItem)
                {
                    SubmissionTicketItem submissionTicketItem1 = (from sti in wmsEntities.SubmissionTicketItem where sti.ReceiptTicketItemID == rti.ID select sti).FirstOrDefault();
                    if (rti.State == "送检中" && submissionTicketItem1 != null)
                    {
                        continue;
                    }
                    else
                    {
                        StockInfo stockInfo = (from si in wmsEntities.StockInfo where si.ReceiptTicketItemID == rti.ID select si).FirstOrDefault();
                        if (stockInfo == null)
                        {
                            MessageBox.Show("找不到对应的库存信息");
                        }
                        else
                        {
                            if (stockInfo.ReceiptAreaAmount != null)
                            {
                                int amountReceiptArea;
                                amountReceiptArea = (int)stockInfo.ReceiptAreaAmount;
                                stockInfo.ReceiptAreaAmount = 0;
                                //TODO stockInfo.SubmissionAreaAmount = amountReceiptArea;
                            }
                        }
                        rti.State = "送检中";
                        SubmissionTicketItem submissionTicketItem = new SubmissionTicketItem();
                        submissionTicketItem.ReceiptTicketItemID = rti.ID;
                        submissionTicketItem.State = "待检";
                        submissionTicket.SubmissionTicketItem.Add(submissionTicketItem);
                    }
                }
                new Thread(() =>
                {
                    try
                    {
                        wmsEntities.Database.ExecuteSqlCommand("UPDATE ReceiptTicket SET State = '送检中' WHERE ID = @receiptTicket", new SqlParameter("receiptTicket", receiptTicketID));
                        wmsEntities.SaveChanges();
                        submissionTicket.No = Utilities.GenerateNo("J", submissionTicket.ID);
                        wmsEntities.SaveChanges();
                        MessageBox.Show("成功");
                    }
                    catch
                    {
                        MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        return;
                    }
                }).Start();
            }
            CallBack();*/
        }

        private void buttonOK_MouseEnter(object sender, EventArgs e)
        {
            buttonOK.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        }

        private void buttonOK_MouseLeave(object sender, EventArgs e)
        {
            buttonOK.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_q;
        }

        private void buttonOK_MouseDown(object sender, MouseEventArgs e)
        {
            buttonOK.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }
    }
}

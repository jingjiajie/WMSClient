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

namespace WMS.UI.FormReceipt
{
    public partial class FormPutwayTicketModify : Form
    {
        private FormMode formMode;
        private int receiptTicketID;
        private int putawayTicketID;
        private Action CallBack = null;
        private Func<int> PersonIDGetter = null;
        private WMSEntities wmsEntities = new WMSEntities();
        public FormPutwayTicketModify()
        {
            InitializeComponent();
        }

        public FormPutwayTicketModify(int ID, FormMode formMode)
        {
            InitializeComponent();
            this.formMode = formMode;
            if (formMode == FormMode.ADD)
            {
                this.receiptTicketID = ID;
            }
            else
            {
                this.putawayTicketID = ID;
            }
        }

        private void FormPutwayTicketModify_Load(object sender, EventArgs e)
        {
            InitComponent();
            if (this.formMode == FormMode.ALTER)
            {
                PutawayTicketView putawayTicketView = (from pt in wmsEntities.PutawayTicketView where pt.ID == this.putawayTicketID select pt).FirstOrDefault();
                if (putawayTicketView != null)
                {
                    Utilities.CopyPropertiesToTextBoxes(putawayTicketView, this);
                }
            }
            else
            {
                TextBox textBoxReceiptTicketID = (TextBox)this.Controls.Find("textBoxReceiptTicketID", true)[0];
                textBoxReceiptTicketID.Text = this.receiptTicketID.ToString();
                textBoxReceiptTicketID.Enabled = false;
            }
        }

        private void InitComponent()
        {
            Utilities.CreateEditPanel(this.tableLayoutPanelTextBoxes, ReceiptMetaData.putawayTicketKeyName);
            this.PersonIDGetter = Utilities.BindTextBoxSelect<FormSelectPerson, Person>(this, "textBoxPersonName", "Name");
        }

        public void SetCallBack(Action a)
        {
            this.CallBack = a;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            PutawayTicket putawayTicket = (from pt in wmsEntities.PutawayTicket where pt.ID == putawayTicketID select pt).FirstOrDefault();
            if (putawayTicket == null)
            {
                MessageBox.Show("该上架单已被删除");
                return;
            }
            string errorInfo;
            if (Utilities.CopyTextBoxTextsToProperties(this, putawayTicket, ReceiptMetaData.putawayTicketKeyName, out errorInfo) == false)
            {
                MessageBox.Show(errorInfo);
                return;
            }
            else
            {
                if (this.PersonIDGetter() != -1)
                {
                    putawayTicket.PersonID = PersonIDGetter();
                }
                new Thread(() =>
                {
                    wmsEntities.SaveChanges();
                    MessageBox.Show("修改成功");
                    CallBack();
                }).Start();
            }
            /*
            PutawayTicket putawayTicket;
            if (this.formMode == FormMode.ADD)
            {
                putawayTicket = new PutawayTicket();
            }
            else
            {
                putawayTicket = (from pt in this.wmsEntities.PutawayTicket where pt.ID == this.putawayTicketID select pt).Single();
            }
            string errorInfo;
            if (Utilities.CopyTextBoxTextsToProperties(this, putawayTicket, ReceiptMetaData.putawayTicketKeyName, out errorInfo) == false)
            {
                MessageBox.Show(errorInfo);
                CallBack();
            }
            else
            {
                new Thread(() =>
                {

                    if (this.formMode == FormMode.ADD)
                    {
                        this.wmsEntities.PutawayTicket.Add(putawayTicket);
                        wmsEntities.SaveChanges();
                        wmsEntities.Database.ExecuteSqlCommand(String.Format("UPDATE ReceiptTicket SET State='收货' WHERE ID={0}", this.receiptTicketID));
                        ReceiptTicketItem[] receiptTicketItem = (from rti in wmsEntities.ReceiptTicketItem where rti.ReceiptTicketID == this.receiptTicketID && rti.State != "作废" select rti).ToArray();
                        PutawayTicket putawayTicket1 = (from pt in wmsEntities.PutawayTicket where pt.ReceiptTicketID == receiptTicketID select pt).FirstOrDefault();
                        foreach (ReceiptTicketItem rti in receiptTicketItem)
                        {
                            rti.State = "收货";

                            wmsEntities.PutawayTicketItem.Add(ReceiptUtilities.ReceiptTicketItemToPutawayTicketItem(rti, putawayTicket1.ID));
                            try
                            {
                                wmsEntities.SaveChanges();
                            }
                            catch
                            {
                                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                                return;
                            }
                        }

                    }
                    else
                    {
                        try
                        {
                            this.wmsEntities.SaveChanges();
                        }
                        catch
                        {
                            MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                            return;
                        }
                   }
                }).Start();
            }

            this.CallBack();
            this.Close();*/
        }
    }
}

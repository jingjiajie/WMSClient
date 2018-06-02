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
    public partial class FormPutawayModify : Form
    {
        private Action CallBack;
        private int putawayTicketID;
        private WMSEntities wmsEntities = new WMSEntities();
        private PutawayTicket putawayTicket;
        private Func<int> PersonIDGetter = null;
        public FormPutawayModify()
        {
            InitializeComponent();
        }

        public FormPutawayModify(int putawayTicketID)
        {
            InitializeComponent();
            this.putawayTicketID = putawayTicketID;
        }

        public void SetCallBack(Action action)
        {
            this.CallBack = action;
        }

        private void FormPutawayModify_Load(object sender, EventArgs e)
        {
            //InitComponent();
            Utilities.CreateEditPanel(this.tableLayoutPanelTextBoxes, ReceiptMetaData.putawayTicketKeyName);
            this.PersonIDGetter = Utilities.BindTextBoxSelect<FormSelectPerson, Person>(this, "textBoxPersonName", "Name");
            putawayTicket = (from pt in wmsEntities.PutawayTicket where pt.ID == this.putawayTicketID select pt).FirstOrDefault();
            PutawayTicketView putawayTicketView = (from ptv in wmsEntities.PutawayTicketView where ptv.ID == this.putawayTicketID select ptv).FirstOrDefault();

            if (putawayTicketView == null)
            {
                MessageBox.Show("找不到该上架单", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                Utilities.CopyPropertiesToTextBoxes(putawayTicketView, this);
                Utilities.CopyPropertiesToComboBoxes(putawayTicketView, this);
            }
        }

        

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string errorInfo;
            //PutawayTicket putawayTicket = (from pt in wmsEntities.PutawayTicket where pt.ID == this.putawayTicketID select pt).FirstOrDefault();
            //if (putwayTicket)
            if (Utilities.CopyTextBoxTextsToProperties(this, putawayTicket, ReceiptMetaData.putawayTicketKeyName, out errorInfo) == true)
            {
                if (Utilities.CopyComboBoxsToProperties(this, putawayTicket, ReceiptMetaData.putawayTicketKeyName) == true)
                {
                    if (this.PersonIDGetter() != -1)
                    {
                        putawayTicket.PersonID = this.PersonIDGetter();
                    }
                    //wmsEntities.PutawayTicket.Add(putawayTicket);
                    new Thread(() =>
                    {
                        try
                        {
                            wmsEntities.SaveChanges();
                            MessageBox.Show("成功","提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Invoke(new Action(() =>
                            {
                                CallBack();
                                this.Hide();
                            }));
                        }
                        catch
                        {
                            MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                            return;
                        }
                    }).Start();
                }
                else
                {
                    MessageBox.Show("单据类型读取失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(errorInfo, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.DataAccess;

namespace WMS.UI.FormReceipt
{
    public partial class FormSubmissionTicketItemModify : Form
    {
        FormMode formMode = new FormMode();
        ReceiptTicketItem receiptTicketItem;
        int submissionTicketID;
        WMSEntities wmsEnities = new WMSEntities();
        public FormSubmissionTicketItemModify()
        {
            InitializeComponent();
        }
        public FormSubmissionTicketItemModify(int submissionTicketID, ReceiptTicketItem receiptTicketItem)
        {
            InitializeComponent();
            this.submissionTicketID = submissionTicketID;
            this.receiptTicketItem = receiptTicketItem;
        }

        private void FormSubmissionTicketModify_Load(object sender, EventArgs e)
        {
            this.tableLayoutPanelTextBoxes.Controls.Clear();
            for (int i = 0; i < ReceiptMetaData.submissionTicketItemKeyName.Length; i++)
            {
                KeyName curKeyName = ReceiptMetaData.submissionTicketItemKeyName[i];

                Label label = new Label();
                label.Text = curKeyName.Name;
                this.tableLayoutPanelTextBoxes.Controls.Add(label);

                TextBox textBox = new TextBox();
                textBox.Name = "textBox" + curKeyName.Key;
                
                //Console.WriteLine("{0}  {1}", label.Text, textBox.Name);


                this.tableLayoutPanelTextBoxes.Controls.Add(textBox);
            }
            this.Controls.Find("textBoxSubmissionTicketID", true)[0].Text = this.submissionTicketID.ToString();
            this.Controls.Find("textBoxSubmissionTicketID", true)[0].Enabled = false;
            this.Controls.Find("textBoxSupplyID", true)[0].Enabled = false;
            this.Controls.Find("textBoxSupplyID", true)[0].Text = this.receiptTicketItem.SupplyID.ToString();
            /*
            if (this.formMode == FormMode.ALTER)
            {

                ReceiptTicketItemView receiptTicketItemView = (from s in this.wmsEntities.ReceiptTicketItemView
                                                               where s.ID == this.receiptTicketItemID
                                                               select s).Single();
                Utilities.CopyPropertiesToTextBoxes(receiptTicketItemView, this);

            }
            this.Controls.Find("textBoxReceiptTicketID", true)[0].Enabled = false;
            this.Controls.Find("textBoxReceiptTicketID", true)[0].Text = this.receiptTicketID.ToString();
            */
        }
    }
}

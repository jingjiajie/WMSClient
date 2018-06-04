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
namespace WMS.UI
{
    public partial class FormSubmissionChooseExcelType : Form
    {
        private int[] ids;
        public FormSubmissionChooseExcelType()
        {
            InitializeComponent();
        }
        public FormSubmissionChooseExcelType(int[] ids)
        {
            InitializeComponent();
            this.ids = ids;
        }

        private void FormSubmissionChooseExcelType_Load(object sender, EventArgs e)
        {

        }

        private SubmissionTicket[] idsToSubmissionTickets(int[] ids)
        {
            List<SubmissionTicket> submissionTicketList = new List<SubmissionTicket>();
            try
            {
                WMSEntities wmsEntities = new WMSEntities();

                foreach (int id in ids)
                {
                    SubmissionTicket submissionTicket = (from s in wmsEntities.SubmissionTicket where s.ID == id select s).FirstOrDefault();
                    if (submissionTicket != null)
                    {
                        submissionTicket.PaintTime = DateTime.Now;
                        submissionTicketList.Add(submissionTicket);
                    }
                }
                wmsEntities.SaveChanges();
            }
            catch
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return null;
            }

            return submissionTicketList.ToArray();
        }

        private void buttonAll_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            StandardFormPreviewExcel standardFormPreviewExcel = new StandardFormPreviewExcel("送检单预览");
            SubmissionTicket[] submissionTicket = idsToSubmissionTickets(this.ids);
            try
            {
                foreach(SubmissionTicket st in submissionTicket)
                {
                    string worksheetName = st.ID.ToString();
                    SubmissionTicketView submissionTicketView = (from stv in wmsEntities.SubmissionTicketView where stv.ID == st.ID select stv).FirstOrDefault();
                    SubmissionTicketItemView[] submissionTicketItemView =
                        (from p in wmsEntities.SubmissionTicketItemView
                         where p.SubmissionTicketID == st.ID
                        select p).ToArray();
                    ReceiptTicketView receiptTicketView = (from rt in wmsEntities.ReceiptTicketView where rt.ID == st.ReceiptTicketID select rt).FirstOrDefault();
                    if (standardFormPreviewExcel.AddPatternTable(@"Excel\SubmissionTicket.xlsx", worksheetName) == false)
                    {
                        this.Close();
                        return;
                    }
                    if (st != null)
                    {
                        standardFormPreviewExcel.AddData("SubmissionTicket", submissionTicketView, worksheetName);
                    }
                    if (receiptTicketView != null)
                    {
                        standardFormPreviewExcel.AddData("ReceiptTicket", receiptTicketView, worksheetName);
                    }
                    standardFormPreviewExcel.AddData("SubmissionTicketItem", submissionTicketItemView, worksheetName);
                    standardFormPreviewExcel.SetPrintScale(0.82f,worksheetName);
                }
                standardFormPreviewExcel.Show();
                this.Close();
            }
            catch
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
            
            //StartPreview(submissionTicketItemView);
        }

        private SubmissionTicket AddPreviewTime(SubmissionTicket submissionTicket)
        {
            
            if (submissionTicket != null)
            {
                submissionTicket.PaintTime = DateTime.Now;
            }           

            return submissionTicket;
        }

        private void StartPreview(SubmissionTicketItemView[] submissionTicketItemView)
        {
            //WMSEntities wmsEntities = new WMSEntities();
            //StandardFormPreviewExcel formPreview = new StandardFormPreviewExcel("送检单预览", (float)0.7);
            //if (formPreview.SetPatternTable(@"Excel\SubmissionTicket.xlsx") == false)
            //{
            //    this.Close();
            //    return;
            //}
            //SubmissionTicketView submissionTicketView = (from stv in wmsEntities.SubmissionTicketView where stv.ID == this.submissionTicket.ID select stv).FirstOrDefault();
            //ReceiptTicketView receiptTicketView = (from rtv in wmsEntities.ReceiptTicketView where rtv.ID == submissionTicketView.ReceiptTicketID select rtv).FirstOrDefault();
            //if (receiptTicketView != null)
            //{
            //    formPreview.AddData("ReceiptTicket", receiptTicketView);
            //}
            //if (submissionTicketView != null)
            //{
            //    formPreview.AddData("SubmissionTicket", submissionTicketView);
            //}
            //formPreview.AddData("SubmissionTicketItem", submissionTicketItemView);
            //formPreview.Show();
            //this.Close();
        }

        private void buttonPass_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            StandardFormPreviewExcel standardFormPreviewExcel = new StandardFormPreviewExcel("送检单预览",0.73f);
            SubmissionTicket[] submissionTicket = idsToSubmissionTickets(this.ids);
            try
            {
                foreach (SubmissionTicket st in submissionTicket)
                {
                    string worksheetName = st.ID.ToString();
                    SubmissionTicketView submissionTicketView = (from stv in wmsEntities.SubmissionTicketView where stv.ID == st.ID select stv).FirstOrDefault();
                    SubmissionTicketItemView[] submissionTicketItemView =
                        (from p in wmsEntities.SubmissionTicketItemView
                         where p.SubmissionTicketID == st.ID && p.State == "合格"
                         select p).ToArray();
                    ReceiptTicketView receiptTicketView = (from rt in wmsEntities.ReceiptTicketView where rt.ID == st.ReceiptTicketID select rt).FirstOrDefault();
                    if (standardFormPreviewExcel.AddPatternTable(@"Excel\SubmissionTicket.xlsx", worksheetName) == false)
                    {
                        this.Close();
                        return;
                    }
                    if (st != null)
                    {
                        standardFormPreviewExcel.AddData("SubmissionTicket", submissionTicketView, worksheetName);
                    }
                    if (receiptTicketView != null)
                    {
                        standardFormPreviewExcel.AddData("ReceiptTicket", receiptTicketView, worksheetName);
                    }
                    standardFormPreviewExcel.AddData("SubmissionTicketItem", submissionTicketItemView, worksheetName);
                    standardFormPreviewExcel.SetPrintScale(0.82f, worksheetName);
                }
                standardFormPreviewExcel.Show();
                this.Close();
            }
            catch
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
        }

        private void buttonNoPass_Click(object sender, EventArgs e)
        {
            WMSEntities wmsEntities = new WMSEntities();
            StandardFormPreviewExcel standardFormPreviewExcel = new StandardFormPreviewExcel("送检单预览",0.73f);
            SubmissionTicket[] submissionTicket = idsToSubmissionTickets(this.ids);
            try
            {
                foreach (SubmissionTicket st in submissionTicket)
                {
                    string worksheetName = st.ID.ToString();
                    SubmissionTicketView submissionTicketView = (from stv in wmsEntities.SubmissionTicketView where stv.ID == st.ID select stv).FirstOrDefault();
                    SubmissionTicketItemView[] submissionTicketItemView =
                        (from p in wmsEntities.SubmissionTicketItemView
                         where p.SubmissionTicketID == st.ID && p.State != "合格"
                         select p).ToArray();
                    ReceiptTicketView receiptTicketView = (from rt in wmsEntities.ReceiptTicketView where rt.ID == st.ReceiptTicketID select rt).FirstOrDefault();
                    if (standardFormPreviewExcel.AddPatternTable(@"Excel\SubmissionTicket.xlsx", worksheetName) == false)
                    {
                        this.Close();
                        return;
                    }
                    if (st != null)
                    {
                        standardFormPreviewExcel.AddData("SubmissionTicket", submissionTicketView, worksheetName);
                    }
                    if (receiptTicketView != null)
                    {
                        standardFormPreviewExcel.AddData("ReceiptTicket", receiptTicketView, worksheetName);
                    }
                    standardFormPreviewExcel.AddData("SubmissionTicketItem", submissionTicketItemView, worksheetName);
                    standardFormPreviewExcel.SetPrintScale(0.82f, worksheetName);
                }
                standardFormPreviewExcel.Show();
                this.Close();
            }
            catch
            {
                MessageBox.Show("无法连接到数据库，请查看网络连接!", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                return;
            }
        }

        private void buttonPass_MouseEnter(object sender, EventArgs e)
        {
            buttonPass.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        }

        private void buttonPass_MouseLeave(object sender, EventArgs e)
        {
            buttonPass.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_q;
        }

        private void buttonPass_MouseDown(object sender, MouseEventArgs e)
        {
            buttonPass.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private void buttonNoPass_MouseEnter(object sender, EventArgs e)
        {
            buttonNoPass.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        }

        private void buttonNoPass_MouseLeave(object sender, EventArgs e)
        {
            buttonNoPass.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_q;
        }

        private void buttonNoPass_MouseDown(object sender, MouseEventArgs e)
        {
            buttonNoPass.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private void buttonAll_MouseEnter(object sender, EventArgs e)
        {
            buttonAll.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        }

        private void buttonAll_MouseLeave(object sender, EventArgs e)
        {
            buttonAll.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_q;
        }

        private void buttonAll_MouseDown(object sender, MouseEventArgs e)
        {
            buttonAll.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.DataAccess;
using unvell.ReoGrid;
using System.Threading;

namespace WMS.UI.FormReceipt
{
    public partial class FormPutaway : Form
    {
        private int receiptTicketID;
        private int putawayTicketID;
        private int userID;
        private int warehouseID;
        private int projectID;
        WMSEntities wmsEntities = new WMSEntities();
        public FormPutaway()
        {
            InitializeComponent();
        }

        public FormPutaway(int receiptTicketID, int warehouseID, int projectID, int userID)
        {
            InitializeComponent();
            this.receiptTicketID = receiptTicketID;
            this.warehouseID = warehouseID;
            this.projectID = projectID;
            this.userID = userID;
        }

        private void FormPutaway_Load(object sender, EventArgs e)
        {
            InitComponents();
            InitPanel();
            WMSEntities wmsEntities = new WMSEntities();

            Search();
        }
        private void InitPanel()
        {
            WMSEntities wmsEntities = new WMSEntities();
            //this.Controls.Clear();
            Utilities.CreateEditPanel(this.tableLayoutPanelProperties, ReceiptMetaData.putawayTicketKeyName);
            this.reoGridControlPutaway.Worksheets[0].SelectionRangeChanged += worksheet_SelectionRangeChanged;
            //TextBox textBoxReceiptTitcket = (TextBox)this.Controls.Find("textBoxReceiptTicketID", true)[0];
            //textBoxReceiptTitcket.Text = this.receiptTicketID.ToString();
            //textBoxReceiptTitcket.Enabled = false;
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
                this.putawayTicketID = -1;
                return;
            }
            int id = ids[0];
            PutawayTicketView putawayTicketView = (from s in this.wmsEntities.PutawayTicketView
                                                                 where s.ID == id
                                                                 select s).FirstOrDefault();
            if (putawayTicketView == null)
            {
                MessageBox.Show("系统错误，未找到相应送检单项目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //putawayTicketView.ReceiptTicketID = this.receiptTicketID;
            //this.putawayTicketID = int.Parse(submissionTicketItemView.SubmissionTicketID.ToString());
            Utilities.CopyPropertiesToTextBoxes(putawayTicketView, this);
            //this.Controls.Find("textBoxReceiptTicketID", true)[0].Text = this.receiptTicketID.ToString();
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
            //this.Controls.Find("textBoxReceiptTicketID", true)[0].Text = this.receiptTicketID.ToString();
        }

        private void InitComponents()
        {
            //初始化
            //string[] columnNames = (from kn in ReceiptMetaData.putawayTicketKeyName select kn.Name).ToArray();
            //初始化表格
            var worksheet = this.reoGridControlPutaway.Worksheets[0];
            worksheet.SelectionMode = WorksheetSelectionMode.Row;
            for (int i = 0; i < ReceiptMetaData.putawayTicketKeyName.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = ReceiptMetaData.putawayTicketKeyName[i].Name;
                worksheet.ColumnHeaders[i].IsVisible = ReceiptMetaData.putawayTicketKeyName[i].Visible;
            }
            worksheet.Columns = ReceiptMetaData.putawayTicketKeyName.Length;
        }

        private void Search()
        {
            this.labelStatus.Text = "搜索中...";

            new Thread(new ThreadStart(() =>
            {
                var wmsEntities = new WMSEntities();
                //ReceiptTicketView[] receiptTicketViews = null;
                PutawayTicketView[] putawayTicketView = null;
                try
                {
                    putawayTicketView = wmsEntities.Database.SqlQuery<PutawayTicketView>(String.Format("SELECT * FROM PutawayTicketView WHERE ReceiptTicketID={0}", receiptTicketID)).ToArray();
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
                    for (int i = 0; i < putawayTicketView.Length; i++)
                    {
                        if (putawayTicketView[i].State == "作废")
                        {
                            continue;
                        }
                        PutawayTicketView curPutawayTicketView = putawayTicketView[i];
                        object[] columns = Utilities.GetValuesByPropertieNames(curPutawayTicketView, (from kn in ReceiptMetaData.putawayTicketKeyName select kn.Key).ToArray());
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

        private void buttonCreatePutaway_Click(object sender, EventArgs e)
        {
            PutawayTicket putawayTicket = new PutawayTicket();
            string errorInfo;
            if (Utilities.CopyTextBoxTextsToProperties(this, putawayTicket, ReceiptMetaData.putawayTicketKeyName, out errorInfo) == false)
            {
                MessageBox.Show(errorInfo);
                return;
            }
            else
            {
                putawayTicket.ReceiptTicketID = this.receiptTicketID;
                putawayTicket.CreateTime = DateTime.Now;
                putawayTicket.CreateUserID = this.userID;
                putawayTicket.LastUpdateTime = DateTime.Now;
                putawayTicket.LastUpdateUserID = this.userID;
                putawayTicket.WarehouseID = this.warehouseID;
                putawayTicket.ProjectID = this.projectID;
                putawayTicket.State = "待上架";
                new Thread(()=>
                {
                    try
                    {
                        wmsEntities.PutawayTicket.Add(putawayTicket);
                        wmsEntities.SaveChanges();
                        putawayTicket.No = Utilities.GenerateNo("P", putawayTicket.ID);
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

        private void buttonModify_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlPutaway.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int putawayTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                PutawayTicket putawayTicket = (from pt in wmsEntities.PutawayTicket where pt.ID == putawayTicketID select pt).Single();
                string errorInfo;
                if (Utilities.CopyTextBoxTextsToProperties(this, putawayTicket, ReceiptMetaData.putawayTicketKeyName, out errorInfo) == false)
                {
                    putawayTicket.LastUpdateTime = DateTime.Now;
                    putawayTicket.LastUpdateUserID = this.userID;
                    MessageBox.Show(errorInfo);
                    Search();
                    return;
                }
                else
                {
                    new Thread(() =>
                    {
                        try
                        {
                            wmsEntities.SaveChanges();
                            Search();
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
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void buttonItemCheck_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlPutaway.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int putawayTicketID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                FormAddPutawayItem formAddPutawayItem = new FormAddPutawayItem(putawayTicketID, this.receiptTicketID, this.warehouseID, this.projectID);
                formAddPutawayItem.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}

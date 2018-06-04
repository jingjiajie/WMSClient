using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.DataAccess;
using System.Reflection;

namespace WMS.UI
{
    public partial class FormJobTicketModify : Form
    {
        private int jobTicketID = -1;
        private int userID = -1;

        private WMSEntities wmsEntities = new WMSEntities();
        private Action modifyFinishedCallback = null;

        private TextBox textBoxPersonName = null;
        private int curPersonID = -1;

        public FormJobTicketModify(int userID,int jobTicketID)
        {
            InitializeComponent();
            this.jobTicketID = jobTicketID;
            this.userID = userID;
        }

        private void FormJobTicketModify_Load(object sender, EventArgs e)
        {
            if (this.jobTicketID == -1)
            {
                throw new Exception("未设置源作业单信息");
            }
            Utilities.CreateEditPanel(this.tableLayoutPanelTextBoxes, JobTicketViewMetaData.KeyNames);

            this.textBoxPersonName = (TextBox)this.Controls.Find("textBoxPersonName", true)[0];
            textBoxPersonName.ReadOnly = true;
            textBoxPersonName.BackColor = Color.White;
            textBoxPersonName.Click += textBoxPersonName_Click;

            JobTicketView jobTicketView = null;
            try
            {
                jobTicketView = (from s in this.wmsEntities.JobTicketView
                                               where s.ID == this.jobTicketID
                                               select s).FirstOrDefault();
            }
            catch
            {
                MessageBox.Show("加载数据失败，请检查网络连接","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            if(jobTicketView == null)
            {
                MessageBox.Show("作业单不存在，请刷新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            Utilities.CopyPropertiesToTextBoxes(jobTicketView, this);
            Utilities.CopyPropertiesToComboBoxes(jobTicketView, this);
        }

        private void textBoxPersonName_Click(object sender, EventArgs e)
        {
            FormSelectPerson form = new FormSelectPerson();
            form.SetSelectFinishedCallback((id) =>
            {
                this.curPersonID = id;
                if (!this.IsDisposed)
                {
                    WMSEntities wmsEntities = new WMSEntities();
                    Person person = (from p in wmsEntities.Person
                                     where p.ID == id
                                     select p).FirstOrDefault();
                    if (person == null)
                    {
                        MessageBox.Show("选中人员不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    this.curPersonID = id;
                    this.textBoxPersonName.Text = person.Name;
                }
            });
            form.Show();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            JobTicket jobTicket = null;
            try
            {
                jobTicket = (from s in this.wmsEntities.JobTicket
                             where s.ID == this.jobTicketID
                             select s).FirstOrDefault();
            }
            catch
            {
                MessageBox.Show("修改失败，请检查网络连接","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (jobTicket == null)
            {
                MessageBox.Show("作业单不存在，请刷新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (this.curPersonID == -1)
            {
                jobTicket.PersonID = null;
            }
            else
            {
                jobTicket.PersonID = this.curPersonID;
            }
            jobTicket.LastUpdateUserID = this.userID;
            jobTicket.LastUpdateTime = DateTime.Now;


            //开始数据库操作
            if (Utilities.CopyTextBoxTextsToProperties(this, jobTicket, JobTicketViewMetaData.KeyNames, out string errorMessage) == false)
            {
                MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(Utilities.CopyComboBoxsToProperties(this,jobTicket, JobTicketViewMetaData.KeyNames) == false)
            {
                MessageBox.Show("内部错误：读取复选框数据失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                wmsEntities.SaveChanges();
            }
            catch
            {
                MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //调用回调函数
            if (this.modifyFinishedCallback != null)
            {
                this.modifyFinishedCallback();
            }
            MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();

        }

        public void SetModifyFinishedCallback(Action callback)
        {
            this.modifyFinishedCallback = callback;
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

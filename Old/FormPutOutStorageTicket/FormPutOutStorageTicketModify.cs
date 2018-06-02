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
    public partial class FormPutOutStorageTicketModify : Form
    {
        private int putOutStorageTicketID = -1;
        private int userID = -1;

        private Func<int> personIDGetter = null;

        private WMSEntities wmsEntities = new WMSEntities();
        private Action modifyFinishedCallback = null;
        private Action addFinishedCallback = null;

        public FormPutOutStorageTicketModify(int userID,int putOutStorageTicketID)
        {
            InitializeComponent();
            this.putOutStorageTicketID = putOutStorageTicketID;
            this.userID = userID;
        }

        private void FormPutOutStorageTicketModify_Load(object sender, EventArgs e)
        {
            if (this.putOutStorageTicketID == -1)
            {
                throw new Exception("未设置源库存信息");
            }
            Utilities.CreateEditPanel(this.tableLayoutPanelTextBoxes, PutOutStorageTicketViewMetaData.KeyNames);
            this.personIDGetter = Utilities.BindTextBoxSelect<FormSelectPerson, Person>(this, "textBoxPersonName", "Name");
            PutOutStorageTicketView putOutStorageTicketView = null;
            try
            {
                putOutStorageTicketView = (from s in this.wmsEntities.PutOutStorageTicketView
                                                                   where s.ID == this.putOutStorageTicketID
                                                                   select s).FirstOrDefault();
            }
            catch
            {
                MessageBox.Show("加载数据失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            if(putOutStorageTicketView == null)
            {
                MessageBox.Show("出库单不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            Utilities.CopyPropertiesToTextBoxes(putOutStorageTicketView, this);
            Utilities.CopyPropertiesToComboBoxes(putOutStorageTicketView, this);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            PutOutStorageTicket putOutStorageTicket = null;
            try
            {
                putOutStorageTicket = (from s in this.wmsEntities.PutOutStorageTicket
                                       where s.ID == this.putOutStorageTicketID
                                       select s).FirstOrDefault();
            }
            catch
            {
                MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (putOutStorageTicket == null)
            {
                MessageBox.Show("出库单不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            putOutStorageTicket.LastUpdateUserID = this.userID;
            putOutStorageTicket.LastUpdateTime = DateTime.Now;
            int personID = this.personIDGetter();
            putOutStorageTicket.PersonID = personID == -1 ? null : (int?)personID;


            //开始数据库操作
            if (Utilities.CopyTextBoxTextsToProperties(this, putOutStorageTicket, PutOutStorageTicketViewMetaData.KeyNames, out string errorMessage) == false)
            {
                MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(Utilities.CopyComboBoxsToProperties(this,putOutStorageTicket, PutOutStorageTicketViewMetaData.KeyNames) == false)
            {
                MessageBox.Show("内部错误，读取选择框数据失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

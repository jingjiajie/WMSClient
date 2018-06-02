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
using System.Threading;

namespace WMS.UI.FormBase
{
    public partial class FormUserModify : Form
    {
        private int userID = -1;
        private int curSupplierID = -1; //这个ID对应界面上的供应商名
        private Action<int> modifyFinishedCallback = null;
        private Action<int> addFinishedCallback = null;
        private FormMode mode = FormMode.ALTER;

        TextBox textBoxUsername = null;
        TextBox textBoxPassword = null;
        TextBox textBoxAuthorityName = null;

        public FormUserModify(int userID = -1)
        {
            InitializeComponent();
            this.userID = userID;
        }

        private void FormUserModify_Load(object sender, EventArgs e)
        {
            if (this.mode == FormMode.ALTER && this.userID == -1)
            {
                throw new Exception("未设置源库存信息");
            }

            WMSEntities wmsEntities = new WMSEntities();

            Utilities.CreateEditPanel(this.tableLayoutPanelTextBoxes, UserMetaData.KeyNames);
            this.textBoxAuthorityName = (TextBox)this.Controls.Find("textBoxAuthorityName",true)[0];
            this.textBoxUsername = (TextBox)this.Controls.Find("textBoxUsername", true)[0];
            this.textBoxPassword = (TextBox)this.Controls.Find("textBoxPassword", true)[0];

            this.textBoxPassword.PasswordChar = '*';

            if (this.mode == FormMode.ALTER)
            {
                UserView userView = null;
                try
                {
                    userView = (from s in wmsEntities.UserView
                                where s.ID == this.userID
                                select s).Single();
                }
                catch
                {
                    MessageBox.Show("无法连接到服务器，请检查网络连接","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                Utilities.CopyPropertiesToTextBoxes(userView, this);
                if(userView.SupplierID.HasValue)
                {
                    this.curSupplierID = userView.SupplierID.Value;
                }

                if((userView.Authority & (int)UserMetaData.AUTHORITY_MANAGER) == (int)UserMetaData.AUTHORITY_MANAGER)
                {
                    this.checkBoxAuthorityManager.Checked = true;
                }else if ((userView.Authority & (int)UserMetaData.AUTHORITY_SUPPLIER) == (int)UserMetaData.AUTHORITY_SUPPLIER)
                {
                    this.checkBoxAuthoritySupplier.Checked = true;
                }
                else
                {
                    if((userView.Authority & (int)UserMetaData.AUTHORITY_RECEIPT_MANAGER) == (int)UserMetaData.AUTHORITY_RECEIPT_MANAGER)
                    {
                        this.checkBoxAuthorityReceipt.Checked = true;
                    }
                    if ((userView.Authority & (int)UserMetaData.AUTHORITY_SHIPMENT_MANAGER) == (int)UserMetaData.AUTHORITY_SHIPMENT_MANAGER)
                    {
                        this.checkBoxAuthorityShipment.Checked = true;
                    }
                    if((userView.Authority & (int)UserMetaData.AUTHORITY_STOCK_MANAGER) == (int)UserMetaData.AUTHORITY_STOCK_MANAGER)
                    {
                        this.checkBoxAuthorityStock.Checked = true;
                    }
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {

            WMSEntities wmsEntities = new WMSEntities();
            User user = null;

            if(textBoxUsername.Text.Length == 0)
            {
                MessageBox.Show("用户名不允许为空","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }else if (textBoxPassword.Text.Length == 0)
            {
                MessageBox.Show("密码不允许为空","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //若修改，则查询原对象。若添加，则新建一个对象。
            try
            {
                if (this.mode == FormMode.ALTER)
                {
                    user = (from s in wmsEntities.User
                            where s.ID == this.userID
                            select s).Single();

                    //检测用户名同名
                    var sameNameUsers = (from u in wmsEntities.User
                                         where u.Username == textBoxUsername.Text
                                            && u.ID != user.ID
                                         select u).ToArray();
                    if (sameNameUsers.Length > 0)
                    {
                        MessageBox.Show("修改用户名失败，已存在同名用户：" + textBoxUsername.Text,"提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else if (mode == FormMode.ADD)
                {
                    //检测用户名同名
                    var sameNameUsers = (from u in wmsEntities.User
                                         where u.Username == textBoxUsername.Text
                                         select u).ToArray();
                    if (sameNameUsers.Length > 0)
                    {
                        MessageBox.Show("添加用户失败，已存在同名用户：" + textBoxUsername.Text,"提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    user = new User();
                    wmsEntities.User.Add(user);
                }
            }
            catch
            {
                MessageBox.Show("操作失败，请检查网络连接","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //开始数据库操作
            if (Utilities.CopyTextBoxTextsToProperties(this, user, UserMetaData.KeyNames, out string errorMessage) == false)
            {
                MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(this.curSupplierID != -1)
            {
                user.SupplierID = this.curSupplierID;
            }
            else
            {
                user.SupplierID = null;
            }

            user.Authority = 0;
            if (this.checkBoxAuthorityManager.Checked)
            {
                user.Authority |= UserMetaData.AUTHORITY_MANAGER;
            }
            if (this.checkBoxAuthorityReceipt.Checked)
            {
                user.Authority |= UserMetaData.AUTHORITY_RECEIPT_MANAGER;
            }
            if (this.checkBoxAuthorityShipment.Checked)
            {
                user.Authority |= UserMetaData.AUTHORITY_SHIPMENT_MANAGER;
            }
            if (this.checkBoxAuthorityStock.Checked)
            {
                user.Authority |= UserMetaData.AUTHORITY_STOCK_MANAGER;
            }
            if (this.checkBoxAuthoritySupplier.Checked)
            {
                user.Authority |= UserMetaData.AUTHORITY_SUPPLIER;
            }
            if(user.Authority == 0)
            {
                MessageBox.Show("用户权限不可以为空","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            new Thread(()=>
            {
                try
                {
                    wmsEntities.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("连接数据库失败，请检查网络连接","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.Invoke(new Action(()=>
                {
                    //调用回调函数
                    if (this.mode == FormMode.ALTER && this.modifyFinishedCallback != null)
                    {
                        this.Invoke(new Action(()=>
                        {
                            this.modifyFinishedCallback(user.ID);
                        }));
                        MessageBox.Show("修改成功！","提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (this.mode == FormMode.ADD && this.addFinishedCallback != null)
                    {
                        this.Invoke(new Action(() =>
                        {
                            this.addFinishedCallback(user.ID);
                        }));
                        MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    this.Close();
                }));
            }).Start();
        }

        public void SetModifyFinishedCallback(Action<int> callback)
        {
            this.modifyFinishedCallback = callback;
        }

        public void SetAddFinishedCallback(Action<int> callback)
        {
            this.addFinishedCallback = callback;
        }

        public void SetMode(FormMode mode)
        {
            this.mode = mode;
            if (mode == FormMode.ALTER)
            {
                this.Text = "修改用户信息";
                this.buttonOK.Text = "修改用户信息";
                this.groupBox1.Text = "修改用户信息";
            }
            else if (mode == FormMode.ADD)
            {
                this.Text = "添加用户信息";
                this.buttonOK.Text = "添加用户信息";
                this.groupBox1.Text = "添加用户信息";
            }
        }


        private void checkBoxAuthorityManager_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxAuthorityShipment_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxAuthorityReceipt_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxAuthorityStock_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxAuthoritySupplier_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void DeleteAuthorityName(TextBox textBox,string delText)
        {
            string srcText = textBox.Text;
            textBox.Text = srcText.Replace(delText, "");
        }

        private void addAuthorityName(TextBox textBox, string addName)
        {
            if(textBox.Text.Contains(addName) == false)
            {
                textBox.Text += addName;
            }
        }

        private void checkBoxAuthorityManager_Click(object sender, EventArgs e)
        {
            if (this.checkBoxAuthorityManager.Checked)
            {
                this.textBoxAuthorityName.Text = "管理员";
                this.checkBoxAuthorityReceipt.Enabled = false;
                this.checkBoxAuthorityReceipt.Checked = false;
                this.checkBoxAuthorityShipment.Enabled = false;
                this.checkBoxAuthorityShipment.Checked = false;
                this.checkBoxAuthorityStock.Enabled = false;
                this.checkBoxAuthorityStock.Checked = false;
                this.checkBoxAuthoritySupplier.Enabled = false;
                this.checkBoxAuthoritySupplier.Checked = false;
            }
            else
            {
                DeleteAuthorityName(this.textBoxAuthorityName, "管理员");
                this.checkBoxAuthorityReceipt.Enabled = true;
                this.checkBoxAuthorityShipment.Enabled = true;
                this.checkBoxAuthorityStock.Enabled = true;
                this.checkBoxAuthoritySupplier.Enabled = true;
            }
        }

        private void checkBoxAuthorityShipment_Click(object sender, EventArgs e)
        {
            if (this.checkBoxAuthorityShipment.Checked)
            {
                addAuthorityName(this.textBoxAuthorityName, "发货员");
            }
            else
            {
                DeleteAuthorityName(this.textBoxAuthorityName, "发货员");
            }
        }

        private void checkBoxAuthorityReceipt_Click(object sender, EventArgs e)
        {
            if (this.checkBoxAuthorityReceipt.Checked)
            {
                addAuthorityName(this.textBoxAuthorityName, "收货员");
            }
            else
            {
                DeleteAuthorityName(this.textBoxAuthorityName, "收货员");
            }
        }

        private void checkBoxAuthorityStock_Click(object sender, EventArgs e)
        {
            if (this.checkBoxAuthorityStock.Checked)
            {
                addAuthorityName(this.textBoxAuthorityName, "库存管理员");
            }
            else
            {
                DeleteAuthorityName(this.textBoxAuthorityName, "库存管理员");
            }
        }

        private void checkBoxAuthoritySupplier_Click(object sender, EventArgs e)
        {
            if (this.checkBoxAuthoritySupplier.Checked)
            {
                this.checkBoxAuthoritySupplier.Checked = false; //假设用户直接关闭了选择供应商的窗口，则不选中供应商权限
                var formSelectSupplier = new FormSelectSupplier();
                formSelectSupplier.SetSelectFinishedCallback((selectedID) =>
                {
                    WMSEntities wmsEntities = new WMSEntities();
                    string supplierName = null;
                    try
                    {
                        supplierName = (from s in wmsEntities.SupplierView
                                               where s.ID == selectedID
                                               select s.Name).FirstOrDefault();
                    }
                    catch
                    {
                        MessageBox.Show("无法连接服务器，请检查网络连接","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (supplierName == null)
                    {
                        MessageBox.Show("选择供应商失败，供应商不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    this.textBoxAuthorityName.Text = "供应商";
                    this.curSupplierID = selectedID;
                    this.Controls.Find("textBoxSupplierName", true)[0].Text = supplierName;
                    this.checkBoxAuthoritySupplier.Checked = true;
                    this.checkBoxAuthorityManager.Enabled = false;
                    this.checkBoxAuthorityManager.Checked = false;
                    this.checkBoxAuthorityReceipt.Enabled = false;
                    this.checkBoxAuthorityReceipt.Checked = false;
                    this.checkBoxAuthorityShipment.Enabled = false;
                    this.checkBoxAuthorityShipment.Checked = false;
                    this.checkBoxAuthorityStock.Enabled = false;
                    this.checkBoxAuthorityStock.Checked = false;
                });
                formSelectSupplier.Show();
            }
            else
            {
                DeleteAuthorityName(this.textBoxAuthorityName, "供应商");
                this.curSupplierID = -1;
                this.Controls.Find("textBoxSupplierName", true)[0].Text = "";
                this.checkBoxAuthorityReceipt.Enabled = true;
                this.checkBoxAuthorityShipment.Enabled = true;
                this.checkBoxAuthorityStock.Enabled = true;
                this.checkBoxAuthorityManager.Enabled = true;
            }
        }

        private void checkBoxAuthorityManager_CheckedChanged_1(object sender, EventArgs e)
        {

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

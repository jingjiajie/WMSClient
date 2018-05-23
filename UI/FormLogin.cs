using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI
{
    public partial class FormLogin : Form
    {
        Point mouseOff;//鼠标移动位置变量
        bool leftFlag;//标签是否为左键

        public FormLogin()
        {
            InitializeComponent();
            this.comboBoxAccountBook.Items.Add("测试账套");
            this.comboBoxAccountBook.SelectedIndex = 0;
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        //启用双缓冲技术
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            string personName = this.textBoxUsername.Text;
            string password = this.textBoxPassword.Text;
            string accountBook = "WMS_Template";
            if (string.IsNullOrWhiteSpace(personName))
            {
                MessageBox.Show("请填写用户名！","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }else if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("请填写密码！","提示", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            Condition condition = new Condition();
            condition.AddCondition("name",personName);
            condition.AddCondition("password", password);
            string condStr = condition.ToString();
            this.labelStatus.Visible = true;
            List<Dictionary<string,object>> personList = RestClient.Get<List<Dictionary<string,object>>>(Defines.ServerURL + "/ledger/" + accountBook + "/person/" + condStr);
            if (personList == null)
            {
                return;
            }
            else if (personList.Count == 0)
            {
                MessageBox.Show("登录失败，请检查用户名和密码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            GlobalData.Person = personList[0];
            GlobalData.AccountBook = accountBook;
            GlobalData.Warehouse = GlobalData.AllWarehouses[this.comboBoxWarehouse.SelectedIndex];
            this.RefreshAssociationData();
            this.labelStatus.Visible = false;
            new FormMain().Show();
            this.Hide();
        }

        private void buttonClosing_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxUsername_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBoxUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.textBoxPassword.Focus();
                this.textBoxPassword.SelectAll();
                return;
            }else if (e.KeyChar == 27) //ESC
            {
                this.Close();
            }
        }

        private void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.buttonEnter.Focus();
                this.buttonEnter.PerformClick();
                return;
            }
            else if (e.KeyChar == 27) //ESC
            {
                this.Close();
            }
        }

        private void FormLogin_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point workAreaPosition = this.PointToClient(Control.MousePosition);
                mouseOff = new Point(-workAreaPosition.X, -workAreaPosition.Y); //得到鼠标偏移量
                leftFlag = true;   //点击左键按下时标注为true;
            }
        }

        private void FormLogin_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }

        private void FormLogin_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }

        private void textBoxUsername_Leave(object sender, EventArgs e)
        {

        }

        private void FormLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 27) //ESC
            {
                this.Close();
            }
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确定退出吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void buttonEnter_MouseEnter(object sender, EventArgs e)
        {
            buttonEnter.BackgroundImage = WMS.UI.Properties.Resources.bottonB1_s;
        }

        private void buttonEnter_MouseLeave(object sender, EventArgs e)
        {
            buttonEnter.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        }

        private void buttonEnter_MouseDown(object sender, MouseEventArgs e)
        {
            buttonEnter.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private void buttonClosing_MouseEnter(object sender, EventArgs e)
        {
            buttonClosing.BackgroundImage = WMS.UI.Properties.Resources.bottonB4_s;
        }

        private void buttonClosing_MouseLeave(object sender, EventArgs e)
        {
            buttonClosing.BackgroundImage = WMS.UI.Properties.Resources.bottonB4_q;
        }

        private void buttonClosing_MouseDown(object sender, MouseEventArgs e)
        {
            buttonClosing.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_s;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormLogin_Shown(object sender, EventArgs e)
        {
            
        }

        private void FormLogin_Activated(object sender, EventArgs e)
        {
            this.textBoxUsername.Focus();
        }

        private void labelusername_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxAccountBook_SelectedIndexChanged(object sender, EventArgs e)
        {
            string accountBook = "WMS_Template";
            var warehouseList = RestClient.Get<List<IDictionary<string, object>>>(Defines.ServerURL + "/warehouse/" + accountBook + "/warehouse/"+
                new Condition().AddOrder("name"));
            if (warehouseList == null)
            {
                //MessageBox.Show("加载仓库信息失败，请检查网络连接！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }
            GlobalData.AllWarehouses = warehouseList;
            this.comboBoxWarehouse.Items.Clear();
            foreach(var warehouse in warehouseList)
            {
                this.comboBoxWarehouse.Items.Add(warehouse["name"].ToString());
            }
            this.comboBoxWarehouse.SelectedIndex = 0;
        }

        private void RefreshAssociationData()
        {

            Condition condWarehouse = new Condition().AddCondition("warehouseId",GlobalData.Warehouse["id"]);
            GlobalData.AllSuppliers = RestClient.Get<List<IDictionary<string, object>>>(
               $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/supplier/{condWarehouse.ToString()}");

            GlobalData.AllMaterials = RestClient.Get<List<IDictionary<string, object>>>(
               $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/material/{condWarehouse.ToString()}");

            GlobalData.AllSupplies = RestClient.Get<List<IDictionary<string, object>>>(
                $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/supply/{condWarehouse.ToString()}");

            GlobalData.AllStorageLocations = RestClient.Get<List<IDictionary<string, object>>>(
               $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/storage_location/{condWarehouse.ToString()}");

            GlobalData.AllStorageAreas = RestClient.Get<List<IDictionary<string, object>>>(
               $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/storage_area/{condWarehouse.ToString()}");
        }
    }
}


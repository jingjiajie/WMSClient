﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WMS.UI
{
    public partial class FormLogin : Form
    {
        Point mouseOff;//鼠标移动位置变量
        bool leftFlag;//标签是否为左键

        public FormLogin()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CN", true)
            {
                DateTimeFormat = {
                    ShortDatePattern = "yyyy-MM-dd",
                    FullDateTimePattern = "yyyy-MM-dd HH:mm:ss",
                    LongTimePattern = "HH:mm:ss" 
                }
            };

            InitializeComponent();
            this.comboBoxAccountBook.Items.Add("默认账套");
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
            FormMain formMain = new FormMain();
            formMain.FormClosed += (s,ev) => { this.Dispose(); };
            formMain.Show();
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
                new Condition().AddOrder("name",OrderItemOrder.DESC));
            if (warehouseList == null)
            {
                this.Close();
                Environment.Exit(0);
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
            
            GlobalData.AllDate = RestClient.Get<IDictionary<string, object[]>>(
            Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/refreshGlobalDate" + "/" + GlobalData.Warehouse["id"]);
            if (GlobalData.AllDate == null)
            {
                MessageBox.Show("请检查网络连接！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            foreach (KeyValuePair<string, Object[]> allData in GlobalData.AllDate)
            {
                string key = allData.Key;
                Object[] data = allData.Value;
                var list = (from x in data.ToList() select (IDictionary < string,object>)x).ToList();
                switch (key)
                {
                    case "AllSuppliers":          
                        GlobalData.AllSuppliers = list;
                        break;
                    case "AllMaterial":
                        GlobalData.AllMaterials = list;
                        break;
                    case "AllSupply":
                        GlobalData.AllSupplies = list;
                        break;
                    case "AllStorageLocation":
                        GlobalData.AllStorageLocations = list;
                        break;
                    case "AllStorageArea":
                        GlobalData.AllStorageAreas = list;
                        break;
                    case "AllPersons":
                        GlobalData.AllPersons = list;
                        break;
                    case "AllDestinations":
                        GlobalData.AllDestinations = list;
                        break;
                    case "AllPackage":
                        GlobalData.AllPackage = list;
                        break;
                    case "AllSalaryItem":
                        GlobalData.AllSalaryItem = list;
                        break;
                    case "AllSalaryType":
                        GlobalData.AllSalaryType = list;
                        break;
                    case "AllSummaryNote":
                        GlobalData.AllSummaryNote = list;
                        break;
                    case "AllSalaryPeriod":
                        GlobalData.AllSalaryPeriod = list;
                        break;
                    case "AllAccountTitle":
                        GlobalData.AllAccountTitle = list;
                        break;
                    case "AllAccountTitleTrue":
                        GlobalData.AllAccountTitleTure = list;
                        break;
                    case "AllTax":
                        GlobalData.AllTax = list;
                        break;
                    case "AllAccountPeriod":
                        GlobalData.AllAccountPeriod = list;
                        break;
                    case "AccountPeriod":
                        if (list.Count == 1)
                        {
                            GlobalData.AccountPeriod = list[0];
                        }
                        else 
                        {
                            GlobalData.AccountPeriod = null;
                        }
                        break;
                }
            }
            
            //Condition condWarehouse = new Condition().AddCondition("warehouseId", GlobalData.Warehouse["id"]);
            //Condition condSalaryPeriod = new Condition().AddCondition("warehouseId", GlobalData.Warehouse["id"]);
            //Condition cond = new Condition();
            //Condition condAccountTitle = new Condition();
            //GlobalData.AllSuppliers = RestClient.Get<List<IDictionary<string, object>>>(
            //   $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/supplier/{condWarehouse.ToString()}/new");

            //GlobalData.AllMaterials = RestClient.Get<List<IDictionary<string, object>>>(
            //   $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/material/{condWarehouse.ToString()}");

            //GlobalData.AllSupplies = RestClient.Get<List<IDictionary<string, object>>>(
            //    $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/supply/{condWarehouse.ToString()}");

            //GlobalData.AllStorageLocations = RestClient.Get<List<IDictionary<string, object>>>(
            //   $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/storage_location/{condWarehouse.ToString()}");

            //GlobalData.AllStorageAreas = RestClient.Get<List<IDictionary<string, object>>>(
            //   $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/storage_area/{condWarehouse.ToString()}");

            //GlobalData.AllPersons = RestClient.Get<List<IDictionary<string, object>>>(
            //   $"{Defines.ServerURL}/ledger/{GlobalData.AccountBook}/person/{{}}");

            //GlobalData.AllPackage = RestClient.Get<List<IDictionary<string, object>>>(
            //  $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/package/{condWarehouse.ToString()}");

            //GlobalData.AllSalaryItem = RestClient.Get<List<IDictionary<string, object>>>(
            // $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/salary_item/{condWarehouse.ToString()}");

            //GlobalData.AllSalaryType = RestClient.Get<List<IDictionary<string, object>>>(
            // $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/salary_type/{condWarehouse.ToString()}");

            //GlobalData.AllSummaryNote = RestClient.Get<List<IDictionary<string, object>>>(
            // $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/summary_note/{condWarehouse.ToString()}");

            //GlobalData.AllSalaryPeriod = RestClient.Get<List<IDictionary<string, object>>>(
            // $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/salary_period/{condSalaryPeriod.AddOrder("endTime", OrderItemOrder.DESC).ToString()}");

            //GlobalData.AllAccountTitle = RestClient.Get<List<IDictionary<string, object>>>(
            //$"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/pay_note/{condAccountTitle}/find_son");

            //GlobalData.AllAccountTitleTure = RestClient.Get<List<IDictionary<string, object>>>(
            //$"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/account_title/{condAccountTitle.ToString()}");

            //GlobalData.AllTax = RestClient.Get<List<IDictionary<string, object>>>(
            //$"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/tax/{{}}");

            //GlobalData.AllAccountPeriod = RestClient.Get<List<IDictionary<string, object>>>(
            //    $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/account_period/{condWarehouse.AddOrder("startTime", OrderItemOrder.DESC).ToString()}");

            //try
            //{
            //    GlobalData.AccountPeriod = RestClient.Get<List<IDictionary<string, object>>>(
            //       $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/account_period/{condWarehouse.AddCondition("ended", 0).AddOrder("startTime", OrderItemOrder.DESC).ToString()}")[0];
            //}
            //catch { GlobalData.AccountPeriod = null; }
        }
    }
}


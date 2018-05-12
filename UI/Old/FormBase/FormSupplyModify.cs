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
    public partial class FormSupplyModify : Form
    {
        private int projectID = -1;
        private int warehouseID = -1;
        private int userID = -1;
        private int supplyID = -1;
        private int supplierID = -1;
        private int componenID = -1;
        private Func<int> SupplierIDGetter = null;
        private Func<int> ComponenIDGetter = null;
        private WMSEntities wmsEntities = new WMSEntities();
        private Action<int> modifyFinishedCallback = null;
        private Action<int> addFinishedCallback = null;
        private FormMode mode = FormMode.ALTER;
        private int history_save = 0;
        private TextBox textboxCreateUserUsername = null;

        public FormSupplyModify(int projectID, int warehouseID, int supplierID, int userID, int supplyID = -1)
        {
            InitializeComponent();
            this.warehouseID = warehouseID;
            this.userID = userID;
            this.projectID = projectID;
            this.supplierID = supplierID;
            this.supplyID = supplyID;
        }
        
        private void FormSupplyModify_Load(object sender, EventArgs e)
        { 
            if (this.mode == FormMode.ALTER && this.supplyID == -1)
            {
                throw new Exception("未设置源零件信息");
            }

            Utilities.CreateEditPanel(this.tableLayoutPanelTextBoxes, SupplyViewMetaData.supplykeyNames);
            TextBox textboxsuppliername = (TextBox)this.Controls.Find("textBoxSupplierName", true)[0];

            TextBox textBoxComponentName = (TextBox)this.Controls.Find("textBoxComponentName", true)[0];
            TextBox textboxLastUpdateUserUsername = (TextBox)this.Controls.Find("textBoxLastUpdateUserUsername", true)[0];
            this.textboxCreateUserUsername = (TextBox)this.Controls.Find("textBoxCreateUserUsername", true)[0];
            textboxsuppliername.ReadOnly = true;


            textboxLastUpdateUserUsername.ReadOnly = true;
            textboxCreateUserUsername.ReadOnly = true;
            textBoxComponentName.ReadOnly = true;
            TextBox textBoxSupplierName = (TextBox)this.Controls.Find("textBoxSupplierName", true)[0];
            
            textBoxSupplierName.BackColor = Color.White;
            textBoxComponentName.BackColor = Color.White;

            this.SupplierIDGetter = Utilities.BindTextBoxSelect<FormSelectSupplier, Supplier>(this, "textBoxSupplierName", "Name");
            this.ComponenIDGetter = Utilities.BindTextBoxSelect<FormSelectComponen, DataAccess.Component>(this, "textBoxComponentName", "Name");

            if (this.mode == FormMode.ALTER)
            {
               
                try
                {
                    SupplyView supplyView = (from s in this.wmsEntities.SupplyView
                                                  where s.ID == this.supplyID
                                                  select s).FirstOrDefault();
                    Utilities.CopyPropertiesToTextBoxes(supplyView, this);
                }
                catch (Exception)
                {
                    MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

            }
            else if (this.mode == FormMode.ADD)
            {
                this.Text = "添加供货信息";
                try
                {
                    User user = (from u in wmsEntities.User
                                 where u.ID == this.userID
                                 select u).FirstOrDefault();
                    if (user == null)
                    {
                        MessageBox.Show("登录用户不存在，请重新登录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    this.textboxCreateUserUsername.Text = user.Username;
                    //this.textBoxCreateTime.Text = DateTime.Now.ToString();
                }
                catch
                {
                    MessageBox.Show("加载失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            //this.Controls.Find("textBoxSupplierName", true)[0].Click += textBoxSupplierName_Click;
            //this.Controls.Find("textBoxComponentName", true)[0].TextChanged += textBoxComponentName_TextChanged;
        }
        private void textBoxSupplierName_Click(object sender, EventArgs e)
        {
           
            var formSelectSupplier = new FormSelectSupplier();
            formSelectSupplier.SetSelectFinishedCallback((selectedID) =>
            {
                WMSEntities wmsEntities = new WMSEntities();
                var supplierName = (from s in wmsEntities.SupplierView
                                       where s.ID == selectedID
                                       select s).FirstOrDefault();
                if (supplierName.Name == null)
                {
                    MessageBox.Show("选择供应商失败，供应商不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                this.supplierID = selectedID;
                this.Controls.Find("textBoxSupplierName", true)[0].Text = supplierName.Name;
                //this.Controls.Find("textBoxSupplierNumber", true)[0].Text = supplierName.Number;
            });
            formSelectSupplier.Show();
            
        }
        private void textBoxComponentName_TextChanged(object sender, EventArgs e)
        {

            int a = ComponenIDGetter();
            WMSEntities wmsEntities = new WMSEntities();
                var componenName = (from s in wmsEntities.ComponentView
                                    where s.ID == a
                                    select s).FirstOrDefault();

                this.Controls.Find("textBoxDefaultReceiptUnit", true)[0].Text = componenName.DefaultReceiptUnit;
                this.Controls.Find("textBoxDefaultReceiptUnitAmount", true)[0].Text = Convert.ToString(componenName.DefaultReceiptUnitAmount);
                this.Controls.Find("textBoxDefaultShipmentUnit", true)[0].Text = componenName.DefaultShipmentUnit;
                this.Controls.Find("textBoxDefaultShipmentUnitAmount", true)[0].Text = Convert.ToString(componenName.DefaultShipmentUnitAmount);


        }
        private void textBoxComponentName_Click(object sender, EventArgs e)
        {

            var formSelectComponent = new FormSelectComponen();
            formSelectComponent.SetSelectFinishedCallback((selectedID) =>
            {
                WMSEntities wmsEntities = new WMSEntities();
                var componenName = (from s in wmsEntities.ComponentView
                                    where s.ID == selectedID
                                    select s).FirstOrDefault();
                if (componenName.Name == null)
                {
                    MessageBox.Show("选择零件失败，零件不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                this.componenID = selectedID;
                this.Controls.Find("textBoxComponentName", true)[0].Text = componenName.Name;
                this.Controls.Find("textBoxDefaultReceiptUnit", true)[0].Text = componenName.DefaultReceiptUnit;
                this.Controls.Find("textBoxDefaultReceiptUnitAmount", true)[0].Text = Convert.ToString(componenName.DefaultReceiptUnitAmount);
                this.Controls.Find("textBoxDefaultShipmentUnit", true)[0].Text = componenName.DefaultShipmentUnit;
                this.Controls.Find("textBoxDefaultShipmentUnitAmount", true)[0].Text = Convert.ToString(componenName.DefaultShipmentUnitAmount);
            });
            formSelectComponent.Show();

        }

        
        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult messageBoxResult = DialogResult.No;//设置对话框的返回值
            var textBoxSupplierName = this.Controls.Find("textBoxSupplierName", true)[0];
            var textBoxNo = this.Controls.Find("textBoxNo", true)[0];
            var textBoxComponentName = this.Controls.Find("textBoxComponentName", true)[0];

            //询问是否保留历史信息
            if (this.mode == FormMode.ALTER && this.history_save == 0)
            {


                messageBoxResult = MessageBox.Show("是否要保留历史信息", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,

                MessageBoxDefaultButton.Button2);
            }
            if (textBoxNo.Text == string.Empty)
            {
                MessageBox.Show("零件代号不能为空！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBoxComponentName.Text == string.Empty)
            {
                MessageBox.Show("零件名称不能为空！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                string componentName = textBoxComponentName.Text;
                try
                {
                    DataAccess.Component componenID1 = (from s in this.wmsEntities.Component where s.Name == componentName select s).FirstOrDefault();

                    this.componenID = componenID1.ID;
                }
                catch
                {

                }
            }


            if (textBoxSupplierName.Text == string.Empty)
            {
                MessageBox.Show("请选择供应商！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                string supplierName = textBoxSupplierName.Text;
                try
                {
                    Supplier supplierID = (from s in this.wmsEntities.Supplier where s.Name == supplierName && s.IsHistory == 0 select s).FirstOrDefault();

                    this.supplierID = supplierID.ID;
                }
                catch
                {

                }
            }




            //if (this.supplierID == -1)
            //{
            //    MessageBox.Show("请输入正确的供应商名称！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            Supply supply = null;
            if (this.mode == FormMode.ALTER)
            {

                try
                {
                    supply = (from s in this.wmsEntities.Supply
                                       where s.ID == this.supplyID
                                       select s).FirstOrDefault();
                }
                catch
                {
                    MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (supply == null)
                {
                    MessageBox.Show("历史零件信息不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    var sameNameUsers = (from u in wmsEntities.Supply
                                         where 
                                        
                                         u.No==textBoxNo.Text
                                            && u.ID != supply.ID && u.IsHistory == 0
                                            &&u.ProjectID ==this.projectID &&u.WarehouseID ==this.warehouseID  
                                         select u).ToArray();
                    if (sameNameUsers.Length > 0)
                    {
                        MessageBox.Show("修改供应信息失败，已存在相同代号条目：" + textBoxNo.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                catch
                {

                    MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //try
                //{
                //    var sameNameUsers = (from u in wmsEntities.Supply
                //                         where u.SupplierID == supplierID
                //                         && u.ComponentID == this.componenID
                //                         && u.No == textBoxNo.Text
                //                            && u.ID != supply.ID && u.IsHistory == 0
                //                            && u.ProjectID == this.projectID && u.WarehouseID == this.warehouseID
                //                         select u).ToArray();
                //    if (sameNameUsers.Length > 0)
                //    {
                //        MessageBox.Show("修改供应信息失败，已存在同名供应商——零件供货条目：" + textBoxSupplierName.Text + "——" + textBoxComponentName.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        return;
                //    }
                //}
                //catch
                //{

                //    MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}


                if (messageBoxResult == DialogResult.Yes)//如果对话框的返回值是YES（按"Y"按钮）
                {
                    //新建零件保留历史信息
                    this.wmsEntities.Supply.Add(supply);

                    try
                    {
                        supply.ID = -1;
                        supply.IsHistory = 1;
                        supply.NewestSupplyID = this.supplyID;
                        supply.LastUpdateUserID = this.userID;
                        supply.LastUpdateTime = DateTime.Now;
                        wmsEntities.SaveChanges();
                    }
                    catch
                    {
                        MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var supplystorge = (from u in wmsEntities.Supply
                                          where u.NewestSupplyID == this.supplyID
                                          select u).ToArray();


                    if (supplystorge.Length > 0)
                    {


                                    MessageBox.Show("历史信息保留成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                this.history_save = 1;
                    }



                    try
                    {
                        supply = (from s in this.wmsEntities.Supply
                                    where s.ID == this.supplyID
                                    select s).FirstOrDefault();
                    }
                    catch
                    {
                        MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (supply == null)
                    {
                        MessageBox.Show("零件信息不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                supply.ProjectID = this.projectID;
                supply.WarehouseID = this.warehouseID;
                supply.SupplierID = this.supplierID;
                supply.ComponentID = this.componenID;


                //开始数据库操作
                if (Utilities.CopyTextBoxTextsToProperties(this, supply, SupplyViewMetaData.supplykeyNames, out string errorMessage) == false)
                {
                    MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    Utilities.CopyComboBoxsToProperties(this, supply, SupplyViewMetaData.KeyNames);
                }
                supply.LastUpdateUserID = this.userID;
                supply.LastUpdateTime = DateTime.Now;
                supply.IsHistory = 0;
                wmsEntities.SaveChanges();

            }
            else if (mode == FormMode.ADD)
            {
                try
                {
                    var sameNameUsers = (from u in wmsEntities.Supply
                                         where 
                                         
                                          u.No == textBoxNo.Text
                                         && u.ProjectID == this.projectID && u.WarehouseID == this.warehouseID
                                         select u).ToArray();
                    if (sameNameUsers.Length > 0)
                    {
                        MessageBox.Show("修改供应信息失败，已存在相同代号条目：" + textBoxNo.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                catch
                {

                    MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                supply = new DataAccess.Supply();

                supply.CreateUserID = this.userID;
                //supply.CreateTime = DateTime.Now;
                supply.ProjectID = this.projectID;
                supply.WarehouseID = this.warehouseID;
                supply.SupplierID = this.supplierID;
                supply.ComponentID = this.componenID;

                //把选择零件的默认包装信息存进供应表
                DataAccess.Component componenID = (from s in this.wmsEntities.Component where s.ID == this.componenID select s).FirstOrDefault();
                PropertyInfo[] proAs = componenID.GetType().GetProperties();
                PropertyInfo[] proBs = supply.GetType().GetProperties();
                for (int i = 0; i < proAs.Length; i++)
                {
                    for (int j = 0; j < proBs.Length; j++)
                    {
                        if (proAs[i].Name == "Default" + proBs[j].Name)
                        {
                            object a = proAs[i].GetValue(componenID, null);
                            proBs[j].SetValue(supply, a, null);
                        }
                    }
                }


                //开始数据库操作
                if (Utilities.CopyTextBoxTextsToProperties(this, supply, SupplyViewMetaData.supplykeyNames, out string errorMessage) == false)
                {
                    MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    Utilities.CopyComboBoxsToProperties(this, supply, SupplyViewMetaData.KeyNames);
                }
                supply.LastUpdateUserID = this.userID;
                supply.LastUpdateTime = DateTime.Now;
                supply.IsHistory = 0;
                this.wmsEntities.Supply.Add(supply);
                wmsEntities.SaveChanges();

            }



            //supply.ProjectID = this.projectID;
            //supply.WarehouseID = this.warehouseID;
            //supply.SupplierID = this.supplierID;
            //supply.ComponentID = this.componenID;


            ////开始数据库操作
            //if (Utilities.CopyTextBoxTextsToProperties(this, supply, SupplyViewMetaData.supplykeyNames, out string errorMessage) == false)
            //{
            //    MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //else
            //{
            //    Utilities.CopyComboBoxsToProperties(this, supply, SupplyViewMetaData.KeyNames);
            //}
            //supply.LastUpdateUserID = this.userID;
            //supply.LastUpdateTime = DateTime.Now;
            //supply.IsHistory = 0;
            //wmsEntities.SaveChanges();

            //调用回调函数
            if (this.mode == FormMode.ALTER && this.modifyFinishedCallback != null)
            {
                this.modifyFinishedCallback(supply.ID);
                MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else if(this.mode == FormMode.ADD && this.addFinishedCallback != null)
            {
                this.addFinishedCallback(supply.ID);
                MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            
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
            if(mode == FormMode.ALTER)
            {
                this.Text = "修改零件信息";
                this.groupBox1.Text = "修改零件信息";
                this.buttonOK.Text = "修改零件信息";
            }else if (mode == FormMode.ADD)
            {
                this.Text = "添加零件信息";
                this.groupBox1.Text = "添加零件信息";
                this.buttonOK.Text = "添加零件信息";
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

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

namespace WMS.UI
{
    public partial class FormShipmentTicketModify : Form
    {
        private int shipmentTicketID = -1;
        private int projectID = -1;
        private int warehouseID = -1;
        private int userID = -1;
        //private int curSupplierID = -1;
        private int curPersonID = -1;
        private Action<int> modifyFinishedCallback = null;
        private Action<int,bool> addFinishedCallback = null;
        private FormMode mode = FormMode.ALTER;

        //private TextBox textBoxSupplierName = null;
        private TextBox textBoxPersonName = null;
        private TextBox textBoxCreateUserUsername = null;
        private TextBox textBoxCreateTime = null;

        public FormShipmentTicketModify(int projectID,int warehouseID, int userID,int shipmentTicketID = -1)
        {
            InitializeComponent();
            this.shipmentTicketID = shipmentTicketID;
            this.userID = userID;
            this.projectID = projectID;
            this.warehouseID = warehouseID;
        }

        private void FormShipmentTicketModify_Load(object sender, EventArgs e)
        {
            if (this.mode == FormMode.ALTER && this.shipmentTicketID == -1)
            {
                throw new Exception("未设置源发货单信息");
            }

            Utilities.CreateEditPanel(this.tableLayoutPanelTextBoxes, ShipmentTicketViewMetaData.KeyNames);
            //this.textBoxSupplierName = (TextBox)this.Controls.Find("textBoxSupplierName", true)[0];
            //textBoxSupplierName.BackColor = Color.White;
            //textBoxSupplierName.Click += textBoxSupplierName_Click;

            this.textBoxCreateUserUsername = (TextBox)this.Controls.Find("textBoxCreateUserUsername", true)[0];
            this.textBoxCreateTime = (TextBox)this.Controls.Find("textBoxCreateTime", true)[0];
            this.textBoxPersonName = (TextBox)this.Controls.Find("textBoxPersonName", true)[0];
            textBoxPersonName.ReadOnly = true;
            textBoxPersonName.BackColor = Color.White;
            textBoxPersonName.Click += textBoxPersonName_Click;

            WMSEntities wmsEntities = new WMSEntities();
            if (this.mode == FormMode.ALTER)
            {
                this.Text = "修改发货单信息";
                ShipmentTicketView shipmentTicketView = null;
                try
                {
                    shipmentTicketView = (from s in wmsEntities.ShipmentTicketView
                                                             where s.ID == this.shipmentTicketID
                                                             select s).FirstOrDefault();
                }
                catch
                {
                    MessageBox.Show("加载数据失败，请检查网络连接","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (this.IsDisposed == false)
                    {
                        this.Invoke(new Action(this.Close));
                    }
                    return;
                }
                if(shipmentTicketView == null)
                {
                    MessageBox.Show("修改失败，发货单不存在","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                Utilities.CopyPropertiesToTextBoxes(shipmentTicketView, this);
                Utilities.CopyPropertiesToComboBoxes(shipmentTicketView, this);
                //if (shipmentTicketView.SupplierID.HasValue)
                //{
                //    this.curSupplierID = shipmentTicketView.SupplierID.Value;
                //}
            }
            else if(this.mode == FormMode.ADD)
            {
                this.Text = "添加发货单";
                try
                {
                    User user = (from u in wmsEntities.User
                                 where u.ID == this.userID
                                 select u).FirstOrDefault();
                    if(user == null)
                    {
                        MessageBox.Show("登录用户不存在，请重新登录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    this.textBoxCreateUserUsername.Text = user.Username;
                    this.textBoxCreateTime.Text = DateTime.Now.ToString();
                }
                catch
                {
                    MessageBox.Show("加载失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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

        //private void textBoxSupplierName_Click(object sender, EventArgs e)
        //{
        //    FormSelectSupplier form = new FormSelectSupplier();
        //    form.SetSelectFinishCallback((supplierID)=>
        //    {
        //        if (this.IsDisposed) return;
        //        WMSEntities wmsEntities = new WMSEntities();
        //        try
        //        {
        //            SupplierView supplierView = (from s in wmsEntities.SupplierView
        //                                         where s.ID == supplierID
        //                                         select s).FirstOrDefault();
        //            if(supplierView == null)
        //            {
        //                MessageBox.Show("供应商不存在，请重新选择", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                return;
        //            }
        //            this.curSupplierID = supplierID;
        //            this.textBoxSupplierName.Text = supplierView.Name;
        //        }
        //        catch
        //        {
        //            MessageBox.Show("加载供应商信息失败，请检查网络连接","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //    });
        //    form.Show();
        //}

        private void buttonOK_Click(object sender, EventArgs e)
        {
            ShipmentTicket shipmentTicket = null;
            WMSEntities wmsEntities = new WMSEntities();
            //若修改，则查询原对象。若添加，则新建一个对象。
            if (this.mode == FormMode.ALTER)
            {
                try
                {
                    shipmentTicket = (from s in wmsEntities.ShipmentTicket
                                      where s.ID == this.shipmentTicketID
                                      select s).FirstOrDefault();
                }
                catch
                {
                    MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (shipmentTicket == null)
                {
                    MessageBox.Show("修改失败，发货单不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                shipmentTicket.LastUpdateUserID = this.userID;
                shipmentTicket.LastUpdateTime = DateTime.Now;
            }
            else if (mode == FormMode.ADD)
            {
                shipmentTicket = new ShipmentTicket();
                shipmentTicket.CreateTime = DateTime.Now;
                shipmentTicket.CreateUserID = this.userID;
                wmsEntities.ShipmentTicket.Add(shipmentTicket);
            }

            shipmentTicket.ProjectID = this.projectID;
            shipmentTicket.WarehouseID = this.warehouseID;
            //if(this.curSupplierID == -1)
            //{
            //    MessageBox.Show("请选择供应商！","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //shipmentTicket.SupplierID = this.curSupplierID;
            if(this.curPersonID == -1)
            {
                shipmentTicket.PersonID = null;
            }
            else
            {
                shipmentTicket.PersonID = this.curPersonID;
            }

            //开始数据库操作
            if (Utilities.CopyTextBoxTextsToProperties(this, shipmentTicket, ShipmentTicketViewMetaData.KeyNames, out string errorMessage) == false)
            {
                MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                Utilities.CopyComboBoxsToProperties(this, shipmentTicket, ShipmentTicketViewMetaData.KeyNames);
            }

            new Thread(() =>
            {
                //生成单号和编号
                try
                {
                    if (string.IsNullOrWhiteSpace(shipmentTicket.No))
                    {
                        if (shipmentTicket.CreateTime.HasValue == false)
                        {
                            MessageBox.Show("单号生成失败（未知创建日期）！请手动填写单号");
                            return;
                        }

                        DateTime createDay = new DateTime(shipmentTicket.CreateTime.Value.Year, shipmentTicket.CreateTime.Value.Month, shipmentTicket.CreateTime.Value.Day);
                        DateTime nextDay = createDay.AddDays(1);
                        int maxRankOfToday = Utilities.GetMaxTicketRankOfDay((from s in wmsEntities.ShipmentTicket
                                                                              where s.CreateTime >= createDay && s.CreateTime < nextDay
                                                                              select s.No).ToArray());
                        if (maxRankOfToday == -1)
                        {
                            MessageBox.Show("单号生成失败！请手动填写单号");
                            return;
                        }
                        shipmentTicket.No = Utilities.GenerateTicketNo("F", shipmentTicket.CreateTime.Value, maxRankOfToday + 1);
                    }
                    //if (string.IsNullOrWhiteSpace(shipmentTicket.Number))
                    //{
                    //    if (shipmentTicket.CreateTime.HasValue == false)
                    //    {
                    //        MessageBox.Show("编号生成失败（未知创建日期）！请手动填写编号");
                    //        return;
                    //    }
                    //    Supplier supplier = (from s in wmsEntities.Supplier
                    //                         where s.ID == shipmentTicket.SupplierID
                    //                         select s).FirstOrDefault();
                    //    if (supplier == null)
                    //    {
                    //        MessageBox.Show("编号生成失败（供应商信息不存在）！请手动填写编号");
                    //        return;
                    //    }
                    //    DateTime createMonth = new DateTime(shipmentTicket.CreateTime.Value.Year, shipmentTicket.CreateTime.Value.Month, 1);
                    //    DateTime nextMonth = createMonth.AddMonths(1);
                    //    var tmp = (from s in wmsEntities.ShipmentTicket
                    //               where s.CreateTime >= createMonth &&
                    //                     s.CreateTime < nextMonth &&
                    //                     s.SupplierID == shipmentTicket.SupplierID
                    //               select s.Number).ToArray();
                    //    int maxRankOfMonth = Utilities.GetMaxTicketRankOfSupplierAndMonth(tmp);
                    //    shipmentTicket.Number = Utilities.GenerateTicketNumber(supplier.Number, shipmentTicket.CreateTime.Value, maxRankOfMonth + 1);
                    //}
                    wmsEntities.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Invoke(new Action(() =>
                    {
                    //调用回调函数
                    if (this.mode == FormMode.ALTER && this.modifyFinishedCallback != null)
                        {
                            this.modifyFinishedCallback(shipmentTicket.ID);
                            MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (this.mode == FormMode.ADD && this.addFinishedCallback != null)
                        {
                            bool openTicket = false;
                            if(MessageBox.Show("添加成功！是否添加零件？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                openTicket = true;
                            }
                            this.addFinishedCallback(shipmentTicket.ID,openTicket);
                        }
                        this.Close();
                    }));
            }).Start();
        }

        public void SetModifyFinishedCallback(Action<int> callback)
        {
            this.modifyFinishedCallback = callback;
        }

        public void SetAddFinishedCallback(Action<int,bool> callback)
        {
            this.addFinishedCallback = callback;
        }

        public void SetMode(FormMode mode)
        {
            this.mode = mode;
            if (mode == FormMode.ALTER)
            {
                this.Text = "修改发货单";
                this.buttonOK.Text = "修改发货单";
            }
            else if (mode == FormMode.ADD)
            {
                this.Text = "添加发货单";
                this.buttonOK.Text = "添加发货单";
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

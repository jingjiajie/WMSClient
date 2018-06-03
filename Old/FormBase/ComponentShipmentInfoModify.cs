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
    public partial class ComponentShipmentInfoModify : Form
    {

        private int ID = -1;
        private int supplierID = -1;
        private int userID = -1;
        private int setItem = -1;
        const int SUPPLY_SHIPMENT = 0;
        const int COMPONENT_SHIPMENTINFO = 1;
        private WMSEntities wmsEntities = new WMSEntities();
        private Action<int> modifyFinishedCallback = null;
        private Action<int> addFinishedCallback = null;
        private FormMode mode = FormMode.ALTER;

        public ComponentShipmentInfoModify(int userID, int setItem, int ID = -1)
        {
            InitializeComponent();
            this.setItem = setItem;
            this.ID = ID;
            this.userID = userID;
        }
        
        private void ComponentShipmentInfoModify_Load(object sender, EventArgs e)
        {
            if (this.setItem == SUPPLY_SHIPMENT)
            {
                if (this.mode == FormMode.ALTER && this.ID == -1)
                {
                    throw new Exception("未设置源零件信息");
                }

                Utilities.CreateEditPanel(this.tableLayoutPanelTextBoxes, SupplyViewMetaData.supplyShipmentInfokeyNames);

                if (this.mode == FormMode.ALTER || this.mode == FormMode.CHECK)
                {

                    try
                    {
                        DataAccess.Supply supply = (from s in this.wmsEntities.Supply
                                                    where s.ID == this.ID
                                                    select s).FirstOrDefault();
                        Utilities.CopyPropertiesToTextBoxes(supply, this);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }

                }
            }
            if (this.setItem == COMPONENT_SHIPMENTINFO)
            {
                if (this.mode == FormMode.ALTER && this.ID == -1)
                {
                    throw new Exception("未设置源零件信息");
                }

                Utilities.CreateEditPanel(this.tableLayoutPanelTextBoxes, ComponenViewMetaData.ComponentShipmentInfokeyNames);

                if (this.mode == FormMode.ALTER || this.mode == FormMode.CHECK)
                {

                    try
                    {
                        DataAccess.Component componen = (from s in this.wmsEntities.Component
                                                         where s.ID == this.ID
                                                         select s).FirstOrDefault();
                        Utilities.CopyPropertiesToTextBoxes(componen, this);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }

                }
            }
            if (this.mode == FormMode.CHECK)
            {
                this.buttonOK.Visible = false;
            }
        }
       

        private void buttonOK_Click(object sender, EventArgs e)
        {

            if (this.setItem == SUPPLY_SHIPMENT)
            {
                DialogResult MsgBoxResult = DialogResult.No;//设置对话框的返回值

                //询问是否保留历史信息
                if (this.mode == FormMode.ALTER)
                {


                    MsgBoxResult = MessageBox.Show("是否要保留历史信息", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,

                    MessageBoxDefaultButton.Button2);
                }
                DataAccess.Supply supply = null;
                if (this.mode == FormMode.ALTER)
                {

                    try
                    {
                        supply = (from s in this.wmsEntities.Supply
                                  where s.ID == this.ID
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


                    if (MsgBoxResult == DialogResult.Yes)//如果对话框的返回值是YES（按"Y"按钮）
                    {
                        //新建零件保留历史信息
                        this.wmsEntities.Supply.Add(supply);

                        try
                        {
                            supply.ID = -1;
                            supply.IsHistory = 1;
                            supply.NewestSupplyID = this.ID;
                            supply.LastUpdateUserID = this.userID;
                            supply.LastUpdateTime = DateTime.Now;
                            wmsEntities.SaveChanges();
                        }
                        catch
                        {
                            MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }


                        try
                        {
                            supply = (from s in this.wmsEntities.Supply
                                      where s.ID == this.ID
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

                }


                //开始数据库操作
                if (Utilities.CopyTextBoxTextsToProperties(this, supply, SupplyViewMetaData.supplyShipmentInfokeyNames, out string errorMessage) == false)
                {
                    MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    Utilities.CopyComboBoxsToProperties(this, supply, SupplyViewMetaData.supplyShipmentInfokeyNames);
                }

                supply.LastUpdateUserID = this.userID;
                supply.LastUpdateTime = DateTime.Now;
                supply.IsHistory = 0;
                wmsEntities.SaveChanges();
                //调用回调函数
                if (this.mode == FormMode.ALTER && this.modifyFinishedCallback != null)
                {
                    this.modifyFinishedCallback(supply.ID);
                    MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }

            if (this.setItem == COMPONENT_SHIPMENTINFO)
            {
                DataAccess.Component componen = null;
                if (this.mode == FormMode.ALTER)
                {

                    try
                    {
                        componen = (from s in this.wmsEntities.Component
                                    where s.ID == this.ID
                                    select s).FirstOrDefault();
                    }
                    catch
                    {
                        MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (componen == null)
                    {
                        MessageBox.Show("历史零件信息不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                }


                //开始数据库操作
                if (Utilities.CopyTextBoxTextsToProperties(this, componen, ComponenViewMetaData.ComponentShipmentInfokeyNames, out string errorMessage) == false)
                {
                    MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    Utilities.CopyComboBoxsToProperties(this, componen, ComponenViewMetaData.ComponentShipmentInfokeyNames);
                }

                wmsEntities.SaveChanges();
                //调用回调函数
                if (this.mode == FormMode.ALTER && this.modifyFinishedCallback != null)
                {
                    this.modifyFinishedCallback(componen.ID);
                    MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
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
                this.Text = "修改零件出货信息";
                this.groupBox1.Text = "修改零件出货信息";
                this.buttonOK.Text = "修改零件出货信息";
            }
            else if (mode == FormMode.CHECK)
            {
                this.Text = "零件出货信息";
                this.groupBox1.Text = "零件出货信息";
                this.buttonOK.Text = "零件出货信息";
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

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

namespace WMS.UI.FormBase
{
    public partial class FormBaseWarehouseModify : Form
    {
        private int setItem = -1;
        const int WAREHOUSE_MODIFY = 0;
        const int PackageUnit_MODIFY = 1;
        private int modifyID = -1;
        private WMSEntities wmsEntities = new WMSEntities();
        private Action<int> modifyFinishedCallback = null;
        private Action<int> addFinishedCallback = null;
        private FormMode mode = FormMode.ALTER;

        public FormBaseWarehouseModify(int setItem, int modifyID = -1)
        {
            InitializeComponent();
            this.modifyID = modifyID;
            this.setItem = setItem;
        }

        private void FormBaseWarehouseModify_Load(object sender, EventArgs e)
        {
            if (this.mode == FormMode.ALTER && this.modifyID == -1)
            {
                throw new Exception("未设置源库存信息");
            }
            this.tableLayoutPanel1.Controls.Clear();
            if (this.setItem == WAREHOUSE_MODIFY)
            {
                for (int i = 1; i < BaseWarehouseMetaData.KeyNames.Length; i++)
                {
                    KeyName curKeyName = BaseWarehouseMetaData.KeyNames[i];
                    Label label = new Label();
                    label.Text = curKeyName.Name;
                    this.tableLayoutPanel1.Controls.Add(label);

                    TextBox textBox = new TextBox();
                    textBox.Name = "textBox" + curKeyName.Key;
                    this.tableLayoutPanel1.Controls.Add(textBox);
                }
                if (this.mode == FormMode.ALTER)
                {
                    try
                    {

                        Warehouse Warehouse = (from s in this.wmsEntities.Warehouse
                                               where s.ID == this.modifyID
                                               select s).Single();
                        Utilities.CopyPropertiesToTextBoxes(Warehouse, this);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }
                }
            }
            if (setItem == PackageUnit_MODIFY)
            {
                for (int i = 1; i < BasePackageUnitMetaData.KeyNames.Length; i++)
                {
                    KeyName curKeyName = BasePackageUnitMetaData.KeyNames[i];
                    Label label = new Label();
                    label.Text = curKeyName.Name;
                    this.tableLayoutPanel1.Controls.Add(label);

                    TextBox textBox = new TextBox();
                    textBox.Name = "textBox" + curKeyName.Key;
                    this.tableLayoutPanel1.Controls.Add(textBox);
                }
                if (this.mode == FormMode.ALTER)
                {
                    try
                    {
                        PackageUnit PackageUnit = (from s in this.wmsEntities.PackageUnit
                                           where s.ID == this.modifyID
                                           select s).Single();
                        Utilities.CopyPropertiesToTextBoxes(PackageUnit, this);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }

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
            if (setItem == WAREHOUSE_MODIFY)
            {
                if (mode == FormMode.ALTER)
                {
                    this.Text = "修改仓库信息";
                    this.buttonModify.Text = "修改仓库信息";
                    this.groupBoxMain.Text = "修改仓库信息";
                }
                else if (mode == FormMode.ADD)
                {
                    this.Text = "添加仓库信息";
                    this.buttonModify.Text = "添加仓库信息";
                    this.groupBoxMain.Text = "添加仓库信息";
                }
            }
            if (setItem == PackageUnit_MODIFY)
            {
                if (mode == FormMode.ALTER)
                {
                    this.Text = "修改包装信息信息";
                    this.buttonModify.Text = "修改包装信息信息";
                    this.groupBoxMain.Text = "修改包装信息信息";
                }
                else if (mode == FormMode.ADD)
                {
                    this.Text = "添加包装信息信息";
                    this.buttonModify.Text = "添加包装信息信息";
                    this.groupBoxMain.Text = "添加包装信息信息";
                }
            }
        }

        private void buttonModify_Click(object sender, EventArgs e)
        {
            var textBoxName = this.Controls.Find("textBoxName", true)[0];
            if (textBoxName.Text == string.Empty)
            {
                MessageBox.Show("名称不能为空！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (setItem == WAREHOUSE_MODIFY)
            {
                Warehouse warehouse = null;

                //若修改，则查询原对象。若添加，则新建对象。
                if (this.mode == FormMode.ALTER)
                {
                    try
                    {
                        warehouse = (from s in this.wmsEntities.Warehouse
                                     where s.ID == this.modifyID
                                     select s).Single();
                    }
                    catch
                    {
                        MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (warehouse == null)
                    {
                        MessageBox.Show("仓库不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    try
                    {
                        var sameNameWarehouse = (from u in wmsEntities.Warehouse
                                             where u.Name == textBoxName.Text
                                                && u.ID != warehouse.ID
                                             select u).ToArray();
                        if (sameNameWarehouse.Length > 0)
                        {
                            MessageBox.Show("修改仓库名失败，已存在同名仓库：" + textBoxName.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    catch
                    {

                        MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                }
                else if (mode == FormMode.ADD)
                {
                    try
                    {
                        var sameNameWarehouse = (from u in wmsEntities.Warehouse
                                             where u.Name == textBoxName.Text
                                             select u).ToArray();
                        if (sameNameWarehouse.Length > 0)
                        {
                            MessageBox.Show("修改仓库名失败，已存在同名仓库：" + textBoxName.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    catch
                    {

                        MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    warehouse = new Warehouse();
                    this.wmsEntities.Warehouse.Add(warehouse);
                }
                //开始数据库操作
                if (Utilities.CopyTextBoxTextsToProperties(this, warehouse, BaseWarehouseMetaData.KeyNames, out string errorMessage) == false)
                {
                    MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                try
                {
                    wmsEntities.SaveChanges();
                }
                catch (Exception)
                {
                    if (this.mode == FormMode.ALTER)
                    {
                        MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (mode == FormMode.ADD)
                    {
                        MessageBox.Show("添加失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                }
                //调用回调函数

                if (this.mode == FormMode.ALTER && this.modifyFinishedCallback != null)
                {
                    this.modifyFinishedCallback(warehouse.ID);
                    MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else if (this.mode == FormMode.ADD && this.addFinishedCallback != null)
                {
                    this.addFinishedCallback(warehouse.ID);
                    MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }

            }
            if (setItem == PackageUnit_MODIFY)
            {
                PackageUnit packageUnit = null;

                //若修改，则查询原对象。若添加，则新建对象。
                if (this.mode == FormMode.ALTER)
                {
                    try
                    {
                        packageUnit = (from s in this.wmsEntities.PackageUnit
                                   where s.ID == this.modifyID
                                   select s).Single();
                    }
                    catch
                    {
                        MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (packageUnit == null)
                    {
                        MessageBox.Show("包装信息不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                }
                else if (mode == FormMode.ADD)
                {
                    packageUnit = new PackageUnit();
                    this.wmsEntities.PackageUnit.Add(packageUnit);
                }
                //开始数据库操作
                if (Utilities.CopyTextBoxTextsToProperties(this, packageUnit, BasePackageUnitMetaData.KeyNames, out string errorMessage) == false)
                {
                    MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                try
                {
                    wmsEntities.SaveChanges();
                }
                catch (Exception)
                {
                    if (this.mode == FormMode.ALTER)
                    {
                        MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (mode == FormMode.ADD)
                    {
                        MessageBox.Show("添加失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                }
                //调用回调函数
                if (this.mode == FormMode.ALTER && this.modifyFinishedCallback != null)
                {
                    this.modifyFinishedCallback(packageUnit.ID);
                    MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else if (this.mode == FormMode.ADD && this.addFinishedCallback != null)
                {
                    this.addFinishedCallback(packageUnit.ID);
                    MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
        }

        private void buttonModify_MouseEnter(object sender, EventArgs e)
        {
            buttonModify.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        }

        private void buttonModify_MouseLeave(object sender, EventArgs e)
        {
            buttonModify.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_q;
        }

        private void buttonModify_MouseDown(object sender, MouseEventArgs e)
        {
            buttonModify.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }
    }
}

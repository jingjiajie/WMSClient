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
    public partial class FormBasePersonModify : Form
    {
        private int personID = -1;
        private int projectID = -1;
        private int warehouseID = -1;
        private WMSEntities wmsEntities = new WMSEntities();
        private Action<int> modifyFinishedCallback = null;
        private Action<int> addFinishedCallback = null;
        private FormMode mode = FormMode.ALTER;


        public FormBasePersonModify(int personID=-1)
        {
            InitializeComponent();
            this.personID = personID;
        }

        private void FormBasePersonModify_Load(object sender, EventArgs e)
        {
            if (this.mode == FormMode.ALTER && this.personID == -1)
            {
                throw new Exception("未设置源库存信息");
            }
            Utilities.CreateEditPanel(this.tableLayoutPanelTextBoxes, BasePersonMetaData.KeyNames);
            TextBox textboxProjectName = (TextBox)this.Controls.Find("textBoxProjectName", true)[0];
            TextBox textboxWarehouseName = (TextBox)this.Controls.Find("textBoxWarehouseName", true)[0];
            if (this.mode == FormMode.ALTER)
            {

                try
                {
                    PersonView personView = (from s in this.wmsEntities.PersonView
                                             where s.ID == this.personID
                                             select s).FirstOrDefault();
                    Utilities.CopyPropertiesToTextBoxes(personView, this);
                }
                catch (Exception)
                {
                    MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

            }
            if (this.mode == FormMode.ADD)
            {
                try
                {
                    var projectName = (from s in this.wmsEntities.ProjectView
                                   where s.ID == GlobalData.ProjectID
                                   select s.Name).FirstOrDefault();
                    textboxProjectName.Text = projectName;
                }
                catch (Exception)
                {
                    MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                try
                {
                    var warehouseName = (from s in this.wmsEntities.WarehouseView
                                   where s.ID == GlobalData.WarehouseID
                                   select s.Name).FirstOrDefault();
                    textboxWarehouseName.Text = warehouseName;
                }
                catch (Exception)
                {
                    MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
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
            if (mode == FormMode.ALTER)
            {
                this.Text = "修改人员信息";
                this.buttonModify.Text = "修改人员信息";
                this.groupBoxMain.Text = "修改人员信息";
            }
            else if (mode == FormMode.ADD)
            {
                this.Text = "添加人员信息";
                this.buttonModify.Text = "添加人员信息";
                this.groupBoxMain.Text = "添加人员信息";
            }
        }

        private void buttonModify_Click(object sender, EventArgs e)
        {
            var textBoxName = this.Controls.Find("textBoxName", true)[0];
            TextBox textboxProjectName = (TextBox)this.Controls.Find("textBoxProjectName", true)[0];
            TextBox textboxWarehouseName = (TextBox)this.Controls.Find("textBoxWarehouseName", true)[0];
            if (textBoxName.Text == string.Empty)
            {
                MessageBox.Show("人员名称不能为空！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            Project project = null;
            if (textboxProjectName.Text == string.Empty)
            {
                MessageBox.Show("项目名称不能为空！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                string projectName = textboxProjectName.Text;
                try
                {
                     project = (from s in this.wmsEntities.Project where s.Name == projectName select s).FirstOrDefault();

                    this.projectID = project.ID;
                }
                catch
                {
                    if (project == null)
                    {
                        MessageBox.Show("输入的项目不存在，请重新确认后出入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
            }



            Warehouse warehouse = null;
            if (textboxWarehouseName.Text == string.Empty)
            {
                MessageBox.Show("仓库名称不能为空！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                string warehouseName = textboxWarehouseName.Text;
                try
                {
                    warehouse = (from s in this.wmsEntities.Warehouse where s.Name == warehouseName select s).FirstOrDefault();

                    this.warehouseID = warehouse.ID;
                }
                catch
                {
                    if (warehouse == null)
                    {
                        MessageBox.Show("输入仓库不存在，请重新确认后出入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
            }



            Person person = null;

            //若修改，则查询原对象。若添加，则新建对象。
            if (this.mode == FormMode.ALTER)
            {
                try
                {
                    person = (from s in this.wmsEntities.Person
                               where s.ID == this.personID
                               select s).FirstOrDefault();
                }
                catch
                {
                    MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (person == null)
                {
                    MessageBox.Show("人员不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    var sameNamePerson = (from u in wmsEntities.Person
                                             where u.Name == textBoxName.Text
                                             &&u.ProjectID==this.projectID
                                             &&u.WarehouseID==this.warehouseID
                                                && u.ID != person.ID
                                             select u).ToArray();
                    if (sameNamePerson.Length > 0)
                    {
                        MessageBox.Show("修改人员名失败，"+ textboxWarehouseName.Text+"仓库"+ textboxProjectName .Text+ "项目中已存在同名人员：" + textBoxName.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    var sameNamePerson = (from u in wmsEntities.Person
                                          where u.Name == textBoxName.Text
                                          && u.ProjectID == this.projectID
                                             && u.WarehouseID == this.warehouseID
                                          select u).ToArray();
                    if (sameNamePerson.Length > 0)
                    {
                        MessageBox.Show("修改人员名失败，" + textboxWarehouseName.Text + "仓库" + textboxProjectName.Text + "项目中已存在同名人员：" + textBoxName.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                catch
                {

                    MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                person = new Person();
                this.wmsEntities.Person.Add(person);
            }

            person.WarehouseID = this.warehouseID;
            person.ProjectID = this.projectID;


            //开始数据库操作
            if (Utilities.CopyTextBoxTextsToProperties(this, person, BasePersonMetaData.KeyNames, out string errorMessage) == false)
            {
                MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                Utilities.CopyComboBoxsToProperties(this, person, BasePersonMetaData.KeyNames);
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
                this.modifyFinishedCallback(person.ID);
                MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else if (this.mode == FormMode.ADD && this.addFinishedCallback != null)
            {
                this.addFinishedCallback(person.ID);
                MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
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

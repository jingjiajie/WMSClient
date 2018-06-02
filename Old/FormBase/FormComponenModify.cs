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
    public partial class FormComponenModify : Form
    {
        private int projectID = -1;
        private int warehouseID = -1;
        private int userID = -1;
        private int componenID = -1;
        private int supplierID = -1;
        private WMSEntities wmsEntities = new WMSEntities();
        private Action<int> modifyFinishedCallback = null;
        private Action<int> addFinishedCallback = null;
        private FormMode mode = FormMode.ALTER;
        private int history_save = 0;

        public FormComponenModify(int projectID, int warehouseID, int supplierID, int userID, int componenID = -1)
        {
            InitializeComponent();
            this.warehouseID = warehouseID;
            this.userID = userID;
            this.projectID = projectID;
            this.supplierID = supplierID;
            this.componenID = componenID;
        }
        
        private void FormComponenModify_Load(object sender, EventArgs e)
        { 
            if (this.mode == FormMode.ALTER && this.componenID == -1)
            {
                throw new Exception("未设置源零件信息");
            }

            Utilities.CreateEditPanel(this.tableLayoutPanelTextBoxes, ComponenViewMetaData.componenmodifykeyNames);



            if (this.mode == FormMode.ALTER)
            {
               
                try
                {
                    ComponentView componenView = (from s in this.wmsEntities.ComponentView
                                                  where s.ID == this.componenID
                                                  select s).FirstOrDefault();
                    Utilities.CopyPropertiesToTextBoxes(componenView, this);
                }
                catch (Exception)
                {
                    MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

            }

        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            var textBoxName = this.Controls.Find("textBoxName", true)[0];


            if (textBoxName.Text == string.Empty)
            {
                MessageBox.Show("零件名称不能为空！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            DataAccess.Component componen = null;
            if (this.mode == FormMode.ALTER)
            {

                try
                {
                    componen = (from s in this.wmsEntities.Component
                                       where s.ID == this.componenID
                                       select s).FirstOrDefault();
                }
                catch
                {
                    MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (componen == null)
                {
                    MessageBox.Show("零件信息不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    var sameNameComponent = (from u in wmsEntities.Component
                                             where u.Name == textBoxName.Text
                                             && u.ID != componen.ID
                                             select u).ToArray();
                    if (sameNameComponent.Length > 0)
                    {
                        MessageBox.Show("修改零件名失败，已存在同名零件：" + textBoxName.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    var sameNameComponent = (from u in wmsEntities.Component
                                             where u.Name == textBoxName.Text
                                             select u).ToArray();
                    if (sameNameComponent.Length > 0)
                    {
                        MessageBox.Show("修改零件名失败，已存在同名零件：" + textBoxName.Text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                catch
                {

                    MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                componen = new DataAccess.Component();
                this.wmsEntities.Component.Add(componen);

            }



            //开始数据库操作
            if (Utilities.CopyTextBoxTextsToProperties(this, componen, ComponenViewMetaData.componenkeyNames, out string errorMessage) == false)
            {
                MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                Utilities.CopyComboBoxsToProperties(this, componen, ComponenViewMetaData.KeyNames);
            }
            //componen.IsHistory = 0;
            wmsEntities.SaveChanges();

            //调用回调函数
            if (this.mode == FormMode.ALTER && this.modifyFinishedCallback != null)
            {
                this.modifyFinishedCallback(componen.ID);
                MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else if(this.mode == FormMode.ADD && this.addFinishedCallback != null)
            {
                this.addFinishedCallback(componen.ID);
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

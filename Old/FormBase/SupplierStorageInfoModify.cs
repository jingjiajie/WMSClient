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
    public partial class SupplierStorageInfoModify : Form
    {
        private int supplierID = -1;
        private int SupplierStorageInfoID = -1;
        private Action<int> modifyFinishedCallback = null;
        private Action<int> addFinishedCallback = null;
        private FormMode mode = FormMode.ALTER;
        private DataAccess.WMSEntities wmsEntities = new DataAccess.WMSEntities();
        public SupplierStorageInfoModify(int supplierid,int SupplierStorageInfoid=-1)
        {
            InitializeComponent();
            this.supplierID = supplierid;
            this.SupplierStorageInfoID = SupplierStorageInfoid;
        }

        private void FormSupplierAnnualInfoModify_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            if (this.mode == FormMode.ALTER && this.supplierID == -1)
            {
                throw new Exception("未设置源供应商信息");
            }
            
            Utilities.CreateEditPanel(this.tableLayoutPanel1, SupplierStorageInfoMetaData.KeyNames);

            WMS.DataAccess.SupplierStorageInfoView supplierstorgrinfoView = new DataAccess.SupplierStorageInfoView();
                if (this.mode == FormMode.ALTER)
                {
                    try
                    {
                        supplierstorgrinfoView = (from s in wmsEntities.SupplierStorageInfoView
                                              where s.ID == this.SupplierStorageInfoID 
                                              select s).Single();
                    }
                    catch
                    {
                        MessageBox.Show("加载数据失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        this.Close();
                        return;
                    }
                    if (supplierstorgrinfoView == null)
                    {
                        MessageBox.Show("修改失败，供应商不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }
                    Utilities.CopyPropertiesToTextBoxes(supplierstorgrinfoView , this);
                }
            
        }
        public void SetMode(FormMode mode)
        {
            this.mode = mode;
            if (mode == FormMode.ALTER)
            {
                this.Text = "修改供应商存货信息";
                this.buttonModify.Text = "修改供应商存货信息";
                this.groupBox1.Text = "修改供应商存货信息";
            }
            else if (mode == FormMode.ADD)
            {
                this.Text = "添加供应商存货信息";
                this.buttonModify.Text = "添加供应商存货信息";
                this.groupBox1.Text = "添加供应商存货信息";
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

        private void buttonModify_Click(object sender, EventArgs e)
        {
            DataAccess.SupplierStorageInfo supp = null ;
            TextBox textBoxYear = (TextBox)this.Controls.Find("textBoxYear", true)[0];
            TextBox textBoxMonth = (TextBox)this.Controls.Find("textBoxMonth", true)[0];
            if(textBoxYear.Text ==string .Empty   )
            {
                MessageBox.Show("存货年份不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information );
                
                return;


            }


            if ( textBoxMonth.Text == string.Empty)
            {
                MessageBox.Show("存货月份不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;


            }


            if (mode == FormMode.ALTER)
            {


                try
                {
                    supp = (from s in this.wmsEntities.SupplierStorageInfo
                            where s.ID == this.SupplierStorageInfoID 
                                select s).FirstOrDefault ();
                }
                catch
                {
                    MessageBox.Show("加载数据失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    this.Close();
                    return;
                }
                if (supp == null)
                {
                    MessageBox.Show("修改失败，供应商不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                if (Utilities.CopyTextBoxTextsToProperties(this, supp, SupplierStorageInfoMetaData .KeyNames, out string errorMessage) == false)
                {
                    MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {

                    wmsEntities.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            else if (mode == FormMode.ADD)
            {

                supp = new DataAccess.SupplierStorageInfo();
                this.wmsEntities.SupplierStorageInfo.Add(supp);
                //开始数据库操作
                if (Utilities.CopyTextBoxTextsToProperties(this, supp, SupplierStorageInfoMetaData.KeyNames, out string errorMessage) == false)
                {
                    MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                try
                {
                    supp.SupplierID = this.supplierID;
                    wmsEntities.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }



                  }


            if (this.mode == FormMode.ALTER && this.modifyFinishedCallback != null)
            {
                this.modifyFinishedCallback(supp.ID);
                MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (this.mode == FormMode.ADD && this.addFinishedCallback != null)
            {
                this.addFinishedCallback(supp.ID);
                MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            this.Close();

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

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
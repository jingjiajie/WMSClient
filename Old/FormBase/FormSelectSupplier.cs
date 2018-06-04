using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using unvell.ReoGrid;
using WMS.DataAccess;
using System.Threading;

namespace WMS.UI
{
    public partial class FormSelectSupplier : Form,IFormSelect
    {
        private Action<int> selectFinishCallback = null;
        private int defaultSupplierID = -1;
        private int projectID = -1;
        private int warehouseID = -1;
        private PagerWidget<SupplierView > pagerWidget = null;
        static Point staticPos = new Point(-1, -1);

        public FormSelectSupplier()
        {
            InitializeComponent();
            //this.defaultSupplierID = supplierid;
        }

        public void SetSelectFinishedCallback(Action<int> selectFinishedCallback)
        {
            this.selectFinishCallback = selectFinishedCallback;
        }


        private void InitComponents()
        {
            this.textBoxSupplierName .Text  = "";
            this.pagerWidget = new PagerWidget<SupplierView>(this.reoGridControlMain, SupplierMetaData.KeyNames, this.projectID, this.warehouseID);
            this.panelPagerWidget.Controls.Add(this.pagerWidget);
            this.pagerWidget.Show();
        }

        private void FormSelectSupplier_Load(object sender, EventArgs e)
        {
            InitComponents();
            this.toolStripComboBox1.SelectedIndex= 1;
            this.pagerWidget.AddCondition("是否历史信息", "0");
            this.pagerWidget.Search();
            //if (this.defaultSupplierID != -1)
            //{
            //    try
            //    {
            //        WMSEntities wmsEntities = new WMSEntities();

            //        this.textBoxSupplierName.Text = (from s in wmsEntities.SupplierView where s.ID == defaultSupplierID select s.Name).FirstOrDefault();
            //        this.Search(defaultSupplierID);
            //    }

            //    catch  {
            //        MessageBox.Show("加载数据失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        this.Close();
            //        return;
            //    }

            //}
            //else
            //{
            //    this.Search();
            //}
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            this.Search();
        }

        private void Search(int selectID=-1)
        {
            string key = this.toolStripComboBox1.Text;
            string value = this.textBoxSupplierName.Text;
            this.pagerWidget.ClearCondition();
            this.pagerWidget.AddCondition("是否历史信息", "0");
            if (key != "无")
            {
                if (this.textBoxSupplierName.Text == "")
                {
                    MessageBox.Show("输入要查找的关键字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (this.textBoxSupplierName.Text != "")
                {
                    this.pagerWidget.AddCondition(key, value);
                    this.pagerWidget.Search(false, selectID);
                }
            }
            else if (key == "无")
            {
                this.pagerWidget.Search(false, selectID);
            }
     
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            this.SelectItem();
        }

        private void SelectItem()
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            if (ids.Length != 1)
            {
                MessageBox.Show("请选择一项");
                return;
            }
            this.selectFinishCallback?.Invoke(ids[0]);
            this.Hide ();
        }

      

        private void textBoxSupplierName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                this.buttonSearch .PerformClick ();
            }
        }

        private void FormSelectSupplier_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void textBoxSupplierName_VisibleChanged(object sender, EventArgs e)
        {
            //if (this.Visible == false)
            //{
            //    staticPos = this.Location;
            //}
            //else
            //{
            //    if (staticPos != new Point(-1, -1))
            //    {
            //        this.Location = staticPos;
            //    }
            //    int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            //    int id = -1;
            //    if (ids.Length > 0)
            //    {
            //        id = ids[0];
            //    }
            //    this.pagerWidget.Search(true, id);
            //}
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.toolStripComboBox1.Text =="无")
            {
                this.textBoxSupplierName.Text ="";
                this.textBoxSupplierName.Enabled = false;
            }
            else if (this.toolStripComboBox1.Text != "无")
            {
                this.textBoxSupplierName.Enabled = true ;
            }

        }
    }
}

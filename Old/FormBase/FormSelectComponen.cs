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
    public partial class FormSelectComponen : Form, IFormSelect
    {
        private PagerWidget<ComponentView> pagerWidget = null;
        private Action<int> selectFinishCallback = null;
        private int defaultComponenID = -1;
        private int projectID = -1;
        private int warehouseID = -1;
        static Point staticPos = new Point(-1, -1);

        public FormSelectComponen()
        {
            InitializeComponent();
        }

        public void SetSelectFinishedCallback(Action<int> selectFinishedCallback)
        {
            this.selectFinishCallback = selectFinishedCallback;
        }

        private void InitComponents()
        {
           
            this.textBoxComponenName.Text = "";
            this.pagerWidget = new PagerWidget<ComponentView>(this.reoGridControlMain, ComponenViewMetaData.KeyNames, this.projectID, this.warehouseID);
            this.panelPagerWidget.Controls.Add(this.pagerWidget);
            this.pagerWidget.Show();
        }

        private void FormSelectComponen_Load(object sender, EventArgs e)
        {
            InitComponents();
            if (this.defaultComponenID != -1)
            {
                try
                {
                    WMSEntities wmsEntities = new WMSEntities();

                    this.textBoxComponenName.Text = (from s in wmsEntities.ComponentView where s.ID == defaultComponenID select s.Name).FirstOrDefault();
                    this.Search(defaultComponenID);

                }

                catch
                {
                    MessageBox.Show("加载数据失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

            }
            else
            {
                this.Search();
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            this.Search();
        }

        private void Search(int selectID = -1)
        {

            if (this.textBoxComponenName.Text != "")
            {
                string value = this.textBoxComponenName.Text;
                this.pagerWidget.ClearCondition();
                this.pagerWidget.AddCondition("零件名称", value);
                this.pagerWidget.Search(false, selectID);
            }
            else
            {
                this.pagerWidget.ClearCondition();
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
            this.Hide();
        }

        private void textBoxComponenName_Click(object sender, EventArgs e)
        {

        }

        private void textBoxComponenName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                this.Search();
            }
        }
        private void FormSelectComponen_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void textBoxComponenName_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == false)
            {
                staticPos = this.Location;
            }
            else
            {
                if (staticPos != new Point(-1, -1))
                {
                    this.Location = staticPos;
                }
                int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
                int id = -1;
                if (ids.Length > 0)
                {
                    id = ids[0];
                }
                this.pagerWidget.Search(true, id);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.DataAccess;
using unvell.ReoGrid;
using System.Threading;

namespace WMS.UI
{
    public partial class FormSelectSupply : Form,IFormSelect
    {
        private WMSEntities wmsEntities = new WMSEntities();
        private int defaultStockInfoID = -1;
        private Action<int> selectFinishCallback = null;
        private Action<int[]> multiselectFinishedCallback = null;
        private PagerWidget<SupplyView> pagerWidget = null;
        private int projectID = -1;
        private int warehouseID = -1;

        public static Mode SelectMode = Mode.NORMAL;
        public enum Mode { NORMAL,MULTISELECT};

        static Point staticPos = new Point(-1, -1);

        public FormSelectSupply(int projectID, int warehouseID, int defaultStockInfoID = -1)
        {
            InitializeComponent();
            this.defaultStockInfoID = defaultStockInfoID;
            this.projectID = projectID;
            this.warehouseID = warehouseID;
        }

        public FormSelectSupply()
        {
            InitializeComponent();
            this.projectID = GlobalData.ProjectID;
            this.warehouseID = GlobalData.WarehouseID;
        }

        public void SetSelectFinishedCallback(Action<int> selectFinishedCallback)
        {
            this.selectFinishCallback = selectFinishedCallback;
        }

        public void SetMultiselectFinishedCallback(Action<int[]> callback)
        {
            this.multiselectFinishedCallback = callback;
        }

        private void InitComponents()
        {
            if (SelectMode == Mode.NORMAL)
            {
                this.pagerWidget = new PagerWidget<SupplyView>(this.reoGridControlMain, SupplyViewMetaData.KeyNames, this.projectID, this.warehouseID);
            }
            else
            {
                this.pagerWidget = new PagerWidget<SupplyView>(this.reoGridControlMain, SupplyViewMetaData.KeyNames, this.projectID, this.warehouseID, PagerWidget<SupplyView>.Mode.MULTISELECT);
            }
            this.panelPagerWidget.Controls.Add(this.pagerWidget);
            this.pagerWidget.Show();
            this.comboBoxSearchCondition.SelectedIndex  = 0;
        }

        private void FormJobTicketSelectStockInfo_Load(object sender, EventArgs e)
        {
            InitComponents();
            this.Search();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            this.Search();
        }

        private void Search(bool savePage = false, int selectID = -1, Action<SupplyView[]> callback = null)
        {
            string key = "";
            string value = this.textBoxSearchContition.Text;
            this.pagerWidget.ClearCondition();
            if (this.comboBoxSearchCondition.SelectedItem.ToString() == "无")
            {
                this.pagerWidget.AddCondition("是否历史信息", "0");
                this.pagerWidget.Search(savePage, selectID, callback);
            }
            else
            {
                if (this.comboBoxSearchCondition.SelectedItem.ToString() == "供货零件代号")
                {

                    key = "代号";

                }
                else
                {
                    key = "零件名";
                }
                this.pagerWidget.AddCondition("是否历史信息", "0");
                this.pagerWidget.AddCondition(key, value);
                this.pagerWidget.Search(savePage, selectID, callback);
            }
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            this.SelectItem();
        }

        private void SelectItem()
        {
            int[] ids;
            if (SelectMode == Mode.NORMAL)
            {
                ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            }
            else
            {
                ids = this.pagerWidget.GetSelectedIDs();
            }

            if (ids.Length <= 0)
            {
                MessageBox.Show("请选择至少一项","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (SelectMode == Mode.NORMAL)
            {
                this.selectFinishCallback?.Invoke(ids[0]);
            }
            else
            {
                this.multiselectFinishedCallback?.Invoke(ids);
            }

            this.Hide();
        }

        private void textBoxComponentNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                this.buttonSearch.PerformClick();
            }
        }

        private void FormSelectStockInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void FormSelectStockInfo_VisibleChanged(object sender, EventArgs e)
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
                //this.pagerWidget.Search(true, id);
            //}
        }
    }
}

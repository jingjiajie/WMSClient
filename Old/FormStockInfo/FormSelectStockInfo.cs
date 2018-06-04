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
    public partial class FormSelectStockInfo : Form,IFormSelect
    {
        private WMSEntities wmsEntities = new WMSEntities();
        private int defaultStockInfoID = -1;
        private Action<int> selectFinishCallback = null;
        private PagerWidget<StockInfoView> pagerWidget = null;
        private int projectID = -1;
        private int warehouseID = -1;

        static Point staticPos = new Point(-1, -1);

        public FormSelectStockInfo(int projectID, int warehouseID, int defaultStockInfoID = -1)
        {
            InitializeComponent();
            this.defaultStockInfoID = defaultStockInfoID;
            this.projectID = projectID;
            this.warehouseID = warehouseID;
        }

        public FormSelectStockInfo()
        {
            InitializeComponent();
            this.projectID = GlobalData.ProjectID;
            this.warehouseID = GlobalData.WarehouseID;
        }

        public void SetSelectFinishedCallback(Action<int> selectFinishedCallback)
        {
            this.selectFinishCallback = selectFinishedCallback;
        }

        private void InitComponents()
        {
            this.comboBoxSearchCondition.SelectedIndex = 0;
            this.pagerWidget = new PagerWidget<StockInfoView>(this.reoGridControlMain, StockInfoViewMetaData.KeyNames, this.projectID, this.warehouseID);
            this.panelPagerWidget.Controls.Add(this.pagerWidget);
            this.pagerWidget.Show();
        }

        private void FormJobTicketSelectStockInfo_Load(object sender, EventArgs e)
        {
            InitComponents();
            if (this.defaultStockInfoID != -1)
            {
                try
                {
                    WMSEntities wmsEntities = new WMSEntities();
                    this.comboBoxSearchCondition.SelectedIndex = 1; //自动选中零件编号选项
                    this.textBoxSearchContition.Text = (from s in wmsEntities.StockInfoView where s.ID == defaultStockInfoID select s.SupplyNo).FirstOrDefault();
                    this.Search(false,defaultStockInfoID);
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

        private void Search(bool savePage = false, int selectID = -1, Action<StockInfoView[]> callback = null)
        {
            string value = this.textBoxSearchContition.Text;
            string key = this.comboBoxSearchCondition.SelectedItem.ToString();

            this.pagerWidget.ClearCondition();
            this.pagerWidget.AddCondition(key, value);
            this.pagerWidget.Search(savePage, selectID,callback);
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

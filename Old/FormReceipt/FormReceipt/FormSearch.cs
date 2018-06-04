using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.DataAccess;
using WMS.UI;
using unvell.ReoGrid;
using System.Threading;

namespace WMS.UI.FormReceipt
{
    public partial class FormSearch : Form
    {
        private WMSEntities wmsEntities = new WMSEntities();
        private int projectID;
        private int warehouseID;
        private int ComponentID = -1;
        private int supplierID = -1;
        private PagerWidget<SupplyView> pagerWidget;
        //private Action CallBack = null;
        private Action<int> selectFinishCallback = null;
        public FormSearch()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public FormSearch(int supplierID, int projectID, int warehouseID)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.supplierID = supplierID;
            this.projectID = projectID;
            this.warehouseID = warehouseID;
        }

        public void SetSelectFinishCallback(Action<int> selectFinishedCallback)
        {
            this.selectFinishCallback = selectFinishedCallback;
        }

        private void FormSearch_Load(object sender, EventArgs e)
        {
            InitComponent();
            WMSEntities wmsEntities = new WMSEntities();
            pagerWidget = new PagerWidget<SupplyView>(this.reoGridControlMain, ReceiptMetaData.componentKeyName, projectID, warehouseID);
            this.panel1.Controls.Add(pagerWidget);
            this.pagerWidget.AddStaticCondition("IsHistory", "0");
            this.pagerWidget.AddStaticCondition("SupplierID", this.supplierID.ToString());
            pagerWidget.Show();
            //this.textBoxSearchContition.Text = (from s in wmsEntities.Component where s.s select s.).FirstOrDefault();
            //this.Search();
            this.pagerWidget.Search();
        }

        private void Search(bool savePage = false, int selectID = -1)
        {
            this.pagerWidget.ClearCondition();
            
            if (this.comboBoxSearchCondition.SelectedIndex != 0)
            {
                this.pagerWidget.AddCondition(this.comboBoxSearchCondition.SelectedItem.ToString(), this.textBoxSearchContition.Text);
            }
            this.pagerWidget.AddOrderBy("ReceiveTimes");
            this.pagerWidget.Search(savePage, selectID);
        }

        private void InitComponent()
        {
            this.comboBoxSearchCondition.SelectedIndex = 0;
            //this.comboBoxSearchCondition.Items.Add("零件编号", )
            //初始化表格
            var worksheet = this.reoGridControlMain.Worksheets[0];
            worksheet.SelectionMode = WorksheetSelectionMode.Row;
            for (int i = 0; i < SupplyViewMetaData.supplykeyNames.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = SupplyViewMetaData.supplykeyNames[i].Name;
                worksheet.ColumnHeaders[i].IsVisible = SupplyViewMetaData.supplykeyNames[i].Visible;
            }
            worksheet.Columns = SupplyViewMetaData.supplykeyNames.Length; //限制表的长度
        }
        /*
        private void Search()
        {
            string value = this.textBoxSearchContition.Text;
            string key = this.comboBoxSearchCondition.SelectedItem.ToString();
            this.labelStatus.Text = "正在搜索...";
            new Thread(new ThreadStart(() =>
            {
                WMS.DataAccess.SupplyView[] supplyView = null;
                if (key == "零件编号")
                {
                    supplyView = (from s in this.wmsEntities.SupplyView
                                      where s.No == value
                                      select s).ToArray();
                }
                else if (key == "零件名称")
                {
                    supplyView = (from s in this.wmsEntities.SupplyView
                                      where s.ComponentName.Contains(value)
                                      select s).ToArray();
                }
                else
                {
                    MessageBox.Show("内部错误，无法识别查询条件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Invoke(new Action(() =>
                {
                    var worksheet = this.reoGridControlMain.Worksheets[0];
                    worksheet.DeleteRangeData(RangePosition.EntireRange);
                    for (int i = 0; i < supplyView.Length; i++)
                    {
                        WMS.DataAccess.SupplyView curSupplyView = supplyView[i];
                        object[] columns = Utilities.GetValuesByPropertieNames(curSupplyView, (from kn in ReceiptMetaData.componentKeyName select kn.Key).ToArray());
                        for (int j = 0; j < worksheet.Columns; j++)
                        {
                            worksheet[i, j] = columns[j] == null ? "" : columns[j].ToString();
                        }
                    }
                    if (supplyView.Length == 0)
                    {
                        worksheet[0, 2] = "没有查询到符合条件的记录";
                    }
                    this.labelStatus.Text = "搜索完成";
                }));
            })).Start();
        }*/

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            this.Search();
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            this.SelectItem();
        }

        private void SelectItem()
        {
            int[] ids = this.GetSelectedIDs();
            if (ids.Length != 1)
            {
                MessageBox.Show("请选择一项");
                return;
            }
            this.selectFinishCallback?.Invoke(ids[0]);
            this.Close();
        }

        private int[] GetSelectedIDs()
        {
            List<int> ids = new List<int>();
            var worksheet = this.reoGridControlMain.Worksheets[0];
            for (int row = worksheet.SelectionRange.Row; row <= worksheet.SelectionRange.EndRow; row++)
            {
                if (worksheet[row, 0] == null) continue;
                if (int.TryParse(worksheet[row, 0].ToString(), out int shipmentTicketID))
                {
                    ids.Add(shipmentTicketID);
                }
            }
            return ids.ToArray();
        }

        private void comboBoxSearchCondition_Click(object sender, EventArgs e)
        {

        }

        private void textBoxSearchContition_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                this.buttonSearch.PerformClick();
            }
        }

        private void comboBoxSearchCondition_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (this.textBoxSearchContition.Enabled)
                {
                    this.textBoxSearchContition.Focus();
                    this.textBoxSearchContition.SelectAll();
                }
                else
                {
                    this.buttonSearch.PerformClick();
                }
            }
        }

        private void comboBoxSearchCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.comboBoxSearchCondition.SelectedIndex == 0)
            {
                this.textBoxSearchContition.Enabled = false;
            }
            else
            {
                this.textBoxSearchContition.Enabled = true;
            }
        }
    }


}

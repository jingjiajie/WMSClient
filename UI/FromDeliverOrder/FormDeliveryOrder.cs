using FrontWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI.FromDeliverOrder
{
    public partial class FormDeliveryOrder : Form
    {
        public FormDeliveryOrder()
        {
            InitializeComponent();
        }
        //查看条目
        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项入库单查看物料条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(new long[] { this.model1.SelectionRange.Row })[0];
            new FormWarehouseEntryItem(rowData).Show();
        }

        private void buttonAlter_Click(object sender, EventArgs e)
        {
            this.synchronizer.Save();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            this.model1.RemoveSelectedRows();
        }
        //回单
        private void buttonReturnSupply_Click(object sender, EventArgs e)
        {

        }

        private void FormDeliveryOrder_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }
        private void model_CellUpdated(object sender, ModelCellUpdatedEventArgs e)
        {
            foreach (var cell in e.UpdatedCells)
            {
                if (cell.ColumnName.StartsWith("lastUpdate")) return;
                this.model1[cell.Row, "lastUpdatePersonId"] = GlobalData.Person["id"];
                this.model1[cell.Row, "lastUpdatePersonName"] = GlobalData.Person["name"];
                this.model1[cell.Row, "lastUpdateTime"] = DateTime.Now;
            }
        }
        //进入备货
        private void buttonTransferOrder_Click(object sender, EventArgs e)
        {

        }

        private void buttonDeliver_Click(object sender, EventArgs e)
        {

        }
    }
}

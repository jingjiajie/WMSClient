using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontWork;

namespace WMS.UI.FormStock
{
    public partial class FormAddStockTakingOrderItemsBySupply : Form
    {
        private IDictionary<string, object> stockTakingOrder = null;
        private Action addFinishedCallback = null;
        public FormAddStockTakingOrderItemsBySupply(IDictionary<string, object> stockTakingOrder)
        {
            this.CenterToScreen();
            InitializeComponent();
            this.stockTakingOrder = stockTakingOrder;
        }

        public void SetAddFinishedCallback(Action callback)
        {
            this.addFinishedCallback = callback;
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            List<int> supplyId = new List<int>();
            for (int i = 0; i < this.model1.RowCount; i++)
            {

                if (this.model1.GetRowSynchronizationState(i) == SynchronizationState.ADDED_UPDATED && (int)this.model1[i, "supplyId"] != 0)
                {
                    supplyId.Add((int)this.model1[i, "supplyId"]);
                }
            }
            if (supplyId.Count == 0) return;
            //首先得到supplyid
            int[] supplyIds = supplyId.ToArray();
            //supplyIds = new int[] { 15 };
            StringBuilder ids = new StringBuilder();
            ids.Append("[");
            foreach (int a in supplyIds)
            {
                ids.Append(a);
                ids.Append(",");
            }
            ids.Remove(ids.Length - 1, 1);
            ids.Append("]");
            this.AddItem(ids.ToString());
        }

        private void AddItem(string ids)
        {
            string body = "{\"stockTakingOrderId\":\"" + this.stockTakingOrder["id"] + "\",\"warehouseId\":\"" + GlobalData.Warehouse["id"] + "\",\"personId\":\"" + GlobalData.Person["id"] + "\"}";
            string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/stocktaking_order_item/add_single/" + ids;
            try
            {
                RestClient.RequestPost<int[]>(url, body);
                MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch
            {
                MessageBox.Show("添加失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }

        private void toolStripTop_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void buttonStartAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, null);
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }

        private void FormAddStockTakingOrderItemsBySupply_Load(object sender, EventArgs e)
        {

        }

        private void FormStockTakingOrderItem_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.addFinishedCallback != null)
            {
                this.addFinishedCallback();
            }
        }
    }
}

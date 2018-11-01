using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontWork;

namespace WMS.UI.FormSettlement
{
    public partial class FormInvoice : Form
    {
        public FormInvoice()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private void FormInvoice_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "time", DateTime.Now},
            });
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
            }
        }

        private void toolStripButtonSend_Click(object sender, EventArgs e)
        {
            //获取选中行ID，过滤掉新建的行（ID为0的）
            int[] selectedIDs = this.model1.GetSelectedRows<int>("id").Except(new int[] { 0 }).ToArray();
            if (selectedIDs.Length == 0)
            {
                MessageBox.Show("请选择一项进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row });
            for (int i = 0; i < this.model1.SelectionRange.Rows; i++)
            {
                if ((int)rowData[i]["state"] == 1)
                {
                    MessageBox.Show("选中发票已经确认寄出，无法重复操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if ((int)rowData[i]["state"] == 2)
                {
                    MessageBox.Show("选中发票已经确认收到，无法进行寄出操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (rowData[i]["trackingNumber"] == null)
                {
                    MessageBox.Show("请输入相应快递单号以继续寄出操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            this.model1[this.model1.SelectionRange.Row, "state"] = 1;
            //this.model1[this.model1.SelectionRange.Row, "sendingTime"] = DateTime.Now;
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
            }
        }

        private void toolStripButtonReceive_Click(object sender, EventArgs e)
        {
            //获取选中行ID，过滤掉新建的行（ID为0的）
            int[] selectedIDs = this.model1.GetSelectedRows<int>("id").Except(new int[] { 0 }).ToArray();
            if (selectedIDs.Length == 0)
            {
                MessageBox.Show("请选择一项进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row });
            for (int i = 0; i < this.model1.SelectionRange.Rows; i++)
            {
                if ((int)rowData[i]["state"] == 0)
                {
                    MessageBox.Show("选中发票未确定寄出，无法进行收到确认操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if ((int)rowData[i]["state"] == 2)
                {
                    MessageBox.Show("选中发票已经确认收到，无法重复确认操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            this.model1[this.model1.SelectionRange.Row, "state"] = 2;
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
            }
        }

        //供应商名称编辑完成，根据名称自动搜索ID和No
        private void SupplierNameEditEnded([Row]int row, [Data] string supplierName)
        {
            IDictionary<string, object> foundSupplier =
                GlobalData.AllSuppliers.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == supplierName && s["warehouseId"] != GlobalData.Warehouse["id"];
                });
            if (foundSupplier == null)
            {
                MessageBox.Show($"供应商\"{supplierName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "supplierId"] = foundSupplier["id"];
                this.model1[row, "supplierNo"] = foundSupplier["no"];
            }
        }

        //供应商代号编辑完成，根据名称自动搜索ID和No
        private void SupplierNoEditEnded([Row]int row, [Data] string supplierNo)
        {
            IDictionary<string, object> foundSupplier =
                GlobalData.AllSuppliers.Find((s) =>
                {
                    if (s["no"] == null) return false;
                    return s["name"].ToString() == supplierNo && s["warehouseId"] != GlobalData.Warehouse["id"];
                });
            if (foundSupplier == null)
            {
                MessageBox.Show($"供应商\"{supplierNo}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "supplierId"] = foundSupplier["id"];
                this.model1[row, "suppliername"] = foundSupplier["name"];
            }
        }

        private string StateForwardMapper([Data]int state)
        {
            switch (state)
            {
                case 0: return "待确认";
                case 1: return "已寄出待确认";
                case 2: return "已确认收到";
                default: return "未知状态";
            }
        }

        private int StateBackwardMapper([Data]string state)
        {
            switch (state)
            {
                case "待确认": return 0;
                case "已寄出待确认": return 1;
                case "已确认收到": return 2;
                default: return -1;
            }
        }
    }
}

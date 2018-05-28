using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontWork;

namespace WMS.UI.FromDeliverOrder
{
    public partial class FormDeliveryOrderReady : Form
    {
        private IDictionary<string, object> stockTakingOrder = null;
        private Action addFinishedCallback = null;
        public FormDeliveryOrderReady()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.modelBoxTransferOrderItems.CurrentModelName = "备货作业设置";

        }

        private void FormDeliveryOrderReady_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }
        //备货作业操作
        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                string body = "{\"warehouseId\":\"" + GlobalData.Warehouse["id"] + "\",\"personId\":\"" + GlobalData.Person["id"] + "\"}";
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/stocktaking_order_item/add_all";
                RestClient.Post<List<IDictionary<string, object>>>(url, body);
                MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (this.addFinishedCallback != null)
                {
                    this.addFinishedCallback();
                }

            }
            catch
            {
                MessageBox.Show("添加失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private int WarehouseIdDefaultValue()
        {
            return (int)GlobalData.Warehouse["id"];
        }

        private int CreatePersonIdDefaultValue()
        {
            return (int)GlobalData.Person["id"];
        }

        private string CreatePersonNameDefaultValue()
        {
            return (string)GlobalData.Person["name"];
        }
        //private DateTime? InspectionTimeDefaultValue()
        //{
        //    int row = (int)this.modelWarehouseEntry.SelectionRange.Row;
        //    DateTime? createTime = this.modelWarehouseEntry[row, "createTime"] as DateTime?;
        //    return createTime;
        //}
    }
}

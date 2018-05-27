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

        private void buttonOK_Click(object sender, EventArgs e)
        {

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
        private DateTime? InspectionTimeDefaultValue()
        {
            int row = (int)this.modelWarehouseEntry.SelectionRange.Row;
            DateTime? createTime = this.modelWarehouseEntry[row, "createTime"] as DateTime?;
            return createTime;
        }
    }
}

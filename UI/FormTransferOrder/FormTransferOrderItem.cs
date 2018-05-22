using FrontWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI.FormTransferOrder
{
    public partial class FormTransferOrderItem : Form
    {
        private IDictionary<string, object> transferOrder = null;
        public FormTransferOrderItem(IDictionary<string, object> transferOrder)
        {
            MethodListenerContainer.Register(this);
            this.transferOrder = transferOrder;
            InitializeComponent();
            this.searchView1.AddStaticCondition("transferOrderId", this.transferOrder["id"]);
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, null);
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            this.model1.RemoveSelectedRows();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            this.synchronizer.Save();
        }
        //待工整单完成
        private void buttonFinishAll_Click(object sender, EventArgs e)
        {

        }
        //部分完成
        private void buttonFinish_Click(object sender, EventArgs e)
        {

        }

        private void FormTransferOrderItem_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }
    }
}

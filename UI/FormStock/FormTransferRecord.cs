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
    public partial class FormTransferRecord : Form
    {
        public FormTransferRecord()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private void FormTransferRecord_Load(object sender, EventArgs e)
        {
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private string TransferAmountForwardMapper(double amount, int row)
        {
            double? unitAmount = (double?)this.model1[row, "transferUnitAmount"];
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                return amount.ToString();
            }
            else
            {
                return Utilities.DoubleToString(amount / unitAmount.Value);
            }
        }

        private string OriginalAmountForwardMapper(double amount, int row)
        {
            double? unitAmount = (double?)this.model1[row, "originalUnitAmount"];
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                return amount.ToString();
            }
            else
            {
                return Utilities.DoubleToString(amount / unitAmount.Value);
            }
        }

    }
}

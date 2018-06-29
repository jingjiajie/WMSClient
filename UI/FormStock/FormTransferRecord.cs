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


        private string SourceStorageLocationOriginalAmountMapper(double amount, int row)
        {
            double? unitAmount = (double?)this.model1[row, "sourceStorageLocationUnitAmount"];
            double? sourceStorageLocationNewAmount = (double?)this.model1[row, "sourceStorageLocationNewAmount"];
            string sourceStorageLocationUnit = (string)this.model1[row, "sourceStorageLocationUnit"];
            if (sourceStorageLocationNewAmount.HasValue == false) { return ""; }
            StringBuilder sb = new StringBuilder();
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                sb.Append(Utilities.DoubleToString(amount));
                sb.Append("-->");
                sb.Append(Utilities.DoubleToString(sourceStorageLocationNewAmount.Value));              
                sb.Append("[" + "个" + "(" + "1" + ")]");
                return sb.ToString();
            }   
            else
            {
                sb.Append(Utilities.DoubleToString(amount / unitAmount.Value));
                sb.Append("-->");
                sb.Append(Utilities.DoubleToString(sourceStorageLocationNewAmount.Value/ unitAmount.Value));
                sb.Append("[" +sourceStorageLocationUnit+ "(" + unitAmount + ")]");
                return sb.ToString();
            }
        }


        private string TargetStorageLocationOriginalAmountMapper(double amount, int row)
        {
 
            double? unitAmount = (double?)this.model1[row, "targetStorageLocationAmount"];
            double? targetStorageLocationNewAmount = (double?)this.model1[row, "targetStorageLocationNewAmount"];
            if (targetStorageLocationNewAmount.HasValue == false) { return ""; }
            String targetStorageLocationUnit =(string)this.model1[row, "targetStorageLocationUnit"];
            StringBuilder sb = new StringBuilder();
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                sb.Append(Utilities.DoubleToString(amount));
                sb.Append("-->");                       
                sb.Append(Utilities.DoubleToString(targetStorageLocationNewAmount.Value));          
                sb.Append("[" + "个" + "(" + "1" + ")]");
                return sb.ToString();
            }
            else
            {
                sb.Append(Utilities.DoubleToString(amount / unitAmount.Value));
                sb.Append("-->");
                sb.Append(Utilities.DoubleToString(targetStorageLocationNewAmount.Value / unitAmount.Value));
                sb.Append("[" + targetStorageLocationUnit + "(" + unitAmount + ")]");
                return sb.ToString();
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

        private void configuration1_Load(object sender, EventArgs e)
        {

        }
    }
}

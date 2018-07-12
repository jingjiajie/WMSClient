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
        private int sourceLength1=0;
        private int sourceLength2=0;
        private int targetLength1=0;
        private int targetLength2=0;

        private void FormTransferRecord_Load(object sender, EventArgs e)
        {
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
            this.getAmountLength();
            this.newRefresh();
        }
        private void  newRefresh()
        {
            for (int i = 0; i < this.model1.RowCount; i++)
            {
                this.model1.RefreshView(i);
            }
        }

        private void getAmountLength() {

            for (int i = 0; i < this.model1.RowCount; i++)
            {
                double? unitAmount = (double?)this.model1[i, "sourceStorageLocationUnitAmount"];
                double? sourceStorageLocationNewAmount = (double?)this.model1[i, "sourceStorageLocationNewAmount"];
                double? originalAmount = (double?)this.model1[i, "sourceStorageLocationOriginalAmount"];
                double? newAmount = (double?)this.model1[i, "sourceStorageLocationNewAmount"];
                if (sourceStorageLocationNewAmount.HasValue == false||originalAmount.HasValue==false||newAmount.HasValue==false) { continue; }       
                if (unitAmount.HasValue == false || unitAmount == 0)
                {
                    if (Utilities.DoubleToString(originalAmount.Value).Length > sourceLength1) { this.sourceLength1 = Utilities.DoubleToString(originalAmount.Value).Length; }
                    if (Utilities.DoubleToString(sourceStorageLocationNewAmount.Value).Length > sourceLength2) { this.sourceLength2 = Utilities.DoubleToString(sourceStorageLocationNewAmount.Value).Length; }
                }
                else
                {
                    if (Utilities.DoubleToString(originalAmount.Value / unitAmount.Value).Length > this.sourceLength1) { this.sourceLength1 = Utilities.DoubleToString(originalAmount.Value / unitAmount.Value).Length; }
                    if (Utilities.DoubleToString(sourceStorageLocationNewAmount.Value / unitAmount.Value).Length > this.sourceLength2){ this.sourceLength2 = Utilities.DoubleToString(sourceStorageLocationNewAmount.Value / unitAmount.Value).Length; }
                }                                                        
            }
            for (int i = 0; i < this.model1.RowCount; i++)
            {
                double? unitAmount = (double?)this.model1[i, "targetStorageLocationAmount"];
                double? sourceStorageLocationNewAmount = (double?)this.model1[i, "targetStorageLocationNewAmount"];
                double? originalAmount = (double?)this.model1[i, "targetStorageLocationOriginalAmount"];
                double? newAmount = (double?)this.model1[i, "targetStorageLocationNewAmount"];
                if (sourceStorageLocationNewAmount.HasValue == false || originalAmount.HasValue == false || newAmount.HasValue == false) { continue; }
                if (unitAmount.HasValue == false || unitAmount == 0)
                {
                    if (Utilities.DoubleToString(originalAmount.Value).Length > targetLength1) { this.targetLength1 = Utilities.DoubleToString(originalAmount.Value).Length; }
                    if (Utilities.DoubleToString(sourceStorageLocationNewAmount.Value).Length > targetLength2) { this.targetLength2 = Utilities.DoubleToString(sourceStorageLocationNewAmount.Value).Length; }
                }
                else
                {
                    if (Utilities.DoubleToString(originalAmount.Value / unitAmount.Value).Length > this.targetLength1) { this.targetLength1 = Utilities.DoubleToString(originalAmount.Value / unitAmount.Value).Length; }
                    if (Utilities.DoubleToString(sourceStorageLocationNewAmount.Value / unitAmount.Value).Length > this.targetLength2) { this.targetLength2 = Utilities.DoubleToString(sourceStorageLocationNewAmount.Value / unitAmount.Value).Length; }
                }
            }        
        }

        private string addString(int length) {
            string s = "  ";
            string s1 = "";
            for (int i = 0; i < length; i++) {               
                    s1 = s1 + s;              
            }
            return s1;
        }


        private string SourceStorageLocationOriginalAmountMapper([Data]double amount, [Row]int row)
        {
            double? unitAmount = (double?)this.model1[row, "sourceStorageLocationUnitAmount"];
            double? sourceStorageLocationNewAmount = (double?)this.model1[row, "sourceStorageLocationNewAmount"];
            string sourceStorageLocationUnit = (string)this.model1[row, "sourceStorageLocationUnit"];
            if (sourceStorageLocationNewAmount.HasValue == false) { return ""; }
            
            StringBuilder sb = new StringBuilder();
            if (unitAmount.HasValue == false || unitAmount == 0)
            {               
                sb.Append(Utilities.DoubleToString(amount));
                if (this.sourceLength1 - Utilities.DoubleToString(amount).Length > 0)
                {
                    sb.Append(this.addString(this.sourceLength1 - Utilities.DoubleToString(amount).Length));
                } 
                sb.Append("-->");
                sb.Append(Utilities.DoubleToString(sourceStorageLocationNewAmount.Value));
                if (this.sourceLength2 - Utilities.DoubleToString(sourceStorageLocationNewAmount.Value).Length > 0)
                {
                    sb.Append(this.addString(this.sourceLength2 - Utilities.DoubleToString(sourceStorageLocationNewAmount.Value).Length));
                }
                sb.Append("[" + "个" + "(" + "1" + ")]");
                return sb.ToString();
            }   
            else
            {
                sb.Append(Utilities.DoubleToString(amount / unitAmount.Value));
                if (this.sourceLength1 - Utilities.DoubleToString(amount / unitAmount.Value).Length > 0)
                {
                    sb.Append(this.addString( this.sourceLength1 - Utilities.DoubleToString(amount / unitAmount.Value).Length));
                }
                sb.Append("-->");              
                sb.Append(Utilities.DoubleToString(sourceStorageLocationNewAmount.Value/ unitAmount.Value));
                if (this.sourceLength2 - Utilities.DoubleToString(sourceStorageLocationNewAmount.Value / unitAmount.Value).Length > 0)
                {
                    sb.Append(this.addString( this.sourceLength2 - Utilities.DoubleToString(sourceStorageLocationNewAmount.Value / unitAmount.Value).Length));
                }
                sb.Append("[" +sourceStorageLocationUnit+ "(" + unitAmount + ")]");
                return sb.ToString();
            }
        }


        private string TargetStorageLocationOriginalAmountMapper([Data]double amount, [Row]int row)
        {
 
            double? unitAmount = (double?)this.model1[row, "targetStorageLocationAmount"];
            double? targetStorageLocationNewAmount = (double?)this.model1[row, "targetStorageLocationNewAmount"];
            if (targetStorageLocationNewAmount.HasValue == false) { return ""; }
            String targetStorageLocationUnit =(string)this.model1[row, "targetStorageLocationUnit"];
            StringBuilder sb = new StringBuilder();
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                sb.Append(Utilities.DoubleToString(amount));
                if (this.sourceLength1 - Utilities.DoubleToString(amount).Length > 0)
                {
                    sb.Append(this.addString(this.sourceLength1 - Utilities.DoubleToString(amount).Length));
                }
                sb.Append("-->");
                sb.Append(Utilities.DoubleToString(targetStorageLocationNewAmount.Value));
                if (this.sourceLength2 - Utilities.DoubleToString(targetStorageLocationNewAmount.Value).Length > 0)
                {
                    sb.Append(this.addString(this.sourceLength2 - Utilities.DoubleToString(targetStorageLocationNewAmount.Value).Length));
                }
                sb.Append("[" + "个" + "(" + "1" + ")]");
                return sb.ToString();
            }
            else
            {
                sb.Append(Utilities.DoubleToString(amount / unitAmount.Value));
                if (this.sourceLength1 - Utilities.DoubleToString(amount / unitAmount.Value).Length > 0)
                {
                    sb.Append(this.addString(this.sourceLength1 - Utilities.DoubleToString(amount / unitAmount.Value).Length));
                }
                sb.Append("-->");
                sb.Append(Utilities.DoubleToString(targetStorageLocationNewAmount.Value / unitAmount.Value));
                if (this.sourceLength2 - Utilities.DoubleToString(targetStorageLocationNewAmount.Value / unitAmount.Value).Length > 0)
                {
                    sb.Append(this.addString( this.sourceLength2 - Utilities.DoubleToString(targetStorageLocationNewAmount.Value / unitAmount.Value).Length));
                }
                sb.Append("[" + targetStorageLocationUnit + "(" + unitAmount + ")]");
                return sb.ToString();
            }
         
        }

        private string OriginalAmountForwardMapper([Data]double amount, [Row]int row)
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

        private string TransferAmountForwardMapper([Data]double amount, [Row]int row)
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

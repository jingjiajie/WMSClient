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
    public partial class FormAddStockRecord : Form
    {
        private Action addFinishedCallback = null;
        private int rowCur = 0;
        public FormAddStockRecord()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            string s = Interaction.InputBox("请输入需要添加的行数", "提示", "1", -1, -1);  //-1表示在屏幕的中间         
            int row = 1;
            try
            {
                row = Convert.ToInt32(s);
            }
            catch
            {
                MessageBox.Show("请输入正确的数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            for (int i = 0; i < row; i++)
            {
                this.model1.InsertRow(0, new Dictionary<string, object>()
                {
                });
            }
        }

        private int WarehouseIdDefaultValue()
        {
            return (int)GlobalData.Warehouse["id"];
        }

        private void FormAddStockRecord_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
        }


        private void StorageLocationNameEditEnded([Row]int row, [Data]string storageAreaName)
        {
            IDictionary<string, object> foundStorageLocations =
                GlobalData.AllStorageLocations.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == storageAreaName;
                });
            if (foundStorageLocations == null)
            {
                MessageBox.Show($"库区\"{storageAreaName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "storageLocationId"] = foundStorageLocations["id"];
                this.model1[row, "storageLocationNo"] = foundStorageLocations["no"];
            }
        }

        private void StorageLocationNoEditEnded([Row]int row,[Data] string storageLocationName)
        {
            IDictionary<string, object> foundStorageLcations =
                GlobalData.AllStorageLocations.Find((s) =>
                {
                    if (s["no"] == null) return false;
                    return s["no"].ToString() == storageLocationName;
                });
            if (foundStorageLcations == null)
            {
                MessageBox.Show($"库位编号\"{storageLocationName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "storageLocationId"] = foundStorageLcations["id"];
                this.model1[row, "storageLocationName"] = foundStorageLcations["name"];
            }
        }

        private string AmountForwardMapper([Data]double amount, [Row] int row)
        {
            double? unitAmount = (double?)this.model1[row, "unitAmount"];
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                return amount.ToString();
            }
            else
            {
                return Utilities.DoubleToString(amount / unitAmount.Value);
            }
        }

        private double AmountBackwardMapper([Data]string strAmount, [Row]int row)
        {
            if (!Double.TryParse(strAmount, out double amount))
            {
                MessageBox.Show($"\"{strAmount}\"不是合法的数字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
            double? unitAmount = (double?)this.model1[row, "unitAmount"];
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                return amount;
            }
            else
            {
                return amount * unitAmount.Value;
            }
        }

        private void UnitAmountEditEnded([Row]int row)
        {
            /*
            if (string.IsNullOrWhiteSpace(this.model1[row, "unitAmount"]?.ToString())) return;            
            this.GetAmount(row, (double)this.model1[row, "unitAmount"]);
            this.GetAvailableAmount(row, (double)this.model1[row, "unitAmount"]);
            */
            this.model1.RefreshView(row);
        }
    /*
        private void GetAmount(int row, double unitAmount)
        {
            //if (this.amountTemp == 0) { return; }
            this.model1[row, "amount"] = Utilities.DoubleToString(this.amountTemp[row] * unitAmount);
        }

        private void GetAvailableAmount(int row, double unitAmount)
        {
            //if (this.availableAmountTemp == 0) { return; }
            this.model1[row, "availableAmount"] = Utilities.DoubleToString(this.availableAmountTemp[row] * unitAmount);
        }
        */
        private string AvailableAmountForwardMapper([Data]double amount, [Row] int row)
        {
            double? unitAmount = (double?)this.model1[row, "unitAmount"];
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                return amount.ToString();
            }
            else
            {
                return Utilities.DoubleToString(amount / unitAmount.Value);
            }
        }

        private double AvailableAmountBackwardMapper([Data]string strAmount, [Row] int row)
        {
            if (!Double.TryParse(strAmount, out double amount))
            {
                MessageBox.Show($"\"{strAmount}\"不是合法的数字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
            double? unitAmount = (double?)this.model1[row, "unitAmount"];
            if (unitAmount.HasValue == false || unitAmount == 0)
            {              
                return amount;
            }
            else
            {
                return amount * unitAmount.Value;
            }
        }

        //===========为了实现一个看起来天经地义的交互逻辑=========

        private void SupplierNoEditEnded([Row]int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "supplierNo"]?.ToString())) return;
            this.model1[row, "supplierName"] = "";
            this.FindSupplierID(row);
            this.TryGetSupplyID(row);
        }

        private void SupplierNameEditEnded([Row]int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "supplierName"]?.ToString())) return;
            this.model1[row, "supplierNo"] = "";
            this.FindSupplierID(row);
            this.TryGetSupplyID(row);
        }

        private void MaterialNoEditEnded([Row]int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "materialNo"]?.ToString())) return;
            //this.model1[row, "materialName"] = "";
            this.FindMaterialID(row);
            this.TryGetSupplyID(row);
        }

        private void MaterialNameEditEnded([Row]int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "materialName"]?.ToString())) return;
            //this.model1[row, "materialNo"] = "";
            this.FindMaterialID(row);
            this.TryGetSupplyID(row);
        }

        private void MaterialProductLineEditEnded([Row]int row)
        {
            this.FindMaterialID(row);
            this.TryGetSupplyID(row);
        }

        private void FindMaterialID(int row)
        {
            this.model1[row, "materialId"] = 0; //先清除物料ID
            string materialNo = this.model1[row, "materialNo"]?.ToString() ?? "";
            string materialName = this.model1[row, "materialName"]?.ToString() ?? "";
            string materialProductLine = this.model1[row, "materialProductLine"]?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(materialNo) && string.IsNullOrWhiteSpace(materialName)) return;
            var foundMaterials = (from m in GlobalData.AllMaterials
                                  where (string.IsNullOrWhiteSpace(materialNo) ? true : (m["no"]?.ToString() ?? "") == materialNo)
                                  && (string.IsNullOrWhiteSpace(materialName) ? true : (m["name"]?.ToString() ?? "") == materialName)
                                  && (string.IsNullOrWhiteSpace(materialProductLine) ? true : materialProductLine == (m["productLine"]?.ToString() ?? ""))
                                  select m).ToArray();
            if (foundMaterials.Length != 1)
            {
                goto FAILED;
            }
            this.model1[row, "materialId"] = foundMaterials[0]["id"];
            this.model1[row, "materialNo"] = foundMaterials[0]["no"];
            this.model1[row, "materialName"] = foundMaterials[0]["name"];
            this.model1[row, "materialProductLine"] = foundMaterials[0]["productLine"];
            return;

            FAILED:
            if (string.IsNullOrWhiteSpace(materialProductLine)) return;
            rowCur = row + 1;
            MessageBox.Show("行:"+rowCur+" 物料 名称："+ materialName+" 代号："+materialNo + " 不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void FindSupplierID(int row)
        {
            this.model1[row, "supplierId"] = 0;//先清除供货商ID
            string supplierNo = this.model1[row, "supplierNo"]?.ToString() ?? "";
            string supplierName = this.model1[row, "supplierName"]?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(supplierNo) && string.IsNullOrWhiteSpace(supplierName)) return;

            var foundSuppliers = (from s in GlobalData.AllSuppliers
                                  where (string.IsNullOrWhiteSpace(supplierNo) ? true : (s["no"]?.ToString() ?? "") == supplierNo)
                                  && (string.IsNullOrWhiteSpace(supplierName) ? true : (s["name"]?.ToString() ?? "") == supplierName)
                                  select s).ToArray();
            if (foundSuppliers.Length != 1) goto FAILED;
            int supplierID = (int)foundSuppliers[0]["id"];
            this.model1[row, "supplierId"] = foundSuppliers[0]["id"];
            this.model1[row, "supplierNo"] = foundSuppliers[0]["no"];
            this.model1[row, "supplierName"] = foundSuppliers[0]["name"];
            return;

            FAILED:
            rowCur = row + 1;
            MessageBox.Show("行:" + rowCur + "供应商 名称："+supplierName+" 代号： "+supplierNo+"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void TryGetSupplyID(int row)
        {
            this.model1[row, "supplyId"] = 0; //先清除供货ID
            int supplierId = (int?)this.model1[row, "supplierId"] ?? 0;
            int materialId = (int?)this.model1[row, "materialId"] ?? 0;
            if (supplierId == 0 || materialId == 0) return;
            var foundSupplies = (from s in GlobalData.AllSupplies
                                 where (int)s["supplierId"] == supplierId
                                 && (int)s["materialId"] == materialId
                                 select s).ToArray();
            //如果找到供货信息，则把供货设置的默认入库信息拷贝到相应字段上
            if (foundSupplies.Length == 1)
            {
                this.model1[row, "supplyId"] = foundSupplies[0]["id"];
            }
            else
            {
                rowCur = row + 1;
                MessageBox.Show("行:" + rowCur +"无此供货！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        //物料名称输入联想
        private object[] MaterialNameAssociation([Data]string str)
        {

            string materialNo = this.model1[this.model1.SelectionRange.Row, "materialNo"]?.ToString() ?? "";
            int[] selectedIDs = this.model1.GetSelectedRows<int>("supplierId").Except(new int[] { 0 }).ToArray();

            if (selectedIDs.Length == 0)
            {
                var a = (from s in GlobalData.AllSupplies
                         where s["materialName"] != null &&
                         s["materialName"].ToString().StartsWith(str)
                         && s["warehouseId"] != GlobalData.Warehouse["id"]
                         && (string.IsNullOrWhiteSpace(materialNo) ? true : (s["materialNo"]?.ToString() ?? "") == materialNo)
                         select s["materialName"]).ToArray();
                return a.GroupBy(p => p).Select(p => p.Key).ToArray();
            }
            else
            {
                var a = (from s in GlobalData.AllSupplies
                         where s["materialName"] != null &&
                         s["materialName"].ToString().StartsWith(str) &&
                         (int)s["supplierId"] == selectedIDs[0]
                         && s["warehouseId"] != GlobalData.Warehouse["id"]
                         && (string.IsNullOrWhiteSpace(materialNo) ? true : (s["materialNo"]?.ToString() ?? "") == materialNo)
                         select s["materialName"]).ToArray();
                return a.GroupBy(p => p).Select(p => p.Key).ToArray();
            }

        }

        //物料代号输入联想
        private object[] MaterialNoAssociation([Data]string str)
        {
            string materialName = this.model1[this.model1.SelectionRange.Row, "materialName"]?.ToString() ?? "";

            int[] selectedIDs = this.model1.GetSelectedRows<int>("supplierId").Except(new int[] { 0 }).ToArray();
            if (selectedIDs.Length == 0)
            {
                var a = (from s in GlobalData.AllSupplies
                         where s["materialNo"] != null &&
                         s["materialNo"].ToString().StartsWith(str)
                         && s["warehouseId"] != GlobalData.Warehouse["id"]
                         && (string.IsNullOrWhiteSpace(materialName) ? true : (s["materialName"]?.ToString() ?? "") == materialName)
                         select s["materialNo"]).ToArray();
                return a.GroupBy(p => p).Select(p => p.Key).ToArray();
            }
            else
            {
                var a = (from s in GlobalData.AllSupplies
                         where s["materialNo"] != null &&
                         s["materialNo"].ToString().StartsWith(str) &&
                         (int)s["supplierId"] == selectedIDs[0]
                         && s["warehouseId"] != GlobalData.Warehouse["id"]
                         && (string.IsNullOrWhiteSpace(materialName) ? true : (s["materialName"]?.ToString() ?? "") == materialName)

                         select s["materialNo"]).ToArray();
                return a.GroupBy(p => p).Select(p => p.Key).ToArray();
            }
        }

        //物料系列输入联想
        private object[] MaterialProductLineAssociation([Data]string str)
        {
            int[] selectedIDs = this.model1.GetSelectedRows<int>("supplierId").Except(new int[] { 0 }).ToArray();
            if (selectedIDs.Length == 0)
            {
                var a = (from s in GlobalData.AllSupplies
                         where s["materialProductLine"] != null &&
                         s["materialProductLine"].ToString().StartsWith(str)
                         && s["warehouseId"] != GlobalData.Warehouse["id"]
                         select s["materialProductLine"]).ToArray();
                return a.GroupBy(p => p).Select(p => p.Key).ToArray();
            }
            else
            {
                var a = (from s in GlobalData.AllSupplies
                         where s["materialProductLine"] != null &&
                         s["materialProductLine"].ToString().StartsWith(str) &&
                         (int)s["supplierId"] == selectedIDs[0]
                         && s["warehouseId"] != GlobalData.Warehouse["id"]
                         select s["materialProductLine"]).ToArray();
                return a.GroupBy(p => p).Select(p => p.Key).ToArray();
            }

        }



        private void FillDefaultValue(int row, string fieldName, object value)
        {
            this.model1[row, fieldName] = value;
        }

        public void SetAddFinishedCallback(Action callback)
        {
            this.addFinishedCallback = callback;
        }

        private void FormReturnSupply_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.addFinishedCallback != null)
            { this.addFinishedCallback(); }
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save()) { this.Close(); }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {

        }

        private int StateBackwardMapper([Data]string enable)
        {
            switch (enable)
            {
                case "待检测": return 0;
                case "不合格": return 1;
                case "合格": return 2;
                default: return -1;
            }
        }

        private string StateForwardMapper([Data]int enable)
        {
            switch (enable)
            {
                case 0: return "待检测";
                case 1: return "不合格";
                case 2: return "合格";
                default: return "未知状态";
            }
        }

    

        private void toolStripButtonDelete_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }
    }
}

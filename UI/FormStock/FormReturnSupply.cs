﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontWork;
using Microsoft.VisualBasic;

namespace WMS.UI.FormStock
{
    public partial class FormReturnSupply : Form
    {
        private Action addFinishedCallback = null;
        //private int rowCur;
        public FormReturnSupply()
        {         
            MethodListenerContainer.Register("FormReturnSupply",this);
            InitializeComponent();
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
                // MessageBox.Show($"库区\"{storageAreaName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.model1.UpdateCellState(row, "storageLocationName", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "库位名称错误！")));
            }
            else
            {
                this.model1[row, "storageLocationId"] = foundStorageLocations["id"];
                this.model1[row, "storageLocationNo"] = foundStorageLocations["no"];
                this.model1.UpdateCellState(row, "storageLocationName", new ModelCellState(ValidationState.OK));
            }
        }

        private void StorageLocationNoEditEnded([Row]int row, [Data] string storageLocationName)
        {
            IDictionary<string, object> foundStorageLcations =
                GlobalData.AllStorageLocations.Find((s) =>
                {
                    if (s["no"] == null) return false;
                    return s["no"].ToString() == storageLocationName;
                });
            if (foundStorageLcations == null)
            {
                //MessageBox.Show($"库位编号\"{storageLocationName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.model1.UpdateCellState(row, "storageLocationNo", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "库位编号错误！")));
            }
            else
            {
                this.model1[row, "storageLocationId"] = foundStorageLcations["id"];
                this.model1[row, "storageLocationName"] = foundStorageLcations["name"];
                this.model1.UpdateCellState(row, "storageLocationNo", new ModelCellState(ValidationState.OK));
            }
        }

        private string AmountForwardMapper(double amount,int row)
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
                //MessageBox.Show($"\"{strAmount}\"不是合法的数字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.model1.UpdateCellState(row, "amount", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "不合法的数字！")));
                return 0;
            }
            double? unitAmount = (double?)this.model1[row, "unitAmount"];
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                this.model1.UpdateCellState(row, "amount", new ModelCellState(ValidationState.OK));
                return amount;
            }
            else
            {
                this.model1.UpdateCellState(row, "amount", new ModelCellState(ValidationState.OK));
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
        //private string AvailableAmountForwardMapper([Data]double amount,[Row] int row)
        //{
        //    double? unitAmount = (double?)this.model1[row, "unitAmount"];
        //    if (unitAmount.HasValue == false || unitAmount == 0)
        //    {
        //        return amount.ToString();
        //    }
        //    else
        //    {
        //        return Utilities.DoubleToString(amount / unitAmount.Value);
        //    }
        //}

        //private double AvailableAmountBackwardMapper([Data]string strAmount, [Row]int row)
        //{
        //    if (!Double.TryParse(strAmount, out double amount))
        //    {
        //        MessageBox.Show($"\"{strAmount}\"不是合法的数字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return 0;
        //    }
        //    double? unitAmount = (double?)this.model1[row, "unitAmount"];
        //    if (unitAmount.HasValue == false || unitAmount == 0)
        //    {
        //        return amount;
        //    }
        //    else
        //    {
        //        return amount * unitAmount.Value;
        //    }
        //}

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
                //MessageBox.Show($"\"{strAmount}\"不是合法的数字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.model1.UpdateCellState(row, "availableAmount", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "不合法的数字！")));
                return 0;
            }
            double? unitAmount = (double?)this.model1[row, "unitAmount"];
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                this.model1.UpdateCellState(row, "availableAmount", new ModelCellState(ValidationState.OK));
                return amount;
            }
            else
            {
                this.model1.UpdateCellState(row, "availableAmount", new ModelCellState(ValidationState.OK));
                return amount * unitAmount.Value;
            }
        }

        public void SupplySerialNoEditEnded([Model] IModel model, [Row] int row)
        {
            string supplySerialNo = model[row, "supplySerialNo"]?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(supplySerialNo)) return;
            var foundSupplies = (from m in GlobalData.AllSupplies
                                 where supplySerialNo == (string)m["serialNo"]
                                 select m).ToArray();
            if (foundSupplies.Length != 1)
            {
                model.UpdateCellState(row, "supplySerialNo", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "供货不存在！")));
                return;
            }
            this.FillSupplyFields(model, row, foundSupplies[0]);
        }

        //public string[] SupplySerialNoAssociation([Model] IModel model, [Row] int row, [Data] string input)
        //{
        //    return (from s in GlobalData.AllSupplies
        //            where s["serialNo"] != null
        //            && s["serialNo"].ToString().StartsWith(input)
        //            && (int)s["supplierId"] == (int)this.model1[row, "supplierId"]
        //            && s["warehouseId"].Equals(GlobalData.Warehouse["id"])
        //            select s["serialNo"]?.ToString()).Distinct().ToArray();
        //}

        private void FillSupplyFields(IModel model, int row, IDictionary<string, object> supply)
        {
            model[row, "supplyId"] = supply["id"];
            model[row, "supplySerialNo"] = supply["serialNo"];
            model[row, "materialId"] = supply["materialId"];
            model[row, "materialNo"] = supply["materialNo"];
            model[row, "materialName"] = supply["materialName"];
            model[row, "materialProductLine"] = supply["materialProductLine"];
            model[row, "supplierId"] = supply["supplierId"];
            model[row, "supplierNo"] = supply["supplierNo"];
            model[row, "supplierName"] = supply["supplierName"];
            model[row, "unit"] = supply["defaultEntryUnit"];
            model[row, "unitAmount"] = supply["defaultEntryUnitAmount"];


            model.UpdateCellState(row, "supplySerialNo", new ModelCellState(ValidationState.OK));
            model.RefreshView(row);
            return;
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
            this.model1.UpdateCellState(row, "materialNo", new ModelCellState(ValidationState.OK));
            this.model1.UpdateCellState(row, "materialName", new ModelCellState(ValidationState.OK));
            this.model1.UpdateCellState(row, "materialProductLine", new ModelCellState(ValidationState.OK));
            this.model1.RefreshView(row);
            return;

            FAILED:
            //if (string.IsNullOrWhiteSpace(materialProductLine)) return;
            //rowCur = row + 1;
            //MessageBox.Show("行:"+rowCur+" 物料 名称："+ materialName+" 代号："+materialNo + " 不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //return;
            this.model1.UpdateCellState(row, "materialNo", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "物料不存在！")));
            this.model1.UpdateCellState(row, "materialName", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "物料不存在！")));
            this.model1.UpdateCellState(row, "materialProductLine", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "物料不存在！")));
            this.model1.RefreshView(row);
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
            this.model1.UpdateCellState(row, "supplierNo", new ModelCellState(ValidationState.OK));
            this.model1.UpdateCellState(row, "supplierName", new ModelCellState(ValidationState.OK));
            this.model1.RefreshView(row);
            return;

            FAILED:
            //rowCur = row + 1;
            //MessageBox.Show("行:" + rowCur + "供应商 名称："+supplierName+" 代号： "+supplierNo+"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //return;
            this.model1.UpdateCellState(row, "supplierNo", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "供应商不存在！")));
            this.model1.UpdateCellState(row, "supplierName", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "供应商不存在！")));
            this.model1.RefreshView(row);
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
                this.model1.UpdateCellState(row, "materialNo", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "物料不存在！")));
                this.model1.UpdateCellState(row, "materialName", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "物料不存在！")));
                this.model1.UpdateCellState(row, "materialProductLine", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "物料不存在！")));
                this.model1.UpdateCellState(row, "supplierNo", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "供应商不存在！")));
                this.model1.UpdateCellState(row, "supplierName", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "供应商不存在！")));
                this.model1.RefreshView(row);
            }
            else
            {
                //rowCur = row + 1;
                //MessageBox.Show("行:" + rowCur +"无此供货！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //return;
                this.model1.UpdateCellState(row, "materialNo", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "物料不存在！")));
                this.model1.UpdateCellState(row, "materialName", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "物料不存在！")));
                this.model1.UpdateCellState(row, "materialProductLine", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "物料不存在！")));
                this.model1.UpdateCellState(row, "supplierNo", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "供应商不存在！")));
                this.model1.UpdateCellState(row, "supplierName", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "供应商不存在！")));
                this.model1.RefreshView(row);
                return;
            }
        }

        private void FillDefaultValue(int row, string fieldName, object value)
        {
            this.model1[row, fieldName] = value;
        }

        private void toolStripButtonAdd_Click_1(object sender, EventArgs e)
        {
            //this.basicView1.Enabled = true;
            //this.reoGridView1.Enabled = true;
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

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            { this.Close(); }
        }

        private void FormReturnSupply_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            //this.updateBasicAndReoGridView();
        }

        private void updateBasicAndReoGridView()
        {
            /*
            if (this.model1.RowCount == 0)
            {
                this.basicView1.Enabled = false;
                this.reoGridView1.Enabled = false;
            }
            else
            {
                this.basicView1.Enabled = true;
                this.reoGridView1.Enabled = true;
            }*/
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

        private int StateBackwardMapper([Data]string enable)
        {
            switch (enable)
            {
                case "不合格": return 1;
                case "合格": return 2;
                default: return -1;
            }
        }

        private string StateForwardMapper(int enable)
        {
            switch (enable)
            {              
                case 1: return "不合格";
                case 2: return "合格";
                default: return "未知状态";
            }
        }

        private void StateContentChanged([Row]int row,[Data]string state   )        
        {           
            if ((int)this.model1[row,"supplyId"]==0) return;          
            var foundSupplies = (from s in GlobalData.AllSupplies
                                  where (int)s["id"] == (int)this.model1[row, "supplyId"]
                                  select s).ToArray();
            if (foundSupplies.Length == 1)
            {               
                if (state =="合格")
                {
                    this.model1[row, "storageLocationNo"] = (string)foundSupplies[0]["defaultDeliveryStorageLocationNo"];
                    this.model1[row, "storageLocationName"] = (string)foundSupplies[0]["defaultDeliveryStorageLocationName"];
                    this.model1[row, "storageLocationId"] = foundSupplies[0]["defaultDeliveryStorageLocationId"] == null ? 0 : (int)foundSupplies[0]["defaultDeliveryStorageLocationId"];
                }
                else if (state =="不合格")
                {
                    this.model1[row, "storageLocationNo"] = (string)foundSupplies[0]["defaultUnqualifiedStorageLocationNo"];
                    this.model1[row, "storageLocationName"] = (string)foundSupplies[0]["defaultUnqualifiedStorageLocationName"];
                    this.model1[row, "storageLocationId"] = foundSupplies[0]["defaultUnqualifiedStorageLocationId"] == null ? 0 : (int)foundSupplies[0]["defaultUnqualifiedStorageLocationId"];
                }
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }
    }
}

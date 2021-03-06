﻿using FrontWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using Microsoft.VisualBasic;

namespace WMS.UI.FormTransferOrder
{
    public partial class FormTransferOrderItem : Form
    {
        private IDictionary<string, object> transferOrder = null;
        private Action addFinishedCallback = null;
        public FormTransferOrderItem(IDictionary<string, object> transferOrder)
        {
            MethodListenerContainer.Register(this);
            this.transferOrder = transferOrder;
            InitializeComponent();
            this.searchView1.AddStaticCondition("transferOrderId", this.transferOrder["id"]);
            if (this.transferOrder["supplierId"] == null) {
                this.basicView1.Mode = "no_supplier";
                this.reoGridView1.Mode = "no_supplier";
            }
        }



        private void model_CellUpdated(object sender, ModelCellUpdatedEventArgs e)
        {
            foreach (var cell in e.UpdatedCells)
            {
                if (cell.FieldName.StartsWith("lastUpdate")) return;
                this.model1[cell.Row, "lastUpdatePersonId"] = GlobalData.Person["id"];
                this.model1[cell.Row, "lastUpdatePersonName"] = GlobalData.Person["name"];
                this.model1[cell.Row, "lastUpdateTime"] = DateTime.Now;
            }
        }
        private int transferOrderIdDefaultValue()
        {
            return (int)this.transferOrder["id"];
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
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
                { "personId",GlobalData.Person["id"]},
                { "personName",GlobalData.Person["name"]},
                { "transferOrderNo", this.transferOrder["no"]},
                { "operateTime", DateTime.Now},
                { "supplierId",this.transferOrder["supplierId"]},
                { "supplierNo",this.transferOrder["supplierNo"]},
                { "supplierName",this.transferOrder["supplierName"]}
            });
            }
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

        private string AmountForwardMapper(double amount, [Row]int row)
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
            if (row == -1)
            {
                return amount;
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
            this.model1.RefreshView(row);
        }

        //待工整单完成
        private void buttonFinishAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认保存当前修改并整单完成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.synchronizer.Save();
            try
            {
                string body = "{\"personId\":\"" + GlobalData.Person["id"] + "\",\"transferOrderId\":\"" +this.transferOrder["id"] + "\"}";
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/transfer_order/transfer_finish";
                RestClient.RequestPost<List<IDictionary<string, object>>>(url, body, "PUT");
                MessageBox.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.searchView1.Search();

            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show(("整单完成移库单条目") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //部分完成
        private void buttonFinish_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认保存当前修改并完成选中条目吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            if (this.model1.HasUnsynchronizedUpdatedRow())
            {
                if (!this.synchronizer.Save()) return;
            }
            this.TransferDone();
        }

        private string StateForwardMapper([Data]int state)
        {
            
            switch (state)
            {
                case 0: return "待备货";
                case 1: return "部分备货";
                case 2: return "备货完成";
                default: return "未知状态";
            }
        }

        private int StateBackwardMapper([Data]string state)
        {

            switch (state)
            {
                case "待备货": return 0;
                case "部分备货": return 1;
                case "备货完成": return 2;
                default: return -1;
            }
        }

        private void TransferDone()
        {
            //获取选中行ID，过滤掉新建的行（ID为0的）
            int[] selectedIDs = this.model1.GetSelectedRows<int>("id").Except(new int[] { 0 }).ToArray();
            if (selectedIDs.Length == 0)
            {
                MessageBox.Show("请选择一项进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string strIDs = serializer.Serialize(selectedIDs);
            try
            {
                string operatioName = "transfer_some/" + (int)GlobalData.Person["id"];
                RestClient.RequestPost<string>(Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/transfer_order/" + operatioName, strIDs, "PUT");
                this.searchView1.Search();
                MessageBox.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show(( "批量完成移库单条目") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FormTransferOrderItem_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            Utilities.BindBlueButton(this.buttonFinish);
            Utilities.BindBlueButton(this.buttonFinishAll);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
            //this.updateBasicAndReoGridView();
        }

        private void toolStripButtonAdd_Click_1(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "personId",GlobalData.Person["id"]},
                { "personName",GlobalData.Person["name"]},
                { "transferOrderNo", this.transferOrder["no"]},
                { "operateTime", DateTime.Now},
            });
        }

        public void SetAddFinishedCallback(Action callback)
        {
            this.addFinishedCallback = callback;
        }

        private void FormTransferOrderItemClosed(object sender, FormClosedEventArgs e)
        {
            if (this.addFinishedCallback != null)
            {
                this.addFinishedCallback();
            }
        }

        private void toolStripTop_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        //=============天经地义的交互逻辑到这里结束===============
    }

    [MethodListener]
    public class FormTransferOrderItemMethodListener
    {
        private void PersonEditEnded([Model] IModel model, [Row] int row, [Data] string personName)
        {
            model[row, "personId"] = 0;//先清除ID
            if (string.IsNullOrWhiteSpace(personName)) return;
            var foundPersons = (from s in GlobalData.AllPersons
                                where s["name"]?.ToString() == personName
                                select s).ToArray();
            if (foundPersons.Length != 1) goto FAILED;
            model[row, "personId"] = (int)foundPersons[0]["id"];
            model[row, "personName"] = foundPersons[0]["name"];
            return;

            FAILED:
            MessageBox.Show($"人员\"{personName}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        public string[] SupplySerialNoAssociation([Model] IModel model, [Row] int row, [Data] string input)
        {
            int supplierId = (int?)model[row, "supplierId"] ?? 0;
            if (supplierId == 0)
            {
                return (from s in GlobalData.AllSupplies
                        where s["serialNo"] != null
                        && s["serialNo"].ToString().StartsWith(input)
                        //&& (int)s["supplierId"] == (int)model[row, "supplierId"]
                        && s["warehouseId"].Equals(GlobalData.Warehouse["id"])
                        select s["serialNo"]?.ToString()).Distinct().ToArray();
            }
            else
            {
                return (from s in GlobalData.AllSupplies
                        where s["serialNo"] != null
                        && s["serialNo"].ToString().StartsWith(input)
                        && (int)s["supplierId"] == (int)model[row, "supplierId"]
                        && s["warehouseId"].Equals(GlobalData.Warehouse["id"])
                        select s["serialNo"]?.ToString()).Distinct().ToArray();
            }
        }

        public void SupplySerialNoEditEnded([Model] IModel model, [Row] int row)
        {
            string supplySerialNo = model[row, "supplySerialNo"]?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(supplySerialNo)) return;
            //var foundSupplies = (from m in GlobalData.AllSupplies
            //                     where supplySerialNo == (string)m["serialNo"]
            //                     select m).ToArray();
            //if (foundSupplies.Length != 1)
            //{
            //    model.UpdateCellState(row, "supplySerialNo", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "供货不存在！")));
            //    return;
            //}
            //this.FillSupplyFields(model, row, foundSupplies[0]);
            int supplierId = (int?)model[row, "supplierId"] ?? 0;
            if (string.IsNullOrWhiteSpace(supplySerialNo)) return;
            if (supplierId == 0)
            {
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
            else
            {
                var foundSupplies = (from m in GlobalData.AllSupplies
                                     where supplySerialNo == (string)m["serialNo"]
                                     && supplierId == (int)m["supplierId"]
                                     select m).ToArray();
                if (foundSupplies.Length != 1)
                {
                    model.UpdateCellState(row, "supplySerialNo", new ModelCellState(new ValidationState(ValidationStateType.ERROR, "供货不存在！")));
                    return;
                }
                this.FillSupplyFields(model, row, foundSupplies[0]);
            }
        }

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
            if (supply["defaultDeliveryAmount"] != null)
            {
                model[row, "scheduledAmount"] = supply["defaultDeliveryAmount"];
            }        
            model[row, "unit"] = supply["defaultDeliveryUnit"];
            model[row, "unitAmount"] = supply["defaultDeliveryUnitAmount"];
            model[row, "sourceUnit"] = supply["defaultDeliveryUnit"];
            model[row, "sourceUnitAmount"] = supply["defaultDeliveryUnitAmount"];

            string targetStorageLocationNo = supply["defaultPrepareTargetStorageLocationNo"] as string;
            string sourceStorageLocationNo = supply["defaultDeliveryStorageLocationNo"] as string;

            model[row, "targetStorageLocationName"] = null;
            model[row, "sourceStorageLocationName"] = null;

            model[row, "targetStorageLocationNo"] = targetStorageLocationNo;
            model[row, "sourceStorageLocationNo"] = sourceStorageLocationNo;

            this.FindStorageLocation(model, row, "targetStorageLocation", FindStorageLocationBy.NO, targetStorageLocationNo, false);
            this.FindStorageLocation(model, row, "sourceStorageLocation", FindStorageLocationBy.NO, sourceStorageLocationNo, false);
            model.UpdateCellState(row, "supplySerialNo", new ModelCellState(ValidationState.OK));
            model.RefreshView(row);
            return;
        }


        private void SourceStorageLocationNoEditEnded([Model] IModel model, [Row] int row, [Data] string storageLocationNo)
        {
            this.FindStorageLocation(model, row, "sourceStorageLocation", FindStorageLocationBy.NO, storageLocationNo);
        }

        private void SourceStorageLocationNameEditEnded([Model] IModel model, [Row] int row, [Data] string storageLocationName)
        {
            this.FindStorageLocation(model, row, "sourceStorageLocation", FindStorageLocationBy.NAME, storageLocationName);
        }

        private void TargetStorageLocationNoEditEnded([Model] IModel model, [Row] int row, [Data] string storageLocationNo)
        {
            this.FindStorageLocation(model, row, "targetStorageLocation", FindStorageLocationBy.NO, storageLocationNo);
        }

        private void TargetStorageLocationNameEditEnded([Model] IModel model, [Row] int row, [Data] string storageLocationName)
        {
            this.FindStorageLocation(model, row, "targetStorageLocation", FindStorageLocationBy.NAME, storageLocationName);
        }

        enum FindStorageLocationBy
        {
            NAME, NO
        }

        private void FindStorageLocation(IModel model, int row, string storageLocationFieldName, FindStorageLocationBy byField, string value, bool warning = true)
        {
            model[row, storageLocationFieldName + "Id"] = 0;//先清除库位ID

            var foundStorageLocations = (from s in GlobalData.AllStorageLocations
                                         where s[byField == FindStorageLocationBy.NAME ? "name" : "no"]?.ToString() == value
                                         select s).ToArray();
            if (foundStorageLocations.Length != 1) goto FAILED;
            model[row, storageLocationFieldName + "Id"] = (int)foundStorageLocations[0]["id"];
            if (byField == FindStorageLocationBy.NAME)
            {
                model[row, storageLocationFieldName + "No"] = foundStorageLocations[0]["no"];
            }
            else
            {
                model[row, storageLocationFieldName + "Name"] = foundStorageLocations[0]["name"];
            }
            model.UpdateCellState(row, storageLocationFieldName + "Name", new ModelCellState(new ValidationState(ValidationStateType.OK)));
            model.UpdateCellState(row, storageLocationFieldName + "No", new ModelCellState(new ValidationState(ValidationStateType.OK)));
            return;

            FAILED:
            model.UpdateCellState(row, storageLocationFieldName + "Name", new ModelCellState(new ValidationState(ValidationStateType.WARNING, $"库位\"{value}\"不存在！")));
            model.UpdateCellState(row, storageLocationFieldName + "No", new ModelCellState(new ValidationState(ValidationStateType.WARNING, $"库位\"{value}\"不存在！")));
            return;
        }

        private void FindSupplyByMaterialAndSupplier(IModel model, int row)
        {
            model[row, "supplyId"] = 0; //先清除供货ID
            int supplierId = (int?)model[row, "supplierId"] ?? 0;
            int materialId = (int?)model[row, "materialId"] ?? 0;
            if (supplierId == 0 || materialId == 0) return;
            var foundSupplies = (from s in GlobalData.AllSupplies
                                 where (int)s["supplierId"] == supplierId
                                 && (int)s["materialId"] == materialId
                                 select s).ToArray();
            //如果找到供货信息，则把供货设置的默认入库信息拷贝到相应字段上
            if (foundSupplies.Length == 1)
            {
                this.FillSupplyFields(model, row, foundSupplies[0]);
                model.RefreshView(row);
            }
        }

        //物料名称输入联想
        private object[] MaterialNameAssociation([Model] IModel model, [Data] string str)
        {

            string materialNo = model[model.SelectionRange.Row, "materialNo"]?.ToString() ?? "";
            int[] selectedIDs = model.GetSelectedRows<int>("supplierId").Except(new int[] { 0 }).ToArray();
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
        private object[] MaterialNoAssociation([Model] IModel model, [Data] string str)
        {
            string materialName = model[model.SelectionRange.Row, "materialName"]?.ToString() ?? "";

            int[] selectedIDs = model.GetSelectedRows<int>("supplierId").Except(new int[] { 0 }).ToArray();
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
        private object[] MaterialProductLineAssociation([Model] IModel model, [Data] string str)
        {
            int[] selectedIDs = model.GetSelectedRows<int>("supplierId").Except(new int[] { 0 }).ToArray();
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

        private void SupplierNoEditEnded([Model] IModel model, [Row] int row)
        {
            if (string.IsNullOrWhiteSpace(model[row, "supplierNo"]?.ToString())) return;
            model[row, "supplierName"] = "";
            this.FindSupplierID(model, row);
            this.FindSupplyByMaterialAndSupplier(model, row);
        }

        private void SupplierNameEditEnded([Model] IModel model, [Row] int row)
        {
            if (string.IsNullOrWhiteSpace(model[row, "supplierName"]?.ToString())) return;
            model[row, "supplierNo"] = "";
            this.FindSupplierID(model, row);
            this.FindSupplyByMaterialAndSupplier(model, row);
        }

        private void TryFindSupplyByMaterialOnly(IModel model, int row)
        {
            model[row, "supplyId"] = 0; //先清除供货ID
            int materialId = (int?)model[row, "materialId"] ?? 0;
            if (materialId == 0) return;
            var foundSupplies = (from s in GlobalData.AllSupplies
                                 where (int)s["materialId"] == materialId
                                 select s).ToArray();
            //如果找到供货信息，则把供货设置的默认入库信息拷贝到相应字段上
            if (foundSupplies.Length == 1)
            {
                this.FillSupplyFields(model, row, foundSupplies[0]);
                model.RefreshView(row);
            }
        }

        private void MaterialNoEditEnded([Model] IModel model, [Row] int row)
        {
            if (string.IsNullOrWhiteSpace(model[row, "materialNo"]?.ToString())) return;
            //this.model[row, "materialName"] = "";
            this.FindMaterialID(model, row);
            this.FindSupplyByMaterialAndSupplier(model, row);
            if (((int?)model[row, "supplyId"] ?? 0) == 0 && ((int?)model[row, "supplierId"] ?? 0) == 0)
            {
                this.TryFindSupplyByMaterialOnly(model, row);
            }
        }

        private void MaterialNameEditEnded([Model] IModel model, [Row] int row)
        {
            if (string.IsNullOrWhiteSpace(model[row, "materialName"]?.ToString())) return;
            // this.model[row, "materialNo"] = "";
            this.FindMaterialID(model, row);
            this.FindSupplyByMaterialAndSupplier(model, row);
            if (((int?)model[row, "supplyId"] ?? 0) == 0 && ((int?)model[row, "supplierId"] ?? 0) == 0)
            {
                this.TryFindSupplyByMaterialOnly(model, row);
            }
        }

        private void MaterialProductLineEditEnded([Model] IModel model, [Row] int row)
        {
            this.FindMaterialID(model, row);
            this.FindSupplyByMaterialAndSupplier(model, row);
            if (((int?)model[row, "supplyId"] ?? 0) == 0 && ((int?)model[row, "supplierId"] ?? 0) == 0)
            {
                this.TryFindSupplyByMaterialOnly(model, row);
            }
        }

        private void FindMaterialID(IModel model, int row)
        {
            model[row, "materialId"] = 0; //先清除物料ID
            string materialNo = model[row, "materialNo"]?.ToString() ?? "";
            string materialName = model[row, "materialName"]?.ToString() ?? "";
            string materialProductLine = model[row, "materialProductLine"]?.ToString() ?? "";
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
            model[row, "materialId"] = foundMaterials[0]["id"];
            model[row, "materialNo"] = foundMaterials[0]["no"];
            model[row, "materialName"] = foundMaterials[0]["name"];
            model[row, "materialProductLine"] = foundMaterials[0]["productLine"];
            return;

            FAILED:
            if (string.IsNullOrWhiteSpace(materialProductLine)) return;
            MessageBox.Show("物料不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void FindSupplierID(IModel model, int row)
        {
            model[row, "supplierId"] = 0;//先清除供货商ID
            string supplierNo = model[row, "supplierNo"]?.ToString() ?? "";
            string supplierName = model[row, "supplierName"]?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(supplierNo) && string.IsNullOrWhiteSpace(supplierName)) return;

            var foundSuppliers = (from s in GlobalData.AllSuppliers
                                  where (string.IsNullOrWhiteSpace(supplierNo) ? true : (s["no"]?.ToString() ?? "") == supplierNo)
                                  && (string.IsNullOrWhiteSpace(supplierName) ? true : (s["name"]?.ToString() ?? "") == supplierName)
                                  select s).ToArray();
            if (foundSuppliers.Length != 1) goto FAILED;
            int supplierID = (int)foundSuppliers[0]["id"];
            model[row, "supplierId"] = foundSuppliers[0]["id"];
            model[row, "supplierNo"] = foundSuppliers[0]["no"];
            model[row, "supplierName"] = foundSuppliers[0]["name"];
            return;

            FAILED:
            MessageBox.Show("供应商不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontWork;
using System.Web.Script.Serialization;

namespace WMS.UI.FormStock
{
    public partial class FormStockRecord : Form
    {    
        public FormStockRecord()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
            this.model1.Refreshed += this.model_Refreshed;
        }

        private void FormStockRecord_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            //this.synchronizer.FindAPI.SetRequestParameter("$AllOrPlus","plus");
            //this.synchronizer.GetCountAPI.SetRequestParameter("$AllOrPlus", "plus");
  //          { type: "get-count",
  //          url: "{$url}/warehouse/{$accountBook}/stock_record/count/{{conditions:$conditions,orders:$orders}}/{$AllOrPlus}",
  //          method: "GET",
  //          responseBody: "$count"},
		//{ type: "find",
  //          url: "{$url}/warehouse/{$accountBook}/stock_record/find_newest/{{page:$page,pageSize:$pageSize,conditions:$conditions,orders:$orders}}/{$AllOrPlus}",
  //          method: "GET",
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
            this.updateBasicAndReoGridView();
        }
        private void model_Refreshed(object sender, ModelRefreshedEventArgs e)
        {
            this.updateBasicAndReoGridView();
        }


        private void updateBasicAndReoGridView()
        {

            if (this.model1.RowCount == 0)
            {
                this.basicView1.Enabled = false;
                this.reoGridView1.Enabled = false;
            }
            else
            {
                this.basicView1.Enabled = true;
                this.reoGridView1.Enabled = true;
            }
        }

        public int[] getSelectRowIds()
        {
            List<int> selectIds = new List<int>();
            if (this.model1.SelectionRange == null) { return new int[] { }; }
            for (int i = 0; i < this.model1.SelectionRange.Rows; i++)
            {
                selectIds.Add(this.model1.SelectionRange.Row + i);
            }
            return selectIds.ToArray();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            //var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row });
            var rowData = this.model1.GetRows(this.getSelectRowIds());
            FormAddStockRecord form = new FormAddStockRecord(rowData);
            form.SetAddFinishedCallback(() =>
            {
                this.searchView1.Search();
                this.updateBasicAndReoGridView();
            });
            form.Show();
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            this.synchronizer.Save();
        }

        private void buttonReturnSupply_Click(object sender, EventArgs e)
        {
            FormReturnSupply form = new FormReturnSupply();
            form.SetAddFinishedCallback(() =>
            {
                this.searchView1.Search();
                this.updateBasicAndReoGridView();
            });
            form.Show();
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
                MessageBox.Show($"库位编号\"{storageLocationName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "storageLocationId"] = foundStorageLcations["id"];
                this.model1[row, "storageLocationName"] = foundStorageLcations["name"];
            }
        }

        private string AmountForwardMapper([Data]double amount,[Row]int row)
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

        private double AmountBackwardMapper([Data]double amount,[Row] int row)
        {
            var rowDate = this.model1.GetRow(row);
            double unitAmount = (double)rowDate["unitAmount"];
            return amount * unitAmount;
        }

        private string AvailableAmountForwardMapper([Data]double amount,[Row]int row)
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

        private double AvailableAmountBackwardMapper([Data]double amount, [Row]int row)
        {
            var rowDate = this.model1.GetRow(row);
            double unitAmount = (double)rowDate["unitAmount"];
            return  amount * unitAmount;
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
            this.model1[row, "materialName"] = "";
            this.FindMaterialID(row);
            this.TryGetSupplyID(row);
        }

        private void MaterialNameEditEnded([Row]int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "materialName"]?.ToString())) return;
            this.model1[row, "materialNo"] = "";
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
            if (string.IsNullOrWhiteSpace(materialProductLine)) return;
            var foundMaterials = (from m in GlobalData.AllMaterials
                                  where (string.IsNullOrWhiteSpace(materialNo) ? true : (m["no"]?.ToString() ?? "") == materialNo)
                                  && (string.IsNullOrWhiteSpace(materialName) ? true : (m["name"]?.ToString() ?? "") == materialName)
                                  && materialProductLine == (m["productLine"]?.ToString() ?? "")
                                  select m).ToArray();
            if (foundMaterials.Length != 1)
            {
                goto FAILED;
            }
            this.model1[row, "materialId"] = foundMaterials[0]["id"];
            this.model1[row, "materialNo"] = foundMaterials[0]["no"];
            this.model1[row, "materialName"] = foundMaterials[0]["name"];
            return;

            FAILED:
            MessageBox.Show("物料不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            MessageBox.Show("供应商不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("无此供货！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void FillDefaultValue(int row, string fieldName, object value)
        {
            this.model1[row, fieldName] = value;
        }

        //=============天经地义的交互逻辑到这里结束===============
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
            }
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

        private void buttonItems_Click(object sender, EventArgs e)
        {
            this.synchronizer.FindAPI.SetRequestParameter("$AllOrPlus","plus");
            this.synchronizer.GetCountAPI.SetRequestParameter("$AllOrPlus","plus");
            this.searchView1.Search();
            this.toolStripButtonAllItem.Visible = true;
            this.buttonNotZero.Visible = false;
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            this.synchronizer.FindAPI.SetRequestParameter("$AllOrPlus", "all");
            this.synchronizer.GetCountAPI.SetRequestParameter("$AllOrPlus", "all");
            this.searchView1.Search();
            this.toolStripButtonAllItem.Visible = false;
            this.buttonNotZero.Visible = true;
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
           
        }

        private void buttonPreview_Click_1(object sender, EventArgs e)
        {
            try
            {
                Condition condWarehouse = new Condition().AddCondition("warehouseId", GlobalData.Warehouse["id"]);
                var previewData = RestClient.Get<object[]>(Defines.ServerURL + "/warehouse/WMS_Template/stock_record/find_newest/"+condWarehouse.ToString());
                if (previewData == null) return;
                StandardFormPreviewExcel formPreviewExcel = new StandardFormPreviewExcel("盘点单预览");
                   string no = "库存";
                   if (!formPreviewExcel.AddPatternTable("Excel/StockRecord.xlsx", no)) return;
                    //formPreviewExcel.AddData("StockInfoCheckTicket", null, no);
                    formPreviewExcel.AddData("StockRecord", previewData, no);
                    formPreviewExcel.Show();
            }
            catch
            {
                MessageBox.Show("无任何数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }
    }
}

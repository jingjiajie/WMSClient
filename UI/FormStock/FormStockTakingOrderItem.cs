using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontWork;

namespace WMS.UI.FormStockTaking
{
    public partial class FormStockTakingOrderItem : Form
    {
        private IDictionary<string, object> stockTakingOrder = null;
        private Action addFinishedCallback = null;
        private double amountTemp;
        private double availableAmountTemp;
        public FormStockTakingOrderItem(IDictionary<string, object> srockTakingOrder)
        {
            MethodListenerContainer.Register(this);            
            this.stockTakingOrder = srockTakingOrder;
            InitializeComponent();         
            this.searchView1.AddStaticCondition("stockTakingOrderId", this.stockTakingOrder["id"]);
        }

      

        private void FormStockTakingOrderItem_Load(object sender, EventArgs e)
        {                       
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private int StockTakingOrderIDDefaultValue()
        {
            return (int)this.stockTakingOrder["id"];
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {               
                { "createPersonId",GlobalData.Person["id"]},             
                { "createPersonName",GlobalData.Person["name"]}
            });
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
             if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            if (this.model1.SelectionRange.Rows == 0) {
                MessageBox.Show("请选择要删除的行", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                return;
            }
            int startRow = this.model1.SelectionRange.Row;
            int selectRows = this.model1.SelectionRange.Rows;
            int[] rows= Enumerable.Range(startRow, selectRows).ToArray();           
            StringBuilder ids = new StringBuilder();
            var rowData = this.model1.GetRows(rows);
            foreach (Dictionary<string, object> a in rowData)
            {                
                ids.Append((int)a["id"]);
                ids.Append(",");
            }
            ids.Remove(ids.Length-1, 1);
            try
            {
                string body = "{\"deleteIds\":[" + ids+ "],\"personId\":\"" + GlobalData.Person["id"] + "\"}";
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/stocktaking_order_item/remove";
                //MessageBox.Show(body);
                //MessageBox.Show(url);
                RestClient.Post<List<IDictionary<string, object>>>(url, body);               
                this.searchView1.Search();            
            }
            catch
            {
                MessageBox.Show("删除失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }         
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
            }
        }

        private void StorageLocationNameEditEnded(int row, string storageAreaName)
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

        private void StorageLocationNoEditEnded(int row, string storageLocationName)
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



        private string AmountForwardMapper(double amount, int row)
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




        private string RealAmountForwardMapper(double amount, int row)
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
        




        //===========为了实现一个看起来天经地义的交互逻辑=========

        private void SupplierNoEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "supplierNo"]?.ToString())) return;
            this.model1[row, "supplierName"] = "";
            this.FindSupplierID(row);
            this.TryGetSupplyID(row);
        }

        private void SupplierNameEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "supplierName"]?.ToString())) return;
            this.model1[row, "supplierNo"] = "";
            this.FindSupplierID(row);
            this.TryGetSupplyID(row);
        }

        private void MaterialNoEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "materialNo"]?.ToString())) return;
            this.model1[row, "materialName"] = "";
            this.FindMaterialID(row);
            this.TryGetSupplyID(row);
        }

        private void MaterialNameEditEnded(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "materialName"]?.ToString())) return;
            this.model1[row, "materialNo"] = "";
            this.FindMaterialID(row);
            this.TryGetSupplyID(row);
        }

        private void MaterialProductLineEditEnded(int row)
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
            try
            {
                string body = "{\"stockTakingOrderId\":\"" + this.stockTakingOrder["id"] + "\",\"warehouseId\":\"" + GlobalData.Warehouse["id"] + "\",\"personId\":\"" + GlobalData.Person["id"] + "\"}";
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/stocktaking_order_item/add_all";
                RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);
                MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.searchView1.Search();           

            }
            catch
            {
                MessageBox.Show("添加失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            this.model1.CurrentModelName = "addSingle";
            this.model1.Mode = "addSingle";
            this.synchronizer.Mode = "addSingle";
            this.reoGridView1.Mode = "addSingle";
            this.basicView1.Mode = "addSingle";
            this.searchView1.Enabled = false;
            this.toolStripButton1.Enabled = false;
            this.toolStripButton2.Visible =false;
            this.pagerView1.Enabled = false;
            this.toolStripButtonAdd.Enabled = false;
            this.toolStripButtonDelete.Enabled = false;
            this.toolStripButtonAlter.Enabled = false;
            this.ButtonCancel.Visible = true;
            this.buttonStartAdd.Visible = true;
            this.model1.InsertRows(new int[] { 0,1,2,3,4 }, null);
        }

        private void SupplierNoEditEnded1(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "supplierNo"]?.ToString())) return;
            this.model1[row, "supplierName"] = "";
            this.FindSupplierID(row);
            this.TryGetSupplyID(row);
            //this.TryAddItem(row);
        }

        private void SupplierNameEditEnded1(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "supplierName"]?.ToString())) return;
            this.model1[row, "supplierNo"] = "";
            this.FindSupplierID(row);
            this.TryGetSupplyID(row);
            //this.TryAddItem(row);
        }

        private void MaterialNoEditEnded1(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "materialNo"]?.ToString())) return;
            this.model1[row, "materialName"] = "";
            this.FindMaterialID(row);
            this.TryGetSupplyID(row);
           // this.TryAddItem(row);
        }

        private void MaterialNameEditEnded1(int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "materialName"]?.ToString())) return;
            this.model1[row, "materialNo"] = "";
            this.FindMaterialID(row);
            this.TryGetSupplyID(row);
            //this.TryAddItem(row);
        }

        private void MaterialProductLineEditEnded1(int row)
        {
            this.FindMaterialID(row);
            this.TryGetSupplyID(row);
            //this.TryAddItem(row);
        }


       

        public void SetAddFinishedCallback(Action callback)
        {
            this.addFinishedCallback = callback;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.model1.CurrentModelName = "default";
            this.ButtonCancel.Visible = false;
            this.buttonStartAdd.Visible = false;
            this.model1.Mode = "default";
            this.synchronizer.Mode = "default";
            this.reoGridView1.Mode = "default";
            this.basicView1.Mode = "default";
            this.searchView1.Enabled = true;
            this.toolStripButton1.Enabled = true;
            this.toolStripButton2.Visible = true;
            this.toolStripButtonAdd.Enabled = true;
            this.toolStripButtonDelete.Enabled = true;
            this.toolStripButtonAlter.Enabled = true;
            this.pagerView1.Enabled = true;
            this.searchView1.Search();
        }

        private void buttonStartAdd_Click(object sender, EventArgs e)
        {
            List<int> supplyId = new List<int>();
            for(int i = 0; i < this.model1.RowCount - 1; i++)
            {

                if (this.model1.GetRowSynchronizationState(i) == SynchronizationState.ADDED_UPDATED && (int)this.model1[i, "supplyId"]!=0)
                {                   
                    supplyId.Add((int)this.model1[i, "supplyId"]);
                }
            }          
            if (supplyId.Count== 0) return;
            //首先得到supplyid
            int[] supplyIds = supplyId.ToArray();
            //supplyIds = new int[] { 15 };
            StringBuilder ids = new StringBuilder();
            ids.Append("[");
            foreach (int a in supplyIds)
            {
                ids.Append(a);
                ids.Append(",");
            }
            ids.Remove(ids.Length - 1, 1);
            ids.Append("]");
            this.TryAddItem(ids.ToString());       
        }

        private void TryAddItem(string ids)
        {
            
            string body = "{\"stockTakingOrderId\":\"" + this.stockTakingOrder["id"] + "\",\"warehouseId\":\"" + GlobalData.Warehouse["id"] + "\",\"personId\":\"" + GlobalData.Person["id"] + "\"}";
            string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/stocktaking_order_item/add_single/"+ids;
            try
            {
                RestClient.RequestPost<int[]>(url, body);                
                this.model1.CurrentModelName = "default";
                this.model1.Mode = "default";
                this.synchronizer.Mode = "default";
                this.reoGridView1.Mode = "default";
                this.basicView1.Mode = "default";
                this.searchView1.Enabled = true;
                this.toolStripButton1.Enabled = true;
                this.toolStripButton2.Visible = true;
                this.toolStripButtonAdd.Enabled = true;
                this.toolStripButtonDelete.Enabled = true;
                this.toolStripButtonAlter.Enabled = true;
                this.pagerView1.Enabled = true;
                this.ButtonCancel.Visible = false;
                this.buttonStartAdd.Visible = false;
                this.searchView1.Search();         
                MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("添加失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.model1.Mode = "default";
                this.model1.CurrentModelName = "default";
                this.synchronizer.Mode = "default";
                this.reoGridView1.Mode = "default";
                this.basicView1.Mode = "default";
                this.searchView1.Enabled = true;
                this.toolStripButton1.Enabled = true;
                this.toolStripButton2.Visible = true;
                this.toolStripButtonAdd.Enabled = true;
                this.toolStripButtonDelete.Enabled = true;
                this.toolStripButtonAlter.Enabled = true;
                this.pagerView1.Enabled = true;
                this.ButtonCancel.Visible = false;
                this.buttonStartAdd.Visible = false;
                this.searchView1.Search();
            }
        }

        private void FormStockTakingOrderItem_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.addFinishedCallback != null)
            {
                this.addFinishedCallback();
            }
        }
    }
}

    


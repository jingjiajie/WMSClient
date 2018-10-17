using FrontWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
using Microsoft.VisualBasic;

namespace WMS.UI.FormBasicInfos
{
    public partial class FormSupply : Form
    {
        public FormSupply()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
            this.model1.CellUpdated += this.model_CellUpdated;
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
        private void FormSupply_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            this.searchView1.Search();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            //Interaction.InputBox();
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
            //this.model1.InsertRow(0, new Dictionary<string, object>()
            //{
            //    { "warehouseId",GlobalData.Warehouse["id"]},
            //    { "createPersonId",GlobalData.Person["id"]},
            //    { "createPersonName",GlobalData.Person["name"]},
            //    { "warehouseName",GlobalData.Warehouse["name"]},
            //    { "createTime",DateTime.Now},
            //    { "enabled",1}
            //});
        }

        private int WarehouseIdDefaultValue()
        {
            return (int)GlobalData.Warehouse["id"];
        }

        private int createPersonIdDefaultValue()
        {
            return (int)GlobalData.Person["id"];
        }

        private string createPersonNameDefaultValue()
        {
            return (string)GlobalData.Person["name"];
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
                this.flesh();
            }
        }
        private void flesh()
        {
            this.searchView1.Search();
            Condition condWarehouse = new Condition().AddCondition("warehouseId", GlobalData.Warehouse["id"]);
            GlobalData.AllSupplies = RestClient.Get<List<IDictionary<string, object>>>(
               $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/supply/{condWarehouse.ToString()}");
        }

        //private void MaterialNameEditEnded(int row, string materialName)
        //{
        //    IDictionary<string, object> foundMaterial =
        //        GlobalData.AllMaterials.Find((s) =>
        //        {
        //            if (s["name"] == null) return false;
        //            return s["name"].ToString() == materialName;
        //        });
        //    if (foundMaterial == null)
        //    {
        //        MessageBox.Show($"物料\"{materialName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //    else
        //    {
        //        this.model1[row, "materialId"] = foundMaterial["id"];
        //        this.model1[row, "materialName"] = foundMaterial["name"];
        //    }
        //}

        private void MaterialNoEditEnded([Row]int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "materialNo"]?.ToString())) return;
            this.FindMaterialID(row);
        }

        private void MaterialNameEditEnded([Row]int row)
        {
            if (string.IsNullOrWhiteSpace(this.model1[row, "materialName"]?.ToString())) return;
            this.FindMaterialID(row);
        }

        private void MaterialProductLineEditEnded([Row]int row)
        {
            this.FindMaterialID(row);
        }
        private void FindMaterialID([Row]int row)
        {
            this.model1[row, "materialId"] = 0; //先清除物料ID
            string materialNo = this.model1[row, "materialNo"]?.ToString() ?? "";
            string materialName = this.model1[row, "materialName"]?.ToString() ?? "";
            string materialProductLine = this.model1[row, "materialProductLine"]?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(materialNo) && string.IsNullOrWhiteSpace(materialName)) return;
            var foundMaterials = (from m in GlobalData.AllMaterials
                                  where (string.IsNullOrWhiteSpace(materialNo) ? true : (m["no"]?.ToString() ?? "") == materialNo)
                                  && (string.IsNullOrWhiteSpace(materialName) ? true : (m["name"]?.ToString() ?? "") == materialName)
                                     && materialProductLine == (m["productLine"]?.ToString() ?? "")
                                     && m["warehouseId"] != GlobalData.Warehouse["id"]
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
            MessageBox.Show($"物料\"{materialName}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        //供应商名称编辑完成，根据名称自动搜索ID和No
        private void SupplierNameEditEnded([Row]int row, [Data] string supplierName)
        {
            IDictionary<string, object> foundSupplier =
                GlobalData.AllSuppliers.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == supplierName && s["warehouseId"] != GlobalData.Warehouse["id"];
                });
            if (foundSupplier == null)
            {
                MessageBox.Show($"供应商\"{supplierName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "supplierId"] = foundSupplier["id"];
            }
        }

        private void toolStripButtonSupplySingleBoxTranPackingInfo_Click(object sender, EventArgs e)
        {
            this.model1.Mode = "default1";
            this.basicView1.Mode = "default1";
            this.reoGridView1.Mode = "default1";
            this.synchronizer.Mode = "default1";
            this.toolStripButton1.Visible = true;
            //this.searchView1.Search();
        }

        private void toolStripButtonSupplyOuterPackingSize_Click(object sender, EventArgs e)
        {
            this.model1.Mode = "default2";
            this.basicView1.Mode = "default2";
            this.reoGridView1.Mode = "default2";
            this.synchronizer.Mode = "default2";
            this.toolStripButton1.Visible = true;
            //this.searchView1.Search();
        }

        private void toolStripButtonSupplyShipmentInfo_Click(object sender, EventArgs e)
        {
            this.model1.Mode = "default3";
            this.basicView1.Mode = "default3";
            this.reoGridView1.Mode = "default3";
            this.synchronizer.Mode = "default3";
            this.toolStripButton1.Visible = true;
            //this.searchView1.Search();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.model1.Mode = "default";
            this.basicView1.Mode = "default";
            this.reoGridView1.Mode = "default";
            this.synchronizer.Mode = "default";
            this.toolStripButton1.Visible = false;
            //this.searchView1.Search();
        }

        private string EntryAmountForwardMapper(double amount, [Row]int row)
        {
            double? unitAmount = (double?)this.model1[row, "defaultEntryUnitAmount"];
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                return amount.ToString();
            }
            else
            {
                return Utilities.DoubleToString(amount / unitAmount.Value);
            }
        }

        private double EntryAmountBackwardMapper([Data]string strAmount, [Row] int row)
        {
            if (!Double.TryParse(strAmount, out double amount))
            {
                MessageBox.Show($"\"{strAmount}\"不是合法的数字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
            double? unitAmount = (double?)this.model1[row, "defaultEntryUnitAmount"];
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                return amount;
            }
            else
            {
                return amount * unitAmount.Value;
            }
        }

        private void EntryUnitAmountEditEnded([Row]int row)
        {
            this.model1.RefreshView(row);
        }

        private string InspectionAmountForwardMapper([Data]double amount, [Row] int row)
        {
            double? unitAmount = (double?)this.model1[row, "defaultInspectionUnitAmount"];
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                return amount.ToString();
            }
            else
            {
                return Utilities.DoubleToString(amount / unitAmount.Value);
            }
        }

        private double InspectionAmountBackwardMapper([Data]string strAmount, [Row]int row)
        {
            if (!Double.TryParse(strAmount, out double amount))
            {
                MessageBox.Show($"\"{strAmount}\"不是合法的数字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
            double? unitAmount = (double?)this.model1[row, "defaultInspectionUnitAmount"];
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                return amount;
            }
            else
            {
                return amount * unitAmount.Value;
            }
        }

        private void InspectionUnitAmountEditEnded([Row]int row)
        {
            this.model1.RefreshView(row);
        }

        private string DeliveryAmountForwardMapper([Data]double amount, [Row] int row)
        {
            double? unitAmount = (double?)this.model1[row, "defaultDeliveryUnitAmount"];
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                return amount.ToString();
            }
            else
            {
                return Utilities.DoubleToString(amount / unitAmount.Value);
            }
        }

        private double DeliveryAmountBackwardMapper([Data]string strAmount, [Row]int row)
        {
            if (!Double.TryParse(strAmount, out double amount))
            {
                MessageBox.Show($"\"{strAmount}\"不是合法的数字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
            double? unitAmount = (double?)this.model1[row, "defaultDeliveryUnitAmount"];
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                return amount;
            }
            else
            {
                return amount * unitAmount.Value;
            }
        }

        private void DeliveryUnitAmountEditEnded([Row]int row)
        {
            this.model1.RefreshView(row);
        }

        private void toolStripButtonSaveFileDialog_Click(object sender, EventArgs e)
        {
            string localFilePath = ShowSaveFileDialog();

            if (localFilePath == null) return;

            //获取选中行ID，过滤掉新建的行（ID为0的）
            int[] selectedIDs = this.model1.GetSelectedRows<int>("id").Except(new int[] { 0 }).ToArray();
            if (selectedIDs.Length == 0)
            {
                MessageBox.Show("请选择一项进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string[] rowData = this.model1.GetSelectedRows<string>("barCodeNo").ToArray();
            for (int i = 0; i < this.model1.SelectionRange.Rows; i++)
            {


                string barCodeNo = (string)rowData[i];
                if (!string.IsNullOrEmpty(barCodeNo))
                {
                    BarcodeWriter writer = new BarcodeWriter();
                    //设置条码格式
                    //使用ITF 格式，不能被现在常用的支付宝、微信扫出来
                    //如果想生成可识别的可以使用 CODE_128 格式
                    writer.Format = BarcodeFormat.CODE_39;

                    //设定条码的一些设置
                    EncodingOptions options = new EncodingOptions();
                    options.Width = 382;
                    options.Height = 115;
                    options.Margin = 2;
                    writer.Options = options;

                    Bitmap bmp = writer.Write(barCodeNo);


                    bmp.Save(localFilePath + "\\" + barCodeNo + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                
            }
            MessageBox.Show("条码导出操作完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        //选择保存路径
        private string ShowSaveFileDialog()
        {
            //string localFilePath = "";
            ////string localFilePath, fileNameExt, newFileName, FilePath; 
            //SaveFileDialog sfd = new SaveFileDialog();

            //sfd.Title = "保存文件";
            ////设置文件类型 
            //sfd.Filter = "jpg图片文件（*.jpg）|*.jpg";

            ////设置默认文件类型显示顺序 
            //sfd.FilterIndex = 1;

            ////保存对话框是否记忆上次打开的目录 
            //sfd.RestoreDirectory = true;

            ////点了保存按钮进入 
            //if (sfd.ShowDialog() == DialogResult.OK)
            //{
            //    localFilePath = sfd.FileName.ToString(); //获得文件路径 
            //}
            //return localFilePath;

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择条形码导出文件夹";
            dialog.ShowNewFolderButton = true;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
            }
            return dialog.SelectedPath;
        }
    }


    [MethodListener]
    public class FormSupplyMethodListener
    {
        //private void defaultEntryStorageLocationNoEditEnded([Model] IModel model, [Row] int row, [Data] string storageLocationNo)
        //{
        //    this.FindStorageLocation(model, row, "defaultEntryStorageLocation", FindStorageLocationBy.NO, storageLocationNo);

        //}
        //private void defaultInspectionStorageLocationNoEditEnded([Model] IModel model, [Row] int row, [Data] string storageLocationNo)
        //{
        //    this.FindStorageLocation(model, row, "defaultInspectionStorageLocation", FindStorageLocationBy.NO, storageLocationNo);

        //}
        //private void defaultQualifiedStorageLocationNoEditEnded([Model] IModel model, [Row] int row, [Data] string storageLocationNo)
        //{
        //    this.FindStorageLocation(model, row, "defaultQualifiedStorageLocation", FindStorageLocationBy.NO, storageLocationNo);

        //}
        //private void defaultUnqualifiedStorageLocationNoEditEnded([Model] IModel model, [Row] int row, [Data] string storageLocationNo)
        //{
        //    this.FindStorageLocation(model, row, "defaultUnqualifiedStorageLocation", FindStorageLocationBy.NO, storageLocationNo);

        //}
        //private void defaultDeliveryStorageLocationNoEditEnded([Model] IModel model, [Row] int row, [Data] string storageLocationNo)
        //{
        //    this.FindStorageLocation(model, row, "defaultDeliveryStorageLocation", FindStorageLocationBy.NO, storageLocationNo);

        //}
        //private void defaultPrepareTargetStorageLocationNoEditEnded([Model] IModel model, [Row] int row, [Data] string storageLocationNo)
        //{
        //    this.FindStorageLocation(model, row, "defaultPrepareTargetStorageLocation", FindStorageLocationBy.NO, storageLocationNo);

        //}

        //enum FindStorageLocationBy
        //{
        //    NAME, NO
        //}

        //private void FindStorageLocation(IModel model, int row, string storageLocationFieldName, FindStorageLocationBy byField, string value, bool warning = true)
        //{
        //    model[row, storageLocationFieldName + "Id"] = 0;//先清除库位ID

        //    var foundStorageLocations = (from s in GlobalData.AllStorageLocations
        //                                 where s[byField == FindStorageLocationBy.NAME ? "name" : "no"]?.ToString() == value
        //                                 select s).ToArray();
        //    if (foundStorageLocations.Length != 1) goto FAILED;
        //    model[row, storageLocationFieldName + "Id"] = (int)foundStorageLocations[0]["id"];
        //    if (byField == FindStorageLocationBy.NAME)
        //    {
        //        model[row, storageLocationFieldName + "No"] = foundStorageLocations[0]["no"];
        //    }
        //    else
        //    {
        //        model[row, storageLocationFieldName + "Name"] = foundStorageLocations[0]["name"];
        //    }
        //    model.UpdateCellState(row, storageLocationFieldName + "Name", new ModelCellState(new ValidationState(ValidationStateType.OK)));
        //    model.UpdateCellState(row, storageLocationFieldName + "No", new ModelCellState(new ValidationState(ValidationStateType.OK)));
        //    return;

        //    FAILED:
        //    model.UpdateCellState(row, storageLocationFieldName + "Name", new ModelCellState(new ValidationState(ValidationStateType.WARNING, $"库位\"{value}\"不存在！")));
        //    model.UpdateCellState(row, storageLocationFieldName + "No", new ModelCellState(new ValidationState(ValidationStateType.WARNING, $"库位\"{value}\"不存在！")));
        //    return;
        //}

        //物料名称输入联想
        private object[] MaterialNameAssociation([Model] IModel model, [Data] string str)
        {
            string materialNo = model[model.SelectionRange.Row, "materialNo"]?.ToString() ?? "";
            var a = (from s in GlobalData.AllMaterials
                     where s["name"] != null &&
                     s["name"].ToString().StartsWith(str)
                     && s["warehouseId"] != GlobalData.Warehouse["id"]
                     && (string.IsNullOrWhiteSpace(materialNo) ? true : (s["no"]?.ToString() ?? "") == materialNo)

                     select s["name"]).ToArray();
            return a.GroupBy(p => p).Select(p => p.Key).ToArray();
        }

        //物料代号输入联想
        private object[] MaterialNoAssociation([Model] IModel model, [Data] string str)
        {
            string materialName = model[model.SelectionRange.Row, "materialName"]?.ToString() ?? "";
            var a = (from s in GlobalData.AllMaterials
                     where s["no"] != null &&
                     s["no"].ToString().StartsWith(str)
                     && s["warehouseId"] != GlobalData.Warehouse["id"]
                      && (string.IsNullOrWhiteSpace(materialName) ? true : (s["name"]?.ToString() ?? "") == materialName)
                     select s["no"]).ToArray();
            return a.GroupBy(p => p).Select(p => p.Key).ToArray();
        }

        //物料系列输入联想
        private object[] MaterialProductLineAssociation([Model] IModel model, [Data] string str)
        {
            var a = (from s in GlobalData.AllMaterials
                     where s["productLine"] != null &&
                     s["productLine"].ToString().StartsWith(str)
                     && s["warehouseId"] != GlobalData.Warehouse["id"]
                     select s["productLine"]).ToArray();
            return a.GroupBy(p => p).Select(p => p.Key).ToArray();
        }

        private void SupplierNoEditEnded([Model] IModel model, [Row] int row)
        {
            if (string.IsNullOrWhiteSpace(model[row, "supplierNo"]?.ToString())) return;
            model[row, "supplierName"] = "";
            this.FindSupplierID(model, row);
        }

        private void SupplierNameEditEnded([Model] IModel model, [Row] int row)
        {
            if (string.IsNullOrWhiteSpace(model[row, "supplierName"]?.ToString())) return;
            model[row, "supplierNo"] = "";
            this.FindSupplierID(model, row);
        }

        private void MaterialNoEditEnded([Model] IModel model, [Row] int row)
        {
            if (string.IsNullOrWhiteSpace(model[row, "materialNo"]?.ToString())) return;
            //this.model[row, "materialName"] = "";
            this.FindMaterialID(model, row);
        }

        private void MaterialNameEditEnded([Model] IModel model, [Row] int row)
        {
            if (string.IsNullOrWhiteSpace(model[row, "materialName"]?.ToString())) return;
            // this.model[row, "materialNo"] = "";
            this.FindMaterialID(model, row);
        }

        private void MaterialProductLineEditEnded([Model] IModel model, [Row] int row)
        {
            this.FindMaterialID(model, row);
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

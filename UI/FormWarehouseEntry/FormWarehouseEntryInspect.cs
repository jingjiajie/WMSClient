using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using FrontWork;

namespace WMS.UI
{
    public partial class FormWarehouseEntryInspect : Form
    {
        private IDictionary<string, object>[] warehouseEntries = null;
        Action<int[]> ToInspectionNoteCallback = null;
        Action RefreshWarehouseEntryCallback = null;

        public FormWarehouseEntryInspect(IDictionary<String, object>[] warehouseEntries,Action<int[]> toInspectionNoteCallback,Action refreshWarehouseEntryCallback)
        {
            MethodListenerContainer.Register(this);
            this.warehouseEntries = warehouseEntries;
            this.RefreshWarehouseEntryCallback = refreshWarehouseEntryCallback;
            this.ToInspectionNoteCallback = toInspectionNoteCallback;
            InitializeComponent();
        }

        private void ModelBoxInspectionNoteItems_SelectedModelChangedEvent(object sender, SelectedModelChangedEventArgs e)
        {
            this.ModelInspectionNoteSelect(this.modelBoxInspectionNoteItems.CurrentModelName);
        }

        private void StorageLocationNoEditEnded(int row, string storageLocationNo)
        {
            this.modelBoxInspectionNoteItems[row, "inspectionStorageLocationId"] = 0;//先清除库位ID
            if (string.IsNullOrWhiteSpace(storageLocationNo)) return;
            var foundinspectionStorageLocations = (from s in GlobalData.AllStorageLocations
                                         where s["no"]?.ToString() == storageLocationNo
                                                   select s).ToArray();
            if (foundinspectionStorageLocations.Length != 1) goto FAILED;
            this.modelBoxInspectionNoteItems[row, "inspectionStorageLocationId"] = (int)foundinspectionStorageLocations[0]["id"];
            this.modelBoxInspectionNoteItems[row, "inspectionStorageLocationName"] = foundinspectionStorageLocations[0]["name"];
            return;

            FAILED:
            MessageBox.Show($"库位\"{storageLocationNo}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void StorageLocationNameEditEnded(int row, string inspectionStorageLocationName)
        {
            this.modelBoxInspectionNoteItems[row, "inspectionStorageLocationId"] = 0;//先清除库位ID
            if (string.IsNullOrWhiteSpace(inspectionStorageLocationName)) return;
            var foundinspectionStorageLocations = (from s in GlobalData.AllStorageLocations
                                         where s["name"]?.ToString() == inspectionStorageLocationName
                                         select s).ToArray();
            if (foundinspectionStorageLocations.Length != 1) goto FAILED;
            this.modelBoxInspectionNoteItems[row, "inspectionStorageLocationId"] = (int)foundinspectionStorageLocations[0]["id"];
            this.modelBoxInspectionNoteItems[row, "inspectionStorageLocationNo"] = foundinspectionStorageLocations[0]["no"];
            return;

            FAILED:
            MessageBox.Show($"库位\"{inspectionStorageLocationName}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private string AmountForwardMapper(double amount, int row)
        {
            double? unitAmount = (double?)this.modelBoxInspectionNoteItems[row, "unitAmount"];
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                return amount.ToString();
            }
            else
            {
                return Utilities.DoubleToString(amount / unitAmount.Value);
            }
        }

        private double AmountBackwardMapper(string strAmount, int row)
        {
            if (!Double.TryParse(strAmount, out double amount))
            {
                MessageBox.Show($"\"{strAmount}\"不是合法的数字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
            double? unitAmount = (double?)this.modelBoxInspectionNoteItems[row, "unitAmount"];
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                return amount;
            }
            else
            {
                return amount * unitAmount.Value;
            }
        }

        private void UnitAmountEditEnded(int row)
        {
            this.modelBoxInspectionNoteItems.RefreshView(row);
        }

        private void FormWarehouseEntryInspect_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);

            Condition condition = new Condition()
    .AddCondition("warehouseEntryId", warehouseEntries.Select((item) => item["id"]).ToArray(), ConditionItemRelation.IN);
            this.synchronizer.SetRequestParameter("$condStr", condition.ToString());
            if (this.synchronizer.Find() == false)
            {
                MessageBox.Show("加载物料条目失败，请检查网络连接！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.modelBoxInspectionNoteItems.GroupBy("warehouseEntryNo");
            if (this.modelBoxInspectionNoteItems.CurrentModelName == null)
            {
                MessageBox.Show("选择的入库单据为空，请先添加物料条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }
            foreach (Model model in this.modelBoxInspectionNoteItems.Models)
            {
                string warehouseEntryNo = model.Name;
                int warehouseEntryID = (int)(this.warehouseEntries.Where((item) => ((string)item["no"]) == warehouseEntryNo).First()["id"]);
                this.modelInspectionNotes.AddRow(new Dictionary<string, object>()
                {
                    { "warehouseEntryNo",warehouseEntryNo },
                    { "warehouseEntryId",warehouseEntryID }
                });
            }
            this.ModelInspectionNoteSelect(this.modelBoxInspectionNoteItems.CurrentModelName);
            this.modelBoxInspectionNoteItems.SelectedModelChanged += ModelBoxInspectionNoteItems_SelectedModelChangedEvent;
        }

        private int WarehouseIdDefaultValue()
        {
            return (int)GlobalData.Warehouse["id"];
        }

        private int CreatePersonIdDefaultValue()
        {
            return (int)GlobalData.Person["id"];
        }

        private string CreatePersonNameDefaultValue()
        {
            return (string)GlobalData.Person["name"];
        }

        private DateTime? InspectionTimeDefaultValue()
        {
            string selectedWarehouseEntryNo = (string)this.modelBoxInspectionNoteItems.CurrentModelName;
            var warehouseEntry = this.warehouseEntries.Where((item) => (string)item["no"] == selectedWarehouseEntryNo).First();
            DateTime? createTime = warehouseEntry["createTime"] as DateTime?;
            return createTime;
        }

        /// <summary>
        /// 如果包含入库单对应送检单，则选中行，否则新建行
        /// </summary>
        /// <param name="warehouseEntryID">入库单ID</param>
        private void ModelInspectionNoteSelect(string warehouseEntryNo)
        {
            var dataRows = this.modelInspectionNotes.GetRows(Util.Range(0, this.modelInspectionNotes.GetRowCount()));
            for (int row = 0; row < dataRows.Length; row++)
            {
                string curNo = (string)dataRows[row]["warehouseEntryNo"];
                if (curNo == warehouseEntryNo)
                {
                    this.modelInspectionNotes.SelectionRange = new Range(row, 0, 1, this.modelInspectionNotes.ColumnCount);
                    return;
                }
            }
            throw new Exception("未找到对应的送检单信息！");
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            InspectArgs inspectArgs = new InspectArgs();
            foreach (Model modelInspectionNoteItems in this.modelBoxInspectionNoteItems.Models)
            {
                string warehouseEntryNo = modelInspectionNoteItems.Name;
                InspectItem inspectItem = new InspectItem();
                inspectArgs.inspectItems.Add(inspectItem);
                for (int i = 0; i < this.modelInspectionNotes.RowCount; i++)
                {
                    if ((string)this.modelInspectionNotes[i, "warehouseEntryNo"] == warehouseEntryNo)
                    {
                        inspectItem.inspectionNote = this.modelInspectionNotes.GetRow<InspectionNote>(i);
                        break;
                    }
                }
                inspectItem.inspectionNoteItems = modelInspectionNoteItems.GetRows<InspectionNoteItem>(Util.Range(0, modelInspectionNoteItems.RowCount));
            }
            
            JsonSerializer serializer = new JsonSerializer();
            string strInspectArgs = serializer.Serialize(inspectArgs);
            try
            {
                int[] inspectionNoteIDs = RestClient.RequestPost<int[]>(Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/warehouse_entry/inspect", strInspectArgs);
                string strIDs = serializer.Serialize(inspectionNoteIDs);
                if (MessageBox.Show("送检成功！是否查看送检单？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.ToInspectionNoteCallback?.Invoke(inspectionNoteIDs);
                    this.Close();
                }
                else
                {
                    this.RefreshWarehouseEntryCallback?.Invoke();
                    this.Close();
                }
            } catch(WebException ex)
            {
                string message = ex.Message;
                if(ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show("送检失败：" + message,"提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void configurationWarehouseEntry_Load(object sender, EventArgs e)
        {

        }
    }


    public class InspectArgs
    {
        public List<InspectItem> inspectItems { get; set; } = new List<InspectItem>();
    }

    public class InspectItem
    {
        public InspectionNote inspectionNote { get; set; }
        public InspectionNoteItem[] inspectionNoteItems { get; set; }
    }

    public class InspectionNote
    {
        public int warehouseEntryId;
        public int warehouseId;
        public string no;
        public string description;
        public DateTime inspectionTime;
        public int createPersonId;
        public DateTime createTime;
    }

    public class InspectionNoteItem
    {
        public int warehouseEntryItemId;
        public double amount;
        public string unit;
        public double unitAmount;
        public int inspectionStorageLocationId;
        public double returnAmount;
        public string returnUnit;
        public double returnUnitAmount;
        public int? returnStorageLocationId;
        public string comment;
        public int? personId;
    }
}

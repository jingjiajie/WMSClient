using FrontWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI
{
    public partial class FormInspectionNoteItem : Form
    {
        const int STATE_NOT_INSPECTED = 0;

        private IDictionary<string, object> inspectionNote = null;
        Action RefreshInspectionNoteCallback = null;
        public FormInspectionNoteItem(IDictionary<string, object>  inspectionNote,Action refreshInspectionNoteCallback)
        {
            MethodListenerContainer.Register(this);
            this.RefreshInspectionNoteCallback = refreshInspectionNoteCallback;
            this.inspectionNote = inspectionNote;
            InitializeComponent();
            this.searchView.AddStaticCondition("inspectionNoteId", this.inspectionNote["id"]);
        }

        private void FormInspectionNoteItem_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            Utilities.BindBlueButton(this.buttonQualified);
            Utilities.BindBlueButton(this.buttonAllPass);
            Utilities.BindBlueButton(this.buttonUnqualified);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView.Search();
        }

        private string AmountForwardMapper(double amount, int row)
        {
            double? unitAmount = (double?)this.model[row, "unitAmount"];
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
            if (row == -1)
            {
                return amount;
            }
            double? unitAmount = (double?)this.model[row, "unitAmount"];
            if (unitAmount.HasValue == false || unitAmount == 0)
            {
                return amount;
            }
            else
            {
                return amount * unitAmount.Value;
            }
        }

        private string ReturnAmountForwardMapper(double amount, int row)
        {
            double? returnUnitAmount = (double?)this.model[row, "returnUnitAmount"];
            if (returnUnitAmount.HasValue == false || returnUnitAmount == 0)
            {
                return amount.ToString();
            }
            else
            {
                return Utilities.DoubleToString(amount / returnUnitAmount.Value);
            }
        }

        private double ReturnAmountBackwardMapper(string strAmount, int row)
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
            double? returnUnitAmount = (double?)this.model[row, "returnUnitAmount"];
            if (returnUnitAmount.HasValue == false || returnUnitAmount == 0)
            {
                return amount;
            }
            else
            {
                return amount * returnUnitAmount.Value;
            }
        }

        private void UnitAmountEditEnded(int row)
        {
            this.model.RefreshView(row);
        }

        private void ReturnUnitAmountEditEnded(int row)
        {
            this.model.RefreshView(row);
        }

        private void PersonEditEnded(int row, string personName)
        {
            this.model[row, "personId"] = 0;//先清除ID
            if (string.IsNullOrWhiteSpace(personName)) return;
            var foundPersons = (from s in GlobalData.AllPersons
                                         where s["name"]?.ToString() == personName
                                         select s).ToArray();
            if (foundPersons.Length != 1) goto FAILED;
            this.model[row, "personId"] = (int)foundPersons[0]["id"];
            this.model[row, "personName"] = foundPersons[0]["name"];
            return;

            FAILED:
            MessageBox.Show($"人员\"{personName}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private string StateForwardMapper(int state)
        {
            switch (state)
            {
                case 0: return "待检";
                case 1: return "全部合格";
                case 2: return "不合格";
                default: return "未知状态";
            }
        }

        private int StateBackwardMapper([Data]string state)
        {
            switch (state)
            {
                case "待检": return 0;
                case "全部合格": return 1;
                case "不合格": return 2;
                default: return -1;
            }
        }

        private void InspectionStorageLocationNoEditEnded(int row, string inspectionStorageLocationNo)
        {
            this.model[row, "inspectionStorageLocationId"] = 0;//先清除库位ID
            if (string.IsNullOrWhiteSpace(inspectionStorageLocationNo)) return;
            var foundStorageLocations = (from s in GlobalData.AllStorageLocations
                                         where s["no"]?.ToString() == inspectionStorageLocationNo
                                         select s).ToArray();
            if (foundStorageLocations.Length != 1) goto FAILED;
            this.model[row, "inspectionStorageLocationId"] = (int)foundStorageLocations[0]["id"];
            this.model[row, "inspectionStorageLocationName"] = foundStorageLocations[0]["name"];
            return;

            FAILED:
            MessageBox.Show($"库位\"{inspectionStorageLocationNo}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void InspectionStorageLocationNameEditEnded(int row, string inspectionStorageLocationName)
        {
            this.model[row, "inspectionStorageLocationId"] = 0;//先清除库位ID
            if (string.IsNullOrWhiteSpace(inspectionStorageLocationName)) return;
            var foundinspectionStorageLocations = (from s in GlobalData.AllStorageLocations
                                         where s["name"]?.ToString() == inspectionStorageLocationName
                                         select s).ToArray();
            if (foundinspectionStorageLocations.Length != 1) goto FAILED;
            this.model[row, "inspectionStorageLocationId"] = (int)foundinspectionStorageLocations[0]["id"];
            this.model[row, "inspectionStorageLocationNo"] = foundinspectionStorageLocations[0]["no"];
            return;

            FAILED:
            MessageBox.Show($"库位\"{inspectionStorageLocationName}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void ReturnStorageLocationNoEditEnded(int row, string returnStorageLocationNo)
        {
            this.model[row, "returnStorageLocationId"] = 0;//先清除库位ID
            if (string.IsNullOrWhiteSpace(returnStorageLocationNo)) return;
            var foundStorageLocations = (from s in GlobalData.AllStorageLocations
                                         where s["no"]?.ToString() == returnStorageLocationNo
                                         select s).ToArray();
            if (foundStorageLocations.Length != 1) goto FAILED;
            this.model[row, "returnStorageLocationId"] = (int)foundStorageLocations[0]["id"];
            this.model[row, "returnStorageLocationName"] = foundStorageLocations[0]["name"];
            return;

            FAILED:
            MessageBox.Show($"库位\"{returnStorageLocationNo}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void ReturnStorageLocationNameEditEnded(int row, string returnStorageLocationName)
        {
            this.model[row, "returnStorageLocationId"] = 0;//先清除库位ID
            if (string.IsNullOrWhiteSpace(returnStorageLocationName)) return;
            var foundStorageLocations = (from s in GlobalData.AllStorageLocations
                                         where s["name"]?.ToString() == returnStorageLocationName
                                         select s).ToArray();
            if (foundStorageLocations.Length != 1) goto FAILED;
            this.model[row, "returnStorageLocationId"] = (int)foundStorageLocations[0]["id"];
            this.model[row, "returnStorageLocationNo"] = foundStorageLocations[0]["no"];
            return;

            FAILED:
            MessageBox.Show($"库位\"{returnStorageLocationName}\"不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            this.model.InsertRow(0,null);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model.RemoveSelectedRows();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView.Search();
            }
        }

        private void buttonAllPass_Click(object sender, EventArgs e)
        {
            var inspectFinishArgs = new InspectFinishArgs();
            inspectFinishArgs.allFinish = true;
            inspectFinishArgs.inspectionNoteId = (int)this.inspectionNote["id"];
            inspectFinishArgs.warehouseEntryId = (int)this.inspectionNote["warehouseEntryId"];
            inspectFinishArgs.personId = (int)GlobalData.Person["id"];
            inspectFinishArgs.version= (int)this.inspectionNote["version"];
            JsonSerializer serializer = new JsonSerializer();
            try
            {
                RestClient.RequestPost<string>(Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/inspection_note/inspect_finish",
                     serializer.Serialize(inspectFinishArgs),
                    "PUT");
                this.searchView.Search();
                MessageBox.Show("操作成功！","提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(WebException ex)
            {
                string msg = ex.Message;
                if(ex.Response != null)
                {
                    msg = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show("操作失败：" + msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void itemInspectFinish(bool isQualified)
        {
            if (this.model.SelectionRange == null)
            {
                MessageBox.Show("请选择一项进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int row = this.model.SelectionRange.Row;
            var inspectFinishArgs = new InspectFinishArgs();
            inspectFinishArgs.allFinish = false;
            inspectFinishArgs.inspectionNoteId = (int)this.inspectionNote["id"];
            inspectFinishArgs.warehouseEntryId = (int)this.inspectionNote["warehouseEntryId"];
            InspectFinishItem inspectFinishItem = new InspectFinishItem();
            inspectFinishArgs.inspectFinishItems = new InspectFinishItem[] { inspectFinishItem };
            inspectFinishArgs.version= (int)this.inspectionNote["version"];
            if (this.model.SelectionRange == null)
            {
                MessageBox.Show("请选择一项进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int selectedRow = this.model.SelectionRange.Row;
            inspectFinishItem.inspectionNoteItemId = (int)this.model[selectedRow, "id"];
            inspectFinishItem.personId = (int?)this.model[selectedRow, "personId"];
            inspectFinishItem.qualified = isQualified;
            inspectFinishItem.returnAmount = (double?)this.model[selectedRow, "returnAmount"];
            inspectFinishItem.returnUnit = (string)this.model[selectedRow, "returnUnit"];
            inspectFinishItem.returnUnitAmount = (double?)this.model[selectedRow, "returnUnitAmount"];
            inspectFinishItem.version= (int)this.model[selectedRow, "version"];
            JsonSerializer serializer = new JsonSerializer();
            try
            {
                RestClient.RequestPost<string>(Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/inspection_note/inspect_finish",
                     serializer.Serialize(inspectFinishArgs),
                    "PUT");
                this.searchView.Search();
                MessageBox.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (WebException ex)
            {
                string msg = ex.Message;
                if (ex.Response != null)
                {
                    msg = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show("操作失败：" + msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FormInspectionNoteItem_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.RefreshInspectionNoteCallback?.Invoke();
        }

        private void buttonUnqualified_Click(object sender, EventArgs e)
        {
            this.itemInspectFinish(false);
        }

        private void buttonQualified_Click(object sender, EventArgs e)
        {
            this.itemInspectFinish(true);
        }

        private void model_SelectionRangeChanged(object sender, ModelSelectionRangeChangedEventArgs e)
        {
            this.RefreshMode();
        }

        private void model_Refreshed(object sender, ModelRefreshedEventArgs e)
        {
            this.RefreshMode();
        }

        private void RefreshMode()
        {
            if (this.model.GetSelectedRow<int>("state") != STATE_NOT_INSPECTED)
            {
                this.basicView1.Mode = "inspected";
                this.reoGridView1.Mode = "inspected";
            }
            else
            {
                this.basicView1.Mode = "default";
                this.reoGridView1.Mode = "default";
            }
        }

        private void basicView1_Load(object sender, EventArgs e)
        {

        }
    }
}

public class InspectFinishArgs
{
    public bool allFinish = false;
    public int inspectionNoteId = -1;
    public int warehouseEntryId = -1;
    public bool qualified = true;
    public int personId = -1;
    public int version = -1;
    public InspectFinishItem[] inspectFinishItems = new InspectFinishItem[] { };
}

public class InspectFinishItem
{
    public int inspectionNoteItemId = -1;
    public bool qualified = true;
    public double? returnAmount;
    public String returnUnit;
    public double? returnUnitAmount;
    public int? personId = -1;
    public int version = -1;
}

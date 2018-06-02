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
            Utilities.BindBlueButton(this.buttonFinished);
            Utilities.BindBlueButton(this.buttonAllPass);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView.Search();
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

        private void buttonFinished_Click(object sender, EventArgs e)
        {
            if(this.model.SelectionRange == null)
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
            if(this.model.SelectionRange == null)
            {
                MessageBox.Show("请选择一项进行操作！","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int selectedRow = this.model.SelectionRange.Row;
            inspectFinishItem.inspectionNoteItemId = (int)this.model[selectedRow, "id"];
            inspectFinishItem.personId = (int?)this.model[selectedRow,"personId"];
            inspectFinishItem.qualified = true;
            inspectFinishItem.returnAmount = (double?)this.model[selectedRow, "returnAmount"];
            inspectFinishItem.returnUnit = (string)this.model[selectedRow, "returnUnit"];
            inspectFinishItem.returnUnitAmount = (double?)this.model[selectedRow, "returnUnitAmount"];
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
    }
}

public class InspectFinishArgs
{
    public bool allFinish = false;
    public int inspectionNoteId = -1;
    public int warehouseEntryId = -1;
    public bool qualified = true;
    public int personId = -1;
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
}

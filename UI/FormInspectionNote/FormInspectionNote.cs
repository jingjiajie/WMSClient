using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using FrontWork;

namespace WMS.UI
{
    public partial class FormInspectionNote : Form
    {
        public FormInspectionNote()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private void FormInspectionNote_Load(object sender, EventArgs e)
        {
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            this.searchView1.Search();
        }

        private string StateForwardMapper(int state)
        {
            switch (state)
            {
                case 0: return "送检中";
                case 1: return "部分送检完成";
                case 2: return "全部送检完成";
                default: return "未知状态";
            }
        }

        private void buttonItems_Click(object sender, EventArgs e)
        {
            if (this.model1.SelectionRange == null || this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项送检单查看物料条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            } 
            var inspectionNote = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row })[0];
            if (inspectionNote["id"] == null)
            {
                MessageBox.Show("请先保存单据再查看条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            new FormInspectionNoteItem(inspectionNote, ()=>this.searchView1.Search()).Show();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
            }
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            //获取选中行ID，过滤掉新建的行（ID为0的）
            int[] selectedIDs = this.model1.GetSelectedRows<int>("id").Except(new int[] { 0 }).ToArray();
            if (selectedIDs.Length == 0)
            {
                MessageBox.Show("请选择一项进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var formChooseType = new FormInspectionNoteChoosePreviewType(selectedIDs);
            formChooseType.Show();
        }

        public void SearchAndSelectByIDs(int[] ids)
        {
            this.searchView1.AddStaticCondition("id", (from id in ids select (object)id).ToArray(), Relation.IN);
            this.searchView1.Search();
            this.model1.SelectRowsByValues("id", ids);
            this.searchView1.ClearStaticCondition("id");
        }

        public void SearchByWarehouseEntryNo(string warehouseEntryNo)
        {
            this.searchView1.AddCondition("warehouseEntryNo", warehouseEntryNo);
            this.searchView1.Search();
        }
    }
}

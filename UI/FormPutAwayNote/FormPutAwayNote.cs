﻿using FrontWork;
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

namespace WMS.UI
{
    public partial class FormPutAwayNote : Form
    {
        public FormPutAwayNote()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
            this.model1.CellUpdated += this.model_CellUpdated;
        }

        private void model_CellUpdated(object sender, ModelCellUpdatedEventArgs e)
        {
            foreach(var cell in e.UpdatedCells)
            {
                if (cell.ColumnName.StartsWith("lastUpdate")) return;
                this.model1[cell.Row, "lastUpdatePersonId"] = GlobalData.Person["id"];
                this.model1[cell.Row, "lastUpdatePersonName"] = GlobalData.Person["name"];
                this.model1[cell.Row, "lastUpdateTime"] = DateTime.Now;
            }
        }

        //添加按钮点击事件
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0,new Dictionary<string, object>()
            {
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "createPersonId",GlobalData.Person["id"]},
                { "createPersonName",GlobalData.Person["name"]},
                { "createTime",DateTime.Now}
            });
        }

        //删除按钮点击事件
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }

        //保存按钮点击事件
        private void buttonSave_Click(object sender, EventArgs e)
        {

        }

        private void FormWarehouseEntry_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url",Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            this.searchView1.AddStaticCondition("type",0);
            this.searchView1.Search();
        }
        
        private void buttonItems_Click(object sender, EventArgs e)
        {
            if (this.model1.SelectionRange == null || this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项上架单查看物料条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var putAwayNote = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row })[0];
            if (putAwayNote["id"] == null)
            {
                MessageBox.Show("请先保存单据再查看条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            new FormPutAwayNoteItem(putAwayNote).Show();
        }

        private string StateForwardMapper(int state)
        {
            switch (state)
            {
                case 0: return "待上架";
                case 1: return "部分上架完成";
                case 2: return "全部上架完成";
                default: return "未知状态";
            }
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            if (this.model1.SelectionRange == null)
            {
                MessageBox.Show("请选择要预览的盘点单！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            List<int> ids = new List<int>();
            for (int i = 0; i < this.model1.SelectionRange.Rows; i++)
            {
                int curRow = this.model1.SelectionRange.Row + i;
                if (this.model1[curRow, "id"] == null) continue;
                ids.Add((int)this.model1[curRow, "id"]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string strIDs = serializer.Serialize(ids);

            var previewData = RestClient.Get<List<IDictionary<string, object>>>(Defines.ServerURL + "/warehouse/WMS_Template/transfer_order/preview/" + strIDs);
            if (previewData == null) return;
            StandardFormPreviewExcel formPreviewExcel = new StandardFormPreviewExcel("上架单预览");
            foreach (IDictionary<string, object> entryAndItem in previewData)
            {
                IDictionary<string, object> transferOrder = (IDictionary<string, object>)entryAndItem["transferOrder"];
                object[] transferOrderItems = (object[])entryAndItem["transferOrderItems"];
                string no = (string)transferOrder["no"];
                if (!formPreviewExcel.AddPatternTable("Excel/PutAwayNote.xlsx", no)) return;
                formPreviewExcel.AddData("putAwayTicket", transferOrder, no);
                formPreviewExcel.AddData("putAwayTicketItems", transferOrderItems, no);
            }
            formPreviewExcel.Show();
        }

        private void toolStripTop_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void buttonAutoPutAway_Click(object sender, EventArgs e)
        {
            try
            {
                string body = "{\"warehouseId\":\"" + GlobalData.Warehouse["id"] + "\",\"personId\":\"" + GlobalData.Person["id"] + "\",\"transferType\":\"" + 0 + "\"}";
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/delivery_order/transfer_auto";
                RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);
                MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.searchView1.Search();

            }
            catch(WebException ex)
            {
                string message = ex.Message;
                if(ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show("添加失败："+message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
            }
        }
    }
}

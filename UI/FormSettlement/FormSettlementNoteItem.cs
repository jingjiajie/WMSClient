﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontWork;

namespace WMS.UI.FormSettlement
{
    public partial class FormSettlementNoteItem : Form
    {
        private IDictionary<string, object> settlementNote = null;
        private Action addFinishedCallback = null;
        public FormSettlementNoteItem(IDictionary<string, object> settlementNote)
        {
            this.settlementNote = settlementNote;
            

            int noteState = (int)this.settlementNote["state"];
            if (noteState ==1)
            {
                this.basicView1.Mode = "default1";
                this.reoGridView1.Mode = "default1";
                this.synchronizer.Mode = "default1";
            }

            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private void FormSettlementNoteItem_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            this.searchView1.AddStaticCondition("settlementNoteId", this.settlementNote["id"]);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);

            this.searchView1.Search();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
            }
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
                this.model1[row, "no"] = foundSupplier["no"];
            }
        }

        //供应商代号编辑完成，根据名称自动搜索ID和No
        private void SupplierNoEditEnded([Row]int row, [Data] string supplierNo)
        {
            IDictionary<string, object> foundSupplier =
                GlobalData.AllSuppliers.Find((s) =>
                {
                    if (s["no"] == null) return false;
                    return s["name"].ToString() == supplierNo && s["warehouseId"] != GlobalData.Warehouse["id"];
                });
            if (foundSupplier == null)
            {
                MessageBox.Show($"供应商\"{supplierNo}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "supplierId"] = foundSupplier["id"];
                this.model1[row, "name"] = foundSupplier["name"];
            }
        }

        public void SetAddFinishedCallback(Action callback)
        {
            this.addFinishedCallback = callback;
        }

        private void FormPutAwayClosed(object sender, FormClosedEventArgs e)
        {
            if (this.addFinishedCallback != null)
            {
                this.addFinishedCallback();
            }
        }

        private string StateForwardMapper([Data]int state)
        {
            switch (state)
            {
                case 0: return "待供货商确认";
                case 1: return "供货商已确认";
                default: return "未知状态";
            }
        }

        private int StateBackwardMapper([Data]string state)
        {
            switch (state)
            {
                case "待供货商确认": return 0;
                case "供货商已确认": return 1;
                default: return -1;
            }
        }
    }
}

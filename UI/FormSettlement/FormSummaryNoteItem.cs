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
    public partial class FormSummaryNoteItem : Form
    {
        public FormSummaryNoteItem()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "summaryNoteID",GlobalData.Person["id"]},             
                { "state",0},
            });
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save()) { this.searchView1.Search(); }
        }
    }

    public class summaryNoteItemState
    {
        public const int WAITTING_FOR_SUPPLIER_CONFIRM = 0;
        public const int SUPPLIER_CONFIRMED = 1;
    }
}

﻿namespace WMS.UI.FormBase
{
    partial class FormUser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUser));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.reoGridControlMain = new unvell.ReoGrid.ReoGridControl();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.labelSelect = new System.Windows.Forms.ToolStripLabel();
            this.comboBoxSearchCondition = new System.Windows.Forms.ToolStripComboBox();
            this.textBoxSearchValue = new System.Windows.Forms.ToolStripTextBox();
            this.buttonSearch = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonAdd = new System.Windows.Forms.ToolStripButton();
            this.buttonAlter = new System.Windows.Forms.ToolStripButton();
            this.buttonDelete = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1.SuspendLayout();
            this.toolStripTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.labelStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 415);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 9, 0);
            this.statusStrip1.Size = new System.Drawing.Size(901, 25);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(54, 20);
            this.toolStripStatusLabel1.Text = "状态：";
            // 
            // labelStatus
            // 
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(69, 20);
            this.labelStatus.Text = "用户管理";
            // 
            // reoGridControlMain
            // 
            this.reoGridControlMain.BackColor = System.Drawing.Color.White;
            this.reoGridControlMain.ColumnHeaderContextMenuStrip = null;
            this.reoGridControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGridControlMain.LeadHeaderContextMenuStrip = null;
            this.reoGridControlMain.Location = new System.Drawing.Point(0, 28);
            this.reoGridControlMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.reoGridControlMain.Name = "reoGridControlMain";
            this.reoGridControlMain.Readonly = true;
            this.reoGridControlMain.RowHeaderContextMenuStrip = null;
            this.reoGridControlMain.Script = null;
            this.reoGridControlMain.SheetTabContextMenuStrip = null;
            this.reoGridControlMain.SheetTabNewButtonVisible = true;
            this.reoGridControlMain.SheetTabVisible = true;
            this.reoGridControlMain.SheetTabWidth = 80;
            this.reoGridControlMain.ShowScrollEndSpacing = true;
            this.reoGridControlMain.Size = new System.Drawing.Size(901, 412);
            this.reoGridControlMain.TabIndex = 6;
            this.reoGridControlMain.Text = "reoGridControl1";
            // 
            // toolStripTop
            // 
            this.toolStripTop.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelSelect,
            this.comboBoxSearchCondition,
            this.textBoxSearchValue,
            this.buttonSearch,
            this.toolStripSeparator1,
            this.buttonAdd,
            this.buttonAlter,
            this.buttonDelete});
            this.toolStripTop.Location = new System.Drawing.Point(0, 0);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Size = new System.Drawing.Size(901, 28);
            this.toolStripTop.TabIndex = 5;
            this.toolStripTop.Text = "toolStrip1";
            // 
            // labelSelect
            // 
            this.labelSelect.Name = "labelSelect";
            this.labelSelect.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelSelect.Size = new System.Drawing.Size(84, 25);
            this.labelSelect.Text = "查询条件：";
            // 
            // comboBoxSearchCondition
            // 
            this.comboBoxSearchCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSearchCondition.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.comboBoxSearchCondition.Name = "comboBoxSearchCondition";
            this.comboBoxSearchCondition.Size = new System.Drawing.Size(128, 28);
            // 
            // textBoxSearchValue
            // 
            this.textBoxSearchValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSearchValue.Name = "textBoxSearchValue";
            this.textBoxSearchValue.Size = new System.Drawing.Size(194, 28);
            // 
            // buttonSearch
            // 
            this.buttonSearch.AutoSize = false;
            this.buttonSearch.Image = ((System.Drawing.Image)(resources.GetObject("buttonSearch.Image")));
            this.buttonSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(60, 25);
            this.buttonSearch.Text = "查询";
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AutoSize = false;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(15, 28);
            // 
            // buttonAdd
            // 
            this.buttonAdd.AutoSize = false;
            this.buttonAdd.Image = ((System.Drawing.Image)(resources.GetObject("buttonAdd.Image")));
            this.buttonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(60, 25);
            this.buttonAdd.Text = "添加";
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonAlter
            // 
            this.buttonAlter.AutoSize = false;
            this.buttonAlter.Image = ((System.Drawing.Image)(resources.GetObject("buttonAlter.Image")));
            this.buttonAlter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonAlter.Name = "buttonAlter";
            this.buttonAlter.Size = new System.Drawing.Size(60, 25);
            this.buttonAlter.Text = "修改";
            this.buttonAlter.Click += new System.EventHandler(this.buttonAlter_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.AutoSize = false;
            this.buttonDelete.Image = ((System.Drawing.Image)(resources.GetObject("buttonDelete.Image")));
            this.buttonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(60, 25);
            this.buttonDelete.Text = "删除";
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // FormUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(901, 440);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.reoGridControlMain);
            this.Controls.Add(this.toolStripTop);
            this.Name = "FormUser";
            this.Text = "FormUser";
            this.Load += new System.EventHandler(this.FormUser_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel labelStatus;
        private unvell.ReoGrid.ReoGridControl reoGridControlMain;
        private System.Windows.Forms.ToolStrip toolStripTop;
        private System.Windows.Forms.ToolStripLabel labelSelect;
        private System.Windows.Forms.ToolStripComboBox comboBoxSearchCondition;
        private System.Windows.Forms.ToolStripTextBox textBoxSearchValue;
        private System.Windows.Forms.ToolStripButton buttonSearch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton buttonAdd;
        private System.Windows.Forms.ToolStripButton buttonAlter;
        private System.Windows.Forms.ToolStripButton buttonDelete;
    }
}
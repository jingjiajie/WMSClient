﻿namespace WMS.UI
{
    partial class StandardFormPreviewExcel
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
            this.reoGridControlMain = new unvell.ReoGrid.ReoGridControl();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.buttonExport = new System.Windows.Forms.ToolStripButton();
            this.buttonPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripTop.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // reoGridControlMain
            // 
            this.reoGridControlMain.BackColor = System.Drawing.Color.White;
            this.reoGridControlMain.ColumnHeaderContextMenuStrip = null;
            this.reoGridControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGridControlMain.LeadHeaderContextMenuStrip = null;
            this.reoGridControlMain.Location = new System.Drawing.Point(0, 30);
            this.reoGridControlMain.Margin = new System.Windows.Forms.Padding(0);
            this.reoGridControlMain.Name = "reoGridControlMain";
            this.reoGridControlMain.Readonly = true;
            this.reoGridControlMain.RowHeaderContextMenuStrip = null;
            this.reoGridControlMain.Script = null;
            this.reoGridControlMain.SheetTabContextMenuStrip = null;
            this.reoGridControlMain.SheetTabNewButtonVisible = true;
            this.reoGridControlMain.SheetTabVisible = true;
            this.reoGridControlMain.SheetTabWidth = 140;
            this.reoGridControlMain.ShowScrollEndSpacing = true;
            this.reoGridControlMain.Size = new System.Drawing.Size(1074, 499);
            this.reoGridControlMain.TabIndex = 4;
            this.reoGridControlMain.Text = "reoGridControl1";
            // 
            // toolStripTop
            // 
            this.toolStripTop.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStripTop.BackgroundImage = global::WMS.UI.Properties.Resources.bottonW_q;
            this.toolStripTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStripTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripTop.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonPrint,
            this.toolStripSeparator1,
            this.buttonExport});
            this.toolStripTop.Location = new System.Drawing.Point(0, 0);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripTop.Size = new System.Drawing.Size(1074, 30);
            this.toolStripTop.TabIndex = 5;
            this.toolStripTop.Text = "toolStrip1";
            // 
            // buttonExport
            // 
            this.buttonExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(123, 35);
            this.buttonExport.Text = "导出Excel";
            // 
            // buttonPrint
            // 
            this.buttonPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(66, 35);
            this.buttonPrint.Text = "打印";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 38);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.toolStripTop, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.reoGridControlMain, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1074, 529);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // StandardFormPreviewExcel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1074, 529);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("黑体", 10F);
            this.Name = "StandardFormPreviewExcel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "标准单据预览窗口";
            this.Load += new System.EventHandler(this.StandardFormPreviewExcel_Load);
            this.Shown += new System.EventHandler(this.StandardFormPreviewExcel_Shown);
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private unvell.ReoGrid.ReoGridControl reoGridControlMain;
        private System.Windows.Forms.ToolStrip toolStripTop;
        private System.Windows.Forms.ToolStripButton buttonPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton buttonExport;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
namespace WMS.UI
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
            try
            {
                base.Dispose(disposing);
            }
            catch //莫名其妙崩溃，先catch临时修复。
            {

            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StandardFormPreviewExcel));
            this.reoGridControlMain = new unvell.ReoGrid.ReoGridControl();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.buttonExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.textBoxScale = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.textBoxPaperSize = new System.Windows.Forms.ToolStripTextBox();
            this.comboBoxPrintSettingsRange = new System.Windows.Forms.ToolStripComboBox();
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
            this.reoGridControlMain.CurrentWorksheetChanged += new System.EventHandler(this.reoGridControlMain_CurrentWorksheetChanged);
            // 
            // toolStripTop
            // 
            this.toolStripTop.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStripTop.BackgroundImage = global::WMS.UI.Properties.Resources.bottonW_q;
            this.toolStripTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStripTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripTop.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonExport,
            this.toolStripSeparator1,
            this.buttonPrint,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.textBoxScale,
            this.toolStripLabel2,
            this.toolStripSeparator3,
            this.toolStripLabel3,
            this.textBoxPaperSize,
            this.comboBoxPrintSettingsRange});
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
            this.buttonExport.Size = new System.Drawing.Size(123, 27);
            this.buttonExport.Text = "导出Excel";
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 30);
            // 
            // buttonPrint
            // 
            this.buttonPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(114, 27);
            this.buttonPrint.Text = "打印单据";
            this.buttonPrint.Click += new System.EventHandler(this.buttonPrint_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 30);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(182, 27);
            this.toolStripLabel1.Text = "打印缩放比例：";
            // 
            // textBoxScale
            // 
            this.textBoxScale.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.textBoxScale.Name = "textBoxScale";
            this.textBoxScale.Size = new System.Drawing.Size(50, 30);
            this.textBoxScale.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxScale.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxScale_KeyDown);
            this.textBoxScale.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxScale_KeyPress);
            this.textBoxScale.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxScale_KeyUp);
            this.textBoxScale.Click += new System.EventHandler(this.textBoxScale_Click);
            this.textBoxScale.TextChanged += new System.EventHandler(this.textBoxScale_TextChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(38, 27);
            this.toolStripLabel2.Text = "倍";
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
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(134, 27);
            this.toolStripLabel3.Text = "纸张大小：";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 30);
            // 
            // textBoxPaperSize
            // 
            this.textBoxPaperSize.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.textBoxPaperSize.Name = "textBoxPaperSize";
            this.textBoxPaperSize.Size = new System.Drawing.Size(110, 30);
            this.textBoxPaperSize.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxPaperSize.TextChanged += new System.EventHandler(this.textBoxPaperSize_TextChanged);
            // 
            // comboBoxPrintSettingsRange
            // 
            this.comboBoxPrintSettingsRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPrintSettingsRange.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.comboBoxPrintSettingsRange.Items.AddRange(new object[] {
            "设置本页",
            "设置全部页"});
            this.comboBoxPrintSettingsRange.Name = "comboBoxPrintSettingsRange";
            this.comboBoxPrintSettingsRange.Size = new System.Drawing.Size(121, 30);
            this.comboBoxPrintSettingsRange.SelectedIndexChanged += new System.EventHandler(this.comboBoxPrintSettingsRange_SelectedIndexChanged);
            // 
            // StandardFormPreviewExcel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1074, 529);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("黑体", 10F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
        private System.Windows.Forms.ToolStripTextBox textBoxScale;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox textBoxPaperSize;
        private System.Windows.Forms.ToolStripComboBox comboBoxPrintSettingsRange;
    }
}
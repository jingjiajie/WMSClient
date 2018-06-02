namespace WMS.UI
{
    partial class FormBaseComponent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBaseComponent));
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.labelStatus = new System.Windows.Forms.ToolStripLabel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.reoGridControlComponen = new unvell.ReoGrid.ReoGridControl();
            this.panelPager = new System.Windows.Forms.Panel();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAlter = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonComponentSingleBoxTranPackingInfo = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonComponentOuterPackingSize = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonComponentShipmentInfo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonImport = new System.Windows.Forms.ToolStripButton();
            this.panelSearchWidget = new System.Windows.Forms.Panel();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.toolStripTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 39);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1352, 677);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.labelStatus});
            this.toolStrip1.Location = new System.Drawing.Point(0, 502);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(978, 20);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(35, 17);
            this.toolStripStatusLabel1.Text = "状态:";
            // 
            // labelStatus
            // 
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(56, 17);
            this.labelStatus.Text = "零件信息";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.reoGridControlComponen, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.toolStrip1, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.panelPager, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.toolStripTop, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panelSearchWidget, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(978, 522);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // reoGridControlComponen
            // 
            this.reoGridControlComponen.BackColor = System.Drawing.Color.White;
            this.reoGridControlComponen.ColumnHeaderContextMenuStrip = null;
            this.reoGridControlComponen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGridControlComponen.LeadHeaderContextMenuStrip = null;
            this.reoGridControlComponen.Location = new System.Drawing.Point(2, 47);
            this.reoGridControlComponen.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.reoGridControlComponen.Name = "reoGridControlComponen";
            this.reoGridControlComponen.RowHeaderContextMenuStrip = null;
            this.reoGridControlComponen.Script = null;
            this.reoGridControlComponen.SheetTabContextMenuStrip = null;
            this.reoGridControlComponen.SheetTabNewButtonVisible = true;
            this.reoGridControlComponen.SheetTabVisible = true;
            this.reoGridControlComponen.SheetTabWidth = 60;
            this.reoGridControlComponen.ShowScrollEndSpacing = true;
            this.reoGridControlComponen.Size = new System.Drawing.Size(974, 423);
            this.reoGridControlComponen.TabIndex = 4;
            this.reoGridControlComponen.Text = "reoGridControl1";
            // 
            // panelPager
            // 
            this.panelPager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPager.Location = new System.Drawing.Point(3, 475);
            this.panelPager.Name = "panelPager";
            this.panelPager.Size = new System.Drawing.Size(972, 24);
            this.panelPager.TabIndex = 5;
            // 
            // toolStripTop
            // 
            this.toolStripTop.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStripTop.BackgroundImage = global::WMS.UI.Properties.Resources.bottonW_q;
            this.toolStripTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStripTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripTop.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAdd,
            this.toolStripButtonAlter,
            this.toolStripButtonDelete,
            this.toolStripSeparator3,
            this.toolStripButtonComponentSingleBoxTranPackingInfo,
            this.toolStripButtonComponentOuterPackingSize,
            this.toolStripButtonComponentShipmentInfo,
            this.toolStripSeparator1,
            this.buttonImport});
            this.toolStripTop.Location = new System.Drawing.Point(0, 25);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Size = new System.Drawing.Size(978, 20);
            this.toolStripTop.TabIndex = 2;
            this.toolStripTop.Text = "toolStrip1";
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAdd.Image")));
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(56, 17);
            this.toolStripButtonAdd.Text = "添加";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonAlter
            // 
            this.toolStripButtonAlter.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAlter.Image")));
            this.toolStripButtonAlter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAlter.Name = "toolStripButtonAlter";
            this.toolStripButtonAlter.Size = new System.Drawing.Size(56, 17);
            this.toolStripButtonAlter.Text = "修改";
            this.toolStripButtonAlter.Click += new System.EventHandler(this.toolStripButtonAlter_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDelete.Image")));
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(56, 17);
            this.toolStripButtonDelete.Text = "删除";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AutoSize = false;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(15, 28);
            // 
            // toolStripButtonComponentSingleBoxTranPackingInfo
            // 
            this.toolStripButtonComponentSingleBoxTranPackingInfo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonComponentSingleBoxTranPackingInfo.Image")));
            this.toolStripButtonComponentSingleBoxTranPackingInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonComponentSingleBoxTranPackingInfo.Name = "toolStripButtonComponentSingleBoxTranPackingInfo";
            this.toolStripButtonComponentSingleBoxTranPackingInfo.Size = new System.Drawing.Size(157, 17);
            this.toolStripButtonComponentSingleBoxTranPackingInfo.Text = "查看/修改单箱包装信息";
            this.toolStripButtonComponentSingleBoxTranPackingInfo.Click += new System.EventHandler(this.toolStripButtonComponentSingleBoxTranPackingInfo_Click);
            // 
            // toolStripButtonComponentOuterPackingSize
            // 
            this.toolStripButtonComponentOuterPackingSize.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonComponentOuterPackingSize.Image")));
            this.toolStripButtonComponentOuterPackingSize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonComponentOuterPackingSize.Name = "toolStripButtonComponentOuterPackingSize";
            this.toolStripButtonComponentOuterPackingSize.Size = new System.Drawing.Size(169, 17);
            this.toolStripButtonComponentOuterPackingSize.Text = "查看/修改零件外包装信息";
            this.toolStripButtonComponentOuterPackingSize.Click += new System.EventHandler(this.toolStripButtonComponentOuterPackingSize_Click);
            // 
            // toolStripButtonComponentShipmentInfo
            // 
            this.toolStripButtonComponentShipmentInfo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonComponentShipmentInfo.Image")));
            this.toolStripButtonComponentShipmentInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonComponentShipmentInfo.Name = "toolStripButtonComponentShipmentInfo";
            this.toolStripButtonComponentShipmentInfo.Size = new System.Drawing.Size(157, 17);
            this.toolStripButtonComponentShipmentInfo.Text = "查看/修改出货包装信息";
            this.toolStripButtonComponentShipmentInfo.Click += new System.EventHandler(this.toolStripButtonComponentShipmentInfo_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AutoSize = false;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(15, 28);
            // 
            // buttonImport
            // 
            this.buttonImport.Image = ((System.Drawing.Image)(resources.GetObject("buttonImport.Image")));
            this.buttonImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(80, 17);
            this.buttonImport.Text = "批量导入";
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // panelSearchWidget
            // 
            this.panelSearchWidget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSearchWidget.Location = new System.Drawing.Point(0, 0);
            this.panelSearchWidget.Margin = new System.Windows.Forms.Padding(0);
            this.panelSearchWidget.Name = "panelSearchWidget";
            this.panelSearchWidget.Size = new System.Drawing.Size(978, 25);
            this.panelSearchWidget.TabIndex = 6;
            // 
            // FormBaseComponent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 522);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FormBaseComponent";
            this.Text = "零件信息";
            this.Load += new System.EventHandler(this.FormBaseComponent_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripTop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonAlter;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripLabel labelStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private unvell.ReoGrid.ReoGridControl reoGridControlComponen;
        private System.Windows.Forms.Panel panelPager;
        private System.Windows.Forms.ToolStripButton toolStripButtonComponentSingleBoxTranPackingInfo;
        private System.Windows.Forms.ToolStripButton toolStripButtonComponentOuterPackingSize;
        private System.Windows.Forms.ToolStripButton toolStripButtonComponentShipmentInfo;
        private System.Windows.Forms.ToolStripButton buttonImport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.Panel panelSearchWidget;
    }
}
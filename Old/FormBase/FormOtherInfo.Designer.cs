namespace WMS.UI
{
    partial class FormOtherInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOtherInfo));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.labelStatus = new System.Windows.Forms.ToolStripLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxWarehouse = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonWarehouseAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAlterWarehouse = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDeleteWarehouse = new System.Windows.Forms.ToolStripButton();
            this.reoGridControlWarehouse = new unvell.ReoGrid.ReoGridControl();
            this.groupBoxProject = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.reoGridControlProject = new unvell.ReoGrid.ReoGridControl();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAddProject = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAlterProject = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelectProject = new System.Windows.Forms.ToolStripButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAddPackageUnit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAlterPackageUnit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelectPackageUnit = new System.Windows.Forms.ToolStripButton();
            this.reoGridControlPackageUnit = new unvell.ReoGrid.ReoGridControl();
            this.groupBoxPackageUnit = new System.Windows.Forms.GroupBox();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxWarehouse.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.toolStripTop.SuspendLayout();
            this.groupBoxProject.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.groupBoxPackageUnit.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.labelStatus});
            this.toolStrip1.Location = new System.Drawing.Point(0, 491);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(839, 25);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(35, 22);
            this.toolStripStatusLabel1.Text = "状态:";
            // 
            // labelStatus
            // 
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(56, 22);
            this.labelStatus.Text = "基本信息";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel1.Controls.Add(this.groupBoxWarehouse, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxPackageUnit, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxProject, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(839, 491);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // groupBoxWarehouse
            // 
            this.groupBoxWarehouse.BackColor = System.Drawing.SystemColors.Menu;
            this.groupBoxWarehouse.Controls.Add(this.tableLayoutPanel2);
            this.groupBoxWarehouse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxWarehouse.Font = new System.Drawing.Font("微软雅黑", 15F);
            this.groupBoxWarehouse.Location = new System.Drawing.Point(419, 0);
            this.groupBoxWarehouse.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxWarehouse.Name = "groupBoxWarehouse";
            this.groupBoxWarehouse.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxWarehouse.Size = new System.Drawing.Size(419, 491);
            this.groupBoxWarehouse.TabIndex = 12;
            this.groupBoxWarehouse.TabStop = false;
            this.groupBoxWarehouse.Text = "仓库信息";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.toolStripTop, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.reoGridControlWarehouse, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 27);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(419, 464);
            this.tableLayoutPanel2.TabIndex = 14;
            // 
            // toolStripTop
            // 
            this.toolStripTop.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStripTop.BackgroundImage = global::WMS.UI.Properties.Resources.bottonW_q;
            this.toolStripTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStripTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripTop.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonWarehouseAdd,
            this.toolStripButtonAlterWarehouse,
            this.toolStripButtonDeleteWarehouse});
            this.toolStripTop.Location = new System.Drawing.Point(0, 0);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Size = new System.Drawing.Size(419, 30);
            this.toolStripTop.TabIndex = 4;
            this.toolStripTop.Text = "toolStrip1";
            // 
            // toolStripButtonWarehouseAdd
            // 
            this.toolStripButtonWarehouseAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonWarehouseAdd.Image")));
            this.toolStripButtonWarehouseAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonWarehouseAdd.Name = "toolStripButtonWarehouseAdd";
            this.toolStripButtonWarehouseAdd.Size = new System.Drawing.Size(80, 27);
            this.toolStripButtonWarehouseAdd.Text = "添加仓库";
            this.toolStripButtonWarehouseAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonAlterWarehouse
            // 
            this.toolStripButtonAlterWarehouse.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAlterWarehouse.Image")));
            this.toolStripButtonAlterWarehouse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAlterWarehouse.Name = "toolStripButtonAlterWarehouse";
            this.toolStripButtonAlterWarehouse.Size = new System.Drawing.Size(56, 27);
            this.toolStripButtonAlterWarehouse.Text = "修改";
            this.toolStripButtonAlterWarehouse.Click += new System.EventHandler(this.toolStripButtonAlter_Click);
            // 
            // toolStripButtonDeleteWarehouse
            // 
            this.toolStripButtonDeleteWarehouse.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDeleteWarehouse.Image")));
            this.toolStripButtonDeleteWarehouse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDeleteWarehouse.Name = "toolStripButtonDeleteWarehouse";
            this.toolStripButtonDeleteWarehouse.Size = new System.Drawing.Size(56, 27);
            this.toolStripButtonDeleteWarehouse.Text = "删除";
            this.toolStripButtonDeleteWarehouse.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // reoGridControlWarehouse
            // 
            this.reoGridControlWarehouse.BackColor = System.Drawing.Color.White;
            this.reoGridControlWarehouse.ColumnHeaderContextMenuStrip = null;
            this.reoGridControlWarehouse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGridControlWarehouse.LeadHeaderContextMenuStrip = null;
            this.reoGridControlWarehouse.Location = new System.Drawing.Point(2, 32);
            this.reoGridControlWarehouse.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.reoGridControlWarehouse.Name = "reoGridControlWarehouse";
            this.reoGridControlWarehouse.RowHeaderContextMenuStrip = null;
            this.reoGridControlWarehouse.Script = null;
            this.reoGridControlWarehouse.SheetTabContextMenuStrip = null;
            this.reoGridControlWarehouse.SheetTabNewButtonVisible = true;
            this.reoGridControlWarehouse.SheetTabVisible = true;
            this.reoGridControlWarehouse.SheetTabWidth = 60;
            this.reoGridControlWarehouse.ShowScrollEndSpacing = true;
            this.reoGridControlWarehouse.Size = new System.Drawing.Size(415, 430);
            this.reoGridControlWarehouse.TabIndex = 13;
            this.reoGridControlWarehouse.Text = "reoGridControl1";
            // 
            // groupBoxProject
            // 
            this.groupBoxProject.BackColor = System.Drawing.SystemColors.Menu;
            this.groupBoxProject.Controls.Add(this.tableLayoutPanel3);
            this.groupBoxProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxProject.Font = new System.Drawing.Font("微软雅黑", 15F);
            this.groupBoxProject.Location = new System.Drawing.Point(0, 0);
            this.groupBoxProject.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxProject.Name = "groupBoxProject";
            this.groupBoxProject.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxProject.Size = new System.Drawing.Size(419, 491);
            this.groupBoxProject.TabIndex = 13;
            this.groupBoxProject.TabStop = false;
            this.groupBoxProject.Text = "项目信息";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.reoGridControlProject, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.toolStrip3, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 27);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(419, 464);
            this.tableLayoutPanel3.TabIndex = 15;
            // 
            // reoGridControlProject
            // 
            this.reoGridControlProject.BackColor = System.Drawing.Color.White;
            this.reoGridControlProject.ColumnHeaderContextMenuStrip = null;
            this.reoGridControlProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGridControlProject.LeadHeaderContextMenuStrip = null;
            this.reoGridControlProject.Location = new System.Drawing.Point(2, 32);
            this.reoGridControlProject.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.reoGridControlProject.Name = "reoGridControlProject";
            this.reoGridControlProject.RowHeaderContextMenuStrip = null;
            this.reoGridControlProject.Script = null;
            this.reoGridControlProject.SheetTabContextMenuStrip = null;
            this.reoGridControlProject.SheetTabNewButtonVisible = true;
            this.reoGridControlProject.SheetTabVisible = true;
            this.reoGridControlProject.SheetTabWidth = 60;
            this.reoGridControlProject.ShowScrollEndSpacing = true;
            this.reoGridControlProject.Size = new System.Drawing.Size(415, 430);
            this.reoGridControlProject.TabIndex = 14;
            this.reoGridControlProject.Text = "reoGridControl1";
            this.reoGridControlProject.Click += new System.EventHandler(this.reoGridControlProject_Click);
            // 
            // toolStrip3
            // 
            this.toolStrip3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStrip3.BackgroundImage = global::WMS.UI.Properties.Resources.bottonW_q;
            this.toolStrip3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStrip3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddProject,
            this.toolStripButtonAlterProject,
            this.toolStripButtonDelectProject});
            this.toolStrip3.Location = new System.Drawing.Point(0, 0);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(419, 30);
            this.toolStrip3.TabIndex = 19;
            this.toolStrip3.Text = "toolStrip1";
            // 
            // toolStripButtonAddProject
            // 
            this.toolStripButtonAddProject.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddProject.Image")));
            this.toolStripButtonAddProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddProject.Name = "toolStripButtonAddProject";
            this.toolStripButtonAddProject.Size = new System.Drawing.Size(80, 27);
            this.toolStripButtonAddProject.Text = "添加项目";
            this.toolStripButtonAddProject.Click += new System.EventHandler(this.toolStripButtonAddProject_Click);
            // 
            // toolStripButtonAlterProject
            // 
            this.toolStripButtonAlterProject.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAlterProject.Image")));
            this.toolStripButtonAlterProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAlterProject.Name = "toolStripButtonAlterProject";
            this.toolStripButtonAlterProject.Size = new System.Drawing.Size(56, 27);
            this.toolStripButtonAlterProject.Text = "修改";
            this.toolStripButtonAlterProject.Click += new System.EventHandler(this.toolStripButtonAlterProject_Click);
            // 
            // toolStripButtonDelectProject
            // 
            this.toolStripButtonDelectProject.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDelectProject.Image")));
            this.toolStripButtonDelectProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelectProject.Name = "toolStripButtonDelectProject";
            this.toolStripButtonDelectProject.Size = new System.Drawing.Size(56, 27);
            this.toolStripButtonDelectProject.Text = "删除";
            this.toolStripButtonDelectProject.Click += new System.EventHandler(this.toolStripButtonDelectProject_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.reoGridControlPackageUnit, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.toolStrip2, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 27);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1, 464);
            this.tableLayoutPanel4.TabIndex = 16;
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStrip2.BackgroundImage = global::WMS.UI.Properties.Resources.bottonW_q;
            this.toolStrip2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddPackageUnit,
            this.toolStripButtonAlterPackageUnit,
            this.toolStripButtonDelectPackageUnit});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(1, 30);
            this.toolStrip2.TabIndex = 20;
            this.toolStrip2.Text = "toolStrip1";
            // 
            // toolStripButtonAddPackageUnit
            // 
            this.toolStripButtonAddPackageUnit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddPackageUnit.Image")));
            this.toolStripButtonAddPackageUnit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddPackageUnit.Name = "toolStripButtonAddPackageUnit";
            this.toolStripButtonAddPackageUnit.Size = new System.Drawing.Size(104, 27);
            this.toolStripButtonAddPackageUnit.Text = "添加包装信息";
            this.toolStripButtonAddPackageUnit.Click += new System.EventHandler(this.toolStripButtonAddPackageUnit_Click);
            // 
            // toolStripButtonAlterPackageUnit
            // 
            this.toolStripButtonAlterPackageUnit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAlterPackageUnit.Image")));
            this.toolStripButtonAlterPackageUnit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAlterPackageUnit.Name = "toolStripButtonAlterPackageUnit";
            this.toolStripButtonAlterPackageUnit.Size = new System.Drawing.Size(56, 27);
            this.toolStripButtonAlterPackageUnit.Text = "修改";
            this.toolStripButtonAlterPackageUnit.Click += new System.EventHandler(this.toolStripButtonAlterPackageUnit_Click);
            // 
            // toolStripButtonDelectPackageUnit
            // 
            this.toolStripButtonDelectPackageUnit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDelectPackageUnit.Image")));
            this.toolStripButtonDelectPackageUnit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelectPackageUnit.Name = "toolStripButtonDelectPackageUnit";
            this.toolStripButtonDelectPackageUnit.Size = new System.Drawing.Size(56, 27);
            this.toolStripButtonDelectPackageUnit.Text = "删除";
            this.toolStripButtonDelectPackageUnit.Click += new System.EventHandler(this.toolStripButtonDelectPackageUnit_Click);
            // 
            // reoGridControlPackageUnit
            // 
            this.reoGridControlPackageUnit.BackColor = System.Drawing.Color.White;
            this.reoGridControlPackageUnit.ColumnHeaderContextMenuStrip = null;
            this.reoGridControlPackageUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGridControlPackageUnit.LeadHeaderContextMenuStrip = null;
            this.reoGridControlPackageUnit.Location = new System.Drawing.Point(2, 32);
            this.reoGridControlPackageUnit.Margin = new System.Windows.Forms.Padding(2);
            this.reoGridControlPackageUnit.Name = "reoGridControlPackageUnit";
            this.reoGridControlPackageUnit.RowHeaderContextMenuStrip = null;
            this.reoGridControlPackageUnit.Script = null;
            this.reoGridControlPackageUnit.SheetTabContextMenuStrip = null;
            this.reoGridControlPackageUnit.SheetTabNewButtonVisible = true;
            this.reoGridControlPackageUnit.SheetTabVisible = true;
            this.reoGridControlPackageUnit.SheetTabWidth = 60;
            this.reoGridControlPackageUnit.ShowScrollEndSpacing = true;
            this.reoGridControlPackageUnit.Size = new System.Drawing.Size(1, 430);
            this.reoGridControlPackageUnit.TabIndex = 15;
            this.reoGridControlPackageUnit.Text = "reoGridControl1";
            // 
            // groupBoxPackageUnit
            // 
            this.groupBoxPackageUnit.BackColor = System.Drawing.SystemColors.Menu;
            this.groupBoxPackageUnit.Controls.Add(this.tableLayoutPanel4);
            this.groupBoxPackageUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxPackageUnit.Font = new System.Drawing.Font("微软雅黑", 15F);
            this.groupBoxPackageUnit.Location = new System.Drawing.Point(838, 0);
            this.groupBoxPackageUnit.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxPackageUnit.Name = "groupBoxPackageUnit";
            this.groupBoxPackageUnit.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxPackageUnit.Size = new System.Drawing.Size(1, 491);
            this.groupBoxPackageUnit.TabIndex = 14;
            this.groupBoxPackageUnit.TabStop = false;
            this.groupBoxPackageUnit.Text = "包装单位信息";
            // 
            // FormOtherInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 516);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FormOtherInfo";
            this.Text = "其他信息";
            this.Load += new System.EventHandler(this.base_Warehouse_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBoxWarehouse.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
            this.groupBoxProject.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.groupBoxPackageUnit.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripLabel labelStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBoxWarehouse;
        private unvell.ReoGrid.ReoGridControl reoGridControlWarehouse;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStrip toolStripTop;
        private System.Windows.Forms.ToolStripButton toolStripButtonWarehouseAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonAlterWarehouse;
        private System.Windows.Forms.ToolStripButton toolStripButtonDeleteWarehouse;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBoxProject;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private unvell.ReoGrid.ReoGridControl reoGridControlProject;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddProject;
        private System.Windows.Forms.ToolStripButton toolStripButtonAlterProject;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelectProject;
        private System.Windows.Forms.GroupBox groupBoxPackageUnit;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private unvell.ReoGrid.ReoGridControl reoGridControlPackageUnit;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddPackageUnit;
        private System.Windows.Forms.ToolStripButton toolStripButtonAlterPackageUnit;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelectPackageUnit;
    }
}
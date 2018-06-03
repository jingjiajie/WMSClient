namespace WMS.UI.FormBase
{
    partial class FormBasePerson
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBasePerson));
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAlter = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.labelStatus = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.reoGridControlPerson = new unvell.ReoGrid.ReoGridControl();
            this.panelPager = new System.Windows.Forms.Panel();
            this.checkBoxOnlyThisProAndWare = new System.Windows.Forms.CheckBox();
            this.panelSearchWidget = new System.Windows.Forms.Panel();
            this.toolStripTop.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
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
            this.toolStripButtonDelete});
            this.toolStripTop.Location = new System.Drawing.Point(0, 25);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Size = new System.Drawing.Size(762, 20);
            this.toolStripTop.TabIndex = 5;
            this.toolStripTop.Text = "toolStrip1";
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
            this.labelStatus.Text = "人员信息";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.labelStatus});
            this.toolStrip1.Location = new System.Drawing.Point(0, 438);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(762, 25);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.reoGridControlPerson, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panelPager, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.toolStripTop, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelSearchWidget, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(762, 438);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // reoGridControlPerson
            // 
            this.reoGridControlPerson.BackColor = System.Drawing.Color.White;
            this.reoGridControlPerson.ColumnHeaderContextMenuStrip = null;
            this.reoGridControlPerson.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGridControlPerson.LeadHeaderContextMenuStrip = null;
            this.reoGridControlPerson.Location = new System.Drawing.Point(2, 47);
            this.reoGridControlPerson.Margin = new System.Windows.Forms.Padding(2);
            this.reoGridControlPerson.Name = "reoGridControlPerson";
            this.reoGridControlPerson.RowHeaderContextMenuStrip = null;
            this.reoGridControlPerson.Script = null;
            this.reoGridControlPerson.SheetTabContextMenuStrip = null;
            this.reoGridControlPerson.SheetTabNewButtonVisible = true;
            this.reoGridControlPerson.SheetTabVisible = true;
            this.reoGridControlPerson.SheetTabWidth = 60;
            this.reoGridControlPerson.ShowScrollEndSpacing = true;
            this.reoGridControlPerson.Size = new System.Drawing.Size(758, 339);
            this.reoGridControlPerson.TabIndex = 9;
            this.reoGridControlPerson.Text = "reoGridControl1";
            // 
            // panelPager
            // 
            this.panelPager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPager.Location = new System.Drawing.Point(3, 391);
            this.panelPager.Name = "panelPager";
            this.panelPager.Size = new System.Drawing.Size(756, 24);
            this.panelPager.TabIndex = 10;
            // 
            // checkBoxOnlyThisProAndWare
            // 
            this.checkBoxOnlyThisProAndWare.AutoSize = true;
            this.checkBoxOnlyThisProAndWare.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.checkBoxOnlyThisProAndWare.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.checkBoxOnlyThisProAndWare.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBoxOnlyThisProAndWare.Location = new System.Drawing.Point(182, 26);
            this.checkBoxOnlyThisProAndWare.Name = "checkBoxOnlyThisProAndWare";
            this.checkBoxOnlyThisProAndWare.Size = new System.Drawing.Size(140, 21);
            this.checkBoxOnlyThisProAndWare.TabIndex = 9;
            this.checkBoxOnlyThisProAndWare.Text = "仅查看当前项目/仓库";
            this.checkBoxOnlyThisProAndWare.UseVisualStyleBackColor = false;
            this.checkBoxOnlyThisProAndWare.CheckedChanged += new System.EventHandler(this.checkBoxOnlyThisProAndWare_CheckedChanged);
            // 
            // panelSearchWidget
            // 
            this.panelSearchWidget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSearchWidget.Location = new System.Drawing.Point(0, 0);
            this.panelSearchWidget.Margin = new System.Windows.Forms.Padding(0);
            this.panelSearchWidget.Name = "panelSearchWidget";
            this.panelSearchWidget.Size = new System.Drawing.Size(762, 25);
            this.panelSearchWidget.TabIndex = 11;
            // 
            // FormBasePerson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 463);
            this.Controls.Add(this.checkBoxOnlyThisProAndWare);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormBasePerson";
            this.Text = "FormBasePerson";
            this.Load += new System.EventHandler(this.FormBasePerson_Load);
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonAlter;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStrip toolStripTop;
        private System.Windows.Forms.ToolStripLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripLabel labelStatus;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private unvell.ReoGrid.ReoGridControl reoGridControlPerson;
        private System.Windows.Forms.Panel panelPager;
        private System.Windows.Forms.CheckBox checkBoxOnlyThisProAndWare;
        private System.Windows.Forms.Panel panelSearchWidget;
    }
}
namespace WMS.UI
{
    partial class FormSelectPerson
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectPerson));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxSelectPosition = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.textBoxPersonName = new System.Windows.Forms.ToolStripTextBox();
            this.buttonSearch = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonSelect = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelPagerWidget = new System.Windows.Forms.Panel();
            this.reoGridControlMain = new unvell.ReoGrid.ReoGridControl();
            this.checkBoxOnlyThisProAndWare = new System.Windows.Forms.CheckBox();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.labelStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 758);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1292, 36);
            this.statusStrip1.TabIndex = 15;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(86, 31);
            this.toolStripStatusLabel1.Text = "状态：";
            // 
            // labelStatus
            // 
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(110, 31);
            this.labelStatus.Text = "选择人员";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackgroundImage = global::WMS.UI.Properties.Resources.bottonW_q;
            this.toolStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.toolStripComboBoxSelectPosition,
            this.toolStripLabel1,
            this.textBoxPersonName,
            this.buttonSearch,
            this.toolStripSeparator2,
            this.buttonSelect});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1292, 39);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(86, 36);
            this.toolStripLabel2.Text = "岗位：";
            // 
            // toolStripComboBoxSelectPosition
            // 
            this.toolStripComboBoxSelectPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxSelectPosition.Name = "toolStripComboBoxSelectPosition";
            this.toolStripComboBoxSelectPosition.Size = new System.Drawing.Size(238, 39);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(134, 36);
            this.toolStripLabel1.Text = "人员名称：";
            // 
            // textBoxPersonName
            // 
            this.textBoxPersonName.Name = "textBoxPersonName";
            this.textBoxPersonName.Size = new System.Drawing.Size(200, 39);
            this.textBoxPersonName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPersonName_KeyPress);
            this.textBoxPersonName.Click += new System.EventHandler(this.textBoxPersonName_Click);
            this.textBoxPersonName.VisibleChanged += new System.EventHandler(this.textBoxPersonName_VisbleChanged);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Image = ((System.Drawing.Image)(resources.GetObject("buttonSearch.Image")));
            this.buttonSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(86, 36);
            this.buttonSearch.Text = "查询";
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AutoSize = false;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(15, 28);
            // 
            // buttonSelect
            // 
            this.buttonSelect.Image = ((System.Drawing.Image)(resources.GetObject("buttonSelect.Image")));
            this.buttonSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(134, 36);
            this.buttonSelect.Text = "确认选择";
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelPagerWidget, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.reoGridControlMain, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 39);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1292, 719);
            this.tableLayoutPanel1.TabIndex = 16;
            // 
            // panelPagerWidget
            // 
            this.panelPagerWidget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPagerWidget.Location = new System.Drawing.Point(4, 663);
            this.panelPagerWidget.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelPagerWidget.Name = "panelPagerWidget";
            this.panelPagerWidget.Size = new System.Drawing.Size(1284, 52);
            this.panelPagerWidget.TabIndex = 17;
            // 
            // reoGridControlMain
            // 
            this.reoGridControlMain.BackColor = System.Drawing.Color.White;
            this.reoGridControlMain.ColumnHeaderContextMenuStrip = null;
            this.reoGridControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGridControlMain.LeadHeaderContextMenuStrip = null;
            this.reoGridControlMain.Location = new System.Drawing.Point(4, 4);
            this.reoGridControlMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.reoGridControlMain.Name = "reoGridControlMain";
            this.reoGridControlMain.Readonly = true;
            this.reoGridControlMain.RowHeaderContextMenuStrip = null;
            this.reoGridControlMain.Script = null;
            this.reoGridControlMain.SheetTabContextMenuStrip = null;
            this.reoGridControlMain.SheetTabNewButtonVisible = true;
            this.reoGridControlMain.SheetTabVisible = true;
            this.reoGridControlMain.SheetTabWidth = 140;
            this.reoGridControlMain.ShowScrollEndSpacing = true;
            this.reoGridControlMain.Size = new System.Drawing.Size(1284, 651);
            this.reoGridControlMain.TabIndex = 15;
            this.reoGridControlMain.Text = "reoGridControl1";
            // 
            // checkBoxOnlyThisProAndWare
            // 
            this.checkBoxOnlyThisProAndWare.AutoSize = true;
            this.checkBoxOnlyThisProAndWare.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.checkBoxOnlyThisProAndWare.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.checkBoxOnlyThisProAndWare.Checked = true;
            this.checkBoxOnlyThisProAndWare.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOnlyThisProAndWare.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBoxOnlyThisProAndWare.Location = new System.Drawing.Point(994, 8);
            this.checkBoxOnlyThisProAndWare.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.checkBoxOnlyThisProAndWare.Name = "checkBoxOnlyThisProAndWare";
            this.checkBoxOnlyThisProAndWare.Size = new System.Drawing.Size(272, 35);
            this.checkBoxOnlyThisProAndWare.TabIndex = 17;
            this.checkBoxOnlyThisProAndWare.Text = "仅查看当前项目/仓库";
            this.checkBoxOnlyThisProAndWare.UseVisualStyleBackColor = false;
            this.checkBoxOnlyThisProAndWare.CheckedChanged += new System.EventHandler(this.checkBoxOnlyThisProAndWare_CheckedChanged);
            // 
            // FormSelectPerson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1292, 794);
            this.Controls.Add(this.checkBoxOnlyThisProAndWare);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectPerson";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择人员";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSelectPerson_FormClosing);
            this.Load += new System.EventHandler(this.FormSelectPerson_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel labelStatus;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox textBoxPersonName;
        private System.Windows.Forms.ToolStripButton buttonSearch;
        private System.Windows.Forms.ToolStripButton buttonSelect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private unvell.ReoGrid.ReoGridControl reoGridControlMain;
        private System.Windows.Forms.Panel panelPagerWidget;
        private System.Windows.Forms.CheckBox checkBoxOnlyThisProAndWare;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxSelectPosition;
    }
}
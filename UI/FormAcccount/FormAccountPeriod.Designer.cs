﻿namespace WMS.UI.FormAcccount
{
    partial class FormAccountPeriod
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
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair1 = new FrontWork.ModeMethodListenerNamesPair();
            FrontWork.SearchViewJsonRESTAdapter.APIParamNamesType apiParamNamesType1 = new FrontWork.SearchViewJsonRESTAdapter.APIParamNamesType();
            FrontWork.SearchViewJsonRESTAdapter.ConditionFieldNamesType conditionFieldNamesType1 = new FrontWork.SearchViewJsonRESTAdapter.ConditionFieldNamesType();
            FrontWork.SearchViewJsonRESTAdapter.OrderParamNamesType orderParamNamesType1 = new FrontWork.SearchViewJsonRESTAdapter.OrderParamNamesType();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAccountPeriod));
            this.printDocument2 = new System.Drawing.Printing.PrintDocument();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.labelStatus = new System.Windows.Forms.ToolStripLabel();
            this.panelPager = new System.Windows.Forms.Panel();
            this.pagerView1 = new FrontWork.PagerView();
            this.panelSearchWidget = new System.Windows.Forms.Panel();
            this.searchView1 = new FrontWork.SearchView();
            this.configuration1 = new FrontWork.Configuration();
            this.panel1 = new System.Windows.Forms.Panel();
            this.basicView1 = new FrontWork.BasicView();
            this.model1 = new FrontWork.Model();
            this.panel2 = new System.Windows.Forms.Panel();
            this.synchronizer = new FrontWork.JsonRESTSynchronizer();
            this.pagerSearchJsonRESTAdapter1 = new FrontWork.PagerSearchJsonRESTAdapter();
            this.reoGridView1 = new FrontWork.ReoGridView();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAlter = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripCarryOver = new System.Windows.Forms.ToolStripButton();
            this.miniToolStrip = new System.Windows.Forms.ToolStrip();
            this.tableLayoutPanel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panelPager.SuspendLayout();
            this.panelSearchWidget.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.toolStripTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.toolStrip1, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.panelPager, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.toolStripTop, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panelSearchWidget, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(925, 525);
            this.tableLayoutPanel2.TabIndex = 18;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.labelStatus});
            this.toolStrip1.Location = new System.Drawing.Point(0, 494);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(925, 31);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(43, 28);
            this.toolStripStatusLabel1.Text = "状态:";
            // 
            // labelStatus
            // 
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(69, 28);
            this.labelStatus.Text = "税务信息";
            // 
            // panelPager
            // 
            this.panelPager.Controls.Add(this.pagerView1);
            this.panelPager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPager.Location = new System.Drawing.Point(0, 456);
            this.panelPager.Margin = new System.Windows.Forms.Padding(0);
            this.panelPager.Name = "panelPager";
            this.panelPager.Size = new System.Drawing.Size(925, 38);
            this.panelPager.TabIndex = 5;
            // 
            // pagerView1
            // 
            this.pagerView1.BackColor = System.Drawing.SystemColors.Control;
            this.pagerView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pagerView1.Location = new System.Drawing.Point(0, 0);
            this.pagerView1.Margin = new System.Windows.Forms.Padding(0);
            this.pagerView1.Mode = "default";
            this.pagerView1.Name = "pagerView1";
            this.pagerView1.PageSize = 50;
            this.pagerView1.Size = new System.Drawing.Size(925, 38);
            this.pagerView1.TabIndex = 16;
            // 
            // panelSearchWidget
            // 
            this.panelSearchWidget.Controls.Add(this.searchView1);
            this.panelSearchWidget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSearchWidget.Location = new System.Drawing.Point(0, 0);
            this.panelSearchWidget.Margin = new System.Windows.Forms.Padding(0);
            this.panelSearchWidget.Name = "panelSearchWidget";
            this.panelSearchWidget.Size = new System.Drawing.Size(925, 31);
            this.panelSearchWidget.TabIndex = 6;
            // 
            // searchView1
            // 
            this.searchView1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.searchView1.Configuration = this.configuration1;
            this.searchView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchView1.Location = new System.Drawing.Point(0, 0);
            this.searchView1.Margin = new System.Windows.Forms.Padding(0);
            this.searchView1.Mode = "default";
            this.searchView1.Name = "searchView1";
            this.searchView1.Size = new System.Drawing.Size(925, 31);
            this.searchView1.TabIndex = 2;
            // 
            // configuration1
            // 
            this.configuration1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configuration1.ConfigurationString = resources.GetString("configuration1.ConfigurationString");
            this.configuration1.Location = new System.Drawing.Point(135, 95);
            this.configuration1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            modeMethodListenerNamesPair1.MethodListenerNames = new string[] {
        "FormAccountPeriod",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair1.Mode = "default";
            this.configuration1.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[] {
        modeMethodListenerNamesPair1};
            this.configuration1.Name = "configuration1";
            this.configuration1.Size = new System.Drawing.Size(180, 180);
            this.configuration1.TabIndex = 11;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.basicView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(925, 95);
            this.panel1.TabIndex = 7;
            // 
            // basicView1
            // 
            this.basicView1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.basicView1.Configuration = this.configuration1;
            this.basicView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicView1.Font = new System.Drawing.Font("黑体", 10F);
            this.basicView1.ItemsPerRow = 3;
            this.basicView1.Location = new System.Drawing.Point(0, 0);
            this.basicView1.Margin = new System.Windows.Forms.Padding(0);
            this.basicView1.Mode = "default";
            this.basicView1.Model = this.model1;
            this.basicView1.Name = "basicView1";
            this.basicView1.Size = new System.Drawing.Size(925, 95);
            this.basicView1.TabIndex = 16;
            // 
            // model1
            // 
            this.model1.AllSelectionRanges = new FrontWork.Range[0];
            this.model1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.model1.Configuration = this.configuration1;
            this.model1.Font = new System.Drawing.Font("宋体", 10F);
            this.model1.Location = new System.Drawing.Point(332, 118);
            this.model1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.model1.Mode = "default";
            this.model1.Name = "model1";
            this.model1.SelectionRange = null;
            this.model1.Size = new System.Drawing.Size(145, 145);
            this.model1.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.configuration1);
            this.panel2.Controls.Add(this.synchronizer);
            this.panel2.Controls.Add(this.pagerSearchJsonRESTAdapter1);
            this.panel2.Controls.Add(this.model1);
            this.panel2.Controls.Add(this.reoGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 157);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(925, 299);
            this.panel2.TabIndex = 8;
            // 
            // synchronizer
            // 
            this.synchronizer.Configuration = this.configuration1;
            this.synchronizer.FieldMapping = new FrontWork.JsonRESTSynchronizer.FieldMappingItem[0];
            this.synchronizer.Location = new System.Drawing.Point(500, 95);
            this.synchronizer.Margin = new System.Windows.Forms.Padding(0);
            this.synchronizer.Mode = "default";
            this.synchronizer.Model = this.model1;
            this.synchronizer.Name = "synchronizer";
            this.synchronizer.Size = new System.Drawing.Size(180, 180);
            this.synchronizer.TabIndex = 10;
            // 
            // pagerSearchJsonRESTAdapter1
            // 
            conditionFieldNamesType1.Key = "key";
            conditionFieldNamesType1.Relation = "relation";
            conditionFieldNamesType1.Values = "values";
            apiParamNamesType1.ConditionParamNames = conditionFieldNamesType1;
            orderParamNamesType1.Key = "key";
            orderParamNamesType1.Order = "order";
            apiParamNamesType1.OrderParamNames = orderParamNamesType1;
            this.pagerSearchJsonRESTAdapter1.APIFieldNames = apiParamNamesType1;
            this.pagerSearchJsonRESTAdapter1.Location = new System.Drawing.Point(672, 115);
            this.pagerSearchJsonRESTAdapter1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pagerSearchJsonRESTAdapter1.Name = "pagerSearchJsonRESTAdapter1";
            this.pagerSearchJsonRESTAdapter1.PagerView = this.pagerView1;
            this.pagerSearchJsonRESTAdapter1.SearchView = this.searchView1;
            this.pagerSearchJsonRESTAdapter1.Size = new System.Drawing.Size(140, 122);
            this.pagerSearchJsonRESTAdapter1.Synchronizer = this.synchronizer;
            this.pagerSearchJsonRESTAdapter1.TabIndex = 9;
            this.pagerSearchJsonRESTAdapter1.Visible = false;
            // 
            // reoGridView1
            // 
            this.reoGridView1.Configuration = this.configuration1;
            this.reoGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGridView1.Font = new System.Drawing.Font("黑体", 11F);
            this.reoGridView1.Location = new System.Drawing.Point(0, 0);
            this.reoGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.reoGridView1.Mode = "default";
            this.reoGridView1.Model = this.model1;
            this.reoGridView1.Name = "reoGridView1";
            this.reoGridView1.Size = new System.Drawing.Size(925, 299);
            this.reoGridView1.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(925, 525);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // toolStripTop
            // 
            this.toolStripTop.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStripTop.BackgroundImage = global::WMS.UI.Properties.Resources.bottonW_q;
            this.toolStripTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStripTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripTop.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.toolStripButtonAdd,
            this.toolStripButtonAlter,
            this.toolStripSeparator3,
            this.toolStripCarryOver});
            this.toolStripTop.Location = new System.Drawing.Point(0, 31);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Size = new System.Drawing.Size(925, 31);
            this.toolStripTop.TabIndex = 2;
            this.toolStripTop.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AutoSize = false;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(15, 28);
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAdd.Image")));
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(63, 28);
            this.toolStripButtonAdd.Text = "添加";
            this.toolStripButtonAdd.Visible = false;
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonAlter
            // 
            this.toolStripButtonAlter.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAlter.Image")));
            this.toolStripButtonAlter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAlter.Name = "toolStripButtonAlter";
            this.toolStripButtonAlter.Size = new System.Drawing.Size(93, 28);
            this.toolStripButtonAlter.Text = "保存修改";
            this.toolStripButtonAlter.Click += new System.EventHandler(this.toolStripButtonAlter_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AutoSize = false;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(15, 28);
            // 
            // toolStripCarryOver
            // 
            this.toolStripCarryOver.Image = global::WMS.UI.Properties.Resources.cancle;
            this.toolStripCarryOver.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripCarryOver.Name = "toolStripCarryOver";
            this.toolStripCarryOver.Size = new System.Drawing.Size(93, 28);
            this.toolStripCarryOver.Text = "期末结转";
            this.toolStripCarryOver.Click += new System.EventHandler(this.toolStripCarryOver_Click);
            // 
            // miniToolStrip
            // 
            this.miniToolStrip.AutoSize = false;
            this.miniToolStrip.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.miniToolStrip.BackgroundImage = global::WMS.UI.Properties.Resources.bottonW_q;
            this.miniToolStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.miniToolStrip.CanOverflow = false;
            this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.miniToolStrip.Location = new System.Drawing.Point(0, 0);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.Size = new System.Drawing.Size(694, 25);
            this.miniToolStrip.TabIndex = 2;
            // 
            // FormAccountPeriod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 525);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormAccountPeriod";
            this.Text = "FormAccountPeriod";
            this.Load += new System.EventHandler(this.FormAccountPeriod_Load);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panelPager.ResumeLayout(false);
            this.panelSearchWidget.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Drawing.Printing.PrintDocument printDocument2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripLabel labelStatus;
        private System.Windows.Forms.Panel panelPager;
        private FrontWork.PagerView pagerView1;
        private System.Windows.Forms.ToolStrip toolStripTop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonAlter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.Panel panelSearchWidget;
        private FrontWork.SearchView searchView1;
        private System.Windows.Forms.Panel panel1;
        private FrontWork.BasicView basicView1;
        private FrontWork.Model model1;
        private System.Windows.Forms.Panel panel2;
        private FrontWork.JsonRESTSynchronizer synchronizer;
        private FrontWork.PagerSearchJsonRESTAdapter pagerSearchJsonRESTAdapter1;
        private FrontWork.ReoGridView reoGridView1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private FrontWork.Configuration configuration1;
        private System.Windows.Forms.ToolStrip miniToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripCarryOver;
    }
}
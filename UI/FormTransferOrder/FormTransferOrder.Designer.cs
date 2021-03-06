﻿namespace WMS.UI.FormTransferOrder
{
    partial class FormTransferOrder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTransferOrder));
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair1 = new FrontWork.ModeMethodListenerNamesPair();
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair2 = new FrontWork.ModeMethodListenerNamesPair();
            FrontWork.SearchViewJsonRESTAdapter.APIParamNamesType apiParamNamesType1 = new FrontWork.SearchViewJsonRESTAdapter.APIParamNamesType();
            FrontWork.SearchViewJsonRESTAdapter.ConditionFieldNamesType conditionFieldNamesType1 = new FrontWork.SearchViewJsonRESTAdapter.ConditionFieldNamesType();
            FrontWork.SearchViewJsonRESTAdapter.OrderParamNamesType orderParamNamesType1 = new FrontWork.SearchViewJsonRESTAdapter.OrderParamNamesType();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelSearchWidget = new System.Windows.Forms.Panel();
            this.searchView1 = new FrontWork.SearchView();
            this.configuration1 = new FrontWork.Configuration();
            this.panelPagerWidget = new System.Windows.Forms.Panel();
            this.pagerView1 = new FrontWork.PagerView();
            this.miniToolStrip = new System.Windows.Forms.ToolStrip();
            this.buttonOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonAlter = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.toolStripAutoTransfer = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonDeliveyPakage = new System.Windows.Forms.ToolStripButton();
            this.buttonPreview = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.basicView1 = new FrontWork.BasicView();
            this.model1 = new FrontWork.Model();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pagerSearchJsonRESTAdapter1 = new FrontWork.PagerSearchJsonRESTAdapter();
            this.synchronizer = new FrontWork.JsonRESTSynchronizer();
            this.reoGridView1 = new FrontWork.ReoGridView();
            this.reoGridView2 = new FrontWork.ReoGridView();
            this.statusStrip1.SuspendLayout();
            this.panelSearchWidget.SuspendLayout();
            this.panelPagerWidget.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStripTop.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.labelStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 534);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 9, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1136, 25);
            this.statusStrip1.TabIndex = 9;
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
            this.labelStatus.Size = new System.Drawing.Size(114, 20);
            this.labelStatus.Text = "移库作业单信息";
            // 
            // panelSearchWidget
            // 
            this.panelSearchWidget.Controls.Add(this.searchView1);
            this.panelSearchWidget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSearchWidget.Location = new System.Drawing.Point(0, 0);
            this.panelSearchWidget.Margin = new System.Windows.Forms.Padding(0);
            this.panelSearchWidget.Name = "panelSearchWidget";
            this.panelSearchWidget.Size = new System.Drawing.Size(1136, 31);
            this.panelSearchWidget.TabIndex = 8;
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
            this.searchView1.Size = new System.Drawing.Size(1136, 31);
            this.searchView1.TabIndex = 13;
            // 
            // configuration1
            // 
            this.configuration1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configuration1.ConfigurationString = resources.GetString("configuration1.ConfigurationString");
            this.configuration1.Location = new System.Drawing.Point(183, 64);
            this.configuration1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            modeMethodListenerNamesPair1.MethodListenerNames = new string[] {
        "FormTransferOrder",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair1.Mode = "default";
            modeMethodListenerNamesPair2.MethodListenerNames = new string[] {
        "FormTransferOrder",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair2.Mode = "supplier-not-editable";
            this.configuration1.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[] {
        modeMethodListenerNamesPair1,
        modeMethodListenerNamesPair2};
            this.configuration1.Name = "configuration1";
            this.configuration1.Size = new System.Drawing.Size(240, 225);
            this.configuration1.TabIndex = 11;
            // 
            // panelPagerWidget
            // 
            this.panelPagerWidget.Controls.Add(this.pagerView1);
            this.panelPagerWidget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPagerWidget.Location = new System.Drawing.Point(3, 492);
            this.panelPagerWidget.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelPagerWidget.Name = "panelPagerWidget";
            this.panelPagerWidget.Size = new System.Drawing.Size(1130, 34);
            this.panelPagerWidget.TabIndex = 7;
            // 
            // pagerView1
            // 
            this.pagerView1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pagerView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pagerView1.Location = new System.Drawing.Point(0, 0);
            this.pagerView1.Margin = new System.Windows.Forms.Padding(0);
            this.pagerView1.Mode = "default";
            this.pagerView1.Name = "pagerView1";
            this.pagerView1.PageSize = 50;
            this.pagerView1.Size = new System.Drawing.Size(1130, 34);
            this.pagerView1.TabIndex = 14;
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
            this.miniToolStrip.Location = new System.Drawing.Point(457, 1);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.Padding = new System.Windows.Forms.Padding(0);
            this.miniToolStrip.Size = new System.Drawing.Size(852, 22);
            this.miniToolStrip.TabIndex = 5;
            // 
            // buttonOpen
            // 
            this.buttonOpen.Image = ((System.Drawing.Image)(resources.GetObject("buttonOpen.Image")));
            this.buttonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(168, 28);
            this.buttonOpen.Text = "查看备货作业单条目";
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AutoSize = false;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(15, 28);
            // 
            // buttonAlter
            // 
            this.buttonAlter.Image = ((System.Drawing.Image)(resources.GetObject("buttonAlter.Image")));
            this.buttonAlter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonAlter.Name = "buttonAlter";
            this.buttonAlter.Size = new System.Drawing.Size(93, 28);
            this.buttonAlter.Text = "保存修改";
            this.buttonAlter.Click += new System.EventHandler(this.buttonAlter_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AutoSize = false;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(15, 28);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.toolStripTop, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelPagerWidget, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.panelSearchWidget, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1136, 559);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // toolStripTop
            // 
            this.toolStripTop.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStripTop.BackgroundImage = global::WMS.UI.Properties.Resources.bottonW_q;
            this.toolStripTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStripTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripTop.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonOpen,
            this.toolStripSeparator2,
            this.toolStripAutoTransfer,
            this.toolStripButtonAdd,
            this.toolStripButtonDelect,
            this.buttonAlter,
            this.toolStripSeparator1,
            this.toolStripButtonDeliveyPakage,
            this.toolStripSeparator3,
            this.buttonPreview});
            this.toolStripTop.Location = new System.Drawing.Point(0, 31);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Padding = new System.Windows.Forms.Padding(0);
            this.toolStripTop.Size = new System.Drawing.Size(1136, 31);
            this.toolStripTop.TabIndex = 5;
            this.toolStripTop.Text = "toolStrip1";
            // 
            // toolStripAutoTransfer
            // 
            this.toolStripAutoTransfer.Image = ((System.Drawing.Image)(resources.GetObject("toolStripAutoTransfer.Image")));
            this.toolStripAutoTransfer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAutoTransfer.Name = "toolStripAutoTransfer";
            this.toolStripAutoTransfer.Size = new System.Drawing.Size(93, 28);
            this.toolStripAutoTransfer.Text = "一键备货";
            this.toolStripAutoTransfer.ToolTipText = "一键备货";
            this.toolStripAutoTransfer.Click += new System.EventHandler(this.toolStripAutoTransfer_Click);
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAdd.Image")));
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(63, 28);
            this.toolStripButtonAdd.Text = "添加";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonDelect
            // 
            this.toolStripButtonDelect.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDelect.Image")));
            this.toolStripButtonDelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelect.Name = "toolStripButtonDelect";
            this.toolStripButtonDelect.Size = new System.Drawing.Size(63, 28);
            this.toolStripButtonDelect.Text = "删除";
            this.toolStripButtonDelect.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AutoSize = false;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(15, 28);
            // 
            // toolStripButtonDeliveyPakage
            // 
            this.toolStripButtonDeliveyPakage.Image = global::WMS.UI.Properties.Resources.cancle;
            this.toolStripButtonDeliveyPakage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDeliveyPakage.Name = "toolStripButtonDeliveyPakage";
            this.toolStripButtonDeliveyPakage.Size = new System.Drawing.Size(123, 28);
            this.toolStripButtonDeliveyPakage.Text = "按备货单发货";
            this.toolStripButtonDeliveyPakage.Click += new System.EventHandler(this.toolStripButtonDeliveyPakage_Click);
            // 
            // buttonPreview
            // 
            this.buttonPreview.Image = ((System.Drawing.Image)(resources.GetObject("buttonPreview.Image")));
            this.buttonPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Size = new System.Drawing.Size(99, 28);
            this.buttonPreview.Text = "导出/打印";
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.basicView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1136, 94);
            this.panel1.TabIndex = 9;
            // 
            // basicView1
            // 
            this.basicView1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.basicView1.Configuration = this.configuration1;
            this.basicView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicView1.Font = new System.Drawing.Font("黑体", 10F);
            this.basicView1.ItemsPerRow = 5;
            this.basicView1.Location = new System.Drawing.Point(0, 0);
            this.basicView1.Margin = new System.Windows.Forms.Padding(0);
            this.basicView1.Mode = "default";
            this.basicView1.Model = this.model1;
            this.basicView1.Name = "basicView1";
            this.basicView1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.basicView1.Size = new System.Drawing.Size(1136, 94);
            this.basicView1.TabIndex = 16;
            // 
            // model1
            // 
            this.model1.AllSelectionRanges = new FrontWork.Range[0];
            this.model1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.model1.Configuration = this.configuration1;
            this.model1.Font = new System.Drawing.Font("宋体", 10F);
            this.model1.Location = new System.Drawing.Point(428, 56);
            this.model1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.model1.Mode = "default";
            this.model1.Name = "model1";
            this.model1.SelectionRange = null;
            this.model1.Size = new System.Drawing.Size(240, 225);
            this.model1.TabIndex = 12;
            this.model1.Refreshed += new System.EventHandler<FrontWork.ModelRefreshedEventArgs>(this.model1_Refreshed);
            this.model1.SelectionRangeChanged += new System.EventHandler<FrontWork.ModelSelectionRangeChangedEventArgs>(this.model1_SelectionRangeChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pagerSearchJsonRESTAdapter1);
            this.panel2.Controls.Add(this.synchronizer);
            this.panel2.Controls.Add(this.model1);
            this.panel2.Controls.Add(this.configuration1);
            this.panel2.Controls.Add(this.reoGridView1);
            this.panel2.Controls.Add(this.reoGridView2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 156);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1136, 334);
            this.panel2.TabIndex = 10;
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
            this.pagerSearchJsonRESTAdapter1.Location = new System.Drawing.Point(851, 89);
            this.pagerSearchJsonRESTAdapter1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pagerSearchJsonRESTAdapter1.Name = "pagerSearchJsonRESTAdapter1";
            this.pagerSearchJsonRESTAdapter1.PagerView = this.pagerView1;
            this.pagerSearchJsonRESTAdapter1.SearchView = this.searchView1;
            this.pagerSearchJsonRESTAdapter1.Size = new System.Drawing.Size(120, 112);
            this.pagerSearchJsonRESTAdapter1.Synchronizer = this.synchronizer;
            this.pagerSearchJsonRESTAdapter1.TabIndex = 14;
            this.pagerSearchJsonRESTAdapter1.Visible = false;
            // 
            // synchronizer
            // 
            this.synchronizer.Configuration = this.configuration1;
            this.synchronizer.FieldMapping = new FrontWork.JsonRESTSynchronizer.FieldMappingItem[0];
            this.synchronizer.Location = new System.Drawing.Point(628, 64);
            this.synchronizer.Margin = new System.Windows.Forms.Padding(0);
            this.synchronizer.Mode = "default";
            this.synchronizer.Model = this.model1;
            this.synchronizer.Name = "synchronizer";
            this.synchronizer.Size = new System.Drawing.Size(240, 225);
            this.synchronizer.TabIndex = 13;
            // 
            // reoGridView1
            // 
            this.reoGridView1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.reoGridView1.Configuration = this.configuration1;
            this.reoGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGridView1.Font = new System.Drawing.Font("黑体", 11F);
            this.reoGridView1.Location = new System.Drawing.Point(0, 0);
            this.reoGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.reoGridView1.Mode = "default";
            this.reoGridView1.Model = this.model1;
            this.reoGridView1.Name = "reoGridView1";
            this.reoGridView1.Size = new System.Drawing.Size(1136, 334);
            this.reoGridView1.TabIndex = 10;
            // 
            // reoGridView2
            // 
            this.reoGridView2.Configuration = null;
            this.reoGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGridView2.Font = new System.Drawing.Font("黑体", 11F);
            this.reoGridView2.Location = new System.Drawing.Point(0, 0);
            this.reoGridView2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.reoGridView2.Mode = "default";
            this.reoGridView2.Model = null;
            this.reoGridView2.Name = "reoGridView2";
            this.reoGridView2.Size = new System.Drawing.Size(1136, 334);
            this.reoGridView2.TabIndex = 6;
            // 
            // FormTransferOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1136, 559);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormTransferOrder";
            this.Text = "FormTransferOrder";
            this.Load += new System.EventHandler(this.FormTransferOrder_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panelSearchWidget.ResumeLayout(false);
            this.panelPagerWidget.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel labelStatus;
        private System.Windows.Forms.Panel panelSearchWidget;
        private System.Windows.Forms.Panel panelPagerWidget;
        private System.Windows.Forms.ToolStrip miniToolStrip;
        private System.Windows.Forms.ToolStripButton buttonOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton buttonAlter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStrip toolStripTop;
        private FrontWork.SearchView searchView1;
        private System.Windows.Forms.Panel panel1;
        private FrontWork.BasicView basicView1;
        private System.Windows.Forms.Panel panel2;
        private FrontWork.ReoGridView reoGridView2;
        private FrontWork.PagerView pagerView1;
        private FrontWork.Configuration configuration1;
        private FrontWork.Model model1;
        private FrontWork.PagerSearchJsonRESTAdapter pagerSearchJsonRESTAdapter1;
        private FrontWork.JsonRESTSynchronizer synchronizer;
        private FrontWork.ReoGridView reoGridView1;
        private System.Windows.Forms.ToolStripButton buttonPreview;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripButton toolStripAutoTransfer;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonDeliveyPakage;
    }
}
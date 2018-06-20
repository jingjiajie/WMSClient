﻿namespace WMS.UI.FormBasicInfos
{
    partial class FormSupply
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSupply));
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair2 = new FrontWork.ModeMethodListenerNamesPair();
            FrontWork.SearchViewJsonRESTAdapter.APIParamNamesType apiParamNamesType2 = new FrontWork.SearchViewJsonRESTAdapter.APIParamNamesType();
            FrontWork.SearchViewJsonRESTAdapter.ConditionFieldNamesType conditionFieldNamesType2 = new FrontWork.SearchViewJsonRESTAdapter.ConditionFieldNamesType();
            FrontWork.SearchViewJsonRESTAdapter.OrderParamNamesType orderParamNamesType2 = new FrontWork.SearchViewJsonRESTAdapter.OrderParamNamesType();
            this.panelSearchWidget = new System.Windows.Forms.Panel();
            this.searchView1 = new FrontWork.SearchView();
            this.configuration1 = new FrontWork.Configuration();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSupplyShipmentInfo = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSupplyOuterPackingSize = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSupplySingleBoxTranPackingInfo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAlter = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.buttonImport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.labelStatus = new System.Windows.Forms.ToolStripLabel();
            this.panelPager = new System.Windows.Forms.Panel();
            this.pagerView1 = new FrontWork.PagerView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pagerSearchJsonRESTAdapter1 = new FrontWork.PagerSearchJsonRESTAdapter();
            this.synchronizer = new FrontWork.JsonRESTSynchronizer();
            this.model1 = new FrontWork.Model();
            this.reoGridView1 = new FrontWork.ReoGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.basicView1 = new FrontWork.BasicView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.panelSearchWidget.SuspendLayout();
            this.toolStripTop.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panelPager.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSearchWidget
            // 
            this.panelSearchWidget.Controls.Add(this.searchView1);
            this.panelSearchWidget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSearchWidget.Location = new System.Drawing.Point(0, 0);
            this.panelSearchWidget.Margin = new System.Windows.Forms.Padding(0);
            this.panelSearchWidget.Name = "panelSearchWidget";
            this.panelSearchWidget.Size = new System.Drawing.Size(1381, 38);
            this.panelSearchWidget.TabIndex = 6;
            // 
            // searchView1
            // 
            this.searchView1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.searchView1.Configuration = this.configuration1;
            this.searchView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchView1.Location = new System.Drawing.Point(0, 0);
            this.searchView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.searchView1.Mode = "default";
            this.searchView1.Name = "searchView1";
            this.searchView1.Size = new System.Drawing.Size(1381, 38);
            this.searchView1.TabIndex = 2;
            // 
            // configuration1
            // 
            this.configuration1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configuration1.ConfigurationString = resources.GetString("configuration1.ConfigurationString");
            this.configuration1.Location = new System.Drawing.Point(125, -4);
            this.configuration1.Margin = new System.Windows.Forms.Padding(2);
            modeMethodListenerNamesPair2.MethodListenerNames = new string[] {
        "FormSupply",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair2.Mode = "default";
            this.configuration1.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[] {
        modeMethodListenerNamesPair2};
            this.configuration1.Name = "configuration1";
            this.configuration1.Size = new System.Drawing.Size(210, 195);
            this.configuration1.TabIndex = 3;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AutoSize = false;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(15, 28);
            // 
            // toolStripButtonSupplyShipmentInfo
            // 
            this.toolStripButtonSupplyShipmentInfo.Image = global::WMS.UI.Properties.Resources.find;
            this.toolStripButtonSupplyShipmentInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSupplyShipmentInfo.Name = "toolStripButtonSupplyShipmentInfo";
            this.toolStripButtonSupplyShipmentInfo.Size = new System.Drawing.Size(133, 35);
            this.toolStripButtonSupplyShipmentInfo.Text = "查看/修改出货信息";
            this.toolStripButtonSupplyShipmentInfo.Click += new System.EventHandler(this.toolStripButtonSupplyShipmentInfo_Click);
            // 
            // toolStripButtonSupplyOuterPackingSize
            // 
            this.toolStripButtonSupplyOuterPackingSize.Image = global::WMS.UI.Properties.Resources.find;
            this.toolStripButtonSupplyOuterPackingSize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSupplyOuterPackingSize.Name = "toolStripButtonSupplyOuterPackingSize";
            this.toolStripButtonSupplyOuterPackingSize.Size = new System.Drawing.Size(169, 35);
            this.toolStripButtonSupplyOuterPackingSize.Text = "查看/修改零件外包装信息";
            this.toolStripButtonSupplyOuterPackingSize.Click += new System.EventHandler(this.toolStripButtonSupplyOuterPackingSize_Click);
            // 
            // toolStripButtonSupplySingleBoxTranPackingInfo
            // 
            this.toolStripButtonSupplySingleBoxTranPackingInfo.Image = global::WMS.UI.Properties.Resources.find;
            this.toolStripButtonSupplySingleBoxTranPackingInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSupplySingleBoxTranPackingInfo.Name = "toolStripButtonSupplySingleBoxTranPackingInfo";
            this.toolStripButtonSupplySingleBoxTranPackingInfo.Size = new System.Drawing.Size(157, 35);
            this.toolStripButtonSupplySingleBoxTranPackingInfo.Text = "查看/修改单箱包装信息";
            this.toolStripButtonSupplySingleBoxTranPackingInfo.Click += new System.EventHandler(this.toolStripButtonSupplySingleBoxTranPackingInfo_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AutoSize = false;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(15, 28);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.Image = global::WMS.UI.Properties.Resources.delete;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(56, 35);
            this.toolStripButtonDelete.Text = "删除";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripButtonAlter
            // 
            this.toolStripButtonAlter.Image = global::WMS.UI.Properties.Resources.cancle;
            this.toolStripButtonAlter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAlter.Name = "toolStripButtonAlter";
            this.toolStripButtonAlter.Size = new System.Drawing.Size(80, 35);
            this.toolStripButtonAlter.Text = "保存修改";
            this.toolStripButtonAlter.Click += new System.EventHandler(this.toolStripButtonAlter_Click);
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.Image = global::WMS.UI.Properties.Resources.add;
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(56, 35);
            this.toolStripButtonAdd.Text = "添加";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // buttonImport
            // 
            this.buttonImport.Image = global::WMS.UI.Properties.Resources.add;
            this.buttonImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(80, 35);
            this.buttonImport.Text = "批量导入";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AutoSize = false;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(15, 28);
            // 
            // toolStripTop
            // 
            this.toolStripTop.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStripTop.BackgroundImage = global::WMS.UI.Properties.Resources.bottonW_q;
            this.toolStripTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStripTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripTop.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStripTop.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2,
            this.toolStripButtonAdd,
            this.toolStripButtonDelete,
            this.toolStripButtonAlter,
            this.toolStripSeparator3,
            this.toolStripButtonSupplySingleBoxTranPackingInfo,
            this.toolStripButtonSupplyOuterPackingSize,
            this.toolStripButtonSupplyShipmentInfo,
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.buttonImport});
            this.toolStripTop.Location = new System.Drawing.Point(0, 38);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Size = new System.Drawing.Size(1381, 38);
            this.toolStripTop.TabIndex = 2;
            this.toolStripTop.Text = "toolStrip1";
            this.toolStripTop.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStripTop_ItemClicked);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::WMS.UI.Properties.Resources.find;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(104, 35);
            this.toolStripButton1.Text = "查看概要信息";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.toolStrip1, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.panelPager, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.toolStripTop, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panelSearchWidget, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 228F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1381, 824);
            this.tableLayoutPanel2.TabIndex = 8;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.labelStatus});
            this.toolStrip1.Location = new System.Drawing.Point(0, 786);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1381, 38);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(35, 35);
            this.toolStripStatusLabel1.Text = "状态:";
            // 
            // labelStatus
            // 
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(56, 35);
            this.labelStatus.Text = "零件信息";
            // 
            // panelPager
            // 
            this.panelPager.Controls.Add(this.pagerView1);
            this.panelPager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPager.Location = new System.Drawing.Point(3, 751);
            this.panelPager.Name = "panelPager";
            this.panelPager.Size = new System.Drawing.Size(1375, 32);
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
            this.pagerView1.Size = new System.Drawing.Size(1375, 32);
            this.pagerView1.TabIndex = 16;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pagerSearchJsonRESTAdapter1);
            this.panel1.Controls.Add(this.synchronizer);
            this.panel1.Controls.Add(this.model1);
            this.panel1.Controls.Add(this.configuration1);
            this.panel1.Controls.Add(this.reoGridView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 304);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1381, 444);
            this.panel1.TabIndex = 7;
            // 
            // pagerSearchJsonRESTAdapter1
            // 
            conditionFieldNamesType2.Key = "key";
            conditionFieldNamesType2.Relation = "relation";
            conditionFieldNamesType2.Values = "values";
            apiParamNamesType2.ConditionParamNames = conditionFieldNamesType2;
            orderParamNamesType2.Key = "key";
            orderParamNamesType2.Order = "order";
            apiParamNamesType2.OrderParamNames = orderParamNamesType2;
            this.pagerSearchJsonRESTAdapter1.APIFieldNames = apiParamNamesType2;
            this.pagerSearchJsonRESTAdapter1.Location = new System.Drawing.Point(761, 13);
            this.pagerSearchJsonRESTAdapter1.Margin = new System.Windows.Forms.Padding(2);
            this.pagerSearchJsonRESTAdapter1.Name = "pagerSearchJsonRESTAdapter1";
            this.pagerSearchJsonRESTAdapter1.PagerView = this.pagerView1;
            this.pagerSearchJsonRESTAdapter1.SearchView = this.searchView1;
            this.pagerSearchJsonRESTAdapter1.Size = new System.Drawing.Size(96, 74);
            this.pagerSearchJsonRESTAdapter1.Synchronizer = this.synchronizer;
            this.pagerSearchJsonRESTAdapter1.TabIndex = 6;
            this.pagerSearchJsonRESTAdapter1.Visible = false;
            // 
            // synchronizer
            // 
            this.synchronizer.Configuration = this.configuration1;
            this.synchronizer.FieldMapping = new FrontWork.JsonRESTSynchronizer.FieldMappingItem[0];
            this.synchronizer.Location = new System.Drawing.Point(545, 13);
            this.synchronizer.Margin = new System.Windows.Forms.Padding(0);
            this.synchronizer.Mode = "default";
            this.synchronizer.Model = this.model1;
            this.synchronizer.Name = "synchronizer";
            this.synchronizer.Size = new System.Drawing.Size(210, 195);
            this.synchronizer.TabIndex = 5;
            // 
            // model1
            // 
            this.model1.AllSelectionRanges = new FrontWork.Range[0];
            this.model1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.model1.Configuration = this.configuration1;
            this.model1.Font = new System.Drawing.Font("宋体", 10F);
            this.model1.Location = new System.Drawing.Point(302, 13);
            this.model1.Margin = new System.Windows.Forms.Padding(2);
            this.model1.Mode = "default";
            this.model1.Name = "model1";
            this.model1.SelectionRange = null;
            this.model1.Size = new System.Drawing.Size(210, 195);
            this.model1.TabIndex = 4;
            // 
            // reoGridView1
            // 
            this.reoGridView1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.reoGridView1.Configuration = this.configuration1;
            this.reoGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGridView1.Font = new System.Drawing.Font("黑体", 11F);
            this.reoGridView1.Location = new System.Drawing.Point(0, 0);
            this.reoGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.reoGridView1.Mode = "default";
            this.reoGridView1.Model = this.model1;
            this.reoGridView1.Name = "reoGridView1";
            this.reoGridView1.Size = new System.Drawing.Size(1381, 444);
            this.reoGridView1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.basicView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 76);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1381, 228);
            this.panel2.TabIndex = 8;
            // 
            // basicView1
            // 
            this.basicView1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.basicView1.Configuration = this.configuration1;
            this.basicView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicView1.Font = new System.Drawing.Font("宋体", 10F);
            this.basicView1.ItemsPerRow = 5;
            this.basicView1.Location = new System.Drawing.Point(0, 0);
            this.basicView1.Margin = new System.Windows.Forms.Padding(0);
            this.basicView1.Mode = "default";
            this.basicView1.Model = this.model1;
            this.basicView1.Name = "basicView1";
            this.basicView1.Size = new System.Drawing.Size(1381, 228);
            this.basicView1.TabIndex = 16;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1381, 824);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // FormSupply
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1381, 824);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("宋体", 10F);
            this.Name = "FormSupply";
            this.Text = "FormSupply";
            this.Load += new System.EventHandler(this.FormSupply_Load);
            this.panelSearchWidget.ResumeLayout(false);
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panelPager.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSearchWidget;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSupplyShipmentInfo;
        private System.Windows.Forms.ToolStripButton toolStripButtonSupplyOuterPackingSize;
        private System.Windows.Forms.ToolStripButton toolStripButtonSupplySingleBoxTranPackingInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonAlter;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripButton buttonImport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStrip toolStripTop;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripLabel labelStatus;
        private System.Windows.Forms.Panel panelPager;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private FrontWork.SearchView searchView1;
        private System.Windows.Forms.Panel panel1;
        private FrontWork.ReoGridView reoGridView1;
        private System.Windows.Forms.Panel panel2;
        private FrontWork.BasicView basicView1;
        private FrontWork.PagerView pagerView1;
        private FrontWork.Configuration configuration1;
        private FrontWork.Model model1;
        private FrontWork.JsonRESTSynchronizer synchronizer;
        private FrontWork.PagerSearchJsonRESTAdapter pagerSearchJsonRESTAdapter1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}
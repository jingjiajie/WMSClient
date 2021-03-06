﻿namespace WMS.UI.FromDeliverOrder
{
    partial class FormDeliveryOrderReady
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDeliveryOrderReady));
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair1 = new FrontWork.ModeMethodListenerNamesPair();
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair2 = new FrontWork.ModeMethodListenerNamesPair();
            FrontWork.JsonRESTSynchronizer.FieldMappingItem fieldMappingItem1 = new FrontWork.JsonRESTSynchronizer.FieldMappingItem();
            FrontWork.JsonRESTSynchronizer.FieldMappingItem fieldMappingItem2 = new FrontWork.JsonRESTSynchronizer.FieldMappingItem();
            FrontWork.JsonRESTSynchronizer.FieldMappingItem fieldMappingItem3 = new FrontWork.JsonRESTSynchronizer.FieldMappingItem();
            FrontWork.JsonRESTSynchronizer.FieldMappingItem fieldMappingItem4 = new FrontWork.JsonRESTSynchronizer.FieldMappingItem();
            FrontWork.JsonRESTSynchronizer.FieldMappingItem fieldMappingItem5 = new FrontWork.JsonRESTSynchronizer.FieldMappingItem();
            FrontWork.JsonRESTSynchronizer.FieldMappingItem fieldMappingItem6 = new FrontWork.JsonRESTSynchronizer.FieldMappingItem();
            FrontWork.JsonRESTSynchronizer.FieldMappingItem fieldMappingItem7 = new FrontWork.JsonRESTSynchronizer.FieldMappingItem();
            this.basicView2 = new FrontWork.BasicView();
            this.configurationTransferOrderItem = new FrontWork.Configuration();
            this.modelBoxTransferOrderItems = new FrontWork.ModelBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.basicView1 = new FrontWork.BasicView();
            this.configurationTransferOrder = new FrontWork.Configuration();
            this.modelTransferOrder = new FrontWork.Model();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.synchronizer = new FrontWork.JsonRESTSynchronizer();
            this.reoGridView1 = new FrontWork.ReoGridView();
            this.tabView1 = new FrontWork.TabView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // basicView2
            // 
            this.basicView2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.basicView2.Configuration = this.configurationTransferOrderItem;
            this.basicView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicView2.Font = new System.Drawing.Font("黑体", 10F);
            this.basicView2.ItemsPerRow = 4;
            this.basicView2.Location = new System.Drawing.Point(3, 19);
            this.basicView2.Margin = new System.Windows.Forms.Padding(0);
            this.basicView2.Mode = "default";
            this.basicView2.Model = this.modelBoxTransferOrderItems;
            this.basicView2.Name = "basicView2";
            this.basicView2.Size = new System.Drawing.Size(883, 115);
            this.basicView2.TabIndex = 4;
            // 
            // configurationTransferOrderItem
            // 
            this.configurationTransferOrderItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configurationTransferOrderItem.ConfigurationString = resources.GetString("configurationTransferOrderItem.ConfigurationString");
            this.configurationTransferOrderItem.Location = new System.Drawing.Point(320, 28);
            modeMethodListenerNamesPair1.MethodListenerNames = new string[] {
        "FormDeliveryOrderReady",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair1.Mode = "default";
            this.configurationTransferOrderItem.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[] {
        modeMethodListenerNamesPair1};
            this.configurationTransferOrderItem.Name = "configurationTransferOrderItem";
            this.configurationTransferOrderItem.Size = new System.Drawing.Size(180, 180);
            this.configurationTransferOrderItem.TabIndex = 5;
            // 
            // modelBoxTransferOrderItems
            // 
            this.modelBoxTransferOrderItems.AllSelectionRanges = new FrontWork.Range[0];
            this.modelBoxTransferOrderItems.Configuration = this.configurationTransferOrderItem;
            this.modelBoxTransferOrderItems.CurrentModelName = "default";
            this.modelBoxTransferOrderItems.Location = new System.Drawing.Point(22, 64);
            this.modelBoxTransferOrderItems.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.modelBoxTransferOrderItems.Mode = "default";
            this.modelBoxTransferOrderItems.Name = "modelBoxTransferOrderItems";
            this.modelBoxTransferOrderItems.SelectionRange = null;
            this.modelBoxTransferOrderItems.Size = new System.Drawing.Size(126, 131);
            this.modelBoxTransferOrderItems.TabIndex = 4;
            this.modelBoxTransferOrderItems.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.groupBox1.Controls.Add(this.basicView1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("黑体", 10F);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.groupBox1.Size = new System.Drawing.Size(889, 100);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "送检单信息";
            // 
            // basicView1
            // 
            this.basicView1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.basicView1.Configuration = this.configurationTransferOrder;
            this.basicView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicView1.Font = new System.Drawing.Font("黑体", 10F);
            this.basicView1.ItemsPerRow = 3;
            this.basicView1.Location = new System.Drawing.Point(0, 19);
            this.basicView1.Margin = new System.Windows.Forms.Padding(0);
            this.basicView1.Mode = "default";
            this.basicView1.Model = this.modelTransferOrder;
            this.basicView1.Name = "basicView1";
            this.basicView1.Padding = new System.Windows.Forms.Padding(6);
            this.basicView1.Size = new System.Drawing.Size(889, 81);
            this.basicView1.TabIndex = 0;
            // 
            // configurationTransferOrder
            // 
            this.configurationTransferOrder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configurationTransferOrder.ConfigurationString = resources.GetString("configurationTransferOrder.ConfigurationString");
            this.configurationTransferOrder.Location = new System.Drawing.Point(506, 49);
            modeMethodListenerNamesPair2.MethodListenerNames = new string[] {
        "FormDeliveryOrderReady",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair2.Mode = "default";
            this.configurationTransferOrder.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[] {
        modeMethodListenerNamesPair2};
            this.configurationTransferOrder.Name = "configurationTransferOrder";
            this.configurationTransferOrder.Size = new System.Drawing.Size(180, 180);
            this.configurationTransferOrder.TabIndex = 1;
            // 
            // modelTransferOrder
            // 
            this.modelTransferOrder.AllSelectionRanges = new FrontWork.Range[0];
            this.modelTransferOrder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.modelTransferOrder.Configuration = this.configurationTransferOrder;
            this.modelTransferOrder.Location = new System.Drawing.Point(692, 71);
            this.modelTransferOrder.Mode = "default";
            this.modelTransferOrder.Name = "modelTransferOrder";
            this.modelTransferOrder.SelectionRange = null;
            this.modelTransferOrder.Size = new System.Drawing.Size(180, 180);
            this.modelTransferOrder.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 400);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(889, 100);
            this.panel2.TabIndex = 3;
            // 
            // buttonOK
            // 
            this.buttonOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOK.Font = new System.Drawing.Font("黑体", 10F);
            this.buttonOK.Location = new System.Drawing.Point(369, 0);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(150, 35);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "确定备货";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.buttonOK, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 500);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(889, 40);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.groupBox2.Controls.Add(this.basicView2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("黑体", 10F);
            this.groupBox2.Location = new System.Drawing.Point(0, 28);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(889, 137);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "送检物料条目";
            // 
            // synchronizer
            // 
            this.synchronizer.Configuration = this.configurationTransferOrderItem;
            fieldMappingItem1.APIFieldName = "id";
            fieldMappingItem1.ModelFieldName = "warehouseEntryItemId";
            fieldMappingItem2.APIFieldName = "defaultInspectionStorageLocationId";
            fieldMappingItem2.ModelFieldName = "inspectionStorageLocationId";
            fieldMappingItem3.APIFieldName = "defaultInspectionStorageLocationNo";
            fieldMappingItem3.ModelFieldName = "inspectionStorageLocationNo";
            fieldMappingItem4.APIFieldName = "defaultInspectionStorageLocationName";
            fieldMappingItem4.ModelFieldName = "inspectionStorageLocationName";
            fieldMappingItem5.APIFieldName = "defaultInspectionAmount";
            fieldMappingItem5.ModelFieldName = "amount";
            fieldMappingItem6.APIFieldName = "defaultInspectionUnit";
            fieldMappingItem6.ModelFieldName = "unit";
            fieldMappingItem7.APIFieldName = "defaultInspectionUnitAmount";
            fieldMappingItem7.ModelFieldName = "unitAmount";
            this.synchronizer.FieldMapping = new FrontWork.JsonRESTSynchronizer.FieldMappingItem[] {
        fieldMappingItem1,
        fieldMappingItem2,
        fieldMappingItem3,
        fieldMappingItem4,
        fieldMappingItem5,
        fieldMappingItem6,
        fieldMappingItem7};
            this.synchronizer.Location = new System.Drawing.Point(150, 52);
            this.synchronizer.Margin = new System.Windows.Forms.Padding(0);
            this.synchronizer.Mode = "default";
            this.synchronizer.Model = this.modelBoxTransferOrderItems;
            this.synchronizer.Name = "synchronizer";
            this.synchronizer.Size = new System.Drawing.Size(180, 180);
            this.synchronizer.TabIndex = 6;
            // 
            // reoGridView1
            // 
            this.reoGridView1.Configuration = this.configurationTransferOrderItem;
            this.reoGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGridView1.Font = new System.Drawing.Font("黑体", 11F);
            this.reoGridView1.Location = new System.Drawing.Point(0, 0);
            this.reoGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.reoGridView1.Mode = "default";
            this.reoGridView1.Model = this.modelBoxTransferOrderItems;
            this.reoGridView1.Name = "reoGridView1";
            this.reoGridView1.Size = new System.Drawing.Size(889, 235);
            this.reoGridView1.TabIndex = 3;
            // 
            // tabView1
            // 
            this.tabView1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tabView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabView1.Font = new System.Drawing.Font("宋体", 10F);
            this.tabView1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabView1.Location = new System.Drawing.Point(0, 0);
            this.tabView1.Margin = new System.Windows.Forms.Padding(0);
            this.tabView1.ModelBox = this.modelBoxTransferOrderItems;
            this.tabView1.Name = "tabView1";
            this.tabView1.Size = new System.Drawing.Size(889, 25);
            this.tabView1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tabView1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(889, 540);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.configurationTransferOrder);
            this.panel1.Controls.Add(this.modelTransferOrder);
            this.panel1.Controls.Add(this.synchronizer);
            this.panel1.Controls.Add(this.configurationTransferOrderItem);
            this.panel1.Controls.Add(this.modelBoxTransferOrderItems);
            this.panel1.Controls.Add(this.reoGridView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 165);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(889, 235);
            this.panel1.TabIndex = 1;
            // 
            // FormDeliveryOrderReady
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 540);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormDeliveryOrderReady";
            this.Text = "FormDeliveryOrderReady";
            this.Load += new System.EventHandler(this.FormDeliveryOrderReady_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FrontWork.BasicView basicView2;
        private FrontWork.Configuration configurationTransferOrderItem;
        private FrontWork.ModelBox modelBoxTransferOrderItems;
        private System.Windows.Forms.GroupBox groupBox1;
        private FrontWork.BasicView basicView1;
        private FrontWork.Configuration configurationTransferOrder;
        private FrontWork.Model modelTransferOrder;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private FrontWork.JsonRESTSynchronizer synchronizer;
        private FrontWork.ReoGridView reoGridView1;
        private FrontWork.TabView tabView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
    }
}
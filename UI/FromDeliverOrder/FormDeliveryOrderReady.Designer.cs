namespace WMS.UI.FromDeliverOrder
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
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair3 = new FrontWork.ModeMethodListenerNamesPair();
            this.basicView2 = new FrontWork.BasicView();
            this.configurationInspectionNoteItem = new FrontWork.Configuration();
            this.modelBoxInspectionNoteItems = new FrontWork.ModelBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.basicView1 = new FrontWork.BasicView();
            this.configurationInspectionNote = new FrontWork.Configuration();
            this.modelInspectionNotes = new FrontWork.Model();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.synchronizer = new FrontWork.JsonRESTSynchronizer();
            this.configurationWarehouseEntry = new FrontWork.Configuration();
            this.modelWarehouseEntry = new FrontWork.Model();
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
            this.basicView2.Configuration = this.configurationInspectionNoteItem;
            this.basicView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicView2.Font = new System.Drawing.Font("黑体", 10F);
            this.basicView2.ItemsPerRow = 4;
            this.basicView2.Location = new System.Drawing.Point(3, 19);
            this.basicView2.Margin = new System.Windows.Forms.Padding(0);
            this.basicView2.Mode = "default";
            this.basicView2.Model = this.modelBoxInspectionNoteItems;
            this.basicView2.Name = "basicView2";
            this.basicView2.Size = new System.Drawing.Size(883, 115);
            this.basicView2.TabIndex = 4;
            // 
            // configurationInspectionNoteItem
            // 
            this.configurationInspectionNoteItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configurationInspectionNoteItem.ConfigurationString = resources.GetString("configurationInspectionNoteItem.ConfigurationString");
            this.configurationInspectionNoteItem.Location = new System.Drawing.Point(121, 3);
            modeMethodListenerNamesPair1.MethodListenerNames = new string[] {
        "FormWarehouseEntryInspect",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair1.Mode = "default";
            this.configurationInspectionNoteItem.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[] {
        modeMethodListenerNamesPair1};
            this.configurationInspectionNoteItem.Name = "configurationInspectionNoteItem";
            this.configurationInspectionNoteItem.Size = new System.Drawing.Size(180, 180);
            this.configurationInspectionNoteItem.TabIndex = 5;
            // 
            // modelBoxInspectionNoteItems
            // 
            this.modelBoxInspectionNoteItems.AllSelectionRanges = new FrontWork.Range[0];
            this.modelBoxInspectionNoteItems.Configuration = this.configurationInspectionNoteItem;
            this.modelBoxInspectionNoteItems.CurrentModelName = "default";
            this.modelBoxInspectionNoteItems.Location = new System.Drawing.Point(22, 121);
            this.modelBoxInspectionNoteItems.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.modelBoxInspectionNoteItems.Mode = "default";
            this.modelBoxInspectionNoteItems.Name = "modelBoxInspectionNoteItems";
            this.modelBoxInspectionNoteItems.SelectionRange = null;
            this.modelBoxInspectionNoteItems.Size = new System.Drawing.Size(126, 131);
            this.modelBoxInspectionNoteItems.TabIndex = 4;
            this.modelBoxInspectionNoteItems.Visible = false;
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
            this.basicView1.Configuration = this.configurationInspectionNote;
            this.basicView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicView1.Font = new System.Drawing.Font("黑体", 10F);
            this.basicView1.ItemsPerRow = 3;
            this.basicView1.Location = new System.Drawing.Point(0, 19);
            this.basicView1.Margin = new System.Windows.Forms.Padding(0);
            this.basicView1.Mode = "default";
            this.basicView1.Model = this.modelInspectionNotes;
            this.basicView1.Name = "basicView1";
            this.basicView1.Padding = new System.Windows.Forms.Padding(6);
            this.basicView1.Size = new System.Drawing.Size(889, 81);
            this.basicView1.TabIndex = 0;
            // 
            // configurationInspectionNote
            // 
            this.configurationInspectionNote.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configurationInspectionNote.ConfigurationString = resources.GetString("configurationInspectionNote.ConfigurationString");
            this.configurationInspectionNote.Location = new System.Drawing.Point(493, 102);
            modeMethodListenerNamesPair2.MethodListenerNames = new string[] {
        "FormWarehouseEntryInspect",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair2.Mode = "default";
            this.configurationInspectionNote.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[] {
        modeMethodListenerNamesPair2};
            this.configurationInspectionNote.Name = "configurationInspectionNote";
            this.configurationInspectionNote.Size = new System.Drawing.Size(180, 180);
            this.configurationInspectionNote.TabIndex = 1;
            // 
            // modelInspectionNotes
            // 
            this.modelInspectionNotes.AllSelectionRanges = new FrontWork.Range[0];
            this.modelInspectionNotes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.modelInspectionNotes.Configuration = this.configurationInspectionNote;
            this.modelInspectionNotes.Location = new System.Drawing.Point(694, 128);
            this.modelInspectionNotes.Mode = "default";
            this.modelInspectionNotes.Name = "modelInspectionNotes";
            this.modelInspectionNotes.SelectionRange = null;
            this.modelInspectionNotes.Size = new System.Drawing.Size(180, 180);
            this.modelInspectionNotes.TabIndex = 2;
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
            this.synchronizer.Configuration = this.configurationInspectionNoteItem;
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
            this.synchronizer.Location = new System.Drawing.Point(280, 102);
            this.synchronizer.Margin = new System.Windows.Forms.Padding(0);
            this.synchronizer.Mode = "default";
            this.synchronizer.Model = this.modelBoxInspectionNoteItems;
            this.synchronizer.Name = "synchronizer";
            this.synchronizer.Size = new System.Drawing.Size(180, 180);
            this.synchronizer.TabIndex = 6;
            // 
            // configurationWarehouseEntry
            // 
            this.configurationWarehouseEntry.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configurationWarehouseEntry.ConfigurationString = "[\r\n    {mode:\"default\",\r\n     fields:[\r\n        {name:\"id\",type:\"int\"},\r\n        " +
    "{name:\"no\",type:\"string\"},\r\n        {name:\"createTime\",type:\"datetime\"}\r\n     ]\r" +
    "\n    }\r\n]";
            this.configurationWarehouseEntry.Location = new System.Drawing.Point(546, -55);
            modeMethodListenerNamesPair3.MethodListenerNames = new string[] {
        "FormWarehouseEntryInspect",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair3.Mode = "default";
            this.configurationWarehouseEntry.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[] {
        modeMethodListenerNamesPair3};
            this.configurationWarehouseEntry.Name = "configurationWarehouseEntry";
            this.configurationWarehouseEntry.Size = new System.Drawing.Size(180, 180);
            this.configurationWarehouseEntry.TabIndex = 2;
            // 
            // modelWarehouseEntry
            // 
            this.modelWarehouseEntry.AllSelectionRanges = new FrontWork.Range[0];
            this.modelWarehouseEntry.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.modelWarehouseEntry.Configuration = this.configurationWarehouseEntry;
            this.modelWarehouseEntry.Location = new System.Drawing.Point(374, -36);
            this.modelWarehouseEntry.Mode = "default";
            this.modelWarehouseEntry.Name = "modelWarehouseEntry";
            this.modelWarehouseEntry.SelectionRange = null;
            this.modelWarehouseEntry.Size = new System.Drawing.Size(180, 180);
            this.modelWarehouseEntry.TabIndex = 1;
            // 
            // reoGridView1
            // 
            this.reoGridView1.Configuration = this.configurationInspectionNoteItem;
            this.reoGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGridView1.Font = new System.Drawing.Font("黑体", 11F);
            this.reoGridView1.Location = new System.Drawing.Point(0, 0);
            this.reoGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.reoGridView1.Mode = "default";
            this.reoGridView1.Model = this.modelBoxInspectionNoteItems;
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
            this.tabView1.ModelBox = this.modelBoxInspectionNoteItems;
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
            this.panel1.Controls.Add(this.configurationInspectionNote);
            this.panel1.Controls.Add(this.modelInspectionNotes);
            this.panel1.Controls.Add(this.synchronizer);
            this.panel1.Controls.Add(this.configurationInspectionNoteItem);
            this.panel1.Controls.Add(this.modelBoxInspectionNoteItems);
            this.panel1.Controls.Add(this.configurationWarehouseEntry);
            this.panel1.Controls.Add(this.modelWarehouseEntry);
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
        private FrontWork.Configuration configurationInspectionNoteItem;
        private FrontWork.ModelBox modelBoxInspectionNoteItems;
        private System.Windows.Forms.GroupBox groupBox1;
        private FrontWork.BasicView basicView1;
        private FrontWork.Configuration configurationInspectionNote;
        private FrontWork.Model modelInspectionNotes;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private FrontWork.JsonRESTSynchronizer synchronizer;
        private FrontWork.Configuration configurationWarehouseEntry;
        private FrontWork.Model modelWarehouseEntry;
        private FrontWork.ReoGridView reoGridView1;
        private FrontWork.TabView tabView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
    }
}
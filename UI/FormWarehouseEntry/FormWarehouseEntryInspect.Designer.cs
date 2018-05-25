namespace WMS.UI
{
    partial class FormWarehouseEntryInspect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWarehouseEntryInspect));
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair2 = new FrontWork.ModeMethodListenerNamesPair();
            FrontWork.JsonRESTSynchronizer.FieldMappingItem fieldMappingItem1 = new FrontWork.JsonRESTSynchronizer.FieldMappingItem();
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair3 = new FrontWork.ModeMethodListenerNamesPair();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabView1 = new FrontWork.TabView();
            this.configurationWarehouseEntry = new FrontWork.Configuration();
            this.modelWarehouseEntry = new FrontWork.Model();
            this.panel1 = new System.Windows.Forms.Panel();
            this.configurationInspectionNote = new FrontWork.Configuration();
            this.modelInspectionNotes = new FrontWork.Model();
            this.synchronizer = new FrontWork.JsonRESTSynchronizer();
            this.configurationInspectionNoteItem = new FrontWork.Configuration();
            this.modelBoxInspectionNoteItems = new FrontWork.ModelBox();
            this.reoGridView1 = new FrontWork.ReoGridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.basicView1 = new FrontWork.BasicView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.basicView2 = new FrontWork.BasicView();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(874, 529);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // tabView1
            // 
            this.tabView1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tabView1.ColumnName = "no";
            this.tabView1.Configuration = this.configurationWarehouseEntry;
            this.tabView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabView1.Font = new System.Drawing.Font("宋体", 10F);
            this.tabView1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabView1.Location = new System.Drawing.Point(0, 0);
            this.tabView1.Margin = new System.Windows.Forms.Padding(0);
            this.tabView1.Mode = "default";
            this.tabView1.Model = this.modelWarehouseEntry;
            this.tabView1.Name = "tabView1";
            this.tabView1.Size = new System.Drawing.Size(874, 25);
            this.tabView1.TabIndex = 0;
            // 
            // configurationWarehouseEntry
            // 
            this.configurationWarehouseEntry.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configurationWarehouseEntry.ConfigurationString = "[\r\n    {mode:\"default\",\r\n     fields:[\r\n        {name:\"id\",type:\"int\"},\r\n        " +
    "{name:\"no\",type:\"string\"},\r\n        {name:\"createTime\",type:\"datetime\"}\r\n     ]\r" +
    "\n    }\r\n]";
            this.configurationWarehouseEntry.Location = new System.Drawing.Point(630, -36);
            modeMethodListenerNamesPair1.MethodListenerNames = new string[] {
        "FormWarehouseEntryInspect",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair1.Mode = "default";
            this.configurationWarehouseEntry.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[] {
        modeMethodListenerNamesPair1};
            this.configurationWarehouseEntry.Name = "configurationWarehouseEntry";
            this.configurationWarehouseEntry.Size = new System.Drawing.Size(180, 180);
            this.configurationWarehouseEntry.TabIndex = 2;
            this.configurationWarehouseEntry.Load += new System.EventHandler(this.configurationWarehouseEntry_Load);
            // 
            // modelWarehouseEntry
            // 
            this.modelWarehouseEntry.AllSelectionRanges = new FrontWork.Range[0];
            this.modelWarehouseEntry.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.modelWarehouseEntry.Configuration = this.configurationWarehouseEntry;
            this.modelWarehouseEntry.Location = new System.Drawing.Point(460, -36);
            this.modelWarehouseEntry.Mode = "default";
            this.modelWarehouseEntry.Name = "modelWarehouseEntry";
            this.modelWarehouseEntry.SelectionRange = null;
            this.modelWarehouseEntry.Size = new System.Drawing.Size(180, 180);
            this.modelWarehouseEntry.TabIndex = 1;
            this.modelWarehouseEntry.SelectionRangeChanged += new System.EventHandler<FrontWork.ModelSelectionRangeChangedEventArgs>(this.modelWarehouseEntry_SelectionRangeChanged);
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
            this.panel1.Size = new System.Drawing.Size(874, 224);
            this.panel1.TabIndex = 1;
            // 
            // configurationInspectionNote
            // 
            this.configurationInspectionNote.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configurationInspectionNote.ConfigurationString = resources.GetString("configurationInspectionNote.ConfigurationString");
            this.configurationInspectionNote.Location = new System.Drawing.Point(526, 131);
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
            // synchronizer
            // 
            this.synchronizer.Configuration = this.configurationInspectionNoteItem;
            fieldMappingItem1.APIFieldName = "id";
            fieldMappingItem1.ModelFieldName = "warehouseEntryItemId";
            this.synchronizer.FieldMapping = new FrontWork.JsonRESTSynchronizer.FieldMappingItem[] {
        fieldMappingItem1};
            this.synchronizer.Location = new System.Drawing.Point(318, 53);
            this.synchronizer.Margin = new System.Windows.Forms.Padding(0);
            this.synchronizer.Mode = "default";
            this.synchronizer.Model = this.modelBoxInspectionNoteItems;
            this.synchronizer.Name = "synchronizer";
            this.synchronizer.Size = new System.Drawing.Size(180, 180);
            this.synchronizer.TabIndex = 6;
            // 
            // configurationInspectionNoteItem
            // 
            this.configurationInspectionNoteItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configurationInspectionNoteItem.ConfigurationString = resources.GetString("configurationInspectionNoteItem.ConfigurationString");
            this.configurationInspectionNoteItem.Location = new System.Drawing.Point(135, 53);
            modeMethodListenerNamesPair3.MethodListenerNames = new string[] {
        "FormWarehouseEntryInspect",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair3.Mode = "default";
            this.configurationInspectionNoteItem.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[] {
        modeMethodListenerNamesPair3};
            this.configurationInspectionNoteItem.Name = "configurationInspectionNoteItem";
            this.configurationInspectionNoteItem.Size = new System.Drawing.Size(180, 180);
            this.configurationInspectionNoteItem.TabIndex = 5;
            // 
            // modelBoxInspectionNoteItems
            // 
            this.modelBoxInspectionNoteItems.AllSelectionRanges = new FrontWork.Range[0];
            this.modelBoxInspectionNoteItems.Configuration = this.configurationInspectionNoteItem;
            this.modelBoxInspectionNoteItems.CurrentModelName = "default";
            this.modelBoxInspectionNoteItems.Location = new System.Drawing.Point(3, 102);
            this.modelBoxInspectionNoteItems.Mode = "default";
            this.modelBoxInspectionNoteItems.Name = "modelBoxInspectionNoteItems";
            this.modelBoxInspectionNoteItems.SelectionRange = null;
            this.modelBoxInspectionNoteItems.Size = new System.Drawing.Size(126, 131);
            this.modelBoxInspectionNoteItems.TabIndex = 4;
            this.modelBoxInspectionNoteItems.Visible = false;
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
            this.reoGridView1.Size = new System.Drawing.Size(874, 224);
            this.reoGridView1.TabIndex = 3;
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
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 489);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(874, 40);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // buttonOK
            // 
            this.buttonOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOK.Font = new System.Drawing.Font("黑体", 10F);
            this.buttonOK.Location = new System.Drawing.Point(362, 0);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(150, 35);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "确定送检";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 389);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(874, 100);
            this.panel2.TabIndex = 3;
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
            this.groupBox1.Size = new System.Drawing.Size(874, 100);
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
            this.basicView1.Location = new System.Drawing.Point(0, 34);
            this.basicView1.Margin = new System.Windows.Forms.Padding(0);
            this.basicView1.Mode = "default";
            this.basicView1.Model = this.modelInspectionNotes;
            this.basicView1.Name = "basicView1";
            this.basicView1.Padding = new System.Windows.Forms.Padding(6);
            this.basicView1.Size = new System.Drawing.Size(874, 66);
            this.basicView1.TabIndex = 0;
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
            this.groupBox2.Size = new System.Drawing.Size(874, 137);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "送检物料条目";
            // 
            // basicView2
            // 
            this.basicView2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.basicView2.Configuration = this.configurationInspectionNoteItem;
            this.basicView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicView2.Font = new System.Drawing.Font("黑体", 10F);
            this.basicView2.ItemsPerRow = 4;
            this.basicView2.Location = new System.Drawing.Point(3, 34);
            this.basicView2.Margin = new System.Windows.Forms.Padding(0);
            this.basicView2.Mode = "default";
            this.basicView2.Model = this.modelBoxInspectionNoteItems;
            this.basicView2.Name = "basicView2";
            this.basicView2.Size = new System.Drawing.Size(868, 100);
            this.basicView2.TabIndex = 4;
            // 
            // FormWarehouseEntryInspect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 529);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("宋体", 10F);
            this.Name = "FormWarehouseEntryInspect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "入库单送检";
            this.Load += new System.EventHandler(this.FormWarehouseEntryInspect_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FrontWork.TabView tabView1;
        private FrontWork.Configuration configurationWarehouseEntry;
        private FrontWork.Model modelWarehouseEntry;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button buttonOK;
        private FrontWork.Configuration configurationInspectionNoteItem;
        private FrontWork.ModelBox modelBoxInspectionNoteItems;
        private FrontWork.ReoGridView reoGridView1;
        private System.Windows.Forms.Panel panel2;
        private FrontWork.Configuration configurationInspectionNote;
        private FrontWork.BasicView basicView1;
        private FrontWork.Model modelInspectionNotes;
        private FrontWork.JsonRESTSynchronizer synchronizer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private FrontWork.BasicView basicView2;
    }
}
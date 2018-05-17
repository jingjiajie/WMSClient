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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWarehouseEntryInspect));
            this.configurationWarehouseEntryNo = new FrontWork.Configuration();
            this.modelWarehouseEntryNo = new FrontWork.Model();
            this.tabView1 = new FrontWork.TabView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.synchronizer = new FrontWork.JsonRESTSynchronizer();
            this.configurationInspectionNoteItem = new FrontWork.Configuration();
            this.modelBoxInspectionNoteItems = new FrontWork.ModelBox();
            this.reoGridView1 = new FrontWork.ReoGridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.modelInspectionNote = new FrontWork.Model();
            this.configurationInspectionNote = new FrontWork.Configuration();
            this.basicView1 = new FrontWork.BasicView();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // configurationWarehouseEntryNo
            // 
            this.configurationWarehouseEntryNo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configurationWarehouseEntryNo.ConfigurationString = "[\r\n    {mode:\"default\",\r\n     fields:[\r\n        {name:\"id\",type:\"int\"},\r\n        " +
    "{name:\"no\",type:\"string\"}\r\n     ]\r\n    }\r\n]";
            this.configurationWarehouseEntryNo.Location = new System.Drawing.Point(1069, 3);
            this.configurationWarehouseEntryNo.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[0];
            this.configurationWarehouseEntryNo.Name = "configurationWarehouseEntryNo";
            this.configurationWarehouseEntryNo.Size = new System.Drawing.Size(180, 180);
            this.configurationWarehouseEntryNo.TabIndex = 2;
            // 
            // modelWarehouseEntryNo
            // 
            this.modelWarehouseEntryNo.AllSelectionRanges = new FrontWork.Range[0];
            this.modelWarehouseEntryNo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.modelWarehouseEntryNo.Configuration = this.configurationWarehouseEntryNo;
            this.modelWarehouseEntryNo.Location = new System.Drawing.Point(895, 0);
            this.modelWarehouseEntryNo.Mode = "default";
            this.modelWarehouseEntryNo.Name = "modelWarehouseEntryNo";
            this.modelWarehouseEntryNo.SelectionRange = null;
            this.modelWarehouseEntryNo.Size = new System.Drawing.Size(180, 180);
            this.modelWarehouseEntryNo.TabIndex = 1;
            // 
            // tabView1
            // 
            this.tabView1.ColumnName = "no";
            this.tabView1.Configuration = this.configurationWarehouseEntryNo;
            this.tabView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabView1.Font = new System.Drawing.Font("宋体", 10F);
            this.tabView1.Location = new System.Drawing.Point(0, 0);
            this.tabView1.Margin = new System.Windows.Forms.Padding(0);
            this.tabView1.Mode = "default";
            this.tabView1.Model = this.modelWarehouseEntryNo;
            this.tabView1.Name = "tabView1";
            this.tabView1.Size = new System.Drawing.Size(1249, 50);
            this.tabView1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tabView1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1249, 800);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.synchronizer);
            this.panel1.Controls.Add(this.configurationInspectionNoteItem);
            this.panel1.Controls.Add(this.modelBoxInspectionNoteItems);
            this.panel1.Controls.Add(this.configurationWarehouseEntryNo);
            this.panel1.Controls.Add(this.modelWarehouseEntryNo);
            this.panel1.Controls.Add(this.reoGridView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 50);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1249, 490);
            this.panel1.TabIndex = 1;
            // 
            // synchronizer
            // 
            this.synchronizer.Configuration = this.configurationInspectionNoteItem;
            this.synchronizer.Location = new System.Drawing.Point(372, 257);
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
            this.configurationInspectionNoteItem.Location = new System.Drawing.Point(189, 257);
            this.configurationInspectionNoteItem.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[0];
            this.configurationInspectionNoteItem.Name = "configurationInspectionNoteItem";
            this.configurationInspectionNoteItem.Size = new System.Drawing.Size(180, 180);
            this.configurationInspectionNoteItem.TabIndex = 5;
            // 
            // modelBoxInspectionNoteItems
            // 
            this.modelBoxInspectionNoteItems.AllSelectionRanges = new FrontWork.Range[0];
            this.modelBoxInspectionNoteItems.Configuration = this.configurationInspectionNoteItem;
            this.modelBoxInspectionNoteItems.CurrentModelName = "default";
            this.modelBoxInspectionNoteItems.Location = new System.Drawing.Point(3, 257);
            this.modelBoxInspectionNoteItems.Mode = "default";
            this.modelBoxInspectionNoteItems.Name = "modelBoxInspectionNoteItems";
            this.modelBoxInspectionNoteItems.SelectionRange = null;
            this.modelBoxInspectionNoteItems.Size = new System.Drawing.Size(180, 180);
            this.modelBoxInspectionNoteItems.TabIndex = 4;
            this.modelBoxInspectionNoteItems.Visible = false;
            // 
            // reoGridView1
            // 
            this.reoGridView1.Configuration = null;
            this.reoGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGridView1.Font = new System.Drawing.Font("黑体", 11F);
            this.reoGridView1.Location = new System.Drawing.Point(0, 0);
            this.reoGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.reoGridView1.Mode = "default";
            this.reoGridView1.Model = null;
            this.reoGridView1.Name = "reoGridView1";
            this.reoGridView1.Size = new System.Drawing.Size(1249, 490);
            this.reoGridView1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.buttonOK, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 740);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1249, 60);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // buttonOK
            // 
            this.buttonOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOK.Font = new System.Drawing.Font("黑体", 10F);
            this.buttonOK.Location = new System.Drawing.Point(499, 0);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(250, 60);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "确定送检";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.modelInspectionNote);
            this.panel2.Controls.Add(this.configurationInspectionNote);
            this.panel2.Controls.Add(this.basicView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 540);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1249, 200);
            this.panel2.TabIndex = 3;
            // 
            // modelInspectionNote
            // 
            this.modelInspectionNote.AllSelectionRanges = new FrontWork.Range[0];
            this.modelInspectionNote.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.modelInspectionNote.Configuration = this.configurationInspectionNote;
            this.modelInspectionNote.Location = new System.Drawing.Point(936, 47);
            this.modelInspectionNote.Mode = "default";
            this.modelInspectionNote.Name = "modelInspectionNote";
            this.modelInspectionNote.SelectionRange = null;
            this.modelInspectionNote.Size = new System.Drawing.Size(180, 180);
            this.modelInspectionNote.TabIndex = 2;
            // 
            // configurationInspectionNote
            // 
            this.configurationInspectionNote.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configurationInspectionNote.ConfigurationString = resources.GetString("configurationInspectionNote.ConfigurationString");
            this.configurationInspectionNote.Location = new System.Drawing.Point(1081, 47);
            this.configurationInspectionNote.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[0];
            this.configurationInspectionNote.Name = "configurationInspectionNote";
            this.configurationInspectionNote.Size = new System.Drawing.Size(180, 180);
            this.configurationInspectionNote.TabIndex = 1;
            // 
            // basicView1
            // 
            this.basicView1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.basicView1.Configuration = this.configurationInspectionNote;
            this.basicView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicView1.Font = new System.Drawing.Font("黑体", 10F);
            this.basicView1.ItemsPerRow = 3;
            this.basicView1.Location = new System.Drawing.Point(0, 0);
            this.basicView1.Margin = new System.Windows.Forms.Padding(0);
            this.basicView1.Mode = "default";
            this.basicView1.Model = this.modelInspectionNote;
            this.basicView1.Name = "basicView1";
            this.basicView1.Size = new System.Drawing.Size(1249, 200);
            this.basicView1.TabIndex = 0;
            // 
            // FormWarehouseEntryInspect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1249, 800);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormWarehouseEntryInspect";
            this.Text = "FormWarehouseEntryInspect";
            this.Load += new System.EventHandler(this.FormWarehouseEntryInspect_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FrontWork.TabView tabView1;
        private FrontWork.Configuration configurationWarehouseEntryNo;
        private FrontWork.Model modelWarehouseEntryNo;
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
        private FrontWork.Model modelInspectionNote;
        private FrontWork.JsonRESTSynchronizer synchronizer;
    }
}
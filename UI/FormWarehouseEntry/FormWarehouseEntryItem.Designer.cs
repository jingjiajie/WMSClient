﻿using System.Windows.Forms;

namespace WMS.UI
{
    public partial class FormWarehouseEntryItem : Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWarehouseEntryItem));
            FrontWork.SearchViewJsonRESTAdapter.APIParamNamesType apiParamNamesType1 = new FrontWork.SearchViewJsonRESTAdapter.APIParamNamesType();
            FrontWork.SearchViewJsonRESTAdapter.ConditionFieldNamesType conditionFieldNamesType1 = new FrontWork.SearchViewJsonRESTAdapter.ConditionFieldNamesType();
            FrontWork.SearchViewJsonRESTAdapter.OrderParamNamesType orderParamNamesType1 = new FrontWork.SearchViewJsonRESTAdapter.OrderParamNamesType();
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair1 = new FrontWork.ModeMethodListenerNamesPair();
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair2 = new FrontWork.ModeMethodListenerNamesPair();
            this.lableStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.buttonAdd = new System.Windows.Forms.ToolStripButton();
            this.buttonDelete = new System.Windows.Forms.ToolStripButton();
            this.buttonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.pagerView1 = new FrontWork.PagerView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pagerSearchJsonRESTAdapter1 = new FrontWork.PagerSearchJsonRESTAdapter();
            this.searchView1 = new FrontWork.SearchView();
            this.configuration1 = new FrontWork.Configuration();
            this.synchronizer = new FrontWork.JsonRESTSynchronizer();
            this.model = new FrontWork.Model();
            this.reoGridView1 = new FrontWork.ReoGridView();
            this.basicView1 = new FrontWork.BasicView();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStripTop.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lableStatus
            // 
            this.lableStatus.Name = "lableStatus";
            this.lableStatus.Size = new System.Drawing.Size(114, 25);
            this.lableStatus.Text = "入库单物料列表";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(54, 25);
            this.toolStripStatusLabel1.Text = "状态：";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lableStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 499);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(974, 30);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.toolStripTop, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.pagerView1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.basicView1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.searchView1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(974, 529);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // toolStripTop
            // 
            this.toolStripTop.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStripTop.BackgroundImage = global::WMS.UI.Properties.Resources.bottonW_q;
            this.toolStripTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStripTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripTop.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonAdd,
            this.buttonDelete,
            this.buttonSave,
            this.toolStripSeparator3});
            this.toolStripTop.Location = new System.Drawing.Point(0, 35);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripTop.Size = new System.Drawing.Size(974, 30);
            this.toolStripTop.TabIndex = 3;
            this.toolStripTop.Text = "toolStrip1";
            // 
            // buttonAdd
            // 
            this.buttonAdd.Image = ((System.Drawing.Image)(resources.GetObject("buttonAdd.Image")));
            this.buttonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(63, 27);
            this.buttonAdd.Text = "添加";
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Image = ((System.Drawing.Image)(resources.GetObject("buttonDelete.Image")));
            this.buttonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(63, 27);
            this.buttonDelete.Text = "删除";
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Image = ((System.Drawing.Image)(resources.GetObject("buttonSave.Image")));
            this.buttonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(93, 27);
            this.buttonSave.Text = "保存修改";
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AutoSize = false;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(10, 28);
            // 
            // pagerView1
            // 
            this.pagerView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pagerView1.Location = new System.Drawing.Point(2, 461);
            this.pagerView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pagerView1.Mode = "default";
            this.pagerView1.Name = "pagerView1";
            this.pagerView1.PageSize = 50;
            this.pagerView1.Size = new System.Drawing.Size(970, 36);
            this.pagerView1.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pagerSearchJsonRESTAdapter1);
            this.panel1.Controls.Add(this.synchronizer);
            this.panel1.Controls.Add(this.model);
            this.panel1.Controls.Add(this.configuration1);
            this.panel1.Controls.Add(this.reoGridView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 235);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(974, 224);
            this.panel1.TabIndex = 13;
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
            this.pagerSearchJsonRESTAdapter1.Location = new System.Drawing.Point(561, 48);
            this.pagerSearchJsonRESTAdapter1.Name = "pagerSearchJsonRESTAdapter1";
            this.pagerSearchJsonRESTAdapter1.PagerView = this.pagerView1;
            this.pagerSearchJsonRESTAdapter1.SearchView = this.searchView1;
            this.pagerSearchJsonRESTAdapter1.Size = new System.Drawing.Size(192, 189);
            this.pagerSearchJsonRESTAdapter1.Synchronizer = this.synchronizer;
            this.pagerSearchJsonRESTAdapter1.TabIndex = 4;
            this.pagerSearchJsonRESTAdapter1.Visible = false;
            // 
            // searchView1
            // 
            this.searchView1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.searchView1.Configuration = this.configuration1;
            this.searchView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchView1.Location = new System.Drawing.Point(0, 0);
            this.searchView1.Margin = new System.Windows.Forms.Padding(0);
            this.searchView1.Mode = "default";
            this.searchView1.Name = "searchView1";
            this.searchView1.Size = new System.Drawing.Size(974, 35);
            this.searchView1.TabIndex = 15;
            // 
            // configuration1
            // 
            this.configuration1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configuration1.ConfigurationString = resources.GetString("configuration1.ConfigurationString");
            this.configuration1.Location = new System.Drawing.Point(49, 66);
            modeMethodListenerNamesPair1.MethodListenerNames = new string[] {
        "FormWarehouseEntryItem",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair1.Mode = "default";
            modeMethodListenerNamesPair2.MethodListenerNames = new string[] {
        "FormWarehouseEntryItem",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair2.Mode = "put_in";
            this.configuration1.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[] {
        modeMethodListenerNamesPair1,
        modeMethodListenerNamesPair2};
            this.configuration1.Name = "configuration1";
            this.configuration1.Size = new System.Drawing.Size(180, 180);
            this.configuration1.TabIndex = 1;
            // 
            // synchronizer
            // 
            this.synchronizer.Configuration = this.configuration1;
            this.synchronizer.FieldMapping = new FrontWork.JsonRESTSynchronizer.FieldMappingItem[0];
            this.synchronizer.Location = new System.Drawing.Point(397, 57);
            this.synchronizer.Margin = new System.Windows.Forms.Padding(0);
            this.synchronizer.Mode = "default";
            this.synchronizer.Model = this.model;
            this.synchronizer.Name = "synchronizer";
            this.synchronizer.Size = new System.Drawing.Size(180, 180);
            this.synchronizer.TabIndex = 3;
            // 
            // model
            // 
            this.model.AllSelectionRanges = new FrontWork.Range[0];
            this.model.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.model.Configuration = this.configuration1;
            this.model.Font = new System.Drawing.Font("宋体", 10F);
            this.model.Location = new System.Drawing.Point(224, 66);
            this.model.Mode = "default";
            this.model.Name = "model";
            this.model.SelectionRange = null;
            this.model.Size = new System.Drawing.Size(180, 180);
            this.model.TabIndex = 2;
            this.model.SelectionRangeChanged += new System.EventHandler<FrontWork.ModelSelectionRangeChangedEventArgs>(this.model_SelectionRangeChanged);
            // 
            // reoGridView1
            // 
            this.reoGridView1.Configuration = this.configuration1;
            this.reoGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGridView1.Font = new System.Drawing.Font("黑体", 11F);
            this.reoGridView1.Location = new System.Drawing.Point(0, 0);
            this.reoGridView1.Mode = "default";
            this.reoGridView1.Model = this.model;
            this.reoGridView1.Name = "reoGridView1";
            this.reoGridView1.Size = new System.Drawing.Size(974, 224);
            this.reoGridView1.TabIndex = 0;
            // 
            // basicView1
            // 
            this.basicView1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.basicView1.Configuration = this.configuration1;
            this.basicView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicView1.Font = new System.Drawing.Font("黑体", 10F);
            this.basicView1.ItemsPerRow = 5;
            this.basicView1.Location = new System.Drawing.Point(0, 65);
            this.basicView1.Margin = new System.Windows.Forms.Padding(0);
            this.basicView1.Mode = "default";
            this.basicView1.Model = this.model;
            this.basicView1.Name = "basicView1";
            this.basicView1.Padding = new System.Windows.Forms.Padding(6, 6, 6, 0);
            this.basicView1.Size = new System.Drawing.Size(974, 170);
            this.basicView1.TabIndex = 14;
            // 
            // FormWarehouseEntryItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 529);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("宋体", 10F);
            this.Name = "FormWarehouseEntryItem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "入库单物料列表";
            this.Load += new System.EventHandler(this.FormWarehouseEntry_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolStripStatusLabel lableStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton buttonDelete;
        private System.Windows.Forms.ToolStripButton buttonSave;
        private System.Windows.Forms.ToolStripButton buttonAdd;
        private System.Windows.Forms.ToolStrip toolStripTop;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private FrontWork.PagerView pagerView1;
        private System.Windows.Forms.Panel panel1;
        private FrontWork.ReoGridView reoGridView1;
        private FrontWork.Model model;
        private FrontWork.Configuration configuration1;
        private FrontWork.BasicView basicView1;
        private FrontWork.JsonRESTSynchronizer synchronizer;
        private FrontWork.PagerSearchJsonRESTAdapter pagerSearchJsonRESTAdapter1;
        private FrontWork.SearchView searchView1;
    }
}
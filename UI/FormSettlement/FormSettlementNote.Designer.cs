namespace WMS.UI.FormSettlement
{
    partial class FormSettlementNote
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
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair2 = new FrontWork.ModeMethodListenerNamesPair();
            FrontWork.SearchViewJsonRESTAdapter.APIParamNamesType apiParamNamesType2 = new FrontWork.SearchViewJsonRESTAdapter.APIParamNamesType();
            FrontWork.SearchViewJsonRESTAdapter.ConditionFieldNamesType conditionFieldNamesType2 = new FrontWork.SearchViewJsonRESTAdapter.ConditionFieldNamesType();
            FrontWork.SearchViewJsonRESTAdapter.OrderParamNamesType orderParamNamesType2 = new FrontWork.SearchViewJsonRESTAdapter.OrderParamNamesType();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettlementNote));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelSearchWidget = new System.Windows.Forms.Panel();
            this.searchView1 = new FrontWork.SearchView();
            this.configuration1 = new FrontWork.Configuration();
            this.panelPager = new System.Windows.Forms.Panel();
            this.pagerSearchJsonRESTAdapter1 = new FrontWork.PagerSearchJsonRESTAdapter();
            this.pagerView1 = new FrontWork.PagerView();
            this.synchronizer = new FrontWork.JsonRESTSynchronizer();
            this.model1 = new FrontWork.Model();
            this.reoGridView1 = new FrontWork.ReoGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.buttonItems = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAlter = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonReceivables = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonReceipts = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.basicView1 = new FrontWork.BasicView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelSearchWidget.SuspendLayout();
            this.panelPager.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStripTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelSearchWidget, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelPager, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.toolStripTop, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.basicView1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.pagerView1, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(849, 441);
            this.tableLayoutPanel1.TabIndex = 21;
            // 
            // panelSearchWidget
            // 
            this.panelSearchWidget.Controls.Add(this.searchView1);
            this.panelSearchWidget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSearchWidget.Location = new System.Drawing.Point(0, 0);
            this.panelSearchWidget.Margin = new System.Windows.Forms.Padding(0);
            this.panelSearchWidget.Name = "panelSearchWidget";
            this.panelSearchWidget.Size = new System.Drawing.Size(849, 25);
            this.panelSearchWidget.TabIndex = 12;
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
            this.searchView1.Padding = new System.Windows.Forms.Padding(3);
            this.searchView1.Size = new System.Drawing.Size(849, 25);
            this.searchView1.TabIndex = 0;
            // 
            // configuration1
            // 
            this.configuration1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configuration1.ConfigurationString = resources.GetString("configuration1.ConfigurationString");
            this.configuration1.Location = new System.Drawing.Point(63, 65);
            this.configuration1.Margin = new System.Windows.Forms.Padding(2);
            modeMethodListenerNamesPair2.MethodListenerNames = new string[] {
        "FormSettlementNote",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair2.Mode = "default";
            this.configuration1.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[] {
        modeMethodListenerNamesPair2};
            this.configuration1.Name = "configuration1";
            this.configuration1.Size = new System.Drawing.Size(180, 180);
            this.configuration1.TabIndex = 1;
            // 
            // panelPager
            // 
            this.panelPager.BackColor = System.Drawing.SystemColors.Control;
            this.panelPager.Controls.Add(this.pagerSearchJsonRESTAdapter1);
            this.panelPager.Controls.Add(this.synchronizer);
            this.panelPager.Controls.Add(this.model1);
            this.panelPager.Controls.Add(this.configuration1);
            this.panelPager.Controls.Add(this.reoGridView1);
            this.panelPager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPager.Location = new System.Drawing.Point(0, 164);
            this.panelPager.Margin = new System.Windows.Forms.Padding(0);
            this.panelPager.Name = "panelPager";
            this.panelPager.Size = new System.Drawing.Size(849, 225);
            this.panelPager.TabIndex = 8;
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
            this.pagerSearchJsonRESTAdapter1.Location = new System.Drawing.Point(645, 75);
            this.pagerSearchJsonRESTAdapter1.Margin = new System.Windows.Forms.Padding(2);
            this.pagerSearchJsonRESTAdapter1.Name = "pagerSearchJsonRESTAdapter1";
            this.pagerSearchJsonRESTAdapter1.PagerView = this.pagerView1;
            this.pagerSearchJsonRESTAdapter1.SearchView = this.searchView1;
            this.pagerSearchJsonRESTAdapter1.Size = new System.Drawing.Size(121, 128);
            this.pagerSearchJsonRESTAdapter1.Synchronizer = this.synchronizer;
            this.pagerSearchJsonRESTAdapter1.TabIndex = 4;
            this.pagerSearchJsonRESTAdapter1.Visible = false;
            // 
            // pagerView1
            // 
            this.pagerView1.BackColor = System.Drawing.SystemColors.Control;
            this.pagerView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pagerView1.Location = new System.Drawing.Point(2, 391);
            this.pagerView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pagerView1.Mode = "default";
            this.pagerView1.Name = "pagerView1";
            this.pagerView1.PageSize = 50;
            this.pagerView1.Size = new System.Drawing.Size(845, 30);
            this.pagerView1.TabIndex = 14;
            // 
            // synchronizer
            // 
            this.synchronizer.Configuration = this.configuration1;
            this.synchronizer.FieldMapping = new FrontWork.JsonRESTSynchronizer.FieldMappingItem[0];
            this.synchronizer.Location = new System.Drawing.Point(457, 75);
            this.synchronizer.Margin = new System.Windows.Forms.Padding(0);
            this.synchronizer.Mode = "default";
            this.synchronizer.Model = this.model1;
            this.synchronizer.Name = "synchronizer";
            this.synchronizer.Size = new System.Drawing.Size(180, 180);
            this.synchronizer.TabIndex = 3;
            // 
            // model1
            // 
            this.model1.AllSelectionRanges = new FrontWork.Range[0];
            this.model1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.model1.Configuration = this.configuration1;
            this.model1.Font = new System.Drawing.Font("宋体", 10F);
            this.model1.Location = new System.Drawing.Point(270, 75);
            this.model1.Margin = new System.Windows.Forms.Padding(2);
            this.model1.Mode = "default";
            this.model1.Name = "model1";
            this.model1.SelectionRange = null;
            this.model1.Size = new System.Drawing.Size(141, 138);
            this.model1.TabIndex = 2;
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
            this.reoGridView1.Size = new System.Drawing.Size(849, 225);
            this.reoGridView1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.labelStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 423);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(849, 18);
            this.statusStrip1.TabIndex = 16;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(44, 13);
            this.toolStripStatusLabel1.Text = "状态：";
            // 
            // labelStatus
            // 
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(68, 13);
            this.labelStatus.Text = "结算单信息";
            // 
            // toolStripTop
            // 
            this.toolStripTop.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStripTop.BackgroundImage = global::WMS.UI.Properties.Resources.bottonW_q;
            this.toolStripTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStripTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripTop.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonItems,
            this.toolStripSeparator2,
            this.toolStripButtonAdd,
            this.toolStripButtonDelete,
            this.toolStripButtonAlter,
            this.toolStripSeparator3,
            this.toolStripButtonReceivables,
            this.toolStripButtonReceipts,
            this.toolStripSeparator1});
            this.toolStripTop.Location = new System.Drawing.Point(0, 25);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Size = new System.Drawing.Size(849, 24);
            this.toolStripTop.TabIndex = 9;
            this.toolStripTop.Text = "toolStrip1";
            // 
            // buttonItems
            // 
            this.buttonItems.Image = global::WMS.UI.Properties.Resources.find;
            this.buttonItems.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonItems.Name = "buttonItems";
            this.buttonItems.Size = new System.Drawing.Size(116, 21);
            this.buttonItems.Text = "查看结算单条目";
            this.buttonItems.ToolTipText = "查看零件条目";
            this.buttonItems.Click += new System.EventHandler(this.buttonItems_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AutoSize = false;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(15, 28);
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAdd.Image")));
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(56, 21);
            this.toolStripButtonAdd.Text = "添加";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDelete.Image")));
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(56, 21);
            this.toolStripButtonDelete.Text = "删除";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripButtonAlter
            // 
            this.toolStripButtonAlter.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAlter.Image")));
            this.toolStripButtonAlter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAlter.Name = "toolStripButtonAlter";
            this.toolStripButtonAlter.Size = new System.Drawing.Size(80, 21);
            this.toolStripButtonAlter.Text = "保存修改";
            this.toolStripButtonAlter.Click += new System.EventHandler(this.toolStripButtonAlter_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AutoSize = false;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(15, 28);
            // 
            // toolStripButtonReceivables
            // 
            this.toolStripButtonReceivables.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonReceivables.Image")));
            this.toolStripButtonReceivables.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReceivables.Name = "toolStripButtonReceivables";
            this.toolStripButtonReceivables.Size = new System.Drawing.Size(128, 21);
            this.toolStripButtonReceivables.Text = "同步到总账应收款";
            this.toolStripButtonReceivables.Click += new System.EventHandler(this.toolStripButtonReceivables_Click);
            // 
            // toolStripButtonReceipts
            // 
            this.toolStripButtonReceipts.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonReceipts.Image")));
            this.toolStripButtonReceipts.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReceipts.Name = "toolStripButtonReceipts";
            this.toolStripButtonReceipts.Size = new System.Drawing.Size(128, 21);
            this.toolStripButtonReceipts.Text = "同步到总账实收款";
            this.toolStripButtonReceipts.Click += new System.EventHandler(this.toolStripButtonReceipts_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 24);
            // 
            // basicView1
            // 
            this.basicView1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.basicView1.Configuration = this.configuration1;
            this.basicView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicView1.Font = new System.Drawing.Font("黑体", 10F);
            this.basicView1.ItemsPerRow = 4;
            this.basicView1.Location = new System.Drawing.Point(0, 49);
            this.basicView1.Margin = new System.Windows.Forms.Padding(0);
            this.basicView1.Mode = "default";
            this.basicView1.Model = this.model1;
            this.basicView1.Name = "basicView1";
            this.basicView1.Padding = new System.Windows.Forms.Padding(3);
            this.basicView1.Size = new System.Drawing.Size(849, 115);
            this.basicView1.TabIndex = 13;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Location = new System.Drawing.Point(709, 547);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(111, 25);
            this.toolStrip1.TabIndex = 20;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // FormSettlementNote
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 441);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FormSettlementNote";
            this.Text = "FormSettlementNote";
            this.Load += new System.EventHandler(this.FormSettlementNote_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panelSearchWidget.ResumeLayout(false);
            this.panelPager.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton buttonItems;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelSearchWidget;
        private FrontWork.SearchView searchView1;
        private FrontWork.Configuration configuration1;
        private System.Windows.Forms.Panel panelPager;
        private FrontWork.PagerSearchJsonRESTAdapter pagerSearchJsonRESTAdapter1;
        private FrontWork.PagerView pagerView1;
        private FrontWork.JsonRESTSynchronizer synchronizer;
        private FrontWork.Model model1;
        private FrontWork.ReoGridView reoGridView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel labelStatus;
        private System.Windows.Forms.ToolStrip toolStripTop;
        private System.Windows.Forms.ToolStripButton toolStripButtonAlter;
        private FrontWork.BasicView basicView1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonReceivables;
        private System.Windows.Forms.ToolStripButton toolStripButtonReceipts;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}
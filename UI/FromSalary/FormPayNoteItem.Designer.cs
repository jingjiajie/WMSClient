namespace WMS.UI.FromSalary
{
    partial class FormPayNoteItem
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
            FrontWork.SearchViewJsonRESTAdapter.APIParamNamesType apiParamNamesType1 = new FrontWork.SearchViewJsonRESTAdapter.APIParamNamesType();
            FrontWork.SearchViewJsonRESTAdapter.ConditionFieldNamesType conditionFieldNamesType1 = new FrontWork.SearchViewJsonRESTAdapter.ConditionFieldNamesType();
            FrontWork.SearchViewJsonRESTAdapter.OrderParamNamesType orderParamNamesType1 = new FrontWork.SearchViewJsonRESTAdapter.OrderParamNamesType();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPayNoteItem));
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair1 = new FrontWork.ModeMethodListenerNamesPair();
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair2 = new FrontWork.ModeMethodListenerNamesPair();
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair3 = new FrontWork.ModeMethodListenerNamesPair();
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair4 = new FrontWork.ModeMethodListenerNamesPair();
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair5 = new FrontWork.ModeMethodListenerNamesPair();
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair6 = new FrontWork.ModeMethodListenerNamesPair();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelSearchWidget = new System.Windows.Forms.Panel();
            this.pagerSearchJsonRESTAdapter1 = new FrontWork.PagerSearchJsonRESTAdapter();
            this.pagerView1 = new FrontWork.PagerView();
            this.searchView1 = new FrontWork.SearchView();
            this.configuration1 = new FrontWork.Configuration();
            this.synchronizer = new FrontWork.JsonRESTSynchronizer();
            this.model1 = new FrontWork.Model();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.panelPager = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.reoGridView1 = new FrontWork.ReoGridView();
            this.basicView1 = new FrontWork.BasicView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.ButtonAllPerson = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAlter = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCalculateAllTax = new System.Windows.Forms.Button();
            this.buttonCclcultateItemsTax = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.panelSearchWidget.SuspendLayout();
            this.panelPager.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStripTop.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.labelStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 400);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(956, 22);
            this.statusStrip1.TabIndex = 25;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(44, 17);
            this.toolStripStatusLabel1.Text = "状态：";
            // 
            // labelStatus
            // 
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(56, 17);
            this.labelStatus.Text = "薪金项目";
            // 
            // panelSearchWidget
            // 
            this.panelSearchWidget.Controls.Add(this.pagerSearchJsonRESTAdapter1);
            this.panelSearchWidget.Controls.Add(this.configuration1);
            this.panelSearchWidget.Controls.Add(this.searchView1);
            this.panelSearchWidget.Controls.Add(this.model1);
            this.panelSearchWidget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSearchWidget.Location = new System.Drawing.Point(0, 0);
            this.panelSearchWidget.Margin = new System.Windows.Forms.Padding(0);
            this.panelSearchWidget.Name = "panelSearchWidget";
            this.panelSearchWidget.Size = new System.Drawing.Size(956, 25);
            this.panelSearchWidget.TabIndex = 12;
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
            this.pagerSearchJsonRESTAdapter1.Location = new System.Drawing.Point(735, -13);
            this.pagerSearchJsonRESTAdapter1.Margin = new System.Windows.Forms.Padding(2);
            this.pagerSearchJsonRESTAdapter1.Name = "pagerSearchJsonRESTAdapter1";
            this.pagerSearchJsonRESTAdapter1.PagerView = this.pagerView1;
            this.pagerSearchJsonRESTAdapter1.SearchView = this.searchView1;
            this.pagerSearchJsonRESTAdapter1.Size = new System.Drawing.Size(39, 38);
            this.pagerSearchJsonRESTAdapter1.Synchronizer = this.synchronizer;
            this.pagerSearchJsonRESTAdapter1.TabIndex = 4;
            this.pagerSearchJsonRESTAdapter1.Visible = false;
            // 
            // pagerView1
            // 
            this.pagerView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pagerView1.Location = new System.Drawing.Point(2, 375);
            this.pagerView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pagerView1.Mode = "default";
            this.pagerView1.Name = "pagerView1";
            this.pagerView1.PageSize = 50;
            this.pagerView1.Size = new System.Drawing.Size(952, 26);
            this.pagerView1.TabIndex = 14;
            // 
            // searchView1
            // 
            this.searchView1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.searchView1.Configuration = this.configuration1;
            this.searchView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchView1.Location = new System.Drawing.Point(0, 0);
            this.searchView1.Margin = new System.Windows.Forms.Padding(2);
            this.searchView1.Mode = "default";
            this.searchView1.Name = "searchView1";
            this.searchView1.Size = new System.Drawing.Size(956, 25);
            this.searchView1.TabIndex = 0;
            // 
            // configuration1
            // 
            this.configuration1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configuration1.ConfigurationString = resources.GetString("configuration1.ConfigurationString");
            this.configuration1.Location = new System.Drawing.Point(644, -28);
            this.configuration1.Margin = new System.Windows.Forms.Padding(2);
            modeMethodListenerNamesPair1.MethodListenerNames = new string[] {
        "FormPayNoteItem",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair1.Mode = "default";
            modeMethodListenerNamesPair2.MethodListenerNames = new string[] {
        "FormPayNoteItem",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair2.Mode = "pay";
            modeMethodListenerNamesPair3.MethodListenerNames = new string[] {
        "FormPayNoteItem",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair3.Mode = "payed";
            modeMethodListenerNamesPair4.MethodListenerNames = new string[] {
        "FormPayNoteItem",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair4.Mode = "add";
            modeMethodListenerNamesPair5.MethodListenerNames = new string[] {
        "FormPayNoteItem",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair5.Mode = "calculate";
            modeMethodListenerNamesPair6.MethodListenerNames = new string[] {
        "FormPayNoteItem",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair6.Mode = "pre-pay";
            this.configuration1.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[] {
        modeMethodListenerNamesPair1,
        modeMethodListenerNamesPair2,
        modeMethodListenerNamesPair3,
        modeMethodListenerNamesPair4,
        modeMethodListenerNamesPair5,
        modeMethodListenerNamesPair6};
            this.configuration1.Name = "configuration1";
            this.configuration1.Size = new System.Drawing.Size(180, 180);
            this.configuration1.TabIndex = 1;
            // 
            // synchronizer
            // 
            this.synchronizer.Configuration = this.configuration1;
            this.synchronizer.FieldMapping = new FrontWork.JsonRESTSynchronizer.FieldMappingItem[0];
            this.synchronizer.Location = new System.Drawing.Point(0, 403);
            this.synchronizer.Margin = new System.Windows.Forms.Padding(0);
            this.synchronizer.Mode = "default";
            this.synchronizer.Model = this.model1;
            this.synchronizer.Name = "synchronizer";
            this.synchronizer.Size = new System.Drawing.Size(180, 19);
            this.synchronizer.TabIndex = 3;
            // 
            // model1
            // 
            this.model1.AllSelectionRanges = new FrontWork.Range[0];
            this.model1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.model1.Configuration = this.configuration1;
            this.model1.Font = new System.Drawing.Font("宋体", 10F);
            this.model1.Location = new System.Drawing.Point(680, -28);
            this.model1.Margin = new System.Windows.Forms.Padding(2);
            this.model1.Mode = "default";
            this.model1.Name = "model1";
            this.model1.SelectionRange = null;
            this.model1.Size = new System.Drawing.Size(51, 73);
            this.model1.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Location = new System.Drawing.Point(712, 621);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(111, 25);
            this.toolStrip1.TabIndex = 24;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // panelPager
            // 
            this.panelPager.BackColor = System.Drawing.SystemColors.Control;
            this.panelPager.Controls.Add(this.tableLayoutPanel3);
            this.panelPager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPager.Location = new System.Drawing.Point(0, 173);
            this.panelPager.Margin = new System.Windows.Forms.Padding(0);
            this.panelPager.Name = "panelPager";
            this.panelPager.Size = new System.Drawing.Size(956, 200);
            this.panelPager.TabIndex = 8;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 191F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.reoGridView1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(956, 200);
            this.tableLayoutPanel3.TabIndex = 16;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Font = new System.Drawing.Font("黑体", 10F);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(765, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 3;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(191, 200);
            this.tableLayoutPanel5.TabIndex = 5;
            // 
            // reoGridView1
            // 
            this.reoGridView1.Configuration = this.configuration1;
            this.reoGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGridView1.Font = new System.Drawing.Font("黑体", 11F);
            this.reoGridView1.Location = new System.Drawing.Point(2, 2);
            this.reoGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.reoGridView1.Mode = "default";
            this.reoGridView1.Model = this.model1;
            this.reoGridView1.Name = "reoGridView1";
            this.reoGridView1.Size = new System.Drawing.Size(761, 196);
            this.reoGridView1.TabIndex = 0;
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
            this.basicView1.Padding = new System.Windows.Forms.Padding(3);
            this.basicView1.Size = new System.Drawing.Size(763, 120);
            this.basicView1.TabIndex = 13;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelSearchWidget, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.synchronizer, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.panelPager, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.toolStripTop, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pagerView1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(956, 422);
            this.tableLayoutPanel1.TabIndex = 26;
            // 
            // toolStripTop
            // 
            this.toolStripTop.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStripTop.BackgroundImage = global::WMS.UI.Properties.Resources.bottonW_q;
            this.toolStripTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStripTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripTop.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonAllPerson,
            this.toolStripButtonAdd,
            this.toolStripButtonDelete,
            this.toolStripButtonAlter});
            this.toolStripTop.Location = new System.Drawing.Point(0, 25);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Size = new System.Drawing.Size(956, 22);
            this.toolStripTop.TabIndex = 9;
            this.toolStripTop.Text = "toolStrip1";
            // 
            // ButtonAllPerson
            // 
            this.ButtonAllPerson.Image = ((System.Drawing.Image)(resources.GetObject("ButtonAllPerson.Image")));
            this.ButtonAllPerson.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonAllPerson.Name = "ButtonAllPerson";
            this.ButtonAllPerson.Size = new System.Drawing.Size(104, 19);
            this.ButtonAllPerson.Text = "添加所有人员";
            this.ButtonAllPerson.Visible = false;
            this.ButtonAllPerson.Click += new System.EventHandler(this.ButtonRefresh_Click);
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAdd.Image")));
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(56, 19);
            this.toolStripButtonAdd.Text = "添加";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDelete.Image")));
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(56, 19);
            this.toolStripButtonDelete.Text = "删除";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripButtonAlter
            // 
            this.toolStripButtonAlter.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAlter.Image")));
            this.toolStripButtonAlter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAlter.Name = "toolStripButtonAlter";
            this.toolStripButtonAlter.Size = new System.Drawing.Size(80, 19);
            this.toolStripButtonAlter.Text = "保存修改";
            this.toolStripButtonAlter.Click += new System.EventHandler(this.toolStripButtonAlter_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 187F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.basicView1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 50);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(950, 120);
            this.tableLayoutPanel2.TabIndex = 15;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.buttonCalculateAllTax, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.buttonCclcultateItemsTax, 1, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Font = new System.Drawing.Font("黑体", 10F);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(763, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 4;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(187, 120);
            this.tableLayoutPanel4.TabIndex = 14;
            // 
            // buttonCalculateAllTax
            // 
            this.buttonCalculateAllTax.BackColor = System.Drawing.Color.White;
            this.buttonCalculateAllTax.BackgroundImage = global::WMS.UI.Properties.Resources.bottonB2_s;
            this.buttonCalculateAllTax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonCalculateAllTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCalculateAllTax.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.buttonCalculateAllTax.FlatAppearance.BorderSize = 0;
            this.buttonCalculateAllTax.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCalculateAllTax.Location = new System.Drawing.Point(21, 63);
            this.buttonCalculateAllTax.Name = "buttonCalculateAllTax";
            this.buttonCalculateAllTax.Size = new System.Drawing.Size(144, 48);
            this.buttonCalculateAllTax.TabIndex = 2;
            this.buttonCalculateAllTax.Text = "计算整单税费";
            this.buttonCalculateAllTax.UseVisualStyleBackColor = false;
            this.buttonCalculateAllTax.Click += new System.EventHandler(this.buttonCalculateAllTax_Click);
            // 
            // buttonCclcultateItemsTax
            // 
            this.buttonCclcultateItemsTax.BackColor = System.Drawing.Color.White;
            this.buttonCclcultateItemsTax.BackgroundImage = global::WMS.UI.Properties.Resources.bottonB2_s;
            this.buttonCclcultateItemsTax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonCclcultateItemsTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCclcultateItemsTax.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.buttonCclcultateItemsTax.FlatAppearance.BorderSize = 0;
            this.buttonCclcultateItemsTax.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCclcultateItemsTax.Location = new System.Drawing.Point(21, 9);
            this.buttonCclcultateItemsTax.Name = "buttonCclcultateItemsTax";
            this.buttonCclcultateItemsTax.Size = new System.Drawing.Size(144, 48);
            this.buttonCclcultateItemsTax.TabIndex = 3;
            this.buttonCclcultateItemsTax.Text = "计算选中条目税费";
            this.buttonCclcultateItemsTax.UseVisualStyleBackColor = false;
            this.buttonCclcultateItemsTax.Click += new System.EventHandler(this.ButtonCalculateItemsTax_Click);
            // 
            // FormPayNoteItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 422);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormPayNoteItem";
            this.Text = "薪金发放单条目";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormPayNoteItem_FormClosed);
            this.Load += new System.EventHandler(this.FormPayNoteItem_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panelSearchWidget.ResumeLayout(false);
            this.panelPager.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel labelStatus;
        private System.Windows.Forms.Panel panelSearchWidget;
        private FrontWork.SearchView searchView1;
        private FrontWork.Configuration configuration1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonAlter;
        private System.Windows.Forms.Panel panelPager;
        private FrontWork.PagerSearchJsonRESTAdapter pagerSearchJsonRESTAdapter1;
        private FrontWork.PagerView pagerView1;
        private FrontWork.JsonRESTSynchronizer synchronizer;
        private FrontWork.Model model1;
        private FrontWork.ReoGridView reoGridView1;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStrip toolStripTop;
        private FrontWork.BasicView basicView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripButton ButtonAllPerson;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button buttonCclcultateItemsTax;
        private System.Windows.Forms.Button buttonCalculateAllTax;
    }
}
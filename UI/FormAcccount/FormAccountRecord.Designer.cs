namespace WMS.UI.FormAcccount
{
    partial class FormAccountRecord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAccountRecord));
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair1 = new FrontWork.ModeMethodListenerNamesPair();
            FrontWork.SearchViewJsonRESTAdapter.APIParamNamesType apiParamNamesType1 = new FrontWork.SearchViewJsonRESTAdapter.APIParamNamesType();
            FrontWork.SearchViewJsonRESTAdapter.ConditionFieldNamesType conditionFieldNamesType1 = new FrontWork.SearchViewJsonRESTAdapter.ConditionFieldNamesType();
            FrontWork.SearchViewJsonRESTAdapter.OrderParamNamesType orderParamNamesType1 = new FrontWork.SearchViewJsonRESTAdapter.OrderParamNamesType();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.labelStatus = new System.Windows.Forms.ToolStripLabel();
            this.panelPager = new System.Windows.Forms.Panel();
            this.pagerView1 = new FrontWork.PagerView();
            this.panelSearchWidget = new System.Windows.Forms.Panel();
            this.searchView1 = new FrontWork.SearchView();
            this.configuration1 = new FrontWork.Configuration();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxAccountPeriod = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAlter = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonTransfer = new System.Windows.Forms.ToolStripButton();
            this.ButtonWriteOff = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDeficit = new System.Windows.Forms.ToolStripButton();
            this.ButtonAccrualCheck = new System.Windows.Forms.ToolStripButton();
            this.buttonPreview = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.synchronizer = new FrontWork.JsonRESTSynchronizer();
            this.model1 = new FrontWork.Model();
            this.pagerSearchJsonRESTAdapter1 = new FrontWork.PagerSearchJsonRESTAdapter();
            this.reoGridView1 = new FrontWork.ReoGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.basicView1 = new FrontWork.BasicView();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxDebitAmount = new System.Windows.Forms.TextBox();
            this.textBoxCreditAmount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxBalance = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanelTreeView = new System.Windows.Forms.TableLayoutPanel();
            this.panelTreeCheck = new System.Windows.Forms.Panel();
            this.checkBoxOtherTitle = new System.Windows.Forms.CheckBox();
            this.checkBoxOwnTitle = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.treeViewAccountTitle = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.miniToolStrip = new System.Windows.Forms.ToolStrip();
            this.tableLayoutPanel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panelPager.SuspendLayout();
            this.panelSearchWidget.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.toolStripTop.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanelTreeView.SuspendLayout();
            this.panelTreeCheck.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.toolStrip1, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.panelPager, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.panelSearchWidget, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel8, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1018, 494);
            this.tableLayoutPanel2.TabIndex = 12;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.labelStatus});
            this.toolStrip1.Location = new System.Drawing.Point(0, 469);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1018, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(35, 22);
            this.toolStripStatusLabel1.Text = "状态:";
            // 
            // labelStatus
            // 
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(56, 22);
            this.labelStatus.Text = "账目记录";
            // 
            // panelPager
            // 
            this.panelPager.Controls.Add(this.pagerView1);
            this.panelPager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPager.Location = new System.Drawing.Point(0, 439);
            this.panelPager.Margin = new System.Windows.Forms.Padding(0);
            this.panelPager.Name = "panelPager";
            this.panelPager.Size = new System.Drawing.Size(1018, 30);
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
            this.pagerView1.Size = new System.Drawing.Size(1018, 30);
            this.pagerView1.TabIndex = 16;
            // 
            // panelSearchWidget
            // 
            this.panelSearchWidget.Controls.Add(this.searchView1);
            this.panelSearchWidget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSearchWidget.Location = new System.Drawing.Point(0, 0);
            this.panelSearchWidget.Margin = new System.Windows.Forms.Padding(0);
            this.panelSearchWidget.Name = "panelSearchWidget";
            this.panelSearchWidget.Size = new System.Drawing.Size(1018, 25);
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
            this.searchView1.Size = new System.Drawing.Size(1018, 25);
            this.searchView1.TabIndex = 2;
            // 
            // configuration1
            // 
            this.configuration1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configuration1.ConfigurationString = resources.GetString("configuration1.ConfigurationString");
            this.configuration1.Location = new System.Drawing.Point(28, 65);
            this.configuration1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            modeMethodListenerNamesPair1.MethodListenerNames = new string[] {
        "FormAccountRecord",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair1.Mode = "default";
            this.configuration1.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[] {
        modeMethodListenerNamesPair1};
            this.configuration1.Name = "configuration1";
            this.configuration1.Size = new System.Drawing.Size(135, 144);
            this.configuration1.TabIndex = 6;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 255F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.toolStripTop, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1018, 25);
            this.tableLayoutPanel3.TabIndex = 9;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tableLayoutPanel5.BackgroundImage = global::WMS.UI.Properties.Resources.bottonW_q;
            this.tableLayoutPanel5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.tableLayoutPanel5.ColumnCount = 4;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.comboBoxAccountPeriod, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(763, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(255, 25);
            this.tableLayoutPanel5.TabIndex = 5;
            // 
            // comboBoxAccountPeriod
            // 
            this.comboBoxAccountPeriod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxAccountPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAccountPeriod.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.comboBoxAccountPeriod.FormattingEnabled = true;
            this.comboBoxAccountPeriod.Location = new System.Drawing.Point(109, 2);
            this.comboBoxAccountPeriod.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxAccountPeriod.Name = "comboBoxAccountPeriod";
            this.comboBoxAccountPeriod.Size = new System.Drawing.Size(108, 27);
            this.comboBoxAccountPeriod.TabIndex = 0;
            this.comboBoxAccountPeriod.SelectedIndexChanged += new System.EventHandler(this.comboBoxAccountPeriod_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(37, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "会计期间";
            // 
            // toolStripTop
            // 
            this.toolStripTop.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStripTop.BackgroundImage = global::WMS.UI.Properties.Resources.bottonW_q;
            this.toolStripTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.toolStripTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripTop.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAdd,
            this.toolStripButtonDelete,
            this.toolStripButtonAlter,
            this.toolStripSeparator3,
            this.ButtonTransfer,
            this.ButtonWriteOff,
            this.toolStripButtonDeficit,
            this.ButtonAccrualCheck,
            this.buttonPreview});
            this.toolStripTop.Location = new System.Drawing.Point(0, 0);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Size = new System.Drawing.Size(763, 25);
            this.toolStripTop.TabIndex = 3;
            this.toolStripTop.Text = "toolStrip1";
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAdd.Image")));
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(56, 22);
            this.toolStripButtonAdd.Text = "添加";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDelete.Image")));
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(56, 22);
            this.toolStripButtonDelete.Text = "删除";
            this.toolStripButtonDelete.Visible = false;
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripButtonAlter
            // 
            this.toolStripButtonAlter.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAlter.Image")));
            this.toolStripButtonAlter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAlter.Name = "toolStripButtonAlter";
            this.toolStripButtonAlter.Size = new System.Drawing.Size(80, 22);
            this.toolStripButtonAlter.Text = "保存修改";
            this.toolStripButtonAlter.Click += new System.EventHandler(this.toolStripButtonAlter_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AutoSize = false;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(15, 28);
            // 
            // ButtonTransfer
            // 
            this.ButtonTransfer.Image = global::WMS.UI.Properties.Resources.cancle;
            this.ButtonTransfer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonTransfer.Name = "ButtonTransfer";
            this.ButtonTransfer.Size = new System.Drawing.Size(56, 22);
            this.ButtonTransfer.Text = "转账";
            this.ButtonTransfer.Visible = false;
            this.ButtonTransfer.Click += new System.EventHandler(this.ButtonTransfer_Click);
            // 
            // ButtonWriteOff
            // 
            this.ButtonWriteOff.Image = global::WMS.UI.Properties.Resources.cancle;
            this.ButtonWriteOff.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonWriteOff.Name = "ButtonWriteOff";
            this.ButtonWriteOff.Size = new System.Drawing.Size(104, 22);
            this.ButtonWriteOff.Text = "冲销当前条目";
            this.ButtonWriteOff.Click += new System.EventHandler(this.ButtonWriteOff_Click);
            // 
            // toolStripButtonDeficit
            // 
            this.toolStripButtonDeficit.Image = global::WMS.UI.Properties.Resources.check;
            this.toolStripButtonDeficit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDeficit.Name = "toolStripButtonDeficit";
            this.toolStripButtonDeficit.Size = new System.Drawing.Size(80, 22);
            this.toolStripButtonDeficit.Text = "赤字提醒";
            this.toolStripButtonDeficit.Click += new System.EventHandler(this.toolStripButtonDeficit_Click);
            // 
            // ButtonAccrualCheck
            // 
            this.ButtonAccrualCheck.Image = global::WMS.UI.Properties.Resources.check;
            this.ButtonAccrualCheck.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonAccrualCheck.Name = "ButtonAccrualCheck";
            this.ButtonAccrualCheck.Size = new System.Drawing.Size(80, 22);
            this.ButtonAccrualCheck.Text = "自动对账";
            this.ButtonAccrualCheck.Click += new System.EventHandler(this.ButtonAccrualCheck_Click);
            // 
            // buttonPreview
            // 
            this.buttonPreview.Image = ((System.Drawing.Image)(resources.GetObject("buttonPreview.Image")));
            this.buttonPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Size = new System.Drawing.Size(121, 22);
            this.buttonPreview.Text = "导出/打印汇总单";
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel9, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanelTreeView, 1, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(0, 50);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(1018, 389);
            this.tableLayoutPanel8.TabIndex = 11;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 1;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 2;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 118F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(838, 389);
            this.tableLayoutPanel9.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.synchronizer);
            this.panel2.Controls.Add(this.pagerSearchJsonRESTAdapter1);
            this.panel2.Controls.Add(this.model1);
            this.panel2.Controls.Add(this.configuration1);
            this.panel2.Controls.Add(this.reoGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 118);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(838, 271);
            this.panel2.TabIndex = 9;
            // 
            // synchronizer
            // 
            this.synchronizer.Configuration = this.configuration1;
            this.synchronizer.FieldMapping = new FrontWork.JsonRESTSynchronizer.FieldMappingItem[0];
            this.synchronizer.Location = new System.Drawing.Point(343, 46);
            this.synchronizer.Margin = new System.Windows.Forms.Padding(0);
            this.synchronizer.Mode = "default";
            this.synchronizer.Model = this.model1;
            this.synchronizer.Name = "synchronizer";
            this.synchronizer.Size = new System.Drawing.Size(135, 144);
            this.synchronizer.TabIndex = 10;
            // 
            // model1
            // 
            this.model1.AllSelectionRanges = new FrontWork.Range[0];
            this.model1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.model1.Configuration = this.configuration1;
            this.model1.Font = new System.Drawing.Font("宋体", 10F);
            this.model1.Location = new System.Drawing.Point(200, 37);
            this.model1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.model1.Mode = "default";
            this.model1.Name = "model1";
            this.model1.SelectionRange = null;
            this.model1.Size = new System.Drawing.Size(109, 116);
            this.model1.TabIndex = 7;
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
            this.pagerSearchJsonRESTAdapter1.Location = new System.Drawing.Point(555, 55);
            this.pagerSearchJsonRESTAdapter1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pagerSearchJsonRESTAdapter1.Name = "pagerSearchJsonRESTAdapter1";
            this.pagerSearchJsonRESTAdapter1.PagerView = this.pagerView1;
            this.pagerSearchJsonRESTAdapter1.SearchView = this.searchView1;
            this.pagerSearchJsonRESTAdapter1.Size = new System.Drawing.Size(105, 98);
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
            this.reoGridView1.Size = new System.Drawing.Size(838, 271);
            this.reoGridView1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(838, 118);
            this.panel1.TabIndex = 10;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.33753F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.66247F));
            this.tableLayoutPanel6.Controls.Add(this.basicView1, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel7, 1, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(838, 118);
            this.tableLayoutPanel6.TabIndex = 11;
            // 
            // basicView1
            // 
            this.basicView1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.basicView1.Configuration = this.configuration1;
            this.basicView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicView1.Font = new System.Drawing.Font("黑体", 10F);
            this.basicView1.ItemsPerRow = 4;
            this.basicView1.Location = new System.Drawing.Point(0, 0);
            this.basicView1.Margin = new System.Windows.Forms.Padding(0);
            this.basicView1.Mode = "default";
            this.basicView1.Model = this.model1;
            this.basicView1.Name = "basicView1";
            this.basicView1.Size = new System.Drawing.Size(631, 118);
            this.basicView1.TabIndex = 18;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tableLayoutPanel7.ColumnCount = 4;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Controls.Add(this.textBoxDebitAmount, 2, 2);
            this.tableLayoutPanel7.Controls.Add(this.textBoxCreditAmount, 2, 3);
            this.tableLayoutPanel7.Controls.Add(this.label4, 1, 3);
            this.tableLayoutPanel7.Controls.Add(this.label5, 1, 1);
            this.tableLayoutPanel7.Controls.Add(this.textBoxBalance, 2, 1);
            this.tableLayoutPanel7.Controls.Add(this.label3, 1, 2);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(631, 0);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 5;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(207, 118);
            this.tableLayoutPanel7.TabIndex = 19;
            // 
            // textBoxDebitAmount
            // 
            this.textBoxDebitAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDebitAmount.Enabled = false;
            this.textBoxDebitAmount.Location = new System.Drawing.Point(87, 46);
            this.textBoxDebitAmount.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxDebitAmount.Name = "textBoxDebitAmount";
            this.textBoxDebitAmount.Size = new System.Drawing.Size(108, 21);
            this.textBoxDebitAmount.TabIndex = 0;
            // 
            // textBoxCreditAmount
            // 
            this.textBoxCreditAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCreditAmount.Enabled = false;
            this.textBoxCreditAmount.Location = new System.Drawing.Point(87, 76);
            this.textBoxCreditAmount.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxCreditAmount.Name = "textBoxCreditAmount";
            this.textBoxCreditAmount.Size = new System.Drawing.Size(108, 21);
            this.textBoxCreditAmount.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(10, 74);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 30);
            this.label4.TabIndex = 3;
            this.label4.Text = "贷方发生额";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(13, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 30);
            this.label5.TabIndex = 4;
            this.label5.Text = "科目总余额";
            // 
            // textBoxBalance
            // 
            this.textBoxBalance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxBalance.Location = new System.Drawing.Point(88, 17);
            this.textBoxBalance.Name = "textBoxBalance";
            this.textBoxBalance.Size = new System.Drawing.Size(106, 21);
            this.textBoxBalance.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(10, 44);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 30);
            this.label3.TabIndex = 2;
            this.label3.Text = "借方发生额";
            // 
            // tableLayoutPanelTreeView
            // 
            this.tableLayoutPanelTreeView.ColumnCount = 1;
            this.tableLayoutPanelTreeView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTreeView.Controls.Add(this.panelTreeCheck, 0, 0);
            this.tableLayoutPanelTreeView.Controls.Add(this.panel4, 0, 1);
            this.tableLayoutPanelTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTreeView.Location = new System.Drawing.Point(838, 0);
            this.tableLayoutPanelTreeView.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelTreeView.Name = "tableLayoutPanelTreeView";
            this.tableLayoutPanelTreeView.RowCount = 2;
            this.tableLayoutPanelTreeView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanelTreeView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTreeView.Size = new System.Drawing.Size(180, 389);
            this.tableLayoutPanelTreeView.TabIndex = 1;
            // 
            // panelTreeCheck
            // 
            this.panelTreeCheck.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panelTreeCheck.Controls.Add(this.checkBoxOtherTitle);
            this.panelTreeCheck.Controls.Add(this.checkBoxOwnTitle);
            this.panelTreeCheck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTreeCheck.Location = new System.Drawing.Point(0, 0);
            this.panelTreeCheck.Margin = new System.Windows.Forms.Padding(0);
            this.panelTreeCheck.Name = "panelTreeCheck";
            this.panelTreeCheck.Size = new System.Drawing.Size(180, 50);
            this.panelTreeCheck.TabIndex = 0;
            // 
            // checkBoxOtherTitle
            // 
            this.checkBoxOtherTitle.AutoSize = true;
            this.checkBoxOtherTitle.Location = new System.Drawing.Point(88, 17);
            this.checkBoxOtherTitle.Name = "checkBoxOtherTitle";
            this.checkBoxOtherTitle.Size = new System.Drawing.Size(72, 16);
            this.checkBoxOtherTitle.TabIndex = 1;
            this.checkBoxOtherTitle.Text = "对方科目";
            this.checkBoxOtherTitle.UseVisualStyleBackColor = true;
            this.checkBoxOtherTitle.CheckedChanged += new System.EventHandler(this.OtherTitleCheckChanged);
            // 
            // checkBoxOwnTitle
            // 
            this.checkBoxOwnTitle.AutoSize = true;
            this.checkBoxOwnTitle.Checked = true;
            this.checkBoxOwnTitle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOwnTitle.Location = new System.Drawing.Point(7, 17);
            this.checkBoxOwnTitle.Name = "checkBoxOwnTitle";
            this.checkBoxOwnTitle.Size = new System.Drawing.Size(72, 16);
            this.checkBoxOwnTitle.TabIndex = 0;
            this.checkBoxOwnTitle.Text = "己方科目";
            this.checkBoxOwnTitle.UseVisualStyleBackColor = true;
            this.checkBoxOwnTitle.CheckedChanged += new System.EventHandler(this.OwnTitleCheckChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.treeViewAccountTitle);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 50);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(180, 339);
            this.panel4.TabIndex = 1;
            // 
            // treeViewAccountTitle
            // 
            this.treeViewAccountTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewAccountTitle.Location = new System.Drawing.Point(0, 0);
            this.treeViewAccountTitle.Margin = new System.Windows.Forms.Padding(0);
            this.treeViewAccountTitle.Name = "treeViewAccountTitle";
            this.treeViewAccountTitle.Size = new System.Drawing.Size(180, 339);
            this.treeViewAccountTitle.TabIndex = 2;
            this.treeViewAccountTitle.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewAccountTitle_AfterSelect);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1018, 494);
            this.tableLayoutPanel1.TabIndex = 11;
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
            this.miniToolStrip.Size = new System.Drawing.Size(509, 25);
            this.miniToolStrip.TabIndex = 3;
            // 
            // FormAccountRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 494);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormAccountRecord";
            this.Text = "FormAccountRecord";
            this.Load += new System.EventHandler(this.FormAccountRecord_Load);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panelPager.ResumeLayout(false);
            this.panelSearchWidget.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.tableLayoutPanelTreeView.ResumeLayout(false);
            this.panelTreeCheck.ResumeLayout(false);
            this.panelTreeCheck.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripLabel labelStatus;
        private System.Windows.Forms.Panel panelPager;
        private FrontWork.PagerView pagerView1;
        private System.Windows.Forms.Panel panelSearchWidget;
        private FrontWork.SearchView searchView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.Panel panel2;
        private FrontWork.JsonRESTSynchronizer synchronizer;
        private FrontWork.Configuration configuration1;
        private FrontWork.Model model1;
        private FrontWork.PagerSearchJsonRESTAdapter pagerSearchJsonRESTAdapter1;
        private FrontWork.ReoGridView reoGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private FrontWork.BasicView basicView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.TextBox textBoxDebitAmount;
        private System.Windows.Forms.TextBox textBoxCreditAmount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxBalance;
        private System.Windows.Forms.ToolStrip miniToolStrip;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.ComboBox comboBoxAccountPeriod;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStrip toolStripTop;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonAlter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton ButtonTransfer;
        private System.Windows.Forms.ToolStripButton ButtonWriteOff;
        private System.Windows.Forms.ToolStripButton toolStripButtonDeficit;
        private System.Windows.Forms.ToolStripButton ButtonAccrualCheck;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTreeView;
        private System.Windows.Forms.Panel panelTreeCheck;
        private System.Windows.Forms.CheckBox checkBoxOtherTitle;
        private System.Windows.Forms.CheckBox checkBoxOwnTitle;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TreeView treeViewAccountTitle;
        private System.Windows.Forms.ToolStripButton buttonPreview;
    }
}
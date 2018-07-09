namespace WMS.UI.FromSalary
{
    partial class FormSelectTax
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectTax));
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair1 = new FrontWork.ModeMethodListenerNamesPair();
            FrontWork.SearchViewJsonRESTAdapter.APIParamNamesType apiParamNamesType1 = new FrontWork.SearchViewJsonRESTAdapter.APIParamNamesType();
            FrontWork.SearchViewJsonRESTAdapter.ConditionFieldNamesType conditionFieldNamesType1 = new FrontWork.SearchViewJsonRESTAdapter.ConditionFieldNamesType();
            FrontWork.SearchViewJsonRESTAdapter.OrderParamNamesType orderParamNamesType1 = new FrontWork.SearchViewJsonRESTAdapter.OrderParamNamesType();
            this.basicView1 = new FrontWork.BasicView();
            this.configuration1 = new FrontWork.Configuration();
            this.model1 = new FrontWork.Model();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.searchView1 = new FrontWork.SearchView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonADD = new System.Windows.Forms.Button();
            this.synchronizer = new FrontWork.JsonRESTSynchronizer();
            this.pagerSearchJsonRESTAdapter1 = new FrontWork.PagerSearchJsonRESTAdapter();
            this.pagerView1 = new FrontWork.PagerView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // basicView1
            // 
            this.basicView1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.basicView1.Configuration = this.configuration1;
            this.basicView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicView1.Font = new System.Drawing.Font("黑体", 10F);
            this.basicView1.ItemsPerRow = 3;
            this.basicView1.Location = new System.Drawing.Point(0, 0);
            this.basicView1.Margin = new System.Windows.Forms.Padding(0);
            this.basicView1.Mode = "default";
            this.basicView1.Model = this.model1;
            this.basicView1.Name = "basicView1";
            this.basicView1.Size = new System.Drawing.Size(780, 80);
            this.basicView1.TabIndex = 20;
            // 
            // configuration1
            // 
            this.configuration1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configuration1.ConfigurationString = resources.GetString("configuration1.ConfigurationString");
            this.configuration1.Location = new System.Drawing.Point(2, 2);
            this.configuration1.Margin = new System.Windows.Forms.Padding(2);
            modeMethodListenerNamesPair1.MethodListenerNames = new string[] {
        "FormSelectPakage",
        "AssociationMethodListener"};
            modeMethodListenerNamesPair1.Mode = "default";
            this.configuration1.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[] {
        modeMethodListenerNamesPair1};
            this.configuration1.Name = "configuration1";
            this.configuration1.Size = new System.Drawing.Size(180, 129);
            this.configuration1.TabIndex = 17;
            // 
            // model1
            // 
            this.model1.AllSelectionRanges = new FrontWork.Range[0];
            this.model1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.model1.Configuration = this.configuration1;
            this.model1.Font = new System.Drawing.Font("宋体", 10F);
            this.model1.Location = new System.Drawing.Point(2, 175);
            this.model1.Margin = new System.Windows.Forms.Padding(2);
            this.model1.Mode = "default";
            this.model1.Name = "model1";
            this.model1.SelectionRange = null;
            this.model1.Size = new System.Drawing.Size(196, 94);
            this.model1.TabIndex = 18;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(780, 386);
            this.tableLayoutPanel2.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.basicView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(780, 80);
            this.panel1.TabIndex = 7;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.searchView1, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.model1, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.panel2, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.configuration1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.synchronizer, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.pagerSearchJsonRESTAdapter1, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.pagerView1, 1, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 80);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(780, 306);
            this.tableLayoutPanel3.TabIndex = 8;
            // 
            // searchView1
            // 
            this.searchView1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.searchView1.Configuration = this.configuration1;
            this.searchView1.Location = new System.Drawing.Point(317, 2);
            this.searchView1.Margin = new System.Windows.Forms.Padding(2);
            this.searchView1.Mode = "default";
            this.searchView1.Name = "searchView1";
            this.searchView1.Size = new System.Drawing.Size(146, 32);
            this.searchView1.TabIndex = 21;
            this.searchView1.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonADD);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(315, 133);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(150, 40);
            this.panel2.TabIndex = 0;
            // 
            // buttonADD
            // 
            this.buttonADD.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonADD.BackgroundImage")));
            this.buttonADD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonADD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonADD.FlatAppearance.BorderSize = 0;
            this.buttonADD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonADD.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.buttonADD.Location = new System.Drawing.Point(0, 0);
            this.buttonADD.Margin = new System.Windows.Forms.Padding(4);
            this.buttonADD.Name = "buttonADD";
            this.buttonADD.Size = new System.Drawing.Size(150, 40);
            this.buttonADD.TabIndex = 11;
            this.buttonADD.Text = "按套餐添加";
            this.buttonADD.UseVisualStyleBackColor = true;
            this.buttonADD.Click += new System.EventHandler(this.buttonADD_Click);
            // 
            // synchronizer
            // 
            this.synchronizer.Configuration = this.configuration1;
            this.synchronizer.FieldMapping = new FrontWork.JsonRESTSynchronizer.FieldMappingItem[0];
            this.synchronizer.Location = new System.Drawing.Point(465, 173);
            this.synchronizer.Margin = new System.Windows.Forms.Padding(0);
            this.synchronizer.Mode = "default";
            this.synchronizer.Model = this.model1;
            this.synchronizer.Name = "synchronizer";
            this.synchronizer.Size = new System.Drawing.Size(180, 133);
            this.synchronizer.TabIndex = 22;
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
            this.pagerSearchJsonRESTAdapter1.Location = new System.Drawing.Point(467, 2);
            this.pagerSearchJsonRESTAdapter1.Margin = new System.Windows.Forms.Padding(2);
            this.pagerSearchJsonRESTAdapter1.Name = "pagerSearchJsonRESTAdapter1";
            this.pagerSearchJsonRESTAdapter1.PagerView = this.pagerView1;
            this.pagerSearchJsonRESTAdapter1.SearchView = this.searchView1;
            this.pagerSearchJsonRESTAdapter1.Size = new System.Drawing.Size(90, 81);
            this.pagerSearchJsonRESTAdapter1.Synchronizer = this.synchronizer;
            this.pagerSearchJsonRESTAdapter1.TabIndex = 21;
            this.pagerSearchJsonRESTAdapter1.Visible = false;
            // 
            // pagerView1
            // 
            this.pagerView1.BackColor = System.Drawing.SystemColors.Control;
            this.pagerView1.Location = new System.Drawing.Point(315, 173);
            this.pagerView1.Margin = new System.Windows.Forms.Padding(0);
            this.pagerView1.Mode = "default";
            this.pagerView1.Name = "pagerView1";
            this.pagerView1.PageSize = 50;
            this.pagerView1.Size = new System.Drawing.Size(117, 10);
            this.pagerView1.TabIndex = 23;
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(780, 386);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // FormSelectTax
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 386);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormSelectTax";
            this.Text = "FormSelectTax";
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FrontWork.BasicView basicView1;
        private FrontWork.Configuration configuration1;
        private FrontWork.Model model1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonADD;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private FrontWork.PagerSearchJsonRESTAdapter pagerSearchJsonRESTAdapter1;
        private FrontWork.JsonRESTSynchronizer synchronizer;
        private FrontWork.SearchView searchView1;
        private FrontWork.PagerView pagerView1;
    }
}
namespace Test
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            FrontWork.ModeMethodListenerNamesPair modeMethodListenerNamesPair2 = new FrontWork.ModeMethodListenerNamesPair();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.targetConfiguration = new FrontWork.Configuration();
            this.targetModel = new FrontWork.Model();
            this.reoGridView2 = new FrontWork.ReoGridView();
            this.sourceConfiguration = new FrontWork.Configuration();
            this.sourceModel = new FrontWork.Model();
            this.reoGridView1 = new FrontWork.ReoGridView();
            this.pivotTableAdapter1 = new FrontWork.PivotTableAdapter(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("黑体", 15F);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(935, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "FrontWork测试项目。如果此窗口被启动，请将UI项目“设为启动项目”再运行。";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(644, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "目标表";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(175, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "原始表";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(372, 402);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(151, 42);
            this.button1.TabIndex = 9;
            this.button1.Text = "TestAdd";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // targetConfiguration
            // 
            this.targetConfiguration.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.targetConfiguration.ConfigurationString = "[\r\n    {mode:\"default\",\r\n     fields:[\r\n     ]\r\n    }\r\n]";
            this.targetConfiguration.Location = new System.Drawing.Point(569, 196);
            this.targetConfiguration.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[0];
            this.targetConfiguration.Name = "targetConfiguration";
            this.targetConfiguration.Size = new System.Drawing.Size(180, 180);
            this.targetConfiguration.TabIndex = 4;
            // 
            // targetModel
            // 
            this.targetModel.AllSelectionRanges = new FrontWork.Range[0];
            this.targetModel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.targetModel.Configuration = this.targetConfiguration;
            this.targetModel.Font = new System.Drawing.Font("宋体", 10F);
            this.targetModel.Location = new System.Drawing.Point(569, 352);
            this.targetModel.Mode = "default";
            this.targetModel.Name = "targetModel";
            this.targetModel.SelectionRange = null;
            this.targetModel.Size = new System.Drawing.Size(150, 150);
            this.targetModel.TabIndex = 2;
            // 
            // reoGridView2
            // 
            this.reoGridView2.Configuration = this.targetConfiguration;
            this.reoGridView2.Font = new System.Drawing.Font("黑体", 11F);
            this.reoGridView2.Location = new System.Drawing.Point(592, 84);
            this.reoGridView2.Mode = "default";
            this.reoGridView2.Model = this.targetModel;
            this.reoGridView2.Name = "reoGridView2";
            this.reoGridView2.Size = new System.Drawing.Size(642, 292);
            this.reoGridView2.TabIndex = 6;
            // 
            // sourceConfiguration
            // 
            this.sourceConfiguration.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.sourceConfiguration.ConfigurationString = "[\r\n    {mode:\"default\",\r\n     fields:[\r\n        {name:\"姓名\"},\r\n        {name:\"科目1\"" +
    "},\r\n        {name:\"科目2\"},\r\n        {name:\"成绩1\",editEnded:\"#testEditEnded\"},\r\n   " +
    "     {name:\"成绩2\"}\r\n     ]\r\n    }\r\n]";
            this.sourceConfiguration.Location = new System.Drawing.Point(216, 237);
            modeMethodListenerNamesPair2.MethodListenerNames = new string[] {
        "Form1"};
            modeMethodListenerNamesPair2.Mode = "default";
            this.sourceConfiguration.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[] {
        modeMethodListenerNamesPair2};
            this.sourceConfiguration.Name = "sourceConfiguration";
            this.sourceConfiguration.Size = new System.Drawing.Size(180, 180);
            this.sourceConfiguration.TabIndex = 3;
            // 
            // sourceModel
            // 
            this.sourceModel.AllSelectionRanges = new FrontWork.Range[0];
            this.sourceModel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.sourceModel.Configuration = this.sourceConfiguration;
            this.sourceModel.Font = new System.Drawing.Font("宋体", 10F);
            this.sourceModel.Location = new System.Drawing.Point(216, 379);
            this.sourceModel.Mode = "default";
            this.sourceModel.Name = "sourceModel";
            this.sourceModel.SelectionRange = null;
            this.sourceModel.Size = new System.Drawing.Size(150, 150);
            this.sourceModel.TabIndex = 1;
            // 
            // reoGridView1
            // 
            this.reoGridView1.Configuration = this.sourceConfiguration;
            this.reoGridView1.Font = new System.Drawing.Font("黑体", 11F);
            this.reoGridView1.Location = new System.Drawing.Point(5, 84);
            this.reoGridView1.Mode = "default";
            this.reoGridView1.Model = this.sourceModel;
            this.reoGridView1.Name = "reoGridView1";
            this.reoGridView1.Size = new System.Drawing.Size(558, 289);
            this.reoGridView1.TabIndex = 5;
            // 
            // pivotTableAdapter1
            // 
            this.pivotTableAdapter1.ColumnNamesAsColumn = new string[] {
        "科目1",
        "科目2"};
            this.pivotTableAdapter1.ColumnNamesAsRow = new string[] {
        "姓名"};
            this.pivotTableAdapter1.ColumnNamesAsValue = new string[] {
        "成绩1",
        "成绩2"};
            this.pivotTableAdapter1.SourceMode = "default";
            this.pivotTableAdapter1.SourceModel = this.sourceModel;
            this.pivotTableAdapter1.TargetMode = "default";
            this.pivotTableAdapter1.TargetModel = this.targetModel;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(562, 403);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(157, 41);
            this.button2.TabIndex = 10;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1246, 532);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.targetConfiguration);
            this.Controls.Add(this.targetModel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.reoGridView2);
            this.Controls.Add(this.sourceConfiguration);
            this.Controls.Add(this.sourceModel);
            this.Controls.Add(this.reoGridView1);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "测试项目";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private FrontWork.Model sourceModel;
        private FrontWork.Model targetModel;
        private FrontWork.Configuration sourceConfiguration;
        private FrontWork.Configuration targetConfiguration;
        private FrontWork.PivotTableAdapter pivotTableAdapter1;
        private FrontWork.ReoGridView reoGridView1;
        private FrontWork.ReoGridView reoGridView2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}


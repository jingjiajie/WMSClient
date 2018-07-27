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
            this.label1 = new System.Windows.Forms.Label();
            this.model1 = new FrontWork.Model();
            this.configuration1 = new FrontWork.Configuration();
            this.model2 = new FrontWork.Model();
            this.configuration2 = new FrontWork.Configuration();
            this.pivotTableAdapter1 = new FrontWork.PivotTableAdapter(this.components);
            this.reoGridView1 = new FrontWork.ReoGridView();
            this.reoGridView2 = new FrontWork.ReoGridView();
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
            // model1
            // 
            this.model1.AllSelectionRanges = new FrontWork.Range[0];
            this.model1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.model1.Configuration = this.configuration1;
            this.model1.Font = new System.Drawing.Font("宋体", 10F);
            this.model1.Location = new System.Drawing.Point(84, 192);
            this.model1.Mode = "default";
            this.model1.Name = "model1";
            this.model1.SelectionRange = null;
            this.model1.Size = new System.Drawing.Size(150, 150);
            this.model1.TabIndex = 1;
            // 
            // configuration1
            // 
            this.configuration1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configuration1.ConfigurationString = "[\r\n    {mode:\"default\",\r\n     fields:[\r\n        {name:\"姓名\"},\r\n        {name:\"科目\"}" +
    ",\r\n        {name:\"成绩\"},\r\n     ]\r\n    }\r\n]";
            this.configuration1.Location = new System.Drawing.Point(84, 348);
            this.configuration1.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[0];
            this.configuration1.Name = "configuration1";
            this.configuration1.Size = new System.Drawing.Size(150, 150);
            this.configuration1.TabIndex = 2;
            // 
            // model2
            // 
            this.model2.AllSelectionRanges = new FrontWork.Range[0];
            this.model2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.model2.Configuration = this.configuration2;
            this.model2.Font = new System.Drawing.Font("宋体", 10F);
            this.model2.Location = new System.Drawing.Point(323, 192);
            this.model2.Mode = "default";
            this.model2.Name = "model2";
            this.model2.SelectionRange = null;
            this.model2.Size = new System.Drawing.Size(150, 150);
            this.model2.TabIndex = 3;
            // 
            // configuration2
            // 
            this.configuration2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configuration2.ConfigurationString = "[\r\n    {mode:\"default\",\r\n     fields:[\r\n     ]\r\n    }\r\n]";
            this.configuration2.Location = new System.Drawing.Point(332, 348);
            this.configuration2.MethodListeners = new FrontWork.ModeMethodListenerNamesPair[0];
            this.configuration2.Name = "configuration2";
            this.configuration2.Size = new System.Drawing.Size(150, 150);
            this.configuration2.TabIndex = 4;
            // 
            // pivotTableAdapter1
            // 
            this.pivotTableAdapter1.ColumnNamesAsColumn = new string[] {
        "科目"};
            this.pivotTableAdapter1.ColumnNamesAsRow = new string[] {
        "姓名"};
            this.pivotTableAdapter1.ColumnNamesAsValue = new string[] {
        "成绩"};
            this.pivotTableAdapter1.SourceMode = "default";
            this.pivotTableAdapter1.SourceModel = this.model1;
            this.pivotTableAdapter1.TargetMode = "default";
            this.pivotTableAdapter1.TargetModel = this.model2;
            // 
            // reoGridView1
            // 
            this.reoGridView1.Configuration = this.configuration1;
            this.reoGridView1.Font = new System.Drawing.Font("黑体", 11F);
            this.reoGridView1.Location = new System.Drawing.Point(5, 28);
            this.reoGridView1.Mode = "default";
            this.reoGridView1.Model = this.model1;
            this.reoGridView1.Name = "reoGridView1";
            this.reoGridView1.Size = new System.Drawing.Size(468, 409);
            this.reoGridView1.TabIndex = 5;
            // 
            // reoGridView2
            // 
            this.reoGridView2.Configuration = this.configuration2;
            this.reoGridView2.Font = new System.Drawing.Font("黑体", 11F);
            this.reoGridView2.Location = new System.Drawing.Point(466, 28);
            this.reoGridView2.Mode = "default";
            this.reoGridView2.Model = this.model2;
            this.reoGridView2.Name = "reoGridView2";
            this.reoGridView2.Size = new System.Drawing.Size(911, 523);
            this.reoGridView2.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1246, 532);
            this.Controls.Add(this.reoGridView2);
            this.Controls.Add(this.reoGridView1);
            this.Controls.Add(this.configuration2);
            this.Controls.Add(this.model2);
            this.Controls.Add(this.configuration1);
            this.Controls.Add(this.model1);
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
        private FrontWork.Model model1;
        private FrontWork.Configuration configuration1;
        private FrontWork.Model model2;
        private FrontWork.Configuration configuration2;
        private FrontWork.PivotTableAdapter pivotTableAdapter1;
        private FrontWork.ReoGridView reoGridView1;
        private FrontWork.ReoGridView reoGridView2;
    }
}


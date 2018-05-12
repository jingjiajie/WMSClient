namespace WMS.UI
{
    partial class FormSubmissionChooseExcelType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSubmissionChooseExcelType));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonPass = new System.Windows.Forms.Button();
            this.buttonAll = new System.Windows.Forms.Button();
            this.buttonNoPass = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel1.Controls.Add(this.buttonPass, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonAll, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonNoPass, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(385, 98);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // buttonPass
            // 
            this.buttonPass.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonPass.BackgroundImage")));
            this.buttonPass.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonPass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPass.FlatAppearance.BorderSize = 0;
            this.buttonPass.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonPass.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonPass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPass.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.buttonPass.Location = new System.Drawing.Point(3, 2);
            this.buttonPass.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonPass.Name = "buttonPass";
            this.buttonPass.Size = new System.Drawing.Size(122, 94);
            this.buttonPass.TabIndex = 0;
            this.buttonPass.Text = "合格条目";
            this.buttonPass.UseVisualStyleBackColor = true;
            this.buttonPass.Click += new System.EventHandler(this.buttonPass_Click);
            this.buttonPass.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonPass_MouseDown);
            this.buttonPass.MouseEnter += new System.EventHandler(this.buttonPass_MouseEnter);
            this.buttonPass.MouseLeave += new System.EventHandler(this.buttonPass_MouseLeave);
            // 
            // buttonAll
            // 
            this.buttonAll.BackgroundImage = global::WMS.UI.Properties.Resources.bottonB2_q;
            this.buttonAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAll.FlatAppearance.BorderSize = 0;
            this.buttonAll.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAll.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.buttonAll.Location = new System.Drawing.Point(259, 2);
            this.buttonAll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAll.Name = "buttonAll";
            this.buttonAll.Size = new System.Drawing.Size(123, 94);
            this.buttonAll.TabIndex = 3;
            this.buttonAll.Text = "所有条目";
            this.buttonAll.UseVisualStyleBackColor = true;
            this.buttonAll.Click += new System.EventHandler(this.buttonAll_Click);
            this.buttonAll.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAll_MouseDown);
            this.buttonAll.MouseEnter += new System.EventHandler(this.buttonAll_MouseEnter);
            this.buttonAll.MouseLeave += new System.EventHandler(this.buttonAll_MouseLeave);
            // 
            // buttonNoPass
            // 
            this.buttonNoPass.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonNoPass.BackgroundImage")));
            this.buttonNoPass.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonNoPass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonNoPass.FlatAppearance.BorderSize = 0;
            this.buttonNoPass.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonNoPass.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonNoPass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNoPass.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.buttonNoPass.Location = new System.Drawing.Point(131, 2);
            this.buttonNoPass.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonNoPass.Name = "buttonNoPass";
            this.buttonNoPass.Size = new System.Drawing.Size(122, 94);
            this.buttonNoPass.TabIndex = 2;
            this.buttonNoPass.Text = "不合格条目";
            this.buttonNoPass.UseVisualStyleBackColor = true;
            this.buttonNoPass.Click += new System.EventHandler(this.buttonNoPass_Click);
            this.buttonNoPass.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonNoPass_MouseDown);
            this.buttonNoPass.MouseEnter += new System.EventHandler(this.buttonNoPass_MouseEnter);
            this.buttonNoPass.MouseLeave += new System.EventHandler(this.buttonNoPass_MouseLeave);
            // 
            // FormSubmissionChooseExcelType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 98);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSubmissionChooseExcelType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择报表类型";
            this.Load += new System.EventHandler(this.FormSubmissionChooseExcelType_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonPass;
        private System.Windows.Forms.Button buttonAll;
        private System.Windows.Forms.Button buttonNoPass;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
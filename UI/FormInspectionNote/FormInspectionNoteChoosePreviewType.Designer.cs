namespace WMS.UI
{
    partial class FormInspectionNoteChoosePreviewType
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonQualified = new System.Windows.Forms.Button();
            this.buttonAll = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.buttonQualified, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonAll, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(560, 157);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonQualified
            // 
            this.buttonQualified.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonQualified.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonQualified.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.buttonQualified.Location = new System.Drawing.Point(10, 10);
            this.buttonQualified.Margin = new System.Windows.Forms.Padding(10);
            this.buttonQualified.Name = "buttonQualified";
            this.buttonQualified.Size = new System.Drawing.Size(260, 137);
            this.buttonQualified.TabIndex = 0;
            this.buttonQualified.Text = "合格条目";
            this.buttonQualified.UseVisualStyleBackColor = false;
            this.buttonQualified.Click += new System.EventHandler(this.buttonQualified_Click);
            // 
            // buttonAll
            // 
            this.buttonAll.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAll.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.buttonAll.Location = new System.Drawing.Point(290, 10);
            this.buttonAll.Margin = new System.Windows.Forms.Padding(10);
            this.buttonAll.Name = "buttonAll";
            this.buttonAll.Size = new System.Drawing.Size(260, 137);
            this.buttonAll.TabIndex = 1;
            this.buttonAll.Text = "所有条目";
            this.buttonAll.UseVisualStyleBackColor = false;
            this.buttonAll.Click += new System.EventHandler(this.buttonAll_Click);
            // 
            // FormInspectionNoteChoosePreviewType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(560, 157);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormInspectionNoteChoosePreviewType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "请选择预览类型";
            this.Load += new System.EventHandler(this.FormInspectionNoteChoosePreviewType_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonAll;
        private System.Windows.Forms.Button buttonQualified;
    }
}
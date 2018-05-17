<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PagerView
    Inherits System.Windows.Forms.UserControl

    'UserControl 重写释放以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TextBoxCurrentPage = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBoxTotalPage = New System.Windows.Forms.TextBox()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.ButtonGo = New System.Windows.Forms.Button()
        Me.ButtonNextPage = New System.Windows.Forms.Button()
        Me.ButtonEndPage = New System.Windows.Forms.Button()
        Me.ButtonPreviousPage = New System.Windows.Forms.Button()
        Me.ButtonStartPage = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 10
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonStartPage, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonPreviousPage, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label2, 4, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonEndPage, 8, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonNextPage, 7, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonGo, 6, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 3, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel3, 5, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1280, 84)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'TextBoxCurrentPage
        '
        Me.TextBoxCurrentPage.Font = New System.Drawing.Font("黑体", 10.0!)
        Me.TextBoxCurrentPage.Location = New System.Drawing.Point(3, 17)
        Me.TextBoxCurrentPage.Margin = New System.Windows.Forms.Padding(3, 0, 0, 3)
        Me.TextBoxCurrentPage.Name = "TextBoxCurrentPage"
        Me.TextBoxCurrentPage.Size = New System.Drawing.Size(97, 38)
        Me.TextBoxCurrentPage.TabIndex = 5
        Me.TextBoxCurrentPage.Text = "1"
        Me.TextBoxCurrentPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label2.Font = New System.Drawing.Font("黑体", 10.0!)
        Me.Label2.Location = New System.Drawing.Point(730, 4)
        Me.Label2.Margin = New System.Windows.Forms.Padding(0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 75)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "/"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextBoxTotalPage
        '
        Me.TextBoxTotalPage.Font = New System.Drawing.Font("黑体", 10.0!)
        Me.TextBoxTotalPage.Location = New System.Drawing.Point(3, 17)
        Me.TextBoxTotalPage.Margin = New System.Windows.Forms.Padding(3, 0, 0, 3)
        Me.TextBoxTotalPage.Name = "TextBoxTotalPage"
        Me.TextBoxTotalPage.ReadOnly = True
        Me.TextBoxTotalPage.Size = New System.Drawing.Size(87, 38)
        Me.TextBoxTotalPage.TabIndex = 8
        Me.TextBoxTotalPage.Text = "1"
        Me.TextBoxTotalPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.TextBoxCurrentPage, 0, 1)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(630, 4)
        Me.TableLayoutPanel2.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 3
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(100, 75)
        Me.TableLayoutPanel2.TabIndex = 9
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 1
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.TextBoxTotalPage, 0, 1)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(750, 4)
        Me.TableLayoutPanel3.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 3
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(90, 75)
        Me.TableLayoutPanel3.TabIndex = 10
        '
        'ButtonGo
        '
        Me.ButtonGo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonGo.Font = New System.Drawing.Font("黑体", 9.0!)
        Me.ButtonGo.Location = New System.Drawing.Point(843, 7)
        Me.ButtonGo.Name = "ButtonGo"
        Me.ButtonGo.Size = New System.Drawing.Size(114, 69)
        Me.ButtonGo.TabIndex = 2
        Me.ButtonGo.Text = "跳转"
        Me.ButtonGo.UseVisualStyleBackColor = True
        '
        'ButtonNextPage
        '
        Me.ButtonNextPage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonNextPage.Font = New System.Drawing.Font("黑体", 9.0!)
        Me.ButtonNextPage.Location = New System.Drawing.Point(963, 7)
        Me.ButtonNextPage.Name = "ButtonNextPage"
        Me.ButtonNextPage.Size = New System.Drawing.Size(114, 69)
        Me.ButtonNextPage.TabIndex = 3
        Me.ButtonNextPage.Text = "下一页"
        Me.ButtonNextPage.UseVisualStyleBackColor = True
        '
        'ButtonEndPage
        '
        Me.ButtonEndPage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonEndPage.Font = New System.Drawing.Font("黑体", 9.0!)
        Me.ButtonEndPage.Location = New System.Drawing.Point(1083, 7)
        Me.ButtonEndPage.Name = "ButtonEndPage"
        Me.ButtonEndPage.Size = New System.Drawing.Size(94, 69)
        Me.ButtonEndPage.TabIndex = 4
        Me.ButtonEndPage.Text = ">|"
        Me.ButtonEndPage.UseVisualStyleBackColor = True
        '
        'ButtonPreviousPage
        '
        Me.ButtonPreviousPage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonPreviousPage.Font = New System.Drawing.Font("黑体", 9.0!)
        Me.ButtonPreviousPage.Location = New System.Drawing.Point(513, 7)
        Me.ButtonPreviousPage.Name = "ButtonPreviousPage"
        Me.ButtonPreviousPage.Size = New System.Drawing.Size(114, 69)
        Me.ButtonPreviousPage.TabIndex = 1
        Me.ButtonPreviousPage.Text = "上一页"
        Me.ButtonPreviousPage.UseVisualStyleBackColor = True
        '
        'ButtonStartPage
        '
        Me.ButtonStartPage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonStartPage.Font = New System.Drawing.Font("黑体", 9.0!)
        Me.ButtonStartPage.Location = New System.Drawing.Point(413, 7)
        Me.ButtonStartPage.Name = "ButtonStartPage"
        Me.ButtonStartPage.Size = New System.Drawing.Size(94, 69)
        Me.ButtonStartPage.TabIndex = 0
        Me.ButtonStartPage.Text = "|<"
        Me.ButtonStartPage.UseVisualStyleBackColor = True
        '
        'PagerView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 24.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Name = "PagerView"
        Me.Size = New System.Drawing.Size(1280, 84)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TextBoxCurrentPage As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents ButtonStartPage As Button
    Friend WithEvents ButtonPreviousPage As Button
    Friend WithEvents ButtonEndPage As Button
    Friend WithEvents ButtonNextPage As Button
    Friend WithEvents ButtonGo As Button
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Friend WithEvents TextBoxTotalPage As TextBox
End Class

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
        Me.ButtonStartPage = New System.Windows.Forms.Button()
        Me.ButtonPreviousPage = New System.Windows.Forms.Button()
        Me.TextBoxCurrentPage = New System.Windows.Forms.TextBox()
        Me.LabelTotalPage = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ButtonEndPage = New System.Windows.Forms.Button()
        Me.ButtonNextPage = New System.Windows.Forms.Button()
        Me.ButtonGo = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1.SuspendLayout()
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
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxCurrentPage, 3, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.LabelTotalPage, 5, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.Label2, 4, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonEndPage, 8, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonNextPage, 7, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonGo, 6, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 7
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1280, 84)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'ButtonStartPage
        '
        Me.ButtonStartPage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonStartPage.Font = New System.Drawing.Font("黑体", 9.0!)
        Me.ButtonStartPage.Location = New System.Drawing.Point(413, 20)
        Me.ButtonStartPage.Name = "ButtonStartPage"
        Me.TableLayoutPanel1.SetRowSpan(Me.ButtonStartPage, 5)
        Me.ButtonStartPage.Size = New System.Drawing.Size(94, 44)
        Me.ButtonStartPage.TabIndex = 0
        Me.ButtonStartPage.Text = "|<"
        Me.ButtonStartPage.UseVisualStyleBackColor = True
        '
        'ButtonPreviousPage
        '
        Me.ButtonPreviousPage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonPreviousPage.Font = New System.Drawing.Font("黑体", 9.0!)
        Me.ButtonPreviousPage.Location = New System.Drawing.Point(513, 20)
        Me.ButtonPreviousPage.Name = "ButtonPreviousPage"
        Me.TableLayoutPanel1.SetRowSpan(Me.ButtonPreviousPage, 5)
        Me.ButtonPreviousPage.Size = New System.Drawing.Size(114, 44)
        Me.ButtonPreviousPage.TabIndex = 1
        Me.ButtonPreviousPage.Text = "上一页"
        Me.ButtonPreviousPage.UseVisualStyleBackColor = True
        '
        'TextBoxCurrentPage
        '
        Me.TextBoxCurrentPage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxCurrentPage.Font = New System.Drawing.Font("黑体", 10.0!)
        Me.TextBoxCurrentPage.Location = New System.Drawing.Point(633, 27)
        Me.TextBoxCurrentPage.Margin = New System.Windows.Forms.Padding(3, 5, 0, 3)
        Me.TextBoxCurrentPage.Name = "TextBoxCurrentPage"
        Me.TableLayoutPanel1.SetRowSpan(Me.TextBoxCurrentPage, 3)
        Me.TextBoxCurrentPage.Size = New System.Drawing.Size(97, 38)
        Me.TextBoxCurrentPage.TabIndex = 5
        Me.TextBoxCurrentPage.Text = "1"
        Me.TextBoxCurrentPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LabelTotalPage
        '
        Me.LabelTotalPage.AutoSize = True
        Me.LabelTotalPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabelTotalPage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelTotalPage.Font = New System.Drawing.Font("黑体", 10.0!)
        Me.LabelTotalPage.Location = New System.Drawing.Point(750, 27)
        Me.LabelTotalPage.Margin = New System.Windows.Forms.Padding(0)
        Me.LabelTotalPage.Name = "LabelTotalPage"
        Me.LabelTotalPage.Size = New System.Drawing.Size(90, 30)
        Me.LabelTotalPage.TabIndex = 6
        Me.LabelTotalPage.Text = "1"
        Me.LabelTotalPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label2.Font = New System.Drawing.Font("黑体", 10.0!)
        Me.Label2.Location = New System.Drawing.Point(730, 27)
        Me.Label2.Margin = New System.Windows.Forms.Padding(0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 30)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "/"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonEndPage
        '
        Me.ButtonEndPage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonEndPage.Font = New System.Drawing.Font("黑体", 9.0!)
        Me.ButtonEndPage.Location = New System.Drawing.Point(1083, 20)
        Me.ButtonEndPage.Name = "ButtonEndPage"
        Me.TableLayoutPanel1.SetRowSpan(Me.ButtonEndPage, 5)
        Me.ButtonEndPage.Size = New System.Drawing.Size(94, 44)
        Me.ButtonEndPage.TabIndex = 4
        Me.ButtonEndPage.Text = ">|"
        Me.ButtonEndPage.UseVisualStyleBackColor = True
        '
        'ButtonNextPage
        '
        Me.ButtonNextPage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonNextPage.Font = New System.Drawing.Font("黑体", 9.0!)
        Me.ButtonNextPage.Location = New System.Drawing.Point(963, 20)
        Me.ButtonNextPage.Name = "ButtonNextPage"
        Me.TableLayoutPanel1.SetRowSpan(Me.ButtonNextPage, 5)
        Me.ButtonNextPage.Size = New System.Drawing.Size(114, 44)
        Me.ButtonNextPage.TabIndex = 3
        Me.ButtonNextPage.Text = "下一页"
        Me.ButtonNextPage.UseVisualStyleBackColor = True
        '
        'ButtonGo
        '
        Me.ButtonGo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonGo.Font = New System.Drawing.Font("黑体", 9.0!)
        Me.ButtonGo.Location = New System.Drawing.Point(843, 20)
        Me.ButtonGo.Name = "ButtonGo"
        Me.TableLayoutPanel1.SetRowSpan(Me.ButtonGo, 5)
        Me.ButtonGo.Size = New System.Drawing.Size(114, 44)
        Me.ButtonGo.TabIndex = 2
        Me.ButtonGo.Text = "跳转"
        Me.ButtonGo.UseVisualStyleBackColor = True
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
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents ButtonStartPage As Button
    Friend WithEvents ButtonPreviousPage As Button
    Friend WithEvents ButtonEndPage As Button
    Friend WithEvents TextBoxCurrentPage As TextBox
    Friend WithEvents LabelTotalPage As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents ButtonNextPage As Button
    Friend WithEvents ButtonGo As Button
End Class

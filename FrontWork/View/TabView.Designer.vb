<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TabView
    Inherits System.Windows.Forms.UserControl

    'UserControl 重写释放以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.TabControl = New System.Windows.Forms.TabControl()
        Me.SuspendLayout()
        '
        'TabControl
        '
        Me.TabControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl.Font = New System.Drawing.Font("黑体", 10.0!)
        Me.TabControl.ItemSize = New System.Drawing.Size(124, 45)
        Me.TabControl.Location = New System.Drawing.Point(0, 0)
        Me.TabControl.Margin = New System.Windows.Forms.Padding(0)
        Me.TabControl.Name = "TabControl"
        Me.TabControl.SelectedIndex = 0
        Me.TabControl.Size = New System.Drawing.Size(1023, 45)
        Me.TabControl.TabIndex = 0
        '
        'TabView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(14.0!, 27.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TabControl)
        Me.Font = New System.Drawing.Font("宋体", 10.0!)
        Me.Margin = New System.Windows.Forms.Padding(0)
        Me.Name = "TabView"
        Me.Size = New System.Drawing.Size(1023, 45)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl As TabControl
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Configurator
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Configurator))
        Dim ModeMethodListenerNamesPair3 As FrontWork.ModeMethodListenerNamesPair = New FrontWork.ModeMethodListenerNamesPair()
        Dim ModeMethodListenerNamesPair4 As FrontWork.ModeMethodListenerNamesPair = New FrontWork.ModeMethodListenerNamesPair()
        Me.TableLayoutPanelMain = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.TabControlBottom = New System.Windows.Forms.TabControl()
        Me.TabPageDescription = New System.Windows.Forms.TabPage()
        Me.TextBoxDescription = New System.Windows.Forms.TextBox()
        Me.TabPageResult = New System.Windows.Forms.TabPage()
        Me.TextBoxResult = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.ButtonAddField = New System.Windows.Forms.Button()
        Me.ButtonRemoveField = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
        Me.ButtonAddMode = New System.Windows.Forms.Button()
        Me.ButtonRemoveMode = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel5 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel6 = New System.Windows.Forms.TableLayoutPanel()
        Me.ButtonAddAPI = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.TableLayoutPanel7 = New System.Windows.Forms.TableLayoutPanel()
        Me.ButtonOK = New System.Windows.Forms.Button()
        Me.BasicViewFields = New FrontWork.BasicView()
        Me.ConfigurationFields = New FrontWork.Configuration()
        Me.ModelBoxFields = New FrontWork.ModelBox()
        Me.ReoGridViewFields = New FrontWork.ReoGridView()
        Me.ConfigurationModes = New FrontWork.Configuration()
        Me.ModelModes = New FrontWork.Model()
        Me.ReoGridView1 = New FrontWork.ReoGridView()
        Me.BasicViewHTTPAPIs = New FrontWork.BasicView()
        Me.ConfigurationHTTPAPIs = New FrontWork.Configuration()
        Me.ModelBoxHTTPAPIs = New FrontWork.ModelBox()
        Me.ReoGridView2 = New FrontWork.ReoGridView()
        Me.TableLayoutPanelMain.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.TabControlBottom.SuspendLayout()
        Me.TabPageDescription.SuspendLayout()
        Me.TabPageResult.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.TableLayoutPanel5.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.TableLayoutPanel6.SuspendLayout()
        Me.TableLayoutPanel7.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanelMain
        '
        Me.TableLayoutPanelMain.ColumnCount = 3
        Me.TableLayoutPanelMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 600.0!))
        Me.TableLayoutPanelMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 468.0!))
        Me.TableLayoutPanelMain.Controls.Add(Me.Panel1, 0, 2)
        Me.TableLayoutPanelMain.Controls.Add(Me.GroupBox1, 0, 0)
        Me.TableLayoutPanelMain.Controls.Add(Me.GroupBox2, 1, 0)
        Me.TableLayoutPanelMain.Controls.Add(Me.GroupBox3, 2, 0)
        Me.TableLayoutPanelMain.Controls.Add(Me.GroupBox4, 0, 1)
        Me.TableLayoutPanelMain.Controls.Add(Me.TableLayoutPanel7, 0, 3)
        Me.TableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelMain.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelMain.Name = "TableLayoutPanelMain"
        Me.TableLayoutPanelMain.RowCount = 4
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 550.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 400.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.TableLayoutPanelMain.Size = New System.Drawing.Size(1674, 1029)
        Me.TableLayoutPanelMain.TabIndex = 4
        '
        'Panel1
        '
        Me.TableLayoutPanelMain.SetColumnSpan(Me.Panel1, 3)
        Me.Panel1.Controls.Add(Me.TabControlBottom)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 953)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1668, 1)
        Me.Panel1.TabIndex = 5
        '
        'TabControlBottom
        '
        Me.TabControlBottom.Controls.Add(Me.TabPageDescription)
        Me.TabControlBottom.Controls.Add(Me.TabPageResult)
        Me.TabControlBottom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlBottom.Font = New System.Drawing.Font("黑体", 10.0!)
        Me.TabControlBottom.Location = New System.Drawing.Point(0, 0)
        Me.TabControlBottom.Name = "TabControlBottom"
        Me.TabControlBottom.SelectedIndex = 0
        Me.TabControlBottom.Size = New System.Drawing.Size(1668, 1)
        Me.TabControlBottom.TabIndex = 3
        '
        'TabPageDescription
        '
        Me.TabPageDescription.BackColor = System.Drawing.SystemColors.Control
        Me.TabPageDescription.Controls.Add(Me.TextBoxDescription)
        Me.TabPageDescription.Location = New System.Drawing.Point(8, 42)
        Me.TabPageDescription.Name = "TabPageDescription"
        Me.TabPageDescription.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageDescription.Size = New System.Drawing.Size(1652, 0)
        Me.TabPageDescription.TabIndex = 0
        Me.TabPageDescription.Text = "说明"
        '
        'TextBoxDescription
        '
        Me.TextBoxDescription.BackColor = System.Drawing.Color.White
        Me.TextBoxDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxDescription.Location = New System.Drawing.Point(3, 3)
        Me.TextBoxDescription.Margin = New System.Windows.Forms.Padding(0)
        Me.TextBoxDescription.Multiline = True
        Me.TextBoxDescription.Name = "TextBoxDescription"
        Me.TextBoxDescription.ReadOnly = True
        Me.TextBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxDescription.Size = New System.Drawing.Size(1646, 0)
        Me.TextBoxDescription.TabIndex = 2
        Me.TextBoxDescription.Text = "字段的说明"
        '
        'TabPageResult
        '
        Me.TabPageResult.BackColor = System.Drawing.SystemColors.Control
        Me.TabPageResult.Controls.Add(Me.TextBoxResult)
        Me.TabPageResult.Location = New System.Drawing.Point(8, 42)
        Me.TabPageResult.Name = "TabPageResult"
        Me.TabPageResult.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageResult.Size = New System.Drawing.Size(1652, 93)
        Me.TabPageResult.TabIndex = 1
        Me.TabPageResult.Text = "生成结果"
        '
        'TextBoxResult
        '
        Me.TextBoxResult.BackColor = System.Drawing.Color.White
        Me.TextBoxResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxResult.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxResult.Location = New System.Drawing.Point(3, 3)
        Me.TextBoxResult.Margin = New System.Windows.Forms.Padding(0)
        Me.TextBoxResult.Multiline = True
        Me.TextBoxResult.Name = "TextBoxResult"
        Me.TextBoxResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxResult.Size = New System.Drawing.Size(1646, 87)
        Me.TextBoxResult.TabIndex = 3
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.BasicViewFields)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Font = New System.Drawing.Font("黑体", 10.0!)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(3, 11, 3, 3)
        Me.GroupBox1.Size = New System.Drawing.Size(594, 544)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "字段配置"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.TableLayoutPanel2)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Font = New System.Drawing.Font("黑体", 10.0!)
        Me.GroupBox2.Location = New System.Drawing.Point(603, 3)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(600, 544)
        Me.GroupBox2.TabIndex = 7
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "所有字段"
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.Panel4, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel3, 0, 1)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 34)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(594, 507)
        Me.TableLayoutPanel2.TabIndex = 4
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.ModelBoxFields)
        Me.Panel4.Controls.Add(Me.ConfigurationFields)
        Me.Panel4.Controls.Add(Me.ReoGridViewFields)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(594, 439)
        Me.Panel4.TabIndex = 4
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 5
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.ButtonAddField, 1, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.ButtonRemoveField, 3, 0)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(3, 442)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 1
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(588, 62)
        Me.TableLayoutPanel3.TabIndex = 5
        '
        'ButtonAddField
        '
        Me.ButtonAddField.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonAddField.Location = New System.Drawing.Point(127, 3)
        Me.ButtonAddField.Name = "ButtonAddField"
        Me.ButtonAddField.Size = New System.Drawing.Size(144, 56)
        Me.ButtonAddField.TabIndex = 0
        Me.ButtonAddField.Text = "添加字段"
        Me.ButtonAddField.UseVisualStyleBackColor = True
        '
        'ButtonRemoveField
        '
        Me.ButtonRemoveField.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonRemoveField.Location = New System.Drawing.Point(317, 3)
        Me.ButtonRemoveField.Name = "ButtonRemoveField"
        Me.ButtonRemoveField.Size = New System.Drawing.Size(144, 56)
        Me.ButtonRemoveField.TabIndex = 1
        Me.ButtonRemoveField.Text = "删除字段"
        Me.ButtonRemoveField.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.TableLayoutPanel1)
        Me.GroupBox3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox3.Font = New System.Drawing.Font("黑体", 10.0!)
        Me.GroupBox3.Location = New System.Drawing.Point(1209, 3)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(462, 544)
        Me.GroupBox3.TabIndex = 8
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "配置模式"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Panel2, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel4, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 34)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(456, 507)
        Me.TableLayoutPanel1.TabIndex = 3
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.ConfigurationModes)
        Me.Panel2.Controls.Add(Me.ModelModes)
        Me.Panel2.Controls.Add(Me.ReoGridView1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(456, 439)
        Me.Panel2.TabIndex = 0
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 5
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.ButtonAddMode, 1, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.ButtonRemoveMode, 3, 0)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(0, 439)
        Me.TableLayoutPanel4.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 1
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(456, 68)
        Me.TableLayoutPanel4.TabIndex = 1
        '
        'ButtonAddMode
        '
        Me.ButtonAddMode.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonAddMode.Location = New System.Drawing.Point(83, 3)
        Me.ButtonAddMode.Name = "ButtonAddMode"
        Me.ButtonAddMode.Size = New System.Drawing.Size(124, 62)
        Me.ButtonAddMode.TabIndex = 0
        Me.ButtonAddMode.Text = "添加模式"
        Me.ButtonAddMode.UseVisualStyleBackColor = True
        '
        'ButtonRemoveMode
        '
        Me.ButtonRemoveMode.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonRemoveMode.Location = New System.Drawing.Point(248, 3)
        Me.ButtonRemoveMode.Name = "ButtonRemoveMode"
        Me.ButtonRemoveMode.Size = New System.Drawing.Size(124, 62)
        Me.ButtonRemoveMode.TabIndex = 1
        Me.ButtonRemoveMode.Text = "删除模式"
        Me.ButtonRemoveMode.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.TableLayoutPanelMain.SetColumnSpan(Me.GroupBox4, 3)
        Me.GroupBox4.Controls.Add(Me.TableLayoutPanel5)
        Me.GroupBox4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox4.Font = New System.Drawing.Font("黑体", 10.0!)
        Me.GroupBox4.Location = New System.Drawing.Point(3, 553)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(1668, 394)
        Me.GroupBox4.TabIndex = 9
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "API配置"
        '
        'TableLayoutPanel5
        '
        Me.TableLayoutPanel5.ColumnCount = 3
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 700.0!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 174.0!))
        Me.TableLayoutPanel5.Controls.Add(Me.BasicViewHTTPAPIs, 0, 0)
        Me.TableLayoutPanel5.Controls.Add(Me.Panel3, 1, 0)
        Me.TableLayoutPanel5.Controls.Add(Me.TableLayoutPanel6, 2, 0)
        Me.TableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel5.Location = New System.Drawing.Point(3, 34)
        Me.TableLayoutPanel5.Name = "TableLayoutPanel5"
        Me.TableLayoutPanel5.RowCount = 1
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel5.Size = New System.Drawing.Size(1662, 357)
        Me.TableLayoutPanel5.TabIndex = 1
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.ModelBoxHTTPAPIs)
        Me.Panel3.Controls.Add(Me.ConfigurationHTTPAPIs)
        Me.Panel3.Controls.Add(Me.ReoGridView2)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(700, 0)
        Me.Panel3.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(788, 357)
        Me.Panel3.TabIndex = 1
        '
        'TableLayoutPanel6
        '
        Me.TableLayoutPanel6.ColumnCount = 1
        Me.TableLayoutPanel6.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel6.Controls.Add(Me.ButtonAddAPI, 0, 1)
        Me.TableLayoutPanel6.Controls.Add(Me.Button2, 0, 3)
        Me.TableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel6.Location = New System.Drawing.Point(1491, 3)
        Me.TableLayoutPanel6.Name = "TableLayoutPanel6"
        Me.TableLayoutPanel6.RowCount = 5
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57.0!))
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57.0!))
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel6.Size = New System.Drawing.Size(168, 351)
        Me.TableLayoutPanel6.TabIndex = 2
        '
        'ButtonAddAPI
        '
        Me.ButtonAddAPI.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonAddAPI.Font = New System.Drawing.Font("黑体", 10.0!)
        Me.ButtonAddAPI.Location = New System.Drawing.Point(3, 110)
        Me.ButtonAddAPI.Name = "ButtonAddAPI"
        Me.ButtonAddAPI.Size = New System.Drawing.Size(162, 51)
        Me.ButtonAddAPI.TabIndex = 0
        Me.ButtonAddAPI.Text = "添加API"
        Me.ButtonAddAPI.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Button2.Font = New System.Drawing.Font("黑体", 10.0!)
        Me.Button2.Location = New System.Drawing.Point(3, 189)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(162, 51)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "删除API"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel7
        '
        Me.TableLayoutPanel7.ColumnCount = 3
        Me.TableLayoutPanelMain.SetColumnSpan(Me.TableLayoutPanel7, 3)
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300.0!))
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel7.Controls.Add(Me.ButtonOK, 1, 0)
        Me.TableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel7.Location = New System.Drawing.Point(3, 952)
        Me.TableLayoutPanel7.Name = "TableLayoutPanel7"
        Me.TableLayoutPanel7.RowCount = 1
        Me.TableLayoutPanel7.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel7.Size = New System.Drawing.Size(1668, 74)
        Me.TableLayoutPanel7.TabIndex = 10
        '
        'ButtonOK
        '
        Me.ButtonOK.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonOK.Font = New System.Drawing.Font("黑体", 10.0!)
        Me.ButtonOK.Location = New System.Drawing.Point(687, 3)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.Size = New System.Drawing.Size(294, 68)
        Me.ButtonOK.TabIndex = 0
        Me.ButtonOK.Text = "确定"
        Me.ButtonOK.UseVisualStyleBackColor = True
        '
        'BasicViewFields
        '
        Me.BasicViewFields.Configuration = Me.ConfigurationFields
        Me.BasicViewFields.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BasicViewFields.Font = New System.Drawing.Font("黑体", 10.0!)
        Me.BasicViewFields.ItemsPerRow = 1
        Me.BasicViewFields.Location = New System.Drawing.Point(3, 42)
        Me.BasicViewFields.Margin = New System.Windows.Forms.Padding(0)
        Me.BasicViewFields.Mode = "default"
        Me.BasicViewFields.Model = Me.ModelBoxFields
        Me.BasicViewFields.Name = "BasicViewFields"
        Me.BasicViewFields.Size = New System.Drawing.Size(588, 499)
        Me.BasicViewFields.TabIndex = 0
        '
        'ConfigurationFields
        '
        Me.ConfigurationFields.BackColor = System.Drawing.SystemColors.Control
        Me.ConfigurationFields.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ConfigurationFields.ConfigurationString = resources.GetString("ConfigurationFields.ConfigurationString")
        Me.ConfigurationFields.Location = New System.Drawing.Point(381, 194)
        ModeMethodListenerNamesPair3.MethodListenerNames = New String(-1) {}
        ModeMethodListenerNamesPair3.Mode = "default"
        Me.ConfigurationFields.MethodListeners = New FrontWork.ModeMethodListenerNamesPair() {ModeMethodListenerNamesPair3}
        Me.ConfigurationFields.Name = "ConfigurationFields"
        Me.ConfigurationFields.Size = New System.Drawing.Size(180, 180)
        Me.ConfigurationFields.TabIndex = 1
        '
        'ModelBoxFields
        '
        Me.ModelBoxFields.AllSelectionRanges = New FrontWork.Range(-1) {}
        Me.ModelBoxFields.Configuration = Me.ConfigurationFields
        Me.ModelBoxFields.CurrentModelName = "default"
        Me.ModelBoxFields.Location = New System.Drawing.Point(244, 194)
        Me.ModelBoxFields.Mode = "default"
        Me.ModelBoxFields.Name = "ModelBoxFields"
        Me.ModelBoxFields.SelectionRange = Nothing
        Me.ModelBoxFields.Size = New System.Drawing.Size(154, 122)
        Me.ModelBoxFields.TabIndex = 3
        Me.ModelBoxFields.Visible = False
        '
        'ReoGridViewFields
        '
        Me.ReoGridViewFields.Configuration = Me.ConfigurationFields
        Me.ReoGridViewFields.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ReoGridViewFields.Font = New System.Drawing.Font("黑体", 11.0!)
        Me.ReoGridViewFields.Location = New System.Drawing.Point(0, 0)
        Me.ReoGridViewFields.Margin = New System.Windows.Forms.Padding(0)
        Me.ReoGridViewFields.Mode = "default"
        Me.ReoGridViewFields.Model = Me.ModelBoxFields
        Me.ReoGridViewFields.Name = "ReoGridViewFields"
        Me.ReoGridViewFields.Size = New System.Drawing.Size(594, 439)
        Me.ReoGridViewFields.TabIndex = 3
        '
        'ConfigurationModes
        '
        Me.ConfigurationModes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        'Me.ConfigurationModes.ConfigurationString = "[" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    {mode:""default""," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    fields:[" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "        {name:""name"",displayName:""模式名称""}" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) &
        '"    ]}" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "]"
        '    Me.ConfigurationModes.ConfiguratorGUI = "[" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    {mode:""default""," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    fields:[" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "        {name:""name"",displayName:""模式名称""}" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) &
        '"    ]}" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "]"
        Me.ConfigurationModes.Location = New System.Drawing.Point(112, 194)
        Me.ConfigurationModes.MethodListeners = New FrontWork.ModeMethodListenerNamesPair(-1) {}
        Me.ConfigurationModes.Name = "ConfigurationModes"
        Me.ConfigurationModes.Size = New System.Drawing.Size(180, 180)
        Me.ConfigurationModes.TabIndex = 2
        '
        'ModelModes
        '
        Me.ModelModes.AllSelectionRanges = New FrontWork.Range(-1) {}
        Me.ModelModes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ModelModes.Configuration = Me.ConfigurationModes
        Me.ModelModes.Location = New System.Drawing.Point(3, 194)
        Me.ModelModes.Mode = "default"
        Me.ModelModes.Name = "ModelModes"
        Me.ModelModes.SelectionRange = Nothing
        Me.ModelModes.Size = New System.Drawing.Size(180, 180)
        Me.ModelModes.TabIndex = 1
        '
        'ReoGridView1
        '
        Me.ReoGridView1.Configuration = Me.ConfigurationModes
        Me.ReoGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ReoGridView1.Font = New System.Drawing.Font("黑体", 11.0!)
        Me.ReoGridView1.Location = New System.Drawing.Point(0, 0)
        Me.ReoGridView1.Mode = "default"
        Me.ReoGridView1.Model = Me.ModelModes
        Me.ReoGridView1.Name = "ReoGridView1"
        Me.ReoGridView1.Size = New System.Drawing.Size(456, 439)
        Me.ReoGridView1.TabIndex = 0
        '
        'BasicViewHTTPAPIs
        '
        Me.BasicViewHTTPAPIs.Configuration = Me.ConfigurationHTTPAPIs
        Me.BasicViewHTTPAPIs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BasicViewHTTPAPIs.Font = New System.Drawing.Font("黑体", 10.0!)
        Me.BasicViewHTTPAPIs.ItemsPerRow = 1
        Me.BasicViewHTTPAPIs.Location = New System.Drawing.Point(0, 0)
        Me.BasicViewHTTPAPIs.Margin = New System.Windows.Forms.Padding(0)
        Me.BasicViewHTTPAPIs.Mode = "default"
        Me.BasicViewHTTPAPIs.Model = Me.ModelBoxHTTPAPIs
        Me.BasicViewHTTPAPIs.Name = "BasicViewHTTPAPIs"
        Me.BasicViewHTTPAPIs.Size = New System.Drawing.Size(700, 357)
        Me.BasicViewHTTPAPIs.TabIndex = 0
        '
        'ConfigurationHTTPAPIs
        '
        Me.ConfigurationHTTPAPIs.BackColor = System.Drawing.SystemColors.Control
        Me.ConfigurationHTTPAPIs.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ConfigurationHTTPAPIs.ConfigurationString = resources.GetString("ConfigurationHTTPAPIs.ConfigurationString")
        'Me.ConfigurationHTTPAPIs.ConfiguratorGUI = resources.GetString("ConfigurationHTTPAPIs.ConfiguratorGUI")
        Me.ConfigurationHTTPAPIs.Location = New System.Drawing.Point(58, 132)
        ModeMethodListenerNamesPair4.MethodListenerNames = New String() {"ConfiguratorMethodListener"}
        ModeMethodListenerNamesPair4.Mode = "default"
        Me.ConfigurationHTTPAPIs.MethodListeners = New FrontWork.ModeMethodListenerNamesPair() {ModeMethodListenerNamesPair4}
        Me.ConfigurationHTTPAPIs.Name = "ConfigurationHTTPAPIs"
        Me.ConfigurationHTTPAPIs.Size = New System.Drawing.Size(180, 180)
        Me.ConfigurationHTTPAPIs.TabIndex = 5
        '
        'ModelBoxHTTPAPIs
        '
        Me.ModelBoxHTTPAPIs.AllSelectionRanges = New FrontWork.Range(-1) {}
        Me.ModelBoxHTTPAPIs.Configuration = Me.ConfigurationHTTPAPIs
        Me.ModelBoxHTTPAPIs.CurrentModelName = "default"
        Me.ModelBoxHTTPAPIs.Location = New System.Drawing.Point(210, 132)
        Me.ModelBoxHTTPAPIs.Mode = "default"
        Me.ModelBoxHTTPAPIs.Name = "ModelBoxHTTPAPIs"
        Me.ModelBoxHTTPAPIs.SelectionRange = Nothing
        Me.ModelBoxHTTPAPIs.Size = New System.Drawing.Size(179, 103)
        Me.ModelBoxHTTPAPIs.TabIndex = 4
        Me.ModelBoxHTTPAPIs.Visible = False
        '
        'ReoGridView2
        '
        Me.ReoGridView2.Configuration = Me.ConfigurationHTTPAPIs
        Me.ReoGridView2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ReoGridView2.Font = New System.Drawing.Font("黑体", 11.0!)
        Me.ReoGridView2.Location = New System.Drawing.Point(0, 0)
        Me.ReoGridView2.Mode = "default"
        Me.ReoGridView2.Model = Me.ModelBoxHTTPAPIs
        Me.ReoGridView2.Name = "ReoGridView2"
        Me.ReoGridView2.Size = New System.Drawing.Size(788, 357)
        Me.ReoGridView2.TabIndex = 0
        '
        'Configurator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(14.0!, 27.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1674, 1029)
        Me.Controls.Add(Me.TableLayoutPanelMain)
        Me.Font = New System.Drawing.Font("宋体", 10.0!)
        Me.Name = "Configurator"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FrontWork配置中心"
        Me.TableLayoutPanelMain.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.TabControlBottom.ResumeLayout(False)
        Me.TabPageDescription.ResumeLayout(False)
        Me.TabPageDescription.PerformLayout()
        Me.TabPageResult.ResumeLayout(False)
        Me.TabPageResult.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.TableLayoutPanel5.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.TableLayoutPanel6.ResumeLayout(False)
        Me.TableLayoutPanel7.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ConfigurationFields As Configuration
    Friend WithEvents BasicViewFields As BasicView
    Friend WithEvents TableLayoutPanelMain As TableLayoutPanel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents TextBoxDescription As TextBox
    Friend WithEvents TabControlBottom As TabControl
    Friend WithEvents TabPageDescription As TabPage
    Friend WithEvents TabPageResult As TabPage
    Friend WithEvents TextBoxResult As TextBox
    Friend WithEvents ModelBoxFields As ModelBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents ConfigurationModes As Configuration
    Friend WithEvents ModelModes As Model
    Friend WithEvents ReoGridView1 As ReoGridView
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents TableLayoutPanel4 As TableLayoutPanel
    Friend WithEvents ButtonAddMode As Button
    Friend WithEvents ButtonRemoveMode As Button
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents ConfigurationHTTPAPIs As Configuration
    Friend WithEvents ModelBoxHTTPAPIs As ModelBox
    Friend WithEvents TableLayoutPanel5 As TableLayoutPanel
    Friend WithEvents BasicViewHTTPAPIs As BasicView
    Friend WithEvents Panel3 As Panel
    Friend WithEvents ReoGridView2 As ReoGridView
    Friend WithEvents TableLayoutPanel6 As TableLayoutPanel
    Friend WithEvents ButtonAddAPI As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Panel4 As Panel
    Friend WithEvents TableLayoutPanel7 As TableLayoutPanel
    Friend WithEvents ButtonOK As Button
    Friend WithEvents ReoGridViewFields As ReoGridView
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Friend WithEvents ButtonAddField As Button
    Friend WithEvents ButtonRemoveField As Button
End Class

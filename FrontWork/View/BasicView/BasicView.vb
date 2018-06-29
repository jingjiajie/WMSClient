Imports FrontWork
Imports Jint.Native
Imports System.ComponentModel
Imports System.Globalization
Imports System.Linq
Imports System.Reflection
Imports System.Threading

''' <summary>
''' 基本视图，以文本框，下拉框等形式提供数据的交互
''' </summary>
Partial Public Class BasicView
    Inherits UserControl
    Implements IDataView


    Private _itemsPerRow As Integer = 3

    Private Property _TargetRow As Integer = -1
    Private Property FormAssociation As New FormAssociation
    Private Property _TextBoxManager As New TextBoxManager
    Private Property _ComboBoxManager As New ComboBoxManager
    Private Property _LabelManager As New LabelManager
    Private Property _ViewModel As New ViewModel

    Private Property _RecordedRows As New List(Of IDictionary(Of String, Object))

    ''' <summary>
    ''' Model对象，用来存取数据
    ''' </summary>
    ''' <returns>Model对象</returns>
    <Description("Model对象"), Category("FrontWork")>
    Public Property Model As IModelCore
        Get
            Return Me._ViewModel.ModelOperationsWrapper.ModelCore
        End Get
        Set(value As IModelCore)
            Me._ViewModel.ModelOperationsWrapper = New ModelOperationsWrapper(value)
        End Set
    End Property

    ''' <summary>
    ''' 配置中心对象，用来获取配置
    ''' </summary>
    ''' <returns>配置中心对象</returns>
    <Description("配置中心对象"), Category("FrontWork")>
    Public Property Configuration As Configuration
        Get
            Return Me._ViewModel.Configuration
        End Get
        Set(value As Configuration)
            Me._ViewModel.Configuration = value
        End Set
    End Property

    ''' <summary>
    ''' 每行显示几个字段
    ''' </summary>
    ''' <returns></returns>
    <Description("每行显示几个字段"), Category("FrontWork")>
    Public Property ItemsPerRow As Integer
        Get
            Return Me._itemsPerRow
        End Get
        Set(value As Integer)
            If Me._itemsPerRow = value Then Return
            Me._itemsPerRow = value
            'Call Me.InitEditPanel()
        End Set
    End Property

    Private Sub TableLayoutPanel_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private switcherModelDataUpdatedEvent As Boolean = True
    Private switcherLocalEvents As Boolean = True '本View内部事件开关，包括文本框文字变化等。不包括外部，例如Model数据变化事件开关
    Private dicFieldUpdated As New Dictionary(Of String, Boolean)
    Private dicFieldEdited As New Dictionary(Of String, Boolean)
    Public Event BeforeRowUpdated As EventHandler(Of BeforeViewRowUpdateEventArgs) Implements IDataView.BeforeRowUpdated
    Public Event BeforeRowAdded As EventHandler(Of BeforeViewRowAddEventArgs) Implements IDataView.BeforeRowAdded
    Public Event BeforeRowRemoved As EventHandler(Of BeforeViewRowRemoveEventArgs) Implements IDataView.BeforeRowRemoved
    Public Event BeforeCellUpdated As EventHandler(Of BeforeViewCellUpdateEventArgs) Implements IDataView.BeforeCellUpdated
    Public Event BeforeSelectionRangeChange As EventHandler(Of BeforeViewSelectionRangeChangeEventArgs) Implements IDataView.BeforeSelectionRangeChange
    Public Event RowUpdated As EventHandler(Of ViewRowUpdatedEventArgs) Implements IDataView.RowUpdated
    Public Event RowAdded As EventHandler(Of ViewRowAddedEventArgs) Implements IDataView.RowAdded
    Public Event RowRemoved As EventHandler(Of ViewRowRemovedEventArgs) Implements IDataView.RowRemoved
    Public Event CellUpdated As EventHandler(Of ViewCellUpdatedEventArgs) Implements IDataView.CellUpdated
    Public Event SelectionRangeChanged As EventHandler(Of ViewSelectionRangeChangedEventArgs) Implements IDataView.SelectionRangeChanged
    Private Property Panel As TableLayoutPanel

    <Description("当前配置模式"), Category("FrontWork")>
    Public Property Mode As String
        Get
            Return Me._ViewModel.Mode
        End Get
        Set(value As String)
            Me._ViewModel.Mode = value
        End Set
    End Property

    Public Sub New()
        Call InitializeComponent()
        Me.Font = New Font("黑体", 10)
        Me.Panel = Me.TableLayoutPanel
        Me._ViewModel.ViewOperationsWrapper = New ViewOperationsWrapper(Me)
    End Sub

    Private Sub UnbindComboBox(comboBox As ComboBox)
        RemoveHandler comboBox.Leave, AddressOf Me.ComboBox_Leave
        RemoveHandler comboBox.SelectedIndexChanged, AddressOf Me.ComboBox_SelectedIndexChanged
    End Sub

    Private Sub BindComboBox(comboBox As ComboBox)
        If Me.DesignMode Then Return '如果是设计器调试，就不用绑定事件了
        AddHandler comboBox.Leave, AddressOf Me.ComboBox_Leave
        AddHandler comboBox.SelectedIndexChanged, AddressOf Me.ComboBox_SelectedIndexChanged
    End Sub

    Private Sub BindTextBox(textBox As BasicViewTextBox)
        If Me.DesignMode Then Return  '如果是设计器调试，就不用绑定事件了
        AddHandler textBox.Leave, AddressOf Me.TextBox_Leave
        AddHandler textBox.Enter, AddressOf Me.TextBox_EnterAssociation
        AddHandler textBox.TextChanged, AddressOf Me.TextBox_TextChangedEvent
    End Sub

    Private Sub UnbindTextBox(textBox As BasicViewTextBox)
        RemoveHandler textBox.Leave, AddressOf Me.TextBox_Leave
        RemoveHandler textBox.Enter, AddressOf Me.TextBox_EnterAssociation
        RemoveHandler textBox.TextChanged, AddressOf Me.TextBox_TextChangedEvent
    End Sub

    '''' <summary>
    '''' 绑定新的Model，将本View的各种事件绑定到Model上以实现数据变化的同步
    '''' </summary>
    'Protected Sub BindModel()
    '    AddHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
    '    AddHandler Me.Model.RowAdded, AddressOf Me.ModelRowAddedEvent
    '    AddHandler Me.Model.RowRemoved, AddressOf Me.ModelRowRemovedEvent
    '    AddHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
    '    AddHandler Me.Model.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
    '    AddHandler Me.Model.Refreshed, AddressOf Me.ModelRefreshedEvent

    '    Me.ImportData()
    'End Sub

    '''' <summary>
    '''' 解绑Model，取消本视图绑定的所有事件
    '''' </summary>
    'Protected Sub UnbindModel()
    '    RemoveHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
    '    RemoveHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
    '    RemoveHandler Me.Model.RowAdded, AddressOf Me.ModelRowAddedEvent
    '    RemoveHandler Me.Model.RowRemoved, AddressOf Me.ModelRowRemovedEvent
    '    RemoveHandler Me.Model.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
    '    RemoveHandler Me.Model.Refreshed, AddressOf Me.ModelRefreshedEvent

    'End Sub

    Private Sub ConfigurationChanged(sender As Object, e As ConfigurationChangedEventArgs)
        Call Me.InitEditPanel()
    End Sub

    'Private Sub ModelSelectionRangeChangedEvent(sender As Object, e As ModelSelectionRangeChangedEventArgs)
    '    Dim modelSelectedRow = Me.GetModelSelectedRow
    '    If modelSelectedRow < 0 Then
    '        Me.Panel.Enabled = False
    '        Call Me.ClearPanelData()
    '        Return
    '    Else
    '        Me.Panel.Enabled = True
    '        Me.ImportData()
    '    End If
    'End Sub

    'Private Sub ModelRowAddedEvent(sender As Object, e As ModelRowAddedEventArgs)
    '    Logger.Debug("TableLayoutView ModelRowAddedEvent: " & Str(Me.GetHashCode))
    'End Sub

    'Private Sub ModelRowRemovedEvent(sender As Object, e As ModelRowRemovedEventArgs)
    '    Logger.Debug("TableLayoutView ModelRowRemovedEvent: " & Str(Me.GetHashCode))
    '    Me.ImportData()
    'End Sub

    'Private Sub ModelCellUpdatedEvent(sender As Object, e As ModelCellUpdatedEventArgs)
    '    Logger.Debug("TableLayoutView ModelCellUpdatedEvent: " & Str(Me.GetHashCode))
    '    Logger.SetMode(LogMode.REFRESH_VIEW)
    '    Dim modelSelectedRow = Me.GetModelSelectedRow
    '    If modelSelectedRow < 0 Then
    '        Return
    '    End If
    '    '如果更新的行不包括本View的目标行，则不刷新
    '    For Each posCell In e.UpdatedCells
    '        If modelSelectedRow = posCell.Row Then
    '            Call Me.ImportField(posCell.ColumnName)
    '            Return
    '        End If
    '    Next
    'End Sub

    'Private Sub ModelRefreshedEvent(sender As Object, e As ModelRefreshedEventArgs)
    '    If switcherModelDataUpdatedEvent = False Then Return '开关被关闭则不执行事件
    '    Call Me.ImportData()
    'End Sub

    'Private Sub ModelRowUpdatedEvent(sender As Object, e As ModelRowUpdatedEventArgs)
    '    If switcherModelDataUpdatedEvent = False Then Return '开关被关闭则不执行事件
    '    Logger.Debug("TableLayoutView ModelRowUpdatedEvent: " & Str(Me.GetHashCode))
    '    Dim modelSelectedRow = Me.GetModelSelectedRow
    '    If modelSelectedRow < 0 Then Return
    '    Dim needToUpdate As Boolean = (From indexRow In e.UpdatedRows
    '                                   Where indexRow.Index = modelSelectedRow
    '                                   Select indexRow.Index).Count > 0
    '    If needToUpdate Then
    '        Call Me.ImportData()
    '    End If
    'End Sub

    Private Sub ClearPanelData()
        Me.switcherLocalEvents = False
        For Each control As Control In Me.Panel.Controls
            Select Case control.GetType
                Case GetType(BasicViewTextBox)
                    control.Text = String.Empty
                Case GetType(ComboBox)
                    Dim comboBox As ComboBox = CType(control, ComboBox)
                    comboBox.SelectedIndex = -1
            End Select
        Next
        Me.switcherLocalEvents = True
    End Sub

    ''' <summary>
    ''' 初始化视图（从配置中心读取配置），允许重复调用
    ''' </summary>
    Protected Sub InitEditPanel()
        '如果基本信息不足，则直接返回
        Logger.SetMode(LogMode.INIT_VIEW)
        If Me.Configuration Is Nothing Then
            Logger.PutMessage("Configuration is not setted")
            Return
        End If
        Call Me.Panel.SuspendLayout()
        Me.BorderStyle = BorderStyle.None
        Me.Panel.Focus() '把焦点转移走，以免有未保存的编辑框被清除数据
        Me.Panel.Controls.Clear()
        Dim fieldConfigurations As FieldConfiguration() = Me.Configuration.GetFieldConfigurations(Me.Mode)
        Dim visibleFieldConfigurations = (From f In fieldConfigurations
                                          Where f.Visible
                                          Select f).ToArray
        '初始化行列数量和大小
        Me.Panel.RowStyles.Clear()
        Me.Panel.ColumnStyles.Clear()
        Dim fieldsPerRow As Integer = Me.ItemsPerRow
        If fieldsPerRow = 0 Then
            Call Me.Panel.ResumeLayout()
            Return
        End If
        Me.Panel.ColumnCount = fieldsPerRow * 2 '计算列数
        Me.Panel.RowCount = System.Math.Floor(visibleFieldConfigurations.Length / fieldsPerRow) + If(visibleFieldConfigurations.Length Mod fieldsPerRow = 0, 0, 1) '计算行数
        For j = 0 To Me.Panel.RowCount
            Me.Panel.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F / Me.Panel.RowCount))
        Next

        '遍历FieldConfiguration()
        Call Me.dicFieldUpdated.Clear()
        Dim col As Integer = -1 'i从0开始循环
        For Each curField As FieldConfiguration In fieldConfigurations
            col += 1
            Me.dicFieldUpdated.Add(curField.Name, False)
            '如果字段不可视，直接跳过
            If curField.Visible = False Then Continue For
            '创建标签
            Dim label As New Label With {
                .Text = curField.DisplayName,
                .Font = Me.Font,
                .Dock = DockStyle.Fill,
                .Margin = New Padding(0)
            }
            Me.Panel.Controls.Add(label)
            '如果没有设定Values字段，认为可以用编辑框体现
            If curField.Values Is Nothing Then
                '创建编辑框
                Dim textBox = Me._TextBoxManager.PopControl()
                With textBox
                    .Name = curField.Name
                    .PlaceHolder = curField.PlaceHolder?.Invoke
                    .ReadOnly = Not curField.Editable
                    .Font = Me.Font
                    .Dock = DockStyle.Fill
                End With
                '将编辑框添加到Panel里
                Me.Panel.Controls.Add(textBox)
                Call Me.BindTextBox(textBox)
            Else '否则可以用ComboBox体现
                Dim comboBox As New ComboBox With {
                                                          .Name = curField.Name,
                                                          .Font = Me.Font,
                                                          .Enabled = curField.Editable,
                                                          .DropDownStyle = ComboBoxStyle.DropDownList,
                                                          .Dock = DockStyle.Fill
                                                      }
                Me.Panel.Controls.Add(comboBox)
                Dim values As Object() = Util.ToArray(Of Object)(curField.Values.Invoke(Me))
                If values IsNot Nothing Then
                    comboBox.Items.AddRange(values)
                End If
                Call Me.BindComboBox(comboBox)
            End If
        Next

        Call Me.AdjustPanelFieldWidth()
        Call Me.Panel.ResumeLayout()

        RemoveHandler Me.Panel.Leave, AddressOf Me.TableLayoutPanel_Leave
        AddHandler Me.Panel.Leave, AddressOf Me.TableLayoutPanel_Leave

        RemoveHandler Me.Panel.Enter, AddressOf Me.TableLayoutPanel_Enter
        AddHandler Me.Panel.Enter, AddressOf Me.TableLayoutPanel_Enter

    End Sub

    Private Sub AdjustPanelFieldWidth()
        Dim textBoxWidthPercent = 100 / Me.ItemsPerRow
        Dim g = Me.Panel.CreateGraphics
        '根据实际标题设置列宽
        For col = 0 To Me.Panel.ColumnCount
            If col Mod 2 = 1 Then Continue For
            Dim maxWidth = 0
            For row = 0 To Me.Panel.RowCount
                Dim label = Me.Panel.GetControlFromPosition(col, row)
                If label Is Nothing Then Continue For
                '宽度加一个字
                Dim textSize = g.MeasureString(label.Text + "A", Me.Font)
                Dim textWidth = textSize.Width
                If textWidth > maxWidth Then
                    maxWidth = textWidth
                End If
            Next
            Me.Panel.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, maxWidth))
            Me.Panel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, textBoxWidthPercent))
        Next
        Call g.Dispose()
    End Sub

    Private Sub ComboBox_SelectedIndexChanged(sender As Object, e As EventArgs)
        If Me.switcherLocalEvents = False Then Return
        Dim comboBox = CType(sender, ComboBox)
        Dim fieldName = comboBox.Name
        Dim curField = Me.Configuration.GetFieldConfiguration(Me.Mode, fieldName)

        If Not Me.dicFieldEdited.ContainsKey(fieldName) Then
            Me.dicFieldEdited.Add(fieldName, True)
        End If
        Call Me.ExportField(fieldName)

        'curField.ContentChanged?.Invoke(Me, comboBox.SelectedItem?.ToString, Me.Model.SelectionRange.Row)
    End Sub

    Private Sub ComboBox_Leave(sender As Object, e As EventArgs)
        If Me.switcherLocalEvents = False Then Return
        Dim fieldName = CType(sender, TextBox).Name
        '如果没被编辑则不保存数据+触发编辑结束事件
        If Not Me.dicFieldEdited.ContainsKey(fieldName) Then Return
        Dim curField = Me.Configuration.GetFieldConfiguration(Me.Mode, fieldName)
        '否则保存数据+触发编辑结束事件
        Call Me.ExportField(fieldName)
    End Sub

    Private Sub TextBox_Leave(sender As Object, e As EventArgs)
        If Me.switcherLocalEvents = False Then Return
        Dim fieldName = CType(sender, TextBox).Name
        '如果没被编辑则不保存数据+触发编辑结束事件
        If Not Me.dicFieldEdited.ContainsKey(fieldName) Then Return
        Dim curField = Me.Configuration.GetFieldConfiguration(Me.Mode, fieldName)
        '否则保存数据+触发编辑结束事件
        Call Me.ExportField(fieldName)
    End Sub

    Private Sub TextBox_EnterAssociation(sender As Object, e As EventArgs)
        Dim textBox As TextBox = sender
        Dim fieldName = textBox.Name
        Dim curField = Me.Configuration.GetFieldConfiguration(Me.Mode, fieldName)
        If curField.Association IsNot Nothing Then
            Me.FormAssociation.TextBox = textBox
            FormAssociation.SetAssociationFunc(Function(str As String)
                                                   Dim ret = curField.Association.Invoke(Me, str)
                                                   Return Util.ToArray(Of AssociationItem)(ret)
                                               End Function)
        End If
    End Sub

    Private Sub TableLayoutPanel_Leave(sender, e)
        Logger.Debug("TableLayoutView Panel Leave: " & Str(Me.GetHashCode))
        Me.switcherLocalEvents = False
    End Sub

    Private Sub TableLayoutPanel_Enter(sender, e)
        Logger.Debug("TableLayoutView Panel Enter: " & Str(Me.GetHashCode))
        Me.switcherLocalEvents = True
    End Sub

    Private Sub TextBox_TextChangedEvent(sender As Object, e As EventArgs)
        If Me.switcherLocalEvents = False Then Return
        Dim textBox As TextBox = sender
        Dim fieldName = CType(sender, Control).Name
        Dim curField = Me.Configuration.GetFieldConfiguration(Me.Mode, fieldName)
        If Not Me.dicFieldEdited.ContainsKey(fieldName) Then
            Me.dicFieldEdited.Add(fieldName, True)
        End If

        'curField.ContentChanged?.Invoke(Me, textBox.Text, Me.Model.SelectionRange.Row)
    End Sub

    '''' <summary>
    '''' 从Model导入一个字段
    '''' </summary>
    '''' <param name="fieldName">字段名</param>
    '''' <returns>是否导入成功</returns>
    'Protected Function ImportField(fieldName As String) As Boolean
    '    Logger.SetMode(LogMode.REFRESH_VIEW)
    '    If Me.Configuration Is Nothing Then
    '        Logger.PutMessage("Configuration is not setted")
    '        Return False
    '    End If
    '    Dim modelSelectedRow = Me.GetModelSelectedRow
    '    If modelSelectedRow < 0 Then
    '        Me.Panel.Enabled = False
    '        Call Me.ClearPanelData()
    '        Return True
    '    End If
    '    Me.Panel.Enabled = True
    '    '遍历Configuration的字段
    '    Dim fieldConfigurations = Me.Configuration.GetFieldConfigurations(Me.Mode)
    '    If fieldConfigurations Is Nothing Then
    '        Logger.PutMessage("Configuration not found!")
    '        Return False
    '    End If
    '    Dim field = (From f In fieldConfigurations Where f.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase) Select f).FirstOrDefault
    '    If Not field.Visible Then Return True
    '    '获取数据
    '    Dim value = Me.Model(modelSelectedRow, fieldName)
    '    '先计算值，过一遍Mapper
    '    Dim text As String
    '    If Not field.ForwardMapper Is Nothing Then
    '        text = field.ForwardMapper.Invoke(Me, value, modelSelectedRow)
    '    Else
    '        text = If(value?.ToString, "")
    '    End If
    '    Logger.SetMode(LogMode.REFRESH_VIEW)
    '    '然后获取Control
    '    Dim curControl = (From control As Control In Me.Panel.Controls
    '                      Where control.Name = field.Name
    '                      Select control).FirstOrDefault()
    '    '根据Control是文本框还是ComboBox，有不一样的行为
    '    Me.switcherLocalEvents = False '关闭本地事件开关， 防止连锁事件
    '    Select Case curControl.GetType()
    '        Case GetType(BasicViewTextBox)
    '            Dim textBox = CType(curControl, TextBox)
    '            textBox.Text = text
    '        Case GetType(ComboBox)
    '            Dim comboBox = CType(curControl, ComboBox)
    '            Dim found = False
    '            For i As Integer = 0 To comboBox.Items.Count - 1
    '                If comboBox.Items(i).ToString = text Then
    '                    found = True
    '                    RemoveHandler comboBox.SelectedIndexChanged, AddressOf Me.ComboBox_SelectedIndexChanged
    '                    comboBox.SelectedIndex = i
    '                    AddHandler comboBox.SelectedIndexChanged, AddressOf Me.ComboBox_SelectedIndexChanged
    '                End If
    '            Next
    '            If found = False Then
    '                Logger.PutMessage("Value """ + text + """" + " not found in comboBox """ + fieldName + """")
    '            End If
    '    End Select
    '    Me.switcherLocalEvents = True
    '    Return True
    'End Function

    ''' <summary>
    ''' 从缓存中将整行数据更新到视图上
    ''' </summary>
    Protected Sub PushRow(row As Integer)
        Me.Panel.Enabled = True
        '清空面板
        Call Me.ClearPanelData()
        Dim rowData = Me._RecordedRows(row)
        For Each kv As KeyValuePair(Of String, Object) In rowData
            Dim key = kv.Key
            Dim value = kv.Value
            Dim Text = If(value?.ToString, "")

            '然后获取Control
            Dim curControl = (From control As Control In Me.Panel.Controls
                              Where control.Name = key AndAlso control.GetType <> GetType(Label)
                              Select control).FirstOrDefault()
            If curControl Is Nothing Then
                Continue For
            End If
            '根据Control是文本框还是ComboBox，有不一样的行为
            Me.switcherLocalEvents = False '关闭本地事件开关， 防止连锁事件
            Select Case curControl.GetType()
                Case GetType(BasicViewTextBox)
                    Dim textBox = CType(curControl, TextBox)
                    textBox.Text = Text
                Case GetType(ComboBox)
                    Dim comboBox = CType(curControl, ComboBox)
                    Dim found = False
                    For i As Integer = 0 To comboBox.Items.Count - 1
                        If comboBox.Items(i).ToString = Text Then
                            found = True
                            RemoveHandler comboBox.SelectedIndexChanged, AddressOf Me.ComboBox_SelectedIndexChanged
                            comboBox.SelectedIndex = i
                            AddHandler comboBox.SelectedIndexChanged, AddressOf Me.ComboBox_SelectedIndexChanged
                        End If
                    Next
                    If found = False Then
                        Logger.PutMessage("Value """ + Text + """" + " not found in comboBox """ + key + """")
                    End If
            End Select
            'curField.EditEnded?.Invoke(Me, text, Me.GetModelSelectedRow)
            Me.switcherLocalEvents = True
        Next
    End Sub

    ''' <summary>
    ''' 导出字段数据到Model
    ''' </summary>
    ''' <param name="fieldName">要导出的字段名</param>
    Protected Sub ExportField(fieldName As String)
        Logger.SetMode(LogMode.SYNC_FROM_VIEW)
        If Not Me.dicFieldEdited.ContainsKey(fieldName) Then Return
        Dim args As New ViewCellUpdatedEventArgs({New CellInfo(Me._targetRow, Nothing, fieldName, Me.GetFieldValue(fieldName))})
        RaiseEvent CellUpdated(Me, args)
        Me.dicFieldEdited.Remove(fieldName)
    End Sub

    ''' <summary>
    ''' 导出所有数据到Model
    ''' </summary>
    Public Sub ExportData()
        Dim dicData As New Dictionary(Of String, Object)
        For Each curField As FieldConfiguration In Me.Configuration.GetFieldConfigurations(Me.Mode)
            '如果字段不可见，则忽略
            If Not curField.Visible Then Continue For
            Dim value = Me.GetFieldValue(curField.Name)
            '将新的值加入更新字典中
            dicData.Add(curField.Name, value)
        Next
        Dim args As New ViewRowUpdatedEventArgs({New RowInfo(Me._targetRow, Nothing, dicData, Nothing)})
        RaiseEvent RowUpdated(Me, args)
        Call Me.dicFieldEdited.Clear()
    End Sub

    ''' <summary>
    ''' 获取字段的数据（经过Mapper等操作之后的最终结果）
    ''' </summary>
    ''' <param name="fieldName">字段名</param>
    ''' <returns>字段的数据</returns>
    Protected Function GetFieldValue(fieldName As String) As String
        '获取Control
        Dim curControl = (From control As Control In Me.Panel.Controls
                          Where control.Name = fieldName AndAlso {GetType(BasicViewTextBox), GetType(ComboBox)}.Contains(control.GetType)
                          Select control).FirstOrDefault
        If curControl Is Nothing Then
            Logger.PutMessage(fieldName + " not found in view!")
            Return Nothing
        End If

        '获取Control中的文字
        Dim text As String = Nothing
        Select Case curControl.GetType
            Case GetType(BasicViewTextBox)
                text = CType(curControl, TextBox).Text
            Case GetType(ComboBox)
                Dim comboBox = CType(curControl, ComboBox)
                Dim selectedItem = comboBox.SelectedItem
                If selectedItem Is Nothing Then
                    text = comboBox.Text
                Else
                    text = selectedItem.ToString
                End If
        End Select
        Return text
    End Function

    Private Sub BasicView_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub BasicView_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize

    End Sub

    Private Sub TableLayoutPanel_MouseDoubleClick(sender As Object, e As MouseEventArgs)

    End Sub

    '初始化一个标签
    Private Sub InitLabel(label As Label, viewColumn As ViewColumn)
        With label
            .Text = viewColumn.DisplayName
            .Name = viewColumn.Name
            .Font = Me.Font
            .Dock = DockStyle.Fill
            .Margin = New Padding(0)
        End With
    End Sub

    Private Sub InitTextBox(textBox As BasicViewTextBox, viewColumn As ViewColumn)
        '创建编辑框
        With textBox
            .Name = viewColumn.Name
            .PlaceHolder = viewColumn.PlaceHolder?.Invoke
            .ReadOnly = Not viewColumn.Editable
            .Font = Me.Font
            .Dock = DockStyle.Fill
        End With
        Call Me.BindTextBox(textBox)
    End Sub

    Private Sub InitComboBox(comboBox As ComboBox, viewColumn As ViewColumn)
        With comboBox
            .Name = viewColumn.Name
            .Font = Me.Font
            .Enabled = viewColumn.Editable
            .DropDownStyle = ComboBoxStyle.DropDownList
            .Dock = DockStyle.Fill
        End With
        Call comboBox.Items.Clear()
        Dim values As Object() = Util.ToArray(Of Object)(viewColumn.Values.Invoke(Me))
        If values IsNot Nothing Then
            comboBox.Items.AddRange(values)
        End If
        Call Me.BindComboBox(comboBox)
    End Sub

    Public Function AddColumns(columns() As ViewColumn) As Boolean Implements IDataView.AddColumns
        Call Me.TableLayoutPanel.SuspendLayout()
        For Each viewColumn In columns
            Dim label = Me._LabelManager.PopControl
            Call Me.InitLabel(label, viewColumn)
            Me.Panel.Controls.Add(label)
            '如果没有设定Values字段，认为可以用编辑框体现
            If viewColumn.Values Is Nothing Then
                Dim textBox = Me._TextBoxManager.PopControl()
                Call Me.InitTextBox(textBox, viewColumn)
                '将编辑框添加到Panel里
                Me.Panel.Controls.Add(textBox)
            Else
                Dim comboBox = Me._ComboBoxManager.PopControl
                Call Me.InitComboBox(comboBox, viewColumn)
                Me.Panel.Controls.Add(comboBox)
            End If
        Next
        Call Me.AdjustPanelFieldWidth()
        Call Me.TableLayoutPanel.ResumeLayout()
        Return True
    End Function

    Public Function UpdateColumns(columnNames() As String, columns() As ViewColumn) As Object Implements IDataView.UpdateColumns
        Call Me.TableLayoutPanel.SuspendLayout()
        For i = 0 To columnNames.Length - 1
            Dim oriColumnName = columnNames(i)
            Dim newColumn = columns(i)
            For Each ctl As Control In Me.TableLayoutPanel.Controls
                If ctl.Name <> oriColumnName Then Continue For
                Select Case ctl.GetType
                    Case GetType(Label)
                        Call Me.InitLabel(ctl, newColumn)
                    Case GetType(BasicViewTextBox)
                        Call Me.InitTextBox(ctl, newColumn)
                    Case GetType(ComboBox)
                        Call Me.InitComboBox(ctl, newColumn)
                End Select
            Next
        Next
        Call Me.AdjustPanelFieldWidth()
        Call Me.TableLayoutPanel.ResumeLayout()
        Return True
    End Function

    Public Function RemoveColumns(columnNames() As String) As Object Implements IDataView.RemoveColumns
        Dim removeControls As New List(Of Control)
        For Each ctl As Control In Me.TableLayoutPanel.Controls
            Dim name = ctl.Name

            If columnNames.Contains(name) Then
                removeControls.Add(ctl)
            End If
        Next
        If removeControls.Count > 0 Then
            Call Me.TableLayoutPanel.SuspendLayout()
            For Each ctl In removeControls
                '将控件从视图上删除
                Me.TableLayoutPanel.Controls.Remove(ctl)
                '回收控件，重复利用
                Select Case ctl.GetType
                    Case GetType(Label)
                        Call Me._LabelManager.PushControl(ctl)
                    Case GetType(BasicViewTextBox)
                        Call Me.UnbindTextBox(ctl)
                        Call Me._TextBoxManager.PushControl(ctl)
                    Case GetType(ComboBox)
                        Call Me.UnbindComboBox(ctl)
                        Call Me._TextBoxManager.PushControl(ctl)
                End Select
            Next
            Call Me.AdjustPanelFieldWidth()
            Call Me.TableLayoutPanel.ResumeLayout()
        End If
        Return True
    End Function

    Public Function AddRows(data() As IDictionary(Of String, Object)) As Integer() Implements IDataView.AddRows
        Me._RecordedRows.AddRange(data)
        Return Nothing
    End Function

    Public Sub InsertRows(rows() As Integer, data() As IDictionary(Of String, Object)) Implements IDataView.InsertRows
        '原始行每次插入之后，行号会变，所以从后向前插入避免问题
        Dim indexDataPairs(rows.Length - 1) As IndexDataPair
        For i = 0 To rows.Length - 1
            indexDataPairs(i) = New IndexDataPair() With {
                .Index = rows(i),
                .Data = data(i)
            }
        Next
        Dim indexDataPairsDESC = (From p In indexDataPairs Order By p.Index Descending Select p).ToArray
        For Each indexDataPair In indexDataPairsDESC
            Call Me._RecordedRows.Insert(indexDataPair.Index, indexDataPair.Data)
        Next
    End Sub

    Public Sub RemoveRows(rows() As Integer) Implements IDataView.RemoveRows
        Dim rowsDESC = (From r In rows.Distinct Order By r Descending Select r).ToArray
        For Each row In rowsDESC
            Call Me._RecordedRows.RemoveAt(row)
        Next
    End Sub

    Public Sub UpdateRows(rows() As Integer, dataOfEachRow() As IDictionary(Of String, Object)) Implements IDataView.UpdateRows
        For i = 0 To rows.Length - 1
            Me._RecordedRows(rows(i)) = dataOfEachRow(i)
        Next
    End Sub

    Public Sub UpdateCells(rows() As Integer, columnNames() As String, dataOfEachCell() As Object) Implements IDataView.UpdateCells
        For i = 0 To rows.Length - 1
            Dim row = rows(i)
            Dim columnName = columnNames(i)
            Dim data = dataOfEachCell(i)
            Dim recordedRowData = Me._RecordedRows(row)
            recordedRowData(columnName) = data
        Next
    End Sub

    Public Function GetRowCount() As Integer Implements IDataView.GetRowCount
        Return Me._RecordedRows.Count
    End Function

    Public Function GetSelectionRanges() As Range() Implements IDataView.GetSelectionRanges
        Throw New NotImplementedException()
    End Function

    Public Sub SetSelectionRanges(ranges() As Range) Implements IDataView.SetSelectionRanges
        If ranges Is Nothing Then
            Me._TargetRow = -1
        Else
            Me._TargetRow = ranges(0).Row
        End If
        Call Me.PushRow(Me._TargetRow)
    End Sub



    Private MustInherit Class ControlManager(Of T As Control)
        Inherits CollectionBase

        Protected MustOverride Function CreateNewControl() As T

        Public Function PopControl() As T
            If Me.InnerList.Count > 0 Then
                Dim ctl = Me.InnerList(0)
                Call Me.InnerList.RemoveAt(0)
                Return ctl
            Else
                Return CreateNewControl()
            End If
        End Function

        Public Sub PushControl(ctl As T)
            Me.InnerList.Add(ctl)
        End Sub
    End Class

    Private Class TextBoxManager
        Inherits ControlManager(Of BasicViewTextBox)

        Protected Overrides Function CreateNewControl() As BasicViewTextBox
            Return New BasicViewTextBox
        End Function

    End Class

    Private Class ComboBoxManager
        Inherits ControlManager(Of ComboBox)

        Protected Overrides Function CreateNewControl() As ComboBox
            Return New ComboBox
        End Function

    End Class

    Private Class LabelManager
        Inherits ControlManager(Of Label)

        Protected Overrides Function CreateNewControl() As Label
            Return New Label
        End Function

    End Class

    Private Structure IndexDataPair
        Property Index As Integer
        Property Data As Object
    End Structure
End Class

Imports FrontWork
Imports Jint.Native
Imports System.ComponentModel
Imports System.Linq
Imports System.Reflection

''' <summary>
''' 基本视图，以文本框，下拉框等形式提供数据的交互
''' </summary>
Public Class BasicView
    Inherits UserControl
    Implements IDataView

    Private _itemsPerRow As Integer = 3
    Private _configuration As Configuration
    Private _model As IModel
    Private _mode As String = "default"

    Private Property JsEngine As New Jint.Engine
    Private Property FormAssociation As New FormAssociation

    ''' <summary>
    ''' Model对象，用来存取数据
    ''' </summary>
    ''' <returns>Model对象</returns>
    <Description("Model对象"), Category("FrontWork")>
    Public Property Model As IModel
        Get
            Return Me._model
        End Get
        Set(value As IModel)
            If Me._model IsNot Nothing Then
                Call Me.UnbindModel()
            End If
            Me._model = value
            If Me._model IsNot Nothing Then
                Call Me.BindModel()
            End If
        End Set
    End Property

    ''' <summary>
    ''' 配置中心对象，用来获取配置
    ''' </summary>
    ''' <returns>配置中心对象</returns>
    <Description("配置中心对象"), Category("FrontWork")>
    Public Property Configuration As Configuration
        Get
            Return Me._configuration
        End Get
        Set(value As Configuration)
            If value Is Me.Configuration Then Return
            If Me._configuration IsNot Nothing Then
                RemoveHandler Me._configuration.ConfigurationChanged, AddressOf Me.ConfigurationChanged
            End If
            Me._configuration = value
            If value IsNot Nothing Then
                AddHandler Me._configuration.ConfigurationChanged, AddressOf Me.ConfigurationChanged
            End If
            Call Me.ConfigurationChanged(Me, New ConfigurationChangedEventArgs)
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
            Call Me.InitEditPanel()
        End Set
    End Property

    Private Sub TableLayoutPanel_Paint(sender As Object, e As PaintEventArgs) Handles TableLayoutPanel.Paint

    End Sub


    Private _targetRow As Long
    Private switcherModelDataUpdatedEvent As Boolean = True
    Private switcherLocalEvents As Boolean = True '本View内部事件开关，包括文本框文字变化等。不包括外部，例如Model数据变化事件开关
    Private dicFieldNameColumn As New Dictionary(Of String, Integer)
    Private dicFieldUpdated As New Dictionary(Of String, Boolean)
    Private dicFieldEdited As New Dictionary(Of String, Boolean)

    Private Property Panel As TableLayoutPanel

    <Description("当前配置模式"), Category("FrontWork")>
    Public Property Mode As String Implements IView.Mode
        Get
            Return Me._mode
        End Get
        Set(value As String)
            If Me._mode = value Then Return
            Me._mode = value
            Call Me.ConfigurationChanged(Me, New ConfigurationChangedEventArgs)
            Call Me.ExportData()
            Call Me.ImportData()
        End Set
    End Property

    Public Sub New()
        Call InitializeComponent()
        Me.Font = New Font("黑体", 10)
        Me.Panel = Me.TableLayoutPanel
    End Sub

    ''' <summary>
    ''' 绑定新的Model，将本View的各种事件绑定到Model上以实现数据变化的同步
    ''' </summary>
    Protected Sub BindModel()
        AddHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
        AddHandler Me.Model.RowAdded, AddressOf Me.ModelRowAddedEvent
        AddHandler Me.Model.RowRemoved, AddressOf Me.ModelRowRemovedEvent
        AddHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
        AddHandler Me.Model.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
        AddHandler Me.Model.Refreshed, AddressOf Me.ModelRefreshedEvent

        Me.ImportData()
    End Sub

    ''' <summary>
    ''' 解绑Model，取消本视图绑定的所有事件
    ''' </summary>
    Protected Sub UnbindModel()
        RemoveHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
        RemoveHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
        RemoveHandler Me.Model.RowAdded, AddressOf Me.ModelRowAddedEvent
        RemoveHandler Me.Model.RowRemoved, AddressOf Me.ModelRowRemovedEvent
        RemoveHandler Me.Model.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
        RemoveHandler Me.Model.Refreshed, AddressOf Me.ModelRefreshedEvent

    End Sub

    Private Sub ConfigurationChanged(sender As Object, e As ConfigurationChangedEventArgs)
        Call Me.InitEditPanel()
    End Sub

    Private Function GetModelSelectedRow() As Long
        Logger.SetMode(LogMode.REFRESH_VIEW)
        If Me.Model Is Nothing Then
            Logger.PutMessage("Model not set!")
            Return -1
        End If
        If Me.Model.AllSelectionRanges.Length = 0 Then
            Return -1
        End If
        If Me.Model.AllSelectionRanges.Length > 1 Then
            'Logger.PutMessage("Multiple range selected, TableLayoutPanelView will only show data of the first range", LogLevel.WARNING)
        End If
        Dim range = Me.Model.AllSelectionRanges(0)
        If range.Rows > 1 Then
            'Logger.PutMessage("Multiple rows selected, TableLayoutPanelView will only show data of the first row", LogLevel.WARNING)
        End If
        Return range.Row
    End Function

    Private Sub ModelSelectionRangeChangedEvent(sender As Object, e As ModelSelectionRangeChangedEventArgs)
        Dim modelSelectedRow = Me.GetModelSelectedRow
        If modelSelectedRow < 0 Then
            Call Me.ClearPanelData()
            Return
        Else
            Me.ImportData()
        End If
    End Sub

    Private Sub ModelRowAddedEvent(sender As Object, e As ModelRowAddedEventArgs)
        Logger.Debug("TableLayoutView ModelRowAddedEvent: " & Str(Me.GetHashCode))
    End Sub

    Private Sub ModelRowRemovedEvent(sender As Object, e As ModelRowRemovedEventArgs)
        Logger.Debug("TableLayoutView ModelRowRemovedEvent: " & Str(Me.GetHashCode))
        Me.ImportData()
    End Sub

    Private Sub ModelCellUpdatedEvent(sender As Object, e As ModelCellUpdatedEventArgs)
        Logger.Debug("TableLayoutView ModelCellUpdatedEvent: " & Str(Me.GetHashCode))
        Logger.SetMode(LogMode.REFRESH_VIEW)
        Dim modelSelectedRow = Me.GetModelSelectedRow
        If modelSelectedRow < 0 Then
            Return
        End If
        '如果更新的行不包括本View的目标行，则不刷新
        For Each posCell In e.UpdatedCells
            If modelSelectedRow = posCell.Row Then
                Call Me.ImportData()
                Return
            End If
        Next
    End Sub

    Private Sub ModelRefreshedEvent(sender As Object, e As ModelRefreshedEventArgs)
        If switcherModelDataUpdatedEvent = False Then Return '开关被关闭则不执行事件
        Call Me.ImportData()
    End Sub

    Private Sub ModelRowUpdatedEvent(sender As Object, e As ModelRowUpdatedEventArgs)
        If switcherModelDataUpdatedEvent = False Then Return '开关被关闭则不执行事件
        Logger.Debug("TableLayoutView ModelRowUpdatedEvent: " & Str(Me.GetHashCode))
        Dim modelSelectedRow = Me.GetModelSelectedRow
        If modelSelectedRow < 0 Then Return
        Dim needToUpdate As Boolean = (From indexRow In e.UpdatedRows
                                       Where indexRow.Index = modelSelectedRow
                                       Select indexRow.Index).Count > 0
        If needToUpdate Then
            Call Me.ImportData()
        End If
    End Sub

    Private Sub CellUpdateEvent(fieldName As String)
        If Me.switcherLocalEvents = False Then Return
        Logger.Debug("TableLayoutView CellUpdateEvent: " & Str(Me.GetHashCode))
        Call Me.ExportField(fieldName)
    End Sub

    Private Sub ClearPanelData()
        Me.switcherLocalEvents = False
        For Each control As Control In Me.Panel.Controls
            Select Case control.GetType
                Case GetType(TextBox)
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
        Me.Panel.Controls.Clear()
        Dim fieldConfiguration As FieldConfiguration() = Me.Configuration.GetFieldConfigurations(Me.Mode)
        '初始化行列数量和大小
        Me.Panel.RowStyles.Clear()
        Me.Panel.ColumnStyles.Clear()
        Dim textBoxWidthPercent = 100 / Me.ItemsPerRow
        Dim fieldsPerRow As Integer = Me.ItemsPerRow
        If fieldsPerRow = 0 Then
            Call Me.Panel.ResumeLayout()
            Return
        End If
        Me.Panel.ColumnCount = fieldsPerRow * 2 '计算列数
        Me.Panel.RowCount = System.Math.Floor(fieldConfiguration.Length / fieldsPerRow) + If(fieldConfiguration.Length Mod fieldsPerRow = 0, 0, 1) '计算行数
        For j = 0 To Me.Panel.RowCount
            Me.Panel.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F / Me.Panel.RowCount))
        Next

        '遍历FieldConfiguration()
        Call Me.dicFieldNameColumn.Clear()
        Call Me.dicFieldUpdated.Clear()
        Dim col As Integer = -1 'i从0开始循环
        For Each curField As FieldConfiguration In fieldConfiguration
            col += 1
            Me.dicFieldNameColumn.Add(curField.Name, col)
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
                Dim textBox As New TextBox With {
                    .Name = curField.Name,
                    .ReadOnly = Not curField.Editable,
                    .Font = Me.Font,
                    .Dock = DockStyle.Fill
                }
                '绑定内容改变记录更新事件
                AddHandler textBox.TextChanged, AddressOf Me.TextBoxTextChangedEvent
                '如果设置了占位符，则想办法给它模拟出一个占位符来。windows居然不支持，呵呵
                If (curField.PlaceHolder IsNot Nothing) Then
                    '加一个label覆盖在上面，看着跟真的placeholder似的
                    Dim labelLayer As Label = New Label()
                    textBox.Controls.Add(labelLayer)
                    labelLayer.Text = curField.PlaceHolder
                    labelLayer.TextAlign = ContentAlignment.MiddleLeft
                    labelLayer.ForeColor = Color.Gray
                    labelLayer.Font = textBox.Font
                    labelLayer.Dock = DockStyle.Fill
                    labelLayer.AutoSize = True
                    AddHandler labelLayer.Click,
                        Sub(obj, e)
                            Call textBox.Focus()
                            '调用编辑框的点击事件
                            Dim onClickMethod = GetType(TextBox).GetMethod("OnClick", BindingFlags.NonPublic Or BindingFlags.Instance)
                            onClickMethod.Invoke(textBox, {EventArgs.Empty})
                        End Sub

                    '防止第一次显示的时候有字也显示占位符的囧境
                    AddHandler textBox.TextChanged,
                        Sub(obj, e)
                            If textBox.Text.Length <> 0 Then
                                Call labelLayer.Hide()
                            Else
                                textBox.Text = ""
                                Call labelLayer.Show()
                            End If
                        End Sub

                    AddHandler textBox.Click,
                        Sub(obj, e)
                            Call labelLayer.Hide()
                        End Sub
                    AddHandler textBox.Leave,
                        Sub(obj, e)
                            If (String.IsNullOrWhiteSpace(textBox.Text)) Then
                                textBox.Text = ""
                                Call labelLayer.Show()
                            End If
                        End Sub
                End If
                Me.Panel.Controls.Add(textBox)
                '如果是设计器调试，就不用绑定事件了
                If Me.DesignMode Then Continue For
                '绑定用户事件
                '输入联想
                If curField.Association IsNot Nothing Then
                    AddHandler textBox.Enter, Sub()
                                                  Me.FormAssociation.TextBox = textBox
                                                  FormAssociation.SetAssociationFunc(Function(str As String)
                                                                                         Dim ret = curField.Association.Invoke(Me, {str})
                                                                                         Return Util.ToArray(Of AssociationItem)(ret)
                                                                                     End Function)
                                              End Sub
                End If
                '内容改变事件
                If curField.ContentChanged IsNot Nothing Then
                    AddHandler textBox.TextChanged, Sub()
                                                        If Me.switcherLocalEvents = False Then Return
                                                        Logger.Debug("TableLayoutView TextBox TextChanged User Event: " & Str(Me.GetHashCode))
                                                        curField.ContentChanged.Invoke(Me, textBox.Text, Me.Model.SelectionRange.Row)
                                                    End Sub
                End If
                '编辑结束事件
                If curField.EditEnded IsNot Nothing Then
                    AddHandler textBox.Leave, Sub()
                                                  If Me.switcherLocalEvents = False Then Return
                                                  Logger.Debug("TableLayoutView TextBox Leave User Event: " & Str(Me.GetHashCode))
                                                  curField.EditEnded.Invoke(Me, textBox.Text, Me.Model.SelectionRange.Row)
                                              End Sub
                End If
                '绑定焦点离开自动保存事件
                AddHandler textBox.Leave, Sub()
                                              If Me.switcherLocalEvents = False Then Return
                                              Logger.Debug("TableLayoutView TextBox Leave Save Data: " & Str(Me.GetHashCode))
                                              Call Me.CellUpdateEvent(textBox.Name)
                                          End Sub
            Else '否则可以用ComboBox体现
                Dim comboBox As New ComboBox With {
                                                          .Name = curField.Name,
                                                          .Font = Me.Font,
                                                          .Enabled = curField.Editable,
                                                          .DropDownStyle = ComboBoxStyle.DropDownList,
                                                          .Dock = DockStyle.Fill
                                                      }
                '绑定内容改变记录更新事件
                AddHandler comboBox.SelectedIndexChanged, AddressOf Me.ComboBoxSelectedIndexChangedEvent
                Me.Panel.Controls.Add(comboBox)
                '如果是设计器调试，就不用触发和绑定事件了
                If Me.DesignMode Then Continue For

                Dim values As Object() = Util.ToArray(Of Object)(curField.Values.Invoke(Me))
                If values IsNot Nothing Then
                    comboBox.Items.AddRange(values)
                End If
                '绑定用户事件
                If curField.ContentChanged IsNot Nothing Then
                    AddHandler comboBox.SelectedIndexChanged, Sub()
                                                                  If Me.switcherLocalEvents = False Then Return
                                                                  Logger.Debug("TableLayoutView ComboBox SelectedIndexChanged User Event: " & Str(Me.GetHashCode))
                                                                  curField.ContentChanged.Invoke(Me, comboBox.SelectedItem?.ToString, Me.Model.SelectionRange.Row)
                                                              End Sub
                End If
                If curField.EditEnded IsNot Nothing Then
                    AddHandler comboBox.Leave, Sub()
                                                   If Me.switcherLocalEvents = False Then Return
                                                   Logger.Debug("TableLayoutView ComboBox Leave User Event: " & Str(Me.GetHashCode))
                                                   curField.EditEnded.Invoke(Me, comboBox.SelectedItem?.ToString, Me.Model.SelectionRange.Row)
                                               End Sub
                End If
                '绑定焦点离开自动保存事件
                AddHandler comboBox.Leave, Sub()
                                               If Me.switcherLocalEvents = False Then Return
                                               Logger.Debug("TableLayoutView ComboBox Leave Save Data: " & Str(Me.GetHashCode))
                                               Call Me.CellUpdateEvent(comboBox.Name)
                                           End Sub
            End If
        Next

        'For j = 0 To fieldsPerRow - 1
        '    Me.Panel.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 100))
        '    Me.Panel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, textBoxWidthPercent))
        'Next

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

        Call Me.Panel.ResumeLayout()

        RemoveHandler Me.Panel.Leave, AddressOf Me.TableLayoutPanel_Leave
        AddHandler Me.Panel.Leave, AddressOf Me.TableLayoutPanel_Leave

        RemoveHandler Me.Panel.Enter, AddressOf Me.TableLayoutPanel_Enter
        AddHandler Me.Panel.Enter, AddressOf Me.TableLayoutPanel_Enter

        Call Me.BindViewToJsEngine()
    End Sub

    Private Sub TableLayoutPanel_Leave(sender, e)
        Logger.Debug("TableLayoutView Panel Leave: " & Str(Me.GetHashCode))
        Me.switcherLocalEvents = False
    End Sub

    Private Sub TableLayoutPanel_Enter(sender, e)
        Logger.Debug("TableLayoutView Panel Enter: " & Str(Me.GetHashCode))
        Me.switcherLocalEvents = True
    End Sub

    '这里不包含用户事件，用户事件在创建时用Lambda表达式置入了已经
    Private Sub TextBoxTextChangedEvent(sender As Object, e As EventArgs)
        If Me.switcherLocalEvents = False Then Return
        Dim controlName = CType(sender, Control).Name
        If Not Me.dicFieldEdited.ContainsKey(controlName) Then
            Me.dicFieldEdited.Add(controlName, True)
        End If
    End Sub

    '这里不包含用户事件，用户事件在创建时用Lambda表达式置入了已经
    Private Sub ComboBoxSelectedIndexChangedEvent(sender As Object, e As EventArgs)
        If Me.switcherLocalEvents = False Then Return
        Dim controlName = CType(sender, Control).Name
        If Not Me.dicFieldEdited.ContainsKey(controlName) Then
            Me.dicFieldEdited.Add(controlName, True)
        End If
        Call Me.ExportField(controlName)
    End Sub

    ''' <summary>
    ''' 从Model导入数据
    ''' </summary>
    ''' <returns>是否导入成功</returns>
    Protected Function ImportData() As Boolean
        Logger.SetMode(LogMode.REFRESH_VIEW)
        If Me.Configuration Is Nothing Then
            Logger.PutMessage("Configuration is not setted")
            Return False
        End If
        If Me.Panel Is Nothing Then
            Logger.PutMessage("Panel is not setted")
            Return False
        End If
        Dim modelSelectedRow = Me.GetModelSelectedRow
        If modelSelectedRow < 0 Then
            Call Me.ClearPanelData()
            Return True
        End If
        If modelSelectedRow >= Me.Model.RowCount Then
            Logger.PutMessage("TargetRow(" & Str(modelSelectedRow) & ") exceeded the max row of model: " & Me.Model.RowCount - 1)
            Return False
        End If
        '清空面板
        Call Me.ClearPanelData()
        '获取数据
        Dim data = Me.Model.GetRows(New Long() {modelSelectedRow})
        '遍历Configuration的字段
        Dim fieldConfiguration = Me.Configuration.GetFieldConfigurations(Me.Mode)
        If fieldConfiguration Is Nothing Then
            Logger.PutMessage("Configuration not found!")
            Return False
        End If
        For Each curField In fieldConfiguration
            Dim curColumn As DataColumn = (From c As DataColumn In data.Columns
                                           Where c.ColumnName.Equals(curField.Name, StringComparison.OrdinalIgnoreCase)
                                           Select c).FirstOrDefault
            '在对象中找不到Configuration描述的字段，直接报错，并接着下一个字段
            If curColumn Is Nothing Then
                Logger.PutMessage("Field """ + curField.Name + """ not found in model")
                Continue For
            End If
            '否则开始Push值
            '先计算值，过一遍Mapper
            Dim value = data.Rows(0)(curColumn)
            Dim text As String

            If Not curField.ForwardMapper Is Nothing Then
                text = curField.ForwardMapper.Invoke(Me, value)
            Else
                text = If(value?.ToString, "")
            End If
            Logger.SetMode(LogMode.REFRESH_VIEW)
            '然后获取Control
            Dim curControl = (From control As Control In Me.Panel.Controls
                              Where control.Name = curField.Name
                              Select control).FirstOrDefault()
            If curControl Is Nothing Then
                'Logger.PutMessage(curField.Name + " not found in view!")
                Continue For
            End If
            '根据Control是文本框还是ComboBox，有不一样的行为
            Me.switcherLocalEvents = False '关闭本地事件开关， 防止连锁事件
            Select Case curControl.GetType()
                Case GetType(TextBox)
                    Dim textBox = CType(curControl, TextBox)
                    textBox.Text = text
                Case GetType(ComboBox)
                    Dim comboBox = CType(curControl, ComboBox)
                    Dim found = False
                    For i As Integer = 0 To comboBox.Items.Count - 1
                        If comboBox.Items(i).ToString = text Then
                            found = True
                            RemoveHandler comboBox.SelectedIndexChanged, AddressOf Me.ComboBoxSelectedIndexChangedEvent
                            comboBox.SelectedIndex = i
                            AddHandler comboBox.SelectedIndexChanged, AddressOf Me.ComboBoxSelectedIndexChangedEvent
                        End If
                    Next
                    If found = False Then
                        Logger.PutMessage("Value """ + text + """" + " not found in comboBox """ + curField.Name + """")
                    End If
            End Select
            Me.switcherLocalEvents = True
        Next
        Return True
    End Function

    ''' <summary>
    ''' 导出字段数据到Model
    ''' </summary>
    ''' <param name="fieldName">要导出的字段名</param>
    Protected Sub ExportField(fieldName As String)
        Logger.SetMode(LogMode.SYNC_FROM_VIEW)
        If Not Me.dicFieldEdited.ContainsKey(fieldName) Then Return
        Dim modelSelectedRow = Me.GetModelSelectedRow
        If modelSelectedRow < 0 Then
            Logger.PutMessage("TableLayoutPanelView export cell data failed, Invalid selection range in model")
            Return
        End If
        If modelSelectedRow < 0 Then '如果目标行为负，则认为未指向确定行，故不导出数据
            Return
        End If
        If modelSelectedRow >= Me.Model.RowCount Then '如果目标行超过Model的最大行，提示错误并返回
            Logger.PutMessage("TargetRow(" & Str(modelSelectedRow) & ") exceeded the max row of model: " & Me.Model.RowCount - 1)
            Return
        End If
        Dim Configuration = (From m As FieldConfiguration In Me.Configuration.GetFieldConfigurations(Me.Mode)
                             Where m.Name = fieldName
                             Select m).FirstOrDefault
        Dim value = Me.GetMappedValue(fieldName, Configuration)
        RemoveHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
        Me.Model.UpdateCell(modelSelectedRow, fieldName, value)
        AddHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
        Me.dicFieldEdited.Remove(fieldName)
    End Sub

    ''' <summary>
    ''' 导出所有数据到Model
    ''' </summary>
    Protected Sub ExportData()
        Logger.SetMode(LogMode.SYNC_FROM_VIEW)
        If Me.dicFieldEdited.Count = 0 Then
            Return
        End If
        Dim modelSelectedRow = Me.GetModelSelectedRow
        If modelSelectedRow < 0 Then
            Logger.PutMessage("TableLayoutPanelView export data failed, Invalid selection range in model")
            Return
        End If
        If modelSelectedRow < 0 Then '如果目标行为负，则认为未指向确定行，故不导出数据
            Return
        End If
        If modelSelectedRow >= Me.Model.RowCount Then '如果目标行超过Model的最大行，提示错误并返回
            Logger.PutMessage("TargetRow(" & Str(modelSelectedRow) & ") exceeded the max row of model: " & Me.Model.RowCount - 1)
            Return
        End If
        Dim dicData As New Dictionary(Of String, Object)
        For Each curField As FieldConfiguration In Me.Configuration.GetFieldConfigurations(Me.Mode)
            '如果字段不可见，则忽略
            If Not curField.Visible Then Continue For
            Dim value = Me.GetMappedValue(curField.Name, curField)
            If value Is Nothing Then Continue For

            '将新的值加入更新字典中
            dicData.Add(curField.Name, value)
        Next
        RemoveHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
        Call Me.Model.UpdateRow(modelSelectedRow, dicData)
        AddHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
        Call Me.dicFieldEdited.Clear()
    End Sub

    ''' <summary>
    ''' 获取字段的数据（经过Mapper等操作之后的最终结果）
    ''' </summary>
    ''' <param name="fieldName">字段名</param>
    ''' <param name="fieldConfiguration">字段Configuration</param>
    ''' <returns>字段的数据</returns>
    Protected Function GetMappedValue(fieldName As String, fieldConfiguration As FieldConfiguration) As Object
        '获取Control
        Dim curControl = (From control As Control In Me.Panel.Controls
                          Where control.Name = fieldName
                          Select control).FirstOrDefault
        If curControl Is Nothing Then
            Logger.PutMessage(fieldName + " not found in view!")
            Return Nothing
        End If

        '获取Control中的文字
        Dim text As String = Nothing
        Select Case curControl.GetType
            Case GetType(TextBox)
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
        '将文字经过ReverseMapper映射成转换后的value
        Dim value As Object
        If Not fieldConfiguration.BackwardMapper Is Nothing Then
            value = fieldConfiguration.BackwardMapper.Invoke(Me, text)
        Else
            value = text
        End If
        Return value
    End Function

    Private Sub BindViewToJsEngine()
        Dim jsEngine = Me.JsEngine
        Dim viewObj = jsEngine.Execute("view = {}").GetValue("view").AsObject
        For Each control In Me.Panel.Controls
            If TypeOf control Is Label Then Continue For '提示标签就不用加到js里了
            Try
                viewObj.Put(control.Name, JsValue.FromObject(jsEngine, control), True)

                If TypeOf control Is TextBox Then
                    Dim tmp = String.Format(
                         <string>
                             {0} = undefined
                             Object.defineProperty(
                                this,
                                "{0}",
                                {{get: function(){{
                                    return view.{0}.Text
                                }},
                                set: function(val){{
                                    view.{0}.Text = val
                                }} }}
                            )
                         </string>.Value, control.Name)
                    jsEngine.Execute(tmp)
                ElseIf TypeOf control Is ComboBox Then
                    Dim tmp = String.Format(
                         <string>
                             {0} = undefined
                             Object.defineProperty(
                                this,
                                "{0}",
                                {{get: function(){{
                                    return view.{0}.SelectedItem
                                }},
                                set: function(val){{
                                    for(var i=0;i &lt; view.{0}.Items.Count;i++){{
                                        if(view.{0}.Items[i] == val){{
                                            view.{0}.SelectedIndex = i; 
                                            return;
                                        }}
                                    }}
                                }} }}
                            )
                         </string>.Value, control.Name)
                    jsEngine.Execute(tmp)
                End If
            Catch ex As Exception
                Logger.SetMode(LogMode.INIT_VIEW)
                Logger.PutMessage(ex.Message)
            End Try
        Next
    End Sub

    Private Sub BasicView_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub BasicView_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If Me.Configuration IsNot Nothing Then Call Me.InitEditPanel()
    End Sub

    Private Sub TableLayoutPanel_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles TableLayoutPanel.MouseDoubleClick
        MessageBox.Show(Me.Size.Width & " x " & Me.Size.Height)
    End Sub

    ''' <summary>
    ''' 获取视图中的单元格
    ''' </summary>
    ''' <param name="row">行号，在BasicView中此参数被忽略</param>
    ''' <param name="fieldName">字段名</param>
    ''' <returns>单元格对象</returns>
    Public Function GetViewComponent(row As Long, fieldName As String) As IViewComponent Implements IDataView.GetViewComponent
        Dim foundControls = Me.Panel.Controls.Find(fieldName, False)
        If foundControls.Length = 0 Then
            Throw New Exception($"ViewComponent ""{fieldName}"" not found!")
        End If
        Return New BasicViewComponent(foundControls(0))
    End Function

    ''' <summary>
    ''' 获取视图中的单元格
    ''' </summary>
    ''' <param name="fieldName">字段名</param>
    ''' <returns>单元格对象</returns>
    Public Function GetViewComponent(fieldName As String) As IViewComponent
        Return Me.GetViewComponent(0, fieldName)
    End Function
End Class

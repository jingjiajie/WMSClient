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
    Implements IAssociableDataView

    Private COLOR_CELL_VALIDATION_ERROR As Color = Color.FromArgb(255, 107, 107)
    Private COLOR_CELL_VALIDATION_WARNING As Color = Color.FromArgb(255, 230, 109)

    Private Class ControlTag
        Public Sub New(index As Integer)
            Me.Index = index
        End Sub

        Public Property Index As Integer

    End Class

    Private Class CellInfo
        Public Property Data As Object
        Public Property State As ViewCellState = New ViewCellState(ValidationState.OK)

        Public Sub New(data As Object)
            Me.Data = data
        End Sub
    End Class

    Private _itemsPerRow As Integer = 3

    Private Property TargetRow As Integer = -1
    Private Property FormAssociation As New AdsorbableAssociationForm
    Private Property _TextBoxManager As New TextBoxManager
    Private Property _ComboBoxManager As New ComboBoxManager
    Private Property _LabelManager As New LabelManager
    Private Property ViewModel As New AssociableDataViewModel(Me)

    Private Property RecordedRows As New List(Of IDictionary(Of String, CellInfo))
    Private Property ViewColumns As New List(Of ViewColumn)

    ''' <summary>
    ''' Model对象，用来存取数据
    ''' </summary>
    ''' <returns>Model对象</returns>
    <Description("Model对象"), Category("FrontWork")>
    Public Property Model As IModel
        Get
            Return Me.ViewModel.Model
        End Get
        Set(value As IModel)
            Me.ViewModel.Model = value
        End Set
    End Property

    ''' <summary>
    ''' 配置中心对象，用来获取配置
    ''' </summary>
    ''' <returns>配置中心对象</returns>
    <Description("配置中心对象"), Category("FrontWork")>
    Public Property Configuration As Configuration
        Get
            Return Me.ViewModel.Configuration
        End Get
        Set(value As Configuration)
            Me.ViewModel.Configuration = value
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
            Call Me.Panel.SuspendLayout()
            Call Me.AdjustRowCountAndStyles()
            Call Me.AdjustColumnCount()
            Call Me.RearrangeControls()
            Call Me.AdjustColumnWidths()
            Call Me.Panel.ResumeLayout()
        End Set
    End Property

    Private Sub TableLayoutPanel_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private dicFieldEdited As New Dictionary(Of String, Boolean)
    Public Event BeforeSelectionRangeChange As EventHandler(Of BeforeViewSelectionRangeChangeEventArgs) Implements IAssociableDataView.BeforeSelectionRangeChange
    Public Event RowUpdated As EventHandler(Of ViewRowUpdatedEventArgs) Implements IAssociableDataView.RowUpdated
    Public Event RowAdded As EventHandler(Of ViewRowAddedEventArgs) Implements IAssociableDataView.RowAdded
    Public Event RowRemoved As EventHandler(Of ViewRowRemovedEventArgs) Implements IAssociableDataView.RowRemoved
    Public Event CellUpdated As EventHandler(Of ViewCellUpdatedEventArgs) Implements IAssociableDataView.CellUpdated
    Public Event SelectionRangeChanged As EventHandler(Of ViewSelectionRangeChangedEventArgs) Implements IAssociableDataView.SelectionRangeChanged
    Public Event EditStarted As EventHandler(Of ViewEditStartedEventArgs) Implements IAssociableDataView.EditStarted
    Public Event ContentChanged As EventHandler(Of ViewContentChangedEventArgs) Implements IAssociableDataView.ContentChanged
    Public Event EditEnded As EventHandler(Of ViewEditEndedEventArgs) Implements IAssociableDataView.EditEnded
    Public Event BeforeRowStateChange As EventHandler(Of ViewBeforeRowStateChangeEventArgs) Implements IDataView.BeforeRowStateChange
    Public Event RowStateChanged As EventHandler(Of ViewRowStateChangedEventArgs) Implements IDataView.RowStateChanged

    Public Custom Event AssociationItemSelected As EventHandler(Of ViewAssociationItemSelectedEventArgs) Implements IAssociableDataView.AssociationItemSelected
        AddHandler(value As EventHandler(Of ViewAssociationItemSelectedEventArgs))
            AddHandler Me.FormAssociation.AssociationItemSelected, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ViewAssociationItemSelectedEventArgs))
            RemoveHandler Me.FormAssociation.AssociationItemSelected, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ViewAssociationItemSelectedEventArgs)

        End RaiseEvent
    End Event

    Private Property Panel As TableLayoutPanel

    <Description("当前配置模式"), Category("FrontWork")>
    Public Property Mode As String
        Get
            Return Me.ViewModel.Mode
        End Get
        Set(value As String)
            Me.ViewModel.Mode = value
        End Set
    End Property

    Public Sub New()
        Call InitializeComponent()
        Me.Panel = Me.TableLayoutPanel
        Me.Panel.Enabled = False
    End Sub

    Private Sub UnbindComboBox(comboBox As ComboBox)
        RemoveHandler comboBox.Enter, AddressOf Me.Combobox_Enter
        RemoveHandler comboBox.Leave, AddressOf Me.ComboBox_Leave
        RemoveHandler comboBox.SelectedIndexChanged, AddressOf Me.ComboBox_SelectedIndexChanged
    End Sub

    Private Sub BindComboBox(comboBox As ComboBox)
        If Me.DesignMode Then Return '如果是设计器调试，就不用绑定事件了
        AddHandler comboBox.Enter, AddressOf Me.Combobox_Enter
        AddHandler comboBox.Leave, AddressOf Me.ComboBox_Leave
        AddHandler comboBox.SelectedIndexChanged, AddressOf Me.ComboBox_SelectedIndexChanged
    End Sub

    Private Sub BindTextBox(textBox As BasicViewTextBox)
        If Me.DesignMode Then Return  '如果是设计器调试，就不用绑定事件了
        AddHandler textBox.Leave, AddressOf Me.TextBox_Leave
        AddHandler textBox.Enter, AddressOf Me.TextBox_Enter
        AddHandler textBox.TextChanged, AddressOf Me.TextBox_TextChangedEvent
    End Sub

    Private Sub UnbindTextBox(textBox As BasicViewTextBox)
        RemoveHandler textBox.Leave, AddressOf Me.TextBox_Leave
        RemoveHandler textBox.Enter, AddressOf Me.TextBox_Enter
        RemoveHandler textBox.TextChanged, AddressOf Me.TextBox_TextChangedEvent
    End Sub

    Private Sub ClearPanelData()
        For Each control As Control In Me.Panel.Controls
            Select Case control.GetType
                Case GetType(BasicViewTextBox)
                    Call Me.UnbindTextBox(control)
                    control.Text = String.Empty
                    Call Me.BindTextBox(control)
                Case GetType(ComboBox)
                    Call Me.UnbindComboBox(control)
                    Dim comboBox As ComboBox = CType(control, ComboBox)
                    comboBox.SelectedIndex = -1
                    Call Me.BindComboBox(control)
            End Select
        Next
    End Sub

    Private Sub AdjustColumnWidths()
        Call Me.Panel.ColumnStyles.Clear()
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
        Dim comboBox = CType(sender, ComboBox)
        Dim fieldName = comboBox.Name

        If Not Me.dicFieldEdited.ContainsKey(fieldName) Then
            Me.dicFieldEdited.Add(fieldName, True)
        End If
        Call Me.ExportField(fieldName)
        Dim args = New ViewContentChangedEventArgs(Me.TargetRow, fieldName, Me.GetFieldValue(fieldName))
        RaiseEvent ContentChanged(Me, args)
    End Sub

    Private Sub Combobox_Enter(sender As Object, e As EventArgs)
        Dim comboBox As ComboBox = sender
        Dim fieldName = comboBox.Name
        Dim args As New ViewEditStartedEventArgs(Me.TargetRow, fieldName, Me.GetFieldValue(fieldName))
        RaiseEvent EditStarted(Me, args)
    End Sub

    Private Sub ComboBox_Leave(sender As Object, e As EventArgs)
        Dim fieldName = CType(sender, Control).Name
        '如果没被编辑则不进行处理
        If Not Me.dicFieldEdited.ContainsKey(fieldName) Then Return
        Dim curField = Me.Configuration.GetField(Me.Mode, fieldName)
        '否则保存数据+触发编辑结束事件
        Call Me.ExportField(fieldName)
        Dim args = New ViewEditEndedEventArgs(Me.TargetRow, fieldName, Me.GetFieldValue(fieldName))
        RaiseEvent EditEnded(Me, args)
    End Sub

    Private Sub TextBox_Leave(sender As Object, e As EventArgs)
        Dim fieldName = CType(sender, TextBox).Name
        '如果没被编辑则不进行处理
        If Not Me.dicFieldEdited.ContainsKey(fieldName) Then Return
        Dim curField = Me.Configuration.GetField(Me.Mode, fieldName)
        '否则保存数据+触发编辑结束事件
        Call Me.ExportField(fieldName)
        Dim args = New ViewEditEndedEventArgs(Me.TargetRow, fieldName, Me.GetFieldValue(fieldName))
        RaiseEvent EditEnded(Me, args)
    End Sub

    Private Sub TextBox_Enter(sender As Object, e As EventArgs)
        Dim textBox As TextBox = sender
        Dim fieldName = textBox.Name
        '绑定联想编辑框
        Me.FormAssociation.AdsorbTextBox = textBox
        Dim args As New ViewEditStartedEventArgs(Me.TargetRow, fieldName, Me.GetFieldValue(fieldName))
        RaiseEvent EditStarted(Me, args)
    End Sub

    Private Sub TableLayoutPanel_Leave(sender, e)
    End Sub

    Private Sub TableLayoutPanel_Enter(sender, e)
    End Sub

    Private Sub TextBox_TextChangedEvent(sender As Object, e As EventArgs)
        Dim textBox As TextBox = sender
        Dim fieldName = CType(sender, Control).Name
        If Not Me.dicFieldEdited.ContainsKey(fieldName) Then
            Me.dicFieldEdited.Add(fieldName, True)
        End If
        Dim args = New ViewContentChangedEventArgs(Me.TargetRow, fieldName, Me.GetFieldValue(fieldName))
        RaiseEvent ContentChanged(Me, args)
    End Sub

    ''' <summary>
    ''' 从缓存中将整行数据更新到视图上
    ''' </summary>
    Protected Sub PushRow(row As Integer)
        Call Me.ClearPanelData() '清空面板
        If row = -1 Then
            Me.Panel.Enabled = False
            Return
        End If
        Me.Panel.Enabled = True
        Dim rowData = Me.RecordedRows(row)
        For Each kv As KeyValuePair(Of String, CellInfo) In rowData
            Dim key = kv.Key
            Dim cellInfo = kv.Value
            Dim Text = If(cellInfo.Data?.ToString, "")

            Call Me.SetFieldValue(key, Text)
        Next
        Call Me.PaintRow(row)
    End Sub

    Private Sub PaintRow(row As Integer)
        Call Me.Panel.SuspendLayout()
        For i = 0 To Me.ViewColumns.Count - 1
            Dim fieldName = Me.ViewColumns(i).Name
            Dim control = Me.GetControlByName(fieldName)
            Dim oriColor = control.BackColor
            Dim targetColor As Color
            Dim cellState = Me.RecordedRows(row)(fieldName).State
            Dim message As String = cellState.ValidationState.Message
            If cellState.ValidationState.Type = ValidationStateType.ERROR Then
                targetColor = COLOR_CELL_VALIDATION_ERROR
            ElseIf cellState.ValidationState.Type = ValidationStateType.WARNING Then
                targetColor = COLOR_CELL_VALIDATION_WARNING
            Else
                targetColor = Color.White
            End If
            If oriColor = targetColor Then Continue For
            control.BackColor = targetColor
            If TypeOf (control) Is BasicViewTextBox Then
                DirectCast(control, BasicViewTextBox).HintMessage = message
            End If
        Next
        Call Me.Panel.ResumeLayout()
    End Sub

    ''' <summary>
    ''' 导出字段数据到Model
    ''' </summary>
    ''' <param name="fieldName">要导出的字段名</param>
    Protected Sub ExportField(fieldName As String)
        Logger.SetMode(LogMode.SYNC_FROM_VIEW)
        If Not Me.dicFieldEdited.ContainsKey(fieldName) Then Return
        Dim newFieldValue = Me.GetFieldValue(fieldName)
        Dim srcFieldValue = Nothing
        If Me.RecordedRows(Me.TargetRow).ContainsKey(fieldName) Then
            srcFieldValue = Me.RecordedRows(Me.TargetRow)(fieldName).Data
        End If

        Me.RecordedRows(Me.TargetRow)(fieldName).Data = newFieldValue '更新缓存值
        Dim cellUpdatedEventArgs As New ViewCellUpdatedEventArgs({New ViewCellInfo(Me.TargetRow, fieldName, newFieldValue)})
        RaiseEvent CellUpdated(Me, cellUpdatedEventArgs)

        Me.dicFieldEdited.Remove(fieldName)
    End Sub

    Private Function GetControlByName(fieldName As String) As Control
        Dim ctrl = (From control As Control In Me.Panel.Controls
                    Where control.Name = fieldName AndAlso control.GetType <> GetType(Label)
                    Select control).FirstOrDefault()
        Return ctrl
    End Function

    Private Sub SetFieldValue(fieldName As String, value As String)
        '然后获取Control
        Dim curControl = Me.GetControlByName(fieldName)
        If curControl Is Nothing Then
            Return
        End If
        '根据Control是文本框还是ComboBox，有不一样的行为
        Select Case curControl.GetType()
            Case GetType(BasicViewTextBox)
                Dim textBox = CType(curControl, TextBox)
                Call Me.UnbindTextBox(textBox)
                textBox.Text = value
                Call Me.BindTextBox(textBox)
            Case GetType(ComboBox)
                Dim comboBox = CType(curControl, ComboBox)
                Call Me.UnbindComboBox(comboBox)
                Dim found = False
                For i As Integer = 0 To comboBox.Items.Count - 1
                    If comboBox.Items(i).ToString = value Then
                        found = True
                        comboBox.SelectedIndex = i
                    End If
                Next
                If found = False Then
                    Logger.PutMessage("Value """ + value + """" + " not found in comboBox """ + fieldName + """")
                End If
                Call Me.BindComboBox(comboBox)
        End Select
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
    Private Sub InitLabel(label As Label, viewColumn As ViewColumn, index As Integer)
        With label
            .Text = viewColumn.DisplayName
            .Name = viewColumn.Name
            .Font = Me.Font
            .Dock = DockStyle.Fill
            .Margin = New Padding(0)
            .Tag = New ControlTag(index)
        End With
    End Sub

    Private Sub InitTextBox(textBox As BasicViewTextBox, viewColumn As ViewColumn, index As Integer)
        Call Me.UnbindTextBox(textBox)
        '创建编辑框
        With textBox
            .Text = Nothing
            .Name = viewColumn.Name
            .PlaceHolder = viewColumn.PlaceHolder
            .ReadOnly = Not viewColumn.Editable
            .Font = Me.Font
            .Dock = DockStyle.Fill
            .Padding = New Padding(0)
            .Tag = New ControlTag(index)
            .HintMessage = Nothing
        End With
        Call Me.BindTextBox(textBox)
    End Sub

    Private Sub InitComboBox(comboBox As ComboBox, viewColumn As ViewColumn, index As Integer)
        Call Me.UnbindComboBox(comboBox)
        With comboBox
            .Name = viewColumn.Name
            .Font = Me.Font
            .Enabled = viewColumn.Editable
            .DropDownStyle = ComboBoxStyle.DropDownList
            .Dock = DockStyle.Fill
            .Padding = New Padding(0)
            .Tag = New ControlTag(index)
        End With
        Call comboBox.Items.Clear()
        Dim values As Object() = viewColumn.Values
        If values IsNot Nothing Then
            comboBox.Items.AddRange(values)
        End If
        Call Me.BindComboBox(comboBox)
    End Sub

    Private Sub AdjustRowCountAndStyles()
        '重新计算行数。如果行数发生变化，则调整行高
        Dim rowCount = System.Math.Floor(Me.ViewColumns.Count / Me.ItemsPerRow) + If(Me.ViewColumns.Count Mod Me.ItemsPerRow = 0, 0, 1) '计算行数
        Me.Panel.RowCount = rowCount
        Me.Panel.RowStyles.Clear()
        For i = 0 To rowCount - 1
            Me.Panel.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F / rowCount))
        Next
    End Sub

    Private Sub AdjustColumnCount()
        '初始化行列数量和大小
        Me.Panel.ColumnStyles.Clear()
        If Me.ItemsPerRow = 0 Then
            Return
        End If
        Me.Panel.ColumnCount = Me.ItemsPerRow * 2 '计算列数
    End Sub

    Private Sub RearrangeControls()
        Dim controlList As New List(Of Control)
        Dim rows = Me.Panel.RowCount
        Dim cols = Me.Panel.ColumnCount
        For Each row In Util.Range(0, rows)
            For Each col In Util.Range(0, cols)
                Dim curControl = Me.Panel.GetControlFromPosition(col, row)
                If curControl IsNot Nothing Then
                    controlList.Add(curControl)
                End If
            Next
        Next
        For Each curControl In controlList
            Me.Panel.Controls.Add(curControl)
        Next
    End Sub

    Public Function AddColumns(columns() As ViewColumn) As Boolean Implements IAssociableDataView.AddColumns
        Static inited = False
        '如果第一次增加列，则将BasicView的默认页面清空
        If Not inited Then
            inited = True
            Me.BorderStyle = BorderStyle.None
            Me.Panel.Controls.Clear()
            Call Me.AdjustColumnCount()
        End If
        Dim startIndex = Me.ViewColumns.Count
        '更新记录的ViewColumns
        Call Me.ViewColumns.AddRange(columns)
        '更新视图
        Call Me.TableLayoutPanel.SuspendLayout()
        Call Me.AdjustRowCountAndStyles()
        For i = 0 To columns.Length - 1
            Dim viewColumn = columns(i)
            Dim index = startIndex + i
            Dim label = Me._LabelManager.PopControl
            Call Me.InitLabel(label, viewColumn, index)
            Me.Panel.Controls.Add(label)
            '如果没有设定Values字段，认为可以用编辑框体现
            If viewColumn.Values Is Nothing Then
                Dim textBox = Me._TextBoxManager.PopControl()
                Call Me.InitTextBox(textBox, viewColumn, index)
                '将编辑框添加到Panel里
                Me.Panel.Controls.Add(textBox)
            Else
                Dim comboBox = Me._ComboBoxManager.PopControl
                Call Me.InitComboBox(comboBox, viewColumn, index)
                Me.Panel.Controls.Add(comboBox)
            End If
        Next
        Call Me.AdjustColumnWidths()
        Call Me.TableLayoutPanel.ResumeLayout()
        Return True
    End Function

    Public Function UpdateColumns(indexes() As Integer, columns() As ViewColumn) As Object Implements IAssociableDataView.UpdateColumns
        '更新记录的ViewColumns
        For i = 0 To indexes.Length - 1
            Dim index = indexes(i)
            Dim column = columns(i)
            Me.ViewColumns(index) = column
        Next

        '更新视图
        Call Me.TableLayoutPanel.SuspendLayout()
        For i = 0 To indexes.Length - 1
            Dim index = indexes(i)
            Dim newColumn = columns(i)
            For Each ctl As Control In Me.TableLayoutPanel.Controls
                If ctl.Tag.Index <> index Then Continue For
                Select Case ctl.GetType
                    Case GetType(Label)
                        Call Me.InitLabel(ctl, newColumn, index)
                    Case GetType(BasicViewTextBox)
                        Call Me.InitTextBox(ctl, newColumn, index)
                    Case GetType(ComboBox)
                        Call Me.InitComboBox(ctl, newColumn, index)
                End Select
            Next
        Next
        Call Me.AdjustColumnWidths()
        Call Me.TableLayoutPanel.ResumeLayout()
        Return True
    End Function

    Public Function RemoveColumns(indexes() As Integer) As Object Implements IAssociableDataView.RemoveColumns
        Call Me.TableLayoutPanel.SuspendLayout()
        '先删除记录的ViewColumns
        Dim indexesDesc = (From i In indexes Order By i Descending Select i).ToArray
        For Each index In indexesDesc
            Me.ViewColumns.RemoveAt(index)
        Next
        '调整行高
        Call Me.AdjustRowCountAndStyles()
        '收集需要删除的Control
        Dim removeControls As New List(Of Control)
        For Each ctl As Control In Me.TableLayoutPanel.Controls
            '如果是被删除的index，则记录要删除的Control
            If indexes.Contains(ctl.Tag.Index) Then
                removeControls.Add(ctl)
            Else '否则，有几个在之前的Control被删除，则index就减几
                Dim tag = CType(ctl.Tag, ControlTag)
                tag.Index -= (From i In indexes Where i < tag.Index Select i).Count
            End If
        Next
        '开始删除Controls
        If removeControls.Count > 0 Then
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
                        Call Me._ComboBoxManager.PushControl(ctl)
                End Select
            Next
            Call Me.AdjustColumnWidths()
            Call Me.TableLayoutPanel.ResumeLayout()
        End If
        Return True
    End Function

    Public Function AddRows(data() As IDictionary(Of String, Object)) As Integer() Implements IAssociableDataView.AddRows
        Dim addRowData(data.Length - 1) As Dictionary(Of String, CellInfo)
        For i = 0 To data.Length - 1
            addRowData(i) = Me.RawDataToCellInfos(data(i))
        Next
        Me.RecordedRows.AddRange(addRowData)
        Return Nothing
    End Function

    Private Function RawDataToCellInfos(data As IDictionary(Of String, Object)) As Dictionary(Of String, CellInfo)
        Dim newData As New Dictionary(Of String, CellInfo)
        For Each viewColumn In Me.ViewColumns
            Dim cellInfo As CellInfo
            If data.ContainsKey(viewColumn.Name) Then
                cellInfo = New CellInfo(data(viewColumn.Name))
            Else
                cellInfo = New CellInfo(Nothing)
            End If
            newData.Add(viewColumn.Name, cellInfo)
        Next
        Return newData
    End Function

    Public Sub InsertRows(rows() As Integer, data() As IDictionary(Of String, Object)) Implements IAssociableDataView.InsertRows
        Dim oriRowCount = Me.RecordedRows.Count
        '原始行每次插入之后，行号会变，所以做调整
        Dim indexRowDataPairs(rows.Length - 1) As IndexRowDataPair
        For i = 0 To rows.Length - 1
            indexRowDataPairs(i) = New IndexRowDataPair() With {
                .Index = rows(i),
                .RowData = Me.RawDataToCellInfos(data(i))
            }
        Next
        Dim adjustedIndexDataPairs = (From i In indexRowDataPairs Order By i.Index Ascending Select i).ToArray
        For i = 0 To adjustedIndexDataPairs.Length - 1
            adjustedIndexDataPairs(i).Index = adjustedIndexDataPairs(i).Index + System.Math.Min(oriRowCount, i)
        Next

        For Each pair In adjustedIndexDataPairs
            Call Me.RecordedRows.Insert(pair.Index, pair.RowData)
        Next
    End Sub

    Public Sub RemoveRows(rows() As Integer) Implements IAssociableDataView.RemoveRows
        Dim rowsDESC = (From r In rows.Distinct Order By r Descending Select r).ToArray
        For Each row In rowsDESC
            Call Me.RecordedRows.RemoveAt(row)
        Next
    End Sub

    Public Sub UpdateRows(rows() As Integer, dataOfEachRow() As IDictionary(Of String, Object)) Implements IAssociableDataView.UpdateRows
        For i = 0 To rows.Length - 1
            Dim rowData = Me.RecordedRows(rows(i))
            For Each item In dataOfEachRow(i)
                If Not rowData.ContainsKey(item.Key) Then Continue For
                rowData(item.Key).Data = item.Value
            Next
            If rows(i) = Me.TargetRow Then
                Call Me.PushRow(Me.TargetRow)
            End If
        Next
    End Sub

    Public Sub UpdateCells(rows() As Integer, columnNames() As String, dataOfEachCell() As Object) Implements IAssociableDataView.UpdateCells
        For i = 0 To rows.Length - 1
            Dim row = rows(i)
            Dim columnName = columnNames(i)
            Dim data = dataOfEachCell(i)
            Dim recordedRowData = Me.RecordedRows(row)
            recordedRowData(columnName).Data = data
        Next

        If rows.Contains(Me.TargetRow) Then
            Call Me.PushRow(Me.TargetRow)
        End If
    End Sub

    Public Function GetSelectionRanges() As Range() Implements IAssociableDataView.GetSelectionRanges
        Throw New NotImplementedException()
    End Function

    Public Sub SetSelectionRanges(ranges() As Range) Implements IAssociableDataView.SetSelectionRanges
        If ranges.Length = 0 Then
            Me.TargetRow = -1
        Else
            Me.TargetRow = ranges(0).Row
        End If
        Call Me.PushRow(Me.TargetRow)
    End Sub

    Public Function GetRowCount() As Integer Implements IAssociableDataView.GetRowCount
        Return Me.RecordedRows.Count
    End Function

    Public Function GetColumns() As ViewColumn() Implements IAssociableDataView.GetColumns
        Return Me.ViewColumns.ToArray
    End Function

    Public Function GetColumnCount() As Integer Implements IAssociableDataView.GetColumnCount
        Return Me.ViewColumns.Count
    End Function

    Public Sub ShowAssociationForm() Implements IAssociableDataView.ShowAssociationForm
        Call Me.FormAssociation.Show()
    End Sub

    Public Sub UpdateAssociationItems(associationItems() As AssociationItem) Implements IAssociableDataView.UpdateAssociationItems
        Me.FormAssociation.UpdateAssociationItems(associationItems)
    End Sub

    Public Sub HideAssociationForm() Implements IAssociableDataView.HideAssociationForm
        Call Me.FormAssociation.Hide()
    End Sub

    Public Sub UpdateRowStates(rows() As Integer, states() As ViewRowState) Implements IDataView.UpdateRowStates
        Return
    End Sub

    Public Function GetRowStates(rows() As Integer) As ViewRowState() Implements IDataView.GetRowStates
        Return Nothing
    End Function

    Public Function GetCellStates(rows() As Integer, fields() As String) As ViewCellState() Implements IDataView.GetCellStates
        Throw New NotImplementedException
    End Function

    Public Sub UpdateCellStates(rows() As Integer, fields() As String, states() As ViewCellState) Implements IDataView.UpdateCellStates
        For i = 0 To rows.Length - 1
            Dim row = rows(i)
            Dim field = fields(i)
            Dim state = states(i)
            Me.RecordedRows(row)(field).State = state
            If row = Me.TargetRow Then
                Call Me.PaintRow(row)
            End If
        Next
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

    Private Structure IndexRowDataPair
        Property Index As Integer
        Property RowData As IDictionary(Of String, CellInfo)
    End Structure
End Class

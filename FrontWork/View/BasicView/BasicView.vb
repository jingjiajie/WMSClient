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


    Private _itemsPerRow As Integer = 3

    Private Property _TargetRow As Integer = -1
    Private Property FormAssociation As New AdsorbableAssociationForm
    Private Property _TextBoxManager As New TextBoxManager
    Private Property _ComboBoxManager As New ComboBoxManager
    Private Property _LabelManager As New LabelManager
    Private Property _ViewModel As New AssociableDataViewModel(Me)

    Private Property _RecordedRows As New List(Of IDictionary(Of String, Object))
    Private Property _Columns As New List(Of ViewColumn)

    ''' <summary>
    ''' Model对象，用来存取数据
    ''' </summary>
    ''' <returns>Model对象</returns>
    <Description("Model对象"), Category("FrontWork")>
    Public Property Model As IModel
        Get
            Return Me._ViewModel.Model
        End Get
        Set(value As IModel)
            Me._ViewModel.Model = value
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
    Public Event BeforeRowUpdate As EventHandler(Of BeforeViewRowUpdateEventArgs) Implements IAssociableDataView.BeforeRowUpdate
    Public Event BeforeRowAdd As EventHandler(Of BeforeViewRowAddEventArgs) Implements IAssociableDataView.BeforeRowAdd
    Public Event BeforeRowRemove As EventHandler(Of BeforeViewRowRemoveEventArgs) Implements IAssociableDataView.BeforeRowRemove
    Public Event BeforeCellUpdate As EventHandler(Of BeforeViewCellUpdateEventArgs) Implements IAssociableDataView.BeforeCellUpdate
    Public Event BeforeSelectionRangeChange As EventHandler(Of BeforeViewSelectionRangeChangeEventArgs) Implements IAssociableDataView.BeforeSelectionRangeChange
    Public Event RowUpdated As EventHandler(Of ViewRowUpdatedEventArgs) Implements IAssociableDataView.RowUpdated
    Public Event RowAdded As EventHandler(Of ViewRowAddedEventArgs) Implements IAssociableDataView.RowAdded
    Public Event RowRemoved As EventHandler(Of ViewRowRemovedEventArgs) Implements IAssociableDataView.RowRemoved
    Public Event CellUpdated As EventHandler(Of ViewCellUpdatedEventArgs) Implements IAssociableDataView.CellUpdated
    Public Event SelectionRangeChanged As EventHandler(Of ViewSelectionRangeChangedEventArgs) Implements IAssociableDataView.SelectionRangeChanged
    Public Event EditStarted As EventHandler(Of ViewEditStartedEventArgs) Implements IAssociableDataView.EditStarted
    Public Event ContentChanged As EventHandler(Of ViewContentChangedEventArgs) Implements IAssociableDataView.ContentChanged
    Public Event EditEnded As EventHandler(Of ViewEditEndedEventArgs) Implements IAssociableDataView.EditEnded

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
            Return Me._ViewModel.Mode
        End Get
        Set(value As String)
            Me._ViewModel.Mode = value
        End Set
    End Property

    Public Sub New()
        Call InitializeComponent()
        Me.Panel = Me.TableLayoutPanel
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
        Dim args = New ViewContentChangedEventArgs(Me._TargetRow, fieldName, Me.GetFieldValue(fieldName))
    End Sub

    Private Sub Combobox_Enter(sender As Object, e As EventArgs)
        Dim comboBox As ComboBox = sender
        Dim fieldName = comboBox.Name
        Dim args As New ViewEditStartedEventArgs(Me._TargetRow, fieldName, Me.GetFieldValue(fieldName))
        RaiseEvent EditStarted(Me, args)
    End Sub

    Private Sub ComboBox_Leave(sender As Object, e As EventArgs)
        Dim fieldName = CType(sender, Control).Name
        '如果没被编辑则不进行处理
        If Not Me.dicFieldEdited.ContainsKey(fieldName) Then Return
        Dim curField = Me.Configuration.GetFieldConfiguration(Me.Mode, fieldName)
        '否则保存数据+触发编辑结束事件
        Call Me.ExportField(fieldName)
        Dim args = New ViewEditEndedEventArgs(Me._TargetRow, fieldName, Me.GetFieldValue(fieldName))
        RaiseEvent EditEnded(Me, args)
    End Sub

    Private Sub TextBox_Leave(sender As Object, e As EventArgs)
        Dim fieldName = CType(sender, TextBox).Name
        '如果没被编辑则不进行处理
        If Not Me.dicFieldEdited.ContainsKey(fieldName) Then Return
        Dim curField = Me.Configuration.GetFieldConfiguration(Me.Mode, fieldName)
        '否则保存数据+触发编辑结束事件
        Call Me.ExportField(fieldName)
        Dim args = New ViewEditEndedEventArgs(Me._TargetRow, fieldName, Me.GetFieldValue(fieldName))
        RaiseEvent EditEnded(Me, args)
    End Sub

    Private Sub TextBox_Enter(sender As Object, e As EventArgs)
        Dim textBox As TextBox = sender
        Dim fieldName = textBox.Name
        '绑定联想编辑框
        Me.FormAssociation.AdsorbTextBox = textBox
        Dim args As New ViewEditStartedEventArgs(Me._TargetRow, fieldName, Me.GetFieldValue(fieldName))
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
        Dim args = New ViewContentChangedEventArgs(Me._TargetRow, fieldName, Me.GetFieldValue(fieldName))
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
        Dim rowData = Me._RecordedRows(row)
        For Each kv As KeyValuePair(Of String, Object) In rowData
            Dim key = kv.Key
            Dim value = kv.Value
            Dim Text = If(value?.ToString, "")

            Call Me.SetFieldValue(key, Text)
        Next
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
        If Me._RecordedRows(Me._TargetRow).ContainsKey(fieldName) Then
            srcFieldValue = Me._RecordedRows(Me._TargetRow)(fieldName)
        End If
        Dim beforeCellUpdateEventArgs As New BeforeViewCellUpdateEventArgs({New ViewCellInfo(Me._TargetRow, fieldName, newFieldValue)})
        RaiseEvent BeforeCellUpdate(Me, beforeCellUpdateEventArgs)
        If beforeCellUpdateEventArgs.Cancel Then
            Call Me.SetFieldValue(fieldName, srcFieldValue)
        Else
            Me._RecordedRows(Me._TargetRow)(fieldName) = newFieldValue '更新缓存值
            Dim cellUpdatedEventArgs As New ViewCellUpdatedEventArgs({New ViewCellInfo(Me._TargetRow, fieldName, newFieldValue)})
            RaiseEvent CellUpdated(Me, cellUpdatedEventArgs)
        End If
        Me.dicFieldEdited.Remove(fieldName)
    End Sub

    Private Sub SetFieldValue(fieldName As String, value As String)
        '然后获取Control
        Dim curControl = (From control As Control In Me.Panel.Controls
                          Where control.Name = fieldName AndAlso control.GetType <> GetType(Label)
                          Select control).FirstOrDefault()
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
        Call Me.UnbindTextBox(textBox)
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
        Call Me.UnbindComboBox(comboBox)
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

    Private Sub AdjustRowCountAndStyles()
        '重新计算行数。如果行数发生变化，则调整行高
        Dim rowCount = System.Math.Floor(Me._Columns.Count / Me.ItemsPerRow) + If(Me._Columns.Count Mod Me.ItemsPerRow = 0, 0, 1) '计算行数
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
        Call Me._Columns.AddRange(columns)

        Call Me.TableLayoutPanel.SuspendLayout()
        '如果第一次增加列，则将BasicView的默认页面清空
        If Not inited Then
            inited = True
            Me.BorderStyle = BorderStyle.None
            Me.Panel.Controls.Clear()
            Call Me.AdjustColumnCount()
        End If

        Call Me.AdjustRowCountAndStyles()
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
        Call Me.AdjustColumnWidths()
        Call Me.TableLayoutPanel.ResumeLayout()
        Return True
    End Function

    Public Function UpdateColumns(columnNames() As String, columns() As ViewColumn) As Object Implements IAssociableDataView.UpdateColumns
        For i = 0 To columnNames.Length - 1
            Dim columnName = columnNames(i)
            Dim column = columns(i)
            For j = 0 To Me._Columns.Count - 1
                If Me._Columns(j).Name = columnName Then
                    Me._Columns(j) = column
                    Exit For
                End If
            Next
        Next
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
        Call Me.AdjustColumnWidths()
        Call Me.TableLayoutPanel.ResumeLayout()
        Return True
    End Function

    Public Function RemoveColumns(columnNames() As String) As Object Implements IAssociableDataView.RemoveColumns
        Me._Columns.RemoveAll(Function(column)
                                  Return columnNames.Contains(column.Name)
                              End Function)

        Call Me.TableLayoutPanel.SuspendLayout()
        Call Me.AdjustRowCountAndStyles()

        '收集需要删除的Control
        Dim removeControls As New List(Of Control)
        For Each ctl As Control In Me.TableLayoutPanel.Controls
            Dim name = ctl.Name

            If columnNames.Contains(name) Then
                removeControls.Add(ctl)
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
        Me._RecordedRows.AddRange(data)
        Return Nothing
    End Function

    Public Sub InsertRows(rows() As Integer, data() As IDictionary(Of String, Object)) Implements IAssociableDataView.InsertRows
        Dim oriRowCount = Me._RecordedRows.Count
        '原始行每次插入之后，行号会变，所以做调整
        Dim indexDataPairs(rows.Length - 1) As IndexDataPair
        For i = 0 To rows.Length - 1
            indexDataPairs(i) = New IndexDataPair() With {
                .Index = rows(i),
                .Data = data(i)
            }
        Next
        Dim adjustedIndexDataPairs = (From i In indexDataPairs Order By i.Index Ascending Select i).ToArray
        For i = 0 To adjustedIndexDataPairs.Length - 1
            adjustedIndexDataPairs(i).Index = adjustedIndexDataPairs(i).Index + System.Math.Min(oriRowCount, i)
        Next

        For Each indexDataPair In adjustedIndexDataPairs
            Call Me._RecordedRows.Insert(indexDataPair.Index, indexDataPair.Data)
        Next
    End Sub

    Public Sub RemoveRows(rows() As Integer) Implements IAssociableDataView.RemoveRows
        Dim rowsDESC = (From r In rows.Distinct Order By r Descending Select r).ToArray
        For Each row In rowsDESC
            Call Me._RecordedRows.RemoveAt(row)
        Next
    End Sub

    Public Sub UpdateRows(rows() As Integer, dataOfEachRow() As IDictionary(Of String, Object)) Implements IAssociableDataView.UpdateRows
        For i = 0 To rows.Length - 1
            Me._RecordedRows(rows(i)) = dataOfEachRow(i)
        Next
        If rows.Contains(Me._TargetRow) Then
            Call Me.PushRow(Me._TargetRow)
        End If
    End Sub

    Public Sub UpdateCells(rows() As Integer, columnNames() As String, dataOfEachCell() As Object) Implements IAssociableDataView.UpdateCells
        For i = 0 To rows.Length - 1
            Dim row = rows(i)
            Dim columnName = columnNames(i)
            Dim data = dataOfEachCell(i)
            Dim recordedRowData = Me._RecordedRows(row)
            recordedRowData(columnName) = data
        Next

        If rows.Contains(Me._TargetRow) Then
            Call Me.PushRow(Me._TargetRow)
        End If
    End Sub

    Public Function GetSelectionRanges() As Range() Implements IAssociableDataView.GetSelectionRanges
        Throw New NotImplementedException()
    End Function

    Public Sub SetSelectionRanges(ranges() As Range) Implements IAssociableDataView.SetSelectionRanges
        If ranges.Length = 0 Then
            Me._TargetRow = -1
        Else
            Me._TargetRow = ranges(0).Row
        End If
        Call Me.PushRow(Me._TargetRow)
    End Sub

    Public Function GetRowCount() As Integer Implements IAssociableDataView.GetRowCount
        Return Me._RecordedRows.Count
    End Function

    Public Function GetColumns() As ViewColumn() Implements IAssociableDataView.GetColumns
        Return Me._Columns.ToArray
    End Function

    Public Function GetColumnCount() As Integer Implements IAssociableDataView.GetColumnCount
        Return Me._Columns.Count
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

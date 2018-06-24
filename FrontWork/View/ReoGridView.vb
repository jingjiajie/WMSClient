Imports FrontWork
Imports Jint.Native
Imports System.ComponentModel
Imports System.Linq
Imports System.Threading
Imports unvell.ReoGrid
Imports unvell.ReoGrid.CellTypes
Imports unvell.ReoGrid.Events

''' <summary>
''' ReoGrid表格视图，将配置的数据以表格的形式呈现出来。
''' 表格控件使用ReoGrid开源控件，感谢ReoGrid作者提供的优秀控件。
''' </summary>
Public Class ReoGridView
    Implements IView

    Private Sub ReoGridControl_Click(sender As Object, e As EventArgs) Handles ReoGridControl.Click

    End Sub

    ''' <summary>
    ''' 同步模式
    ''' </summary>
    Protected Enum SyncMode
        ''' <summary>
        ''' 与Model保持同步
        ''' </summary>
        SYNC
        ''' <summary>
        ''' 与Model脱离同步
        ''' </summary>
        NOT_SYNC
    End Enum

    ''' <summary>
    ''' 单元格状态，绘制单元格颜色用
    ''' </summary>
    Protected Enum CellState
        [Default] = 0
        UNSYNCHRONIZED = 1
        INVALID_DATA = 2
    End Enum

    Private COLOR_UNSYNCHRONIZED As Color = Color.AliceBlue
    Private COLOR_SYNCHRONIZED As Color = Color.Transparent

    Private canChangeSelectionRange As Boolean = True
    Private copied As Boolean = False '是否复制粘贴。如果为真，则下次选区改变事件时处理粘贴后的选区
    Private copyStartCell As CellPosition '复制起始单元格。用来判断复制是否选区没变导致不会触发选区改变事件
    Private dicCellDataChangedEvent As New Dictionary(Of Integer, FieldMethod) 'CellDataChanged时触发的事件列表(ContentChanged)
    Private dicTextChangedEvent As New Dictionary(Of Integer, FieldMethod) '编辑框文本改变时触发的事件列表(ContentChanged)
    Private dicBeforeSelectionRangeChangeEvent As New Dictionary(Of Integer, FieldMethod) '选择编辑框改变时触发的事件列表(EditEnded)
    Private dicNameColumn As New Dictionary(Of String, Integer)
    Private dicCellState As New Dictionary(Of Integer, Dictionary(Of Integer, Long))
    Private textBox As TextBox = Nothing
    Private formAssociation As FormAssociation
    Private Workbook As ReoGridControl = Nothing
    Private _mode As String = "default"

    Private RowInited As New List(Of Integer) '已经初始化过的行，保证每行只初始化一次
    Private dicCellEdited As New Dictionary(Of CellPosition, Boolean)
    Private _curSyncMode = SyncMode.NOT_SYNC
    Private _configuration As Configuration
    Private _model As Model

    Public Property Panel As Worksheet
    Private Property JsEngine As New Jint.Engine

    ''' <summary>
    ''' 同步模式，是否和Model数据是同步的
    ''' （如果Model没有数据，本视图上显示“暂无数据”，就处于不同步状态）
    ''' </summary>
    ''' <returns>同步模式</returns>
    Protected Property CurSyncMode As SyncMode
        Get
            Return Me._curSyncMode
        End Get
        Set(value As SyncMode)
            If Me._curSyncMode = value Then Return
            Console.WriteLine("CurSyncMode Changing:" & CStr(value))
            Me._curSyncMode = value
            Call Me.dicCellState.Clear()
            If value = SyncMode.SYNC Then
                Me.formAssociation.StayUnvisible = False
                Me.ReoGridControl.Enabled = True
            Else
                Me.formAssociation.StayUnvisible = True
            End If
        End Set
    End Property

    ''' <summary>
    ''' 绑定的Model对象，用来存取数据
    ''' </summary>
    ''' <returns>Model对象</returns>
    <Description("Model对象"), Category("FrontWork")>
    Public Property Model As Model
        Get
            Return Me._model
        End Get
        Set(value As Model)
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
    ''' 绑定的配置中心对象，用来获取配置
    ''' </summary>
    ''' <returns>配置中心对象</returns>
    <Description("配置中心对象"), Category("FrontWork")>
    Public Property Configuration As Configuration
        Get
            Return Me._configuration
        End Get
        Set(value As Configuration)
            If Me._configuration IsNot Nothing Then
                RemoveHandler Me._configuration.ConfigurationChanged, AddressOf Me.ConfigurationChanged
            End If
            Me._configuration = value
            If Me._configuration IsNot Nothing Then
                AddHandler Me._configuration.ConfigurationChanged, AddressOf Me.ConfigurationChanged
            End If
            Call Me.ConfigurationChanged(Me, New ConfigurationChangedEventArgs)
        End Set
    End Property

    ''' <summary>
    ''' 当前模式
    ''' </summary>
    ''' <returns></returns>
    <Description("当前配置模式"), Category("FrontWork")>
    Public Property Mode As String
        Get
            Return Me._mode
        End Get
        Set(value As String)
            If Me._mode.Equals(value, StringComparison.OrdinalIgnoreCase) Then Return
            Me._mode = value
            Call Me.ConfigurationChanged(Me, New ConfigurationChangedEventArgs)
        End Set
    End Property

    Public Sub New()
        Call InitializeComponent()

        Me.Panel = Me.ReoGridControl.CurrentWorksheet
        Me.Workbook = Me.ReoGridControl
        AddHandler Me.Workbook.PreviewKeyDown, AddressOf Me.WorkbookPreviewKeyDown
    End Sub

    ''' <summary>
    ''' 绑定Model，为Model绑定各种事件，实现视图和Model的数据同步
    ''' </summary>
    Protected Sub BindModel()
        AddHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
        AddHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
        AddHandler Me.Model.RowAdded, AddressOf Me.ModelRowAddedEvent
        AddHandler Me.Model.RowRemoved, AddressOf Me.ModelRowRemovedEvent
        AddHandler Me.Model.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
        AddHandler Me.Model.Refreshed, AddressOf Me.ModelRefreshedEvent
        AddHandler Me.Model.RowSynchronizationStateChanged, AddressOf Me.ModelRowSynchronizationStateChangedEvent

        Call Me.ImportData()
    End Sub

    ''' <summary>
    ''' 解绑Model，取消所有该视图绑定的事件
    ''' </summary>
    Protected Sub UnbindModel()
        RemoveHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
        RemoveHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
        RemoveHandler Me.Model.RowAdded, AddressOf Me.ModelRowAddedEvent
        RemoveHandler Me.Model.RowRemoved, AddressOf Me.ModelRowRemovedEvent
        RemoveHandler Me.Model.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
        RemoveHandler Me.Model.Refreshed, AddressOf Me.ModelRefreshedEvent
        RemoveHandler Me.Model.RowSynchronizationStateChanged, AddressOf Me.ModelRowSynchronizationStateChangedEvent
    End Sub

    Private Sub ConfigurationChanged(sender As Object, e As ConfigurationChangedEventArgs)
        Call Me.InitEditPanel()
        If Me.Model IsNot Nothing Then
            Call Me.ImportData()
        End If
    End Sub

    Private Sub ModelRowSynchronizationStateChangedEvent(sender As Object, e As ModelRowSynchronizationStateChangedEventArgs)
        Dim rows = (From r In e.SynchronizationStateUpdatedRows Select r.Index).ToArray
        Call Me.RefreshRowSynchronizationStates(rows)
    End Sub

    Private Sub ModelSelectionRangeChangedEvent(sender As Object, e As ModelSelectionRangeChangedEventArgs)
        Logger.Debug("==ReoGrid ModelSelectionRangeChanged " & Str(Me.GetHashCode))
        If Me.Model.RowCount = 0 Then
            Me.CurSyncMode = SyncMode.NOT_SYNC
            Call Me.ShowDefaultPage()
            Return
        End If
        If Me.CurSyncMode = SyncMode.NOT_SYNC Then
            Me.CurSyncMode = SyncMode.SYNC
            Call Me.ImportData()
            Return
        End If
        Call Me.RefreshSelectionRange()
    End Sub

    Private Sub ModelRefreshedEvent(sender As Object, e As ModelRefreshedEventArgs)
        Logger.Debug("==ReoGrid ModelRefreshedEvent")
        Call Me.InitEditPanel()
        Call Me.ImportData()
        Call Me.RefreshSelectionRange()
        Call Me.RefreshRowSynchronizationStates()
    End Sub

    Private Sub ModelRowUpdatedEvent(sender As Object, e As ModelRowUpdatedEventArgs)
        Logger.Debug("==ReoGrid ModelDataUpdatedEvent")
        If Me.CurSyncMode = SyncMode.NOT_SYNC Then
            Call Me.ImportData()
            Return
        End If
        Dim rows As Integer() = (From item In e.UpdatedRows Select item.Index).ToArray
        Me.ImportData(rows)
    End Sub

    Private Sub ModelRowAddedEvent(sender As Object, e As ModelRowAddedEventArgs)
        Dim oriRows As Integer() = (From item In e.AddedRows Select item.Index).ToArray
        If Me.CurSyncMode = SyncMode.NOT_SYNC Then
            Call Me.ImportData()
            Return
        End If
        Logger.Debug("Reogrid Added Rows: " & oriRows.ToString)
        '原始行每次插入之后，行号会变，所以做调整
        Dim realRowsASC = (From r In oriRows Order By r Ascending Select r).ToArray
        For i = 0 To realRowsASC.Length - 1
            realRowsASC(i) = realRowsASC(i) + i
        Next
        '将调整后的行分别插入表格中
        For i = 0 To realRowsASC.Length - 1
            Me.InsertRows(realRowsASC(i), 1)
        Next
        '刷新数据
        Call Me.ImportData(realRowsASC)
    End Sub

    Private Sub ModelCellUpdatedEvent(sender As Object, e As ModelCellUpdatedEventArgs)
        Logger.Debug("==ReoGrid ModelCellUpdatedEvent: " + Str(Me.GetHashCode))
        If Me.CurSyncMode = SyncMode.NOT_SYNC Then
            Call Me.ImportData()
            Return
        End If
        For Each posCell In e.UpdatedCells
            Me.ImportCell(posCell.Row, posCell.ColumnName)
        Next
    End Sub

    Private Sub ModelRowRemovedEvent(sender As Object, e As ModelRowRemovedEventArgs)
        If Me.Model.RowCount = 0 Then
            Me.CurSyncMode = SyncMode.NOT_SYNC
            Call Me.ShowDefaultPage()
            Return
        End If
        If Me.CurSyncMode = SyncMode.NOT_SYNC Then
            Call Me.ImportData()
            Return
        End If

        '因为每次删除行会导致行号改变，所以倒序删除
        Dim rowDESC = (From indexRow In e.RemovedRows
                       Order By indexRow.Index Descending
                       Select indexRow.Index).ToArray

        For Each curRow In rowDESC
            If Me.Panel.Rows > curRow Then
                Me.DeleteRows(curRow, 1)
            End If
        Next
    End Sub

    '删除行，自动移动dicCellState和dicCellEdited
    Private Sub DeleteRows(row As Integer, count As Integer)
        Dim newDicCellEdited As New Dictionary(Of CellPosition, Boolean)
        For Each cellPos In Me.dicCellEdited.Keys
            If cellPos.Row >= row And cellPos.Row <= row + count - 1 Then
                '删除行，不加入newDicCelledited
            ElseIf cellPos.Row > row Then
                cellPos.Row -= count
                newDicCellEdited.Add(cellPos, True)
            Else
                newDicCellEdited.Add(cellPos, True)
            End If
        Next
        Me.dicCellEdited = newDicCellEdited

        Dim newDicCellState As New Dictionary(Of Integer, Dictionary(Of Integer, Long))
        For Each entry In Me.dicCellState
            If entry.Key >= row And entry.Key <= row + count - 1 Then
                '删掉
            ElseIf entry.Key > row Then
                newDicCellState.Add(entry.Key - count, entry.Value)
            Else
                newDicCellState.Add(entry.Key, entry.Value)
            End If
        Next
        Me.dicCellState = newDicCellState
        '去掉选区变化事件，防止删除行时触发选区变化事件，造成无用刷新和警告
        RemoveHandler Me.Panel.BeforeSelectionRangeChange, AddressOf Me.BeforeSelectionRangeChange
        Me.Panel.DeleteRows(row, count)
        AddHandler Me.Panel.BeforeSelectionRangeChange, AddressOf Me.BeforeSelectionRangeChange
        Call Me.PaintRows(Util.Range(row, Me.Panel.RowCount))
    End Sub

    Private Sub InsertRows(row As Integer, count As Integer)
        Dim newDicCellEdited As New Dictionary(Of CellPosition, Boolean)
        For Each cellPos In Me.dicCellEdited.Keys
            If cellPos.Row >= row Then
                cellPos.Row += count
                newDicCellEdited.Add(cellPos, True)
            Else
                newDicCellEdited.Add(cellPos, True)
            End If
        Next
        Me.dicCellEdited = newDicCellEdited

        Dim newDicCellState As New Dictionary(Of Integer, Dictionary(Of Integer, Long))
        For Each entry In Me.dicCellState
            If entry.Key >= row Then
                newDicCellState.Add(entry.Key + count, entry.Value)
            Else
                newDicCellState.Add(entry.Key, entry.Value)
            End If
        Next
        Me.dicCellState = newDicCellState
        '去掉选区变化事件，防止插入行时触发选区变化事件，造成无用刷新和警告
        RemoveHandler Me.Panel.BeforeSelectionRangeChange, AddressOf Me.BeforeSelectionRangeChange
        Call Me.Panel.InsertRows(row, count)
        AddHandler Me.Panel.BeforeSelectionRangeChange, AddressOf Me.BeforeSelectionRangeChange
        Call Me.PaintRows(Util.Range(row, Me.Panel.RowCount))
    End Sub

    ''' <summary>
    ''' 根据各个单元格的状态，按行绘制单元格的颜色
    ''' </summary>
    Private Sub PaintRows(Optional rows As Integer() = Nothing)
        If rows Is Nothing Then
            rows = Util.Range(0, Me.Panel.Rows)
        End If
        For Each row In rows
            For Each col In Util.Range(0, Me.Panel.Columns)
                Dim cellState = Me.GetCellState(row, col)
                If (cellState And CellState.INVALID_DATA) > 0 Then
                    Me.Panel.SetRangeStyles(row, col, 1, 1, New WorksheetRangeStyle() With {
                        .Flag = PlainStyleFlag.BackColor,
                        .BackColor = Color.IndianRed
                    })
                    Me.Panel.SetRangeBorders(row, col, 1, 1, BorderPositions.All, RangeBorderStyle.SilverSolid)
                ElseIf (cellState And CellState.UNSYNCHRONIZED) Then
                    Me.Panel.SetRangeBorders(row, col, 1, 1, BorderPositions.All, RangeBorderStyle.SilverSolid)
                    Me.Panel.SetRangeStyles(row, col, 1, 1, New WorksheetRangeStyle() With {
                        .Flag = PlainStyleFlag.BackColor,
                        .BackColor = Me.COLOR_UNSYNCHRONIZED
                    })
                    Me.Panel.SetRangeBorders(row, col, 1, 1, BorderPositions.All, RangeBorderStyle.SilverSolid)
                Else
                    Me.Panel.SetRangeBorders(row, col, 1, 1, BorderPositions.All, RangeBorderStyle.SilverSolid)
                    Me.Panel.SetRangeStyles(row, col, 1, 1, New WorksheetRangeStyle() With {
                        .Flag = PlainStyleFlag.BackColor,
                        .BackColor = Color.Transparent
                    })
                    Me.Panel.SetRangeBorders(row, col, 1, 1, BorderPositions.All, RangeBorderStyle.Empty)
                End If
            Next
        Next
    End Sub

    ''' <summary>
    ''' 从Model同步选区
    ''' </summary>
    Protected Sub RefreshSelectionRange()
        Logger.SetMode(LogMode.REFRESH_VIEW)
        If Me.CurSyncMode = SyncMode.NOT_SYNC Then Return
        If Me.Model.AllSelectionRanges.Length <= 0 Then
            Me.Panel.SelectionRange = RangePosition.Empty
            Return
        End If
        If Me.Model.AllSelectionRanges.Length > 1 Then
            Logger.PutMessage("Multiple range selected, ReoGridView will only show range of the first one", LogLevel.WARNING)
        End If
        Dim range = Me.Model.AllSelectionRanges(0)
        RemoveHandler Me.Panel.BeforeSelectionRangeChange, AddressOf Me.BeforeSelectionRangeChange
        Me.Panel.SelectionRange = New RangePosition(range.Row, range.Column, range.Rows, range.Columns)
        AddHandler Me.Panel.BeforeSelectionRangeChange, AddressOf Me.BeforeSelectionRangeChange
    End Sub

    ''' <summary>
    ''' 从Model同步各行同步状态
    ''' </summary>
    ''' <param name="rows"></param>
    Protected Sub RefreshRowSynchronizationStates(Optional rows As Integer() = Nothing)
        If Me.CurSyncMode = SyncMode.NOT_SYNC Then Return
        Logger.SetMode(LogMode.REFRESH_VIEW)
        If rows Is Nothing Then
            rows = Me.Range(0, Me.Model.RowCount)
        End If
        For Each row In rows
            Dim state = Me.Model.GetRowSynchronizationState(row)
            If row >= Me.Panel.RowCount Then
                Logger.PutMessage($"Row number {row} exceeded max row in the ReoGridView")
                Return
            End If
            If state <> SynchronizationState.SYNCHRONIZED Then
                Call Me.AddCellState(row, CellState.UNSYNCHRONIZED)
            Else
                Call Me.RemoveCellState(row, CellState.UNSYNCHRONIZED)
            End If
        Next
        Call Me.PaintRows(rows)
    End Sub

    ''' <summary>
    ''' 显示默认页面
    ''' </summary>
    Protected Sub ShowDefaultPage()
        Me.CurSyncMode = SyncMode.NOT_SYNC
        Call Me.Panel.DeleteRangeData(RangePosition.EntireRange)
        Me.ReoGridControl.Enabled = False
        '设定表格初始有10行，非同步模式
        Me.Panel.Rows = 10
        '设定提示文本
        Me.Panel.Item(0, 0) = "暂无数据"
        Call Me.PaintRows()
    End Sub

    ''' <summary>
    ''' 初始化视图（允许重复调用）
    ''' </summary>
    Protected Sub InitEditPanel()
        Logger.SetMode(LogMode.INIT_VIEW)
        Call Me.Panel.Reset()
        If Me.Configuration Is Nothing Then
            Logger.PutMessage("Configuration not set!")
            Return
        End If
        Dim fieldConfiguration As FieldConfiguration() = Me.Configuration.GetFieldConfigurations(Me.Mode)
        If fieldConfiguration Is Nothing Then
            Logger.PutMessage("Configuration of not found!")
            Return
        End If

        '创建联想窗口
        If Not Me.DesignMode AndAlso Me.textBox Is Nothing Then
            Me.Panel.StartEdit()
            Me.Panel.EndEdit(EndEditReason.NormalFinish)
            For Each control As Control In Me.ReoGridControl.Controls
                If TypeOf (control) Is TextBox AndAlso control.Name = "" Then
                    Me.textBox = control
                    Exit For
                End If
            Next
            Me.formAssociation = New FormAssociation
            Me.formAssociation.StayUnvisible = True
            If Me.textBox Is Nothing Then
                Logger.SetMode(LogMode.INIT_VIEW)
                Logger.PutMessage("Table textbox not found")
            End If
            RemoveHandler Me.textBox.PreviewKeyDown, AddressOf Me.TextboxPreviewKeyDown
            AddHandler Me.textBox.PreviewKeyDown, AddressOf Me.TextboxPreviewKeyDown
            RemoveHandler Me.Panel.CellMouseDown, AddressOf Me.CellMouseDown
            AddHandler Me.Panel.CellMouseDown, AddressOf Me.CellMouseDown
        End If

        '禁止自动判断单元格格式
        Me.Panel.SetSettings(WorksheetSettings.Edit_AutoFormatCell, False)
        '清空列Name和列号的对应关系
        Call Me.dicNameColumn.Clear()
        '清空状态
        Call Me.dicCellEdited.Clear()
        Call Me.dicCellState.Clear()
        '清空事件
        Call Me.dicBeforeSelectionRangeChangeEvent.Clear()
        Call Me.dicCellDataChangedEvent.Clear()
        Call Me.dicTextChangedEvent.Clear()
        Dim curColumn = 0
        '遍历FieldConfiguration()
        For i = 0 To fieldConfiguration.Length - 1
            Dim curField = fieldConfiguration(i)
            '如果字段不可视，直接跳过
            If curField.Visible = False Then Continue For
            '否则开始初始化表头
            Me.dicNameColumn.Add(curField.Name, curColumn)
            Me.Panel.ColumnHeaders.Item(curColumn).Text = curField.DisplayName
            If Not Me.DesignMode Then
                '给字段注册事件
                '内容改变事件
                If curField.ContentChanged IsNot Nothing Then
                    If curField.Values Is Nothing Then '如果是文本框，同时绑定到文本改变事件和单元格内容变化事件
                        Me.dicTextChangedEvent.Add(curColumn, curField.ContentChanged)
                        Me.dicCellDataChangedEvent.Add(curColumn, curField.ContentChanged)
                    Else '否则是ComboBox，仅绑定到CellDataChanged事件
                        Me.dicCellDataChangedEvent.Add(curColumn, curField.ContentChanged)
                    End If
                End If
                '编辑完成事件
                If curField.EditEnded IsNot Nothing Then
                    Me.dicBeforeSelectionRangeChangeEvent.Add(curColumn, curField.EditEnded)
                End If
            End If
            curColumn += 1
        Next
        '设定表头列数
        Me.Panel.Columns = curColumn

        '给worksheet添加事件
        '换行初始化行，绑定JS变量
        If Not Me.DesignMode Then
            RemoveHandler Me.Panel.BeforeSelectionRangeChange, AddressOf BeforeSelectionRangeChange
            AddHandler Me.Panel.BeforeSelectionRangeChange, AddressOf BeforeSelectionRangeChange
            '编辑事件
            RemoveHandler Me.Panel.CellDataChanged, AddressOf Me.CellDataChanged
            AddHandler Me.Panel.CellDataChanged, AddressOf Me.CellDataChanged
            RemoveHandler Me.textBox.TextChanged, AddressOf Me.textBoxTextChanged
            AddHandler Me.textBox.TextChanged, AddressOf Me.textBoxTextChanged
            RemoveHandler Me.textBox.Leave, AddressOf Me.textBoxLeave
            AddHandler Me.textBox.Leave, AddressOf Me.textBoxLeave

            Call Me.InitRow(Me.Panel.SelectionRange.Row)
            Call Me.BindRowToJsEngine(Me.Panel.SelectionRange.Row)
            Call Me.BindAssociation(0) '最开始默认0,0的时候，不会触发选取更改。所以手动绑定一下单元格联想
        End If
    End Sub

    Private Sub textBoxLeave(sender As Object, e As EventArgs)
        Me.Workbook.Focus()
    End Sub

    Private Sub AutoFitColumnWidth(col As Integer)
        Call Me.Panel.AutoFitColumnWidth(col)
        Dim columnHeader = Me.Panel.ColumnHeaders.Item(col)
        Dim columnHeaderText = columnHeader.Text
        Dim width
        If columnHeaderText Is Nothing Then
            width = 20
        Else
            width = 20 + columnHeaderText.Sum(
            Function(c)
                If Char.IsLetterOrDigit(c) OrElse
                    Char.IsWhiteSpace(c) OrElse
                    Char.IsSymbol(c) Then
                    Return 8
                Else
                    Return 16
                End If
            End Function)
        End If
        Me.Panel.ColumnHeaders(col).Width = System.Math.Max(width, Me.Panel.ColumnHeaders.Item(col).Width + 10)
    End Sub

    ''' <summary>
    ''' 初始化一行的下拉框，只读等
    ''' </summary>
    ''' <param name="row">行号</param>
    Private Sub InitRow(row As Integer)
        Logger.SetMode(LogMode.INIT_VIEW)
        If Me.CurSyncMode = SyncMode.NOT_SYNC Then Return
        Dim fieldConfiguration As FieldConfiguration() = Me.Configuration.GetFieldConfigurations(Me.Mode)
        If fieldConfiguration Is Nothing Then
            Logger.PutMessage("Configuration of mode not found!")
            Return
        End If

        Dim worksheet = Me.Panel
        RemoveHandler worksheet.CellDataChanged, AddressOf Me.CellDataChanged
        '遍历FieldConfiguration()
        For i = 0 To fieldConfiguration.Length - 1
            Dim curField = fieldConfiguration(i)
            If Not curField.Visible Then Continue For
            Dim col = Me.dicNameColumn(curField.Name)
            '如果字段不可视，直接跳过
            If curField.Visible = False Then Continue For
            '否则开始初始化当前格
            Dim curCell = worksheet.CreateAndGetCell(row, col)
            '如果设定了Values，则执行Values获取值
            If curField.Values IsNot Nothing Then
                Dim comboBox = New DropdownListCell(CType(curField.Values.Invoke(Me), IEnumerable(Of Object)))
                AddHandler comboBox.DropdownOpened, Sub()
                                                        worksheet.SelectionRange = New RangePosition(row, col, 1, 1)
                                                    End Sub
                worksheet(row, col) = comboBox
            End If

            If curField.Editable = False Then
                curCell.IsReadOnly = True
            End If
        Next
        AddHandler worksheet.CellDataChanged, AddressOf Me.CellDataChanged
        RowInited.Add(row)
    End Sub

    Private Sub RowAddedEvent(rangePosition As RangePosition)
        If Me.CurSyncMode = SyncMode.NOT_SYNC Then Return
        Call Me.ExportRows(rangePosition)
    End Sub

    Private Sub RowUpdatedEvent(rangePosition As RangePosition)
        If Me.CurSyncMode = SyncMode.NOT_SYNC Then Return
        Call Me.ExportRows(rangePosition)
    End Sub

    Private Sub CellUpdatedEvent(rangePosition As RangePosition)
        If Me.CurSyncMode = SyncMode.NOT_SYNC Then Return
        Call Me.ExportCells(rangePosition)
    End Sub

    Private Sub CellMouseDown(sender As Object, e As EventArgs)
        Me.canChangeSelectionRange = True
        Me.formAssociation.StayVisible = False
    End Sub

    Private Sub TextboxPreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs)
        If Me.formAssociation.TextBox Is Nothing Then
            Me.formAssociation.TextBox = Me.textBox
        End If
        If (e.KeyCode = Keys.Up OrElse e.KeyCode = Keys.Down) AndAlso Me.formAssociation IsNot Nothing AndAlso Me.formAssociation.Visible = True Then
            Me.canChangeSelectionRange = False
            Me.formAssociation.StayVisible = True
            Dim threadRestartEdit = New Thread(
                Sub()
                    Call Thread.Sleep(10)
                    Call Me.Workbook.Invoke(
                    Sub()
                        Call Me.Panel.StartEdit()
                    End Sub)
                End Sub)
            Call threadRestartEdit.Start()
        ElseIf e.KeyCode = Keys.Enter And Me.formAssociation.Selected Then
            Me.canChangeSelectionRange = False
            Dim threadEnableChangeSelectionRange = New Thread(
                Sub()
                    Call Thread.Sleep(100)
                    Me.canChangeSelectionRange = True
                End Sub)
            Call threadEnableChangeSelectionRange.Start()
        Else
            Me.canChangeSelectionRange = True
            Me.formAssociation.StayVisible = False
        End If
    End Sub

    '选择行改变时初始化新的行，只初始化选区首行
    Private Sub BeforeSelectionRangeChange(sender As Object, e As BeforeSelectionChangeEventArgs)
        If Me.CurSyncMode = SyncMode.NOT_SYNC Then Return
        If Me.canChangeSelectionRange = False Then
            e.IsCancelled = True
            Return
        End If
        Dim worksheet = Me.Panel
        Dim row = Me.Panel.SelectionRange.Row
        Dim rows = Me.Panel.SelectionRange.Rows
        Dim col = Me.Panel.SelectionRange.Col
        Dim cols = Me.Panel.SelectionRange.Cols

        Dim modelRowCount = Me.Model.RowCount
        Dim newRow = System.Math.Min(e.StartRow, e.EndRow)
        Dim newCol = System.Math.Min(e.StartCol, e.EndCol)
        Dim newRows = System.Math.Min(System.Math.Max(e.StartRow, e.EndRow) - newRow + 1, modelRowCount) '选择整列的话行数会超出最大行数，可能是ReoGrid的bug
        Dim newCols = System.Math.Max(e.StartCol, e.EndCol) - newCol + 1

        '隐藏联想
        Call Me.formAssociation.Hide()

        '复制一份已编辑单元格，供后面调用编辑完成事件使用
        Dim editedCells = Me.dicCellEdited.Keys.ToArray

        '首先更新数据
        '判断是单元格更新还是整行更新
        Dim selectionRange = Me.Panel.SelectionRange
        If selectionRange.IsSingleCell Then
            RemoveHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
            Call Me.CellUpdatedEvent(selectionRange)
            AddHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
        Else
            RemoveHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
            Call Me.RowUpdatedEvent(selectionRange)
            AddHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
        End If
        Call Me.PaintRows(Util.Range(selectionRange.Row, selectionRange.Row + selectionRange.Rows))

        If Me.copied Then
            '如果新选区起始单元格和复制起始单元格相同，说明是复制引起的选取变化
            If New CellPosition(newRow, newCol).Equals(Me.copyStartCell) Then
                '对于复制的选区，全部作为已修改状态
                For curRow = newRow To newRow + newRows - 1
                    For curCol = newCol To newCol + newCols - 1
                        Me.dicCellEdited.Add(New CellPosition(curRow, curCol), True)
                    Next
                Next
                Dim copyRange = New RangePosition(newRow, newCol, newRows, newCols)
                '同步复制选区的数据
                RemoveHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
                Call Me.RowUpdatedEvent(copyRange)
                AddHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
                '重新绘制复制选区的颜色。复制时会覆盖掉原来的颜色
                Call Me.PaintRows(Util.Range(copyRange.Row, copyRange.Row + copyRange.Rows))
                Me.copied = False
            Else '否则说明复制只复制了一个单元格，没有触发选取变化，而是下一次选取变化触发。此时更新复制的单元格
                Me.copied = False
            End If
        End If

        '同步Model的选区
        RemoveHandler Me.Model.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
        Me.Model.AllSelectionRanges = New Range() {New Range(newRow, newCol, newRows, newCols)}
        AddHandler Me.Model.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent

        '初始化新的选中行。如果选区首行没变，就不重新初始化行了
        If Not newRow = Me.Panel.SelectionRange.Row Then
            If Not RowInited.Contains(newRow) Then
                Me.InitRow(newRow)
            End If
            Me.BindRowToJsEngine(newRow)
        End If

        Me.BindAssociation(newCol)

        '触发编辑完成事件
        For curRow = row To row + rows - 1
            For curCol = col To col + cols - 1
                '如果该列有编辑完成事件
                If Me.dicBeforeSelectionRangeChangeEvent.ContainsKey(curCol) Then
                    '如果该列没被编辑，则继续下一列
                    If Not editedCells.Contains(New CellPosition(curRow, curCol)) Then Continue For
                    Dim data = If(Me.Panel(curRow, curCol), "")
                    Me.dicBeforeSelectionRangeChangeEvent(curCol)?.Invoke(Me, curRow, data)
                End If
            Next
        Next
    End Sub

    Private Sub BindAssociation(col As Integer)
        If Me.Panel.SelectionRange.IsSingleCell AndAlso Me.dicNameColumn.Count > 0 Then
            Dim newColName = (From mc In Me.dicNameColumn Where mc.Value = col Select mc.Key).First
            Dim modeConfiguration = Me.Configuration.GetFieldConfigurations(Me.Mode)
            Dim curField = (From m In modeConfiguration Where m.Name = newColName Select m).First
            If curField.Association Is Nothing Then
                formAssociation.SetAssociationFunc(Nothing)
            Else
                formAssociation.SetAssociationFunc(Function(str As String)
                                                       Dim ret = curField.Association.Invoke(Me, str)
                                                       Return Util.ToArray(Of AssociationItem)(ret)
                                                   End Function)
            End If
        End If
    End Sub

    Private Sub textBoxTextChanged(sender As Object, e As EventArgs)
        Static switcher = True '事件开关，False为关，True为开
        If switcher = False Then Return '开关关掉则直接返回
        Logger.Debug("ReoGrid View textChanged: " & Str(Me.GetHashCode))
        Dim worksheet = Me.Panel
        Dim row = Me.Panel.SelectionRange.Row
        Dim col = Me.Panel.SelectionRange.Col
        If Not Me.RowInited.Contains(row) Then '如果本行未被初始化，不要触发事件
            Return
        End If
        '在CellDataChanged事件里记录单元格被编辑
        ''记录单元格被编辑
        'If Not Me.dicCellEdited.ContainsKey(New CellPosition(row, col)) Then
        '    Me.dicCellEdited.Add(New CellPosition(row, col), True)
        'End If
        '要是这个列没设置TextChanged事件，就不用刷了
        If Not Me.dicTextChangedEvent.ContainsKey(col) Then Exit Sub
        '否则执行设置的事件
        switcher = False
        worksheet.EditingCell.Data = Me.textBox.Text
        switcher = True
        Dim fieldMethod = Me.dicTextChangedEvent(col)
        fieldMethod.Invoke(Me, Me.textBox.Text, worksheet.EditingCell.Row)
    End Sub

    Private Sub CellDataChanged(sender As Object, e As CellEventArgs)
        Logger.Debug("ReoGrid View CellDataChanged: " & Str(Me.GetHashCode))
        If Me.CurSyncMode = SyncMode.NOT_SYNC Then Return
        Dim worksheet = Me.Panel
        Dim row = e.Cell.Row
        Dim col = e.Cell.Column
        Dim fieldName = (From nameCol In Me.dicNameColumn Where nameCol.Value = col Select nameCol.Key).First
        Dim fieldConfig = (From config In Me.Configuration.GetFieldConfigurations(Me.Mode) Where config.Name = fieldName Select config).First

        If Not Me.RowInited.Contains(row) Then '如果本行未被初始化，不要触发事件
            Return
        End If
        '===========系统事件写这里
        If Me.CurSyncMode = SyncMode.SYNC Then
            If Not Me.dicCellEdited.ContainsKey(e.Cell.Position) Then
                Me.dicCellEdited.Add(e.Cell.Position, True)
                'TODO 注释掉了，看看有没有bug
                ''只要数据有修改，直接将Model对应行的同步状态改为未同步
                'Me.Model.UpdateRowSynchronizationState(e.Cell.Row, SynchronizationState.UNSYNCHRONIZED)
            End If
        End If
        '如果当前格是下拉框，验证数据是否在下拉框可选项范围中，如果不在，则标红
        '同时，下拉框数据改变，直接同步到Model
        If fieldConfig.Values IsNot Nothing Then
            '同步数据
            RemoveHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
            Call Me.CellUpdatedEvent(e.Cell.PositionAsRange)
            AddHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
            '验证数据
            Call Me.ValidateComboBoxData(row, col)
        End If
        Call Me.PaintRows({row})

        '===========用户事件写这里
        '要是这个列没设置ContentChanged事件，就不用刷了
        If Not Me.dicCellDataChangedEvent.ContainsKey(col) Then Exit Sub
        '否则执行设置的事件
        'If Me.textBox.Visible = True Then
        '    Me.eventSwitcher = False
        '    worksheet.EditingCell.Data = Me.textBox.Text
        '    Me.eventSwitcher = True
        'End If
        Dim fieldMethod = Me.dicCellDataChangedEvent(col)
        fieldMethod.Invoke(Me, Me.Panel(row, col)?.ToString, row)
    End Sub

    ''' <summary>
    ''' 验证下拉框数据，不正确时将单元格状态增加CellState.INVALID_DATA，正确时去除CellState.INVALID_DATA
    ''' </summary>
    ''' <param name="row">行</param>
    ''' <param name="col">列</param>
    Private Sub ValidateComboBoxData(row As Integer, col As Integer)
        Dim fieldName = (From nameCol In Me.dicNameColumn Where nameCol.Value = col Select nameCol.Key).First
        Dim fieldConfig = (From config In Me.Configuration.GetFieldConfigurations(Me.Mode) Where config.Name = fieldName Select config).First
        '验证数据
        Dim values As Object() = Util.ToArray(Of String)(fieldConfig.Values.Invoke(Me))
        If Not values.Contains(Me.Panel(row, col)) Then
            Call Me.AddCellState(row, col, CellState.INVALID_DATA)
        Else
            Call Me.RemoveCellState(row, col, CellState.INVALID_DATA)
        End If
    End Sub


    Private Sub BindRowToJsEngine(row As Integer)
        Dim jsEngine = Me.JsEngine
        Dim worksheet = Me.Panel
        Dim viewObj = jsEngine.Execute("view = {}").GetValue("view").AsObject
        Dim fieldConfiguration = Me.Configuration.GetFieldConfigurations(Me.Mode)
        jsEngine.SetValue("DropdownListCellItemsToArray", New Func(Of DropdownListCell.DropdownItemsCollection, Object())(AddressOf Me.DropdownListCellItemsToArray))
        Dim col = -1
        For i = 0 To fieldConfiguration.Length - 1
            Try
                Dim curField = fieldConfiguration(i)
                If Not curField.Visible Then Continue For
                col += 1
                If curField.Values IsNot Nothing Then '如果设置了Values，则是下拉框
                    viewObj.Put(curField.Name, JsValue.FromObject(jsEngine, worksheet.CreateAndGetCell(row, col)), True)
                    Dim tmp = String.Format(
                        <string>
                             {0} = undefined;
                             Object.defineProperty(
                                this,
                                "{0}",
                                {{get: function(){{
                                    return view.{0}.Data
                                }},
                                set: function(val){{
                                    if(val == undefined) return;
                                    var itemArray = DropdownListCellItemsToArray(view.{0}.Body.Items)
                                    for(var i=0;i &lt; itemArray.length;i++){{
                                        if(itemArray[i] == val){{
                                            view.{0}.Data = val;
                                            return;
                                        }}
                                    }}
                                }} }}
                            )
                         </string>.Value, curField.Name)
                    jsEngine.Execute(tmp)
                Else '否则是普通的文本格
                    viewObj.Put(curField.Name, JsValue.FromObject(jsEngine, worksheet.CreateAndGetCell(row, col)), True)
                    Dim tmp = String.Format(
                        <string>
                             {0} = undefined
                             Object.defineProperty(
                                this,
                                "{0}",
                                {{get: function(){{
                                    return view.{0}.Data
                                }},
                                set: function(val){{
                                    if(val == undefined) return;
                                    view.{0}.Data = val
                                }} }}
                            )
                         </string>.Value, curField.Name)
                    jsEngine.Execute(tmp)
                End If
            Catch ex As Exception
                Logger.SetMode(LogMode.INIT_VIEW)
                Logger.PutMessage(ex.Message)
            End Try
        Next
    End Sub

    ''' <summary>
    ''' 从Model导入单元格数据
    ''' </summary>
    ''' <param name="row">要导入的行</param>
    ''' <param name="colName">要导入的列名</param>
    ''' <returns>是否导入成功</returns>
    Protected Function ImportCell(row As Integer, colName As String) As Boolean
        Logger.Debug("==ReoGrid ImportCell: " + Str(Me.GetHashCode))
        Logger.SetMode(LogMode.REFRESH_VIEW)
        If Me.Model.RowCount = 0 Then
            Me.CurSyncMode = SyncMode.NOT_SYNC
            Call Me.ShowDefaultPage()
            Return True
        ElseIf Me.CurSyncMode = SyncMode.NOT_SYNC Then
            Me.Panel.Rows = 1
            Me.CurSyncMode = SyncMode.SYNC
        End If

        If Me.Configuration Is Nothing Then
            Logger.PutMessage("Configuration is not setted")
            Return False
        End If
        If Me.Panel Is Nothing Then
            Logger.PutMessage("Panel is not setted")
            Return False
        End If
        '获取当前的Configuration
        Dim fieldConfigurations = Me.Configuration.GetFieldConfigurations(Me.Mode)
        If fieldConfigurations Is Nothing Then
            Logger.PutMessage("Configuration not found!")
            Return False
        End If
        Dim field = (From f In fieldConfigurations Where f.Name.Equals(colName, StringComparison.OrdinalIgnoreCase) Select f).FirstOrDefault
        If field.Visible = False Then Return True
        '传入数据
        '否则开始导入值
        '先计算值，过一遍Mapper
        Dim value = Me.Model(row, colName)
        Dim text As String
        If Not field.ForwardMapper Is Nothing Then
            text = field.ForwardMapper.Invoke(value, row, Me)
        Else
            text = If(value?.ToString, "")
        End If
        Logger.SetMode(LogMode.REFRESH_VIEW)
        '然后获取单元格

        If Not Me.dicNameColumn.ContainsKey(colName) Then
            Throw New FrontWorkException($"{Me.Name} doesn't contains field:""{colName}""")
        End If
        Dim reoGridColumnNum = Me.dicNameColumn(colName)
        Dim reoGridCell = Panel.GetCell(row, reoGridColumnNum)
        If reoGridCell Is Nothing Then
            If String.IsNullOrEmpty(text) Then
                Return True
            Else
                reoGridCell = Panel.CreateAndGetCell(row, reoGridColumnNum)
            End If
        End If
        '根据Configuration中的Field类型，处理View中的单元格
        If field.Values Is Nothing Then '没有Values，是文本框
            RemoveHandler Me.Panel.CellDataChanged, AddressOf CellDataChanged
            reoGridCell.Data = text
            AddHandler Me.Panel.CellDataChanged, AddressOf CellDataChanged
        Else '有Values，是ComboBox框
            Dim values = Util.ToArray(Of String)(field.Values.Invoke(Me))
            If values.Contains(text) = False Then
                Logger.PutMessage("Value """ + text + """" + " not found in comboBox """ + field.Name + """")
            End If
            RemoveHandler Me.Panel.CellDataChanged, AddressOf CellDataChanged
            reoGridCell.Data = text
            AddHandler Me.Panel.CellDataChanged, AddressOf CellDataChanged
            Call Me.ValidateComboBoxData(reoGridCell.Row, reoGridCell.Column)
        End If

        Call Me.PaintRows({row})
        Me.AutoFitColumnWidth(Me.dicNameColumn(colName))
        Return True
    End Function

    ''' <summary>
    ''' 从Model导入数据
    ''' </summary>
    ''' <param name="rows">要导入的行</param>
    ''' <returns>是否导入成功</returns>
    Protected Function ImportData(Optional rows As Integer() = Nothing) As Boolean
        Logger.Debug("==ReoGrid ImportData: " + Str(Me.GetHashCode))
        Logger.SetMode(LogMode.REFRESH_VIEW)
        If Me.Model.RowCount = 0 Then
            Me.CurSyncMode = SyncMode.NOT_SYNC
            Call Me.ShowDefaultPage()
            Return True
        ElseIf Me.CurSyncMode = SyncMode.NOT_SYNC Then
            Me.Panel.Rows = 1
            Me.CurSyncMode = SyncMode.SYNC
        End If

        If Me.Configuration Is Nothing Then
            Logger.PutMessage("Configuration is not setted")
            Return False
        End If
        If Me.Panel Is Nothing Then
            Logger.PutMessage("Panel is not setted")
            Return False
        End If
        '获取当前的Configuration
        Dim fieldConfiguration = Me.Configuration.GetFieldConfigurations(Me.Mode)
        If fieldConfiguration Is Nothing Then
            Logger.PutMessage("Configuration not found!")
            Return False
        End If
        Dim dataTable = Me.Model.ToDataTable
        '清空ReoGrid相应行
        If rows Is Nothing Then
            Me.Panel.DeleteRangeData(RangePosition.EntireRange)
            Me.Panel.Rows = dataTable.Rows.Count
            Me.RowInited.Clear()
        Else
            Me.ClearRows(rows)
        End If
        '遍历传入数据
        For Each curDataRowNum In If(rows Is Nothing, Me.Range(dataTable.Rows.Count), rows)
            Dim curDataRow = dataTable.Rows(curDataRowNum)
            Dim curReoGridRowNum = curDataRowNum
            Me.InitRow(curReoGridRowNum)
            '遍历列（Configuration)
            For Each curField In fieldConfiguration
                Dim curDataColumn As DataColumn = (From c As DataColumn In dataTable.Columns
                                                   Where c.ColumnName.Equals(curField.Name, StringComparison.OrdinalIgnoreCase)
                                                   Select c).FirstOrDefault
                '在对象中找不到Configuration描述的字段，直接报错，并接着下一个字段
                If curDataColumn Is Nothing Then
                    Logger.PutMessage("Field """ + curField.Name + """ not found in model")
                    Continue For
                End If
                '否则开始Push值
                '先计算值，过一遍Mapper
                Dim value = curDataRow(curDataColumn)
                Dim text As String
                If Not curField.ForwardMapper Is Nothing Then
                    text = curField.ForwardMapper.Invoke(value, curDataRowNum, Me)
                Else
                    text = If(value?.ToString, "")
                End If

                If String.IsNullOrEmpty(text) Then Continue For '如果推的内容是空白，就不显示在格里了，节约创建单元格的内存空间
                Logger.SetMode(LogMode.REFRESH_VIEW)
                '然后获取单元格
                If Me.dicNameColumn.ContainsKey(curField.Name) = False Then
                    'Logger.PutMessage("Field """ & curField.Name & """ not found in view")
                    Continue For
                End If
                Dim curReoGridColumnNum = Me.dicNameColumn(curField.Name)
                Dim curReoGridCell = Panel.CreateAndGetCell(curReoGridRowNum, curReoGridColumnNum)
                '根据Configuration中的Field类型，处理View中的单元格
                If curField.Values Is Nothing Then '没有Values，是文本框
                    RemoveHandler Me.Panel.CellDataChanged, AddressOf CellDataChanged
                    curReoGridCell.Data = text
                    AddHandler Me.Panel.CellDataChanged, AddressOf CellDataChanged
                Else '有Values，是ComboBox框
                    Dim values = Util.ToArray(Of String)(curField.Values.Invoke(Me))
                    If values.Contains(text) = False Then
                        Logger.PutMessage("Value """ + text + """" + " not found in comboBox """ + curField.Name + """")
                    End If
                    RemoveHandler Me.Panel.CellDataChanged, AddressOf CellDataChanged
                    curReoGridCell.Data = text
                    AddHandler Me.Panel.CellDataChanged, AddressOf CellDataChanged
                    Call Me.ValidateComboBoxData(curReoGridCell.Row, curReoGridCell.Column)
                End If
            Next
            Call Me.PaintRows({curReoGridRowNum})
        Next
        For col = 0 To Me.Panel.Columns - 1
            Me.AutoFitColumnWidth(col)
        Next
        Return True
    End Function

    ''' <summary>
    ''' 创建从0到n的数组，包含0不包含length
    ''' </summary>
    ''' <param name="n"></param>
    ''' <returns></returns>
    Private Function Range(n As Integer) As Integer()
        Return Me.Range(0, n)
    End Function

    ''' <summary>
    ''' 创建从start到end的数组，包含start不包含end
    ''' </summary>
    ''' <param name="start"></param>
    ''' <param name="end"></param>
    ''' <returns></returns>
    Private Function Range(start As Integer, [end] As Integer) As Integer()
        Dim array([end] - start - 1) As Integer
        For i = 0 To array.Length - 1
            array(i) = start + i
        Next
        Return array
    End Function

    ''' <summary>
    ''' 清空行
    ''' </summary>
    ''' <param name="rows">要清空的行</param>
    Protected Sub ClearRows(rows As Integer())
        RemoveHandler Me.Panel.CellDataChanged, AddressOf Me.CellDataChanged
        For Each row In rows
            For col = 0 To Me.Panel.Columns - 1
                Dim cell = Me.Panel.GetCell(row, col)
                If cell Is Nothing Then Continue For
                cell.Data = Nothing
            Next
        Next
        AddHandler Me.Panel.CellDataChanged, AddressOf Me.CellDataChanged
    End Sub

    ''' <summary>
    ''' 导出选中行的数据到Model
    ''' </summary>
    Protected Sub ExportRows(rangePosition As RangePosition)
        Logger.Debug("==ReoGrid ExportData")
        Logger.SetMode(LogMode.SYNC_FROM_VIEW)
        If Me.CurSyncMode = SyncMode.NOT_SYNC Then Return

        Dim rowsUpdated As List(Of Integer) = Me.Range(rangePosition.Row, System.Math.Min(Me.Model.RowCount, rangePosition.EndRow + 1)).ToList
        Dim updateData = New List(Of Dictionary(Of String, Object))

        '删除掉没有真正修改内容的行
        rowsUpdated.RemoveAll(Function(row)
                                  For i = 0 To Me.Model.ColumnCount - 1
                                      If Me.dicCellEdited.ContainsKey(New CellPosition(row, i)) Then Return False
                                  Next
                                  Return True
                              End Function)
        'rowsUpdated的每一项和updateData的每一项相对应
        For Each curReoGridRowNum In rowsUpdated
            updateData.Add(Me.RowToDictionary(curReoGridRowNum))
        Next

        Try
            If rowsUpdated.Count > 0 Then
                RemoveHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
                Me.Model.UpdateRows(rowsUpdated.ToArray, updateData.ToArray)
                AddHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
            End If
        Catch ex As FrontWorkException
            MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            '删除dicCellEdited中所有在本次同步的选区之内的单元格
            Dim removeKeys = (From k In Me.dicCellEdited.Keys Where rangePosition.Contains(k) Select k).ToArray
            For Each removeKey In removeKeys
                Me.dicCellEdited.Remove(removeKey)
            Next
        End Try
    End Sub

    ''' <summary>
    ''' 导出选中的单元格的数据到Model
    ''' </summary>
    Protected Sub ExportCells(rangePosition As RangePosition)
        Logger.Debug("==ReoGrid ExportCell: " + Str(Me.GetHashCode))
        Logger.SetMode(LogMode.SYNC_FROM_VIEW)
        If Me.CurSyncMode = SyncMode.NOT_SYNC Then Return

        If rangePosition.Cols <> 1 Then
            Throw New FrontWorkException("ExportCells() can only be used when single column selected")
        End If

        Dim rowsUpdated As List(Of Integer) = Me.Range(rangePosition.Row, System.Math.Min(Me.Model.RowCount, rangePosition.EndRow + 1)).ToList
        Dim colUpdated As Integer = rangePosition.Col
        Dim fieldName = (From item In Me.dicNameColumn Where item.Value = colUpdated Select item.Key).FirstOrDefault
        Dim fieldConfiguration = (From fm In Me.Configuration.GetFieldConfigurations(Me.Mode) Where fm.Name = fieldName Select fm).FirstOrDefault
        If fieldConfiguration Is Nothing Then
            Logger.PutMessage("FieldConfiguration not found of column index: " & Str(colUpdated))
            Return
        End If
        Dim updateCellData = New List(Of Object)

        '删除掉没有真正修改内容的行
        rowsUpdated.RemoveAll(Function(row)
                                  If Me.dicCellEdited.ContainsKey(New CellPosition(row, colUpdated)) Then
                                      Return False
                                  Else
                                      Return True
                                  End If
                              End Function)

        'rowsUpdated的每一项和updateData的每一项相对应
        For Each curReoGridRowNum In rowsUpdated
            Call updateCellData.Add(Me.GetMappedCellData(curReoGridRowNum, colUpdated, fieldConfiguration))
        Next

        If rowsUpdated.Count > 0 Then
            RemoveHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
            Try
                Me.Model.UpdateCells(rowsUpdated.ToArray, Util.Times(fieldName, rowsUpdated.LongCount), updateCellData.ToArray)
            Catch ex As FrontWorkException
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End Try
            AddHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
        End If

        '删除dicCellEdited中所有在本次同步的选区之内的单元格
        Dim removeKeys = (From k In Me.dicCellEdited.Keys Where rangePosition.Contains(k) Select k).ToArray
        For Each removeKey In removeKeys
            Me.dicCellEdited.Remove(removeKey)
        Next
    End Sub

    ''' <summary>
    ''' 获取单元格数据结果（经过Mapper等之后的最终结果）
    ''' </summary>
    ''' <param name="row">行</param>
    ''' <param name="col">列</param>
    ''' <param name="fieldConfiguration">字段的Configuration</param>
    ''' <returns></returns>
    Protected Function GetMappedCellData(row As Integer, col As Integer, fieldConfiguration As FieldConfiguration)
        Dim curReoGridCell = Me.Panel.GetCell(row, col)
        '获取Cell中的文字
        Dim text As String
        If curReoGridCell Is Nothing Then
            text = ""
        Else
            text = If(Me.Panel.GetCell(row, col).Data, "")
        End If

        '将文字经过ReverseMapper映射成转换后的value
        Dim value As Object
        If fieldConfiguration.BackwardMapper IsNot Nothing Then
            value = fieldConfiguration.BackwardMapper.Invoke(text, row, Me)
        Else
            value = text
        End If
        Return value
    End Function

    ''' <summary>
    ''' 将整行数据转换成字典
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <returns>生成的字典</returns>
    Protected Function RowToDictionary(row As Integer) As Dictionary(Of String, Object)
        Dim dic = New Dictionary(Of String, Object)

        For Each curField As FieldConfiguration In Me.Configuration.GetFieldConfigurations(Me.Mode)
            '如果字段不可见，则不pull
            If Not curField.Visible Then Continue For
            '然后获取Cell
            If Me.dicNameColumn.ContainsKey(curField.Name) = False Then
                Logger.PutMessage("Field """ & curField.Name & """ not found in view")
                Continue For
            End If
            Dim curReoGridColumnNum = Me.dicNameColumn(curField.Name)
            Dim curReoGridCell = Panel.GetCell(row, curReoGridColumnNum)

            '获取DataTable的列
            Dim modelDataTable = Model.ToDataTable
            Dim curColumn As DataColumn = (From c As DataColumn In modelDataTable.Columns
                                           Where c.ColumnName = curField.Name
                                           Select c).FirstOrDefault
            If curColumn Is Nothing Then
                Logger.PutMessage("Field """ + curField.Name + """ not found in model!")
                Continue For
            End If
            '获取Cell中的内容
            If curField.Name = "state" Then
                Console.WriteLine()
            End If
            Dim value = Me.GetMappedCellData(row, curReoGridColumnNum, curField)
            '将新的值赋予Model中的相应单元格
            dic.Add(curField.Name, value)
        Next
        Return dic
    End Function

    Private Function DropdownListCellItemsToArray(items As DropdownListCell.DropdownItemsCollection) As Object()
        Return items.ToArray
    End Function

    ''' <summary>
    ''' 获取单元格的状态
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="col">列号</param>
    ''' <returns>单元格状态</returns>
    Protected Function GetCellState(row As Integer, col As Integer) As CellState
        If Me.dicCellState.ContainsKey(row) AndAlso Me.dicCellState(row).ContainsKey(col) Then
            Return Me.dicCellState(row)(col)
        Else
            Return CellState.Default
        End If
    End Function

    ''' <summary>
    ''' 增加单元格状态
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="col">列号</param>
    ''' <param name="cellState">单元格状态</param>
    Protected Sub AddCellState(row As Integer, col As Integer, cellState As CellState)
        Dim oriCellState = Me.GetCellState(row, col)
        cellState = oriCellState Or cellState
        If Not Me.dicCellState.ContainsKey(row) Then Me.dicCellState.Add(row, New Dictionary(Of Integer, Long))
        If Not Me.dicCellState(row).ContainsKey(col) Then
            Me.dicCellState(row).Add(col, cellState)
        Else
            Me.dicCellState(row)(col) = cellState
        End If
    End Sub

    ''' <summary>
    ''' 增加整行的单元格状态
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="cellState">单元格状态</param>
    Protected Sub AddCellState(row As Integer, cellState As CellState)
        If Not Me.dicCellState.ContainsKey(row) Then Me.dicCellState.Add(row, New Dictionary(Of Integer, Long))
        Dim cols = Util.Range(0, Me.Panel.Columns)
        For Each col In cols
            Dim oriCellState = Me.GetCellState(row, col)
            Dim newCellState = oriCellState Or cellState
            Me.dicCellState(row)(col) = newCellState
        Next
    End Sub

    ''' <summary>
    ''' 去除单元格状态
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="col">列号</param>
    ''' <param name="cellState">要去除的状态</param>
    Protected Sub RemoveCellState(row As Integer, col As Integer, cellState As CellState)
        Dim oriCellState = Me.GetCellState(row, col)
        Dim newCellState = oriCellState And Not cellState
        If Not Me.dicCellState.ContainsKey(row) Then Me.dicCellState.Add(row, New Dictionary(Of Integer, Long))
        If Not Me.dicCellState(row).ContainsKey(col) Then
            Me.dicCellState(row).Add(col, newCellState)
        Else
            Me.dicCellState(row)(col) = newCellState
        End If
    End Sub

    ''' <summary>
    ''' 去除单元格状态
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="cellState">要去除的状态</param>
    Protected Sub RemoveCellState(row As Integer, cellState As CellState)
        If Not Me.dicCellState.ContainsKey(row) Then Me.dicCellState.Add(row, New Dictionary(Of Integer, Long))
        Dim cols = Util.Range(0, Me.Panel.Columns)
        For Each col In cols
            Dim oriCellState = Me.GetCellState(row, col)
            Dim newCellState = oriCellState And Not cellState
            Me.dicCellState(row)(col) = newCellState
        Next
    End Sub

    ''' <summary>
    ''' 清除整行的单元格状态
    ''' </summary>
    ''' <param name="row">行号</param>
    Protected Sub ClearCellState(row as Integer)
        If Me.dicCellState.ContainsKey(row) Then
            Call Me.dicCellState.Remove(row)
        End If
    End Sub

    '''' <summary>
    '''' 获取视图中的单元格
    '''' </summary>
    '''' <param name="row">行号</param>
    '''' <param name="fieldName">字段名</param>
    '''' <returns>单元格对象</returns>
    'Public Function GetViewComponent(row as Integer, fieldName As String) As IViewComponent Implements IDataView.GetViewComponent
    '    If Me.Panel.RowCount <= row Then
    '        Throw New FrontWorkException($"Row {row} exceeded the last row of ReoGridView")
    '    End If
    '    Return Me.Panel.CreateAndGetCell(row, Me.dicNameColumn(Name))
    'End Function

    Private Sub ReoGridView_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub WorkbookPreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs)
        '处理复制不会触发事件的问题
        If e.Control AndAlso e.KeyCode = Keys.V Then
            Dim row = Me.Panel.SelectionRange.Row
            Dim col = Me.Panel.SelectionRange.Col
            Me.Panel.SelectionRange = New RangePosition(row, col, 1, 1)
            Me.copyStartCell = New CellPosition(row, col)
            Me.dicCellEdited.Add(Me.copyStartCell, True)
            Me.copied = True
        ElseIf e.KeyCode = Keys.Delete Then '处理Del删除单元格不会触发事件的问题
            Dim row = Me.Panel.SelectionRange.Row
            Dim rows = Me.Panel.SelectionRange.Rows
            Dim col = Me.Panel.SelectionRange.Col
            Dim cols = Me.Panel.SelectionRange.Cols
            For curRow = row To row + rows - 1
                For curCol = col To col + cols - 1
                    Dim cell = Me.Panel.GetCell(curRow, curCol)
                    If cell Is Nothing Then Continue For
                    Me.CellDataChanged(Me.Panel, New CellEventArgs(cell))
                Next
            Next
        End If
    End Sub
End Class

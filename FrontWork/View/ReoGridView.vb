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
    Implements IAssociableDataView

    Private COLOR_ROW_UNSYNCHRONIZED As Color = Color.AliceBlue
    Private COLOR_ROW_SYNCHRONIZED As Color = Color.Transparent
    Private COLOR_CELL_VALIDATION_ERROR As Color = Color.FromArgb(255, 107, 107)
    Private COLOR_CELL_VALIDATION_WARNING As Color = Color.FromArgb(255, 230, 109)

    Protected Class CellTag '太爽了，ReoGridView的Cell居然支持Tag，不用像Model一样麻烦了
        Public Property State As New ViewCellState
        Public Property Edited As Boolean
    End Class

    Protected Class ColumnTag
        Public Property ViewColumn As ViewColumn
        Public ReadOnly Property Name As String
            Get
                Return Me.ViewColumn.Name
            End Get
        End Property

    End Class

    Protected Class RowTag
        Public Property Inited As Boolean
        Public Property Temporary As Boolean = False
        Public Property RowState As New ViewRowState
        Public Property RowSyncState As SynchronizationState
            Get
                Return RowState.SynchronizationState
            End Get
            Set(value As SynchronizationState)
                RowState.SynchronizationState = value
            End Set
        End Property
    End Class

    Private Property ViewModel As AssociableDataViewModel

    Private canChangeSelectionRangeNextTime As Boolean = True
    Private copied As Boolean = False '是否复制粘贴。如果为真，则下次选区改变事件时处理粘贴后的选区
    Private copyStartCellPos As unvell.ReoGrid.CellPosition '复制起始单元格。用来判断复制是否选区没变导致不会触发选区改变事件
    Private bindedAssociationTextBox As Boolean = False

    Private textBox As TextBox = Nothing
    Private formAssociation As AdsorbableAssociationForm
    Private Workbook As ReoGridControl = Nothing

    Public Event EditStarted As EventHandler(Of ViewEditStartedEventArgs) Implements IEditableDataView.EditStarted
    Public Event ContentChanged As EventHandler(Of ViewContentChangedEventArgs) Implements IEditableDataView.ContentChanged
    Public Event EditEnded As EventHandler(Of ViewEditEndedEventArgs) Implements IEditableDataView.EditEnded
    Public Event RowUpdated As EventHandler(Of ViewRowUpdatedEventArgs) Implements IEditableDataView.RowUpdated
    Public Event RowAdded As EventHandler(Of ViewRowAddedEventArgs) Implements IEditableDataView.RowAdded
    Public Event RowRemoved As EventHandler(Of ViewRowRemovedEventArgs) Implements IEditableDataView.RowRemoved
    Public Event CellUpdated As EventHandler(Of ViewCellUpdatedEventArgs) Implements IEditableDataView.CellUpdated
    Private Event BeforeSelectionRangeChange As EventHandler(Of BeforeViewSelectionRangeChangeEventArgs) Implements ISelectableDataView.BeforeSelectionRangeChange
    Public Event SelectionRangeChanged As EventHandler(Of ViewSelectionRangeChangedEventArgs) Implements ISelectableDataView.SelectionRangeChanged
    Public Event BeforeRowStateChange As EventHandler(Of ViewBeforeRowStateChangeEventArgs) Implements IDataView.BeforeRowStateChange
    Public Event RowStateChanged As EventHandler(Of ViewRowStateChangedEventArgs) Implements IDataView.RowStateChanged

    Public Custom Event AssociationItemSelected As EventHandler(Of ViewAssociationItemSelectedEventArgs) Implements IAssociableDataView.AssociationItemSelected
        AddHandler(value As EventHandler(Of ViewAssociationItemSelectedEventArgs))
            AddHandler Me.formAssociation.AssociationItemSelected, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ViewAssociationItemSelectedEventArgs))
            RemoveHandler Me.formAssociation.AssociationItemSelected, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ViewAssociationItemSelectedEventArgs)

        End RaiseEvent
    End Event

    Public Property Panel As Worksheet

    Private Property NoColumn As Boolean = True
    Private Property NoRow As Boolean = True
    Private ReadOnly Property InSync As Boolean
        Get
            If Not NoColumn AndAlso Not NoRow Then Return True
            Return False
        End Get
    End Property

    Protected ReadOnly Property ViewColumns As ViewColumn()
        Get
            If Me.NoColumn Then Return {}
            Dim _viewColumns(Me.Panel.ColumnCount - 1) As ViewColumn
            For i = 0 To Me.Panel.ColumnCount - 1
                _viewColumns(i) = CType(Me.Panel.ColumnHeaders(i).Tag, ColumnTag).ViewColumn
            Next
            Return _viewColumns
        End Get
    End Property

    '''' <summary>
    '''' 同步模式，是否和Model数据是同步的
    '''' （如果Model没有数据，本视图上显示“暂无数据”，就处于不同步状态）
    '''' </summary>
    '''' <returns>同步模式</returns>
    'Protected Property CurSyncMode As SyncMode
    '    Get
    '        Return Me._curSyncMode
    '    End Get
    '    Set(value As SyncMode)
    '        If Me._curSyncMode = value Then Return
    '        Me._curSyncMode = value
    '        If value = SyncMode.SYNC Then
    '            Call Me.HideDefaultPage()
    '        Else
    '            Call Me.ShowDefaultPage()
    '        End If
    '    End Set
    'End Property

    ''' <summary>
    ''' 绑定的Model对象，用来存取数据
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
    ''' 绑定的配置中心对象，用来获取配置
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
    ''' 当前模式
    ''' </summary>
    ''' <returns></returns>
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
        Me.Panel = Me.ReoGridControl.CurrentWorksheet
        Me.Workbook = Me.ReoGridControl
        AddHandler Me.Workbook.PreviewKeyDown, AddressOf Me.WorkbookPreviewKeyDown

        If Not Me.DesignMode Then
            '创建联想窗口
            Me.Panel.StartEdit()
            Me.Panel.EndEdit(EndEditReason.NormalFinish)
            For Each control As Control In Me.ReoGridControl.Controls
                If TypeOf (control) Is TextBox AndAlso control.Name = "" Then
                    Me.textBox = control
                    Exit For
                End If
            Next
            If Me.textBox Is Nothing Then
                Throw New FrontWorkException("ReoGridView TextBox not found")
            End If
            Me.formAssociation = New AdsorbableAssociationForm
            RemoveHandler Me.textBox.PreviewKeyDown, AddressOf Me.TextboxPreviewKeyDown
            AddHandler Me.textBox.PreviewKeyDown, AddressOf Me.TextboxPreviewKeyDown
        End If

        '绑定ViewModel
        Me.ViewModel = New AssociableDataViewModel(Me)
    End Sub

    'Private Sub ModelRowSynchronizationStateChangedEvent(sender As Object, e As ModelRowSynchronizationStateChangedEventArgs)
    '    Dim rows = (From r In e.SynchronizationStateUpdatedRows Select r.Row).ToArray
    '    Call Me.RefreshRowSynchronizationStates(rows)
    'End Sub

    ''' <summary>
    ''' 根据各个单元格的状态，按行绘制单元格的颜色
    ''' </summary>
    Private Sub PaintRowsAndCells(Optional rows As Integer() = Nothing)
        If rows Is Nothing Then
            rows = Util.Range(0, Me.Panel.Rows)
        End If
        Dim rowColors = Me.GetRowColors(rows)
        For i = 0 To rows.Length - 1
            '绘制行
            Dim row = rows(i)
            'Me.Panel.SetRangeBorders(row, 0, 1, Me.Panel.ColumnCount, BorderPositions.All, RangeBorderStyle.SilverSolid)
            Me.Panel.SetRangeStyles(row, 0, 1, Me.Panel.ColumnCount, New WorksheetRangeStyle() With {
                        .Flag = PlainStyleFlag.BackColor,
                        .BackColor = rowColors(i)
                    })
            Me.Panel.SetRangeBorders(row, 0, 1, Me.Panel.ColumnCount, BorderPositions.All, RangeBorderStyle.SilverSolid)
            '绘制行内单元格
            Dim cols = Util.Range(0, Me.Panel.ColumnCount)
            Call Me.PaintCells(Util.Times(row, cols.Length), cols)
        Next
    End Sub

    Private Sub PaintCells(rows As Integer(), cols As Integer())
        Dim cellColors = Me.GetCellColors(rows, cols)
        For j = 0 To rows.Length - 1
            Dim row = rows(j), col = cols(j)
            Dim oriCellColor = Me.Panel.GetCell(row, col)?.Style.BackColor
            If oriCellColor.HasValue AndAlso oriCellColor.Value = cellColors(j) Then Continue For
            Me.Panel.SetRangeStyles(row, col, 1, 1, New WorksheetRangeStyle() With {
                .Flag = PlainStyleFlag.BackColor,
                .BackColor = cellColors(j)
            })
            Me.Panel.SetRangeBorders(row, col, 1, 1, BorderPositions.All, RangeBorderStyle.SilverSolid)
        Next
    End Sub

    Private Function GetCellColors(rows As Integer(), cols As Integer()) As Color()
        Dim tmpRowColors As New Dictionary(Of Integer, Color)
        Dim cellStates = Me.GetCellStates(rows, cols)
        Dim result(rows.Length - 1) As Color
        For i = 0 To result.Length - 1
            Dim row = rows(i)
            Dim curCellState = cellStates(i)
            If curCellState.ValidationState.Type = ValidationStateType.ERROR Then
                result(i) = COLOR_CELL_VALIDATION_ERROR
            ElseIf curCellState.ValidationState.Type = ValidationStateType.WARNING Then
                result(i) = COLOR_CELL_VALIDATION_WARNING
            ElseIf tmpRowColors.ContainsKey(row) Then
                result(i) = tmpRowColors(row)
            Else
                Dim curRowColor = Me.GetRowColors({row})(0)
                tmpRowColors(row) = curRowColor
                result(i) = curRowColor
            End If
        Next
        Return result
    End Function

    Private Function GetRowColors(rows As Integer()) As Color()
        Dim result(rows.Length - 1) As Color
        For i = 0 To result.Length - 1
            Dim rowState = DirectCast(Me.Panel.RowHeaders(rows(i)).Tag, RowTag).RowState
            If rowState.SynchronizationState <> SynchronizationState.SYNCHRONIZED Then
                result(i) = COLOR_ROW_UNSYNCHRONIZED
            Else
                result(i) = COLOR_ROW_SYNCHRONIZED
            End If
        Next
        Return result
    End Function

    ''' <summary>
    ''' 从Model同步选区
    ''' </summary>
    Protected Sub SetSelectionRanges(ranges As Range()) Implements IAssociableDataView.SetSelectionRanges
        If ranges.Length <= 0 Then
            RemoveHandler Me.Panel.BeforeSelectionRangeChange, AddressOf Me.ReoGrid_BeforeSelectionRangeChange
            Me.Panel.SelectionRange = RangePosition.Empty
            AddHandler Me.Panel.BeforeSelectionRangeChange, AddressOf Me.ReoGrid_BeforeSelectionRangeChange
            Return
        End If
        Dim range = ranges(0)
        RemoveHandler Me.Panel.BeforeSelectionRangeChange, AddressOf Me.ReoGrid_BeforeSelectionRangeChange
        Me.Panel.SelectionRange = New RangePosition(range.Row, range.Column, range.Rows, range.Columns)
        AddHandler Me.Panel.BeforeSelectionRangeChange, AddressOf Me.ReoGrid_BeforeSelectionRangeChange
    End Sub

    ''' <summary>
    ''' 显示默认页面
    ''' </summary>
    Protected Sub ShowDefaultPage()
        Call Me.Panel.DeleteRangeData(RangePosition.EntireRange)
        Me.ReoGridControl.Enabled = False
        '设定表格初始有10行，非同步模式
        Me.Panel.Rows = 10
        For i = 0 To Me.Panel.RowCount - 1
            Me.Panel.RowHeaders(i).Tag = New RowTag
        Next
        '设定提示文本
        Me.Panel.Item(0, 0) = "暂无数据"
        Call Me.PaintRowsAndCells()
    End Sub

    Private Sub textBoxLeave(sender As Object, e As EventArgs)
        Me.Workbook.Focus()
    End Sub

    Private Sub AutoFitColumnWidth(col As Integer, Optional suspendUIUpdates As Boolean = True)
        If suspendUIUpdates Then Call Me.Panel.SuspendUIUpdates()
        Me.Panel.ColumnHeaders(col).Width = 0
        Call Me.Panel.AutoFitColumnWidth(col)
        Dim columnHeader = Me.Panel.ColumnHeaders.Item(col)
        Dim columnHeaderText = columnHeader.Text
        Dim width
        If columnHeaderText Is Nothing Then
            width = 20
        Else
            width = 20 + columnHeaderText.Length * 10
        End If
        Me.Panel.ColumnHeaders(col).Width = System.Math.Max(width, Me.Panel.ColumnHeaders(col).Width + 10)
        If suspendUIUpdates Then Call Me.Panel.ResumeUIUpdates()
    End Sub

    ''' <summary>
    ''' 初始化一行的下拉框，只读等
    ''' </summary>
    ''' <param name="row">行号</param>
    Private Sub InitRow(row As Integer)
        If Me.Panel.RowHeaders(row).Tag?.Inited Then Return
        Dim worksheet = Me.Panel
        RemoveHandler worksheet.CellDataChanged, AddressOf Me.CellDataChanged
        '遍历列
        For col = 0 To Me.Panel.ColumnCount - 1
            Dim curViewColumn As ViewColumn = CType(Me.Panel.ColumnHeaders(col).Tag, ColumnTag).ViewColumn
            '如果设定了Values，则执行Values获取值
            If curViewColumn.Values IsNot Nothing Then
                Dim comboBox = New DropdownListCell(CType(curViewColumn.Values, IEnumerable(Of Object)))
                Dim curCol = col
                AddHandler comboBox.DropdownOpened, Sub()
                                                        worksheet.SelectionRange = New RangePosition(row, curCol, 1, 1)
                                                    End Sub
                worksheet(row, col) = comboBox
            End If

            If Not curViewColumn.Editable Then
                Dim curCell = worksheet.CreateAndGetCell(row, col)
                curCell.IsReadOnly = True
            End If
        Next
        AddHandler worksheet.CellDataChanged, AddressOf Me.CellDataChanged
        Me.Panel.RowHeaders(row).Tag.Inited = True
    End Sub

    'Private Sub TextboxPreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs)
    '    If Me.formAssociation.AdsorbTextBox Is Nothing Then
    '        Me.formAssociation.AdsorbTextBox = Me.textBox
    '    End If
    '    If (e.KeyCode = Keys.Up OrElse e.KeyCode = Keys.Down) AndAlso Me.formAssociation IsNot Nothing AndAlso Me.formAssociation.Visible = True Then
    '        Me.canChangeSelectionRange = False
    '        Me.formAssociation.StayVisible = True
    '        Dim threadRestartEdit = New Thread(
    '            Sub()
    '                Call Thread.Sleep(10)
    '                Call Me.Workbook.Invoke(
    '                Sub()
    '                    Call Me.Panel.StartEdit()
    '                End Sub)
    '            End Sub)
    '        Call threadRestartEdit.Start()
    '    ElseIf e.KeyCode = Keys.Enter And Me.formAssociation.Selected Then
    '        Me.canChangeSelectionRange = False
    '        Dim threadEnableChangeSelectionRange = New Thread(
    '            Sub()
    '                Call Thread.Sleep(100)
    '                Me.canChangeSelectionRange = True
    '            End Sub)
    '        Call threadEnableChangeSelectionRange.Start()
    '    Else
    '        Me.canChangeSelectionRange = True
    '        Me.formAssociation.StayVisible = False
    '    End If
    'End Sub

    '选择行改变时初始化新的行，只初始化选区首行
    Private Sub ReoGrid_BeforeSelectionRangeChange(sender As Object, e As BeforeSelectionChangeEventArgs)
        If Me.NoColumn OrElse Me.NoRow Then Return
        If Me.canChangeSelectionRangeNextTime = False Then
            e.IsCancelled = True
            Me.canChangeSelectionRangeNextTime = True
            Return
        End If
        Dim worksheet = Me.Panel
        Dim row = Me.Panel.SelectionRange.Row
        Dim rows = Me.Panel.SelectionRange.Rows
        Dim col = Me.Panel.SelectionRange.Col
        Dim cols = Me.Panel.SelectionRange.Cols

        Dim newRow = System.Math.Min(e.StartRow, e.EndRow)
        Dim newCol = System.Math.Min(e.StartCol, e.EndCol)
        Dim newRows = System.Math.Max(e.StartRow, e.EndRow) - newRow + 1 '选择整列的话行数会超出最大行数，可能是ReoGrid的bug
        If newRows > Me.Panel.RowCount Then
            newRows = Me.Panel.RowCount
        End If
        Dim newCols = System.Math.Max(e.StartCol, e.EndCol) - newCol + 1
        Dim copyRange As RangePosition = Nothing '如果之前复制，则会为此赋值

        '要更新的数据的选区
        Dim selectionRange = Me.Panel.SelectionRange
        Dim syncRange = selectionRange

        '如果新选区起始单元格和复制起始单元格不同，说明是复制单独一个单元格
        Dim copiedSingleCell = False
        If Me.copied AndAlso Not New unvell.ReoGrid.CellPosition(newRow, newCol).Equals(Me.copyStartCellPos) Then
            copiedSingleCell = True
        End If

        '如果是复制的单独单元格，则额外同步此单元格的数据并触发事件
        If Me.copied AndAlso copiedSingleCell Then
            Call Me.SetCellEdited(Me.copyStartCellPos, True)
            Call Me.ExportCells(New RangePosition(Me.copyStartCellPos), False)
            RaiseRangeEditEnded(New RangePosition(Me.copyStartCellPos))
            Call Me.SetCellEdited(Me.copyStartCellPos, False)
        ElseIf Me.copied Then
            '对于复制的选区，全部作为已修改状态
            For curRow = newRow To newRow + newRows - 1
                For curCol = newCol To newCol + newCols - 1
                    Me.SetCellEdited(New unvell.ReoGrid.CellPosition(curRow, curCol), True)
                Next
            Next
            copyRange = New RangePosition(newRow, newCol, newRows, newCols)
            syncRange = copyRange
        End If

        '同步数据
        Call Me.ExportCells(syncRange, False)
        ''绘制行
        'Call Me.PaintRows(Util.Range(selectionRange.Row, selectionRange.Row + selectionRange.Rows))

        '同步Model的选区
        RaiseEvent SelectionRangeChanged(Me, New ViewSelectionRangeChangedEventArgs({New Range(newRow, newCol, newRows, newCols)}))

        '触发编辑完成事件
        Call Me.RaiseRangeEditEnded(syncRange)
        Me.copied = False '清除复制状态

        '清除编辑状态
        SetCellEdited(syncRange, False)
    End Sub

    Private Sub RaiseRangeEditEnded(range As RangePosition)
        For curRow = range.Row To range.Row + range.Rows - 1
            For curCol = range.Col To range.Col + range.Cols - 1
                '如果该列没被编辑，则继续下一列
                If Not GetCellEdited(New unvell.ReoGrid.CellPosition(curRow, curCol)) Then Continue For
                Dim data = If(Me.Panel(curRow, curCol), "")
                Dim columnName = Me.FindNameByColumn(curCol)
                RaiseEvent EditEnded(Me, New ViewEditEndedEventArgs(curRow, columnName, data))
            Next
        Next
    End Sub

    Private Sub CellEditingTextChangingEvent(sender As Object, e As EventArgs)
        If Not bindedAssociationTextBox Then
            bindedAssociationTextBox = True
            Call Me.BindAssociationTextBox()
        End If
        Dim row = Me.Panel.SelectionRange.Row
        Dim col = Me.Panel.SelectionRange.Col
        If Not Me.Panel.RowHeaders(row).Tag.Inited Then '如果本行未被初始化，不要触发事件
            Return
        End If
        Dim colName = CType(Me.Panel.ColumnHeaders(col).Tag, ColumnTag).Name
        Me.SetCellEdited(New unvell.ReoGrid.CellPosition(row, col), True)
        RaiseEvent ContentChanged(Me, New ViewContentChangedEventArgs(row, colName, Me.textBox.Text))
    End Sub

    'TextBox文字改变的时候不会触发。TextBox编辑完成(EndEdit)时会触发。ComboBox选项改变的时候会触发
    Private Sub CellDataChanged(sender As Object, e As CellEventArgs)
        If Not Me.InSync Then Return
        Dim worksheet = Me.Panel
        Dim row = e.Cell.Row
        Dim col = e.Cell.Column
        '如果本行未被初始化，不要触发事件
        If Not Me.Panel.RowHeaders(row).Tag.Inited Then
            Return
        End If
        '如果已经设置为已编辑，说明在此事件之前已经触发过内容改变。故不要重复触发
        If Me.GetCellEdited(e.Cell.Position) Then Return
        '否则设置此单元格被编辑
        Call Me.SetCellEdited(e.Cell.Position, True)

        Dim fieldName = Me.FindNameByColumn(col)
        Dim viewColumn = (From v In Me.ViewColumns Where v.Name = fieldName Select v).First

        '如果当前格是下拉框，验证数据是否在下拉框可选项范围中，如果不在，则标红
        '同时，下拉框数据改变，直接同步到Model
        If viewColumn.Values IsNot Nothing Then
            '同步数据
            Call Me.ExportCells(e.Cell.PositionAsRange)
        End If
        ''内容改变应该会触发Model的同步状态改变，从而触发相关函数。不用在这里paint
        'Call Me.PaintRows({row})

        '触发内容改变事件
        RaiseEvent ContentChanged(Me, New ViewContentChangedEventArgs(row, fieldName, Me.GetCellData(row, col)))
    End Sub

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
    ''' 导出选中行的数据
    ''' </summary>
    Protected Sub ExportRows(rangePosition As RangePosition, Optional clearCellEdited As Boolean = True)
        If Not Me.InSync Then Return

        Dim rowsUpdated As List(Of Integer) = Me.Range(rangePosition.Row, System.Math.Min(Me.Panel.RowCount, rangePosition.EndRow + 1)).ToList
        Dim updateData = New List(Of Dictionary(Of String, Object))

        '删除掉没有真正修改内容的行
        rowsUpdated.RemoveAll(Function(row)
                                  For i = 0 To Me.ViewColumns.Count - 1
                                      If Me.GetCellEdited(New unvell.ReoGrid.CellPosition(row, i)) Then Return False
                                  Next
                                  Return True
                              End Function)
        'rowsUpdated的每一项和updateData的每一项相对应
        For Each curReoGridRowNum In rowsUpdated
            updateData.Add(Me.RowToDictionary(curReoGridRowNum))
        Next

        If rowsUpdated.Count > 0 Then
            Dim rowInfos(rowsUpdated.Count - 1) As ViewRowInfo
            For i = 0 To rowInfos.Length - 1
                rowInfos(i) = New ViewRowInfo(rowsUpdated(i), updateData(i), CType(Me.Panel.RowHeaders(rowsUpdated(i)).Tag, RowTag).RowState)
            Next
            RaiseEvent RowUpdated(Me, New ViewRowUpdatedEventArgs(rowInfos))
        End If

        If clearCellEdited Then
            '设置所有在本次同步的选区之内的单元格编辑状态为未编辑
            Call Me.SetCellEdited(rangePosition, False)
        End If
    End Sub

    ''' <summary>
    ''' 导出选中的单元格的数据到Model
    ''' </summary>
    Protected Sub ExportCells(rangePosition As RangePosition, Optional clearCellEdited As Boolean = True)
        If Not Me.InSync Then Return

        Dim modelRowCount = Me.Panel.RowCount
        Dim rowsUpdated As List(Of Integer) = Me.Range(rangePosition.Row, System.Math.Min(modelRowCount, rangePosition.EndRow + 1)).ToList
        Dim updatedCells As New List(Of ViewCellInfo) '更新的单元格
        For row = rangePosition.Row To rangePosition.Row + rowsUpdated.Count - 1
            For col = rangePosition.Col To rangePosition.Col + rangePosition.Cols
                If Me.GetCellEdited(New unvell.ReoGrid.CellPosition(row, col)) Then
                    Dim fieldName = Me.FindNameByColumn(col)
                    updatedCells.Add(New ViewCellInfo(row, fieldName, Me.GetCellData(row, col)))
                End If
            Next
        Next

        If updatedCells.Count > 0 Then
            RaiseEvent CellUpdated(Me, New ViewCellUpdatedEventArgs(updatedCells.ToArray))
        End If

        If clearCellEdited Then
            '设置所有在本次同步的选区之内的单元格编辑状态为未编辑
            Call Me.SetCellEdited(rangePosition, False)
        End If
    End Sub

    ''' <summary>
    ''' 获取单元格数据结果（经过Mapper等之后的最终结果）
    ''' </summary>
    ''' <param name="row">行</param>
    ''' <param name="col">列</param>
    ''' <returns></returns>
    Protected Function GetCellData(row As Integer, col As Integer) As String
        Dim curReoGridCell = Me.Panel.GetCell(row, col)
        '获取Cell中的文字
        Dim text As String
        If curReoGridCell Is Nothing Then
            text = ""
        Else
            text = If(Me.Panel.GetCell(row, col).Data, "")
        End If
        Return text
    End Function

    ''' <summary>
    ''' 将整行数据转换成字典
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <returns>生成的字典</returns>
    Protected Function RowToDictionary(row As Integer) As Dictionary(Of String, Object)
        Dim dic = New Dictionary(Of String, Object)
        For Each viewColumn As ViewColumn In Me.ViewColumns
            '然后获取Cell
            Dim curReoGridColumnNum = Me.FindColumnByName(viewColumn.Name)
            '获取Cell中的内容
            Dim value = Me.GetCellData(row, curReoGridColumnNum)
            '将新的值赋予Model中的相应单元格
            dic.Add(viewColumn.Name, value)
        Next
        Return dic
    End Function

    Private Function DropdownListCellItemsToArray(items As DropdownListCell.DropdownItemsCollection) As Object()
        Return items.ToArray
    End Function

    '''' <summary>
    '''' 增加单元格状态
    '''' </summary>
    '''' <param name="row">行号</param>
    '''' <param name="col">列号</param>
    '''' <param name="cellState">单元格状态</param>
    'Protected Sub AddCellState(row As Integer, col As Integer, cellState As CellState)
    '    Dim oriCellState = Me.GetCellState(row, col)
    '    cellState = oriCellState Or cellState
    '    Call Me.SetCellState(New CellPosition(row, col), cellState)
    'End Sub

    '''' <summary>
    '''' 增加整行的单元格状态
    '''' </summary>
    '''' <param name="row">行号</param>
    '''' <param name="cellState">单元格状态</param>
    'Protected Sub AddCellState(row As Integer, cellState As CellState)
    '    Dim cols = Util.Range(0, Me.Panel.Columns)
    '    For Each col In cols
    '        Dim oriCellState = Me.GetCellState(row, col)
    '        Dim newCellState = oriCellState Or cellState
    '        Call Me.SetCellState(New CellPosition(row, col), newCellState)
    '    Next
    'End Sub

    '''' <summary>
    '''' 去除单元格状态
    '''' </summary>
    '''' <param name="row">行号</param>
    '''' <param name="col">列号</param>
    '''' <param name="cellState">要去除的状态</param>
    'Protected Sub RemoveCellState(row As Integer, col As Integer, cellState As CellState)
    '    Dim oriCellState = Me.GetCellState(row, col)
    '    Dim newCellState = oriCellState And Not cellState
    '    Call Me.SetCellState(New CellPosition(row, col), newCellState)
    'End Sub

    '''' <summary>
    '''' 去除单元格状态
    '''' </summary>
    '''' <param name="row">行号</param>
    '''' <param name="cellState">要去除的状态</param>
    'Protected Sub RemoveCellState(row As Integer, cellState As CellState)
    '    Dim cols = Util.Range(0, Me.Panel.Columns)
    '    For Each col In cols
    '        Dim oriCellState = Me.GetCellState(row, col)
    '        Dim newCellState = oriCellState And Not cellState
    '        Call Me.SetCellState(New CellPosition(row, col), newCellState)
    '    Next
    'End Sub

    Private Sub ReoGridView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '禁止自动判断单元格格式
        Me.Panel.SetSettings(WorksheetSettings.Edit_AutoFormatCell, False)
        Call Me.ShowDefaultPage()

        If Not Me.DesignMode Then
            '给worksheet添加事件
            RemoveHandler Me.Panel.BeforeSelectionRangeChange, AddressOf ReoGrid_BeforeSelectionRangeChange
            AddHandler Me.Panel.BeforeSelectionRangeChange, AddressOf ReoGrid_BeforeSelectionRangeChange
            '编辑事件
            RemoveHandler Me.Panel.CellDataChanged, AddressOf Me.CellDataChanged
            AddHandler Me.Panel.CellDataChanged, AddressOf Me.CellDataChanged
            RemoveHandler Me.Panel.CellEditTextChanging, AddressOf Me.CellEditingTextChangingEvent
            AddHandler Me.Panel.CellEditTextChanging, AddressOf Me.CellEditingTextChangingEvent
            RemoveHandler Me.textBox.Leave, AddressOf Me.textBoxLeave
            AddHandler Me.textBox.Leave, AddressOf Me.textBoxLeave
        End If
    End Sub

    Private Sub BindAssociationTextBox()
        '绑定联想窗口编辑框
        Me.formAssociation.AdsorbTextBox = Me.textBox
    End Sub

    Private Sub TextboxPreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs)
        Select Case e.KeyCode
            Case Keys.Up, Keys.Down
                If Me.formAssociation.Visible Then
                    '如果上下键即将触发选取改变，则将选区改变事件禁止一次
                    If Not ((e.KeyCode = Keys.Up AndAlso Me.Panel.SelectionRange.Row = 0) OrElse
                        e.KeyCode = Keys.Down AndAlso Me.Panel.SelectionRange.Row = Me.Panel.RowCount - 1) Then
                        Me.canChangeSelectionRangeNextTime = False
                    End If
                    '10毫秒后重新启动编辑
                    Dim threadRestartEdit As New Thread(
                        Sub()
                            Thread.Sleep(10)
                            Me.Workbook.Invoke(
                                Sub()
                                    RemoveHandler Me.Panel.CellEditTextChanging, AddressOf Me.CellEditingTextChangingEvent
                                    Me.Panel.StartEdit()
                                    AddHandler Me.Panel.CellEditTextChanging, AddressOf Me.CellEditingTextChangingEvent
                                End Sub)
                        End Sub)
                    threadRestartEdit.Start()
                End If
        End Select
    End Sub

    Private Sub WorkbookPreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs)
        '处理复制不会触发事件的问题
        If e.Control AndAlso e.KeyCode = Keys.V Then
            Dim row = Me.Panel.SelectionRange.Row
            Dim col = Me.Panel.SelectionRange.Col
            Me.Panel.SelectionRange = New RangePosition(row, col, 1, 1)
            Me.copyStartCellPos = New unvell.ReoGrid.CellPosition(row, col)
            Call Me.SetCellEdited(Me.copyStartCellPos, True)
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

    Public Sub ShowAssociationForm() Implements IAssociableDataView.ShowAssociationForm
        If Not Me.bindedAssociationTextBox Then Return   
        Call Me.formAssociation.Show()
    End Sub

    Public Sub UpdateAssociationItems(associationItems() As AssociationItem) Implements IAssociableDataView.UpdateAssociationItems
        Call Me.formAssociation.UpdateAssociationItems(associationItems)
    End Sub

    Public Sub HideAssociationForm() Implements IAssociableDataView.HideAssociationForm
        Call Me.formAssociation.Hide()
    End Sub

    Public Function GetSelectionRanges() As Range() Implements ISelectableDataView.GetSelectionRanges
        Throw New NotImplementedException()
    End Function

    Public Function AddColumns(viewColumns() As ViewColumn) As Boolean Implements IDataView.AddColumns
        Dim oriColumnCount = If(Me.NoColumn, 0, Me.Panel.ColumnCount)
        Me.Panel.ColumnCount = oriColumnCount + viewColumns.Length
        For i = 0 To viewColumns.Length - 1
            Dim column = oriColumnCount + i
            Dim columnHeader = Me.Panel.ColumnHeaders.Item(column)
            Dim viewColumn = viewColumns(i)
            columnHeader.Text = viewColumn.DisplayName
            columnHeader.Tag = New ColumnTag() With {
                .ViewColumn = viewColumn
            }
        Next
        If Me.NoColumn Then
            Me.NoColumn = False
            If Me.InSync Then
                Me.ReoGridControl.Enabled = True
            End If
        End If
        Return True
    End Function

    Public Function UpdateColumns(indexes() As Integer, newViewColumns() As ViewColumn) As Object Implements IDataView.UpdateColumns
        For i = 0 To indexes.Length - 1
            Dim index = indexes(i)
            Dim newViewColumn = newViewColumns(i)
            Me.Panel.ColumnHeaders(index).Tag = New ColumnTag With {.ViewColumn = newViewColumn}
            Me.Panel.ColumnHeaders(index).Text = newViewColumn.DisplayName
        Next
        Return True
    End Function

    Public Function RemoveColumns(indexes() As Integer) As Object Implements IDataView.RemoveColumns
        If indexes.Length >= Me.Panel.ColumnCount Then
            Me.NoColumn = True
            Call Me.ShowDefaultPage()
            Return True
        End If
        Me.Panel.ColumnCount -= indexes.Length
        Return True
    End Function

    Public Function GetColumns() As ViewColumn() Implements IDataView.GetColumns
        Return Me.ViewColumns.ToArray
    End Function

    Public Function AddRows(data() As IDictionary(Of String, Object)) As Integer() Implements IDataView.AddRows
        'Call Me.Panel.SuspendUIUpdates()
        Dim startRow = If(NoRow, 0, Me.Panel.RowCount)
        Dim rows = data.Length
        Dim rowNums = Util.Range(startRow, startRow + rows)
        If Me.NoRow Then '如果当前是默认页面，则隐藏默认页面
            Me.ReoGridControl.Enabled = True
            Me.Panel.RowCount = data.Length
            Me.NoRow = False
        Else
            Me.Panel.AppendRows(data.Length)
        End If
        For Each row In rowNums
            Me.Panel.RowHeaders(row).Tag = New RowTag
            Call Me.InitRow(row)
        Next
        Call Me.UpdateRows(rowNums, data, False)
        ' Call Me.Panel.ResumeUIUpdates()
        Return rowNums
    End Function

    Public Sub InsertRows(rows() As Integer, data() As IDictionary(Of String, Object)) Implements IDataView.InsertRows
        'Call Me.Panel.SuspendUIUpdates()
        If rows.Length <> data.Length Then
            Throw New FrontWorkException($"InsertRows failed: Length of rows({rows.Length}) must be equal to Length of data({data.Length})")
        End If
        If rows.Length = 0 Then Return
        Dim addedTemporaryRow = False
        If Me.NoRow Then '如果当前是默认页面，则先隐藏默认页面，并保留一个临时行。待插入完毕后删除临时行
            addedTemporaryRow = True
            Me.ReoGridControl.Enabled = True
            Me.Panel.RowCount = 1
            Me.Panel.RowHeaders(0).Tag = New RowTag With {.Temporary = True}
            Me.NoRow = False
        End If
        Dim viewRowInfos(rows.Length - 1) As ViewRowInfo
        For i = 0 To rows.Length - 1
            viewRowInfos(i) = New ViewRowInfo(rows(i), data(i), Nothing)
        Next
        Dim oriViewRows = Me.Panel.RowCount
        Dim adjustedRowInfos = viewRowInfos
        Util.AdjustInsertIndexes(adjustedRowInfos, Function(rowInfo) rowInfo.Row, Sub(rowInfo, newRow) rowInfo.Row = newRow, oriViewRows)
        Dim adjustedRows As New List(Of Integer)
        Dim adjustedRowData As New List(Of IDictionary(Of String, Object))
        For Each adjustedRowInfo In adjustedRowInfos
            Dim row = adjustedRowInfo.Row
            Dim rowData = adjustedRowInfo.RowData
            adjustedRows.Add(row)
            adjustedRowData.Add(rowData)
            '去掉选区变化事件，防止插入行时触发选区变化事件，造成无用刷新和警告
            RemoveHandler Me.Panel.BeforeSelectionRangeChange, AddressOf Me.ReoGrid_BeforeSelectionRangeChange
            If row >= Me.Panel.RowCount Then
                Call Me.Panel.AppendRows(1)
                Dim rowNum = Me.Panel.RowCount - 1
                Me.Panel.RowHeaders(rowNum).Tag = New RowTag
                Call Me.InitRow(rowNum)
            Else
                Call Me.Panel.InsertRows(row, 1)
                Me.Panel.RowHeaders(row).Tag = New RowTag
                Call Me.InitRow(row)
            End If
            AddHandler Me.Panel.BeforeSelectionRangeChange, AddressOf Me.ReoGrid_BeforeSelectionRangeChange
        Next
        Call Me.UpdateRows(adjustedRows.ToArray, adjustedRowData.ToArray, False)
        If addedTemporaryRow Then
            '删除之前加入的临时行
            For i = 0 To Me.Panel.RowCount - 1
                If Me.Panel.RowHeaders(i).Tag.Temporary Then
                    Call Me.Panel.DeleteRows(i, 1)
                    Exit For
                End If
            Next
        End If
        'Call Me.Panel.ResumeUIUpdates()
    End Sub

    Public Sub RemoveRows(rows() As Integer) Implements IDataView.RemoveRows
        'Call Me.Panel.SuspendUIUpdates()

        If rows.Length >= Me.Panel.RowCount Then
            Me.NoRow = True
            Call Me.ShowDefaultPage()
            'Call Me.Panel.ResumeUIUpdates()
            Return
        End If
        Dim rowsDESC = (From r In rows Order By r Descending Select r).ToArray
        For Each row In rowsDESC
            '去掉选区变化事件，防止删除行时触发选区变化事件，造成无用刷新和警告
            RemoveHandler Me.Panel.BeforeSelectionRangeChange, AddressOf Me.ReoGrid_BeforeSelectionRangeChange
            Me.Panel.DeleteRows(row, 1)
            AddHandler Me.Panel.BeforeSelectionRangeChange, AddressOf Me.ReoGrid_BeforeSelectionRangeChange
        Next
        Dim minRemovedRow = rowsDESC(rowsDESC.Length - 1)
        Call Me.PaintRowsAndCells(Util.Range(minRemovedRow, Me.Panel.RowCount))
        'Call Me.Panel.ResumeUIUpdates()
    End Sub

    Public Sub UpdateRows(rows() As Integer, dataOfEachRow() As IDictionary(Of String, Object)) Implements IDataView.UpdateRows
        Call Me.UpdateRows(rows, dataOfEachRow, True)
    End Sub

    Public Sub UpdateRows(rows() As Integer, dataOfEachRow() As IDictionary(Of String, Object), suspendUIUpdates As Boolean)
        'If suspendUIUpdates Then Call Me.Panel.SuspendUIUpdates()
        Dim cellInfos As New List(Of ViewCellInfo)
        '清空ReoGrid相应行
        Me.ClearRows(rows)
        '遍历传入数据
        For i = 0 To rows.Length - 1
            Dim curRowNum = rows(i)
            '遍历列
            For Each viewColumn In Me.ViewColumns
                If Not dataOfEachRow(i).ContainsKey(viewColumn.Name) Then Continue For
                Dim value = dataOfEachRow(i)(viewColumn.Name)
                cellInfos.Add(New ViewCellInfo(curRowNum, viewColumn.Name, value))
            Next
        Next
        Call Me.UpdateCells(cellInfos.Select(Function(c) c.Row).ToArray,
                            cellInfos.Select(Function(c) c.ColumnName).ToArray,
                            cellInfos.Select(Function(c) c.CellData).ToArray,
                            False)

        'If suspendUIUpdates Then Call Me.Panel.ResumeUIUpdates()
    End Sub

    Public Sub UpdateCells(rows() As Integer, columnNames() As String, dataOfEachCell() As Object) Implements IDataView.UpdateCells
        Call Me.UpdateCells(rows, columnNames, dataOfEachCell, True)
    End Sub

    Public Sub UpdateCells(rows() As Integer, columnNames() As String, dataOfEachCell() As Object, suspendUIUpdates As Boolean)
        'If suspendUIUpdates Then Call Me.Panel.SuspendUIUpdates()
        For i = 0 To rows.Length - 1
            Dim row = rows(i)
            Dim colName = columnNames(i)
            Dim value = dataOfEachCell(i)
            '获取单元格
            Dim column = Me.FindColumnByName(colName)
            If column = -1 Then Continue For
            Dim cell = Panel.GetCell(row, column)
            If cell Is Nothing Then
                If String.IsNullOrEmpty(value) Then
                    Continue For
                Else
                    cell = Panel.CreateAndGetCell(row, column)
                End If
            End If
            Dim viewColumn = Me.ViewColumns.Where(Function(v) v.Name = colName).First

            '根据ViewColumn中的Field类型，处理View中的单元格
            RemoveHandler Me.Panel.CellDataChanged, AddressOf CellDataChanged
            cell.Data = value
            AddHandler Me.Panel.CellDataChanged, AddressOf CellDataChanged
        Next
        For Each colName In columnNames
            Me.AutoFitColumnWidth(Me.FindColumnByName(colName))
        Next
        Call Me.PaintCells(rows, Me.GetColumnsByNames(columnNames))
        'If suspendUIUpdates Then Call Me.Panel.ResumeUIUpdates()
    End Sub

    Public Function GetRowCount() As Integer Implements IDataView.GetRowCount
        Return If(Me.NoRow, 0, Me.Panel.RowCount)
    End Function

    Public Function GetColumnCount() As Integer Implements IDataView.GetColumnCount
        Return If(Me.NoColumn, 0, Me.ViewColumns.Count)
    End Function

    ''' <summary>
    ''' 根据列名称寻找列号
    ''' </summary>
    ''' <param name="name">列名</param>
    ''' <returns>列号，找不到返回-1</returns>
    Private Function FindColumnByName(name As String) As Integer
        If Me.NoColumn Then Return -1
        For i = 0 To Me.Panel.ColumnCount - 1
            If Me.Panel.ColumnHeaders(i).Tag.Name?.Equals(name) Then
                Return i
            End If
        Next
        Return -1
    End Function

    ''' <summary>
    ''' 根据列号寻找列名 
    ''' </summary>
    ''' <param name="column">列号</param>
    ''' <returns>列名</returns>
    Private Function FindNameByColumn(column As Integer) As String
        If Me.NoColumn Then Return Nothing
        If column >= Me.Panel.ColumnCount Then
            Throw New FrontWorkException($"Column {column} exceeded max column of {Me.Name}:{Me.Panel.ColumnCount - 1}")
        End If
        Return Me.Panel.ColumnHeaders(column).Tag.Name
    End Function

    Public Function GetCellStates(rows As Integer(), cols As Integer()) As ViewCellState()
        Dim result(rows.Length - 1) As ViewCellState
        For i = 0 To result.Length - 1
            Dim row = rows(i)
            Dim col = cols(i)
            Dim cell = Me.Panel.GetCell(row, col)
            If cell Is Nothing Then
                result(i) = New ViewCellState
            ElseIf cell.Tag Is Nothing Then
                result(i) = New ViewCellState
            Else
                result(i) = CType(cell.Tag, CellTag).State
            End If
        Next
        Return result
    End Function

    Public Function GetCellStates(rows As Integer(), fields As String()) As ViewCellState() Implements IDataView.GetCellStates
        Dim cols = Me.GetColumnsByNames(fields)
        Return Me.GetCellStates(rows, cols)
    End Function

    Public Function GetCellState(row As Integer, col As Integer) As ViewCellState
        Return Me.GetCellStates({row}, {col})?(0)
    End Function

    Public Function GetCellState(row As Integer, field As String) As ViewCellState
        Return Me.GetCellStates({row}, {field})?(0)
    End Function

    Private Function GetColumnsByNames(fieldNames As String()) As Integer()
        Dim tmp As New Dictionary(Of String, Integer)
        Dim result(fieldNames.Length - 1) As Integer
        Dim viewColumns = Me.ViewColumns
        For i = 0 To result.Length - 1
            Dim fieldName = fieldNames(i)
            If tmp.ContainsKey(fieldName) Then
                result(i) = tmp(fieldName)
            Else
                For j = 0 To viewColumns.Length - 1
                    If viewColumns(j).Name = fieldName Then
                        result(i) = j
                        Exit For
                    End If
                Next
            End If
        Next
        Return result
    End Function

    Public Sub UpdateCellStates(rows As Integer(), cols As Integer(), states As ViewCellState())
        For i = 0 To rows.Length - 1
            Dim cell = Me.Panel.CreateAndGetCell(rows(i), cols(i))
            Dim cellTag As CellTag = cell.Tag
            If cellTag Is Nothing Then
                cellTag = New CellTag
            End If
            cellTag.State = states(i)
            cell.Tag = cellTag
        Next
        Call Me.PaintCells(rows, cols)
    End Sub

    Public Sub UpdateCellState(row As Integer, col As Integer, state As ViewCellState)
        Call Me.UpdateCellStates({row}, {col}, {state})
    End Sub

    Public Sub UpdateCellStates(rows As Integer(), fields As String(), states As ViewCellState()) Implements IDataView.UpdateCellStates
        Dim cols = Me.GetColumnsByNames(fields)
        Call Me.UpdateCellStates(rows, cols, states)
    End Sub

    Public Sub UpdateCellState(row As Integer, field As String, state As ViewCellState)
        Call Me.UpdateCellStates({row}, {field}, {state})
    End Sub

    Private Function GetCellEdited(pos As unvell.ReoGrid.CellPosition) As Boolean
        Dim cell = Me.Panel.GetCell(pos)
        If cell Is Nothing Then Return False
        If cell.Tag Is Nothing Then Return False
        Return CType(cell.Tag, CellTag).Edited
    End Function

    Private Sub SetCellEdited(pos As unvell.ReoGrid.CellPosition, edited As Boolean)
        If pos.Row > Me.Panel.RowCount - 1 Then Return
        If pos.Col > Me.Panel.ColumnCount - 1 Then Return
        Dim cell = Me.Panel.CreateAndGetCell(pos)
        Dim cellTag As CellTag = cell.Tag
        If cellTag Is Nothing Then
            If edited = False Then Return
            cellTag = New CellTag
        End If
        cellTag.Edited = edited
        cell.Tag = cellTag
    End Sub

    Private Sub SetCellEdited(rangePosition As RangePosition, edited As Boolean)
        For row = rangePosition.Row To rangePosition.EndRow
            For col = rangePosition.Col To rangePosition.EndCol
                Call Me.SetCellEdited(New unvell.ReoGrid.CellPosition(row, col), edited)
            Next
        Next
    End Sub

    Public Sub UpdateRowState(rows As Integer(), states As ViewRowState()) Implements IDataView.UpdateRowStates
        For i = 0 To rows.Length - 1
            CType(Me.Panel.RowHeaders(rows(i)).Tag, RowTag).RowState = states(i)
        Next
        Call Me.PaintRowsAndCells(rows)
    End Sub

    Public Function GetRowStates(rows() As Integer) As ViewRowState() Implements IDataView.GetRowStates
        Dim states(rows.Length - 1) As ViewRowState
        For i = 0 To rows.Length - 1
            states(i) = CType(Me.Panel.RowHeaders(rows(i)).Tag, RowTag).RowState
        Next
        Return states
    End Function
End Class

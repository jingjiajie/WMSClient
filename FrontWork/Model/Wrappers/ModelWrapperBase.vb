Public Class ModelWrapperBase
    Implements IModelCore

    ''' <summary>
    ''' Model刷新事件
    ''' </summary>
    Public Event Refreshed As EventHandler(Of ModelRefreshedEventArgs) Implements IModelCore.Refreshed

    ''' <summary>
    ''' 增加行事件
    ''' </summary>
    Public Event RowAdded As EventHandler(Of ModelRowAddedEventArgs) Implements IModelCore.RowAdded

    ''' <summary>
    ''' 更新行数据事件
    ''' </summary>
    Public Event RowUpdated As EventHandler(Of ModelRowUpdatedEventArgs) Implements IModelCore.RowUpdated

    ''' <summary>
    ''' 删除行事件
    ''' </summary>
    Public Event BeforeRowRemove As EventHandler(Of ModelBeforeRowRemoveEventArgs) Implements IModelCore.BeforeRowRemove

    ''' <summary>
    ''' 删除行事件
    ''' </summary>
    Public Event RowRemoved As EventHandler(Of ModelRowRemovedEventArgs) Implements IModelCore.RowRemoved

    ''' <summary>
    ''' 单元格数据更新事件
    ''' </summary>
    Public Event CellUpdated As EventHandler(Of ModelCellUpdatedEventArgs) Implements IModelCore.CellUpdated

    ''' <summary>
    ''' 选区改变事件
    ''' </summary>
    Public Event SelectionRangeChanged As EventHandler(Of ModelSelectionRangeChangedEventArgs) Implements IModelCore.SelectionRangeChanged

    ''' <summary>
    ''' 行同步状态改变事件
    ''' </summary>
    Public Event RowSynchronizationStateChanged As EventHandler(Of ModelRowSynchronizationStateChangedEventArgs) Implements IModelCore.RowSynchronizationStateChanged

    ''' <summary>
    ''' ModelCore对象
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Property ModelCore As IModelCore


    Public Function GetColumnCount() As Integer Implements IModelCore.GetColumnCount
        Return Me.ModelCore.GetColumnCount
    End Function

    Public Function GetRowCount() As Integer Implements IModelCore.GetRowCount
        Return Me.ModelCore.GetRowCount
    End Function

    Public Function GetSelectionRanges() As Range() Implements IModelCore.GetSelectionRanges
        Return Me.ModelCore.GetSelectionRanges
    End Function

    Public Sub SetSelectionRanges(ranges As Range()) Implements IModelCore.SetSelectionRanges
        Call Me.ModelCore.SetSelectionRanges(ranges)
    End Sub

    ''' <summary>
    ''' 获取行
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <returns>相应行数据</returns>
    Public Function GetRows(rows As Integer()) As IDictionary(Of String, Object)() Implements IModelCore.GetRows
        Return Me.ModelCore.GetRows(rows)
    End Function

    ''' <summary>
    ''' 增加行
    ''' </summary>
    ''' <param name="dataOfEachRow">增加行的数据</param>
    ''' <returns>增加的行号</returns>
    Public Function AddRows(dataOfEachRow As IDictionary(Of String, Object)()) As Integer() Implements IModelCore.AddRows
        Dim addRowCount = dataOfEachRow.Length
        Dim oriRowCount = Me.GetRowCount
        Dim insertRows = Util.Range(Me.GetRowCount, Me.GetRowCount + addRowCount)
        Call Me.InsertRows(insertRows, dataOfEachRow)
        Return insertRows
    End Function


    ''' <summary>
    ''' 插入行
    ''' </summary>
    ''' <param name="rows">插入行行号</param>
    ''' <param name="dataOfEachRow">数据</param>
    Public Sub InsertRows(rows As Integer(), dataOfEachRow As IDictionary(Of String, Object)()) Implements IModelCore.InsertRows
        Call Me.ModelCore.InsertRows(rows, dataOfEachRow)
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="rows">删除行行号</param>
    Public Sub RemoveRows(rows As Integer()) Implements IModelCore.RemoveRows
        Call Me.ModelCore.RemoveRows(rows)
    End Sub

    ''' <summary>
    ''' 更新行
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="dataOfEachRow">对应的数据</param>
    Public Sub UpdateRows(rows As Integer(), dataOfEachRow As IDictionary(Of String, Object)()) Implements IModelCore.UpdateRows
        Call Me.ModelCore.UpdateRows(rows, dataOfEachRow)
    End Sub


    ''' <summary>
    ''' 更新单元格
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="columnNames">列名</param>
    ''' <param name="dataOfEachCell">相应的数据</param>
    Public Sub UpdateCells(rows As Integer(), columnNames As String(), dataOfEachCell As Object()) Implements IModelCore.UpdateCells
        Call Me.ModelCore.UpdateCells(rows, columnNames, dataOfEachCell)
    End Sub

    ''' <summary>
    ''' 刷新Model
    ''' </summary>
    ''' <param name="dataTable">数据表</param>
    ''' <param name="ranges">选区</param>
    ''' <param name="syncStates">各行同步状态</param>
    Public Overloads Sub Refresh(dataTable As DataTable, ranges As Range(), syncStates As SynchronizationState()) Implements IModelCore.Refresh
        Call Me.ModelCore.Refresh(dataTable, ranges, syncStates)
    End Sub


    Public Sub UpdateRowIDs(oriRowIDs As Guid(), newIDs As Guid()) Implements IModelCore.UpdateRowIDs
        Call Me.ModelCore.UpdateRowIDs(oriRowIDs, newIDs)
    End Sub

    ''' <summary>
    ''' 获取行ID
    ''' </summary>
    ''' <param name="rowNums">行号</param>
    ''' <returns>行ID</returns>
    Public Function GetRowIDs(rowNums As Integer()) As Guid() Implements IModelCore.GetRowIDs
        Return Me.ModelCore.GetRowIDs(rowNums)
    End Function

    Public Function GetRowIndexes(rowIDs As Guid()) As Integer() Implements IModelCore.GetRowIndexes
        Return Me.ModelCore.GetRowIndexes(rowIDs)
    End Function

    ''' <summary>
    ''' 更新行同步状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="syncStates">同步状态</param>
    Public Sub UpdateRowSynchronizationStates(rows As Integer(), syncStates As SynchronizationState()) Implements IModelCore.UpdateRowSynchronizationStates
        Call Me.ModelCore.UpdateRowSynchronizationStates(rows, syncStates)
    End Sub

    ''' <summary>
    ''' 获取行同步状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <returns>同步状态</returns>
    Public Function GetRowSynchronizationStates(rows As Integer()) As SynchronizationState() Implements IModelCore.GetRowSynchronizationStates
        Return Me.ModelCore.GetRowSynchronizationStates(rows)
    End Function

    Public Sub AddColumns(columns() As ModelColumn) Implements IModelCore.AddColumns
        Call Me.ModelCore.AddColumns(columns)
    End Sub

    Public Sub RemoveColumns(columnNames() As String) Implements IModelCore.RemoveColumns
        Call Me.ModelCore.RemoveColumns(columnNames)
    End Sub

    Public Function GetColumns() As ModelColumn() Implements IModelCore.GetColumns
        Return Me.ModelCore.GetColumns
    End Function

    Public Function GetColumns(columnNames() As String) As ModelColumn() Implements IModelCore.GetColumns
        Return Me.ModelCore.GetColumns(columnNames)
    End Function

    Public Function GetCells(rows() As Integer, columnNames() As String) As Object() Implements IModelCore.GetCells
        Return Me.ModelCore.GetCells(rows, columnNames)
    End Function

    Public Function ToDataTable() As DataTable Implements IModelCore.ToDataTable
        Return Me.ModelCore.ToDataTable
    End Function

    Public Sub RaiseRefreshedEvent(sender As Object, args As ModelRefreshedEventArgs)
        RaiseEvent Refreshed(sender, args)
    End Sub

    Public Sub RaiseCellUpdatedEvent(sender As Object, args As ModelCellUpdatedEventArgs)
        RaiseEvent CellUpdated(sender, args)
    End Sub

    Public Sub RaiseRowUpdatedEvent(sender As Object, args As ModelRowUpdatedEventArgs)
        RaiseEvent RowUpdated(sender, args)
    End Sub

    Public Sub RaiseRowAddedEvent(sender As Object, args As ModelRowAddedEventArgs)
        RaiseEvent RowAdded(sender, args)
    End Sub

    Public Sub RaiseBeforeRowRemoveEvent(sender As Object, args As ModelBeforeRowRemoveEventArgs)
        RaiseEvent BeforeRowRemove(sender, args)
    End Sub

    Public Sub RaiseRowRemovedEvent(sender As Object, args As ModelRowRemovedEventArgs)
        RaiseEvent RowRemoved(sender, args)
    End Sub

    Public Sub RaiseSelectionRangeChangedEvent(sender As Object, args As ModelSelectionRangeChangedEventArgs)
        RaiseEvent SelectionRangeChanged(sender, args)
    End Sub

    Public Sub RaiseRowSynchronizationStateChangedEvent(sender As Object, args As ModelRowSynchronizationStateChangedEventArgs)
        RaiseEvent RowSynchronizationStateChanged(sender, args)
    End Sub
End Class

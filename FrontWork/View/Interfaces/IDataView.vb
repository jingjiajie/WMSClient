Public Interface IDataView
    Inherits IView

    Function AddColumns(viewColumns As ViewColumn()) As Boolean
    Function UpdateColumns(oriColumnNames As String(), newViewColumns As ViewColumn())
    Function RemoveColumns(columnNames As String())
    Function GetColumns() As ViewColumn()

    ''' <summary>
    ''' 增加若干行
    ''' </summary>
    ''' <param name="data">新增行的数据</param>
    ''' <returns>新增行的行号</returns>
    Function AddRows(data As IDictionary(Of String, Object)()) As Integer()

    ''' <summary>
    ''' 插入若干行
    ''' </summary>
    ''' <param name="rows">插入行的行号</param>
    ''' <param name="data">插入行的数据</param>
    Sub InsertRows(rows As Integer(), data As IDictionary(Of String, Object)())

    ''' <summary>
    ''' 删除若干行
    ''' </summary>
    ''' <param name="rows">删除的行号</param>
    Sub RemoveRows(rows As Integer())

    ''' <summary>
    ''' 更新若干行
    ''' </summary>
    ''' <param name="rows">更新的行号</param>
    ''' <param name="dataOfEachRow">更新的数据，和行号一一对应</param>
    Sub UpdateRows(rows As Integer(), dataOfEachRow As IDictionary(Of String, Object)())

    ''' <summary>
    ''' 更新若干单元格
    ''' </summary>
    ''' <param name="rows">更新的行号</param>
    ''' <param name="columnNames">更新的字段名，和行号一一对应</param>
    ''' <param name="dataOfEachCell">更新的数据，和行号一一对应</param>
    Sub UpdateCells(rows As Integer(), columnNames As String(), dataOfEachCell As Object())

    ''' <summary>
    ''' View所存储数据的行数
    ''' </summary>
    ''' <returns></returns>
    Function GetRowCount() As Integer

    ''' <summary>
    ''' View所存储数据的列数
    ''' </summary>
    ''' <returns></returns>
    Function GetColumnCount() As Integer

    ''' <summary>
    ''' View的选区
    ''' </summary>
    ''' <returns></returns>
    Function GetSelectionRanges() As Range()

    ''' <summary>
    ''' 设置View的选区
    ''' </summary>
    ''' <param name="ranges">选区</param>
    Sub SetSelectionRanges(ranges As Range())

    Event BeforeRowUpdated As EventHandler(Of BeforeViewRowUpdateEventArgs)
    Event BeforeRowAdded As EventHandler(Of BeforeViewRowAddEventArgs)
    Event BeforeRowRemoved As EventHandler(Of BeforeViewRowRemoveEventArgs)
    Event BeforeCellUpdated As EventHandler(Of BeforeViewCellUpdateEventArgs)
    Event BeforeSelectionRangeChange As EventHandler(Of BeforeViewSelectionRangeChangeEventArgs)

    Event RowUpdated As EventHandler(Of ViewRowUpdatedEventArgs)
    Event RowAdded As EventHandler(Of ViewRowAddedEventArgs)
    Event RowRemoved As EventHandler(Of ViewRowRemovedEventArgs)
    Event CellUpdated As EventHandler(Of ViewCellUpdatedEventArgs)
    Event SelectionRangeChanged As EventHandler(Of ViewSelectionRangeChangedEventArgs)
End Interface

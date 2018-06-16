''' <summary>
''' Model模型的总接口
''' </summary>
Public Interface IModelCore
    ''' <summary>
    ''' Model增加列
    ''' </summary>
    ''' <param name="columns">要增加的各列</param>
    Sub AddColumns(columns As ModelColumn())

    ''' <summary>
    ''' Model删除列
    ''' </summary>
    ''' <param name="columnNames">要删除的各列名</param>
    Sub RemoveColumns(columnNames As String())

    ''' <summary>
    ''' 获取Model的所有列
    ''' </summary>
    ''' <returns></returns>
    Function GetColumns() As ModelColumn()

    ''' <summary>
    ''' 获取Model的所有列
    ''' </summary>
    ''' <returns></returns>
    Function GetColumns(columnNames As String()) As ModelColumn()

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
    ''' 更新若干行的同步状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="syncStates">同步状态</param>
    Sub UpdateRowSynchronizationStates(rows As Integer(), syncStates As SynchronizationState())

    ''' <summary>
    ''' 获取若干行的同步状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <returns>对应的同步状态</returns>
    Function GetRowSynchronizationStates(rows As Integer()) As SynchronizationState()

    ''' <summary>
    ''' 根据行号获取行ID
    ''' </summary>
    ''' <param name="rowNums">行号</param>
    ''' <returns>行ID</returns>
    Function GetRowIDs(rowNums As Integer()) As Guid()

    ''' <summary>
    ''' 获取行号
    ''' </summary>
    ''' <param name="rowIDs">行ID</param>
    ''' <returns>行号</returns>
    Function GetRowIndexes(rowIDs As Guid()) As Integer()

    ''' <summary>
    ''' 更新行ID
    ''' </summary>
    ''' <param name="oriRowIDs">原行ID</param>
    ''' <param name="newIDs">新ID</param>
    Sub UpdateRowIDs(oriRowIDs As Guid(), newIDs As Guid())

    ''' <summary>
    ''' 获取行数据
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <returns>数据</returns>
    Function GetRows(rows As Integer()) As IDictionary(Of String, Object)()

    ''' <summary>
    ''' 获取单元格数据
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="columnNames">列号</param>
    ''' <returns></returns>
    Function GetCells(rows As Integer(), columnNames As String()) As Object()

    ''' <summary>
    ''' 刷新视图
    ''' </summary>
    ''' <param name="dataTable">数据表</param>
    ''' <param name="selectionRange">选区</param>
    ''' <param name="syncStates">各行同步状态</param>
    Sub Refresh(dataTable As DataTable, selectionRange As Range(), syncStates As SynchronizationState())

    ''' <summary>
    ''' Model所存储数据的行数
    ''' </summary>
    ''' <returns></returns>
    Function GetRowCount() As Integer

    ''' <summary>
    ''' Model所存储数据的列数
    ''' </summary>
    ''' <returns></returns>
    Function GetColumnCount() As Integer

    ''' <summary>
    ''' Model的选区
    ''' </summary>
    ''' <returns></returns>
    Function GetSelectionRanges() As Range()

    ''' <summary>
    ''' 设置Model的选区
    ''' </summary>
    ''' <param name="ranges">选区</param>
    Sub SetSelectionRanges(ranges As Range())

    ''' <summary>
    ''' 转换为DataTable
    ''' </summary>
    ''' <returns></returns>
    Function ToDataTable() As DataTable

    '事件
    ''' <summary>
    ''' Model被刷新事件
    ''' </summary>
    Event Refreshed As EventHandler(Of ModelRefreshedEventArgs)

    ''' <summary>
    ''' 行增加事件
    ''' </summary>
    Event RowAdded As EventHandler(Of ModelRowAddedEventArgs)

    ''' <summary>
    ''' 行数据更新事件
    ''' </summary>
    Event RowUpdated As EventHandler(Of ModelRowUpdatedEventArgs)

    ''' <summary>
    ''' 删除行事件
    ''' </summary>
    Event RowRemoved As EventHandler(Of ModelRowRemovedEventArgs)

    ''' <summary>
    ''' 删除行事件
    ''' </summary>
    Event BeforeRowRemove As EventHandler(Of ModelBeforeRowRemoveEventArgs)

    ''' <summary>
    ''' 单元格数据更新事件
    ''' </summary>
    Event CellUpdated As EventHandler(Of ModelCellUpdatedEventArgs)

    ''' <summary>
    ''' 选区改变事件
    ''' </summary>
    Event SelectionRangeChanged As EventHandler(Of ModelSelectionRangeChangedEventArgs)

    ''' <summary>
    ''' 行同步状态改变事件
    ''' </summary>
    Event RowSynchronizationStateChanged As EventHandler(Of ModelRowSynchronizationStateChangedEventArgs)
End Interface

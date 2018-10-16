''' <summary>
''' ModelCore的接口
''' </summary>
Public Interface IModelCore
    ''' <summary>
    ''' 获取Model相关信息
    ''' </summary>
    ''' <param name="infoItem"></param>
    ''' <returns></returns>
    Function GetInfo(infoItem As ModelInfo) As Object

    ''' <summary>
    ''' Model增加列
    ''' </summary>
    ''' <param name="columns">要增加的各列</param>
    Sub AddColumns(columns As ModelColumn())

    ''' <summary>
    ''' Model更新列
    ''' </summary>
    ''' <param name="indexes">要更新列的位置</param>
    ''' <param name="columns">新的列信息</param>
    Sub UpdateColumn(indexes As Integer(), columns As ModelColumn())

    ''' <summary>
    ''' Model删除列
    ''' </summary>
    ''' <param name="indexes">要删除的列的位置</param>
    Sub RemoveColumns(indexes As Integer())

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
    ''' <param name="states">状态</param>
    Sub UpdateRowStates(rows As Integer(), states As ModelRowState())

    ''' <summary>
    ''' 获取若干行的状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <returns>对应的状态</returns>
    Function GetRowStates(rows As Integer()) As ModelRowState()

    ''' <summary>
    ''' 获取单元格的状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="fields">列名</param>
    ''' <returns></returns>
    Function GetCellStates(rows As Integer(), fields As String()) As ModelCellState()

    ''' <summary>
    ''' 更新单元格的状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="fields">列名</param>
    ''' <param name="states">单元格状态</param>
    Sub UpdateCellStates(rows As Integer(), fields As String(), states As ModelCellState())

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
    ''' 刷新Model
    ''' </summary>
    ''' <param name="args">刷新参数</param>
    Sub Refresh(args As ModelRefreshArgs)

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
    ''' 行状态改变事件
    ''' </summary>
    Event RowStateChanged As EventHandler(Of ModelRowStateChangedEventArgs)

    ''' <summary>
    ''' 单元格状态改变事件
    ''' </summary>
    Event CellStateChanged As EventHandler(Of ModelCellStateChangedEventArgs)
End Interface

Public Enum ModelInfo
    HAS_ERROR_CELL
    HAS_WARNING_CELL
    HAS_UNSYNCHRONIZED_ROW
End Enum
Public Interface IDataView
    Inherits IView

    Event BeforeRowStateChange As EventHandler(Of ViewBeforeRowStateChangeEventArgs)
    Event RowStateChanged As EventHandler(Of ViewRowStateChangedEventArgs)

    Function AddColumns(viewColumns As ViewColumn()) As Boolean
    Function UpdateColumns(indexes As Integer(), newViewColumns As ViewColumn())
    Function RemoveColumns(indexes As Integer())
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
    ''' 设置行同步状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="states">行状态</param>
    Sub UpdateRowStates(rows As Integer(), states As ViewRowState())

    ''' <summary>
    ''' 获取行状态
    ''' </summary>
    ''' <param name="rows">状态</param>
    Function GetRowStates(rows As Integer()) As ViewRowState()

    ''' <summary>
    ''' 获取单元格状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="fields">字段</param>
    ''' <returns></returns>
    Function GetCellStates(rows As Integer(), fields As String()) As ViewCellState()

    ''' <summary>
    ''' 更新单元格状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="fields">字段</param>
    Sub UpdateCellStates(rows As Integer(), fields As String(), states As ViewCellState())

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

End Interface

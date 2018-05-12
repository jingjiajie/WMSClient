''' <summary>
''' Model模型的总接口
''' </summary>
Public Interface IModel
    '数据操作方法
    ''' <summary>
    ''' 增加一行
    ''' </summary>
    ''' <param name="data">新增行的数据</param>
    ''' <returns>新增行的行号</returns>
    Function AddRow(data As Dictionary(Of String, Object)) As Long

    ''' <summary>
    ''' 增加若干行
    ''' </summary>
    ''' <param name="data">新增行的数据</param>
    ''' <returns>新增行的行号</returns>
    Function AddRows(data As Dictionary(Of String, Object)()) As Long()

    ''' <summary>
    ''' 插入一行
    ''' </summary>
    ''' <param name="row">插入行号</param>
    ''' <param name="data">插入行数据</param>
    Sub InsertRow(row As Long, data As Dictionary(Of String, Object))

    ''' <summary>
    ''' 插入若干行
    ''' </summary>
    ''' <param name="rows">插入行的行号</param>
    ''' <param name="data">插入行的数据</param>
    Sub InsertRows(rows As Long(), data As Dictionary(Of String, Object)())

    ''' <summary>
    ''' 删除一行
    ''' </summary>
    ''' <param name="row">删除的行号</param>
    Sub RemoveRow(row As Long)

    ''' <summary>
    ''' 删除一行
    ''' </summary>
    ''' <param name="rowID">删除行的ID</param>
    Sub RemoveRow(rowID As Guid)

    ''' <summary>
    ''' 删除若干行
    ''' </summary>
    ''' <param name="rows">删除的行号</param>
    Sub RemoveRows(rows As Long())

    ''' <summary>
    ''' 删除若干行
    ''' </summary>
    ''' <param name="rowIDs">删除的行ID</param>
    Sub RemoveRows(rowIDs As Guid())

    ''' <summary>
    ''' 删除若干行
    ''' </summary>
    ''' <param name="startRow">起始行的行号</param>
    ''' <param name="rowCount">删除的行数</param>
    Sub RemoveRows(startRow As Long, rowCount As Long)

    ''' <summary>
    ''' 删除选中的行
    ''' </summary>
    Sub RemoveSelectedRows()

    ''' <summary>
    ''' 更新一行
    ''' </summary>
    ''' <param name="row">更新的行号</param>
    ''' <param name="data">更新的数据</param>
    Sub UpdateRow(row As Long, data As Dictionary(Of String, Object))

    ''' <summary>
    ''' 更新一行
    ''' </summary>
    ''' <param name="rowID">更新行的ID</param>
    ''' <param name="data">更新的数据</param>
    Sub UpdateRow(rowID As Guid, data As Dictionary(Of String, Object))

    ''' <summary>
    ''' 更新若干行
    ''' </summary>
    ''' <param name="rows">更新的行号</param>
    ''' <param name="dataOfEachRow">更新的数据，和行号一一对应</param>
    Sub UpdateRows(rows As Long(), dataOfEachRow As Dictionary(Of String, Object)())

    ''' <summary>
    ''' 更新若干行
    ''' </summary>
    ''' <param name="rowIDs">更新的行ID</param>
    ''' <param name="dataOfEachRow">更新的数据，和ID一一对应</param>
    Sub UpdateRows(rowIDs As Guid(), dataOfEachRow As Dictionary(Of String, Object)())

    ''' <summary>
    ''' 更新若干单元格
    ''' </summary>
    ''' <param name="rows">更新的行号</param>
    ''' <param name="columnNames">更新的字段名，和行号一一对应</param>
    ''' <param name="dataOfEachCell">更新的数据，和行号一一对应</param>
    Sub UpdateCells(rows As Long(), columnNames As String(), dataOfEachCell As Object())

    ''' <summary>
    ''' 更新一个单元格
    ''' </summary>
    ''' <param name="row">更新的行号</param>
    ''' <param name="columnName">更新的字段名</param>
    ''' <param name="data">更新的数据</param>
    Sub UpdateCell(row As Long, columnName As String, data As Object)

    ''' <summary>
    ''' 更新若干单元格
    ''' </summary>
    ''' <param name="rowID">更新行的ID</param>
    ''' <param name="columnNames">更新</param>
    ''' <param name="dataOfEachCell"></param>
    Sub UpdateCells(rowID As Guid(), columnNames As String(), dataOfEachCell As Object())

    ''' <summary>
    ''' 更新一个单元格
    ''' </summary>
    ''' <param name="rowID">行ID</param>
    ''' <param name="columnName">列名</param>
    ''' <param name="data">单元格数据</param>
    Sub UpdateCell(rowID As Guid, columnName As String, data As Object)

    ''' <summary>
    ''' 更新若干行的同步状态
    ''' </summary>
    ''' <param name="rows">行ID</param>
    ''' <param name="syncStates">同步状态</param>
    Sub UpdateRowSynchronizationStates(rows As Guid(), syncStates As SynchronizationState())

    ''' <summary>
    ''' 更新若干行的同步状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="syncStates">同步状态</param>
    Sub UpdateRowSynchronizationStates(rows As Long(), syncStates As SynchronizationState())

    ''' <summary>
    ''' 更新一行的同步状态
    ''' </summary>
    ''' <param name="row">行ID</param>
    ''' <param name="syncState">同步状态</param>
    Sub UpdateRowSynchronizationState(row As Guid, syncState As SynchronizationState)

    ''' <summary>
    ''' 更新一行的同步状态
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="syncState">同步状态</param>
    Sub UpdateRowSynchronizationState(row As Long, syncState As SynchronizationState)

    ''' <summary>
    ''' 获取若干行同步状态
    ''' </summary>
    ''' <param name="rows">行ID</param>
    ''' <returns>相应的同步状态</returns>
    Function GetRowSynchronizationStates(rows As Guid()) As SynchronizationState()

    ''' <summary>
    ''' 获取若干行的同步状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <returns>对应的同步状态</returns>
    Function GetRowSynchronizationStates(rows As Long()) As SynchronizationState()

    ''' <summary>
    ''' 获取一行的同步状态
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <returns>对应的同步状态</returns>
    Function GetRowSynchronizationState(row As Long) As SynchronizationState

    ''' <summary>
    ''' 获取若干行的同步状态
    ''' </summary>
    ''' <param name="row">行ID</param>
    ''' <returns>对应的同步状态</returns>
    Function GetRowSynchronizationState(row As Guid) As SynchronizationState

    ''' <summary>
    ''' 根据行号获取行ID
    ''' </summary>
    ''' <param name="rowNum">行号</param>
    ''' <returns>行ID</returns>
    Function GetRowID(rowNum As Long) As Guid

    ''' <summary>
    ''' 根据行号获取行ID
    ''' </summary>
    ''' <param name="rowNums">行号</param>
    ''' <returns>行ID</returns>
    Function GetRowIDs(rowNums As Long()) As Guid()

    ''' <summary>
    ''' 获取行数据
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <returns>数据</returns>
    Function GetRows(rows As Long()) As DataTable

    ''' <summary>
    ''' 获取行数据
    ''' </summary>
    ''' <param name="rowIDs">行号</param>
    ''' <returns>数据</returns>
    Function GetRows(rowIDs As Guid()) As DataTable

    ''' <summary>
    ''' 获取Model内部的DataTable
    ''' </summary>
    ''' <returns>DataTable对象</returns>
    Function GetDataTable() As DataTable

    ''' <summary>
    ''' 获取单元格数据
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="column">列号</param>
    ''' <returns></returns>
    Function GetCell(row As Long, column As Long) As Object

    ''' <summary>
    ''' 获取单元格数据
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="columnName">列名</param>
    ''' <returns></returns>
    Function GetCell(row As Long, columnName As String) As Object

    ''' <summary>
    ''' 刷新视图
    ''' </summary>
    ''' <param name="dataTable">数据表</param>
    ''' <param name="selectionRange">选区</param>
    ''' <param name="syncStates">各行同步状态</param>
    Sub Refresh(dataTable As DataTable, selectionRange As Range(), syncStates As SynchronizationState())

    '属性相关
    ''' <summary>
    ''' 配置中心对象
    ''' </summary>
    ''' <returns></returns>
    Property Configuration As Configuration

    ''' <summary>
    ''' 当前配置模式
    ''' </summary>
    ''' <returns></returns>
    Property Mode As String

    ''' <summary>
    ''' Model所存储数据的行数
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property RowCount As Long

    ''' <summary>
    ''' Model所存储数据的列数
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property ColumnCount As Long

    ''' <summary>
    ''' Model的选区
    ''' </summary>
    ''' <returns></returns>
    Property AllSelectionRanges As Range()

    ''' <summary>
    ''' Model的第一个选区。对于只支持一个选区的表格控件，此参数比较实用
    ''' </summary>
    ''' <returns></returns>
    Property SelectionRange As Range

    ''' <summary>
    ''' Model的名称，用来区分不同Model
    ''' </summary>
    ''' <returns></returns>
    Property Name As String

    Default Property Item(row As Long, column As Long) As Object
    Default Property Item(row As Long, columnName As String) As Object

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

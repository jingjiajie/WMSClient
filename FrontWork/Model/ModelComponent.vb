Imports System.ComponentModel
Imports System.Linq
Imports System.Reflection
Imports FrontWork

''' <summary>
''' 模型类
''' </summary>
Public Class ModelComponent
    Inherits Component
    Implements IConfigurableModel

    Protected Property ModelOperator As New ConfigurableModelOperator(New ModelConfigurationWrapper(New ModelCore))

    ''' <summary>
    ''' 配置中心对象
    ''' </summary>
    ''' <returns></returns>
    <DisplayName("配置中心对象"), Description("配置中心对象"), Category("FrontWork")>
    Public Property Configuration As Configuration Implements IConfigurableModel.Configuration
        Get
            Return Me.ModelOperator.Configuration
        End Get
        Set(value As Configuration)
            Me.ModelOperator.Configuration = value
        End Set
    End Property

    ''' <summary>
    ''' 当前模式
    ''' </summary>
    ''' <returns></returns>
    <DisplayName("配置模式"), Description("当前配置模式"), Category("FrontWork")>
    Public Property Mode As String Implements IConfigurableModel.Mode
        Get
            Return Me.ModelOperator.Mode
        End Get
        Set(value As String)
            ModelOperator.Mode = value
        End Set
    End Property

    ''' <summary>
    ''' 数据行数
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    Public ReadOnly Property RowCount As Integer
        Get
            Return Me.ModelOperator.GetRowCount
        End Get
    End Property

    Public Function GetRowCount() As Integer Implements IModel.GetRowCount
        Return Me.ModelOperator.GetRowCount
    End Function

    <Browsable(False)>
    Public ReadOnly Property ColumnCount As Integer
        Get
            Return Me.GetColumnCount
        End Get
    End Property

    Public Function GetColumnCount() As Integer Implements IModel.GetColumnCount
        Return Me.ModelOperator.GetColumnCount
    End Function

    Public Function GetSelectionRanges() As Range() Implements IModel.GetSelectionRanges
        Return Me.ModelOperator.GetSelectionRanges
    End Function

    Public Sub SetSelectionRanges(ranges As Range()) Implements IModel.SetSelectionRanges
        Call Me.ModelOperator.SetSelectionRanges(ranges)
    End Sub

    ''' <summary>
    ''' 选区
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    Public Property AllSelectionRanges As Range()
        Get
            Return Me.GetSelectionRanges
        End Get
        Set(value As Range())
            Me.SetSelectionRanges(value)
        End Set
    End Property

    ''' <summary>
    ''' 选区
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    Public Property AllSelectionRanges(i As Integer) As Range
        Get
            Return Me.GetSelectionRanges(i)
        End Get
        Set(value As Range)
            Dim allSelectionRanges = Me.GetSelectionRanges
            allSelectionRanges(i) = value
            Me.SetSelectionRanges(allSelectionRanges)
        End Set
    End Property

    ''' <summary>
    ''' 首个选区
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    Public Property SelectionRange As Range
        Get
            If Me.AllSelectionRanges Is Nothing Then Return Nothing
            If Me.AllSelectionRanges.Length = 0 Then Return Nothing
            Return Me.AllSelectionRanges(0)
        End Get
        Set(value As Range)
            If value Is Nothing Then
                Me.AllSelectionRanges = {}
            ElseIf Me.AllSelectionRanges Is Nothing Then
                Me.AllSelectionRanges = {value}
            ElseIf Me.AllSelectionRanges.Length = 0 Then
                Me.AllSelectionRanges = {value}
            Else
                Me.AllSelectionRanges(0) = value
            End If
        End Set
    End Property

    Default Public Property _Item(row As Integer) As IDictionary(Of String, Object)
        Get
            Return Me.GetRow(row)
        End Get
        Set(value As IDictionary(Of String, Object))
            Call Me.UpdateRow(row, value)
        End Set
    End Property

    Default Public Property _Item(row As Integer, column As Integer) As Object
        Get
            Return Me.GetCell(row, column)
        End Get
        Set(value As Object)
            Call Me.UpdateCell(row, column, value)
        End Set
    End Property

    Default Public Property _Item(row As Integer, columnName As String) As Object
        Get
            Return Me.GetCell(row, columnName)
        End Get
        Set(value As Object)
            Call Me.UpdateCell(row, columnName, value)
        End Set
    End Property

    Public Function GetCell(row As Integer, columnName As String) As Object
        Return Me.ModelOperator.GetCell(row, columnName)
    End Function

    ''' <summary>
    ''' 获取行并自动转换成相应类型的对象返回
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="rows"></param>
    ''' <returns></returns>
    Public Function GetRows(Of T As New)(rows As Integer()) As T()
        Return ModelOperator.GetRows(Of T)(rows)
    End Function

    Public Function GetRow(Of T As New)(row As Integer) As T
        Return Me.ModelOperator.GetRow(Of T)(row)
    End Function

    ''' <summary>
    ''' 获取行
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <returns>相应行数据</returns>
    Public Function GetRows(rows As Integer()) As IDictionary(Of String, Object)() Implements IModel.GetRows
        Return Me.ModelOperator.GetRows(rows)
    End Function

    ''' <summary>
    ''' 获取行
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <returns>相应行数据</returns>
    Public Function GetRow(row As Integer) As IDictionary(Of String, Object)
        Return Me.ModelOperator.GetRow(row)
    End Function

    ''' <summary>
    ''' 增加行
    ''' </summary>
    ''' <param name="data">增加行的数据</param>
    ''' <returns>增加的行号</returns>
    Public Function AddRow(data As IDictionary(Of String, Object)) As Integer
        Return Me.ModelOperator.AddRow(data)
    End Function

    ''' <summary>
    ''' 增加行
    ''' </summary>
    ''' <param name="dataOfEachRow">增加行的数据</param>
    ''' <returns>增加的行号</returns>
    Public Function AddRows(dataOfEachRow As IDictionary(Of String, Object)()) As Integer() Implements IModel.AddRows
        Return Me.ModelOperator.AddRows(dataOfEachRow)
    End Function

    ''' <summary>
    ''' 插入行
    ''' </summary>
    ''' <param name="row">插入行行号</param>
    ''' <param name="data">数据</param>
    Public Sub InsertRow(row As Integer, data As IDictionary(Of String, Object))
        Call Me.ModelOperator.InsertRow(row, data)
    End Sub

    ''' <summary>
    ''' 插入行
    ''' </summary>
    ''' <param name="rows">插入行行号</param>
    ''' <param name="dataOfEachRow">数据</param>
    Public Sub InsertRows(rows As Integer(), dataOfEachRow As IDictionary(Of String, Object)()) Implements IModel.InsertRows
        Call Me.ModelOperator.InsertRows(rows, dataOfEachRow)
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="row">删除行行号</param>
    Public Sub RemoveRow(row As Integer)
        Call Me.ModelOperator.RemoveRow(row)
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="startRow">起始行号</param>
    ''' <param name="rowCount">删除行数</param>
    Public Sub RemoveRows(startRow As Integer, rowCount As Integer)
        Call Me.ModelOperator.RemoveRows(startRow, rowCount)
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="rows">删除行行号</param>
    Public Sub RemoveRows(rows As Integer()) Implements IModel.RemoveRows
        Call Me.ModelOperator.RemoveRows(rows)
    End Sub

    Public Sub RemoveSelectedRows()
        Call Me.ModelOperator.RemoveSelectedRows()
    End Sub


    ''' <summary>
    ''' 更新行
    ''' </summary>
    ''' <param name="row">更新行行号</param>
    ''' <param name="data">数据</param>
    Public Sub UpdateRow(row As Integer, data As IDictionary(Of String, Object))
        Call Me.ModelOperator.UpdateRow(row, data)
    End Sub

    ''' <summary>
    ''' 更新行
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="dataOfEachRow">对应的数据</param>
    Public Sub UpdateRows(rows As Integer(), dataOfEachRow As IDictionary(Of String, Object)()) Implements IModel.UpdateRows
        Call Me.ModelOperator.UpdateRows(rows, dataOfEachRow)
    End Sub

    ''' <summary>
    ''' 更新单元格
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="columnName">列名</param>
    ''' <param name="data">更新的数据</param>
    Public Sub UpdateCell(row As Integer, columnName As String, data As Object)
        Call Me.ModelOperator.UpdateCell(row, columnName, data)
    End Sub

    ''' <summary>
    ''' 更新单元格
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="columnNames">列名</param>
    ''' <param name="dataOfEachCell">相应的数据</param>
    Public Sub UpdateCells(rows As Integer(), columnNames As String(), dataOfEachCell As Object()) Implements IModel.UpdateCells
        Call Me.ModelOperator.UpdateCells(rows, columnNames, dataOfEachCell)
    End Sub

    Public Overloads Sub Refresh(args As ModelRefreshArgs) Implements IModel.Refresh
        Call Me.ModelOperator.Refresh(args)
    End Sub

    Public Sub UpdateRowStates(rows As Integer(), states As ModelRowState()) Implements IModel.UpdateRowStates
        Call Me.ModelOperator.UpdateRowStates(rows, states)
    End Sub

    ''' <summary>
    ''' 更新行同步状态
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="state">同步状态</param>
    Public Sub UpdateRowState(row As Integer, state As ModelRowState)
        Call Me.ModelOperator.UpdateRowState(row, state)
    End Sub

    ''' <summary>
    ''' 获取行状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <returns>状态</returns>
    Public Function GetRowStates(rows As Integer()) As ModelRowState() Implements IModel.GetRowStates
        Return Me.ModelOperator.GetRowStates(rows)
    End Function

    ''' <summary>
    ''' 获取行同步状态
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <returns>同步状态</returns>
    Public Function GetRowState(row As Integer) As ModelRowState
        Return Me.ModelOperator.GetRowState(row)
    End Function

    Public Function GetRowSynchronizationStates(rows As Integer()) As SynchronizationState()
        Dim rowStates = Me.GetRowStates(rows)
        Return (From s In rowStates Select s.SynchronizationState).ToArray
    End Function

    Public Function GetRowSynchronizationState(row As Integer) As SynchronizationState
        Return Me.GetRowSynchronizationStates({row})(0)
    End Function

    ''' <summary>
    ''' Model刷新事件
    ''' </summary>
    Public Custom Event Refreshed As EventHandler(Of ModelRefreshedEventArgs) Implements IModel.Refreshed
        AddHandler(value As EventHandler(Of ModelRefreshedEventArgs))
            AddHandler Me.ModelOperator.Refreshed, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRefreshedEventArgs))
            RemoveHandler Me.ModelOperator.Refreshed, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRefreshedEventArgs)
        End RaiseEvent
    End Event

    ''' <summary>
    ''' 增加行事件
    ''' </summary>
    Public Custom Event RowAdded As EventHandler(Of ModelRowAddedEventArgs) Implements IModel.RowAdded
        AddHandler(value As EventHandler(Of ModelRowAddedEventArgs))
            AddHandler Me.ModelOperator.RowAdded, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRowAddedEventArgs))
            RemoveHandler Me.ModelOperator.RowAdded, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRowAddedEventArgs)

        End RaiseEvent
    End Event

    ''' <summary>
    ''' 更新行数据事件
    ''' </summary>
    Public Custom Event RowUpdated As EventHandler(Of ModelRowUpdatedEventArgs) Implements IModel.RowUpdated
        AddHandler(value As EventHandler(Of ModelRowUpdatedEventArgs))
            AddHandler Me.ModelOperator.RowUpdated, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRowUpdatedEventArgs))
            RemoveHandler Me.ModelOperator.RowUpdated, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRowUpdatedEventArgs)

        End RaiseEvent
    End Event

    ''' <summary>
    ''' 删除行事件
    ''' </summary>
    Public Custom Event BeforeRowRemove As EventHandler(Of ModelBeforeRowRemoveEventArgs) Implements IModel.BeforeRowRemove
        AddHandler(value As EventHandler(Of ModelBeforeRowRemoveEventArgs))
            AddHandler Me.ModelOperator.BeforeRowRemove, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelBeforeRowRemoveEventArgs))
            RemoveHandler Me.ModelOperator.BeforeRowRemove, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelBeforeRowRemoveEventArgs)
        End RaiseEvent
    End Event

    ''' <summary>
    ''' 删除行事件
    ''' </summary>
    Public Custom Event RowRemoved As EventHandler(Of ModelRowRemovedEventArgs) Implements IModel.RowRemoved
        AddHandler(value As EventHandler(Of ModelRowRemovedEventArgs))
            AddHandler Me.ModelOperator.RowRemoved, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRowRemovedEventArgs))
            RemoveHandler Me.ModelOperator.RowRemoved, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRowRemovedEventArgs)

        End RaiseEvent
    End Event

    ''' <summary>
    ''' 单元格数据更新事件
    ''' </summary>
    Public Custom Event CellUpdated As EventHandler(Of ModelCellUpdatedEventArgs) Implements IModel.CellUpdated
        AddHandler(value As EventHandler(Of ModelCellUpdatedEventArgs))
            AddHandler Me.ModelOperator.CellUpdated, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelCellUpdatedEventArgs))
            RemoveHandler Me.ModelOperator.CellUpdated, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelCellUpdatedEventArgs)

        End RaiseEvent
    End Event

    ''' <summary>
    ''' 选区改变事件
    ''' </summary>
    Public Custom Event SelectionRangeChanged As EventHandler(Of ModelSelectionRangeChangedEventArgs) Implements IModel.SelectionRangeChanged
        AddHandler(value As EventHandler(Of ModelSelectionRangeChangedEventArgs))
            AddHandler Me.ModelOperator.SelectionRangeChanged, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelSelectionRangeChangedEventArgs))
            RemoveHandler Me.ModelOperator.SelectionRangeChanged, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelSelectionRangeChangedEventArgs)

        End RaiseEvent
    End Event

    ''' <summary>
    ''' 行同步状态改变事件
    ''' </summary>
    Public Custom Event RowStateChanged As EventHandler(Of ModelRowStateChangedEventArgs) Implements IModel.RowStateChanged
        AddHandler(value As EventHandler(Of ModelRowStateChangedEventArgs))
            AddHandler Me.ModelOperator.RowStateChanged, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRowStateChangedEventArgs))
            RemoveHandler Me.ModelOperator.RowStateChanged, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRowStateChangedEventArgs)

        End RaiseEvent
    End Event

    Public Function ContainsColumn(columnName As String) As Boolean
        Return Me.GetColumns({columnName})(0) IsNot Nothing
    End Function

    Public Sub SelectRowsByValues(Of T)(columnName As String, values As T())
        Call Me.ModelOperator.SelectRowsByValues(columnName, values)
    End Sub

    ''' <summary>
    ''' 获取所有选中行
    ''' </summary>
    ''' <typeparam name="T">要映射成的类型</typeparam>
    ''' <returns>选中行映射后的对象数组</returns>
    Public Function GetSelectedRows(Of T As New)() As T()
        Return Me.ModelOperator.GetSelectedRows(Of T)
    End Function

    ''' <summary>
    ''' 获取所有选中行的某一列
    ''' </summary>
    ''' <typeparam name="T">返回类型</typeparam>
    ''' <param name="columnName">列名</param>
    ''' <returns>所有选中行指定列的数据</returns>
    Public Function GetSelectedRows(Of T)(columnName As String) As T()
        Return Me.ModelOperator.GetSelectedRows(Of T)(columnName)
    End Function

    Public Function GetSelectedRows() As IDictionary(Of String, Object)()
        Return Me.ModelOperator.GetSelectedRows
    End Function

    Public Function GetSelectedRow() As IDictionary(Of String, Object)
        Return Me.ModelOperator.GetSelectedRow
    End Function

    Public Function GetSelectedRow(Of T As New)() As IDictionary(Of String, Object)
        Return Me.ModelOperator.GetSelectedRow(Of T)
    End Function

    Public Function GetSelectedRow(Of T)(columnName As String) As T
        Return Me.ModelOperator.GetSelectedRow(Of T)(columnName)
    End Function

    ''' <summary>
    ''' 删除新增但未编辑的行
    ''' </summary>
    Public Sub RemoveUneditedNewRows()
        Call Me.ModelOperator.RemoveUneditedNewRows()
    End Sub

    Public Function HasUnsynchronizedUpdatedRow() As Boolean
        Return Me.ModelOperator.HasUnsynchronizedUpdatedRow
    End Function

    Public Sub AddColumns(columns() As ModelColumn) Implements IModel.AddColumns
        Call Me.ModelOperator.AddColumns(columns)
    End Sub

    Public Sub RemoveColumns(indexes As Integer()) Implements IModel.RemoveColumns
        Call Me.ModelOperator.RemoveColumns(indexes)
    End Sub

    Public Function GetColumns() As ModelColumn() Implements IModel.GetColumns
        Return Me.ModelOperator.GetColumns
    End Function

    Public Function GetColumns(columnNames() As String) As ModelColumn() Implements IModel.GetColumns
        Return Me.ModelOperator.GetColumns(columnNames)
    End Function

    Public Function GetCells(rows() As Integer, columnNames() As String) As Object() Implements IModel.GetCells
        Return Me.ModelOperator.GetCells(rows, columnNames)
    End Function

    Public Sub RefreshView(rows As Integer())
        Call Me.ModelOperator.RefreshView(rows)
    End Sub

    Public Sub RefreshView(row As Integer)
        Call Me.ModelOperator.RefreshView(row)
    End Sub

    Public Sub RaiseRefreshedEvent(sender As Object, args As ModelRefreshedEventArgs)
        Call Me.ModelOperator.RaiseRefreshedEvent(sender, args)
    End Sub

    Public Sub RaiseCellUpdatedEvent(sender As Object, args As ModelCellUpdatedEventArgs)
        Call Me.ModelOperator.RaiseCellUpdatedEvent(sender, args)
    End Sub

    Public Sub RaiseRowUpdatedEvent(sender As Object, args As ModelRowUpdatedEventArgs)
        Call Me.ModelOperator.RaiseRowUpdatedEvent(sender, args)
    End Sub

    Public Sub RaiseRowAddedEvent(sender As Object, args As ModelRowAddedEventArgs)
        Call Me.ModelOperator.RaiseRowAddedEvent(sender, args)
    End Sub

    Public Sub RaiseBeforeRowRemoveEvent(sender As Object, args As ModelBeforeRowRemoveEventArgs)
        Call Me.ModelOperator.RaiseBeforeRowRemoveEvent(sender, args)
    End Sub

    Public Sub RaiseRowRemovedEvent(sender As Object, args As ModelRowRemovedEventArgs)
        Call Me.ModelOperator.RaiseRowRemovedEvent(sender, args)
    End Sub

    Public Sub RaiseSelectionRangeChangedEvent(sender As Object, args As ModelSelectionRangeChangedEventArgs)
        Call Me.ModelOperator.RaiseSelectionRangeChangedEvent(sender, args)
    End Sub

    Public Sub RaiseRowSynchronizationStateChangedEvent(sender As Object, args As ModelRowStateChangedEventArgs)
        Call Me.ModelOperator.RaiseRowStateChangedEvent(sender, args)
    End Sub

    Public Sub UpdateColumn(indexes() As Integer, columns() As ModelColumn) Implements IModel.UpdateColumn
        DirectCast(ModelOperator, IConfigurableModel).UpdateColumn(indexes, columns)
    End Sub
End Class


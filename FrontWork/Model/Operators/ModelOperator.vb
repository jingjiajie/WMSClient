Imports FrontWork
Imports System.Linq
Imports System.Reflection

Public Class ModelOperator
    Implements IModel
    Private _ModelCore As IModelCore

    Public Property ModelCore As IModelCore
        Get
            Return Me._ModelCore
        End Get
        Set(value As IModelCore)
            Me._ModelCore = value
        End Set
    End Property

    ''' <summary>
    ''' 数据行数
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property RowCount As Integer Implements IModel.RowCount
        Get
            Return Me.GetRowCount
        End Get
    End Property

    Public ReadOnly Property ColumnCount As Integer Implements IModel.ColumnCount
        Get
            Return Me.GetColumnCount
        End Get
    End Property

    Public Sub New()

    End Sub

    Public Sub New(modelCore As IModelCore)
        Me._ModelCore = modelCore
    End Sub

    Public Custom Event Refreshed As EventHandler(Of ModelRefreshedEventArgs) Implements IModelCore.Refreshed
        AddHandler(value As EventHandler(Of ModelRefreshedEventArgs))
            AddHandler _ModelCore.Refreshed, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRefreshedEventArgs))
            RemoveHandler _ModelCore.Refreshed, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRefreshedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event RowAdded As EventHandler(Of ModelRowAddedEventArgs) Implements IModelCore.RowAdded
        AddHandler(value As EventHandler(Of ModelRowAddedEventArgs))
            AddHandler _ModelCore.RowAdded, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRowAddedEventArgs))
            RemoveHandler _ModelCore.RowAdded, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRowAddedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event RowUpdated As EventHandler(Of ModelRowUpdatedEventArgs) Implements IModelCore.RowUpdated
        AddHandler(value As EventHandler(Of ModelRowUpdatedEventArgs))
            AddHandler _ModelCore.RowUpdated, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRowUpdatedEventArgs))
            RemoveHandler _ModelCore.RowUpdated, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRowUpdatedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event RowRemoved As EventHandler(Of ModelRowRemovedEventArgs) Implements IModelCore.RowRemoved
        AddHandler(value As EventHandler(Of ModelRowRemovedEventArgs))
            AddHandler _ModelCore.RowRemoved, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRowRemovedEventArgs))
            RemoveHandler _ModelCore.RowRemoved, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRowRemovedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event BeforeRowRemove As EventHandler(Of ModelBeforeRowRemoveEventArgs) Implements IModelCore.BeforeRowRemove
        AddHandler(value As EventHandler(Of ModelBeforeRowRemoveEventArgs))
            AddHandler _ModelCore.BeforeRowRemove, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelBeforeRowRemoveEventArgs))
            RemoveHandler _ModelCore.BeforeRowRemove, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelBeforeRowRemoveEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event CellUpdated As EventHandler(Of ModelCellUpdatedEventArgs) Implements IModelCore.CellUpdated
        AddHandler(value As EventHandler(Of ModelCellUpdatedEventArgs))
            AddHandler _ModelCore.CellUpdated, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelCellUpdatedEventArgs))
            RemoveHandler _ModelCore.CellUpdated, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelCellUpdatedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event SelectionRangeChanged As EventHandler(Of ModelSelectionRangeChangedEventArgs) Implements IModelCore.SelectionRangeChanged
        AddHandler(value As EventHandler(Of ModelSelectionRangeChangedEventArgs))
            AddHandler _ModelCore.SelectionRangeChanged, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelSelectionRangeChangedEventArgs))
            RemoveHandler _ModelCore.SelectionRangeChanged, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelSelectionRangeChangedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event RowStateChanged As EventHandler(Of ModelRowStateChangedEventArgs) Implements IModelCore.RowStateChanged
        AddHandler(value As EventHandler(Of ModelRowStateChangedEventArgs))
            AddHandler _ModelCore.RowStateChanged, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRowStateChangedEventArgs))
            RemoveHandler _ModelCore.RowStateChanged, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRowStateChangedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event CellStateChanged As EventHandler(Of ModelCellStateChangedEventArgs) Implements IModelCore.CellStateChanged
        AddHandler(value As EventHandler(Of ModelCellStateChangedEventArgs))
            AddHandler _ModelCore.CellStateChanged, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelCellStateChangedEventArgs))
            RemoveHandler _ModelCore.CellStateChanged, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelCellStateChangedEventArgs)
        End RaiseEvent
    End Event


    Public Property Name As String

    Default Public Property _Item(row As Integer) As IDictionary(Of String, Object) Implements IModel._Item
        Get
            Return Me.GetRow(row)
        End Get
        Set(value As IDictionary(Of String, Object))
            Call Me.UpdateRow(row, value)
        End Set
    End Property

    Default Public Property _Item(row As Integer, columnName As String) As Object Implements IModel._Item
        Get
            Return Me.GetCell(row, columnName)
        End Get
        Set(value As Object)
            Call Me.UpdateCell(row, columnName, value)
        End Set
    End Property


    Public Function GetCell(row As Integer, columnName As String) As Object Implements IModel.GetCell
        Return Me._ModelCore.GetCells({row}, {columnName})(0)
    End Function


    ''' <summary>
    ''' 获取行并自动转换成相应类型的对象返回
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="rows"></param>
    ''' <returns></returns>
    Public Overloads Function GetRows(Of T As New)(rows As Integer()) As T() Implements IModel.GetRows
        Dim rowData = Me.GetRows(rows)
        Dim result(rows.Length - 1) As T
        For i = 0 To result.Length - 1
            result(i) = Me.DictionaryToObject(Of T)(rowData(i))
        Next
        Return result
    End Function

    Public Function GetRow(Of T As New)(row As Integer) As T Implements IModel.GetRow
        Return Me.GetRows(Of T)({row})(0)
    End Function

    ''' <summary>
    ''' 获取行
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <returns>相应行数据</returns>
    Public Function GetRow(row As Integer) As IDictionary(Of String, Object) Implements IModel.GetRow
        Return Me.GetRows({row})(0)
    End Function

    ''' <summary>
    ''' 增加行
    ''' </summary>
    ''' <param name="data">增加行的数据</param>
    ''' <returns>增加的行号</returns>
    Public Function AddRow(data As IDictionary(Of String, Object)) As Integer Implements IModel.AddRow
        Return Me.AddRows({data})(0)
    End Function


    ''' <summary>
    ''' 插入行
    ''' </summary>
    ''' <param name="row">插入行行号</param>
    ''' <param name="data">数据</param>
    Public Sub InsertRow(row As Integer, data As IDictionary(Of String, Object)) Implements IModel.InsertRow
        Call Me.InsertRows({row}, {data})
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="row">删除行行号</param>
    Public Sub RemoveRow(row As Integer) Implements IModel.RemoveRow
        Me.RemoveRows({row})
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="startRow">起始行号</param>
    ''' <param name="rowCount">删除行数</param>
    Public Overloads Sub RemoveRows(startRow As Integer, rowCount As Integer) Implements IModel.RemoveRows
        Me.RemoveRows(Util.Range(startRow, startRow + rowCount))
    End Sub

    Public Overloads Sub RemoveRows(rows As Integer()) Implements IModelCore.RemoveRows
        Call Me._ModelCore.RemoveRows(rows)
    End Sub

    Public Sub RemoveSelectedRows() Implements IModel.RemoveSelectedRows
        If Me.AllSelectionRanges Is Nothing Then Return
        Dim removeRows As New List(Of Integer)
        For Each range In Me.AllSelectionRanges
            For i = 0 To range.Rows - 1
                removeRows.Add(range.Row + i)
            Next
        Next
        Call Me.RemoveRows(removeRows.Distinct.ToArray)
    End Sub

    ''' <summary>
    ''' 更新行
    ''' </summary>
    ''' <param name="row">更新行行号</param>
    ''' <param name="data">数据</param>
    Public Sub UpdateRow(row As Integer, data As IDictionary(Of String, Object)) Implements IModel.UpdateRow
        Call Me.UpdateRows({row}, {data})
    End Sub

    ''' <summary>
    ''' 更新行
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="dataOfEachRow">对应的数据</param>

    Public Shadows Sub UpdateRows(rows As Integer(), dataOfEachRow As IDictionary(Of String, Object)()) Implements IModel.UpdateRows
        Call Me._ModelCore.UpdateRows(rows, dataOfEachRow)
    End Sub

    ''' <summary>
    ''' 更新单元格
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="columnName">列名</param>
    ''' <param name="data">更新的数据</param>
    Public Sub UpdateCell(row As Integer, columnName As String, data As Object) Implements IModel.UpdateCell
        Me.UpdateCells({row}, New String() {columnName}, New Object() {data})
    End Sub

    ''' <summary>
    ''' DataRow转字典
    ''' </summary>
    ''' <param name="dataRow">DataRow对象</param>
    ''' <returns>转换结果</returns>
    Protected Function DataRowToDictionary(dataRow As DataRow) As IDictionary(Of String, Object) Implements IModel.DataRowToDictionary
        Dim result As New Dictionary(Of String, Object)
        Dim columns = dataRow.Table.Columns
        For Each column As DataColumn In columns
            result.Add(column.ColumnName, If(dataRow(column) Is DBNull.Value, Nothing, dataRow(column)))
        Next
        Return result
    End Function

    ''' <summary>
    ''' 更新行状态
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="state">同步状态</param>
    Public Sub UpdateRowState(row As Integer, state As ModelRowState) Implements IModel.UpdateRowState
        Call Me.UpdateRowStates({row}, {state})
    End Sub

    ''' <summary>
    ''' 获取行状态
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <returns>状态</returns>
    Public Function GetRowState(row As Integer) As ModelRowState Implements IModel.GetRowState
        Return Me.GetRowStates({row})(0)
    End Function

    Public Function ContainsColumn(columnName As String) As Boolean Implements IModel.ContainsColumn
        Return Me.GetColumns({columnName})(0) IsNot Nothing
    End Function

    Public Sub SelectRowsByValues(Of T)(columnName As String, values As T()) Implements IModel.SelectRowsByValues
        If values Is Nothing Then
            Me.AllSelectionRanges = {}
            Return
        End If
        Dim dataRows = Me._ModelCore.GetRows(Util.Range(0, Me._ModelCore.GetRowCount))
        Dim targetRows As New List(Of Integer)
        For i = 0 To dataRows.Count - 1
            Dim curRowValue = dataRows(i)(columnName)
            If values.Contains(curRowValue) Then
                targetRows.Add(i)
            End If
        Next
        '对目标行号分组
        Dim rowGroups As New List(Of List(Of Integer))
        For Each row In targetRows
            Dim lastGroup As List(Of Integer)
            If rowGroups.Count = 0 Then
                lastGroup = New List(Of Integer)
                rowGroups.Add(lastGroup)
            Else
                lastGroup = rowGroups.Last
            End If
            If lastGroup.Count = 0 OrElse lastGroup.Last + 1 = row Then
                lastGroup.Add(row)
            Else
                rowGroups.Add(New List(Of Integer)({row}))
            End If
        Next
        '生成选区
        Dim ranges As New List(Of Range)
        For Each rowGroup In rowGroups
            Dim newRange = New Range(rowGroup(0), 0, rowGroup.Count, Me._ModelCore.GetColumnCount)
            ranges.Add(newRange)
        Next
        Me.AllSelectionRanges = ranges.ToArray
    End Sub

    Private Function DictionaryToObject(Of T As New)(dic As IDictionary(Of String, Object)) As T
        Dim result As New T
        Dim type = GetType(T)
        For Each entry In dic
            Dim key = entry.Key
            Dim prop = type.GetProperty(key, BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.IgnoreCase)
            '如果找到了相应属性，优先为属性映射值
            If prop IsNot Nothing Then
                Dim value As Object = Nothing
                Try
                    value = Convert.ChangeType(entry.Value, prop.PropertyType)
                Catch ex As Exception
                    Throw New FrontWorkException($"Value {entry.Value} of ""{key}"" cannot be converted to {prop.PropertyType.Name} for {type.Name}.{prop.Name}")
                End Try
                prop.SetValue(result, value, Nothing)
                Continue For
            End If
            '否则尝试寻找相应字段并赋值
            Dim field = type.GetField(key, BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.IgnoreCase)
            If field IsNot Nothing Then
                Dim value As Object = Nothing
                If entry.Value Is Nothing Then
                    Continue For
                Else
                    Try
                        value = Convert.ChangeType(entry.Value, field.FieldType)
                    Catch ex As Exception
                        Throw New FrontWorkException($"Value ""{entry.Value}"" of ""{key}"" cannot be converted to {field.FieldType.Name} for {type.Name}.{field.Name}")
                    End Try
                    field.SetValue(result, value)
                End If
            End If
        Next
        Return result
    End Function

    ''' <summary>
    ''' 获取所有选中行
    ''' </summary>
    ''' <typeparam name="T">要映射成的类型</typeparam>
    ''' <returns>选中行映射后的对象数组</returns>
    Public Function GetSelectedRows(Of T As New)() As T() Implements IModel.GetSelectedRows
        If Me.AllSelectionRanges.Length = 0 Then Return {}
        Dim selectedRows As New List(Of Integer)
        For Each curSelectionRange In Me.AllSelectionRanges
            Dim row = curSelectionRange.Row
            Dim rows = curSelectionRange.Rows
            For i = 0 To rows - 1
                Dim curRow = row + i
                selectedRows.Add(curRow)
            Next
        Next
        Return Me.GetRows(Of T)(selectedRows.ToArray)
    End Function

    Public Function GetSelectedRows() As IDictionary(Of String, Object)() Implements IModel.GetSelectedRows
        If Me.AllSelectionRanges.Length = 0 Then Return {}
        Dim selectedRows As New List(Of Integer)
        For Each curSelectionRange In Me.AllSelectionRanges
            Dim row = curSelectionRange.Row
            Dim rows = curSelectionRange.Rows
            For i = 0 To rows - 1
                Dim curRow = row + i
                selectedRows.Add(curRow)
            Next
        Next
        Return Me.GetRows(selectedRows.ToArray)
    End Function

    ''' <summary>
    ''' 获取所有选中行的某一列
    ''' </summary>
    ''' <typeparam name="T">返回类型</typeparam>
    ''' <param name="columnName">列名</param>
    ''' <returns>所有选中行指定列的数据</returns>
    Public Function GetSelectedRows(Of T)(columnName As String) As T() Implements IModel.GetSelectedRows
        If Me.AllSelectionRanges.Length = 0 Then Return {}
        Dim selectedRows As New List(Of Integer)
        For Each curSelectionRange In Me.AllSelectionRanges
            Dim row = curSelectionRange.Row
            Dim rows = curSelectionRange.Rows
            For i = 0 To rows - 1
                Dim curRow = row + i
                selectedRows.Add(curRow)
            Next
        Next
        Dim rowData = Me.GetRows(selectedRows.ToArray)
        Dim result As New List(Of T)
        For Each curRowData In rowData
            If Not curRowData.ContainsKey(columnName) Then
                Throw New FrontWorkException($"{Me.Name} doesn't contains column ""{columnName}""!")
            End If
            result.Add(curRowData(columnName))
        Next
        Return result.ToArray
    End Function

    Public Function GetSelectedRow() As IDictionary(Of String, Object) Implements IModel.GetSelectedRow
        Dim selectedData = Me.GetSelectedRows()
        If selectedData.Length > 0 Then
            Return selectedData(0)
        Else
            Return Nothing
        End If
    End Function

    Public Function GetSelectedRow(Of T As New)() As IDictionary(Of String, Object) Implements IModel.GetSelectedRow
        Dim selectedData() As T = Me.GetSelectedRows(Of T)
        If selectedData.Length > 0 Then
            Return selectedData(0)
        Else
            Return Nothing
        End If
    End Function

    Public Function GetSelectedRow(Of T)(columnName As String) As T Implements IModel.GetSelectedRow
        Dim selectedData = Me.GetSelectedRows(Of T)(columnName)
        If selectedData.Length > 0 Then
            Return selectedData(0)
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' 删除新增但未编辑的行
    ''' </summary>
    Public Sub RemoveUneditedNewRows() Implements IModel.RemoveUneditedNewRows
        Dim rows As New List(Of Integer)
        For i = 0 To Me.GetRowCount - 1
            If Me.GetRowState(i).SynchronizationState = SynchronizationState.ADDED Then
                Call rows.Add(i)
            End If
        Next
        Call Me.RemoveRows(rows.ToArray)
    End Sub

    Public Function HasUnsynchronizedUpdatedRow() As Boolean Implements IModel.HasUnsynchronizedUpdatedRow
        Return Me._ModelCore.GetInfo(ModelInfo.HAS_UNSYNCHRONIZED_ROW)
    End Function

    Public Sub RefreshView(rows As Integer()) Implements IModel.RefreshView
        Dim args = New ModelRowUpdatedEventArgs() With {
            .UpdatedRows = (From row In rows Select New ModelRowInfo(row, Me(row), Me.GetRowState(row))).ToArray
        }
        Call Me.RaiseRowUpdatedEvent(Me, args)
    End Sub

    Public Sub RefreshView(row As Integer) Implements IModel.RefreshView
        Call Me.RefreshView({row})
    End Sub

    Public Function GetColumnCount() As Integer Implements IModel.GetColumnCount
        Return Me._ModelCore.GetColumnCount
    End Function

    Public Function GetRowCount() As Integer Implements IModel.GetRowCount
        Return Me._ModelCore.GetRowCount
    End Function

    Public Function GetSelectionRanges() As Range() Implements IModel.GetSelectionRanges
        Return Me._ModelCore.GetSelectionRanges
    End Function

    Public Sub SetSelectionRanges(ranges As Range()) Implements IModel.SetSelectionRanges
        Call Me._ModelCore.SetSelectionRanges(ranges)
    End Sub

    ''' <summary>
    ''' 获取行
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <returns>相应行数据</returns>
    Public Overloads Function GetRows(rows As Integer()) As IDictionary(Of String, Object)() Implements IModel.GetRows
        Return Me._ModelCore.GetRows(rows)
    End Function

    ''' <summary>
    ''' 增加行
    ''' </summary>
    ''' <param name="dataOfEachRow">增加行的数据</param>
    ''' <returns>增加的行号</returns>
    Public Function AddRows(dataOfEachRow As IDictionary(Of String, Object)()) As Integer() Implements IModel.AddRows
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
    Public Sub InsertRows(rows As Integer(), dataOfEachRow As IDictionary(Of String, Object)()) Implements IModel.InsertRows
        Call Me._ModelCore.InsertRows(rows, dataOfEachRow)
    End Sub

    ''' <summary>
    ''' 更新单元格
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="columnNames">列名</param>
    ''' <param name="dataOfEachCell">相应的数据</param>
    Public Sub UpdateCells(rows As Integer(), columnNames As String(), dataOfEachCell As Object()) Implements IModel.UpdateCells
        Call Me._ModelCore.UpdateCells(rows, columnNames, dataOfEachCell)
    End Sub

    Public Sub AddColumns(columns() As ModelColumn) Implements IModel.AddColumns
        Call Me._ModelCore.AddColumns(columns)
    End Sub

    Public Sub RemoveColumns(indexes() As Integer) Implements IModel.RemoveColumns
        Call Me._ModelCore.RemoveColumns(indexes)
    End Sub

    Public Function GetColumns() As ModelColumn() Implements IModel.GetColumns
        Return Me._ModelCore.GetColumns
    End Function

    Public Function GetColumns(columnNames() As String) As ModelColumn() Implements IModel.GetColumns
        Return Me._ModelCore.GetColumns(columnNames)
    End Function

    Public Function GetCells(rows() As Integer, columnNames() As String) As Object() Implements IModel.GetCells
        Return Me._ModelCore.GetCells(rows, columnNames)
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

    Public Sub RaiseRowStateChangedEvent(sender As Object, args As ModelRowStateChangedEventArgs)
        RaiseEvent RowStateChanged(sender, args)
    End Sub

    Public Sub UpdateRowStates(rows() As Integer, states() As ModelRowState) Implements IModel.UpdateRowStates
        Me._ModelCore.UpdateRowStates(rows, states)
    End Sub

    Public Function GetRowStates(rows() As Integer) As ModelRowState() Implements IModel.GetRowStates
        Return Me._ModelCore.GetRowStates(rows)
    End Function

    Public Sub Refresh(args As ModelRefreshArgs) Implements IModel.Refresh
        Me._ModelCore.Refresh(args)
    End Sub

    Public Sub UpdateColumn(indexes() As Integer, columns() As ModelColumn) Implements IModel.UpdateColumn
        Me._ModelCore.UpdateColumn(indexes, columns)
    End Sub

    Public Property AllSelectionRanges As Range() Implements IModel.AllSelectionRanges
        Get
            Return Me.GetSelectionRanges
        End Get
        Set(value As Range())
            Me.SetSelectionRanges(value)
        End Set
    End Property

    Public Property AllSelectionRanges(i As Integer) As Range Implements IModel.AllSelectionRanges
        Get
            Return Me.GetSelectionRanges(i)
        End Get
        Set(value As Range)
            Dim allSelectionRanges = Me.GetSelectionRanges
            allSelectionRanges(i) = value
            Me.SetSelectionRanges(allSelectionRanges)
        End Set
    End Property

    Public Property SelectionRange As Range Implements IModel.SelectionRange
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

    Public Function GetRowSynchronizationStates(rows As Integer()) As SynchronizationState() Implements IModel.GetRowSynchronizationStates
        Dim rowStates = Me.GetRowStates(rows)
        Return (From s In rowStates Select s.SynchronizationState).ToArray
    End Function

    Public Function GetRowSynchronizationState(row As Integer) As SynchronizationState Implements IModel.GetRowSynchronizationState
        Return Me.GetRowSynchronizationStates({row})(0)
    End Function

    Public Function GetCellState(row As Integer, field As String) As ModelCellState Implements IModel.GetCellState
        Return Me._ModelCore.GetCellStates({row}, {field})(0)
    End Function

    Public Sub UpdateCellState(row As Integer, field As String, states As ModelCellState) Implements IModel.UpdateCellState
        Call Me._ModelCore.UpdateCellStates({row}, {field}, {states})
    End Sub

    Public Function GetCellStates(rows() As Integer, fields() As String) As ModelCellState() Implements IModelCore.GetCellStates
        Return Me._ModelCore.GetCellStates(rows, fields)
    End Function

    Public Sub UpdateCellStates(rows() As Integer, fields() As String, states() As ModelCellState) Implements IModelCore.UpdateCellStates
        Call Me._ModelCore.UpdateCellStates(rows, fields, states)
    End Sub

    Public Function HasErrorCell() As Boolean Implements IModel.HasErrorCell
        Return Me._ModelCore.GetInfo(ModelInfo.HAS_ERROR_CELL)
    End Function

    Public Function HasWarningCell() As Boolean Implements IModel.HasWarningCell
        Return Me._ModelCore.GetInfo(ModelInfo.HAS_WARNING_CELL)
    End Function

    Public Function GetInfo(infoItem As ModelInfo) As Object Implements IModelCore.GetInfo
        Return Me._ModelCore.GetInfo(infoItem)
    End Function

    Public Sub UpdateCellValidationStates(rows() As Integer, fields() As String, validationStates() As ValidationState) Implements IModel.UpdateCellValidationStates
        Dim states = Me.ModelCore.GetCellStates(rows, fields)
        For i = 0 To states.Length - 1
            states(i).ValidationState = validationStates(i)
        Next
        Call Me.ModelCore.UpdateCellStates(rows, fields, states)
    End Sub

    Public Sub UpdateCellValidationState(row As Integer, field As String, state As ValidationState) Implements IModel.UpdateCellValidationState
        Call Me.UpdateCellValidationStates({row}, {field}, {state})
    End Sub

    Public Sub InsertRows(row As Integer, count As Integer, data() As IDictionary(Of String, Object)) Implements IModel.InsertRows
        Call Me.ModelCore.InsertRows(Util.Range(row, row + count), data)
    End Sub
End Class

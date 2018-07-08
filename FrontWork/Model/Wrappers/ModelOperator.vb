Imports FrontWork
Imports System.Linq
Imports System.Reflection

Public Class ModelOperator
    Inherits ModelWrapperBase
    Implements IModel

    Public Overrides Property Model As IModel
        Get
            Return MyBase.Model
        End Get
        Set(value As IModel)
            If MyBase.Model IsNot Nothing Then
                Call Me.UnbindModelCore(MyBase.Model)
            End If
            MyBase.Model = value
            If MyBase.Model IsNot Nothing Then
                Call Me.BindModelCore(MyBase.Model)
            End If
        End Set
    End Property


    ''' <summary>
    ''' 数据行数
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property RowCount As Integer
        Get
            Return Me.GetRowCount
        End Get
    End Property

    Public ReadOnly Property ColumnCount As Integer
        Get
            Return Me.GetColumnCount
        End Get
    End Property

    Public Sub New()

    End Sub

    Public Sub New(modelCore As IModel)
        Me.Model = modelCore
    End Sub


    Private Sub BindModelCore(modelCore As IModel)
        AddHandler Me.Model.CellUpdated, AddressOf Me.RaiseCellUpdatedEvent
        AddHandler Me.Model.RowUpdated, AddressOf Me.RaiseRowUpdatedEvent
        AddHandler Me.Model.RowAdded, AddressOf Me.RaiseRowAddedEvent
        AddHandler Me.Model.BeforeRowRemove, AddressOf Me.RaiseBeforeRowRemoveEvent
        AddHandler Me.Model.RowRemoved, AddressOf Me.RaiseRowRemovedEvent
        AddHandler Me.Model.SelectionRangeChanged, AddressOf Me.RaiseSelectionRangeChangedEvent
        AddHandler Me.Model.Refreshed, AddressOf Me.RaiseRefreshedEvent
        AddHandler Me.Model.RowStateChanged, AddressOf Me.RaiseRowStateChangedEvent
    End Sub

    Private Sub UnbindModelCore(modelCore As IModel)
        RemoveHandler Me.Model.CellUpdated, AddressOf Me.RaiseCellUpdatedEvent
        RemoveHandler Me.Model.RowUpdated, AddressOf Me.RaiseRowUpdatedEvent
        RemoveHandler Me.Model.RowAdded, AddressOf Me.RaiseRowAddedEvent
        RemoveHandler Me.Model.BeforeRowRemove, AddressOf Me.RaiseBeforeRowRemoveEvent
        RemoveHandler Me.Model.RowRemoved, AddressOf Me.RaiseRowRemovedEvent
        RemoveHandler Me.Model.SelectionRangeChanged, AddressOf Me.RaiseSelectionRangeChangedEvent
        RemoveHandler Me.Model.Refreshed, AddressOf Me.RaiseRefreshedEvent
        RemoveHandler Me.Model.RowStateChanged, AddressOf Me.RaiseRowStateChangedEvent
    End Sub

    Public Property Name As String


    ''' <summary>
    ''' 选区
    ''' </summary>
    ''' <returns></returns>
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
        Return Me.Model.GetCells({row}, {columnName})(0)
    End Function


    ''' <summary>
    ''' 获取行并自动转换成相应类型的对象返回
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="rows"></param>
    ''' <returns></returns>
    Public Overloads Function GetRows(Of T As New)(rows As Integer()) As T()
        Dim rowData = Me.GetRows(rows)
        Dim result(rows.Length - 1) As T
        For i = 0 To result.Length - 1
            result(i) = Me.DictionaryToObject(Of T)(rowData(i))
        Next
        Return result
    End Function

    Public Function GetRow(Of T As New)(row As Integer) As T
        Return Me.GetRows(Of T)({row})(0)
    End Function

    ''' <summary>
    ''' 获取行
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <returns>相应行数据</returns>
    Public Function GetRow(row As Integer) As IDictionary(Of String, Object)
        Return MyBase.GetRows({row})(0)
    End Function

    ''' <summary>
    ''' 增加行
    ''' </summary>
    ''' <param name="data">增加行的数据</param>
    ''' <returns>增加的行号</returns>
    Public Function AddRow(data As IDictionary(Of String, Object)) As Integer
        Return Me.AddRows({data})(0)
    End Function


    ''' <summary>
    ''' 插入行
    ''' </summary>
    ''' <param name="row">插入行行号</param>
    ''' <param name="data">数据</param>
    Public Sub InsertRow(row As Integer, data As IDictionary(Of String, Object))
        Call Me.InsertRows({row}, {data})
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="row">删除行行号</param>
    Public Sub RemoveRow(row As Integer)
        MyBase.RemoveRows({row})
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="startRow">起始行号</param>
    ''' <param name="rowCount">删除行数</param>
    Public Overloads Sub RemoveRows(startRow As Integer, rowCount As Integer)
        MyBase.RemoveRows(Util.Range(startRow, startRow + rowCount))
    End Sub

    Public Sub RemoveSelectedRows()
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
    Public Sub UpdateRow(row As Integer, data As IDictionary(Of String, Object))
        Call Me.UpdateRows(
            New Integer() {row},
            New Dictionary(Of String, Object)() {data}
        )
    End Sub

    ''' <summary>
    ''' 更新行
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="dataOfEachRow">对应的数据</param>

    Public Shadows Sub UpdateRows(rows As Integer(), dataOfEachRow As IDictionary(Of String, Object)()) Implements IModel.UpdateRows
        Call Me.Model.UpdateRows(rows, dataOfEachRow)
    End Sub

    ''' <summary>
    ''' 更新单元格
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="columnName">列名</param>
    ''' <param name="data">更新的数据</param>
    Public Sub UpdateCell(row As Integer, columnName As String, data As Object)
        MyBase.UpdateCells({row}, New String() {columnName}, New Object() {data})
    End Sub

    ''' <summary>
    ''' DataRow转字典
    ''' </summary>
    ''' <param name="dataRow">DataRow对象</param>
    ''' <returns>转换结果</returns>
    Protected Function DataRowToDictionary(dataRow As DataRow) As IDictionary(Of String, Object)
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
    Public Sub UpdateRowState(row As Integer, state As ModelRowState)
        Call MyBase.UpdateRowStates({row}, {state})
    End Sub

    ''' <summary>
    ''' 获取行状态
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <returns>状态</returns>
    Public Function GetRowState(row As Integer) As ModelRowState
        Return MyBase.GetRowStates({row})(0)
    End Function

    Public Function ContainsColumn(columnName As String) As Boolean
        Return Me.GetColumns({columnName})(0) IsNot Nothing
    End Function

    Public Sub SelectRowsByValues(Of T)(columnName As String, values As T())
        If values Is Nothing Then
            Me.AllSelectionRanges = {}
            Return
        End If
        Dim dataTable = Me.Model.ToDataTable
        Dim targetRows As New List(Of Integer)
        For i = 0 To dataTable.Rows.Count - 1
            Dim curRowValue = dataTable.Rows(i)(columnName)
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
            Dim newRange = New Range(rowGroup(0), 0, rowGroup.Count, dataTable.Columns.Count)
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
    Public Function GetSelectedRows(Of T As New)() As T()
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

    Public Function GetSelectedRows() As IDictionary(Of String, Object)()
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
    Public Function GetSelectedRows(Of T)(columnName As String) As T()
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

    Public Function GetSelectedRow() As IDictionary(Of String, Object)
        Dim selectedData = Me.GetSelectedRows()
        If selectedData.Length > 0 Then
            Return selectedData(0)
        Else
            Return Nothing
        End If
    End Function

    Public Function GetSelectedRow(Of T As New)() As IDictionary(Of String, Object)
        Dim selectedData() As T = Me.GetSelectedRows(Of T)
        If selectedData.Length > 0 Then
            Return selectedData(0)
        Else
            Return Nothing
        End If
    End Function

    Public Function GetSelectedRow(Of T)(columnName As String) As T
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
    Public Sub RemoveUneditedNewRows()
        Dim rows As New List(Of Integer)
        For i = 0 To Me.GetRowCount - 1
            If Me.GetRowState(i).SynchronizationState = SynchronizationState.ADDED Then
                Call rows.Add(i)
            End If
        Next
        Call Me.RemoveRows(rows.ToArray)
    End Sub

    Public Function HasUnsynchronizedUpdatedRow() As Boolean
        For i = 0 To Me.RowCount - 1
            If {SynchronizationState.UPDATED,
                SynchronizationState.ADDED_UPDATED
               }.Contains(Me.GetRowState(i).SynchronizationState) Then
                Return True
            End If
        Next
        Return False
    End Function


    Public Sub RefreshView(rows As Integer())
        Dim args = New ModelRowUpdatedEventArgs() With {
            .UpdatedRows = (From row In rows Select New ModelRowInfo(row, Me(row), Me.GetRowState(row))).ToArray
        }
        Call MyBase.RaiseRowUpdatedEvent(Me, args)
    End Sub

    Public Sub RefreshView(row As Integer)
        Call Me.RefreshView({row})
    End Sub

End Class

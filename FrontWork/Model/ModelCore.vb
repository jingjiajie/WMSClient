Imports System.ComponentModel
Imports System.Linq
Imports System.Reflection
Imports FrontWork

''' <summary>
''' 模型类
''' </summary>
Public Class ModelCore
    Implements IModelCore

    Private _modelColumns As New List(Of ModelColumn)
    Private _allSelectionRange As Range() = New Range() {}
    Private _configuration As Configuration
    Private _dicRowGuid As New Dictionary(Of DataRow, Guid)
    Private _mode As String = "default"
    Private _dicRowSyncState As New Dictionary(Of DataRow, SynchronizationState)

    ''' <summary>
    ''' 数据表
    ''' </summary>
    ''' <returns></returns>
    Private Property Data As New DataTable

    Public Sub New()

    End Sub

    Public Function GetRowCount() As Integer Implements IModelCore.GetRowCount
        Return Me.Data.Rows.Count
    End Function

    Public Function GetColumnCount() As Integer Implements IModelCore.GetColumnCount
        Return Me.Data.Columns.Count
    End Function

    Public Function GetSelectionRanges() As Range() Implements IModelCore.GetSelectionRanges
        Return Me._allSelectionRange
    End Function

    Public Sub SetSelectionRanges(ranges As Range()) Implements IModelCore.SetSelectionRanges
        If ranges Is Nothing Then
            ranges = {}
        End If
        Me._allSelectionRange = ranges
        RaiseEvent SelectionRangeChanged(Me, New ModelSelectionRangeChangedEventArgs() With {
                                     .NewSelectionRange = ranges
                                    })
    End Sub

    Public Sub AddColumns(columns As ModelColumn()) Implements IModelCore.AddColumns
        For Each column In columns
            If Not Me.Data.Columns.Contains(column.Name) Then
                Dim newColumn As New DataColumn
                newColumn.ColumnName = column.Name
                newColumn.DataType = column.Type
                newColumn.AllowDBNull = column.Nullable
                Me.Data.Columns.Add(newColumn)
            End If
        Next
        Me._modelColumns.AddRange(columns)
    End Sub

    Public Sub RemoveColumns(columnNames As String()) Implements IModelCore.RemoveColumns
        For Each columnName In columnNames
            Me.Data.Columns.Remove(columnName)
            For i = 0 To Me._modelColumns.Count - 1
                If Me._modelColumns(i).Name.Equals(columnName) Then
                    Me._modelColumns.RemoveAt(i)
                    Exit For
                End If
            Next
        Next
    End Sub

    Public Function GetColumns() As ModelColumn() Implements IModelCore.GetColumns
        Return Me._modelColumns.ToArray
    End Function

    Public Function GetColumns(columnNames As String()) As ModelColumn() Implements IModelCore.GetColumns
        Dim result(columnNames.Length - 1) As ModelColumn
        For i = 0 To columnNames.Length - 1
            Dim columnName = columnNames(i)
            For Each modelColumn In Me._modelColumns
                If modelColumn.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase) Then
                    result(i) = modelColumn
                    Exit For
                End If
            Next
        Next
        Return result
    End Function

    ''' <summary>
    ''' 获取行
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <returns>相应行数据</returns>
    Public Function GetRows(rows As Integer()) As IDictionary(Of String, Object)() Implements IModelCore.GetRows
        Dim result As New List(Of IDictionary(Of String, Object))
        Try
            For Each row In rows
                result.Add(Me.DataRowToDictionary(Me.Data.Rows(row)))
            Next
        Catch ex As Exception
            Throw New FrontWorkException("GetRows failed: " & ex.Message)
        End Try

        Return result.ToArray
    End Function

    Public Function GetCells(rows As Integer(), columnNames As String()) As Object() Implements IModelCore.GetCells
        If rows.Length <> columnNames.Length Then
            Throw New FrontWorkException($"Count of rows({rows.Length}) must be equal to columnNames({columnNames.Length})")
        End If
        Dim result(rows.Length - 1) As Object
        For i = 0 To rows.Length - 1
            Dim row = rows(i)
            Dim columnName = columnNames(i)
            If row >= Me.Data.Rows.Count Then
                Throw New FrontWorkException($"Row:{row} exceeded the max row of Model({Me.Data.Rows.Count - 1})")
            End If
            If Not Me.Data.Columns.Contains(columnName) Then
                Throw New FrontWorkException($"Model doesn't contain column:""{columnName}""")
            End If
            Dim data As Object = Me.Data.Rows(row)(columnName)
            If IsDBNull(data) Then
                result(i) = Nothing
            Else
                result(i) = data
            End If
        Next
        Return result
    End Function

    ''' <summary>
    ''' 增加行
    ''' </summary>
    ''' <param name="dataOfEachRow">增加行的数据</param>
    ''' <returns>增加的行号</returns>
    Public Function AddRows(dataOfEachRow As IDictionary(Of String, Object)()) As Integer() Implements IModelCore.AddRows
        Dim addRowCount = dataOfEachRow.Length
        Dim oriRowCount = Me.Data.Rows.Count
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
        'TODO 把行号升序排列之后数据并没有跟着升序排列，导致bug
        If dataOfEachRow Is Nothing Then
            dataOfEachRow = Util.Times(Of IDictionary(Of String, Object))(Nothing, rows.Length)
        End If
        Dim indexRowPairs As New List(Of ModelRowInfo)
        Dim oriRowCount = Me.Data.Rows.Count
        '原始行每次插入之后，行号会变，所以做调整
        Dim realRowsASC = (From r In rows Order By r Ascending Select r).ToArray
        For i = 0 To realRowsASC.Length - 1
            realRowsASC(i) = realRowsASC(i) + System.Math.Min(oriRowCount, i)
        Next
        '开始添加数据
        For i = 0 To realRowsASC.Length - 1
            Dim realRow = realRowsASC(i)
            Dim curData = If(dataOfEachRow(i), New Dictionary(Of String, Object))
            Dim newRow = Me.Data.NewRow
            '置入默认值
            For Each curColumn In Me._modelColumns
                If curColumn.DefaultValue Is Nothing Then Continue For
                Dim fieldName = curColumn.Name
                If Not curData.ContainsKey(fieldName) Then curData.Add(fieldName, Nothing)
                If curData(fieldName) Is Nothing Then
                    curData(fieldName) = curColumn.DefaultValue.Invoke
                End If
            Next
            '将值写入datatable
            For Each item In curData
                If Not Me.Data.Columns.Contains(item.Key) Then Continue For
                newRow(item.Key) = If(item.Value, DBNull.Value)
            Next
            Me.Data.Rows.InsertAt(newRow, realRow)
            Dim newIndexRowPair As New ModelRowInfo(realRow, Me.GetRowID(Me.Data.Rows(realRow)), If(curData, New Dictionary(Of String, Object)), Me.GetRowSynchronizationStates({realRow})(0))
            indexRowPairs.Add(newIndexRowPair)
        Next

        RaiseEvent RowAdded(Me, New ModelRowAddedEventArgs() With {
                             .AddedRows = indexRowPairs.ToArray
                            })

        Dim columnCount = Me.Data.Columns.Count
        Dim selectionRanges As New List(Of Range)
        For Each curRow In realRowsASC
            If selectionRanges.Count = 0 OrElse selectionRanges.Last.Row + 1 <> curRow Then
                selectionRanges.Add(New Range(curRow, 0, 1, columnCount))
                Continue For
            End If
            If selectionRanges.Last.Row + 1 = curRow Then
                Dim lastRange = selectionRanges.Last
                selectionRanges(selectionRanges.Count - 1) = New Range(lastRange.Row, lastRange.Column, lastRange.Rows + 1, lastRange.Columns)
            End If
        Next
        Me.SetSelectionRanges(selectionRanges.ToArray)

        Me.UpdateRowSynchronizationStates(realRowsASC, Util.Times(SynchronizationState.ADDED, realRowsASC.Length))
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="rows">删除行行号</param>
    Public Sub RemoveRows(rows As Integer()) Implements IModelCore.RemoveRows
        If rows.Length = 0 Then Return
        Dim indexRowList = New List(Of ModelRowInfo)
        Try
            '每次删除行后行号会变，所以要做调整
            Dim rowsASC = (From r In rows
                           Order By r Ascending
                           Select r).ToArray
            Dim realRows(rowsASC.Length - 1) As Integer
            For i = 0 To rowsASC.Length - 1
                realRows(i) = rowsASC(i) - i
            Next
            '触发事件时的行按传入的行号，和行ID。
            For Each curRowNum In rowsASC
                Dim newIndexRowPair = New ModelRowInfo(curRowNum,
                                                  Me.GetRowID(Me.Data.Rows(curRowNum)),
                                                  Me.DataRowToDictionary(Me.Data.Rows(curRowNum)),
                                                  Me.GetRowSynchronizationState(Me.Data.Rows(curRowNum)))
                indexRowList.Add(newIndexRowPair)
            Next
            Dim beforeRowRemoveEventArgs As New ModelBeforeRowRemoveEventArgs With {
                .RemoveRows = indexRowList.ToArray
            }
            RaiseEvent BeforeRowRemove(Me, beforeRowRemoveEventArgs)
            If beforeRowRemoveEventArgs.Cancel Then Return
            For Each curRowNum In realRows
                If Me._dicRowGuid.ContainsKey(Me.Data.Rows(curRowNum)) Then
                    Me._dicRowGuid.Remove(Me.Data.Rows(curRowNum))
                End If
                Me.Data.Rows.RemoveAt(curRowNum)
            Next
        Catch ex As Exception
            Throw New FrontWorkException("RemoveRows failed: " & ex.Message)
        End Try
        RaiseEvent RowRemoved(Me, New ModelRowRemovedEventArgs() With {
                                        .RemovedRows = indexRowList.ToArray
                                   })
        If Me.Data.Rows.Count = 0 Then
            Me.SetSelectionRanges({})
        Else
            Me.SetSelectionRanges({New Range(Math.Min(rows.Min, Me.Data.Rows.Count - 1), 0, 1, Me.Data.Columns.Count)})
        End If
    End Sub

    ''' <summary>
    ''' 更新行
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="dataOfEachRow">对应的数据</param>
    Public Sub UpdateRows(rows As Integer(), dataOfEachRow As IDictionary(Of String, Object)()) Implements IModelCore.UpdateRows
        Dim rowSyncStatePairs As New List(Of RowSynchronizationStatePair)
        Try
            Dim i = 0
            For Each row In rows
                For Each item In dataOfEachRow(i)
                    Dim key = item.Key
                    Dim value = item.Value
                    If value Is Nothing Then
                        Me.Data.Rows(row)(key) = DBNull.Value
                    Else
                        Dim colType = Me.Data.Columns(key).DataType
                        If colType = value.GetType Then
                            Me.Data.Rows(row)(key) = value
                        Else
                            Try
                                Convert.ChangeType(value, colType)
                            Catch ex As Exception
                                Throw New InvalidDataException($"""{value}""不是有效的格式")
                            End Try
                        End If
                    End If
                Next
                '将被更新的行的同步状态修改为已更新或已添加
                Dim curState = Me.GetRowSynchronizationStates({row})(0)
                If curState = SynchronizationState.ADDED Then
                    rowSyncStatePairs.Add(New RowSynchronizationStatePair(row, SynchronizationState.ADDED_UPDATED))
                ElseIf curState = SynchronizationState.SYNCHRONIZED Then
                    rowSyncStatePairs.Add(New RowSynchronizationStatePair(row, SynchronizationState.UPDATED))
                End If
                i += 1
            Next

            Dim updatedRows(rows.Length - 1) As ModelRowInfo
            For i = 0 To rows.Length - 1
                updatedRows(i) = New ModelRowInfo(rows(i), Me.GetRowID(Me.Data.Rows(rows(i))), Me.DataRowToDictionary(Me.Data.Rows(rows(i))), Me.GetRowSynchronizationState(Me.Data.Rows(rows(i))))
            Next

            Dim eventArgs = New ModelRowUpdatedEventArgs() With {
                                        .UpdatedRows = updatedRows
                                   }

            RaiseEvent RowUpdated(Me, eventArgs)
            Call Me.UpdateRowSynchronizationStates(rowSyncStatePairs.Select(Function(pair)
                                                                                Return CInt(pair.Row)
                                                                            End Function).ToArray,
                                                   rowSyncStatePairs.Select(Function(pair)
                                                                                Return pair.SynchronizationState
                                                                            End Function).ToArray)
        Catch ex As Exception
            Throw New FrontWorkException("UpdateRows failed: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 更新单元格
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="columnNames">列名</param>
    ''' <param name="dataOfEachCell">相应的数据</param>
    Public Sub UpdateCells(rows As Integer(), columnNames As String(), dataOfEachCell As Object()) Implements IModelCore.UpdateCells
        Dim posCellPairs As New List(Of ModelCellInfo)
        Dim rowSyncStatePairs As New List(Of RowSynchronizationStatePair)
        For i = 0 To rows.Length - 1
            Dim row = rows(i)
            Dim columnName = columnNames(i)
            Dim dataColumn = (From col As DataColumn In Me.Data.Columns
                              Where col.ColumnName.Equals(columnName, StringComparison.OrdinalIgnoreCase)
                              Select col).FirstOrDefault
            If dataColumn Is Nothing Then
                Throw New FrontWorkException($"UpdateCells failed: column ""{columnName}"" not found!")
            End If
            Try
                Me.Data.Rows(rows(i))(dataColumn) = If(dataOfEachCell(i), DBNull.Value)
            Catch ex As ArgumentException
                Me.Data.Rows(rows(i))(dataColumn) = DBNull.Value
                Throw New InvalidDataException($"""{dataOfEachCell(i)}""不是有效的格式")
            End Try
            posCellPairs.Add(New ModelCellInfo(rows(i), Me.GetRowID(Me.Data.Rows(rows(i))), columnName, dataOfEachCell(i)))
            Dim curState = Me.GetRowSynchronizationState(Me.Data.Rows(rows(i)))
            Select Case curState
                Case SynchronizationState.ADDED
                    rowSyncStatePairs.Add(New RowSynchronizationStatePair(row, SynchronizationState.ADDED_UPDATED))
                Case SynchronizationState.SYNCHRONIZED
                    rowSyncStatePairs.Add(New RowSynchronizationStatePair(row, SynchronizationState.UPDATED))
            End Select
        Next

        RaiseEvent CellUpdated(Me, New ModelCellUpdatedEventArgs() With {
                                    .UpdatedCells = posCellPairs.ToArray
                               })
        Me.UpdateRowSynchronizationStates(
            rowSyncStatePairs.Select(Function(pair)
                                         Return CInt(pair.Row)
                                     End Function).ToArray,
            rowSyncStatePairs.Select(Function(pair)
                                         Return pair.SynchronizationState
                                     End Function).ToArray)
    End Sub

    ''' <summary>
    ''' 刷新Model
    ''' </summary>
    ''' <param name="dataTable">数据表</param>
    ''' <param name="ranges">选区</param>
    ''' <param name="syncStates">各行同步状态</param>
    Public Overloads Sub Refresh(dataTable As DataTable, ranges As Range(), syncStates As SynchronizationState()) Implements IModelCore.Refresh
        '刷新选区
        Me._allSelectionRange = If(ranges, {})
        '刷新数据
        Me._Data = dataTable
        '刷新同步状态字典
        Call Me._dicRowSyncState.Clear()
        If syncStates IsNot Nothing Then
            For i = 0 To syncStates.Length - 1
                If dataTable.Rows.Count <= i Then
                    Throw New FrontWorkException("Length of syncStates exceeded the max row of dataTable")
                End If
                Dim row = dataTable.Rows(i)
                Dim syncState = syncStates(i)
                Me._dicRowSyncState.Add(row, syncState)
            Next
        End If
        '触发刷新事件
        RaiseEvent Refreshed(Me, New ModelRefreshedEventArgs)
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

    Private Function GetRowID(row As DataRow) As Guid
        If Not Me._dicRowGuid.ContainsKey(row) Then
            Me._dicRowGuid.Add(row, Guid.NewGuid)
        End If
        Return Me._dicRowGuid(row)
    End Function

    ''' <summary>
    ''' 获取行ID
    ''' </summary>
    ''' <param name="rowNums">行号</param>
    ''' <returns>行ID</returns>
    Public Function GetRowIDs(rowNums As Integer()) As Guid() Implements IModelCore.GetRowIDs
        Dim dataRows(rowNums.Length - 1) As DataRow
        Dim rowIDs(rowNums.Length - 1) As Guid
        For i = 0 To rowNums.Length - 1
            Dim rowNum = rowNums(i)
            If Me.Data.Rows.Count <= rowNum Then
                Throw New FrontWorkException($"Row {rowNum} exceeded the max row of model")
            End If
            Dim dataRow = Me.Data.Rows(rowNum)
            rowIDs(i) = Me.GetRowID(dataRow)
        Next
        Return rowIDs
    End Function

    Public Function GetRowIndexes(rowIDs As Guid()) As Integer() Implements IModelCore.GetRowIndexes
        Dim results As New List(Of Integer)
        For Each rowID In rowIDs
            Dim dataRow = (From rg In Me._dicRowGuid Where rg.Value = rowID Select rg.Key).FirstOrDefault
            If dataRow Is Nothing Then
                results.Add(-1)
            Else
                results.Add(Me.Data.Rows.IndexOf(dataRow))
            End If
        Next
        Return results.ToArray
    End Function

    Protected Function GetDataRow(rowID As Guid) As DataRow
        Return (From rowGuid In Me._dicRowGuid Where rowGuid.Value = rowID Select rowGuid.Key).FirstOrDefault
    End Function

    ''' <summary>
    ''' 更新行同步状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="syncStates">同步状态</param>
    Public Sub UpdateRowSynchronizationStates(rows As Integer(), syncStates As SynchronizationState()) Implements IModelCore.UpdateRowSynchronizationStates
        If rows.Length <> syncStates.Length Then
            Throw New FrontWorkException("Length of rows must be same of the length of syncStates")
        End If
        Dim updatedRows = New List(Of ModelRowInfo)
        For i = 0 To rows.Length - 1
            Dim row = rows(i)
            Dim syncState = syncStates(i)
            If row >= Me.Data.Rows.Count Then
                Throw New FrontWorkException($"Row {row} exceeded the max row of model")
            End If
            Me.SetRowSynchronizationState(Me.Data.Rows(row), syncState)
            Dim newIndexRowSynchronizationStatePair = New ModelRowInfo(row, Me.GetRowIDs({row})(0), Me.GetRows({row})(0), syncState)
            updatedRows.Add(newIndexRowSynchronizationStatePair)
        Next
        Dim eventArgs = New ModelRowSynchronizationStateChangedEventArgs(updatedRows.ToArray)
        RaiseEvent RowSynchronizationStateChanged(Me, eventArgs)
    End Sub


    Private Sub SetRowSynchronizationState(row As DataRow, state As SynchronizationState)
        If Me._dicRowSyncState.ContainsKey(row) Then
            Me._dicRowSyncState(row) = state
        Else
            Me._dicRowSyncState.Add(row, state)
        End If
    End Sub

    Private Function GetRowSynchronizationState(row As DataRow) As SynchronizationState
        If Not Me._dicRowSyncState.ContainsKey(row) Then
            Me._dicRowSyncState.Add(row, SynchronizationState.SYNCHRONIZED)
        End If
        Return Me._dicRowSyncState(row)
    End Function

    ''' <summary>
    ''' 获取行同步状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <returns>同步状态</returns>
    Public Function GetRowSynchronizationStates(rows As Integer()) As SynchronizationState() Implements IModelCore.GetRowSynchronizationStates
        Dim states(rows.Length - 1) As SynchronizationState
        For i = 0 To rows.Length - 1
            Dim rowNum = rows(i)
            If Me.Data.Rows.Count <= rowNum Then
                Throw New FrontWorkException($"Row {rowNum} exceeded max row of model!")
            End If
            Dim dataRow = Me.Data.Rows(rowNum)
            states(i) = Me.GetRowSynchronizationState(dataRow)
        Next
        Return states
    End Function

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

    Public Sub UpdateRowIDs(oriRowIDs As Guid(), newIDs As Guid()) Implements IModelCore.UpdateRowIDs
        For i = 0 To oriRowIDs.Length
            Dim row = Me.GetDataRow(oriRowIDs(i))
            Me._dicRowGuid(row) = newIDs(i)
        Next
    End Sub

    Public Function ToDataTable() As DataTable Implements IModelCore.ToDataTable
        Return Me.Data
    End Function


    Friend Structure RowSynchronizationStatePair
        Public Row As Integer
        Public SynchronizationState As SynchronizationState

        Public Sub New(row As Integer, state As SynchronizationState)
            Me.Row = row
            Me.SynchronizationState = state
        End Sub
    End Structure
End Class


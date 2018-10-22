Imports System.ComponentModel
Imports System.Linq
Imports System.Reflection
Imports FrontWork

''' <summary>
''' 模型类
''' </summary>
Public Class ModelCore
    Implements IModelCore

    Private _WarningCellCount As Integer = 0
    Private _ErrorCellCount As Integer = 0
    Private _modelColumns As New List(Of ModelColumn)
    Private _allSelectionRange As Range() = New Range() {}
    Private Property RowStates As New List(Of ModelRowState)


    ''' <summary>
    ''' 数据表
    ''' </summary>
    ''' <returns></returns>
    Private Property Data As New DataTable

    Public Event Refreshed As EventHandler(Of ModelRefreshedEventArgs) Implements IModelCore.Refreshed
    Public Event RowAdded As EventHandler(Of ModelRowAddedEventArgs) Implements IModelCore.RowAdded
    Public Event RowUpdated As EventHandler(Of ModelRowUpdatedEventArgs) Implements IModelCore.RowUpdated
    Public Event BeforeRowRemove As EventHandler(Of ModelBeforeRowRemoveEventArgs) Implements IModelCore.BeforeRowRemove
    Public Event RowRemoved As EventHandler(Of ModelRowRemovedEventArgs) Implements IModelCore.RowRemoved
    Public Event CellUpdated As EventHandler(Of ModelCellUpdatedEventArgs) Implements IModelCore.CellUpdated
    Public Event SelectionRangeChanged As EventHandler(Of ModelSelectionRangeChangedEventArgs) Implements IModelCore.SelectionRangeChanged
    Public Event RowStateChanged As EventHandler(Of ModelRowStateChangedEventArgs) Implements IModelCore.RowStateChanged
    Public Event CellStateChanged As EventHandler(Of ModelCellStateChangedEventArgs) Implements IModelCore.CellStateChanged

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
                newColumn.DataType = GetType(ModelCell)
                newColumn.AllowDBNull = False
                Me.Data.Columns.Add(newColumn)
            End If
        Next
        Me._modelColumns.AddRange(columns)
    End Sub

    Public Sub RemoveColumns(indexes As Integer()) Implements IModelCore.RemoveColumns
        Dim indexesDESC = indexes.OrderByDescending(Function(i) i).ToArray
        For Each index In indexesDESC
            Call Me.Data.Columns.RemoveAt(index)
            Call Me._modelColumns.RemoveAt(index)
        Next
    End Sub

    Public Sub UpdateColumns(indexes As Integer(), columns As ModelColumn()) Implements IModelCore.UpdateColumn
        If indexes.Length <> columns.Length Then
            Throw New FrontWorkException($"UpdateColumns failed: Length of indexes({indexes.Length}) must be equal to Length of modelColumns({columns.Length})")
        End If
        For i = 0 To indexes.Length - 1
            Dim index = indexes(i)
            Dim column = columns(i)
            '更新记录的ModelColumns
            Me._modelColumns(index) = column
            '更新数据表
            Dim dataColumn = Me.Data.Columns(index)
            dataColumn.ColumnName = column.Name
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
            Dim modelCell As ModelCell = Me.Data.Rows(row)(columnName)
            result(i) = modelCell.Data
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
        '如果存在Nothing，则创建空的Dictionary代替
        If dataOfEachRow Is Nothing Then
            dataOfEachRow = Util.Times(Of IDictionary(Of String, Object))(Nothing, rows.Length)
        Else
            For i = 0 To dataOfEachRow.Length - 1
                If dataOfEachRow(i) Is Nothing Then
                    dataOfEachRow(i) = New Dictionary(Of String, Object)
                End If
            Next
        End If

        Dim columnsHavingDefaultValue = (From c In Me._modelColumns Where c.DefaultValue IsNot Nothing Select c).ToArray
        If columnsHavingDefaultValue.Length > 0 Then
            '首先对数据置入默认值
            For i = 0 To dataOfEachRow.Length - 1
                For Each column In columnsHavingDefaultValue
                    Dim field = column.Name
                    Dim curData = dataOfEachRow(i)
                    '根据ModelColumn中的设定来置入默认值
                    If Not curData.ContainsKey(field) Then curData.Add(field, Nothing)
                    If curData(field) Is Nothing Then
                        curData(field) = column.DefaultValue
                    End If
                Next
            Next
        End If
        '带有插入请求的原始行号的RowInfo
        Dim oriRowInfos = (From i In Util.Range(0, rows.Length)
                           Select New ModelRowInfo(rows(i), dataOfEachRow(i), New ModelRowState(If(dataOfEachRow(i) Is Nothing OrElse dataOfEachRow(i).Count = 0, SynchronizationState.ADDED, SynchronizationState.ADDED_UPDATED)))).ToArray
        Dim adjustedRowInfos(oriRowInfos.Length - 1) As ModelRowInfo
        For i = 0 To adjustedRowInfos.Length - 1
            adjustedRowInfos(i) = oriRowInfos(i).Clone
        Next
        Util.AdjustInsertIndexes(adjustedRowInfos, Function(rowInfo) rowInfo.Row, Sub(rowInfo, newRow) rowInfo.Row = newRow, Me.GetRowCount)
        '开始添加数据
        For i = 0 To adjustedRowInfos.Length - 1
            Dim realRow = adjustedRowInfos(i).Row
            Dim curData = If(adjustedRowInfos(i).RowData, New Dictionary(Of String, Object))
            Dim dataCountWithoutDefaultValue = curData.Count
            Dim newRow = Me.Data.NewRow()
            '初始化所有单元格为ModelCell对象
            For j = 0 To newRow.ItemArray.Length - 1
                newRow(j) = New ModelCell
            Next
            '将值写入datatable
            For Each item In curData
                If Not Me.Data.Columns.Contains(item.Key) Then Continue For
                Dim colType = Me._modelColumns(Me.Data.Columns.IndexOf(item.Key)).Type
                Try
                    Dim convertedValue As Object = Util.ChangeType(item.Value, colType)
                    DirectCast(newRow(item.Key), ModelCell).Data = convertedValue
                Catch ex As Exception
                    Throw New FrontWorkException($"Value {item.Value} of ""{item.Key}"" cannot be converted to {colType.Name}")
                End Try
            Next
            Me.Data.Rows.InsertAt(newRow, realRow)
            '增加行状态的记录, 如果增加的是空数据， 则为ADDED， 否则为ADDED_UPDATED
            'Dim rowState As SynchronizationState = SynchronizationState.ADDED
            'If dataCountWithoutDefaultValue > 0 Then
            '    rowState = SynchronizationState.ADDED_UPDATED
            'End If
            Me.RowStates.Insert(realRow, adjustedRowInfos(i).State)
        Next
        RaiseEvent RowAdded(Me, New ModelRowAddedEventArgs() With {
                             .AddedRows = oriRowInfos
                            })

        RaiseEvent RowStateChanged(Me, New ModelRowStateChangedEventArgs(adjustedRowInfos))

        ''将同步状态全部设置为ADDED
        'Me.UpdateRowStates(adjustedRowInfos.Select(Function(rowInfo)
        '                                               Return rowInfo.Row
        '                                           End Function).ToArray,
        '                   Util.Times(New ModelRowState(SynchronizationState.ADDED), adjustedRowInfos.Length))

        Dim columnCount = Me.Data.Columns.Count
        Dim selectionRanges As New List(Of Range)
        For Each curRowInfo In adjustedRowInfos
            Dim curRow = curRowInfo.Row
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
                                                  Me.DataRowToDictionary(Me.Data.Rows(curRowNum)),
                                                  Me.RowStates(curRowNum))
                indexRowList.Add(newIndexRowPair)
            Next
            Dim beforeRowRemoveEventArgs As New ModelBeforeRowRemoveEventArgs With {
                .RemoveRows = indexRowList.ToArray
            }
            RaiseEvent BeforeRowRemove(Me, beforeRowRemoveEventArgs)
            If beforeRowRemoveEventArgs.Cancel Then Return
            For Each curRowNum In realRows
                Me.Data.Rows.RemoveAt(curRowNum)
                '删除行状态记录
                Me.RowStates.RemoveAt(curRowNum)
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
                    Dim columnInfo = Me._modelColumns(Me.Data.Columns.IndexOf(key))
                    Dim colType = columnInfo.Type
                    If String.IsNullOrWhiteSpace(value) Then
                        If colType = GetType(String) Then
                            DirectCast(Me.Data.Rows(row)(key), ModelCell).Data = value
                        Else
                            If Not columnInfo.Nullable Then
                                Dim col = Me.GetColumns({key})(0)
                                Throw New FrontWorkException($"""{col.Name}""不允许为空！")
                            End If
                            DirectCast(Me.Data.Rows(row)(key), ModelCell).Data = Nothing
                        End If
                    Else
                        If colType = value.GetType Then
                            DirectCast(Me.Data.Rows(row)(key), ModelCell).Data = value
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
                Dim curState = Me.GetRowStates({row})(0)
                If curState.SynchronizationState = SynchronizationState.ADDED Then
                    rowSyncStatePairs.Add(New RowSynchronizationStatePair(row, SynchronizationState.ADDED_UPDATED))
                ElseIf curState.SynchronizationState = SynchronizationState.SYNCHRONIZED Then
                    rowSyncStatePairs.Add(New RowSynchronizationStatePair(row, SynchronizationState.UPDATED))
                End If
                i += 1
            Next

            Dim updatedRows(rows.Length - 1) As ModelRowInfo
            For i = 0 To rows.Length - 1
                updatedRows(i) = New ModelRowInfo(rows(i), Me.DataRowToDictionary(Me.Data.Rows(rows(i))), Me.RowStates(rows(i)))
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
        Dim updateStateCells As New List(Of CellPositionStatePair)
        For i = 0 To rows.Length - 1
            Dim row = rows(i)
            Dim columnName = columnNames(i)
            Dim col = Me.Data.Columns.IndexOf(columnName)
            If col = -1 Then
                Throw New FrontWorkException($"UpdateCells failed: column ""{columnName}"" not found!")
            End If
            Dim column = Me._modelColumns(col)
            Try
                Dim convertedValue = If(String.IsNullOrWhiteSpace(dataOfEachCell(i)), Nothing, Util.ChangeType(dataOfEachCell(i), column.Type))
                DirectCast(Me.Data.Rows(row)(col), ModelCell).Data = convertedValue
                Dim oriCellState = Me.GetCellState(row, columnName)
                posCellPairs.Add(New ModelCellInfo(rows(i), columnName, dataOfEachCell(i), Me.GetCellState(rows(i), columnName)))
                If oriCellState.ValidationState.Type <> ValidationStateType.OK Then
                    updateStateCells.Add(New CellPositionStatePair(row, columnName, New ModelCellState(ValidationState.OK)))
                End If
            Catch ex As IndexOutOfRangeException
                Throw New FrontWorkException($"UpdateCells failed: index {i} exceeded max row index: {Me.GetRowCount}")
            Catch ex As Exception
                updateStateCells.Add(New CellPositionStatePair(row, columnName, New ModelCellState(New ValidationState(ValidationStateType.ERROR, $"""{dataOfEachCell(i)}""不是有效的格式"))))
            End Try
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
        '更新单元格状态
        Me.UpdateCellStates(
            updateStateCells.Select(Function(item) item.Row).ToArray,
            updateStateCells.Select(Function(item) item.Field).ToArray,
            updateStateCells.Select(Function(item) item.State).ToArray
        )
    End Sub

    Public Overloads Sub Refresh(args As ModelRefreshArgs) Implements IModelCore.Refresh
        Dim dataRows = args.DataRows
        Dim ranges As Range() = args.SelectionRanges
        '清除状态
        Me._WarningCellCount = 0
        Me._ErrorCellCount = 0
        '刷新选区
        Me._allSelectionRange = If(ranges, {})
        Call Me.Data.Rows.Clear()
        If dataRows IsNot Nothing Then
            '刷新数据
            For Each dataRow In dataRows
                Dim newRow = Me.Data.NewRow
                '初始化成ModelCell
                For i = 0 To newRow.ItemArray.Length - 1
                    newRow(i) = New ModelCell
                Next
                Me.Data.Rows.Add(newRow)
                For Each colAndValue In dataRow
                    Dim colName = colAndValue.Key
                    Dim colValue = colAndValue.Value
                    If Me.Data.Columns.Contains(colName) Then
                        Dim column = Me._modelColumns(Me.Data.Columns.IndexOf(colName))
                        Dim colType = column.Type
                        Try
                            Dim convertedValue = If(colValue Is Nothing, Nothing, Util.ChangeType(colValue, colType))
                            DirectCast(newRow(colName), ModelCell).Data = If(convertedValue, Nothing)
                        Catch ex As Exception
                            Throw New FrontWorkException($"Value {colValue} of ""{colName}"" cannot be converted to {colType.Name}")
                        End Try
                    End If
                Next
            Next
            '刷新同步状态字典 
            Dim stateArray(dataRows.Length - 1) As ModelRowState
            For i = 0 To stateArray.Length - 1
                stateArray(i) = New ModelRowState(SynchronizationState.SYNCHRONIZED)
            Next
            Me.RowStates.Clear()
            Me.RowStates.AddRange(stateArray)
        Else
            Me.RowStates.Clear()
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
            Dim modelCell = DirectCast(dataRow(column), ModelCell)
            result.Add(column.ColumnName, modelCell.Data)
        Next
        Return result
    End Function

    Public Sub UpdateRowStates(rows As Integer(), states As ModelRowState(), raisesEvent As Boolean)
        If rows.Length <> states.Length Then
            Throw New FrontWorkException("Length of rows must be same of the length of syncStates")
        End If
        Dim updatedRows = New List(Of ModelRowInfo)
        For i = 0 To rows.Length - 1
            Dim row = rows(i)
            Dim state = states(i)
            If row >= Me.Data.Rows.Count Then
                Throw New FrontWorkException($"Row {row} exceeded the max row of model")
            End If
            Me.RowStates(row) = states(i)
            Dim newIndexStatePair = New ModelRowInfo(row, Me.GetRows({row})(0), state)
            updatedRows.Add(newIndexStatePair)
        Next
        If raisesEvent Then
            Dim eventArgs = New ModelRowStateChangedEventArgs(updatedRows.ToArray)
            RaiseEvent RowStateChanged(Me, eventArgs)
        End If
    End Sub

    ''' <summary>
    ''' 更新行同步状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="states">状态</param>
    Public Sub UpdateRowStates(rows As Integer(), states As ModelRowState()) Implements IModelCore.UpdateRowStates
        Call Me.UpdateRowStates(rows, states, True)
    End Sub

    Private Sub UpdateRowSynchronizationStates(rows As Integer(), syncStates As SynchronizationState())
        Dim states(rows.Length - 1) As ModelRowState
        For i = 0 To rows.Length - 1
            states(i) = Me.RowStates(rows(i))
            states(i).SynchronizationState = syncStates(i)
        Next
        Call Me.UpdateRowStates(rows, states)
    End Sub

    Private Sub SetRowSynchronizationState(row As DataRow, state As SynchronizationState)
        Dim rowNum = Me.Data.Rows.IndexOf(row)
        Me.RowStates(rowNum).SynchronizationState = state
    End Sub

    Private Function GetRowSynchronizationState(row As DataRow) As SynchronizationState
        Dim rowNum = Me.Data.Rows.IndexOf(row)
        Return Me.RowStates(rowNum).SynchronizationState
    End Function

    ''' <summary>
    ''' 获取行同步状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <returns>同步状态</returns>
    Public Function GetRowStates(rows As Integer()) As ModelRowState() Implements IModelCore.GetRowStates
        Dim states(rows.Length - 1) As ModelRowState
        For i = 0 To rows.Length - 1
            Dim rowNum = rows(i)
            If Me.Data.Rows.Count <= rowNum Then
                Throw New FrontWorkException($"Row {rowNum} exceeded max row of model!")
            End If
            states(i) = Me.RowStates(rowNum)
        Next
        Return states
    End Function

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

    Public Function GetCellStates(rows As Integer(), fields As String()) As ModelCellState() Implements IModelCore.GetCellStates
        If rows.Length <> fields.Length Then
            Throw New FrontWorkException($"Length of rows({rows.Length}) must be equal to length of fields({fields.Length})")
        End If
        Dim result(rows.Length - 1) As ModelCellState
        For i = 0 To rows.Length - 1
            Dim row = rows(i)
            Dim field = fields(i)
            Dim cell As ModelCell = Me.Data.Rows(row)(field)
            result(i) = cell.State
        Next
        Return result
    End Function

    Private Function GetCellState(row As Integer, field As String) As ModelCellState
        Return Me.GetCellStates({row}, {field})(0)
    End Function

    Public Sub UpdateCellStates(rows As Integer(), fields As String(), states As ModelCellState()) Implements IModelCore.UpdateCellStates
        If rows.Length <> fields.Length OrElse rows.Length <> states.Length Then
            Throw New FrontWorkException($"Length of row, fields and states must be equal")
        End If
        Dim fieldCol As New Dictionary(Of String, DataColumn)
        For Each field In fields
            Dim col = Me.Data.Columns.IndexOf(field)
            If col = -1 Then
                Throw New FrontWorkException($"Cannot find field ""{field}"" in model!")
            End If
            fieldCol(field) = Me.Data.Columns(col)
        Next

        For i = 0 To rows.Length - 1
            Dim row = rows(i)
            Dim field = fields(i)
            Dim state = states(i)
            Dim oriState = Me.GetCellState(row, field)
            If oriState.ValidationState.Type <> state.ValidationState.Type Then
                Dim cell As ModelCell = Me.Data.Rows(row)(fieldCol(field))
                cell.State = state

                If oriState.ValidationState.Type = ValidationStateType.ERROR Then
                    Me._ErrorCellCount -= 1
                ElseIf oriState.ValidationState.Type = ValidationStateType.WARNING Then
                    Me._WarningCellCount -= 1
                End If

                If state.ValidationState.Type = ValidationStateType.ERROR Then
                    Me._ErrorCellCount += 1
                ElseIf state.ValidationState.Type = ValidationStateType.WARNING Then
                    Me._WarningCellCount += 1
                End If
            End If
        Next
        RaiseEvent CellStateChanged(Me, New ModelCellStateChangedEventArgs(Me.GetCellInfos(rows, fields)))
    End Sub

    Private Function GetCellInfos(rows As Integer(), fields As String()) As ModelCellInfo()
        Dim dataOfCells = Me.GetCells(rows, fields)
        Dim states = Me.GetCellStates(rows, fields)
        Dim result(rows.Length - 1) As ModelCellInfo
        For i = 0 To rows.Length - 1
            result(i) = New ModelCellInfo(rows(i), fields(i), dataOfCells(i), states(i))
        Next
        Return result
    End Function

    Public Function GetInfo(infoItem As ModelInfo) As Object Implements IModelCore.GetInfo
        Select Case infoItem
            Case ModelInfo.HAS_WARNING_CELL
                Return Me._WarningCellCount > 0
            Case ModelInfo.HAS_ERROR_CELL
                Return Me._ErrorCellCount > 0
            Case ModelInfo.HAS_UNSYNCHRONIZED_ROW
                For i = 0 To Me.GetRowCount - 1
                    If {SynchronizationState.UPDATED,
                        SynchronizationState.ADDED_UPDATED
                       }.Contains(Me.GetRowStates({i})(0).SynchronizationState) Then
                        Return True
                    End If
                Next
                Return False
            Case Else
                Throw New FrontWorkException($"Bad ModelInfo: {infoItem.ToString}")
        End Select
    End Function

    Private Structure RowSynchronizationStatePair
        Public Row As Integer
        Public SynchronizationState As SynchronizationState

        Public Sub New(row As Integer, state As SynchronizationState)
            Me.Row = row
            Me.SynchronizationState = state
        End Sub
    End Structure

    Private Structure CellPositionStatePair
        Public Row As Integer
        Public Field As String
        Public State As ModelCellState

        Public Sub New(row As Integer, field As String, state As ModelCellState)
            Me.Row = row
            Me.Field = field
            Me.State = state
        End Sub
    End Structure

    Private Class ModelCell
        Public State As ModelCellState = New ModelCellState(ValidationState.OK)
        Public Data As Object

        Public Sub New()

        End Sub

        Public Sub New(state As ModelCellState, data As Object)
            Me.State = state
            Me.Data = data
        End Sub

        Public Sub New(data As Object)
            Me.Data = data
            Me.State = New ModelCellState(ValidationState.OK)
        End Sub

    End Class
End Class
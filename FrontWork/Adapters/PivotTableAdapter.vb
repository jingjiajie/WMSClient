Imports System.Linq
Imports System.Text
Imports FrontWork

Public Class PivotTableAdapter
    Private Property PositionMappingList As New List(Of PositionMap)

    Public Property SourceModel As IConfigurableModel
        Get
            Return Me.SourceModelOperator.Model
        End Get
        Set(value As IConfigurableModel)
            If Me.SourceModel IsNot Nothing Then
                Call Me.UnbindSourceModel(Me.SourceModel)
            End If
            Me.SourceModelOperator.Model = value
            If Me.SourceModel IsNot Nothing Then
                Call Me.ClearPositionMap()
                Call Me.BindSourceModel(Me.SourceModel)
            End If
        End Set
    End Property

    Public Property TargetModel As IConfigurableModel
        Get
            Return Me.TargetModelOperator.Model
        End Get
        Set(value As IConfigurableModel)
            If Me.TargetModel IsNot Nothing Then
                Call Me.UnbindTargetModel(Me.TargetModel)
            End If
            Me.TargetModelOperator.Model = value
            If Me.TargetModel IsNot Nothing Then
                Call Me.ClearPositionMap()
                Call Me.BindTargetModel(Me.TargetModel)
            End If
        End Set
    End Property

    Public Property SourceMode As String = "default"
    Public Property TargetMode As String = "default"

    Public Property ColumnNamesAsRow As String()
    Public Property ColumnNamesAsColumn As String()
    Public Property ColumnNamesAsValue As String()

    Private Property SourceModelOperator As New ConfigurableModelOperator
    Private Property TargetModelOperator As New ConfigurableModelOperator

    Private Sub BindSourceModel(model As IConfigurableModel)
        AddHandler model.RowAdded, AddressOf Me.SourceModelRowAddedEvent
        AddHandler model.CellUpdated, AddressOf Me.SourceModelCellUpdatedEvent
        AddHandler model.RowUpdated, AddressOf Me.SourceModelRowUpdatedEvent
        AddHandler model.RowRemoved, AddressOf Me.SourceModelRowRemovedEvent
    End Sub

    Private Sub UnbindSourceModel(model As IConfigurableModel)
        RemoveHandler model.RowAdded, AddressOf Me.SourceModelRowAddedEvent
        RemoveHandler model.CellUpdated, AddressOf Me.SourceModelCellUpdatedEvent
        RemoveHandler model.RowUpdated, AddressOf Me.SourceModelRowUpdatedEvent
        RemoveHandler model.RowRemoved, AddressOf Me.SourceModelRowRemovedEvent
    End Sub

    Private Sub BindTargetModel(model As IConfigurableModel)
        AddHandler model.CellUpdated, AddressOf Me.TargetModelCellUpdatedEvent
        AddHandler model.RowUpdated, AddressOf Me.TargetModelRowUpdatedEvent
        AddHandler model.RowAdded, AddressOf Me.TargetModelRowAddedEvent
        AddHandler model.RowRemoved, AddressOf Me.TargetModelRowRemovedEvent
    End Sub

    Private Sub UnbindTargetModel(model As IConfigurableModel)
        RemoveHandler model.CellUpdated, AddressOf Me.TargetModelCellUpdatedEvent
        RemoveHandler model.RowUpdated, AddressOf Me.TargetModelRowUpdatedEvent
        RemoveHandler model.RowAdded, AddressOf Me.TargetModelRowAddedEvent
        RemoveHandler model.RowRemoved, AddressOf Me.TargetModelRowRemovedEvent
    End Sub

    Private Sub TargetModelCellUpdatedEvent(sender As Object, e As ModelCellUpdatedEventArgs)
        Call Me.UnpivotCell(e.UpdatedCells.Select(Function(c) c.Row).ToArray,
                            e.UpdatedCells.Select(Function(c) c.ColumnName).ToArray)
    End Sub

    Private Sub TargetModelRowRemovedEvent(sender As Object, e As ModelRowRemovedEventArgs)
        Throw New NotImplementedException()
    End Sub

    Private Sub TargetModelRowAddedEvent(sender As Object, e As ModelRowAddedEventArgs)
        Throw New NotImplementedException()
    End Sub

    Private Sub TargetModelRowUpdatedEvent(sender As Object, e As ModelRowUpdatedEventArgs)
        Throw New NotImplementedException()
    End Sub

    Private Sub SourceModelRowAddedEvent(sender As Object, e As ModelRowAddedEventArgs)
        Dim rowNums = (From r In e.AddedRows Select r.Row).ToArray
        Call Me.PivotColumns(rowNums)
        Call Me.PivotRows(rowNums)
    End Sub

    Private Sub SourceModelCellUpdatedEvent(sender As Object, e As ModelCellUpdatedEventArgs)
        Dim rowNums = (From c In e.UpdatedCells Select c.Row).ToArray
        Call Me.PivotRows(rowNums)
    End Sub

    Private Sub SourceModelRowRemovedEvent(sender As Object, e As ModelRowRemovedEventArgs)
        Throw New NotImplementedException()
    End Sub

    Private Sub SourceModelRowUpdatedEvent(sender As Object, e As ModelRowUpdatedEventArgs)
        Throw New NotImplementedException()
    End Sub

    Private Sub PivotColumns(rows As Integer())
        Dim colsAsCol = Me.ColumnNamesAsColumn
        Dim colsAsValue = Me.ColumnNamesAsValue
        Dim colsAsRow = Me.ColumnNamesAsRow
        Dim oriTargetColumnNames = (From f In TargetModel.Configuration.GetFields(Me.TargetMode) Select f.Name)
        Dim addTargetFields As New List(Of Field) '要添加到TargetModel的字段
        '将定行列加入目标Model，如果目标Model中不存在相应的列
        For Each colAsRow In colsAsRow
            If Not oriTargetColumnNames.Contains(colAsRow) Then
                addTargetFields.Add(Me.SourceModelOperator.Configuration.GetField(Me.SourceMode, colAsRow))
            End If
        Next
        Dim allSourceRowData = Me.SourceModelOperator.GetRows(rows)
        '计算定列列和定值列生成的列
        For i = 0 To rows.Length - 1
            Dim row = rows(i)
            Dim sourceRowData = allSourceRowData(i)
            Dim colsAsColValues(Me.ColumnNamesAsColumn.Length - 1) As String
            For j = 0 To Me.ColumnNamesAsColumn.Length - 1
                colsAsColValues(j) = sourceRowData(Me.ColumnNamesAsColumn(j))
            Next
            Dim targetColumnNames = Me.GetPivotColumnNames(colsAsColValues, Me.ColumnNamesAsValue)
            For j = 0 To targetColumnNames.Length - 1
                Dim targetColumnName = targetColumnNames(j)
                '如果目标表中或者本次添加的列中已经包含同名列，则不要重复添加
                If oriTargetColumnNames.Contains(targetColumnName) OrElse
                    addTargetFields.FindIndex(Function(f) f.Name = targetColumnName) > 0 Then Continue For
                '否则生成新字段，添加到目标表
                Dim sourceValueField = Me.SourceModelOperator.Configuration.GetField(Me.SourceMode, Me.ColumnNamesAsValue(j))
                Dim newField As Field = sourceValueField.Clone
                newField.Name = targetColumnName
                newField.DisplayName = targetColumnName
                addTargetFields.Add(newField)
            Next
        Next
        Me.TargetModelOperator.Configuration.AddFields(Me.TargetMode, addTargetFields.ToArray)
    End Sub

    Private Sub PivotRows(sourceRows As Integer())
        Dim addRows As New List(Of IDictionary(Of String, Object))
        Dim updateData As New List(Of PositionDataPair)
        Dim allSourceRowData = Me.SourceModelOperator.GetRows(sourceRows)
        Dim allTargetRowData = Me.TargetModelOperator.GetRows(Util.Range(0, TargetModelOperator.GetRowCount)).ToList
        For i = 0 To sourceRows.Length - 1
            Dim sourceRow = sourceRows(i)
            Dim sourceRowData = allSourceRowData(i)
            '确定定行列，如果没有则新建
            Dim targetRow = Me.GetTargetRow(sourceRowData, allTargetRowData.ToArray, Me.ColumnNamesAsRow)
            If targetRow = -1 Then
                Dim newRow As New Dictionary(Of String, Object)
                For Each col In Me.ColumnNamesAsRow
                    newRow.Add(col, sourceRowData(col))
                Next
                targetRow = Me.TargetModelOperator.GetRowCount + addRows.Count
                addRows.Add(newRow)
                '把要添加和更新的行先同步到allTargetRowData，以免在下次循环时读取不到变化
                allTargetRowData.Add(newRow)
            End If

            Dim colsAsColValues(Me.ColumnNamesAsColumn.Length - 1) As String
            For j = 0 To Me.ColumnNamesAsColumn.Length - 1
                colsAsColValues(j) = If(sourceRowData(Me.ColumnNamesAsColumn(j))?.ToString, "")
            Next
            Dim targetColNames = Me.GetPivotColumnNames(colsAsColValues, Me.ColumnNamesAsValue)
            For j = 0 To Me.ColumnNamesAsValue.Length - 1
                Dim targetColName = targetColNames(j)
                Dim targetValue = sourceRowData(Me.ColumnNamesAsValue(j))
                updateData.Add(New PositionDataPair(targetRow, targetColName, targetValue))
                '将对应关系添加到位置映射里
                Call Me.SetPositionMap(New Position(sourceRow, Me.ColumnNamesAsValue(j)), New Position(targetRow, targetColName))
                '把要添加和更新的行先同步到allTargetRowData，以免在下次循环时读取不到变化
                allTargetRowData(targetRow)(targetColName) = targetValue
            Next
        Next
        RemoveHandler Me.TargetModel.RowAdded, AddressOf Me.TargetModelRowAddedEvent
        RemoveHandler Me.TargetModel.CellUpdated, AddressOf Me.TargetModelCellUpdatedEvent
        Me.TargetModelOperator.AddRows(addRows.ToArray)
        Me.TargetModelOperator.UpdateCells(updateData.Select(Function(p) p.Row).ToArray,
                                           updateData.Select(Function(p) p.ColumnName).ToArray,
                                           updateData.Select(Function(p) p.Data).ToArray)
        AddHandler Me.TargetModel.RowAdded, AddressOf Me.TargetModelRowAddedEvent
        AddHandler Me.TargetModel.CellUpdated, AddressOf Me.TargetModelCellUpdatedEvent
    End Sub

    Private Sub UnpivotCell(targetRows As Integer(), targetColumnNames As String())
        Dim allCellData = Me.TargetModelOperator.GetCells(targetRows, targetColumnNames)
        Dim updateSourceData As New List(Of PositionDataPair)
        For i = 0 To allCellData.Length - 1
            Dim cellData = allCellData(i)
            Dim mappedSourcePos = Me.GetSourcePosition(New Position(targetRows(i), targetColumnNames(i)))
            updateSourceData.Add(New PositionDataPair(mappedSourcePos, cellData))
        Next
        RemoveHandler Me.SourceModel.CellUpdated, AddressOf Me.SourceModelCellUpdatedEvent
        Call Me.SourceModelOperator.UpdateCells(updateSourceData.Select(Function(p) p.Row).ToArray,
                                                updateSourceData.Select(Function(p) p.ColumnName).ToArray,
                                                updateSourceData.Select(Function(p) p.Data).ToArray)
        AddHandler Me.SourceModel.CellUpdated, AddressOf Me.SourceModelCellUpdatedEvent
    End Sub

    Private Function GetPivotColumnNames(colsAsCol As String(), colsAsValue As String()) As String()
        Dim sbMainColName As New StringBuilder
        For Each colAsCol In colsAsCol
            sbMainColName.Append(colAsCol)
        Next
        Dim mainColName = sbMainColName.ToString
        Dim colNames(colsAsValue.Length - 1) As String
        For i = 0 To colsAsValue.Length - 1
            colNames(i) = mainColName + colsAsValue(i)
        Next
        Return colNames
    End Function

    Private Function GetTargetRow(rowData As IDictionary(Of String, Object), targetRowData As IDictionary(Of String, Object)(), colsAsRow As String()) As Integer
        For row = 0 To targetRowData.Length - 1
            Dim mismatched = False
            For Each col In colsAsRow
                Dim targetValue = targetRowData(row)(col)
                Dim srcValue = rowData(col)
                If Not targetValue.Equals(srcValue) Then
                    mismatched = True
                    Exit For
                End If
            Next
            If Not mismatched Then
                Return row
            End If
        Next
        Return -1
    End Function

    Private Sub SetPositionMap(sourceModelPosition As Position, targetModelPosition As Position)
        Me.PositionMappingList.RemoveAll(Function(map) map.SourcePosition = sourceModelPosition OrElse map.TargetPosition = targetModelPosition)
        Me.PositionMappingList.Add(New PositionMap(sourceModelPosition, targetModelPosition))
    End Sub

    Private Sub ClearPositionMap()
        Me.PositionMappingList.Clear()
    End Sub

    Private Function GetTargetPosition(sourceModelPosition As Position) As Position
        Dim foundMap = Me.PositionMappingList.Find(Function(map) map.SourcePosition = sourceModelPosition)
        If foundMap Is Nothing Then Return Nothing
        Return foundMap.TargetPosition
    End Function

    Private Function GetSourcePosition(targetModelPosition As Position) As Position
        Dim foundMap = Me.PositionMappingList.Find(Function(map) map.TargetPosition = targetModelPosition)
        If foundMap Is Nothing Then Return Nothing
        Return foundMap.SourcePosition
    End Function

    Private Class Position
        Public Property Row As Integer
        Public Property ColumnName As String

        Public Sub New(row As Integer, columnName As String)
            Me.Row = row
            Me.ColumnName = columnName
        End Sub

        Public Shared Operator =(pos1 As Position, pos2 As Position) As Boolean
            If pos1.Row <> pos2.Row Then Return False
            If pos1.ColumnName <> pos2.ColumnName Then Return False
            Return True
        End Operator

        Public Shared Operator <>(pos1 As Position, pos2 As Position) As Boolean
            Return Not pos1 = pos2
        End Operator
    End Class

    Private Class PositionDataPair
        Public Sub New(row As Integer, columnName As String, data As Object)
            Me.Position = New Position(row, columnName)
            Me.Data = data
        End Sub

        Public Sub New(pos As Position, data As Object)
            Me.Position = pos
            Me.Data = data
        End Sub

        Public Property Row As Integer
            Get
                Return Me.Position.Row
            End Get
            Set(value As Integer)
                Me.Position.Row = value
            End Set
        End Property

        Public Property ColumnName As String
            Get
                Return Me.Position.ColumnName
            End Get
            Set(value As String)
                Me.Position.ColumnName = value
            End Set
        End Property

        Public Property Data As Object
        Public Property Position As Position
    End Class

    Private Class PositionMap
        Public Sub New(sourcePosition As Position, targetPosition As Position)
            Me.SourcePosition = sourcePosition
            Me.TargetPosition = targetPosition
        End Sub

        Public Property SourcePosition As Position
        Public Property TargetPosition As Position
    End Class
End Class

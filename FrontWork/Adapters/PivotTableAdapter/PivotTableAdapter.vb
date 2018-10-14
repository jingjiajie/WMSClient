Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Linq
Imports System.Text
Imports FrontWork

Public Class PivotTableAdapter
    Private Property _CellMapManager As New CellMapManager
    Private Property _ColumnMapManager As New ColumnMapManager

    '<Category("FrontWork"), Description("使用说明"), Editor(GetType(PivotTableManualUITypeEditor), GetType(UITypeEditor))>
    'Public Property Manual As String = "使用说明"

    Private _SourceModel As IConfigurableModel
    Private _TargetModel As IConfigurableModel

    <Category("FrontWork")>
    Public Property SourceModel As IConfigurableModel
        Get
            Return Me._SourceModel
        End Get
        Set(value As IConfigurableModel)
            If Me.SourceModel IsNot Nothing Then
                Call Me.UnbindSourceModel(Me.SourceModel)
            End If
            Me._SourceModel = value
            If Me.SourceModel IsNot Nothing Then
                Call Me._CellMapManager.ClearPositionMaps()
                Call Me.BindSourceModel(Me.SourceModel)
            End If
        End Set
    End Property

    <Category("FrontWork")>
    Public Property TargetModel As IConfigurableModel
        Get
            Return Me._TargetModel
        End Get
        Set(value As IConfigurableModel)
            If Me.TargetModel IsNot Nothing Then
                Call Me.UnbindTargetModel(Me.TargetModel)
            End If
            Me._TargetModel = value
            If Me.TargetModel IsNot Nothing Then
                Call Me._CellMapManager.ClearPositionMaps()
                Call Me.BindTargetModel(Me.TargetModel)
            End If
        End Set
    End Property

    <Category("FrontWork")>
    Public Property SourceMode As String = "default"
    <Category("FrontWork")>
    Public Property TargetMode As String = "default"

    <Category("FrontWork")>
    Public Property ColumnNamesAsRow As String()
    <Category("FrontWork")>
    Public Property ColumnNamesAsColumn As String()
    <Category("FrontWork")>
    Public Property ColumnNamesAsValue As String()

    Private ReadOnly Property ColumnDisplayNamesAsValue As String()
        Get
            Dim displayNames(Me.ColumnNamesAsValue.Length - 1) As String
            For i = 0 To displayNames.Length - 1
                Dim field = Me.SourceModel.Configuration.GetField(Me.SourceMode, Me.ColumnNamesAsValue(i))
                displayNames(i) = field.DisplayName.GetValue
            Next
            Return displayNames
        End Get
    End Property

    Private Sub BindSourceModel(model As IConfigurableModel)
        AddHandler model.RowAdded, AddressOf Me.SourceModelRowAddedEvent
        AddHandler model.CellUpdated, AddressOf Me.SourceModelCellUpdatedEvent
        AddHandler model.RowUpdated, AddressOf Me.SourceModelRowUpdatedEvent
        AddHandler model.RowRemoved, AddressOf Me.SourceModelRowRemovedEvent
        AddHandler model.Refreshed, AddressOf Me.SourceModelRefreshedEvent
    End Sub

    Private Sub UnbindSourceModel(model As IConfigurableModel)
        RemoveHandler model.RowAdded, AddressOf Me.SourceModelRowAddedEvent
        RemoveHandler model.CellUpdated, AddressOf Me.SourceModelCellUpdatedEvent
        RemoveHandler model.RowUpdated, AddressOf Me.SourceModelRowUpdatedEvent
        RemoveHandler model.RowRemoved, AddressOf Me.SourceModelRowRemovedEvent
        RemoveHandler model.Refreshed, AddressOf Me.SourceModelRefreshedEvent
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
        Call Me.UnpivotCells(e.UpdatedCells.Select(Function(c) c.Row).ToArray,
                            e.UpdatedCells.Select(Function(c) c.FieldName).ToArray)
    End Sub

    Private Sub TargetModelRowRemovedEvent(sender As Object, e As ModelRowRemovedEventArgs)
        Throw New FrontWorkException("PivotTableAdapter: Cannot remove rows from target model!")
    End Sub

    Private Sub TargetModelRowAddedEvent(sender As Object, e As ModelRowAddedEventArgs)
        Throw New FrontWorkException("PivotTableAdapter: Cannot add rows from target model!")
    End Sub

    Private Sub TargetModelRowUpdatedEvent(sender As Object, e As ModelRowUpdatedEventArgs)
        Dim rows = e.UpdatedRows.Select(Function(c) c.Row).ToArray
        Dim targetFields = Me.TargetModel.Configuration.GetFields(Me.TargetMode)
        Dim fieldNames = (From f In targetFields Select f.Name.GetValue).ToArray
        Dim updatableFieldNames = fieldNames.Where(
        Function(name)
            Return Not Me.ColumnNamesAsRow.Contains(name)
        End Function).ToArray

        Dim updateCells(rows.Length * updatableFieldNames.Length - 1) As CellPosition
        For i = 0 To rows.Length - 1
            For j = 0 To updatableFieldNames.Length - 1
                Dim row = rows(i)
                Dim colName = updatableFieldNames(j)
                updateCells(i * updatableFieldNames.Length + j) = New CellPosition(row, colName)
            Next
        Next

        Call Me.UnpivotCells(updateCells.Select(Function(c) c.Row).ToArray,
                            updateCells.Select(Function(c) c.ColumnName).ToArray)
    End Sub

    Private Sub SourceModelRefreshedEvent(sender As Object, e As ModelRefreshedEventArgs)
        Me.TargetModel.Refresh(New ModelRefreshArgs(Nothing, Nothing))
        Me.TargetModel.Configuration.ClearFields(Me.TargetMode)
        Call Me.PivotColumns(Util.Range(0, Me.SourceModel.GetRowCount))
        Call Me.PivotRows(Util.Range(0, Me.SourceModel.GetRowCount))
    End Sub

    Private Sub SourceModelRowAddedEvent(sender As Object, e As ModelRowAddedEventArgs)
        Dim rowNums = (From r In e.AddedRows Select r.Row).ToArray
        Dim adjustedRowNums = Util.AdjustInsertIndexes(rowNums, Me.SourceModel.GetRowCount - rowNums.Length)
        Call Me._CellMapManager.AdjustSourceRowsAfterRowsInserted(rowNums)
        Call Me.PivotColumns(adjustedRowNums)
        Call Me.PivotRows(adjustedRowNums)
    End Sub

    Private Sub SourceModelCellUpdatedEvent(sender As Object, e As ModelCellUpdatedEventArgs)
        Dim rowNums = (From c In e.UpdatedCells Select c.Row).ToArray
        Dim cellNames = (From c In e.UpdatedCells Select c.FieldName).ToArray
        Call Me.PivotCells(rowNums, cellNames)
    End Sub

    Private Sub SourceModelRowRemovedEvent(sender As Object, e As ModelRowRemovedEventArgs)
        Dim removedRowNums = e.RemovedRows.Select(Function(r) r.Row).ToArray
        Call Me._CellMapManager.AdjustSourceRowsAfterRowsRemoved(removedRowNums)
    End Sub

    Private Sub SourceModelRowUpdatedEvent(sender As Object, e As ModelRowUpdatedEventArgs)
        Throw New NotImplementedException()
    End Sub

    Private Sub PivotColumns(rows As Integer())
        Dim colsAsCol = Me.ColumnNamesAsColumn
        Dim colsAsRow = Me.ColumnNamesAsRow
        Dim oriTargetColumnNames = (From f In TargetModel.Configuration.GetFields(Me.TargetMode) Select f.Name.GetValue)
        Dim addTargetFields As New List(Of Field) '要添加到TargetModel的字段
        '将定行列加入目标Model，如果目标Model中不存在相应的列
        For Each colAsRow In colsAsRow
            If Not oriTargetColumnNames.Contains(colAsRow) Then
                Dim sourceFieldAsRow = Me.SourceModel.Configuration.GetField(Me.SourceMode, colAsRow)
                Dim newFieldAsRow As Field = sourceFieldAsRow.Clone
                newFieldAsRow.Editable = New FieldProperty(Of Boolean)(False) '目标表的定行列禁止编辑
                addTargetFields.Add(newFieldAsRow)
            End If
        Next
        Dim allSourceRowData = Me.SourceModel.GetRows(rows)
        '计算定列列和定值列生成的列
        For i = 0 To rows.Length - 1
            Dim row = rows(i)
            Dim sourceRowData = allSourceRowData(i)
            Dim colsAsColValues(Me.ColumnNamesAsColumn.Length - 1) As Object
            For j = 0 To Me.ColumnNamesAsColumn.Length - 1
                colsAsColValues(j) = sourceRowData(Me.ColumnNamesAsColumn(j))
            Next
            Dim targetColumnNames = Me.GetPivotColumnNames(colsAsColValues, Me.ColumnDisplayNamesAsValue)
            For j = 0 To targetColumnNames.Length - 1
                Dim targetColumnName = targetColumnNames(j)
                '如果目标表中或者本次添加的列中已经包含同名列，则不要重复添加
                If oriTargetColumnNames.Contains(targetColumnName) OrElse
                    addTargetFields.FindIndex(Function(f) f.Name.GetValue = targetColumnName) > 0 Then Continue For
                '否则生成新字段，添加到目标表
                Dim sourceValueField = Me.SourceModel.Configuration.GetField(Me.SourceMode, Me.ColumnNamesAsValue(j))
                Dim newField As Field = sourceValueField.Clone
                newField.Name = New FieldProperty(Of String)(targetColumnName)
                newField.DisplayName = New FieldProperty(Of String)(targetColumnName)
                addTargetFields.Add(newField)
                '记录列映射
                Me._ColumnMapManager.SetColumnMap(colsAsColValues, Me.ColumnNamesAsValue(j), targetColumnName)
            Next
        Next
        Me.TargetModel.Configuration.AddFields(Me.TargetMode, addTargetFields.ToArray)
    End Sub

    Private Sub PivotRows(sourceRows As Integer())
        Dim addRows As New List(Of IDictionary(Of String, Object))
        Dim updateData As New List(Of CellPositionDataPair)
        Dim allSourceRowData = Me.SourceModel.GetRows(sourceRows)
        Dim allTargetRowData = Me.TargetModel.GetRows(Util.Range(0, TargetModel.GetRowCount)).ToList
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
                targetRow = Me.TargetModel.GetRowCount + addRows.Count
                addRows.Add(newRow)
                '把要添加和更新的行先同步到allTargetRowData，以免在下次循环时读取不到变化
                allTargetRowData.Add(newRow)
            End If

            Dim colsAsColValues(Me.ColumnNamesAsColumn.Length - 1) As String
            For j = 0 To Me.ColumnNamesAsColumn.Length - 1
                colsAsColValues(j) = If(sourceRowData(Me.ColumnNamesAsColumn(j))?.ToString, "")
            Next
            Dim targetColNames = Me.GetPivotColumnNames(colsAsColValues, Me.ColumnDisplayNamesAsValue)
            For j = 0 To Me.ColumnNamesAsValue.Length - 1
                Dim targetColName = targetColNames(j)
                Dim targetValue = sourceRowData(Me.ColumnNamesAsValue(j))
                updateData.Add(New CellPositionDataPair(targetRow, targetColName, targetValue))
                '将对应关系添加到位置映射里
                Call Me._CellMapManager.SetPositionMap(New CellPosition(sourceRow, Me.ColumnNamesAsValue(j)), New CellPosition(targetRow, targetColName))
                '把要添加和更新的行先同步到allTargetRowData，以免在下次循环时读取不到变化
                allTargetRowData(targetRow)(targetColName) = targetValue
            Next
        Next
        RemoveHandler Me.TargetModel.RowAdded, AddressOf Me.TargetModelRowAddedEvent
        RemoveHandler Me.TargetModel.CellUpdated, AddressOf Me.TargetModelCellUpdatedEvent
        If addRows.Count > 0 Then
            Me.TargetModel.AddRows(addRows.ToArray)
        End If
        If updateData.Count > 0 Then
            Me.TargetModel.UpdateCells(updateData.Select(Function(p) p.Row).ToArray,
                                           updateData.Select(Function(p) p.ColumnName).ToArray,
                                           updateData.Select(Function(p) p.Data).ToArray)
        End If
        AddHandler Me.TargetModel.RowAdded, AddressOf Me.TargetModelRowAddedEvent
        AddHandler Me.TargetModel.CellUpdated, AddressOf Me.TargetModelCellUpdatedEvent
    End Sub

    Private Sub UnpivotCells(targetRows As Integer(), targetColumnNames As String())
        Dim allCellData = Me.TargetModel.GetCells(targetRows, targetColumnNames)
        Dim allRowData = Me.TargetModel.GetRows(targetRows)
        Dim addSourceRows As New List(Of IDictionary(Of String, Object))
        Dim updateSourceData As New List(Of CellPositionDataPair)
        For i = 0 To allCellData.Length - 1
            Dim targetRow = targetRows(i)
            Dim targetColumnName = targetColumnNames(i)
            Dim cellData = allCellData(i)

            Dim mappedSourcePos = Me._CellMapManager.GetSourcePosition(New CellPosition(targetRows(i), targetColumnNames(i)))
            '
            If mappedSourcePos IsNot Nothing Then
                '如果有映射单元格，则更新源Model对应单元格
                updateSourceData.Add(New CellPositionDataPair(mappedSourcePos, cellData))
            Else
                Dim colMap = Me._ColumnMapManager.GetColumnMap(targetColumnName)
                'Me.GetSourceRow(targetRowData, Me.SourceModel.GetRows(Util.Range(0, Me.SourceModel.GetRowCount)), Me.ColumnNamesAsRow, Me.ColumnNamesAsColumn)
                Dim newSourceRowNum = Me.SourceModel.GetRowCount + addSourceRows.Count
                Dim targetRowData = allRowData(i)
                Dim newSourceRow = New Dictionary(Of String, Object)

                For Each colAsRow In Me.ColumnNamesAsRow
                    newSourceRow.Add(colAsRow, targetRowData(colAsRow))
                Next
                For j = 0 To Me.ColumnNamesAsColumn.Length - 1
                    Dim colName = Me.ColumnNamesAsColumn(j)
                    newSourceRow.Add(colName, colMap.SourceColumnsAsColumnValues(j))
                Next
                newSourceRow.Add(colMap.SourceColumnAsValueName, cellData)
                addSourceRows.Add(newSourceRow)
                Dim newSourceRowColumnsAsColumnValues = colMap.SourceColumnsAsColumnValues.ToArray
                '将新加的一整行都进行单元格映射
                Dim pivotColumnNames = Me.GetPivotColumnNames(newSourceRowColumnsAsColumnValues, Me.ColumnDisplayNamesAsValue)
                For j = 0 To pivotColumnNames.Length - 1
                    Me._CellMapManager.SetPositionMap(New CellPosition(newSourceRowNum, Me.ColumnNamesAsValue(j)),
                                                      New CellPosition(targetRow, pivotColumnNames(j)))
                Next
            End If
        Next
        If addSourceRows.Count > 0 Then
            RemoveHandler Me.SourceModel.RowAdded, AddressOf Me.SourceModelRowAddedEvent
            Call Me.SourceModel.AddRows(addSourceRows.ToArray)
            AddHandler Me.SourceModel.RowAdded, AddressOf Me.SourceModelRowAddedEvent
        End If

        If updateSourceData.Count > 0 Then
            RemoveHandler Me.SourceModel.CellUpdated, AddressOf Me.SourceModelCellUpdatedEvent
            Call Me.SourceModel.UpdateCells(updateSourceData.Select(Function(p) p.Row).ToArray,
                                                updateSourceData.Select(Function(p) p.ColumnName).ToArray,
                                                updateSourceData.Select(Function(p) p.Data).ToArray)
            AddHandler Me.SourceModel.CellUpdated, AddressOf Me.SourceModelCellUpdatedEvent
        End If
    End Sub

    Private Sub PivotCells(sourceRows As Integer(), sourceColumnNames As String())
        Dim allCellData = Me.SourceModel.GetCells(sourceRows, sourceColumnNames)
        Dim updateTargetData As New List(Of CellPositionDataPair)
        For i = 0 To allCellData.Length - 1
            Dim cellData = allCellData(i)
            Dim mappedTargetPos = Me._CellMapManager.GetTargetPosition(New CellPosition(sourceRows(i), sourceColumnNames(i)))
            If mappedTargetPos Is Nothing Then
                Throw New FrontWorkException($"Unmapped source model cell: Row {sourceRows(i)},Col ""{sourceColumnNames(i)}""")
            End If
            updateTargetData.Add(New CellPositionDataPair(mappedTargetPos, cellData))
        Next
        RemoveHandler Me.TargetModel.CellUpdated, AddressOf Me.TargetModelCellUpdatedEvent
        Call Me.TargetModel.UpdateCells(updateTargetData.Select(Function(p) p.Row).ToArray,
                                                updateTargetData.Select(Function(p) p.ColumnName).ToArray,
                                                updateTargetData.Select(Function(p) p.Data).ToArray)
        AddHandler Me.TargetModel.CellUpdated, AddressOf Me.TargetModelCellUpdatedEvent
    End Sub

    Private Function GetPivotColumnNames(colsAsColValue As Object(), colsAsValue As String()) As String()
        Dim sbMainColName As New StringBuilder
        For Each colAsColValue In colsAsColValue
            sbMainColName.Append(colAsColValue)
        Next
        Dim mainColName = sbMainColName.ToString
        Dim colNames(colsAsValue.Length - 1) As String
        For i = 0 To colsAsValue.Length - 1
            colNames(i) = mainColName & colsAsValue(i)
        Next
        Return colNames
    End Function

    Private Function GetTargetRow(sourceRowData As IDictionary(Of String, Object), allTargetRowData As IDictionary(Of String, Object)(), colsAsRow As String()) As Integer
        For row = 0 To allTargetRowData.Length - 1
            Dim mismatched = False
            For Each col In colsAsRow
                Dim targetValue = allTargetRowData(row)(col)
                Dim srcValue = sourceRowData(col)
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

    Private Function GetSourceRow(targetRowData As IDictionary(Of String, Object), allSourceRowData As IDictionary(Of String, Object)(), colsAsRow As String(), colsAsColumn As String()) As Integer
        For sourceRow = 0 To allSourceRowData.Length - 1
            Dim sourceRowData = allSourceRowData(sourceRow)
            Dim mismatched As Boolean = False
            For Each colAsRow In colsAsRow
                If Not sourceRowData(colAsRow).Equals(targetRowData(colAsRow)) Then
                    mismatched = True
                    Exit For
                End If
            Next
            If mismatched Then Continue For
            For Each colAsColumn In colsAsColumn
                If Not sourceRowData(colAsColumn).Equals(targetRowData(colAsColumn)) Then
                    mismatched = True
                    Exit For
                End If
            Next
            If Not mismatched Then Return sourceRow
        Next
        Return -1
    End Function

    Private Class PivotTableManualUITypeEditor
        Inherits UITypeEditor

        Public Overrides Function GetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
            Return UITypeEditorEditStyle.Modal
        End Function

        Public Overrides Function EditValue(context As ITypeDescriptorContext, provider As IServiceProvider, value As Object) As Object
            Dim formManual As New ManualBrowserForm("https://github.com/jingjiajie/FrontWork/wiki/%E9%85%8D%E7%BD%AE%E4%B8%AD%E5%BF%83---%E9%85%8D%E7%BD%AE%E4%B8%AD%E5%BF%83%E7%BB%84%E4%BB%B6(Configuration)", "PivotTableAdapter使用说明")
            Call formManual.Show()
            Return value
        End Function
    End Class
End Class

Imports System.ComponentModel
Imports System.Linq
Imports FrontWork

Public Class EditableDataViewModel
    Implements IConfigurable
    Protected ViewOperator As New EditableDataViewOperator
    Protected _Configuration As Configuration
    Protected _Mode As String = "default"
    Protected _Model As IModel

    Public Overridable Property Configuration As Configuration Implements IConfigurable.Configuration
        Get
            Return Me._Configuration
        End Get
        Set(value As Configuration)
            Dim oldFields As Field() = {}
            If Me._Configuration IsNot Nothing Then
                oldFields = Me._Configuration.GetFields(Me.Mode)
                RemoveHandler Me._Configuration.Refreshed, AddressOf Me.ConfigurationRefreshedEvent
                RemoveHandler Me._Configuration.FieldAdded, AddressOf Me.ConfigurationFieldAddedEvent
                RemoveHandler Me._Configuration.FieldUpdated, AddressOf Me.ConfigurationFieldUpdatedEvent
                RemoveHandler Me._Configuration.FieldRemoved, AddressOf Me.ConfigurationFieldRemovedEvent
            End If
            Me._Configuration = value
            Dim newFields As Field() = {}
            If Me._Configuration IsNot Nothing Then
                newFields = Me._Configuration.GetFields(Me.Mode)
                AddHandler Me._Configuration.Refreshed, AddressOf Me.ConfigurationRefreshedEvent
                AddHandler Me._Configuration.FieldAdded, AddressOf Me.ConfigurationFieldAddedEvent
                AddHandler Me._Configuration.FieldUpdated, AddressOf Me.ConfigurationFieldUpdatedEvent
                AddHandler Me._Configuration.FieldRemoved, AddressOf Me.ConfigurationFieldRemovedEvent
                Call Me.ConfigurationRefreshedEvent(Me, Nothing)
            End If
        End Set
    End Property

    Private Sub ConfigurationFieldRemovedEvent(sender As Object, e As ConfigurationFieldRemovedEventArgs)
        Dim oldColumns = Me.ViewOperator.GetColumns
        Dim allFields = Me.Configuration.GetFields(Me.Mode)
        Dim visibleFields = (From f In allFields Where f.Visible Select f).ToArray
        Dim newColumns = Me.FieldConfigurationsToViewColumn(visibleFields)
        Call Me.RefreshViewSchema(oldColumns, newColumns)
        If Me.Model IsNot Nothing AndAlso Me.View IsNot Nothing AndAlso Me.View.GetRowCount > 0 Then
            Call Me.PushModelRow(Util.Range(0, Me.ViewOperator.GetRowCount))
        End If
    End Sub

    Private Sub ConfigurationFieldUpdatedEvent(sender As Object, e As ConfigurationFieldUpdatedEventArgs)
        Dim oldColumns = Me.ViewOperator.GetColumns
        Dim allFields = Me.Configuration.GetFields(Me.Mode)
        Dim visibleFields = (From f In allFields Where f.Visible Select f).ToArray
        Dim newColumns = Me.FieldConfigurationsToViewColumn(visibleFields)
        Call Me.RefreshViewSchema(oldColumns, newColumns)
        If Me.Model IsNot Nothing AndAlso Me.View IsNot Nothing AndAlso Me.View.GetRowCount > 0 Then
            Call Me.PushModelRow(Util.Range(0, Me.ViewOperator.GetRowCount))
        End If
    End Sub

    Private Sub ConfigurationFieldAddedEvent(sender As Object, e As ConfigurationFieldAddedEventArgs)
        Dim oldColumns = Me.ViewOperator.GetColumns
        Dim allFields = Me.Configuration.GetFields(Me.Mode)
        Dim visibleFields = (From f In allFields Where f.Visible Select f).ToArray
        Dim newColumns = Me.FieldConfigurationsToViewColumn(visibleFields)
        Call Me.RefreshViewSchema(oldColumns, newColumns)
        If Me.Model IsNot Nothing AndAlso Me.View IsNot Nothing AndAlso Me.View.GetRowCount > 0 Then
            Call Me.PushModelRow(Util.Range(0, Me.ViewOperator.GetRowCount))
        End If
    End Sub

    Protected Overridable Sub ConfigurationRefreshedEvent(sender As Object, e As ConfigurationRefreshedEventArgs)
        If Me.Configuration Is Nothing Then
            Throw New FrontWorkException($"Configuration not set for view!")
        End If
        Dim oldColumns = Me.ViewOperator.GetColumns
        Dim allFields = Me.Configuration.GetFields(Me.Mode)
        Dim visibleFields = (From f In allFields Where f.Visible Select f).ToArray
        Dim newColumns = Me.FieldConfigurationsToViewColumn(visibleFields)
        Call Me.RefreshViewSchema(oldColumns, newColumns)
        If Me.Model IsNot Nothing AndAlso Me.View IsNot Nothing AndAlso Me.View.GetRowCount > 0 Then
            Call Me.PushModelRow(Util.Range(0, Me.ViewOperator.GetRowCount))
        End If
    End Sub


    Public Overridable Property Mode As String Implements IConfigurable.Mode
        Get
            Return Me._Mode
        End Get
        Set(value As String)
            If Me._Mode IsNot Nothing AndAlso Me._Mode.Equals(value, StringComparison.OrdinalIgnoreCase) Then
                Return
            End If
            Me._Mode = value
            Call Me.ConfigurationRefreshedEvent(Me, Nothing)
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(view As IEditableDataView)
        Me.View = view
    End Sub

    Public Sub New(model As IModel)
        Me.Model = model
    End Sub

    Public Sub New(model As IModel, view As IEditableDataView)
        Me.Model = model
        Me.View = view
    End Sub

    ''' <summary>
    ''' Model对象，用来存取数据
    ''' </summary>
    ''' <returns>Model对象</returns>
    Public Overridable Property Model As IModel
        Get
            Return Me._Model
        End Get
        Set(value As IModel)
            If value Is Me._Model Then Return
            If Me._Model IsNot Nothing Then
                Call Me.UnbindModel()
            End If
            Me._Model = value
            If Me._Model IsNot Nothing Then
                Call Me.BindModel()
            End If
        End Set
    End Property

    ''' <summary>
    ''' ViewOperationsWrapper对象，用来代理View
    ''' </summary>
    ''' <returns>ViewOperationsWrapper对象</returns>
    Public Overridable Property View As IEditableDataView
        Get
            Return Me.ViewOperator.View
        End Get
        Set(value As IEditableDataView)
            If value Is Me.ViewOperator.View Then Return
            If Me.ViewOperator.View IsNot Nothing Then
                Call Me.UnbindView()
            End If
            Me.ViewOperator.View = value
            If Me.ViewOperator IsNot Nothing Then
                Call Me.BindView()
            End If
        End Set
    End Property

    ''' <summary>
    ''' 自动比对新视图与原视图的差别，并命令视图更新。
    ''' 采用各字段位置不变，新视图字段改变更新，字段增加则增加，字段减少则删除的策略
    ''' </summary>
    Protected Overridable Sub RefreshViewSchema(oldColumns As ViewColumn(), newColumns As ViewColumn())
        If oldColumns.Length = 0 AndAlso newColumns.Length = 0 Then Return
        Dim updateColumns As New List(Of KeyValuePair(Of Integer, ViewColumn))
        Dim addColumns As New List(Of ViewColumn)
        Dim removeColumns As New List(Of Integer)
        For i = 0 To Math.Max(oldColumns.Length, newColumns.Length) - 1
            If oldColumns.Length > i AndAlso newColumns.Length > i Then
                If oldColumns(i) = newColumns(i) Then Continue For
                updateColumns.Add(New KeyValuePair(Of Integer, ViewColumn)(i, newColumns(i)))
            ElseIf oldColumns.Length > i AndAlso newColumns.Length <= i Then
                removeColumns.Add(i)
            ElseIf oldColumns.Length <= i AndAlso newColumns.Length > i Then
                addColumns.Add(newColumns(i))
            Else
                Throw New FrontWorkException("Unexpected exception")
            End If
        Next
        If updateColumns.Count > 0 Then
            Call Me.ViewOperator.UpdateColumns((From kv In updateColumns Select kv.Key).ToArray, (From kv In updateColumns Select kv.Value).ToArray)
        End If
        If addColumns.Count > 0 Then
            Call Me.ViewOperator.AddColumns(addColumns.ToArray)
        End If
        If removeColumns.Count > 0 Then
            Call Me.ViewOperator.RemoveColumns(removeColumns.ToArray)
        End If
    End Sub

    Protected Overridable Function FieldConfigurationsToViewColumn(fields As Field()) As ViewColumn()
        Dim context = New ModelViewInvocationContext(Me.Model, Me.View)
        Dim result(fields.Length - 1) As ViewColumn
        For i = 0 To fields.Length - 1
            Dim curField = fields(i)
            Dim newViewColumn = New ViewColumn(context, curField.PlaceHolder, curField.Values, curField.Name, curField.DisplayName, curField.Type.GetValue, curField.Editable)
            result(i) = newViewColumn
        Next
        Return result
    End Function

    ''' <summary>
    ''' 绑定新的Model，将本View的各种事件绑定到Model上以实现数据变化的同步
    ''' </summary>
    Protected Overridable Sub BindModel()
        AddHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
        AddHandler Me.Model.RowAdded, AddressOf Me.ModelRowAddedEvent
        AddHandler Me.Model.RowRemoved, AddressOf Me.ModelRowRemovedEvent
        AddHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
        AddHandler Me.Model.CellStateChanged, AddressOf Me.ModelCellStateChanged
        AddHandler Me.Model.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
        AddHandler Me.Model.RowStateChanged, AddressOf Me.ModelRowStateChanged
        AddHandler Me.Model.Refreshed, AddressOf Me.ModelRefreshedEvent

        Call Me.ModelRefreshedEvent(Me, Nothing)
    End Sub

    ''' <summary>
    ''' 解绑Model，取消本视图绑定的所有事件
    ''' </summary>
    Protected Overridable Sub UnbindModel()
        RemoveHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
        RemoveHandler Me.Model.CellStateChanged, AddressOf Me.ModelCellStateChanged
        RemoveHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
        RemoveHandler Me.Model.RowAdded, AddressOf Me.ModelRowAddedEvent
        RemoveHandler Me.Model.RowRemoved, AddressOf Me.ModelRowRemovedEvent
        RemoveHandler Me.Model.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
        RemoveHandler Me.Model.RowStateChanged, AddressOf Me.ModelRowStateChanged
        RemoveHandler Me.Model.Refreshed, AddressOf Me.ModelRefreshedEvent
    End Sub

    Private Sub ViewEditEndedEvent(sender As Object, e As ViewEditEndedEventArgs)
        Dim context As New ModelViewEditInvocationContext(Me.Model, Me.View, e.Row, e.ColumnName, e.CellData)
        Dim fieldName = e.ColumnName
        Dim curField = Me.Configuration.GetField(Me.Mode, fieldName)
        If curField.EditEnded IsNot Nothing Then
            curField.EditEnded.Invoke(context)
        End If
    End Sub

    Private Sub ViewContentChangedEvent(sender As Object, e As ViewContentChangedEventArgs)
        Dim context As New ModelViewEditInvocationContext(Me.Model, Me.View, e.Row, e.ColumnName, e.CellData)
        Dim fieldName = e.ColumnName
        Dim curField = Me.Configuration.GetField(Me.Mode, fieldName)
        If curField.ContentChanged IsNot Nothing Then
            curField.ContentChanged.Invoke(context)
        End If
    End Sub

    Protected Overridable Sub BindView()
        AddHandler Me.ViewOperator.CellUpdated, AddressOf Me.ViewCellUpdatedEvent
        AddHandler Me.ViewOperator.RowUpdated, AddressOf Me.ViewRowUpdatedEvent
        AddHandler Me.ViewOperator.RowAdded, AddressOf Me.ViewRowAddedEvent
        AddHandler Me.ViewOperator.RowRemoved, AddressOf Me.ViewRowRemovedEvent
        AddHandler Me.ViewOperator.SelectionRangeChanged, AddressOf Me.ViewSelectionRangeChangedEvent

        AddHandler Me.View.ContentChanged, AddressOf Me.ViewContentChangedEvent
        AddHandler Me.View.EditEnded, AddressOf Me.ViewEditEndedEvent
        If Me.Configuration IsNot Nothing Then
            Call Me.ConfigurationRefreshedEvent(Me, Nothing)
            Call Me.ModelRefreshedEvent(Me, Nothing)
        End If
    End Sub

    Protected Overridable Sub ViewSelectionRangeChangedEvent(sender As Object, e As ViewSelectionRangeChangedEventArgs)
        RemoveHandler Me.Model.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
        Me.Model.AllSelectionRanges = e.NewSelectionRanges
        AddHandler Me.Model.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
    End Sub

    Protected Overridable Sub ViewRowRemovedEvent(sender As Object, e As ViewRowRemovedEventArgs)
        Dim rows = (From r In e.Rows Select r.Row).ToArray

        Try
            RemoveHandler Me.Model.RowRemoved, AddressOf Me.ModelRowRemovedEvent
            Call Me.Model.RemoveRows(rows)
            AddHandler Me.Model.RowRemoved, AddressOf Me.ModelRowRemovedEvent
        Catch ex As FrontWorkException
            Call MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Protected Overridable Sub ViewRowAddedEvent(sender As Object, e As ViewRowAddedEventArgs)
        Dim rows = (From r In e.Rows Select r.Row).ToArray
        Dim data = (From r In e.Rows Select r.RowData).ToArray

        For i = 0 To rows.Length - 1
            data(i) = Me.GetBackwardMappedRowData(data(i), rows(i))
        Next

        Try
            RemoveHandler Me.Model.RowAdded, AddressOf Me.ModelRowAddedEvent
            Call Me.Model.InsertRows(rows, data)
            AddHandler Me.Model.RowAdded, AddressOf Me.ModelRowAddedEvent
        Catch ex As FrontWorkException
            Call MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Protected Overridable Sub ViewRowUpdatedEvent(sender As Object, e As ViewRowUpdatedEventArgs)
        Throw New NotImplementedException("Don't use this event! It will be soon deleted.")
        'Dim rows = (From r In e.Rows Select r.Row).ToArray
        'Dim data = (From r In e.Rows Select r.RowData).ToArray
        'Dim fields = Me.Configuration.GetFields(Me.Mode)
        'Dim uneditableFieldNames = (From f In fields
        '                            Where f.Editable.GetValue = False
        '                            Select f.Name.GetValue).ToArray
        'For i = 0 To rows.Length - 1
        '    Dim keys = data(i).Keys.ToArray
        '    For Each key In keys
        '        If uneditableFieldNames.Contains(key) Then
        '            data(i).Remove(key)
        '        End If
        '    Next
        '    data(i) = Me.GetBackwardMappedRowData(data(i), rows(i))
        'Next

        'Try
        '    RemoveHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
        '    Call Me.Model.UpdateRows(rows, data)
        '    AddHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
        'Catch ex As FrontWorkException
        '    Call MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        'End Try
    End Sub

    Protected Overridable Sub ViewCellUpdatedEvent(sender As Object, e As ViewCellUpdatedEventArgs)
        '删除所有不能编辑的单元格
        Dim cellInfos As New List(Of ViewCellInfo)(e.Cells)
        cellInfos.RemoveAll(
            Function(cellInfo)
                Return Not Me.Configuration.GetField(Me.Mode, cellInfo.ColumnName).Editable.GetValue
            End Function)

        Dim columns = cellInfos.Select(Function(cellInfo)
                                           Return Me.Configuration.GetField(Me.Mode, cellInfo.ColumnName)
                                       End Function)
        Dim validationError As New List(Of CellPositionValidationStatePair)
        Dim updateCell As New List(Of ViewCellInfo)

        For i = 0 To cellInfos.Count - 1
            Dim column = columns(i)
            Dim cellInfo = cellInfos(i)
            Dim rawData = cellInfo.CellData
            Try
                Dim convertedData As Object
                convertedData = Util.ChangeType(rawData, column.Type.GetValue)
                Dim mappedData = Me.GetBackwardMappedCellData(convertedData, columns(i).Name, cellInfo.Row)
                updateCell.Add(New ViewCellInfo(cellInfo.Row, cellInfo.ColumnName, mappedData))
            Catch ex As Exception
                validationError.Add(New CellPositionValidationStatePair(
                                        cellInfo.Row,
                                        cellInfo.ColumnName,
                                        New ValidationState(ValidationStateType.ERROR, $"""{rawData}""不是有效的格式！"))
                                    )
            End Try
        Next

        Try
            RemoveHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
            Call Me.Model.UpdateCells(updateCell.Select(
                                        Function(item) item.Row).ToArray,
                                        updateCell.Select(Function(item) item.ColumnName).ToArray,
                                        updateCell.Select(Function(item) item.CellData).ToArray)
            AddHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
        Catch ex As FrontWorkException
            Call MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

        Call Me.Model.UpdateCellValidationStates(
            validationError.Select(Function(item) item.CellPosition.Row).ToArray,
            validationError.Select(Function(item) item.CellPosition.Field).ToArray,
            validationError.Select(Function(item) item.State).ToArray
        )
    End Sub

    Protected Overridable Sub UnbindView()
        RemoveHandler Me.ViewOperator.CellUpdated, AddressOf Me.ViewCellUpdatedEvent
        RemoveHandler Me.ViewOperator.RowUpdated, AddressOf Me.ViewRowUpdatedEvent
        RemoveHandler Me.ViewOperator.RowAdded, AddressOf Me.ViewRowAddedEvent
        RemoveHandler Me.ViewOperator.RowRemoved, AddressOf Me.ViewRowRemovedEvent
        RemoveHandler Me.ViewOperator.SelectionRangeChanged, AddressOf Me.ViewSelectionRangeChangedEvent

        RemoveHandler Me.View.ContentChanged, AddressOf Me.ViewContentChangedEvent
        RemoveHandler Me.View.EditEnded, AddressOf Me.ViewEditEndedEvent
    End Sub

    Protected Overridable Sub ModelSelectionRangeChangedEvent(sender As Object, e As ModelSelectionRangeChangedEventArgs)
        RemoveHandler Me.ViewOperator.SelectionRangeChanged, AddressOf Me.ViewSelectionRangeChangedEvent
        Me.ViewOperator.SetSelectionRanges(e.NewSelectionRange)
        AddHandler Me.ViewOperator.SelectionRangeChanged, AddressOf Me.ViewSelectionRangeChangedEvent
    End Sub

    Protected Overridable Sub ModelRowAddedEvent(sender As Object, e As ModelRowAddedEventArgs)
        Dim indexes = (From r In e.AddedRows Select r.Row).ToArray
        Dim data = (From r In e.AddedRows Select r.RowData).ToArray
        For i = 0 To data.Length - 1
            If data(i) Is Nothing Then
                data(i) = New Dictionary(Of String, Object)
            Else
                data(i) = Me.GetForwardMappedRowData(data(i), indexes(i))
            End If
        Next
        RemoveHandler Me.ViewOperator.RowAdded, AddressOf Me.ViewRowAddedEvent
        Call Me.ViewOperator.InsertRows(indexes, data)
        AddHandler Me.ViewOperator.RowAdded, AddressOf Me.ViewRowAddedEvent
    End Sub

    Protected Overridable Sub ModelRowRemovedEvent(sender As Object, e As ModelRowRemovedEventArgs)
        Dim indexes = (From r In e.RemovedRows Select r.Row).ToArray
        RemoveHandler Me.ViewOperator.RowRemoved, AddressOf Me.ViewRowRemovedEvent
        Call Me.ViewOperator.RemoveRows(indexes)
        AddHandler Me.ViewOperator.RowRemoved, AddressOf Me.ViewRowRemovedEvent
    End Sub

    Private Sub ModelRowStateChanged(sender As Object, e As ModelRowStateChangedEventArgs)
        Dim rows = (From r In e.StateUpdatedRows Select r.Row).ToArray
        Dim states = (From r In e.StateUpdatedRows Select New ViewRowState(r.State)).ToArray
        Call Me.ViewOperator.UpdateRowStates(rows, states)
    End Sub

    Private Sub ModelCellStateChanged(sender As Object, e As ModelCellStateChangedEventArgs)
        Dim rows = (From c In e.StateUpdatedCells Select c.Row).ToArray
        Dim fields = (From c In e.StateUpdatedCells Select c.FieldName).ToArray
        Dim states = (From c In e.StateUpdatedCells Select New ViewCellState(c.State)).ToArray
        Call Me.ViewOperator.UpdateCellStates(rows, fields, states)
    End Sub

    Protected Overridable Sub ModelCellUpdatedEvent(sender As Object, e As ModelCellUpdatedEventArgs)
        If Me.Configuration Is Nothing Then
            Throw New FrontWorkException("Configuration is not setted")
        End If
        If Me.ViewOperator Is Nothing Then
            Throw New FrontWorkException("View is not set")
        End If

        Dim modelCellInfos = (From c In e.UpdatedCells Select CType(c.Clone, ModelCellInfo)).ToList
        modelCellInfos.RemoveAll(Function(cellInfo)
                                     Dim curField = Me.Configuration.GetField(Me.Mode, cellInfo.FieldName)
                                     Return Not curField.Visible.GetValue
                                 End Function)

        For i = 0 To modelCellInfos.Count - 1
            Dim curCellInfo = modelCellInfos(i)
            Dim colName = curCellInfo.FieldName
            Dim row = curCellInfo.Row
            curCellInfo.CellData = Me.GetForwardMappedCellData(curCellInfo.CellData, colName, row)
        Next
        RemoveHandler Me.ViewOperator.CellUpdated, AddressOf Me.ViewCellUpdatedEvent
        Call Me.ViewOperator.UpdateCells(modelCellInfos.Select(Function(cellInfo) cellInfo.Row).ToArray,
                                                  modelCellInfos.Select(Function(cellInfo) cellInfo.FieldName).ToArray,
                                                  modelCellInfos.Select(Function(cellInfo) cellInfo.CellData).ToArray)
        AddHandler Me.ViewOperator.CellUpdated, AddressOf Me.ViewCellUpdatedEvent
    End Sub

    Protected Overridable Sub ModelRefreshedEvent(sender As Object, e As ModelRefreshedEventArgs)
        Dim data = Me.Model.GetRows(Util.Range(0, Me.Model.GetRowCount))
        Dim newRowCount = data.Length
        Dim oriViewRowCount = Me.ViewOperator.GetRowCount
        For i = 0 To newRowCount - 1
            data(i) = Me.GetForwardMappedRowData(data(i), i)
        Next

        RemoveHandler Me.ViewOperator.RowAdded, AddressOf Me.ViewRowAddedEvent
        RemoveHandler Me.ViewOperator.RowUpdated, AddressOf ViewRowUpdatedEvent
        RemoveHandler Me.ViewOperator.RowRemoved, AddressOf Me.ViewRowRemovedEvent
        RemoveHandler Me.ViewOperator.SelectionRangeChanged, AddressOf Me.ViewSelectionRangeChangedEvent
        RemoveHandler Me.ViewOperator.RowStateChanged, AddressOf Me.ViewRowStateChangedEvent

        If newRowCount < oriViewRowCount Then '原来视图行数大于新的行数，删除部分行
            Call Me.ViewOperator.RemoveRows(Util.Range(newRowCount, oriViewRowCount))
        ElseIf newRowCount > oriViewRowCount Then '原来视图行数小于新的行数，则增加部分行
            Dim addData(newRowCount - oriViewRowCount - 1) As IDictionary(Of String, Object)
            For i = 0 To addData.Length - 1
                addData(i) = data(oriViewRowCount + i)
            Next
            Call Me.ViewOperator.AddRows(addData)
        End If
        Dim commonRowCount = System.Math.Min(newRowCount, oriViewRowCount)
        If commonRowCount > 0 Then
            Dim updateData(commonRowCount - 1) As IDictionary(Of String, Object)
            For i = 0 To updateData.Length - 1
                updateData(i) = data(i)
            Next
            Call Me.ViewOperator.UpdateRows(Util.Range(0, updateData.Length), updateData)
        End If
        Call Me.ViewOperator.SetSelectionRanges(Me.Model.AllSelectionRanges)
        '刷新行状态
        Dim rowNums = Util.Range(0, newRowCount)
        Dim modelRowStates = Me.Model.GetRowStates(rowNums)
        Dim viewRowStates = (From s In modelRowStates Select New ViewRowState(s)).ToArray
        Call Me.ViewOperator.UpdateRowStates(rowNums, viewRowStates)
        AddHandler Me.ViewOperator.RowAdded, AddressOf Me.ViewRowAddedEvent
        AddHandler Me.ViewOperator.RowUpdated, AddressOf ViewRowUpdatedEvent
        AddHandler Me.ViewOperator.RowRemoved, AddressOf Me.ViewRowRemovedEvent
        AddHandler Me.ViewOperator.SelectionRangeChanged, AddressOf Me.ViewSelectionRangeChangedEvent
        AddHandler Me.ViewOperator.RowStateChanged, AddressOf Me.ViewRowStateChangedEvent
    End Sub

    Private Sub ViewRowStateChangedEvent(sender As Object, e As ViewRowStateChangedEventArgs)
        RemoveHandler Me.Model.RowStateChanged, AddressOf Me.ModelRowStateChanged
        Dim viewRowInfos = e.StateChangedRows
        Dim modelRowStates(viewRowInfos.Length - 1) As ModelRowState
        Dim rows(viewRowInfos.Length - 1) As Integer
        For i = 0 To modelRowStates.Length - 1
            Dim viewRowInfo = viewRowInfos(i)
            modelRowStates(i) = New ModelRowState(viewRowInfo.RowState)
            rows(i) = viewRowInfo.Row
        Next
        Call Me.Model.UpdateRowStates(rows, modelRowStates)
        AddHandler Me.Model.RowStateChanged, AddressOf Me.ModelRowStateChanged
    End Sub

    Protected Overridable Sub ModelRowUpdatedEvent(sender As Object, e As ModelRowUpdatedEventArgs)
        Dim rows = (From r In e.UpdatedRows Select r.Row).ToArray
        Call Me.PushModelRow(rows)
    End Sub

    Protected Overridable Sub PushModelRow(rows As Integer())
        If Me.Model Is Nothing Then Return
        Dim data = Me.Model.GetRows(rows)

        '遍历传入数据
        For i = 0 To rows.Length - 1
            Dim curRowNum = rows(i)
            data(i) = Me.GetForwardMappedRowData(data(i), curRowNum)
        Next
        RemoveHandler Me.ViewOperator.RowUpdated, AddressOf Me.ViewRowUpdatedEvent
        Call Me.ViewOperator.UpdateRows(rows, data)
        AddHandler Me.ViewOperator.RowUpdated, AddressOf Me.ViewRowUpdatedEvent
    End Sub

    Protected Overridable Function GetForwardMappedRowData(rowData As IDictionary(Of String, Object), rowNum As Integer) As IDictionary(Of String, Object)
        Dim result As New Dictionary(Of String, Object)
        For Each kv In rowData
            Dim key = kv.Key
            result.Add(key, Me.GetForwardMappedCellData(rowData(key), key, rowNum))
        Next
        Return result
    End Function

    Protected Overridable Function GetForwardMappedCellData(cellData As Object, fieldName As String, rowNum As Integer) As Object
        Dim context As New ModelViewEditInvocationContext(Me.Model, Me.View, rowNum, fieldName, cellData)
        Dim fields = Me.Configuration.GetFields(Me.Mode)
        Dim curField = (From f In fields Where f.Name.GetValue?.ToString.Equals(fieldName, StringComparison.OrdinalIgnoreCase) Select f).FirstOrDefault
        Dim result = Nothing
        If curField Is Nothing Then
            result = cellData
        ElseIf curField.ForwardMapper IsNot Nothing Then
            result = curField.ForwardMapper.Invoke(context)
        Else
            result = cellData
        End If
        Return result
    End Function


    Protected Overridable Function GetBackwardMappedRowData(rowData As IDictionary(Of String, Object), rowNum As Integer) As IDictionary(Of String, Object)
        Dim keys = rowData.Keys.ToArray
        For Each key In keys
            rowData(key) = Me.GetBackwardMappedCellData(rowData(key), key, rowNum)
        Next
        Return rowData
    End Function

    Protected Overridable Function GetBackwardMappedCellData(cellData As Object, fieldName As String, rowNum As Integer) As Object
        Dim context As New ModelViewEditInvocationContext(Me.Model, Me.View, rowNum, fieldName, cellData)
        Dim fields = Me.Configuration.GetFields(Me.Mode)
        Dim curField = (From f In fields Where f.Name.GetValue?.ToString.Equals(fieldName, StringComparison.OrdinalIgnoreCase) Select f).First
        If curField Is Nothing Then
            Throw New FrontWorkException($"Field ""{fieldName}"" not found in Configuration!")
        End If
        Dim result = Nothing
        If curField.BackwardMapper IsNot Nothing Then
            result = curField.BackwardMapper.Invoke(context)
        Else
            result = cellData
        End If
        Return result
    End Function
End Class

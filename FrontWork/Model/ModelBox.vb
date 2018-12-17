Imports System.ComponentModel
Imports System.Linq
Imports FrontWork

Public Class ModelBox
    Inherits UserControl
    Implements IConfigurableModel
    Private _currentModelName As String

    Private _configuration As Configuration
    Private _mode As String

    Public Property Configuration As Configuration Implements IConfigurableModel.Configuration
        Get
            Return Me._configuration
        End Get
        Set(value As Configuration)
            Me._configuration = value
            For Each model As Model In Me.Models
                model.Configuration = value
            Next
        End Set
    End Property

    Public Property Mode As String Implements IConfigurableModel.Mode
        Get
            Return Me._mode
        End Get
        Set(value As String)
            Me._mode = value
            For Each model As Model In Me.Models
                model.Mode = value
            Next
        End Set
    End Property

    Public Event SelectedModelChanged As EventHandler(Of SelectedModelChangedEventArgs)
    Public Event ModelCollectionChanged As EventHandler(Of ModelCollectionChangedEventArgs)

    Public Event Refreshed As EventHandler(Of ModelRefreshedEventArgs) Implements IModelCore.Refreshed
    Public Event RowAdded As EventHandler(Of ModelRowAddedEventArgs) Implements IModelCore.RowAdded
    Public Event RowUpdated As EventHandler(Of ModelRowUpdatedEventArgs) Implements IModelCore.RowUpdated
    Public Event RowRemoved As EventHandler(Of ModelRowRemovedEventArgs) Implements IModelCore.RowRemoved
    Public Event BeforeRowRemove As EventHandler(Of ModelBeforeRowRemoveEventArgs) Implements IModelCore.BeforeRowRemove
    Public Event CellUpdated As EventHandler(Of ModelCellUpdatedEventArgs) Implements IModelCore.CellUpdated
    Public Event SelectionRangeChanged As EventHandler(Of ModelSelectionRangeChangedEventArgs) Implements IModelCore.SelectionRangeChanged
    Public Event RowStateChanged As EventHandler(Of ModelRowStateChangedEventArgs) Implements IModelCore.RowStateChanged
    Public Event CellStateChanged As EventHandler(Of ModelCellStateChangedEventArgs) Implements IModelCore.CellStateChanged

    Public Property CurrentModel As Model

    <DesignerSerializationVisibility(False)>
    Public ReadOnly Property Models As New ModelCollection

    Private Sub BindModel(model As IModelCore)
        AddHandler model.Refreshed, AddressOf Me.ModelRefreshedEvent
        AddHandler model.RowAdded, AddressOf Me.ModelRowAddedEvent
        AddHandler model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
        AddHandler model.RowRemoved, AddressOf Me.ModelRowRemovedEvent
        AddHandler model.BeforeRowRemove, AddressOf Me.ModelBeforeRowRemoveEvent
        AddHandler model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
        AddHandler model.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
        AddHandler model.RowStateChanged, AddressOf Me.ModelRowStateChangedEvent
        AddHandler model.CellStateChanged, AddressOf Me.ModelCellStateChangedEvent
    End Sub

    Private Sub UnbindModel(model As IModelCore)
        RemoveHandler model.Refreshed, AddressOf Me.ModelRefreshedEvent
        RemoveHandler model.RowAdded, AddressOf Me.ModelRowAddedEvent
        RemoveHandler model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
        RemoveHandler model.RowRemoved, AddressOf Me.ModelRowRemovedEvent
        RemoveHandler model.BeforeRowRemove, AddressOf Me.ModelBeforeRowRemoveEvent
        RemoveHandler model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
        RemoveHandler model.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
        RemoveHandler model.RowStateChanged, AddressOf Me.ModelRowStateChangedEvent
        RemoveHandler model.CellStateChanged, AddressOf Me.ModelCellStateChangedEvent
    End Sub

    <Description("当前模型名称"), Category("FrontWork")>
    Public Property CurrentModelName As String
        Get
            Return Me._currentModelName
        End Get
        Set(value As String)
            If value Is Nothing Then
                Throw New FrontWorkException("ModelName cannot be null!")
            End If
            If Me.CurrentModel IsNot Nothing Then
                Call Me.UnbindModel(Me.CurrentModel)
            End If
            If Not Me._Models.Contains(value) Then
                Dim newModel As New Model
                newModel.Name = value
                newModel.Configuration = Me.Configuration
                Me._Models.SetModel(newModel)
                Me.CurrentModel = newModel
            Else
                Me.CurrentModel = Me._Models(value)
            End If
            Call Me.BindModel(Me.CurrentModel)
            Me._currentModelName = value
            RaiseEvent SelectedModelChanged(Me, New SelectedModelChangedEventArgs)
            RaiseEvent Refreshed(Me, New ModelRefreshedEventArgs) '更换Model触发Model刷新事件
        End Set
    End Property

    Private Sub ModelRefreshedEvent(sender As Object, e As ModelRefreshedEventArgs)
        RaiseEvent Refreshed(sender, e)
    End Sub
    Private Sub ModelRowAddedEvent(sender As Object, e As ModelRowAddedEventArgs)
        RaiseEvent RowAdded(sender, e)
    End Sub
    Private Sub ModelRowUpdatedEvent(sender As Object, e As ModelRowUpdatedEventArgs)
        RaiseEvent RowUpdated(sender, e)
    End Sub
    Private Sub ModelRowRemovedEvent(sender As Object, e As ModelRowRemovedEventArgs)
        RaiseEvent RowRemoved(sender, e)
    End Sub
    Private Sub ModelBeforeRowRemoveEvent(sender As Object, e As ModelBeforeRowRemoveEventArgs)
        RaiseEvent BeforeRowRemove(sender, e)
    End Sub
    Private Sub ModelCellUpdatedEvent(sender As Object, e As ModelCellUpdatedEventArgs)
        RaiseEvent CellUpdated(sender, e)
    End Sub
    Private Sub ModelSelectionRangeChangedEvent(sender As Object, e As ModelSelectionRangeChangedEventArgs)
        RaiseEvent SelectionRangeChanged(sender, e)
    End Sub
    Private Sub ModelRowStateChangedEvent(sender As Object, e As ModelRowStateChangedEventArgs)
        RaiseEvent RowStateChanged(sender, e)
    End Sub
    Private Sub ModelCellStateChangedEvent(sender As Object, e As ModelCellStateChangedEventArgs)
        RaiseEvent CellStateChanged(sender, e)
    End Sub

    Public Property AllSelectionRanges As Range() Implements IModel.AllSelectionRanges
        Get
            Return DirectCast(CurrentModel, IModel).AllSelectionRanges
        End Get
        Set(value As Range())
            DirectCast(CurrentModel, IModel).AllSelectionRanges = value
        End Set
    End Property

    Public Property AllSelectionRanges(i As Integer) As Range Implements IModel.AllSelectionRanges
        Get
            Return DirectCast(CurrentModel, IModel).AllSelectionRanges(i)
        End Get
        Set(value As Range)
            DirectCast(CurrentModel, IModel).AllSelectionRanges(i) = value
        End Set
    End Property

    Public Property SelectionRange As Range Implements IModel.SelectionRange
        Get
            Return DirectCast(CurrentModel, IModel).SelectionRange
        End Get
        Set(value As Range)
            DirectCast(CurrentModel, IModel).SelectionRange = value
        End Set
    End Property

    Default Public Property _Item(row As Integer, column As String) As Object Implements IModel._Item
        Get
            Return DirectCast(CurrentModel, IModel)(row, column)
        End Get
        Set(value As Object)
            DirectCast(CurrentModel, IModel)(row, column) = value
        End Set
    End Property

    Default Public Property _Item(row As Integer) As IDictionary(Of String, Object) Implements IModel._Item
        Get
            Return DirectCast(CurrentModel, IModel)(row)
        End Get
        Set(value As IDictionary(Of String, Object))
            DirectCast(CurrentModel, IModel)(row) = value
        End Set
    End Property

    Public ReadOnly Property RowCount As Integer Implements IModel.RowCount
        Get
            Return DirectCast(CurrentModel, IModel).RowCount
        End Get
    End Property

    Public ReadOnly Property ColumnCount As Integer Implements IModel.ColumnCount
        Get
            Return DirectCast(CurrentModel, IModel).ColumnCount
        End Get
    End Property

    Private Property IModelCore_Name As String Implements IModelCore.Name
        Get
            Return DirectCast(CurrentModel, IModel).Name
        End Get
        Set(value As String)
            DirectCast(CurrentModel, IModel).Name = value
        End Set
    End Property

    Public Sub New()
        Call MyBase.New
        If Not Me.DesignMode Then Me.Visible = False
        Me.CurrentModelName = "default"
        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        AddHandler Me.Models.ModelCollectionChanged,
            Sub(sender, e)
                RaiseEvent ModelCollectionChanged(sender, e)
            End Sub
    End Sub

    Public Sub GroupBy(fieldName As String)
        Dim dataRows = Me.GetRows(Util.Range(0, Me.GetRowCount))
        'If Not Me.GetColumns.Contains(fieldName) Then
        '    Throw New FrontWorkException($"""{fieldName}"" not exist in {Me.Name}")
        'End If
        Dim groups = (From row In dataRows
                      Group By key = CStr(If(row(fieldName), "")) Into g = Group
                      Select New With {.Key = key, .Values = g}).ToArray
        Call Me.Models.Clear() '清空所有Model
        For Each group In groups
            Dim groupName = group.Key
            Dim groupRows = group.Values.ToArray
            '创建新的Model
            Dim newModel = New Model
            newModel.Configuration = Me.Configuration
            newModel.Name = groupName
            newModel.Refresh(New ModelRefreshArgs(groupRows, {New Range(0, 0, 1, 1)}))
            Me.Models.SetModel(newModel)
        Next
        If groups.Length > 0 Then
            Dim firstGroup = groups(0)
            Me.CurrentModelName = firstGroup.Key
        Else
            Me._currentModelName = Nothing
        End If
    End Sub

    Public Function GetCell(row As Integer, columnName As String) As Object Implements IModel.GetCell
        Return DirectCast(CurrentModel, IModel).GetCell(row, columnName)
    End Function

    Public Function GetRows(Of T As New)(rows() As Integer) As T() Implements IModel.GetRows
        Return DirectCast(CurrentModel, IModel).GetRows(Of T)(rows)
    End Function

    Public Function GetRow(Of T As New)(row As Integer) As T Implements IModel.GetRow
        Return DirectCast(CurrentModel, IModel).GetRow(Of T)(row)
    End Function

    Public Function GetRow(row As Integer) As IDictionary(Of String, Object) Implements IModel.GetRow
        Return DirectCast(CurrentModel, IModel).GetRow(row)
    End Function

    Public Function AddRow(data As IDictionary(Of String, Object)) As Integer Implements IModel.AddRow
        Return DirectCast(CurrentModel, IModel).AddRow(data)
    End Function

    Public Sub InsertRows(row As Integer, count As Integer, data() As IDictionary(Of String, Object)) Implements IModel.InsertRows
        DirectCast(CurrentModel, IModel).InsertRows(row, count, data)
    End Sub

    Public Sub InsertRow(row As Integer, data As IDictionary(Of String, Object)) Implements IModel.InsertRow
        DirectCast(CurrentModel, IModel).InsertRow(row, data)
    End Sub

    Public Sub RemoveRow(row As Integer) Implements IModel.RemoveRow
        DirectCast(CurrentModel, IModel).RemoveRow(row)
    End Sub

    Public Sub RemoveRows(startRow As Integer, rowCount As Integer) Implements IModel.RemoveRows
        DirectCast(CurrentModel, IModel).RemoveRows(startRow, rowCount)
    End Sub

    Public Sub RemoveSelectedRows() Implements IModel.RemoveSelectedRows
        DirectCast(CurrentModel, IModel).RemoveSelectedRows()
    End Sub

    Public Sub UpdateRow(row As Integer, data As IDictionary(Of String, Object)) Implements IModel.UpdateRow
        DirectCast(CurrentModel, IModel).UpdateRow(row, data)
    End Sub

    Public Sub UpdateCell(row As Integer, columnName As String, data As Object) Implements IModel.UpdateCell
        DirectCast(CurrentModel, IModel).UpdateCell(row, columnName, data)
    End Sub

    Public Function DataRowToDictionary(dataRow As DataRow) As IDictionary(Of String, Object) Implements IModel.DataRowToDictionary
        Return DirectCast(CurrentModel, IModel).DataRowToDictionary(dataRow)
    End Function

    Public Sub UpdateRowState(row As Integer, state As ModelRowState) Implements IModel.UpdateRowState
        DirectCast(CurrentModel, IModel).UpdateRowState(row, state)
    End Sub

    Public Function GetRowState(row As Integer) As ModelRowState Implements IModel.GetRowState
        Return DirectCast(CurrentModel, IModel).GetRowState(row)
    End Function

    Public Function ContainsColumn(columnName As String) As Boolean Implements IModel.ContainsColumn
        Return DirectCast(CurrentModel, IModel).ContainsColumn(columnName)
    End Function

    Public Sub SelectRowsByValues(Of T)(columnName As String, values() As T) Implements IModel.SelectRowsByValues
        DirectCast(CurrentModel, IModel).SelectRowsByValues(columnName, values)
    End Sub

    Public Function GetSelectedRows(Of T As New)() As T() Implements IModel.GetSelectedRows
        Return DirectCast(CurrentModel, IModel).GetSelectedRows(Of T)()
    End Function

    Public Function GetSelectedRows() As IDictionary(Of String, Object)() Implements IModel.GetSelectedRows
        Return DirectCast(CurrentModel, IModel).GetSelectedRows()
    End Function

    Public Function GetSelectedRows(Of T)(columnName As String) As T() Implements IModel.GetSelectedRows
        Return DirectCast(CurrentModel, IModel).GetSelectedRows(Of T)(columnName)
    End Function

    Public Function GetSelectedRow() As IDictionary(Of String, Object) Implements IModel.GetSelectedRow
        Return DirectCast(CurrentModel, IModel).GetSelectedRow()
    End Function

    Public Function GetSelectedRow(Of T As New)() As IDictionary(Of String, Object) Implements IModel.GetSelectedRow
        Return DirectCast(CurrentModel, IModel).GetSelectedRow(Of T)()
    End Function

    Public Function GetSelectedRow(Of T)(columnName As String) As T Implements IModel.GetSelectedRow
        Return DirectCast(CurrentModel, IModel).GetSelectedRow(Of T)(columnName)
    End Function

    Public Sub RemoveUneditedNewRows() Implements IModel.RemoveUneditedNewRows
        DirectCast(CurrentModel, IModel).RemoveUneditedNewRows()
    End Sub

    Public Function HasUnsynchronizedUpdatedRow() As Boolean Implements IModel.HasUnsynchronizedUpdatedRow
        Return DirectCast(CurrentModel, IModel).HasUnsynchronizedUpdatedRow()
    End Function

    Public Function HasErrorCell() As Boolean Implements IModel.HasErrorCell
        Return DirectCast(CurrentModel, IModel).HasErrorCell()
    End Function

    Public Function HasWarningCell() As Boolean Implements IModel.HasWarningCell
        Return DirectCast(CurrentModel, IModel).HasWarningCell()
    End Function

    Public Sub RefreshView(rows() As Integer) Implements IModel.RefreshView
        DirectCast(CurrentModel, IModel).RefreshView(rows)
    End Sub

    Public Sub RefreshView(row As Integer) Implements IModel.RefreshView
        DirectCast(CurrentModel, IModel).RefreshView(row)
    End Sub

    Public Function GetRowSynchronizationState(row As Integer) As SynchronizationState Implements IModel.GetRowSynchronizationState
        Return DirectCast(CurrentModel, IModel).GetRowSynchronizationState(row)
    End Function

    Public Function GetRowSynchronizationStates(rows() As Integer) As SynchronizationState() Implements IModel.GetRowSynchronizationStates
        Return DirectCast(CurrentModel, IModel).GetRowSynchronizationStates(rows)
    End Function

    Public Function GetCellState(row As Integer, field As String) As ModelCellState Implements IModel.GetCellState
        Return DirectCast(CurrentModel, IModel).GetCellState(row, field)
    End Function

    Public Sub UpdateCellState(row As Integer, field As String, state As ModelCellState) Implements IModel.UpdateCellState
        DirectCast(CurrentModel, IModel).UpdateCellState(row, field, state)
    End Sub

    Public Sub UpdateCellValidationStates(rows() As Integer, fields() As String, states() As ValidationState) Implements IModel.UpdateCellValidationStates
        DirectCast(CurrentModel, IModel).UpdateCellValidationStates(rows, fields, states)
    End Sub

    Public Sub UpdateCellValidationState(row As Integer, field As String, state As ValidationState) Implements IModel.UpdateCellValidationState
        DirectCast(CurrentModel, IModel).UpdateCellValidationState(row, field, state)
    End Sub

    Public Function GetInfo(infoItem As ModelInfo) As Object Implements IModelCore.GetInfo
        Return DirectCast(CurrentModel, IModel).GetInfo(infoItem)
    End Function

    Public Sub AddColumns(columns() As ModelColumn) Implements IModelCore.AddColumns
        DirectCast(CurrentModel, IModel).AddColumns(columns)
    End Sub

    Public Sub UpdateColumn(indexes() As Integer, columns() As ModelColumn) Implements IModelCore.UpdateColumn
        DirectCast(CurrentModel, IModel).UpdateColumn(indexes, columns)
    End Sub

    Public Sub RemoveColumns(indexes() As Integer) Implements IModelCore.RemoveColumns
        DirectCast(CurrentModel, IModel).RemoveColumns(indexes)
    End Sub

    Public Function GetColumns() As ModelColumn() Implements IModelCore.GetColumns
        Return DirectCast(CurrentModel, IModel).GetColumns()
    End Function

    Public Function GetColumns(columnNames() As String) As ModelColumn() Implements IModelCore.GetColumns
        Return DirectCast(CurrentModel, IModel).GetColumns(columnNames)
    End Function

    Public Function AddRows(data() As IDictionary(Of String, Object)) As Integer() Implements IModelCore.AddRows
        Return DirectCast(CurrentModel, IModel).AddRows(data)
    End Function

    Public Sub InsertRows(rows() As Integer, data() As IDictionary(Of String, Object)) Implements IModelCore.InsertRows
        DirectCast(CurrentModel, IModel).InsertRows(rows, data)
    End Sub

    Public Sub RemoveRows(rows() As Integer) Implements IModelCore.RemoveRows
        DirectCast(CurrentModel, IModel).RemoveRows(rows)
    End Sub

    Public Sub UpdateRows(rows() As Integer, dataOfEachRow() As IDictionary(Of String, Object)) Implements IModelCore.UpdateRows
        DirectCast(CurrentModel, IModel).UpdateRows(rows, dataOfEachRow)
    End Sub

    Public Sub UpdateCells(rows() As Integer, columnNames() As String, dataOfEachCell() As Object) Implements IModelCore.UpdateCells
        DirectCast(CurrentModel, IModel).UpdateCells(rows, columnNames, dataOfEachCell)
    End Sub

    Public Sub UpdateRowStates(rows() As Integer, states() As ModelRowState) Implements IModelCore.UpdateRowStates
        DirectCast(CurrentModel, IModel).UpdateRowStates(rows, states)
    End Sub

    Public Function GetRowStates(rows() As Integer) As ModelRowState() Implements IModelCore.GetRowStates
        Return DirectCast(CurrentModel, IModel).GetRowStates(rows)
    End Function

    Public Function GetCellStates(rows() As Integer, fields() As String) As ModelCellState() Implements IModelCore.GetCellStates
        Return DirectCast(CurrentModel, IModel).GetCellStates(rows, fields)
    End Function

    Public Sub UpdateCellStates(rows() As Integer, fields() As String, states() As ModelCellState) Implements IModelCore.UpdateCellStates
        DirectCast(CurrentModel, IModel).UpdateCellStates(rows, fields, states)
    End Sub

    Public Function GetRows(rows() As Integer) As IDictionary(Of String, Object)() Implements IModelCore.GetRows
        Return DirectCast(CurrentModel, IModel).GetRows(rows)
    End Function

    Public Function GetCells(rows() As Integer, columnNames() As String) As Object() Implements IModelCore.GetCells
        Return DirectCast(CurrentModel, IModel).GetCells(rows, columnNames)
    End Function

    Public Shadows Sub Refresh(args As ModelRefreshArgs) Implements IModelCore.Refresh
        DirectCast(CurrentModel, IModel).Refresh(args)
    End Sub

    Public Function GetRowCount() As Integer Implements IModelCore.GetRowCount
        Return DirectCast(CurrentModel, IModel).GetRowCount()
    End Function

    Public Function GetColumnCount() As Integer Implements IModelCore.GetColumnCount
        Return DirectCast(CurrentModel, IModel).GetColumnCount()
    End Function

    Public Function GetSelectionRanges() As Range() Implements IModelCore.GetSelectionRanges
        Return DirectCast(CurrentModel, IModel).GetSelectionRanges()
    End Function

    Public Sub SetSelectionRanges(ranges() As Range) Implements IModelCore.SetSelectionRanges
        DirectCast(CurrentModel, IModel).SetSelectionRanges(ranges)
    End Sub
End Class

Public Class ModelCollection
    Inherits CollectionBase

    Default Public Property Item(modelName As String) As Model
        Get
            Return Me.GetModel(modelName)
        End Get
        Set(value As Model)
            Call Me.SetModel(value)
        End Set
    End Property

    Public Function GetModel(modelName As String) As Model
        If Not Me.Contains(modelName) Then
            Throw New FrontWorkException($"Model ""{modelName}"" not exist in ModelCollection!")
        End If
        For Each model As Model In Me.InnerList
            If model.Name.Equals(modelName, StringComparison.OrdinalIgnoreCase) Then
                Return model
            End If
        Next
        Return Nothing
    End Function

    Public Sub SetModel(model As Model)
        For i = 0 To Me.InnerList.Count - 1
            Dim curModel As Model = Me.InnerList(i)
            If curModel.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase) Then
                Me.InnerList(i) = model
                RaiseEvent ModelCollectionChanged(Me, New ModelCollectionChangedEventArgs)
                Return
            End If
        Next
        Me.InnerList.Add(model)
        RaiseEvent ModelCollectionChanged(Me, New ModelCollectionChangedEventArgs)
    End Sub

    Public Function Contains(modelName As String) As Boolean
        For Each curModel As Model In Me.InnerList
            If curModel.Name.Equals(modelName, StringComparison.OrdinalIgnoreCase) Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Event ModelCollectionChanged As EventHandler(Of ModelCollectionChangedEventArgs)
End Class
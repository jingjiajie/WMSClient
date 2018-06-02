Imports System.ComponentModel
Imports System.Linq
Imports FrontWork

Public Class ModelBox
    Implements IModel
    Private _currentModelName As String
    Private _configuration As Configuration
    Private _mode As String = "default"

    Private _models As New ModelCollection

    Public Shadows Property Name As String Implements IModel.Name
        Get
            Return MyBase.Name
        End Get
        Set(value As String)
            MyBase.Name = value
        End Set
    End Property

    <DesignerSerializationVisibility(False)>
    Public ReadOnly Property Models As ModelCollection
        Get
            Return Me._models
        End Get
    End Property

    ''' <summary>
    ''' 当前模式
    ''' </summary>
    ''' <returns></returns>
    <Description("当前配置模式"), Category("FrontWork")>
    Public Property Mode As String Implements IModel.Mode
        Get
            Return Me._mode
        End Get
        Set(value As String)
            Me._mode = value
            For Each model As IModel In Me._models
                model.Mode = Me._mode
            Next
        End Set
    End Property

    <Description("当前模型名称"), Category("FrontWork")>
    Public Property CurrentModelName As String
        Get
            Return Me._currentModelName
        End Get
        Set(value As String)
            If value Is Nothing Then
                Throw New FrontWorkException("ModelName cannot be null!")
            End If
            Dim targetModel As IModel = Nothing
            If Not Me._models.Contains(value) Then
                Dim newModel As New Model
                newModel.Name = value
                newModel.Configuration = Me.Configuration
                targetModel = newModel
                Me._models.SetModel(newModel)
            Else
                targetModel = Me._models(value)
            End If
            If Me._currentModelName IsNot Nothing Then Call Me.UnBindModel(targetModel)
            Me._currentModelName = value
            If Me._currentModelName IsNot Nothing Then Call Me.BindModel(targetModel)
            RaiseEvent SelectedModelChangedEvent(Me, New SelectedModelChangedEventArgs)
            Call Me.RaiseRefreshedEvent(Me, New ModelRefreshedEventArgs)
        End Set
    End Property

    <Description("配置中心对象"), Category("FrontWork")>
    Public Property Configuration As Configuration Implements IModel.Configuration
        Get
            Return Me._configuration
        End Get
        Set(value As Configuration)
            Me._configuration = value
            Call Me.ConfigurationChanged(Me, New ConfigurationChangedEventArgs)
        End Set
    End Property

    Public Sub New()
        If Not Me.DesignMode Then Me.Visible = False
        Me.CurrentModelName = "default"
        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        AddHandler Me.Models.ModelCollectionChanged,
            Sub(sender, e)
                RaiseEvent ModelCollectionChangedEvent(sender, e)
            End Sub
    End Sub

    Private Sub BindModel(model As IModel)
        AddHandler model.Refreshed, AddressOf Me.RaiseRefreshedEvent
        AddHandler model.RowAdded, AddressOf Me.RaiseRowAddedEvent
        AddHandler model.RowUpdated, AddressOf Me.RaiseRowUpdatedEvent
        AddHandler model.BeforeRowRemove, AddressOf Me.RaiseBeforeRowRemoveEvent
        AddHandler model.RowRemoved, AddressOf Me.RaiseRowRemovedEvent
        AddHandler model.CellUpdated, AddressOf Me.RaiseCellUpdatedEvent
        AddHandler model.SelectionRangeChanged, AddressOf Me.RaiseSelectionRangeChangedEvent
        AddHandler model.RowSynchronizationStateChanged, AddressOf Me.RaiseRowSynchronizationStateChangedEvent
    End Sub

    Private Sub UnBindModel(model As IModel)
        RemoveHandler model.Refreshed, AddressOf Me.RaiseRefreshedEvent
        RemoveHandler model.RowAdded, AddressOf Me.RaiseRowAddedEvent
        RemoveHandler model.RowUpdated, AddressOf Me.RaiseRowUpdatedEvent
        RemoveHandler model.BeforeRowRemove, AddressOf Me.RaiseBeforeRowRemoveEvent
        RemoveHandler model.RowRemoved, AddressOf Me.RaiseRowRemovedEvent
        RemoveHandler model.CellUpdated, AddressOf Me.RaiseCellUpdatedEvent
        RemoveHandler model.SelectionRangeChanged, AddressOf Me.RaiseSelectionRangeChangedEvent
        RemoveHandler model.RowSynchronizationStateChanged, AddressOf Me.RaiseRowSynchronizationStateChangedEvent
    End Sub

    '''' <summary>
    '''' 是否包含指定名称的Model
    '''' </summary>
    '''' <param name="modelName">Model名称</param>
    '''' <returns>是否包含此Model</returns>
    'Public Function ContainsModel(modelName As String) As Boolean
    '    Return Me.dicModels.ContainsKey(modelName)
    'End Function

    Private Sub ConfigurationChanged(sender As Object, args As ConfigurationChangedEventArgs)
        For Each model In Me.Models
            model.Configuration = Me.Configuration
        Next
    End Sub

    Public ReadOnly Property RowCount As Integer Implements IModel.RowCount
        Get
            Return Me.GetCurModel.RowCount
        End Get
    End Property

    Public ReadOnly Property ColumnCount As Integer Implements IModel.ColumnCount
        Get
            Return Me.GetCurModel.ColumnCount
        End Get
    End Property

    Public Property AllSelectionRanges As Range() Implements IModel.AllSelectionRanges
        Get
            Return Me.GetCurModel.AllSelectionRanges
        End Get
        Set(value As Range())
            Me.GetCurModel.AllSelectionRanges = value
        End Set
    End Property

    Public Property SelectionRange As Range Implements IModel.SelectionRange
        Get
            Return Me.GetCurModel.SelectionRange
        End Get
        Set(value As Range)
            Me.GetCurModel.SelectionRange = value
        End Set
    End Property

    Default Public Property Item(row As Integer, column As Integer) As Object Implements IModel.Item
        Get
            Return Me.GetCurModel(row, column)
        End Get
        Set(value As Object)
            Me.GetCurModel(row, column) = value
        End Set
    End Property

    Default Public Property Item(row As Integer, columnName As String) As Object Implements IModel.Item
        Get
            Return Me.GetCurModel(row, columnName)
        End Get
        Set(value As Object)
            Me.GetCurModel(row, columnName) = value
        End Set
    End Property

    'IModel的事件
    Public Event Refreshed As EventHandler(Of ModelRefreshedEventArgs) Implements IModel.Refreshed
    Public Event RowAdded As EventHandler(Of ModelRowAddedEventArgs) Implements IModel.RowAdded
    Public Event RowUpdated As EventHandler(Of ModelRowUpdatedEventArgs) Implements IModel.RowUpdated
    Public Event RowRemoved As EventHandler(Of ModelRowRemovedEventArgs) Implements IModel.RowRemoved
    Public Event BeforeRowRemoved As EventHandler(Of ModelBeforeRowRemoveEventArgs) Implements IModel.BeforeRowRemove
    Public Event CellUpdated As EventHandler(Of ModelCellUpdatedEventArgs) Implements IModel.CellUpdated
    Public Event SelectionRangeChanged As EventHandler(Of ModelSelectionRangeChangedEventArgs) Implements IModel.SelectionRangeChanged
    Public Event RowSynchronizationStateChanged As EventHandler(Of ModelRowSynchronizationStateChangedEventArgs) Implements IModel.RowSynchronizationStateChanged

    'ModelBox的事件
    Public Event SelectedModelChangedEvent As EventHandler(Of SelectedModelChangedEventArgs)
    Public Event ModelCollectionChangedEvent As EventHandler(Of ModelCollectionChangedEventArgs)

    Public Sub RaiseRefreshedEvent(sender As Object, e As ModelRefreshedEventArgs)
        RaiseEvent Refreshed(sender, e)
    End Sub
    Public Sub RaiseRowAddedEvent(sender As Object, e As EventArgs)
        RaiseEvent RowAdded(sender, e)
    End Sub
    Public Sub RaiseRowUpdatedEvent(sender As Object, e As EventArgs)
        RaiseEvent RowUpdated(sender, e)
    End Sub
    Public Sub RaiseBeforeRowRemoveEvent(sender As Object, e As EventArgs)
        RaiseEvent BeforeRowRemoved(sender, e)
    End Sub
    Public Sub RaiseRowRemovedEvent(sender As Object, e As EventArgs)
        RaiseEvent RowRemoved(sender, e)
    End Sub
    Public Sub RaiseCellUpdatedEvent(sender As Object, e As EventArgs)
        RaiseEvent CellUpdated(sender, e)
    End Sub
    Public Sub RaiseSelectionRangeChangedEvent(sender As Object, e As EventArgs)
        RaiseEvent SelectionRangeChanged(sender, e)
    End Sub
    Public Sub RaiseRowSynchronizationStateChangedEvent(sender As Object, e As EventArgs)
        RaiseEvent RowSynchronizationStateChanged(sender, e)
    End Sub

    Public Sub InsertRows(rows() As Integer, data() As IDictionary(Of String, Object)) Implements IModel.InsertRows
        Me.GetCurModel.InsertRows(rows, data)
    End Sub

    Public Sub InsertRow(row As Integer, data As IDictionary(Of String, Object)) Implements IModel.InsertRow
        Me.GetCurModel.InsertRow(row, data)
    End Sub

    Public Sub RemoveRow(row As Integer) Implements IModel.RemoveRow
        Me.GetCurModel.RemoveRow(row)
    End Sub

    Public Sub RemoveRow(rowID As Guid) Implements IModel.RemoveRow
        Me.GetCurModel.RemoveRow(rowID)
    End Sub

    Public Sub RemoveRows(rows() As Integer) Implements IModel.RemoveRows
        Me.GetCurModel.RemoveRows(rows)
    End Sub

    Public Sub RemoveRows(rowIDs() As Guid) Implements IModel.RemoveRows
        Me.GetCurModel.RemoveRows(rowIDs)
    End Sub

    Public Sub RemoveRows(startRow As Integer, rowCount As Integer) Implements IModel.RemoveRows
        Me.GetCurModel.RemoveRows(startRow, rowCount)
    End Sub

    Public Sub RemoveSelectedRows() Implements IModel.RemoveSelectedRows
        Call Me.GetCurModel.RemoveSelectedRows()
    End Sub

    Public Sub UpdateRow(row As Integer, data As IDictionary(Of String, Object)) Implements IModel.UpdateRow
        Call Me.GetCurModel.UpdateRow(row, data)
    End Sub

    Public Sub UpdateRow(rowID As Guid, data As IDictionary(Of String, Object)) Implements IModel.UpdateRow
        Me.GetCurModel.UpdateRow(rowID, data)
    End Sub

    Public Sub UpdateRows(rows() As Integer, dataOfEachRow() As IDictionary(Of String, Object)) Implements IModel.UpdateRows
        Me.GetCurModel.UpdateRows(rows, dataOfEachRow)
    End Sub

    Public Sub UpdateRows(rowIDs() As Guid, dataOfEachRow() As IDictionary(Of String, Object)) Implements IModel.UpdateRows
        Me.GetCurModel.UpdateRows(rowIDs, dataOfEachRow)
    End Sub

    Public Sub UpdateCells(rows() As Integer, columnNames() As String, dataOfEachCell() As Object) Implements IModel.UpdateCells
        Me.GetCurModel.UpdateCells(rows, columnNames, dataOfEachCell)
    End Sub

    Public Sub UpdateCells(rowID() As Guid, columnNames() As String, dataOfEachCell() As Object) Implements IModel.UpdateCells
        Me.GetCurModel.UpdateCells(rowID, columnNames, dataOfEachCell)
    End Sub

    Public Sub UpdateCell(row As Integer, columnName As String, data As Object) Implements IModel.UpdateCell
        Me.GetCurModel.UpdateCell(row, columnName, data)
    End Sub

    Public Sub UpdateCell(rowID As Guid, columnName As String, data As Object) Implements IModel.UpdateCell
        Me.GetCurModel.UpdateCell(rowID, columnName, data)
    End Sub

    Public Sub UpdateRowSynchronizationStates(rows() As Guid, syncStates() As SynchronizationState) Implements IModel.UpdateRowSynchronizationStates
        Me.GetCurModel.UpdateRowSynchronizationStates(rows, syncStates)
    End Sub

    Public Sub UpdateRowSynchronizationStates(rows() As Integer, syncStates() As SynchronizationState) Implements IModel.UpdateRowSynchronizationStates
        Me.GetCurModel.UpdateRowSynchronizationStates(rows, syncStates)
    End Sub

    Public Sub UpdateRowSynchronizationState(row As Guid, syncState As SynchronizationState) Implements IModel.UpdateRowSynchronizationState
        Me.GetCurModel.UpdateRowSynchronizationState(row, syncState)
    End Sub

    Public Sub UpdateRowSynchronizationState(row As Integer, syncState As SynchronizationState) Implements IModel.UpdateRowSynchronizationState
        Me.GetCurModel.UpdateRowSynchronizationState(row, syncState)
    End Sub

    Public Shadows Sub Refresh(dataTable As DataTable, selectionRange() As Range, syncStates() As SynchronizationState) Implements IModel.Refresh
        Me.GetCurModel.Refresh(dataTable, selectionRange, syncStates)
    End Sub

    Private Sub ModelBook_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Function AddRow(data As IDictionary(Of String, Object)) As Integer Implements IModel.AddRow
        Return Me.GetCurModel.AddRow(data)
    End Function

    Public Function AddRows(data() As IDictionary(Of String, Object)) As Integer() Implements IModel.AddRows
        Return Me.GetCurModel.AddRows(data)
    End Function

    Public Function GetRowSynchronizationStates(rows() As Guid) As SynchronizationState() Implements IModel.GetRowSynchronizationStates
        Return Me.GetCurModel.GetRowSynchronizationStates(rows)
    End Function

    Public Function GetRowSynchronizationStates(rows() As Integer) As SynchronizationState() Implements IModel.GetRowSynchronizationStates
        Return Me.GetCurModel.GetRowSynchronizationStates(rows)
    End Function

    Public Function GetRowSynchronizationState(row As Integer) As SynchronizationState Implements IModel.GetRowSynchronizationState
        Return Me.GetCurModel.GetRowSynchronizationState(row)
    End Function

    Public Function GetRowSynchronizationState(row As Guid) As SynchronizationState Implements IModel.GetRowSynchronizationState
        Return Me.GetCurModel.GetRowSynchronizationState(row)
    End Function

    Public Function GetRowID(rowNum As Integer) As Guid Implements IModel.GetRowID
        Return Me.GetCurModel.GetRowID(rowNum)
    End Function

    Public Function GetRowIDs(rowNums() As Integer) As Guid() Implements IModel.GetRowIDs
        Return Me.GetCurModel.GetRowIDs(rowNums)
    End Function

    Public Function GetRowIndex(rowID As Guid) As Integer Implements IModel.GetRowIndex
        Return Me.GetCurModel.GetRowIndex(rowID)
    End Function

    Public Sub UpdateRowID(oriRowID As Guid, newID As Guid) Implements IModel.UpdateRowID
        Me.GetCurModel.UpdateRowID(oriRowID, newID)
    End Sub

    Function GetRows(Of T As New)(rows As Integer()) As T() Implements IModel.GetRows
        Return Me.GetCurModel.GetRows(Of T)(rows)
    End Function

    Function GetRow(Of T As New)(row As Integer) As T Implements IModel.GetRow
        Return Me.GetCurModel.GetRow(Of T)(row)
    End Function

    Public Function GetRows(rows() As Integer) As IDictionary(Of String, Object)() Implements IModel.GetRows
        Return Me.GetCurModel.GetRows(rows)
    End Function

    Public Function GetRows(rowIDs() As Guid) As IDictionary(Of String, Object)() Implements IModel.GetRows
        Return Me.GetCurModel.GetRows(rowIDs)
    End Function

    Public Function GetDataTable() As DataTable Implements IModel.GetDataTable
        Return Me.GetCurModel.GetDataTable
    End Function

    Private Function GetCurModel() As IModel
        If Not Me.Models.Contains(Me.CurrentModelName) Then
            Throw New FrontWorkException($"ModelBook doesn't have model:""{Me.CurrentModelName}""")
        End If
        Return Me.Models(Me.CurrentModelName)
    End Function

    Public Function GetCell(row As Integer, column As Integer) As Object Implements IModel.GetCell
        Return Me.GetCurModel.GetCell(row, column)
    End Function

    Public Function GetCell(row As Integer, columnName As String) As Object Implements IModel.GetCell
        Return Me.GetCurModel.GetCell(row, columnName)
    End Function

    Public Function ContainsColumn(columnName As String) As Boolean Implements IModel.ContainsColumn
        Return Me.GetCurModel.ContainsColumn(columnName)
    End Function

    Public Sub GroupBy(fieldName As String)
        Dim datatable = Me.GetCurModel.GetDataTable
        If Not datatable.Columns.Contains(fieldName) Then
            Throw New FrontWorkException($"""{fieldName}"" not exist in {Me.Name}")
        End If
        Dim groups = (From row In datatable.AsEnumerable
                      Group By key = CStr(If(row(fieldName), "")) Into g = Group
                      Select New With {.Key = key, .Values = g}).ToArray
        Call Me.Models.Clear() '清空所有Model
        For Each group In groups
            Dim groupName = group.Key
            Dim rows = group.Values
            '创建新的Model
            Dim newDataTable = datatable.Clone
            For Each row In rows
                newDataTable.Rows.Add(row.ItemArray)
            Next
            Dim newModel = New Model
            newModel.Refresh(newDataTable, {New Range(0, 0, 1, 1)}, Nothing)
            newModel.Name = groupName
            Me.Models.SetModel(newModel)
        Next
        If groups.Length > 0 Then
            Dim firstGroup = groups(0)
            Me.CurrentModelName = firstGroup.Key
        Else
            Me._currentModelName = Nothing
        End If
    End Sub

    Public Function GetRow(row As Integer) As IDictionary(Of String, Object) Implements IModel.GetRow
        Return Me.GetCurModel.GetRow(row)
    End Function
End Class

Public Class ModelCollection
    Inherits CollectionBase

    Default Public Property Item(modelName As String) As IModel
        Get
            Return Me.GetModel(modelName)
        End Get
        Set(value As IModel)
            Call Me.SetModel(value)
        End Set
    End Property

    Public Function GetModel(modelName As String) As IModel
        If Not Me.Contains(modelName) Then
            Throw New FrontWorkException($"Model ""{modelName}"" not exist in ModelCollection!")
        End If
        For Each model As IModel In Me.InnerList
            If model.Name.Equals(modelName, StringComparison.OrdinalIgnoreCase) Then
                Return model
            End If
        Next
        Return Nothing
    End Function

    Public Sub SetModel(model As IModel)
        For i = 0 To Me.InnerList.Count - 1
            Dim curModel As IModel = Me.InnerList(i)
            If curModel.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase) Then
                Me.InnerList(i) = model
                RaiseEvent ModelCollectionChanged(Me, New ModelCollectionChangedEventArgs)
                Return
            End If
        Next
        Me.InnerList.Add(model)
        RaiseEvent ModelCollectionChanged(Me, New ModelCollectionChangedEventArgs)
    End Sub

    Public Sub Remove(model As IModel)
        Call Me.InnerList.Remove(model)
        RaiseEvent ModelCollectionChanged(Me, New ModelCollectionChangedEventArgs)
    End Sub

    Public Function Contains(modelName As String) As Boolean
        For Each model As IModel In Me.InnerList
            If model.Name.Equals(modelName, StringComparison.OrdinalIgnoreCase) Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Event ModelCollectionChanged As EventHandler(Of ModelCollectionChangedEventArgs)
End Class
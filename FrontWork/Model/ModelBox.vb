Imports System.ComponentModel
Imports System.Linq
Imports FrontWork

Public Class ModelBox
    Implements IModel
    Private _currentModelName As String
    Private _configuration As Configuration
    Private _mode As String = "default"

    Private dicModels As New Dictionary(Of String, IModel)

    Public Shadows Property Name As String Implements IModel.Name
        Get
            Return MyBase.Name
        End Get
        Set(value As String)
            MyBase.Name = value
        End Set
    End Property

    <DesignerSerializationVisibility(False)>
    Public ReadOnly Property Models As IModel()
        Get
            Return (From m In Me.dicModels.Values Select m).ToArray
        End Get
    End Property

    <DesignerSerializationVisibility(False)>
    Public ReadOnly Property Models(name As String) As IModel
        Get
            If Not Me.dicModels.ContainsKey(name) Then
                Throw New Exception($"Modelbox doesn't contain model:{name}")
            End If
            Return Me.dicModels(name)
        End Get
    End Property

    <DesignerSerializationVisibility(False)>
    Public ReadOnly Property Models(index As Integer) As IModel
        Get
            Dim _models = Me.Models
            If _models.Length <= index Then
                Throw New Exception($"ModelBox {Me.Name} doesn't have model {index}")
            End If
            Return _models(index)
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
            For Each model In Me.dicModels.Values
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
            If String.IsNullOrWhiteSpace(value) Then
                Throw New Exception("ModelName cannot be null or whitespace!")
            End If
            If Not Me.dicModels.ContainsKey(value) Then
                Dim newModel As New Model
                newModel.Name = value
                newModel.Configuration = Me.Configuration
                Me.dicModels.Add(value, newModel)
            End If
            If Not String.IsNullOrWhiteSpace(Me._currentModelName) Then Call Me.UnBindModel(Me.dicModels(value))
            Me._currentModelName = value
            If Not String.IsNullOrWhiteSpace(Me._currentModelName) Then Call Me.BindModel(Me.dicModels(value))
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

    End Sub

    Private Sub BindModel(model As IModel)
        AddHandler model.Refreshed, AddressOf Me.RaiseRefreshedEvent
        AddHandler model.RowAdded, AddressOf Me.RaiseRowAddedEvent
        AddHandler model.RowUpdated, AddressOf Me.RaiseRowUpdatedEvent
        AddHandler model.RowRemoved, AddressOf Me.RaiseRowRemovedEvent
        AddHandler model.CellUpdated, AddressOf Me.RaiseCellUpdatedEvent
        AddHandler model.SelectionRangeChanged, AddressOf Me.RaiseSelectionRangeChangedEvent
        AddHandler model.RowSynchronizationStateChanged, AddressOf Me.RaiseRowSynchronizationStateChangedEvent
    End Sub

    Private Sub UnBindModel(model As IModel)
        RemoveHandler model.Refreshed, AddressOf Me.RaiseRefreshedEvent
        RemoveHandler model.RowAdded, AddressOf Me.RaiseRowAddedEvent
        RemoveHandler model.RowUpdated, AddressOf Me.RaiseRowUpdatedEvent
        RemoveHandler model.RowRemoved, AddressOf Me.RaiseRowRemovedEvent
        RemoveHandler model.CellUpdated, AddressOf Me.RaiseCellUpdatedEvent
        RemoveHandler model.SelectionRangeChanged, AddressOf Me.RaiseSelectionRangeChangedEvent
        RemoveHandler model.RowSynchronizationStateChanged, AddressOf Me.RaiseRowSynchronizationStateChangedEvent
    End Sub

    Private Sub ConfigurationChanged(sender As Object, args As ConfigurationChangedEventArgs)
        For Each model In Me.dicModels.Values
            model.Configuration = Me.Configuration
        Next
    End Sub

    Public ReadOnly Property RowCount As Long Implements IModel.RowCount
        Get
            Return Me.GetCurModel.RowCount
        End Get
    End Property

    Public ReadOnly Property ColumnCount As Long Implements IModel.ColumnCount
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

    Public Property Item(row As Long, column As Long) As Object Implements IModel.Item
        Get
            Return Me.GetCurModel(row, column)
        End Get
        Set(value As Object)
            Me.GetCurModel(row, column) = value
        End Set
    End Property

    Public Property Item(row As Long, columnName As String) As Object Implements IModel.Item
        Get
            Return Me.GetCurModel(row, columnName)
        End Get
        Set(value As Object)
            Me.GetCurModel(row, columnName) = value
        End Set
    End Property

    Public Event Refreshed As EventHandler(Of ModelRefreshedEventArgs) Implements IModel.Refreshed
    Public Event RowAdded As EventHandler(Of ModelRowAddedEventArgs) Implements IModel.RowAdded
    Public Event RowUpdated As EventHandler(Of ModelRowUpdatedEventArgs) Implements IModel.RowUpdated
    Public Event RowRemoved As EventHandler(Of ModelRowRemovedEventArgs) Implements IModel.RowRemoved
    Public Event CellUpdated As EventHandler(Of ModelCellUpdatedEventArgs) Implements IModel.CellUpdated
    Public Event SelectionRangeChanged As EventHandler(Of ModelSelectionRangeChangedEventArgs) Implements IModel.SelectionRangeChanged
    Public Event RowSynchronizationStateChanged As EventHandler(Of ModelRowSynchronizationStateChangedEventArgs) Implements IModel.RowSynchronizationStateChanged

    Public Sub RaiseRefreshedEvent(sender As Object, e As ModelRefreshedEventArgs)
        RaiseEvent Refreshed(sender, e)
    End Sub
    Public Sub RaiseRowAddedEvent(sender As Object, e As EventArgs)
        RaiseEvent RowAdded(sender, e)
    End Sub
    Public Sub RaiseRowUpdatedEvent(sender As Object, e As EventArgs)
        RaiseEvent RowUpdated(sender, e)
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

    Public Sub InsertRows(rows() As Long, data() As Dictionary(Of String, Object)) Implements IModel.InsertRows
        Me.GetCurModel.InsertRows(rows, data)
    End Sub

    Public Sub InsertRow(row As Long, data As Dictionary(Of String, Object)) Implements IModel.InsertRow
        Me.GetCurModel.InsertRow(row, data)
    End Sub

    Public Sub RemoveRow(row As Long) Implements IModel.RemoveRow
        Me.GetCurModel.RemoveRow(row)
    End Sub

    Public Sub RemoveRow(rowID As Guid) Implements IModel.RemoveRow
        Me.GetCurModel.RemoveRow(rowID)
    End Sub

    Public Sub RemoveRows(rows() As Long) Implements IModel.RemoveRows
        Me.GetCurModel.RemoveRows(rows)
    End Sub

    Public Sub RemoveRows(rowIDs() As Guid) Implements IModel.RemoveRows
        Me.GetCurModel.RemoveRows(rowIDs)
    End Sub

    Public Sub RemoveRows(startRow As Long, rowCount As Long) Implements IModel.RemoveRows
        Me.GetCurModel.RemoveRows(startRow, rowCount)
    End Sub

    Public Sub RemoveSelectedRows() Implements IModel.RemoveSelectedRows
        Call Me.GetCurModel.RemoveSelectedRows()
    End Sub

    Public Sub UpdateRow(row As Long, data As Dictionary(Of String, Object)) Implements IModel.UpdateRow
        Call Me.GetCurModel.UpdateRow(row, data)
    End Sub

    Public Sub UpdateRow(rowID As Guid, data As Dictionary(Of String, Object)) Implements IModel.UpdateRow
        Me.GetCurModel.UpdateRow(rowID, data)
    End Sub

    Public Sub UpdateRows(rows() As Long, dataOfEachRow() As Dictionary(Of String, Object)) Implements IModel.UpdateRows
        Me.GetCurModel.UpdateRows(rows, dataOfEachRow)
    End Sub

    Public Sub UpdateRows(rowIDs() As Guid, dataOfEachRow() As Dictionary(Of String, Object)) Implements IModel.UpdateRows
        Me.GetCurModel.UpdateRows(rowIDs, dataOfEachRow)
    End Sub

    Public Sub UpdateCells(rows() As Long, columnNames() As String, dataOfEachCell() As Object) Implements IModel.UpdateCells
        Me.GetCurModel.UpdateCells(rows, columnNames, dataOfEachCell)
    End Sub

    Public Sub UpdateCells(rowID() As Guid, columnNames() As String, dataOfEachCell() As Object) Implements IModel.UpdateCells
        Me.GetCurModel.UpdateCells(rowID, columnNames, dataOfEachCell)
    End Sub

    Public Sub UpdateCell(row As Long, columnName As String, data As Object) Implements IModel.UpdateCell
        Me.GetCurModel.UpdateCell(row, columnName, data)
    End Sub

    Public Sub UpdateCell(rowID As Guid, columnName As String, data As Object) Implements IModel.UpdateCell
        Me.GetCurModel.UpdateCell(rowID, columnName, data)
    End Sub

    Public Sub UpdateRowSynchronizationStates(rows() As Guid, syncStates() As SynchronizationState) Implements IModel.UpdateRowSynchronizationStates
        Me.GetCurModel.UpdateRowSynchronizationStates(rows, syncStates)
    End Sub

    Public Sub UpdateRowSynchronizationStates(rows() As Long, syncStates() As SynchronizationState) Implements IModel.UpdateRowSynchronizationStates
        Me.GetCurModel.UpdateRowSynchronizationStates(rows, syncStates)
    End Sub

    Public Sub UpdateRowSynchronizationState(row As Guid, syncState As SynchronizationState) Implements IModel.UpdateRowSynchronizationState
        Me.GetCurModel.UpdateRowSynchronizationState(row, syncState)
    End Sub

    Public Sub UpdateRowSynchronizationState(row As Long, syncState As SynchronizationState) Implements IModel.UpdateRowSynchronizationState
        Me.GetCurModel.UpdateRowSynchronizationState(row, syncState)
    End Sub

    Public Shadows Sub Refresh(dataTable As DataTable, selectionRange() As Range, syncStates() As SynchronizationState) Implements IModel.Refresh
        Me.GetCurModel.Refresh(dataTable, selectionRange, syncStates)
    End Sub

    Private Sub ModelBook_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Function AddRow(data As Dictionary(Of String, Object)) As Long Implements IModel.AddRow
        Return Me.GetCurModel.AddRow(data)
    End Function

    Public Function AddRows(data() As Dictionary(Of String, Object)) As Long() Implements IModel.AddRows
        Return Me.GetCurModel.AddRows(data)
    End Function

    Public Function GetRowSynchronizationStates(rows() As Guid) As SynchronizationState() Implements IModel.GetRowSynchronizationStates
        Return Me.GetCurModel.GetRowSynchronizationStates(rows)
    End Function

    Public Function GetRowSynchronizationStates(rows() As Long) As SynchronizationState() Implements IModel.GetRowSynchronizationStates
        Return Me.GetCurModel.GetRowSynchronizationStates(rows)
    End Function

    Public Function GetRowSynchronizationState(row As Long) As SynchronizationState Implements IModel.GetRowSynchronizationState
        Return Me.GetCurModel.GetRowSynchronizationState(row)
    End Function

    Public Function GetRowSynchronizationState(row As Guid) As SynchronizationState Implements IModel.GetRowSynchronizationState
        Return Me.GetCurModel.GetRowSynchronizationState(row)
    End Function

    Public Function GetRowID(rowNum As Long) As Guid Implements IModel.GetRowID
        Return Me.GetCurModel.GetRowID(rowNum)
    End Function

    Public Function GetRowIDs(rowNums() As Long) As Guid() Implements IModel.GetRowIDs
        Return Me.GetCurModel.GetRowIDs(rowNums)
    End Function

    Public Function GetRows(rows() As Long) As DataTable Implements IModel.GetRows
        Return Me.GetCurModel.GetRows(rows)
    End Function

    Public Function GetRows(rowIDs() As Guid) As DataTable Implements IModel.GetRows
        Return Me.GetCurModel.GetRows(rowIDs)
    End Function

    Public Function GetDataTable() As DataTable Implements IModel.GetDataTable
        Return Me.GetCurModel.GetDataTable
    End Function

    Private Function GetCurModel() As IModel
        If Not Me.dicModels.ContainsKey(Me.CurrentModelName) Then
            Throw New Exception($"ModelBook doesn't have model:""{Me.CurrentModelName}""")
        End If
        Return Me.dicModels(Me.CurrentModelName)
    End Function

    Public Function GetCell(row As Long, column As Long) As Object Implements IModel.GetCell
        Return Me.GetCurModel.GetCell(row, column)
    End Function

    Public Function GetCell(row As Long, columnName As String) As Object Implements IModel.GetCell
        Return Me.GetCurModel.GetCell(row, columnName)
    End Function

    Public Sub RemoveModel(modelName As String)
        If Not Me.dicModels.ContainsKey(modelName) Then
            Throw New Exception($"Model ""{modelName}"" not exist in {Me.Name}")
        End If
        Me.dicModels.Remove(modelName)
    End Sub
End Class

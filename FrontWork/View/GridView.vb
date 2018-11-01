'Imports System.ComponentModel
'Imports FrontWork

'<ToolboxItem(False)>
'Public Class GridView
'    Implements IAssociableDataView

'    Private Class CellTag
'        Public Property SynchronizationState As SynchronizationState = SynchronizationState.SYNCHRONIZED
'    End Class

'    Private Property ViewColumns As New List(Of ViewColumn)

'    Public Event AssociationItemSelected As EventHandler(Of ViewAssociationItemSelectedEventArgs) Implements IAssociableDataView.AssociationItemSelected
'    Public Event EditStarted As EventHandler(Of ViewEditStartedEventArgs) Implements IEditableDataView.EditStarted
'    Public Event ContentChanged As EventHandler(Of ViewContentChangedEventArgs) Implements IEditableDataView.ContentChanged
'    Public Event EditEnded As EventHandler(Of ViewEditEndedEventArgs) Implements IEditableDataView.EditEnded
'    Public Event RowUpdated As EventHandler(Of ViewRowUpdatedEventArgs) Implements IEditableDataView.RowUpdated
'    Public Event RowAdded As EventHandler(Of ViewRowAddedEventArgs) Implements IEditableDataView.RowAdded
'    Public Event RowRemoved As EventHandler(Of ViewRowRemovedEventArgs) Implements IEditableDataView.RowRemoved
'    Public Event CellUpdated As EventHandler(Of ViewCellUpdatedEventArgs) Implements IEditableDataView.CellUpdated
'    Public Event BeforeSelectionRangeChange As EventHandler(Of BeforeViewSelectionRangeChangeEventArgs) Implements ISelectableDataView.BeforeSelectionRangeChange
'    Public Event SelectionRangeChanged As EventHandler(Of ViewSelectionRangeChangedEventArgs) Implements ISelectableDataView.SelectionRangeChanged
'    Public Event BeforeRowStateChange As EventHandler(Of ViewBeforeRowStateChangeEventArgs) Implements IDataView.BeforeRowStateChange
'    Public Event RowStateChanged As EventHandler(Of ViewRowStateChangedEventArgs) Implements IDataView.RowStateChanged

'    Public Sub ShowAssociationForm() Implements IAssociableDataView.ShowAssociationForm
'        Throw New NotImplementedException()
'    End Sub

'    Public Sub UpdateAssociationItems(associationItems() As AssociationItem) Implements IAssociableDataView.UpdateAssociationItems
'        Throw New NotImplementedException()
'    End Sub

'    Public Sub HideAssociationForm() Implements IAssociableDataView.HideAssociationForm
'        Throw New NotImplementedException()
'    End Sub

'    Public Sub SetSelectionRanges(ranges() As Range) Implements ISelectableDataView.SetSelectionRanges

'    End Sub

'    Public Sub InsertRows(rows() As Integer, data() As IDictionary(Of String, Object)) Implements IDataView.InsertRows
'        Throw New NotImplementedException()
'    End Sub

'    Public Sub RemoveRows(rows() As Integer) Implements IDataView.RemoveRows
'        Throw New NotImplementedException()
'    End Sub

'    Public Sub UpdateRows(rows() As Integer, dataOfEachRow() As IDictionary(Of String, Object)) Implements IDataView.UpdateRows
'        Throw New NotImplementedException()
'    End Sub

'    Public Sub UpdateCells(rows() As Integer, columnNames() As String, dataOfEachCell() As Object) Implements IDataView.UpdateCells
'        Throw New NotImplementedException()
'    End Sub

'    Public Sub UpdateRowStates(rows() As Integer, states() As ViewRowState) Implements IDataView.UpdateRowStates
'        Throw New NotImplementedException()
'    End Sub

'    Public Function GetSelectionRanges() As Range() Implements ISelectableDataView.GetSelectionRanges
'        Throw New NotImplementedException()
'    End Function

'    Public Function AddColumns(viewColumns() As ViewColumn) As Boolean Implements IDataView.AddColumns
'        Throw New NotImplementedException()
'    End Function

'    Public Function UpdateColumns(indexes() As Integer, newViewColumns() As ViewColumn) As Object Implements IDataView.UpdateColumns
'        Throw New NotImplementedException()
'    End Function

'    Public Function RemoveColumns(indexes() As Integer) As Object Implements IDataView.RemoveColumns
'        Throw New NotImplementedException()
'    End Function

'    Public Function GetColumns() As ViewColumn() Implements IDataView.GetColumns
'        Throw New NotImplementedException()
'    End Function

'    Public Function AddRows(data() As IDictionary(Of String, Object)) As Integer() Implements IDataView.AddRows
'        Throw New NotImplementedException()
'    End Function

'    Public Function GetRowStates(rows() As Integer) As ViewRowState() Implements IDataView.GetRowStates
'        Throw New NotImplementedException()
'    End Function

'    Public Function GetRowCount() As Integer Implements IDataView.GetRowCount
'        Throw New NotImplementedException()
'    End Function

'    Public Function GetColumnCount() As Integer Implements IDataView.GetColumnCount
'        Throw New NotImplementedException()
'    End Function
'End Class

Imports FrontWork

Public Class ViewOperationsWrapper
    Implements IDataView
    Public Property View As IDataView

    Public Custom Event BeforeRowUpdate As EventHandler(Of BeforeViewRowUpdateEventArgs) Implements IDataView.BeforeRowUpdate
        AddHandler(value As EventHandler(Of BeforeViewRowUpdateEventArgs))
            AddHandler View.BeforeRowUpdate, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of BeforeViewRowUpdateEventArgs))
            RemoveHandler View.BeforeRowUpdate, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As BeforeViewRowUpdateEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event BeforeRowAdd As EventHandler(Of BeforeViewRowAddEventArgs) Implements IDataView.BeforeRowAdd
        AddHandler(value As EventHandler(Of BeforeViewRowAddEventArgs))
            AddHandler View.BeforeRowAdd, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of BeforeViewRowAddEventArgs))
            RemoveHandler View.BeforeRowAdd, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As BeforeViewRowAddEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event BeforeRowRemove As EventHandler(Of BeforeViewRowRemoveEventArgs) Implements IDataView.BeforeRowRemove
        AddHandler(value As EventHandler(Of BeforeViewRowRemoveEventArgs))
            AddHandler View.BeforeRowRemove, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of BeforeViewRowRemoveEventArgs))
            RemoveHandler View.BeforeRowRemove, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As BeforeViewRowRemoveEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event BeforeCellUpdate As EventHandler(Of BeforeViewCellUpdateEventArgs) Implements IDataView.BeforeCellUpdate
        AddHandler(value As EventHandler(Of BeforeViewCellUpdateEventArgs))
            AddHandler View.BeforeCellUpdate, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of BeforeViewCellUpdateEventArgs))
            RemoveHandler View.BeforeCellUpdate, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As BeforeViewCellUpdateEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event RowUpdated As EventHandler(Of ViewRowUpdatedEventArgs) Implements IDataView.RowUpdated
        AddHandler(value As EventHandler(Of ViewRowUpdatedEventArgs))
            AddHandler View.RowUpdated, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ViewRowUpdatedEventArgs))
            RemoveHandler View.RowUpdated, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ViewRowUpdatedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event RowAdded As EventHandler(Of ViewRowAddedEventArgs) Implements IDataView.RowAdded
        AddHandler(value As EventHandler(Of ViewRowAddedEventArgs))
            AddHandler View.RowAdded, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ViewRowAddedEventArgs))
            RemoveHandler View.RowAdded, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ViewRowAddedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event RowRemoved As EventHandler(Of ViewRowRemovedEventArgs) Implements IDataView.RowRemoved
        AddHandler(value As EventHandler(Of ViewRowRemovedEventArgs))
            AddHandler View.RowRemoved, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ViewRowRemovedEventArgs))
            RemoveHandler View.RowRemoved, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ViewRowRemovedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event CellUpdated As EventHandler(Of ViewCellUpdatedEventArgs) Implements IDataView.CellUpdated
        AddHandler(value As EventHandler(Of ViewCellUpdatedEventArgs))
            AddHandler View.CellUpdated, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ViewCellUpdatedEventArgs))
            RemoveHandler View.CellUpdated, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ViewCellUpdatedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event BeforeSelectionRangeChange As EventHandler(Of BeforeViewSelectionRangeChangeEventArgs) Implements IDataView.BeforeSelectionRangeChange
        AddHandler(value As EventHandler(Of BeforeViewSelectionRangeChangeEventArgs))
            AddHandler View.BeforeSelectionRangeChange, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of BeforeViewSelectionRangeChangeEventArgs))
            RemoveHandler View.BeforeSelectionRangeChange, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As BeforeViewSelectionRangeChangeEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event SelectionRangeChanged As EventHandler(Of ViewSelectionRangeChangedEventArgs) Implements IDataView.SelectionRangeChanged
        AddHandler(value As EventHandler(Of ViewSelectionRangeChangedEventArgs))
            AddHandler View.SelectionRangeChanged, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ViewSelectionRangeChangedEventArgs))
            RemoveHandler View.SelectionRangeChanged, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ViewSelectionRangeChangedEventArgs)
        End RaiseEvent
    End Event

    Public Sub New(view As IDataView)
        Me.View = view
    End Sub

    Public Sub New()

    End Sub

    Public Sub InsertRows(rows() As Integer, data() As IDictionary(Of String, Object)) Implements IDataView.InsertRows
        View.InsertRows(rows, data)
    End Sub

    Public Sub InsertRow(row As Integer, data As IDictionary(Of String, Object))
        Call Me.InsertRows({row}, {data})
    End Sub

    Public Sub RemoveRows(rows() As Integer) Implements IDataView.RemoveRows
        View.RemoveRows(rows)
    End Sub

    Public Sub RemoveRow(row As Integer)
        Call Me.RemoveRows({row})
    End Sub

    Public Sub UpdateRows(rows() As Integer, dataOfEachRow() As IDictionary(Of String, Object)) Implements IDataView.UpdateRows
        View.UpdateRows(rows, dataOfEachRow)
    End Sub

    Public Sub UpdateRow(row As Integer, data As IDictionary(Of String, Object))
        Call Me.UpdateRows({row}, {data})
    End Sub

    Public Sub UpdateCells(rows() As Integer, columnNames() As String, dataOfEachCell() As Object) Implements IDataView.UpdateCells
        View.UpdateCells(rows, columnNames, dataOfEachCell)
    End Sub

    Public Sub SetSelectionRanges(ranges() As Range) Implements IDataView.SetSelectionRanges
        View.SetSelectionRanges(ranges)
    End Sub

    Public Sub SetSelectionRange(range As Range)
        Call Me.SetSelectionRanges({range})
    End Sub

    Public Function AddRows(data() As IDictionary(Of String, Object)) As Integer() Implements IDataView.AddRows
        Return View.AddRows(data)
    End Function

    Public Function AddRow(data As IDictionary(Of String, Object)) As Integer
        Return Me.AddRows({data})(0)
    End Function

    Public Function GetSelectionRanges() As Range() Implements IDataView.GetSelectionRanges
        Return View.GetSelectionRanges()
    End Function

    Public Function GetSelectionRange() As Range
        Dim ranges = Me.GetSelectionRanges
        If ranges Is Nothing OrElse ranges.Length = 0 Then
            Return Nothing
        Else
            Return ranges(0)
        End If
    End Function

    Public Function AddColumns(columns() As ViewColumn) As Boolean Implements IDataView.AddColumns
        Return View.AddColumns(columns)
    End Function

    Public Function RemoveColumns(columnNames() As String) As Object Implements IDataView.RemoveColumns
        Return View.RemoveColumns(columnNames)
    End Function

    Public Function UpdateColumns(columnNames() As String, columns() As ViewColumn) As Object Implements IDataView.UpdateColumns
        Return View.UpdateColumns(columnNames, columns)
    End Function

    Public Function GetColumns() As ViewColumn() Implements IDataView.GetColumns
        Return View.GetColumns()
    End Function

    Public Function GetRowCount() As Integer Implements IDataView.GetRowCount
        Return View.GetRowCount()
    End Function

    Public Function GetColumnCount() As Integer Implements IDataView.GetColumnCount
        Return View.GetColumnCount()
    End Function
End Class

Imports FrontWork

Public Class DataViewOperatorBase
    Implements IDataView

    Public Overridable Property View As IDataView

    Public Sub New(view As IDataView)
        Me.View = view
    End Sub

    Public Sub New()

    End Sub

    Public Custom Event BeforeRowStateChange As EventHandler(Of ViewBeforeRowStateChangeEventArgs) Implements IDataView.BeforeRowStateChange
        AddHandler(value As EventHandler(Of ViewBeforeRowStateChangeEventArgs))
            AddHandler View.BeforeRowStateChange, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ViewBeforeRowStateChangeEventArgs))
            RemoveHandler View.BeforeRowStateChange, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ViewBeforeRowStateChangeEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event RowStateChanged As EventHandler(Of ViewRowStateChangedEventArgs) Implements IDataView.RowStateChanged
        AddHandler(value As EventHandler(Of ViewRowStateChangedEventArgs))
            AddHandler View.RowStateChanged, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ViewRowStateChangedEventArgs))
            RemoveHandler View.RowStateChanged, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ViewRowStateChangedEventArgs)
        End RaiseEvent
    End Event

    Public Overridable Function AddColumns(viewColumns() As ViewColumn) As Boolean Implements IDataView.AddColumns
        Return View.AddColumns(viewColumns)
    End Function

    Public Overridable Function UpdateColumns(indexes() As Integer, newViewColumns() As ViewColumn) As Object Implements IDataView.UpdateColumns
        Return View.UpdateColumns(indexes, newViewColumns)
    End Function

    Public Overridable Function RemoveColumns(indexes() As Integer) As Object Implements IDataView.RemoveColumns
        Return View.RemoveColumns(indexes)
    End Function

    Public Overridable Function GetColumns() As ViewColumn() Implements IDataView.GetColumns
        Return View.GetColumns()
    End Function

    Public Overridable Function AddRows(data() As IDictionary(Of String, Object)) As Integer() Implements IDataView.AddRows
        Return View.AddRows(data)
    End Function

    Public Overridable Sub InsertRows(rows() As Integer, data() As IDictionary(Of String, Object)) Implements IDataView.InsertRows
        View.InsertRows(rows, data)
    End Sub

    Public Overridable Sub RemoveRows(rows() As Integer) Implements IDataView.RemoveRows
        View.RemoveRows(rows)
    End Sub

    Public Overridable Sub UpdateRows(rows() As Integer, dataOfEachRow() As IDictionary(Of String, Object)) Implements IDataView.UpdateRows
        View.UpdateRows(rows, dataOfEachRow)
    End Sub

    Public Overridable Sub UpdateCells(rows() As Integer, columnNames() As String, dataOfEachCell() As Object) Implements IDataView.UpdateCells
        View.UpdateCells(rows, columnNames, dataOfEachCell)
    End Sub

    Public Overridable Function GetRowCount() As Integer Implements IDataView.GetRowCount
        Return View.GetRowCount()
    End Function

    Public Overridable Function GetColumnCount() As Integer Implements IDataView.GetColumnCount
        Return View.GetColumnCount()
    End Function

    Public Sub UpdateRowStates(rows() As Integer, states() As ViewRowState) Implements IDataView.UpdateRowStates
        Call Me.View.UpdateRowStates(rows, states)
    End Sub

    Public Function GetRowStates(rows() As Integer) As ViewRowState() Implements IDataView.GetRowStates
        Return View.GetRowStates(rows)
    End Function

    Public Function GetCellStates(rows() As Integer, fields() As String) As ViewCellState() Implements IDataView.GetCellStates
        Return View.GetCellStates(rows, fields)
    End Function

    Public Sub UpdateCellStates(rows() As Integer, fields() As String, states As ViewCellState()) Implements IDataView.UpdateCellStates
        View.UpdateCellStates(rows, fields, states)
    End Sub
End Class

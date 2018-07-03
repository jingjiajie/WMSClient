Imports FrontWork

Public Class DataViewOperationsWrapperBase
    Implements IDataView

    Public Overridable Property View As IDataView

    Public Sub New(view As IDataView)
        Me.View = view
    End Sub

    Public Sub New()

    End Sub

    Public Overridable Function AddColumns(viewColumns() As ViewColumn) As Boolean Implements IDataView.AddColumns
        Return View.AddColumns(viewColumns)
    End Function

    Public Overridable Function UpdateColumns(oriColumnNames() As String, newViewColumns() As ViewColumn) As Object Implements IDataView.UpdateColumns
        Return View.UpdateColumns(oriColumnNames, newViewColumns)
    End Function

    Public Overridable Function RemoveColumns(columnNames() As String) As Object Implements IDataView.RemoveColumns
        Return View.RemoveColumns(columnNames)
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

    Public Sub UpdateRowSynchronizationStates(rows() As Integer, synchronizationStates() As SynchronizationState) Implements IDataView.UpdateRowSynchronizationStates
        Call Me.View.UpdateRowSynchronizationStates(rows, synchronizationStates)
    End Sub
End Class

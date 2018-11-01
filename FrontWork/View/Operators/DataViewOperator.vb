Imports FrontWork

Public Class DataViewOperator
    Inherits DataViewOperatorBase
    Implements IDataView

    Public Overridable Sub InsertRow(row As Integer, data As IDictionary(Of String, Object))
        Call Me.InsertRows({row}, {data})
    End Sub

    Public Overridable Sub RemoveRow(row As Integer)
        Call Me.RemoveRows({row})
    End Sub

    Public Overridable Sub UpdateRow(row As Integer, data As IDictionary(Of String, Object))
        Call Me.UpdateRows({row}, {data})
    End Sub

    Public Overridable Function AddRow(data As IDictionary(Of String, Object)) As Integer
        Return Me.AddRows({data})(0)
    End Function

    Public Overridable Sub UpdateCell(row As Integer, columnName As String, data As Object)
        Call Me.UpdateCells({row}, {columnName}, {data})
    End Sub

End Class

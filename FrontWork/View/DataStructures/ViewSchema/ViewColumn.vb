Public Class ViewColumn
    Public Property Name As String
    Public Property Type As Type = GetType(String)
    Public Property Values As FieldMethod

    Public Shared Operator =(viewColumn1 As ViewColumn, viewColumn2 As ViewColumn) As Boolean
        If viewColumn1 Is viewColumn2 Then Return True
        If viewColumn1 Is Nothing OrElse viewColumn2 Is Nothing Then Return False
        If viewColumn1.Name <> viewColumn2.Name Then Return False
        If viewColumn1.Type <> viewColumn2.Type Then Return False
        If viewColumn1.Values <> viewColumn2.Values Then Return False
        Return True
    End Operator

    Public Shared Operator <>(viewColumn1 As ViewColumn, viewColumn2 As ViewColumn) As Boolean
        Return Not viewColumn1 = viewColumn2
    End Operator
End Class

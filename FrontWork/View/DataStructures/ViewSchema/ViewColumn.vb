Public Class ViewColumn
    Public Property Name As String
    Public Property DisplayName As String
    Public Property Type As Type = GetType(String)
    Public Property Editable As Boolean = False
    Public Property PlaceHolder As FieldMethod
    Public Property Values As FieldMethod

    Public Shared Operator =(viewColumn1 As ViewColumn, viewColumn2 As ViewColumn) As Boolean
        If viewColumn1 Is viewColumn2 Then Return True
        If viewColumn1 Is Nothing OrElse viewColumn2 Is Nothing Then Return False
        If viewColumn1.Name <> viewColumn2.Name Then Return False
        If viewColumn1.DisplayName <> viewColumn2.DisplayName Then Return False
        If viewColumn1.Editable <> viewColumn2.Editable Then Return False
        If viewColumn1.PlaceHolder <> viewColumn2.PlaceHolder Then Return False
        If viewColumn1.Type <> viewColumn2.Type Then Return False
        If viewColumn1.Values <> viewColumn2.Values Then Return False
        Return True
    End Operator

    Public Shared Operator <>(viewColumn1 As ViewColumn, viewColumn2 As ViewColumn) As Boolean
        Return Not viewColumn1 = viewColumn2
    End Operator
End Class

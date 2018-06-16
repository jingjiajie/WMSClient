Public Class ModelColumn
    Public Property Name As String
    Public Property Type As Type = GetType(String)
    Public Property Nullable As Boolean = True
    Public Property DefaultValue As FieldMethod = Nothing
End Class

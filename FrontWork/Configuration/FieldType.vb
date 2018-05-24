Public Class FieldType
    Public Property FieldType As Type

    Public Shared Function FromString(typeName As String) As FieldType
        If String.IsNullOrWhiteSpace(typeName) Then
            Throw New FrontWorkException("FieldType cannot be null!")
        End If
        Dim newInstance As New FieldType
        If typeName.Equals("int", StringComparison.OrdinalIgnoreCase) Then
            newInstance.FieldType = GetType(Integer)
        ElseIf typeName.Equals("double", StringComparison.OrdinalIgnoreCase) Then
            newInstance.FieldType = GetType(Double)
        ElseIf typeName.Equals("string", StringComparison.OrdinalIgnoreCase) Then
            newInstance.FieldType = GetType(String)
        ElseIf typeName.Equals("bool", StringComparison.OrdinalIgnoreCase) Then
            newInstance.FieldType = GetType(Boolean)
        ElseIf typeName.Equals("datetime", StringComparison.OrdinalIgnoreCase) Then
            newInstance.FieldType = GetType(DateTime)
        Else
            Throw New FrontWorkException($"Type {typeName} is not supported" & vbCrLf &
                                         "now only ""int"",""double"",""string"",""bool"",""datetime"" are supported.")
        End If
        Return newInstance
    End Function
End Class

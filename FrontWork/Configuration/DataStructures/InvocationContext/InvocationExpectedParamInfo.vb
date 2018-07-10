Public Class InvocationExpectedParameterInfo
    Public Sub New(paramName As String, paramType As Type, attributeType As Type)
        Me.AttributeType = attributeType
        Me.ValueType = paramType
        Me.ParameterName = paramName
    End Sub

    Public Property AttributeType As Type
    Public Property ValueType As Type
    Public Property ParameterName As String

End Class
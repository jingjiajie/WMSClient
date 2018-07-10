Public Class InvocationContextItem
    Public Property Value As Object
    Public Property AttributeType As Type

    Public Sub New(value As Object, attributeType As Type)
        Me.Value = value
        Me.AttributeType = attributeType
    End Sub
End Class
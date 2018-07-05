Public Class InvocationContext
    Public Property ContextItems As New ContextItemCollection

    Public Sub New()

    End Sub

    Public Sub New(ParamArray items As InvocationContextItem())
        Call Me.ContextItems.Add(items)
    End Sub
End Class

Public Class ContextItemCollection
    Inherits CollectionBase

    Public Sub Add(item As InvocationContextItem)
        Me.InnerList.Add(item)
    End Sub

    Public Sub Add(ParamArray items() As InvocationContextItem)
        For Each item In items
            Call Me.Add(item)
        Next
    End Sub

    Public Function ToArray() As InvocationContextItem()
        Dim objArray = Me.InnerList.ToArray
        Dim result(Me.InnerList.Count - 1) As InvocationContextItem
        For i = 0 To objArray.Length - 1
            result(i) = objArray(i)
        Next
        Return result
    End Function
End Class

Public Class InvocationContextItem
    Public Property InvocationSource As Object
    Public Property Attribute As Object

    Public Sub New(invocationSource As Object, attribute As Object)
        Me.InvocationSource = invocationSource
        Me.Attribute = attribute
    End Sub
End Class
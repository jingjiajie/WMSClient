Public Class InvocationContextItemCollection
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
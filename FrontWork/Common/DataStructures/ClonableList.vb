Public Class ClonableList(Of T As New)
    Inherits List(Of T)
    Implements ICloneable

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim newList As New List(Of T)(Me.Count)
        For i = 0 To Me.Count - 1
            newList.Add(Util.DeepClone(Me(i)))
        Next
        Return newList
    End Function

    Public Sub New()

    End Sub

    Public Sub New(srcList As IEnumerable(Of T))
        Me.AddRange(srcList)
    End Sub

    'Public Sub New(srcArray As T())
    '    Me.AddRange(srcArray)
    'End Sub
End Class

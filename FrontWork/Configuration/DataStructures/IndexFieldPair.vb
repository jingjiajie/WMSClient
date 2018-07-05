Imports FrontWork

Public Class IndexFieldPair
    Public Sub New()
    End Sub

    Public Sub New(index As Integer, field As Field)
        Me.Index = index
        Me.Field = field
    End Sub

    Public Property Index As Integer
    Public Property Field As Field
End Class

Public Class CellPosition
    Public Sub New(row As Integer, field As String)
        Me.Row = row
        Me.Field = field
    End Sub

    Public Property Row As Integer
    Public Property Field As String
End Class

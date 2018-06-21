Public Class BeforeViewRowRemoveEventArgs
    Inherits EventArgs

    Public Property Rows As RowInfo()

    Public Sub New(rows As RowInfo())
        Me.Rows = rows
    End Sub
End Class

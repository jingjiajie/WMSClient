Public Class BeforeViewRowAddEventArgs
    Inherits FrontWorkEventArgs

    Public Property Rows As ViewRowInfo()
    Public Property Cancel As Boolean = False

    Public Sub New(rows As ViewRowInfo())
        Me.Rows = rows
    End Sub
End Class

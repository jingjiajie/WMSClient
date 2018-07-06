Public Class ViewRowUpdatedEventArgs
    Inherits FrontWorkEventArgs

    Public Property Rows As ViewRowInfo()

    Public Sub New(rows As ViewRowInfo())
        Me.Rows = rows
    End Sub
End Class

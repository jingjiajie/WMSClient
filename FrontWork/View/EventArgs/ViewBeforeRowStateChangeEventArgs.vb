Imports FrontWork

Public Class ViewBeforeRowStateChangeEventArgs
    Inherits FrontWorkEventArgs

    Public Sub New()

    End Sub

    Public Sub New(rows() As ViewRowInfo, cancel As Boolean)
        Me.Rows = rows
        Me.Cancel = cancel
    End Sub

    Public Property Rows As ViewRowInfo()
    Public Property Cancel As Boolean = False

End Class

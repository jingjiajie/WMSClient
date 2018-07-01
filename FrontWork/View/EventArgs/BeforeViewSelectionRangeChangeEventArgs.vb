Imports FrontWork

Public Class BeforeViewSelectionRangeChangeEventArgs
    Inherits FrontWorkEventArgs

    Public Sub New(newSelectionRanges() As Range, cancel As Boolean)
        Me.NewSelectionRanges = newSelectionRanges
        Me.Cancel = cancel
    End Sub

    Public Property NewSelectionRanges As Range()
    Public Property Cancel As Boolean = False
End Class

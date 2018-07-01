Imports FrontWork

Public Class ViewSelectionRangeChangedEventArgs
    Inherits FrontWorkEventArgs

    Public Sub New(newSelectionRanges() As Range)
        Me.NewSelectionRanges = newSelectionRanges
    End Sub

    Public Property NewSelectionRanges As Range()
End Class

Imports FrontWork

Public Class ViewSelectionRangeChangedEventArgs
    Inherits EventArgs

    Public Sub New(newSelectionRanges() As Range)
        Me.NewSelectionRanges = newSelectionRanges
    End Sub

    Public Property NewSelectionRanges As Range()
End Class

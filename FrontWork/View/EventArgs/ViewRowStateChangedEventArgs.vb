Imports FrontWork

Public Class ViewRowStateChangedEventArgs
    Inherits FrontWorkEventArgs

    Public Sub New()

    End Sub

    Public Sub New(stateChangedRows() As ViewRowInfo)
        Me.StateChangedRows = stateChangedRows
    End Sub

    Public Property StateChangedRows As ViewRowInfo()

End Class

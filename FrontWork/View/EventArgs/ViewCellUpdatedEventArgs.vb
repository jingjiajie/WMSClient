Public Class ViewCellUpdatedEventArgs
    Inherits EventArgs

    Public Property Cells As ViewCellInfo()

    Public Sub New(cells As ViewCellInfo())
        Me.Cells = cells
    End Sub
End Class

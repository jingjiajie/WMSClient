Public Class ViewCellUpdatedEventArgs
    Inherits FrontWorkEventArgs

    Public Property Cells As ViewCellInfo()

    Public Sub New(cells As ViewCellInfo())
        Me.Cells = cells
    End Sub
End Class

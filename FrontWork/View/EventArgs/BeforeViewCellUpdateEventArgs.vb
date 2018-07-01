Public Class BeforeViewCellUpdateEventArgs
    Inherits FrontWorkEventArgs

    Public Property Cells As ViewCellInfo()
    Public Property Cancel As Boolean = False

    Public Sub New(cells As ViewCellInfo())
        Me.Cells = cells
    End Sub
End Class

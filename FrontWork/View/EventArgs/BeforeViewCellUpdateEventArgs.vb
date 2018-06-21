Public Class BeforeViewCellUpdateEventArgs
    Inherits EventArgs

    Public Property Cells As CellInfo()

    Public Sub New(cells As CellInfo())
        Me.Cells = cells
    End Sub
End Class

Public Structure ModelCellPositionStatePair
    Public CellPosition As CellPosition
    Public State As ModelCellState

    Public Sub New(row As Integer, field As String, state As ModelCellState)
        Me.CellPosition = New CellPosition(row, field)
        Me.State = state
    End Sub

    Public Sub New(cellPosition As CellPosition, state As ModelCellState)
        Me.CellPosition = cellPosition
        Me.State = state
    End Sub
End Structure

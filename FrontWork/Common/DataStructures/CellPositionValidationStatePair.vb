Public Structure CellPositionValidationStatePair
    Public CellPosition As CellPosition
    Public State As ValidationState

    Public Sub New(row As Integer, field As String, state As ValidationState)
        Me.CellPosition = New CellPosition(row, field)
        Me.State = state
    End Sub

    Public Sub New(cellPosition As CellPosition, state As ValidationState)
        Me.CellPosition = cellPosition
        Me.State = state
    End Sub
End Structure

Public Structure CellPositionDataPair
    Public CellPosition As CellPosition
    Public Data As Object

    Public Sub New(cellPosition As CellPosition, data As Object)
        Me.CellPosition = cellPosition
        Me.Data = data
    End Sub

    Public Sub New(row As Integer, field As String, data As Object)
        Me.CellPosition = New CellPosition(row, field)
        Me.Data = data
    End Sub
End Structure

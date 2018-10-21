Public Class ValidationException
    Inherits FrontWorkException

    Public Property CellPositions As CellPosition()
    Public Property ValidationStates As ValidationState()

    Public Sub New(cellPositions As CellPosition(), validationStates As ValidationState())
        Call MyBase.New("ValidationException")
        Me.ValidationStates = validationStates
        Me.CellPositions = cellPositions
    End Sub

    Public Sub New(cellPosition As CellPosition, validationState As ValidationState)
        Call MyBase.New("ValidationException")
        Me.ValidationStates = {validationState}
        Me.CellPositions = {cellPosition}
    End Sub

    Public Sub New(row As Integer, field As String, validationState As ValidationState)
        Call Me.New(New CellPosition(row, field), validationState)
    End Sub
End Class

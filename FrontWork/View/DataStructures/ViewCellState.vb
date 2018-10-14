Public Class ViewCellState
    Public ValidationState As ValidationState = ValidationState.OK

    Public Sub New()

    End Sub

    Public Sub New(validationState As ValidationState)
        Me.ValidationState = validationState
    End Sub

    Public Sub New(modelCellState As ModelCellState)
        Me.ValidationState = modelcellState.ValidationState
    End Sub
End Class

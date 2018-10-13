Imports FrontWork

Public Class ModelCellState
    Public Sub New(validationState As ValidationState)
        Me.ValidationState = validationState
    End Sub

    Public Property ValidationState As New ValidationState(Nothing, ValidationStateType.OK)
End Class

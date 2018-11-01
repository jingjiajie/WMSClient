Imports FrontWork

Public Structure ModelCellState
    Public Sub New(validationState As ValidationState)
        If validationState Is Nothing Then
            Me.ValidationState = New ValidationState(Nothing, ValidationStateType.OK)
        Else
            Me.ValidationState = validationState
        End If
    End Sub

    Public Property ValidationState As ValidationState
End Structure

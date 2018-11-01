Imports FrontWork

Public Structure ModelRowState
    Public SynchronizationState As SynchronizationState

    Public Sub New(synchronizationState As SynchronizationState)
        Me.SynchronizationState = synchronizationState
    End Sub

    Public Sub New(viewRowState As ViewRowState)
        Me.SynchronizationState = viewRowState.SynchronizationState
    End Sub
End Structure

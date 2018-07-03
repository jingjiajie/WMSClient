Imports FrontWork

Public Class ViewRowState
    Public SynchronizationState As SynchronizationState = SynchronizationState.SYNCHRONIZED

    Public Sub New()

    End Sub

    Public Sub New(synchronizationState As SynchronizationState)
        Me.SynchronizationState = synchronizationState
    End Sub

    Public Sub New(modelRowState As ModelRowState)
        Me.SynchronizationState = modelRowState.SynchronizationState
    End Sub
End Class

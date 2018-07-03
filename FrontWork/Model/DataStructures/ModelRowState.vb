Imports FrontWork

Public Class ModelRowState
    Public SynchronizationState As SynchronizationState = SynchronizationState.SYNCHRONIZED

    Public Sub New(synchronizationState As SynchronizationState)
        Me.SynchronizationState = synchronizationState
    End Sub

    Public Sub New(viewRowState As ViewRowState)
        Me.SynchronizationState = viewRowState.SynchronizationState
    End Sub

    Public Sub New()

    End Sub
End Class

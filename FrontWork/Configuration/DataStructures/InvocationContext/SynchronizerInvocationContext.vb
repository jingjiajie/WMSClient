Public Class SynchronizerInvocationContext
    Inherits InvocationContext

    Public Sub New(synchronizer As ISynchronizer)
        Call MyBase.New
        Me.ContextItems.Add(New InvocationContextItem(synchronizer, GetType(SynchronizerAttribute)))
    End Sub
End Class

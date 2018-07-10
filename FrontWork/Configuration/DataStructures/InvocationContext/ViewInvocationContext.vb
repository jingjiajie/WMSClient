Public Class ViewInvocationContext
    Inherits InvocationContext

    Public Sub New(view As IView)
        Call MyBase.New
        Me.ContextItems.Add(New InvocationContextItem(view, GetType(ViewAttribute)))
    End Sub
End Class

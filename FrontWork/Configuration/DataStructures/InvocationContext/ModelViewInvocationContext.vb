Public Class ModelViewInvocationContext
    Inherits InvocationContext

    Public Sub New(model As IModel, view As IView)
        Call MyBase.New
        Me.ContextItems.Add(New InvocationContextItem(model, GetType(ModelAttribute)))
        Me.ContextItems.Add(New InvocationContextItem(view, GetType(ViewAttribute)))
    End Sub
End Class

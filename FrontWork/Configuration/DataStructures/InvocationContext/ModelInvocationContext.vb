Public Class ModelInvocationContext
    Inherits InvocationContext

    Public Sub New(model As IModel)
        Call MyBase.New
        Me.ContextItems.Add(New InvocationContextItem(model, GetType(ModelAttribute)))
    End Sub
End Class

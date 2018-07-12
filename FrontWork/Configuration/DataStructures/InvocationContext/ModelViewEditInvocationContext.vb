Public Class ModelViewEditInvocationContext
    Inherits ModelViewInvocationContext

    Public Sub New(model As IModel, view As IView, rowNum As Integer, colName As String, data As Object)
        Call MyBase.New(model, view)
        Me.ContextItems.Add(New InvocationContextItem(data, GetType(DataAttribute)))
        Me.ContextItems.Add(New InvocationContextItem(rowNum, GetType(RowAttribute)))
        Me.ContextItems.Add(New InvocationContextItem(colName, GetType(FieldAttribute)))
    End Sub

End Class

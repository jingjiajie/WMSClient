Public Class ViewEditInvocationContext
    Inherits ViewInvocationContext

    Public Sub New(view As IView, rowNum As Integer, colName As String, data As Object)
        Call MyBase.New(view)
        Me.ContextItems.Add(New InvocationContextItem(data, GetType(DataAttribute)))
        Me.ContextItems.Add(New InvocationContextItem(rowNum, GetType(RowAttribute)))
        Me.ContextItems.Add(New InvocationContextItem(colName, GetType(FieldAttribute)))
    End Sub
End Class

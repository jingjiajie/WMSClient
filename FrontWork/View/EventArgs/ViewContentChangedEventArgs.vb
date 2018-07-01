Public Class ViewContentChangedEventArgs
    Inherits ViewEditEventArgs

    Public Sub New()
    End Sub

    Public Sub New(row As Integer, columnName As String, cellData As Object)
        MyBase.New(row, columnName, cellData)
    End Sub
End Class

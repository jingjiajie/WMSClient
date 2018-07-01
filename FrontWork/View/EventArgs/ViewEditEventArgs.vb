Public Class ViewEditEventArgs
    Inherits FrontWorkEventArgs

    Public Property Row As Integer
    Public Property ColumnName As String
    Public Property CellData As Object

    Public Sub New()

    End Sub

    Public Sub New(row As Integer, columnName As String, cellData As Object)
        Me.Row = row
        Me.ColumnName = columnName
        Me.CellData = cellData
    End Sub
End Class

''' <summary>
''' 视图一行数据的信息
''' </summary>
Public Class ViewRowInfo
    Public Property Row As Integer
    Public Property RowData As IDictionary(Of String, Object)
    Public Property RowState As ViewRowState

    Public Sub New()

    End Sub

    Public Sub New(row As Integer, rowData As IDictionary(Of String, Object), rowState As ViewRowState)
        Me.Row = row
        Me.RowData = rowData
        Me.RowState = rowState
    End Sub
End Class

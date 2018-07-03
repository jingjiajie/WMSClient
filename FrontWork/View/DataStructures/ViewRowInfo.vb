''' <summary>
''' 一行数据的索引和数据
''' </summary>
Public Class ViewRowInfo
    ''' <summary>
    ''' 行号
    ''' </summary>
    ''' <returns></returns>
    Public Property Row As Integer

    ''' <summary>
    ''' 行数据
    ''' </summary>
    ''' <returns></returns>
    Public Property RowData As IDictionary(Of String, Object)

    Public Property RowState As ViewRowState

    Public Sub New(row As Integer, rowData As IDictionary(Of String, Object), rowState As ViewRowState)
        Me.Row = row
        Me.RowData = rowData
        Me.RowState = rowState
    End Sub

    Public Sub New()

    End Sub
End Class

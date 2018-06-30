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

    Public Sub New(row As Integer, dataRow As IDictionary(Of String, Object))
        Me.Row = row
        Me.RowData = dataRow
    End Sub

    Public Sub New()

    End Sub
End Class

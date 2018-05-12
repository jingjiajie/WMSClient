''' <summary>
''' 一行数据的索引和数据
''' </summary>
Public Class IndexRowPair
    ''' <summary>
    ''' 行号
    ''' </summary>
    ''' <returns></returns>
    Public Property Index As Long

    ''' <summary>
    ''' 行ID
    ''' </summary>
    ''' <returns></returns>
    Public Property RowID As Guid

    ''' <summary>
    ''' 行数据
    ''' </summary>
    ''' <returns></returns>
    Public Property RowData As Dictionary(Of String, Object)

    Public Sub New(index As Long, rowID As Guid, dataRow As Dictionary(Of String, Object))
        Me.Index = index
        Me.RowData = dataRow
        Me.RowID = rowID
    End Sub

    Public Sub New()

    End Sub
End Class

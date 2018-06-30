''' <summary>
''' 一行数据的索引和数据
''' </summary>
Public Class ModelRowInfo
    ''' <summary>
    ''' 行号
    ''' </summary>
    ''' <returns></returns>
    Public Property Row As Integer

    ''' <summary>
    ''' 行ID
    ''' </summary>
    ''' <returns></returns>
    Public Property RowID As Guid

    ''' <summary>
    ''' 同步状态
    ''' </summary>
    ''' <returns></returns>
    Public Property SynchronizationState As SynchronizationState

    ''' <summary>
    ''' 行数据
    ''' </summary>
    ''' <returns></returns>
    Public Property RowData As Dictionary(Of String, Object)

    Public Sub New(row As Integer, rowID As Guid, dataRow As Dictionary(Of String, Object), syncState As SynchronizationState)
        Me.Row = row
        Me.RowData = dataRow
        Me.RowID = rowID
        Me.SynchronizationState = syncState
    End Sub

    Public Sub New()

    End Sub
End Class

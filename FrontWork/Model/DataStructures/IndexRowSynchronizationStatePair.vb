''' <summary>
''' 一行的索引和行同步状态
''' </summary>
Public Class IndexRowSynchronizationStatePair
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
    ''' 同步状态
    ''' </summary>
    ''' <returns></returns>
    Public Property SynchronizationState As SynchronizationState

    Public Sub New(index As Long, rowID As Guid, synchronizationState As SynchronizationState)
        Me.Index = index
        Me.RowID = rowID
        Me.SynchronizationState = synchronizationState
    End Sub

    Public Sub New()

    End Sub
End Class

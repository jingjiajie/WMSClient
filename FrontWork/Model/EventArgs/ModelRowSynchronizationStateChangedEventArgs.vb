''' <summary>
''' Model行同步状态改变时间参数
''' </summary>
Public Class ModelRowSynchronizationStateChangedEventArgs
    Inherits FrontWorkEventArgs

    ''' <summary>
    ''' 同步状态改变的行的信息
    ''' </summary>
    ''' <returns></returns>
    Public Property SynchronizationStateUpdatedRows As ModelRowInfo()

    Public Sub New(synchronizationStateUpdatedRows As ModelRowInfo())
        Me.SynchronizationStateUpdatedRows = synchronizationStateUpdatedRows
    End Sub
End Class

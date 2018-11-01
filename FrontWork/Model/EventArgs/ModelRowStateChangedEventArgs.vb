''' <summary>
''' Model行状态改变事件参数
''' </summary>
Public Class ModelRowStateChangedEventArgs
    Inherits FrontWorkEventArgs

    ''' <summary>
    ''' 状态改变的行的信息
    ''' </summary>
    ''' <returns></returns>
    Public Property StateUpdatedRows As ModelRowInfo()

    Public Sub New(stateUpdatedRows As ModelRowInfo())
        Me.StateUpdatedRows = stateUpdatedRows
    End Sub
End Class

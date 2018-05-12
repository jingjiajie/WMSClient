''' <summary>
''' Model删除行时间参数
''' </summary>
Public Class ModelRowRemovedEventArgs
    Inherits EventArgs

    ''' <summary>
    ''' 删除行的信息和数据
    ''' </summary>
    ''' <returns></returns>
    Public Property RemovedRows As IndexRowPair()
End Class

''' <summary>
''' Model删除行事件参数
''' </summary>
Public Class ModelRowRemovedEventArgs
    Inherits FrontWorkEventArgs

    ''' <summary>
    ''' 删除行的信息和数据
    ''' </summary>
    ''' <returns></returns>
    Public Property RemovedRows As ModelRowInfo()
End Class

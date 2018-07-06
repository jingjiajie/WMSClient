''' <summary>
''' Model整行更新事件参数
''' </summary>
Public Class ModelRowUpdatedEventArgs
    Inherits FrontWorkEventArgs

    ''' <summary>
    ''' 更新的行的信息和数据
    ''' </summary>
    ''' <returns></returns>
    Public Property UpdatedRows As ModelRowInfo()
End Class

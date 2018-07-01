''' <summary>
''' Model新增行事件
''' </summary>
Public Class ModelRowAddedEventArgs
    Inherits FrontWorkEventArgs

    ''' <summary>
    ''' 新增行的信息和数据
    ''' </summary>
    ''' <returns></returns>
    Public Property AddedRows As ModelRowInfo()
End Class

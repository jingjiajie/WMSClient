''' <summary>
''' Model新增行事件
''' </summary>
Public Class ModelRowAddedEventArgs
    Inherits EventArgs

    ''' <summary>
    ''' 新增行的信息和数据
    ''' </summary>
    ''' <returns></returns>
    Public Property AddedRows As IndexRowPair()
End Class

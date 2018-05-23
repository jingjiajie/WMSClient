''' <summary>
''' Model即将删除行事件参数
''' </summary>
Public Class ModelBeforeRowRemoveEventArgs
    Inherits EventArgs

    ''' <summary>
    ''' 删除行的信息和数据
    ''' </summary>
    ''' <returns></returns>
    Public Property RemoveRows As RowInfo()

    ''' <summary>
    ''' 是否取消删除
    ''' </summary>
    ''' <returns></returns>
    Public Property Cancel As Boolean = False
End Class

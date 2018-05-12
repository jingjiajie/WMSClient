''' <summary>
''' Model选区改变事件参数
''' </summary>
Public Class ModelSelectionRangeChangedEventArgs
    Inherits EventArgs

    ''' <summary>
    ''' 新的选区
    ''' </summary>
    ''' <returns></returns>
    Public Property NewSelectionRange As Range()
End Class

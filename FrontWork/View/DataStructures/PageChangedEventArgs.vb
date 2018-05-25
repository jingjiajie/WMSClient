''' <summary>
''' 页码变化事件参数
''' </summary>
Public Class PageChangedEventArgs
    Inherits EventArgs
    ''' <summary>
    ''' 当前页（从1开始）
    ''' </summary>
    ''' <returns></returns>
    Public Property CurrentPage as Integer

    Public Sub New(currentPage as Integer)
        Me.CurrentPage = currentPage
    End Sub

    Public Sub New()

    End Sub
End Class

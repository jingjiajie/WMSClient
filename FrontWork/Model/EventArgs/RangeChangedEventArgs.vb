''' <summary>
''' Range数据改变事件参数
''' </summary>
Public Class RangeChangedEventArgs
    Inherits EventArgs

    ''' <summary>
    ''' 新的Range
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property NewRange As Range

    Public Sub New(newRange As Range)
        Me.NewRange = newRange
    End Sub
End Class

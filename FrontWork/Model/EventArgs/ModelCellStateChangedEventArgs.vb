''' <summary>
''' Model单元格状态改变事件参数
''' </summary>
Public Class ModelCellStateChangedEventArgs
    Inherits FrontWorkEventArgs

    ''' <summary>
    ''' 状态改变的单元格的信息
    ''' </summary>
    ''' <returns></returns>
    Public Property StateUpdatedCells As ModelCellInfo()

    Public Sub New(stateUpdatedCells As ModelCellInfo())
        Me.StateUpdatedCells = stateUpdatedCells
    End Sub
End Class

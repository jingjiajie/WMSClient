''' <summary>
''' Model的单元格数据更新事件
''' </summary>
Public Class ModelCellUpdatedEventArgs
    Inherits FrontWorkEventArgs

    ''' <summary>
    ''' 更新的单元格和数据信息
    ''' </summary>
    ''' <returns></returns>
    Public Property UpdatedCells As ModelCellInfo()
End Class

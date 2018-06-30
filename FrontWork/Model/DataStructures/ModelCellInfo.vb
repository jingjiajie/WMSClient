''' <summary>
''' 一个单元格的索引和数据
''' </summary>
Public Class ModelCellInfo
    ''' <summary>
    ''' 所在行号
    ''' </summary>
    ''' <returns></returns>
    Public Property Row As Integer

    ''' <summary>
    ''' 所在行ID
    ''' </summary>
    ''' <returns></returns>
    Public Property RowID As Guid

    ''' <summary>
    ''' 列名
    ''' </summary>
    ''' <returns></returns>
    Public Property ColumnName As String

    ''' <summary>
    ''' 单元格数据
    ''' </summary>
    ''' <returns></returns>
    Public Property CellData As Object

    Public Sub New(row As Integer, rowID As Guid, columnName As String, cellData As Object)
        Me.Row = row
        Me.CellData = cellData
        Me.ColumnName = columnName
        Me.RowID = rowID
    End Sub
End Class

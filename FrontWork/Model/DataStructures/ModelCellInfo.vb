''' <summary>
''' 一个单元格的索引和数据
''' </summary>
Public Class ModelCellInfo
    Implements ICloneable
    ''' <summary>
    ''' 所在行号
    ''' </summary>
    ''' <returns></returns>
    Public Property Row As Integer

    ''' <summary>
    ''' 列名
    ''' </summary>
    ''' <returns></returns>
    Public Property FieldName As String

    ''' <summary>
    ''' 单元格数据
    ''' </summary>
    ''' <returns></returns>
    Public Property CellData As Object

    ''' <summary>
    ''' 单元格状态
    ''' </summary>
    ''' <returns></returns>
    Public Property State As ModelCellState

    Public Sub New(row As Integer, fieldName As String, cellData As Object, state As ModelCellState)
        Me.Row = row
        Me.CellData = cellData
        Me.FieldName = fieldName
        Me.State = state
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim newInstance As New ModelCellInfo(Me.Row, Me.FieldName, Me.CellData, Me.State)
        Return newInstance
    End Function
End Class

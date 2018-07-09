''' <summary>
''' 一行数据的索引和数据
''' </summary>
Public Class ModelRowInfo
    Implements ICloneable
    ''' <summary>
    ''' 行号
    ''' </summary>
    ''' <returns></returns>
    Public Property Row As Integer

    ''' <summary>
    ''' 同步状态
    ''' </summary>
    ''' <returns></returns>
    Public Property State As ModelRowState

    ''' <summary>
    ''' 行数据
    ''' </summary>
    ''' <returns></returns>
    Public Property RowData As Dictionary(Of String, Object)

    Public Sub New(row As Integer, dataRow As Dictionary(Of String, Object), state As ModelRowState)
        Me.Row = row
        Me.RowData = dataRow
        Me.State = state
    End Sub

    Public Sub New()

    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim newObj As New ModelRowInfo
        newObj.Row = Me.Row
        newObj.State = Me.State
        newObj.RowData = Me.RowData
        Return newObj
    End Function
End Class

﻿''' <summary>
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
    Public Property ColumnName As String

    ''' <summary>
    ''' 单元格数据
    ''' </summary>
    ''' <returns></returns>
    Public Property CellData As Object

    Public Sub New(row As Integer, columnName As String, cellData As Object)
        Me.Row = row
        Me.CellData = cellData
        Me.ColumnName = columnName
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim newInstance As New ModelCellInfo(Me.Row, Me.ColumnName, Me.CellData)
        Return newInstance
    End Function
End Class

''' <summary>
''' 选区范围
''' </summary>
Public Class Range
    Private _row as Integer
    Private _column as Integer
    Private _rows as Integer
    Private _columns as Integer

    ''' <summary>
    ''' 行号
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Row As Integer
        Get
            Return Me._row
        End Get
    End Property

    ''' <summary>
    ''' 列号
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Column As Integer
        Get
            Return Me._column
        End Get
    End Property

    ''' <summary>
    ''' 行数
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Rows As Integer
        Get
            Return Me._rows
        End Get
    End Property

    ''' <summary>
    ''' 列数
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Columns As Integer
        Get
            Return Me._columns
        End Get
    End Property

    Public Sub New(row As Integer, column As Integer, rows As Integer, columns As Integer)
        Me._row = row
        Me._column = column
        Me._rows = rows
        Me._columns = columns
    End Sub
End Class

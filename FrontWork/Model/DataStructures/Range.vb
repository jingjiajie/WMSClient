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
    Public Property Row as Integer
        Get
            Return Me._row
        End Get
        Set(value as Integer)
            Me._row = value
            RaiseEvent RangeChanged(New RangeChangedEventArgs(Me))
        End Set
    End Property

    ''' <summary>
    ''' 列号
    ''' </summary>
    ''' <returns></returns>
    Public Property Column as Integer
        Get
            Return Me._column
        End Get
        Set(value as Integer)
            Me._column = value
            RaiseEvent RangeChanged(New RangeChangedEventArgs(Me))
        End Set
    End Property

    ''' <summary>
    ''' 行数
    ''' </summary>
    ''' <returns></returns>
    Public Property Rows as Integer
        Get
            Return Me._rows
        End Get
        Set(value as Integer)
            Me._rows = value
            RaiseEvent RangeChanged(New RangeChangedEventArgs(Me))
        End Set
    End Property

    ''' <summary>
    ''' 列数
    ''' </summary>
    ''' <returns></returns>
    Public Property Columns as Integer
        Get
            Return Me._columns
        End Get
        Set(value as Integer)
            Me._columns = value
            RaiseEvent RangeChanged(New RangeChangedEventArgs(Me))
        End Set
    End Property

    Public Sub New(row as Integer, column as Integer, rows as Integer, columns as Integer)
        Me._row = row
        Me._column = column
        Me._rows = rows
        Me._columns = columns
    End Sub

    ''' <summary>
    ''' 选区改变事件
    ''' </summary>
    ''' <param name="e">事件参数</param>
    Public Event RangeChanged(e As RangeChangedEventArgs)
End Class

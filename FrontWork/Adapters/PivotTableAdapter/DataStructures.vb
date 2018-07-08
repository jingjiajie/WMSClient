Partial Public Class PivotTableAdapter

    Private Class Position
        Public Property Row As Integer
        Public Property ColumnName As String

        Public Sub New(row As Integer, columnName As String)
            Me.Row = row
            Me.ColumnName = columnName
        End Sub

        Public Shared Operator =(pos1 As Position, pos2 As Position) As Boolean
            If pos1.Row <> pos2.Row Then Return False
            If pos1.ColumnName <> pos2.ColumnName Then Return False
            Return True
        End Operator

        Public Shared Operator <>(pos1 As Position, pos2 As Position) As Boolean
            Return Not pos1 = pos2
        End Operator
    End Class

    Private Class PositionDataPair
        Public Sub New(row As Integer, columnName As String, data As Object)
            Me.Position = New Position(row, columnName)
            Me.Data = data
        End Sub

        Public Sub New(pos As Position, data As Object)
            Me.Position = pos
            Me.Data = data
        End Sub

        Public Property Row As Integer
            Get
                Return Me.Position.Row
            End Get
            Set(value As Integer)
                Me.Position.Row = value
            End Set
        End Property

        Public Property ColumnName As String
            Get
                Return Me.Position.ColumnName
            End Get
            Set(value As String)
                Me.Position.ColumnName = value
            End Set
        End Property

        Public Property Data As Object
        Public Property Position As Position
    End Class

    Private Class PositionMap
        Public Sub New(sourcePosition As Position, targetPosition As Position)
            Me.SourcePosition = sourcePosition
            Me.TargetPosition = targetPosition
        End Sub

        Public Property SourcePosition As Position
        Public Property TargetPosition As Position
    End Class
End Class
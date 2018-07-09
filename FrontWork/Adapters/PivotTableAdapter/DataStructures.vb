Partial Public Class PivotTableAdapter

    Private Class CellPosition
        Public Property Row As Integer
        Public Property ColumnName As String

        Public Sub New(row As Integer, columnName As String)
            Me.Row = row
            Me.ColumnName = columnName
        End Sub

        Public Shared Operator =(pos1 As CellPosition, pos2 As CellPosition) As Boolean
            If pos1.Row <> pos2.Row Then Return False
            If pos1.ColumnName <> pos2.ColumnName Then Return False
            Return True
        End Operator

        Public Shared Operator <>(pos1 As CellPosition, pos2 As CellPosition) As Boolean
            Return Not pos1 = pos2
        End Operator

        Public Overrides Function ToString() As String
            Return $"({Me.Row}, {Me.ColumnName})"
        End Function
    End Class

    Private Class CellPositionDataPair
        Public Sub New(row As Integer, columnName As String, data As Object)
            Me.Position = New CellPosition(row, columnName)
            Me.Data = data
        End Sub

        Public Sub New(pos As CellPosition, data As Object)
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
        Public Property Position As CellPosition
    End Class

    Private Class CellMap
        Public Sub New(sourcePosition As CellPosition, targetPosition As CellPosition)
            Me.SourcePosition = sourcePosition
            Me.TargetPosition = targetPosition
        End Sub

        Public Overrides Function ToString() As String
            Return Me.SourcePosition?.ToString & " => " & Me.TargetPosition?.ToString
        End Function

        Public Property SourcePosition As CellPosition
        Public Property TargetPosition As CellPosition
    End Class

    Private Class ColumnMap
        Public Property SourceColumnsAsColumnValues As List(Of Object)
        Public Property SourceColumnAsValueName As String
        Public Property TargetColumnName As String
    End Class
End Class
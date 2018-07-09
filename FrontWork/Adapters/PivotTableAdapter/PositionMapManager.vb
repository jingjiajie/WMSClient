Imports System.Linq

Partial Public Class PivotTableAdapter
    Private Class CellMapManager
        Private Property PositionMappingList As New List(Of CellMap)

        Public Sub SetPositionMap(sourceModelPosition As CellPosition, targetModelPosition As CellPosition)
            Me.PositionMappingList.RemoveAll(Function(map) map.SourcePosition = sourceModelPosition OrElse map.TargetPosition = targetModelPosition)
            Me.PositionMappingList.Add(New CellMap(sourceModelPosition, targetModelPosition))
        End Sub

        Public Sub ClearPositionMaps()
            Me.PositionMappingList.Clear()
        End Sub

        Public Function GetTargetPosition(sourceModelPosition As CellPosition) As CellPosition
            Dim foundMap = Me.PositionMappingList.Find(Function(map) map.SourcePosition = sourceModelPosition)
            If foundMap Is Nothing Then Return Nothing
            Return foundMap.TargetPosition
        End Function

        Public Function GetSourcePosition(targetModelPosition As CellPosition) As CellPosition
            Dim foundMap = Me.PositionMappingList.Find(Function(map) map.TargetPosition = targetModelPosition)
            If foundMap Is Nothing Then Return Nothing
            Return foundMap.SourcePosition
        End Function

        Public Sub AdjustSourceRowsAfterRowsRemoved(rows As Integer())
            '将被删除行相应的映射记录删除
            Me.PositionMappingList.RemoveAll(Function(m) rows.Contains(m.SourcePosition.Row))
            '将其他行的映射记录的行相应前移
            For Each map In Me.PositionMappingList
                map.SourcePosition.Row -= rows.Aggregate(0, Function(sum, row) If(row < map.SourcePosition.Row, sum + 1, sum))
            Next
        End Sub

        Public Sub AdjustSourceRowsAfterRowsInserted(rows As Integer())
            Dim sourcePositions = Me.PositionMappingList.Select(Function(m) m.SourcePosition).ToArray
            For i = 0 To sourcePositions.Length - 1
                Dim sourcePosition = sourcePositions(i)
                sourcePosition.Row += rows.Aggregate(0, Function(sum, row) If(row <= sourcePosition.Row, sum + 1, sum))
            Next
        End Sub
    End Class
End Class
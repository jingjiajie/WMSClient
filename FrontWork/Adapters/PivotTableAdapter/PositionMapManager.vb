Partial Public Class PivotTableAdapter
    Private Class PositionMapManager
        Private Property PositionMappingList As New List(Of PositionMap)

        Public Sub SetPositionMap(sourceModelPosition As Position, targetModelPosition As Position)
            Me.PositionMappingList.RemoveAll(Function(map) map.SourcePosition = sourceModelPosition OrElse map.TargetPosition = targetModelPosition)
            Me.PositionMappingList.Add(New PositionMap(sourceModelPosition, targetModelPosition))
        End Sub

        Public Sub ClearPositionMap()
            Me.PositionMappingList.Clear()
        End Sub

        Public Function GetTargetPosition(sourceModelPosition As Position) As Position
            Dim foundMap = Me.PositionMappingList.Find(Function(map) map.SourcePosition = sourceModelPosition)
            If foundMap Is Nothing Then Return Nothing
            Return foundMap.TargetPosition
        End Function

        Public Function GetSourcePosition(targetModelPosition As Position) As Position
            Dim foundMap = Me.PositionMappingList.Find(Function(map) map.TargetPosition = targetModelPosition)
            If foundMap Is Nothing Then Return Nothing
            Return foundMap.SourcePosition
        End Function
    End Class
End Class
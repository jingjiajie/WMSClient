Imports System.Linq

Partial Public Class PivotTableAdapter
    Private Class ColumnMapManager

        Private Property ColumnMappingList As New List(Of ColumnMap)

        Public Sub SetColumnMap(sourceColumnAsColumnValues As Object(), sourceColumnAsValueName As String, targetColumnName As String)
            Dim foundMap = Me.ColumnMappingList.Find(Function(m) m.TargetColumnName = targetColumnName)
            If foundMap Is Nothing Then
                foundMap = New ColumnMap()
                foundMap.TargetColumnName = targetColumnName
                Me.ColumnMappingList.Add(foundMap)
            End If
            foundMap.SourceColumnAsValueName = sourceColumnAsValueName
            foundMap.SourceColumnsAsColumnValues = sourceColumnAsColumnValues.ToList
        End Sub

        Public Sub ClearColumnMaps()
            Me.ColumnMappingList.Clear()
        End Sub

        Public Function GetTargetColumnName(sourceColumnAsColumnValues As Object(), sourceColumnAsValueName As String) As String
            Dim foundMap = Me.GetColumnMap(sourceColumnAsColumnValues, sourceColumnAsValueName)
            If foundMap Is Nothing Then Return -1
            Return foundMap.TargetColumnName
        End Function

        Public Function GetColumnMap(targetColumnName As String) As ColumnMap
            Dim foundMap = Me.ColumnMappingList.Find(Function(map) map.TargetColumnName = targetColumnName)
            Return foundMap
        End Function

        Public Function GetColumnMap(sourceColumnAsColumnValues As Object(), sourceColumnAsValueName As String) As ColumnMap
            Dim foundMap = Me.ColumnMappingList.Find(
                Function(map)
                    If map.SourceColumnAsValueName <> sourceColumnAsValueName Then Return False
                    For i = 0 To map.SourceColumnsAsColumnValues.Count
                        If Not map.SourceColumnsAsColumnValues(i).Equals(sourceColumnAsColumnValues(i)) Then
                            Return False
                        End If
                    Next
                    Return True
                End Function)
            Return foundMap
        End Function
    End Class
End Class
Imports FrontWork

Public Class ModelRefreshArgs
    Public Property DataRows As IDictionary(Of String, Object)()
    Public Property SelectionRanges As Range()

    Public Sub New(dataRows As IDictionary(Of String, Object)(), selectionRanges() As Range)
        Me.DataRows = dataRows
        Me.SelectionRanges = selectionRanges
    End Sub
End Class

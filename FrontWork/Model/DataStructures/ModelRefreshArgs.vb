Imports FrontWork

Public Class ModelRefreshArgs
    Public Property DataTable As DataTable
    Public Property SelectionRanges As Range()
    Public Property RowStates As ModelRowState()

    Public Sub New(dataTable As DataTable, selectionRanges() As Range, rowStates() As ModelRowState)
        Me.DataTable = dataTable
        Me.SelectionRanges = selectionRanges
        Me.RowStates = rowStates
    End Sub
End Class

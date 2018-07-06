Imports FrontWork

Public Class ModelRefreshArgs
    Public Property DataTable As DataTable
    Public Property SelectionRanges As Range()

    Public Sub New(dataTable As DataTable, selectionRanges() As Range)
        Me.DataTable = dataTable
        Me.SelectionRanges = selectionRanges
    End Sub
End Class

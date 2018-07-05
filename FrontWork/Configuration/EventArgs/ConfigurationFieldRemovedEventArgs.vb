Imports FrontWork

Public Class ConfigurationFieldRemovedEventArgs
    Inherits FrontWorkEventArgs

    Public Sub New()
    End Sub

    Public Sub New(removedFields() As IndexFieldPair, mode As String)
        Me.RemovedFields = removedFields
        Me.Mode = mode
    End Sub

    Public Property RemovedFields As IndexFieldPair()
    Public Property Mode As String
End Class

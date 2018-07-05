Imports FrontWork

Public Class ConfigurationFieldAddedEventArgs
    Inherits FrontWorkEventArgs

    Public Sub New()
    End Sub

    Public Sub New(addedFields() As IndexFieldPair, mode As String)
        Me.AddedFields = addedFields
        Me.Mode = mode
    End Sub

    Public Property AddedFields As IndexFieldPair()
    Public Property Mode As String
End Class

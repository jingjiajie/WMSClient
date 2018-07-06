Imports FrontWork

Public Class ConfigurationFieldUpdatedEventArgs
    Inherits FrontWorkEventArgs

    Public Sub New()
    End Sub

    Public Sub New(updatedFields() As IndexFieldPair, mode As String)
        Me.UpdatedFields = updatedFields
        Me.Mode = mode
    End Sub

    Public Property UpdatedFields As IndexFieldPair()
    Public Property Mode As String
End Class

Imports FrontWork

Public Class BeforeConfigurationFieldUpdateEventArgs
    Inherits FrontWorkEventArgs

    Public Sub New()
    End Sub

    Public Sub New(updateFields() As IndexFieldPair, mode As String)
        Me.UpdateFields = updateFields
        Me.Mode = mode
    End Sub

    Public Property Cancel As Boolean = False
    Public Property UpdateFields As IndexFieldPair()
    Public Property Mode As String

End Class

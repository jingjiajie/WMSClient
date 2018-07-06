Imports FrontWork

Public Class BeforeConfigurationFieldAddEventArgs
    Inherits FrontWorkEventArgs

    Public Sub New()
    End Sub

    Public Sub New(addFields() As IndexFieldPair, mode As String)
        Me.AddFields = addFields
        Me.Mode = mode
    End Sub

    Public Property Cancel As Boolean = False
    Public Property AddFields As IndexFieldPair()
    Public Property Mode As String

End Class

Imports FrontWork

Public Class BeforeConfigurationFieldRemoveEventArgs
    Inherits FrontWorkEventArgs

    Public Sub New()
    End Sub

    Public Sub New(removeFields() As IndexFieldPair, mode As String)
        Me.RemoveFields = removeFields
        Me.Mode = mode
    End Sub

    Public Property Cancel As Boolean = False
    Public Property RemoveFields As IndexFieldPair()
    Public Property Mode As String

End Class

Public Class MethodNotFoundException
    Inherits FrontWorkException

    Public Sub New(message As String)
        Call MyBase.New(message)
    End Sub

End Class

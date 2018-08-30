Public Class HTTPResponse
    Public StatusCode As Integer

    Public Sub New(statusCode As Integer, bodyString As String, errorMessage As String)
        Me.StatusCode = statusCode
        Me.BodyString = bodyString
        Me.ErrorMessage = errorMessage
    End Sub

    Public Sub New(statusCode As Integer, bodyString As String)
        Me.StatusCode = statusCode
        Me.BodyString = bodyString
    End Sub

    Public Property BodyString As String
    Public Property ErrorMessage As String
End Class

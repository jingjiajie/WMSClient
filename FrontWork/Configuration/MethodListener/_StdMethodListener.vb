Public Class _StdMethodListener
    Inherits MethodListenerBase
    Public Function BoolForwardMapper(val As Boolean) As String
        If val = True Then
            Return "是"
        Else
            Return "否"
        End If
    End Function

    Public Function BoolBackwardMapper(str As String) As Boolean
        Dim strTrues = {
            "是", "真", "对", "true", "yes", "1"
        }
        For Each strTrue In strTrues
            If strTrue.Equals(str, StringComparison.OrdinalIgnoreCase) Then Return True
        Next
        Return False
    End Function

    Public Function BoolValues() As String()
        Return {"是", "否"}
    End Function

    Public Function Now() As DateTime
        Return DateTime.Now
    End Function
End Class

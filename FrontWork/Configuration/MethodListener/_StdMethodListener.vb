<MethodListener("_StdMethodListener")>
Public Class _StdMethodListener
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

    Public Function DateTimePlaceHolder() As String
        Return "年-月-日 (时:分:秒 可选)"
    End Function

    Public Function DateTimeAssociation() As String()
        Dim associationList As New List(Of String)
        Dim now = DateTime.Now
        For i = 0 To 9
            Dim curDate = now.AddDays(-i)
            associationList.Add(curDate.ToShortDateString)
        Next
        Return associationList.ToArray
    End Function
End Class

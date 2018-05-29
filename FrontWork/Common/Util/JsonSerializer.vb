Imports System.Text.RegularExpressions
Imports System.Web.Script.Serialization

Public Class JsonSerializer
    Inherits JavaScriptSerializer

    Public Shadows Function Serialize(obj As Object) As String
        Dim serializedStr = MyBase.Serialize(obj)
        serializedStr = Regex.Replace(serializedStr, "\\/Date\((\d+)\)\\/",
                                      Function(match)
                                          Dim dt = New DateTime(1970, 1, 1)
                                          dt = dt.AddMilliseconds(Long.Parse(match.Groups(1).Value))
                                          dt = dt.ToLocalTime()
                                          Return dt.ToString
                                          'Return dt.ToString("yyyy-MM-dd HH:mm:ss")
                                      End Function)
        Return serializedStr
    End Function

End Class

Public Class ValidationState
    Public Property Message As String
    Public Property Type As ValidationStateType

    Public Sub New(message As String, type As ValidationStateType)
        Me.Message = message
        Me.Type = type
    End Sub
End Class

Public Enum ValidationStateType
    [ERROR] '错误
    WARNING '警告
    OK '正确
End Enum
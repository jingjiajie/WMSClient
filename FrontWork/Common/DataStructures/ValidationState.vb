Public Class ValidationState
    Public Property Message As String
    Public Property Type As ValidationStateType

    Public Shared ReadOnly Property OK As New ValidationState(ValidationStateType.OK, Nothing)

    Public Sub New(type As ValidationStateType, Optional message As String = Nothing)
        Me.Message = message
        Me.Type = type
    End Sub
End Class

Public Enum ValidationStateType
    [ERROR] '错误
    WARNING '警告
    OK '正确
End Enum
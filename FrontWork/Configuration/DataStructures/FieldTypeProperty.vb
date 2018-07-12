Imports Jint.Native

Public Class FieldTypeProperty
    Inherits FieldProperty

    Public Sub New(value As Object)
        MyBase.New(value)
    End Sub

    Public Sub New(methodName As String, _methodListenerNames() As String)
        MyBase.New(methodName, _methodListenerNames)
    End Sub

    Public Sub New(jsValue As JsValue, methodListenerNames() As String)
        MyBase.New(jsValue, methodListenerNames)
    End Sub

    Public Shadows Function GetValue() As Type
        Return Me.Invoke(New InvocationContext)
    End Function

    Public Shadows Function Invoke(context As InvocationContext) As Type
        Dim typeName As String = MyBase.Invoke(context)
        If String.IsNullOrWhiteSpace(typeName) Then
            Throw New FrontWorkException("FieldType cannot be null!")
        End If
        If typeName.Equals("int", StringComparison.OrdinalIgnoreCase) Then
            Return GetType(Integer)
        ElseIf typeName.Equals("double", StringComparison.OrdinalIgnoreCase) Then
            Return GetType(Double)
        ElseIf typeName.Equals("string", StringComparison.OrdinalIgnoreCase) Then
            Return GetType(String)
        ElseIf typeName.Equals("bool", StringComparison.OrdinalIgnoreCase) Then
            Return GetType(Boolean)
        ElseIf typeName.Equals("datetime", StringComparison.OrdinalIgnoreCase) Then
            Return GetType(DateTime)
        Else
            Throw New FrontWorkException($"Type {typeName} is not supported" & vbCrLf &
                                         "now only ""int"",""double"",""string"",""bool"",""datetime"" are supported.")
        End If
    End Function

    Public Overrides Function Clone() As Object
        If Me.IsStaticValue Then Return New FieldTypeProperty(Me.Value)
        Return New FieldTypeProperty(Me.TargetMethodName, Me.MethodListenerNames)
    End Function
End Class

Imports Jint.Native

Public Class FieldProperty(Of T)
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

    Public Overridable Shadows Function GetValue() As T
        Return Me.Invoke(New InvocationContext)
    End Function

    Public Overridable Shadows Function Invoke(context As InvocationContext) As T
        Dim ret = MyBase.Invoke(context)
        If (GetType(T) = GetType(AssociationItem())) Then
            Console.WriteLine()
        End If
        Return Util.ChangeType(ret, GetType(T))
    End Function

    Public Overrides Function Clone() As Object
        If Me.IsStaticValue Then Return New FieldProperty(Of T)(Me.Value)
        Return New FieldProperty(Of T)(Me.TargetMethodName, Me.MethodListenerNames)
    End Function
End Class

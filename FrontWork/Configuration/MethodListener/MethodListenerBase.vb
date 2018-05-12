''' <summary>
''' 方法监听器基类
''' </summary>
Public MustInherit Class MethodListenerBase
    Public Overridable Property MethodListenerName As String = Me.GetType.Name
End Class

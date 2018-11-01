<AttributeUsage(AttributeTargets.Class, Inherited:=True)>
Public Class MethodListenerAttribute
    Inherits FrontWorkAttribute
    Implements IInvocationParameterAttribute

    Public Sub New()

    End Sub

    Public Sub New(name As String)
        Me.Name = name
    End Sub

    Public Property Name As String = Nothing
End Class

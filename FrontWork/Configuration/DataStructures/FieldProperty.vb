Imports System.ComponentModel.Design
Imports System.Linq
Imports System.Reflection
Imports System.Text
Imports Jint.Native

Public Class FieldProperty
    Implements ICloneable

    Protected Shared presetMethodListenerNames As String() = {"_StdMethodListener"}

    ''' <summary>
    ''' 是否返回静态值
    ''' </summary>
    ''' <returns></returns>
    Protected Property IsStaticValue As Boolean = False

    ''' <summary>
    ''' 参数列表
    ''' </summary>
    ''' <returns></returns>
    Protected Property InvocationContext As InvocationContext = Nothing

    ''' <summary>
    ''' 值。如果是静态值则此值为静态值，如果为方法则此值为函数返回值
    ''' </summary>
    ''' <returns></returns>
    Protected Property Value As Object

    ''' <summary>
    ''' 要执行的函数名
    ''' </summary>
    ''' <returns></returns>
    Protected Property TargetMethodName As String

    Protected Property MethodListenerNames As String() = {}

    ''' <summary>
    ''' 创建一个新的FieldProperty
    ''' </summary>
    ''' <param name="methodName">方法名</param>
    ''' <param name="_methodListenerNames">方法监听器的名字</param>
    Public Sub New(methodName As String, _methodListenerNames As String())
        Call Me.SetMethodListenerNames(_methodListenerNames)
        Me.TargetMethodName = methodName
    End Sub

    ''' <summary>
    ''' 创建一个具有固定返回值的FieldProperty
    ''' </summary>
    ''' <param name="value">字段的值</param>
    Public Sub New(value As Object)
        Me.IsStaticValue = True
        Me.Value = value
    End Sub

    Public Sub New(jsValue As JsValue, methodListenerNames As String())
        If jsValue.IsString Then '如果为#开头的字符串，则调用MethodListener的方法
            Dim strValue = jsValue.ToString
            If strValue.StartsWith("#") Then
                Dim methodName = strValue.Substring(1)
                '实例化一个绑定MethodListener方法的方法。运行时该方法动态执行MethodListener中的相应方法
                Call Me.SetMethodListenerNames(methodListenerNames)
                Me.TargetMethodName = methodName
            Else
                Me.IsStaticValue = True
                Me.Value = strValue
                Return
            End If
        Else
            Me.IsStaticValue = True
            Me.Value = jsValue.ToObject
            Return
        End If
    End Sub

    Public Shared Operator =(fieldMethod1 As FieldProperty, fieldMethod2 As FieldProperty) As Boolean
        If fieldMethod1 Is fieldMethod2 Then Return True
        If fieldMethod1 Is Nothing OrElse fieldMethod2 Is Nothing Then Return False
        If fieldMethod1.IsStaticValue <> fieldMethod2.IsStaticValue Then Return False
        '返回静态值的情况
        If fieldMethod1.IsStaticValue Then
            If fieldMethod1.Value.Equals(fieldMethod2.Value) Then
                Return True
            Else
                Return False
            End If
        End If
        '动态调用的情况
        If Not fieldMethod1.TargetMethodName.Equals(fieldMethod2.TargetMethodName) Then Return False
        If Not fieldMethod1.MethodListenerNames?.Length = fieldMethod2.MethodListenerNames?.Length Then Return False
        For i = 0 To fieldMethod1.MethodListenerNames.Length - 1
            If Not fieldMethod1.MethodListenerNames(i).Equals(fieldMethod2.MethodListenerNames(i)) Then Return False
        Next
        Return True
    End Operator

    Public Shared Operator <>(fieldMethod1 As FieldProperty, fieldMethod2 As FieldProperty) As Boolean
        Return Not fieldMethod1 = fieldMethod2
    End Operator

    ''' <summary>
    ''' 重新设置方法监听器
    ''' </summary>
    ''' <param name="methodListenerNames"></param>
    Public Sub SetMethodListenerNames(methodListenerNames As String())
        Me.MethodListenerNames = methodListenerNames.Union(presetMethodListenerNames).ToArray
    End Sub

    Public Overridable Function GetValue() As Object
        Return Me.Invoke(New InvocationContext)
    End Function

    ''' <summary>
    ''' 执行函数
    ''' </summary>
    ''' <param name="context">参数上下文</param>
    ''' <returns>返回值，执行函数后，返回ReturnValue</returns>
    Public Overridable Function Invoke(context As InvocationContext) As Object
        Me.InvocationContext = context
        Call Me.TargetFunc()
        Return Value
    End Function

    Public Overridable Function Clone() As Object Implements ICloneable.Clone
        If Me.IsStaticValue Then Return New FieldProperty(Me.Value)
        Return New FieldProperty(Me.TargetMethodName, Me.MethodListenerNames)
    End Function

    Protected Sub TargetFunc()
        If Me.IsStaticValue Then
            Me.Value = Me.Value
            Return
        End If
        '如果是设计器模式，直接返回空，不要调用方法监听器
        If System.Diagnostics.Process.GetCurrentProcess.ProcessName = "devenv" Then
            Me.Value = Nothing
            Return
        End If
        For i = 0 To Me.MethodListenerNames.Length - 1
            Dim methodListenerName = Me.MethodListenerNames(i)
            Dim methodListener = MethodListenerContainer.Get(methodListenerName)
            If methodListener Is Nothing Then
                Throw New MethodNotFoundException(vbCrLf & $"  MethodListener: ""{methodListenerName}"" not found!" &
                                    vbCrLf &
                                    "  Have you register your MethodListener into MethodListenerContainer before it should be called?")
            End If
            Dim method As MethodInfo = Nothing
            Try
                method = methodListener.GetType().GetMethod(Me.TargetMethodName, BindingFlags.Instance Or BindingFlags.Static Or BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.IgnoreCase)
            Catch ex As Exception
                Throw New FrontWorkException($"Get ""{Me.TargetMethodName}"" from ""{methodListener.GetType.Name}"" failed:" & vbCrLf & ex.Message)
            End Try
            '如果到了最后一个方法监听器还没有找到目标方法，则抛出错误
            If method Is Nothing Then
                If i = Me.MethodListenerNames.Length - 1 Then
                    Dim sbMethodListenerNames = New StringBuilder
                    sbMethodListenerNames.Append("[")
                    For Each name In Me.MethodListenerNames
                        sbMethodListenerNames.Append(name)
                        sbMethodListenerNames.Append(", ")
                    Next
                    sbMethodListenerNames.Length -= 1
                    sbMethodListenerNames.Append("]")
                    Throw New MethodNotFoundException($"Method: ""{Me.TargetMethodName}"" not found in MethodListener: {sbMethodListenerNames.ToString}!")
                    Return
                Else
                    Continue For
                End If
            End If
            Me.Value = Me.InvocationContext.Invoke(methodListener, method)
            Return
        Next
    End Sub

    Public Shared Widening Operator CType(fieldProperty As FieldProperty) As String
        Return fieldProperty.GetValue?.ToString
    End Operator
End Class

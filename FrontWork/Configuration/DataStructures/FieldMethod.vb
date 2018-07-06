Imports System.ComponentModel.Design
Imports System.Linq
Imports System.Reflection
Imports System.Text
Imports Jint.Native
''' <summary>
''' 字段方法，对于函数类型的字段属性，字段属性的实际类型为此类型
''' </summary>
Public Class FieldMethod
    Implements ICloneable

    Private Shared presetMethodListenerNames As String() = {"_StdMethodListener"}

    ''' <summary>
    ''' 配置中声明该方法的字符串
    ''' </summary>
    ''' <returns></returns>
    Public Property DeclareString As String

    ''' <summary>
    ''' 是否返回静态值
    ''' </summary>
    ''' <returns></returns>
    Private Property ReturnsStaticValue As Boolean = False

    ''' <summary>
    ''' 如果返回静态值，则返回该值
    ''' </summary>
    ''' <returns></returns>
    Private Property StaticReturnValue As Object = Nothing

    ''' <summary>
    ''' 参数列表
    ''' </summary>
    ''' <returns></returns>
    Private Property InvocationContext As InvocationContext = Nothing

    ''' <summary>
    ''' 返回值，Invoke后返回此值
    ''' </summary>
    ''' <returns></returns>
    Private Property ReturnValue As Object

    ''' <summary>
    ''' 要执行的函数名
    ''' </summary>
    ''' <returns></returns>
    Private Property TargetMethodName As String

    Private Property MethodListenerNames As String() = {}

    Public Shared Operator =(fieldMethod1 As FieldMethod, fieldMethod2 As FieldMethod) As Boolean
        If fieldMethod1 Is fieldMethod2 Then Return True
        If fieldMethod1 Is Nothing OrElse fieldMethod2 Is Nothing Then Return False
        If fieldMethod1.ReturnsStaticValue <> fieldMethod2.ReturnsStaticValue Then Return False
        '返回静态值的情况
        If fieldMethod1.ReturnsStaticValue Then
            If fieldMethod1.StaticReturnValue.Equals(fieldMethod2.StaticReturnValue) Then
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

    Public Shared Operator <>(fieldMethod1 As FieldMethod, fieldMethod2 As FieldMethod) As Boolean
        Return Not fieldMethod1 = fieldMethod2
    End Operator
    ''' <summary>
    ''' 重新设置方法监听器
    ''' </summary>
    ''' <param name="methodListenerNames"></param>
    Public Sub SetMethodListenerNames(methodListenerNames As String())
        Me.MethodListenerNames = methodListenerNames.Union(presetMethodListenerNames).ToArray
    End Sub

    ''' <summary>
    ''' 执行函数
    ''' </summary>
    ''' <param name="context">参数上下文</param>
    ''' <returns>返回值，执行函数后，返回ReturnValue</returns>
    Public Function Invoke(context As InvocationContext) As Object
        Me.InvocationContext = context
        Me.ReturnValue = Nothing
        Call Me.TargetFunc()
        Return ReturnValue
    End Function

    ''' <summary>
    ''' 创建一个新的FieldMethod
    ''' </summary>
    ''' <param name="methodName">方法名</param>
    ''' <param name="_methodListenerNames">方法监听器的名字</param>
    ''' <returns>新建的FieldMethod</returns>
    Public Shared Function NewInstance(methodName As String, _methodListenerNames As String(), declareString As String) As FieldMethod
        Dim fieldMethod As New FieldMethod
        fieldMethod.DeclareString = declareString
        If methodName.StartsWith("#") Then methodName = methodName.Substring(1)
        _methodListenerNames = If(_methodListenerNames, {}).Union(presetMethodListenerNames).ToArray
        fieldMethod.MethodListenerNames = _methodListenerNames
        fieldMethod.TargetMethodName = methodName
        Return fieldMethod
    End Function

    ''' <summary>
    ''' 创建一个具有固定返回值的FieldMethod
    ''' </summary>
    ''' <param name="returnValue">返回值</param>
    ''' <returns>创建的FieldMethod</returns>
    Public Shared Function NewInstance(returnValue As Object, declareString As String) As FieldMethod
        Dim newFieldMethod = New FieldMethod
        newFieldMethod.DeclareString = declareString
        newFieldMethod.ReturnsStaticValue = True
        newFieldMethod.StaticReturnValue = returnValue
        Return newFieldMethod
    End Function

    Public Shared Function FromJsValue(jsValue As JsValue, methodListenerNames As String()) As FieldMethod
        If jsValue.IsString Then '如果为#开头的字符串，则调用MethodListener的方法
            Dim strValue = jsValue.ToString
            If strValue.StartsWith("#") Then
                Dim methodName = strValue.Substring(1)
                '实例化一个绑定MethodListener方法的方法。运行时该方法动态执行MethodListener中的相应方法
                Dim newFieldMethod As FieldMethod = FieldMethod.NewInstance(methodName, methodListenerNames, strValue)
                Return newFieldMethod
            Else
                Return FieldMethod.NewInstance(strValue, strValue)
            End If
        Else
            Return FieldMethod.NewInstance(jsValue.ToObject, CType(jsValue.ToString, String))
            'Else
            '    throw new FrontWorkException($"Unsupported value for field:{jsValue.ToString}")
        End If
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim newFieldMethod As New FieldMethod
        newFieldMethod.DeclareString = Me.DeclareString
        newFieldMethod.ReturnsStaticValue = Me.ReturnsStaticValue
        newFieldMethod.StaticReturnValue = Me.StaticReturnValue
        newFieldMethod.MethodListenerNames = Me.MethodListenerNames
        newFieldMethod.TargetMethodName = Me.TargetMethodName
        Return newFieldMethod
    End Function

    Private Structure CandidateParamInheritDepth
        Public Property CandidateParam As Object
        Public Property InheritDepth As Integer
    End Structure

    Private Structure ExpectedParamInfo
        Public Sub New(valueType As Type, attributeType As Type)
            Me.New()
            Me.AttributeType = attributeType
            Me.ValueType = valueType
        End Sub

        Public Property AttributeType As Type
        Public Property ValueType As Type

    End Structure

    Private Shared Function MatchParams(context As InvocationContext, expectedParamInfos As ExpectedParamInfo()) As Object()
        '匹配参数列表结果
        Dim paramList As New List(Of Object)
        '遍历期待参数列表，若找到传入参数中合适的参数类型，则将传入参数加入到paramList，若找不到，加入Nothing
        For Each expectedParamInfo In expectedParamInfos
            '首先如果目标函数的参数标了注解，则按注解寻找。找不到抛出异常
            If expectedParamInfo.AttributeType IsNot Nothing Then
                Dim found = False
                For Each contextItem As InvocationContextItem In context.ContextItems
                    If contextItem.AttributeType = expectedParamInfo.AttributeType Then
                        paramList.Add(contextItem.Value)
                        found = True
                        Exit For
                    End If
                Next
                If Not found Then
                    Throw New AttributedValueNotFoundException($"Attributed parameter [{expectedParamInfo.AttributeType.Name}] not found!")
                Else
                    Continue For
                End If
            Else '如果没有标注注解，则寻找类型最匹配的参数
                Dim expectedParamType = expectedParamInfo.ValueType
                '候选参数列表
                Dim candidateParams As New List(Of CandidateParamInheritDepth)
                For Each contextItem As InvocationContextItem In context.ContextItems
                    Dim param = contextItem.Value
                    If param Is Nothing Then Continue For

                    If param.GetType.Equals(expectedParamType) OrElse
                    param.GetType.IsSubclassOf(expectedParamType) OrElse
                    param.GetType.GetInterface(expectedParamType.Name) IsNot Nothing Then
                        candidateParams.Add(New CandidateParamInheritDepth With {
                        .CandidateParam = param,
                        .InheritDepth = GetInheritDepth(expectedParamType, param.GetType)
                    })
                        Exit For
                    End If
                Next
                '如果没找到匹配参数，则置入Nothing
                If candidateParams.Count = 0 Then
                    paramList.Add(DefaultForType(expectedParamType))
                Else '否则寻找最匹配参数
                    Dim minInheriteDepth = (From c In candidateParams Select c.InheritDepth).Min
                    Dim bestMatchParam = (From c In candidateParams Where c.InheritDepth = minInheriteDepth Select c.CandidateParam).First
                    paramList.Add(bestMatchParam)
                End If
            End If
        Next
        Return paramList.ToArray
    End Function

    Private Shared Function GetInheritDepth(baseType As Type, subType As Type) As Integer
        Dim depth = 0
        '如果是同一类型，返回0
        If subType.Equals(baseType) Then Return 0
        '如果是实现接口，返回1
        If subType.GetInterface(baseType.Name) IsNot Nothing Then Return 1
        If Not subType.IsSubclassOf(baseType) Then
            Throw New FrontWorkException($"SubType: {subType.Name} is not a sub type of BaseType: {baseType.Name}")
        End If
        Dim curType = subType
        While Not curType.Equals(baseType)
            depth += 1
            curType = curType.BaseType
        End While
        Return depth
    End Function

    Private Shared Function DefaultForType(targetType As Type) As Object
        If targetType.IsValueType Then
            Return Activator.CreateInstance(targetType)
        Else
            Return Nothing
        End If
    End Function

    Private Sub TargetFunc()
        If Me.ReturnsStaticValue Then
            Me.ReturnValue = Me.StaticReturnValue
            Return
        End If
        '如果是设计器模式，直接返回空，不要调用方法监听器
        If System.Diagnostics.Process.GetCurrentProcess.ProcessName = "devenv" Then
            Me.ReturnValue = Nothing
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
            Dim [paramArray] As Object()
            Dim targetMethodParams = method.GetParameters
            If targetMethodParams.Length > 0 Then
                Dim expectedParamInfos(targetMethodParams.Length - 1) As ExpectedParamInfo
                For j = 0 To targetMethodParams.Length - 1
                    expectedParamInfos(j).ValueType = targetMethodParams(j).ParameterType
                    Dim customInvocationParamAttributes = targetMethodParams(j).GetCustomAttributes(GetType(IInvocationParameterAttribute), True)
                    If customInvocationParamAttributes.Length = 0 Then
                        Continue For
                    Else
                        expectedParamInfos(j).AttributeType = customInvocationParamAttributes(0).GetType
                    End If
                Next
                [paramArray] = MatchParams(Me.InvocationContext, expectedParamInfos)
            Else
                [paramArray] = {}
            End If

            Try
                Me.ReturnValue = method.Invoke(methodListener, [paramArray].ToArray)
                Return
            Catch ex As Exception
                Throw New FrontWorkException($"Invoke method ""{Me.TargetMethodName}"" in methodListener {methodListenerName} failed: " + ex.Message)
                Return
            End Try
        Next
    End Sub
End Class

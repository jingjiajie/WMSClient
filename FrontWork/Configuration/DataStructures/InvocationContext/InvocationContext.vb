Imports System.Linq
Imports System.Reflection

Public Class InvocationContext
    Public Property ContextItems As New InvocationContextItemCollection

    Public Function Invoke(targetObject As Object, method As MethodInfo) As Object
        '根据方法的参数生成InvocationExpectedParameterInfo()
        Dim parameterArray As Object()
        Dim targetMethodParams = method.GetParameters
        If targetMethodParams.Length > 0 Then
            Dim expectedParamInfos(targetMethodParams.Length - 1) As InvocationExpectedParameterInfo
            For j = 0 To targetMethodParams.Length - 1
                Dim valueType = targetMethodParams(j).ParameterType
                Dim attrType As Type = Nothing
                Dim customInvocationParamAttributes = targetMethodParams(j).GetCustomAttributes(GetType(IInvocationParameterAttribute), True)
                If customInvocationParamAttributes.Length = 0 Then
                    attrType = Nothing
                Else
                    attrType = customInvocationParamAttributes(0).GetType
                End If
                expectedParamInfos(j) = New InvocationExpectedParameterInfo(targetMethodParams(j).Name, valueType, attrType)
            Next
            parameterArray = Me.MatchParameters(method, expectedParamInfos)
        Else
            parameterArray = {}
        End If
        Return method.Invoke(targetObject, parameterArray)
    End Function

    Public Function MatchParameters(method As MethodInfo, expectedParamInfos As InvocationExpectedParameterInfo()) As Object()
        '匹配参数列表结果
        Dim paramList As New List(Of Object)
        '遍历期待参数列表，若找到传入参数中合适的参数类型，则将传入参数加入到paramList，若找不到，加入Nothing
        For Each expectedParamInfo In expectedParamInfos
            Dim expectedType = expectedParamInfo.ValueType
            '首先如果目标函数的参数标了注解，则按注解寻找。找不到抛出异常
            If expectedParamInfo.AttributeType IsNot Nothing Then
                Dim found = False
                For Each contextItem As InvocationContextItem In Me.ContextItems
                    If contextItem.AttributeType = expectedParamInfo.AttributeType Then
                        Dim value = Nothing
                        Try
                            value = Util.ChangeType(contextItem.Value, expectedType)
                        Catch ex As IncompatibleTypeException
                            Dim appendMessage = vbCrLf & $"When matching ""{contextItem.Value}"" to parameter" & vbCrLf &
                            $"{method.DeclaringType.Name}.{method.Name}( " &
                            $"{If(expectedParamInfo.AttributeType Is Nothing, "", $"[{expectedParamInfo.AttributeType.Name}]")} " &
                            $"{expectedType.Name} {expectedParamInfo.ParameterName} )"

                            Throw New ParameterMatchingException(ex.Message & appendMessage)
                        End Try
                        paramList.Add(value)
                        found = True
                        Exit For
                    End If
                Next
                If Not found Then
                    paramList.Add(Me.DefaultForType(expectedType))
                Else
                    Continue For
                End If
            Else '如果没有标注注解，则寻找类型最匹配的参数
                Dim expectedParamType = expectedParamInfo.ValueType
                '候选参数列表
                Dim candidateParams As New List(Of CandidateParamInheritDepth)
                For Each contextItem As InvocationContextItem In Me.ContextItems
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

    Private Function GetInheritDepth(baseType As Type, subType As Type) As Integer
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

    Private Function DefaultForType(targetType As Type) As Object
        If targetType.IsValueType Then
            Return Activator.CreateInstance(targetType)
        Else
            Return Nothing
        End If
    End Function

    Private Structure CandidateParamInheritDepth
        Public Property CandidateParam As Object
        Public Property InheritDepth As Integer
    End Structure
End Class


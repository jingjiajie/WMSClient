Imports Jint.Native
Imports System.IO
Imports System.Net
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web.Script.Serialization

''' <summary>
''' RESTful接口，Json数据格式的API信息
''' </summary>
Public Class JsonRESTAPIInfo
    ''' <summary>
    ''' URL模板，允许使用字符串插值格式来嵌入Request参数
    ''' </summary>
    ''' <returns></returns>
    Public Property URLTemplate As String

    ''' <summary>
    ''' HTTP方法
    ''' </summary>
    ''' <returns></returns>
    Public Property HTTPMethod As HTTPMethod

    ''' <summary>
    ''' 请求体模板
    ''' </summary>
    ''' <returns></returns>
    Public Property RequestBodyTemplate As String

    ''' <summary>
    ''' 相应体模板
    ''' </summary>
    ''' <returns></returns>
    Public Property ResponseBodyTemplate As String

    ''' <summary>
    ''' API调用完成后的回调函数
    ''' </summary>
    ''' <returns></returns>
    Public Property Callback As Func(Of HttpWebResponse, WebException, Boolean)

    Private requestJSEngine As New Jint.Engine
    Private responseJSEngine As New Jint.Engine

    Public Sub New()
        requestJSEngine.SetValue("log", New Action(Of String)(AddressOf Console.WriteLine))
        responseJSEngine.SetValue("log", New Action(Of String)(AddressOf Console.WriteLine))
        Dim strMapProperty = <string>
                            function getPropertyIgnoreCase(obj,propName){
                                for(p in obj){
                                    if(p.toLowerCase() == propName.toLowerCase()){
                                        return obj[p];
                                    }
                                }
                                return null;
                            }

                             function mapProperty(objs,propName){
                                if(typeof(objs)!='object' || typeof(propName)!='string'){
                                    log("mapProperty usage: mapProperty(&lt;object&gt;,&lt;propertyName&gt;)");
                                    return null;
                                }
                                var result = []
                                for(var i=0;i&lt;objs.length;i++){
                                    result.push(getPropertyIgnoreCase(objs[i],propName))
                                }
                                return result
                             }
                         </string>.Value
        requestJSEngine.Execute(strMapProperty)
        responseJSEngine.Execute(strMapProperty)
    End Sub

    ''' <summary>
    ''' 设置响应体参数
    ''' </summary>
    ''' <param name="paramName">参数名</param>
    Public Sub SetResponseParameter(paramName As String)
        Me.responseJSEngine.SetValue(paramName, JsValue.Null)
    End Sub

    ''' <summary>
    ''' 设置请求参数
    ''' </summary>
    ''' <param name="paramName">参数名</param>
    ''' <param name="value">参数值</param>
    Public Sub SetRequestParameter(paramName As String, value As Object)
        If value Is Nothing Then '如果是Nothing，直接设置为null
            Me.requestJSEngine.SetValue(paramName, JsValue.Null)
        ElseIf TypeOf (value) Is JsValue Then '如果是JsValue，原样置入
            Call Me.requestJSEngine.SetValue(paramName, value)
        ElseIf value.GetType.IsValueType OrElse TypeOf value Is String Then '对于基本类型，直接设置值
            Me.requestJSEngine.SetValue(paramName, value)
        Else '对于其他对象，序列化成json对象
            Dim serializer As New JavaScriptSerializer
            Dim serializedStr = serializer.Serialize(value)
            serializedStr = Regex.Replace(serializedStr, "\\/Date\((\d+)\)\\/",
                                      Function(match)
                                          Dim dt = New DateTime(1970, 1, 1)
                                          dt = dt.AddMilliseconds(Long.Parse(match.Groups(1).Value))
                                          dt = dt.ToLocalTime()
                                          Return dt.ToString("yyyy-MM-dd HH:mm:ss")
                                      End Function)
            Call Me.SetJsonRequestParameter(paramName, serializedStr)
        End If
    End Sub

    ''' <summary>
    ''' 设置Json请求参数
    ''' </summary>
    ''' <param name="paramName">参数名</param>
    ''' <param name="jsonString">参数值</param>
    Public Sub SetJsonRequestParameter(paramName As String, jsonString As String)
        Try
            If jsonString Is Nothing Then
                Me.requestJSEngine.Execute($"{paramName} = null;")
            Else
                Me.requestJSEngine.Execute($"{paramName} = JSON.parse('{jsonString}');")
            End If
        Catch
            Throw New FrontWorkException($"Invalid jsonString: ""{jsonString}""")
        End Try
    End Sub

    ''' <summary>
    ''' 获取Url，如果Url模板中包含参数，此处获取的Url为最终结果
    ''' </summary>
    ''' <returns>url</returns>
    Public Function GetURL() As String
        Logger.SetMode(LogMode.SYNCHRONIZER)
        Dim resultURL As New StringBuilder
        Dim reg = "\{(((?<clojure>\{)|(?<-clojure>\})|[^{}])+(?(clojure)(?!)))\}"
        Dim curMatch = Regex.Match(Me.URLTemplate, reg)
        Dim lastMatch As Match = Nothing
        Do While curMatch.Success '遍历匹配到的各个"{表达式}"
            Dim expr As String = curMatch.Groups(1).Value
            Dim value As String
            If String.IsNullOrEmpty(expr) Then
                value = "{}"
            Else
                Try
                    Dim exprType = requestJSEngine.Execute($"typeof({expr})").GetCompletionValue.ToString
                    If exprType = "object" OrElse exprType = "array" Then
                        value = Me.requestJSEngine.Execute($"JSON.stringify({expr})").GetCompletionValue.ToString
                    Else
                        value = Me.requestJSEngine.Execute(expr).GetCompletionValue.ToString
                    End If
                Catch ex As Exception
                    Throw New FrontWorkException($"Invalid expression: ""{expr}""" & vbCrLf & $"Message: {ex.Message}")
                    Return Nothing
                End Try
            End If
            If lastMatch Is Nothing Then
                If curMatch.Index <> 0 Then '如果是首个匹配，且不是从头开始。则将字符串开头至匹配开始的字符加进来
                    resultURL.Append(Me.URLTemplate.Substring(0, curMatch.Index))
                End If
            Else '增加上次匹配结束到本次匹配开始中间的字符串
                Dim lastSpanIndex = lastMatch.Index + lastMatch.Value.Length '上一个匹配到此匹配中间间隔的字符串的起始下标
                '将变量替换好的字符串
                Dim replacedStr = Me.URLTemplate.Substring(lastSpanIndex, curMatch.Index - lastSpanIndex)
                resultURL.Append(replacedStr)
            End If
            resultURL.Append(value)
            lastMatch = curMatch
            curMatch = curMatch.NextMatch
        Loop
        '将最后一个匹配到字符串结尾的部分加上
        If lastMatch Is Nothing Then '如果lastMatch为空，说明整个字符串没有插值
            resultURL.Append(Me.URLTemplate)
        Else '否则lastMatch是最后一个匹配，因为curMatch在循环结束后一定会next到最后一个匹配的下一个，也就是空匹配
            resultURL.Append(Me.URLTemplate.Substring(lastMatch.Index + lastMatch.Length))
        End If
        Return resultURL.ToString
    End Function

    ''' <summary>
    ''' 获取请求体，如果请求体中包含参数，此处为最终结果
    ''' </summary>
    ''' <returns></returns>
    Public Function GetRequestBody() As String
        If String.IsNullOrEmpty(Me.RequestBodyTemplate) Then Return String.Empty
        Return Me.requestJSEngine.Execute($"JSON.stringify({RequestBodyTemplate})").GetCompletionValue.ToString
    End Function

    ''' <summary>
    ''' 获取响应体参数值
    ''' </summary>
    ''' <param name="responsebody">响应体</param>
    ''' <param name="paramNames">响应体参数</param>
    ''' <returns>参数值</returns>
    Public Function GetResponseParameters(responsebody As String, paramNames As String()) As Object()
        Logger.SetMode(LogMode.SYNCHRONIZER)
        Dim responseJsValue As JsValue = Nothing
        Try
            responseJsValue = Me.requestJSEngine.Execute("$_EPFResponse=" & responsebody).GetValue("$_EPFResponse")
        Catch
            Throw New FrontWorkException($"Invalid ResponseBody:{responsebody}")
        End Try
        '根据ResponseBody获取各个ResponseParameter的位置
        Dim paramPaths As Dictionary(Of String, String()) = Me.GetResponseBodyTemplateParamPaths(paramNames)
        Dim result(paramNames.Length - 1) As Object
        For i = 0 To paramNames.Length - 1
            Dim paramName = paramNames(i)
            '如果此参数没有找到路径，报错并Continue
            If Not paramPaths.ContainsKey(paramName) Then
                Logger.PutMessage($"Parameter ""{paramName}"" not found in ResponseBodyTemplate!")
                Continue For
            End If
            Dim paramPath = paramPaths(paramName)
            Dim curJsValue As JsValue = responseJsValue
            '根据参数的路径去寻找参数
            For Each prop In paramPath
                curJsValue = curJsValue.AsObject.GetOwnProperty(prop).Value
            Next
            result(i) = curJsValue.ToObject
        Next
        Return result
    End Function

    ''' <summary>
    ''' 在响应体中寻找相应体参数的路径
    ''' </summary>
    ''' <param name="paramNames">参数名</param>
    ''' <returns>各个参数的路径</returns>
    Private Function GetResponseBodyTemplateParamPaths(paramNames As String()) As Dictionary(Of String, String())
        Dim result As New Dictionary(Of String, String())
        Me.responseJSEngine.Execute("objsToFind = {};")

        For Each paramName In paramNames
            'Me.responseJSEngine.Execute($"{paramName} = new Object();")
            Me.responseJSEngine.Execute($"objsToFind['{paramName}'] = {paramName};")
        Next
        Dim strFindData = '从给定jsonTemplate中寻找$data变量
            <string>
                var objPaths = {}

                //填充objPaths，将从curObj中找到的对象的路径填充到objPaths里
                function fillObjPath(curObj){
                  if(!fillObjPath.stackInited){
                    fillObjPath.stack = []
                    fillObjPath.stackInited = true
                  }
                  var foundName = findKeyInObject(curObj,objsToFind)
                  if(foundName){
                    objPaths[foundName] = copyArray(fillObjPath.stack)
                    return
                  }
                  if(typeof(curObj) == 'object'){
                      for(var key in curObj){
                        fillObjPath.stack.push(key)
                        fillObjPath(curObj[key])
                        fillObjPath.stack.pop()
                      }
                  }
                }

                //在对象中根据值搜索key，找到返回key，找不到返回null
                function findKeyInObject(value,obj){
                  for(var key in obj){
                    if(obj[key] == value){
                      return key
                    }
                  }
                  return null
                }

                //数组拷贝
                function copyArray(arr) {
                    let res = []
                    for (let i = 0; i &lt; arr.length; i++) {
                     res.push(arr[i])
                    }
                    return res
                }
            </string>.Value
        Me.responseJSEngine.Execute(strFindData)
        Try
            Me.responseJSEngine.Execute(String.Format("var $_EPFTargetObject = {0}", Me._ResponseBodyTemplate))
        Catch
            Throw New FrontWorkException("Invalid ResponseBodyTemplate")
        End Try
        Me.responseJSEngine.Execute("fillObjPath($_EPFTargetObject)")
        For Each paramName In paramNames
            Dim dataPath = Me.responseJSEngine.Execute($"objPaths['{paramName}']").GetCompletionValue
            If dataPath.IsUndefined OrElse dataPath.IsNull Then
                Logger.PutMessage($"{paramName} not found!")
                Continue For
            End If
            '把Object()转为String()
            Dim dataPathArray = Util.ToArray(Of String)(dataPath.ToObject)
            result.Add(paramName, dataPathArray)
        Next
        Return result
    End Function

    Public Function Invoke() As HTTPResponse
        Static calledLeaveDotsAndSlashesEscaped As Boolean = False
        If Not calledLeaveDotsAndSlashesEscaped Then
            Call LeaveDotsAndSlashesEscaped()
            calledLeaveDotsAndSlashesEscaped = True
        End If
        Dim strUrl = Me.GetURL
        Dim uri = New Uri(strUrl)
        Dim requestBody = Me.GetRequestBody
        Logger.Debug(Me.HTTPMethod.ToString & " " & strUrl & vbCrLf & requestBody)
        Dim httpWebRequest = CType(WebRequest.Create(uri), HttpWebRequest)
        httpWebRequest.Timeout = 15000
        httpWebRequest.ReadWriteTimeout = 15000
        ServicePointManager.DefaultConnectionLimit = 500

        httpWebRequest.Method = Me.HTTPMethod.ToString
        httpWebRequest.ServicePoint.Expect100Continue = False
        If Me.HTTPMethod = HTTPMethod.POST OrElse Me.HTTPMethod = HTTPMethod.PUT Then
            httpWebRequest.ContentType = "application/json"
            Dim bytes = Encoding.UTF8.GetBytes(requestBody)
            Dim stream = httpWebRequest.GetRequestStream
            stream.Write(bytes, 0, bytes.Length)
        End If

        Dim httpResponse As HTTPResponse
        Try
            Using response = CType(httpWebRequest.GetResponse, HttpWebResponse)
                Dim code = response.StatusCode
                Dim reader = New StreamReader(response.GetResponseStream)
                Dim body = reader.ReadToEnd
                httpResponse = New HTTPResponse(code, body)
            End Using
        Catch ex As WebException
            Dim response As HttpWebResponse = ex.Response
            Dim code As Integer
            Dim errorMsg As String
            If response Is Nothing Then
                code = -1
                errorMsg = ex.Message
            Else
                code = response.StatusCode
                errorMsg = (New StreamReader(response.GetResponseStream)).ReadToEnd
            End If
            httpResponse = New HTTPResponse(code, Nothing, errorMsg)
        End Try

        Return httpResponse
    End Function

    Private Const UnEscapeDotsAndSlashes As Integer = &H2000000

    Private Shared Sub LeaveDotsAndSlashesEscaped()
        Dim getSyntaxMethod = GetType(UriParser).GetMethod("GetSyntax", BindingFlags.[Static] Or BindingFlags.NonPublic)

        If getSyntaxMethod Is Nothing Then
            Throw New MissingMethodException("UriParser", "GetSyntax")
        End If

        Dim uriParser = getSyntaxMethod.Invoke(Nothing, New Object() {"http"})
        Dim flagsFieldInfo As FieldInfo = GetType(UriParser).GetField("m_Flags", BindingFlags.NonPublic Or BindingFlags.GetField Or BindingFlags.SetField Or BindingFlags.Instance)

        If flagsFieldInfo Is Nothing Then
            Throw New MissingFieldException("UriParser", "m_Flags")
        End If

        Dim flags As Integer = CInt(flagsFieldInfo.GetValue(uriParser))
        flags = flags And Not UnEscapeDotsAndSlashes
        flagsFieldInfo.SetValue(uriParser, flags)
    End Sub
End Class

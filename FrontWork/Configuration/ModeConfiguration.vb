Imports System.Linq
''' <summary>
''' 模式配置，一个配置中心包含若干模式配置。一个模式配置中包含若干字段，API配置等信息
''' </summary>
Public Class ModeConfiguration
    Private _methodListeners As String()

    ''' <summary>
    ''' 模式名称
    ''' </summary>
    ''' <returns></returns>
    Public Property Mode As String

    ''' <summary>
    ''' 字段配置信息
    ''' </summary>
    ''' <returns></returns>
    Public Property Fields As FieldConfiguration() = {}

    ''' <summary>
    ''' HTTPAPI配置信息
    ''' </summary>
    ''' <returns></returns>
    Public Property HTTPAPIs As HTTPAPIConfiguration() = {}

    ''' <summary>
    ''' 方法监听器
    ''' </summary>
    Public Property MethodListenerNames As String()
        Get
            Return Me._methodListeners
        End Get
        Set(value As String())
            Me._methodListeners = value
            Call Me.SetMethodListener(value)
        End Set
    End Property

    '''' <summary>
    '''' 所属配置中心
    '''' </summary>
    '''' <returns></returns>
    'Public Property Configuration As Configuration

    ''' <summary>
    ''' Js引擎
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property JsEngine As New Jint.Engine

    ''' <summary>
    ''' 从JsValue转换
    ''' </summary>
    ''' <param name="modeMethodListenerNamesPairs">方法监听器名称</param>
    ''' <param name="jsValue">JsValue对象</param>
    ''' <returns></returns>
    Public Shared Function FromJsValue(modeMethodListenerNamesPairs As ModeMethodListenerNamesPair(), jsValue As Jint.Native.JsValue) As ModeConfiguration()
        If jsValue Is Nothing Then Throw New Exception("JsValue can not be null!")
        '如果是数组，则遍历解析
        If jsValue.IsArray Then
            Dim newModeConfigurations As New List(Of ModeConfiguration)
            For i As Integer = 0 To jsValue.AsArray.GetLength - 1
                Dim newModeConfiguration = MakeModeConfigurationFromJsValue(modeMethodListenerNamesPairs, jsValue.AsArray.Get(i), newModeConfigurations)
                newModeConfigurations.Add(newModeConfiguration)
            Next
            Return newModeConfigurations.ToArray
        ElseIf jsValue.IsObject Then
            '如果是对象，则直接解析
            Dim newModeConfiguration = MakeModeConfigurationFromJsValue(modeMethodListenerNamesPairs, jsValue, New List(Of ModeConfiguration))
            Return New ModeConfiguration() {newModeConfiguration}
        Else '既不是数组又不是对象，报错返回空
            Throw New Exception("Only js object or array is accepted to generate EditPanelModeConfiguration!")
        End If
    End Function

    ''' <summary>
    ''' 生成一个ModeConfiguration
    ''' </summary>
    ''' <param name="modeMethodListenerNamesPairs">方法监听器名称</param>
    ''' <param name="jsValue">JsValue对象</param>
    ''' <param name="prevModeConfigurations">之前已经初始化过的modeConfiguration，用来处理ref</param>
    ''' <returns></returns>
    Private Shared Function MakeModeConfigurationFromJsValue(modeMethodListenerNamesPairs As ModeMethodListenerNamesPair(), jsValue As Jint.Native.JsValue, prevModeConfigurations As List(Of ModeConfiguration)) As ModeConfiguration
        If jsValue Is Nothing Then Throw New Exception("JsValue can not be null!")
        If Not jsValue.IsObject Then
            Throw New Exception("Not a valid JsObject!")
            Return Nothing
        End If

        Dim jsObject = jsValue.AsObject

        '把字段，赋值给EditPanelModeConfiguration
        Dim newModeConfiguration As New ModeConfiguration
        If jsObject.HasOwnProperty("mode") Then
            newModeConfiguration.Mode = jsObject.GetOwnProperty("mode").Value.ToObject
        Else
            newModeConfiguration.Mode = "default"
            Throw New Exception("""mode"" property not found, set as ""default"" automatically")
        End If
        '该模式的方法监听器名称
        Dim methodListenerNames As String() = New String() {}
        Dim modeMethodListenerPair = (From mmp In modeMethodListenerNamesPairs
                                      Where mmp.Mode = newModeConfiguration.Mode
                                      Select mmp).FirstOrDefault
        If modeMethodListenerPair IsNot Nothing Then
            methodListenerNames = modeMethodListenerPair.MethodListenerNames
        End If

        '如果有引用模式，将引用模式的数据直接拷贝到当前模式
        If jsObject.HasOwnProperty("ref") Then
            Dim jsValueModeRef = jsObject.GetOwnProperty("ref").Value
            If Not jsValueModeRef.IsString Then
                Throw New Exception("ref property must be a string!")
            End If
            Dim modeRef As String = jsValueModeRef.ToString
            If modeRef = newModeConfiguration.Mode Then
                Throw New Exception($"Mode ""{modeRef}"" cannot reference it self!")
            End If
            '寻找引用的模式
            Dim foundModeConfiguration = prevModeConfigurations.Find(
                Function(modeConfiguration)
                    Return modeConfiguration.Mode = modeRef
                End Function)
            If foundModeConfiguration Is Nothing Then
                Throw New Exception($"Mode ""{modeRef}"" not found!")
            End If
            Dim newModeName = newModeConfiguration.Mode
            newModeConfiguration = Util.DeepClone(foundModeConfiguration)
            newModeConfiguration.Mode = newModeName
        End If

        '解析字段配置。如果ref引用了同名字段，则覆盖
        If jsObject.HasOwnProperty("fields") Then
            Dim newFields = FieldConfiguration.FromJsValue(methodListenerNames, jsObject.GetOwnProperty("fields").Value, newModeConfiguration.Fields)
            newModeConfiguration.Fields = newFields
        Else
            Throw New Exception("""fields"" property is necessary!")
        End If

        If jsObject.HasOwnProperty("httpAPIs") Then
            Dim newHTTPApis = HTTPAPIConfiguration.FromJSValue(methodListenerNames, jsObject.GetOwnProperty("httpAPIs").Value, newModeConfiguration.HTTPAPIs)
            newModeConfiguration.HTTPAPIs = newModeConfiguration.HTTPAPIs.Where(
                 Function(api)
                     Return (From a In newHTTPApis Where a.Type = api.Type Select a).Count = 0
                 End Function).Union(newHTTPApis).ToArray
        End If

        Return newModeConfiguration
    End Function

    ''' <summary>
    ''' 设置方法监听器
    ''' </summary>
    ''' <param name="methodListenerNames">方法监听器</param>
    Public Sub SetMethodListener(methodListenerNames As String())
        For Each field In Me.Fields
            Call field.SetMethodListener(methodListenerNames)
        Next
        For Each httpAPI In Me.HTTPAPIs
            Call httpAPI.SetMethodListener(methodListenerNames)
        Next
    End Sub
End Class

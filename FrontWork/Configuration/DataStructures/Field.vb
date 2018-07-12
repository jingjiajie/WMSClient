Imports System.Linq
Imports System.Reflection
Imports System.Text
Imports Jint.Native

''' <summary>
''' 字段配置信息
''' </summary>
Public Class Field
    Implements ICloneable
    Private _name As FieldProperty(Of String)
    Private _type As FieldTypeProperty = New FieldTypeProperty("string")
    ''' <summary>
    ''' 显示名称
    ''' </summary>
    ''' <returns></returns>
    Public Property DisplayName As FieldProperty(Of String) = Nothing

    ''' <summary>
    ''' 字段类型
    ''' </summary>
    ''' <returns></returns>
    Public Property Type As FieldTypeProperty
        Get
            Return _type
        End Get
        Set(value As FieldTypeProperty)
            If value Is Nothing Then
                Throw New FrontWorkException("Field Type cannot be null!")
            End If
            Me._type = value
        End Set
    End Property

    ''' <summary>
    ''' 占位符
    ''' </summary>
    ''' <returns></returns>
    Public Property PlaceHolder As New FieldProperty(Of String)(Nothing) 'no param returns string

    ''' <summary>
    ''' 是否可视
    ''' </summary>
    ''' <returns></returns>
    Public Property Visible As New FieldProperty(Of Boolean)(True)

    ''' <summary>
    ''' 是否可编辑
    ''' </summary>
    ''' <returns></returns>
    Public Property Editable As New FieldProperty(Of Boolean)(True)

    ''' <summary>
    ''' 字段可以选择的几种值，函数类型（）: Object数组
    ''' </summary>
    ''' <returns></returns>
    Public Property Values As FieldProperty(Of Object())

    ''' <summary>
    ''' 字段的默认值，函数类型（）: Object
    ''' </summary>
    ''' <returns></returns>
    Public Property DefaultValue As FieldProperty(Of Object)

    ''' <summary>
    ''' 前向映射，从模型显示到视图时的转换。函数类型(Object) : String
    ''' </summary>
    ''' <returns></returns>
    Public Property ForwardMapper As FieldProperty(Of String)

    ''' <summary>
    ''' 反向映射，从视图映射到模型时的转换。函数类型(String) : Object
    ''' </summary>
    ''' <returns></returns>
    Public Property BackwardMapper As FieldProperty(Of Object)

    ''' <summary>
    ''' 联想提示，回调传入用户已经输入的内容，返回联想提示内容。函数类型(String) : AssociationItem()
    ''' </summary>
    ''' <returns></returns>
    Public Property Association As FieldProperty(Of AssociationItem())

    ''' <summary>
    ''' 内容改变事件，传入该字段用户已经输入的文本，函数类型(String)
    ''' </summary>
    ''' <returns></returns>
    Public Property ContentChanged As FieldProperty 'Params String No Returns

    ''' <summary>
    ''' 编辑结束事件，传入该字段用户已经输入的文本，函数类型(String)
    ''' </summary>
    ''' <returns></returns>
    Public Property EditEnded As FieldProperty 'Params String No Returns

    ''' <summary>
    ''' 字段名
    ''' </summary>
    ''' <returns></returns>
    Public Property Name As FieldProperty(Of String)
        Get
            Return Me._name
        End Get
        Set(value As FieldProperty(Of String))
            If DisplayName Is Nothing Then DisplayName = value
            _name = value
        End Set
    End Property

    ''' <summary>
    ''' 从Jint.JsValue转换
    ''' </summary>
    ''' <param name="methodListenerNames">方法监听器名称</param>
    ''' <param name="jsValue">要转换的JsValue</param>
    ''' <returns></returns>
    Public Shared Function FromJsValue(methodListenerNames As String(), jsValue As JsValue, refFieldConfigurations As Field()) As Field()
        If jsValue Is Nothing Then Throw New FrontWorkException("JsValue can not be null!")
        '如果是数组，则遍历解析
        If jsValue.IsArray Then
            '先把引用的所有字段都拷贝过来，再根据新的配置进行更新
            Dim fieldConfigurations As List(Of Field) = refFieldConfigurations.ToList
            Dim jsArray = jsValue.AsArray
            For i = 0 To jsValue.AsArray.GetLength - 1
                If Not jsArray.Get(i).AsObject().HasOwnProperty("name") Then
                    Throw New FrontWorkException("Field configuration must contains ""name"" property!")
                End If
                '新配置的名称
                Dim name = jsArray.Get(i).AsObject.GetOwnProperty("name").Value.ToString
                '根据名称寻找是否存在引用的相应字段
                Dim foundRef = (From r In refFieldConfigurations Where r.Name.GetValue = name Select r).FirstOrDefault
                '生成合并后的配置（若无引用，则是全新配置）
                Dim newFieldConfiguration = MakeFieldConfigurationFromJsValue(jsArray.Get(i), methodListenerNames, foundRef)
                '如果字段有引用，则将合并的配置放到引用原来的位置
                If foundRef IsNot Nothing Then
                    '寻找新配置在已经拷贝过来的引用中是第几个
                    Dim pos = fieldConfigurations.FindIndex(
                        Function(field)
                            Return field.Name = foundRef.Name
                        End Function)
                    fieldConfigurations(pos) = newFieldConfiguration
                Else '否则新增加一条配置
                    fieldConfigurations.Add(newFieldConfiguration)
                End If
            Next
            Return fieldConfigurations.ToArray
        Else '不是数组，报错
            Throw New FrontWorkException("Only js array is accepted to generate field configuration!")
        End If
    End Function

    ''' <summary>
    ''' 从JsValue转换一项字段配置
    ''' </summary>
    ''' <param name="methodListenerNames">方法监听器名称</param>
    ''' <param name="jsValue">JsValue</param>
    ''' <returns></returns>
    Private Shared Function MakeFieldConfigurationFromJsValue(jsValue As JsValue, methodListenerNames As String(), refFieldConfiguration As Field) As Field
        If jsValue Is Nothing Then Throw New FrontWorkException("JsValue can not be null!")
        If Not jsValue.IsObject Then
            Throw New FrontWorkException("Invalid JsObject!")
            Return Nothing
        End If
        Dim jsObject = jsValue.AsObject

        '新建FieldConfiguration
        Dim newFieldConfiguration As Field
        If refFieldConfiguration IsNot Nothing Then
            newFieldConfiguration = refFieldConfiguration.Clone
        Else
            newFieldConfiguration = New Field
        End If
        Dim jsEngine = ModeConfiguration.JsEngine

        Dim myType = GetType(Field)
        '遍历字典，赋值给FieldConfiguration
        For Each item In jsObject.GetOwnProperties
            Dim key = item.Key
            Dim value = item.Value.Value.ToObject
            '获取js字段对应的FieldConfiguration属性，找不到就跳过并报错
            Dim prop = myType.GetProperty(key, BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.IgnoreCase)
            If prop Is Nothing Then
                Throw New FrontWorkException("can not resolve property:""" + key + """ in json configure")
                Continue For
            ElseIf prop.PropertyType = GetType(FieldProperty) OrElse
                prop.PropertyType.IsSubclassOf(GetType(FieldProperty)) Then '如果是FieldProperty，调用构造函数
                Dim jsProp = item.Value.Value
                Dim constructor = prop.PropertyType.GetConstructor({GetType(JsValue), GetType(String())})
                Dim newFieldProperty = constructor.Invoke({jsProp, methodListenerNames})
                prop.SetValue(newFieldConfiguration, newFieldProperty, Nothing)
            Else
                Throw New FrontWorkException($"Field property ""{prop.Name}"" must be FieldProperty type!")
            End If
        Next
        Return newFieldConfiguration
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return Util.DeepClone(Me)
    End Function

    Public Sub SetMethodListener(methodListenerNames As String())
        Dim myType = Me.GetType
        Dim myProps = myType.GetProperties
        For Each myProp In myProps
            If myProp.PropertyType = GetType(FieldProperty) orelse
                myProp.PropertyType.IsSubclassOf(gettype(FieldProperty)) Then
                Dim value = CType(myProp.GetValue(Me, Nothing), FieldProperty)
                value?.SetMethodListenerNames(methodListenerNames)
            End If
        Next
    End Sub

    Public Overrides Function ToString() As String
        Return Me.DisplayName.GetValue
    End Function
End Class

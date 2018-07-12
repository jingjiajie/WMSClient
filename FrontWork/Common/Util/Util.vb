Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text
Imports System.Web.Script.Serialization

''' <summary>
''' 公用工具类
''' </summary>
Public Class Util
    Private Shared jsEngine As New Jint.Engine

    ''' <summary>
    ''' 将Object对象转换成数组
    ''' </summary>
    ''' <typeparam name="TElem">数组类型</typeparam>
    ''' <param name="obj">源对象</param>
    ''' <returns>转换结果数组</returns>
    Public Shared Function ToArray(Of TElem)(obj As Object) As TElem()
        If obj Is Nothing Then
            Return New TElem() {}
        End If
        Dim objType = obj.GetType

        If Not objType.IsArray Then '源类型不是数组,则把obj作为新数组的唯一一项
            Return New TElem() {Util.ChangeType(obj, GetType(TElem))}
        Else '源类型是数组，则直接遍历转型
            Return Util.ChangeType(obj, GetType(TElem).MakeArrayType)
        End If
    End Function

    ''' <summary>
    ''' 生成从Start到End（不包含End）的连续整数
    ''' </summary>
    ''' <param name="start">开始数字</param>
    ''' <param name="[end]">结束数字（不包含）</param>
    ''' <returns></returns>
    Public Shared Function Range(start As Integer, [end] As Integer) As Integer()
        Dim length = [end] - start
        Dim result(length - 1) As Integer
        For i = 0 To length - 1
            result(i) = start + i
        Next
        Return result
    End Function

    ''' <summary>
    ''' 将一个值重复若干次
    ''' </summary>
    ''' <typeparam name="T">值的类型</typeparam>
    ''' <param name="data">要重复值</param>
    ''' <param name="repeatTimes">重复次数</param>
    ''' <returns>重复若干次的结果，数组</returns>
    Public Shared Function Times(Of T)(data As T, repeatTimes As Integer) As T()
        Dim result(repeatTimes - 1) As T
        For i = 0 To repeatTimes - 1
            result(i) = data
        Next
        Return result
    End Function

    Public Shared Function DeepClone(Of T As New)(src As T) As T
        Dim newObj As New T
        Dim srcType = src.GetType
        Dim fields = srcType.GetFields(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
        For Each field In fields
            Dim srcValue = field.GetValue(src)
            If field.FieldType = GetType(String) OrElse field.FieldType.IsValueType Then
                field.SetValue(newObj, srcValue)
            ElseIf field.FieldType.GetInterface("ICloneable") Is Nothing Then
                Throw New FrontWorkException($"Field {field.Name} must implement ICloneable")
            ElseIf srcValue Is Nothing Then
                field.SetValue(newObj, srcValue)
            Else
                field.SetValue(newObj, CType(srcValue, ICloneable).Clone)
            End If
        Next
        Return newObj
    End Function

    Public Shared Function DeepClone(Of T As New)(srcArray As T()) As T()
        Dim newArray(srcArray.Length - 1) As T
        For i = 0 To srcArray.Length - 1
            newArray(i) = DeepClone(srcArray(i))
        Next
        Return newArray
    End Function

    Public Shared Function FindFirstVisibleParent(c As Control) As Control
        If c.Parent IsNot Nothing Then
            If c.Parent.Visible Then
                Return c.Parent
            Else
                Return FindFirstVisibleParent(c)
            End If
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function UrlEncode(str As String) As String
        'str = str.Replace("/", "%2f")
        str = Web.HttpUtility.UrlEncode(str)
        Return str
    End Function

    Public Shared Sub AdjustInsertIndexes(Of T)(objs As T(), funcGetRow As Func(Of T, Integer), funcSetRow As Action(Of T, Integer), originalRowCount As Integer)
        '原始行每次插入之后，行号会变，所以做调整
        objs = (From obj In objs Order By funcGetRow(obj) Ascending Select obj).ToArray '行号调整后的对象数组
        Dim insertedCount = 0
        For i = 0 To objs.Length - 1
            Dim oriRow = funcGetRow(objs(i))
            funcSetRow(objs(i), funcGetRow(objs(i)) + insertedCount)
            If oriRow < originalRowCount Then
                insertedCount += 1
            End If
        Next
    End Sub

    Public Shared Function AdjustInsertIndexes(srcIndexes As Integer(), originalRowCount As Integer)
        '原始行每次插入之后，行号会变，所以做调整
        Dim indexesASC = (From i In srcIndexes Order By i Ascending Select i).ToArray '行号调整后
        Dim adjustedIndexes(indexesASC.Length - 1) As Integer
        Dim insertedCount = 0
        For i = 0 To indexesASC.Length - 1
            adjustedIndexes(i) = indexesASC(i) + insertedCount
            If indexesASC(i) < originalRowCount Then
                insertedCount += 1
            End If
        Next
        Return adjustedIndexes
    End Function

    Public Shared Function EqualOrBothNothing(obj1 As Object, obj2 As Object) As Boolean
        If obj1 Is Nothing AndAlso obj2 Is Nothing Then Return True
        If obj1 Is Nothing OrElse obj2 Is Nothing Then Return False
        Return obj1.Equals(obj2)
    End Function

    Public Shared Function ChangeType(srcValue As Object, targetType As Type) As Object
        If srcValue Is Nothing Then Return Nothing
        Dim srcType = srcValue.GetType
        '如果源值和目标值都是数组，则分别对每一项进行转换
        If srcType.IsArray AndAlso targetType.IsArray Then
            Dim srcArray = CType(srcValue, Object())
            Dim targetArray = Array.CreateInstance(targetType.GetElementType, srcArray.Length)
            For i = 0 To srcArray.Length - 1
                targetArray(i) = ChangeType(srcArray(i), targetType.GetElementType)
            Next
            Return targetArray
        End If
        '如果是父子类关系，则直接返回
        If srcType = targetType OrElse
            srcType.IsSubclassOf(targetType) OrElse
            srcType.GetInterface(targetType.Name) IsNot Nothing Then
            Return srcValue
        End If
        '否则如果目标类有对应的转换构造函数，则调用相应的转换构造函数
        Dim constructor = targetType.GetConstructor({srcType})
        If constructor IsNot Nothing Then
            Dim value = constructor.Invoke({srcValue})
            Return value
        End If
        '最后尝试标准库ChangeType，再不行就报错
        Try
            Return Convert.ChangeType(srcValue, targetType)
        Catch ex As Exception
            Throw New IncompatibleTypeException(srcValue, targetType)
        End Try
    End Function
End Class


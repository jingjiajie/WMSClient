Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text
Imports System.Web.Script.Serialization

''' <summary>
''' 公用工具类
''' </summary>
Friend Class Util
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
            Return New TElem() {System.Convert.ChangeType(obj, GetType(TElem))}
        Else '源类型是数组，则直接遍历转型
            Dim objPropertyLength = objType.GetProperty("Length")
            Dim objMethodGetValue = objType.GetMethod("GetValue", New Type() {GetType(Integer)})
            Dim objLength = CType(objPropertyLength.GetValue(obj, Nothing), Integer)
            Dim result(objLength - 1) As TElem
            For i As Integer = 0 To objLength - 1
                Dim srcValue = objMethodGetValue.Invoke(obj, New Object() {i})
                Dim constructor = GetType(TElem).GetConstructor({srcValue.GetType})
                If constructor IsNot Nothing Then
                    result(i) = constructor.Invoke({srcValue})
                Else
                    result(i) = System.Convert.ChangeType(srcValue, GetType(TElem))
                End If
            Next
            Return result
        End If
    End Function

    ''' <summary>
    ''' 生成从Start到End（不包含End）的连续整数
    ''' </summary>
    ''' <param name="start">开始数字</param>
    ''' <param name="[end]">结束数字（不包含）</param>
    ''' <returns></returns>
    Public Shared Function Range(start As Long, [end] As Long) As Long()
        Dim length = [end] - start
        Dim result(length - 1) As Long
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
    Public Shared Function Times(Of T)(data As T, repeatTimes As Long) As T()
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
            ElseIf field.FieldType.GetInterface("ICloneable") = Nothing Then
                Throw New Exception($"Field {field.Name} must implement ICloneable")
            ElseIf srcValue Is Nothing Then
                field.SetValue(newObj, srcValue)
            Else
                field.SetValue(newObj, CType(srcValue, ICloneable).Clone)
            End If
        Next
        Return newObj
    End Function
End Class

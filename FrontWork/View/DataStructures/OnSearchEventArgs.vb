''' <summary>
''' 搜索事件参数
''' </summary>
Public Class OnSearchEventArgs
    Inherits EventArgs

    ''' <summary>
    ''' 搜索条件
    ''' </summary>
    ''' <returns></returns>
    Public Property Conditions As SearchConditionItem() = New SearchConditionItem() {}

    ''' <summary>
    ''' 排序条件
    ''' </summary>
    ''' <returns></returns>
    Public Property Orders As OrderConditionItem() = New OrderConditionItem() {}

    Public Sub New(conditions As SearchConditionItem(), orders As OrderConditionItem())
        Me.Conditions = conditions
        Me.Orders = orders
    End Sub

    Public Sub New()

    End Sub
End Class



''' <summary>
''' 搜索条件项
''' </summary>
Public Class SearchConditionItem
    ''' <summary>
    ''' 字段名
    ''' </summary>
    ''' <returns></returns>
    Public Property Key As String

    ''' <summary>
    ''' 关系，等于，或者大于，小于，在中间等等
    ''' </summary>
    ''' <returns></returns>
    Public Property Relation As Relation = Relation.EQUAL

    ''' <summary>
    ''' 比较值，如果是一对一关系，则只有第一个值生效。如果是一对多关系，则相应数量的值生效。
    ''' </summary>
    ''' <returns></returns>
    Public Property Values As Object()

    Public Sub New(key As String, relation As Relation, values As Object())
        Me.Key = key
        Me.Relation = relation
        Me.Values = values
    End Sub
End Class


''' <summary>
''' 排序条件项
''' </summary>
Public Class OrderConditionItem
    ''' <summary>
    ''' 字段名称
    ''' </summary>
    ''' <returns></returns>
    Public Property Key As String

    ''' <summary>
    ''' 排序顺序，正序或者倒序
    ''' </summary>
    ''' <returns></returns>
    Public Property Order As Order = [Order].ASC

    Public Sub New(key As String, order As Order)
        Me.Key = key
        Me.Order = order
    End Sub
End Class

''' <summary>
''' 排序顺序
''' </summary>
Public Enum Order
    ''' <summary>
    ''' 升序排列
    ''' </summary>
    ASC

    ''' <summary>
    ''' 降序排列
    ''' </summary>
    DESC
End Enum


''' <summary>
''' 搜索关系
''' </summary>
Public Enum Relation
    ''' <summary>
    ''' 等于
    ''' </summary>
    EQUAL

    ''' <summary>
    ''' 大于
    ''' </summary>
    GREATER_THAN_OR_EQUAL_TO

    ''' <summary>
    ''' 小于
    ''' </summary>
    LESS_THAN_OR_EQUAL_TO

    ''' <summary>
    ''' 包含
    ''' </summary>
    CONTAINS

    ''' <summary>
    ''' 介于
    ''' </summary>
    BETWEEN

    ''' <summary>
    ''' 包含于
    ''' </summary>
    [IN]

    ''' <summary>
    ''' 以xxx开始
    ''' </summary>
    STARTS_WITH
    ''' <summary>
    ''' 以xxx结束
    ''' </summary>
    ENDS_WITH
End Enum
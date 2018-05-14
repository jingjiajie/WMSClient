Imports System.ComponentModel
Imports System.Linq
Imports FrontWork.OnSearchEventArgs

''' <summary>
''' 搜索视图。提供基本的搜索条件，比较条件，排序条件等功能。
''' 配合各种适配器来适配不同的同步器，从而实现按搜索条件将数据从后端检索并同步到Model中
''' </summary>
Public Class SearchView
    Inherits UserControl
    Implements IView
    Private _configuration As Configuration
    Private _mode As String = "default"

    Private Property StaticConditions As New List(Of SearchConditionItem)

    ''' <summary>
    ''' 用户按下查询按键触发的事件
    ''' </summary>
    Public Event OnSearch As EventHandler(Of OnSearchEventArgs)

    <Description("当前配置模式"), Category("FrontWork")>
    Public Property Mode As String Implements IView.Mode
        Get
            Return Me._mode
        End Get
        Set(value As String)
            Me._mode = value
            Call Me.ConfigurationChanged(Me, New ConfigurationChangedEventArgs)
        End Set
    End Property

    ''' <summary>
    ''' 配置中心对象，用来获取配置
    ''' </summary>
    ''' <returns></returns>
    <Description("配置中心对象"), Category("FrontWork")>
    Public Property Configuration As Configuration
        Get
            Return Me._configuration
        End Get
        Set(value As Configuration)
            If Me._configuration IsNot Nothing Then
                RemoveHandler Me._configuration.ConfigurationChanged, AddressOf Me.ConfigurationChanged
            End If
            Me._configuration = value
            Call Me.InitEditPanel()
            If Me._configuration IsNot Nothing Then
                AddHandler Me._configuration.ConfigurationChanged, AddressOf Me.ConfigurationChanged
            End If
        End Set
    End Property

    ''' <summary>
    ''' 初始化搜索视图，允许重复调用
    ''' </summary>
    Protected Sub InitEditPanel()
        Call Me.ComboBoxSearchKey.Items.Clear()
        Call Me.ComboBoxSearchKey.Items.Add("无")
        Call Me.ComboBoxOrderKey.Items.Clear()
        Call Me.ComboBoxOrderKey.Items.Add("无")

        If Me.Configuration Is Nothing Then Return
        Dim fieldConfiguration = Me.Configuration.GetFieldConfigurations(Me.Mode)
        If fieldConfiguration Is Nothing Then Return
        Dim fieldNames = (From field In fieldConfiguration
                          Where field.Visible
                          Select field.DisplayName).ToArray
        Call Me.ComboBoxSearchKey.Items.AddRange(fieldNames)
        Call Me.ComboBoxOrderKey.Items.AddRange(fieldNames)
        If Me.ComboBoxSearchKey.Items.Count > 0 Then Me.ComboBoxSearchKey.SelectedIndex = 0
        If Me.ComboBoxOrderKey.Items.Count > 0 Then Me.ComboBoxOrderKey.SelectedIndex = 0
    End Sub

    Private Sub SearchWidget_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.ComboBoxSearchRelation.SelectedIndex = 0
        Me.ComboBoxOrder.SelectedIndex = 0
    End Sub

    Private Sub TableLayoutPanel1_Paint(sender As Object, e As PaintEventArgs) Handles TableLayoutPanel1.Paint

    End Sub

    Private Sub ButtonSearch_Click(sender As Object, e As EventArgs) Handles ButtonSearch.Click
        Dim eventArgs = Me.GetSearchEventArgs
        RaiseEvent OnSearch(Me, eventArgs)
    End Sub

    Private Sub ConfigurationChanged(sender As Object, e As ConfigurationChangedEventArgs)
        Call Me.InitEditPanel()
    End Sub

    ''' <summary>
    ''' 根据用户的搜索条件设置，生成OnSearchEventArgs
    ''' </summary>
    ''' <returns>返回生成的OnSearchEventArgs</returns>
    Protected Function GetSearchEventArgs() As OnSearchEventArgs
        If Me.Configuration Is Nothing Then
            Throw New Exception("Configuration not set in SearchWidget")
        End If
        Dim newSearchArgs = New OnSearchEventArgs

        If Me.ComboBoxSearchKey.SelectedIndex <> 0 Then
            Dim searchDisplayName = Me.ComboBoxSearchKey.SelectedItem?.ToString
            Dim relation As Relation
            Dim searchValue = Me.TextBoxSearchCondition.Text
            Dim searchName = (From m In Me.Configuration.GetFieldConfigurations(Me.Mode)
                              Where m.DisplayName = searchDisplayName
                              Select m.Name).First
            Select Case Me.ComboBoxSearchRelation.SelectedItem.ToString
                Case "包含"
                    relation = Relation.CONTAINS
                Case "等于"
                    relation = Relation.EQUAL
                Case "介于"
                    relation = Relation.BETWEEN
                Case "大于等于"
                    relation = Relation.GREATER_THAN_OR_EQUAL_TO
                Case "小于等于"
                    relation = Relation.LESS_THAN_OR_EQUAL_TO
            End Select
            newSearchArgs.Conditions = Me.StaticConditions.Union({New SearchConditionItem(searchName, relation, {searchValue})})
        End If

        If Me.ComboBoxOrderKey.SelectedIndex <> 0 Then
            Dim orderDisplayName = Me.ComboBoxOrderKey.SelectedItem?.ToString
            Dim orderName = (From m In Me.Configuration.GetFieldConfigurations(Mode)
                             Where m.DisplayName = orderDisplayName
                             Select m.Name).First
            Dim order As Order
            Select Case Me.ComboBoxOrder.SelectedIndex
                Case 0
                    order = Order.ASC
                Case 1
                    order = Order.DESC
            End Select
            newSearchArgs.Orders = {New OrderConditionItem(orderName, order)}
        End If
        Return newSearchArgs
    End Function

    Private Sub ComboBoxOrderKey_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxOrderKey.SelectedIndexChanged
        If Me.ComboBoxOrderKey.SelectedIndex = 0 Then
            Me.ComboBoxOrder.Enabled = False
        Else
            Me.ComboBoxOrder.Enabled = True
        End If
    End Sub

    Private Sub ComboBoxSearchKey_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxSearchKey.SelectedIndexChanged
        '如果选择无搜索条件，则禁用搜索值和搜索关系框
        If Me.ComboBoxSearchKey.SelectedIndex = 0 Then
            Me.ComboBoxSearchRelation.Enabled = False
            Me.TextBoxSearchCondition.Enabled = False
        Else '否则允许设置搜索值和搜索关系。并根据字段的类型提供不同的关系
            Me.ComboBoxSearchRelation.Enabled = True
            Me.TextBoxSearchCondition.Enabled = True
            Dim selectedDisplayName = Me.ComboBoxSearchKey.SelectedItem.ToString
            Dim field = (From f In Me.Configuration.GetFieldConfigurations(Me.Mode) Where f.DisplayName = selectedDisplayName Select f).First
            Call Me.RefreshSearchByType(field.Type)
        End If
    End Sub

    ''' <summary>
    ''' 根据字段的类型刷新搜索面板
    ''' </summary>
    ''' <param name="type"></param>
    Private Sub RefreshSearchByType(type As String)
        If type.Equals("string", StringComparison.OrdinalIgnoreCase) Then
            Me.ComboBoxSearchRelation.Items.Clear()
            Me.ComboBoxSearchRelation.Items.AddRange({
                "包含", "等于"
            })
            Me.ComboBoxSearchRelation.SelectedIndex = 0
        ElseIf type.Equals("int", StringComparison.OrdinalIgnoreCase) _
            OrElse type.Equals("double", StringComparison.OrdinalIgnoreCase) Then
            Me.ComboBoxSearchRelation.Items.Clear()
            Me.ComboBoxSearchRelation.Items.AddRange({
                "等于", "大于等于", "小于等于"
            })
            Me.ComboBoxSearchRelation.SelectedIndex = 0
        ElseIf type.Equals("datetime", StringComparison.OrdinalIgnoreCase) Then
            Me.ComboBoxSearchRelation.Items.Clear()
            Me.ComboBoxSearchRelation.Items.AddRange({
                "介于"
            })
            Me.ComboBoxSearchRelation.SelectedIndex = 0
        Else
            Me.ComboBoxSearchRelation.Items.Clear()
            Me.ComboBoxSearchRelation.Items.AddRange({
                "包含", "等于", "大于等于", "小于等于", "介于"
            })
            Me.ComboBoxSearchRelation.SelectedIndex = 0
        End If
    End Sub

    Public Sub Search()
        Call Me.ButtonSearch.PerformClick()
    End Sub

    ''' <summary>
    ''' 添加静态搜索条件，即每一次搜索都会自动附加的条件
    ''' </summary>
    ''' <param name="key">字段名</param>
    ''' <param name="values">值列表</param>
    ''' <param name="relation">关系</param>
    Public Sub AddStaicCondition(key As String, values As Object(), Optional relation As Relation = Relation.EQUAL)
        Me.StaticConditions.Add(New SearchConditionItem(key, relation, values))
    End Sub

    ''' <summary>
    ''' 添加静态搜索条件，即每一次搜索都会自动附加的条件
    ''' </summary>
    ''' <param name="key">字段名</param>
    ''' <param name="value">值</param>
    ''' <param name="relation">关系</param>
    Public Sub AddStaticCondition(key As String, value As Object, Optional relation As Relation = Relation.EQUAL)
        Call Me.AddStaticCondition(key, {value}, relation)
    End Sub
End Class

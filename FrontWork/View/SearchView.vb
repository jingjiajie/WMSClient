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
    Public Property Mode As String
        Get
            Return Me._mode
        End Get
        Set(value As String)
            Me._mode = value
            Call Me.ConfigurationRefreshed(Me, New ConfigurationRefreshedEventArgs)
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
                RemoveHandler Me._configuration.Refreshed, AddressOf Me.ConfigurationRefreshed
            End If
            Me._configuration = value
            Call Me.InitEditPanel()
            If Me._configuration IsNot Nothing Then
                AddHandler Me._configuration.Refreshed, AddressOf Me.ConfigurationRefreshed
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
        Dim fieldConfiguration = Me.Configuration.GetFields(Me.Mode)
        If fieldConfiguration Is Nothing Then Return
        Dim fieldNames = (From field In fieldConfiguration
                          Where field.Visible.GetValue
                          Select field.DisplayName.GetValue).ToArray
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
        Call Me.Search()
    End Sub

    Private Sub ConfigurationRefreshed(sender As Object, e As ConfigurationRefreshedEventArgs)
        Call Me.InitEditPanel()
    End Sub

    ''' <summary>
    ''' 根据用户的搜索条件设置，生成OnSearchEventArgs
    ''' </summary>
    ''' <returns>返回生成的OnSearchEventArgs</returns>
    Public Function GetSearchEventArgs() As OnSearchEventArgs
        If Me.Configuration Is Nothing Then
            Throw New FrontWorkException($"Configuration not set in {Me.Name}")
        End If
        Dim newSearchArgs = New OnSearchEventArgs

        If Me.ComboBoxSearchKey.SelectedIndex <> 0 Then
            Dim searchDisplayName = Me.ComboBoxSearchKey.SelectedItem?.ToString
            Dim fieldConfiguration = (From f In Me.Configuration.GetFields(Me.Mode)
                                      Where f.DisplayName = searchDisplayName
                                      Select f).FirstOrDefault
            Dim valueCount = 1 '最终上传的Value个数
            Dim relation As Relation '根据选择的关系确定ValueCount和Relation
            Select Case Me.ComboBoxSearchRelation.SelectedItem.ToString
                Case "包含"
                    relation = Relation.CONTAINS
                Case "等于"
                    relation = Relation.EQUAL
                Case "介于"
                    valueCount = 2
                    relation = Relation.BETWEEN
                Case "大于等于"
                    relation = Relation.GREATER_THAN_OR_EQUAL_TO
                Case "小于等于"
                    relation = Relation.LESS_THAN_OR_EQUAL_TO
            End Select
            Dim texts = {Me.TextBoxSearchCondition.Text, Me.TextBoxSearchCondition1.Text} '输入字符串
            Dim mappedValues(valueCount - 1) As Object '映射后的值
            Dim searchValues(valueCount - 1) As Object '转型后最终用于搜索的值
            If fieldConfiguration.BackwardMapper IsNot Nothing Then
                For i = 0 To valueCount - 1
                    Dim context As New ViewEditInvocationContext(Me, -1, fieldConfiguration.Name, texts(i))
                    mappedValues(i) = fieldConfiguration.BackwardMapper.Invoke(context)
                Next
            Else
                For i = 0 To valueCount - 1
                    mappedValues(i) = texts(i)
                Next
            End If
            For i = 0 To valueCount - 1
                Try
                    searchValues(i) = Convert.ChangeType(mappedValues(i), fieldConfiguration.Type.GetValue)
                Catch
                    MessageBox.Show($"""{texts(i)}""不是合法的数据，请检查输入！")
                    Return Nothing
                End Try
            Next
            Dim searchName = (From m In Me.Configuration.GetFields(Me.Mode)
                              Where m.DisplayName = searchDisplayName
                              Select m.Name).First
            newSearchArgs.Conditions = Me.StaticConditions.Union({New SearchConditionItem(searchName, relation, searchValues)}).ToArray
        Else
            newSearchArgs.Conditions = Me.StaticConditions.ToArray
        End If

        If Me.ComboBoxOrderKey.SelectedIndex <> 0 Then
            Dim orderDisplayName = Me.ComboBoxOrderKey.SelectedItem?.ToString
            Dim orderName = (From m In Me.Configuration.GetFields(Mode)
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
            Me.TextBoxSearchCondition1.Enabled = False
        Else '否则允许设置搜索值和搜索关系。并根据字段的类型提供不同的关系
            Me.ComboBoxSearchRelation.Enabled = True
            Me.TextBoxSearchCondition.Enabled = True
            Me.TextBoxSearchCondition1.Enabled = True
            Dim selectedDisplayName = Me.ComboBoxSearchKey.SelectedItem.ToString
            Dim field = (From f In Me.Configuration.GetFields(Me.Mode) Where f.DisplayName.GetValue = selectedDisplayName Select f).First
            Call Me.RefreshSearchByType(field.Type.GetValue)
        End If
    End Sub

    ''' <summary>
    ''' 根据字段的类型刷新搜索面板
    ''' </summary>
    ''' <param name="type"></param>
    Private Sub RefreshSearchByType(type As Type)
        Select Case type
            Case GetType(String)
                Me.ComboBoxSearchRelation.Items.Clear()
                Me.ComboBoxSearchRelation.Items.AddRange({
                    "包含", "等于"
                })
                Me.ComboBoxSearchRelation.SelectedIndex = 0
            Case GetType(Integer), GetType(Double)
                Me.ComboBoxSearchRelation.Items.Clear()
                Me.ComboBoxSearchRelation.Items.AddRange({
                    "等于", "大于等于", "小于等于"
                })
                Me.ComboBoxSearchRelation.SelectedIndex = 0
            Case GetType(DateTime)
                Me.ComboBoxSearchRelation.Items.Clear()
                Me.ComboBoxSearchRelation.Items.AddRange({
                    "介于"
                })
                Me.ComboBoxSearchRelation.SelectedIndex = 0
            Case GetType(Boolean)
                Me.ComboBoxSearchRelation.Items.Clear()
                Me.ComboBoxSearchRelation.Items.AddRange({
                    "等于"
                })
                Me.ComboBoxSearchRelation.SelectedIndex = 0
            Case Else
                Me.ComboBoxSearchRelation.Items.Clear()
                Me.ComboBoxSearchRelation.Items.AddRange({
                    "包含", "等于", "大于等于", "小于等于", "介于"
                })
                Me.ComboBoxSearchRelation.SelectedIndex = 0
        End Select
    End Sub

    Public Sub Search()
        Dim eventArgs = Me.GetSearchEventArgs
        If eventArgs Is Nothing Then Return
        RaiseEvent OnSearch(Me, eventArgs)
    End Sub

    ''' <summary>
    ''' 添加静态搜索条件，即每一次搜索都会自动附加的条件
    ''' </summary>
    ''' <param name="key">字段名</param>
    ''' <param name="values">值列表</param>
    ''' <param name="relation">关系</param>
    Public Sub AddStaticCondition(key As String, values As Object(), Optional relation As Relation = Relation.EQUAL)
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

    ''' <summary>
    ''' 清除静态搜索条件，即每一次搜索都会自动附加的条件
    ''' </summary>
    Public Sub ClearStaticCondition()
        Call Me.StaticConditions.Clear()
    End Sub

    ''' <summary>
    ''' 清除静态搜索条件，即每一次搜索都会自动附加的条件
    ''' </summary>
    ''' <param name="key">要删除的key</param>
    Public Sub ClearStaticCondition(key As String)
        Call Me.StaticConditions.RemoveAll(
            Function(cond)
                Return cond.Key.Equals(key, StringComparison.OrdinalIgnoreCase)
            End Function)
    End Sub

    Private Sub SearchView_EnabledChanged(sender As Object, e As EventArgs) Handles MyBase.EnabledChanged

    End Sub

    Private Sub ComboBoxSearchRelation_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxSearchRelation.SelectedIndexChanged
        Dim relationStr = ComboBoxSearchRelation.SelectedItem.ToString
        Call Me.TableLayoutPanel1.SuspendLayout()
        If relationStr = "介于" Then
            Me.TableLayoutPanel1.SetColumnSpan(Me.TextBoxSearchCondition, 1)
            Me.TableLayoutPanel1.SetCellPosition(Me.TextBoxSearchCondition1, New TableLayoutPanelCellPosition(5, 2))
            Me.TextBoxSearchCondition1.Visible = True
        Else
            Me.TextBoxSearchCondition1.Visible = False
            Me.TableLayoutPanel1.SetCellPosition(Me.TextBoxSearchCondition1, New TableLayoutPanelCellPosition(5, 1))
            Me.TableLayoutPanel1.SetColumnSpan(Me.TextBoxSearchCondition, 2)
        End If
        Call Me.TableLayoutPanel1.ResumeLayout()
    End Sub

    Public Sub AddCondition(key As String, values As Object(), Optional relation As Relation = Relation.EQUAL)
        If values.Length = 0 Then
            Throw New FrontWorkException($"{Me.Name}: Values of condition cannot be empty!")
        End If
        Dim displayName As FieldProperty = Nothing
        Dim strRelation = Me.RelationToString(relation)
        Dim fields = Me.Configuration.GetFields(Me.Mode)

        '如果能找到Name对应的字段，则直接取出
        For Each field In fields
            If field.Name.GetValue?.ToString.Equals(key, StringComparison.OrdinalIgnoreCase) Then
                displayName = field.DisplayName
            End If
        Next
        '若找不到，则按DisplayName查找，再找不到就抛错
        If displayName Is Nothing Then
            For Each field In fields
                If field.DisplayName.GetValue?.ToString.Equals(key, StringComparison.OrdinalIgnoreCase) Then
                    displayName = field.DisplayName
                End If
            Next
        End If
        If displayName Is Nothing Then
            Throw New FrontWorkException($"{Me.Name}: field ""{key}"" not found in configuration!")
        End If
        Me.ComboBoxSearchKey.SelectedItem = displayName.GetValue()
        Me.TextBoxSearchCondition.Text = values(0)
        If values.Length > 1 Then
            Me.TextBoxSearchCondition1.Text = values(1)
        End If
        Me.ComboBoxSearchRelation.SelectedItem = strRelation
    End Sub

    Public Sub AddCondition(key As String, value As Object, Optional relation As Relation = Relation.EQUAL)
        Call Me.AddCondition(key, {value}, relation)
    End Sub

    Private Function RelationToString(relation As Relation) As String
        Select Case relation
            Case Relation.GREATER_THAN_OR_EQUAL_TO
                Return "大于等于"
            Case Relation.LESS_THAN_OR_EQUAL_TO
                Return "小于等于"
            Case Relation.EQUAL
                Return "等于"
            Case Relation.CONTAINS
                Return "包含"
            Case Relation.BETWEEN
                Return "介于"
            Case Else
                Throw New FrontWorkException("Unknown Relation " + relation.ToString)
        End Select
    End Function

    Private Function StringToRelation(str As String) As Relation
        Select Case str
            Case "大于等于"
                Return Relation.GREATER_THAN_OR_EQUAL_TO
            Case "小于等于"
                Return Relation.LESS_THAN_OR_EQUAL_TO
            Case "等于"
                Return Relation.EQUAL
            Case "介于"
                Return Relation.BETWEEN
            Case "包含"
                Return Relation.CONTAINS
            Case Else
                Throw New FrontWorkException("Unknown Relation " + str)
        End Select
    End Function
End Class

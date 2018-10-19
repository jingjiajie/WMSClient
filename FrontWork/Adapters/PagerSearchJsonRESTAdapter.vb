Imports System.ComponentModel
Imports System.Globalization
Imports System.IO
Imports System.Net
Imports FrontWork

Public Class PagerSearchJsonRESTAdapter
    Inherits SearchViewJsonRESTAdapter

    Private _pagerView As PagerView

    <Description("PagerView搜索视图对象"), Category("FrontWork")>
    Public Property PagerView As PagerView
        Get
            Return Me._pagerView
        End Get
        Set(value As PagerView)
            If Me._pagerView IsNot Nothing Then
                RemoveHandler Me._pagerView.OnCurrentPageChanged, AddressOf Me.PagerViewPageChanged
            End If
            Me._pagerView = value
            If Me._pagerView IsNot Nothing Then
                AddHandler Me._pagerView.OnCurrentPageChanged, AddressOf Me.PagerViewPageChanged
            End If
        End Set
    End Property

    Protected Sub PagerViewPageChanged(sender As Object, e As PageChangedEventArgs)
        Call Me.Search(Me.SearchView.GetSearchEventArgs, False)
    End Sub

    Protected Overrides Sub SearchViewOnSearch(sender As Object, args As OnSearchEventArgs)
        If Me.Synchronizer.FindAPI Is Nothing Then
            Throw New FrontWorkException("FindAPI not set!")
        End If
        Call Me.Search(args, True)
    End Sub

    Public Overloads Function Search(searchArgs As OnSearchEventArgs, resetPage As Boolean) As Boolean
        If Me.Synchronizer.GetCountAPI Is Nothing Then
            Throw New FrontWorkException("get-count API not set!")
        End If
        If resetPage Then
            RemoveHandler Me.PagerView.OnCurrentPageChanged, AddressOf Me.PagerViewPageChanged
            Me.PagerView.CurrentPage = 1
            AddHandler Me.PagerView.OnCurrentPageChanged, AddressOf Me.PagerViewPageChanged
        End If
        '=====开始搜索内容
        If Me.PagerView Is Nothing Then
            Logger.PutMessage("PagerSearchJsonRESTAdapter: PagerView not set!", LogLevel.WARNING)
        Else
            Me.Synchronizer.FindAPI.SetRequestParameter("$page", Me.PagerView.CurrentPage)
            Me.Synchronizer.FindAPI.SetRequestParameter("$pageSize", Me.PagerView.PageSize)
        End If

        '=====刷新分页总数
        '获取搜索结果
        If Not MyBase.Search(searchArgs) Then Return False

        Call Me.SetConditionAndOrdersToAPI(Me.Synchronizer.GetCountAPI, searchArgs)
        '获取搜索结果总数量
        Dim responseStr As String = Nothing
        Dim response = Me.Synchronizer.GetCountAPI.Invoke()
        If response.StatusCode = 200 Then
            responseStr = response.BodyString
        Else
            Dim message = response.ErrorMessage
            MessageBox.Show("查询结果总数失败：" & message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Me.Synchronizer.GetCountAPI.SetResponseParameter("$count")
        Dim countStr = Me.Synchronizer.GetCountAPI.GetResponseParameters(responseStr, {"$count"})(0)
        Dim count = -1
        If Not Integer.TryParse(countStr, count) Then
            MessageBox.Show($"查询结果总数失败{countStr}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        If count = 0 Then
            Me.PagerView.TotalPage = 1
        Else
            Me.PagerView.TotalPage = Math.Floor(count / Me.PagerView.PageSize) + If(count Mod Me.PagerView.PageSize = 0, 0, 1)
        End If
        Return True
    End Function

    Public Sub New()
        Me.LabelAdapterName.Font = New Font("Microsoft Yahei UI", 8)
        Me.LabelAdapterName.Text = "Pager&&Search"
    End Sub

End Class

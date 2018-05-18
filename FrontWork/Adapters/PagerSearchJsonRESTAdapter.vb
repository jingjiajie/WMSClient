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
        Call Me.SearchView.ButtonSearch.PerformClick()
    End Sub

    Protected Overrides Function SearchViewOnSearch(sender As Object, args As OnSearchEventArgs) As Boolean
        Logger.SetMode(LogMode.DEFAULT_MODE)
        If Me.Synchronizer.GetCountAPI Is Nothing Then
            throw new FrontWorkException("get-count API not set!")
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
        If Not MyBase.SearchViewOnSearch(sender, args) Then Return False

        Call Me.SetConditionAndOrdersToAPI(Me.Synchronizer.GetCountAPI, args)
        '获取搜索结果总数量
        Dim responseStr As String = Nothing
        Try
            Dim response = Me.Synchronizer.GetCountAPI.Invoke()
            responseStr = New StreamReader(response.GetResponseStream).ReadToEnd
        Catch ex As WebException
            Dim message = ex.Message
            If ex.Response IsNot Nothing Then
                message = New StreamReader(ex.Response.GetResponseStream).ReadToEnd
            End If
            MessageBox.Show("查询结果总数失败：" & message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End Try

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
            Me.PagerView.TotalPage = count / Me.PagerView.PageSize + If(count Mod Me.PagerView.PageSize = 0, 0, 1)
        End If
        Return True
    End Function

    Public Sub New()
        Me.LabelAdapterName.Font = New Font("Microsoft Yahei UI", 8)
        Me.LabelAdapterName.Text = "Pager&&Search"
    End Sub

End Class

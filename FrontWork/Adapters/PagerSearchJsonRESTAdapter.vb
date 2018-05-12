Imports System.ComponentModel
Imports System.Globalization
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

    Protected Overrides Sub SearchViewOnSearch(sender As Object, args As OnSearchEventArgs)
        Logger.SetMode(LogMode.DEFAULT_MODE)
        If Me.PagerView Is Nothing Then
            Logger.PutMessage("PagerSearchJsonRESTAdapter: PagerView not set!", LogLevel.WARNING)
        Else
            Me.Synchronizer.PullAPI.SetRequestParameter("$page", Me.PagerView.CurrentPage)
            Me.Synchronizer.PullAPI.SetRequestParameter("$pageSize", Me.PagerView.PageSize)
        End If

        MyBase.SearchViewOnSearch(sender, args)
    End Sub

    Public Sub New()
        Me.LabelAdapterName.Font = New Font("Microsoft Yahei UI", 8)
        Me.LabelAdapterName.Text = "Pager&&Search"
    End Sub

End Class

Imports System.ComponentModel
Imports FrontWork

Public Class PagerView
    Implements IView
    Private _currentPage As Long = 1
    Private _totalPage As Long = 1
    Private _mode As String = "default"

    ''' <summary>
    ''' 总页码，从1开始
    ''' </summary>
    ''' <returns>总页码</returns>
    <Description("总页码（从1开始）"), Category("FrontWork"), Browsable(False), DesignerSerializationVisibility(False)>
    Public Property TotalPage As Long
        Get
            Return Me._totalPage
        End Get
        Set(value As Long)
            If value < 1 Then Throw New Exception("Page must be greater than 1")
            If value < Me.CurrentPage Then
                Throw New Exception($"TotalPage:{value} cannot be less than CurrentPage:{Me.CurrentPage}")
            End If
            Me._totalPage = value
            Me.LabelTotalPage.Text = CStr(value)
        End Set
    End Property

    ''' <summary>
    ''' 当前页码，从1开始
    ''' </summary>
    ''' <returns>当前页码</returns>
    <Description("当前页（从1开始）"), Category("FrontWork"), Browsable(False), DesignerSerializationVisibility(False)>
    Public Property CurrentPage As Long
        Get
            Return Me._currentPage
        End Get
        Set(value As Long)
            If value < 1 Then Throw New Exception("Page must be greater than 1")
            If value > Me.TotalPage Then
                Throw New Exception($"CurrentPage:{value} exceeded TotalPage:{Me.TotalPage}")
            End If
            Me._currentPage = value
            Me.TextBoxCurrentPage.Text = CStr(value)
            Dim eventArgs As New PageChangedEventArgs(value)
            RaiseEvent OnCurrentPageChanged(Me, eventArgs)
        End Set
    End Property

    ''' <summary>
    ''' 每页大小，默认50行
    ''' </summary>
    ''' <returns></returns>
    <Description("每页大小"), Category("FrontWork")>
    Public Property PageSize As Long = 50

    ''' <summary>
    ''' 当前模式
    ''' </summary>
    ''' <returns></returns>
    <Description("当前配置模式"), Category("FrontWork")>
    Public Property Mode As String Implements IView.Mode
        Get
            Return Me._mode
        End Get
        Set(value As String)
            Me._mode = value
        End Set
    End Property

    ''' <summary>
    ''' 当前页码改变事件（可能由于用户点击下一页，或者程序改变CurrentPage触发）
    ''' </summary>
    Public Event OnCurrentPageChanged As EventHandler(Of PageChangedEventArgs)

    Private Sub TableLayoutPanel1_Paint(sender As Object, e As PaintEventArgs) Handles TableLayoutPanel1.Paint

    End Sub

    Private Sub TextBoxCurrentPage_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxCurrentPage.KeyPress
        If e.KeyChar = vbBack Then Return
        If Not Char.IsNumber(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub ButtonGo_Click(sender As Object, e As EventArgs) Handles ButtonGo.Click
        If Not (Me.CurrentPage >= 1 And Me.CurrentPage <= Me.TotalPage) Then
            MessageBox.Show($"请输入{1}到{Me.TotalPage}之间的页码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
        RaiseEvent OnCurrentPageChanged(Me, New PageChangedEventArgs(Me.CurrentPage))
    End Sub

    Private Sub TextBoxCurrentPage_TextChanged(sender As Object, e As EventArgs) Handles TextBoxCurrentPage.TextChanged
        If String.IsNullOrEmpty(Me.TextBoxCurrentPage.Text) Then
            Me._currentPage = -1
            Return
        End If
        Me._currentPage = CInt(Me.TextBoxCurrentPage.Text)
    End Sub

    Private Sub ButtonNextPage_Click(sender As Object, e As EventArgs) Handles ButtonNextPage.Click
        If Me.CurrentPage = -1 Then
            Me.CurrentPage = 1
            Return
        End If
        If Me.CurrentPage >= Me.TotalPage Then Return
        Me.CurrentPage += 1
    End Sub

    Private Sub ButtonEndPage_Click(sender As Object, e As EventArgs) Handles ButtonEndPage.Click
        If Me.CurrentPage = Me.TotalPage Then Return
        Me.CurrentPage = Me.TotalPage
    End Sub

    Private Sub ButtonPreviousPage_Click(sender As Object, e As EventArgs) Handles ButtonPreviousPage.Click
        If Me.CurrentPage = -1 Then
            Me.CurrentPage = 1
            Return
        End If
        If Me.CurrentPage <= 1 Then Return
        Me.CurrentPage -= 1
    End Sub

    Private Sub ButtonStartPage_Click(sender As Object, e As EventArgs) Handles ButtonStartPage.Click
        If Me.CurrentPage = 1 Then Return
        Me.CurrentPage = 1
    End Sub
End Class

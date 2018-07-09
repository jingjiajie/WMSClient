
Public Class ManualBrowserForm
    Public Sub New(url As String, title As String)

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Me.Text = title
        Me.WebBrowser1.Navigate(url)
    End Sub

    Private Sub ManualBrowserForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
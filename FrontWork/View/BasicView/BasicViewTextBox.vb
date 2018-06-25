Imports System.Reflection

Friend Class BasicViewTextBox
    Inherits TextBox

    Private Property LabelLayer As Label

    Public Property PlaceHolder As String
        Get
            Return Me.LabelLayer.Text
        End Get
        Set(value As String)
            Me.LabelLayer.Text = value
            If String.IsNullOrWhiteSpace(value) Then Call Me.LabelLayer.Hide()
        End Set
    End Property

    Public Sub New()
        Call Me.InitLabelPlaceholder()
    End Sub

    Private Sub InitLabelPlaceholder()
        '如果设置了占位符，则想办法给它模拟出一个占位符来。windows居然不支持，呵呵
        '加一个label覆盖在上面，看着跟真的placeholder似的
        Me.LabelLayer = New Label
        Me.Controls.Add(LabelLayer)
        LabelLayer.TextAlign = ContentAlignment.MiddleLeft
        LabelLayer.ForeColor = Color.Gray
        LabelLayer.Font = Me.Font
        LabelLayer.Dock = DockStyle.Fill
        LabelLayer.AutoSize = True
        AddHandler LabelLayer.Click, AddressOf Me.LabelLayerClick
    End Sub

    Private Sub LabelLayerClick()
        Call Me.Focus()
        '调用编辑框的点击事件
        Dim onClickMethod = GetType(TextBox).GetMethod("OnClick", BindingFlags.NonPublic Or BindingFlags.Instance)
        onClickMethod.Invoke(Me, {EventArgs.Empty})
    End Sub

    '防止第一次显示的时候有字也显示占位符的囧境
    Private Sub TextBox_TextChanged(sender As Object, e As EventArgs) Handles Me.TextChanged
        If Me.Text.Length <> 0 Then
            Call LabelLayer.Hide()
        Else
            If String.IsNullOrWhiteSpace(Me.PlaceHolder) Then Return
            Me.Text = ""
            Call LabelLayer.Show()
        End If
    End Sub

    Private Sub TextBox_Enter(sender As Object, e As EventArgs) Handles Me.Enter
        Call LabelLayer.Hide()
    End Sub

    Private Sub TextBox_Leave(sender As Object, e As EventArgs) Handles Me.Leave
        If String.IsNullOrWhiteSpace(Me.PlaceHolder) Then Return
        If String.IsNullOrWhiteSpace(Me.Text) Then
            Me.Text = ""
            Call LabelLayer.Show()
        End If
    End Sub

    Private Sub TextBox_FontChanged() Handles Me.FontChanged
        Me.LabelLayer.Font = Me.Font
    End Sub
End Class

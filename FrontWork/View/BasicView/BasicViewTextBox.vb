Imports System.Reflection

Friend Class BasicViewTextBox
    Inherits TextBox

    Private Property ToolTip As ToolTip
    Private Property LabelLayer As Label

    Private _Message As String = Nothing

    Public Property PlaceHolder As String
        Get
            Return Me.LabelLayer.Text
        End Get
        Set(value As String)
            Me.LabelLayer.Text = value
            If String.IsNullOrWhiteSpace(value) Then Call Me.LabelLayer.Hide()
        End Set
    End Property

    Public Property HintMessage As String
        Get
            Return Me._Message
        End Get
        Set(value As String)
            Me._Message = value
            If Not String.IsNullOrWhiteSpace(value) Then
                If Me.ToolTip Is Nothing Then Call Me.InitToolTip()
                Call Me.ToolTip.SetToolTip(Me, value)
                Me.ToolTip.Active = True
            End If
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

    Private Sub InitToolTip()
        Me.ToolTip = New ToolTip
        Me.ToolTip.Active = False
        Me.ToolTip.InitialDelay = 300
        Me.ToolTip.AutoPopDelay = 5000
        Me.ToolTip.ReshowDelay = 0
        Me.ToolTip.ShowAlways = True
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

    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'BasicViewTextBox
        '
        Me.ResumeLayout(False)

    End Sub

    Private Sub BasicViewTextBox_MouseHover(sender As Object, e As EventArgs)

    End Sub

    Private Sub BasicViewTextBox_MouseLeave(sender As Object, e As EventArgs) Handles MyBase.MouseLeave
        If Me.ToolTip IsNot Nothing Then
            Me.ToolTip.Active = False
        End If
    End Sub

    Private Sub BasicViewTextBox_MouseEnter(sender As Object, e As EventArgs) Handles MyBase.MouseEnter
        If Not String.IsNullOrWhiteSpace(Me.HintMessage) Then
            Me.ToolTip.Active = True
        End If
    End Sub

    Private Sub BasicViewTextBox_Click(sender As Object, e As EventArgs)

    End Sub
End Class

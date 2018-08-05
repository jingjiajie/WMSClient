Imports System.Threading

''' <summary>
''' 联想窗口，为视图提供输入联想功能
''' </summary>
Partial Public Class AdsorbableAssociationForm
    Inherits Form

    ''' <summary>
    ''' 联想项移动方向，向上或者向下移动
    ''' </summary>
    Public Enum MoveDirection
        UP
        DOWN
    End Enum

    Public Event AssociationItemSelected As EventHandler(Of ViewAssociationItemSelectedEventArgs)

    Private _textBox As TextBox = Nothing
    Private _parentForm As Form = Nothing

    Public Property AdsorbTextBox As TextBox
        Get
            Return Me._textBox
        End Get
        Set(value As TextBox)
            If Me._textBox IsNot Nothing Then
                Call Me.UnBindTextBox(Me._textBox)
            End If
            Me._textBox = value
            If Me._textBox IsNot Nothing Then
                Call Me.BindTextBox(Me._textBox)
            End If
        End Set
    End Property

    Public Sub New(adsorbTextbox As TextBox)
        Call Me.New
        Me.AdsorbTextBox = adsorbTextbox
    End Sub

    Public Sub New()
        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        AddHandler Me.GotFocus, AddressOf formAssociate_GotFocus
    End Sub

    ''' <summary>
    ''' 刷新联想
    ''' </summary>
    Public Sub UpdateAssociationItems(associationItems As AssociationItem())
        Me.listBox.Items.Clear()
        For Each item In associationItems
            If item IsNot Nothing Then
                Me.listBox.Items.Add(item)
            End If
        Next
    End Sub

    Private Sub BindTextBox(textBox As TextBox)
        AddHandler textBox.PreviewKeyDown, AddressOf textBox_PreviewKeyDown
        AddHandler textBox.LocationChanged, AddressOf textBox_LocationChanged
        Me._parentForm = Me.FindTopParentForm(textBox)
        AddHandler Me._parentForm.LocationChanged, AddressOf parentForm_LocationChanged
        AddHandler Me._parentForm.Deactivate, AddressOf parentForm_Deactivate
    End Sub

    Private Sub UnBindTextBox(textBox As TextBox)
        RemoveHandler textBox.PreviewKeyDown, AddressOf textBox_PreviewKeyDown
        RemoveHandler textBox.LocationChanged, AddressOf textBox_LocationChanged
        RemoveHandler Me._parentForm.LocationChanged, AddressOf parentForm_LocationChanged
        RemoveHandler Me._parentForm.Deactivate, AddressOf parentForm_Deactivate
    End Sub

    Private Sub textBox_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs)
        If Not Me.Visible Then Return
        Select Case e.KeyCode
            Case Keys.Up
                Me.MoveSelection(MoveDirection.UP)
            Case Keys.Down
                Me.MoveSelection(MoveDirection.DOWN)
            Case Keys.Enter, Keys.Tab
                Me.SelectItem()
        End Select
    End Sub

    Private Function FindTopParentForm(ByVal c As Control) As Form
        If c.Parent Is Nothing Then
            If TypeOf (c) IsNot Form Then
                Throw New FrontWorkException("textbox must be added in a Form before binded to FormAssociation!")
            End If
            Return c
        Else
            Return FindTopParentForm(c.Parent)
        End If
    End Function

    Private Sub textBox_LocationChanged(sender As Object, e As EventArgs)
        If Me.Visible <> True Then Return
        Call Me.AdjustPosition()
    End Sub

    Private Sub parentForm_LocationChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Me.Visible <> True Then Return
        Call Me.AdjustPosition()
    End Sub

    Private Sub parentForm_Deactivate(sender, e)
        If Me._parentForm.WindowState = FormWindowState.Minimized Then
            Call MyBase.Hide()
        End If
    End Sub

    Public Shadows Sub Show()
        Call MyBase.Show()
        Call Me.AdjustPosition()
    End Sub

    Private Sub SelectItem()
        If Me.listBox.SelectedItem IsNot Nothing Then
            Dim args = New ViewAssociationItemSelectedEventArgs(CType(Me.listBox.SelectedItem, AssociationItem))
            RaiseEvent AssociationItemSelected(Me, args)
        End If
    End Sub

    Private Sub formAssociate_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.GotFocus
        Me.GiveBackFocus()
    End Sub

    ''' <summary>
    ''' 移动选择项
    ''' </summary>
    ''' <param name="direction">方向，上移或者下移</param>
    Public Sub MoveSelection(ByVal direction As MoveDirection)
        If direction = MoveDirection.UP Then
            If Me.listBox.SelectedIndex > 0 Then
                Me.listBox.SelectedIndex -= 1
            End If
        ElseIf direction = MoveDirection.DOWN Then
            If Me.listBox.SelectedIndex < Me.listBox.Items.Count - 1 Then
                Me.listBox.SelectedIndex += 1
            End If
        End If
    End Sub

    Private Sub GiveBackFocus()
        If Me._textBox IsNot Nothing Then
            Me._textBox.Focus()
            If _textBox.SelectionLength > 0 Then
                _textBox.SelectionLength = 0
                _textBox.SelectionStart = _textBox.Text.Length
            End If
        End If
    End Sub

    Private Sub SetPosition(ByVal x As Integer, ByVal y As Integer)
        Me.Location = New Point(x, y)
    End Sub

    Private Sub FormAssociate_Load(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Const WS_EX_NOACTIVATE As Integer = 134217728

    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or WS_EX_NOACTIVATE
            Return cp
        End Get
    End Property

    Private Sub listBox_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) Handles listBox.DoubleClick
        Me.SelectItem()
    End Sub

    Private Sub listBox_Click(ByVal sender As Object, ByVal e As EventArgs) Handles listBox.Click
        GiveBackFocus()
    End Sub

    Private Sub listBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles listBox.SelectedIndexChanged

    End Sub

    Private Sub AdjustPosition()
        Dim textBoxScreenPosition As Point = _textBox.PointToScreen(New Point(0, 0))
        Dim x = textBoxScreenPosition.X - Me.Padding.Left + _textBox.Padding.Left - 2
        Dim y = textBoxScreenPosition.Y + _textBox.Height - Me.Padding.Top - 3
        Me.SetPosition(x, y)
        Me.GiveBackFocus()
    End Sub
End Class
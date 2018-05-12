Imports System.Threading

''' <summary>
''' 联想窗口，为视图提供输入联想功能
''' </summary>
Partial Public Class FormAssociation
    Inherits Form

    ''' <summary>
    ''' 联想项移动方向，向上或者向下移动
    ''' </summary>
    Public Enum MoveDirection
        UP
        DOWN
    End Enum

    Private _textBox As TextBox = Nothing
    Private _parentForm As Form = Nothing

    Public Property AssociationFunc As Func(Of String, AssociationItem())

    Public Property TextBox As TextBox
        Get
            Return Me._textBox
        End Get
        Set(value As TextBox)
            If Me._textBox IsNot Nothing Then Call Me.UnBindTextBox(Me._textBox)
            Me._textBox = value
            If Me._textBox IsNot Nothing Then Call Me.BindTextBox(Me._textBox)
        End Set
    End Property

    ''' <summary>
    ''' 是否处于已选中联想项目状态。从用户回车选中，到下次联想开始之前，此项为True
    ''' </summary>
    ''' <returns>选中状态</returns>
    Public Property Selected As Boolean = False

    ''' <summary>
    ''' 是否保持可视，此项为true，则联想窗口不会隐藏
    ''' </summary>
    ''' <returns></returns>
    Public Property StayVisible As Boolean = False

    ''' <summary>
    ''' 是否保持隐藏，此项为true，则联想窗口不会显示
    ''' </summary>
    ''' <returns></returns>
    Public Property StayUnvisible As Boolean = False

    ''' <summary>
    ''' 设置联想函数，联想前会调用此函数，将返回值作为联想提示结果
    ''' </summary>
    ''' <param name="func">联想函数，参数为用户已经输入的字符串。返回所有联想结果</param>
    Public Sub SetAssociationFunc(func As Func(Of String, AssociationItem()))
        Me.AssociationFunc = func
    End Sub

    ''' <summary>
    ''' 构造函数，构造时绑定编辑框。联想窗口自动为编辑框添加相应事件，在输入时弹出联想提示
    ''' </summary>
    ''' <param name="textBox">要绑定的编辑框</param>
    Public Sub New(ByVal textBox As TextBox)
        Call Me.New
        Me.TextBox = textBox
    End Sub

    Public Sub New()
        InitializeComponent()
        AddHandler Me.GotFocus, AddressOf formAssociate_GotFocus
    End Sub

    Private Sub BindTextBox(textBox As TextBox)
        AddHandler textBox.PreviewKeyDown, AddressOf textBox_PreviewKeyDown
        AddHandler textBox.TextChanged, AddressOf textBox_TextChanged
        AddHandler textBox.Leave, AddressOf textBox_Leave
        AddHandler textBox.VisibleChanged, AddressOf textBox_VisibleChanged
        Me._parentForm = Me.FindTopParentForm(textBox)
        AddHandler Me._parentForm.LocationChanged, AddressOf textBoxBaseForm_LocationChanged
        AddHandler Me._parentForm.Deactivate, AddressOf parentForm_Deactivate
    End Sub

    Private Sub UnBindTextBox(textBox As TextBox)
        RemoveHandler textBox.PreviewKeyDown, AddressOf textBox_PreviewKeyDown
        RemoveHandler textBox.TextChanged, AddressOf textBox_TextChanged
        RemoveHandler textBox.Leave, AddressOf textBox_Leave
        RemoveHandler textBox.VisibleChanged, AddressOf textBox_VisibleChanged
        RemoveHandler Me._parentForm.LocationChanged, AddressOf textBoxBaseForm_LocationChanged
        RemoveHandler Me._parentForm.Deactivate, AddressOf parentForm_Deactivate
    End Sub

    Private Sub parentForm_Deactivate(sender, e)
        If Me._parentForm.WindowState = FormWindowState.Minimized Then
            Call MyBase.Hide()
        End If
    End Sub

    Private Sub textBoxBaseForm_LocationChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Me.Visible <> True Then Return
        Me.AdjustPosition()
    End Sub

    Private Sub textBox_VisibleChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Not Me._textBox.Visible Then
            Call Me.Hide()
        End If
    End Sub

    Private Sub textBox_Leave(ByVal sender As Object, ByVal e As EventArgs)
        If Me.Focused <> True Then
            Call Me.Hide()
        End If
    End Sub

    Private Function FindTopParentForm(ByVal c As Control) As Form
        If c.Parent Is Nothing Then
            If TypeOf (c) IsNot Form Then
                Throw New Exception("textbox must be added in a Form before binded to FormAssociation!")
            End If
            Return c
        Else
            Return FindTopParentForm(c.Parent)
        End If
    End Function

    Private Sub textBox_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Me._textBox.Focused And Me.AssociationFunc IsNot Nothing Then
            Call Me.Show()
            Call Me.RefreshAssociation()
        End If
        Me.Selected = False
    End Sub

    ''' <summary>
    ''' 刷新联想
    ''' </summary>
    Protected Sub RefreshAssociation()
        Static newestListBoxDataTime = DateTime.Now
        If Me.StayUnvisible Then Return
        If String.IsNullOrEmpty(_textBox.Text) OrElse Me.AssociationFunc Is Nothing Then
            Me.Hide()
            Return
        End If
        Call Me.listBox.Items.Clear()
        Me.listBox.Items.Add(New AssociationItem() With {.Word = "加载中..."})
        Dim threadGetItems = New Thread(
            Sub()
                Dim threadStartTime As DateTime = DateTime.Now
                newestListBoxDataTime = threadStartTime
                Try
                    Dim data = Me.AssociationFunc.Invoke(_textBox.Text)
                    If newestListBoxDataTime > threadStartTime Then '如果已经有更新的联想返回了，本次联想就废弃
                        Return
                    End If

                    Me.Invoke(New Action(Sub()
                                             Me.listBox.Items.Clear()
                                             Me.listBox.Items.AddRange(data)
                                             If data.Length = 0 Then
                                                 Me.Hide()
                                             ElseIf Me.Visible = False AndAlso _textBox.Visible = True Then
                                                 Me.Show()
                                             End If
                                         End Sub))
                Catch
                    Return
                End Try
            End Sub)
        Call threadGetItems.Start()
    End Sub

    Private Sub textBox_PreviewKeyDown(ByVal sender As Object, ByVal e As PreviewKeyDownEventArgs)
        If e.KeyCode = Keys.Up Then
            Me.MoveSelection(MoveDirection.UP)
        ElseIf e.KeyCode = Keys.Down Then
            Me.MoveSelection(MoveDirection.DOWN)
        ElseIf e.KeyCode = Keys.Enter Then
            Me.SelectItem()
        End If
    End Sub

    Private Sub SelectItem()
        If Me.Visible = False Then
            Return
        End If

        If Me.listBox.SelectedItem IsNot Nothing Then
            Me.StayVisible = True
            RemoveHandler _textBox.TextChanged, AddressOf Me.textBox_TextChanged
            _textBox.Text = (TryCast(Me.listBox.SelectedItem, AssociationItem)).Word
            AddHandler _textBox.TextChanged, AddressOf Me.textBox_TextChanged
            Me.Selected = True
            Me.StayVisible = False
            Me.Hide()
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

    Private Sub AdjustPosition()
        Dim textBoxScreenPosition As Point = _textBox.PointToScreen(New Point(0, 0))
        Dim x = textBoxScreenPosition.X - Me.Padding.Left + _textBox.Padding.Left - 2
        Dim y = textBoxScreenPosition.Y + _textBox.Height - Me.Padding.Top - 3
        Me.SetPosition(x, y)
        Me.GiveBackFocus()
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

    Public Shadows Sub Show()
        If Me.StayUnvisible Then Return
        If Me.Visible = False Then
            MyBase.Show()
        End If

        Me.AdjustPosition()
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


    Public Shadows Sub Hide()
        If Me.StayVisible Then
            Return
        Else
            MyBase.Hide()
        End If
    End Sub

    Private Sub listBox_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) Handles listBox.DoubleClick
        Me.SelectItem()
    End Sub

    Private Sub listBox_Click(ByVal sender As Object, ByVal e As EventArgs) Handles listBox.Click
        GiveBackFocus()
    End Sub

    Private Sub listBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles listBox.SelectedIndexChanged

    End Sub
End Class
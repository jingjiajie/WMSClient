Imports System.ComponentModel
Imports System.Linq
Imports FrontWork


Public Class TabView
    Implements IView
    Private _modelBox As ModelBox

    <Description("绑定的ModelBox对象"), Category("FrontWork")>
    Public Property ModelBox As ModelBox
        Get
            Return _modelBox
        End Get
        Set(value As ModelBox)
            If Me._modelBox IsNot Nothing Then
                Call Me.UnbindModelBox()
            End If
            _modelBox = value
            Call Me.TryBindModelBox()
        End Set
    End Property

    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
    End Sub

    Private Sub TabView_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        Call Me.AdjustItemSize()
    End Sub

    Private Sub AdjustItemSize()
        Dim rowCount = If(Me.TabControl.RowCount = 0, 1, Me.TabControl.RowCount)
        Dim oriItemSize = Me.TabControl.ItemSize
        Me.TabControl.ItemSize = New Size(oriItemSize.Width, Me.Height / rowCount)
    End Sub

    Private Sub TryBindModelBox()
        If Me.DesignMode Then Return

        If Me.ModelBox Is Nothing Then
            Return
        End If
        Call Me.BindModelBox()
    End Sub

    Private Sub BindModelBox()
        If Me.DesignMode Then Return
        AddHandler Me.ModelBox.ModelCollectionChanged, AddressOf Me.ModelCollectionChangedEvent
        AddHandler Me.ModelBox.SelectedModelChanged, AddressOf Me.SelectedModelChangedEvent

        Call Me.ImportModelNames()
        Call Me.RefreshSelectionRange()
    End Sub

    Private Sub UnbindModelBox()
        If Me.DesignMode Then Return
        RemoveHandler Me.ModelBox.ModelCollectionChanged, AddressOf Me.ModelCollectionChangedEvent
        RemoveHandler Me.ModelBox.SelectedModelChanged, AddressOf Me.SelectedModelChangedEvent
    End Sub

    Private Sub ModelCollectionChangedEvent(sender As Object, e As ModelCollectionChangedEventArgs)
        Call Me.ImportModelNames()
        Call Me.RefreshSelectionRange()
    End Sub

    Private Sub ImportModelNames()
        If Me.DesignMode Then Return
        If Me.ModelBox Is Nothing Then
            Throw New FrontWorkException($"ModelBox not set in {Me.Name}!")
        End If
        RemoveHandler Me.TabControl.SelectedIndexChanged, AddressOf Me.TabControl_SelectedIndexChanged
        Call Me.TabControl.TabPages.Clear()
        For Each model As Model In Me.ModelBox.Models
            Me.TabControl.TabPages.Add(New TabPage With {
                .Name = model.Name,
                .Text = model.Name
            })
        Next
        AddHandler Me.TabControl.SelectedIndexChanged, AddressOf Me.TabControl_SelectedIndexChanged
    End Sub

    Private Sub SelectedModelChangedEvent(sender As Object, e As SelectedModelChangedEventArgs)
        Call Me.RefreshSelectionRange()
    End Sub

    Private Sub ModelRowRemovedEvent(sender As Object, e As ModelRowRemovedEventArgs)
        RemoveHandler Me.TabControl.SelectedIndexChanged, AddressOf Me.TabControl_SelectedIndexChanged
        For Each indexDataRow In e.RemovedRows
            Me.TabControl.TabPages.RemoveAt(indexDataRow.Row)
        Next
        AddHandler Me.TabControl.SelectedIndexChanged, AddressOf Me.TabControl_SelectedIndexChanged
    End Sub

    Private Sub TabControl_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl.SelectedIndexChanged
        If Me.DesignMode Then Return
        If Me.ModelBox Is Nothing Then
            Throw New FrontWorkException($"ModelBox not set in {Me.Name}!")
        End If
        RemoveHandler Me.ModelBox.SelectedModelChanged, AddressOf Me.SelectedModelChangedEvent
        Me.ModelBox.CurrentModelName = Me.TabControl.SelectedTab.Name
        AddHandler Me.ModelBox.SelectedModelChanged, AddressOf Me.SelectedModelChangedEvent
    End Sub

    Private Sub RefreshSelectionRange()
        If Me.DesignMode Then Return
        If Me.ModelBox Is Nothing Then
            Throw New FrontWorkException($"ModelBox not set in {Me.Name}!")
        End If
        RemoveHandler Me.TabControl.SelectedIndexChanged, AddressOf Me.TabControl_SelectedIndexChanged
        If Me.ModelBox.CurrentModelName IsNot Nothing Then
            For Each tabPage As TabPage In Me.TabControl.TabPages
                If tabPage.Name = Me.ModelBox.CurrentModelName Then
                    Me.TabControl.SelectedTab = tabPage
                    Exit For
                End If
            Next
        End If
        AddHandler Me.TabControl.SelectedIndexChanged, AddressOf Me.TabControl_SelectedIndexChanged
    End Sub

    Private Sub TabView_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class

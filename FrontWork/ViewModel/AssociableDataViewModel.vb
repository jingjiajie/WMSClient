Imports FrontWork

Public Class AssociableDataViewModel
    Inherits EditableDataViewModel

    Private Property AssociationVisible As Boolean = False
    Private Property EditingRow As Integer = -1
    Private Property EditingColumnName As String
    Private Shadows Property ViewOperationsWrapper As New AssociableDataViewOperator

    Public Shadows Property View As IAssociableDataView
        Get
            Return MyBase.View
        End Get
        Set(value As IAssociableDataView)
            If MyBase.View IsNot value Then
                MyBase.View = value
            End If
            If Me.ViewOperationsWrapper.View IsNot value Then
                Me.ViewOperationsWrapper.View = value
            End If
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(model As IModel)
        Call MyBase.New(model)
    End Sub

    Public Sub New(view As IAssociableDataView)
        Call MyBase.New(view)
        Me.View = view
    End Sub

    Public Sub New(model As IModel, view As IAssociableDataView)
        Call MyBase.New(model, view)
        Me.View = view
    End Sub

    Protected Overrides Sub BindView()
        AddHandler Me.View.ContentChanged, AddressOf Me.ViewContentChangedEvent
        AddHandler Me.View.EditEnded, AddressOf Me.ViewEditEndedEvent
        AddHandler Me.View.AssociationItemSelected, AddressOf Me.ViewAssociationItemSelectedEvent
        Call MyBase.BindView()
    End Sub

    Protected Overrides Sub UnbindView()
        RemoveHandler Me.View.ContentChanged, AddressOf Me.ViewContentChangedEvent
        RemoveHandler Me.View.EditEnded, AddressOf Me.ViewEditEndedEvent
        RemoveHandler Me.View.AssociationItemSelected, AddressOf Me.ViewAssociationItemSelectedEvent
        Call MyBase.UnbindView()
    End Sub

    Private Sub ViewEditEndedEvent(sender As Object, e As ViewEditEndedEventArgs)
        If Me.AssociationVisible Then
            Call Me.View.HideAssociationForm()
            Me.AssociationVisible = False
        End If
    End Sub

    Private Sub ViewContentChangedEvent(sender As Object, e As ViewContentChangedEventArgs)
        Dim fieldName = e.ColumnName
        Me.EditingColumnName = fieldName
        Me.EditingRow = e.Row
        Dim curField = Me.Configuration.GetField(Me.Mode, fieldName)
        If curField.Association Is Nothing Then GoTo NO_ASSOCIATION_ITEM '如果字段没有联想，则直接返回
        Dim data = e.CellData
        If data.ToString?.Length > 0 Then
            Dim context As New ModelViewEditInvocationContext(Me.Model, Me.View, e.Row, e.ColumnName, data)
            Dim associationItems = curField.Association.Invoke(context)
            If associationItems Is Nothing OrElse associationItems.Length = 0 Then GoTo NO_ASSOCIATION_ITEM '如果联想内容为空，则隐藏联想并返回
            '执行到这里一定是有联想内容的。所以显示联想窗口并刷新联想内容
            If Not Me.AssociationVisible Then
                Call Me.View.ShowAssociationForm()
                Me.AssociationVisible = True
            End If
            '执行联想函数，获取联想内容并更新到视图
            Call Me.View.UpdateAssociationItems(associationItems)
        Else
            Call Me.View.HideAssociationForm()
            Me.AssociationVisible = False
        End If
        Return

NO_ASSOCIATION_ITEM:
        If Me.AssociationVisible Then
            Call Me.View.HideAssociationForm()
            Me.AssociationVisible = False
        End If
        Return
    End Sub

    Private Sub ViewAssociationItemSelectedEvent(sender As Object, e As ViewAssociationItemSelectedEventArgs)
        Dim selectedWord As String = e?.SelectedAssociationItem?.Word
        RemoveHandler Me.View.CellUpdated, AddressOf Me.ViewCellUpdatedEvent
        Me.ViewOperationsWrapper.UpdateCell(Me.EditingRow, Me.EditingColumnName, selectedWord)
        AddHandler Me.View.CellUpdated, AddressOf Me.ViewCellUpdatedEvent
        If Me.AssociationVisible Then
            Me.View.HideAssociationForm()
            Me.AssociationVisible = False
        End If
    End Sub
End Class

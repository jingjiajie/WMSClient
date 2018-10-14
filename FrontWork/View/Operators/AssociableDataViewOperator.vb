Imports FrontWork

Public Class AssociableDataViewOperator
    Inherits EditableDataViewOperator
    Implements IAssociableDataView

    Public Shadows Property View As IAssociableDataView
        Get
            Return MyBase.View
        End Get
        Set(value As IAssociableDataView)
            MyBase.View = value
        End Set
    End Property

    Public Custom Event AssociationItemSelected As EventHandler(Of ViewAssociationItemSelectedEventArgs) Implements IAssociableDataView.AssociationItemSelected
        AddHandler(value As EventHandler(Of ViewAssociationItemSelectedEventArgs))
            AddHandler View.AssociationItemSelected, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ViewAssociationItemSelectedEventArgs))
            RemoveHandler View.AssociationItemSelected, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ViewAssociationItemSelectedEventArgs)
        End RaiseEvent
    End Event

    Public Sub ShowAssociationForm() Implements IAssociableDataView.ShowAssociationForm
        View.ShowAssociationForm()
    End Sub

    Public Sub UpdateAssociationItems(associationItems() As AssociationItem) Implements IAssociableDataView.UpdateAssociationItems
        View.UpdateAssociationItems(associationItems)
    End Sub

    Public Sub HideAssociationForm() Implements IAssociableDataView.HideAssociationForm
        View.HideAssociationForm()
    End Sub
End Class

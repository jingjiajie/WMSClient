Public Interface IAssociableDataView
    Inherits IEditableDataView

    Sub ShowAssociationForm()
    Sub UpdateAssociationItems(associationItems As AssociationItem())
    Sub HideAssociationForm()

    Event AssociationItemSelected As EventHandler(Of ViewAssociationItemSelectedEventArgs)

End Interface

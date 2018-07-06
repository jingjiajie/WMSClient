Public Class ViewAssociationItemSelectedEventArgs
    Inherits FrontWorkEventArgs

    Public Property SelectedAssociationItem As AssociationItem

    Public Sub New()

    End Sub

    Public Sub New(selectedAssociationItem As AssociationItem)
        Me.SelectedAssociationItem = selectedAssociationItem
    End Sub
End Class

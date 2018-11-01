Public Interface IEditableDataView
    Inherits ISelectableDataView

    Event EditStarted As EventHandler(Of ViewEditStartedEventArgs)
    Event ContentChanged As EventHandler(Of ViewContentChangedEventArgs)
    Event EditEnded As EventHandler(Of ViewEditEndedEventArgs)

    Event RowUpdated As EventHandler(Of ViewRowUpdatedEventArgs)
    Event RowAdded As EventHandler(Of ViewRowAddedEventArgs)
    Event RowRemoved As EventHandler(Of ViewRowRemovedEventArgs)
    Event CellUpdated As EventHandler(Of ViewCellUpdatedEventArgs)

End Interface

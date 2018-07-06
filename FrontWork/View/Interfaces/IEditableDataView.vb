Public Interface IEditableDataView
    Inherits ISelectableDataView

    Event EditStarted As EventHandler(Of ViewEditStartedEventArgs)
    Event ContentChanged As EventHandler(Of ViewContentChangedEventArgs)
    Event EditEnded As EventHandler(Of ViewEditEndedEventArgs)

    Event BeforeRowUpdate As EventHandler(Of BeforeViewRowUpdateEventArgs)
    Event BeforeRowAdd As EventHandler(Of BeforeViewRowAddEventArgs)
    Event BeforeRowRemove As EventHandler(Of BeforeViewRowRemoveEventArgs)
    Event BeforeCellUpdate As EventHandler(Of BeforeViewCellUpdateEventArgs)

    Event RowUpdated As EventHandler(Of ViewRowUpdatedEventArgs)
    Event RowAdded As EventHandler(Of ViewRowAddedEventArgs)
    Event RowRemoved As EventHandler(Of ViewRowRemovedEventArgs)
    Event CellUpdated As EventHandler(Of ViewCellUpdatedEventArgs)

End Interface

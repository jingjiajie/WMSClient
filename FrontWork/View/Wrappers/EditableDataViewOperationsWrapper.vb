Imports FrontWork

Public Class EditableDataViewOperationsWrapper
    Inherits SelectableDataViewOperationsWrapper
    Implements IEditableDataView

    Public Shadows Property View As IEditableDataView
        Get
            Return MyBase.View
        End Get
        Set(value As IEditableDataView)
            MyBase.View = value
        End Set
    End Property

    Public Custom Event EditStarted As EventHandler(Of ViewEditStartedEventArgs) Implements IEditableDataView.EditStarted
        AddHandler(value As EventHandler(Of ViewEditStartedEventArgs))
            AddHandler View.EditStarted, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ViewEditStartedEventArgs))
            RemoveHandler View.EditStarted, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ViewEditStartedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event ContentChanged As EventHandler(Of ViewContentChangedEventArgs) Implements IEditableDataView.ContentChanged
        AddHandler(value As EventHandler(Of ViewContentChangedEventArgs))
            AddHandler View.ContentChanged, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ViewContentChangedEventArgs))
            RemoveHandler View.ContentChanged, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ViewContentChangedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event EditEnded As EventHandler(Of ViewEditEndedEventArgs) Implements IEditableDataView.EditEnded
        AddHandler(value As EventHandler(Of ViewEditEndedEventArgs))
            AddHandler View.EditEnded, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ViewEditEndedEventArgs))
            RemoveHandler View.EditEnded, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ViewEditEndedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event BeforeRowUpdate As EventHandler(Of BeforeViewRowUpdateEventArgs) Implements IEditableDataView.BeforeRowUpdate
        AddHandler(value As EventHandler(Of BeforeViewRowUpdateEventArgs))
            AddHandler View.BeforeRowUpdate, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of BeforeViewRowUpdateEventArgs))
            RemoveHandler View.BeforeRowUpdate, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As BeforeViewRowUpdateEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event BeforeRowAdd As EventHandler(Of BeforeViewRowAddEventArgs) Implements IEditableDataView.BeforeRowAdd
        AddHandler(value As EventHandler(Of BeforeViewRowAddEventArgs))
            AddHandler View.BeforeRowAdd, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of BeforeViewRowAddEventArgs))
            RemoveHandler View.BeforeRowAdd, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As BeforeViewRowAddEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event BeforeRowRemove As EventHandler(Of BeforeViewRowRemoveEventArgs) Implements IEditableDataView.BeforeRowRemove
        AddHandler(value As EventHandler(Of BeforeViewRowRemoveEventArgs))
            AddHandler View.BeforeRowRemove, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of BeforeViewRowRemoveEventArgs))
            RemoveHandler View.BeforeRowRemove, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As BeforeViewRowRemoveEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event BeforeCellUpdate As EventHandler(Of BeforeViewCellUpdateEventArgs) Implements IEditableDataView.BeforeCellUpdate
        AddHandler(value As EventHandler(Of BeforeViewCellUpdateEventArgs))
            AddHandler View.BeforeCellUpdate, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of BeforeViewCellUpdateEventArgs))
            RemoveHandler View.BeforeCellUpdate, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As BeforeViewCellUpdateEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event RowUpdated As EventHandler(Of ViewRowUpdatedEventArgs) Implements IEditableDataView.RowUpdated
        AddHandler(value As EventHandler(Of ViewRowUpdatedEventArgs))
            AddHandler View.RowUpdated, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ViewRowUpdatedEventArgs))
            RemoveHandler View.RowUpdated, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ViewRowUpdatedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event RowAdded As EventHandler(Of ViewRowAddedEventArgs) Implements IEditableDataView.RowAdded
        AddHandler(value As EventHandler(Of ViewRowAddedEventArgs))
            AddHandler View.RowAdded, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ViewRowAddedEventArgs))
            RemoveHandler View.RowAdded, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ViewRowAddedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event RowRemoved As EventHandler(Of ViewRowRemovedEventArgs) Implements IEditableDataView.RowRemoved
        AddHandler(value As EventHandler(Of ViewRowRemovedEventArgs))
            AddHandler View.RowRemoved, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ViewRowRemovedEventArgs))
            RemoveHandler View.RowRemoved, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ViewRowRemovedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event CellUpdated As EventHandler(Of ViewCellUpdatedEventArgs) Implements IEditableDataView.CellUpdated
        AddHandler(value As EventHandler(Of ViewCellUpdatedEventArgs))
            AddHandler View.CellUpdated, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ViewCellUpdatedEventArgs))
            RemoveHandler View.CellUpdated, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ViewCellUpdatedEventArgs)
        End RaiseEvent
    End Event
End Class

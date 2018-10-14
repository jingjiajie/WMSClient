Imports FrontWork

Public Class SelectableDataViewOperator
    Inherits DataViewOperator
    Implements ISelectableDataView

    Public Shadows Property View As ISelectableDataView
        Get
            Return MyBase.View
        End Get
        Set(value As ISelectableDataView)
            MyBase.View = value
        End Set
    End Property

    Public Custom Event BeforeSelectionRangeChange As EventHandler(Of BeforeViewSelectionRangeChangeEventArgs) Implements ISelectableDataView.BeforeSelectionRangeChange
        AddHandler(value As EventHandler(Of BeforeViewSelectionRangeChangeEventArgs))
            AddHandler View.BeforeSelectionRangeChange, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of BeforeViewSelectionRangeChangeEventArgs))
            RemoveHandler View.BeforeSelectionRangeChange, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As BeforeViewSelectionRangeChangeEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event SelectionRangeChanged As EventHandler(Of ViewSelectionRangeChangedEventArgs) Implements ISelectableDataView.SelectionRangeChanged
        AddHandler(value As EventHandler(Of ViewSelectionRangeChangedEventArgs))
            AddHandler View.SelectionRangeChanged, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ViewSelectionRangeChangedEventArgs))
            RemoveHandler View.SelectionRangeChanged, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ViewSelectionRangeChangedEventArgs)
        End RaiseEvent
    End Event

    Public Overridable Sub SetSelectionRanges(ranges() As Range) Implements ISelectableDataView.SetSelectionRanges
        Me.View.SetSelectionRanges(ranges)
    End Sub

    Public Overridable Sub SetSelectionRange(range As Range)
        Call Me.SetSelectionRanges({range})
    End Sub

    Public Overridable Function GetSelectionRanges() As Range() Implements ISelectableDataView.GetSelectionRanges
        Return Me.View.GetSelectionRanges()
    End Function

    Public Overridable Function GetSelectionRange() As Range
        Dim ranges = Me.GetSelectionRanges
        If ranges Is Nothing OrElse ranges.Length = 0 Then
            Return Nothing
        Else
            Return ranges(0)
        End If
    End Function

End Class

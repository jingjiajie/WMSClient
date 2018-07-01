Public Interface ISelectableDataView
    Inherits IDataView
    ''' <summary>
    ''' 获取View的选区
    ''' </summary>
    ''' <returns></returns>
    Function GetSelectionRanges() As Range()

    ''' <summary>
    ''' 设置View的选区
    ''' </summary>
    ''' <param name="ranges">选区</param>
    Sub SetSelectionRanges(ranges As Range())

    Event BeforeSelectionRangeChange As EventHandler(Of BeforeViewSelectionRangeChangeEventArgs)
    Event SelectionRangeChanged As EventHandler(Of ViewSelectionRangeChangedEventArgs)
End Interface

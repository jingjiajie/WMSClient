Public Interface IDataView
    Inherits IView
    Function GetViewComponent(row As Long, fieldName As String) As IViewComponent
End Interface

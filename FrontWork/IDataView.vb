Public Interface IDataView
    Inherits IView
    Function GetViewComponent(row as Integer, fieldName As String) As IViewComponent
End Interface

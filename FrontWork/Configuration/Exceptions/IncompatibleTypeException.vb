Public Class IncompatibleTypeException
    Inherits FrontWorkException

    Public Sub New(sourceValue As Object, targetType As Type)
        Call MyBase.New($"Cannot convert ""{If(sourceValue, "null")}"" of {sourceValue.GetType.Name} to {targetType.Name}")
    End Sub
End Class

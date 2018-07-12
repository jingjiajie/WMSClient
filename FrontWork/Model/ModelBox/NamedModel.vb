Imports FrontWork

Public Class NamedModel
    Inherits ModelComponent
    Implements IModel

    Public Sub New(model As ModelComponent, name As String)
        Call MyBase.New(model)
        Me.Name = name
    End Sub

    Public Property Name As String
End Class

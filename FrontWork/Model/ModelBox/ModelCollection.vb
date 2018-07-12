Public Class ModelCollection
    Inherits CollectionBase

    Default Public Property Item(modelName As String) As IModel
        Get
            Return Me.GetModel(modelName)
        End Get
        Set(value As IModel)
            Call Me.SetModel(value, modelName)
        End Set
    End Property

    Public Function GetModel(modelName As String) As IModel
        If Not Me.Contains(modelName) Then
            Throw New FrontWorkException($"Model ""{modelName}"" not exist in ModelCollection!")
        End If
        For Each pair As NamedModel In Me.InnerList
            If pair.Name.Equals(modelName, StringComparison.OrdinalIgnoreCase) Then
                Return pair.Model
            End If
        Next
        Return Nothing
    End Function

    Public Sub SetModel(model As IModel, name As String)
        For i = 0 To Me.InnerList.Count - 1
            Dim curPair As NamedModel = Me.InnerList(i)
            If curPair.Name.Equals(name, StringComparison.OrdinalIgnoreCase) Then
                Me.InnerList(i) = model
                RaiseEvent ModelCollectionChanged(Me, New ModelCollectionChangedEventArgs)
                Return
            End If
        Next
        Me.InnerList.Add(New NamedModel(model, name))
        RaiseEvent ModelCollectionChanged(Me, New ModelCollectionChangedEventArgs)
    End Sub

    Public Function Contains(modelName As String) As Boolean
        For Each pair As NamedModel In Me.InnerList
            If pair.Name.Equals(modelName, StringComparison.OrdinalIgnoreCase) Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Event ModelCollectionChanged As EventHandler(Of ModelCollectionChangedEventArgs)
End Class
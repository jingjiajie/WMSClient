Imports System.ComponentModel
Imports System.Linq
Imports FrontWork

Public Class ModelBox
    Inherits Model
    Implements IModel
    Private _currentModelName As String
    Private _configuration As Configuration
    Private _mode As String

    Public Event SelectedModelChanged As EventHandler(Of SelectedModelChangedEventArgs)
    Public Event ModelCollectionChanged As EventHandler(Of ModelCollectionChangedEventArgs)

    <DesignerSerializationVisibility(False)>
    Public ReadOnly Property Models As New ModelCollection

    <Description("当前模型名称"), Category("FrontWork")>
    Public Property CurrentModelName As String
        Get
            Return Me._currentModelName
        End Get
        Set(value As String)
            If value Is Nothing Then
                Throw New FrontWorkException("ModelName cannot be null!")
            End If
            If Not Me._Models.Contains(value) Then
                Dim newModel As New Model
                newModel.Name = value
                newModel.Configuration = Me.Configuration
                Me._Models.SetModel(newModel)
                Me.ModelOperator.ModelCore = newModel
            Else
                Me.ModelOperator.ModelCore = Me._Models(value)
            End If
            Me._currentModelName = value
            RaiseEvent SelectedModelChanged(Me, New SelectedModelChangedEventArgs)
            Call Me.ModelOperator.RaiseRefreshedEvent(Me, New ModelRefreshedEventArgs)
        End Set
    End Property

    Public Sub New()
        Call MyBase.New
        If Not Me.DesignMode Then Me.Visible = False
        Me.CurrentModelName = "default"
        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        AddHandler Me.Models.ModelCollectionChanged,
            Sub(sender, e)
                RaiseEvent ModelCollectionChanged(sender, e)
            End Sub
    End Sub

    Public Sub GroupBy(fieldName As String)
        Dim dataRows = Me.GetRows(Util.Range(0, Me.GetRowCount))
        'If Not Me.GetColumns.Contains(fieldName) Then
        '    Throw New FrontWorkException($"""{fieldName}"" not exist in {Me.Name}")
        'End If
        Dim groups = (From row In dataRows
                      Group By key = CStr(If(row(fieldName), "")) Into g = Group
                      Select New With {.Key = key, .Values = g}).ToArray
        Call Me.Models.Clear() '清空所有Model
        For Each group In groups
            Dim groupName = group.Key
            Dim groupRows = group.Values.ToArray
            '创建新的Model
            Dim newModel = New Model
            newModel.Configuration = Me.Configuration
            newModel.Name = groupName
            newModel.Refresh(New ModelRefreshArgs(groupRows, {New Range(0, 0, 1, 1)}))
            Me.Models.SetModel(newModel)
        Next
        If groups.Length > 0 Then
            Dim firstGroup = groups(0)
            Me.CurrentModelName = firstGroup.Key
        Else
            Me._currentModelName = Nothing
        End If
    End Sub
End Class

Public Class ModelCollection
    Inherits CollectionBase

    Default Public Property Item(modelName As String) As Model
        Get
            Return Me.GetModel(modelName)
        End Get
        Set(value As Model)
            Call Me.SetModel(value)
        End Set
    End Property

    Public Function GetModel(modelName As String) As Model
        If Not Me.Contains(modelName) Then
            Throw New FrontWorkException($"Model ""{modelName}"" not exist in ModelCollection!")
        End If
        For Each model As Model In Me.InnerList
            If model.Name.Equals(modelName, StringComparison.OrdinalIgnoreCase) Then
                Return model
            End If
        Next
        Return Nothing
    End Function

    Public Sub SetModel(model As Model)
        For i = 0 To Me.InnerList.Count - 1
            Dim curModel As Model = Me.InnerList(i)
            If curModel.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase) Then
                Me.InnerList(i) = model
                RaiseEvent ModelCollectionChanged(Me, New ModelCollectionChangedEventArgs)
                Return
            End If
        Next
        Me.InnerList.Add(model)
        RaiseEvent ModelCollectionChanged(Me, New ModelCollectionChangedEventArgs)
    End Sub

    Public Function Contains(modelName As String) As Boolean
        For Each curModel As Model In Me.InnerList
            If curModel.Name.Equals(modelName, StringComparison.OrdinalIgnoreCase) Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Event ModelCollectionChanged As EventHandler(Of ModelCollectionChangedEventArgs)
End Class
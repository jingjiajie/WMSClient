Imports System.Linq

Public Class ConfigurableModelOperator
    Inherits ModelOperator
    Implements IConfigurableModel

    Private _configuration As Configuration
    Private _mode As String = "default"

    Public Sub New(modelCore As IModelCore)
        Call MyBase.New(modelCore)
    End Sub

    Public Sub New()

    End Sub

    Public Shadows Property ModelCore As IModelCore
        Get
            Return MyBase.ModelCore
        End Get
        Set(value As IModelCore)
            MyBase.ModelCore = value
        End Set
    End Property

    ''' <summary>
    ''' 配置中心对象
    ''' </summary>
    ''' <returns></returns>
    Public Property Configuration As Configuration Implements IConfigurableModel.Configuration
        Get
            Return Me._configuration
        End Get
        Set(value As Configuration)
            If Me._configuration IsNot Nothing Then
                RemoveHandler Me._configuration.Refreshed, AddressOf Me.ConfigurationRefreshedEvent
                RemoveHandler Me._configuration.FieldAdded, AddressOf Me.ConfigurationFieldAddedEvent
                RemoveHandler Me._configuration.FieldUpdated, AddressOf Me.ConfigurationFieldUpdatedEvent
                RemoveHandler Me._configuration.FieldRemoved, AddressOf Me.ConfigurationFieldRemovedEvent
            End If
            Me._configuration = value
            If Me._configuration IsNot Nothing Then
                Call Me.ConfigurationRefreshedEvent(Me, Nothing)
                AddHandler Me._configuration.Refreshed, AddressOf Me.ConfigurationRefreshedEvent
                AddHandler Me._configuration.FieldAdded, AddressOf Me.ConfigurationFieldAddedEvent
                AddHandler Me._configuration.FieldUpdated, AddressOf Me.ConfigurationFieldUpdatedEvent
                AddHandler Me._configuration.FieldRemoved, AddressOf Me.ConfigurationFieldRemovedEvent
            End If
        End Set
    End Property

    Private Sub ConfigurationFieldRemovedEvent(sender As Object, e As ConfigurationFieldRemovedEventArgs)
        Call Me.ModelCore.RemoveColumns(e.RemovedFields.Select(Function(f) f.Index).ToArray)
    End Sub

    Private Sub ConfigurationFieldUpdatedEvent(sender As Object, e As ConfigurationFieldUpdatedEventArgs)
        Dim context = New ModelInvocationContext(Me)
        Call Me.ModelCore.UpdateColumn(e.UpdatedFields.Select(Function(f) f.Index).ToArray,
                                   e.UpdatedFields.Select(Function(f) New ModelColumn(context, f.Field)).ToArray)
    End Sub

    Private Sub ConfigurationFieldAddedEvent(sender As Object, e As ConfigurationFieldAddedEventArgs)
        Call Me.RefreshCoreSchema(Me.Configuration)
    End Sub

    ''' <summary>
    ''' 当前模式
    ''' </summary>
    ''' <returns></returns>
    Public Property Mode As String Implements IConfigurableModel.Mode
        Get
            Return Me._mode
        End Get
        Set(value As String)
            Me._mode = value
            If Me.Configuration IsNot Nothing Then
                Call Me.ConfigurationRefreshedEvent(Me, Nothing)
            End If
        End Set
    End Property

    Private Sub ConfigurationRefreshedEvent(sender As Object, e As ConfigurationRefreshedEventArgs)
        Call Me.RefreshCoreSchema(Me.Configuration)
    End Sub

    Private Sub RefreshCoreSchema(config As Configuration)
        Dim context = New ModelInvocationContext(Me)
        Dim fields = config.GetFields(Me.Mode)
        Dim addColumns As New List(Of ModelColumn)
        For Each field In fields
            If Not Me.ContainsColumn(field.Name) Then
                Dim newColumn As New ModelColumn(context, field)
                addColumns.Add(newColumn)
            End If
        Next
        If addColumns.Count > 0 Then
            Call Me.ModelCore.AddColumns(addColumns.ToArray)
        End If
    End Sub
End Class

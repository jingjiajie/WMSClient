Public Class ModelConfigurationWrapper
    Inherits ModelWrapperBase
    Implements IModelCore

    Private _configuration As Configuration
    Private _mode As String = "default"

    ''' <summary>
    ''' 配置中心对象
    ''' </summary>
    ''' <returns></returns>
    Public Property Configuration As Configuration
        Get
            Return Me._configuration
        End Get
        Set(value As Configuration)
            If Me._configuration IsNot Nothing Then
                RemoveHandler Me._configuration.ConfigurationChanged, AddressOf Me.ConfigurationChanged
            End If
            Me._configuration = value
            If Me._configuration IsNot Nothing Then
                Call Me.RefreshCoreSchema(Me._configuration)
                AddHandler Me._configuration.ConfigurationChanged, AddressOf Me.ConfigurationChanged
            End If
        End Set
    End Property

    ''' <summary>
    ''' 当前模式
    ''' </summary>
    ''' <returns></returns>
    Public Property Mode As String
        Get
            Return Me._mode
        End Get
        Set(value As String)
            Me._mode = value
            Call Me.ConfigurationChanged(Me, New ConfigurationChangedEventArgs)
        End Set
    End Property

    Public Sub New(modelCore As IModelCore)
        Me.ModelCore = modelCore
    End Sub

    Private Sub ConfigurationChanged(sender As Object, e As ConfigurationChangedEventArgs)
        Call Me.RefreshCoreSchema(Me.Configuration)
        Call Me.RaiseRefreshedEvent(Me, New ModelRefreshedEventArgs)
    End Sub

    Private Sub RefreshCoreSchema(config As Configuration)
        Dim fields = config.GetFieldConfigurations(Me.Mode)
        Dim addColumns As New List(Of ModelColumn)
        For Each field In fields
            If Not Me.ContainsColumn(field.Name) Then
                Dim newColumn As New ModelColumn
                With newColumn
                    .Name = field.Name
                    .Type = field.Type.FieldType
                    .Nullable = True
                    .DefaultValue = field.DefaultValue
                End With
                addColumns.Add(newColumn)
            End If
        Next
        If addColumns.Count > 0 Then
            Call Me.ModelCore.AddColumns(addColumns.ToArray)
        End If
    End Sub

    Private Function ContainsColumn(columnName As String) As Boolean
        Return Me.GetColumns({columnName})(0) IsNot Nothing
    End Function
End Class

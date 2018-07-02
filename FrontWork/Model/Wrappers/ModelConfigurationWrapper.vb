Public Class ModelConfigurationWrapper
    Inherits ModelOperationsWrapper
    Implements IModel

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
                Call Me.ConfigurationChanged(Me, Nothing)
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

    Public Sub New(modelCore As IModel)
        Call MyBase.New(modelCore)
    End Sub

    Private Sub ConfigurationChanged(sender As Object, e As ConfigurationChangedEventArgs)
        Call Me.RefreshCoreSchema(Me.Configuration)
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
            Call Me.Model.AddColumns(addColumns.ToArray)
        End If
    End Sub

End Class

Imports FrontWork

Public Class ModelColumn
    Private _defaultValue As FieldProperty

    Public Sub New(context As InvocationContext, defaultValue As FieldProperty, name As String, type As Type, nullable As Boolean)
        _defaultValue = defaultValue
        Me.Context = context
        Me.Name = name
        Me.Type = type
        Me.Nullable = nullable
    End Sub

    Public Sub New(context As InvocationContext, field As Field)
        Call Me.New(context, field.DefaultValue, field.Name.GetValue, field.Type.GetValue, True)
    End Sub

    Public Property Context As InvocationContext
    Public Property Name As String
    Public Property Type As Type = GetType(String)
    Public Property Nullable As Boolean = True
    Public ReadOnly Property DefaultValue As Object
        Get
            Return Me._defaultValue?.Invoke(Me.Context)
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return Me.Name
    End Function
End Class

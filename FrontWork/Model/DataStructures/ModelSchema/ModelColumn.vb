Imports FrontWork

Public Class ModelColumn
    Private _defaultValue As FieldMethod

    Public Sub New(context As InvocationContext, defaultValue As FieldMethod, name As String, type As Type, nullable As Boolean)
        _defaultValue = defaultValue
        Me.Context = context
        Me.Name = name
        Me.Type = type
        Me.Nullable = nullable
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

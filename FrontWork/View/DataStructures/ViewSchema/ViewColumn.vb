Imports FrontWork

Public Class ViewColumn
    Public Sub New(context As InvocationContext, placeHolder As FieldProperty(Of String), values As FieldProperty(Of Object()), name As String, displayName As String, type As Type, editable As Boolean)
        Me.Context = context
        _placeHolder = placeHolder
        _values = values
        Me.Name = name
        Me.DisplayName = displayName
        Me.Type = type
        Me.Editable = editable
    End Sub

    Private Property Context As InvocationContext
    Private Property _placeHolder As FieldProperty(Of String)
    Private Property _values As FieldProperty(Of Object())

    Public Property Name As String
    Public Property DisplayName As String
    Public Property Type As Type = GetType(String)
    Public Property Editable As Boolean = False
    Public ReadOnly Property PlaceHolder As String
        Get
            Return Me._placeHolder?.Invoke(Me.Context)
        End Get
    End Property

    Public ReadOnly Property Values As Object()
        Get
            Return Me._values?.Invoke(Me.Context)
        End Get
    End Property

    Public Shared Operator =(viewColumn1 As ViewColumn, viewColumn2 As ViewColumn) As Boolean
        If viewColumn1 Is viewColumn2 Then Return True
        If viewColumn1 Is Nothing OrElse viewColumn2 Is Nothing Then Return False
        If viewColumn1.Name <> viewColumn2.Name Then Return False
        If viewColumn1.DisplayName <> viewColumn2.DisplayName Then Return False
        If viewColumn1.Editable <> viewColumn2.Editable Then Return False
        If Not Util.EqualOrBothNothing(viewColumn1._placeHolder, viewColumn2._placeHolder) Then Return False
        If viewColumn1.Type <> viewColumn2.Type Then Return False
        If Not Util.EqualOrBothNothing(viewColumn1._values, viewColumn2._values) Then Return False
        Return True
    End Operator

    Public Shared Operator <>(viewColumn1 As ViewColumn, viewColumn2 As ViewColumn) As Boolean
        Return Not viewColumn1 = viewColumn2
    End Operator

    Public Overrides Function ToString() As String
        Return Me.DisplayName
    End Function
End Class

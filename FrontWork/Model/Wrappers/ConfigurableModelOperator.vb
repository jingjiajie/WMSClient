Public Class ConfigurableModelOperator
    Inherits ModelOperator
    Implements IConfigurableModel

    Public Shadows Property Model As IConfigurableModel
        Get
            Return MyBase.Model
        End Get
        Set(value As IConfigurableModel)
            MyBase.Model = value
        End Set
    End Property

    Public Property Configuration As Configuration Implements IConfigurableModel.Configuration
        Get
            Return Me.Model.Configuration
        End Get
        Set(value As Configuration)
            Me.Model.Configuration = value
        End Set
    End Property

    Public Property Mode As String Implements IConfigurableModel.Mode
        Get
            Return Me.Model.Mode
        End Get
        Set(value As String)
            Me.Model.Mode = value
        End Set
    End Property
End Class

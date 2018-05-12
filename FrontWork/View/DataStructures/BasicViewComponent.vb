Imports FrontWork

Public Class BasicViewComponent
    Implements IViewComponent

    Private Property control As Control

    Public Sub New(control As Control)
        Me.control = control
    End Sub

    Public Property Text As String Implements IViewComponent.Text
        Get
            Return control.Text
        End Get
        Set(value As String)
            control.Text = value
        End Set
    End Property

    Public Property Color As Color Implements IViewComponent.Color
        Get
            Return control.BackColor
        End Get
        Set(value As Color)
            control.BackColor = value
        End Set
    End Property
End Class

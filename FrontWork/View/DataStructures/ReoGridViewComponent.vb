Imports FrontWork
Imports unvell.ReoGrid

Public Class ReoGridViewComponent
    Implements IViewComponent

    Private Property Cell As Cell

    Public Property Text As String Implements IViewComponent.Text
        Get
            Return Me.Cell.Data
        End Get
        Set(value As String)
            Me.Cell.Data = value
        End Set
    End Property

    Public Property Color As Color Implements IViewComponent.Color
        Get
            Return Me.Cell.Style.BackColor
        End Get
        Set(value As Color)
            Me.Cell.Style.BackColor = value
        End Set
    End Property
End Class

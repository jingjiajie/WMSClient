Imports System.ComponentModel
Imports System.Drawing.Design

Public Class ConfiguratorEditor
    Inherits UITypeEditor

    Public Overrides Function GetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function

    Public Overrides Function EditValue(context As ITypeDescriptorContext, provider As IServiceProvider, value As Object) As Object
        Dim configurator As New Configurator
        configurator.SetJson(value.ToString)
        configurator.ShowDialog()
        Return configurator.ToJson
    End Function
End Class

Imports System.Text
Imports FrontWork

Public Class Configurator
    Public Sub New()
        MethodListenerContainer.Register("ConfiguratorMethodListener", Me)
        ' 此调用是设计器所必需的。
        InitializeComponent()
        Me.ModelModes.AddRow(New Dictionary(Of String, Object) From {
            {"name", "default"}
        })
        ' 在 InitializeComponent() 调用之后添加任何初始化。
        AddHandler Me.ModelModes.SelectionRangeChanged, AddressOf Me.ModelModesSelectionChanged
    End Sub

    Private Sub ModelModesSelectionChanged(sender As Object, e As ModelSelectionRangeChangedEventArgs)
        If Me.ModelModes.SelectionRange Is Nothing Then Return
        Dim data = Me.ModelModes(Me.ModelModes.SelectionRange.Row, "name")
        If data IsNot Nothing Then
            Me.ModelBoxFields.CurrentModelName = data.ToString
            Me.ModelBoxHTTPAPIs.CurrentModelName = data.ToString
        End If
    End Sub

    Private Sub ButtonAddField_Click(sender As Object, e As EventArgs) Handles ButtonAddField.Click
        Me.ModelBoxFields.AddRow(New Dictionary(Of String, Object) From {
         {"visible", True}, {"editable", True}
        })
    End Sub

    Private Sub ButtonRemoveField_Click(sender As Object, e As EventArgs) Handles ButtonRemoveField.Click
        Call Me.ModelBoxFields.RemoveSelectedRows()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Public Function ToJson() As String
        'Dim sbJson As New StringBuilder
        'sbJson.AppendLine("[")
        'For i = 0 To Me.ModelBoxFields.Models.Length - 1
        '    Dim modelFields = Me.ModelBoxFields.Models(i)
        '    Dim datatableFields = modelFields.GetDataTable
        '    sbJson.AppendLine($"{{""mode"":""{modelFields.Name}"",")
        '    '生成字段配置
        '    sbJson.AppendLine(vbTab & """fields"":[")
        '    For j = 0 To datatableFields.Rows.Count - 1
        '        Dim row = datatableFields.Rows(j)
        '        Dim curField = Me.GenerateFieldConfiguration(row)
        '        If curField IsNot Nothing Then
        '            sbJson.Append(curField)
        '            If j <> datatableFields.Rows.Count - 1 Then
        '                sbJson.Append(",")
        '            End If
        '            sbJson.AppendLine()
        '        End If
        '    Next
        '    sbJson.Append(vbTab)
        '    sbJson.AppendLine("]")
        '    '生成API配置
        '    Dim modelHTTPAPIs = Me.ModelBoxHTTPAPIs.Models(modelFields.Name)
        '    Dim datatableHTTPAPIs = modelHTTPAPIs.GetDataTable
        '    sbJson.AppendLine(vbTab & ",""httpAPIs"":[")
        '    For j = 0 To datatableHTTPAPIs.Rows.Count - 1
        '        Dim row = datatableHTTPAPIs.Rows(j)
        '        Dim curAPI = Me.GenerateHTTPAPIConfiguration(row)
        '        If curAPI IsNot Nothing Then
        '            sbJson.Append(curAPI)
        '            If j <> datatableHTTPAPIs.Rows.Count - 1 Then
        '                sbJson.Append(",")
        '            End If
        '            sbJson.AppendLine()
        '        End If
        '    Next
        '    sbJson.Append(vbTab)
        '    sbJson.AppendLine("]")
        '    sbJson.Append("}")
        '    If i <> Me.ModelBoxFields.Models.Length - 1 Then
        '        sbJson.Append(",")
        '    End If
        '    sbJson.AppendLine()
        'Next
        'sbJson.Append("]")
        'Return sbJson.ToString

    End Function


    Private Sub ButtonGenerateJson_Click(sender As Object, e As EventArgs)
        Me.TextBoxResult.Text = Me.ToJson.ToString
        Me.TabControlBottom.SelectedIndex = 1
    End Sub

    Private Function GenerateFieldConfiguration(row As DataRow) As String
        If IsDBNull(row("name")) Then Return Nothing
        Dim sbJson As New StringBuilder
        sbJson.Append(vbTab & vbTab)
        sbJson.Append("{""name"":""" & row("name") & """,")
        If Not IsDBNull(row("displayName")) AndAlso Not String.IsNullOrWhiteSpace(row("displayName")) Then
            sbJson.Append("""displayName"":""" & row("displayName") & """,")
        End If
        If Not IsDBNull(row("visible")) Then
            sbJson.Append("""visible"":" & If(row("visible") = True, "true", "false") & ",")
        End If
        If Not IsDBNull(row("editable")) Then
            sbJson.Append("""editable"":" & If(row("editable") = True, "true", "false") & ",")
        End If
        If Not IsDBNull(row("placeHolder")) AndAlso Not String.IsNullOrWhiteSpace(row("placeHolder")) Then
            sbJson.Append("""placeHolder"":""" & row("placeHolder") & """,")
        End If
        If Not IsDBNull(row("values")) AndAlso Not String.IsNullOrWhiteSpace(row("values")) Then
            sbJson.Append("""values"":""" & row("values") & """,")
        End If
        If Not IsDBNull(row("association")) AndAlso Not String.IsNullOrWhiteSpace(row("association")) Then
            sbJson.Append("""association"":""" & row("association") & """,")
        End If
        If Not IsDBNull(row("forwardMapper")) AndAlso Not String.IsNullOrWhiteSpace(row("forwardMapper")) Then
            Dim str As String = row("forwardMapper")
            sbJson.Append("""forwardMapper"":""" & str & """,")
        End If
        If Not IsDBNull(row("backwardMapper")) AndAlso Not String.IsNullOrWhiteSpace(row("backwardMapper")) Then
            Dim str As String = row("backwardMapper")
            sbJson.Append("""backwardMapper"":""" & str & """,")
        End If
        If Not IsDBNull(row("contentChanged")) AndAlso Not String.IsNullOrWhiteSpace(row("contentChanged")) Then
            Dim str As String = row("contentChanged")
            sbJson.Append("""contentChanged"":""" & str & """,")
        End If
        If Not IsDBNull(row("editEnded")) AndAlso Not String.IsNullOrWhiteSpace(row("editEnded")) Then
            Dim str As String = row("editEnded")
            sbJson.Append("""editEnded"":""" & str & """,")
        End If
        sbJson.Length = sbJson.Length - 1
        sbJson.Append("}")
        Return sbJson.ToString
    End Function

    Private Function GenerateHTTPAPIConfiguration(row As DataRow) As String
        If IsDBNull(row("type")) Then Return Nothing
        Dim sbJson As New StringBuilder
        sbJson.Append(vbTab & vbTab)
        sbJson.Append("{""type"":""" & row("type") & """,")
        If Not IsDBNull(row("url")) AndAlso Not String.IsNullOrWhiteSpace(row("url")) Then
            sbJson.Append("""url"":""" & row("url") & """,")
        End If
        If Not IsDBNull(row("method")) AndAlso Not String.IsNullOrWhiteSpace(row("method")) Then
            sbJson.Append("""method"":""" & row("method") & """,")
        End If
        If Not IsDBNull(row("requestBody")) AndAlso Not String.IsNullOrWhiteSpace(row("requestBody")) Then
            sbJson.Append("""requestBody"":""" & row("requestBody") & """,")
        End If
        If Not IsDBNull(row("responseBody")) AndAlso Not String.IsNullOrWhiteSpace(row("responseBody")) Then
            sbJson.Append("""responseBody"":""" & row("responseBody") & """,")
        End If
        If Not IsDBNull(row("callback")) AndAlso Not String.IsNullOrWhiteSpace(row("callback")) Then
            sbJson.Append("""callback"":""" & row("callback") & """,")
        End If
        sbJson.Length = sbJson.Length - 1
        sbJson.Append("}")
        Return sbJson.ToString
    End Function

    Private Sub ButtonLoad_Click(sender As Object, e As EventArgs)
        Me.SetJson(Me.TextBoxResult.Text)
    End Sub

    Public Function SetJson(json As String) As Boolean
        Dim cfg As New Configuration
        Try
            cfg.Configurate(json)
        Catch ex As Exception
            Call MessageBox.Show("输入的Json不合法，错误信息：" & vbCrLf & ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
        For Each modeConfiguration In cfg.ModeConfigurations
            If modeConfiguration.Mode = "default" Then
                Me.ModelModes.SelectionRange = New Range(0, 0, 1, 1)
            Else
                Me.ModelModes.AddRow(New Dictionary(Of String, Object) From {
                {"name", modeConfiguration.Mode}
            })
            End If
            '将ModelBox切换到相应的Model
            Me.ModelBoxFields.CurrentModelName = modeConfiguration.Mode
            Me.ModelBoxHTTPAPIs.CurrentModelName = modeConfiguration.Mode
            '添加Fields配置
            Dim dataTableFields = Me.ModelBoxFields.GetDataTable
            Call dataTableFields.Rows.Clear()
            For Each curField In modeConfiguration.Fields
                Dim newRow = dataTableFields.NewRow
                dataTableFields.Rows.Add(newRow)
                newRow("name") = curField.Name
                newRow("displayName") = curField.DisplayName
                newRow("visible") = curField.Visible
                newRow("editable") = curField.Editable
                newRow("placeHolder") = curField.PlaceHolder
                newRow("values") = curField.Values?.DeclareString
                newRow("association") = curField.Association?.DeclareString
                newRow("forwardMapper") = curField.ForwardMapper?.DeclareString
                newRow("backwardMapper") = curField.BackwardMapper?.DeclareString
                newRow("contentChanged") = curField.ContentChanged?.DeclareString
                newRow("editEnded") = curField.EditEnded?.DeclareString
            Next
            Call Me.ModelBoxFields.Refresh(dataTableFields, {New Range(0, 0, 1, 1)}, Nothing)
            '添加HTTPAPI配置
            Dim dataTableAPIs = ModelBoxHTTPAPIs.GetDataTable
            Call dataTableAPIs.Rows.Clear()
            For Each curAPI In modeConfiguration.HTTPAPIs
                Dim newRow = dataTableAPIs.NewRow
                dataTableAPIs.Rows.Add(newRow)
                newRow("type") = curAPI.Type
                newRow("url") = curAPI.URL
                newRow("method") = curAPI.Method
                newRow("requestBody") = curAPI.RequestBody
                newRow("responseBody") = curAPI.ResponseBody
            Next
            Call Me.ModelBoxHTTPAPIs.Refresh(dataTableAPIs, {New Range(0, 0, 1, 1)}, Nothing)
        Next
        Me.TextBoxResult.Text = json
        Return True
    End Function

    Private Sub ButtonAddMode_Click(sender As Object, e As EventArgs) Handles ButtonAddMode.Click
        Dim modeName = InputBox("请输入要添加的模式名称", "提示")
        If String.IsNullOrWhiteSpace(modeName) Then
            MessageBox.Show("模式名称不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Me.ModelModes.AddRow(New Dictionary(Of String, Object) From {{"name", modeName}})
    End Sub

    Private Sub ButtonRemoveMode_Click(sender As Object, e As EventArgs) Handles ButtonRemoveMode.Click
        'If Me.ModelModes.SelectionRange.Rows = Me.ModelModes.RowCount Then
        '    MessageBox.Show("不允许删除所有模式", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    Return
        'End If
        'For row = Me.ModelModes.SelectionRange.Row To Me.ModelModes.SelectionRange.Row + Me.ModelModes.SelectionRange.Rows - 1
        '    Dim modelName = Me.ModelModes(row, "name")
        '    Me.ModelBoxFields.RemoveModel(modelName)
        '    Me.ModelBoxHTTPAPIs.RemoveModel(modelName)
        'Next
        'Call Me.ModelModes.RemoveSelectedRows()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.ModelBoxHTTPAPIs.RemoveSelectedRows()
    End Sub

    Private Sub ButtonAddAPI_Click(sender As Object, e As EventArgs) Handles ButtonAddAPI.Click
        Call Me.ModelBoxHTTPAPIs.AddRow(Nothing)
    End Sub

    Private Sub ConfigurationHTTPAPIs_Load(sender As Object, e As EventArgs) Handles ConfigurationHTTPAPIs.Load

    End Sub

    Private Sub Configurator_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Sub APITypeContentChanged(view As IView, type As String)
        If type = "pushFinishedCallback" Then
            Me.BasicViewHTTPAPIs.Mode = "push-callback"
        Else
            Me.BasicViewHTTPAPIs.Mode = "default"
        End If
    End Sub

    Private Sub ButtonOK_Click(sender As Object, e As EventArgs) Handles ButtonOK.Click
        Me.TextBoxResult.Text = Me.ToJson
        Me.Close()
    End Sub
End Class
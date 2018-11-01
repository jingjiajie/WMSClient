Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web.Script.Serialization
Imports FrontWork
Imports Jint.Native

Public Class JsonRESTSynchronizer
    Inherits UserControl
    Implements ISynchronizer
    Private _model As IModel
    Private _configuration As Configuration
    Private _mode As String = "default"
    Private Property RequestParams As New List(Of ModeParams)
    Private Property JsonRequestParams As New List(Of ModeParams)

    Private jsEngine As New Jint.Engine
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label

    ''' <summary>
    ''' 字段映射配置
    ''' </summary>
    ''' <returns></returns>
    <Description("API字段名和Model字段名的映射配置"), Category("FrontWork")>
    <Editor(GetType(Design.ArrayEditor), GetType(UITypeEditor))>
    Public Property FieldMapping As FieldMappingItem() = {}

    ''' <summary>
    ''' 增加行的API信息
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property AddAPI As JsonRESTAPIInfo

    ''' <summary>
    ''' 更新数据的API信息
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property UpdateAPI As JsonRESTAPIInfo

    ''' <summary>
    ''' 删除行的API信息
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property RemoveAPI As JsonRESTAPIInfo

    ''' <summary>
    ''' 拉取数据的API信息
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property FindAPI As JsonRESTAPIInfo

    ''' <summary>
    ''' 拉取数据的API信息
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property GetCountAPI As JsonRESTAPIInfo

    ''' <summary>
    ''' 推送数据完成回调函数
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property PushFinishedCallback As Action

    ''' <summary>
    ''' Model对象
    ''' </summary>
    ''' <returns></returns>
    <Description("Model对象"), Category("FrontWork")>
    Public Property Model As IModel
        Get
            Return Me._model
        End Get
        Set(value As IModel)
            If Me._model IsNot Nothing Then
                'TODO 保存数据
                Call Me.UnbindModel()
            End If
            Me._model = value
            If Me._model IsNot Nothing Then
                Call Me.BindModel()
            End If
        End Set
    End Property

    ''' <summary>
    ''' 配置中心对象
    ''' </summary>
    ''' <returns></returns>
    <Description("配置中心对象"), Category("FrontWork")>
    Public Property Configuration As Configuration Implements ISynchronizer.Configuration
        Get
            Return Me._configuration
        End Get
        Set(value As Configuration)
            If Me._configuration IsNot Nothing Then
                RemoveHandler Me._configuration.Refreshed, AddressOf Me.ConfigurationRefreshedEvent
            End If
            Me._configuration = value
            If Me._configuration IsNot Nothing Then
                AddHandler Me._configuration.Refreshed, AddressOf Me.ConfigurationRefreshedEvent
            End If
            Call Me.ConfigurationRefreshedEvent(Me, New EventArgs)
        End Set
    End Property

    ''' <summary>
    ''' 当前配置模式
    ''' </summary>
    ''' <returns></returns>
    <Description("当前配置模式"), Category("FrontWork")>
    Public Property Mode As String Implements ISynchronizer.Mode
        Get
            Return Me._mode
        End Get
        Set(value As String)
            Me._mode = value
            Call Me.ConfigurationRefreshedEvent(Me, Nothing)
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub SetRequestParameter(name As String, value As Object, Optional mode As String = "default")
        '如果目标模式正是当前模式，则对各API设置请求参数
        If mode = Me.Mode Then
            Me.FindAPI?.SetRequestParameter(name, value)
            Me.AddAPI?.SetRequestParameter(name, value)
            Me.RemoveAPI?.SetRequestParameter(name, value)
            Me.UpdateAPI?.SetRequestParameter(name, value)
            Me.GetCountAPI?.SetRequestParameter(name, value)
        End If

        '添加到参数记录中，以备切换模式时还能重新设置参数
        Dim foundModeParams = (From mp In Me.RequestParams
                               Where mp.Mode.Equals(mode, StringComparison.OrdinalIgnoreCase)
                               Select mp).FirstOrDefault
        If foundModeParams Is Nothing Then
            Me.RequestParams.Add(New ModeParams With {
                .Mode = mode,
                .Params = New Dictionary(Of String, Object) From {
                    {name, value}
                }
            })
        ElseIf Not foundModeParams.Params.ContainsKey(name) Then
            foundModeParams.Params.Add(name, value)
        Else
            foundModeParams.Params(name) = value
        End If
    End Sub

    Public Sub SetJsonRequestParameter(name As String, jsonValue As String, Optional mode As String = "default")
        '如果目标模式正是当前模式，则对各API设置请求参数
        If mode = Me.Mode Then
            Me.FindAPI?.SetJsonRequestParameter(name, jsonValue)
            Me.AddAPI?.SetJsonRequestParameter(name, jsonValue)
            Me.RemoveAPI?.SetJsonRequestParameter(name, jsonValue)
            Me.UpdateAPI?.SetJsonRequestParameter(name, jsonValue)
            Me.GetCountAPI?.SetJsonRequestParameter(name, jsonValue)
        End If

        '添加到参数记录中，以备切换模式时还能重新设置参数
        Dim foundModeParams = (From mp In Me.RequestParams
                               Where mp.Mode.Equals(mode, StringComparison.OrdinalIgnoreCase)
                               Select mp).FirstOrDefault
        If foundModeParams Is Nothing Then
            Me.RequestParams.Add(New ModeParams With {
                .Mode = mode,
                .Params = New Dictionary(Of String, Object) From {
                    {name, jsonValue}
                }
            })
        Else
            foundModeParams.Params.Add(name, jsonValue)
        End If
    End Sub

    Private Sub BindModel()
        AddHandler Me.Model.BeforeRowRemove, AddressOf Me.ModelBeforeRowRemoveEvent
    End Sub

    Private Sub UnbindModel()
        RemoveHandler Me.Model.BeforeRowRemove, AddressOf Me.ModelBeforeRowRemoveEvent
    End Sub

    Private Sub InitSynchronizer()
        Dim context As New SynchronizerInvocationContext(Me)
        If Me._configuration Is Nothing Then Return
        Dim httpAPIConfigs = Me._configuration.GetHTTPAPIConfigurations(Me.Mode)
        Dim requestParams = (From mp In Me.RequestParams
                             Where mp.Mode.Equals(Me.Mode, StringComparison.OrdinalIgnoreCase)
                             Select mp).FirstOrDefault
        Dim requestJsonParams = (From mp In Me.JsonRequestParams
                                 Where mp.Mode.Equals(Me.Mode, StringComparison.OrdinalIgnoreCase)
                                 Select mp).FirstOrDefault
        For Each apiConfig In httpAPIConfigs
            If apiConfig.Type.Equals("pushFinishedCallback", StringComparison.OrdinalIgnoreCase) Then
                Me.PushFinishedCallback =
                    Sub()
                        Call apiConfig.Callback?.Invoke(context)
                    End Sub
                Continue For
            End If

            Dim newAPIInfo As New JsonRESTAPIInfo
            If requestParams IsNot Nothing Then
                For Each param In requestParams.Params
                    newAPIInfo.SetRequestParameter(param.Key, param.Value)
                Next
            End If
            If requestJsonParams IsNot Nothing Then
                For Each param In requestJsonParams.Params
                    newAPIInfo.SetJsonRequestParameter(param.Key, param.Value.ToString)
                Next
            End If
            newAPIInfo.URLTemplate = apiConfig.URL
            If Not String.IsNullOrWhiteSpace(apiConfig.Method) Then
                newAPIInfo.HTTPMethod = HTTPMethod.Parse(apiConfig.Method)
            End If
            newAPIInfo.RequestBodyTemplate = apiConfig.RequestBody
            newAPIInfo.ResponseBodyTemplate = apiConfig.ResponseBody
            newAPIInfo.Callback =
                Function(res, ex) As Boolean
                    Dim callBackContext As New SynchronizerInvocationContext(Me)
                    Return apiConfig.Callback?.Invoke(callBackContext)
                End Function
            If apiConfig.Type.Equals("find", StringComparison.OrdinalIgnoreCase) Then
                Me.FindAPI = newAPIInfo
            ElseIf apiConfig.Type.Equals("add", StringComparison.OrdinalIgnoreCase) Then
                Me.AddAPI = newAPIInfo
            ElseIf apiConfig.Type.Equals("update", StringComparison.OrdinalIgnoreCase) Then
                Me.UpdateAPI = newAPIInfo
            ElseIf apiConfig.Type.Equals("remove", StringComparison.OrdinalIgnoreCase) Then
                Me.RemoveAPI = newAPIInfo
            ElseIf apiConfig.Type.Equals("get-count", StringComparison.OrdinalIgnoreCase) Then
                Me.GetCountAPI = newAPIInfo
            End If
        Next
    End Sub

    Private Sub ConfigurationRefreshedEvent(sender As Object, e As EventArgs)
        Call Me.InitSynchronizer()
    End Sub

    Private Sub ModelBeforeRowRemoveEvent(sender As Object, e As ModelBeforeRowRemoveEventArgs)
        If Me.RemoveAPI Is Nothing Then
            Throw New FrontWorkException("Remove API not setted!")
        End If
        '如果是新增的行，不要将删除操作同步到服务器。
        Dim rowDataList = (From rowInfo In e.RemoveRows
                           Where rowInfo.State.SynchronizationState <> SynchronizationState.ADDED _
                           AndAlso rowInfo.State.SynchronizationState <> SynchronizationState.ADDED_UPDATED
                           Select rowInfo.RowData).ToArray
        If rowDataList.Count > 0 Then
            Me.RemoveAPI.SetRequestParameter("$data", rowDataList)
            Dim res = Me.RemoveAPI.Invoke()
            If res.StatusCode = 200 Then
                MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                e.Cancel = True
                Dim message = res.ErrorMessage
                MessageBox.Show("删除失败：" & message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                'TODO 回调函数的参数类型也不对 If Me.RemoveAPI.Callback IsNot Nothing Then
                '    Me.RemoveAPI.Callback.Invoke(ex.Response, ex)
                'End If
            End If
        End If
    End Sub

    Public Function Find() As Boolean Implements ISynchronizer.Find
        Logger.SetMode(LogMode.SYNCHRONIZER)
        If Me.FindAPI Is Nothing Then
            Throw New FrontWorkException("Find API Not set!")
        End If
        Dim res = Me.FindAPI.Invoke
        If res.StatusCode = 200 Then
            Dim responseStr = res.BodyString
            Me.FindAPI.SetResponseParameter("$data")
            Dim data = Me.FindAPI.GetResponseParameters(responseStr, {"$data"})(0)
            If data Is Nothing Then Return False

            '更新Model
            Dim resultList As New List(Of IDictionary(Of String, Object))
            '判断是对象还是对象数组
            If TypeOf (data) Is IDictionary(Of String, Object) Then '如果是对象，则作为数组的第一项
                Dim value = CType(data, IDictionary(Of String, Object))
                resultList.Add(value)
            Else '否则是数组
                Dim valueArray = CType(data, Object())
                For Each value In valueArray
                    resultList.Add(value)
                Next
            End If
            Dim mappedList As New List(Of IDictionary(Of String, Object))
            For Each item In resultList
                Dim curRow = item
                Dim newRow As New Dictionary(Of String, Object)
                For Each kv In curRow
                    Dim mappedModelKey = Me.GetMappedModelFieldName(kv.Key)
                    If Not newRow.ContainsKey(mappedModelKey) Then
                        newRow.Add(mappedModelKey, kv.Value)
                    End If
                Next
                mappedList.Add(newRow)
            Next
            '修改完成后整体触发刷新事件
            Dim selectionRanges As New List(Of Range)
            For Each oriRange In Me.Model.AllSelectionRanges
                '截取选区，如果原选区超过了数据表的范围，则进行截取
                If oriRange.Row >= resultList.Count Then Continue For
                Dim newRow = oriRange.Row
                Dim newCol = oriRange.Column
                Dim newRows = oriRange.Rows
                Dim newCols = oriRange.Columns
                If oriRange.Row + oriRange.Rows > resultList.Count Then
                    newRows = resultList.Count - newRow
                End If
                'If oriRange.Column + oriRange.Columns > dataTable.Columns.Count Then
                '    newCols = dataTable.Columns.Count - newCol
                'End If
                selectionRanges.Add(New Range(newRow, newCol, newRows, newCols))
            Next
            '如果实在没有选区了，就自动选第一行第一列
            If selectionRanges.Count = 0 AndAlso resultList.Count > 0 Then
                selectionRanges.Add(New Range(0, 0, 1, 1))
            End If
            Call Me.Model.Refresh(New ModelRefreshArgs(mappedList.ToArray, selectionRanges.ToArray))

            'TODO 回调参数的类型也不对 Call Me.FindAPI.Callback?.Invoke(response, Nothing)
        Else
            ' TODO 回调参数的类型也不对 Call Me.FindAPI.Callback?.Invoke(CType(ex.Response, HttpWebResponse), ex)
            Dim message = res.ErrorMessage
            MessageBox.Show("查询失败：" & message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        Return True
    End Function

    Public Function Save() As Boolean Implements ISynchronizer.Save
        Logger.SetMode(LogMode.SYNCHRONIZER)
        If Me.Model Is Nothing Then
            Throw New FrontWorkException("Model not set!")
        End If
        '获取焦点以触发所有视图的编辑完成保存
        '防止最后一个编辑的单元格不能保存
        Call Util.FindFirstVisibleParent(Me)?.Focus()
        If Me.Model.HasErrorCell Then
            MessageBox.Show("请正确填写所有信息再进行保存！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        Dim addedData As New List(Of IDictionary(Of String, Object))
        Dim addedRows As New List(Of Integer)
        Dim updatedData As New List(Of IDictionary(Of String, Object))
        Dim updatdRows As New List(Of Integer)
        For row = 0 To Me.Model.RowCount - 1
            Dim syncState = Me.Model.GetRowSynchronizationState(row)
            If syncState = SynchronizationState.SYNCHRONIZED Then Continue For
            Dim rowData = Me.ModelRowToAPIDictionary(Me.Model.GetRow(row))
            Select Case syncState
                Case SynchronizationState.ADDED_UPDATED
                    addedData.Add(rowData)
                    addedRows.Add(row)
                Case SynchronizationState.UPDATED
                    updatedData.Add(rowData)
                    updatdRows.Add(row)
            End Select
        Next

        If addedData.Count > 0 Then
            Call Me.AddAPI.SetRequestParameter("$data", addedData.ToArray)
            Dim res = Me.AddAPI.Invoke()
            If res.StatusCode = 200 Then
                '将相应行的同步状态更新为已同步
                Me.Model.UpdateRowStates(addedRows.ToArray, Util.Times(New ModelRowState With {.SynchronizationState = SynchronizationState.SYNCHRONIZED}, addedRows.Count))
            Else
                Dim message = res.ErrorMessage
                MessageBox.Show($"保存失败：" & message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If
        End If

        If updatedData.Count > 0 Then
            Call Me.UpdateAPI.SetRequestParameter("$data", updatedData.ToArray)
            Dim res = Me.UpdateAPI.Invoke()
            If res.StatusCode = 200 Then
                '将相应行的同步状态更新为已同步
                Me.Model.UpdateRowStates(updatdRows.ToArray, Util.Times(New ModelRowState With {.SynchronizationState = SynchronizationState.SYNCHRONIZED}, updatdRows.Count))
            Else
                Dim message = res.ErrorMessage
                MessageBox.Show($"保存失败：" & message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If
        End If

            Call Me.Model.RemoveUneditedNewRows()
        MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Return True
    End Function

    Private Function ModelRowToAPIDictionary(dataRow As IDictionary(Of String, Object)) As IDictionary(Of String, Object)
        Dim result As New Dictionary(Of String, Object)
        For Each colAndValue In dataRow
            result.Add(Me.GetMappedAPIFieldName(colAndValue.Key), colAndValue.Value)
        Next
        Return result
    End Function

    Private Function GetMappedModelFieldName(apiFieldName As String) As String
        If String.IsNullOrWhiteSpace(apiFieldName) Then
            Throw New FrontWorkException($"{Me.Name}: FieldMapping APIFieldName cannot be empty!")
        End If
        For Each fieldMappingItem In Me.FieldMapping
            If fieldMappingItem.APIFieldName?.Equals(apiFieldName, StringComparison.OrdinalIgnoreCase) Then
                Return fieldMappingItem.ModelFieldName
            End If
        Next
        Return apiFieldName
    End Function

    Private Function GetMappedAPIFieldName(modelFieldName As String) As String
        If String.IsNullOrWhiteSpace(modelFieldName) Then
            Throw New FrontWorkException($"{Me.Name}: FieldMapping ModelFieldName cannot be empty!")
        End If
        For Each fieldMappingItem In Me.FieldMapping
            If fieldMappingItem.ModelFieldName?.Equals(modelFieldName, StringComparison.OrdinalIgnoreCase) Then
                Return fieldMappingItem.APIFieldName
            End If
        Next
        Return modelFieldName
    End Function

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(JsonRESTSynchronizer))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.PictureBox1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label2, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(180, 180)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = CType(resources.GetObject("PictureBox1.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(180, 120)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.0!)
        Me.Label1.Location = New System.Drawing.Point(3, 150)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(174, 30)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Synchronizer"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label2.Font = New System.Drawing.Font("Microsoft YaHei UI", 8.0!)
        Me.Label2.Location = New System.Drawing.Point(0, 120)
        Me.Label2.Margin = New System.Windows.Forms.Padding(0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(180, 30)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "REST&&JSON"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'JsonRESTSynchronizer
        '
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Margin = New System.Windows.Forms.Padding(0)
        Me.Name = "JsonRESTSynchronizer"
        Me.Size = New System.Drawing.Size(180, 180)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Private Sub JsonRESTSynchronizer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Me.DesignMode Then Me.Visible = False
        Call Me.InitializeComponent()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Class ModeParams
        Public Property Mode As String
        Public Property Params As Dictionary(Of String, Object)
    End Class

    <TypeConverter(GetType(FieldMappingItem.FieldMappingItemTypeConverter))>
    Public Class FieldMappingItem
        <Description("API的字段名称")>
        Public Property APIFieldName As String = Nothing
        <Description("Model中的字段名称")>
        Public Property ModelFieldName As String = Nothing

        Friend Class FieldMappingItemTypeConverter
            Inherits TypeConverter

            Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
                Dim pair = CType(value, FieldMappingItem)
                If destinationType = GetType(String) Then
                    Return $"{pair.APIFieldName} => {pair.ModelFieldName}"
                End If
                Return MyBase.ConvertTo(context, culture, value, destinationType)
            End Function
        End Class
    End Class
End Class

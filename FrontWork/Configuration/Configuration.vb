Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Globalization
Imports System.Linq
Imports System.Reflection
Imports Jint.Native

''' <summary>
''' 配置中心，集中存储一组组件的配置信息
''' </summary>
Public Class Configuration
    Inherits UserControl

    Public Event BeforeFieldAdd As EventHandler(Of BeforeConfigurationFieldAddEventArgs)
    Public Event BeforeFieldUpdate As EventHandler(Of BeforeConfigurationFieldUpdateEventArgs)
    Public Event BeforeFieldRemove As EventHandler(Of BeforeConfigurationFieldRemoveEventArgs)
    Public Event FieldAdded As EventHandler(Of ConfigurationFieldAddedEventArgs)
    Public Event FieldUpdated As EventHandler(Of ConfigurationFieldUpdatedEventArgs)
    Public Event FieldRemoved As EventHandler(Of ConfigurationFieldRemovedEventArgs)
    Public Event Refreshed As EventHandler(Of ConfigurationRefreshedEventArgs)

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label1 As Label

    Private _configurationString As String
    Private _methodListeners As ModeMethodListenerNamesPair() = {}
    Private jsEngine As New Jint.Engine

    Public Sub New()
        jsEngine.SetValue("log", New Action(Of Object)(AddressOf Console.WriteLine))
    End Sub

    ''' <summary>
    ''' 配置模式
    ''' </summary>
    ''' <returns></returns>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property ModeConfigurations As New List(Of ModeConfiguration) From {New ModeConfiguration() With {.Mode = "default"}}

    ''' <summary>
    ''' 配置字符串，Json格式。读取后自动分析转换为Configuration对象
    ''' </summary>
    ''' <returns></returns>
    <Description("配置字符串"), Category("FrontWork")>
    <Editor(GetType(ConfigurationEditor), GetType(UITypeEditor))>
    Public Property ConfigurationString As String
        Get
            Return Me._configurationString
        End Get
        Set(value As String)
            Me._configurationString = value
            Me.Configurate(Me._configurationString)
            If Me.MethodListeners.Length > 0 Then
                For Each modeMethodListeners In Me._methodListeners
                    Call Me.SetMethodListener(modeMethodListeners.MethodListenerNames, modeMethodListeners.Mode)
                Next
            End If
            RaiseEvent Refreshed(Me, New ConfigurationRefreshedEventArgs)
        End Set
    End Property

    <Description("方法监听器"), Category("FrontWork")>
    <Editor(GetType(Design.ArrayEditor), GetType(UITypeEditor))>
    Public Property MethodListeners As ModeMethodListenerNamesPair()
        Get
            Return Me._methodListeners
        End Get
        Set(value As ModeMethodListenerNamesPair())
            Me._methodListeners = value
            If Me.DesignMode Then Return '设计器模式不要注册方法监听器
            If Me.ModeConfigurations.Count > 0 Then
                For Each modeMethodListeners In Me._methodListeners
                    Call Me.SetMethodListener(modeMethodListeners.MethodListenerNames, modeMethodListeners.Mode)
                Next
            End If
        End Set
    End Property

    Private Sub BindModeConfiguration(modeConfiguration As ModeConfiguration)
        AddHandler modeConfiguration.BeforeFieldAdd, AddressOf RaiseBeforeFieldAddEvent
        AddHandler modeConfiguration.BeforeFieldUpdate, AddressOf RaiseBeforeFieldUpdateEvent
        AddHandler modeConfiguration.BeforeFieldRemove, AddressOf RaiseBeforeFieldRemoveEvent
        AddHandler modeConfiguration.FieldAdded, AddressOf RaiseFieldAddedEvent
        AddHandler modeConfiguration.FieldUpdated, AddressOf RaiseFieldUpdatedEvent
        AddHandler modeConfiguration.FieldRemoved, AddressOf RaiseFieldRemovedEvent
    End Sub

    Private Sub UnbindModeConfiguration(modeConfiguration As ModeConfiguration)
        RemoveHandler modeConfiguration.BeforeFieldAdd, AddressOf RaiseBeforeFieldAddEvent
        RemoveHandler modeConfiguration.BeforeFieldUpdate, AddressOf RaiseBeforeFieldUpdateEvent
        RemoveHandler modeConfiguration.BeforeFieldRemove, AddressOf RaiseBeforeFieldRemoveEvent
        RemoveHandler modeConfiguration.FieldAdded, AddressOf RaiseFieldAddedEvent
        RemoveHandler modeConfiguration.FieldUpdated, AddressOf RaiseFieldUpdatedEvent
        RemoveHandler modeConfiguration.FieldRemoved, AddressOf RaiseFieldRemovedEvent
    End Sub

    ''' <summary>
    ''' 当前的配置信息是否包含某种模式
    ''' </summary>
    ''' <param name="mode">模式名称</param>
    ''' <returns>是否包含模式</returns>
    Public Function ContainsMode(mode As String) As Boolean
        Dim foundConfiguration = (From m In Me.modeConfigurations Where m.Mode = mode Select m).FirstOrDefault
        If foundConfiguration IsNot Nothing Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' 设置方法监听器，用来执行配置信息中所指定的字段对应的的本地函数名
    ''' </summary>
    ''' <param name="methodListenerNames">方法监听器类名</param>
    ''' <param name="mode">设置到的模式</param>
    Public Sub SetMethodListener(methodListenerNames As String(), mode As String)
        If Me.DesignMode Then Return '设计器设计的时候就不用绑方法监听器了
        Dim foundModeConfiguration = (From m In Me.modeConfigurations Where m.Mode.Equals(mode, StringComparison.OrdinalIgnoreCase) Select m).FirstOrDefault
        If foundModeConfiguration Is Nothing Then
            throw new FrontWorkException($"mode ""{mode}"" not found!")
            Return
        End If

        foundModeConfiguration.MethodListenerNames = methodListenerNames
    End Sub

    ''' <summary>
    ''' 获取当前模式的字段配置
    ''' </summary>
    ''' <returns>字段的配置信息</returns>
    Public Function GetFields(mode As String) As Field()
        Dim foundModeConfiguration = (From m In ModeConfigurations Where m.Mode = mode Select m).FirstOrDefault
        If foundModeConfiguration Is Nothing Then
            Throw New FrontWorkException($"Mode ""{mode}"" not found!")
        Else
            Return foundModeConfiguration.Fields.ToArray
        End If
    End Function

    Private Function GetModeCofiguration(mode As String) As ModeConfiguration
        Dim foundConfiguration = (From c In Me.ModeConfigurations Where c.Mode = mode).FirstOrDefault
        If foundConfiguration Is Nothing Then
            Throw New ModeNotFoundException($"Mode {mode} not found!")
        End If
        Return foundConfiguration
    End Function

    ''' <summary>
    ''' 获取当前模式的字段配置
    ''' </summary>
    ''' <returns>字段的配置信息</returns>
    Public Function GetField(mode As String, fieldName As String) As Field
        Dim fields = Me.GetFields(mode)
        Dim field = (From f In fields Where f.Name.GetValue?.ToString.Equals(fieldName, StringComparison.OrdinalIgnoreCase) Select f).FirstOrDefault
        If field Is Nothing Then Throw New FrontWorkException($"Field ""{fieldName}"" not found in mode ""{mode}""!")
        Return field
    End Function

    Public Function AddFields(mode As String, fields As Field())
        Return Me.GetModeCofiguration(mode).AddFields(fields)
    End Function

    Public Function AddField(mode As String, field As Field)
        Return Me.GetModeCofiguration(mode).AddFields({field})
    End Function

    Public Function InsertFields(mode As String, indexes As Integer(), fields As Field())
        Return Me.GetModeCofiguration(mode).InsertFields(indexes, fields)
    End Function

    Public Function InsertField(mode As String, index As Integer, field As Field)
        Return Me.InsertFields(mode, {index}, {field})
    End Function

    Public Function UpdateFields(mode As String, indexes As Integer(), fields As Field())
        Return Me.GetModeCofiguration(mode).UpdateFields(indexes, fields)
    End Function

    Public Function UpdateField(mode As String, index As Integer, field As Field)
        Return Me.UpdateFields(mode, {index}, {field})
    End Function

    Public Function RemoveFields(mode As String, indexes As Integer())
        Return Me.GetModeCofiguration(mode).RemoveFields(indexes)
    End Function

    Public Function RemoveField(mode As String, index As Integer)
        Return Me.RemoveFields(mode, {index})
    End Function

    Public Function ClearFields(mode As String) As Boolean
        Return Me.GetModeCofiguration(mode).ClearFields
    End Function

    ''' <summary>
    ''' 获取当前模式的HTTPAPIs的配置信息
    ''' </summary>
    ''' <returns>HTTPAPIs配置信息</returns>
    Public Function GetHTTPAPIConfigurations(mode As String) As HTTPAPI()
        Dim foundModeConfiguration = (From m In ModeConfigurations Where m.Mode = mode Select m).FirstOrDefault
        If foundModeConfiguration Is Nothing Then
            Return {}
        Else
            Return foundModeConfiguration.HTTPAPIs.ToArray
        End If
    End Function

    ''' <summary>
    ''' 配置，输入json字符串进行分析并配置为json所描述的配置
    ''' </summary>
    ''' <param name="jsonStr">json配置字符串</param>
    Public Sub Configurate(jsonStr As String)
        If String.IsNullOrWhiteSpace(jsonStr) Then Return
        Dim jsValue As JsValue = Nothing
        Try
            jsValue = jsEngine.Execute("$_FWJsonResult = " + jsonStr).GetValue("$_FWJsonResult")
        Catch ex As Exception
            If Me.DesignMode Then
                MessageBox.Show($"配置中心的配置字符串错误，请检查" & vbCrLf & ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            Throw New FrontWorkException("ConfigurationString error: " + ex.Message)
        End Try
        '为旧的ModeConfigurations解绑事件
        Dim oldModeCondigurations = Me.ModeConfigurations
        If oldModeCondigurations?.Count > 0 Then
            For Each oldModeConfiguration In oldModeCondigurations
                Call Me.UnbindModeConfiguration(oldModeConfiguration)
            Next
        End If
        Dim newModeConfigurations = ModeConfiguration.FromJsValue(Me.MethodListeners, jsValue)
        If newModeConfigurations Is Nothing Then Return
        '为新的ModeConfigurations绑定事件
        For Each newModeConfiguration In newModeConfigurations
            Call Me.BindModeConfiguration(newModeConfiguration)
        Next
        Me.ModeConfigurations.Clear()
        Me.ModeConfigurations.AddRange(newModeConfigurations)
    End Sub

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Configuration))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.PictureBox1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
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
        Me.PictureBox1.Size = New System.Drawing.Size(180, 140)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.5!)
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(0, 140)
        Me.Label1.Margin = New System.Windows.Forms.Padding(0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(180, 40)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Config"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Configuration
        '
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.DoubleBuffered = True
        Me.Name = "Configuration"
        Me.Size = New System.Drawing.Size(180, 180)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Private Sub Configuration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Me.DesignMode Then Me.Visible = False
        Call InitializeComponent()
    End Sub

    Public Sub RaiseBeforeFieldAddEvent(sender As Object, eventArgs As BeforeConfigurationFieldAddEventArgs)
        RaiseEvent BeforeFieldAdd(sender, eventArgs)
    End Sub

    Public Sub RaiseBeforeFieldUpdateEvent(sender As Object, eventArgs As BeforeConfigurationFieldUpdateEventArgs)
        RaiseEvent BeforeFieldUpdate(sender, eventArgs)
    End Sub

    Public Sub RaiseBeforeFieldRemoveEvent(sender As Object, eventArgs As BeforeConfigurationFieldRemoveEventArgs)
        RaiseEvent BeforeFieldRemove(sender, eventArgs)
    End Sub

    Public Sub RaiseFieldAddedEvent(sender As Object, eventArgs As ConfigurationFieldAddedEventArgs)
        RaiseEvent FieldAdded(sender, eventArgs)
    End Sub

    Public Sub RaiseFieldUpdatedEvent(sender As Object, eventArgs As ConfigurationFieldUpdatedEventArgs)
        RaiseEvent FieldUpdated(sender, eventArgs)
    End Sub

    Public Sub RaiseFieldRemovedEvent(sender As Object, eventArgs As ConfigurationFieldRemovedEventArgs)
        RaiseEvent FieldRemoved(sender, eventArgs)
    End Sub

    Public Sub RaiseRefreshedEvent(sender As Object, eventArgs As ConfigurationRefreshedEventArgs)
        RaiseEvent Refreshed(sender, eventArgs)
    End Sub
End Class

<TypeConverter(GetType(ModeMethodListenerNamesPair.ModeMethodListenerPairTypeConverter))>
Public Class ModeMethodListenerNamesPair

    <Description("方法监听器")>
    Public Property MethodListenerNames As String() = {}

    <Description("要应用该方法监听器的模式")>
    Public Property Mode As String

    Friend Class ModeMethodListenerPairTypeConverter
        Inherits TypeConverter

        Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
            Dim pair = CType(value, ModeMethodListenerNamesPair)
            If destinationType = GetType(String) Then
                Return $"{pair.Mode} => {pair.MethodListenerNames.ToString}"
            End If
            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function
    End Class
End Class
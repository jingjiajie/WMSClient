Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Web.Script.Serialization
Imports Jint.Native

''' <summary>
''' SearchView和JsonRESTSynchronizer的适配器
''' </summary>
Public Class SearchViewJsonRESTAdapter
    Inherits UserControl

    Private _synchronizer As JsonRESTSynchronizer
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label3 As Label
    Friend WithEvents LabelAdapterName As Label
    Friend WithEvents Label1 As Label
    Private _searchView As SearchView

    ''' <summary>
    ''' JsonRESTSynchronizer对象
    ''' </summary>
    ''' <returns></returns>
    <Description("JsonREST同步器对象"), Category("FrontWork")>
    Public Property Synchronizer As JsonRESTSynchronizer
        Get
            Return Me._synchronizer
        End Get
        Set(value As JsonRESTSynchronizer)
            Me._synchronizer = value
        End Set
    End Property

    ''' <summary>
    ''' 搜索视图对象
    ''' </summary>
    ''' <returns></returns>
    <Description("搜索视图（SearchView）对象"), Category("FrontWork")>
    Public Property SearchView As SearchView
        Get
            Return Me._searchView
        End Get
        Set(value As SearchView)
            If Me._searchView IsNot Nothing Then
                RemoveHandler Me._searchView.OnSearch, AddressOf Me.SearchViewOnSearch
            End If
            Me._searchView = value
            If Me.SearchView IsNot Nothing Then
                AddHandler Me._searchView.OnSearch, AddressOf Me.SearchViewOnSearch
            End If
        End Set
    End Property

    <Description("生成的API中各个字段的名称"), Category("FrontWork")>
    Public Property APIFieldNames As APIParamNamesType = New APIParamNamesType

    Public Sub New()
        If Not Me.DesignMode Then Me.Visible = False
        Call Me.InitializeComponent()
    End Sub

    Private Function SynchronizerPullCallback(res As HttpWebResponse, ex As WebException) As Boolean
        If res IsNot Nothing AndAlso res.StatusCode = 200 Then Return True
        If res IsNot Nothing Then
            Dim responseBodyReader = New StreamReader(res.GetResponseStream)
            Dim responseBody = responseBodyReader.ReadToEnd
            MessageBox.Show(responseBody, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf ex IsNot Nothing Then
            MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
        Return False
    End Function

    Protected Sub SetConditionAndOrdersToAPI(api As JsonRESTAPIInfo, args As OnSearchEventArgs)
        Static jsEngine As New Jint.Engine
        Static jsSerializer = New JsonSerializer
        '添加搜索条件
        Dim conditions = args.Conditions
        jsEngine.Execute("var $conditions = [];")
        If conditions IsNot Nothing Then
            For Each condition In conditions
                Dim key = condition.Key
                Dim relation = condition.Relation
                Dim values = condition.Values
                For i = 0 To values.Length - 1
                    values(i) = Util.UrlEncode(values(i))
                Next
                Dim jsonValues = jsSerializer.Serialize(values)

                Dim fieldKey = Me.APIFieldNames.ConditionParamNames.Key
                Dim fieldRelation = Me.APIFieldNames.ConditionParamNames.Relation
                Dim fieldValues = Me.APIFieldNames.ConditionParamNames.Values
                jsEngine.Execute($"$conditions.push({{""{fieldKey}"":""{key}"",""{fieldRelation}"":""{relation}"",""{fieldValues}"":{jsonValues} }});")
            Next
        End If

        Dim orders = args.Orders
        jsEngine.Execute("var $orders = [];")
        If orders IsNot Nothing Then
            For Each orderItem In orders
                Dim key = orderItem.Key
                Dim order = orderItem.Order

                Dim fieldKey = Me.APIFieldNames.OrderParamNames.Key
                Dim fieldOrder = Me.APIFieldNames.OrderParamNames.Order
                jsEngine.Execute($"$orders.push({{""{fieldKey}"":""{key}"",""{fieldOrder}"":""{order}"" }});")
            Next
        End If

        api.SetRequestParameter("$conditions", jsEngine.GetValue("$conditions"))
        api.SetRequestParameter("$orders", jsEngine.GetValue("$orders"))
    End Sub

    Protected Overridable Sub SearchViewOnSearch(sender As Object, args As OnSearchEventArgs)
        If Me.Synchronizer.FindAPI Is Nothing Then
            Throw New FrontWorkException("FindAPI not set!")
        End If
        Call Me.Search(args)
    End Sub

    Protected Overridable Function Search(args As OnSearchEventArgs) As Boolean
        Me.SetConditionAndOrdersToAPI(Me.Synchronizer.FindAPI, args)
        Return Me.Synchronizer.Find()
    End Function

    Protected Overridable Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SearchViewJsonRESTAdapter))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.LabelAdapterName = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Label3, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.PictureBox1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.LabelAdapterName, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 2)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 4
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(180, 180)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label3.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Label3.Location = New System.Drawing.Point(0, 150)
        Me.Label3.Margin = New System.Windows.Forms.Padding(0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(180, 30)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Adapter"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = CType(resources.GetObject("PictureBox1.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(180, 90)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'LabelAdapterName
        '
        Me.LabelAdapterName.AutoSize = True
        Me.LabelAdapterName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelAdapterName.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.LabelAdapterName.Location = New System.Drawing.Point(0, 90)
        Me.LabelAdapterName.Margin = New System.Windows.Forms.Padding(0)
        Me.LabelAdapterName.Name = "LabelAdapterName"
        Me.LabelAdapterName.Size = New System.Drawing.Size(180, 30)
        Me.LabelAdapterName.TabIndex = 1
        Me.LabelAdapterName.Text = "SearchView" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.LabelAdapterName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Font = New System.Drawing.Font("Microsoft YaHei UI", 8.5!)
        Me.Label1.Location = New System.Drawing.Point(0, 120)
        Me.Label1.Margin = New System.Windows.Forms.Padding(0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(180, 30)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Synchronizer"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SearchViewJsonRESTAdapter
        '
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Name = "SearchViewJsonRESTAdapter"
        Me.Size = New System.Drawing.Size(180, 180)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Private Sub SearchViewJsonRESTAdapter_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub


    '===============================================
    <TypeConverter(GetType(APIParamNamesType.APIFieldNamesTypeConverter))>
    Public Class APIParamNamesType
        Public Property ConditionParamNames As New ConditionFieldNamesType
        Public Property OrderParamNames As New OrderParamNamesType

        Friend Class APIFieldNamesTypeConverter
            Inherits TypeConverter

            Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
                If destinationType = GetType(String) Then
                    Return "API parameter names"
                End If
                Return MyBase.ConvertTo(context, culture, value, destinationType)
            End Function

            Public Overrides Function GetPropertiesSupported(context As ITypeDescriptorContext) As Boolean
                Return True
            End Function

            Public Overrides Function GetProperties(context As ITypeDescriptorContext, value As Object, attributes() As Attribute) As PropertyDescriptorCollection
                Return TypeDescriptor.GetProperties(GetType(APIParamNamesType), attributes)
            End Function
        End Class
    End Class

    <TypeConverter(GetType(ConditionFieldNamesType.ConditionParamNamesTypeConverter))>
    Public Class ConditionFieldNamesType
        Public Property Key As String = "key"
        Public Property Relation As String = "relation"
        Public Property Values As String = "values"

        Friend Class ConditionParamNamesTypeConverter
            Inherits TypeConverter

            Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
                If destinationType = GetType(String) Then
                    Return "Condition parameter names"
                End If
                Return MyBase.ConvertTo(context, culture, value, destinationType)
            End Function

            Public Overrides Function GetPropertiesSupported(context As ITypeDescriptorContext) As Boolean
                Return True
            End Function

            Public Overrides Function GetProperties(context As ITypeDescriptorContext, value As Object, attributes() As Attribute) As PropertyDescriptorCollection
                Return TypeDescriptor.GetProperties(GetType(ConditionFieldNamesType), attributes)
            End Function
        End Class
    End Class

    <TypeConverter(GetType(OrderParamNamesType.OrderFieldNamesTypeConverter))>
    Public Class OrderParamNamesType
        Public Property Key As String = "key"
        Public Property Order As String = "order"

        Public Class OrderFieldNamesTypeConverter
            Inherits TypeConverter

            Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
                If destinationType = GetType(String) Then
                    Return "Order parameter names"
                End If
                Return MyBase.ConvertTo(context, culture, value, destinationType)
            End Function

            Public Overrides Function GetPropertiesSupported(context As ITypeDescriptorContext) As Boolean
                Return True
            End Function

            Public Overrides Function GetProperties(context As ITypeDescriptorContext, value As Object, attributes() As Attribute) As PropertyDescriptorCollection
                Return TypeDescriptor.GetProperties(GetType(OrderParamNamesType), attributes)
            End Function
        End Class
    End Class
End Class
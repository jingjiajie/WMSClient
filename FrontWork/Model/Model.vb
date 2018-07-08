Imports System.ComponentModel
Imports System.Linq
Imports System.Reflection
Imports FrontWork

''' <summary>
''' 模型类
''' </summary>
Partial Public Class Model
    Inherits UserControl
    Implements IConfigurableModel

    Public WithEvents TableLayoutPanel1 As TableLayoutPanel
    Public WithEvents PanelIcon As Panel
    Public WithEvents LabelText As Label

    Protected Property ModelConfigurationWrapper As ModelConfigurationWrapper

    Public Shadows Property Name As String
        Get
            Return MyBase.Name
        End Get
        Set(value As String)
            Me.ModelConfigurationWrapper.Name = value
            MyBase.Name = value
        End Set
    End Property

    ''' <summary>
    ''' 配置中心对象
    ''' </summary>
    ''' <returns></returns>
    <Description("配置中心对象"), Category("FrontWork")>
    Public Property Configuration As Configuration Implements IConfigurableModel.Configuration
        Get
            Return Me.ModelConfigurationWrapper.Configuration
        End Get
        Set(value As Configuration)
            Me.ModelConfigurationWrapper.Configuration = value
        End Set
    End Property

    ''' <summary>
    ''' 数据行数
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    Public ReadOnly Property RowCount As Integer
        Get
            Return Me.ModelConfigurationWrapper.GetRowCount
        End Get
    End Property

    Public Function GetRowCount() As Integer Implements IModel.GetRowCount
        Return Me.ModelConfigurationWrapper.GetRowCount
    End Function

    <Browsable(False)>
    Public ReadOnly Property ColumnCount As Integer
        Get
            Return Me.GetColumnCount
        End Get
    End Property

    Public Function GetColumnCount() As Integer Implements IModel.GetColumnCount
        Return Me.ModelConfigurationWrapper.GetColumnCount
    End Function

    Public Function GetSelectionRanges() As Range() Implements IModel.GetSelectionRanges
        Return Me.ModelConfigurationWrapper.GetSelectionRanges
    End Function

    Public Sub SetSelectionRanges(ranges As Range()) Implements IModel.SetSelectionRanges
        Call Me.ModelConfigurationWrapper.SetSelectionRanges(ranges)
    End Sub

    ''' <summary>
    ''' 选区
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    Public Property AllSelectionRanges As Range()
        Get
            Return Me.GetSelectionRanges
        End Get
        Set(value As Range())
            Me.SetSelectionRanges(value)
        End Set
    End Property

    ''' <summary>
    ''' 选区
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    Public Property AllSelectionRanges(i As Integer) As Range
        Get
            Return Me.GetSelectionRanges(i)
        End Get
        Set(value As Range)
            Dim allSelectionRanges = Me.GetSelectionRanges
            allSelectionRanges(i) = value
            Me.SetSelectionRanges(allSelectionRanges)
        End Set
    End Property

    ''' <summary>
    ''' 首个选区
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    Public Property SelectionRange As Range
        Get
            If Me.AllSelectionRanges Is Nothing Then Return Nothing
            If Me.AllSelectionRanges.Length = 0 Then Return Nothing
            Return Me.AllSelectionRanges(0)
        End Get
        Set(value As Range)
            If value Is Nothing Then
                Me.AllSelectionRanges = {}
            ElseIf Me.AllSelectionRanges Is Nothing Then
                Me.AllSelectionRanges = {value}
            ElseIf Me.AllSelectionRanges.Length = 0 Then
                Me.AllSelectionRanges = {value}
            Else
                Me.AllSelectionRanges(0) = value
            End If
        End Set
    End Property

    Default Public Property _Item(row As Integer) As IDictionary(Of String, Object)
        Get
            Return Me.GetRow(row)
        End Get
        Set(value As IDictionary(Of String, Object))
            Call Me.UpdateRow(row, value)
        End Set
    End Property

    Default Public Property _Item(row As Integer, column As Integer) As Object
        Get
            Return Me.GetCell(row, column)
        End Get
        Set(value As Object)
            Call Me.UpdateCell(row, column, value)
        End Set
    End Property

    Default Public Property _Item(row As Integer, columnName As String) As Object
        Get
            Return Me.GetCell(row, columnName)
        End Get
        Set(value As Object)
            Call Me.UpdateCell(row, columnName, value)
        End Set
    End Property

    ''' <summary>
    ''' 当前模式
    ''' </summary>
    ''' <returns></returns>
    <Description("当前配置模式"), Category("FrontWork")>
    Public Property Mode As String Implements IConfigurableModel.Mode
        Get
            Return Me.ModelConfigurationWrapper.Mode
        End Get
        Set(value As String)
            ModelConfigurationWrapper.Mode = value
        End Set
    End Property

    Public Sub New()
        Me.ModelConfigurationWrapper = New ModelConfigurationWrapper(New ModelCore)
    End Sub

    Public Function GetCell(row As Integer, columnName As String) As Object
        Return Me.ModelConfigurationWrapper.GetCell(row, columnName)
    End Function

    ''' <summary>
    ''' 获取行并自动转换成相应类型的对象返回
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="rows"></param>
    ''' <returns></returns>
    Public Function GetRows(Of T As New)(rows As Integer()) As T()
        Return ModelConfigurationWrapper.GetRows(Of T)(rows)
    End Function

    Public Function GetRow(Of T As New)(row As Integer) As T
        Return Me.ModelConfigurationWrapper.GetRow(Of T)(row)
    End Function

    ''' <summary>
    ''' 获取行
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <returns>相应行数据</returns>
    Public Function GetRows(rows As Integer()) As IDictionary(Of String, Object)() Implements IModel.GetRows
        Return Me.ModelConfigurationWrapper.GetRows(rows)
    End Function

    ''' <summary>
    ''' 获取行
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <returns>相应行数据</returns>
    Public Function GetRow(row As Integer) As IDictionary(Of String, Object)
        Return Me.ModelConfigurationWrapper.GetRow(row)
    End Function

    ''' <summary>
    ''' 增加行
    ''' </summary>
    ''' <param name="data">增加行的数据</param>
    ''' <returns>增加的行号</returns>
    Public Function AddRow(data As IDictionary(Of String, Object)) As Integer
        Return Me.ModelConfigurationWrapper.AddRow(data)
    End Function

    ''' <summary>
    ''' 增加行
    ''' </summary>
    ''' <param name="dataOfEachRow">增加行的数据</param>
    ''' <returns>增加的行号</returns>
    Public Function AddRows(dataOfEachRow As IDictionary(Of String, Object)()) As Integer() Implements IModel.AddRows
        Return Me.ModelConfigurationWrapper.AddRows(dataOfEachRow)
    End Function

    ''' <summary>
    ''' 插入行
    ''' </summary>
    ''' <param name="row">插入行行号</param>
    ''' <param name="data">数据</param>
    Public Sub InsertRow(row As Integer, data As IDictionary(Of String, Object))
        Call Me.ModelConfigurationWrapper.InsertRow(row, data)
    End Sub

    ''' <summary>
    ''' 插入行
    ''' </summary>
    ''' <param name="rows">插入行行号</param>
    ''' <param name="dataOfEachRow">数据</param>
    Public Sub InsertRows(rows As Integer(), dataOfEachRow As IDictionary(Of String, Object)()) Implements IModel.InsertRows
        Call Me.ModelConfigurationWrapper.InsertRows(rows, dataOfEachRow)
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="row">删除行行号</param>
    Public Sub RemoveRow(row As Integer)
        Call Me.ModelConfigurationWrapper.RemoveRow(row)
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="startRow">起始行号</param>
    ''' <param name="rowCount">删除行数</param>
    Public Sub RemoveRows(startRow As Integer, rowCount As Integer)
        Call Me.ModelConfigurationWrapper.RemoveRows(startRow, rowCount)
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="rows">删除行行号</param>
    Public Sub RemoveRows(rows As Integer()) Implements IModel.RemoveRows
        Call Me.ModelConfigurationWrapper.RemoveRows(rows)
    End Sub

    Public Sub RemoveSelectedRows()
        Call Me.ModelConfigurationWrapper.RemoveSelectedRows()
    End Sub


    ''' <summary>
    ''' 更新行
    ''' </summary>
    ''' <param name="row">更新行行号</param>
    ''' <param name="data">数据</param>
    Public Sub UpdateRow(row As Integer, data As IDictionary(Of String, Object))
        Call Me.ModelConfigurationWrapper.UpdateRow(row, data)
    End Sub

    ''' <summary>
    ''' 更新行
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="dataOfEachRow">对应的数据</param>
    Public Sub UpdateRows(rows As Integer(), dataOfEachRow As IDictionary(Of String, Object)()) Implements IModel.UpdateRows
        Call Me.ModelConfigurationWrapper.UpdateRows(rows, dataOfEachRow)
    End Sub

    ''' <summary>
    ''' 更新单元格
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="columnName">列名</param>
    ''' <param name="data">更新的数据</param>
    Public Sub UpdateCell(row As Integer, columnName As String, data As Object)
        Call Me.ModelConfigurationWrapper.UpdateCell(row, columnName, data)
    End Sub

    ''' <summary>
    ''' 更新单元格
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="columnNames">列名</param>
    ''' <param name="dataOfEachCell">相应的数据</param>
    Public Sub UpdateCells(rows As Integer(), columnNames As String(), dataOfEachCell As Object()) Implements IModel.UpdateCells
        Call Me.ModelConfigurationWrapper.UpdateCells(rows, columnNames, dataOfEachCell)
    End Sub

    Public Overloads Sub Refresh(args As ModelRefreshArgs) Implements IModel.Refresh
        Call Me.ModelConfigurationWrapper.Refresh(args)
    End Sub

    Public Sub UpdateRowStates(rows As Integer(), states As ModelRowState()) Implements IModel.UpdateRowStates
        Call Me.ModelConfigurationWrapper.UpdateRowStates(rows, states)
    End Sub

    ''' <summary>
    ''' 更新行同步状态
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="state">同步状态</param>
    Public Sub UpdateRowState(row As Integer, state As ModelRowState)
        Call Me.ModelConfigurationWrapper.UpdateRowState(row, state)
    End Sub

    ''' <summary>
    ''' 获取行状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <returns>状态</returns>
    Public Function GetRowStates(rows As Integer()) As ModelRowState() Implements IModel.GetRowStates
        Return Me.ModelConfigurationWrapper.GetRowStates(rows)
    End Function

    ''' <summary>
    ''' 获取行同步状态
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <returns>同步状态</returns>
    Public Function GetRowState(row As Integer) As ModelRowState
        Return Me.ModelConfigurationWrapper.GetRowState(row)
    End Function

    Public Function GetRowSynchronizationStates(rows As Integer()) As SynchronizationState()
        Dim rowStates = Me.GetRowStates(rows)
        Return (From s In rowStates Select s.SynchronizationState).ToArray
    End Function

    Public Function GetRowSynchronizationState(row As Integer) As SynchronizationState
        Return Me.GetRowSynchronizationStates({row})(0)
    End Function

    ''' <summary>
    ''' Model刷新事件
    ''' </summary>
    Public Custom Event Refreshed As EventHandler(Of ModelRefreshedEventArgs) Implements IModel.Refreshed
        AddHandler(value As EventHandler(Of ModelRefreshedEventArgs))
            AddHandler Me.ModelConfigurationWrapper.Refreshed, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRefreshedEventArgs))
            RemoveHandler Me.ModelConfigurationWrapper.Refreshed, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRefreshedEventArgs)
        End RaiseEvent
    End Event

    ''' <summary>
    ''' 增加行事件
    ''' </summary>
    Public Custom Event RowAdded As EventHandler(Of ModelRowAddedEventArgs) Implements IModel.RowAdded
        AddHandler(value As EventHandler(Of ModelRowAddedEventArgs))
            AddHandler Me.ModelConfigurationWrapper.RowAdded, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRowAddedEventArgs))
            RemoveHandler Me.ModelConfigurationWrapper.RowAdded, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRowAddedEventArgs)

        End RaiseEvent
    End Event

    ''' <summary>
    ''' 更新行数据事件
    ''' </summary>
    Public Custom Event RowUpdated As EventHandler(Of ModelRowUpdatedEventArgs) Implements IModel.RowUpdated
        AddHandler(value As EventHandler(Of ModelRowUpdatedEventArgs))
            AddHandler Me.ModelConfigurationWrapper.RowUpdated, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRowUpdatedEventArgs))
            RemoveHandler Me.ModelConfigurationWrapper.RowUpdated, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRowUpdatedEventArgs)

        End RaiseEvent
    End Event

    ''' <summary>
    ''' 删除行事件
    ''' </summary>
    Public Custom Event BeforeRowRemove As EventHandler(Of ModelBeforeRowRemoveEventArgs) Implements IModel.BeforeRowRemove
        AddHandler(value As EventHandler(Of ModelBeforeRowRemoveEventArgs))
            AddHandler Me.ModelConfigurationWrapper.BeforeRowRemove, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelBeforeRowRemoveEventArgs))
            RemoveHandler Me.ModelConfigurationWrapper.BeforeRowRemove, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelBeforeRowRemoveEventArgs)
        End RaiseEvent
    End Event

    ''' <summary>
    ''' 删除行事件
    ''' </summary>
    Public Custom Event RowRemoved As EventHandler(Of ModelRowRemovedEventArgs) Implements IModel.RowRemoved
        AddHandler(value As EventHandler(Of ModelRowRemovedEventArgs))
            AddHandler Me.ModelConfigurationWrapper.RowRemoved, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRowRemovedEventArgs))
            RemoveHandler Me.ModelConfigurationWrapper.RowRemoved, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRowRemovedEventArgs)

        End RaiseEvent
    End Event

    ''' <summary>
    ''' 单元格数据更新事件
    ''' </summary>
    Public Custom Event CellUpdated As EventHandler(Of ModelCellUpdatedEventArgs) Implements IModel.CellUpdated
        AddHandler(value As EventHandler(Of ModelCellUpdatedEventArgs))
            AddHandler Me.ModelConfigurationWrapper.CellUpdated, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelCellUpdatedEventArgs))
            RemoveHandler Me.ModelConfigurationWrapper.CellUpdated, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelCellUpdatedEventArgs)

        End RaiseEvent
    End Event

    ''' <summary>
    ''' 选区改变事件
    ''' </summary>
    Public Custom Event SelectionRangeChanged As EventHandler(Of ModelSelectionRangeChangedEventArgs) Implements IModel.SelectionRangeChanged
        AddHandler(value As EventHandler(Of ModelSelectionRangeChangedEventArgs))
            AddHandler Me.ModelConfigurationWrapper.SelectionRangeChanged, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelSelectionRangeChangedEventArgs))
            RemoveHandler Me.ModelConfigurationWrapper.SelectionRangeChanged, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelSelectionRangeChangedEventArgs)

        End RaiseEvent
    End Event

    ''' <summary>
    ''' 行同步状态改变事件
    ''' </summary>
    Public Custom Event RowStateChanged As EventHandler(Of ModelRowStateChangedEventArgs) Implements IModel.RowStateChanged
        AddHandler(value As EventHandler(Of ModelRowStateChangedEventArgs))
            AddHandler Me.ModelConfigurationWrapper.RowStateChanged, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRowStateChangedEventArgs))
            RemoveHandler Me.ModelConfigurationWrapper.RowStateChanged, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRowStateChangedEventArgs)

        End RaiseEvent
    End Event

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Model))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelText = New System.Windows.Forms.Label()
        Me.PanelIcon = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.LabelText, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.PanelIcon, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(150, 150)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'LabelText
        '
        Me.LabelText.AutoSize = True
        Me.LabelText.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelText.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.5!)
        Me.LabelText.Location = New System.Drawing.Point(0, 125)
        Me.LabelText.Margin = New System.Windows.Forms.Padding(0)
        Me.LabelText.Name = "LabelText"
        Me.LabelText.Size = New System.Drawing.Size(150, 25)
        Me.LabelText.TabIndex = 1
        Me.LabelText.Text = "Model"
        Me.LabelText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelIcon
        '
        Me.PanelIcon.BackgroundImage = CType(resources.GetObject("PanelIcon.BackgroundImage"), System.Drawing.Image)
        Me.PanelIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PanelIcon.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelIcon.Location = New System.Drawing.Point(0, 0)
        Me.PanelIcon.Margin = New System.Windows.Forms.Padding(0)
        Me.PanelIcon.Name = "PanelIcon"
        Me.PanelIcon.Size = New System.Drawing.Size(150, 125)
        Me.PanelIcon.TabIndex = 2
        '
        'Model
        '
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("宋体", 10.0!)
        Me.Name = "Model"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Private Sub Model_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Me.DesignMode Then Me.Visible = False
        Call Me.InitializeComponent()
    End Sub

    Public Function ContainsColumn(columnName As String) As Boolean
        Return Me.GetColumns({columnName})(0) IsNot Nothing
    End Function

    Public Sub SelectRowsByValues(Of T)(columnName As String, values As T())
        Call Me.ModelConfigurationWrapper.SelectRowsByValues(columnName, values)
    End Sub

    ''' <summary>
    ''' 获取所有选中行
    ''' </summary>
    ''' <typeparam name="T">要映射成的类型</typeparam>
    ''' <returns>选中行映射后的对象数组</returns>
    Public Function GetSelectedRows(Of T As New)() As T()
        Return Me.ModelConfigurationWrapper.GetSelectedRows(Of T)
    End Function

    ''' <summary>
    ''' 获取所有选中行的某一列
    ''' </summary>
    ''' <typeparam name="T">返回类型</typeparam>
    ''' <param name="columnName">列名</param>
    ''' <returns>所有选中行指定列的数据</returns>
    Public Function GetSelectedRows(Of T)(columnName As String) As T()
        Return Me.ModelConfigurationWrapper.GetSelectedRows(Of T)(columnName)
    End Function

    Public Function GetSelectedRows() As IDictionary(Of String, Object)()
        Return Me.ModelConfigurationWrapper.GetSelectedRows
    End Function

    Public Function GetSelectedRow() As IDictionary(Of String, Object)
        Return Me.ModelConfigurationWrapper.GetSelectedRow
    End Function

    Public Function GetSelectedRow(Of T As New)() As IDictionary(Of String, Object)
        Return Me.ModelConfigurationWrapper.GetSelectedRow(Of T)
    End Function

    Public Function GetSelectedRow(Of T)(columnName As String) As T
        Return Me.ModelConfigurationWrapper.GetSelectedRow(Of T)(columnName)
    End Function

    ''' <summary>
    ''' 删除新增但未编辑的行
    ''' </summary>
    Public Sub RemoveUneditedNewRows()
        Call Me.ModelConfigurationWrapper.RemoveUneditedNewRows()
    End Sub

    Public Function HasUnsynchronizedUpdatedRow() As Boolean
        Return Me.ModelConfigurationWrapper.HasUnsynchronizedUpdatedRow
    End Function

    Public Sub AddColumns(columns() As ModelColumn) Implements IModel.AddColumns
        Call Me.ModelConfigurationWrapper.AddColumns(columns)
    End Sub

    Public Sub RemoveColumns(columnNames() As String) Implements IModel.RemoveColumns
        Call Me.ModelConfigurationWrapper.RemoveColumns(columnNames)
    End Sub

    Public Function GetColumns() As ModelColumn() Implements IModel.GetColumns
        Return Me.ModelConfigurationWrapper.GetColumns
    End Function

    Public Function GetColumns(columnNames() As String) As ModelColumn() Implements IModel.GetColumns
        Return Me.ModelConfigurationWrapper.GetColumns(columnNames)
    End Function

    Public Function GetCells(rows() As Integer, columnNames() As String) As Object() Implements IModel.GetCells
        Return Me.ModelConfigurationWrapper.GetCells(rows, columnNames)
    End Function

    Public Function ToDataTable() As DataTable Implements IModel.ToDataTable
        Return Me.ModelConfigurationWrapper.ToDataTable
    End Function

    Public Sub RefreshView(rows As Integer())
        Call Me.ModelConfigurationWrapper.RefreshView(rows)
    End Sub

    Public Sub RefreshView(row As Integer)
        Call Me.ModelConfigurationWrapper.RefreshView(row)
    End Sub

    Public Sub RaiseRefreshedEvent(sender As Object, args As ModelRefreshedEventArgs)
        Call Me.ModelConfigurationWrapper.RaiseRefreshedEvent(sender, args)
    End Sub

    Public Sub RaiseCellUpdatedEvent(sender As Object, args As ModelCellUpdatedEventArgs)
        Call Me.ModelConfigurationWrapper.RaiseCellUpdatedEvent(sender, args)
    End Sub

    Public Sub RaiseRowUpdatedEvent(sender As Object, args As ModelRowUpdatedEventArgs)
        Call Me.ModelConfigurationWrapper.RaiseRowUpdatedEvent(sender, args)
    End Sub

    Public Sub RaiseRowAddedEvent(sender As Object, args As ModelRowAddedEventArgs)
        Call Me.ModelConfigurationWrapper.RaiseRowAddedEvent(sender, args)
    End Sub

    Public Sub RaiseBeforeRowRemoveEvent(sender As Object, args As ModelBeforeRowRemoveEventArgs)
        Call Me.ModelConfigurationWrapper.RaiseBeforeRowRemoveEvent(sender, args)
    End Sub

    Public Sub RaiseRowRemovedEvent(sender As Object, args As ModelRowRemovedEventArgs)
        Call Me.ModelConfigurationWrapper.RaiseRowRemovedEvent(sender, args)
    End Sub

    Public Sub RaiseSelectionRangeChangedEvent(sender As Object, args As ModelSelectionRangeChangedEventArgs)
        Call Me.ModelConfigurationWrapper.RaiseSelectionRangeChangedEvent(sender, args)
    End Sub

    Public Sub RaiseRowSynchronizationStateChangedEvent(sender As Object, args As ModelRowStateChangedEventArgs)
        Call Me.ModelConfigurationWrapper.RaiseRowStateChangedEvent(sender, args)
    End Sub
End Class
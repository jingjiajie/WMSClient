Imports System.ComponentModel
Imports System.Linq
Imports System.Reflection
Imports FrontWork

''' <summary>
''' 模型类
''' </summary>
Partial Public Class Model
    Inherits UserControl
    Implements IModelCore

    Public WithEvents TableLayoutPanel1 As TableLayoutPanel
    Public WithEvents PanelIcon As Panel
    Public WithEvents LabelText As Label

    Protected Property ModelOperationsWrapper As ModelOperationsWrapper

    Public Shadows Property Name As String
        Get
            Return MyBase.Name
        End Get
        Set(value As String)
            Me.ModelOperationsWrapper.Name = value
            MyBase.Name = value
        End Set
    End Property

    ''' <summary>
    ''' 配置中心对象
    ''' </summary>
    ''' <returns></returns>
    <Description("配置中心对象"), Category("FrontWork")>
    Public Property Configuration As Configuration
        Get
            Return Me.ModelOperationsWrapper.Configuration
        End Get
        Set(value As Configuration)
            Me.ModelOperationsWrapper.Configuration = value
        End Set
    End Property

    ''' <summary>
    ''' 数据行数
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    Public ReadOnly Property RowCount As Integer
        Get
            Return Me.ModelOperationsWrapper.GetRowCount
        End Get
    End Property

    Public Function GetRowCount() As Integer Implements IModelCore.GetRowCount
        Return Me.ModelOperationsWrapper.GetRowCount
    End Function

    <Browsable(False)>
    Public ReadOnly Property ColumnCount As Integer
        Get
            Return Me.GetColumnCount
        End Get
    End Property

    Public Function GetColumnCount() As Integer Implements IModelCore.GetColumnCount
        Return Me.ModelOperationsWrapper.GetColumnCount
    End Function

    Public Function GetSelectionRanges() As Range() Implements IModelCore.GetSelectionRanges
        Return Me.ModelOperationsWrapper.GetSelectionRanges
    End Function

    Public Sub SetSelectionRanges(ranges As Range()) Implements IModelCore.SetSelectionRanges
        Call Me.ModelOperationsWrapper.SetSelectionRanges(ranges)
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
    Public Property Mode As String
        Get
            Return Me.ModelOperationsWrapper.Mode
        End Get
        Set(value As String)
            ModelOperationsWrapper.Mode = value
        End Set
    End Property

    Public Sub New()
        Me.ModelOperationsWrapper = New ModelOperationsWrapper(New ModelCore)
    End Sub

    Public Function GetCell(row As Integer, columnName As String) As Object
        Return Me.ModelOperationsWrapper.GetCell(row, columnName)
    End Function

    ''' <summary>
    ''' 获取行并自动转换成相应类型的对象返回
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="rows"></param>
    ''' <returns></returns>
    Public Function GetRows(Of T As New)(rows As Integer()) As T()
        Return ModelOperationsWrapper.GetRows(Of T)(rows)
    End Function

    Public Function GetRow(Of T As New)(row As Integer) As T
        Return Me.ModelOperationsWrapper.GetRow(Of T)(row)
    End Function

    ''' <summary>
    ''' 获取行
    ''' </summary>
    ''' <param name="rowIDs">行ID</param>
    ''' <returns>相应行数据</returns>
    Public Function GetRows(rowIDs As Guid()) As IDictionary(Of String, Object)()
        Return Me.ModelOperationsWrapper.GetRows(rowIDs)
    End Function

    ''' <summary>
    ''' 获取行
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <returns>相应行数据</returns>
    Public Function GetRows(rows As Integer()) As IDictionary(Of String, Object)() Implements IModelCore.GetRows
        Return Me.ModelOperationsWrapper.GetRows(rows)
    End Function

    ''' <summary>
    ''' 获取行
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <returns>相应行数据</returns>
    Public Function GetRow(row As Integer) As IDictionary(Of String, Object)
        Return Me.ModelOperationsWrapper.GetRow(row)
    End Function

    ''' <summary>
    ''' 增加行
    ''' </summary>
    ''' <param name="data">增加行的数据</param>
    ''' <returns>增加的行号</returns>
    Public Function AddRow(data As IDictionary(Of String, Object)) As Integer
        Return Me.ModelOperationsWrapper.AddRow(data)
    End Function

    ''' <summary>
    ''' 增加行
    ''' </summary>
    ''' <param name="dataOfEachRow">增加行的数据</param>
    ''' <returns>增加的行号</returns>
    Public Function AddRows(dataOfEachRow As IDictionary(Of String, Object)()) As Integer() Implements IModelCore.AddRows
        Return Me.ModelOperationsWrapper.AddRows(dataOfEachRow)
    End Function

    ''' <summary>
    ''' 插入行
    ''' </summary>
    ''' <param name="row">插入行行号</param>
    ''' <param name="data">数据</param>
    Public Sub InsertRow(row As Integer, data As IDictionary(Of String, Object))
        Call Me.ModelOperationsWrapper.InsertRow(row, data)
    End Sub

    ''' <summary>
    ''' 插入行
    ''' </summary>
    ''' <param name="rows">插入行行号</param>
    ''' <param name="dataOfEachRow">数据</param>
    Public Sub InsertRows(rows As Integer(), dataOfEachRow As IDictionary(Of String, Object)()) Implements IModelCore.InsertRows
        Call Me.ModelOperationsWrapper.InsertRows(rows, dataOfEachRow)
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="rowID">删除行ID</param>
    Public Sub RemoveRow(rowID As Guid)
        Me.ModelOperationsWrapper.RemoveRow(rowID)
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="row">删除行行号</param>
    Public Sub RemoveRow(row As Integer)
        Call Me.ModelOperationsWrapper.RemoveRow(row)
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="startRow">起始行号</param>
    ''' <param name="rowCount">删除行数</param>
    Public Sub RemoveRows(startRow As Integer, rowCount As Integer)
        Call Me.ModelOperationsWrapper.RemoveRows(startRow, rowCount)
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="rowIDs">删除行ID</param>
    Public Sub RemoveRows(rowIDs As Guid())
        Call Me.ModelOperationsWrapper.RemoveRows(rowIDs)
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="rows">删除行行号</param>
    Public Sub RemoveRows(rows As Integer()) Implements IModelCore.RemoveRows
        Call Me.ModelOperationsWrapper.RemoveRows(rows)
    End Sub

    Public Sub RemoveSelectedRows()
        Call Me.ModelOperationsWrapper.RemoveSelectedRows()
    End Sub

    ''' <summary>
    ''' 更新行
    ''' </summary>
    ''' <param name="rowID">更新行ID</param>
    ''' <param name="data">数据</param>
    Public Sub UpdateRow(rowID As Guid, data As IDictionary(Of String, Object))
        Call Me.ModelOperationsWrapper.UpdateRow(rowID, data)
    End Sub

    ''' <summary>
    ''' 更新行
    ''' </summary>
    ''' <param name="row">更新行行号</param>
    ''' <param name="data">数据</param>
    Public Sub UpdateRow(row As Integer, data As IDictionary(Of String, Object))
        Call Me.ModelOperationsWrapper.UpdateRow(row, data)
    End Sub

    ''' <summary>
    ''' 更新行
    ''' </summary>
    ''' <param name="rowIDs">更新的行ID</param>
    ''' <param name="dataOfEachRow">相应的数据</param>
    Public Sub UpdateRows(rowIDs As Guid(), dataOfEachRow As IDictionary(Of String, Object)())
        Call Me.UpdateRows(rowIDs, dataOfEachRow)
    End Sub

    ''' <summary>
    ''' 更新行
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="dataOfEachRow">对应的数据</param>
    Public Sub UpdateRows(rows As Integer(), dataOfEachRow As IDictionary(Of String, Object)()) Implements IModelCore.UpdateRows
        Call Me.ModelOperationsWrapper.UpdateRows(rows, dataOfEachRow)
    End Sub

    ''' <summary>
    ''' 更新单元格
    ''' </summary>
    ''' <param name="row">行ID</param>
    ''' <param name="columnName">列名</param>
    ''' <param name="data">更新的数据</param>
    Public Sub UpdateCell(row As Guid, columnName As String, data As Object)
        Call Me.ModelOperationsWrapper.UpdateCell(row, columnName, data)
    End Sub

    ''' <summary>
    ''' 更新单元格
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="columnName">列名</param>
    ''' <param name="data">更新的数据</param>
    Public Sub UpdateCell(row As Integer, columnName As String, data As Object)
        Call Me.ModelOperationsWrapper.UpdateCell(row, columnName, data)
    End Sub

    ''' <summary>
    ''' 更新单元格
    ''' </summary>
    ''' <param name="rowIDs">行号</param>
    ''' <param name="columnNames">列名</param>
    ''' <param name="dataOfEachCell">对应的数据</param>
    Public Sub UpdateCells(rowIDs As Guid(), columnNames As String(), dataOfEachCell As Object())
        Call Me.ModelOperationsWrapper.UpdateCells(rowIDs, columnNames, dataOfEachCell)
    End Sub

    ''' <summary>
    ''' 更新单元格
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="columnNames">列名</param>
    ''' <param name="dataOfEachCell">相应的数据</param>
    Public Sub UpdateCells(rows As Integer(), columnNames As String(), dataOfEachCell As Object()) Implements IModelCore.UpdateCells
        Call Me.ModelOperationsWrapper.UpdateCells(rows, columnNames, dataOfEachCell)
    End Sub

    ''' <summary>
    ''' 刷新Model
    ''' </summary>
    ''' <param name="dataTable">数据表</param>
    ''' <param name="ranges">选区</param>
    ''' <param name="syncStates">各行同步状态</param>
    Public Overloads Sub Refresh(dataTable As DataTable, ranges As Range(), syncStates As SynchronizationState()) Implements IModelCore.Refresh
        Call Me.ModelOperationsWrapper.Refresh(dataTable, ranges, syncStates)
    End Sub

    ''' <summary>
    ''' 获取行ID
    ''' </summary>
    ''' <param name="rowNum">行号</param>
    ''' <returns>行ID</returns>
    Public Function GetRowID(rowNum As Integer) As Guid
        Return Me.ModelOperationsWrapper.GetRowID(rowNum)
    End Function

    Public Sub UpdateRowIDs(oriRowIDs As Guid(), newIDs As Guid()) Implements IModelCore.UpdateRowIDs
        Call Me.ModelOperationsWrapper.UpdateRowIDs(oriRowIDs, newIDs)
    End Sub

    ''' <summary>
    ''' 获取行ID
    ''' </summary>
    ''' <param name="rowNums">行号</param>
    ''' <returns>行ID</returns>
    Public Function GetRowIDs(rowNums As Integer()) As Guid() Implements IModelCore.GetRowIDs
        Return Me.ModelOperationsWrapper.GetRowIDs(rowNums)
    End Function

    Public Function GetRowIndexes(rowIDs As Guid()) As Integer() Implements IModelCore.GetRowIndexes
        Return Me.ModelOperationsWrapper.GetRowIndexes(rowIDs)
    End Function

    Public Function GetRowIndex(rowID As Guid) As Integer
        Return Me.GetRowIndexes({rowID})(0)
    End Function

    ''' <summary>
    ''' 更新行同步状态
    ''' </summary>
    ''' <param name="rowIDs">行ID</param>
    ''' <param name="syncStates">同步状态</param>
    Public Sub UpdateRowSynchronizationStates(rowIDs As Guid(), syncStates As SynchronizationState())
        Call Me.ModelOperationsWrapper.UpdateRowSynchronizationStates(rowIDs, syncStates)
    End Sub

    ''' <summary>
    ''' 更新行同步状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="syncStates">同步状态</param>
    Public Sub UpdateRowSynchronizationStates(rows As Integer(), syncStates As SynchronizationState()) Implements IModelCore.UpdateRowSynchronizationStates
        Call Me.ModelOperationsWrapper.UpdateRowSynchronizationStates(rows, syncStates)
    End Sub

    ''' <summary>
    ''' 更新行同步状态
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="syncState">同步状态</param>
    Public Sub UpdateRowSynchronizationState(row As Integer, syncState As SynchronizationState)
        Call Me.ModelOperationsWrapper.UpdateRowSynchronizationState(row, syncState)
    End Sub

    ''' <summary>
    ''' 更新行同步状态
    ''' </summary>
    ''' <param name="rowID">行ID</param>
    ''' <param name="syncState">同步状态</param>
    Public Sub UpdateRowSynchronizationState(rowID As Guid, syncState As SynchronizationState)
        Call Me.ModelOperationsWrapper.UpdateRowSynchronizationState(rowID, syncState)
    End Sub

    ''' <summary>
    ''' 获取行同步状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <returns>同步状态</returns>
    Public Function GetRowSynchronizationStates(rows As Integer()) As SynchronizationState() Implements IModelCore.GetRowSynchronizationStates
        Return Me.ModelOperationsWrapper.GetRowSynchronizationStates(rows)
    End Function

    ''' <summary>
    ''' 获取行同步状态
    ''' </summary>
    ''' <param name="rowIDs">行ID</param>
    ''' <returns>同步状态</returns>
    Public Function GetRowSynchronizationStates(rowIDs As Guid()) As SynchronizationState()
        Return Me.ModelOperationsWrapper.GetRowSynchronizationStates(rowIDs)
    End Function

    ''' <summary>
    ''' 获取行同步状态
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <returns>同步状态</returns>
    Public Function GetRowSynchronizationState(row As Integer) As SynchronizationState
        Return Me.ModelOperationsWrapper.GetRowSynchronizationState(row)
    End Function

    ''' <summary>
    ''' 获取行同步状态
    ''' </summary>
    ''' <param name="rowID">行ID</param>
    ''' <returns>同步状态</returns>
    Public Function GetRowSynchronizationStates(rowID As Guid) As SynchronizationState
        Return Me.ModelOperationsWrapper.GetRowSynchronizationStates(rowID)
    End Function

    ''' <summary>
    ''' Model刷新事件
    ''' </summary>
    Public Custom Event Refreshed As EventHandler(Of ModelRefreshedEventArgs) Implements IModelCore.Refreshed
        AddHandler(value As EventHandler(Of ModelRefreshedEventArgs))
            AddHandler Me.ModelOperationsWrapper.Refreshed, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRefreshedEventArgs))
            RemoveHandler Me.ModelOperationsWrapper.Refreshed, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRefreshedEventArgs)
        End RaiseEvent
    End Event

    ''' <summary>
    ''' 增加行事件
    ''' </summary>
    Public Custom Event RowAdded As EventHandler(Of ModelRowAddedEventArgs) Implements IModelCore.RowAdded
        AddHandler(value As EventHandler(Of ModelRowAddedEventArgs))
            AddHandler Me.ModelOperationsWrapper.RowAdded, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRowAddedEventArgs))
            RemoveHandler Me.ModelOperationsWrapper.RowAdded, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRowAddedEventArgs)

        End RaiseEvent
    End Event

    ''' <summary>
    ''' 更新行数据事件
    ''' </summary>
    Public Custom Event RowUpdated As EventHandler(Of ModelRowUpdatedEventArgs) Implements IModelCore.RowUpdated
        AddHandler(value As EventHandler(Of ModelRowUpdatedEventArgs))
            AddHandler Me.ModelOperationsWrapper.RowUpdated, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRowUpdatedEventArgs))
            RemoveHandler Me.ModelOperationsWrapper.RowUpdated, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRowUpdatedEventArgs)

        End RaiseEvent
    End Event

    ''' <summary>
    ''' 删除行事件
    ''' </summary>
    Public Custom Event BeforeRowRemove As EventHandler(Of ModelBeforeRowRemoveEventArgs) Implements IModelCore.BeforeRowRemove
        AddHandler(value As EventHandler(Of ModelBeforeRowRemoveEventArgs))
            AddHandler Me.ModelOperationsWrapper.BeforeRowRemove, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelBeforeRowRemoveEventArgs))
            RemoveHandler Me.ModelOperationsWrapper.BeforeRowRemove, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelBeforeRowRemoveEventArgs)
        End RaiseEvent
    End Event

    ''' <summary>
    ''' 删除行事件
    ''' </summary>
    Public Custom Event RowRemoved As EventHandler(Of ModelRowRemovedEventArgs) Implements IModelCore.RowRemoved
        AddHandler(value As EventHandler(Of ModelRowRemovedEventArgs))
            AddHandler Me.ModelOperationsWrapper.RowRemoved, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRowRemovedEventArgs))
            RemoveHandler Me.ModelOperationsWrapper.RowRemoved, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRowRemovedEventArgs)

        End RaiseEvent
    End Event

    ''' <summary>
    ''' 单元格数据更新事件
    ''' </summary>
    Public Custom Event CellUpdated As EventHandler(Of ModelCellUpdatedEventArgs) Implements IModelCore.CellUpdated
        AddHandler(value As EventHandler(Of ModelCellUpdatedEventArgs))
            AddHandler Me.ModelOperationsWrapper.CellUpdated, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelCellUpdatedEventArgs))
            RemoveHandler Me.ModelOperationsWrapper.CellUpdated, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelCellUpdatedEventArgs)

        End RaiseEvent
    End Event

    ''' <summary>
    ''' 选区改变事件
    ''' </summary>
    Public Custom Event SelectionRangeChanged As EventHandler(Of ModelSelectionRangeChangedEventArgs) Implements IModelCore.SelectionRangeChanged
        AddHandler(value As EventHandler(Of ModelSelectionRangeChangedEventArgs))
            AddHandler Me.ModelOperationsWrapper.SelectionRangeChanged, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelSelectionRangeChangedEventArgs))
            RemoveHandler Me.ModelOperationsWrapper.SelectionRangeChanged, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelSelectionRangeChangedEventArgs)

        End RaiseEvent
    End Event

    ''' <summary>
    ''' 行同步状态改变事件
    ''' </summary>
    Public Custom Event RowSynchronizationStateChanged As EventHandler(Of ModelRowSynchronizationStateChangedEventArgs) Implements IModelCore.RowSynchronizationStateChanged
        AddHandler(value As EventHandler(Of ModelRowSynchronizationStateChangedEventArgs))
            AddHandler Me.ModelOperationsWrapper.RowSynchronizationStateChanged, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRowSynchronizationStateChangedEventArgs))
            RemoveHandler Me.ModelOperationsWrapper.RowSynchronizationStateChanged, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRowSynchronizationStateChangedEventArgs)

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
        Call Me.ModelOperationsWrapper.SelectRowsByValues(columnName, values)
    End Sub

    ''' <summary>
    ''' 获取所有选中行
    ''' </summary>
    ''' <typeparam name="T">要映射成的类型</typeparam>
    ''' <returns>选中行映射后的对象数组</returns>
    Public Function GetSelectedRows(Of T As New)() As T()
        Return Me.ModelOperationsWrapper.GetSelectedRows(Of T)
    End Function

    ''' <summary>
    ''' 获取所有选中行的某一列
    ''' </summary>
    ''' <typeparam name="T">返回类型</typeparam>
    ''' <param name="columnName">列名</param>
    ''' <returns>所有选中行指定列的数据</returns>
    Public Function GetSelectedRows(Of T)(columnName As String) As T()
        Return Me.ModelOperationsWrapper.GetSelectedRows(Of T)(columnName)
    End Function

    Public Function GetSelectedRows() As IDictionary(Of String, Object)()
        Return Me.ModelOperationsWrapper.GetSelectedRows
    End Function

    ''' <summary>
    ''' 删除新增但未编辑的行
    ''' </summary>
    Public Sub RemoveUneditedNewRows()
        Call Me.ModelOperationsWrapper.RemoveUneditedNewRows()
    End Sub

    Public Sub AddColumns(columns() As ModelColumn) Implements IModelCore.AddColumns
        Call Me.ModelOperationsWrapper.AddColumns(columns)
    End Sub

    Public Sub RemoveColumns(columnNames() As String) Implements IModelCore.RemoveColumns
        Call Me.ModelOperationsWrapper.RemoveColumns(columnNames)
    End Sub

    Public Function GetColumns() As ModelColumn() Implements IModelCore.GetColumns
        Return Me.ModelOperationsWrapper.GetColumns
    End Function

    Public Function GetColumns(columnNames() As String) As ModelColumn() Implements IModelCore.GetColumns
        Return Me.ModelOperationsWrapper.GetColumns(columnNames)
    End Function

    Public Function GetCells(rows() As Integer, columnNames() As String) As Object() Implements IModelCore.GetCells
        Return Me.ModelOperationsWrapper.GetCells(rows, columnNames)
    End Function

    Public Function ToDataTable() As DataTable Implements IModelCore.ToDataTable
        Return Me.ModelOperationsWrapper.ToDataTable
    End Function

    Public Sub RefreshView(rows As Integer())
        Call Me.ModelOperationsWrapper.RefreshView(rows)
    End Sub

    Public Sub RefreshView(row As Integer)
        Call Me.ModelOperationsWrapper.RefreshView(row)
    End Sub

    Public Sub RaiseRefreshedEvent(sender As Object, args As ModelRefreshedEventArgs)
        Call Me.ModelOperationsWrapper.RaiseRefreshedEvent(sender, args)
    End Sub

    Public Sub RaiseCellUpdatedEvent(sender As Object, args As ModelCellUpdatedEventArgs)
        Call Me.ModelOperationsWrapper.RaiseCellUpdatedEvent(sender, args)
    End Sub

    Public Sub RaiseRowUpdatedEvent(sender As Object, args As ModelRowUpdatedEventArgs)
        Call Me.ModelOperationsWrapper.RaiseRowUpdatedEvent(sender, args)
    End Sub

    Public Sub RaiseRowAddedEvent(sender As Object, args As ModelRowAddedEventArgs)
        Call Me.ModelOperationsWrapper.RaiseRowAddedEvent(sender, args)
    End Sub

    Public Sub RaiseBeforeRowRemoveEvent(sender As Object, args As ModelBeforeRowRemoveEventArgs)
        Call Me.ModelOperationsWrapper.RaiseBeforeRowRemoveEvent(sender, args)
    End Sub

    Public Sub RaiseRowRemovedEvent(sender As Object, args As ModelRowRemovedEventArgs)
        Call Me.ModelOperationsWrapper.RaiseRowRemovedEvent(sender, args)
    End Sub

    Public Sub RaiseSelectionRangeChangedEvent(sender As Object, args As ModelSelectionRangeChangedEventArgs)
        Call Me.ModelOperationsWrapper.RaiseSelectionRangeChangedEvent(sender, args)
    End Sub

    Public Sub RaiseRowSynchronizationStateChangedEvent(sender As Object, args As ModelRowSynchronizationStateChangedEventArgs)
        Call Me.ModelOperationsWrapper.RaiseRowSynchronizationStateChangedEvent(sender, args)
    End Sub
End Class
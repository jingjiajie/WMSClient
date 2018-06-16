Imports System.ComponentModel
Imports System.Linq
Imports System.Reflection
Imports FrontWork

''' <summary>
''' 模型类
''' </summary>
Public Class Model
    Inherits UserControl
    Implements IModelCore

    Private _configuration As Configuration
    Private _mode As String = "default"
    Private _modelCore As IModelCore
    Public WithEvents TableLayoutPanel1 As TableLayoutPanel
    Public WithEvents PanelIcon As Panel
    Public WithEvents LabelText As Label

    Protected Property ModelCore As IModelCore
        Get
            Return Me._modelCore
        End Get
        Set(value As IModelCore)
            If Me._modelCore IsNot Nothing Then
                Call Me.UnbindModelCore(Me.ModelCore)
            End If
            Me._modelCore = value
            If Me._modelCore IsNot Nothing Then
                Call Me.BindModelCore(Me.ModelCore)
            End If
        End Set
    End Property

    Public Sub New()
        If Me.ModelCore Is Nothing Then
            Me.ModelCore = New ModelCore
        End If
    End Sub

    Private Sub BindModelCore(modelCore As IModelCore)
        AddHandler Me.ModelCore.CellUpdated, AddressOf Me.RaiseCellUpdatedEvent
        AddHandler Me.ModelCore.RowUpdated, AddressOf Me.RaiseRowUpdatedEvent
        AddHandler Me.ModelCore.RowAdded, AddressOf Me.RaiseRowAddedEvent
        AddHandler Me.ModelCore.BeforeRowRemove, AddressOf Me.RaiseBeforeRowRemoveEvent
        AddHandler Me.ModelCore.RowRemoved, AddressOf Me.RaiseRowRemovedEvent
        AddHandler Me.ModelCore.SelectionRangeChanged, AddressOf Me.RaiseSelectionRangeChangedEvent
        AddHandler Me.ModelCore.Refreshed, AddressOf Me.RaiseRefreshedEvent
        AddHandler Me.ModelCore.RowSynchronizationStateChanged, AddressOf Me.RaiseRowSynchronizationStateChangedEvent
    End Sub

    Private Sub UnbindModelCore(modelCore As IModelCore)
        RemoveHandler Me.ModelCore.CellUpdated, AddressOf Me.RaiseCellUpdatedEvent
        RemoveHandler Me.ModelCore.RowUpdated, AddressOf Me.RaiseRowUpdatedEvent
        RemoveHandler Me.ModelCore.RowAdded, AddressOf Me.RaiseRowAddedEvent
        RemoveHandler Me.ModelCore.BeforeRowRemove, AddressOf Me.RaiseBeforeRowRemoveEvent
        RemoveHandler Me.ModelCore.RowRemoved, AddressOf Me.RaiseRowRemovedEvent
        RemoveHandler Me.ModelCore.SelectionRangeChanged, AddressOf Me.RaiseSelectionRangeChangedEvent
        RemoveHandler Me.ModelCore.Refreshed, AddressOf Me.RaiseRefreshedEvent
        RemoveHandler Me.ModelCore.RowSynchronizationStateChanged, AddressOf Me.RaiseRowSynchronizationStateChangedEvent
    End Sub

    Public Shadows Property Name As String
        Get
            Return MyBase.Name
        End Get
        Set(value As String)
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
            Return Me._configuration
        End Get
        Set(value As Configuration)
            If Me._configuration IsNot Nothing Then
                RemoveHandler Me._configuration.ConfigurationChanged, AddressOf Me.ConfigurationChanged
            End If
            Me._configuration = value
            If Me._configuration IsNot Nothing Then
                Call Me.RefreshCoreSchema(Me._configuration)
                AddHandler Me._configuration.ConfigurationChanged, AddressOf Me.ConfigurationChanged
            End If
        End Set
    End Property

    ''' <summary>
    ''' 数据行数
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    Public ReadOnly Property RowCount As Integer
        Get
            Return Me.GetRowCount
        End Get
    End Property

    Public Function GetRowCount() As Integer Implements IModelCore.GetRowCount
        Return Me.ModelCore.GetRowCount
    End Function

    <Browsable(False)>
    Public ReadOnly Property ColumnCount As Integer
        Get
            Return Me.GetColumnCount
        End Get
    End Property

    Public Function GetColumnCount() As Integer Implements IModelCore.GetColumnCount
        Return Me.ModelCore.GetColumnCount
    End Function

    Public Function GetSelectionRanges() As Range() Implements IModelCore.GetSelectionRanges
        Return Me.ModelCore.GetSelectionRanges
    End Function

    Public Sub SetSelectionRanges(ranges As Range()) Implements IModelCore.SetSelectionRanges
        Call Me.ModelCore.SetSelectionRanges(ranges)
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
            Return Me._mode
        End Get
        Set(value As String)
            Me._mode = value
            Call Me.ConfigurationChanged(Me, New ConfigurationChangedEventArgs)
        End Set
    End Property

    Private Sub ConfigurationChanged(sender As Object, e As ConfigurationChangedEventArgs)
        Call Me.RefreshCoreSchema(Me.Configuration)
        RaiseEvent Refreshed(Me, New ModelRefreshedEventArgs)
    End Sub

    Private Sub RefreshCoreSchema(config As Configuration)
        Dim fields = config.GetFieldConfigurations(Me.Mode)
        Dim addColumns As New List(Of ModelColumn)
        For Each field In fields
            If Not Me.ContainsColumn(field.Name) Then
                Dim newColumn As New ModelColumn
                With newColumn
                    .Name = field.Name
                    .Type = field.Type.FieldType
                    .Nullable = True
                    .DefaultValue = field.DefaultValue
                End With
                addColumns.Add(newColumn)
            End If
        Next
        If addColumns.Count > 0 Then
            Call Me.ModelCore.AddColumns(addColumns.ToArray)
        End If
    End Sub

    Public Function GetCell(row As Integer, columnName As String) As Object
        Return Me.ModelCore.GetCells({row}, {columnName})(0)
    End Function

    Public Function GetCells(rows As Integer(), columnNames As String()) As Object()
        Return ModelCore.GetCells(rows, columnNames)
    End Function

    ''' <summary>
    ''' 获取行并自动转换成相应类型的对象返回
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="rows"></param>
    ''' <returns></returns>
    Public Function GetRows(Of T As New)(rows As Integer()) As T()
        Dim rowData = Me.GetRows(rows)
        Dim result(rows.Length - 1) As T
        For i = 0 To result.Length - 1
            result(i) = Me.DictionaryToObject(Of T)(rowData(i))
        Next
        Return result
    End Function

    Public Function GetRow(Of T As New)(row As Integer) As T
        Return Me.GetRows(Of T)({row})(0)
    End Function

    ''' <summary>
    ''' 获取行
    ''' </summary>
    ''' <param name="rowIDs">行ID</param>
    ''' <returns>相应行数据</returns>
    Public Function GetRows(rowIDs As Guid()) As IDictionary(Of String, Object)()
        Dim rowNums(rowIDs.Length - 1) As Integer
        For i = 0 To rowIDs.Length - 1
            Dim rowID = rowIDs(i)
            Dim rowNum = Me.GetRowIndex(rowID)
            If rowNum = -1 Then
                Throw New FrontWorkException($"Invalid RowID: {rowID}")
            End If
            rowNums(i) = rowNum
        Next
        Return Me.GetRows(rowNums)
    End Function

    ''' <summary>
    ''' 获取行
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <returns>相应行数据</returns>
    Public Function GetRows(rows As Integer()) As IDictionary(Of String, Object)() Implements IModelCore.GetRows
        Return Me.ModelCore.GetRows(rows)
    End Function

    ''' <summary>
    ''' 获取行
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <returns>相应行数据</returns>
    Public Function GetRow(row As Integer) As IDictionary(Of String, Object)
        Return Me.GetRows({row})(0)
    End Function

    ''' <summary>
    ''' 增加行
    ''' </summary>
    ''' <param name="data">增加行的数据</param>
    ''' <returns>增加的行号</returns>
    Public Function AddRow(data As IDictionary(Of String, Object)) As Integer
        Return Me.AddRows({data})(0)
    End Function

    ''' <summary>
    ''' 增加行
    ''' </summary>
    ''' <param name="dataOfEachRow">增加行的数据</param>
    ''' <returns>增加的行号</returns>
    Public Function AddRows(dataOfEachRow As IDictionary(Of String, Object)()) As Integer() Implements IModelCore.AddRows
        Dim addRowCount = dataOfEachRow.Length
        Dim oriRowCount = Me.GetRowCount
        Dim insertRows = Util.Range(RowCount, RowCount + addRowCount)
        Call Me.InsertRows(insertRows, dataOfEachRow)
        Return insertRows
    End Function

    ''' <summary>
    ''' 插入行
    ''' </summary>
    ''' <param name="row">插入行行号</param>
    ''' <param name="data">数据</param>
    Public Sub InsertRow(row As Integer, data As IDictionary(Of String, Object))
        Call Me.InsertRows({row}, {data})
    End Sub

    ''' <summary>
    ''' 插入行
    ''' </summary>
    ''' <param name="rows">插入行行号</param>
    ''' <param name="dataOfEachRow">数据</param>
    Public Sub InsertRows(rows As Integer(), dataOfEachRow As IDictionary(Of String, Object)()) Implements IModelCore.InsertRows
        Call Me.ModelCore.InsertRows(rows, dataOfEachRow)
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="rowID">删除行ID</param>
    Public Sub RemoveRow(rowID As Guid)
        Me.RemoveRows({rowID})
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="row">删除行行号</param>
    Public Sub RemoveRow(row As Integer)
        Me.RemoveRows({row})
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="startRow">起始行号</param>
    ''' <param name="rowCount">删除行数</param>
    Public Sub RemoveRows(startRow As Integer, rowCount As Integer)
        Me.RemoveRows(Util.Range(startRow, startRow + rowCount))
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="rowIDs">删除行ID</param>
    Public Sub RemoveRows(rowIDs As Guid())
        Dim rowNums(rowIDs.Length - 1) As Integer
        For i = 0 To rowIDs.Length - 1
            Dim rowID = rowIDs(i)
            Dim rowNum = Me.GetRowIndex(rowID)
            If rowNum = -1 Then
                Throw New FrontWorkException($"Invalid RowID: {rowID}")
            End If
            rowNums(i) = rowNum
        Next
        Me.RemoveRows(rowNums)
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="rows">删除行行号</param>
    Public Sub RemoveRows(rows As Integer()) Implements IModelCore.RemoveRows
        Call Me.ModelCore.RemoveRows(rows)
    End Sub

    Public Sub RemoveSelectedRows()
        If Me.AllSelectionRanges Is Nothing Then Return
        Dim removeRowIDs As New List(Of Guid)
        For Each range In Me.AllSelectionRanges
            For i = 0 To range.Rows - 1
                removeRowIDs.Add(Me.GetRowID(range.Row + i))
            Next
        Next
        Call Me.RemoveRows(removeRowIDs.Distinct.ToArray)
    End Sub

    ''' <summary>
    ''' 更新行
    ''' </summary>
    ''' <param name="rowID">更新行ID</param>
    ''' <param name="data">数据</param>
    Public Sub UpdateRow(rowID As Guid, data As IDictionary(Of String, Object))
        Me.UpdateRows({rowID}, {data})
    End Sub

    ''' <summary>
    ''' 更新行
    ''' </summary>
    ''' <param name="row">更新行行号</param>
    ''' <param name="data">数据</param>
    Public Sub UpdateRow(row As Integer, data As IDictionary(Of String, Object))
        Call Me.UpdateRows(
            New Integer() {row},
            New Dictionary(Of String, Object)() {data}
        )
    End Sub

    ''' <summary>
    ''' 更新行
    ''' </summary>
    ''' <param name="rowIDs">更新的行ID</param>
    ''' <param name="dataOfEachRow">相应的数据</param>
    Public Sub UpdateRows(rowIDs As Guid(), dataOfEachRow As IDictionary(Of String, Object)())
        Dim rowNums(rowIDs.Length - 1) As Integer
        For i = 0 To rowIDs.Length - 1
            Dim rowID = rowIDs(i)
            Dim rowNum = Me.GetRowIndex(rowID)
            If rowNum = -1 Then
                Throw New FrontWorkException($"Invalid RowID: {rowID}")
            End If
            rowNums(i) = rowNum
        Next
        Call Me.UpdateRows(rowNums, dataOfEachRow)
    End Sub

    ''' <summary>
    ''' 更新行
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="dataOfEachRow">对应的数据</param>
    Public Sub UpdateRows(rows As Integer(), dataOfEachRow As IDictionary(Of String, Object)()) Implements IModelCore.UpdateRows
        Call Me.ModelCore.UpdateRows(rows, dataOfEachRow)
    End Sub

    ''' <summary>
    ''' 更新单元格
    ''' </summary>
    ''' <param name="row">行ID</param>
    ''' <param name="columnName">列名</param>
    ''' <param name="data">更新的数据</param>
    Public Sub UpdateCell(row As Guid, columnName As String, data As Object)
        Me.UpdateCells({row}, {columnName}, {data})
    End Sub

    ''' <summary>
    ''' 更新单元格
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="columnName">列名</param>
    ''' <param name="data">更新的数据</param>
    Public Sub UpdateCell(row As Integer, columnName As String, data As Object)
        Me.UpdateCells({row}, New String() {columnName}, New Object() {data})
    End Sub

    ''' <summary>
    ''' 更新单元格
    ''' </summary>
    ''' <param name="rowIDs">行号</param>
    ''' <param name="columnNames">列名</param>
    ''' <param name="dataOfEachCell">对应的数据</param>
    Public Sub UpdateCells(rowIDs As Guid(), columnNames As String(), dataOfEachCell As Object())
        Dim rowNums(rowIDs.Length - 1) As Integer
        For i = 0 To rowIDs.Length - 1
            Dim rowID = rowIDs(i)
            Dim rowNum = Me.GetRowIndex(rowID)
            If rowNum = -1 Then
                Throw New FrontWorkException($"Invalid RowID: {rowID}")
            End If
            rowNums(i) = rowNum
        Next
        Me.UpdateCells(rowNums, columnNames, dataOfEachCell)
    End Sub

    ''' <summary>
    ''' 更新单元格
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="columnNames">列名</param>
    ''' <param name="dataOfEachCell">相应的数据</param>
    Public Sub UpdateCells(rows As Integer(), columnNames As String(), dataOfEachCell As Object()) Implements IModelCore.UpdateCells
        Call Me.ModelCore.UpdateCells(rows, columnNames, dataOfEachCell)
    End Sub

    ''' <summary>
    ''' 刷新Model
    ''' </summary>
    ''' <param name="dataTable">数据表</param>
    ''' <param name="ranges">选区</param>
    ''' <param name="syncStates">各行同步状态</param>
    Public Overloads Sub Refresh(dataTable As DataTable, ranges As Range(), syncStates As SynchronizationState()) Implements IModelCore.Refresh
        Call Me.ModelCore.Refresh(dataTable, ranges, syncStates)
    End Sub

    ''' <summary>
    ''' DataRow转字典
    ''' </summary>
    ''' <param name="dataRow">DataRow对象</param>
    ''' <returns>转换结果</returns>
    Protected Function DataRowToDictionary(dataRow As DataRow) As IDictionary(Of String, Object)
        Dim result As New Dictionary(Of String, Object)
        Dim columns = dataRow.Table.Columns
        For Each column As DataColumn In columns
            result.Add(column.ColumnName, If(dataRow(column) Is DBNull.Value, Nothing, dataRow(column)))
        Next
        Return result
    End Function

    ''' <summary>
    ''' 获取行ID
    ''' </summary>
    ''' <param name="rowNum">行号</param>
    ''' <returns>行ID</returns>
    Public Function GetRowID(rowNum As Integer) As Guid
        Return Me.GetRowIDs({rowNum})(0)
    End Function

    Public Sub UpdateRowIDs(oriRowIDs As Guid(), newIDs As Guid()) Implements IModelCore.UpdateRowIDs
        Call Me.ModelCore.UpdateRowIDs(oriRowIDs, newIDs)
    End Sub

    ''' <summary>
    ''' 获取行ID
    ''' </summary>
    ''' <param name="rowNums">行号</param>
    ''' <returns>行ID</returns>
    Public Function GetRowIDs(rowNums As Integer()) As Guid() Implements IModelCore.GetRowIDs
        Return Me.ModelCore.GetRowIDs(rowNums)
    End Function

    Public Function GetRowIndexes(rowIDs As Guid()) As Integer() Implements IModelCore.GetRowIndexes
        Return Me.ModelCore.GetRowIndexes(rowIDs)
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
        If rowIDs.Length <> syncStates.Length Then
            Throw New FrontWorkException("Length of rows must be same of the length of syncStates")
        End If
        Dim rowNums(rowIDs.Length - 1) As Integer
        For i = 0 To rowIDs.Length - 1
            Dim rowID = rowIDs(i)
            Dim rowNum = Me.GetRowIndex(rowID)
            If rowNum < 0 Then
                Throw New FrontWorkException($"Row ID:{rowID} not found!")
            End If
            rowNums(i) = rowNum
        Next
        Call Me.UpdateRowSynchronizationStates(rowNums, syncStates)
    End Sub

    ''' <summary>
    ''' 更新行同步状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="syncStates">同步状态</param>
    Public Sub UpdateRowSynchronizationStates(rows As Integer(), syncStates As SynchronizationState()) Implements IModelCore.UpdateRowSynchronizationStates
        Call Me.ModelCore.UpdateRowSynchronizationStates(rows, syncStates)
    End Sub

    ''' <summary>
    ''' 更新行同步状态
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="syncState">同步状态</param>
    Public Sub UpdateRowSynchronizationState(row As Integer, syncState As SynchronizationState)
        Call Me.UpdateRowSynchronizationStates({row}, {syncState})
    End Sub

    ''' <summary>
    ''' 更新行同步状态
    ''' </summary>
    ''' <param name="rowID">行ID</param>
    ''' <param name="syncState">同步状态</param>
    Public Sub UpdateRowSynchronizationState(rowID As Guid, syncState As SynchronizationState)
        Call Me.UpdateRowSynchronizationStates({rowID}, {syncState})
    End Sub

    ''' <summary>
    ''' 获取行同步状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <returns>同步状态</returns>
    Public Function GetRowSynchronizationStates(rows As Integer()) As SynchronizationState() Implements IModelCore.GetRowSynchronizationStates
        Return Me.ModelCore.GetRowSynchronizationStates(rows)
    End Function

    ''' <summary>
    ''' 获取行同步状态
    ''' </summary>
    ''' <param name="rowIDs">行ID</param>
    ''' <returns>同步状态</returns>
    Public Function GetRowSynchronizationStates(rowIDs As Guid()) As SynchronizationState()
        Dim rows(rowIDs.Length - 1) As Integer
        For i = 0 To rowIDs.Length - 1
            Dim row = Me.GetRowIndex(rowIDs(i))
            If row < 0 Then
                Throw New FrontWorkException($"Row ID:{rowIDs(i)} not found!")
            End If
            rows(i) = row
        Next
        Return Me.GetRowSynchronizationStates(rows)
    End Function

    ''' <summary>
    ''' 获取行同步状态
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <returns>同步状态</returns>
    Public Function GetRowSynchronizationState(row As Integer) As SynchronizationState
        Return Me.GetRowSynchronizationStates({row})(0)
    End Function

    ''' <summary>
    ''' 获取行同步状态
    ''' </summary>
    ''' <param name="rowID">行ID</param>
    ''' <returns>同步状态</returns>
    Public Function GetRowSynchronizationStates(rowID As Guid) As SynchronizationState
        Return Me.GetRowSynchronizationStates({rowID})(0)
    End Function

    ''' <summary>
    ''' Model刷新事件
    ''' </summary>
    Public Event Refreshed As EventHandler(Of ModelRefreshedEventArgs) Implements IModelCore.Refreshed

    ''' <summary>
    ''' 增加行事件
    ''' </summary>
    Public Event RowAdded As EventHandler(Of ModelRowAddedEventArgs) Implements IModelCore.RowAdded

    ''' <summary>
    ''' 更新行数据事件
    ''' </summary>
    Public Event RowUpdated As EventHandler(Of ModelRowUpdatedEventArgs) Implements IModelCore.RowUpdated

    ''' <summary>
    ''' 删除行事件
    ''' </summary>
    Public Event BeforeRowRemove As EventHandler(Of ModelBeforeRowRemoveEventArgs) Implements IModelCore.BeforeRowRemove

    ''' <summary>
    ''' 删除行事件
    ''' </summary>
    Public Event RowRemoved As EventHandler(Of ModelRowRemovedEventArgs) Implements IModelCore.RowRemoved

    ''' <summary>
    ''' 单元格数据更新事件
    ''' </summary>
    Public Event CellUpdated As EventHandler(Of ModelCellUpdatedEventArgs) Implements IModelCore.CellUpdated

    ''' <summary>
    ''' 选区改变事件
    ''' </summary>
    Public Event SelectionRangeChanged As EventHandler(Of ModelSelectionRangeChangedEventArgs) Implements IModelCore.SelectionRangeChanged

    ''' <summary>
    ''' 行同步状态改变事件
    ''' </summary>
    Public Event RowSynchronizationStateChanged As EventHandler(Of ModelRowSynchronizationStateChangedEventArgs) Implements IModelCore.RowSynchronizationStateChanged

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
        If values Is Nothing Then
            Me.AllSelectionRanges = {}
            Return
        End If
        Dim dataTable = Me.ModelCore.ToDataTable
        Dim targetRows As New List(Of Integer)
        For i = 0 To dataTable.Rows.Count - 1
            Dim curRowValue = dataTable.Rows(i)(columnName)
            If values.Contains(curRowValue) Then
                targetRows.Add(i)
            End If
        Next
        '对目标行号分组
        Dim rowGroups As New List(Of List(Of Integer))
        For Each row In targetRows
            Dim lastGroup As List(Of Integer)
            If rowGroups.Count = 0 Then
                lastGroup = New List(Of Integer)
                rowGroups.Add(lastGroup)
            Else
                lastGroup = rowGroups.Last
            End If
            If lastGroup.Count = 0 OrElse lastGroup.Last + 1 = row Then
                lastGroup.Add(row)
            Else
                rowGroups.Add(New List(Of Integer)({row}))
            End If
        Next
        '生成选区
        Dim ranges As New List(Of Range)
        For Each rowGroup In rowGroups
            Dim newRange = New Range(rowGroup(0), 0, rowGroup.Count, dataTable.Columns.Count)
            ranges.Add(newRange)
        Next
        Me.AllSelectionRanges = ranges.ToArray
    End Sub

    Private Function DictionaryToObject(Of T As New)(dic As IDictionary(Of String, Object)) As T
        Dim result As New T
        Dim type = GetType(T)
        For Each entry In dic
            Dim key = entry.Key
            Dim prop = type.GetProperty(key, BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.IgnoreCase)
            '如果找到了相应属性，优先为属性映射值
            If prop IsNot Nothing Then
                Dim value As Object = Nothing
                Try
                    value = Convert.ChangeType(entry.Value, prop.PropertyType)
                Catch ex As Exception
                    Throw New FrontWorkException($"Value {entry.Value} of ""{key}"" cannot be converted to {prop.PropertyType.Name} for {type.Name}.{prop.Name}")
                End Try
                prop.SetValue(result, value, Nothing)
                Continue For
            End If
            '否则尝试寻找相应字段并赋值
            Dim field = type.GetField(key, BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.IgnoreCase)
            If field IsNot Nothing Then
                Dim value As Object = Nothing
                If entry.Value Is Nothing Then
                    Continue For
                Else
                    Try
                        value = Convert.ChangeType(entry.Value, field.FieldType)
                    Catch ex As Exception
                        Throw New FrontWorkException($"Value ""{entry.Value}"" of ""{key}"" cannot be converted to {field.FieldType.Name} for {type.Name}.{field.Name}")
                    End Try
                    field.SetValue(result, value)
                End If
            End If
        Next
        Return result
    End Function

    ''' <summary>
    ''' 获取所有选中行
    ''' </summary>
    ''' <typeparam name="T">要映射成的类型</typeparam>
    ''' <returns>选中行映射后的对象数组</returns>
    Public Function GetSelectedRows(Of T As New)() As T()
        If Me.AllSelectionRanges.Length = 0 Then Return {}
        Dim selectedRows As New List(Of Integer)
        For Each curSelectionRange In Me.AllSelectionRanges
            Dim row = curSelectionRange.Row
            Dim rows = curSelectionRange.Rows
            For i = 0 To rows
                Dim curRow = row + i
                selectedRows.Add(curRow)
            Next
        Next
        Return Me.GetRows(Of T)(selectedRows.ToArray)
    End Function

    ''' <summary>
    ''' 获取所有选中行的某一列
    ''' </summary>
    ''' <typeparam name="T">返回类型</typeparam>
    ''' <param name="columnName">列名</param>
    ''' <returns>所有选中行指定列的数据</returns>
    Public Function GetSelectedRows(Of T)(columnName As String) As T()
        If Me.AllSelectionRanges.Length = 0 Then Return {}
        Dim selectedRows As New List(Of Integer)
        For Each curSelectionRange In Me.AllSelectionRanges
            Dim row = curSelectionRange.Row
            Dim rows = curSelectionRange.Rows
            For i = 0 To rows - 1
                Dim curRow = row + i
                selectedRows.Add(curRow)
            Next
        Next
        Dim rowData = Me.GetRows(selectedRows.ToArray)
        Dim result As New List(Of T)
        For Each curRowData In rowData
            If Not curRowData.ContainsKey(columnName) Then
                Throw New FrontWorkException($"{Me.Name} doesn't contains column ""{columnName}""!")
            End If
            result.Add(curRowData(columnName))
        Next
        Return result.ToArray
    End Function

    ''' <summary>
    ''' 删除新增但未编辑的行
    ''' </summary>
    Public Sub RemoveUneditedNewRows()
        Dim rows As New List(Of Integer)
        For i = 0 To Me.GetRowCount - 1
            If Me.GetRowSynchronizationState(i) = SynchronizationState.ADDED Then
                Call rows.Add(i)
            End If
        Next
        Call Me.RemoveRows(rows.ToArray)
    End Sub

    Public Sub AddColumns(columns() As ModelColumn) Implements IModelCore.AddColumns
        Call Me.ModelCore.AddColumns(columns)
    End Sub

    Public Sub RemoveColumns(columnNames() As String) Implements IModelCore.RemoveColumns
        Call Me.ModelCore.RemoveColumns(columnNames)
    End Sub

    Public Function GetColumns() As ModelColumn() Implements IModelCore.GetColumns
        Return Me.ModelCore.GetColumns
    End Function

    Public Function GetColumns(columnNames() As String) As ModelColumn() Implements IModelCore.GetColumns
        Return Me.ModelCore.GetColumns(columnNames)
    End Function

    Private Function IModelCore_GetCells(rows() As Integer, columnNames() As String) As Object() Implements IModelCore.GetCells
        Throw New NotImplementedException()
    End Function

    Public Function ToDataTable() As DataTable Implements IModelCore.ToDataTable
        Return Me.ModelCore.ToDataTable
    End Function

    Protected Sub RaiseRefreshedEvent(sender As Object, args As ModelRefreshedEventArgs)
        RaiseEvent Refreshed(sender, args)
    End Sub

    Protected Sub RaiseCellUpdatedEvent(sender As Object, args As ModelCellUpdatedEventArgs)
        RaiseEvent CellUpdated(sender, args)
    End Sub

    Protected Sub RaiseRowUpdatedEvent(sender As Object, args As ModelRowUpdatedEventArgs)
        RaiseEvent RowUpdated(sender, args)
    End Sub

    Protected Sub RaiseRowAddedEvent(sender As Object, args As ModelRowAddedEventArgs)
        RaiseEvent RowAdded(sender, args)
    End Sub

    Protected Sub RaiseBeforeRowRemoveEvent(sender As Object, args As ModelBeforeRowRemoveEventArgs)
        RaiseEvent BeforeRowRemove(sender, args)
    End Sub

    Protected Sub RaiseRowRemovedEvent(sender As Object, args As ModelRowRemovedEventArgs)
        RaiseEvent RowRemoved(sender, args)
    End Sub

    Protected Sub RaiseSelectionRangeChangedEvent(sender As Object, args As ModelSelectionRangeChangedEventArgs)
        RaiseEvent SelectionRangeChanged(sender, args)
    End Sub

    Protected Sub RaiseRowSynchronizationStateChangedEvent(sender As Object, args As ModelRowSynchronizationStateChangedEventArgs)
        RaiseEvent RowSynchronizationStateChanged(sender, args)
    End Sub
End Class
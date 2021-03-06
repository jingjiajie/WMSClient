﻿Imports System.ComponentModel
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

    Protected Property ModelCore As IConfigurableModel

    Public Shadows Property Name As String Implements IModelCore.Name
        Get
            Return MyBase.Name
        End Get
        Set(value As String)
            Me.ModelCore.Name = value
            MyBase.Name = value
        End Set
    End Property

    Public Property Configuration As Configuration Implements IConfigurable.Configuration
        Get
            Return DirectCast(ModelCore, IConfigurableModel).Configuration
        End Get
        Set(value As Configuration)
            DirectCast(ModelCore, IConfigurableModel).Configuration = value
        End Set
    End Property

    Public Property Mode As String Implements IConfigurable.Mode
        Get
            Return DirectCast(ModelCore, IConfigurableModel).Mode
        End Get
        Set(value As String)
            DirectCast(ModelCore, IConfigurableModel).Mode = value
        End Set
    End Property

    Public Property AllSelectionRanges As Range() Implements IModel.AllSelectionRanges
        Get
            Return DirectCast(ModelCore, IConfigurableModel).AllSelectionRanges
        End Get
        Set(value As Range())
            DirectCast(ModelCore, IConfigurableModel).AllSelectionRanges = value
        End Set
    End Property

    Public Property AllSelectionRanges(i As Integer) As Range Implements IModel.AllSelectionRanges
        Get
            Return DirectCast(ModelCore, IConfigurableModel).AllSelectionRanges(i)
        End Get
        Set(value As Range)
            DirectCast(ModelCore, IConfigurableModel).AllSelectionRanges(i) = value
        End Set
    End Property

    Public Property SelectionRange As Range Implements IModel.SelectionRange
        Get
            Return DirectCast(ModelCore, IConfigurableModel).SelectionRange
        End Get
        Set(value As Range)
            DirectCast(ModelCore, IConfigurableModel).SelectionRange = value
        End Set
    End Property

    Default Public Property _Item(row As Integer, column As String) As Object Implements IModel._Item
        Get
            Return DirectCast(ModelCore, IConfigurableModel)(row, column)
        End Get
        Set(value As Object)
            DirectCast(ModelCore, IConfigurableModel)(row, column) = value
        End Set
    End Property

    Default Public Property _Item(row As Integer) As IDictionary(Of String, Object) Implements IModel._Item
        Get
            Return DirectCast(ModelCore, IConfigurableModel)(row)
        End Get
        Set(value As IDictionary(Of String, Object))
            DirectCast(ModelCore, IConfigurableModel)(row) = value
        End Set
    End Property

    Public ReadOnly Property RowCount As Integer Implements IModel.RowCount
        Get
            Return DirectCast(ModelCore, IConfigurableModel).RowCount
        End Get
    End Property

    Public ReadOnly Property ColumnCount As Integer Implements IModel.ColumnCount
        Get
            Return DirectCast(ModelCore, IConfigurableModel).ColumnCount
        End Get
    End Property

    Public Sub New()
        Me.ModelCore = New ConfigurableModelOperator(New ModelCore)
    End Sub

    Public Custom Event Refreshed As EventHandler(Of ModelRefreshedEventArgs) Implements IModelCore.Refreshed
        AddHandler(value As EventHandler(Of ModelRefreshedEventArgs))
            AddHandler DirectCast(ModelCore, IConfigurableModel).Refreshed, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRefreshedEventArgs))
            RemoveHandler DirectCast(ModelCore, IConfigurableModel).Refreshed, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRefreshedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event RowAdded As EventHandler(Of ModelRowAddedEventArgs) Implements IModelCore.RowAdded
        AddHandler(value As EventHandler(Of ModelRowAddedEventArgs))
            AddHandler DirectCast(ModelCore, IConfigurableModel).RowAdded, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRowAddedEventArgs))
            RemoveHandler DirectCast(ModelCore, IConfigurableModel).RowAdded, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRowAddedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event RowUpdated As EventHandler(Of ModelRowUpdatedEventArgs) Implements IModelCore.RowUpdated
        AddHandler(value As EventHandler(Of ModelRowUpdatedEventArgs))
            AddHandler DirectCast(ModelCore, IConfigurableModel).RowUpdated, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRowUpdatedEventArgs))
            RemoveHandler DirectCast(ModelCore, IConfigurableModel).RowUpdated, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRowUpdatedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event RowRemoved As EventHandler(Of ModelRowRemovedEventArgs) Implements IModelCore.RowRemoved
        AddHandler(value As EventHandler(Of ModelRowRemovedEventArgs))
            AddHandler DirectCast(ModelCore, IConfigurableModel).RowRemoved, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRowRemovedEventArgs))
            RemoveHandler DirectCast(ModelCore, IConfigurableModel).RowRemoved, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRowRemovedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event BeforeRowRemove As EventHandler(Of ModelBeforeRowRemoveEventArgs) Implements IModelCore.BeforeRowRemove
        AddHandler(value As EventHandler(Of ModelBeforeRowRemoveEventArgs))
            AddHandler DirectCast(ModelCore, IConfigurableModel).BeforeRowRemove, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelBeforeRowRemoveEventArgs))
            RemoveHandler DirectCast(ModelCore, IConfigurableModel).BeforeRowRemove, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelBeforeRowRemoveEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event CellUpdated As EventHandler(Of ModelCellUpdatedEventArgs) Implements IModelCore.CellUpdated
        AddHandler(value As EventHandler(Of ModelCellUpdatedEventArgs))
            AddHandler DirectCast(ModelCore, IConfigurableModel).CellUpdated, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelCellUpdatedEventArgs))
            RemoveHandler DirectCast(ModelCore, IConfigurableModel).CellUpdated, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelCellUpdatedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event SelectionRangeChanged As EventHandler(Of ModelSelectionRangeChangedEventArgs) Implements IModelCore.SelectionRangeChanged
        AddHandler(value As EventHandler(Of ModelSelectionRangeChangedEventArgs))
            AddHandler DirectCast(ModelCore, IConfigurableModel).SelectionRangeChanged, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelSelectionRangeChangedEventArgs))
            RemoveHandler DirectCast(ModelCore, IConfigurableModel).SelectionRangeChanged, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelSelectionRangeChangedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event RowStateChanged As EventHandler(Of ModelRowStateChangedEventArgs) Implements IModelCore.RowStateChanged
        AddHandler(value As EventHandler(Of ModelRowStateChangedEventArgs))
            AddHandler DirectCast(ModelCore, IConfigurableModel).RowStateChanged, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelRowStateChangedEventArgs))
            RemoveHandler DirectCast(ModelCore, IConfigurableModel).RowStateChanged, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelRowStateChangedEventArgs)
        End RaiseEvent
    End Event

    Public Custom Event CellStateChanged As EventHandler(Of ModelCellStateChangedEventArgs) Implements IModelCore.CellStateChanged
        AddHandler(value As EventHandler(Of ModelCellStateChangedEventArgs))
            AddHandler DirectCast(ModelCore, IConfigurableModel).CellStateChanged, value
        End AddHandler
        RemoveHandler(value As EventHandler(Of ModelCellStateChangedEventArgs))
            RemoveHandler DirectCast(ModelCore, IConfigurableModel).CellStateChanged, value
        End RemoveHandler
        RaiseEvent(sender As Object, e As ModelCellStateChangedEventArgs)
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

    Public Function GetCell(row As Integer, columnName As String) As Object Implements IModel.GetCell
        Return DirectCast(ModelCore, IConfigurableModel).GetCell(row, columnName)
    End Function

    Public Function GetRows(Of T As New)(rows() As Integer) As T() Implements IModel.GetRows
        Return DirectCast(ModelCore, IConfigurableModel).GetRows(Of T)(rows)
    End Function

    Public Function GetRow(Of T As New)(row As Integer) As T Implements IModel.GetRow
        Return DirectCast(ModelCore, IConfigurableModel).GetRow(Of T)(row)
    End Function

    Public Function GetRow(row As Integer) As IDictionary(Of String, Object) Implements IModel.GetRow
        Return DirectCast(ModelCore, IConfigurableModel).GetRow(row)
    End Function

    Public Function AddRow(data As IDictionary(Of String, Object)) As Integer Implements IModel.AddRow
        Return DirectCast(ModelCore, IConfigurableModel).AddRow(data)
    End Function

    Public Sub InsertRow(row As Integer, data As IDictionary(Of String, Object)) Implements IModel.InsertRow
        DirectCast(ModelCore, IConfigurableModel).InsertRow(row, data)
    End Sub

    Public Sub RemoveRow(row As Integer) Implements IModel.RemoveRow
        DirectCast(ModelCore, IConfigurableModel).RemoveRow(row)
    End Sub

    Public Sub RemoveRows(startRow As Integer, rowCount As Integer) Implements IModel.RemoveRows
        DirectCast(ModelCore, IConfigurableModel).RemoveRows(startRow, rowCount)
    End Sub

    Public Sub RemoveSelectedRows() Implements IModel.RemoveSelectedRows
        DirectCast(ModelCore, IConfigurableModel).RemoveSelectedRows()
    End Sub

    Public Function GetRows(rows() As Integer) As IDictionary(Of String, Object)() Implements IModel.GetRows
        Return DirectCast(ModelCore, IConfigurableModel).GetRows(rows)
    End Function

    Public Sub UpdateRow(row As Integer, data As IDictionary(Of String, Object)) Implements IModel.UpdateRow
        DirectCast(ModelCore, IConfigurableModel).UpdateRow(row, data)
    End Sub

    Public Sub UpdateCell(row As Integer, columnName As String, data As Object) Implements IModel.UpdateCell
        DirectCast(ModelCore, IConfigurableModel).UpdateCell(row, columnName, data)
    End Sub

    Public Function DataRowToDictionary(dataRow As DataRow) As IDictionary(Of String, Object) Implements IModel.DataRowToDictionary
        Return DirectCast(ModelCore, IConfigurableModel).DataRowToDictionary(dataRow)
    End Function

    Public Sub UpdateRowState(row As Integer, state As ModelRowState) Implements IModel.UpdateRowState
        DirectCast(ModelCore, IConfigurableModel).UpdateRowState(row, state)
    End Sub

    Public Function GetRowState(row As Integer) As ModelRowState Implements IModel.GetRowState
        Return DirectCast(ModelCore, IConfigurableModel).GetRowState(row)
    End Function

    Public Function ContainsColumn(columnName As String) As Boolean Implements IModel.ContainsColumn
        Return DirectCast(ModelCore, IConfigurableModel).ContainsColumn(columnName)
    End Function

    Public Sub SelectRowsByValues(Of T)(columnName As String, values() As T) Implements IModel.SelectRowsByValues
        DirectCast(ModelCore, IConfigurableModel).SelectRowsByValues(columnName, values)
    End Sub

    Public Function GetSelectedRows(Of T As New)() As T() Implements IModel.GetSelectedRows
        Return DirectCast(ModelCore, IConfigurableModel).GetSelectedRows(Of T)()
    End Function

    Public Function GetSelectedRows() As IDictionary(Of String, Object)() Implements IModel.GetSelectedRows
        Return DirectCast(ModelCore, IConfigurableModel).GetSelectedRows()
    End Function

    Public Function GetSelectedRows(Of T)(columnName As String) As T() Implements IModel.GetSelectedRows
        Return DirectCast(ModelCore, IConfigurableModel).GetSelectedRows(Of T)(columnName)
    End Function

    Public Function GetSelectedRow() As IDictionary(Of String, Object) Implements IModel.GetSelectedRow
        Return DirectCast(ModelCore, IConfigurableModel).GetSelectedRow()
    End Function

    Public Function GetSelectedRow(Of T As New)() As IDictionary(Of String, Object) Implements IModel.GetSelectedRow
        Return DirectCast(ModelCore, IConfigurableModel).GetSelectedRow(Of T)()
    End Function

    Public Function GetSelectedRow(Of T)(columnName As String) As T Implements IModel.GetSelectedRow
        Return DirectCast(ModelCore, IConfigurableModel).GetSelectedRow(Of T)(columnName)
    End Function

    Public Sub RemoveUneditedNewRows() Implements IModel.RemoveUneditedNewRows
        DirectCast(ModelCore, IConfigurableModel).RemoveUneditedNewRows()
    End Sub

    Public Function HasUnsynchronizedUpdatedRow() As Boolean Implements IModel.HasUnsynchronizedUpdatedRow
        Return DirectCast(ModelCore, IConfigurableModel).HasUnsynchronizedUpdatedRow()
    End Function

    Public Sub RefreshView(rows() As Integer) Implements IModel.RefreshView
        DirectCast(ModelCore, IConfigurableModel).RefreshView(rows)
    End Sub

    Public Sub RefreshView(row As Integer) Implements IModel.RefreshView
        DirectCast(ModelCore, IConfigurableModel).RefreshView(row)
    End Sub

    Public Function GetRowSynchronizationStates(rows() As Integer) As SynchronizationState() Implements IModel.GetRowSynchronizationStates
        Return DirectCast(ModelCore, IConfigurableModel).GetRowSynchronizationStates(rows)
    End Function

    Public Sub AddColumns(columns() As ModelColumn) Implements IModelCore.AddColumns
        DirectCast(ModelCore, IConfigurableModel).AddColumns(columns)
    End Sub

    Public Sub UpdateColumn(indexes() As Integer, columns() As ModelColumn) Implements IModelCore.UpdateColumn
        DirectCast(ModelCore, IConfigurableModel).UpdateColumn(indexes, columns)
    End Sub

    Public Sub RemoveColumns(indexes() As Integer) Implements IModelCore.RemoveColumns
        DirectCast(ModelCore, IConfigurableModel).RemoveColumns(indexes)
    End Sub

    Public Function GetColumns() As ModelColumn() Implements IModelCore.GetColumns
        Return DirectCast(ModelCore, IConfigurableModel).GetColumns()
    End Function

    Public Function GetColumns(columnNames() As String) As ModelColumn() Implements IModelCore.GetColumns
        Return DirectCast(ModelCore, IConfigurableModel).GetColumns(columnNames)
    End Function

    Public Function AddRows(data() As IDictionary(Of String, Object)) As Integer() Implements IModelCore.AddRows
        Return DirectCast(ModelCore, IConfigurableModel).AddRows(data)
    End Function

    Public Sub InsertRows(rows() As Integer, data() As IDictionary(Of String, Object)) Implements IModelCore.InsertRows
        DirectCast(ModelCore, IConfigurableModel).InsertRows(rows, data)
    End Sub

    Public Sub RemoveRows(rows() As Integer) Implements IModelCore.RemoveRows
        DirectCast(ModelCore, IConfigurableModel).RemoveRows(rows)
    End Sub

    Public Sub UpdateRows(rows() As Integer, dataOfEachRow() As IDictionary(Of String, Object)) Implements IModelCore.UpdateRows
        DirectCast(ModelCore, IConfigurableModel).UpdateRows(rows, dataOfEachRow)
    End Sub

    Public Sub UpdateCells(rows() As Integer, columnNames() As String, dataOfEachCell() As Object) Implements IModelCore.UpdateCells
        DirectCast(ModelCore, IConfigurableModel).UpdateCells(rows, columnNames, dataOfEachCell)
    End Sub

    Public Sub UpdateRowStates(rows() As Integer, states() As ModelRowState) Implements IModelCore.UpdateRowStates
        DirectCast(ModelCore, IConfigurableModel).UpdateRowStates(rows, states)
    End Sub

    Public Function GetRowStates(rows() As Integer) As ModelRowState() Implements IModelCore.GetRowStates
        Return DirectCast(ModelCore, IConfigurableModel).GetRowStates(rows)
    End Function

    Public Function GetCells(rows() As Integer, columnNames() As String) As Object() Implements IModelCore.GetCells
        Return DirectCast(ModelCore, IConfigurableModel).GetCells(rows, columnNames)
    End Function

    Public Shadows Sub Refresh(args As ModelRefreshArgs) Implements IModelCore.Refresh
        DirectCast(ModelCore, IConfigurableModel).Refresh(args)
    End Sub

    Public Function GetRowCount() As Integer Implements IModelCore.GetRowCount
        Return DirectCast(ModelCore, IConfigurableModel).GetRowCount()
    End Function

    Public Function GetColumnCount() As Integer Implements IModelCore.GetColumnCount
        Return DirectCast(ModelCore, IConfigurableModel).GetColumnCount()
    End Function

    Public Function GetSelectionRanges() As Range() Implements IModelCore.GetSelectionRanges
        Return DirectCast(ModelCore, IConfigurableModel).GetSelectionRanges()
    End Function

    Public Sub SetSelectionRanges(ranges() As Range) Implements IModelCore.SetSelectionRanges
        DirectCast(ModelCore, IConfigurableModel).SetSelectionRanges(ranges)
    End Sub

    Public Function GetRowSynchronizationState(row As Integer) As SynchronizationState Implements IModel.GetRowSynchronizationState
        Return DirectCast(ModelCore, IConfigurableModel).GetRowSynchronizationState(row)
    End Function

    Public Function GetCellState(row As Integer, field As String) As ModelCellState Implements IModel.GetCellState
        Return DirectCast(ModelCore, IConfigurableModel).GetCellState(row, field)
    End Function

    Public Sub UpdateCellState(row As Integer, field As String, states As ModelCellState) Implements IModel.UpdateCellState
        DirectCast(ModelCore, IConfigurableModel).UpdateCellState(row, field, states)
    End Sub

    Public Function GetCellStates(rows() As Integer, fields() As String) As ModelCellState() Implements IModelCore.GetCellStates
        Return DirectCast(ModelCore, IConfigurableModel).GetCellStates(rows, fields)
    End Function

    Public Sub UpdateCellStates(rows() As Integer, fields() As String, states() As ModelCellState) Implements IModelCore.UpdateCellStates
        DirectCast(ModelCore, IConfigurableModel).UpdateCellStates(rows, fields, states)
    End Sub

    Public Function HasErrorCell() As Boolean Implements IModel.HasErrorCell
        Return DirectCast(ModelCore, IConfigurableModel).HasErrorCell()
    End Function

    Public Function HasWarningCell() As Boolean Implements IModel.HasWarningCell
        Return DirectCast(ModelCore, IConfigurableModel).HasWarningCell()
    End Function

    Public Function GetInfo(infoItem As ModelInfo) As Object Implements IModelCore.GetInfo
        Return DirectCast(ModelCore, IConfigurableModel).GetInfo(infoItem)
    End Function

    Public Sub UpdateCellValidationStates(rows() As Integer, fields() As String, states() As ValidationState) Implements IModel.UpdateCellValidationStates
        DirectCast(ModelCore, IConfigurableModel).UpdateCellValidationStates(rows, fields, states)
    End Sub

    Public Sub UpdateCellValidationState(row As Integer, field As String, state As ValidationState) Implements IModel.UpdateCellValidationState
        DirectCast(ModelCore, IConfigurableModel).UpdateCellValidationState(row, field, state)
    End Sub

    Public Sub InsertRows(row As Integer, count As Integer, data() As IDictionary(Of String, Object)) Implements IModel.InsertRows
        DirectCast(ModelCore, IConfigurableModel).InsertRows(row, count, data)
    End Sub
End Class
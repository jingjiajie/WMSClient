'Imports System.ComponentModel

'Public Class ViewModel
'    Private _model As Model

'    ''' <summary>
'    ''' Model对象，用来存取数据
'    ''' </summary>
'    ''' <returns>Model对象</returns>
'    <Description("Model对象"), Category("FrontWork")>
'    Public Property Model As Model
'        Get
'            Return Me._model
'        End Get
'        Set(value As Model)
'            If Me._model IsNot Nothing Then
'                Call Me.UnbindModel()
'            End If
'            Me._model = value
'            If Me._model IsNot Nothing Then
'                Call Me.BindModel()
'            End If
'        End Set
'    End Property

'    ''' <summary>
'    ''' 绑定新的Model，将本View的各种事件绑定到Model上以实现数据变化的同步
'    ''' </summary>
'    Protected Sub BindModel()
'        AddHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
'        AddHandler Me.Model.RowAdded, AddressOf Me.ModelRowAddedEvent
'        AddHandler Me.Model.RowRemoved, AddressOf Me.ModelRowRemovedEvent
'        AddHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
'        AddHandler Me.Model.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
'        AddHandler Me.Model.Refreshed, AddressOf Me.ModelRefreshedEvent

'        Me.ImportData()
'    End Sub

'    ''' <summary>
'    ''' 解绑Model，取消本视图绑定的所有事件
'    ''' </summary>
'    Protected Sub UnbindModel()
'        RemoveHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
'        RemoveHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
'        RemoveHandler Me.Model.RowAdded, AddressOf Me.ModelRowAddedEvent
'        RemoveHandler Me.Model.RowRemoved, AddressOf Me.ModelRowRemovedEvent
'        RemoveHandler Me.Model.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
'        RemoveHandler Me.Model.Refreshed, AddressOf Me.ModelRefreshedEvent

'    End Sub


'    Private Sub ModelSelectionRangeChangedEvent(sender As Object, e As ModelSelectionRangeChangedEventArgs)
'        Dim modelSelectedRow = Me.GetModelSelectedRow
'        If modelSelectedRow < 0 Then
'            Call Me.ClearPanelData()
'            Return
'        Else
'            Me.ImportData()
'        End If
'    End Sub

'    Private Sub ModelRowAddedEvent(sender As Object, e As ModelRowAddedEventArgs)
'        Logger.Debug("TableLayoutView ModelRowAddedEvent: " & Str(Me.GetHashCode))
'    End Sub

'    Private Sub ModelRowRemovedEvent(sender As Object, e As ModelRowRemovedEventArgs)
'        Logger.Debug("TableLayoutView ModelRowRemovedEvent: " & Str(Me.GetHashCode))
'        Me.ImportData()
'    End Sub

'    Private Sub ModelCellUpdatedEvent(sender As Object, e As ModelCellUpdatedEventArgs)
'        Logger.Debug("TableLayoutView ModelCellUpdatedEvent: " & Str(Me.GetHashCode))
'        Logger.SetMode(LogMode.REFRESH_VIEW)
'        Dim modelSelectedRow = Me.GetModelSelectedRow
'        If modelSelectedRow < 0 Then
'            Return
'        End If
'        '如果更新的行不包括本View的目标行，则不刷新
'        For Each posCell In e.UpdatedCells
'            If modelSelectedRow = posCell.Row Then
'                Call Me.ImportField(posCell.ColumnName)
'                Return
'            End If
'        Next
'    End Sub

'    Private Sub ModelRefreshedEvent(sender As Object, e As ModelRefreshedEventArgs)
'        If switcherModelDataUpdatedEvent = False Then Return '开关被关闭则不执行事件
'        Call Me.ImportData()
'    End Sub

'    Private Sub ModelRowUpdatedEvent(sender As Object, e As ModelRowUpdatedEventArgs)
'        If switcherModelDataUpdatedEvent = False Then Return '开关被关闭则不执行事件
'        Logger.Debug("TableLayoutView ModelRowUpdatedEvent: " & Str(Me.GetHashCode))
'        Dim modelSelectedRow = Me.GetModelSelectedRow
'        If modelSelectedRow < 0 Then Return
'        Dim needToUpdate As Boolean = (From indexRow In e.UpdatedRows
'                                       Where indexRow.Index = modelSelectedRow
'                                       Select indexRow.Index).Count > 0
'        If needToUpdate Then
'            Call Me.ImportData()
'        End If
'    End Sub
'End Class

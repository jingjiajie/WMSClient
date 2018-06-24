Imports System.ComponentModel
Imports System.Linq
Imports FrontWork

Public Class ViewModel
    Private _ModelOperationsWrapper As ModelOperationsWrapper
    Private _ViewOperationsWrapper As ViewOperationsWrapper
    Private _Configuration As Configuration
    Private _Mode As String = "default"

    Public Property Configuration As Configuration
        Get
            Return Me._Configuration
        End Get
        Set(value As Configuration)
            Dim oldFields As FieldConfiguration() = {}
            If Me._Configuration IsNot Nothing Then
                oldFields = Me._Configuration.GetFieldConfigurations(Me.Mode)
                RemoveHandler Me._Configuration.ConfigurationChanged, AddressOf Me.ConfigurationChangedEvent
            End If
            Me._Configuration = value
            Dim newFields As FieldConfiguration() = {}
            If Me._Configuration IsNot Nothing Then
                newFields = Me._Configuration.GetFieldConfigurations(Me.Mode)
                AddHandler Me._Configuration.ConfigurationChanged, AddressOf Me.ConfigurationChangedEvent
                Call Me.ConfigurationChangedEvent(Me, Nothing)
            End If
        End Set
    End Property

    Public Property Mode As String
        Get
            Return Me._Mode
        End Get
        Set(value As String)
            If Me._Mode IsNot Nothing AndAlso Me._Mode.Equals(value, StringComparison.OrdinalIgnoreCase) Then
                Return
            End If
            Me._Mode = value
            Call Me.ConfigurationChangedEvent(Me, Nothing)
        End Set
    End Property

    Private Sub ConfigurationChangedEvent(sender As Object, e As ConfigurationChangedEventArgs)
        Call Me.RefreshViewSchema()
    End Sub

    ''' <summary>
    ''' ModelOperationsWrapper对象，用来存取数据
    ''' </summary>
    ''' <returns>ModelOperationsWrapper对象</returns>
    Public Property ModelOperationsWrapper As ModelOperationsWrapper
        Get
            Return Me._ModelOperationsWrapper
        End Get
        Set(value As ModelOperationsWrapper)
            If Me._ModelOperationsWrapper IsNot Nothing Then
                Call Me.UnbindModel()
            End If
            Me._ModelOperationsWrapper = value
            If Me._ModelOperationsWrapper IsNot Nothing Then
                Call Me.BindModel()
            End If
        End Set
    End Property

    ''' <summary>
    ''' ViewOperationsWrapper对象，用来代理View
    ''' </summary>
    ''' <returns>ViewOperationsWrapper对象</returns>
    Public Property ViewOperationsWrapper As ViewOperationsWrapper
        Get
            Return Me._ViewOperationsWrapper
        End Get
        Set(value As ViewOperationsWrapper)
            If Me._ViewOperationsWrapper IsNot Nothing Then
                Call Me.UnbindView()
            End If
            Me._ViewOperationsWrapper = value
            If Me._ViewOperationsWrapper IsNot Nothing Then
                Call Me.BindView()
            End If
        End Set
    End Property

    Private Sub RefreshViewSchema()
        Dim oldColumns = Me.ViewOperationsWrapper.GetColumns
        Dim newColumns
        Me.Configuration.GetFieldConfigurations(Me.Mode)
    End Sub

    Private Function FieldConfigurationsToViewColumn(fields As FieldConfiguration()) As ViewColumn()
        Dim result(fields.Length - 1) As ViewColumn
        For i = 0 To fields.Length - 1
            Dim curField = fields(i)
            Dim newViewColumn = New ViewColumn
            newViewColumn.Name = curField.Name
            newViewColumn.Type = curField.Type.FieldType
            newViewColumn.Values = curField.Values
            result(i) = newViewColumn
        Next
        Return result
    End Function

    ''' <summary>
    ''' 绑定新的Model，将本View的各种事件绑定到Model上以实现数据变化的同步
    ''' </summary>
    Protected Sub BindModel()
        AddHandler Me.ModelOperationsWrapper.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
        AddHandler Me.ModelOperationsWrapper.RowAdded, AddressOf Me.ModelRowAddedEvent
        AddHandler Me.ModelOperationsWrapper.RowRemoved, AddressOf Me.ModelRowRemovedEvent
        AddHandler Me.ModelOperationsWrapper.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
        AddHandler Me.ModelOperationsWrapper.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
        AddHandler Me.ModelOperationsWrapper.Refreshed, AddressOf Me.ModelRefreshedEvent
    End Sub

    ''' <summary>
    ''' 解绑Model，取消本视图绑定的所有事件
    ''' </summary>
    Protected Sub UnbindModel()
        RemoveHandler Me.ModelOperationsWrapper.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
        RemoveHandler Me.ModelOperationsWrapper.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
        RemoveHandler Me.ModelOperationsWrapper.RowAdded, AddressOf Me.ModelRowAddedEvent
        RemoveHandler Me.ModelOperationsWrapper.RowRemoved, AddressOf Me.ModelRowRemovedEvent
        RemoveHandler Me.ModelOperationsWrapper.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
        RemoveHandler Me.ModelOperationsWrapper.Refreshed, AddressOf Me.ModelRefreshedEvent
    End Sub

    Protected Sub BindView()
        AddHandler Me.ViewOperationsWrapper.CellUpdated, AddressOf Me.ViewCellUpdatedEvent
        AddHandler Me.ViewOperationsWrapper.RowUpdated, AddressOf Me.ViewRowUpdatedEvent
        AddHandler Me.ViewOperationsWrapper.RowAdded, AddressOf Me.ViewRowAddedEvent
        AddHandler Me.ViewOperationsWrapper.RowRemoved, AddressOf Me.ViewRowRemovedEvent
        AddHandler Me.ViewOperationsWrapper.SelectionRangeChanged, AddressOf Me.ViewSelectionRangeChangedEvent
    End Sub

    Private Sub ViewSelectionRangeChangedEvent(sender As Object, e As ViewSelectionRangeChangedEventArgs)
        RemoveHandler Me.ModelOperationsWrapper.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
        Me.ModelOperationsWrapper.AllSelectionRanges = e.NewSelectionRanges
        AddHandler Me.ModelOperationsWrapper.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
    End Sub

    Private Sub ViewRowRemovedEvent(sender As Object, e As ViewRowRemovedEventArgs)
        Dim rows = (From r In e.Rows Select r.Index).ToArray

        Try
            RemoveHandler Me.ModelOperationsWrapper.RowRemoved, AddressOf Me.ModelRowRemovedEvent
            Call Me.ModelOperationsWrapper.RemoveRows(rows)
            AddHandler Me.ModelOperationsWrapper.RowRemoved, AddressOf Me.ModelRowRemovedEvent
        Catch ex As FrontWorkException
            Call MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub ViewRowAddedEvent(sender As Object, e As ViewRowAddedEventArgs)
        Dim rows = (From r In e.Rows Select r.Index).ToArray
        Dim data = (From r In e.Rows Select r.RowData).ToArray

        For i = 0 To rows.Length - 1
            data(i) = Me.GetBackwardMappedRowData(data(i), rows(i))
        Next

        Try
            RemoveHandler Me.ModelOperationsWrapper.RowAdded, AddressOf Me.ModelRowAddedEvent
            Call Me.ModelOperationsWrapper.InsertRows(rows, data)
            AddHandler Me.ModelOperationsWrapper.RowAdded, AddressOf Me.ModelRowAddedEvent
        Catch ex As FrontWorkException
            Call MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub ViewRowUpdatedEvent(sender As Object, e As ViewRowUpdatedEventArgs)
        Dim rows = (From r In e.Rows Select r.Index).ToArray
        Dim data = (From r In e.Rows Select r.RowData).ToArray

        For i = 0 To rows.Length - 1
            data(i) = Me.GetBackwardMappedRowData(data(i), rows(i))
        Next

        Try
            RemoveHandler Me.ModelOperationsWrapper.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
            Call Me.ModelOperationsWrapper.UpdateRows(rows, data)
            AddHandler Me.ModelOperationsWrapper.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
        Catch ex As FrontWorkException
            Call MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub ViewCellUpdatedEvent(sender As Object, e As ViewCellUpdatedEventArgs)
        Dim rows = (From c In e.Cells Select c.Row).ToArray
        Dim columnNames = (From c In e.Cells Select c.ColumnName).ToArray
        Dim data = (From c In e.Cells Select c.CellData).ToArray

        For i = 0 To rows.Length - 1
            data(i) = Me.GetBackwardMappedCellData(data(i), columnNames(i), rows(i))
        Next

        Try
            RemoveHandler Me.ModelOperationsWrapper.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
            Call Me.ModelOperationsWrapper.UpdateCells(rows, columnNames, data)
            AddHandler Me.ModelOperationsWrapper.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
        Catch ex As FrontWorkException
            Call MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Protected Sub UnbindView()
        RemoveHandler Me.ViewOperationsWrapper.CellUpdated, AddressOf Me.ViewCellUpdatedEvent
        RemoveHandler Me.ViewOperationsWrapper.RowUpdated, AddressOf Me.ViewRowUpdatedEvent
        RemoveHandler Me.ViewOperationsWrapper.RowAdded, AddressOf Me.ViewRowAddedEvent
        RemoveHandler Me.ViewOperationsWrapper.RowRemoved, AddressOf Me.ViewRowRemovedEvent
        RemoveHandler Me.ViewOperationsWrapper.SelectionRangeChanged, AddressOf Me.ViewSelectionRangeChangedEvent
    End Sub

    Private Sub ModelSelectionRangeChangedEvent(sender As Object, e As ModelSelectionRangeChangedEventArgs)
        RemoveHandler Me.ViewOperationsWrapper.SelectionRangeChanged, AddressOf Me.ViewSelectionRangeChangedEvent
        Me._ViewOperationsWrapper.SetSelectionRanges(e.NewSelectionRange)
        AddHandler Me.ViewOperationsWrapper.SelectionRangeChanged, AddressOf Me.ViewSelectionRangeChangedEvent
    End Sub

    Private Sub ModelRowAddedEvent(sender As Object, e As ModelRowAddedEventArgs)
        Dim indexes = (From r In e.AddedRows Select r.Index).ToArray
        Dim data = (From r In e.AddedRows Select r.RowData).ToArray
        For i = 0 To data.Length - 1
            data(i) = Me.GetForwardMappedRowData(data(i), indexes(i))
        Next
        RemoveHandler Me.ViewOperationsWrapper.RowAdded, AddressOf Me.ViewRowAddedEvent
        Call Me._ViewOperationsWrapper.InsertRows(indexes, data)
        AddHandler Me.ViewOperationsWrapper.RowAdded, AddressOf Me.ViewRowAddedEvent
    End Sub

    Private Sub ModelRowRemovedEvent(sender As Object, e As ModelRowRemovedEventArgs)
        Dim indexes = (From r In e.RemovedRows Select r.Index)
        RemoveHandler Me.ViewOperationsWrapper.RowRemoved, AddressOf Me.ViewRowRemovedEvent
        Call Me._ViewOperationsWrapper.RemoveRows(indexes)
        AddHandler Me.ViewOperationsWrapper.RowRemoved, AddressOf Me.ViewRowRemovedEvent
    End Sub

    Private Sub ModelCellUpdatedEvent(sender As Object, e As ModelCellUpdatedEventArgs)
        Dim rows = (From cellInfo In e.UpdatedCells Select cellInfo.Row).ToArray
        Dim columnNames = (From cellInfo In e.UpdatedCells Select cellInfo.ColumnName).ToArray
        Dim data = (From cellInfo In e.UpdatedCells Select cellInfo.CellData).ToArray

        If Me.Configuration Is Nothing Then
            Throw New FrontWorkException("Configuration is not setted")
        End If
        If Me.ViewOperationsWrapper Is Nothing Then
            Throw New FrontWorkException("View is not set")
        End If

        For i = 0 To rows.Length - 1
            Dim colName = columnNames(i)
            Dim row = rows(i)
            data(i) = Me.GetForwardMappedCellData(data(i), colName, row)
        Next
        RemoveHandler Me.ViewOperationsWrapper.CellUpdated, AddressOf Me.ViewCellUpdatedEvent
        Call Me.ViewOperationsWrapper.UpdateCells(rows, columnNames, data)
        AddHandler Me.ViewOperationsWrapper.CellUpdated, AddressOf Me.ViewCellUpdatedEvent
    End Sub

    Private Sub ModelRefreshedEvent(sender As Object, e As ModelRefreshedEventArgs)
        'TODO 待优化
        Dim rowCount = Me._ViewOperationsWrapper.GetRowCount
        Dim data = Me._ModelOperationsWrapper.GetRows(Util.Range(0, rowCount))
        For i = 0 To data.Length - 1
            data(i) = Me.GetForwardMappedRowData(data(i), i)
        Next
        RemoveHandler Me.ViewOperationsWrapper.RowAdded, AddressOf Me.ViewRowAddedEvent
        RemoveHandler Me.ViewOperationsWrapper.RowRemoved, AddressOf Me.ViewRowRemovedEvent
        Call Me._ViewOperationsWrapper.RemoveRows(Util.Range(0, rowCount))
        Call Me._ViewOperationsWrapper.AddRows(data)
        AddHandler Me.ViewOperationsWrapper.RowAdded, AddressOf Me.ViewRowAddedEvent
        AddHandler Me.ViewOperationsWrapper.RowRemoved, AddressOf Me.ViewRowRemovedEvent
    End Sub

    Private Sub ModelRowUpdatedEvent(sender As Object, e As ModelRowUpdatedEventArgs)
        Dim rows = (From r In e.UpdatedRows Select r.Index).ToArray
        Dim data = (From r In e.UpdatedRows Select r.RowData).ToArray

        '遍历传入数据
        For i = 0 To rows.Length - 1
            Dim curRowNum = rows(i)
            data(i) = Me.GetForwardMappedRowData(data(i), curRowNum)
        Next
        RemoveHandler Me.ViewOperationsWrapper.RowUpdated, AddressOf Me.ViewRowUpdatedEvent
        Call Me.ViewOperationsWrapper.UpdateRows(rows, data)
        AddHandler Me.ViewOperationsWrapper.RowUpdated, AddressOf Me.ViewRowUpdatedEvent
    End Sub

    Private Function GetForwardMappedRowData(rowData As IDictionary(Of String, Object), rowNum As Integer) As IDictionary(Of String, Object)
        For Each kv In rowData
            Dim key = kv.Key
            rowData(key) = Me.GetForwardMappedCellData(rowData(key), key, rowNum)
        Next
        Return rowData
    End Function

    Private Function GetForwardMappedCellData(cellData As Object, fieldName As String, rowNum As Integer) As Object
        Dim fields = Me.Configuration.GetFieldConfigurations(Me.Mode)
        Dim curField = (From f In fields Where f.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase) Select f).First
        If curField Is Nothing Then
            Throw New FrontWorkException($"Field ""{fieldName}"" not found in Configuration!")
        End If
        If curField.ForwardMapper IsNot Nothing Then
            cellData = curField.ForwardMapper.Invoke(cellData, Me, rowNum)
        End If
        Return cellData
    End Function


    Private Function GetBackwardMappedRowData(rowData As IDictionary(Of String, Object), rowNum As Integer) As IDictionary(Of String, Object)
        For Each kv In rowData
            Dim key = kv.Key
            rowData(key) = Me.GetBackwardMappedCellData(rowData(key), key, rowNum)
        Next
        Return rowData
    End Function

    Private Function GetBackwardMappedCellData(cellData As Object, fieldName As String, rowNum As Integer) As Object
        Dim fields = Me.Configuration.GetFieldConfigurations(Me.Mode)
        Dim curField = (From f In fields Where f.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase) Select f).First
        If curField Is Nothing Then
            Throw New FrontWorkException($"Field ""{fieldName}"" not found in Configuration!")
        End If
        If curField.BackwardMapper IsNot Nothing Then
            cellData = curField.BackwardMapper.Invoke(cellData, Me, rowNum)
        End If
        Return cellData
    End Function
End Class

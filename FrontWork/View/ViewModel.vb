Imports System.ComponentModel
Imports System.Linq
Imports FrontWork

Public Class ViewModel
    Private _ModelOperationsWrapper As ModelOperationsWrapper
    Private _ViewOperationsWrapper As ViewOperationsWrapper
    Public Property Configuration As Configuration
    Public Property Mode As String = "default"

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
        Me.ModelOperationsWrapper.AllSelectionRanges = e.NewSelectionRanges
    End Sub

    Private Sub ViewRowRemovedEvent(sender As Object, e As ViewRowRemovedEventArgs)
        Throw New NotImplementedException()
    End Sub

    Private Sub ViewRowAddedEvent(sender As Object, e As ViewRowAddedEventArgs)
        Throw New NotImplementedException()
    End Sub

    Private Sub ViewRowUpdatedEvent(sender As Object, e As ViewRowUpdatedEventArgs)
        Throw New NotImplementedException()
    End Sub

    Private Sub ViewCellUpdatedEvent(sender As Object, e As ViewCellUpdatedEventArgs)
        Dim rows = (From c In e.Cells Select c.Row).ToArray
        Dim columnNames = (From c In e.Cells Select c.ColumnName).ToArray
        Dim data = (From c In e.Cells Select c.CellData).ToArray
        Dim fields = Me.Configuration.GetFieldConfigurations(Me.Mode)

        For i = 0 To columnNames.Length - 1
            Dim columnName = columnNames(i)
            Dim curField = (From f In fields Where f.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase) Select f).First
            '获取视图中的文字
            Dim text As String = data(i)

            '将文字经过ReverseMapper映射成转换后的value
            Dim value As Object
            If Not curField.BackwardMapper Is Nothing Then
                value = curField.BackwardMapper.Invoke(text, rows(i), Me)
            Else
                value = text
            End If
            data(i) = value
        Next

        If rows.Count > 0 Then
            RemoveHandler Me.ModelOperationsWrapper.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
            Try
                Call Me.ModelOperationsWrapper.UpdateRows(rows, data)
            Catch ex As FrontWorkException
                Call MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End Try
            AddHandler Me.ModelOperationsWrapper.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
        End If
    End Sub

    Protected Sub UnbindView()
        RemoveHandler Me.ViewOperationsWrapper.CellUpdated, AddressOf Me.ViewCellUpdatedEvent
        RemoveHandler Me.ViewOperationsWrapper.RowUpdated, AddressOf Me.ViewRowUpdatedEvent
        RemoveHandler Me.ViewOperationsWrapper.RowAdded, AddressOf Me.ViewRowAddedEvent
        RemoveHandler Me.ViewOperationsWrapper.RowRemoved, AddressOf Me.ViewRowRemovedEvent
        RemoveHandler Me.ViewOperationsWrapper.SelectionRangeChanged, AddressOf Me.ViewSelectionRangeChangedEvent
    End Sub

    Private Sub ModelSelectionRangeChangedEvent(sender As Object, e As ModelSelectionRangeChangedEventArgs)
        Me._ViewOperationsWrapper.SetSelectionRanges(e.NewSelectionRange)
    End Sub

    Private Sub ModelRowAddedEvent(sender As Object, e As ModelRowAddedEventArgs)
        Dim indexes = (From r In e.AddedRows Select r.Index).ToArray
        Dim data = (From r In e.AddedRows Select r.RowData).ToArray
        Call Me._ViewOperationsWrapper.InsertRows(indexes, data)
    End Sub

    Private Sub ModelRowRemovedEvent(sender As Object, e As ModelRowRemovedEventArgs)
        Dim indexes = (From r In e.RemovedRows Select r.Index)
        Call Me._ViewOperationsWrapper.RemoveRows(indexes)
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
        '获取当前的Configuration
        Dim fieldConfigurations = Me.Configuration.GetFieldConfigurations(Me.Mode)
        If fieldConfigurations Is Nothing Then
            Logger.PutMessage("Configuration not found!")
        End If
        For i = 0 To rows.Length - 1
            Dim colName = columnNames(i)
            Dim field = (From f In fieldConfigurations Where f.Name.Equals(colName, StringComparison.OrdinalIgnoreCase) Select f).FirstOrDefault
            If Not field.Visible Then Return
            '传入数据
            '否则开始导入值
            '先计算值，过一遍Mapper
            Dim value = data(i)
            Dim text As String
            If Not field.ForwardMapper Is Nothing Then
                text = field.ForwardMapper.Invoke(value, rows(i), Me)
            Else
                text = value?.ToString
            End If
            data(i) = text
        Next
        Call Me.ViewOperationsWrapper.UpdateCells(rows, columnNames, data)
    End Sub

    Private Sub ModelRefreshedEvent(sender As Object, e As ModelRefreshedEventArgs)
        'TODO 待优化
        Dim rowCount = Me._ViewOperationsWrapper.GetRowCount
        Call Me._ViewOperationsWrapper.RemoveRows(Util.Range(0, rowCount))
        Call Me._ViewOperationsWrapper.AddRows(Me._ModelOperationsWrapper.GetRows(Util.Range(0, rowCount)))
    End Sub

    Private Sub ModelRowUpdatedEvent(sender As Object, e As ModelRowUpdatedEventArgs)
        Dim rows = (From r In e.UpdatedRows Select r.Index).ToArray
        Dim data = (From r In e.UpdatedRows Select r.RowData).ToArray

        If Me.Configuration Is Nothing Then
            Throw New FrontWorkException("Configuration is not setted")
        End If
        If Me.ViewOperationsWrapper Is Nothing Then
            Throw New FrontWorkException("View is not set")
        End If
        '获取当前的Configuration
        Dim fieldConfiguration = Me.Configuration.GetFieldConfigurations(Me.Mode)
        If fieldConfiguration Is Nothing Then
            Throw New FrontWorkException("Configuration not found!")
        End If
        '遍历传入数据
        For i = 0 To rows.Length - 1
            Dim curRow = rows(i)
            Dim curData = data(i)
            '遍历列（Configuration)
            For Each curField In fieldConfiguration
                If Not curField.Visible Then Continue For
                If Not curData.ContainsKey(curField.Name) Then
                    '在对象中找不到Configuration描述的字段，直接报错，并接着下一个字段
                    Logger.PutMessage("Field """ + curField.Name + """ not found in model")
                    Continue For
                End If
                '否则开始Push值
                '先计算值，过一遍Mapper
                Dim value = curData(curField.Name)
                Dim text As String
                If Not curField.ForwardMapper Is Nothing Then
                    text = curField.ForwardMapper.Invoke(value, curRow, Me.ViewOperationsWrapper)
                Else
                    text = value?.ToString
                End If
                data(i)(curField.Name) = text
            Next
        Next
        Call Me.ViewOperationsWrapper.UpdateRows(rows, data)
    End Sub

    Private Function GetForwardMappedRow(rowData As IDictionary(Of String, Object)) As IDictionary(Of String, Object)

    End Function
End Class

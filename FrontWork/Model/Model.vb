Imports System.ComponentModel
Imports System.Linq
Imports FrontWork

''' <summary>
''' 模型类
''' </summary>
Public Class Model
    Inherits UserControl
    Implements IModel

    Private _selectionRange As Range() = New Range() {}
    Private _configuration As Configuration
    Private _dicRowGuid As New Dictionary(Of DataRow, Guid)
    Private _mode As String = "default"
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label1 As Label
    Private _dicRowSyncState As New Dictionary(Of DataRow, SynchronizationState)

    Public Shadows Property Name As String Implements IModel.Name
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
    Public Property Configuration As Configuration Implements IModel.Configuration
        Get
            Return Me._configuration
        End Get
        Set(value As Configuration)
            If Me._configuration IsNot Nothing Then
                RemoveHandler Me._configuration.ConfigurationChanged, AddressOf Me.ConfigurationChanged
            End If
            Me._configuration = value
            If Me._configuration IsNot Nothing Then
                Call Me.InitDataTable()
                AddHandler Me._configuration.ConfigurationChanged, AddressOf Me.ConfigurationChanged
            End If
        End Set
    End Property

    ''' <summary>
    ''' 数据表
    ''' </summary>
    ''' <returns></returns>
    Private Property Data As New DataTable

    Public Sub New()

    End Sub

    ''' <summary>
    ''' 数据行数
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    Public ReadOnly Property RowCount As Long Implements IModel.RowCount
        Get
            Return Me.Data.Rows.Count
        End Get
    End Property

    ''' <summary>
    ''' 数据列数
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    Public ReadOnly Property ColumnCount As Long Implements IModel.ColumnCount
        Get
            Return Me.Data.Columns.Count
        End Get
    End Property

    ''' <summary>
    ''' 选区
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    Public Property AllSelectionRanges As Range() Implements IModel.AllSelectionRanges
        Get
            Return Me._selectionRange
        End Get
        Set(value As Range())
            Me._selectionRange = value
            For Each range In value
                Me.BindRangeChangedEventToSelectionRangeChangedEvent(range)
            Next
            RaiseEvent SelectionRangeChanged(Me, New ModelSelectionRangeChangedEventArgs() With {
                                         .NewSelectionRange = value
                                        })
        End Set
    End Property

    ''' <summary>
    ''' 首个选区
    ''' </summary>
    ''' <returns></returns>
    <Browsable(False)>
    Public Property SelectionRange As Range Implements IModel.SelectionRange
        Get
            If Me.AllSelectionRanges Is Nothing Then Return Nothing
            If Me.AllSelectionRanges.Length = 0 Then Return Nothing
            Return Me.SelectionRange(0)
        End Get
        Set(value As Range)
            If value Is Nothing Then
                Me.AllSelectionRanges = {}
            ElseIf Me.AllSelectionRanges Is Nothing Then
                Me.AllSelectionRanges = {value}
            ElseIf Me.AllSelectionRanges.Length = 0 Then
                Me.AllSelectionRanges = {value}
            Else
                Me.SelectionRange(0) = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' 选区
    ''' </summary>
    ''' <param name="i">索引</param>
    ''' <returns></returns>
    <Browsable(False)>
    Public Property SelectionRange(i) As Range
        Get
            Return Me._selectionRange(i)
        End Get
        Set(value As Range)
            Me._selectionRange(i) = value
            Me.BindRangeChangedEventToSelectionRangeChangedEvent(value)
            RaiseEvent SelectionRangeChanged(Me, New ModelSelectionRangeChangedEventArgs() With {
                                         .NewSelectionRange = Me._selectionRange
                                        })
        End Set
    End Property

    Default Public Property _Item(row As Long, column As Long) As Object Implements IModel.Item
        Get
            Return Me.GetCell(row, column)
        End Get
        Set(value As Object)
            Call Me.UpdateCell(row, column, value)
        End Set
    End Property

    Default Public Property _Item(row As Long, columnName As String) As Object Implements IModel.Item
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
    Public Property Mode As String Implements IModel.Mode
        Get
            Return Me._mode
        End Get
        Set(value As String)
            Me._mode = value
            Call Me.ConfigurationChanged(Me, New ConfigurationChangedEventArgs)
        End Set
    End Property

    Private Sub ConfigurationChanged(sender As Object, e As ConfigurationChangedEventArgs)
        Call Me.InitDataTable()
        RaiseEvent Refreshed(Me, New ModelRefreshedEventArgs)
    End Sub

    Private Sub InitDataTable()
        If Me.Configuration Is Nothing Then Return

        Dim fieldConfiguration = Me.Configuration.GetFieldConfigurations(Me.Mode)
        For Each curField In fieldConfiguration
            If Not Me.Data.Columns.Contains(curField.Name) Then
                Dim newColumn As New DataColumn
                newColumn.ColumnName = curField.Name
                newColumn.DataType = GetType(Object)
                Me.Data.Columns.Add(newColumn)
            End If
        Next
    End Sub

    Private Sub BindRangeChangedEventToSelectionRangeChangedEvent(range As Range)
        AddHandler range.RangeChanged, Sub()
                                           RaiseEvent SelectionRangeChanged(Me, New ModelSelectionRangeChangedEventArgs() With {
                                                                       .NewSelectionRange = Me.AllSelectionRanges
                                                                   })
                                       End Sub
    End Sub

    ''' <summary>
    ''' 获取数据表
    ''' </summary>
    ''' <returns></returns>
    Public Function GetDataTable() As DataTable Implements IModel.GetDataTable
        GetDataTable = Me.Data
        Exit Function
    End Function

    Public Function GetCell(row As Long, column As Long) As Object Implements IModel.GetCell
        If row >= Me.Data.Rows.Count Then
            Throw New Exception($"Row:{row} exceeded the max row of Model({Me.Data.Rows.Count - 1})")
        End If
        If column >= Me.Data.Columns.Count Then
            Throw New Exception($"Column:{column} exceeded the max column of Model({Me.Data.Columns.Count - 1})")
        End If
        Dim data As Object = Me.Data.Rows(row)(CType(column, Integer))
        If IsDBNull(data) Then
            Return Nothing
        Else
            Return data
        End If
    End Function

    Public Function GetCell(row As Long, columnName As String) As Object Implements IModel.GetCell
        If row >= Me.Data.Rows.Count Then
            Throw New Exception($"Row:{row} exceeded the max row of Model({Me.Data.Rows.Count - 1})")
        End If
        If Not Me.Data.Columns.Contains(columnName) Then
            Throw New Exception($"Model doesn't contain column:{columnName}")
        End If
        Dim data As Object = Me.Data.Rows(row)(columnName)
        If IsDBNull(data) Then
            Return Nothing
        Else
            Return data
        End If
    End Function

    ''' <summary>
    ''' 获取行
    ''' </summary>
    ''' <param name="rowIDs">行ID</param>
    ''' <returns>相应行数据</returns>
    Public Function GetRows(rowIDs As Guid()) As DataTable Implements IModel.GetRows
        Dim rowNums(rowIDs.Length - 1) As Long
        For i = 0 To rowIDs.Length - 1
            Dim rowID = rowIDs(i)
            Dim rowNum = Me.GetRowNum(rowID)
            If rowNum = -1 Then
                Throw New Exception($"Invalid RowID: {rowID}")
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
    Public Function GetRows(rows As Long()) As DataTable Implements IModel.GetRows
        Dim dataTable = Me.Data.Clone
        Try
            For Each row In rows
                Dim newRow = dataTable.NewRow
                newRow.ItemArray = Me.Data.Rows(row).ItemArray
                dataTable.Rows.Add(newRow)
            Next
        Catch ex As Exception
            Throw New Exception("GetRows failed: " & ex.Message)
        End Try

        Return dataTable
    End Function

    ''' <summary>
    ''' 增加行
    ''' </summary>
    ''' <param name="data">增加行的数据</param>
    ''' <returns>增加的行号</returns>
    Public Function AddRow(data As Dictionary(Of String, Object)) As Long Implements IModel.AddRow
        Return Me.AddRows({data})(0)
    End Function

    ''' <summary>
    ''' 增加行
    ''' </summary>
    ''' <param name="dataOfEachRow">增加行的数据</param>
    ''' <returns>增加的行号</returns>
    Public Function AddRows(dataOfEachRow As Dictionary(Of String, Object)()) As Long() Implements IModel.AddRows
        Dim addRowCount = dataOfEachRow.Length
        Dim oriRowCount = Me.Data.Rows.Count
        Dim insertRows = Util.Range(RowCount, RowCount + addRowCount)
        Call Me.InsertRows(insertRows, dataOfEachRow)
        Return insertRows
    End Function

    ''' <summary>
    ''' 插入行
    ''' </summary>
    ''' <param name="row">插入行行号</param>
    ''' <param name="data">数据</param>
    Public Sub InsertRow(row As Long, data As Dictionary(Of String, Object)) Implements IModel.InsertRow
        Call Me.InsertRows({row}, {data})
    End Sub

    ''' <summary>
    ''' 插入行
    ''' </summary>
    ''' <param name="rows">插入行行号</param>
    ''' <param name="dataOfEachRow">数据</param>
    Public Sub InsertRows(rows As Long(), dataOfEachRow As Dictionary(Of String, Object)()) Implements IModel.InsertRows
        If Me.Configuration Is Nothing Then Throw New Exception($"Configuration not set to Model:{Me.Name}!")
        Dim fields = Configuration.GetFieldConfigurations(Me.Mode)
        Dim indexRowPairs As New List(Of IndexRowPair)
        '原始行每次插入之后，行号会变，所以做调整
        Dim realRowsASC = (From r In rows Order By r Ascending Select r).ToArray
        For i = 0 To realRowsASC.Length - 1
            realRowsASC(i) = realRowsASC(i) + i
        Next
        '开始添加数据
        For i = 0 To realRowsASC.Length - 1
            Dim realRow = realRowsASC(i)
            Dim curData = If(dataOfEachRow(i), New Dictionary(Of String, Object))
            Dim newRow = Me.Data.NewRow
            '置入默认值
            For Each curField In fields
                If curField.DefaultValue Is Nothing Then Continue For
                Dim fieldName = curField.Name
                If Not curData.ContainsKey(fieldName) Then curData.Add(fieldName, Nothing)
                If curData(fieldName) Is Nothing Then
                    curData(fieldName) = curField.DefaultValue.Invoke
                End If
            Next
            '将值写入datatable
            For Each item In curData
                newRow(item.Key) = item.Value
            Next
            Me.Data.Rows.InsertAt(newRow, realRow)
            Dim newIndexRowPair As New IndexRowPair(realRow, Me.GetRowID(Me.Data.Rows(realRow)), If(curData, New Dictionary(Of String, Object)))
            indexRowPairs.Add(newIndexRowPair)
        Next

        RaiseEvent RowAdded(Me, New ModelRowAddedEventArgs() With {
                             .AddedRows = indexRowPairs.ToArray
                            })

        Dim columnCount = Me.Data.Columns.Count
        Dim selectionRanges As New List(Of Range)
        For Each curRow In realRowsASC
            If selectionRanges.Count = 0 OrElse selectionRanges.Last.Row + 1 <> curRow Then
                selectionRanges.Add(New Range(curRow, 0, 1, columnCount))
                Continue For
            End If
            If selectionRanges.Last.Row + 1 = curRow Then
                selectionRanges.Last.Rows += 1
            End If
        Next
        Me.AllSelectionRanges = selectionRanges.ToArray

        Me.UpdateRowSynchronizationStates(realRowsASC, Util.Times(SynchronizationState.UNSYNCHRONIZED, realRowsASC.Length))
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="rowID">删除行ID</param>
    Public Sub RemoveRow(rowID As Guid) Implements IModel.RemoveRow
        Me.RemoveRows({rowID})
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="row">删除行行号</param>
    Public Sub RemoveRow(row As Long) Implements IModel.RemoveRow
        Me.RemoveRows({row})
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="startRow">起始行号</param>
    ''' <param name="rowCount">删除行数</param>
    Public Sub RemoveRows(startRow As Long, rowCount As Long) Implements IModel.RemoveRows
        Me.RemoveRows(Util.Range(startRow, startRow + rowCount))
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="rowIDs">删除行ID</param>
    Public Sub RemoveRows(rowIDs As Guid()) Implements IModel.RemoveRows
        Dim rowNums(rowIDs.Length - 1) As Long
        For i = 0 To rowIDs.Length - 1
            Dim rowID = rowIDs(i)
            Dim rowNum = Me.GetRowNum(rowID)
            If rowNum = -1 Then
                Throw New Exception($"Invalid RowID: {rowID}")
            End If
            rowNums(i) = rowNum
        Next
        Me.RemoveRows(rowNums)
    End Sub

    ''' <summary>
    ''' 删除行
    ''' </summary>
    ''' <param name="rows">删除行行号</param>
    Public Sub RemoveRows(rows As Long()) Implements IModel.RemoveRows
        If rows.Length = 0 Then Return
        Dim indexRowList = New List(Of IndexRowPair)
        Try
            '每次删除行后行号会变，所以要做调整
            Dim realRows(rows.Length - 1) As Long
            For i = 0 To rows.Length - 1
                realRows(i) = rows(i) - i
            Next
            For Each curRowNum In realRows
                Dim newIndexRowPair = New IndexRowPair(curRowNum, Me.GetRowID(Me.Data.Rows(curRowNum)), Me.DataRowToDictionary(Me.Data.Rows(curRowNum)))
                indexRowList.Add(newIndexRowPair)
                Me.Data.Rows.RemoveAt(curRowNum)
            Next
        Catch ex As Exception
            Throw New Exception("RemoveRows failed: " & ex.Message)
        End Try
        RaiseEvent RowRemoved(Me, New ModelRowRemovedEventArgs() With {
                                        .RemovedRows = indexRowList.ToArray
                                   })
        If Me.Data.Rows.Count = 0 Then
            Me.AllSelectionRanges = {}
        Else
            Me.AllSelectionRanges = {New Range(Math.Min(rows.Min, Me.Data.Rows.Count - 1), 0, 1, Me.Data.Columns.Count)}
        End If
    End Sub

    Public Sub RemoveSelectedRows() Implements IModel.RemoveSelectedRows
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
    Public Sub UpdateRow(rowID As Guid, data As Dictionary(Of String, Object)) Implements IModel.UpdateRow
        Me.UpdateRows({rowID}, {data})
    End Sub

    ''' <summary>
    ''' 更新行
    ''' </summary>
    ''' <param name="row">更新行行号</param>
    ''' <param name="data">数据</param>
    Public Sub UpdateRow(row As Long, data As Dictionary(Of String, Object)) Implements IModel.UpdateRow
        Call Me.UpdateRows(
            New Long() {row},
            New Dictionary(Of String, Object)() {data}
        )
    End Sub

    ''' <summary>
    ''' 更新行
    ''' </summary>
    ''' <param name="rowIDs">更新的行ID</param>
    ''' <param name="dataOfEachRow">相应的数据</param>
    Public Sub UpdateRows(rowIDs As Guid(), dataOfEachRow As Dictionary(Of String, Object)()) Implements IModel.UpdateRows
        Dim rowNums(rowIDs.Length - 1) As Long
        For i = 0 To rowIDs.Length - 1
            Dim rowID = rowIDs(i)
            Dim rowNum = Me.GetRowNum(rowID)
            If rowNum = -1 Then
                Throw New Exception($"Invalid RowID: {rowID}")
            End If
            rowNums(i) = rowNum
        Next
        Me.UpdateRows(rowNums, dataOfEachRow)
    End Sub

    ''' <summary>
    ''' 更新行
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <param name="dataOfEachRow">对应的数据</param>
    Public Sub UpdateRows(rows As Long(), dataOfEachRow As Dictionary(Of String, Object)()) Implements IModel.UpdateRows
        Try
            Dim i = 0
            For Each row In rows
                For Each item In dataOfEachRow(i)
                    Dim key = item.Key
                    Dim value = item.Value
                    Me.Data.Rows(row)(key) = value
                Next
                i += 1
            Next

            Dim updatedRows(rows.Length - 1) As IndexRowPair
            For i = 0 To rows.Length - 1
                updatedRows(i) = New IndexRowPair(rows(i), Me.GetRowID(Me.Data.Rows(rows(i))), Me.DataRowToDictionary(Me.Data.Rows(rows(i))))
            Next

            Dim eventArgs = New ModelRowUpdatedEventArgs() With {
                                        .UpdatedRows = updatedRows
                                   }

            RaiseEvent RowUpdated(Me, eventArgs)
            '将被更新的行的同步状态修改为未同步
            Me.UpdateRowSynchronizationStates(rows, Util.Times(SynchronizationState.UNSYNCHRONIZED, rows.Length))

        Catch ex As Exception
            Throw New Exception("UpdateRows failed: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 更新单元格
    ''' </summary>
    ''' <param name="row">行ID</param>
    ''' <param name="columnName">列名</param>
    ''' <param name="data">更新的数据</param>
    Public Sub UpdateCell(row As Guid, columnName As String, data As Object) Implements IModel.UpdateCell
        Me.UpdateCells({row}, {columnName}, {data})
    End Sub

    ''' <summary>
    ''' 更新单元格
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="columnName">列名</param>
    ''' <param name="data">更新的数据</param>
    Public Sub UpdateCell(row As Long, columnName As String, data As Object) Implements IModel.UpdateCell
        Me.UpdateCells(New Long() {row}, New String() {columnName}, New Object() {data})
    End Sub

    ''' <summary>
    ''' 更新单元格
    ''' </summary>
    ''' <param name="rowIDs">行号</param>
    ''' <param name="columnNames">列名</param>
    ''' <param name="dataOfEachCell">对应的数据</param>
    Public Sub UpdateCells(rowIDs As Guid(), columnNames As String(), dataOfEachCell As Object()) Implements IModel.UpdateCells
        Dim rowNums(rowIDs.Length - 1) As Long
        For i = 0 To rowIDs.Length - 1
            Dim rowID = rowIDs(i)
            Dim rowNum = Me.GetRowNum(rowID)
            If rowNum = -1 Then
                Throw New Exception($"Invalid RowID: {rowID}")
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
    Public Sub UpdateCells(rows As Long(), columnNames As String(), dataOfEachCell As Object()) Implements IModel.UpdateCells
        Dim posCellPairs As New List(Of PositionCellPair)
        For i = 0 To rows.Length - 1
            Dim columnName = columnNames(i)
            Dim dataColumn = (From col As DataColumn In Me.Data.Columns
                              Where col.ColumnName.Equals(columnName, StringComparison.OrdinalIgnoreCase)
                              Select col).FirstOrDefault
            If dataColumn Is Nothing Then
                Throw New Exception("UpdateCells failed: column """ & columnName & """ not found in model")
            End If
            Me.Data.Rows(rows(i))(dataColumn) = dataOfEachCell(i)
            posCellPairs.Add(New PositionCellPair(rows(i), Me.GetRowID(Me.Data.Rows(rows(i))), columnName, dataOfEachCell(i)))
        Next

        RaiseEvent CellUpdated(Me, New ModelCellUpdatedEventArgs() With {
                                    .UpdatedCells = posCellPairs.ToArray
                               })
        Me.UpdateRowSynchronizationStates(rows, Util.Times(SynchronizationState.UNSYNCHRONIZED, rows.Length))
    End Sub

    ''' <summary>
    ''' 刷新Model
    ''' </summary>
    ''' <param name="dataTable">数据表</param>
    ''' <param name="ranges">选区</param>
    ''' <param name="syncStates">各行同步状态</param>
    Public Overloads Sub Refresh(dataTable As DataTable, ranges As Range(), syncStates As SynchronizationState()) Implements IModel.Refresh
        '刷新选区
        Me._selectionRange = If(ranges, {})
        For Each range In Me._selectionRange
            Me.BindRangeChangedEventToSelectionRangeChangedEvent(range)
        Next
        '刷新数据
        Me._Data = dataTable
        '刷新同步状态字典
        Call Me._dicRowSyncState.Clear()
        If syncStates IsNot Nothing Then
            For i = 0 To syncStates.Length - 1
                If dataTable.Rows.Count <= i Then
                    Throw New Exception("Length of syncStates exceeded the max row of dataTable")
                End If
                Dim row = dataTable.Rows(i)
                Dim syncState = syncStates(i)
                Me._dicRowSyncState.Add(row, syncState)
            Next
        End If
        '触发刷新事件
        RaiseEvent Refreshed(Me, New ModelRefreshedEventArgs)
    End Sub

    ''' <summary>
    ''' DataRow转字典
    ''' </summary>
    ''' <param name="dataRow">DataRow对象</param>
    ''' <returns>转换结果</returns>
    Protected Function DataRowToDictionary(dataRow As DataRow) As Dictionary(Of String, Object)
        Dim result As New Dictionary(Of String, Object)
        Dim columns = dataRow.Table.Columns
        For Each column As DataColumn In columns
            result.Add(column.ColumnName, dataRow(column))
        Next
        Return result
    End Function

    Private Function GetRowID(row As DataRow) As Guid
        If Not Me._dicRowGuid.ContainsKey(row) Then
            Me._dicRowGuid.Add(row, Guid.NewGuid)
        End If
        Return Me._dicRowGuid(row)
    End Function

    ''' <summary>
    ''' 获取行ID
    ''' </summary>
    ''' <param name="rowNum">行号</param>
    ''' <returns>行ID</returns>
    Public Function GetRowID(rowNum As Long) As Guid Implements IModel.GetRowID
        Return Me.GetRowIDs({rowNum})(0)
    End Function

    ''' <summary>
    ''' 获取行ID
    ''' </summary>
    ''' <param name="rowNums">行号</param>
    ''' <returns>行ID</returns>
    Public Function GetRowIDs(rowNums As Long()) As Guid() Implements IModel.GetRowIDs
        Dim dataRows(rowNums.Length - 1) As DataRow
        Dim rowIDs(rowNums.Length - 1) As Guid
        For i = 0 To rowNums.Length - 1
            Dim rowNum = rowNums(i)
            If Me.Data.Rows.Count <= rowNum Then
                Throw New Exception($"Row {rowNum} exceeded the max row of model")
            End If
            Dim dataRow = Me.Data.Rows(rowNum)
            rowIDs(i) = Me.GetRowID(dataRow)
        Next
        Return rowIDs
    End Function

    Protected Function GetRowNum(rowID As Guid) As Long
        Dim dataRow = (From rg In Me._dicRowGuid Where rg.Value = rowID Select rg.Key).FirstOrDefault
        If dataRow Is Nothing Then Return -1
        Return Me.Data.Rows.IndexOf(dataRow)
    End Function

    Protected Function GetDataRow(rowID As Guid) As DataRow
        Return (From rowGuid In Me._dicRowGuid Where rowGuid.Value = rowID Select rowGuid.Key).FirstOrDefault
    End Function

    ''' <summary>
    ''' 更新行同步状态
    ''' </summary>
    ''' <param name="rowIDs">行ID</param>
    ''' <param name="syncStates">同步状态</param>
    Public Sub UpdateRowSynchronizationStates(rowIDs As Guid(), syncStates As SynchronizationState()) Implements IModel.UpdateRowSynchronizationStates
        If rowIDs.Length <> syncStates.Length Then
            Throw New Exception("Length of rows must be same of the length of syncStates")
        End If
        Dim rowNums(rowIDs.Length - 1) As Long
        For i = 0 To rowIDs.Length - 1
            Dim rowID = rowIDs(i)
            Dim rowNum = Me.GetRowNum(rowID)
            If rowNum < 0 Then
                Throw New Exception($"Row ID:{rowID} not found!")
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
    Public Sub UpdateRowSynchronizationStates(rows As Long(), syncStates As SynchronizationState()) Implements IModel.UpdateRowSynchronizationStates
        If rows.Length <> syncStates.Length Then
            Throw New Exception("Length of rows must be same of the length of syncStates")
        End If
        Dim updatedRows = New List(Of IndexRowSynchronizationStatePair)
        For i = 0 To rows.Length - 1
            Dim row = rows(i)
            Dim syncState = syncStates(i)
            If row >= Me.Data.Rows.Count Then
                Throw New Exception($"Row {row} exceeded the max row of model")
            End If
            Me.SetRowSynchronizationState(Me.Data.Rows(row), syncState)
            Dim newIndexRowSynchronizationStatePair = New IndexRowSynchronizationStatePair(row, Me.GetRowID(row), syncState)
            updatedRows.Add(newIndexRowSynchronizationStatePair)
        Next
        Dim eventArgs = New ModelRowSynchronizationStateChangedEventArgs(updatedRows.ToArray)
        RaiseEvent RowSynchronizationStateChanged(Me, eventArgs)
    End Sub

    ''' <summary>
    ''' 更新行同步状态
    ''' </summary>
    ''' <param name="row">行号</param>
    ''' <param name="syncState">同步状态</param>
    Public Sub UpdateRowSynchronizationState(row As Long, syncState As SynchronizationState) Implements IModel.UpdateRowSynchronizationState
        Call Me.UpdateRowSynchronizationStates({row}, {syncState})
    End Sub

    ''' <summary>
    ''' 更新行同步状态
    ''' </summary>
    ''' <param name="rowID">行ID</param>
    ''' <param name="syncState">同步状态</param>
    Public Sub UpdateRowSynchronizationState(rowID As Guid, syncState As SynchronizationState) Implements IModel.UpdateRowSynchronizationState
        Call Me.UpdateRowSynchronizationStates({rowID}, {syncState})
    End Sub

    Private Sub SetRowSynchronizationState(row As DataRow, state As SynchronizationState)
        If Me._dicRowSyncState.ContainsKey(row) Then
            Me._dicRowSyncState(row) = state
        Else
            Me._dicRowSyncState.Add(row, state)
        End If
    End Sub

    Private Function GetRowSynchronizationState(row As DataRow) As SynchronizationState
        If Not Me._dicRowSyncState.ContainsKey(row) Then
            Me._dicRowSyncState.Add(row, SynchronizationState.UNSYNCHRONIZED)
        End If
        Return Me._dicRowSyncState(row)
    End Function

    ''' <summary>
    ''' 获取行同步状态
    ''' </summary>
    ''' <param name="rows">行号</param>
    ''' <returns>同步状态</returns>
    Public Function GetRowSynchronizationStates(rows As Long()) As SynchronizationState() Implements IModel.GetRowSynchronizationStates
        Dim states(rows.Length - 1) As SynchronizationState
        For i = 0 To rows.Length - 1
            Dim rowNum = rows(i)
            If Me.Data.Rows.Count <= rowNum Then
                Throw New Exception($"Row {rowNum} exceeded max row of model!")
            End If
            Dim dataRow = Me.Data.Rows(rowNum)
            states(i) = Me.GetRowSynchronizationState(dataRow)
        Next
        Return states
    End Function

    ''' <summary>
    ''' 获取行同步状态
    ''' </summary>
    ''' <param name="rowIDs">行ID</param>
    ''' <returns>同步状态</returns>
    Public Function GetRowSynchronizationStates(rowIDs As Guid()) As SynchronizationState() Implements IModel.GetRowSynchronizationStates
        Dim rows(rowIDs.Length - 1) As Long
        For i = 0 To rowIDs.Length - 1
            Dim row = Me.GetRowNum(rowIDs(i))
            If row < 0 Then
                Throw New Exception($"Row ID:{rowIDs(i)} not found!")
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
    Public Function GetRowSynchronizationState(row As Long) As SynchronizationState Implements IModel.GetRowSynchronizationState
        Return Me.GetRowSynchronizationStates({row})(0)
    End Function

    ''' <summary>
    ''' 获取行同步状态
    ''' </summary>
    ''' <param name="rowID">行ID</param>
    ''' <returns>同步状态</returns>
    Public Function GetRowSynchronizationStates(rowID As Guid) As SynchronizationState Implements IModel.GetRowSynchronizationState
        Return Me.GetRowSynchronizationStates({rowID})(0)
    End Function

    ''' <summary>
    ''' Model刷新事件
    ''' </summary>
    Public Event Refreshed As EventHandler(Of ModelRefreshedEventArgs) Implements IModel.Refreshed

    ''' <summary>
    ''' 增加行事件
    ''' </summary>
    Public Event RowAdded As EventHandler(Of ModelRowAddedEventArgs) Implements IModel.RowAdded

    ''' <summary>
    ''' 更新行数据事件
    ''' </summary>
    Public Event RowUpdated As EventHandler(Of ModelRowUpdatedEventArgs) Implements IModel.RowUpdated

    ''' <summary>
    ''' 删除行事件
    ''' </summary>
    Public Event RowRemoved As EventHandler(Of ModelRowRemovedEventArgs) Implements IModel.RowRemoved

    ''' <summary>
    ''' 单元格数据更新事件
    ''' </summary>
    Public Event CellUpdated As EventHandler(Of ModelCellUpdatedEventArgs) Implements IModel.CellUpdated

    ''' <summary>
    ''' 选区改变事件
    ''' </summary>
    Public Event SelectionRangeChanged As EventHandler(Of ModelSelectionRangeChangedEventArgs) Implements IModel.SelectionRangeChanged

    ''' <summary>
    ''' 行同步状态改变事件
    ''' </summary>
    Public Event RowSynchronizationStateChanged As EventHandler(Of ModelRowSynchronizationStateChangedEventArgs) Implements IModel.RowSynchronizationStateChanged

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Model))
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
        Me.Label1.Location = New System.Drawing.Point(0, 140)
        Me.Label1.Margin = New System.Windows.Forms.Padding(0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(180, 40)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Model"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Model
        '
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.DoubleBuffered = True
        Me.Name = "Model"
        Me.Size = New System.Drawing.Size(180, 180)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Private Sub Model_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Me.DesignMode Then Me.Visible = False
        Call Me.InitializeComponent()
    End Sub
End Class

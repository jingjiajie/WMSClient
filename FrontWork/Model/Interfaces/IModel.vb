Imports FrontWork
''' <summary>
''' Model接口
''' </summary>
Public Interface IModel
    Inherits IModelCore

    Property AllSelectionRanges As Range()
    Property AllSelectionRanges(i As Integer) As Range
    Property SelectionRange As Range
    Default Property _Item(row As Integer, column As String) As Object
    Default Property _Item(row As Integer) As IDictionary(Of String, Object)
    ReadOnly Property RowCount As Integer
    ReadOnly Property ColumnCount As Integer
    Function GetCell(row As Integer, columnName As String) As Object
    Overloads Function GetRows(Of T As New)(rows() As Integer) As T()
    Function GetRow(Of T As New)(row As Integer) As T
    Function GetRow(row As Integer) As IDictionary(Of String, Object)
    Function AddRow(data As IDictionary(Of String, Object)) As Integer
    Overloads Sub InsertRows(row As Integer, count As Integer, data As IDictionary(Of String, Object)())
    Sub InsertRow(row As Integer, data As IDictionary(Of String, Object))
    Sub RemoveRow(row As Integer)
    Overloads Sub RemoveRows(startRow As Integer, rowCount As Integer)
    Sub RemoveSelectedRows()
    Sub UpdateRow(row As Integer, data As IDictionary(Of String, Object))
    Sub UpdateCell(row As Integer, columnName As String, data As Object)
    Function DataRowToDictionary(dataRow As DataRow) As IDictionary(Of String, Object)
    Sub UpdateRowState(row As Integer, state As ModelRowState)
    Function GetRowState(row As Integer) As ModelRowState
    Function ContainsColumn(columnName As String) As Boolean
    Sub SelectRowsByValues(Of T)(columnName As String, values() As T)
    Function GetSelectedRows(Of T As New)() As T()
    Function GetSelectedRows() As IDictionary(Of String, Object)()
    Function GetSelectedRows(Of T)(columnName As String) As T()
    Function GetSelectedRow() As IDictionary(Of String, Object)
    Function GetSelectedRow(Of T As New)() As IDictionary(Of String, Object)
    Function GetSelectedRow(Of T)(columnName As String) As T
    Sub RemoveUneditedNewRows()
    Function HasUnsynchronizedUpdatedRow() As Boolean
    Function HasErrorCell() As Boolean
    Function HasWarningCell() As Boolean
    Sub RefreshView(rows() As Integer)
    Sub RefreshView(row As Integer)
    Function GetRowSynchronizationState(row As Integer) As SynchronizationState
    Function GetRowSynchronizationStates(rows As Integer()) As SynchronizationState()
    Function GetCellState(row As Integer, field As String) As ModelCellState
    Sub UpdateCellState(row As Integer, field As String, state As ModelCellState)
    Sub UpdateCellValidationStates(rows As Integer(), fields As String(), states As ValidationState())
    Sub UpdateCellValidationState(row As Integer, field As String, state As ValidationState)
End Interface
Imports System.ComponentModel
Imports System.Linq
Imports FrontWork


Public Class TabView
    Implements IView
    Private _configuration As Configuration
    Private _model As IModel
    Private _mode As String = "default"
    Private _columnName As String

    <Description("配置中心"), Category("FrontWork")>
    Public Property Configuration As Configuration
        Get
            Return _configuration
        End Get
        Set(value As Configuration)
            _configuration = value
            Call Me.TryBindModel()
        End Set
    End Property

    <Description("Model对象"), Category("FrontWork")>
    Public Property Model As IModel
        Get
            Return _model
        End Get
        Set(value As IModel)
            If Me._model IsNot Nothing Then
                Call Me.UnbindModel()
            End If
            _model = value
            Call Me.TryBindModel()
        End Set
    End Property

    <Description("绑定Model的列名"), Category("FrontWork")>
    Public Property ColumnName As String
        Get
            Return _columnName
        End Get
        Set(value As String)
            _columnName = value
            Call Me.TryBindModel()
        End Set
    End Property

    <Description("配置模式"), Category("FrontWork")>
    Public Property Mode As String Implements IView.Mode
        Get
            Return _mode
        End Get
        Set(value As String)
            _mode = value
            Call Me.TryBindModel()
        End Set
    End Property

    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
    End Sub

    Private Sub TabView_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        Call Me.AdjustItemSize()
    End Sub

    Private Sub AdjustItemSize()
        Dim rowCount = If(Me.TabControl.RowCount = 0, 1, Me.TabControl.RowCount)
        Dim oriItemSize = Me.TabControl.ItemSize
        Me.TabControl.ItemSize = New Size(oriItemSize.Width, Me.Height / rowCount)
    End Sub

    Private Sub TryBindModel()
        If Me.DesignMode Then Return
        If String.IsNullOrWhiteSpace(Me.Mode) Then
            Return
        ElseIf String.IsNullOrWhiteSpace(Me.ColumnName) Then
            Return
        ElseIf Me.Model Is Nothing Then
            Return
        ElseIf Me.Configuration Is Nothing Then
            Return
        End If
        Call Me.BindModel()
    End Sub

    Private Sub BindModel()
        If Me.DesignMode Then Return
        AddHandler Me.Model.RowAdded, AddressOf Me.ModelRowAddedEvent
        AddHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
        AddHandler Me.Model.RowRemoved, AddressOf Me.ModelRowRemovedEvent
        AddHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
        AddHandler Me.Model.Refreshed, AddressOf Me.ModelRefreshedEvent
        AddHandler Me.Model.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent

        Call Me.ImportCells()
        Call Me.RefreshSelectionRange()
    End Sub

    Private Sub UnbindModel()
        If Me.DesignMode Then Return
        RemoveHandler Me.Model.RowAdded, AddressOf Me.ModelRowAddedEvent
        RemoveHandler Me.Model.RowUpdated, AddressOf Me.ModelRowUpdatedEvent
        RemoveHandler Me.Model.RowRemoved, AddressOf Me.ModelRowRemovedEvent
        RemoveHandler Me.Model.CellUpdated, AddressOf Me.ModelCellUpdatedEvent
        RemoveHandler Me.Model.Refreshed, AddressOf Me.ModelRefreshedEvent
        RemoveHandler Me.Model.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent

    End Sub

    Private Sub ModelRefreshedEvent(sender As Object, e As ModelRefreshedEventArgs)
        Call Me.ImportCells()
        Call Me.RefreshSelectionRange()
    End Sub

    Private Sub ImportCells(Optional rows As Integer() = Nothing)
        If Me.DesignMode Then Return
        If Me.Model Is Nothing Then
            Throw New FrontWorkException($"Model not set in {Me.Name}!")
        End If
        Dim fieldConfigurations = Me.Configuration.GetFieldConfigurations(Me.Mode)
        If fieldConfigurations Is Nothing Then
            Throw New FrontWorkException($"Mode Configuration:""{Me.Mode}"" not found!")
        End If
        Dim field = (From f In fieldConfigurations Where f.Name.Equals(Me.ColumnName, StringComparison.OrdinalIgnoreCase) Select f).FirstOrDefault
        '如果列名在Configuration里不存在，或者在Model里不存在的情况，直接返回
        If field Is Nothing Then Return
        If Not Me.Model.ContainsColumn(Me.ColumnName) Then Return
        RemoveHandler Me.TabControl.SelectedIndexChanged, AddressOf Me.TabControl_SelectedIndexChanged
        If rows Is Nothing Then
            Call Me.TabControl.TabPages.Clear()
            For i = 0 To Me.Model.RowCount - 1
                Me.TabControl.TabPages.Add(String.Empty)
            Next
            rows = Util.ToArray(Of Integer)(Util.Range(0, Me.Model.RowCount))
        End If
        AddHandler Me.TabControl.SelectedIndexChanged, AddressOf Me.TabControl_SelectedIndexChanged
        For i = 0 To rows.Length - 1
            Dim row = rows(i)
            Dim value = Me.Model(row, Me.ColumnName)
            Dim text As String
            If field.ForwardMapper IsNot Nothing Then
                Dim ret = field.ForwardMapper.Invoke(Me, value)
                If String.IsNullOrWhiteSpace(ret) Then
                    text = vbTab
                Else
                    text = ret.ToString
                End If
            Else
                If String.IsNullOrWhiteSpace(value) Then
                    text = vbTab
                Else
                    text = value.ToString
                End If
            End If
            Me.TabControl.TabPages.Item(row).Text = text
        Next
    End Sub

    Private Sub ModelSelectionRangeChangedEvent(sender As Object, e As ModelSelectionRangeChangedEventArgs)
        Call Me.RefreshSelectionRange()
    End Sub

    Private Sub ModelCellUpdatedEvent(sender As Object, e As ModelCellUpdatedEventArgs)
        Call Me.ImportCells((From posCell In e.UpdatedCells
                             Where posCell.ColumnName.Equals(Me.ColumnName, StringComparison.OrdinalIgnoreCase)
                             Select CInt(posCell.Row)).ToArray)
    End Sub

    Private Sub ModelRowRemovedEvent(sender As Object, e As ModelRowRemovedEventArgs)
        RemoveHandler Me.TabControl.SelectedIndexChanged, AddressOf Me.TabControl_SelectedIndexChanged
        For Each indexDataRow In e.RemovedRows
            Me.TabControl.TabPages.RemoveAt(indexDataRow.Index)
        Next
        AddHandler Me.TabControl.SelectedIndexChanged, AddressOf Me.TabControl_SelectedIndexChanged
    End Sub

    Private Sub ModelRowUpdatedEvent(sender As Object, e As ModelRowUpdatedEventArgs)
        Call Me.ImportCells((From indexRow In e.UpdatedRows
                             Select CInt(indexRow.Index)).ToArray)
    End Sub

    Private Sub ModelRowAddedEvent(sender As Object, e As ModelRowAddedEventArgs)
        Dim rows As Integer() = (From item In e.AddedRows Select CInt(item.Index)).ToArray
        For i = 0 To rows.Length - 1
            Dim row = rows(i)
            Me.TabControl.TabPages.Insert(row, "")
            Me.TabControl.TabPages(row).Margin = New Padding(0)
            Me.TabControl.TabPages(row).Padding = New Padding(0)
        Next
        '刷新数据
        Call Me.ImportCells(rows)
    End Sub

    Private Sub TabControl_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl.SelectedIndexChanged
        If Me.DesignMode Then Return
        If Me.Model Is Nothing Then
            Throw New FrontWorkException($"Model not set in {Me.Name}!")
        End If
        RemoveHandler Me.Model.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
        Me.Model.SelectionRange = New Range(Me.TabControl.SelectedIndex, 0, 1, Me.Model.ColumnCount)
        AddHandler Me.Model.SelectionRangeChanged, AddressOf Me.ModelSelectionRangeChangedEvent
    End Sub

    Private Sub RefreshSelectionRange()
        If Me.DesignMode Then Return
        If Me.Model Is Nothing Then
            Throw New FrontWorkException($"Model not set in {Me.Name}!")
        End If
        RemoveHandler Me.TabControl.SelectedIndexChanged, AddressOf Me.TabControl_SelectedIndexChanged
        If Me.Model.SelectionRange IsNot Nothing Then
            Me.TabControl.SelectedIndex = Me.Model.SelectionRange.Row
        End If
        AddHandler Me.TabControl.SelectedIndexChanged, AddressOf Me.TabControl_SelectedIndexChanged
    End Sub

    Private Sub TabView_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class

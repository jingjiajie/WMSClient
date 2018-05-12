Imports System.Threading

''' <summary>
''' 日志模式
''' </summary>
Friend Enum LogMode As Integer
    DEFAULT_MODE
    INIT_VIEW
    LOAD_MODE_METHODLISTENER
    REFRESH_VIEW
    SYNC_FROM_VIEW
    SYNCHRONIZER
End Enum

''' <summary>
''' 日志级别
''' </summary>
Friend Enum LogLevel As Integer
    FATAL_ERROR
    WARNING
    INFOMATION
    DEBUG
End Enum

''' <summary>
''' 日志管理器，用来输出调试信息
''' </summary>
Friend Class Logger
    Private Shared curErrorConfig As LogConfig = Nothing
    Private Shared dicModeErrorConfig As Dictionary(Of LogMode, LogConfig) = Nothing

    Shared Sub New()
        dicModeErrorConfig = New Dictionary(Of LogMode, LogConfig) From {
            {LogMode.DEFAULT_MODE, New LogConfig()},
            {LogMode.INIT_VIEW, New LogConfig("Initializing view")},
            {LogMode.REFRESH_VIEW, New LogConfig("Refreshing view")},
            {LogMode.SYNC_FROM_VIEW, New LogConfig("Synchronizing data from view")},
            {LogMode.LOAD_MODE_METHODLISTENER, New LogConfig("Loading mode MethodListener")},
            {LogMode.SYNCHRONIZER, New LogConfig("Model Adapter failure")}
        }
    End Sub

    ''' <summary>
    ''' 设置日志模式
    ''' </summary>
    ''' <param name="mode">日志模式</param>
    Public Shared Sub SetMode(mode As LogMode)
        curErrorConfig = dicModeErrorConfig(mode)
    End Sub

    ''' <summary>
    ''' 输出信息
    ''' </summary>
    ''' <param name="message">消息文本</param>
    ''' <param name="level">日志级别</param>
    Public Shared Sub PutMessage(message As String, Optional level As LogLevel = LogLevel.FATAL_ERROR)
        Dim levelHint As String = ""
        Select Case level
            Case LogLevel.FATAL_ERROR
                levelHint = "Error"
            Case LogLevel.WARNING
                levelHint = "Warning"
            Case LogLevel.INFOMATION
                levelHint = "Info"
        End Select
        Console.WriteLine("[FrontWork][" + levelHint + "] " + curErrorConfig.Prefix + ": " + message)
    End Sub

    ''' <summary>
    ''' 输出调试信息，仅在Debug模式下输出
    ''' </summary>
    ''' <param name="message"></param>
    Public Shared Sub Debug(message As String)
        Console.WriteLine(message)
    End Sub
End Class


Friend Class LogConfig
    Public Property Prefix As String = Nothing

    Public Sub New()
    End Sub

    Public Sub New(prefix As String)
        Me.Prefix = prefix
    End Sub

End Class

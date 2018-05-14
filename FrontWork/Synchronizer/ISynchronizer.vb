''' <summary>
''' 同步器总接口
''' </summary>
Public Interface ISynchronizer
    ''' <summary>
    ''' 配置中心对象
    ''' </summary>
    ''' <returns></returns>
    Property Configuration As Configuration

    ''' <summary>
    ''' 当前配置模式
    ''' </summary>
    ''' <returns></returns>
    Property Mode As String

    ''' <summary>
    ''' 从服务器拉取数据到Model
    ''' </summary>
    ''' <returns></returns>
    Function Find() As Boolean

    ''' <summary>
    ''' 从Model推送变化的数据到服务器
    ''' </summary>
    ''' <returns></returns>
    Function Save() As Boolean
End Interface

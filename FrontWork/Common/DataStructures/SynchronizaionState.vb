''' <summary>
''' 行同步状态
''' </summary>
Public Enum SynchronizationState
    ''' <summary>
    ''' 同步
    ''' </summary>
    SYNCHRONIZED

    ''' <summary>
    ''' 新增未编辑的行
    ''' </summary>
    ADDED

    ''' <summary>
    ''' 新增并编辑的行
    ''' </summary>
    ADDED_UPDATED

    ''' <summary>
    ''' 已更新
    ''' </summary>
    UPDATED

End Enum

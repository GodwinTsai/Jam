public interface ISingleton
{
    /// <summary>
    /// 热重启时，是否要释放该单例
    /// </summary>
    /// <returns></returns>
    bool DestroyOnReboot();

    /// <summary>
    /// 释放单例的回调
    /// </summary>
    void OnDestroy();

    bool IsDestroying();

    void OnBeforeRebootEvent();

    void OnAfterRebootEvent();
}
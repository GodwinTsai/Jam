/// <summary>
/// 既能手动创建也能自动创建的单例（当调用 Instance时，如果Instance为空则自动创建一个单例）
/// </summary>
public abstract class SingletonClassAuto<T> : ISingleton where T : SingletonClassAuto<T>, new()
{
    private bool _isDestroying;
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (object.Equals(_instance,null))
            {
                _instance = SingletonManager.ForceGetInstance<T>();
                _instance.Init();
            }
            return _instance;
        }
    }

    public static bool IsInstantiated
    {
        get { return _instance != null; }
    }

    /// <summary>
    /// 热重启时，是否自动销毁该单例
    /// </summary>
    // public abstract bool DestroyOnReboot();
    public virtual bool DestroyOnReboot()
    {
        return false;
    }

    /// <summary>
    /// 手动销毁该类型的单例
    /// </summary>
    public static void DestroyInstance()
    {
        if (IsInstantiated)
        {
            SingletonManager.DestroyInstance<T>();
        }
    }

    public virtual void Init()
    {
        
    }

    /// <summary>
    /// 销毁该单例时的回调，子类可以在此清理单例，子类Override时必须调用基类的OnDestroy()
    /// </summary>
    void ISingleton.OnDestroy()
    {
        _instance = null;
        _isDestroying = true;
        OnDestroyEvent();
    }

    protected virtual void OnDestroyEvent()
    {
    }

    public bool IsDestroying()
    {
        return _isDestroying;
    }

    public virtual void OnBeforeRebootEvent()
    {

    }

    public virtual void OnAfterRebootEvent()
    {

    }
}

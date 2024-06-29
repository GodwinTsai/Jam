using UnityEngine;

/// <summary>
/// 既能手动创建也能自动创建的单例（当调用 Instance时，如果Instance为空则自动创建一个单例）
/// </summary>
public abstract class SingletonMonobehaviourAuto<T> : MonoBehaviour, ISingleton where T : MonoBehaviour, ISingleton, new()
{
    private bool _isDestroying;
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = SingletonManager.ForceGetInstance<T>();
            }
            return _instance;
        }
    }

    public static bool IsInstantiated
    {
        get { return _instance != null; }
    }

    /// <summary>
    /// 在Awake时将单例添加到SingletonManager，例如有些单例时挂在场景里的或者是Instantiate()出来的, 子类Overrider时，需要调用base.Awake()
    /// </summary>
    private void Awake()
    {
        _instance = this as T;
        SingletonManager.AddInstance(_instance);

        AwakeEvent();
    }

    protected virtual void AwakeEvent()
    {

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

    /// <summary>
    /// 销毁该单例时的回调，子类可以在此清理单例，子类Override时必须调用基类的OnDestroy()
    /// </summary>
    public void OnDestroy()
    {
        MTDebug.LogFormat("[Singleton] {0} OnDestroy", GetType());
        _instance = null;
        _isDestroying = true;

        // NOTE@yangwei: 防止手动Destory掉了单例的GameObject，但是没有通知SingletonManager
        if (SingletonManager.FindInstance<T>() != null)
        {
            MTDebug.Log("[Singleton] Remove destroyed instance from SingletonManager");
            SingletonManager.DestroyInstance<T>();
        }

        OnDestroyEvent();
    }

    protected virtual void OnDestroyEvent()
    {
    }

    /// <summary>
    /// 热重启时，是否自动销毁该单例
    /// </summary>
    // public abstract bool DestroyOnReboot();
    public virtual bool DestroyOnReboot()
    {
        return false;
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

using System;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager : MonoBehaviour
{
#if UNITY_EDITOR
    public LinkedList<ISingleton> _singletons = new LinkedList<ISingleton>();
#else
    private LinkedList<ISingleton> _singletons = new LinkedList<ISingleton>();
#endif

    private static object _lock = new object();
    private static SingletonManager _instance;
    private static bool _isAppQuitting;
    private static bool _isAppRebooting;

    public static bool IsAppQuitting
    {
        get { return _isAppQuitting; }
    }

    public static bool IsAppRebooting
    {
        get { return _isAppRebooting; }
    }

    /// <summary>
    /// 获取(创建)SingletonManager单例，如果当前正在退出游戏，返回null
    /// </summary>
    private static SingletonManager Instance
    {
        get
        {
            if (_isAppQuitting || _isAppRebooting)
            {
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    GameObject singleton = null;
#if UNITY_EDITOR
                    // 避免在Editor下重新编译后创建多个SingletonManager
                    singleton = GameObject.Find("SingletonManager");
#endif
                    if (singleton == null)
                    {
                        singleton = new GameObject("SingletonManager");
                        _instance = singleton.AddComponent<SingletonManager>();
                    }
                    else
                    {
                        _instance = singleton.GetComponent<SingletonManager>();
                    }
                    if (Application.isPlaying)
                    {
                        DontDestroyHolder.DontDestroy(_instance.gameObject);
                    }
                }

                return _instance;
            }
        }
    }

    public static void Init()
    {
        if (Instance != null)
        {
            MTDebug.Log("[Singleton] SingletonManager Init success");
        }
    }

    /// <summary>
    /// 添加已有的实例对象为单例，例如Instantiate()或者加载场景实例化出来的MonoBehaviour，在其Awake函数里向SingletonManager注册该函数
    /// </summary>
    public static bool AddInstance<T>(T singleton) where T : class, ISingleton
    {
        if (Instance != null)
        {
            T target = FindInstance<T>();
            if (target != null && target != singleton)
            {
                MTDebug.LogWarning($"[Singleton] Aleady contains instance of type {typeof(T)}, ");
                DestroyInstance<T>();
                target = null;
            }

            if (target == null)
            {
                MTDebug.Log("[Singleton] Add instance of type " + typeof(T));
                Instance._singletons.AddLast(singleton);
                if (singleton is MonoBehaviour monoBehaviour)
                {
                    if (monoBehaviour.gameObject == Instance.gameObject)
                    {
                        throw new Exception("[Singleton] Can't add component to SingletonManager");
                    }
                    monoBehaviour.transform.SetParent(Instance.transform);
                }
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 查找类型T的单例，如果没有，返回空
    /// </summary>
    public static T FindInstance<T>() where T : ISingleton
    {
        RemoveInvalidNode();

        if (Instance != null)
        {
            var node = Instance._singletons.First;
            while (node != null)
            {
                if (node.Value is T)
                {
                    return (T)node.Value;
                }

                node = node.Next;
            }
        }

        return default(T);
    }


    /// <summary>
    /// 获取T类型的单例，如果没有则创建一个并返回
    /// </summary>
    public static T ForceGetInstance<T>() where T : class, ISingleton, new()
    {
        T target = null;
        if (Instance != null)
        {
            target = FindInstance<T>();
            if (object.Equals(target, null))
            {

                Type typeOfTarget = typeof(T);
                if (typeOfTarget.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    T existNode = Instance.gameObject.GetComponentInChildren<T>();
                    if (existNode != null)
                    {
                        target = existNode;
                    }
                    else
                    {
                        target = new GameObject(typeOfTarget.ToString()).AddComponent(typeOfTarget) as T;
                        (target as MonoBehaviour).transform.SetParent(Instance.transform);
                    }
                }
                else
                {
                    target = new T();
                }

                MTDebug.Log("[Singleton] Create instance of " + typeOfTarget);
                AddInstance(target);
            }
        }

        return target;
    }

    /// <summary>
    /// 销毁指定类型的单例
    /// </summary>
    public static void DestroyInstance<T>() where T : ISingleton
    {
        RemoveInvalidNode();

        ISingleton target = FindInstance<T>();

        if (target != null)
        {
            Instance._singletons.Remove(target);
            if (!target.IsDestroying())
            {
                MTDebug.Log("[Singleton] Destory singleton " + target.GetType());
                if (target is MonoBehaviour)
                {
                    if (((MonoBehaviour) target).gameObject == Instance.gameObject)
                    {
                        throw new Exception("[Singleton] Can't Destroy SingletonManager");
                    }

                    Destroy(((MonoBehaviour) target).gameObject);
                }
                else
                {
                    try
                    {
                        target.OnDestroy();
                    }
                    catch (Exception e)
                    {
                        MTDebug.LogErrorFormat("[Singleton] Destroy singleton exception, msg:{0}, stacktrace:{1}", e.Message, e.StackTrace);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 热重启时，销毁所有的可以销毁的单例
    /// </summary>
    private static void DestroyAllInstanceOnReboot()
    {
        RemoveInvalidNode();

        if (Instance != null)
        {
            var node = Instance._singletons.First;
            while (node != null)
            {
                if (node.Value.DestroyOnReboot())
                {
                    if (!node.Value.IsDestroying())
                    {
                        MTDebug.Log("[Singleton] Destory singleton " + node.Value.GetType());
                        if (node.Value is MonoBehaviour)
                        {
                            Destroy(((MonoBehaviour) node.Value).gameObject);
                        }
                        else
                        {
                            try
                            {
                                node.Value.OnDestroy();
                            }
                            catch (Exception e)
                            {
                                MTDebug.LogErrorFormat("[Singleton] Destroy singleton exception, msg:{0}, stacktrace:{1}", e.Message, e.StackTrace);
                            }
                        }
                    }
                    var nodeDestroyed = node;
                    node = node.Next;
                    Instance._singletons.Remove(nodeDestroyed);
                }
                else
                {
                    node = node.Next;
                }
            }
        }
    }

    private static void RemoveInvalidNode()
    {
        if (Instance != null)
        {
            var node = Instance._singletons.First;
            while (node != null)
            {
                var theNode = node;
                node = node.Next;

                if (theNode.Value == null || theNode.Value.IsDestroying())
                {
                    MTDebug.Log("[Singleton] Remove null or destroying instance");
                    Instance._singletons.Remove(theNode);
                }
            }
        }
    }

    public static void BeforeReboot()
    {
        if (Instance != null)
        {
            var node = Instance._singletons.First;
            while (node != null)
            {
                node.Value.OnBeforeRebootEvent();
                if (node.Value.DestroyOnReboot() && node.Value is MonoBehaviour)
                {
                    var component = node.Value as MonoBehaviour;
                    component.enabled = false;
                }
                node = node.Next;
            }
        }
    }

    public static void OnReboot()
    {
        DestroyAllInstanceOnReboot();
        _isAppRebooting = true;
    }

    public static void AfterReboot()
    {
        _isAppRebooting = false;

        if (Instance != null)
        {
            var node = Instance._singletons.First;
            while (node != null)
            {
                node.Value.OnAfterRebootEvent();
                node = node.Next;
            }
        }
    }

    private void OnDestroy()
    {
        MTDebug.Log($"[Singleton] Destroy SingletonManager");
        _isAppQuitting = true;
    }

    private void OnApplicationQuit()
    {
        _isAppQuitting = true;
    }

    public static void OnEnterEditMode()
    {
        _isAppQuitting = false;
        _isAppRebooting = false;
    }
}
using UnityEngine;

/// <summary>
/// 不销毁对象的Holder；
/// </summary>
public class DontDestroyHolder : MonoBehaviour
{
    private static GameObject _holder;

    public static GameObject Holder
    {
        get
        {
            if (_holder == null)
            {
                MTDebug.LogError("[DontDestroy] DontDestroyHolder is null");
                GameObject go = new GameObject("DontDestroyHolder");
                go.AddComponent<DontDestroyHolder>();
            }
            return _holder;
        }
    }

    private void Awake()
    {
        if (_holder != null)
        {
            MTDebug.LogError("[DontDestroy] Aleady has instance of DontDestroyHolder");
            Destroy(gameObject);
            return;
        }
        _holder = gameObject;
        DontDestroyOnLoad(gameObject);
    }

    public static void DontDestroy(GameObject target)
    {
        target.transform.SetParent(Holder.transform);
    }
}
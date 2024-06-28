using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

/// <summary>
/// MTDebug自定日志输出函数
/// MT_DEBUG_LOG:如果定义该宏，才会输出Log和Warning日志
/// </summary>
public class MTDebug
{
    public static Action<string> logErrorFormatEvent;

    // Log
    // ReSharper disable Unity.PerformanceAnalysis
    [Conditional("MT_DEBUG_LOG")]
    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// 打印带console pro filter的日志
    /// </summary>
    /// <param name="filter">console pro filter 名称</param>
    /// <param name="message">日志信息</param>
    [Conditional("MT_DEBUG_LOG")]
    public static void LogWithFilter(string filter, object message)
    {
        //UnityEngine.Debug.LogFormat("#<{0}># {1}", filter, message);
        UnityEngine.Debug.LogFormat("<{0}> {1}", filter, message);
    }
    
    /// <summary>
    /// 打印带console pro filter的error日志
    /// </summary>
    /// <param name="filter">console pro filter 名称</param>
    /// <param name="message">日志信息</param>
    [Conditional("MT_DEBUG_LOG")]
    public static void LogErrorWithFilter(string filter, object message)
    {
        UnityEngine.Debug.LogErrorFormat("<{0}> {1}", filter, message);
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// 打印自带类名和方法名的日志
    /// </summary>
    /// <param name="message"></param>
    [Conditional("MT_DEBUG_LOG")]
    public static void LogWithStackInfo(object message = null)
    {
        UnityEngine.Debug.Log($"{GetStackInfo()} {message}");
    }
    
    /// <summary>
    /// 打印带console pro filter和自带类名和方法名的日志
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="message"></param>
    [Conditional("MT_DEBUG_LOG")]
    public static void LogWithFilterStackInfo(string filter, object message = null)
    {
        UnityEngine.Debug.Log($"#<{filter}># {GetStackInfo()} {message}");
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    [Conditional("MT_DEBUG_LOG")]
    public static void Log(UnityEngine.Color color, string message)
    {
        UnityEngine.Debug.Log(ColorMessage(color, message));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    [Conditional("MT_DEBUG_LOG")]
    public static void LogFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(format, args);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    [Conditional("MT_DEBUG_LOG")]
    public static void LogFormat(UnityEngine.Color color, string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(ColorMessage(color, format, args));
    }

    // LogWarning
    // ReSharper disable Unity.PerformanceAnalysis
    [Conditional("MT_DEBUG_LOG")]
    public static void LogWarning(object message)
    {
        UnityEngine.Debug.LogWarning(message);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    [Conditional("MT_DEBUG_LOG")]
    public static void LogWarning(UnityEngine.Color color, string message)
    {
        UnityEngine.Debug.LogWarning(ColorMessage(color, message));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    [Conditional("MT_DEBUG_LOG")]
    public static void LogWarningFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(format, args);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    [Conditional("MT_DEBUG_LOG")]
    public static void LogWarningFormat(UnityEngine.Color color, string format, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(ColorMessage(color, format, args));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// 注意：此方法只适用于暴露开发期间的报错，不会上报Fabric。
    /// </summary>
    /// <param name="message"></param>
    public static void LogErrorWithoutCrashlytics(object message)
    {
        UnityEngine.Debug.LogError(message);
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// 注意：此方法只适用于暴露开发期间的报错，不会上报Fabric。
    /// </summary>
    /// <param name="format"></param>
    /// <param name="args"></param>
    public static void LogErrorWithoutCrashFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(format, args);
    }

    private static Action<object> _errorReporter;

    /// <summary>
    /// 设置错误汇报机制
    /// </summary>
    /// <param name="errorReporter"></param>
    public static void SetErrorReporter(Action<object> errorReporter)
    {
        _errorReporter -= errorReporter;
        _errorReporter += errorReporter;
    }

    // LogError
    // ReSharper disable Unity.PerformanceAnalysis
    public static void LogError(object message)
    {
        UnityEngine.Debug.LogError(message);
        if (_errorReporter != null && message != null)
            _errorReporter(message);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public static void LogError(UnityEngine.Color color, string message)
    {
        UnityEngine.Debug.LogError(ColorMessage(color, message));
        if (_errorReporter != null && message != null)
            _errorReporter(message);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// logerror只输出不包含args的信息
    /// 详细信息通过TrackTechLog记录
    /// 参数不一致会捕获异常并通过打印报错
    /// </summary>
    /// <param name="format">format message，ie：”开启面板报错，当前关卡{0}“</param>
    /// <param name="args">format message 参数，ie：100</param>
    public static void LogErrorFormat(string format, params object[] args)
    {
        // 防止args与实际format不匹配，或者注册了错误回调调用报错
        try
        {
            // OptFormatErrorMsg(format, args);
            Debug.LogErrorFormat(format, args);
            if (_errorReporter != null)
                _errorReporter(string.Format(format, args));
        }
        catch (Exception e)
        {
            Debug.LogError($"msg:{format} argsLength:{(args == null ? "null" : args.Length)} stackTrack:{e.StackTrace}");
        }

    }

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// logerror只输出不包含args的信息
    /// 详细信息通过TrackTechLog记录
    /// 参数不一致会捕获异常并通过打印报错
    /// </summary>
    /// <param name="color">窗口日志的特殊显示颜色</param>
    /// <param name="format">format message，ie：”开启面板报错，当前关卡：{0}“</param>
    /// <param name="args">format message 参数，ie：100</param>
    public static void LogErrorFormat(UnityEngine.Color color, string format, params object[] args)
    {
        // 防止args与实际format不匹配，或者注册了错误回调调用报错
        try
        {
            // OptFormatErrorMsg(format, args);
            UnityEngine.Debug.LogErrorFormat(ColorMessage(color, format, args));
            if (_errorReporter != null)
                _errorReporter(string.Format(format, args));
        }
        catch (Exception e)
        {
            Debug.LogError($"msg:{format} argsLength:{args?.Length} stackTrack:{e.StackTrace}");
        }
    }

    // 如果args不为空且长度大于0，替换args所有元素为"TrackTechLog"
    // 该方法会是的track的查询难度增加，屏蔽掉使用
    private static void OptFormatErrorMsg(string format, params object[] args)
    {
        #if !UNITY_EDITOR // ! MT_DEBUG_LOG // 全部开启，后期如果影响看错误日志，再在debug模式下关闭
        // 防止在非MT_DEBUG_LOG下，args与实际format不匹配，或者注册了错误回调调用报错
        if (args is { Length: > 0 })
        {
            // 详细的错误信息会传输到TrackTechLog
            logErrorFormatEvent?.Invoke(string.Format(format, args));
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = "TrackTechLog";
            }
        }
        #endif
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public static void LogErrorWithStackInfo(object message)
    {
        UnityEngine.Debug.LogError($"{GetStackInfo()}: {message}");
        if (_errorReporter != null && message != null)
            _errorReporter($"{GetStackInfo()} {message}");
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// 只在Editor下提示错误
    /// </summary>
    /// <param name="format"></param>
    /// <param name="args"></param>
    [Conditional("MT_DEBUG_LOG")]
    public static void LogErrorFormatEditor(string format, params object[] args)
    {
#if UNITY_EDITOR
        UnityEngine.Debug.LogErrorFormat(format, args);
#endif
    }

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// 只在Debug包提示错误
    /// </summary>
    /// <param name="message"></param>
    [Conditional("MT_DEBUG_LOG")]
    public static void LogErrorDebug(string message)
    {
        UnityEngine.Debug.LogError(message);
        if (_errorReporter != null && message != null)
        {
            _errorReporter(message);
        }
    }
    
    /// <summary>
    /// 只在Debug包提示错误
    /// </summary>
    /// <param name="message"></param>
    [Conditional("MT_DEBUG_LOG")]
    public static void LogErrorDebugWithStackInfo(string message)
    {
        UnityEngine.Debug.LogError($"{GetStackInfo()}: {message}");
        if (_errorReporter != null && message != null)
        {
            _errorReporter(message);
        }
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// 只在Editor下提示错误,自带堆栈
    /// </summary>
    /// <param name="format"></param>
    /// <param name="args"></param>
    [Conditional("MT_DEBUG_LOG")]
    public static void LogErrorEditorWithStackInfo(object message)
    {
#if UNITY_EDITOR
        UnityEngine.Debug.LogError($"{GetStackInfo()}: {message}");
#endif
    }

    // Color Message Format
    static string ColorMessage(UnityEngine.Color color, object message)
    {
        return string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte) (color.r * 255f), (byte) (color.g * 255f), (byte) (color.b * 255f), message);
    }

    static string ColorMessage(UnityEngine.Color color, string format, params object[] args)
    {
        string formatMsg = String.Format(format, args);
        return string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte) (color.r * 255f), (byte) (color.g * 255f), (byte) (color.b * 255f), formatMsg);
    }
    
    private static string GetStackInfo()
    {
        StackTrace st = new StackTrace(true);
        StackFrame sf = st.GetFrame(2);
        if (sf != null)
        {
            Type declaringType = sf.GetMethod().DeclaringType;
            if (declaringType != null)
            {
                return $"#<{declaringType.Name}># [{sf.GetMethod()}_{sf.GetFileLineNumber()}]";
            }
        }
        
        return "GetStackInfo()获取方法的声明类为NULL";
    }
    
    public static string GetDelegateLog(Delegate action)
    {
        if (action == null) return "null";
        return action.Target?.GetType().Name + "." + action.Method.Name;
    }
}
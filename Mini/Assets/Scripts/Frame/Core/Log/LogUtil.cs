// ==================================================
// @Author: caiguorong
// @Date: 
// @Desc: 
// ==================================================

using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class LogUtil
{
	#region Log Methods
	[Conditional("DEBUG_LOG")]
	public static void Log(object msg)
	{
		Debug.Log(msg);
	}
	
	[Conditional("DEBUG_LOG")]
	public static void LogColor(object msg, Color color)
	{
		var colorMsg = FormatColorMsg(msg, color);
		Debug.Log(colorMsg);
	}
	
	[Conditional("DEBUG_LOG")]
	public static void LogFormat(string msg, params object[] args)
	{
		Debug.LogFormat(msg, args);
	}
	
	[Conditional("DEBUG_LOG")]
	public static void LogWarning(object msg)
	{
		Debug.LogWarning(msg);
	}

	public static void LogException(Exception e)
	{
		Debug.LogException(e);
	}
	
	public static void LogError(object msg)
	{
		LogErrorWithStack(msg);
	}
	
	public static void LogErrorFormat(string format, params object[] args)
	{
		var msg = string.Format(format, args);
		LogErrorWithStack(msg);
	}
	
	[Conditional("DEBUG_LOG")]
	public static void LogErrorDebug(object msg)
	{
		LogErrorWithStack(msg);
	}
	
	public static void LogErrorEditor(object msg)
	{
#if UNITY_EDITOR
		LogErrorWithStack(msg);
#endif
	}
	
	#endregion

	#region Private Methods

	private static void LogErrorWithStack(object msg)
	{
#if DEBUG_LOG
		Debug.LogError($"{msg}\nstack:{GetStackInfo()}");
#else
		Debug.LogError(msg);
#endif
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

	private static string FormatColorMsg(object msg, Color color)
	{
		var colorMsg = $"<color=#{color.r:X2}{color.g:X2}{color.b:X2}>{msg}</color>";
		return colorMsg;
	}

	#endregion
}
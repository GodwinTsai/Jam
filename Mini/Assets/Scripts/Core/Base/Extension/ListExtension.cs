using System.Collections.Generic;

public static class ListExtension
{
    /// <summary>
    /// 快速移除List的Item，会把List的顺序打乱
    /// </summary>
    public static void FastRemove<T>(this List<T> list, T item)
    {
        int itemIndex = list.IndexOf(item);
        if (itemIndex >= 0)
        {
            list[itemIndex] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
        }
    }
        
    /// <summary>
    /// 快速移除List的Item，会把List的顺序打乱
    /// </summary>
    public static void FastRemoveAt<T>(this List<T> list, int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < list.Count)
        {
            list[itemIndex] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
        }
    }

    /// <summary>
    /// 弹出并获取的列表中的最后一个
    /// </summary>
    public static T PopLast<T>(this List<T> list)
    {
        if (list.Count == 0)
        {
            return default;
        }

        var t = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        return t;
    }
}
using System.Collections.Generic;

public static class HashSetExtension
{
    public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> range)
    {
        if (hashSet != null && range != null)
        {
            foreach (var item in range)
            {
                hashSet.Add(item);
            }
        }
    }
        
    public static bool IsNullOrEmpty<T>(this HashSet<T> list)
    {
        return list == null || list.Count == 0;
    }
}

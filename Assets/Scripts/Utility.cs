using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static bool Include<T>(this List<T> list, T obj, bool include = true)
    {
        bool contains = list.Contains(obj);
        if (include)
        {
            if (!contains)
            {
                list.Add(obj);
                return true;
            }
        }
        else
        {
            if (contains)
            {
                list.Remove(obj);
                return true;
            }
        }
        return false;
    }
}

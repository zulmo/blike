using System;
using System.Collections.Generic;

public static class IListExtensions
{
    private static Random Rng = new Random();

    public static int FindIndex<T>(this IList<T> list, T item)
    {
        for (int i = 0, count = list.Count; i < count; ++i)
        {
            if(Object.ReferenceEquals(list[i], item))
            {
                return i;
            }
        }

        return -1;
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static T RandomElement<T>(this IList<T> list)
    {
        return list[list.RandomIndex()];
    }

    public static int RandomIndex<T>(this IList<T> list)
    {
        return Rng.Next(list.Count);
    }
}

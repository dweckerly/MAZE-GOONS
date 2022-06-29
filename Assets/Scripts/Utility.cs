using System;
using System.Collections;
using System.Collections.Generic;

public static class Utility 
{
    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static bool IsSameArray<T>(this T[] arr0, T[] arr1)
    {
        if (arr0.Length != arr1.Length) return false;
        for(int i = 0; i < arr0.Length; i++)
        {
            if (!arr0[i].Equals(arr1[i])) return false;
        }
        return true;
    }
}

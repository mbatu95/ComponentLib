using System;
using System.Collections.Generic;


namespace ListToHashExtension
{
    public static class ListToHashExtension
    {

        public static HashSet<T> ToHashSet<T>(this List<T> list)
        {
            HashSet<T> hash = new HashSet<T>();

            foreach (var element in list)
            {
                hash.Add(element);
            }
            return hash;
        }


        public static List<T> ToList<T>(this HashSet<T> hash)
        {
            List<T> list = new List<T>();

            foreach (var element in hash)
            {
                list.Add(element);
            }
            return list;
        }


    }
}

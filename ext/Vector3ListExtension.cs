using System;
using System.Numerics;
using System.Collections.Generic;


namespace Vector3List
{
    public static class Vector3ListExtension
    {
        

        


        public static List<double> ToList(this Vector3 vec)
        {
            List<double> list = new List<double>{ vec.X, vec.Y, vec.Z };
            list[0] = vec.X;
            list[1] = vec.Y;
            list[2] = vec.Z;
            return list;
        }

        public static Vector3 ToVector3(this List<double> list)
        {
            if (list.Count != 3)
            { throw new Exception("Error: List must only contain 3 elements"); }

            else
            {
                Vector3 vec = new Vector3();
                vec.X = (float)list[0];
                vec.Y = (float)list[1];
                vec.Z = (float)list[2];
                return vec;
            }
        }







    }




}


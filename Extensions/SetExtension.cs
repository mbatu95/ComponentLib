using System;
using System.Collections.Generic;


namespace SetExtension
{
	public static class SetFunctions
	{
		public static bool bIsSubset<T>(List<T> listSubset, List<T> listSuperset)
		{
			bool bIsSubsetResult = true;

			foreach (var element in listSubset)
            {
				if (!(listSuperset.Contains(element)))
                {
					bIsSubsetResult = false;
					return bIsSubsetResult;
                }
            }
			return bIsSubsetResult;

		}

		public static bool bIsSubset<T>(HashSet<T> hashSubset, HashSet<T> hashSuperset)
		{
			bool bIsSubsetResult = true;

			foreach (var element in hashSubset)
			{
				if (!(hashSuperset.Contains(element)))
				{
					bIsSubsetResult = false;
					return bIsSubsetResult;
				}
			}
			return bIsSubsetResult;

		}






		public static bool bAreSetsEqual<T>(List<T> listA, List<T> listB)
        {
			bool bAsubsetB = bIsSubset<T>(listA, listB);
			bool bBsubsetA = bIsSubset<T>(listB, listA);
			bool bResult = bAsubsetB && bBsubsetA;
			return bResult;


        }


		public static List<T> listSetIntersection<T>(List<T> listA, List<T> listB)
        {
			List<T> listIntersection = new List<T>();
			foreach (var element in listA)
            {
				if (listB.Contains(element))
                {
					listIntersection.Add(element);
                }
            }
			return listIntersection;
        }




	}
}
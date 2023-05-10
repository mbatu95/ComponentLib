using System;
using System.Numerics;
using System.Collections.Generic;


namespace VectorExtension
{
    public static class VectorExtension
    {
        public static Vector3 vecFindNormal(this Vector3 vec)
        {
            Vector3 vecNonParallel = Vector3.UnitX;
            int iDecimals = 5;
            Vector3 vecCross = Vector3.Cross(vec, vecNonParallel);
            if (Math.Round(vecCross.fMagnitude(),iDecimals) == 0)
            {
                vecNonParallel = Vector3.UnitY;
                vecCross = Vector3.Cross(vec, vecNonParallel);
            }

            Vector3 vecNormal = vecNonParallel - Vector3.Dot(vec, vecNonParallel) * vec;
            vecNormal = Vector3.Normalize(vecNormal);
            return vecNormal;
        }

        public static float fMagnitude(this Vector3 vec)
        {
            float fMag = (float)Math.Sqrt(Vector3.Dot(vec, vec));
            return fMag;
        }

        public static float fSignedAngle(Vector3 vec1, Vector3 vec2)
        {
            Vector3 v1N = vec1 / vec1.fMagnitude();
            Vector3 v2N = vec2 / vec2.fMagnitude();

            Vector3 vC = Vector3.Cross(v1N, v2N);
            float fSignedAngle = MathF.Asin(vC.fMagnitude());
            return fSignedAngle;

        }


    }
}

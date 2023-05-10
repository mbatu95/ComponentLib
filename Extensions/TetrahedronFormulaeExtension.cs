using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using Hy;
namespace TetrahedronExtension
{
    public static class TetrahedronFormulae
    {

        public static float fSignedTetrahedronVolume(List<Vector3> listVertices)
        {
            if (listVertices.Count != 4)
            {
                throw new ArgumentException("Error: Tetrahedron does not have 4 vertices");
            }

            Vector3 a = listVertices[1] - listVertices[0];
            Vector3 b = listVertices[2] - listVertices[0];
            Vector3 c = listVertices[3] - listVertices[0];

            float fSignedVol = (1f / 6f) * (Vector3.Dot(Vector3.Cross(a, b), c));
            return fSignedVol;


        }

    }
}

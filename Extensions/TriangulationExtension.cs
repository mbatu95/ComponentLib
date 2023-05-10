using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using GeometryExtension;

namespace TriangulationExtension
{



	public static class Triangulation
	{
        public static List<List<Vector3>> listTriangulateConvexSurfaceInterior(List<Vector3> listConvexHull3D, List<Vector3> listInternalPoints3D)
        {
            Vector3 vecA = Vector3.Normalize(listConvexHull3D[1] - listConvexHull3D[0]);

            Vector3 vecB = Vector3.Normalize(listConvexHull3D[2] - listConvexHull3D[0]);

            List<Vector2> listConvexHull2D = new List<Vector2>();
            List<Vector2> listInternalPoints2D = new List<Vector2>();
            Vector2 vecProjection;


            Dictionary<Vector2, Vector3> dicMatchVec2ToVec3 = new Dictionary<Vector2, Vector3>();
            foreach (var vec in listConvexHull3D)
            {
                vecProjection = vecProjectToPlane(vec, vecA, vecB);
                dicMatchVec2ToVec3.Add(vecProjection, vec);
                listConvexHull2D.Add(vecProjection);
            }
            foreach (var vec in listInternalPoints3D)
            {
                vecProjection = vecProjectToPlane(vec, vecA, vecB);
                dicMatchVec2ToVec3.Add(vecProjection, vec);
                listInternalPoints2D.Add(vecProjection);
            }

            List<List<Vector2>> listEdges2D = listTriangulateConvexSurfaceInterior(listConvexHull2D, listInternalPoints2D);

            List < List < Vector3 >> listEdges3D = new List<List<Vector3>>();

            foreach(var listEdge2D in listEdges2D)
            {
                List<Vector3> listEdge3D = new List<Vector3>();
                foreach (var vec2D in listEdge2D)
                {
                    Vector3 vec3D = dicMatchVec2ToVec3[vec2D];
                    listEdge3D.Add(vec3D);
                }
                listEdges3D.Add(listEdge3D);
            }
            return listEdges3D;
            //Vector3 vecNormal = Vector3.Normalize(Vector3.Cross(vecB, vecA));
        }


        public static Vector2 vecProjectToPlane(Vector3 vecToProject, Vector3 vecBasisX, Vector3 vecBasisY)
        {

            float fProjectX = Vector3.Dot(vecToProject, vecBasisX);
            float fProjectY = Vector3.Dot(vecToProject, vecBasisY);

            Vector2 vecNew = new Vector2(fProjectX, fProjectY);
            return vecNew;

        }


        //Note: Does not include linking convex hull
        public static List<List<Vector2>> listTriangulateConvexSurfaceInterior(List<Vector2> listConvexHull, List<Vector2> listInternalPoints)
        {

            List<Triangle2D> listTrianglesInSurface = new List<Triangle2D>();

			List<Vector2> listAddedPoints = new List<Vector2>();
			List<List<Vector2>> listConnections = new List<List<Vector2>>();

            int iConvexHullCount = listConvexHull.Count;
            Triangle2D oTriangle;
            List<Vector2> listOuterEdge;
            List<Vector2> listInnerEdge;
            //Connect Hull points to each other and to original node
            for (int i = 1; i < iConvexHullCount; i++)
            {
                /*listOuterEdge = new List<Vector2>();
                listOuterEdge.Add(listConvexHull[i]);
                listOuterEdge.Add(listConvexHull[i - 1]);
                listConnections.Add(listOuterEdge);

                listInnerEdge = new List<Vector2>();
                listInnerEdge.Add(listConvexHull[i]);
                listInnerEdge.Add(listConvexHull[0]);
                listConnections.Add(listInnerEdge);*/

                oTriangle = new Triangle2D(listConvexHull[0], listConvexHull[i], listConvexHull[i - 1]);
                listTrianglesInSurface.Add(oTriangle);

            }


            /*listOuterEdge = new List<Vector2>();
            listOuterEdge.Add(listConvexHull[0]);
            listOuterEdge.Add(listConvexHull[iConvexHullCount-1]);
            listConnections.Add(listOuterEdge);

            listInnerEdge = new List<Vector2>();
            listInnerEdge.Add(listConvexHull[iConvexHullCount-1]);
            listInnerEdge.Add(listConvexHull[0]);
            listConnections.Add(listInnerEdge);
*/

            oTriangle = new Triangle2D(listConvexHull[0], listConvexHull[iConvexHullCount - 1], listConvexHull[iConvexHullCount - 2]);
            listTrianglesInSurface.Add(oTriangle);

            



            foreach (var vecPoint in listInternalPoints)
            {
				listAddedPoints.Add(vecPoint);
                //Check which triangle it is inside


                List<Triangle2D> listTrianglesContainingPoint = new List<Triangle2D>();
                foreach (var oTriangleInSurface in listTrianglesInSurface)
                {
                    if (bIsInsideTriangle(vecPoint, oTriangleInSurface))
                    {
                        listTrianglesContainingPoint.Add(oTriangleInSurface);
                    }
                }

                //Smallest triangle is first element
                listTrianglesContainingPoint.OrderBy(triangle => triangle.fSignedArea);
                Triangle2D oSmallestTriangle = listTrianglesContainingPoint[0];


                for(int i = 0; i<3; i++)
                {
                    List<Vector2> listEdge = new List<Vector2>();
                    listEdge.Add(vecPoint);
                    listEdge.Add(oSmallestTriangle.listVertices[i]);
                    listConnections.Add(listEdge);

                }
                Triangle2D oNewTriangle;
                for (int i = 1; i < 3; i++)
                {
                    oNewTriangle = new Triangle2D(vecPoint, oSmallestTriangle.listVertices[i], oSmallestTriangle.listVertices[i - 1]);
                    listTrianglesInSurface.Add(oNewTriangle);

                }
                oNewTriangle = new Triangle2D(vecPoint, oSmallestTriangle.listVertices[2], oSmallestTriangle.listVertices[0]);
                listTrianglesInSurface.Add(oNewTriangle);




            }
            return listConnections;
        }


        



        public static bool bIsInsideTriangle(Vector2 vecPoint, Triangle2D oTriangle)
        {

            //Calculates determinant of triangle and then 3 barycentric weights
            Vector2 B = oTriangle.listVertices[1] - oTriangle.listVertices[0];
            Vector2 C = oTriangle.listVertices[2] - oTriangle.listVertices[0];
            Vector2 P = vecPoint - oTriangle.listVertices[0];

            float detAB = (B.X * C.Y) - (B.Y * C.X);
            float wA = (P.X*(B.Y  - C.Y)+P.Y*(C.X - B.X) + detAB) / detAB;
            float wB = ((P.X * C.Y) - (P.Y * C.X)) / detAB;
            float wC = ((P.Y*B.X) - (P.X*B.Y)) / detAB;

            if ((wA >= 0)&&(wA <= 1) && (wB >= 0) && (wB <= 1) && (wC >= 0) && (wC <= 1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }







    }
	public class Triangle2D
    {

		public List<List<Vector2>> listEdges = new List<List<Vector2>>(3);
		public List<Vector2> listVertices = new List<Vector2>(3);
        public float fSignedArea;
		public Triangle2D() { }

		public Triangle2D(List<Vector2> listVertices)
        {
            this.listVertices = listVertices;
            List<Vector2> listLine1 = new List<Vector2>();
            List<Vector2> listLine2 = new List<Vector2>();
            List<Vector2> listLine3 = new List<Vector2>();

            listLine1.Add(listVertices[0]);
            listLine2.Add(listVertices[0]);
            listLine1.Add(listVertices[1]);
            listLine3.Add(listVertices[1]);
            listLine2.Add(listVertices[2]);
            listLine3.Add(listVertices[2]);

           

            this.listEdges.Add(listLine1);
            this.listEdges.Add(listLine2);
            this.listEdges.Add(listLine3);

            this.fSignedArea = fComputeSignedArea(this);


        }

        public Triangle2D(Vector2 a, Vector2 b, Vector2 c)
        {
            List<Vector2> listVertices = new List<Vector2>();
            listVertices.Add(a);
            listVertices.Add(b);
            listVertices.Add(c);

            this.listVertices = listVertices;
            List<Vector2> listLine1 = new List<Vector2>();
            List<Vector2> listLine2 = new List<Vector2>();
            List<Vector2> listLine3 = new List<Vector2>();

            listLine1.Add(listVertices[0]);
            listLine2.Add(listVertices[0]);
            listLine1.Add(listVertices[1]);
            listLine3.Add(listVertices[1]);
            listLine2.Add(listVertices[2]);
            listLine3.Add(listVertices[2]);



            this.listEdges.Add(listLine1);
            this.listEdges.Add(listLine2);
            this.listEdges.Add(listLine3);

            this.fSignedArea = fComputeSignedArea(this);


        }


        public Triangle2D(List<Vector2> listVertices, List<List<Vector2>> listEdges)
        {
            this.listEdges = listEdges;
            this.listVertices = listVertices;
            this.fSignedArea = fComputeSignedArea(this);

        }

        public static float fComputeSignedArea(Triangle2D oTriangle)
        {
            Vector2 B = oTriangle.listVertices[1] - oTriangle.listVertices[0];
            Vector2 C = oTriangle.listVertices[2] - oTriangle.listVertices[0];
            float detAB = (B.X * C.Y) - (B.Y * C.X);
            float fArea = 0.5f * (detAB);

            return fArea;


        }



    }

}
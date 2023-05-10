using System;
using System.Collections.Generic;
using System.Numerics;
using Hy;
using SetExtension;
using System.Linq;
using VectorExtension;  

/// <summary>
/// Lots of redundant information - needs trimming / simplification
/// </summary>
namespace GeometryExtension
{

    //Outdated
    public class TriangleIntersectionInfo
    {

        public Vector3 vecIntersection;
        public Triangle oThisTriangle;
        public Triangle oOtherTriangle;
        public List<Vector3> listLineInThis;
        //Values of 0 or 1
        public int iThisIdentifier;


        public TriangleIntersectionInfo(Vector3 vecIntersection, Triangle listThisTriangle, Triangle listOtherTriangle, List<Vector3> listLineInThis, int iThisIdentifier)
        {
            this.vecIntersection = vecIntersection;
            this.oThisTriangle = listThisTriangle;
            this.oOtherTriangle = listOtherTriangle;
            this.listLineInThis = listLineInThis;
            this.iThisIdentifier = iThisIdentifier;

        }
        public TriangleIntersectionInfo()
        {
            this.oThisTriangle = null;
            this.oOtherTriangle = null;
            this.listLineInThis = null;

        }

        public override int GetHashCode()
        {
            if (vecIntersection == null) return 0;
            return vecIntersection.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            TriangleIntersectionInfo other = obj as TriangleIntersectionInfo;
            return other != null && other.vecIntersection == this.vecIntersection;
        }


    }




    public class VertexInfo
    {
        public Vector3 vecVertex;
        public List<Triangle> listTriangles;
        public List<Vector3> listConnectedVertices;



        public VertexInfo()
        {
            this.vecVertex = Vector3.Zero;
            this.listTriangles = new List<Triangle>();
            this.listConnectedVertices = new List<Vector3>();

        }

        public VertexInfo(Vector3 vecVertex, List<Triangle> listTriangles, List<Vector3> listConnectedVertices)
        {
            this.vecVertex = vecVertex;
            this.listTriangles = listTriangles;
            this.listConnectedVertices = listConnectedVertices;
        }

        public VertexInfo(Vector3 vecVertex)
        {
            this.vecVertex = vecVertex;
            this.listTriangles = new List<Triangle>();
            this.listConnectedVertices = new List<Vector3>();
        }

        public override int GetHashCode()
        {
            if (listTriangles == null) return 0;
            return vecVertex.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            VertexInfo other = obj as VertexInfo;
            return other != null && other.vecVertex == this.vecVertex;
        }
    }


    public class PolygonFace
    {
        public List<Edge> listEdges = new List<Edge>();
        public List<Vector3> listVertices = new List<Vector3>();
        public PolygonFace()
        {
            this.listEdges = null;
            this.listVertices = null;
        }
        public PolygonFace(List<Vector3> listVertices, List<List<Vector3>> listEdges)
        {
            List<Edge> listEdgesFormatted = new List<Edge>();
            foreach (var listEdge in listEdges)
            {
                Edge oEdge = new Edge(listEdge);
                listEdgesFormatted.Add(oEdge);
            }
            this.listEdges = listEdgesFormatted;
            this.listVertices = listVertices;

        }

        public PolygonFace(List<Vector3> listVertices, List<Edge> listEdges)
        {
            this.listEdges = listEdges;
            this.listVertices = listVertices;

        }

        public PolygonFace(List<Edge> listEdges)
        {
            this.listEdges = listEdges;
            foreach (var oEdge in listEdges)
            {
                foreach (var vecVertex in oEdge.listEdgeEndPoints)
                {
                    if (!(this.listVertices.Contains(vecVertex)))
                    {
                        this.listVertices.Add(vecVertex);
                    }
                }
            }
        }


        /*public void OrderEdges(this PolygonFace oPolygon)
        {
            Edge oFirstEdge = oPolygon.listEdges[0];
            Vector3 vecNextVertex = oFirstEdge.listEdgeEndPoints[0];

            List<Edge> listOrderedEdges = new List<Edge>();
            List<Edge> listRemainingEdges = new List<Edge>(oPolygon.listEdges);
            int iSides = oPolygon.listEdges.Count;

            for (int i = 1; i < iSides; i++)
            {            
                
                foreach (var oEdge in listRemainingEdges)
                {
                    foreach (var vecVertex in oEdge.listEdgeEndPoints)
                    {
                        if (vecVertex == vecNextVertex)
                        {
                            List<Vector3> listNewPoint = new List<Vector3>(oEdge.listEdgeEndPoints);
                            listNewPoint.Remove(vecVertex);
                            vecNextVertex = listNewPoint[0];
                            listOrderedEdges.Add(oEdge);

                        }
                    }
                }

                listRemainingEdges.Remove(listOrderedEdges[^1]);



            }
            //this.listEdges = listOrderedEdges;
            this.listEdges = new List<Edge>(listOrderedEdges);
        }

*/
        public Vector3 vecCentre()
        {
            int iVertexCount = this.listVertices.Count;
            Vector3 vecSum = Vector3.Zero;
            for (int i = 0; i < iVertexCount; i++)
            {
                vecSum += this.listVertices[i];
            }

            Vector3 vecCentre = vecSum / iVertexCount;
            return vecCentre;
        }

        public override int GetHashCode()
        {
            List<Vector3> listOrdered = this.listVertices.OrderBy(x => Vector3.Dot(x, x)).ToList();



            unchecked // Overflow is fine, just wrap
            {
                int hash = (int)2166136261;
                // Suitable nullity checks etc, of course :)
                int n = listOrdered.Count();
                for (int i = 0; i < n; i++)
                {
                    hash = (hash * 16777619) ^ listOrdered[i].GetHashCode();
                }
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            PolygonFace other = obj as PolygonFace;
            return other != null && SetFunctions.bAreSetsEqual<Vector3>(this.listVertices, other.listVertices);
        }

    }

    public class Triangle : PolygonFace
    {
        /*public List<Edge> listEdges = new List<Edge>(3);
        public List<Vector3> listVertices = new List<Vector3>(3);*/
        public Triangle()
        {
            this.listEdges = null;
            this.listVertices = null;
        }
        public Triangle(List<Vector3> listVertices, List<List<Vector3>> listEdges)
        {
            List<Edge> listEdgesFormatted = new List<Edge>();
            foreach (var listEdge in listEdges)
            {
                Edge oEdge = new Edge(listEdge);
                listEdgesFormatted.Add(oEdge);
            }
            this.listEdges = listEdgesFormatted;
            this.listVertices = listVertices;

        }

        public Triangle(List<Vector3> listVertices, List<Edge> listEdges)
        {
            this.listEdges = listEdges;
            this.listVertices = listVertices;

        }


        public Triangle(List<Vector3> listVertices)
        {
            this.listVertices = listVertices;
            List<Vector3> listLine1 = new List<Vector3>();
            List<Vector3> listLine2 = new List<Vector3>();
            List<Vector3> listLine3 = new List<Vector3>();

            listLine1.Add(listVertices[0]);
            listLine2.Add(listVertices[0]);
            listLine1.Add(listVertices[1]);
            listLine3.Add(listVertices[1]);
            listLine2.Add(listVertices[2]);
            listLine3.Add(listVertices[2]);

            Edge oEdge1 = new Edge(listLine1);
            Edge oEdge2 = new Edge(listLine2);
            Edge oEdge3 = new Edge(listLine3);

            this.listEdges = new List<Edge>(3);
            this.listEdges.Add(oEdge1);
            this.listEdges.Add(oEdge2);
            this.listEdges.Add(oEdge3);



        }

        public Triangle(Vector3 vecA, Vector3 vecB, Vector3 vecC)
        {
            List<Vector3> listVertices = new List<Vector3>();
            listVertices.Add(vecA);
            listVertices.Add(vecB);
            listVertices.Add(vecC);

            this.listVertices = listVertices;
            List<Vector3> listLine1 = new List<Vector3>();
            List<Vector3> listLine2 = new List<Vector3>();
            List<Vector3> listLine3 = new List<Vector3>();

            listLine1.Add(listVertices[0]);
            listLine2.Add(listVertices[0]);
            listLine1.Add(listVertices[1]);
            listLine3.Add(listVertices[1]);
            listLine2.Add(listVertices[2]);
            listLine3.Add(listVertices[2]);

            Edge oEdge1 = new Edge(listLine1);
            Edge oEdge2 = new Edge(listLine2);
            Edge oEdge3 = new Edge(listLine3);

            this.listEdges = new List<Edge>(3);
            this.listEdges.Add(oEdge1);
            this.listEdges.Add(oEdge2);
            this.listEdges.Add(oEdge3);



        }


        public float fArea()
        {
            Vector3 vecA = this.listVertices[1] - this.listVertices[0];
            Vector3 vecB = this.listVertices[2] - this.listVertices[0];
            float fA = 0.5f*MathF.Abs(Vector3.Cross(vecA, vecB).fMagnitude());
            return fA;
        }

        public Vector3 vecNormal()
        {
            Vector3 vecA = this.listVertices[1] - this.listVertices[0];
            Vector3 vecB = this.listVertices[2] - this.listVertices[0];
            Vector3 vecCross = Vector3.Cross(vecA, vecB);
            Vector3 vecNormal = vecCross.vecNormalized();
            return vecNormal;
        }
        

    }


    public class Edge
    {
        public List<Triangle> listNeighbouringTriangles;
        public List<Vector3> listEdgeEndPoints;



        public Edge()
        {
            this.listNeighbouringTriangles = new List<Triangle>();
            this.listEdgeEndPoints = new List<Vector3>();
        }

        public Edge(Edge oEdge)
        {
            this.listNeighbouringTriangles = oEdge.listNeighbouringTriangles;
            this.listEdgeEndPoints = oEdge.listEdgeEndPoints;
        
        }

        public Edge(List<Vector3> listEdgeEndPoints)
        {
            this.listNeighbouringTriangles = new List<Triangle>();
            this.listEdgeEndPoints = listEdgeEndPoints;
        }
        public Edge(Vector3 vecA, Vector3 vecB)
        {

            List<Vector3> listEdgeEndPoints = new List<Vector3>();
            listEdgeEndPoints.Add(vecA);
            listEdgeEndPoints.Add(vecB);


            this.listNeighbouringTriangles = new List<Triangle>();
            this.listEdgeEndPoints = listEdgeEndPoints;
        }

        public Edge(List<Vector3> listEdgeEndPoints, List<Triangle> listNeighbouringTriangles)
        {
            this.listNeighbouringTriangles = listNeighbouringTriangles;
            this.listEdgeEndPoints = listEdgeEndPoints;
        }

        public float fLength()
        {
            float fLength = Vector3.Distance(listEdgeEndPoints[0], listEdgeEndPoints[1]);
            return fLength;
        }

        //Independent of order of list
        public override int GetHashCode()
        {
            /*
            List<Vector3> listOrdered = this.listEdgeEndPoints.OrderBy(x => Vector3.Dot(x, x)).ToList();



            unchecked // Overflow is fine, just wrap
            {
                int hash = (int)2166136261;
                // Suitable nullity checks etc, of course :)
                int n = listOrdered.Count();
                for (int i = 0; i < n; i++)
                {
                    hash = (hash * 16777619) ^ listOrdered[i].GetHashCode();
                }
                return hash;
            }*/
            int iHashCode = 0;
            foreach (var pt in this.listEdgeEndPoints)
            {
                iHashCode ^= pt.GetHashCode();
            }
            return iHashCode;
        }

        public override bool Equals(object obj)
        {
            Edge other = obj as Edge;
            return other != null && SetFunctions.bAreSetsEqual<Vector3>(this.listEdgeEndPoints, other.listEdgeEndPoints);
        }


    }



    public class Intersection
    {
        public Edge oEdgeFormingIntersection;
        public Triangle oTriangleFormingIntersection;
        public Vector3 vecIntersection;
        public Triangle oTriangleTowardsLastIntersection;

        //Used to denote which solid the edge forming the intersection lies in
        public int iSolidIdentifier;

        public Intersection()
        {
           
        }
        public Intersection(Vector3 vecIntersection)
        {
            this.oEdgeFormingIntersection = new Edge();
            this.oTriangleFormingIntersection = new Triangle();
            this.vecIntersection = vecIntersection;
        }


        public Intersection(Vector3 vecIntersection, Edge oEdgeFormingIntersection, Triangle oTriangleFormingIntersection, int iSolidIdentifier)
        {
            this.oEdgeFormingIntersection = oEdgeFormingIntersection;
            this.oTriangleFormingIntersection = oTriangleFormingIntersection;
            this.vecIntersection = vecIntersection;
            this.iSolidIdentifier = iSolidIdentifier;
        }


        public Intersection(Vector3 vecIntersection, Edge oEdgeFormingIntersection, Triangle oTriangleFormingIntersection, Triangle oTriangleTowardsLastIntersection, int iSolidIdentifier)
        {
            this.oEdgeFormingIntersection = oEdgeFormingIntersection;
            this.oTriangleFormingIntersection = oTriangleFormingIntersection;
            this.vecIntersection = vecIntersection;
            this.iSolidIdentifier = iSolidIdentifier;
            this.oTriangleTowardsLastIntersection = oTriangleTowardsLastIntersection;
        }


        public override int GetHashCode()
        {
            if (vecIntersection == null) return 0;
            return vecIntersection.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Intersection other = obj as Intersection;
            return other != null && other.vecIntersection == this.vecIntersection;
        }


    }



    public class Icosphere
    {
        public Assembly oAsm;
        public List<Vector3> listVertices;
        public List<Edge> listEdges;
        public List<Triangle> listTriangle;


        public Icosphere(int iRecursionLevel)
        {

            oAsm = (new Icosahedron()).oAsm;
            Solid oSolid = oAsm.oGetSolid(0);
            
            for (int i = 0; i < iRecursionLevel; i++)
            {

                uint iNumberOfTriangles = oSolid.nNumberOfTriangles();
                Assembly oNewAsm = new Assembly();
                Solid oNewSolid = oNewAsm.oGetSolid(0);
                for (uint j = 0; j< iNumberOfTriangles; j++)
                {

                    IntVector3 ivecTriangleIndices = oSolid.vecGetTriangle(j);



                    uint iOldAIndex = (uint)ivecTriangleIndices.iX;
                    uint iOldBIndex = (uint)ivecTriangleIndices.iY;
                    uint iOldCIndex = (uint)ivecTriangleIndices.iZ;


                    Vector3 vecA = oSolid.vecGetVertex(iOldAIndex);
                    Vector3 vecB = oSolid.vecGetVertex(iOldBIndex);
                    Vector3 vecC = oSolid.vecGetVertex(iOldCIndex);

                    Vector3 vecMidAB = funcNormalisedMidpoint(vecA, vecB);
                    Vector3 vecMidAC = funcNormalisedMidpoint(vecA, vecC);
                    Vector3 vecMidBC = funcNormalisedMidpoint(vecB, vecC);


                    uint iNewAIndex = oNewSolid.AddVertex(vecA);
                    uint iNewBIndex = oNewSolid.AddVertex(vecB);
                    uint iNewCIndex = oNewSolid.AddVertex(vecC);


                    uint iMidABIndex = oNewSolid.AddVertex(vecMidAB);
                    uint iMidACIndex = oNewSolid.AddVertex(vecMidAC);
                    uint iMidBCIndex = oNewSolid.AddVertex(vecMidBC);

                    oNewSolid.AddTriangle(iMidABIndex, iMidACIndex, iNewAIndex);
                    oNewSolid.AddTriangle(iMidABIndex, iMidBCIndex, iNewBIndex);
                    oNewSolid.AddTriangle(iMidBCIndex, iMidACIndex, iNewCIndex);
                    oNewSolid.AddTriangle(iMidABIndex, iMidACIndex, iMidBCIndex);






                }
                oAsm = oNewAsm;
            }


            //Slow - need to update algorithm
            listVertices = oSolid.listFindVertices();
            /*listVertices = oSolid.listFindVertices();
            listEdges = oSolid.listFindEdges();
            listTriangle = oSolid.listFindTriangles();
*/


        }

        public Func<Vector3, Vector3, Vector3> funcNormalisedMidpoint = ((vecA, vecB) => Vector3.Normalize(0.5f*vecA + 0.5f*vecB));
        



    }

    public class Icosahedron
    {


        public Assembly oAsm;
        public List<Vector3> listVertices;
        public List<Edge> listEdges;
        public List<Triangle> listTriangle;



        public Icosahedron()
        {
            oAsm = new Assembly();
            Solid oSolid = oAsm.oGetSolid(0);
            float t = (1f + MathF.Sqrt(5f)) / 2f;
            oSolid.AddVertex(Vector3.Normalize(new Vector3(-1, t, 0)));
            oSolid.AddVertex(Vector3.Normalize(new Vector3(1, t, 0)));
            oSolid.AddVertex(Vector3.Normalize(new Vector3(-1, -t, 0)));
            oSolid.AddVertex(Vector3.Normalize(new Vector3(1, -t, 0)));

            oSolid.AddVertex(Vector3.Normalize(new Vector3(0, -1, t)));
            oSolid.AddVertex(Vector3.Normalize(new Vector3(0, 1, t)));
            oSolid.AddVertex(Vector3.Normalize(new Vector3(0, -1, -t)));
            oSolid.AddVertex(Vector3.Normalize(new Vector3(0, 1, -t)));

            oSolid.AddVertex(Vector3.Normalize(new Vector3(t, 0, -1)));
            oSolid.AddVertex(Vector3.Normalize(new Vector3(t, 0, 1)));
            oSolid.AddVertex(Vector3.Normalize(new Vector3(-t, 0, -1)));
            oSolid.AddVertex(Vector3.Normalize(new Vector3(-t, 0, 1)));

            oSolid.AddTriangle(0, 11, 5);
            oSolid.AddTriangle(0, 5, 1);
            oSolid.AddTriangle(0, 1, 7);
            oSolid.AddTriangle(0, 7, 10);
            oSolid.AddTriangle(0, 10, 11);

            // 5 adjacent faces
            oSolid.AddTriangle(1, 5, 9);
            oSolid.AddTriangle(5, 11, 4);
            oSolid.AddTriangle(11, 10, 2);
            oSolid.AddTriangle(10, 7, 6);
            oSolid.AddTriangle(7, 1, 8);

            // 5 faces around point 3
            oSolid.AddTriangle(3, 9, 4);
            oSolid.AddTriangle(3, 4, 2);
            oSolid.AddTriangle(3, 2, 6);
            oSolid.AddTriangle(3, 6, 8);
            oSolid.AddTriangle(3, 8, 9);

            // 5 adjacent faces
            oSolid.AddTriangle(4, 9, 5);
            oSolid.AddTriangle(2, 4, 11);
            oSolid.AddTriangle(6, 2, 10);
            oSolid.AddTriangle(8, 6, 7);
            oSolid.AddTriangle(9, 8, 1);

            
            listVertices = oSolid.listFindVertices();
            listEdges = oSolid.listFindEdges();
            listTriangle = oSolid.listFindTriangles();


        }

        public Icosahedron(float fSize)
        {
            oAsm = new Assembly();
            Solid oSolid = oAsm.oGetSolid(0);
            float t = (1f + MathF.Sqrt(5f)) / 2f;
            oSolid.AddVertex(fSize*Vector3.Normalize(new Vector3(-1, t, 0)));
            oSolid.AddVertex(fSize * Vector3.Normalize(new Vector3(1, t, 0)));
            oSolid.AddVertex(fSize * Vector3.Normalize(new Vector3(-1, -t, 0)));
            oSolid.AddVertex(fSize * Vector3.Normalize(new Vector3(1, -t, 0)));

            oSolid.AddVertex(fSize * Vector3.Normalize(new Vector3(0, -1, t)));
            oSolid.AddVertex(fSize * Vector3.Normalize(new Vector3(0, 1, t)));
            oSolid.AddVertex(fSize * Vector3.Normalize(new Vector3(0, -1, -t)));
            oSolid.AddVertex(fSize * Vector3.Normalize(new Vector3(0, 1, -t)));

            oSolid.AddVertex(fSize * Vector3.Normalize(new Vector3(t, 0, -1)));
            oSolid.AddVertex(fSize * Vector3.Normalize(new Vector3(t, 0, 1)));
            oSolid.AddVertex(fSize * Vector3.Normalize(new Vector3(-t, 0, -1)));
            oSolid.AddVertex(fSize * Vector3.Normalize(new Vector3(-t, 0, 1)));

            oSolid.AddTriangle(0, 11, 5);
            oSolid.AddTriangle(0, 5, 1);
            oSolid.AddTriangle(0, 1, 7);
            oSolid.AddTriangle(0, 7, 10);
            oSolid.AddTriangle(0, 10, 11);

            // 5 adjacent faces
            oSolid.AddTriangle(1, 5, 9);
            oSolid.AddTriangle(5, 11, 4);
            oSolid.AddTriangle(11, 10, 2);
            oSolid.AddTriangle(10, 7, 6);
            oSolid.AddTriangle(7, 1, 8);

            // 5 faces around point 3
            oSolid.AddTriangle(3, 9, 4);
            oSolid.AddTriangle(3, 4, 2);
            oSolid.AddTriangle(3, 2, 6);
            oSolid.AddTriangle(3, 6, 8);
            oSolid.AddTriangle(3, 8, 9);

            // 5 adjacent faces
            oSolid.AddTriangle(4, 9, 5);
            oSolid.AddTriangle(2, 4, 11);
            oSolid.AddTriangle(6, 2, 10);
            oSolid.AddTriangle(8, 6, 7);
            oSolid.AddTriangle(9, 8, 1);



            listVertices = oSolid.listFindVertices();
            listEdges = oSolid.listFindEdges();
            listTriangle = oSolid.listFindTriangles();


        }
    }



    public static class MeshInformation{
       /* public static Dictionary<Vector3, VertexInfo> oGenerateVerticesInfo(this Solid solid)
        {
            Dictionary<Vector3, VertexInfo> oVerticesInfo = new Dictionary<Vector3, VertexInfo>();

            List<Vector3> listVertices = solid.listGetVertices();
            foreach (var vecVertex in listVertices)
            {
                oVerticesInfo.Add(vecVertex, new VertexInfo(vecVertex));
            }

            List<Triangle> listTriangles = solid.listFindTriangles();

            foreach(var oTriangle in listTriangles)
            {

                foreach (var vecVertex in oTriangle.listVertices)
                {
                    oVerticesInfo[vecVertex].listTriangles.Add(oTriangle);

                    foreach (var listEdge in oTriangle.listEdges)
                    {
                        if (listEdge.Contains(vecVertex))
                        {
                            List<Vector3> listConnectedEdge = new List<Vector3>(listEdge);
                            listConnectedEdge.Remove(vecVertex);
                            oVerticesInfo[vecVertex].listConnectedVertices.Add(listConnectedEdge[0]);
                        }
                    }

                }
                
            }




            return oVerticesInfo;
        }
*/

        public static List<Edge> listFindEdges(this Solid solid)
        {
            List<Triangle> listTriangles = solid.listFindTriangles();
            List<Edge> listEdges = new List<Edge>();
            /*foreach (var oTriangle in listTriangles)
            {
                foreach (var listEdge in oTriangle.listEdges)
                {

                    Edge oEdge = new Edge(listEdge);
                    oEdge.listNeighbouringTriangles.Add(oTriangle);

                    //Not sure if contains will work
                    if (!(listEdges.Contains(oEdge)))
                    {
                        listEdges.Add(oEdge);
                    }

                    else
                    {
                        Edge oOldEdge = listEdges.Find(item => SetFunctions.bAreSetsEqual<Vector3>(oEdge.listEdgeEndPoints, item.listEdgeEndPoints));

                        foreach (var oOtherTriangle in oOldEdge.listNeighbouringTriangles)
                        {
                            if (!(oEdge.listNeighbouringTriangles.Contains(oOtherTriangle)))
                            {
                                oEdge.listNeighbouringTriangles.Add(oOtherTriangle);
                            }
                        }
                        listEdges.Remove(oOldEdge);
                        listEdges.Add(oEdge);

                    }
                }



            }*/
            foreach (var oTriangle in listTriangles)
            {
                foreach (var oEdge in oTriangle.listEdges)
                {
                    if (!(listEdges.Contains(oEdge)))
                    {
                        listEdges.Add(oEdge);
                    }

                }
            }


            return listEdges;


        }



        //Currently checks every triangle in mesh - could restrict by assuming / calculating a maximum triangle size
        public static bool bIsInsideMesh(this Solid solid, Vector3 vecTestPoint)
        {
            
            List<Triangle> listTriangles = solid.listFindTriangles();
            bool bIsInside = bIsInsideMesh(listTriangles, vecTestPoint);
            return bIsInside;
        }

        public static bool bIsInsideMesh(this List<Triangle> listTriangles, Vector3 vecTestPoint)
        {
            bool bIsInside;

            //Max z coord needs to be a large number
            //Note that this formula doesn't work if it passes exactly through a node - added pi to each variable to minimise this (Almost zero chance of happening = no problems right?)
            float fMaxZ = 1000000;
            Vector3 vecPointAtInf = new Vector3((float)Math.PI,(float)Math.PI, fMaxZ);

            List<Vector3> listLine = new List<Vector3>();
            listLine.Add(vecTestPoint);
            listLine.Add(vecPointAtInf);
            int iCount = 0;
            foreach (var oTriangle in listTriangles)
            {
                if (bLineIntersectsPlane(oTriangle, listLine))
                {
                    if (bLineIntersectsTriangle(oTriangle, listLine))
                    {
                        iCount++;
                    }
                }
            }

            int iCheck = ((iCount % 2) + 2) % 2;
            if (iCheck == 0)
            {
                bIsInside = false;
            }
            else
            {
                bIsInside = true;
            }
            return bIsInside;
        }


        public static bool bLineIntersectsPlane(Triangle oTriangle, List<Vector3> listLine)
        {
            if ((oTriangle.listVertices.Count != 3) || (listLine.Count != 2))
            {
                throw new Exception("Error: Invalid input");
            }


            float fSign1;
            float fSign2;

            Vector3 vecA = oTriangle.listVertices[1] - oTriangle.listVertices[0];
            Vector3 vecB = oTriangle.listVertices[2] - oTriangle.listVertices[0];

            Vector3 vecNormal = Vector3.Cross(vecA, vecB);
            Vector3 vecPlaneToStart = listLine[0] - oTriangle.listVertices[0];
            Vector3 vecPlaneToEnd = listLine[1] - oTriangle.listVertices[1];
            fSign1 = Vector3.Dot(vecPlaneToStart, vecNormal);
            fSign2 = Vector3.Dot(vecPlaneToEnd, vecNormal);


            //Checks to see if they have the same sign - note that the edge case of the point lying on the plane is not included here

            bool bDoesIntersect;

            if (fSign1 * fSign2 < 0)
            {
                bDoesIntersect = true;
            }
            else
            {
                bDoesIntersect = false;
            }

            return bDoesIntersect;

        }

        public static bool bLineIntersectsPlane(Triangle oTriangle, Edge oEdge)
        {
            //Can format to remove need for other bLineIntersectsPlane function, but this works for now
            List<Vector3> listEdge = oEdge.listEdgeEndPoints;
            bool bDoesIntersect = bLineIntersectsPlane(oTriangle, listEdge);
            return bDoesIntersect;

        }



        public static bool bLineIntersectsTriangle(Triangle oTriangle, Edge oEdge)
        {
            //Can remove later on, but this works for now
            List<Vector3> listEdge = oEdge.listEdgeEndPoints;
            bool bDoesIntersect = bLineIntersectsTriangle(oTriangle, listEdge);
            return bDoesIntersect;
        }

        //Note: Assumes line intersects plane
        public static bool bLineIntersectsTriangle(Triangle oTriangle, List<Vector3> listLine)
        {
            if ((oTriangle.listVertices.Count != 3) || (listLine.Count != 2))
            {
                throw new Exception("Error: Invalid input");
            }

            List<Vector3> listTet1 = new List<Vector3>();
            List<Vector3> listTet2 = new List<Vector3>();
            List<Vector3> listTet3 = new List<Vector3>();


            listTet1.Add(oTriangle.listVertices[0]);
            listTet1.Add(oTriangle.listVertices[1]);
            listTet1.Add(listLine[0]);
            listTet1.Add(listLine[1]);

            listTet2.Add(oTriangle.listVertices[1]);
            listTet2.Add(oTriangle.listVertices[2]);
            listTet2.Add(listLine[0]);
            listTet2.Add(listLine[1]);

            listTet3.Add(oTriangle.listVertices[2]);
            listTet3.Add(oTriangle.listVertices[0]);
            listTet3.Add(listLine[0]);
            listTet3.Add(listLine[1]);



            float fSignedVol1 = fSignedTetrahedronVolume(listTet1);
            float fSignedVol2 = fSignedTetrahedronVolume(listTet2);
            float fSignedVol3 = fSignedTetrahedronVolume(listTet3);


            //Checks to see if they have the same sign - note that the edge case of the point lying on the plane is not included here

            bool bDoesIntersect;

            if (((fSignedVol1 > 0) && (fSignedVol2 > 0) && (fSignedVol3 > 0)) || ((fSignedVol1 < 0) && (fSignedVol2 < 0) && (fSignedVol3 < 0)))
            {
                bDoesIntersect = true;
            }
            else
            {
                bDoesIntersect = false;
            }

            return bDoesIntersect;

        }

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


        public static Vector3 vecFindIntersection(Triangle oTriangle, List<Vector3> listLine)
        {
            if ((oTriangle.listVertices.Count != 3) || (listLine.Count != 2))
            {
                throw new Exception("Error: Invalid input");
            }

            Vector3 vecA = listLine[0];
            Vector3 vecB = listLine[1];
            Vector3 vecNormal = vecFindNormalToTriangle(oTriangle);
            Vector3 vecTriangleVertex = oTriangle.listVertices[0];
            float fNumerator = Vector3.Dot(vecNormal, vecTriangleVertex - vecA);
            float fDenominator = Vector3.Dot(vecNormal, vecB - vecA);

            float fLambda = (fNumerator / fDenominator);

            Vector3 vecIntersection = (1 - fLambda) * vecA + fLambda * vecB;
            return vecIntersection;

        }

        public static Vector3 vecFindIntersection(Triangle oTriangle, Edge oLine)
        {
            Vector3 vecIntersection = vecFindIntersection(oTriangle, oLine.listEdgeEndPoints);
            return vecIntersection;
        }

        public static Vector3 vecFindNormalToTriangle(Triangle oTriangle)
        {
            if (oTriangle.listVertices.Count != 3)
            {
                throw new Exception("Error: Invalid input");
            }

            Vector3 a = oTriangle.listVertices[1] - oTriangle.listVertices[0];
            Vector3 b = oTriangle.listVertices[2] - oTriangle.listVertices[0];

            Vector3 vecNormal = Vector3.Cross(a, b);
            vecNormal = Vector3.Normalize(vecNormal);
            return vecNormal;


        }

        public static bool bEdgeDoesIntersectTriangle( Edge oEdge, Triangle oTriangle)
        {
            bool bDoesIntersectReturn = false;
            if(bLineIntersectsPlane(oTriangle, oEdge))
            {
                if (bLineIntersectsTriangle(oTriangle, oEdge))
                {
                    bDoesIntersectReturn = true;
                    return bDoesIntersectReturn;
                }
            }
            return bDoesIntersectReturn;

        }

        public static List<Triangle> listFindTriangles(this Solid solid)
        {
            uint iTriangleCount = solid.nNumberOfTriangles();
            //List<List<Vector3>> listTriangles = new List<List<Vector3>>();
            List<Triangle> listTriangles = new List<Triangle>();


            for (uint i = 0; i < iTriangleCount; i++)
            {
                IntVector3 ivecTriangleIndices = solid.vecGetTriangle(i);


                List<Vector3> listTriangleVertices = new List<Vector3>();
                listTriangleVertices.Add(solid.vecGetVertex((uint)ivecTriangleIndices.iX));
                listTriangleVertices.Add(solid.vecGetVertex((uint)ivecTriangleIndices.iY));
                listTriangleVertices.Add(solid.vecGetVertex((uint)ivecTriangleIndices.iZ));
                //listTriangles.Add(listTriangleVertices);

                Triangle oTriangle = new Triangle(listTriangleVertices);
                listTriangles.Add(oTriangle);

            }


            UpdateTriangleNeighbourRelations(ref listTriangles);
            return listTriangles;

        }

        public static List<Vector3> listFindVertices(this Solid solid)
        {
            uint iVertexCount = solid.nNumberOfVertices();
            //List<List<Vector3>> listTriangles = new List<List<Vector3>>();
            List<Vector3> listVertices = new List<Vector3>();


            for (uint i = 0; i < iVertexCount; i++)
            {
                Vector3 vecVertex = solid.vecGetVertex(i);
                listVertices.Add(vecVertex);

                

            }

            return listVertices;

        }

        public static List<Edge> listEdgesOfPolygon(List<Vector3> listPolygonBoundary)
        {
            int iCount = listPolygonBoundary.Count;
            List<Edge> listEdges = new List<Edge>();
            for (int i = 0; i < iCount; i++)
            {
                int iNext = (i + 1) % iCount;
                Edge oEdge = new Edge(listPolygonBoundary[i], listPolygonBoundary[iNext]);
                listEdges.Add(oEdge);
            }
            return listEdges;
        }


        public static void UpdateTriangleNeighbourRelations(ref List<Triangle> listTriangles)
        {

            Dictionary<Edge, List<Triangle>> dicGroupTrianglesByEdge = new Dictionary<Edge, List<Triangle>>();

            foreach (var oTriangle in listTriangles)
            {
                foreach(var oEdge in oTriangle.listEdges)
                {
                    if (dicGroupTrianglesByEdge.ContainsKey(oEdge))
                    {
                        if (!(dicGroupTrianglesByEdge[oEdge].Contains(oTriangle)))
                        {
                            List<Triangle> listUpdatedTriangles = new List<Triangle>(dicGroupTrianglesByEdge[oEdge]);
                            listUpdatedTriangles.Add(oTriangle);

                            
                            dicGroupTrianglesByEdge.Remove(oEdge);
                            dicGroupTrianglesByEdge.Add(oEdge, listUpdatedTriangles);
                        }

                    }
                    else
                    {
                        List<Triangle> listTrianglesOnEdge = new List<Triangle>();
                        listTrianglesOnEdge.Add(oTriangle);
                        dicGroupTrianglesByEdge.Add(oEdge, listTrianglesOnEdge);

                    }
                }
            }

            foreach (var oTriangle in listTriangles)
            {
                foreach (var oEdge in oTriangle.listEdges)
                {
                    oEdge.listNeighbouringTriangles = dicGroupTrianglesByEdge[oEdge];
                    

                }
            }





        }


        public static Vector3 vecVertexNotOnEdge(this Triangle oTriangle, Edge oEdge)
        {
            if (!oTriangle.listEdges.Contains(oEdge))
            {
                throw new Exception("Error: Edge does not belong to this triangle");
            }
            List<Vector3> listOtherVertex = new List<Vector3>(oTriangle.listVertices);
            foreach (var vec in oEdge.listEdgeEndPoints)
            {
                listOtherVertex.Remove(vec);
            }
            Vector3 vecOtherVertex = listOtherVertex[0];
            return vecOtherVertex;

        }






    }
}

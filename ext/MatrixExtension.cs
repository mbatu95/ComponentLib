using System;
using System.Collections.Generic;
using System.Numerics;
using Vector3List;
using VectorExtension;
namespace MatricesExtension
{
    public class Matrix
    {
        public double[,] aMatrix;

        public int iM;
        public int iN;

        public Matrix()
        {

        }

        public Matrix(Matrix A)
        {
            this.iM = A.iM;
            this.iN = A.iN;
            this.aMatrix = new double[A.iM, A.iN];
            for (int i = 0; i < A.iM; i++)
            {
                for (int j = 0; j < A.iN; j++)
                {
                    this.aMatrix[i, j] = A.aMatrix[i,j];
                }
            }

        }

        public Matrix(int m, int n)
        {
            this.aMatrix = new double[m, n];
            this.iM = m;
            this.iN = n;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    this.aMatrix[i, j] = 0;
                }
            }
        }

        public Matrix(int iN)
        {
            this.aMatrix = new double[iN, iN];
            this.iM = iN;
            this.iN = iN;
            for (int i = 0; i < iN; i++)
            {
                for (int j = 0; j < iN; j++)
                {
                    this.aMatrix[i, j] = 0;
                }
            }
        }




        public Matrix(double[,] matrixEntries)
        {
            this.aMatrix = matrixEntries;
            this.iM = matrixEntries.GetLength(0);
            this.iN = matrixEntries.GetLength(1);
            
        }

        

        public static Matrix operator *(Matrix A, Matrix B)
        { 
          if (A.iN != B.iM)
            {
                throw new Exception("Error: Matrix dimensions not compatable");
            }
          else
            {
                Matrix C = new Matrix(A.iM, B.iN);
                for (int i = 0; i < C.iM; i++)
                    {
                        for (int j = 0; j < C.iN; j++)
                        {
                            double sum = 0;
                            for (int k = 0; k < A.iN; k++)
                            {
                                sum += A.aMatrix[i,k] * B.aMatrix[k,j];
                            }
                            C.aMatrix[i, j] = sum;

                        }
                    }
                return C;

            }

        }

        public static Matrix operator +(Matrix A, Matrix B)
        {
            if ((A.iN != B.iN)||(A.iM!= B.iM))
            {
                throw new Exception("Error: Matrix dimensions not compatable");
            }
            else
            {
                Matrix C = new Matrix(A.iM, A.iN);
                for (int i = 0; i < C.iM; i++)
                {
                    for (int j = 0; j < C.iN; j++)
                    {
                        
                        C.aMatrix[i, j] = A.aMatrix[i,j] + B.aMatrix[i,j];

                    }
                }
                return C;

            }

        }
        public static Matrix operator -(Matrix A, Matrix B)
        {
            if ((A.iN != B.iN) || (A.iM != B.iM))
            {
                throw new Exception("Error: Matrix dimensions not compatable");
            }
            else
            {
                Matrix C = new Matrix(A.iM, A.iN);
                for (int i = 0; i < C.iM; i++)
                {
                    for (int j = 0; j < C.iN; j++)
                    {

                        C.aMatrix[i, j] = A.aMatrix[i, j] - B.aMatrix[i, j];

                    }
                }
                return C;

            }

        }


        public static Matrix operator +(Matrix A) => A;

        public static Matrix operator -(Matrix A)
        {
            
            Matrix C = new Matrix(A.iM, A.iN);
            for (int i = 0; i < C.iM; i++)
            {
                for (int j = 0; j < C.iN; j++)
                {

                    C.aMatrix[i, j] = -A.aMatrix[i, j];

                }
            }
            return C;

            

        }



        

        //Need to define methods for Vector2 and 3 separately
        //Currently only works for square 3x3 matrices
        public static Vector3 operator *(Matrix A, Vector3 v)
        {
            if (A.iN != 3)
            {
                throw new Exception("Error: Matrix and vector dimensions not compatable");
            }
            else if (A.iM != 3)
            {
                throw new Exception("Error: Matrix must produce a Vector3 (haven't finished coding for generic vectors)");
            }
            else
            {
                List<double> vList = v.ToList();
                List<double> uList = new List<double>();
                for (int k = 0; k<3; k++)
                {
                    uList.Add(0);
                }

                for (int i = 0; i<A.iM; i++)
                {
                    for (int j = 0; j < A.iN; j++)
                    {
                        uList[i] += A.aMatrix[i, j] * vList[j];
                    }
                }
                Vector3 u = uList.ToVector3();
                return u;

            }

                
        }

        public static Matrix operator *(float f, Matrix A)
        {

            Matrix B = new Matrix(A.iM, A.iN);
            for (int i = 0; i < B.iM; i++)
            {
                for (int j = 0; j < B.iN; j++)
                {
                    B.aMatrix[i, j] = f * A.aMatrix[i, j];


                }
            }

            return B;

        }
        public static Vector3 vecRotationMatrixToEulerAngleDeg(Matrix oA)
        {
            int iDecimalPlaces = 5;
            if ((oA.iN != 3) || (oA.iM != 3))
            {
                throw new Exception("Error: Matrix not a 3x3 matrix");
            }
            else
            {
                Vector3 vecEulerAngle = new Vector3();

                vecEulerAngle.Y = (float)Math.Round(-Math.Asin(oA.aMatrix[2, 0]), iDecimalPlaces);
                double denominator = (float)Math.Round(Math.Cos(vecEulerAngle.Y), iDecimalPlaces);
                if (denominator == 0)
                {
                    vecEulerAngle.Z = 0;
                    if (oA.aMatrix[2, 0] == -1)
                    {
                        vecEulerAngle.X = (float)Math.Round(vecEulerAngle.Z + Math.Atan2(oA.aMatrix[0, 1], oA.aMatrix[0, 2]), iDecimalPlaces);
                    }
                    else
                    {
                        vecEulerAngle.X = (float)Math.Round(-vecEulerAngle.Z + (float)Math.Atan2(-oA.aMatrix[0, 1], -oA.aMatrix[0, 2]), iDecimalPlaces);

                    }
                }
                else
                {

                    vecEulerAngle.X = (float)Math.Round(Math.Atan2(oA.aMatrix[2, 1] / denominator, oA.aMatrix[2, 2] / denominator), iDecimalPlaces);
                    vecEulerAngle.Z = (float)Math.Round(Math.Atan2(oA.aMatrix[1, 0] / denominator, oA.aMatrix[0, 0] / denominator), iDecimalPlaces);
                }
                vecEulerAngle = vecEulerAngle * (180 / (float)Math.PI);
                return vecEulerAngle;
            }
        }

        public static Matrix oRotationMatrixAroundVector(Vector3 vecV, float fAngleRad)
        {
            Vector3 vecAxis = Vector3.Normalize(vecV);
            
            //Rodrigues rotation matrix
            Matrix oW = new Matrix(3, 3);
            oW.aMatrix[0, 0] = 0;
            oW.aMatrix[0, 1] = -vecAxis.Z;
            oW.aMatrix[0, 2] = vecAxis.Y;
            oW.aMatrix[1, 0] = vecAxis.Z;
            oW.aMatrix[1, 1] = 0;
            oW.aMatrix[1, 2] = -vecAxis.X;
            oW.aMatrix[2, 0] = -vecAxis.Y;
            oW.aMatrix[2, 1] = vecAxis.X;
            oW.aMatrix[2, 2] = 0;

            Matrix oI = new IdentityMatrix(3);

            Matrix oRotationMatrix = oI + (((float)Math.Sin(fAngleRad)) * oW) + ((1 - (float)Math.Cos(fAngleRad)) * (oW * oW));

            return oRotationMatrix;

        }


        public static Matrix oRotationMatrixToMoveVector(Vector3 vec1, Vector3 vec2)
        {



            if ((vec1 == Vector3.Zero) || (vec2 == Vector3.Zero))
            {
                throw new Exception("Error: Vector cannot be zero");

            }

            else
            {
                Vector3 vec3 = Vector3.Cross(vec1, vec2);

                int iDecimalPlaces = 5;
                float fTolerance = MathF.Pow(10,-iDecimalPlaces);
                if (vec3.fMagnitude() < fTolerance)
                {
                    vec3 = Vector3.UnitX;
                    if (Vector3.Cross(vec1, vec3).fMagnitude() < fTolerance)
                    {
                        vec3 = Vector3.UnitY;
                    }
                    vec3 = vec3 - Vector3.Dot(vec1, vec3) * vec1;



                }

                vec3 = Vector3.Normalize(vec3);
                float fVec1Mag = vec1.fMagnitude();
                float fVec2Mag = vec2.fMagnitude();
                float fCosAngle = (Vector3.Dot(vec1, vec2) / (fVec1Mag * fVec2Mag));
                //Round value to avoid floating point errors
                fCosAngle = MathF.Round(fCosAngle, iDecimalPlaces);
                float fAngle = (float)Math.Acos(fCosAngle);

                Matrix oRotationMatrix = oRotationMatrixAroundVector(vec3, fAngle);
                return oRotationMatrix;
            }
        }


    }


    public class IdentityMatrix : Matrix
    {
        public IdentityMatrix(int n)
        {
            this.aMatrix = new double[n, n];
            this.iM = n;
            this.iN = n;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        this.aMatrix[i, j] = 1;
                    }
                    else
                    {
                        this.aMatrix[i, j] = 0;
                    }
                }
            }
        }
    }







}


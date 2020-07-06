using System;

namespace projektGrafika
{
    struct Matrix
    {
        public float[,] matrix;

        public Matrix(int n)
        {
            matrix = new float[n, n];
        }

        public static Matrix Projection(float viewAngle, float aspectRatio, float nearDistance, float farDistance)
        {
            float fFovRad = 1.0f / (float)Math.Tan(viewAngle * 0.5f / 180.0f * 3.14159f);

            Matrix projected = new Matrix(4);
            projected.matrix[0, 0] = aspectRatio * fFovRad;
            projected.matrix[1, 1] = fFovRad;
            projected.matrix[2, 2] = farDistance / (farDistance - nearDistance);
            projected.matrix[3, 2] = (-farDistance * nearDistance) / (farDistance - nearDistance);
            projected.matrix[2, 3] = 1.0f;
            projected.matrix[3, 3] = 0.0f;
            return projected;
        }

        public static Vector3D Multiply(Matrix m, Vector3D v)
        {
            Vector3D result = new Vector3D();
            result.x = v.x * m.matrix[0, 0] + v.y * m.matrix[1, 0] + v.z * m.matrix[2, 0] + v.w * m.matrix[3, 0];
            result.y = v.x * m.matrix[0, 1] + v.y * m.matrix[1, 1] + v.z * m.matrix[2, 1] + v.w * m.matrix[3, 1];
            result.z = v.x * m.matrix[0, 2] + v.y * m.matrix[1, 2] + v.z * m.matrix[2, 2] + v.w * m.matrix[3, 2];
            result.w = v.x * m.matrix[0, 3] + v.y * m.matrix[1, 3] + v.z * m.matrix[2, 3] + v.w * m.matrix[3, 3];

            return result;

        }
        public static Matrix RotationX(float angle)
        {
            Matrix rotationX = new Matrix(4);
            rotationX.matrix[0, 0] = 1;
            rotationX.matrix[1, 1] = (float)Math.Cos(angle);
            rotationX.matrix[1, 2] = (float)Math.Sin(angle);
            rotationX.matrix[2, 1] = (float)(Math.Sin(angle) * -1);
            rotationX.matrix[2, 2] = (float)Math.Cos(angle);
            rotationX.matrix[3, 3] = 1;
            return rotationX;
        }

        public static Matrix RotationY(float angle)
        {
            Matrix rotationY = new Matrix(4);
            rotationY.matrix[0, 0] = (float)Math.Cos(angle);
            rotationY.matrix[0, 2] = (float)Math.Sin(angle);
            rotationY.matrix[2, 0] = (float)(Math.Sin(angle) * -1);
            rotationY.matrix[1, 1] = 1.0F;
            rotationY.matrix[2, 2] = (float)Math.Cos(angle * 0.5F);
            rotationY.matrix[3, 3] = 1.0F;
            return rotationY;
        }

        public static Matrix RotationZ(float angle)
        {
            Matrix rotationZ = new Matrix(4);

            rotationZ.matrix[0, 0] = (float)Math.Cos(angle);
            rotationZ.matrix[0, 1] = (float)Math.Sin(angle);
            rotationZ.matrix[1, 0] = (float)(Math.Sin(angle) * -1);
            rotationZ.matrix[1, 1] = (float)Math.Cos(angle);
            rotationZ.matrix[2, 2] = 1;
            rotationZ.matrix[3, 3] = 1;
            return rotationZ;
        }

        public static Matrix Matrix_Identity()
        {
            Matrix identity = new Matrix(4);
            for (int i = 0; i < 4; i++)
            {
                identity.matrix[i, i] = 1.0f;

            }
            return identity;

        }

        public static Matrix Translation(float x, float y, float z)
        {
            Matrix translated = new Matrix(4);

            translated.matrix[0, 0] = 1.0f;
            translated.matrix[1, 1] = 1.0f;
            translated.matrix[2, 2] = 1.0f;
            translated.matrix[3, 3] = 1.0f;
            translated.matrix[3, 0] = x;
            translated.matrix[3, 1] = y;
            translated.matrix[3, 2] = z;
            return translated;
        }

        public static Matrix MultiplyMatrix(Matrix m1, Matrix m2)
        {
            Matrix multiplied = new Matrix(4);
            for (int c = 0; c < 4; c++)
                for (int r = 0; r < 4; r++)
                    multiplied.matrix[r, c] = m1.matrix[r, 0] * m2.matrix[0, c] + m1.matrix[r, 1] * m2.matrix[1, c] + m1.matrix[r, 2] * m2.matrix[2, c] + m1.matrix[r, 3] * m2.matrix[3, c];
            return multiplied;
        }

        public static Matrix PointAt(Vector3D positionVector, Vector3D targetVector, Vector3D upVector)
        {
            // New forward direction
            Vector3D newForward = Vector3D.SubVectors(targetVector, positionVector);
            newForward = Vector3D.VectorNormalise(newForward);

            //New up direction
            Vector3D a = Vector3D.MulNumber(newForward, Vector3D.VectorDot(upVector, newForward));
            Vector3D newUpvector = Vector3D.SubVectors(upVector, a);
            newUpvector = Vector3D.VectorNormalise(newUpvector);

            //new right direction
            Vector3D newRight = Vector3D.VectorCrossProduct(newUpvector, newForward);

            //translation new matrix
            Matrix translated = new Matrix(4);
            translated.matrix[0, 0] = newRight.x;
            translated.matrix[0, 1] = newRight.y;
            translated.matrix[0, 2] = newRight.z;
            translated.matrix[0, 3] = 0.0f;

            translated.matrix[1, 0] = newUpvector.x;
            translated.matrix[1, 1] = newUpvector.y;
            translated.matrix[1, 2] = newUpvector.z;
            translated.matrix[1, 3] = 0.0f;

            translated.matrix[2, 0] = newForward.x;
            translated.matrix[2, 1] = newForward.y;
            translated.matrix[2, 2] = newForward.z;
            translated.matrix[2, 3] = 0.0f;

            translated.matrix[3, 0] = positionVector.x;
            translated.matrix[3, 1] = positionVector.y;
            translated.matrix[3, 2] = positionVector.z;
            translated.matrix[3, 3] = 1.0f;

            return translated;


        }

        public static Matrix QuickInverseMatrix(Matrix m) //Only for specific (rotation/translation) matrixes
        {
            Matrix inverse = new Matrix(4);
            inverse.matrix[0, 0] = m.matrix[0, 0];
            inverse.matrix[0, 1] = m.matrix[1, 0];
            inverse.matrix[0, 2] = m.matrix[2, 0];
            inverse.matrix[0, 3] = 0.0f;

            inverse.matrix[1, 0] = m.matrix[0, 1];
            inverse.matrix[1, 1] = m.matrix[1, 1];
            inverse.matrix[1, 2] = m.matrix[2, 1];
            inverse.matrix[1, 3] = 0.0f;

            inverse.matrix[2, 0] = m.matrix[0, 2];
            inverse.matrix[2, 1] = m.matrix[1, 2];
            inverse.matrix[2, 2] = m.matrix[2, 2];
            inverse.matrix[2, 3] = 0.0f;

            inverse.matrix[3, 0] = -(m.matrix[3, 0] * inverse.matrix[0, 0] + m.matrix[3, 1] * inverse.matrix[1, 0] + m.matrix[3, 2] * inverse.matrix[2, 0]);
            inverse.matrix[3, 1] = -(m.matrix[3, 0] * inverse.matrix[0, 1] + m.matrix[3, 1] * inverse.matrix[1, 1] + m.matrix[3, 2] * inverse.matrix[2, 1]);
            inverse.matrix[3, 2] = -(m.matrix[3, 0] * inverse.matrix[0, 2] + m.matrix[3, 1] * inverse.matrix[1, 2] + m.matrix[3, 2] * inverse.matrix[2, 2]);
            inverse.matrix[3, 3] = 1.0f;

            return inverse;



        }

    }
}

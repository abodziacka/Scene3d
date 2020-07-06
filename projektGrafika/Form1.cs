using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projektGrafika
{
    public partial class Form1 : Form
    {

        //Create scene
        Mesh scene = new Mesh("objects7.obj");

        public Bitmap bmp;
        int forward = 0;

        Matrix matProjection = new Matrix(4);
        Matrix matRotationZ = new Matrix(4);
        Matrix matRotationX = new Matrix(4);
        Matrix matTranslation = new Matrix(4);
        Matrix matWorld = new Matrix(4);

        public Vector3D vCamera = new Vector3D();
        public Vector3D vForward = new Vector3D();
        float rotZ = 21;
        float rotX = 21;
        float angleZ =90;
        float angleX = 90;


        Draw draw = new Draw();
        float zPosition=7.0f;


        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            Scene();


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Scene()
        {
            Graphics g = Graphics.FromImage(bmp);
            SolidBrush brush = new SolidBrush(Color.Black);
            g.FillRectangle(brush, 0, 0, bmp.Width, bmp.Height);

            //Implement of projection matrix (3d->2d)
            float nearDistance = 0.1f;
            float farDistance = 1000.0f;
            float viewAngle = 90.0f;
            float aspectRatio = (float)bmp.Height / (float)bmp.Width;

            matProjection = Matrix.Projection(viewAngle, aspectRatio, nearDistance, farDistance);


            //Rotation
            matRotationZ = Matrix.RotationZ(angleZ);
            matRotationX = Matrix.RotationX(angleX);

            List<Triangle> trianglesToRaster = new List<Triangle>();


            matTranslation = Matrix.Translation(0.0f, 0.0f, zPosition);

            //new position after rotation
            matWorld = Matrix.Matrix_Identity();
            matWorld = Matrix.MultiplyMatrix(matRotationZ, matRotationX);
            matWorld = Matrix.MultiplyMatrix(matWorld, matTranslation);

            //travels along the vector in the diraction we want camera to point
            Vector3D vLookDiraction = new Vector3D();

            Vector3D vUp = new Vector3D { x = 0, y = 1, z = 0 };
            Vector3D vTarget = new Vector3D { x = 0, y = 0, z = 1 };

            Matrix matCameraRot = Matrix.RotationY(0);
            vLookDiraction = Matrix.Multiply(matCameraRot, vTarget);
            vTarget = Vector3D.AddVectors(vCamera, vLookDiraction);

            Matrix matCamera = Matrix.PointAt(vCamera, vTarget, vUp);

            //Make view matrix from camera
            Matrix matView = Matrix.QuickInverseMatrix(matCamera);



            foreach (var triangle in scene.allTriangles)
            {
                Triangle projected, transformed, viewed;

                projected = new Triangle(3);
                transformed = new Triangle(3);
                viewed = new Triangle(3);

                for (int i = 0; i < 3; i++)
                {
                    transformed.points[i] = Matrix.Multiply(matWorld, triangle.points[i]);

                }

                //to show  triangles on the front (can we see it and what color)
                Vector3D normal, line1, line2;

                line1 = Vector3D.SubVectors(transformed.points[1], transformed.points[0]);
                line2 = Vector3D.SubVectors(transformed.points[2], transformed.points[0]);

                normal = Vector3D.VectorCrossProduct(line1, line2);
                normal = Vector3D.VectorNormalise(normal);



                Vector3D cameraRayVector = Vector3D.SubVectors(transformed.points[0], vCamera);

                if (Vector3D.VectorDot(normal, cameraRayVector) < 0.0f)
                {

                    //Shadows
                    Vector3D lightDirection = new Vector3D
                    {
                        x = 0.0f,
                        y = 1.0f,
                        z = -1.0f
                    };
                    lightDirection = Vector3D.VectorNormalise(lightDirection);

                    float dp = (float)Math.Max(0.1f, Vector3D.VectorDot(lightDirection, normal));

                    Color c = draw.ModifyColor(Color.Black, dp);

                    //Convert transformed tri to new triangles using matri form camera
                    viewed.points[0] = Matrix.Multiply(matView, transformed.points[0]);
                    viewed.points[1] = Matrix.Multiply(matView, transformed.points[1]);
                    viewed.points[2] = Matrix.Multiply(matView, transformed.points[2]);


                    //Create project triangles on the screen (3D->2D)
                    for (int i = 0; i < 3; i++)
                    {
                        projected.points[i] = Matrix.Multiply(matProjection, viewed.points[i]);

                    }
                    for (int i = 0; i < 3; i++)
                    {
                        projected.points[i] = Vector3D.DivNumber(projected.points[i], projected.points[i].w);

                    }
                    projected.color = c;


                    //scale into view (from -1 to 1, to 0 to 2)
                    Vector3D vectorOffSetView = new Vector3D { x = 1, y = 1, z = 0 };
                    for (int i = 0; i < 3; i++)
                    {
                        projected.points[i] = Vector3D.AddVectors(projected.points[i], vectorOffSetView);

                    }

                    //scaled to my view area axes
                    projected.points[0].x *= 0.5f * (float)bmp.Width;
                    projected.points[0].y *= 0.5f * (float)bmp.Height;
                    projected.points[1].x *= 0.5f * (float)bmp.Width;
                    projected.points[1].y *= 0.5f * (float)bmp.Height;
                    projected.points[2].x *= 0.5f * (float)bmp.Width;
                    projected.points[2].y *= 0.5f * (float)bmp.Height;

                    //sorting triangles from back to front
                    projected.zValue = (projected.points[0].z + projected.points[1].z + projected.points[2].z) / 3.0f;
                    trianglesToRaster.Add(projected);

                }


            }

            trianglesToRaster.Sort((a, b) => b.zValue.CompareTo(a.zValue));
            foreach (var tri in trianglesToRaster)
            {
                draw.DrawTriangle(tri, bmp, tri.color);
                pictureBox1.Image = bmp;
            }
        }
       

        private void Form1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                vCamera.y -= 0.05f;
                Scene();
            }
            if (e.KeyCode == Keys.Down)
            {
                vCamera.y += 0.05f;
                Scene();

            }
            if (e.KeyCode == Keys.Right)
            {
                vCamera.x += 0.05f;
                Scene();

            }
            if (e.KeyCode == Keys.Left)
            {
                vCamera.x -= 0.05f;
                Scene();

            }
            if (e.KeyCode == Keys.Z)
            {
                angleZ =0.1f*rotZ;
                rotZ++;
                Scene();

            }
            if (e.KeyCode == Keys.X)
            {
                angleX = 0.1f * rotX;
                rotX++;
                Scene();

            }
            if (e.KeyCode == Keys.W)
            {
                if (forward<6)
                {
                    zPosition = zPosition - 0.5f;
                    Scene();
                    forward++;
                }
            }

            if (e.KeyCode == Keys.S)
            {
                zPosition = zPosition + 0.5f;
                Scene();
                forward--;
            }
        }
    }
}

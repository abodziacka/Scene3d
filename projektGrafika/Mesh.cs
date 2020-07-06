using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace projektGrafika
{
    public struct Mesh
    {
        public List<Triangle> allTriangles;

        public Mesh(string fileName)
        {
            List<Vector3D> vectors = new List<Vector3D>();
            allTriangles = new List<Triangle>();

            StreamReader sr = new StreamReader(fileName);
            string line = sr.ReadLine();

            while (String.IsNullOrEmpty(line) ==false)
            {
                //location of the single vector in 3d space
                if (line[0] == 'v')
                {
                    Vector3D v = new Vector3D();

                    string[] sections = line.Split(' ');

                    v.x = float.Parse(sections[1], CultureInfo.InvariantCulture.NumberFormat);
                    v.y = float.Parse(sections[2], CultureInfo.InvariantCulture.NumberFormat);
                    v.z = float.Parse(sections[3], CultureInfo.InvariantCulture.NumberFormat);

                    vectors.Add(v);
                }
                //vectors in the front (indexes of points which are on the front)
                if (line[0]=='f')
                {
                    int[] f = new int[3];
                    string[] sections = line.Split(' ');

                    f[0] = Convert.ToInt32(sections[1]);
                    f[1] = Convert.ToInt32(sections[2]);
                    f[2] = Convert.ToInt32(sections[3]);

                    Triangle t = new Triangle
                    {
                        points = new Vector3D[]
                        {
                            //points of these front triangles
                            vectors[f[0]-1],
                            vectors[f[1]-1],
                            vectors[f[2]-1]
                        }
                    };
                    allTriangles.Add(t);
                }
                line = sr.ReadLine();
            }
        }
    }
}

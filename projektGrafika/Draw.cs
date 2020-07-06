using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektGrafika
{
    public class Draw
    {
        public void DrawTriangle(Triangle t, Bitmap bmp, Color color)
        {
            Graphics g = Graphics.FromImage(bmp);
            SolidBrush contourBrush = new SolidBrush(Color.LavenderBlush);
            SolidBrush fillBrush = new SolidBrush(color);
            Pen pen = new Pen(contourBrush);

            Point[] points = new Point[3];

            for (int i = 0; i < t.points.Length; i++)
            {
                int x = (int)Math.Round(t.points[i].x);
                int y = (int)Math.Round(t.points[i].y);
                Point p = new Point(x, y);
                points[i] = p;

            }
            
            g.FillPolygon(fillBrush,points);
            g.DrawLine(pen, t.points[0].x, t.points[0].y, t.points[1].x, t.points[1].y);
            g.DrawLine(pen, t.points[1].x, t.points[1].y, t.points[2].x, t.points[2].y);
            g.DrawLine(pen, t.points[2].x, t.points[2].y, t.points[0].x, t.points[0].y);


        }

        public Color ModifyColor(Color color, float colorChanging)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (colorChanging < 0)
            {
                colorChanging = 1 + colorChanging;
                red *= colorChanging;
                green *= colorChanging;
                blue *= colorChanging;
            }
            else
            {
                red = (255 - red) * colorChanging + red;
                green = (255 - green) * colorChanging + green;
                blue = (255 - blue) * colorChanging + blue;
            }

            return Color.FromArgb((int)red, (int)green, (int)blue);
        }
    }
}

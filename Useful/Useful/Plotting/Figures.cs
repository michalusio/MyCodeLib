using System;
using System.Drawing;

namespace Useful.Plotting
{
    public static class Figures
    {
        public static Plot2D ReuleauxPolygon(float dx, int n, float scale, Color color)
        {
            Plot2D plot2D = new Plot2D();
            var num1 = 0.0f;
            while (num1 <= 2.0 * Math.PI)
            {
                var num2 =
                    (float)
                    (2.0 * Math.Cos(Math.PI / (2 * n)) *
                     Math.Cos(0.5 * (num1 + Math.PI / n * (2.0 * Math.Floor(n * (double) num1 / (2.0 * Math.PI)) + 1.0))) -
                     Math.Cos(Math.PI / n * (2.0 * Math.Floor(n * (double) num1 / (2.0 * Math.PI)) + 1.0)));
                var num3 =
                    (float)
                    (2.0 * Math.Cos(Math.PI / (2 * n)) *
                     Math.Sin(0.5 * (num1 + Math.PI / n * (2.0 * Math.Floor(n * (double) num1 / (2.0 * Math.PI)) + 1.0))) -
                     Math.Sin(Math.PI / n * (2.0 * Math.Floor(n * (double) num1 / (2.0 * Math.PI)) + 1.0)));
                plot2D.AddPoint(new PPoint2D(num2 * scale, num3 * scale, color));
                num1 += dx;
            }
            return plot2D;
        }

        public static Plot2D ReuleauxPolygonF(float dx, int n, float scale, Color color)
        {
            Plot2D plot2D = new Plot2D();
            var num1 = 0.0f;
            while (num1 <= 2.0 * Math.PI)
            {
                var num2 =
                    (float)
                    (2.0 * Math.Cos(Math.PI / (2 * n)) *
                     Math.Cos(0.5 * (num1 + Math.PI / n * (2.0 * Math.Floor(n * (double) num1 / (2.0 * Math.PI)) + 1.0))) -
                     Math.Cos(Math.PI / n * (2.0 * Math.Floor(n * (double) num1 / (2.0 * Math.PI)) + 1.0)));
                var num3 =
                    (float)
                    (2.0 * Math.Cos(Math.PI / (2 * n)) *
                     Math.Sin(0.5 * (num1 + Math.PI / n * (2.0 * Math.Floor(n * (double) num1 / (2.0 * Math.PI)) + 1.0))) -
                     Math.Sin(Math.PI / n * (2.0 * Math.Floor(n * (double) num1 / (2.0 * Math.PI)) + 1.0)));
                var num4 = (float) Math.Sqrt(num2 * (double) num2 + num3 * (double) num3);
                var num5 = num2 / num4;
                var num6 = num3 / num4;
                var num7 = num4 * scale;
                var num8 = dx;
                while (num8 <= (double) num7)
                {
                    plot2D.AddPoint(new PPoint2D(num5 * num8, num6 * num8, color));
                    num8 += dx;
                }
                num1 += dx;
            }
            return plot2D;
        }

        public static Plot2D Line2D(float dx, PPoint2D a, PPoint2D b, Color color)
        {
            Plot2D plot2D = new Plot2D();
            var num1 = a.X - b.X;
            var num2 = a.Y - b.Y;
            var num3 = (float) Math.Sqrt(num1 * (double) num1 + num2 * (double) num2);
            var num4 = num1 / num3;
            var num5 = num2 / num3;
            var num6 = 0.0f;
            while (num6 <= (double) num3)
            {
                plot2D.AddPoint(new PPoint2D(num6 * num4 + b.X, num6 * num5 + b.Y, color));
                num6 += dx;
            }
            return plot2D;
        }

        public static Plot3D Line3D(float dx, PPoint3D a, PPoint3D b, Color color)
        {
            Plot3D plot3D = new Plot3D();
            var num1 = a.X - b.X;
            var num2 = a.Y - b.Y;
            var num3 = a.Z - b.Z;
            var num4 = (float) Math.Sqrt(num1 * (double) num1 + num2 * (double) num2 + num3 * (double) num3);
            var num5 = num1 / num4;
            var num6 = num2 / num4;
            var num7 = num3 / num4;
            var num8 = 0.0f;
            while (num8 <= (double) num4)
            {
                plot3D.AddPoint(new PPoint3D(num8 * num5 + b.X, num8 * num6 + b.Y, num8 * num7 + b.Z, color));
                num8 += dx;
            }
            return plot3D;
        }

        public static Plot2D Circle(float dx, float r, Color color)
        {
            Plot2D plot2D = new Plot2D();
            var num = 0.0f;
            while (num <= 2.0 * Math.PI)
            {
                plot2D.AddPoint(new PPoint2D((float) Math.Cos(num) * r, (float) Math.Sin(num) * r, color));
                num += dx;
            }
            return plot2D;
        }

        public static Plot2D CircleF(float dx, float r, Color color)
        {
            Plot2D plot2D = new Plot2D();
            var num1 = 0.0f;
            while (num1 <= 2.0 * Math.PI)
            {
                var num2 = (float) Math.Cos(num1);
                var num3 = (float) Math.Sin(num1);
                var num4 = dx;
                while (num4 <= (double) r)
                {
                    plot2D.AddPoint(new PPoint2D(num2 * num4, num3 * num4, color));
                    num4 += dx;
                }
                num1 += dx;
            }
            return plot2D;
        }

        public static Plot2D Ellipse(float dx, float a, float b, Color color)
        {
            Plot2D plot2D = new Plot2D();
            var num = 0.0f;
            while (num <= 2.0 * Math.PI)
            {
                plot2D.AddPoint(new PPoint2D((float) Math.Cos(num) * a, (float) Math.Sin(num) * b, color));
                num += dx;
            }
            return plot2D;
        }

        public static Plot2D EllipseF(float dx, float a, float b, Color color)
        {
            Plot2D plot2D = new Plot2D();
            var num1 = 0.0f;
            while (num1 <= 2.0 * Math.PI)
            {
                var num2 = (float) Math.Cos(num1) * a;
                var num3 = (float) Math.Sin(num1) * b;
                var num4 = (float) Math.Sqrt(num2 * (double) num2 + num3 * (double) num3);
                var num5 = num2 / num4;
                var num6 = num3 / num4;
                var num7 = dx;
                while (num7 <= (double) num4)
                {
                    plot2D.AddPoint(new PPoint2D(num5 * num7, num6 * num7, color));
                    num7 += dx;
                }
                num1 += dx;
            }
            return plot2D;
        }

        public static Plot3D Sphere(float dx, float r, Color color)
        {
            Plot3D plot3D = new Plot3D();
            var num1 = 0.0f;
            while (num1 <= Math.PI)
            {
                var num2 = 0.0f;
                while (num2 <= 2.0 * Math.PI)
                {
                    plot3D.AddPoint(new PPoint3D((float) (Math.Sin(num1) * Math.Cos(num2)) * r,
                        (float) (Math.Sin(num1) * Math.Sin(num2)) * r, (float) Math.Cos(num1) * r, color));
                    num2 += dx;
                }
                num1 += dx;
            }
            return plot3D;
        }

        public static Plot3D SphereF(float dx, float r, Color color)
        {
            Plot3D plot3D = new Plot3D();
            var num1 = 0.0f;
            while (num1 <= Math.PI)
            {
                var num2 = 0.0f;
                while (num2 <= 2.0 * Math.PI)
                {
                    var num3 = (float) (Math.Sin(num1) * Math.Cos(num2));
                    var num4 = (float) (Math.Sin(num1) * Math.Sin(num2));
                    var num5 = (float) Math.Cos(num1);
                    var num6 = dx;
                    while (num6 <= (double) r)
                    {
                        plot3D.AddPoint(new PPoint3D(num3 * num6, num4 * num6, num5 * num6, color));
                        num6 += dx;
                    }
                    num2 += dx;
                }
                num1 += dx;
            }
            return plot3D;
        }

        public static Plot3D Ellipsoid(float dx, float a, float b, float c, Color color)
        {
            Plot3D plot3D = new Plot3D();
            var num1 = 0.0f;
            while (num1 <= Math.PI)
            {
                var num2 = 0.0f;
                while (num2 <= 2.0 * Math.PI)
                {
                    plot3D.AddPoint(new PPoint3D((float) (Math.Sin(num1) * Math.Cos(num2)) * a,
                        (float) (Math.Sin(num1) * Math.Sin(num2)) * b, (float) Math.Cos(num1) * c, color));
                    num2 += dx;
                }
                num1 += dx;
            }
            return plot3D;
        }

        public static Plot3D EllipsoidF(float dx, float a, float b, float c, Color color)
        {
            Plot3D plot3D = new Plot3D();
            var num1 = 0.0f;
            while (num1 <= Math.PI)
            {
                var num2 = 0.0f;
                while (num2 <= 2.0 * Math.PI)
                {
                    var num3 = (float) (Math.Sin(num1) * Math.Cos(num2)) * a;
                    var num4 = (float) (Math.Sin(num1) * Math.Sin(num2)) * b;
                    var num5 = (float) Math.Cos(num1) * c;
                    var num6 = (float) Math.Sqrt(num3 * (double) num3 + num4 * (double) num4 + num5 * (double) num5);
                    var num7 = num3 / num6;
                    var num8 = num4 / num6;
                    var num9 = num5 / num6;
                    var num10 = dx;
                    while (num10 <= (double) num6)
                    {
                        plot3D.AddPoint(new PPoint3D(num7 * num10, num8 * num10, num9 * num10, color));
                        num10 += dx;
                    }
                    num2 += dx;
                }
                num1 += dx;
            }
            return plot3D;
        }

        public static Plot2D Rectangle(float dx, float a, float b, Color color)
        {
            Plot2D plot2D = new Plot2D();
            var x = 0.0f;
            while (x <= (double) a)
            {
                plot2D.AddPoint(new PPoint2D(x, 0.0f, color));
                plot2D.AddPoint(new PPoint2D(x, b, color));
                x += dx;
            }
            var y = 0.0f;
            while (y <= (double) b)
            {
                plot2D.AddPoint(new PPoint2D(0.0f, y, color));
                plot2D.AddPoint(new PPoint2D(a, y, color));
                y += dx;
            }
            return plot2D;
        }

        public static Plot2D RectangleF(float dx, float a, float b, Color color)
        {
            Plot2D plot2D = new Plot2D();
            var x = 0.0f;
            while (x <= (double) a)
            {
                var y = 0.0f;
                while (y <= (double) b)
                {
                    plot2D.AddPoint(new PPoint2D(x, y, color));
                    y += dx;
                }
                x += dx;
            }
            return plot2D;
        }

        public static Plot3D Cuboid(float dx, float a, float b, float c, Color color)
        {
            Plot3D plot3D = new Plot3D();
            var x = 0.0f;
            while (x <= (double) a)
            {
                plot3D.AddPoint(new PPoint3D(x, 0.0f, 0.0f, color));
                plot3D.AddPoint(new PPoint3D(x, b, 0.0f, color));
                plot3D.AddPoint(new PPoint3D(x, 0.0f, c, color));
                plot3D.AddPoint(new PPoint3D(x, b, c, color));
                x += dx;
            }
            var y = 0.0f;
            while (y <= (double) b)
            {
                plot3D.AddPoint(new PPoint3D(0.0f, y, 0.0f, color));
                plot3D.AddPoint(new PPoint3D(a, y, 0.0f, color));
                plot3D.AddPoint(new PPoint3D(0.0f, y, c, color));
                plot3D.AddPoint(new PPoint3D(a, y, c, color));
                y += dx;
            }
            var z = 0.0f;
            while (z <= (double) c)
            {
                plot3D.AddPoint(new PPoint3D(0.0f, 0.0f, z, color));
                plot3D.AddPoint(new PPoint3D(a, 0.0f, z, color));
                plot3D.AddPoint(new PPoint3D(0.0f, b, z, color));
                plot3D.AddPoint(new PPoint3D(a, b, z, color));
                z += dx;
            }
            return plot3D;
        }

        public static Plot3D CuboidF(float dx, float a, float b, float c, Color color)
        {
            Plot3D plot3D = new Plot3D();
            var x = 0.0f;
            while (x <= (double) a)
            {
                var y = 0.0f;
                while (y <= (double) b)
                {
                    var z = 0.0f;
                    while (z <= (double) c)
                    {
                        plot3D.AddPoint(new PPoint3D(x, y, z, color));
                        z += dx;
                    }
                    y += dx;
                }
                x += dx;
            }
            return plot3D;
        }
    }
}
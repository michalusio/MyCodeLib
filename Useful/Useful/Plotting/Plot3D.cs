using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using Useful.Functions;
using Useful.Other;
using Useful.Plotting.Transformations;

namespace Useful.Plotting
{
    public class Plot3D
    {
        private readonly List<PPoint3D> _points = new List<PPoint3D>();
        public float LinesH;
        public float LinesL;
        public float LinesV;
        internal float MaxZ = float.MinValue;
        internal float MinZ = float.MaxValue;
        public float Size = 1f;

        public static Plot3D operator +(Plot3D a, PPoint3D b)
        {
            Plot3D plot3D = new Plot3D();
            plot3D._points.AddRange(a._points);
            plot3D._points.Add(b);
            double num = a.Size;
            plot3D.Size = (float) num;
            return plot3D;
        }

        public static Plot3D operator +(Plot3D a, Plot3D b)
        {
            Plot3D plot3D = new Plot3D();
            plot3D._points.AddRange(a._points);
            plot3D._points.AddRange(b._points);
            plot3D.MaxZ = a.MaxZ <= (double) b.MaxZ ? b.MaxZ : a.MaxZ;
            plot3D.MinZ = a.MinZ >= (double) b.MinZ ? b.MinZ : a.MinZ;
            plot3D.Size = (float) ((a.Size + (double) b.Size) * 0.5);
            plot3D.LinesH = (float) ((a.LinesH + (double) b.LinesH) * 0.5);
            plot3D.LinesV = (float) ((a.LinesV + (double) b.LinesV) * 0.5);
            plot3D.LinesL = (float) ((a.LinesL + (double) b.LinesL) * 0.5);
            return plot3D;
        }

        public static Plot3D operator *(Plot3D a, Transform3DList b)
        {
            var l = a._points.ConvertAll(el => new PPoint3D(el.X, el.Y, el.Z, el.Color, el.Visible));
            Parallel.For(0, a._points.Count, i =>
            {
                PPoint3D point = a._points[i];
                foreach (ITransformation3D transform in b.Transforms)
                    transform.Transform(ref point, l);
                a._points[i] = point;
            });
            return a;
        }

        public static Plot3D operator /(Plot3D a, Transform3DList b)
        {
            var l = a._points.ConvertAll(el => new PPoint3D(el.X, el.Y, el.Z, el.Color, el.Visible));
            Parallel.For(0, a._points.Count, i =>
            {
                PPoint3D point = a._points[i];
                foreach (ITransformation3D transform in b.Transforms)
                {
                    INvertibleTransformation3D transformation3D = transform as INvertibleTransformation3D;
                    if (transformation3D == null)
                        throw new InvalidCastException("Transformation not invertible!");
                    transformation3D.Invert(ref point, l);
                }
                a._points[i] = point;
            });
            return a;
        }

        public static Plot3D operator *(Plot3D a, ITransformation3D b)
        {
            var l = a._points.ConvertAll(el => new PPoint3D(el.X, el.Y, el.Z, el.Color, el.Visible));
            Parallel.For(0, a._points.Count, i =>
            {
                PPoint3D point = a._points[i];
                b.Transform(ref point, l);
                a._points[i] = point;
            });
            return a;
        }

        public static Plot3D operator /(Plot3D a, ITransformation3D b)
        {
            var l = a._points.ConvertAll(el => new PPoint3D(el.X, el.Y, el.Z, el.Color, el.Visible));
            Parallel.For(0, a._points.Count, i =>
            {
                PPoint3D point = a._points[i];
                INvertibleTransformation3D transformation3D = b as INvertibleTransformation3D;
                if (transformation3D == null)
                    throw new InvalidCastException("Transformation not invertible!");
                transformation3D.Invert(ref point, l);
                a._points[i] = point;
            });
            return a;
        }

        public static Plot3D PlotFunction(string formula, float minX, float maxX, float minY, float maxY)
        {
            Plot3D plot3D = new Plot3D();
            var x1 = minX;
            var num1 = (float) (1.0 / 800.0 * (maxX - (double) minX));
            var num2 = (float) (1.0 / 800.0 * (maxY - (double) minY));
            if (num1 < 0.0 || num2 < 0.0)
                throw new ArgumentException("max<min !");
            Onp onp = new Onp();
            var onPformula = onp.Parse(formula);
            var x2 = new Dictionary<char, double>(2) {{'x', 0.0}, {'y', 0.0}};
            while (x1 <= (double) maxX)
            {
                x2['x'] = x1;
                var y = minY;
                while (y <= (double) maxY)
                {
                    x2['y'] = y;
                    var z = (float) onp.Solve(onPformula, x2);
                    if (z > (double) plot3D.MaxZ)
                        plot3D.MaxZ = z;
                    if (z < (double) plot3D.MinZ)
                        plot3D.MinZ = z;
                    plot3D._points.Add(new PPoint3D(x1, y, z));
                    y += num2;
                }
                x1 += num1;
            }
            return plot3D;
        }

        public static Plot3D PlotFunction(string formula, float minX, float maxX, float minY, float maxY, Color color)
        {
            Plot3D plot3D = new Plot3D();
            var x1 = minX;
            var num1 = (float) (1.0 / 800.0 * (maxX - (double) minX));
            var num2 = (float) (1.0 / 800.0 * (maxY - (double) minY));
            if (num1 < 0.0 || num2 < 0.0)
                throw new ArgumentException("max<min !");
            Onp onp = new Onp();
            var onPformula = onp.Parse(formula);
            var x2 = new Dictionary<char, double>(2) {{'x', 0.0}, {'y', 0.0}};
            while (x1 <= (double) maxX)
            {
                x2['x'] = x1;
                var y = minY;
                while (y <= (double) maxY)
                {
                    x2['y'] = y;
                    var z = (float) onp.Solve(onPformula, x2);
                    if (z > (double) plot3D.MaxZ)
                        plot3D.MaxZ = z;
                    if (z < (double) plot3D.MinZ)
                        plot3D.MinZ = z;
                    plot3D._points.Add(new PPoint3D(x1, y, z, color));
                    y += num2;
                }
                x1 += num1;
            }
            return plot3D;
        }

        public static Plot3D PlotFunction(string formula, Range xx, Range yy)
        {
            Plot3D plot3D = new Plot3D();
            var x1 = xx.Min;
            if (xx.Diff < 0.0 || yy.Diff < 0.0)
                throw new ArgumentException("max<min !");
            Onp onp = new Onp();
            var onPformula = onp.Parse(formula);
            var x2 = new Dictionary<char, double>(2) {{'x', 0.0}, {'y', 0.0}};
            while (x1 <= (double) xx.Max)
            {
                x2['x'] = x1;
                var y = yy.Min;
                while (y <= (double) yy.Max)
                {
                    x2['y'] = y;
                    var z = (float) onp.Solve(onPformula, x2);
                    if (z > (double) plot3D.MaxZ)
                        plot3D.MaxZ = z;
                    if (z < (double) plot3D.MinZ)
                        plot3D.MinZ = z;
                    plot3D._points.Add(new PPoint3D(x1, y, z));
                    y += yy.Diff;
                }
                x1 += xx.Diff;
            }
            return plot3D;
        }

        public static Plot3D PlotFunction(string formula, Range xx, Range yy, Color color)
        {
            Plot3D plot3D = new Plot3D();
            var x1 = xx.Min;
            if (xx.Diff < 0.0 || yy.Diff < 0.0)
                throw new ArgumentException("max<min !");
            Onp onp = new Onp();
            var onPformula = onp.Parse(formula);
            var x2 = new Dictionary<char, double>(2) {{'x', 0.0}, {'y', 0.0}};
            while (x1 <= (double) xx.Max)
            {
                x2['x'] = x1;
                var y = yy.Min;
                while (y <= (double) yy.Max)
                {
                    x2['y'] = y;
                    var z = (float) onp.Solve(onPformula, x2);
                    if (z > (double) plot3D.MaxZ)
                        plot3D.MaxZ = z;
                    if (z < (double) plot3D.MinZ)
                        plot3D.MinZ = z;
                    plot3D._points.Add(new PPoint3D(x1, y, z, color));
                    y += yy.Diff;
                }
                x1 += xx.Diff;
            }
            return plot3D;
        }

        public static Plot3D PlotField(string formula, float minX, float maxX, float minY, float maxY, float minZ,
            float maxZ)
        {
            Plot3D plot3D = new Plot3D();
            var x1 = minX;
            var num1 = (float) (1.0 / 800.0 * (maxX - (double) minX));
            var num2 = (float) (1.0 / 800.0 * (maxY - (double) minY));
            var num3 = (float) (1.0 / 800.0 * (maxZ - (double) minZ));
            if (num1 < 0.0 || num2 < 0.0 || num3 < 0.0)
                throw new ArgumentException("max<min !");
            Onp onp = new Onp();
            var onPformula = onp.Parse(formula);
            var x2 = new Dictionary<char, double>(3) {{'x', 0.0}, {'y', 0.0}, {'z', 0.0}};
            while (x1 <= (double) maxX)
            {
                x2['x'] = x1;
                var y = minY;
                while (y <= (double) maxY)
                {
                    x2['y'] = y;
                    var z = minZ;
                    while (z <= (double) maxZ)
                    {
                        x2['z'] = z;
                        if (onp.Solve(onPformula, x2) > 0.0)
                        {
                            if (z > (double) plot3D.MaxZ)
                                plot3D.MaxZ = z;
                            if (z < (double) plot3D.MinZ)
                                plot3D.MinZ = z;
                            plot3D._points.Add(new PPoint3D(x1, y, z));
                        }
                        z += num3;
                    }
                    y += num2;
                }
                x1 += num1;
            }
            return plot3D;
        }

        public static Plot3D PlotField(string formula, float minX, float maxX, float minY, float maxY, float minZ,
            float maxZ, Color color)
        {
            Plot3D plot3D = new Plot3D();
            var x1 = minX;
            var num1 = (float) (1.0 / 800.0 * (maxX - (double) minX));
            var num2 = (float) (1.0 / 800.0 * (maxY - (double) minY));
            var num3 = (float) (1.0 / 800.0 * (maxZ - (double) minZ));
            if (num1 < 0.0 || num2 < 0.0 || num3 < 0.0)
                throw new ArgumentException("max<min !");
            Onp onp = new Onp();
            var onPformula = onp.Parse(formula);
            var x2 = new Dictionary<char, double>(3) {{'x', 0.0}, {'y', 0.0}, {'z', 0.0}};
            while (x1 <= (double) maxX)
            {
                x2['x'] = x1;
                var y = minY;
                while (y <= (double) maxY)
                {
                    x2['y'] = y;
                    var z = minZ;
                    while (z <= (double) maxZ)
                    {
                        x2['z'] = z;
                        if (onp.Solve(onPformula, x2) > 0.0)
                        {
                            if (z > (double) plot3D.MaxZ)
                                plot3D.MaxZ = z;
                            if (z < (double) plot3D.MinZ)
                                plot3D.MinZ = z;
                            plot3D._points.Add(new PPoint3D(x1, y, z, color));
                        }
                        z += num3;
                    }
                    y += num2;
                }
                x1 += num1;
            }
            return plot3D;
        }

        public static Plot3D PlotField(string formula, Range xx, Range yy, Range zz)
        {
            Plot3D plot3D = new Plot3D();
            var x1 = xx.Min;
            if (xx.Diff < 0.0 || yy.Diff < 0.0 || zz.Diff < 0.0)
                throw new ArgumentException("max<min !");
            Onp onp = new Onp();
            var onPformula = onp.Parse(formula);
            var x2 = new Dictionary<char, double>(3) {{'x', 0.0}, {'y', 0.0}, {'z', 0.0}};
            while (x1 <= (double) xx.Max)
            {
                x2['x'] = x1;
                var y = yy.Min;
                while (y <= (double) yy.Max)
                {
                    x2['y'] = y;
                    var z = zz.Min;
                    while (z <= (double) zz.Max)
                    {
                        x2['z'] = z;
                        if (onp.Solve(onPformula, x2) > 0.0)
                        {
                            if (z > (double) plot3D.MaxZ)
                                plot3D.MaxZ = z;
                            if (z < (double) plot3D.MinZ)
                                plot3D.MinZ = z;
                            plot3D._points.Add(new PPoint3D(x1, y, z));
                        }
                        z += zz.Diff;
                    }
                    y += yy.Diff;
                }
                x1 += xx.Diff;
            }
            return plot3D;
        }

        public static Plot3D PlotField(string formula, Range xx, Range yy, Range zz, Color color)
        {
            Plot3D plot3D = new Plot3D();
            var x1 = xx.Min;
            if (xx.Diff < 0.0 || yy.Diff < 0.0 || zz.Diff < 0.0)
                throw new ArgumentException("max<min !");
            Onp onp = new Onp();
            var onPformula = onp.Parse(formula);
            var x2 = new Dictionary<char, double>(3) {{'x', 0.0}, {'y', 0.0}, {'z', 0.0}};
            while (x1 <= (double) xx.Max)
            {
                x2['x'] = x1;
                var y = yy.Min;
                while (y <= (double) yy.Max)
                {
                    x2['y'] = y;
                    var z = zz.Min;
                    while (z <= (double) zz.Max)
                    {
                        x2['z'] = z;
                        if (onp.Solve(onPformula, x2) > 0.0)
                        {
                            if (z > (double) plot3D.MaxZ)
                                plot3D.MaxZ = z;
                            if (z < (double) plot3D.MinZ)
                                plot3D.MinZ = z;
                            plot3D._points.Add(new PPoint3D(x1, y, z, color));
                        }
                        z += zz.Diff;
                    }
                    y += yy.Diff;
                }
                x1 += xx.Diff;
            }
            return plot3D;
        }

        public static Plot3D PlotParametric(string fX, string fY, string fZ, char[] vars, Range[] ranges)
        {
            Plot3D p = new Plot3D();
            Onp o = new Onp();
            var fX1 = o.Parse(fX);
            var fY1 = o.Parse(fY);
            var fZ1 = o.Parse(fZ);
            var d = new Dictionary<char, double>();
            foreach (var var in vars)
                d.Add(var, 0.0);
            PlotP(o, fX1, fY1, fZ1, vars, ranges, p, d, 0, Color.Black);
            return p;
        }

        public static Plot3D PlotParametric(string fX, string fY, string fZ, char[] vars, Range[] ranges, Color color)
        {
            Plot3D p = new Plot3D();
            Onp o = new Onp();
            var fX1 = o.Parse(fX);
            var fY1 = o.Parse(fY);
            var fZ1 = o.Parse(fZ);
            var d = new Dictionary<char, double>();
            foreach (var var in vars)
                d.Add(var, 0.0);
            PlotP(o, fX1, fY1, fZ1, vars, ranges, p, d, 0, color);
            return p;
        }

        private static void PlotP(Onp o, Queue<string> fX, Queue<string> fY, Queue<string> fZ, char[] vars,
            Range[] ranges, Plot3D p, Dictionary<char, double> d, int a, Color color)
        {
            if (a >= vars.Length - 1)
            {
                var num = ranges[a].Min;
                while (num <= (double) ranges[a].Max)
                {
                    d[vars[a]] = num;
                    p.AddPoint(new PPoint3D((float) o.Solve(fX, d), (float) o.Solve(fY, d), (float) o.Solve(fZ, d),
                        color));
                    num += ranges[a].Diff;
                }
            }
            else
            {
                var num = ranges[a].Min;
                while (num <= (double) ranges[a].Max)
                {
                    d[vars[a]] = num;
                    PlotP(o, fX, fY, fZ, vars, ranges, p, d, a + 1, color);
                    num += ranges[a].Diff;
                }
            }
        }

        public void AddPoint(PPoint3D p)
        {
            if (p.Z > (double) MaxZ)
                MaxZ = p.Z;
            if (p.Z < (double) MinZ)
                MinZ = p.Z;
            _points.Add(p);
        }

        public void ClearPoints()
        {
            MaxZ = float.MinValue;
            MinZ = float.MaxValue;
            _points.Clear();
        }

        public int Count()
        {
            return _points.Count;
        }

        public IEnumerable<PPoint3D> GetPoints()
        {
            return _points;
        }

        public Bitmap ToBitMap(int w, int h)
        {
            var num1 = float.MaxValue;
            var num2 = float.MinValue;
            var num3 = float.MaxValue;
            var num4 = float.MinValue;
            foreach (PPoint3D point in _points)
            {
                if (point.X < (double) num1)
                    num1 = point.X;
                if (point.X > (double) num2)
                    num2 = point.X;
                if (point.Y < (double) num3)
                    num3 = point.Y;
                if (point.Y > (double) num4)
                    num4 = point.Y;
            }
            _points.Sort(new DistComparer
            {
                CamX = (float) (num1 - 0.0500000007450581 * (num2 - (double) num1)),
                CamY = (float) (num3 - 0.0500000007450581 * (num4 - (double) num3)),
                CamZ = (float) (MaxZ * 1.04999995231628)
            });
            var num5 = float.MaxValue;
            var num6 = float.MinValue;
            var num7 = float.MaxValue;
            var num8 = float.MinValue;
            var ppoint2DList = new List<PPoint2D>();
            foreach (PPoint3D point in _points)
            {
                PPoint2D ppoint2D = new PPoint2D((float) (0.5 * (point.X - (double) point.Y)), 0.5f * point.Y + point.Z,
                    point.Color, point.Visible);
                ppoint2DList.Add(ppoint2D);
                if (ppoint2D.X < (double) num5)
                    num5 = ppoint2D.X;
                if (ppoint2D.X > (double) num6)
                    num6 = ppoint2D.X;
                if (ppoint2D.Y < (double) num7)
                    num7 = ppoint2D.Y;
                if (ppoint2D.Y > (double) num8)
                    num8 = ppoint2D.Y;
            }
            Bitmap bitmap = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            Graphics graphics = Graphics.FromImage(bitmap);
            if (_points.Count < 2 || Math.Abs(num6 - num5) + (double) Math.Abs(num8 - num7) < 5.60519385729927E-44)
            {
                graphics.Clear(Color.Black);
            }
            else
            {
                graphics.Clear(Color.White);
                var num9 = (float) (w * 0.899999976158142 / (num6 - (double) num5));
                var num10 = (float) (h * 0.899999976158142 / (num8 - (double) num7));
                var num11 = MaxZ - MinZ;
                var num12 = Size * 0.5f;
                var num13 = w * 0.025f;
                var num14 = h * 0.025f;
                var num15 = 1f / num13;
                var num16 = 1f / num14;
                var num17 = (float) ((num16 + (double) num15) * 0.100000001490116);
                var num18 = num13 - num12;
                var num19 = num14 - num12;
                for (var index = 0; index < ppoint2DList.Count; ++index)
                {
                    PPoint2D ppoint2D = ppoint2DList[index];
                    if (ppoint2D.Visible)
                    {
                        var x = (ppoint2D.X - num5) * num9 + num18;
                        var y = (num8 - ppoint2D.Y) * num10 + num19;
                        if (Math.Abs(LinesH) > 4.20389539297445E-45 && Extensions.Mod(_points[index].X, LinesH) < num15 ||
                            Math.Abs(LinesV) > 4.20389539297445E-45 && Extensions.Mod(_points[index].Y, LinesV) < num16 ||
                            Math.Abs(LinesL) > 4.20389539297445E-45 && Extensions.Mod(_points[index].Z, LinesL) < num17)
                            graphics.FillEllipse(Brushes.Black, x, y, Size, Size);
                        else if (ppoint2D.Color == Color.Empty)
                            graphics.FillEllipse(
                                new SolidBrush(Extensions.HsvToRgb(360.0 * (_points[index].Z - (double) MinZ) / num11,
                                    1.0, 1.0)), x, y, Size, Size);
                        else
                            graphics.FillEllipse(new SolidBrush(ppoint2D.Color), x, y, Size, Size);
                    }
                }
            }
            return bitmap;
        }

        public Bitmap ToBitMap(int w, int h, Rectangle3D view)
        {
            var num1 = view.X;
            var num2 = view.X + view.Width;
            var num3 = view.Y;
            var num4 = view.Y + view.Height;
            _points.Sort(new DistComparer
            {
                CamX = (float) (num1 - 0.0500000007450581 * (num2 - (double) num1)),
                CamY = (float) (num3 - 0.0500000007450581 * (num4 - (double) num3)),
                CamZ = (float) (MaxZ * 1.04999995231628)
            });
            var ppoint2DList = new List<PPoint2D>();
            foreach (PPoint3D point in _points)
            {
                PPoint2D ppoint2D1 = new PPoint2D((float) (0.5 * (point.X - (double) point.Y)), 0.5f * point.Y + point.Z,
                    point.Color);
                var num5 = point.Visible ? 1 : 0;
                ppoint2D1.Visible = num5 != 0;
                PPoint2D ppoint2D2 = ppoint2D1;
                ppoint2DList.Add(ppoint2D2);
            }
            Bitmap bitmap = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            Graphics graphics = Graphics.FromImage(bitmap);
            if (Math.Abs(num2 - num1) + (double) Math.Abs(num4 - num3) < 5.60519385729927E-44)
            {
                graphics.Clear(Color.Black);
            }
            else
            {
                graphics.Clear(Color.White);
                var num5 = (float) (w * 0.899999976158142 / (num2 - (double) num1));
                var num6 = (float) (h * 0.899999976158142 / (num4 - (double) num3));
                var num7 = MaxZ - MinZ;
                var num8 = Size * 0.5f;
                var num9 = w * 0.025f;
                var num10 = h * 0.025f;
                var num11 = 1f / num9;
                var num12 = 1f / num10;
                var num13 = (float) ((num12 + (double) num11) * 0.100000001490116);
                var num14 = num9 - num8;
                var num15 = num10 - num8;
                for (var index = 0; index < ppoint2DList.Count; ++index)
                {
                    PPoint2D ppoint2D = ppoint2DList[index];
                    if (ppoint2D.Visible)
                    {
                        var x = (ppoint2D.X - num1) * num5 + num14;
                        var y = (num4 - ppoint2D.Y) * num6 + num15;
                        if (Math.Abs(LinesH) > 4.20389539297445E-45 && Extensions.Mod(_points[index].X, LinesH) < num11 ||
                            Math.Abs(LinesV) > 4.20389539297445E-45 && Extensions.Mod(_points[index].Y, LinesV) < num12 ||
                            Math.Abs(LinesL) > 4.20389539297445E-45 && Extensions.Mod(_points[index].Z, LinesL) < num13)
                            graphics.FillEllipse(Brushes.Black, x, y, Size, Size);
                        else if (ppoint2D.Color == Color.Empty)
                            graphics.FillEllipse(
                                new SolidBrush(Extensions.HsvToRgb(360.0 * (_points[index].Z - (double) MinZ) / num7,
                                    1.0, 1.0)), x, y, Size, Size);
                        else
                            graphics.FillEllipse(new SolidBrush(ppoint2D.Color), x, y, Size, Size);
                    }
                }
            }
            return bitmap;
        }

        public Bitmap ToBitMap(int w, int h, Rectangle3D view, PPoint3D light)
        {
            var num1 = view.X;
            var num2 = view.X + view.Width;
            var num3 = view.Y;
            var num4 = view.Y + view.Height;
            DistComparer comparer = new DistComparer
            {
                CamX = (float) (num1 - 0.0500000007450581 * (num2 - (double) num1)),
                CamY = (float) (num3 - 0.0500000007450581 * (num4 - (double) num3)),
                CamZ = (float) (MaxZ * 1.04999995231628)
            };
            _points.Sort(comparer);
            var ppoint2DList = new List<PPoint2D>();
            foreach (PPoint3D point in _points)
            {
                PPoint2D ppoint2D1 = new PPoint2D((float) (0.5 * (point.X - (double) point.Y)), 0.5f * point.Y + point.Z,
                    point.Color);
                var num5 = point.Visible ? 1 : 0;
                ppoint2D1.Visible = num5 != 0;
                PPoint2D ppoint2D2 = ppoint2D1;
                ppoint2DList.Add(ppoint2D2);
            }
            Bitmap bitmap = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            Graphics graphics = Graphics.FromImage(bitmap);
            if (Math.Abs(num2 - num1) + (double) Math.Abs(num4 - num3) < 5.60519385729927E-44)
            {
                graphics.Clear(Color.Black);
            }
            else
            {
                graphics.Clear(Color.White);
                var num5 = (float) (w * 0.899999976158142 / (num2 - (double) num1));
                var num6 = (float) (h * 0.899999976158142 / (num4 - (double) num3));
                var num7 = MaxZ - MinZ;
                var num8 = Size * 0.5f;
                var num9 = w * 0.025f;
                var num10 = h * 0.025f;
                var num11 = 1f / num9;
                var num12 = 1f / num10;
                var num13 = (float) ((num12 + (double) num11) * 0.100000001490116);
                var num14 = num9 - num8;
                var num15 = num10 - num8;
                for (var index = 0; index < ppoint2DList.Count; ++index)
                {
                    PPoint2D ppoint2D = ppoint2DList[index];
                    if (ppoint2D.Visible)
                    {
                        var d = ColorLight(_points[index], comparer, light);
                        var x = (ppoint2D.X - num1) * num5 + num14;
                        var y = (num4 - ppoint2D.Y) * num6 + num15;
                        if (Math.Abs(LinesH) > 4.20389539297445E-45 && Extensions.Mod(_points[index].X, LinesH) < num11 ||
                            Math.Abs(LinesV) > 4.20389539297445E-45 && Extensions.Mod(_points[index].Y, LinesV) < num12 ||
                            Math.Abs(LinesL) > 4.20389539297445E-45 && Extensions.Mod(_points[index].Z, LinesL) < num13)
                            graphics.FillEllipse(Brushes.Black, x, y, Size, Size);
                        else if (ppoint2D.Color == Color.Empty)
                            graphics.FillEllipse(
                                new SolidBrush(
                                    ColorMultiply(
                                        Extensions.HsvToRgb(360.0 * (_points[index].Z - (double) MinZ) / num7, 1.0, 1.0),
                                        d)), x, y, Size, Size);
                        else
                            graphics.FillEllipse(new SolidBrush(ColorMultiply(ppoint2D.Color, d)), x, y, Size, Size);
                    }
                }
            }
            return bitmap;
        }

        private Color ColorMultiply(Color c, float d)
        {
            return Color.FromArgb((int) (c.R * (double) d), (int) (c.G * (double) d), (int) (c.B * (double) d));
        }

        private float ColorLight(PPoint3D pPoint3D, DistComparer comparer, PPoint3D light)
        {
            PPoint3D ppoint3D1 = new PPoint3D(pPoint3D.X - comparer.CamX, pPoint3D.Y - comparer.CamY,
                pPoint3D.Z - comparer.CamZ);
            PPoint3D ppoint3D2 = new PPoint3D(pPoint3D.X - light.X, pPoint3D.Y - light.Y, pPoint3D.Z - light.Z);
            return
                (float)
                (((ppoint3D1.X * (double) ppoint3D2.X + ppoint3D1.Y * (double) ppoint3D2.Y +
                   ppoint3D1.Z * (double) ppoint3D2.Z) /
                  Math.Sqrt(ppoint3D1.SquareLen() * (double) ppoint3D2.SquareLen()) + 1.0) / 2.0);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Threading.Tasks;
using Useful.Functions;
using Useful.Plotting.Transformations;

namespace Useful.Plotting
{
  public class Plot2D
  {
    private readonly List<PPoint2D> _points = new List<PPoint2D>();
    public float Size = 1f;
    public float LinesH;
    public float LinesV;
    public bool Connect;

      public static Plot2D operator +(Plot2D a, PPoint2D b)
    {
      Plot2D plot2D = new Plot2D();
      plot2D._points.AddRange(a._points);
      plot2D._points.Add(b);
      double num1 = a.Size;
      plot2D.Size = (float) num1;
      int num2 = a.Connect ? 1 : 0;
      plot2D.Connect = num2 != 0;
      double num3 = a.LinesH;
      plot2D.LinesH = (float) num3;
      double num4 = a.LinesV;
      plot2D.LinesV = (float) num4;
      return plot2D;
    }

    public static Plot2D operator +(Plot2D a, Plot2D b)
    {
      Plot2D plot2D = new Plot2D();
      plot2D._points.AddRange(a._points);
      plot2D._points.AddRange(b._points);
      double num1 = (a.Size + (double) b.Size) * 0.5;
      plot2D.Size = (float) num1;
      int num2 = !a.Connect ? 0 : (b.Connect ? 1 : 0);
      plot2D.Connect = num2 != 0;
      double num3 = (a.LinesH + (double) b.LinesH) * 0.5;
      plot2D.LinesH = (float) num3;
      double num4 = (a.LinesV + (double) b.LinesV) * 0.5;
      plot2D.LinesV = (float) num4;
      return plot2D;
    }

    public static Plot2D operator *(Plot2D a, Transform2DList b)
    {
      List<PPoint2D> l = a._points.ConvertAll(el => new PPoint2D(el.X, el.Y, el.Color, el.Visible));
      Parallel.For(0, a._points.Count, i =>
      {
          PPoint2D point = a._points[i];
          foreach (ITransformation2D transform in b.Transforms)
              transform.Transform(ref point, l);
          a._points[i] = point;
      });
      return a;
    }

    public static Plot2D operator /(Plot2D a, Transform2DList b)
    {
      List<PPoint2D> l = a._points.ConvertAll(el => new PPoint2D(el.X, el.Y, el.Color, el.Visible));
      Parallel.For(0, a._points.Count, i =>
      {
          PPoint2D point = a._points[i];
          foreach (ITransformation2D transform in b.Transforms)
          {
              INvertibleTransformation2D transformation2D = transform as INvertibleTransformation2D;
              if (transformation2D == null)
                  throw new InvalidCastException("Transformation not invertible!");
              transformation2D.Invert(ref point, l);
          }
          a._points[i] = point;
      });
      return a;
    }

    public static Plot2D operator *(Plot2D a, ITransformation2D b)
    {
      List<PPoint2D> l = a._points.ConvertAll(el => new PPoint2D(el.X, el.Y, el.Color, el.Visible));
      Parallel.For(0, a._points.Count, i =>
      {
          PPoint2D point = a._points[i];
          b.Transform(ref point, l);
          a._points[i] = point;
      });
      return a;
    }

    public static Plot2D operator /(Plot2D a, ITransformation2D b)
    {
      List<PPoint2D> l = a._points.ConvertAll(el => new PPoint2D(el.X, el.Y, el.Color, el.Visible));
      Parallel.For(0, a._points.Count, i =>
      {
          PPoint2D point = a._points[i];
          INvertibleTransformation2D transformation2D = b as INvertibleTransformation2D;
          if (transformation2D == null)
              throw new InvalidCastException("Transformation not invertible!");
          transformation2D.Invert(ref point, l);
          a._points[i] = point;
      });
      return a;
    }

    public static Plot2D PlotFunction(string formula, float minX, float maxX)
    {
      Plot2D plot2D = new Plot2D() { Connect = true };
      float x = minX;
      float num = (float) (1.0 / 800.0 * (maxX - (double) minX));
      if (num < 0.0)
        throw new ArgumentException("maxX<minX !");
      Onp onp = new Onp();
      Queue<string> onPformula = onp.Parse(formula);
      while (x <= (double) maxX)
      {
        plot2D._points.Add(new PPoint2D(x, (float) onp.Solve(onPformula, x)));
        x += num;
      }
      return plot2D;
    }

    public static Plot2D PlotFunction(string formula, float minX, float maxX, Color color)
    {
      Plot2D plot2D = new Plot2D() { Connect = true };
      float x = minX;
      float num = (float) (1.0 / 800.0 * (maxX - (double) minX));
      if (num < 0.0)
        throw new ArgumentException("maxX<minX !");
      Onp onp = new Onp();
      Queue<string> onPformula = onp.Parse(formula);
      while (x <= (double) maxX)
      {
        plot2D._points.Add(new PPoint2D(x, (float) onp.Solve(onPformula, x), color));
        x += num;
      }
      return plot2D;
    }

    public static Plot2D PlotFunction(string formula, Range xx)
    {
      Plot2D plot2D = new Plot2D() { Connect = true };
      float x = xx.Min;
      if (xx.Diff < 0.0)
        throw new ArgumentException("maxX<minX !");
      Onp onp = new Onp();
      Queue<string> onPformula = onp.Parse(formula);
      while (x <= (double) xx.Max)
      {
        plot2D._points.Add(new PPoint2D(x, (float) onp.Solve(onPformula, x)));
        x += xx.Diff;
      }
      return plot2D;
    }

    public static Plot2D PlotFunction(string formula, Range xx, Color color)
    {
      Plot2D plot2D = new Plot2D() { Connect = true };
      float x = xx.Min;
      if (xx.Diff < 0.0)
        throw new ArgumentException("maxX<minX !");
      Onp onp = new Onp();
      Queue<string> onPformula = onp.Parse(formula);
      while (x <= (double) xx.Max)
      {
        plot2D._points.Add(new PPoint2D(x, (float) onp.Solve(onPformula, x), color));
        x += xx.Diff;
      }
      return plot2D;
    }

    public static Plot2D PlotField(string formula, float minX, float maxX, float minY, float maxY)
    {
      Plot2D plot2D = new Plot2D();
      float x1 = minX;
      float num1 = (float) (1.0 / 800.0 * (maxX - (double) minX));
      float num2 = (float) (1.0 / 800.0 * (maxY - (double) minY));
      if (num1 < 0.0 || num2 < 0.0)
        throw new ArgumentException("max<min !");
      Onp onp = new Onp();
      Queue<string> onPformula = onp.Parse(formula);
      Dictionary<char, double> x2 = new Dictionary<char, double>(2) { { 'x', 0.0 }, { 'y', 0.0 } };
      while (x1 <= (double) maxX)
      {
        x2['x'] = x1;
        float y = minY;
        while (y <= (double) maxY)
        {
          x2['y'] = y;
          if (onp.Solve(onPformula, x2) > 0.0)
            plot2D._points.Add(new PPoint2D(x1, y));
          y += num2;
        }
        x1 += num1;
      }
      return plot2D;
    }

    public static Plot2D PlotField(string formula, float minX, float maxX, float minY, float maxY, Color color)
    {
      Plot2D plot2D = new Plot2D();
      float x1 = minX;
      float num1 = (float) (1.0 / 800.0 * (maxX - (double) minX));
      float num2 = (float) (1.0 / 800.0 * (maxY - (double) minY));
      if (num1 < 0.0 || num2 < 0.0)
        throw new ArgumentException("max<min !");
      Onp onp = new Onp();
      Queue<string> onPformula = onp.Parse(formula);
      Dictionary<char, double> x2 = new Dictionary<char, double>(2) { { 'x', 0.0 }, { 'y', 0.0 } };
      while (x1 <= (double) maxX)
      {
        x2['x'] = x1;
        float y = minY;
        while (y <= (double) maxY)
        {
          x2['y'] = y;
          if (onp.Solve(onPformula, x2) > 0.0)
            plot2D._points.Add(new PPoint2D(x1, y, color));
          y += num2;
        }
        x1 += num1;
      }
      return plot2D;
    }

    public static Plot2D PlotField(string formula, Range xx, Range yy)
    {
      Plot2D plot2D = new Plot2D();
      float x1 = xx.Min;
      if (xx.Diff < 0.0 || yy.Diff < 0.0)
        throw new ArgumentException("max<min !");
      Onp onp = new Onp();
      Queue<string> onPformula = onp.Parse(formula);
      Dictionary<char, double> x2 = new Dictionary<char, double>(2) { { 'x', 0.0 }, { 'y', 0.0 } };
      while (x1 <= (double) xx.Max)
      {
        x2['x'] = x1;
        float y = yy.Min;
        while (y <= (double) yy.Max)
        {
          x2['y'] = y;
          if (onp.Solve(onPformula, x2) > 0.0)
            plot2D._points.Add(new PPoint2D(x1, y));
          y += yy.Diff;
        }
        x1 += xx.Diff;
      }
      return plot2D;
    }

    public static Plot2D PlotField(string formula, Range xx, Range yy, Color color)
    {
      Plot2D plot2D = new Plot2D();
      float x1 = xx.Min;
      if (xx.Diff < 0.0 || yy.Diff < 0.0)
        throw new ArgumentException("max<min !");
      Onp onp = new Onp();
      Queue<string> onPformula = onp.Parse(formula);
      Dictionary<char, double> x2 = new Dictionary<char, double>(2) { { 'x', 0.0 }, { 'y', 0.0 } };
      while (x1 <= (double) xx.Max)
      {
        x2['x'] = x1;
        float y = yy.Min;
        while (y <= (double) yy.Max)
        {
          x2['y'] = y;
          if (onp.Solve(onPformula, x2) > 0.0)
            plot2D._points.Add(new PPoint2D(x1, y, color));
          y += yy.Diff;
        }
        x1 += xx.Diff;
      }
      return plot2D;
    }

    public static Plot2D PlotParametric(string fX, string fY, char[] vars, Range[] ranges)
    {
      Plot2D p = new Plot2D();
      Onp o = new Onp();
      Queue<string> fX1 = o.Parse(fX);
      Queue<string> fY1 = o.Parse(fY);
      Dictionary<char, double> d = new Dictionary<char, double>();
      foreach (char var in vars)
        d.Add(var, 0.0);
      PlotP(o, fX1, fY1, vars, ranges, p, d, 0, Color.Black);
      return p;
    }

    public static Plot2D PlotParametric(string fX, string fY, char[] vars, Range[] ranges, Color color)
    {
      Plot2D p = new Plot2D();
      Onp o = new Onp();
      Queue<string> fX1 = o.Parse(fX);
      Queue<string> fY1 = o.Parse(fY);
      Dictionary<char, double> d = new Dictionary<char, double>();
      foreach (char var in vars)
        d.Add(var, 0.0);
      PlotP(o, fX1, fY1, vars, ranges, p, d, 0, color);
      return p;
    }

    private static void PlotP(Onp o, Queue<string> fX, Queue<string> fY, IReadOnlyList<char> vars, IReadOnlyList<Range> ranges, Plot2D p, Dictionary<char, double> d, int a, Color color)
    {
      if (a >= vars.Count - 1)
      {
        float num = ranges[a].Min;
        while (num <= (double) ranges[a].Max)
        {
          d[vars[a]] = num;
          p.AddPoint(new PPoint2D((float) o.Solve(fX, d), (float) o.Solve(fY, d), color));
          num += ranges[a].Diff;
        }
      }
      else
      {
        float num = ranges[a].Min;
        while (num <= (double) ranges[a].Max)
        {
          d[vars[a]] = num;
          PlotP(o, fX, fY, vars, ranges, p, d, a + 1, color);
          num += ranges[a].Diff;
        }
      }
    }

    public void AddPoint(PPoint2D p)
    {
      _points.Add(p);
    }

    public void ClearPoints()
    {
      _points.Clear();
    }

    public int Count()
    {
      return _points.Count;
    }

    public IEnumerable<PPoint2D> GetPoints()
    {
      return _points;
    }

    public Bitmap ToBitMap(int w, int h)
    {
      float num1 = float.MaxValue;
      float num2 = float.MinValue;
      float num3 = float.MaxValue;
      float num4 = float.MinValue;
      foreach (PPoint2D point in _points)
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
      Bitmap bitmap = new Bitmap(w, h, PixelFormat.Format24bppRgb);
      Graphics graphics1 = Graphics.FromImage(bitmap);
      if (_points.Count < 2 || Math.Abs(num2 - num1) + (double) Math.Abs(num4 - num3) < 5.60519385729927E-44)
      {
        graphics1.Clear(Color.Black);
      }
      else
      {
        Font font1 = new Font("Comic Sans", 12f);
        graphics1.Clear(Color.White);
        float num5 = (float) (w * 0.899999976158142 / (num2 - (double) num1));
        float num6 = (float) (h * 0.899999976158142 / (num4 - (double) num3));
        float num7 = Size * 0.5f;
        float num8 = w * 0.025f;
        float num9 = h * 0.025f;
        graphics1.DrawLine(Pens.Black, new PointF(-num1 * num5 + num8, h), new PointF(-num1 * num5 + num8, 0.0f));
        graphics1.DrawLine(Pens.Black, new PointF(w, -num3 * num6 + num9), new PointF(0.0f, -num3 * num6 + num9));
        float num10;
        if (Math.Abs(LinesH) > 4.20389539297445E-45)
        {
          for (int index = (int) Math.Floor(num3 / (double) LinesH) - 1; (double) index < Math.Ceiling(num4 / (double) LinesH) + 1.0; ++index)
          {
            graphics1.DrawLine(Pens.Black, new PointF(-num1 * num5, (num4 - index * LinesH) * num6 + num9), new PointF((float) (-(double) num1 * num5 + 2.0 * num8), (num4 - index * LinesH) * num6 + num9));
            Graphics graphics2 = graphics1;
            num10 = index * LinesH;
            string @string = num10.ToString(CultureInfo.CurrentCulture);
            Font font2 = font1;
            Brush black = Brushes.Black;
            PointF point = new PointF((float) (-(double) num1 * num5 + 0.5 * num8), (num4 - index * LinesH) * num6);
            graphics2.DrawString(@string, font2, black, point);
          }
        }
        if (Math.Abs(LinesV) > 4.20389539297445E-45)
        {
          for (int index = (int) Math.Floor(num1 / (double) LinesV) - 1; (double) index < Math.Ceiling(num2 / (double) LinesV) + 1.0; ++index)
          {
            if (index != 0)
            {
              graphics1.DrawLine(Pens.Black, new PointF((index * LinesV - num1) * num5 + num8, -num3 * num6), new PointF((index * LinesV - num1) * num5 + num8, (float) (-(double) num3 * num6 + 2.0 * num9)));
              Graphics graphics2 = graphics1;
              num10 = index * LinesV;
              string @string = num10.ToString(CultureInfo.CurrentCulture);
              Font font2 = font1;
              Brush black = Brushes.Black;
              PointF point = new PointF((float) ((index * (double) LinesV - num1) * num5 + 0.5 * num8), -num3 * num6 + num9);
              graphics2.DrawString(@string, font2, black, point);
            }
          }
        }
        float num11 = num8 - num7;
        float num12 = num9 - num7;
        float num13 = 0.0f;
        float num14 = 0.0f;
        for (int index = 0; index < _points.Count; ++index)
        {
          PPoint2D ppoint2D = _points[index];
          if (ppoint2D.Visible)
          {
            float x = (ppoint2D.X - num1) * num5 + num11;
            float y = (num4 - ppoint2D.Y) * num6 + num12;
            if (Connect && index != 0)
              graphics1.DrawLine(new Pen(ppoint2D.Color, Size), x + num7, y + num7, num13 + num7, num14 + num7);
            else
              graphics1.FillEllipse(new SolidBrush(ppoint2D.Color), x, y, Size, Size);
            num13 = x;
            num14 = y;
          }
        }
      }
      return bitmap;
    }

    public Bitmap ToBitMap(int w, int h, RectangleF view)
    {
      float x1 = view.X;
      float num1 = view.X + view.Width;
      float y1 = view.Y;
      float num2 = view.Y + view.Height;
      Bitmap bitmap = new Bitmap(w, h, PixelFormat.Format24bppRgb);
      Graphics graphics = Graphics.FromImage(bitmap);
      if (Math.Abs(num1 - x1) + (double) Math.Abs(num2 - y1) < 5.60519385729927E-44)
      {
        graphics.Clear(Color.Black);
      }
      else
      {
        Font font = new Font("Comic Sans", 12f);
        graphics.Clear(Color.White);
        float num3 = (float) (w * 0.899999976158142 / (num1 - (double) x1));
        float num4 = (float) (h * 0.899999976158142 / (num2 - (double) y1));
        float num5 = Size * 0.5f;
        float num6 = w * 0.025f;
        float num7 = h * 0.025f;
        graphics.DrawLine(Pens.Black, new PointF(-x1 * num3 + num6, h), new PointF(-x1 * num3 + num6, 0.0f));
        graphics.DrawLine(Pens.Black, new PointF(w, -y1 * num4 + num7), new PointF(0.0f, -y1 * num4 + num7));
        if (Math.Abs(LinesH) > 4.20389539297445E-45)
        {
          for (int index = (int) Math.Floor(y1 / (double) LinesH) - 1; (double) index < Math.Ceiling(num2 / (double) LinesH) + 1.0; ++index)
          {
            graphics.DrawLine(Pens.Black, new PointF(-x1 * num3, (num2 - index * LinesH) * num4 + num7), new PointF((float) (-(double) x1 * num3 + 2.0 * num6), (num2 - index * LinesH) * num4 + num7));
            graphics.DrawString(index.ToString(), font, Brushes.Black, new PointF((float) (-(double) x1 * num3 - 2.0 * num6), (float) ((num2 - index * (double) LinesH) * num4 - 1.5 * num7)));
          }
        }
        if (Math.Abs(LinesV) > 4.20389539297445E-45)
        {
          for (int index = (int) Math.Floor(x1 / (double) LinesV) - 1; (double) index < Math.Ceiling(num1 / (double) LinesV) + 1.0; ++index)
          {
            if (index != 0)
            {
              graphics.DrawLine(Pens.Black, new PointF((index * LinesV - x1) * num3 + num6, -y1 * num4), new PointF((index * LinesV - x1) * num3 + num6, (float) (-(double) y1 * num4 + 2.0 * num7)));
              graphics.DrawString(index.ToString(), font, Brushes.Black, new PointF((index * LinesV - x1) * num3, (float) (-(double) y1 * num4 + 2.5 * num7)));
            }
          }
        }
        float num8 = num6 - num5;
        float num9 = num7 - num5;
        float num10 = 0.0f;
        float num11 = 0.0f;
        for (int index = 0; index < _points.Count; ++index)
        {
          PPoint2D ppoint2D = _points[index];
          if (ppoint2D.Visible)
          {
            float x2 = (ppoint2D.X - x1) * num3 + num8;
            float y2 = (num2 - ppoint2D.Y) * num4 + num9;
            if (Connect && index != 0)
              graphics.DrawLine(new Pen(ppoint2D.Color, Size), x2 + num5, y2 + num5, num10 + num5, num11 + num5);
            else
              graphics.FillEllipse(new SolidBrush(ppoint2D.Color), x2, y2, Size, Size);
            num10 = x2;
            num11 = y2;
          }
        }
      }
      return bitmap;
    }
  }
}

using System;
using System.Collections.Generic;

namespace Useful.Plotting.Transformations
{
  public class Rotation2D : INvertibleTransformation2D
  {
    private float _a;
    private float _c;
    private float _s;

    public float Alpha
    {
      get
      {
        return _a;
      }
      set
      {
        _a = value;
        _c = (float) Math.Cos((double) value);
        _s = (float) Math.Sin((double) value);
      }
    }

    public Rotation2D(float a)
    {
      Alpha = a;
    }

    public void Transform(ref PPoint2D point, List<PPoint2D> allPoints)
    {
      float num = (float) ((double) point.X * (double) _c - (double) point.Y * (double) _s);
      point.Y = (float) ((double) point.X * (double) _s + (double) point.Y * (double) _c);
      point.X = num;
    }

    public void Invert(ref PPoint2D point, List<PPoint2D> allPoints)
    {
      float num = (float) ((double) point.X * (double) _c + (double) point.Y * (double) _s);
      point.Y = (float) ((double) point.Y * (double) _c - (double) point.X * (double) _s);
      point.X = num;
    }
  }
}

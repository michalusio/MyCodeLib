using System;
using System.Collections.Generic;

namespace Useful.Plotting.Transformations
{
  public class Rotation3D : INvertibleTransformation3D
  {
    private float _a;
    private float _ca;
    private float _sa;
    private float _b;
    private float _cb;
    private float _sb;
    private float _c;
    private float _cc;
    private float _sc;

    public float Alpha
    {
      get
      {
        return _a;
      }
      set
      {
        _a = value;
        _ca = (float) Math.Cos((double) value);
        _sa = (float) Math.Sin((double) value);
      }
    }

    public float Beta
    {
      get
      {
        return _b;
      }
      set
      {
        _b = value;
        _cb = (float) Math.Cos((double) value);
        _sb = (float) Math.Sin((double) value);
      }
    }

    public float Gamma
    {
      get
      {
        return _c;
      }
      set
      {
        _c = value;
        _cc = (float) Math.Cos((double) value);
        _sc = (float) Math.Sin((double) value);
      }
    }

    public Rotation3D(float a, float b, float c)
    {
      Alpha = a;
      Beta = b;
      Gamma = c;
    }

    public void Transform(ref PPoint3D point, List<PPoint3D> allPoints)
    {
      float num1 = (float) ((double) point.X * ((double) _cc * (double) _cb - (double) _sa * (double) _sb * (double) _sc) - (double) point.Y * (double) _ca * (double) _sc + (double) point.Z * ((double) _cc * (double) _sb + (double) _sa * (double) _sc * (double) _cb));
      float num2 = (float) ((double) point.X * ((double) _sc * (double) _cb + (double) _sa * (double) _sb * (double) _cc) + (double) point.Y * (double) _ca * (double) _cc + (double) point.Z * ((double) _sc * (double) _sb - (double) _sa * (double) _cb * (double) _cc));
      point.Z = (float) ((double) point.Y * (double) _sa + (double) point.Z * (double) _ca * (double) _cb - (double) point.X * (double) _ca * (double) _sb);
      point.X = num1;
      point.Y = num2;
    }

    public void Invert(ref PPoint3D point, List<PPoint3D> allPoints)
    {
      float num1 = (float) ((double) point.X * ((double) _cc * (double) _cb + (double) _sa * (double) _sb * (double) _sc) + (double) point.Y * (double) _ca * (double) _sc - (double) point.Z * ((double) _cc * (double) _sb + (double) _sa * (double) _sc * (double) _cb));
      float num2 = (float) (-(double) point.X * ((double) _sc * (double) _cb + (double) _sa * (double) _sb * (double) _cc) + (double) point.Y * (double) _ca * (double) _cc + (double) point.Z * ((double) _sc * (double) _sb + (double) _sa * (double) _cb * (double) _cc));
      point.Z = (float) (-(double) point.Y * (double) _sa + (double) point.Z * (double) _ca * (double) _cb + (double) point.X * (double) _ca * (double) _sb);
      point.X = num1;
      point.Y = num2;
    }
  }
}

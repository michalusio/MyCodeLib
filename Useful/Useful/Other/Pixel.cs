using System;

namespace Useful.Other
{
  public class Pixel
  {
    public int R;
    public int G;
    public int B;

    public Pixel(byte r, byte g, byte b)
    {
      R =  r;
      G =  g;
      B =  b;
    }

    public Pixel(int r, int g, int b)
    {
      R = r;
      G = g;
      B = b;
    }

    public Pixel Add(Pixel p)
    {
      R = R + p.R;
      G = G + p.G;
      B = B + p.B;
      return this;
    }

    public Pixel Div(int x)
    {
      R = R / x;
      G = G / x;
      B = B / x;
      return this;
    }

    public override string ToString()
    {
      int num = Math.Min(R, Math.Min(G, B));
      return "[" + (num < 0 ? R - num : R) + "|" + (num < 0 ? G - num : G) + "|" + (num < 0 ? B - num : B) + "]";
    }

    public byte GetR()
    {
      return (byte) Math.Min(byte.MaxValue, Math.Max(R, 0));
    }

    public byte GetG()
    {
      return (byte) Math.Min(byte.MaxValue, Math.Max(G, 0));
    }

    public byte GetB()
    {
      return (byte) Math.Min(byte.MaxValue, Math.Max(B, 0));
    }
  }
}

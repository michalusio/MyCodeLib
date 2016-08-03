using System;

namespace Useful.Other
{
  public static class Hashing
  {
    private static readonly byte[] _ = new byte[256];

    public static void Permute(Random rnd)
    {
      for (var index = 0; index < 256; ++index)
        _[index] = (byte) index;
      for (var index = 0; index < 256; ++index)
      {
        var num1 = (byte) rnd.Next(256);
        var num2 = _[index];
        _[index] = _[num1];
        _[num1] = num2;
      }
    }

    public static byte PearsonB(string str, byte h)
    {
      for (var index = 1; index < str.Length; ++index)
        h = _[h ^ str[index]];
      return h;
    }

    public static short PearsonS(string str)
    {
      var num1 = (byte) str[0];
      var num2 = (byte) (str[0] + 1 & byte.MaxValue);
      for (var index = 1; index < str.Length; ++index)
      {
        num1 = _[num1 ^ str[index]];
        num2 = _[num2 ^ str[index]];
      }
      return (short) ((num2 << 8) + num1);
    }

    public static int PearsonI(string str)
    {
      var num1 = (byte) str[0];
      var num2 = (byte) (str[0] + 1 & byte.MaxValue);
      var num3 = (byte) (str[0] + 2 & byte.MaxValue);
      var num4 = (byte) (str[0] + 3 & byte.MaxValue);
      for (var index = 1; index < str.Length; ++index)
      {
        num1 = _[num1 ^ str[index]];
        num2 = _[num2 ^ str[index]];
        num3 = _[num3 ^ str[index]];
        num4 = _[num4 ^ str[index]];
      }
      return (num4 << 24) + (num3 << 16) + (num2 << 8) + num1;
    }

    public static long PearsonL(string str)
    {
      var num1 = (byte) str[0];
      var num2 = (byte) (str[0] + 1 & byte.MaxValue);
      var num3 = (byte) (str[0] + 2 & byte.MaxValue);
      var num4 = (byte) (str[0] + 3 & byte.MaxValue);
      var num5 = (byte) (str[0] + 4 & byte.MaxValue);
      var num6 = (byte) (str[0] + 5 & byte.MaxValue);
      var num7 = (byte) (str[0] + 6 & byte.MaxValue);
      var num8 = (byte) (str[0] + 7 & byte.MaxValue);
      for (var index = 1; index < str.Length; ++index)
      {
        num1 = _[num1 ^ str[index]];
        num2 = _[num2 ^ str[index]];
        num3 = _[num3 ^ str[index]];
        num4 = _[num4 ^ str[index]];
        num5 = _[num5 ^ str[index]];
        num6 = _[num6 ^ str[index]];
        num7 = _[num7 ^ str[index]];
        num8 = _[num8 ^ str[index]];
      }
      return (num8 << 24) + (num7 << 16) + (num6 << 8) + num5 + (num4 << 24) + (num3 << 16) + (num2 << 8) + num1;
    }
  }
}

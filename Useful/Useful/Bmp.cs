using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;

namespace Useful
{
  public static class Bmp
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void WriteInt(Stream raf, int i)
    {
      raf.Write(BitConverter.GetBytes(i), 0, 4);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void WriteShort(Stream raf, short s)
    {
      raf.Write(BitConverter.GetBytes(s), 0, 2);
    }

    public static void Save(string path, Pixel[] array, int w)
    {
      try
      {
        FileStream fileStream = new FileStream(path, FileMode.Create);
        fileStream.WriteByte(66);
        fileStream.WriteByte(77);
        WriteInt(fileStream, 54 + array.Length << 2);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        WriteInt(fileStream, 54);
        WriteInt(fileStream, 40);
        WriteInt(fileStream, w);
        WriteInt(fileStream, array.Length / w);
        WriteShort(fileStream, 1);
        WriteShort(fileStream, 32);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        WriteInt(fileStream, array.Length << 2);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        fileStream.WriteByte(0);
        foreach (Pixel pixel in array)
        {
          fileStream.WriteByte(pixel.GetB());
          fileStream.WriteByte(pixel.GetG());
          fileStream.WriteByte(pixel.GetR());
          fileStream.WriteByte(0);
        }
        fileStream.Close();
      }
      catch (IOException ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    public static Color[] Encode(Pixel[] array, int factor)
    {
      List<Color> colorList = new List<Color>();
      Pixel pixel1 = array[0];
      int index = 1;
      int alpha = 0;
      for (; index < array.Length; ++index)
      {
        Pixel pixel2 = array[index];
        if (Math.Abs(pixel2.R - pixel1.R) + Math.Abs(pixel2.B - pixel1.B) + Math.Abs(pixel2.G - pixel1.G) > factor)
        {
          colorList.Add(Color.FromArgb(alpha, pixel1.GetR(), pixel1.GetG(), pixel1.GetB()));
          pixel1 = pixel2;
          alpha = 0;
        }
        else
          ++alpha;
      }
      colorList.Add(Color.FromArgb(alpha, pixel1.GetR(), pixel1.GetG(), pixel1.GetB()));
      return colorList.ToArray();
    }
  }
}

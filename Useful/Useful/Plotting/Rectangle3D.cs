﻿namespace Useful.Plotting
{
    public class Rectangle3D
    {
        public float Depth;
        public float Height;
        public float Width;
        public float X;
        public float Y;
        public float Z;

        public Rectangle3D(float x, float y, float z, float w, float h, float d)
        {
            X = x;
            Y = y;
            Z = z;
            Width = w;
            Height = h;
            Depth = d;
        }
    }
}
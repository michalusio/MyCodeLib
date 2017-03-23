using System;
using System.Collections.Generic;

namespace Useful.Plotting.Transformations
{
    public class Rotation3D : INvertibleTransformation3D
    {
        private float _a;
        private float _b;
        private float _c;
        private float _ca;
        private float _cb;
        private float _cc;
        private float _sa;
        private float _sb;
        private float _sc;

        public Rotation3D(float a, float b, float c)
        {
            Alpha = a;
            Beta = b;
            Gamma = c;
        }

        public float Alpha
        {
            get { return _a; }
            set
            {
                _a = value;
                _ca = (float) Math.Cos(value);
                _sa = (float) Math.Sin(value);
            }
        }

        public float Beta
        {
            get { return _b; }
            set
            {
                _b = value;
                _cb = (float) Math.Cos(value);
                _sb = (float) Math.Sin(value);
            }
        }

        public float Gamma
        {
            get { return _c; }
            set
            {
                _c = value;
                _cc = (float) Math.Cos(value);
                _sc = (float) Math.Sin(value);
            }
        }

        public void Transform(ref PPoint3D point, List<PPoint3D> allPoints)
        {
            var num1 =
                (float)
                (point.X * (_cc * (double) _cb - _sa * (double) _sb * _sc) - point.Y * (double) _ca * _sc +
                 point.Z * (_cc * (double) _sb + _sa * (double) _sc * _cb));
            var num2 =
                (float)
                (point.X * (_sc * (double) _cb + _sa * (double) _sb * _cc) + point.Y * (double) _ca * _cc +
                 point.Z * (_sc * (double) _sb - _sa * (double) _cb * _cc));
            point.Z = (float) (point.Y * (double) _sa + point.Z * (double) _ca * _cb - point.X * (double) _ca * _sb);
            point.X = num1;
            point.Y = num2;
        }

        public void Invert(ref PPoint3D point, List<PPoint3D> allPoints)
        {
            var num1 =
                (float)
                (point.X * (_cc * (double) _cb + _sa * (double) _sb * _sc) + point.Y * (double) _ca * _sc -
                 point.Z * (_cc * (double) _sb + _sa * (double) _sc * _cb));
            var num2 =
                (float)
                (-(double) point.X * (_sc * (double) _cb + _sa * (double) _sb * _cc) + point.Y * (double) _ca * _cc +
                 point.Z * (_sc * (double) _sb + _sa * (double) _cb * _cc));
            point.Z = (float) (-(double) point.Y * _sa + point.Z * (double) _ca * _cb + point.X * (double) _ca * _sb);
            point.X = num1;
            point.Y = num2;
        }
    }
}
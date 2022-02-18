using System.Windows;
using System;
namespace IntegraControlsXL.Common
{
    public class Size
    {
        public Size() { }
        public Size(double sx, double sy) { X = sx; Y = sy; }
        public double X = 0;
        public double Y = 0;

        public static implicit operator Rect(Size instance)
        {
            if (instance == null)
                return new Rect(0, 0, 0, 0);

            return new Rect(0, 0, Math.Max(instance.X, 0), Math.Max(instance.Y, 0));
        }
    }
}

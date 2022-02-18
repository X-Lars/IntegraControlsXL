using System.Windows;

namespace IntegraControlsXL.Common
{
    public class Range
    {
        public Range(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public double Min { get; }
        public double Max { get; }
    }

    public class Limit
    {
        public Limit(double minX, double sizeX, double minY, double sizeY)
        { 
            MinX = minX; 
            MaxX = minX + sizeX;
            MinY = minY;
            MaxY = minY + sizeY;
        }

        public Range X => new (MinX, MaxX);
        public Range Y => new (MinY, MaxY);

        public double CX => SX / 2;
        public double CY => SX / 2;

        public double SX => MaxX - MinX;
        public double SY => MaxY - MinY;

        public double MinX { get; }
        public double MaxX { get; }
        public double MinY { get; }
        public double MaxY { get; }

        public static implicit operator Rect(Limit instance)
        {
            if (instance == null)
                return new Rect(0, 0, 0, 0);

            return new Rect(new Point(instance.MinX, instance.MinY), new Point(instance.MaxX, instance.MaxY));
        }
    }

    public class LimitX : Limit
    {
        public LimitX(double x, double y, double size) : base(x, size, y, 0) { }
    }

    public class LimitY : Limit
    {
        public LimitY(double x, double y, double size) : base(x, 0, y, size) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IntegraControlsXL.Common
{
    public class LimitedPoint
    {
        private List<LimitedPoint> _LimitedPoints = new List<LimitedPoint>();

        public double X { get; set; }
        public double Y { get; set; }

        public double MinX { get; set; }
        public double MinY { get; set; }

        public double MaxX { get; set; }
        public double MaxY { get; set; }

        public LimitedPoint(double x, double y) : this(x, y, 0, 0, 0, 0) { }

        public LimitedPoint(double x, double y, double minX, double minY, double maxX, double maxY)
        {
            this.X = x;
            this.Y = y;
            this.MinX = minX;
            this.MinY = minY;
            this.MaxX = maxX;
            this.MaxY = maxY;
        }

        public LimitedPoint this[int i]
        {
            get { return _LimitedPoints[i]; }
            set { _LimitedPoints[i] = value; }
        }

        public static implicit operator Point(LimitedPoint instance)
        {
            if (instance == null)
                return new Point();

            return new Point(instance.X, instance.Y);
        }
    }
}

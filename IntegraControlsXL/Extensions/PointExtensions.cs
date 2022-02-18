using IntegraControlsXL.Common;
using System.Windows;

namespace IntegraControlsXL.Extensions
{
    public static class PointExtensions
    {
        public static double Distance(this Point instance, Point point)
        {
            double distanceX = instance.X - point.X;
            double distanceY = instance.Y - point.Y;

            return distanceX * distanceX + distanceY * distanceY;
        }
    }
}

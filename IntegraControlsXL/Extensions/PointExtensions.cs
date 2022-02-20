using IntegraControlsXL.Common;
using System;
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

        public static void Sin(this Point instance, double frequency)
        {
            instance.Y = Math.Sin(frequency * (2 * Math.PI) * instance.X);
        }
    }
}

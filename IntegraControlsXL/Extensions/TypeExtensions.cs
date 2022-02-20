using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegraControlsXL.Extensions
{
    public static class TypeExtensions
    {
        public static double Radians(this double degrees)
        {
            return Math.PI * degrees / 180;
        }

        public static double Degrees(this double radians)
        {
            return radians * (180 / Math.PI);
        }
    }
}

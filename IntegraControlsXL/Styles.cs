using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using StylesXL;

namespace IntegraControlsXL
{
    internal static class Styles
    {
        static Styles()
        {
            StyleManager.Initialize();

            StyleManager.StyleChanged += StyleChanged;

            StyleChanged(null, EventArgs.Empty);
        }

        private static void StyleChanged(object sender, EventArgs e)
        {
            GraphBorder = StyleManager.Brush(StylesXL.Brushes.Border);
            GraphBackground = StyleManager.Brush(StylesXL.Brushes.Control);
            GraphSelected = StyleManager.Brush(StylesXL.Brushes.ControlSelected);
            GraphHighlight = StyleManager.Brush(StylesXL.Brushes.Highlight);
            GraphTint = StyleManager.Brush(StylesXL.Brushes.Tint);
        }

        public static Brush GraphBorder { get; private set; }
        public static Brush GraphBackground { get; private set; }
        public static Brush GraphSelected { get; private set; }
        public static Brush GraphHighlight { get; private set; }
        public static Brush GraphTint { get; private set; }
    }
}

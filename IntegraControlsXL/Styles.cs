using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using StylesXL;
using StylesXL.Extensions;
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
            GraphBackground        = StyleManager.Brush(StylesXL.Brushes.Control);
            GraphBorder            = StyleManager.Brush(StylesXL.Brushes.Border);
            GraphHighlight         = StyleManager.Brush(StylesXL.Brushes.Highlight);
            GraphSelected          = StyleManager.Brush(StylesXL.Brushes.ControlSelected);
            GraphSelectedHighlight = StyleManager.Brush(StylesXL.Brushes.ControlHighlight);
            GraphTint              = StyleManager.Brush(StylesXL.Brushes.Tint);
            
            GraphBorderPen     = new Pen(GraphBorder, 1);
            GraphConstraintPen = new Pen(GraphTint, 1) { DashStyle = DashStyles.Dash };
            GraphGridPen       = new Pen(GraphBorder.Alpha(), 0.25) { DashStyle = DashStyles.Dot };
            GraphSelectedPen   = new Pen(GraphSelected, 1);

            GraphPattern = new DrawingBrush()
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect(0, 0, 10, 10),
                ViewportUnits = BrushMappingMode.Absolute,
                Viewbox = new Rect(0, 0, 10, 10),
                ViewboxUnits = BrushMappingMode.Absolute,
                Drawing = new GeometryDrawing
                {
                    Pen = new Pen(GraphSelected, 0.5),
                    Geometry = Geometry.Parse("M0,0 L10,10 M5,-5 L15,5 M-5,5 L5,15")
                }
            };

        }

        public static Brush GraphBackground { get; private set; }
        public static Brush GraphBorder { get; private set; }
        public static Brush GraphHighlight { get; private set; }
        public static Brush GraphSelected { get; private set; }
        public static Brush GraphSelectedHighlight { get; private set; }
        public static Brush GraphTint { get; private set; }

        public static Brush GraphPattern { get; private set; }

        public static Pen GraphBorderPen { get; private set; }
        public static Pen GraphConstraintPen { get; private set; }
        public static Pen GraphSelectedPen { get; private set; }
        public static Pen GraphGridPen { get; private set; }
    }
}

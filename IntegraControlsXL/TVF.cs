using IntegraControlsXL.Common;
using System.Windows;
using System.Windows.Media;
using IntegraXL.Core;

namespace IntegraControlsXL
{
    public class TVF : GraphControl
    {
        public TVF() : base(1, 4, 4) 
        {
            ShowGrid = true;
        }

        public static readonly DependencyProperty FilterTypeProperty = DependencyProperty.Register(nameof(FilterType), typeof(IntegraTVFFilterType), typeof(TVF), new FrameworkPropertyMetadata(IntegraTVFFilterType.LPF, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update));
        public static readonly DependencyProperty CutoffProperty     = DependencyProperty.Register(nameof(Cutoff), typeof(double), typeof(TVF), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty ResonanceProperty  = DependencyProperty.Register(nameof(Resonance), typeof(double), typeof(TVF), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));

        public IntegraTVFFilterType FilterType
        {
            get { return (IntegraTVFFilterType)GetValue(FilterTypeProperty); }
            set { SetValue(FilterTypeProperty, value); }
        }

        public double Resonance
        {
            get { return (double)GetValue(ResonanceProperty); }
            set { SetValue(ResonanceProperty, value); }
        }


        public double Cutoff
        {
            get { return (double)GetValue(CutoffProperty); }
            set { SetValue(CutoffProperty, value); }
        }


        public override void Update()
        {
            switch(FilterType)
            {
                case IntegraTVFFilterType.OFF:
                    CP[0] = new ControlPointX(this, CutoffProperty, 0, 127, new LimitX(0, Segments.CY, Segments.SX));
                    CP[0].IsVisble = false;
                    break;
                case IntegraTVFFilterType.LPF:
                    CP[0] = new ControlPoint(this, CutoffProperty, ResonanceProperty, 0, 127, 0, 127, new Limit(0, Segments.SX, Segments.CY, SegmentSize.Y));
                    break;
                case IntegraTVFFilterType.BPF:
                    CP[0] = new ControlPoint(this, CutoffProperty, ResonanceProperty, 0, 127, 0, 127, new Limit(0, Segments.SX, Segments.CY, SegmentSize.Y));
                    break;
                case IntegraTVFFilterType.HPF:
                    CP[0] = new ControlPoint(this, CutoffProperty, ResonanceProperty, 0, 127, 0, 127, new Limit(0, Segments.SX, Segments.CY, SegmentSize.Y));
                    break;
                case IntegraTVFFilterType.PKG:
                    CP[0] = new ControlPoint(this, CutoffProperty, ResonanceProperty, 0, 127, 0, 127, new Limit(0, Segments.SX, Segments.CY, SegmentSize.Y));
                    break;
                case IntegraTVFFilterType.LPF2:
                    CP[0] = new ControlPointX(this, CutoffProperty, 0, 127, new LimitX(0, Segments.CY, Segments.SX));
                    break;
                case IntegraTVFFilterType.LPF3:
                    CP[0] = new ControlPointX(this, CutoffProperty, 0, 127, new LimitX(0, Segments.CY, Segments.SX));
                    break;
            }


            base.Update();
        }


        private StreamGeometry GetHPFGeometry()
        {
            StreamGeometry stream = new StreamGeometry();

            var cp = CP[0];

            var rSize = cp.Limit.MaxY - cp.Limit.MinY;
            var rFactor = rSize / cp.MaxY;
            var rCurrent = rFactor * cp.ValueY;

            using (StreamGeometryContext ctx = stream.Open())
            {
                ctx.BeginFigure(new Point(cp.X - (rSize * 1.5 - rCurrent) * 2.5, 0), false, false);

                ctx.BezierTo(new Point(cp.X - (rSize * 1.5 - rCurrent) * 2, SegmentSize.Y), 
                             new Point(cp.X - rSize * 1.5 + rCurrent, cp.Y),
                             cp, true, true);

                ctx.BezierTo(new Point(cp.X + rSize * 1.5 - rCurrent, cp.Y), 
                             new Point(cp.X + rSize * 1.5 - rCurrent, Segments.CY),
                             new Point(cp.X + rSize * 1.5 + rCurrent, Segments.CY), true, true);

                ctx.LineTo(new Point(Segments.SX, Segments.CY), true, true);
            }

            return stream;
        }

        private StreamGeometry GetLPFGeometry()
        {
            StreamGeometry stream = new StreamGeometry();

            var cp = CP[0];

            var rSize = cp.Limit.MaxY - cp.Limit.MinY;
            var rFactor = rSize / cp.MaxY;
            var rCurrent = rFactor * cp.ValueY;

            using (StreamGeometryContext ctx = stream.Open())
            {
                ctx.BeginFigure(new Point(0, Segments.CY), false, false);
                ctx.LineTo(new Point(cp.X - rSize * 1.5 - rCurrent, Segments.CY), true, true);

                ctx.BezierTo(new Point(cp.X - rSize * 1.5 + rCurrent, Segments.CY),
                             new Point(cp.X - rSize * 1.5 + rCurrent, cp.Y),
                             cp, true, true);

                ctx.BezierTo(new Point(cp.X + rSize * 1.5 - rCurrent, cp.Y),
                             new Point(cp.X + (rSize * 1.5 - rCurrent) * 2, SegmentSize.Y),
                             new Point(cp.X + (rSize * 1.5 - rCurrent) * 2.5, 0), true, true);
            }

            return stream;
        }

        private StreamGeometry GetBPFGeometry()
        {
            StreamGeometry stream = new StreamGeometry();

            var cp = CP[0];

            var rSize = cp.Limit.MaxY - cp.Limit.MinY;
            var rFactor = rSize / cp.MaxY;
            var rCurrent = rFactor * cp.ValueY;

            using (StreamGeometryContext ctx = stream.Open())
            {

                ctx.BeginFigure(new Point(cp.X - (rSize * 1.5 - rCurrent) * 2.5, 0), false, false);

                ctx.BezierTo(new Point(cp.X - (rSize * 1.5 - rCurrent) * 2, SegmentSize.Y),
                             new Point(cp.X -  rSize * 1.5 + rCurrent, cp.Y),
                             cp, true, true);

                ctx.BezierTo(new Point(cp.X +  rSize * 1.5 - rCurrent, cp.Y),
                             new Point(cp.X + (rSize * 1.5 - rCurrent) * 2, SegmentSize.Y),
                             new Point(cp.X + (rSize * 1.5 - rCurrent) * 2.5, 0), true, true);
            }

            return stream;
        }

        private StreamGeometry GetPKGGeometry()
        {
            StreamGeometry stream = new StreamGeometry();

            var cp = CP[0];

            var rSize = cp.Limit.MaxY - cp.Limit.MinY;
            var rFactor = rSize / cp.MaxY;
            var rCurrent = rFactor * cp.ValueY;

            using (StreamGeometryContext ctx = stream.Open())
            {
                ctx.BeginFigure(new Point(0, Segments.CY), false, false);

                ctx.LineTo(new Point(cp.X - rSize * 1.5 - rCurrent, Segments.CY), true, true);

                ctx.BezierTo(new Point(cp.X - rSize * 1.5 + rCurrent, Segments.CY),
                             new Point(cp.X - rSize * 1.5 + rCurrent, cp.Y),
                             cp, true, true);

                ctx.BezierTo(new Point(cp.X + rSize * 1.5 - rCurrent, cp.Y),
                             new Point(cp.X + rSize * 1.5 - rCurrent, Segments.CY),
                             new Point(cp.X + rSize * 1.5 + rCurrent, Segments.CY), true, true);

                ctx.LineTo(new Point(Segments.SX, Segments.CY), true, true);
            }

            return stream;
        }

        private StreamGeometry GetLPF2Geometry()
        {
            StreamGeometry stream = new StreamGeometry();

            var cp = CP[0];

            //var rSize = cp.Limit.MaxY - cp.Limit.MinY;
            //var rFactor = rSize / cp.MaxY;
            //var rCurrent = rFactor * cp.ValueY;

            using (StreamGeometryContext ctx = stream.Open())
            {
                ctx.BeginFigure(new Point(0, Segments.CY), false, false);
                ctx.LineTo(cp, true, true);

                ctx.BeginFigure(cp, true, true);
                ctx.BezierTo(new Point(cp.X + SegmentSize.X, Segments.CY),
                             new Point(cp.X + SegmentSize.X * 1.5, Segments.CY),
                             new Point(cp.X + SegmentSize.X * 3, 0), true, true);

                ctx.LineTo(new Point(cp.X + SegmentSize.X * 2 - SegmentSize.Y, 0), false, true);

                ctx.BezierTo(new Point(cp.X + SegmentSize.X * 2 - SegmentSize.Y * 1.5, SegmentSize.Y),
                             new Point(cp.X + SegmentSize.X * 0.5, Segments.CY),
                             cp, true, true);
            }

            return stream;
        }

        private StreamGeometry GetLPF3Geometry()
        {
            StreamGeometry stream = new StreamGeometry();

            var cp = CP[0];

            var rSize = cp.Limit.MaxY - cp.Limit.MinY;
            var rFactor = rSize / cp.MaxY;
            var rCurrent = rFactor * cp.ValueY;

            using (StreamGeometryContext ctx = stream.Open())
            {
                ctx.BeginFigure(new Point(0, Segments.CY), false, false);
                ctx.LineTo(cp, true, true);

                ctx.BezierTo(new Point(cp.X + SegmentSize.X * 1.5, cp.Y),
                             new Point(cp.X + SegmentSize.X * 1.5, cp.Y),
                             new Point(cp.X + SegmentSize.X * 2, 0), true, true);
            }

            return stream;
        }
        public override void RenderGraph(DrawingContext dc)
        {
            switch (FilterType)
            {
                case IntegraTVFFilterType.OFF:
                    dc.DrawLine(Styles.GraphSelectedPen, new Point(0, Segments.CY), new Point(Segments.SX, Segments.CY));
                    dc.DrawLine(Styles.GraphConstraintPen, new Point(CP[0].X, CP[0].Limit.MinY), new Point(CP[0].X, 0));
                    break;
                case IntegraTVFFilterType.LPF:
                    dc.DrawGeometry(null, Styles.GraphSelectedPen, GetLPFGeometry());
                    dc.DrawLine(Styles.GraphConstraintPen, CP[0], new Point(CP[0].X, 0));
                    break;
                case IntegraTVFFilterType.BPF:
                    dc.DrawGeometry(null, Styles.GraphSelectedPen, GetBPFGeometry());
                    dc.DrawLine(Styles.GraphConstraintPen, CP[0], new Point(CP[0].X, 0));
                    break;
                case IntegraTVFFilterType.HPF:
                    dc.DrawGeometry(null, Styles.GraphSelectedPen, GetHPFGeometry());
                    dc.DrawLine(Styles.GraphConstraintPen, CP[0], new Point(CP[0].X, 0));
                    break;
                case IntegraTVFFilterType.PKG:
                    dc.DrawGeometry(null, Styles.GraphSelectedPen, GetPKGGeometry());
                    dc.DrawLine(Styles.GraphConstraintPen, CP[0], new Point(CP[0].X, 0));
                    break;
                case IntegraTVFFilterType.LPF2:
                    dc.DrawGeometry(Styles.GraphPattern, Styles.GraphSelectedPen, GetLPF2Geometry());
                    break;
                case IntegraTVFFilterType.LPF3:
                    dc.DrawGeometry(Styles.GraphPattern, Styles.GraphSelectedPen, GetLPF3Geometry());
                    break;
            }
        }
    }
}

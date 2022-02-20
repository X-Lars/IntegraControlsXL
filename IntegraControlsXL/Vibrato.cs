using IntegraControlsXL.Common;
using System;
using System.Windows;
using System.Windows.Media;

namespace IntegraControlsXL
{

    public class Vibrato : GraphControl
    {
        public Vibrato() : base(2, 4, 2) { }

        private static DependencyProperty RateProperty  = DependencyProperty.Register(nameof(Rate),  typeof(double), typeof(Vibrato), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        private static DependencyProperty DepthProperty = DependencyProperty.Register(nameof(Depth), typeof(double), typeof(Vibrato), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        private static DependencyProperty DelayProperty = DependencyProperty.Register(nameof(Delay), typeof(double), typeof(Vibrato), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));

        public double Rate
        {
            get { return (double)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }
        public double Depth
        {
            get => (double)GetValue(DepthProperty);
            set => SetValue(DepthProperty, value);
        }
        public double Delay
        {
            get => (double)GetValue(DelayProperty);
            set => SetValue(DelayProperty, value);
        }

        private double _WaveSX => SegmentSize.X / 4;
        private double _WaveX => Segments.CX - _WaveSX;

        public override void Update()
        {
            CP[0] = new ControlPointX(this, DelayProperty, -64, 63, new LimitX(0, Segments.CY, _WaveX), 64);
            CP[1] = new ControlPoint(this, RateProperty, DepthProperty, -64, 63, -64, 63, new Limit(Segments.CX, SegmentSize.X * 2, Segments.CY, SegmentSize.Y), 64, 64);
            //Geo = GetWave();
            base.Update();
        }
        public StreamGeometry Geo;
        public StreamGeometry GetWave()
        {
            var rate  = CP[1].X - _WaveX;
            var depth = CP[1].Y - Segments.CY;

            var length = Math.Abs(CP[1].X - _WaveX);
            int waveCount = (int)((Segments.SX - _WaveX) / length) + 1;
            int fadeCount = (int)(_WaveX / rate) + 1;
            double df = depth * (depth / 40);

            StreamGeometry stream = new StreamGeometry();
            
            using(StreamGeometryContext ctx = stream.Open())
            {
                var waveCX = (CP[1].X - _WaveX) / 2 + _WaveX;

                ctx.BeginFigure(new Point(_WaveX, Segments.CY), false, false);

                for (int i = 0; i < waveCount; i++)
                {
                    ctx.BezierTo(new Point(_WaveX + rate / 2 + rate * i, Segments.CY + df), new Point(_WaveX + rate / 2 + rate * i, Segments.CY - df), new Point(CP[1].X + length * i, Segments.CY), true, true);
                }
                
                ctx.BeginFigure(new Point(_WaveX, Segments.CY), false, false);

                for (int i = 0; i < fadeCount; i++)
                {
                    if (_WaveX -  (rate * i) < CP[0].X)
                    {
                        double c = fadeCount - i; // 3 2 1 0
                        var v = Math.Abs(i -fadeCount );
                        double fade = (c / fadeCount) * df;
                        ctx.BezierTo(new Point(_WaveX - rate / 2 - rate * i, Segments.CY - fade), new Point(_WaveX - rate / 2 - rate * i, Segments.CY + fade), new Point(_WaveX - rate - length * i, Segments.CY), true, true);
                    }
                    else
                    {
                        ctx.BezierTo(new Point(_WaveX - rate / 2 - rate * i, Segments.CY - df), new Point(_WaveX - rate / 2 - rate * i, Segments.CY + df), new Point(_WaveX - rate - length * i, Segments.CY), true, true);
                    }
                }
            }

            return stream;
        }

        public override void Render(DrawingContext dc)
        {
            //if (!IsLoaded)
            //    return;
            dc.DrawLine(Styles.GraphSelectedPen, new Point(0, Segments.CY), CP[0]);


            //StreamGeometry stream = new StreamGeometry();
            //using (StreamGeometryContext ctx = stream.Open())
            //{
            //    var wcx = (CP[1].X - _WaveX) / 2 + _WaveX;
            //    ctx.BeginFigure(new Point(0, Segments.CY), false, false);
            //    ctx.BezierTo(new Point(SegmentSize.X / 2, Segments.CY), new Point(SegmentSize.X / 2, 0), new Point(SegmentSize.X, Segments.CY), false, true);
            //}
            //stream.Freeze();
            //dc.DrawGeometry(Styles.GraphSelected, new Pen(Styles.GraphSelected, 1), stream);

            //dc.DrawRectangle(Styles.GraphTint, null, stream.Bounds);
            dc.DrawGeometry(null, Styles.GraphSelectedPen, GetWave());
        }
    }
}

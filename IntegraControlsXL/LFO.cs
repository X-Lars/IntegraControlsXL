using IntegraControlsXL.Common;
using IntegraXL.Core;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace IntegraControlsXL
{
    public class LFO : GraphControl
    {
        public LFO() : base(4, 4, 4)
        {
            ShowGrid = true;
        }

        public static readonly DependencyProperty WaveformProperty = DependencyProperty.Register(nameof(Waveform), typeof(IntegraLFOWaveform), typeof(LFO), new FrameworkPropertyMetadata(IntegraLFOWaveform.SIN, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update));
        public static readonly DependencyProperty FadeModeProperty = DependencyProperty.Register(nameof(FadeMode), typeof(IntegraLFOFadeMode), typeof(LFO), new FrameworkPropertyMetadata(IntegraLFOFadeMode.ONIN, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update));
        public static readonly DependencyProperty DelayTimeProperty = DependencyProperty.Register(nameof(DelayTime), typeof(double), typeof(LFO), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty RateProperty = DependencyProperty.Register(nameof(Rate), typeof(double), typeof(LFO), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(nameof(Offset), typeof(int), typeof(LFO), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update));
        public static readonly DependencyProperty FadeTimeProperty = DependencyProperty.Register(nameof(FadeTime), typeof(double), typeof(LFO), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));



        public double FadeTime
        {
            get { return (double)GetValue(FadeTimeProperty); }
            set { SetValue(FadeTimeProperty, value); }
        }

        public IntegraLFOWaveform Waveform
        {
            get { return (IntegraLFOWaveform)GetValue(WaveformProperty); }
            set { SetValue(WaveformProperty, value); }
        }

        public IntegraLFOFadeMode FadeMode
        {
            get { return (IntegraLFOFadeMode)GetValue(FadeModeProperty); }
            set { SetValue(FadeModeProperty, value); }
        }

        public double DelayTime
        {
            get { return (double)GetValue(DelayTimeProperty); }
            set { SetValue(DelayTimeProperty, value); }
        }

        public double Rate
        {
            get { return (double)GetValue(RateProperty); }
            set { SetValue(RateProperty, value); }
        }

        public int Offset
        {
            get { return (int)GetValue(OffsetProperty); }
            set { SetValue(OffsetProperty, value); }
        }

        public enum WaveDirection : int
        {
            Up = -1,
            Down = 1,
        }
        private bool _Invert => FadeMode == IntegraLFOFadeMode.ONOUT || FadeMode == IntegraLFOFadeMode.OFFOUT;

        private double _WaveSizeMin => SegmentSize.X / 8;
        private double _WaveSizeMax => SegmentSize.X;


        private double WaveSX = 0;
        private double _WaveSegmentSize => WaveSX / 2;
        private double _WaveCount = 0;
        private double _FadeCount = 0;
        private int _OffsetY => (int)((Offset / 50) * (SegmentSize.Y / 2));
        private double WaveSegmentSX { get; set; }

        /// <summary>
        /// CP[1]
        /// </summary>
        private double RateX
        {
            get
            {
                if (Rate < 128)
                    return CP[1].X - _WaveSizeMin;
                else
                {
                    double offset = Rate - 128; // Value offset into the notes enumeration
                    double factor = CP[1].Limit.SX / 21;

                    return CP[1].X - _WaveSizeMin - (factor * offset);
                }
            }
        }

        private double _FadeFactorX { get; set; }

        public override void Update()
        {
            CP[0] = new ControlPointX(this, DelayTimeProperty, 0, 127, new LimitX(0, Segments.CY + _OffsetY, SegmentSize.X * 2));
            CP[1] = new RateControlPoint(this, RateProperty,   0, 149, new LimitX(CP[0].X + _WaveSizeMin, Segments.CY + _OffsetY, SegmentSize.X - _WaveSizeMin));
            CP[2] = new ControlPointX(this, FadeTimeProperty,  0, 127, new LimitX(CP[0].X, SegmentSize.Y*3 + _OffsetY, SegmentSize.X * 2));
            CP[3] = new ControlPointX(this, FadeTimeProperty,  0, 127, new LimitX(CP[0].X, SegmentSize.Y + _OffsetY, SegmentSize.X * 2));
            
            
            WaveSX = _WaveSizeMax - (RateX - CP[0].X);

            _WaveCount = Math.Ceiling(((Segments.SX - CP[0].X) / WaveSX));
            _FadeCount = (CP[2].X - CP[0].X) / WaveSX;

            _FadeFactorX = (CP[2].X - CP[0].X) / CP[2].Limit.SX;

            switch (Waveform)
            {
                case IntegraLFOWaveform.SIN:
                    WaveSegmentSX = WaveSX / 4;
                    break;
                case IntegraLFOWaveform.TRI:
                    WaveSegmentSX = WaveSX / 4;
                    break;
                case IntegraLFOWaveform.SAWUP:
                    break;
                case IntegraLFOWaveform.SAWDOWN:
                    break;
                case IntegraLFOWaveform.SQR:
                    WaveSegmentSX = WaveSX / 2;
                    break;
                case IntegraLFOWaveform.RND:
                    CP[1].IsVisble = false;
                    break;
                case IntegraLFOWaveform.BENDUP:
                    WaveSegmentSX = WaveSX;
                    break;
                case IntegraLFOWaveform.BENDDOWN:
                    break;
                case IntegraLFOWaveform.TRP:
                    break;
                case IntegraLFOWaveform.SH:
                    break;
                case IntegraLFOWaveform.CHS:
                    break;
                case IntegraLFOWaveform.VSIN:
                    break;
                case IntegraLFOWaveform.STEP:
                    break;

            }

            base.Update();
        }

        /// <summary>
        /// Gets the offset from the wave's center Y to the wave's height.
        /// </summary>
        /// <param name="x">The X location to get the Y location for.</param>
        /// <returns>The height of the wave from the center at the specified X location.</returns>
        private double GetWaveOffsetY(double x)
        {
            return (x < CP[2].X) ? SegmentSize.Y * (x - CP[0].X) / (CP[2].X - CP[0].X) : SegmentSize.Y;
        }

        #region Methods: Geometry

        public StreamGeometry GetSINGeometry()
        {
            StreamGeometry stream = new StreamGeometry();

            using (StreamGeometryContext ctx = stream.Open())
            {
                Point cp = CP[0];
                Point h0 = cp;

                ctx.BeginFigure(cp, false, false);

                // WAVE LOOP
                for (int i = 0; i <= _WaveCount; i++)
                {
                    // WAVE OFFSET
                    Point wave = new(cp.X + (i * WaveSX), cp.Y);

                    // WAVE UPPER
                    Point p1 = new Point(wave.X + WaveSegmentSX, cp.Y);
                    p1.Offset(0, GetWaveOffsetY(p1.X));

                    // WAVE LOWER
                    Point p2 = new Point(p1.X + WaveSegmentSX * 2, cp.Y);
                    p2.Offset(0, -GetWaveOffsetY(p2.X));

                    Point h1 = new (p1.X - WaveSegmentSX, p1.Y); // P1 OUT
                    Point h2 = new (p1.X + WaveSegmentSX, p1.Y); // P2 IN
                    Point h3 = new (p2.X - WaveSegmentSX, p2.Y); // P2 OUT

                    ctx.BezierTo(h0, h1, p1, true, true);
                    ctx.BezierTo(h2, h3, p2, true, true);

                    h0 = new Point(p2.X + WaveSegmentSX, p2.Y); // P1 IN
                }
            }

            if(FadeMode == IntegraLFOFadeMode.ONOUT || FadeMode == IntegraLFOFadeMode.OFFOUT)
                stream.Transform = new ScaleTransform(-1, 1, CP[0].X, CP[0].Y);

            return stream;
        }

        public StreamGeometry GetTRIGeometry()
        {
            StreamGeometry stream = new StreamGeometry();
            
            using (StreamGeometryContext ctx = stream.Open())
            {
                Point cp = CP[0];

                ctx.BeginFigure(cp, false, false);
                
                // WAVE LOOP
                for (int i = 0; i <= _WaveCount; i++)
                {
                    // WAVE OFFSET
                    Point wave = new Point(cp.X + (i * WaveSX), cp.Y);
                    
                    // WAVE UPPER
                    Point p1 = new Point(wave.X + WaveSegmentSX, cp.Y);
                    p1.Offset(0, GetWaveOffsetY(p1.X));

                    // WAVE LOWER
                    Point p2 = new Point(p1.X + WaveSegmentSX * 2, cp.Y);
                    p2.Offset(0, -GetWaveOffsetY(p2.X));

                    ctx.LineTo(p1, true, true);
                    ctx.LineTo(p2, true, true);
                }

            }

            if (FadeMode == IntegraLFOFadeMode.ONOUT || FadeMode == IntegraLFOFadeMode.OFFOUT)
                stream.Transform = new ScaleTransform(-1, 1, CP[0].X, CP[0].Y);

            return stream;
        }

        public StreamGeometry GetSAWGeometry(WaveDirection direction)
        {
            StreamGeometry stream = new StreamGeometry();
            
            using (StreamGeometryContext ctx = stream.Open())
            {
                Point cp = CP[0];

                ctx.BeginFigure(cp, false, false);

                for (int i = 0; i <= _WaveCount; i++)
                {
                    // WAVE OFFSET
                    Point wave = new Point(cp.X + (i * WaveSX), cp.Y);

                    // WAVE UPPER
                    Point p1 = new Point(wave.X, cp.Y);
                    p1.Offset(0, -GetWaveOffsetY(p1.X));

                    // WAVE LOWER
                    Point p2 = new Point(p1.X + WaveSX, cp.Y);
                    p2.Offset(0, GetWaveOffsetY(p2.X));

                    ctx.LineTo(p1, true, true);
                    ctx.LineTo(p2, true, true);
                }

            }

            TransformGroup transform = new ();

            if(direction == WaveDirection.Down)
            {
                transform.Children.Add(new ScaleTransform(1, -1, CP[0].X, CP[0].Y));
            }
            
            if (FadeMode == IntegraLFOFadeMode.ONOUT || FadeMode == IntegraLFOFadeMode.OFFOUT)
            {
                transform.Children.Add(new ScaleTransform(-1, -1, CP[0].X, CP[0].Y));
            }

            stream.Transform = transform;

            return stream;
        }

        public StreamGeometry GetSQRGeometry()
        {
            StreamGeometry stream = new ();
            
            using (StreamGeometryContext ctx = stream.Open())
            {
                Point cp = CP[0];

                ctx.BeginFigure(cp, false, false);
               
                // WAVE LOOP
                for (int i = 0; i <= _WaveCount; i++)
                {
                    // WAVE OFFSET
                    Point wave = new (cp.X + (i * WaveSX), cp.Y);

                    // WAVE UP
                    Point p1 = new (wave.X, wave.Y);
                    p1.Offset(0, GetWaveOffsetY(p1.X));

                    // WAVE UP OFFSET
                    Point p2 = new (p1.X + WaveSegmentSX, p1.Y);

                    // WAVE DOWN
                    Point p3 = new (p2.X, wave.Y);
                    p3.Offset(0, -GetWaveOffsetY(p3.X));

                    // WAVE DOWN OFFSET
                    Point p4 = new (p3.X + WaveSegmentSX, p3.Y);

                    ctx.LineTo(p1, true, true);
                    ctx.LineTo(p2, true, true);
                    ctx.LineTo(p3, true, true);
                    ctx.LineTo(p4, true, true);
                }

            }

            if (FadeMode == IntegraLFOFadeMode.ONOUT || FadeMode == IntegraLFOFadeMode.OFFOUT)
            {
                stream.Transform = new ScaleTransform(-1, -1, CP[0].X, CP[0].Y);
            }

            return stream;
        }

        // TODO
        public StreamGeometry GetRNDGeometry()
        {
            Random random = new Random();
            StreamGeometry stream = new StreamGeometry();
            var cp = CP[0];
            using (StreamGeometryContext ctx = stream.Open())
            {
                ctx.BeginFigure(cp, false, false);

                for (int i = 0; i <= 4; i++)
                {
                    var rndPosY = random.NextDouble() * SegmentSize.Y * 1.5;
                    var rndNegY = random.NextDouble() * SegmentSize.Y * 1.5;
                    var rndPosX = random.NextDouble() * (SegmentSize.X / 2);

                    var f1 = (_FadeCount - i) > 0 ? (1 - (_FadeCount - i) / _FadeCount) : 1;
                    var f2 = (_FadeCount - i) > 0 ? f1 + (((1 - (_FadeCount - (i + 1)) / _FadeCount) - f1) / 2) : 1;

                    var p1 = new Point(cp.X + (SegmentSize.X * i) + rndPosX, Segments.CY);
                    var p2 = new Point(cp.X + (SegmentSize.X * i) + SegmentSize.X / 2 + rndPosX, Segments.CY);

                    var handleUp = new Point((cp.X + (SegmentSize.X * i)) + (p1.X - (cp.X + (SegmentSize.X * i))) / 2, p1.Y + (p1.Y - Segments.CY) / 2 * f1);
                    var handleMid1 = new Point(p1.X, Segments.CY + rndPosY * f1);
                    var handleMid2 = new Point(p1.X, Segments.CY - rndNegY * f2);
                    var handleDown = new Point(p2.X - ((p2.X - p1.X) / 2), p2.Y - (Segments.CY - p2.Y) / 2 * f2);

                    ctx.BezierTo(handleUp, handleMid1, p1, true, true);
                    ctx.BezierTo(handleMid2, handleDown, p2, true, true);
                }

            }

            if (_Invert)
                stream.Transform = new ScaleTransform(-1, 1, cp.X, cp.Y);

            return stream;
        }

        public StreamGeometry GetBENDGeometry(WaveDirection direction)
        {
            StreamGeometry stream = new StreamGeometry();
            var cp = CP[0];
            using (StreamGeometryContext ctx = stream.Open())
            {

                var fadeFactor =  ((CP[2].X - CP[0].X) / CP[2].Limit.MaxX);

                var rateFactor = ((Math.Floor(RateX) - CP[0].X + _WaveSizeMin) / (SegmentSize.X - _WaveSizeMin));


                var rateX = CP[1].ValueX * rateFactor;
                var delayOffset = ((SegmentSize.Y / 2) / (SegmentSize.X * 2) * (cp.Limit.MaxX - cp.X));
                Debug.Print($"{rateFactor}");
                ctx.BeginFigure(CP[3], true, true);


                Point p1 = new(CP[2].X, cp.Y - (SegmentSize.Y) + (SegmentSize.Y / 1.25 * fadeFactor));
                Point p2 = new(CP[2].X + ((WaveSX -_WaveSizeMin) * 4) - ((CP[2].X - CP[0].X) / 4) + _WaveSizeMin, CP[0].Y);

                Point h1 = new(p1.X + SegmentSize.X / 10, p1.Y);
                //Point h2 = new(CP[2].X + Segments.CX - ((SegmentSize.X * rateFactor) * 2) + _WaveSizeMin, cp.Y);
                Point h2 = new(CP[2].X , cp.Y);

                ctx.LineTo(p1, true, true);
                //ctx.LineTo(p2, true, true);
                ctx.BezierTo(p1, h2, p2, true, true);
                ctx.LineTo(new Point(cp.X + Segments.SX, cp.Y), true, true);
                ctx.LineTo(cp, true, true);


                //ctx.BezierTo(cp,
                //             new Point(cp.X, cp.Y - delayFactor),
                //             new Point(cp.X, cp.Y - delayFactor), true, true);

                //ctx.BezierTo(new Point(cp.X, cp.Y - delayFactor),
                //             new Point(cp.X + Segments.SX - rateX - SegmentSize.X, cp.Y),
                //             new Point(cp.X + Segments.SX - rateX , cp.Y), true, true);
                //ctx.LineTo(new Point(cp.X + Segments.SX, cp.Y), true, true);
                //ctx.BezierTo(new Point(cp.X , cp.Y - SegmentSize.Y),
                //             new Point(CP[1].X, cp.Y),
                //             new Point(CP[1].X - cp.X, cp.Y), true, true);

                //var f1 = (_FadedWaves - i) > 0 ? (1 - (_FadedWaves - i) / _FadedWaves) : 1;
                //var f2 = (_FadedWaves - i) > 0 ? f1 + (((1 - (_FadedWaves - (i + 1)) / _FadedWaves) - f1) / 2) : 1;

                //var handleUp = new Point(cp.X + _WaveSegSX + (_WaveSX * 2 * i), cp.Y + Segments.CY * f1);
                //var p1 = new Point(cp.X + _WaveSX + (_WaveSX * 2 * i), cp.Y);
                //var handleDown = new Point(p1.X + _WaveSegSX, cp.Y - Segments.CY * f2);
                //var p2 = new Point(p1.X + _WaveSX, cp.Y);

                //ctx.BezierTo(handleUp, p1, p1, true, true);
                //ctx.BezierTo(p1, handleDown, p2, true, true);


            }
            if (_Invert)
                stream.Transform = new ScaleTransform(-1, 1, cp.X, cp.Y);

            return stream;
        }

        public StreamGeometry GetFadeGeometry()
        {
            StreamGeometry stream = new StreamGeometry();
            var cp = CP[0];
            using (StreamGeometryContext ctx = stream.Open())
            {

                ctx.BeginFigure(cp, true, true);
                ctx.LineTo(CP[2], true, true);
                ctx.LineTo(CP[3], true, true);
            }

            TransformGroup transform = new TransformGroup();

            if (_Invert)
            {
                transform.Children.Add(new ScaleTransform(-1, -1, cp.X, cp.Y));
                //stream.Transform = new ScaleTransform(-1, 1, cp.X, cp.Y);
            }

            stream.Transform = transform;
            return stream;
        }

        #endregion

        #region Overrides: Graph Control

        public override void RenderBackground(DrawingContext dc)
        {
            // FADE INDCATOR

            dc.DrawGeometry(Styles.GraphPattern, Styles.GraphConstraintPen, GetFadeGeometry());
            dc.DrawLine(Styles.GraphConstraintPen, new Point(CP[2].X, 0), new Point(CP[2].X, Segments.SY));
        }

        public override void RenderGraph(DrawingContext dc)
        {
            // DELAY TIME INDICATOR
            dc.DrawLine(Styles.GraphConstraintPen, new Point(CP[0].X, 0), new Point(CP[0].X, Segments.SY));

            

            switch (Waveform)
            {
                case IntegraLFOWaveform.SIN:
                    dc.DrawGeometry(null, Styles.GraphSelectedPen, GetSINGeometry());
                    break;
                case IntegraLFOWaveform.TRI:
                    dc.DrawGeometry(null, Styles.GraphSelectedPen, GetTRIGeometry());
                    break;
                case IntegraLFOWaveform.SAWUP:
                    dc.DrawGeometry(null, Styles.GraphSelectedPen, GetSAWGeometry(WaveDirection.Up));
                    break;
                case IntegraLFOWaveform.SAWDOWN:
                    dc.DrawGeometry(null, Styles.GraphSelectedPen, GetSAWGeometry(WaveDirection.Down));
                    break;
                case IntegraLFOWaveform.SQR:
                    dc.DrawGeometry(null, Styles.GraphSelectedPen, GetSQRGeometry());
                    break;
                case IntegraLFOWaveform.RND:
                    dc.DrawGeometry(null, Styles.GraphSelectedPen, GetRNDGeometry());
                    break;
                case IntegraLFOWaveform.BENDUP:
                    dc.DrawGeometry(Styles.GraphSelectedHighlight, Styles.GraphSelectedPen, GetBENDGeometry(WaveDirection.Up));
                    break;
                case IntegraLFOWaveform.BENDDOWN:
                    dc.DrawGeometry(null, Styles.GraphSelectedPen, GetBENDGeometry(WaveDirection.Down));
                    break;
                case IntegraLFOWaveform.TRP:
                    break;
                case IntegraLFOWaveform.SH:
                    break;
                case IntegraLFOWaveform.CHS:
                    break;
                case IntegraLFOWaveform.VSIN:
                    break;
                case IntegraLFOWaveform.STEP:
                    break;

            }

        }

        #endregion
    }
}

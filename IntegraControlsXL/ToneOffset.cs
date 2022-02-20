using IntegraControlsXL.Common;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace IntegraControlsXL
{
    public class ToneOffset : GraphControl
    {
        public ToneOffset() : base(3, 4, 2) { }

        public override void Update()
        {
            CP[0] = new ControlPointX(this, AttackProperty,  -64, 63, new LimitX(0, Segments.SY, SegmentSize.X), 64);
            CP[1] = new ControlPointX(this, DecayProperty,   -64, 63, new LimitX(CP[0].X, Segments.CY, SegmentSize.X), 64);
            CP[2] = new ControlPointX(this, ReleaseProperty, -64, 63, new LimitX(CP[1].X + SegmentSize.X, 0, SegmentSize.X), 64);

            base.Update();
        }

        public static readonly DependencyProperty AttackProperty  = DependencyProperty.Register(nameof(Attack),  typeof(double), typeof(ToneOffset), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty DecayProperty   = DependencyProperty.Register(nameof(Decay),   typeof(double), typeof(ToneOffset), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty ReleaseProperty = DependencyProperty.Register(nameof(Release), typeof(double), typeof(ToneOffset), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));

        public double Attack
        {
            get { return (double)GetValue(AttackProperty); }
            set { SetValue(AttackProperty, value); }
        }

        public double Decay
        {
            get { return (double)GetValue(DecayProperty); }
            set { SetValue(DecayProperty, value); }
        }

        public double Release
        {
            get { return (double)GetValue(ReleaseProperty); }
            set { SetValue(ReleaseProperty, value); }
        }


        public override void Render(DrawingContext dc)
        {
            dc.DrawLine(Styles.GraphSelectedPen, new Point(Segments.X, 0), new Point(CP[0].X, CP[0].Y));
            dc.DrawLine(Styles.GraphSelectedPen, CP[0], CP[1]);
            dc.DrawLine(Styles.GraphBorderPen, CP[1], new Point(CP[1].X + SegmentSize.X, Segments.CY));
            dc.DrawLine(Styles.GraphSelectedPen, new Point(CP[1].X + SegmentSize.X, Segments.CY), CP[2]);
        }

    }
}

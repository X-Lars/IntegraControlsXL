using IntegraControlsXL.Common;
using System;
using System.Windows;
using System.Windows.Media;

namespace IntegraControlsXL
{
    public class MotionalSurround : GraphControl
    {
        public MotionalSurround() : base(1, 2, 2)
        {
            ShowGrid = true;
        }

        public static readonly DependencyProperty LRProperty = DependencyProperty.Register(nameof(LR), typeof(double), typeof(MotionalSurround), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty FBProperty = DependencyProperty.Register(nameof(FB), typeof(double), typeof(MotionalSurround), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));

        public double LR
        {
            get => (double)GetValue(LRProperty);
            set => SetValue(LRProperty, value);
        }

        public double FB
        {
            get => (double)GetValue(FBProperty);
            set => SetValue(FBProperty, value);
        }

        public override void Update()
        {
            CP[0] = new ControlPoint(this, LRProperty, FBProperty, -64, 63, -64, 63, new Limit(0, Segments.SX, 0, Segments.SY), 64, 64);

            base.Update();
        }

        public override void RenderBackground(DrawingContext dc)
        {
            base.RenderBackground(dc);

            var max = Math.Max(SegmentSize.X, SegmentSize.Y);
            var factor = max / 10;

            for (int i = 1; i < 11; i++)
            {
                dc.DrawEllipse(null, Styles.GraphGridPen, new Point(Segments.CX, Segments.CY), factor * i, factor * i);
            }
        }

        public override void RenderGraph(DrawingContext dc)
        {
        }
    }

    public class MotionalSurroundCommon : GraphControl
    {
        public MotionalSurroundCommon() : base(17, 2, 2)
        {
            ShowGrid = true;
        }

        public static readonly DependencyProperty LR1Property = DependencyProperty.Register(nameof(LR1), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty FB1Property = DependencyProperty.Register(nameof(FB1), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty LR2Property = DependencyProperty.Register(nameof(LR2), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty FB2Property = DependencyProperty.Register(nameof(FB2), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty LR3Property = DependencyProperty.Register(nameof(LR3), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty FB3Property = DependencyProperty.Register(nameof(FB3), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty LR4Property = DependencyProperty.Register(nameof(LR4), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty FB4Property = DependencyProperty.Register(nameof(FB4), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty LR5Property = DependencyProperty.Register(nameof(LR5), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty FB5Property = DependencyProperty.Register(nameof(FB5), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty LR6Property = DependencyProperty.Register(nameof(LR6), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty FB6Property = DependencyProperty.Register(nameof(FB6), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty LR7Property = DependencyProperty.Register(nameof(LR7), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty FB7Property = DependencyProperty.Register(nameof(FB7), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty LR8Property = DependencyProperty.Register(nameof(LR8), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty FB8Property = DependencyProperty.Register(nameof(FB8), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty LR9Property = DependencyProperty.Register(nameof(LR9), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty FB9Property = DependencyProperty.Register(nameof(FB9), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty LR10Property = DependencyProperty.Register(nameof(LR10), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty FB10Property = DependencyProperty.Register(nameof(FB10), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty LR11Property = DependencyProperty.Register(nameof(LR11), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty FB11Property = DependencyProperty.Register(nameof(FB11), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty LR12Property = DependencyProperty.Register(nameof(LR12), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty FB12Property = DependencyProperty.Register(nameof(FB12), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty LR13Property = DependencyProperty.Register(nameof(LR13), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty FB13Property = DependencyProperty.Register(nameof(FB13), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty LR14Property = DependencyProperty.Register(nameof(LR14), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty FB14Property = DependencyProperty.Register(nameof(FB14), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty LR15Property = DependencyProperty.Register(nameof(LR15), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty FB15Property = DependencyProperty.Register(nameof(FB15), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty LR16Property = DependencyProperty.Register(nameof(LR16), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty FB16Property = DependencyProperty.Register(nameof(FB16), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty LRExtProperty = DependencyProperty.Register(nameof(LRExt), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty FBExtProperty = DependencyProperty.Register(nameof(FBExt), typeof(double), typeof(MotionalSurroundCommon), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));

        public double LR1 { get => (double)GetValue(LR1Property); set => SetValue(LR1Property, value); }
        public double FB1 { get => (double)GetValue(FB1Property); set => SetValue(FB1Property, value); }
        public double LR2 { get => (double)GetValue(LR2Property); set => SetValue(LR2Property, value); }
        public double FB2 { get => (double)GetValue(FB2Property); set => SetValue(FB2Property, value); }
        public double LR3 { get => (double)GetValue(LR3Property); set => SetValue(LR3Property, value); }
        public double FB3 { get => (double)GetValue(FB3Property); set => SetValue(FB3Property, value); }
        public double LR4 { get => (double)GetValue(LR4Property); set => SetValue(LR4Property, value); }
        public double FB4 { get => (double)GetValue(FB4Property); set => SetValue(FB4Property, value); }
        public double LR5 { get => (double)GetValue(LR5Property); set => SetValue(LR5Property, value); }
        public double FB5 { get => (double)GetValue(FB5Property); set => SetValue(FB5Property, value); }
        public double LR6 { get => (double)GetValue(LR6Property); set => SetValue(LR6Property, value); }
        public double FB6 { get => (double)GetValue(FB6Property); set => SetValue(FB6Property, value); }
        public double LR7 { get => (double)GetValue(LR7Property); set => SetValue(LR7Property, value); }
        public double FB7 { get => (double)GetValue(FB7Property); set => SetValue(FB7Property, value); }
        public double LR8 { get => (double)GetValue(LR8Property); set => SetValue(LR8Property, value); }
        public double FB8 { get => (double)GetValue(FB8Property); set => SetValue(FB8Property, value); }
        public double LR9 { get => (double)GetValue(LR9Property); set => SetValue(LR9Property, value); }
        public double FB9 { get => (double)GetValue(FB9Property); set => SetValue(FB9Property, value); }
        public double LR10 { get => (double)GetValue(LR10Property); set => SetValue(LR10Property, value); }
        public double FB10 { get => (double)GetValue(FB10Property); set => SetValue(FB10Property, value); }
        public double LR11 { get => (double)GetValue(LR11Property); set => SetValue(LR11Property, value); }
        public double FB11 { get => (double)GetValue(FB11Property); set => SetValue(FB11Property, value); }
        public double LR12 { get => (double)GetValue(LR12Property); set => SetValue(LR12Property, value); }
        public double FB12 { get => (double)GetValue(FB12Property); set => SetValue(FB12Property, value); }
        public double LR13 { get => (double)GetValue(LR13Property); set => SetValue(LR13Property, value); }
        public double FB13 { get => (double)GetValue(FB13Property); set => SetValue(FB13Property, value); }
        public double LR14 { get => (double)GetValue(LR14Property); set => SetValue(LR14Property, value); }
        public double FB14 { get => (double)GetValue(FB14Property); set => SetValue(FB14Property, value); }
        public double LR15 { get => (double)GetValue(LR15Property); set => SetValue(LR15Property, value); }
        public double FB15 { get => (double)GetValue(FB15Property); set => SetValue(FB15Property, value); }
        public double LR16 { get => (double)GetValue(LR16Property); set => SetValue(LR16Property, value); }
        public double FB16 { get => (double)GetValue(FB16Property); set => SetValue(FB16Property, value); }
        public double LRExt { get => (double)GetValue(LRExtProperty); set => SetValue(LRExtProperty, value); }
        public double FBExt { get => (double)GetValue(FBExtProperty); set => SetValue(FBExtProperty, value); }

        public override void Update()
        {
            CP[0] = new ControlPoint(this, LR1Property, FB1Property, -64, 63, -64, 63, new Limit(0, Segments.SX, 0, Segments.SY), 64, 64);
            CP[1] = new ControlPoint(this, LR2Property, FB2Property, -64, 63, -64, 63, new Limit(0, Segments.SX, 0, Segments.SY), 64, 64);
            CP[2] = new ControlPoint(this, LR3Property, FB3Property, -64, 63, -64, 63, new Limit(0, Segments.SX, 0, Segments.SY), 64, 64);
            CP[3] = new ControlPoint(this, LR4Property, FB4Property, -64, 63, -64, 63, new Limit(0, Segments.SX, 0, Segments.SY), 64, 64);
            CP[4] = new ControlPoint(this, LR5Property, FB5Property, -64, 63, -64, 63, new Limit(0, Segments.SX, 0, Segments.SY), 64, 64);
            CP[5] = new ControlPoint(this, LR6Property, FB6Property, -64, 63, -64, 63, new Limit(0, Segments.SX, 0, Segments.SY), 64, 64);
            CP[6] = new ControlPoint(this, LR7Property, FB7Property, -64, 63, -64, 63, new Limit(0, Segments.SX, 0, Segments.SY), 64, 64);
            CP[7] = new ControlPoint(this, LR8Property, FB8Property, -64, 63, -64, 63, new Limit(0, Segments.SX, 0, Segments.SY), 64, 64);
            CP[8] = new ControlPoint(this, LR9Property, FB9Property, -64, 63, -64, 63, new Limit(0, Segments.SX, 0, Segments.SY), 64, 64);
            CP[9] = new ControlPoint(this, LR10Property, FB10Property, -64, 63, -64, 63, new Limit(0, Segments.SX, 0, Segments.SY), 64, 64);
            CP[10] = new ControlPoint(this, LR11Property, FB11Property, -64, 63, -64, 63, new Limit(0, Segments.SX, 0, Segments.SY), 64, 64);
            CP[11] = new ControlPoint(this, LR12Property, FB12Property, -64, 63, -64, 63, new Limit(0, Segments.SX, 0, Segments.SY), 64, 64);
            CP[12] = new ControlPoint(this, LR13Property, FB13Property, -64, 63, -64, 63, new Limit(0, Segments.SX, 0, Segments.SY), 64, 64);
            CP[13] = new ControlPoint(this, LR14Property, FB14Property, -64, 63, -64, 63, new Limit(0, Segments.SX, 0, Segments.SY), 64, 64);
            CP[14] = new ControlPoint(this, LR15Property, FB15Property, -64, 63, -64, 63, new Limit(0, Segments.SX, 0, Segments.SY), 64, 64);
            CP[15] = new ControlPoint(this, LR16Property, FB16Property, -64, 63, -64, 63, new Limit(0, Segments.SX, 0, Segments.SY), 64, 64);
            CP[16] = new ControlPoint(this, LRExtProperty, FBExtProperty, -64, 63, -64, 63, new Limit(0, Segments.SX, 0, Segments.SY), 64, 64);

            base.Update();
        }

        public override void RenderBackground(DrawingContext dc)
        {
            base.RenderBackground(dc);

            var max = Math.Max(SegmentSize.X, SegmentSize.Y);
            var factor = max / 10;

            for (int i = 1; i < 11; i++)
            {
                dc.DrawEllipse(null, Styles.GraphGridPen, new Point(Segments.CX, Segments.CY), factor * i, factor * i);
            }
        }

        public override void RenderGraph(DrawingContext dc)
        {
        }
    }
}

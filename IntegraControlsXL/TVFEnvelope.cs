using IntegraControlsXL.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace IntegraControlsXL
{
    public enum HandleStyles
    {
        None,
        Square,
        Circle
    }

    public class TVFEnvelope : GraphControl
    {
        #region Dependency Properties

        private static readonly DependencyProperty Time1Property = DependencyProperty.Register(nameof(Time1), typeof(double), typeof(TVFEnvelope), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        private static readonly DependencyProperty Time2Property = DependencyProperty.Register(nameof(Time2), typeof(double), typeof(TVFEnvelope), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        private static readonly DependencyProperty Time3Property = DependencyProperty.Register(nameof(Time3), typeof(double), typeof(TVFEnvelope), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        private static readonly DependencyProperty Time4Property = DependencyProperty.Register(nameof(Time4), typeof(double), typeof(TVFEnvelope), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));

        private static readonly DependencyProperty Level0Property = DependencyProperty.Register(nameof(Level0), typeof(double), typeof(TVFEnvelope), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        private static readonly DependencyProperty Level1Property = DependencyProperty.Register(nameof(Level1), typeof(double), typeof(TVFEnvelope), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        private static readonly DependencyProperty Level2Property = DependencyProperty.Register(nameof(Level2), typeof(double), typeof(TVFEnvelope), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        private static readonly DependencyProperty Level3Property = DependencyProperty.Register(nameof(Level3), typeof(double), typeof(TVFEnvelope), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        private static readonly DependencyProperty Level4Property = DependencyProperty.Register(nameof(Level4), typeof(double), typeof(TVFEnvelope), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));

        public double Time1
        {
            get { return (double)GetValue(Time1Property); }
            set { SetValue(Time1Property, value); }
        }
        public double Time2
        {
            get { return (double)GetValue(Time2Property); }
            set { SetValue(Time2Property, value); }
        }
        public double Time3
        {
            get { return (double)GetValue(Time3Property); }
            set { SetValue(Time3Property, value); }
        }
        public double Time4
        {
            get { return (double)GetValue(Time4Property); }
            set { SetValue(Time4Property, value); }
        }
        public double Level0
        {
            get { return (double)GetValue(Level0Property); }
            set { SetValue(Level0Property, value); }
        }
        public double Level1
        {
            get { return (double)GetValue(Level1Property); }
            set { SetValue(Level1Property, value); }
        }
        public double Level2
        {
            get { return (double)GetValue(Level2Property); }
            set { SetValue(Level2Property, value); }
        }
        public double Level3
        {
            get { return (double)GetValue(Level3Property); }
            set { SetValue(Level3Property, value); }
        }
        public double Level4
        {
            get { return (double)GetValue(Level4Property); }
            set { SetValue(Level4Property, value); }
        }

        
        #endregion

        #region Constructor

        public TVFEnvelope() : base(5, 5, 2) { }

        #endregion

        public override void Update()
        {
            //TODO: GetSegmentX   // GetSegmentY
            CP[0] = new ControlPointY(this, Level0Property, 0, 127, new LimitY(0, 0, Segments.SY));
            CP[1] = new ControlPoint(this, Time1Property, Level1Property, 0, 127, 0, 127, new Limit(0, SegmentSize.X, 0, Segments.SY));
            CP[2] = new ControlPoint(this, Time2Property, Level2Property, 0, 127, 0, 127, new Limit(CP[1].X, SegmentSize.X, 0, Segments.SY));
            CP[3] = new ControlPoint(this, Time3Property, Level3Property, 0, 127, 0, 127, new Limit(CP[2].X, SegmentSize.X, 0, Segments.SY));
            CP[4] = new ControlPoint(this, Time4Property, Level4Property, 0, 127, 0, 127, new Limit(Segments.SX - SegmentSize.X, SegmentSize.X, 0, Segments.SY));

            base.Update();
        }

        public override void RenderBackground(DrawingContext dc)
        {
            dc.DrawLine(Styles.GraphConstraintPen, new Point(Segments.SX - SegmentSize.X, 0), new Point(Segments.SX - SegmentSize.X, Segments.SY));

            if(Index != -1)
            {
                dc.DrawLine(Styles.GraphConstraintPen, new Point(0, CP[Index].Y), new Point(Segments.SX, CP[Index].Y));
            }
        }

        public override void RenderGraph(DrawingContext dc)
        {
            dc.DrawLine(Styles.GraphSelectedPen, CP[0], CP[1]);
            dc.DrawLine(Styles.GraphSelectedPen, CP[1], CP[2]);
            dc.DrawLine(Styles.GraphSelectedPen, CP[2], CP[3]);
            dc.DrawLine(Styles.GraphBorderPen, CP[3], new Point(SegmentSize.X * 4, CP[3].Y));
            
            dc.DrawLine(Styles.GraphSelectedPen, new Point(SegmentSize.X * 4, CP[3].Y), CP[4]);
            dc.DrawLine(Styles.GraphBorderPen, CP[4], new Point(Segments.SX, CP[4].Y));
        }
    }
}

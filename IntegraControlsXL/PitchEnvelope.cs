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

    public class PitchEnvelope : GraphControl
    {
        #region Dependency Properties

        private static readonly DependencyProperty Time01Property = DependencyProperty.Register("Time 1", typeof(double), typeof(PitchEnvelope), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        private static readonly DependencyProperty Time02Property = DependencyProperty.Register("Time 2", typeof(double), typeof(PitchEnvelope), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        private static readonly DependencyProperty Time03Property = DependencyProperty.Register("Time 3", typeof(double), typeof(PitchEnvelope), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        private static readonly DependencyProperty Time04Property = DependencyProperty.Register("Time 4", typeof(double), typeof(PitchEnvelope), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));

        private static readonly DependencyProperty Level00Property = DependencyProperty.Register("Level 0", typeof(double), typeof(PitchEnvelope), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        private static readonly DependencyProperty Level01Property = DependencyProperty.Register("Level 1", typeof(double), typeof(PitchEnvelope), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        private static readonly DependencyProperty Level02Property = DependencyProperty.Register("Level 2", typeof(double), typeof(PitchEnvelope), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        private static readonly DependencyProperty Level03Property = DependencyProperty.Register("Level 3", typeof(double), typeof(PitchEnvelope), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        private static readonly DependencyProperty Level04Property = DependencyProperty.Register("Level 4", typeof(double), typeof(PitchEnvelope), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));

        public double Time01
        {
            get { return (double)GetValue(Time01Property); }
            set { SetValue(Time01Property, value); }
        }
        public double Time02
        {
            get { return (double)GetValue(Time02Property); }
            set { SetValue(Time02Property, value); }
        }
        public double Time03
        {
            get { return (double)GetValue(Time03Property); }
            set { SetValue(Time03Property, value); }
        }
        public double Time04
        {
            get { return (double)GetValue(Time04Property); }
            set { SetValue(Time04Property, value); }
        }
        public double Level00
        {
            get { return (double)GetValue(Level00Property); }
            set { SetValue(Level00Property, value); }
        }
        public double Level01
        {
            get { return (double)GetValue(Level01Property); }
            set { SetValue(Level01Property, value); }
        }
        public double Level02
        {
            get { return (double)GetValue(Level02Property); }
            set { SetValue(Level02Property, value); }
        }
        public double Level03
        {
            get { return (double)GetValue(Level03Property); }
            set { SetValue(Level03Property, value); }
        }
        public double Level04
        {
            get { return (double)GetValue(Level04Property); }
            set { SetValue(Level04Property, value); }
        }

        
        #endregion


        #region Brushes & Pens


        #endregion


        #region Constructor

        public PitchEnvelope() : base(5, 5, 2) { }

        #endregion

        public override void Update()
        {
            //TODO: GetSegmentX   // GetSegmentY
            CP[0] = new ControlPointY(this, Level00Property, -63, 63, new LimitY(0, 0, Segments.SY), 63);
            CP[1] = new ControlPoint(this, Time01Property, Level01Property, 0, 127, -63, 63, new Limit(0, SegmentSize.X, 0, Segments.SY), 0, 63);
            CP[2] = new ControlPoint(this, Time02Property, Level02Property, 0, 127, -63, 63, new Limit(CP[1].X, SegmentSize.X, 0, Segments.SY), 0, 63);
            CP[3] = new ControlPoint(this, Time03Property, Level03Property, 0, 127, -63, 63, new Limit(CP[2].X, SegmentSize.X, 0, Segments.SY), 0, 63);
            CP[4] = new ControlPoint(this, Time04Property, Level04Property, 0, 127, -63, 63, new Limit(Segments.SX - SegmentSize.X, SegmentSize.X, 0, Segments.SY), 0, 63);

            base.Update();
        }

        public override void Render(DrawingContext dc)
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

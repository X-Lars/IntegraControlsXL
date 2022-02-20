using IntegraControlsXL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace IntegraControlsXL
{
    public class StepLFO : GraphControl
    {
        public StepLFO() : base(16, 16, 4) 
        {
            
        }

        public static readonly DependencyProperty Step1Property  = DependencyProperty.Register("LFO Step 1",  typeof(double), typeof(StepLFO), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty Step2Property  = DependencyProperty.Register("LFO Step 2",  typeof(double), typeof(StepLFO), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty Step3Property  = DependencyProperty.Register("LFO Step 3",  typeof(double), typeof(StepLFO), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty Step4Property  = DependencyProperty.Register("LFO Step 4",  typeof(double), typeof(StepLFO), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty Step5Property  = DependencyProperty.Register("LFO Step 5",  typeof(double), typeof(StepLFO), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty Step6Property  = DependencyProperty.Register("LFO Step 6",  typeof(double), typeof(StepLFO), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty Step7Property  = DependencyProperty.Register("LFO Step 7",  typeof(double), typeof(StepLFO), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty Step8Property  = DependencyProperty.Register("LFO Step 8",  typeof(double), typeof(StepLFO), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty Step9Property  = DependencyProperty.Register("LFO Step 9",  typeof(double), typeof(StepLFO), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty Step10Property = DependencyProperty.Register("LFO Step 10", typeof(double), typeof(StepLFO), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty Step11Property = DependencyProperty.Register("LFO Step 11", typeof(double), typeof(StepLFO), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty Step12Property = DependencyProperty.Register("LFO Step 12", typeof(double), typeof(StepLFO), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty Step13Property = DependencyProperty.Register("LFO Step 13", typeof(double), typeof(StepLFO), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty Step14Property = DependencyProperty.Register("LFO Step 14", typeof(double), typeof(StepLFO), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty Step15Property = DependencyProperty.Register("LFO Step 15", typeof(double), typeof(StepLFO), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));
        public static readonly DependencyProperty Step16Property = DependencyProperty.Register("LFO Step 16", typeof(double), typeof(StepLFO), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update, CoerceRound));

        public double Step1  { get { return (double)GetValue(Step1Property); }  set { SetValue(Step1Property,  value); } }
        public double Step2  { get { return (double)GetValue(Step2Property); }  set { SetValue(Step2Property,  value); } }
        public double Step3  { get { return (double)GetValue(Step3Property); }  set { SetValue(Step3Property,  value); } }
        public double Step4  { get { return (double)GetValue(Step4Property); }  set { SetValue(Step4Property,  value); } }
        public double Step5  { get { return (double)GetValue(Step5Property); }  set { SetValue(Step5Property,  value); } }
        public double Step6  { get { return (double)GetValue(Step6Property); }  set { SetValue(Step6Property,  value); } }
        public double Step7  { get { return (double)GetValue(Step7Property); }  set { SetValue(Step7Property,  value); } }
        public double Step8  { get { return (double)GetValue(Step8Property); }  set { SetValue(Step8Property,  value); } }
        public double Step9  { get { return (double)GetValue(Step9Property); }  set { SetValue(Step9Property,  value); } }
        public double Step10 { get { return (double)GetValue(Step10Property); } set { SetValue(Step10Property, value); } }
        public double Step11 { get { return (double)GetValue(Step11Property); } set { SetValue(Step11Property, value); } }
        public double Step12 { get { return (double)GetValue(Step12Property); } set { SetValue(Step12Property, value); } }
        public double Step13 { get { return (double)GetValue(Step13Property); } set { SetValue(Step13Property, value); } }
        public double Step14 { get { return (double)GetValue(Step14Property); } set { SetValue(Step14Property, value); } }
        public double Step15 { get { return (double)GetValue(Step15Property); } set { SetValue(Step15Property, value); } }
        public double Step16 { get { return (double)GetValue(Step16Property); } set { SetValue(Step16Property, value); } }


        public override void Update()
        {
            var sy = SegmentSize.Y;
            var sx = SegmentSize.X;

            CP[0]  = new ControlPointY(this, Step1Property,  -36, 36, new LimitY(sx / 2, sy, sy * 2), 36);
            CP[1]  = new ControlPointY(this, Step2Property,  -36, 36, new LimitY(CP[0].X  + sx, sy, sy * 2), 36);
            CP[2]  = new ControlPointY(this, Step3Property,  -36, 36, new LimitY(CP[1].X  + sx, sy, sy * 2), 36);
            CP[3]  = new ControlPointY(this, Step4Property,  -36, 36, new LimitY(CP[2].X  + sx, sy, sy * 2), 36);
            CP[4]  = new ControlPointY(this, Step5Property,  -36, 36, new LimitY(CP[3].X  + sx, sy, sy * 2), 36);
            CP[5]  = new ControlPointY(this, Step6Property,  -36, 36, new LimitY(CP[4].X  + sx, sy, sy * 2), 36);
            CP[6]  = new ControlPointY(this, Step7Property,  -36, 36, new LimitY(CP[5].X  + sx, sy, sy * 2), 36);
            CP[7]  = new ControlPointY(this, Step8Property,  -36, 36, new LimitY(CP[6].X  + sx, sy, sy * 2), 36);
            CP[8]  = new ControlPointY(this, Step9Property,  -36, 36, new LimitY(CP[7].X  + sx, sy, sy * 2), 36);
            CP[9]  = new ControlPointY(this, Step10Property, -36, 36, new LimitY(CP[8].X  + sx, sy, sy * 2), 36);
            CP[10] = new ControlPointY(this, Step11Property, -36, 36, new LimitY(CP[9].X  + sx, sy, sy * 2), 36);
            CP[11] = new ControlPointY(this, Step12Property, -36, 36, new LimitY(CP[10].X + sx, sy, sy * 2), 36);
            CP[12] = new ControlPointY(this, Step13Property, -36, 36, new LimitY(CP[11].X + sx, sy, sy * 2), 36);
            CP[13] = new ControlPointY(this, Step14Property, -36, 36, new LimitY(CP[12].X + sx, sy, sy * 2), 36);
            CP[14] = new ControlPointY(this, Step15Property, -36, 36, new LimitY(CP[13].X + sx, sy, sy * 2), 36);
            CP[15] = new ControlPointY(this, Step16Property, -36, 36, new LimitY(CP[14].X + sx, sy, sy * 2), 36);

            base.Update();
        }

        public override void Render(DrawingContext dc)
        {
            var sx = SegmentSize.X;
            var scx = sx / 2;

            if (Index != -1)
            {
                Rect rect = CP[Index].Limit;
                rect.Inflate(scx, 0);
                dc.DrawRectangle(Styles.GraphHighlight, null, rect);
            }

            dc.DrawLine(Styles.GraphGridPen, new Point(0, Segments.CY), new Point(Segments.SX, Segments.CY));

            for (int i = 0; i < CP.Length; i++)
            {
                dc.DrawLine(Styles.GraphSelectedPen, new Point(CP[i].X - scx, CP[i].Y), new Point(CP[i].X + scx, CP[i].Y));
            }

            for (int i = 1; i < CP.Length; i++)
            {
                dc.DrawLine(Styles.GraphSelectedPen, new Point(i * sx, CP[i].Y), new Point(i * sx, CP[i - 1].Y));
            }

            dc.DrawLine(Styles.GraphSelectedPen, new Point(0, Segments.CY), new Point(0, CP[0].Y));
            dc.DrawLine(Styles.GraphSelectedPen, new Point(Segments.SX, Segments.CY), new Point(Segments.SX, CP[15].Y));

            
        }
    }
}

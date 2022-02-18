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

    //public class PitchEnvelope : GraphControl
    //{
    //    #region Dependency Properties

    //    private static DependencyProperty Time01Property = DependencyProperty.Register("Time01", typeof(int), typeof(PitchEnvelope), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update));
    //    private static DependencyProperty Time02Property = DependencyProperty.Register("Time02", typeof(int), typeof(PitchEnvelope), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update));
    //    private static DependencyProperty Time03Property = DependencyProperty.Register("Time03", typeof(int), typeof(PitchEnvelope), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update));
    //    private static DependencyProperty Time04Property = DependencyProperty.Register("Time04", typeof(int), typeof(PitchEnvelope), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update));

    //    private static DependencyProperty Level00Property = DependencyProperty.Register("Level00", typeof(int), typeof(PitchEnvelope), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update));
    //    private static DependencyProperty Level01Property = DependencyProperty.Register("Level01", typeof(int), typeof(PitchEnvelope), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update));
    //    private static DependencyProperty Level02Property = DependencyProperty.Register("Level02", typeof(int), typeof(PitchEnvelope), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update));
    //    private static DependencyProperty Level03Property = DependencyProperty.Register("Level03", typeof(int), typeof(PitchEnvelope), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update));
    //    private static DependencyProperty Level04Property = DependencyProperty.Register("Level04", typeof(int), typeof(PitchEnvelope), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Update));

    //    private static DependencyProperty HandleStyleProperty = DependencyProperty.Register("HandleStyle", typeof(HandleStyles), typeof(PitchEnvelope), new UIPropertyMetadata(HandleStyles.Circle));
    //    private static DependencyProperty ShowLabelsProperty = DependencyProperty.Register("ShowLabels", typeof(bool), typeof(PitchEnvelope), new UIPropertyMetadata(true));

    //    public int Time01
    //    {
    //        get { return (int)GetValue(Time01Property); }
    //        set { SetValue(Time01Property, value); }
    //    }
    //    public int Time02
    //    {
    //        get { return (int)GetValue(Time02Property); }
    //        set { SetValue(Time02Property, value); }
    //    }
    //    public int Time03
    //    {
    //        get { return (int)GetValue(Time03Property); }
    //        set { SetValue(Time03Property, value); }
    //    }
    //    public int Time04
    //    {
    //        get { return (int)GetValue(Time04Property); }
    //        set { SetValue(Time04Property, value); }
    //    }
    //    public int Level00
    //    {
    //        get { return (int)GetValue(Level00Property); }
    //        set { SetValue(Level00Property, value); }
    //    }
    //    public int Level01
    //    {
    //        get { return (int)GetValue(Level01Property); }
    //        set { SetValue(Level01Property, value); }
    //    }
    //    public int Level02
    //    {
    //        get { return (int)GetValue(Level02Property); }
    //        set { SetValue(Level02Property, value); }
    //    }
    //    public int Level03
    //    {
    //        get { return (int)GetValue(Level03Property); }
    //        set { SetValue(Level03Property, value); }
    //    }
    //    public int Level04
    //    {
    //        get { return (int)GetValue(Level04Property); }
    //        set { SetValue(Level04Property, value); }
    //    }

    //    public HandleStyles HandleStyle
    //    {
    //        get { return (HandleStyles)GetValue(HandleStyleProperty); }
    //        set { SetValue(HandleStyleProperty, value); }
    //    }

    //    public bool ShowLabels
    //    {
    //        get { return (bool)GetValue(ShowLabelsProperty); }
    //        set { SetValue(ShowLabelsProperty, value); }
    //    }

    //    private int InvalidatePropertyValue(int value)
    //    {
    //        if (value < 0)
    //        {
    //            value = 0;
    //        }
    //        if (value > _MaxValue)
    //        {
    //            value = _MaxValue;
    //        }

    //        return value;
    //    }

    //    #endregion

    //    // The "size" of an object for mouse over purposes.
    //    private const int _HitRadius = 3;

    //    private const int _Padding = 5;

    //    // We're over an object if the distance squared
    //    // between the mouse and the object is less than this.
    //    private const int _HitDistance = _HitRadius * _HitRadius;

    //    private int _Selected = -1;
    //    private int _IsMouseOver = -1;

    //    // Mouse offset correction to the center of the clicked point
    //    private double MouseOffsetX;
    //    private double MouseOffsetY;


    //    private double _SegmentSizeX;
    //    private double _FactorX;
    //    private double _FactorY;

    //    private bool _IsInitialized = false;
    //    //private bool _DrawGraphHandles = false;

    //    /// <summary>
    //    /// Maximum pitch envelope properties value
    //    /// </summary>
    //    private const int _MaxValue = 127;

    //    private ControlPoint[] _Points = new ControlPoint[7];

    //    #region Brushes & Pens

    //    private Pen _GraphBordersPen;
    //    private Pen _GraphDataPen;
    //    private Pen _PointGuidesPen;
    //    private Pen _PointConstraintsPen;
    //    private Pen _ActivePointPen;

    //    private Brush _ActivePointBrush;
    //    private Brush _InactivePointBrush;
    //    private Brush _MouseOverPointBrush;
    //    private Brush _LabelForegroundBrush;
    //    private Brush _LabelBackgroundBrush;
    //    private Brush _BoundsBrush;

    //    #endregion


    //    #region Constructor

    //    static PitchEnvelope()
    //    {
    //        DefaultStyleKeyProperty.OverrideMetadata(typeof(PitchEnvelope), new FrameworkPropertyMetadata(typeof(PitchEnvelope)));
    //    }

    //    public PitchEnvelope() : base(7, 7, 2)
    //    {
    //    }

    //    #endregion

    //    public override void Update()
    //    {
    //        //_Points[0] = new ControlPoint(_Padding, (Level00 * _FactorY) + _Padding, _Padding, _Padding, _Padding, ActualHeight - _Padding);
    //        //_Points[1] = new ControlPoint((Time01 * _FactorX) + _Padding, (Level01 * _FactorY) + _Padding, _Padding, _Padding, _SegmentSizeX + _Padding, ActualHeight - _Padding);
    //        //_Points[2] = new ControlPoint(_Points[1].X + (Time02 * _FactorX), (Level02 * _FactorY) + _Padding, _Points[1].X, _Padding, _Points[1].X + _SegmentSizeX, ActualHeight - _Padding);
    //        //_Points[3] = new ControlPoint(_Points[2].X + (Time03 * _FactorX), (Level03 * _FactorY) + _Padding, _Points[2].X, _Padding, _Points[2].X + _SegmentSizeX, ActualHeight - _Padding);
    //        //_Points[4] = new ControlPoint((_SegmentSizeX * 4) + _Padding, _Points[3].Y, (_SegmentSizeX * 4) + _Padding, _Padding, (_SegmentSizeX * 4) + _Padding, ActualHeight - _Padding);
    //        //_Points[5] = new ControlPoint(_Points[4].X + (Time04 * _FactorX), (Level04 * _FactorY) + _Padding, _Points[4].X, _Padding, _Points[4].X + _SegmentSizeX, ActualHeight - _Padding);
    //        //_Points[6] = new ControlPoint(ActualWidth - _Padding, _Points[5].Y, ActualWidth - _Padding, _Padding, ActualWidth - _Padding, ActualHeight - _Padding);

    //        base.Update();
    //    }


    //    protected override void OnRender(DrawingContext dc)
    //    {
    //        base.OnRender(dc);
    //        // TODO: Default Brushes & Pens

    //        if (!_IsInitialized)
    //            return;

    //        if (ActualWidth == 0 || ActualHeight == 0)
    //            return;

    //        // Draw the background
    //        dc.DrawRectangle(this.Background, null, new Rect(RenderSize));

    //        // Draw the inner content border based on the padding
    //        dc.DrawRectangle(null, _GraphBordersPen, new Rect(_Padding, _Padding, this.ActualWidth - _Padding * 2, this.ActualHeight - _Padding * 2));

    //        // Horizontal center line
    //        dc.DrawLine(_GraphBordersPen, new Point(_Padding + 1, this.ActualHeight / 2), new Point(this.ActualWidth - _Padding - 1, this.ActualHeight / 2));

    //        // Draw value bounds rectangel
    //        if (_Selected != -1)
    //        {
    //            if (_Points[_Selected].MinX != _Points[_Selected].MaxX)
    //                dc.DrawRectangle(_BoundsBrush, null, new Rect(_Points[_Selected].MinX, _Points[_Selected].MinY, _SegmentSizeX, ActualHeight - _Padding));
    //        }

    //        // Linked point constraint guides
    //        dc.DrawLine(_PointConstraintsPen, new Point(_Padding, _Points[3].Y), new Point(this.ActualWidth - _Padding, _Points[3].Y));
    //        dc.DrawLine(_PointConstraintsPen, new Point(_Padding, _Points[5].Y), new Point(this.ActualWidth - _Padding, _Points[5].Y));

    //        // Actual Graph lines
    //        for (int i = 0; i < _Points.Length - 1; i++)
    //        {
    //            dc.DrawLine(_GraphDataPen, _Points[i], _Points[i + 1]);
    //        }

    //        // Draw constraint indicators
    //        switch (_Selected)
    //        {
    //            case 1:
    //            case 2:
    //            case 3:
    //            case 5:
    //                dc.DrawLine(_PointGuidesPen, new Point(_Padding, _Points[_Selected].Y), new Point(this.ActualWidth - _Padding, _Points[_Selected].Y));
    //                dc.DrawLine(_PointGuidesPen, new Point(_Points[_Selected].X, _Padding), new Point(_Points[_Selected].X, ActualHeight - _Padding));
    //                break;

    //            case 0:
    //            case 4:
    //            case 6:
    //                dc.DrawLine(_PointGuidesPen, new Point(_Points[_Selected].X, _Padding), new Point(_Points[_Selected].X, ActualHeight - _Padding));
    //                break;
    //        }

    //        // Graph handles
    //        if (HandleStyle != HandleStyles.None)
    //        {
    //            for (int i = 0; i < _Points.Length; i++)
    //            {
    //                Rect rect = new Rect(_Points[i].X - _HitRadius, _Points[i].Y - _HitRadius, _HitRadius * 2 + 1, _HitRadius * 2 + 1);

    //                if (HandleStyle == HandleStyles.Square)
    //                {
    //                    dc.DrawRectangle(_InactivePointBrush, null, rect);

    //                    if (i == _Selected)
    //                    {
    //                        dc.DrawRectangle(_ActivePointBrush, _ActivePointPen, rect);
    //                    }
    //                    else if (i == _IsMouseOver)
    //                    {
    //                        dc.DrawRectangle(_MouseOverPointBrush, null, rect);
    //                    }
    //                }
    //                else
    //                {

    //                    dc.DrawEllipse(_InactivePointBrush, null, _Points[i], _HitRadius, _HitRadius);

    //                    if (i == _Selected)
    //                    {
    //                        dc.DrawEllipse(_ActivePointBrush, _ActivePointPen, _Points[i], _HitRadius, _HitRadius);
    //                    }
    //                    else if (i == _IsMouseOver)
    //                    {
    //                        dc.DrawEllipse(_MouseOverPointBrush, null, _Points[i], _HitRadius, _HitRadius);
    //                    }
    //                }

    //            }
    //        }

    //        // Draw the data text label
    //        if (_Selected != -1)
    //        {
    //            if (ShowLabels == true)
    //            {
    //                string label = string.Empty;

    //                switch (_Selected)
    //                {
    //                    case 0:
    //                        label = string.Format("L[{0}]", Level00);
    //                        break;
    //                    case 1:
    //                        label = string.Format("T[{0}] L[{1}]", Time01, Level01);
    //                        break;
    //                    case 2:
    //                        label = string.Format("T[{0}] L[{1}]", Time02, Level02);
    //                        break;
    //                    case 3:
    //                        label = string.Format("T[{0}] L[{1}]", Time03, Level03);
    //                        break;
    //                    case 4:
    //                        label = string.Format("L[{0}]", Level03);
    //                        break;
    //                    case 5:
    //                        label = string.Format("T[{0}] L[{1}]", Time04, Level04);
    //                        break;
    //                    case 6:
    //                        label = string.Format("L[{0}]", Level04);
    //                        break;
    //                }

    //                // Prevent rendering error when text is empty string
    //                if (label != string.Empty)
    //                {
    //                    // The text to show inside the label
    //                    FormattedText text = new FormattedText(label, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(FontFamily, FontStyle, FontWeight, FontStretches.Normal), FontSize, Foreground);

    //                    // Convert the text to geometry to be able to get it's bounds
    //                    Geometry geo = text.BuildGeometry(new Point(_Points[_Selected].X, _Points[_Selected].Y));

    //                    // Mirror the text vertically
    //                    ScaleTransform mirrorY = new ScaleTransform(1, -1, _Points[_Selected].X, _Points[_Selected].Y);

    //                    // Translate the text in -X so the cursor doesn't overlay
    //                    TranslateTransform translateX = new TranslateTransform(-text.Width - _Padding * 1.5, _Padding);

    //                    // Apply the transformations
    //                    dc.PushTransform(mirrorY);
    //                    dc.PushTransform(translateX);

    //                    dc.DrawRectangle(_LabelBackgroundBrush, null, new Rect(geo.Bounds.X - _Padding, geo.Bounds.Y - _Padding, geo.Bounds.Width + _Padding * 2, geo.Bounds.Height + _Padding * 2));
    //                    dc.DrawGeometry(_LabelForegroundBrush, null, geo);
    //                    //dc.DrawText(text, new Point(_Points[_Selected].X, _Points[_Selected].Y));
    //                    //dc.DrawGeometry(Foreground, new Pen(Foreground, 1.0), textGeometry);
    //                }
    //            }
    //        }
    //    }


    //}
}

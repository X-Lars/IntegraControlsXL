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

    public class PitchEnvelope : UserControl
    {
        #region Dependency Properties

        private static DependencyProperty Time01Property = DependencyProperty.Register("Time01", typeof(int), typeof(PitchEnvelope), new UIPropertyMetadata(0));
        private static DependencyProperty Time02Property = DependencyProperty.Register("Time02", typeof(int), typeof(PitchEnvelope), new UIPropertyMetadata(0));
        private static DependencyProperty Time03Property = DependencyProperty.Register("Time03", typeof(int), typeof(PitchEnvelope), new UIPropertyMetadata(0));
        private static DependencyProperty Time04Property = DependencyProperty.Register("Time04", typeof(int), typeof(PitchEnvelope), new UIPropertyMetadata(0));

        private static DependencyProperty Level00Property = DependencyProperty.Register("Level00", typeof(int), typeof(PitchEnvelope), new UIPropertyMetadata(0));
        private static DependencyProperty Level01Property = DependencyProperty.Register("Level01", typeof(int), typeof(PitchEnvelope), new UIPropertyMetadata(0));
        private static DependencyProperty Level02Property = DependencyProperty.Register("Level02", typeof(int), typeof(PitchEnvelope), new UIPropertyMetadata(0));
        private static DependencyProperty Level03Property = DependencyProperty.Register("Level03", typeof(int), typeof(PitchEnvelope), new UIPropertyMetadata(0));
        private static DependencyProperty Level04Property = DependencyProperty.Register("Level04", typeof(int), typeof(PitchEnvelope), new UIPropertyMetadata(0));

        private static DependencyProperty HandleStyleProperty = DependencyProperty.Register("HandleStyle", typeof(HandleStyles), typeof(PitchEnvelope), new UIPropertyMetadata(HandleStyles.Circle));
        private static DependencyProperty ShowLabelsProperty = DependencyProperty.Register("ShowLabels", typeof(bool), typeof(PitchEnvelope), new UIPropertyMetadata(true));

        public int Time01
        {
            get { return (int)GetValue(Time01Property); }
            set { SetValue(Time01Property, InvalidatePropertyValue(value)); }
        }
        public int Time02
        {
            get { return (int)GetValue(Time02Property); }
            set { SetValue(Time02Property, InvalidatePropertyValue(value)); }
        }
        public int Time03
        {
            get { return (int)GetValue(Time03Property); }
            set { SetValue(Time03Property, InvalidatePropertyValue(value)); }
        }
        public int Time04
        {
            get { return (int)GetValue(Time04Property); }
            set { SetValue(Time04Property, InvalidatePropertyValue(value)); }
        }
        public int Level00
        {
            get { return (int)GetValue(Level00Property); }
            set { SetValue(Level00Property, InvalidatePropertyValue(value)); }
        }
        public int Level01
        {
            get { return (int)GetValue(Level01Property); }
            set { SetValue(Level01Property, InvalidatePropertyValue(value)); }
        }
        public int Level02
        {
            get { return (int)GetValue(Level02Property); }
            set { SetValue(Level02Property, InvalidatePropertyValue(value)); }
        }
        public int Level03
        {
            get { return (int)GetValue(Level03Property); }
            set { SetValue(Level03Property, InvalidatePropertyValue(value)); }
        }
        public int Level04
        {
            get { return (int)GetValue(Level04Property); }
            set { SetValue(Level04Property, InvalidatePropertyValue(value)); }
        }

        public HandleStyles HandleStyle
        {
            get { return (HandleStyles)GetValue(HandleStyleProperty); }
            set { SetValue(HandleStyleProperty, value); }
        }

        public bool ShowLabels
        {
            get { return (bool)GetValue(ShowLabelsProperty); }
            set { SetValue(ShowLabelsProperty, value); }
        }

        private int InvalidatePropertyValue(int value)
        {
            if (value < 0)
            {
                value = 0;
            }
            if (value > _MaxValue)
            {
                value = _MaxValue;
            }

            return value;
        }

        #endregion

        // The "size" of an object for mouse over purposes.
        private const int _HitRadius = 3;

        private const int _Padding = 5;

        // We're over an object if the distance squared
        // between the mouse and the object is less than this.
        private const int _HitDistance = _HitRadius * _HitRadius;

        private int _Selected = -1;
        private int _IsMouseOver = -1;

        // Mouse offset correction to the center of the clicked point
        private double MouseOffsetX;
        private double MouseOffsetY;


        private double _SegmentSizeX;
        private double _FactorX;
        private double _FactorY;

        private bool _IsInitialized = false;
        //private bool _DrawGraphHandles = false;

        /// <summary>
        /// Maximum pitch envelope properties value
        /// </summary>
        private const int _MaxValue = 127;

        private LimitedPoint[] _Points = new LimitedPoint[7];

        #region Brushes & Pens

        private Pen _GraphBordersPen;
        private Pen _GraphDataPen;
        private Pen _PointGuidesPen;
        private Pen _PointConstraintsPen;
        private Pen _ActivePointPen;

        private Brush _ActivePointBrush;
        private Brush _InactivePointBrush;
        private Brush _MouseOverPointBrush;
        private Brush _LabelForegroundBrush;
        private Brush _LabelBackgroundBrush;
        private Brush _BoundsBrush;

        #endregion


        #region Constructor

        static PitchEnvelope()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PitchEnvelope), new FrameworkPropertyMetadata(typeof(PitchEnvelope)));
        }

        public PitchEnvelope()
        {
            // Invert the controls Y coordinate to start from the bottom left corner
            this.LayoutTransform = new ScaleTransform(1, -1);

            // Hook up the event handlers
            this.Loaded += ControlLoaded;
            this.MouseDown += OnMouseDown;
            this.MouseMove += OnMouseMove;
            this.MouseUp += OnMouseUp;
        }

        #endregion

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            InitializePoints();
        }

        private void ControlLoaded(object sender, RoutedEventArgs e)
        {
            this.InitializePoints();
            this.InitializeStyle();
            _IsInitialized = true;
        }

        private void InitializeStyle()
        {
            BorderThickness = new Thickness(1);
            Padding = new Thickness(0);

            Color backgroundColor = ((SolidColorBrush)Background).Color;
            Color foregroundColor = ((SolidColorBrush)Foreground).Color;

            Brush backgroundOverlayBrush;
            Brush foregroundOverlayBrush;

            // Check for transparent background
            if (backgroundColor.A == 0)
            {
                Window parentWindow = Window.GetWindow(this);

                // Use parent window background
                if (parentWindow != null)
                {
                    Background = Window.GetWindow(this).Background;
                    backgroundColor = ((SolidColorBrush)Background).Color;
                }
            }

            // If parent window background is transparent or parent window is null, create a slightly visible background brush
            if (backgroundColor.A == 0)
            {
                backgroundColor = Color.FromScRgb(0.25f, 0, 0, 0);
                Background = new SolidColorBrush(backgroundColor);
            }

            if (((backgroundColor.R + backgroundColor.G + backgroundColor.B) / 3) > 64)
            {
                // Light background, use dark overlay
                backgroundOverlayBrush = new SolidColorBrush(Color.FromScRgb(0.25f, 0, 0, 0));
            }
            else
            {
                // Dark background, use light overlay
                backgroundOverlayBrush = new SolidColorBrush(Color.FromScRgb(0.25f, 255, 255, 255));
            }

            if (((foregroundColor.R + foregroundColor.G + foregroundColor.B) / 3) > 192)
            {
                // Light background, use dark overlay
                foregroundOverlayBrush = new SolidColorBrush(Color.FromScRgb(0.25f, 0, 0, 0));
            }
            else
            {
                // Dark background, use light overlay
                foregroundOverlayBrush = new SolidColorBrush(Color.FromScRgb(0.25f, 255, 255, 255));
            }

            _GraphBordersPen = new Pen();
            _GraphBordersPen.Brush = backgroundOverlayBrush;
            _GraphBordersPen.Thickness = 1;

            _PointGuidesPen = new Pen();
            _PointGuidesPen.Brush = backgroundOverlayBrush;
            _PointGuidesPen.Thickness = 0.5;
            _PointGuidesPen.DashStyle = DashStyles.Dash;

            _GraphDataPen = new Pen();
            _GraphDataPen.Brush = Foreground;
            _GraphDataPen.Thickness = 2;

            _PointConstraintsPen = new Pen();
            _PointConstraintsPen.Brush = Foreground;
            _PointConstraintsPen.Thickness = 0.25;
            _PointConstraintsPen.DashStyle = DashStyles.Dot;

            _ActivePointPen = new Pen();
            _ActivePointPen.Brush = foregroundOverlayBrush;
            _ActivePointPen.Thickness = 1;

            _ActivePointBrush = Background;
            _InactivePointBrush = Foreground;
            _MouseOverPointBrush = foregroundOverlayBrush;

            _LabelBackgroundBrush = backgroundOverlayBrush;
            _LabelForegroundBrush = Background;

            _BoundsBrush = new SolidColorBrush(Color.FromScRgb(0.1f, foregroundColor.ScR, foregroundColor.ScG, foregroundColor.ScB));
        }

        private void InitializeGraphScaleFactors()
        {
            // Get the number of segments excluding the first and last polygon points
            _SegmentSizeX = (this.ActualWidth - _Padding * 2) / (_Points.Length - 2);

            // Divide the segments by the maximum MIDI value
            _FactorX = _SegmentSizeX / _MaxValue;
            _FactorY = (this.ActualHeight - _Padding * 2) / _MaxValue;
        }

        private void InitializePoints()
        {
            this.InitializeGraphScaleFactors();

            _Points[0] = new LimitedPoint(_Padding, (Level00 * _FactorY) + _Padding, _Padding, _Padding, _Padding, ActualHeight - _Padding);
            _Points[1] = new LimitedPoint((Time01 * _FactorX) + _Padding, (Level01 * _FactorY) + _Padding, _Padding, _Padding, _SegmentSizeX + _Padding, ActualHeight - _Padding);
            _Points[2] = new LimitedPoint(_Points[1].X + (Time02 * _FactorX), (Level02 * _FactorY) + _Padding, _Points[1].X, _Padding, _Points[1].X + _SegmentSizeX, ActualHeight - _Padding);
            _Points[3] = new LimitedPoint(_Points[2].X + (Time03 * _FactorX), (Level03 * _FactorY) + _Padding, _Points[2].X, _Padding, _Points[2].X + _SegmentSizeX, ActualHeight - _Padding);
            _Points[4] = new LimitedPoint((_SegmentSizeX * 4) + _Padding, _Points[3].Y, (_SegmentSizeX * 4) + _Padding, _Padding, (_SegmentSizeX * 4) + _Padding, ActualHeight - _Padding);
            _Points[5] = new LimitedPoint(_Points[4].X + (Time04 * _FactorX), (Level04 * _FactorY) + _Padding, _Points[4].X, _Padding, _Points[4].X + _SegmentSizeX, ActualHeight - _Padding);
            _Points[6] = new LimitedPoint(ActualWidth - _Padding, _Points[5].Y, ActualWidth - _Padding, _Padding, ActualWidth - _Padding, ActualHeight - _Padding);

            this.InvalidateVisual();
        }

        private void InvalidatePointLimits()
        {
            _Points[2].MinX = _Points[1].X;
            _Points[2].MaxX = _Points[1].X + _SegmentSizeX;

            _Points[3].MinX = _Points[2].X;
            _Points[3].MaxX = _Points[2].X + _SegmentSizeX;

            _Points[5].MinX = _Points[4].X;
            _Points[5].MaxX = _Points[4].X + _SegmentSizeX;
        }

        private void UpdatePropertyValues()
        {
            Level00 = (int)((_Points[0].Y - _Padding) / _FactorY);
            Level01 = (int)((_Points[1].Y - _Padding) / _FactorY);
            Level02 = (int)((_Points[2].Y - _Padding) / _FactorY);
            Level03 = (int)((_Points[3].Y - _Padding) / _FactorY);
            Level04 = (int)((_Points[5].Y - _Padding) / _FactorY);

            Time01 = (int)((_Points[1].X - _Padding) / _FactorX);
            Time02 = (int)((_Points[2].X - _Points[1].X) / _FactorX);
            Time03 = (int)((_Points[3].X - _Points[2].X) / _FactorX);
            Time04 = (int)((_Points[5].X - _Points[4].X) / _FactorX);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            // TODO: Default Brushes & Pens

            if (!_IsInitialized)
                return;

            // Draw the background
            dc.DrawRectangle(this.Background, null, new Rect(RenderSize));

            // Draw the inner content border based on the padding
            dc.DrawRectangle(null, _GraphBordersPen, new Rect(_Padding, _Padding, this.ActualWidth - _Padding * 2, this.ActualHeight - _Padding * 2));

            // Horizontal center line
            dc.DrawLine(_GraphBordersPen, new Point(_Padding + 1, this.ActualHeight / 2), new Point(this.ActualWidth - _Padding - 1, this.ActualHeight / 2));

            // Draw value bounds rectangel
            if (_Selected != -1)
            {
                if (_Points[_Selected].MinX != _Points[_Selected].MaxX)
                    dc.DrawRectangle(_BoundsBrush, null, new Rect(_Points[_Selected].MinX, _Points[_Selected].MinY, _SegmentSizeX, ActualHeight - _Padding));
            }

            // Linked point constraint guides
            dc.DrawLine(_PointConstraintsPen, new Point(_Padding, _Points[3].Y), new Point(this.ActualWidth - _Padding, _Points[3].Y));
            dc.DrawLine(_PointConstraintsPen, new Point(_Padding, _Points[5].Y), new Point(this.ActualWidth - _Padding, _Points[5].Y));

            // Actual Graph lines
            for (int i = 0; i < _Points.Length - 1; i++)
            {
                dc.DrawLine(_GraphDataPen, _Points[i], _Points[i + 1]);
            }

            // Draw constraint indicators
            switch (_Selected)
            {
                case 1:
                case 2:
                case 3:
                case 5:
                    dc.DrawLine(_PointGuidesPen, new Point(_Padding, _Points[_Selected].Y), new Point(this.ActualWidth - _Padding, _Points[_Selected].Y));
                    dc.DrawLine(_PointGuidesPen, new Point(_Points[_Selected].X, _Padding), new Point(_Points[_Selected].X, ActualHeight - _Padding));
                    break;

                case 0:
                case 4:
                case 6:
                    dc.DrawLine(_PointGuidesPen, new Point(_Points[_Selected].X, _Padding), new Point(_Points[_Selected].X, ActualHeight - _Padding));
                    break;
            }

            // Graph handles
            if (HandleStyle != HandleStyles.None)
            {
                for (int i = 0; i < _Points.Length; i++)
                {
                    Rect rect = new Rect(_Points[i].X - _HitRadius, _Points[i].Y - _HitRadius, _HitRadius * 2 + 1, _HitRadius * 2 + 1);

                    if (HandleStyle == HandleStyles.Square)
                    {
                        dc.DrawRectangle(_InactivePointBrush, null, rect);

                        if (i == _Selected)
                        {
                            dc.DrawRectangle(_ActivePointBrush, _ActivePointPen, rect);
                        }
                        else if (i == _IsMouseOver)
                        {
                            dc.DrawRectangle(_MouseOverPointBrush, null, rect);
                        }
                    }
                    else
                    {

                        dc.DrawEllipse(_InactivePointBrush, null, _Points[i], _HitRadius, _HitRadius);

                        if (i == _Selected)
                        {
                            dc.DrawEllipse(_ActivePointBrush, _ActivePointPen, _Points[i], _HitRadius, _HitRadius);
                        }
                        else if (i == _IsMouseOver)
                        {
                            dc.DrawEllipse(_MouseOverPointBrush, null, _Points[i], _HitRadius, _HitRadius);
                        }
                    }

                }
            }

            // Draw the data text label
            if (_Selected != -1)
            {
                if (ShowLabels == true)
                {
                    string label = string.Empty;

                    switch (_Selected)
                    {
                        case 0:
                            label = string.Format("L[{0}]", Level00);
                            break;
                        case 1:
                            label = string.Format("T[{0}] L[{1}]", Time01, Level01);
                            break;
                        case 2:
                            label = string.Format("T[{0}] L[{1}]", Time02, Level02);
                            break;
                        case 3:
                            label = string.Format("T[{0}] L[{1}]", Time03, Level03);
                            break;
                        case 4:
                            label = string.Format("L[{0}]", Level03);
                            break;
                        case 5:
                            label = string.Format("T[{0}] L[{1}]", Time04, Level04);
                            break;
                        case 6:
                            label = string.Format("L[{0}]", Level04);
                            break;
                    }

                    // Prevent rendering error when text is empty string
                    if (label != string.Empty)
                    {
                        // The text to show inside the label
                        FormattedText text = new FormattedText(label, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(FontFamily, FontStyle, FontWeight, FontStretches.Normal), FontSize, Foreground);

                        // Convert the text to geometry to be able to get it's bounds
                        Geometry geo = text.BuildGeometry(new Point(_Points[_Selected].X, _Points[_Selected].Y));

                        // Mirror the text vertically
                        ScaleTransform mirrorY = new ScaleTransform(1, -1, _Points[_Selected].X, _Points[_Selected].Y);

                        // Translate the text in -X so the cursor doesn't overlay
                        TranslateTransform translateX = new TranslateTransform(-text.Width - _Padding * 1.5, _Padding);

                        // Apply the transformations
                        dc.PushTransform(mirrorY);
                        dc.PushTransform(translateX);

                        dc.DrawRectangle(_LabelBackgroundBrush, null, new Rect(geo.Bounds.X - _Padding, geo.Bounds.Y - _Padding, geo.Bounds.Width + _Padding * 2, geo.Bounds.Height + _Padding * 2));
                        dc.DrawGeometry(_LabelForegroundBrush, null, geo);
                        //dc.DrawText(text, new Point(_Points[_Selected].X, _Points[_Selected].Y));
                        //dc.DrawGeometry(Foreground, new Pen(Foreground, 1.0), textGeometry);
                    }
                }
            }
        }


        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition(this);
            int hitPoint;

            if (IsMouseOverHandle(mousePosition, out hitPoint))
            {
                // Index of selected point
                _Selected = hitPoint;

                MouseOffsetX = _Points[hitPoint].X - mousePosition.X;
                MouseOffsetY = _Points[hitPoint].Y - mousePosition.Y;

                this.InvalidateVisual();
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _Selected = -1;
            UpdatePropertyValues();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (_Selected != -1)
                {
                    double x = e.GetPosition(this).X + MouseOffsetX;
                    double y = e.GetPosition(this).Y + MouseOffsetY;

                    // Validate X position limits
                    if (Math.Floor(x) < _Points[_Selected].MinX)
                        x = _Points[_Selected].MinX;
                    else if (Math.Ceiling(x) > _Points[_Selected].MaxX)
                        x = _Points[_Selected].MaxX;

                    // Validate Y position limits
                    if (Math.Floor(y) < _Points[_Selected].MinY)
                        y = _Points[_Selected].MinY;
                    else if (Math.Ceiling(y) > _Points[_Selected].MaxY)
                        y = _Points[_Selected].MaxY;

                    // Store offset to update linked points
                    double offsetX = (x - _Points[_Selected].X);
                    double offsetY = (y - _Points[_Selected].Y);

                    // Update linked points
                    switch (_Selected)
                    {
                        case 1:
                            _Points[2].X += offsetX;
                            _Points[3].X += offsetX;

                            break;

                        case 2:
                            _Points[3].X += offsetX;
                            break;

                        case 3:
                            _Points[4].Y += offsetY;
                            break;

                        case 4:
                            _Points[3].Y += offsetY;
                            break;
                        case 5:
                            _Points[6].Y += offsetY;
                            break;
                        case 6:
                            _Points[5].Y += offsetY;
                            break;

                    }

                    // Update selected point
                    _Points[_Selected].X = x;
                    _Points[_Selected].Y = y;

                    InvalidatePointLimits();
                    UpdatePropertyValues();
                }
            }
            else
            {
                int hitPoint;

                if (IsMouseOverHandle(e.GetPosition(this), out hitPoint))
                {
                    _IsMouseOver = hitPoint;
                }
                else
                {
                    _IsMouseOver = -1;
                }
            }

            this.InvalidateVisual();
        }


        private bool IsMouseOverHandle(Point mousePosition, out int hitPoint)
        {
            for (int i = 0; i < _Points.Length; i++)
            {
                if (Distance(_Points[i], mousePosition) < _HitDistance)
                {
                    hitPoint = i;
                    return true;
                }
            }

            hitPoint = -1;
            return false;

        }

        // Calculate the distance squared between two points.
        private double Distance(Point pointA, Point pointB)
        {
            double distanceX = pointA.X - pointB.X;
            double distanceY = pointA.Y - pointB.Y;

            return distanceX * distanceX + distanceY * distanceY;
        }
    }
}

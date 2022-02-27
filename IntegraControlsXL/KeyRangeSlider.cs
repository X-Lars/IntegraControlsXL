using IntegraControlsXL.Common;
using IntegraXL.Core;
using StylesXL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace IntegraControlsXL
{

    public enum HandleDirection
    {
        Left,
        Right
    }

    public class KeyRangeSlider : Control
    {
        /// <summary>
        /// The mouse X offset relative to the center of the selected <see cref="ControlPoint"/>.
        /// </summary>
        public double MouseOffsetX = 0;

        /// <summary>
        /// Gets or sets the index specifying the selected control point.
        /// </summary>
        public int Index { get; private set; } = -1;

        /// <summary>
        /// Gets or sets the index specifying the mouse focused control point.
        /// </summary>
        public int MouseIndex { get; protected set; } = -1;

        private double LowerX = 0;
        private double UpperX = 0;

        private double Factor = 0;

        private double SizeX;
        private double SizeY;

        private const double HANDLE_WIDTH = 4;

        private double MinX;
        private double MaxX;

        private double _Lower = 0;
        private double _Upper = 0;

        private double Lower
        {
            get => _Lower;
            set
            {
                value = value < 0 ? 0 : value > 127 ? 127 : value;

                if(_Lower != value)
                {
                    _Lower = value;
                    ValueLower = (IntegraScales)value;

                    if (value > _Upper)
                        ValueUpper = ValueLower;
                }
            }
        }

        private double Upper
        {
            get => _Upper;
            set
            {
                value = value < 0 ? 0 : value > 127 ? 127 : value;

                if (_Upper != value)
                {
                    _Upper = value;

                    ValueUpper = (IntegraScales)value;

                    if(value < _Lower)
                    {
                        ValueLower = ValueUpper;
                    }
                }
            }
        }

        static KeyRangeSlider()
        {
            StyleManager.Initialize();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KeyRangeSlider), new FrameworkPropertyMetadata(typeof(KeyRangeSlider)));
        }

        public KeyRangeSlider()
        {
            LayoutTransform = new ScaleTransform(1, -1);
            ClipToBounds = true;

            Loaded += (s, e) => InitializeSlider();
            StyleManager.StyleChanged += (sender, e) => InvalidateVisual();

            MinWidth  = 50;
            MinHeight = 10;
            //MaxHeight = 10;

            Height = double.NaN;
            Width  = double.NaN;

            IsHitTestVisible = true;
        }

        public static readonly DependencyProperty ValueLowerProperty = DependencyProperty.Register(nameof(ValueLower), typeof(IntegraScales), typeof(KeyRangeSlider), new FrameworkPropertyMetadata(IntegraScales.C_, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValueChanged));

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            KeyRangeSlider slider = d as KeyRangeSlider;

            int oldValue = (int)(IntegraScales)e.OldValue;
            int newValue = (int)(IntegraScales)e.NewValue;

            if (oldValue == newValue)
                return;

            Debug.Print($"Update: {e.Property.Name}");
            //switch (slider.Index)
            //{
            //    case 0:
            //        if (newValue > (int)slider.ValueUpper)
            //            slider.ValueUpper = (IntegraScales)newValue;
            //        break;
            //    case 1:
            //        if (newValue < (int)slider.ValueLower)
            //            slider.ValueLower = (IntegraScales)newValue;
            //        break;
            //}

            slider.Update();
        }

        public static readonly DependencyProperty ValueUpperProperty = DependencyProperty.Register(nameof(ValueUpper), typeof(IntegraScales), typeof(KeyRangeSlider), new FrameworkPropertyMetadata(IntegraScales.G9, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValueChanged));



        public bool ShowLabels
        {
            get { return (bool)GetValue(ShowLabelsProperty); }
            set { SetValue(ShowLabelsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowLabels.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowLabelsProperty = DependencyProperty.Register(nameof(ShowLabels), typeof(bool), typeof(KeyRangeSlider), new PropertyMetadata(true));


        public IntegraScales ValueLower
        {
            get { return (IntegraScales)GetValue(ValueLowerProperty); }
            set { SetValue(ValueLowerProperty, value); }
        }

        public IntegraScales ValueUpper
        {
            get { return (IntegraScales)GetValue(ValueUpperProperty); }
            set { SetValue(ValueUpperProperty, value); }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            InitializeSlider();
        }

        private StreamGeometry GetHandle(HandleDirection direction, double x)
        {
            StreamGeometry geometry = new StreamGeometry();

            using(StreamGeometryContext ctx = geometry.Open())
            {
                var offset = ShowLabels ? 20 : 0;
                if (direction == HandleDirection.Left)
                {
                    ctx.BeginFigure(new Point(x + HANDLE_WIDTH, offset), false, false);
                    ctx.LineTo(new Point(x, offset), true, true);
                    ctx.LineTo(new Point(x, MaxHeight), true, true);
                    ctx.LineTo(new Point(x + HANDLE_WIDTH, MaxHeight), true, true);
                }
                else
                {
                    ctx.BeginFigure(new Point(x - HANDLE_WIDTH, offset), false, false);
                    ctx.LineTo(new Point(x, offset), true, true);
                    ctx.LineTo(new Point(x, MaxHeight), true, true);
                    ctx.LineTo(new Point(x - HANDLE_WIDTH, MaxHeight), true, true);
                }
            }

            geometry.Freeze();

            return geometry;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            
            var offset = ShowLabels ? 20 : 0;

            dc.DrawRectangle(System.Windows.Media.Brushes.Transparent, null, new Rect(0, 0, ActualWidth, ActualHeight));

            dc.DrawRectangle(Styles.GraphBackground, null, new Rect(0 + HANDLE_WIDTH, offset, ActualWidth - HANDLE_WIDTH * 2, ActualHeight));

            dc.DrawRectangle(Styles.GraphSelectedHighlight, null, new Rect(new Point(LowerX, offset), new Point(UpperX, ActualHeight)));

            if (Index == 0)
            {
                dc.DrawGeometry(null, Styles.GraphSelectedPen, GetHandle(HandleDirection.Right, LowerX));
            }
            else
            {
                dc.DrawGeometry(null, Styles.GraphBorderPen, GetHandle(HandleDirection.Right, LowerX));
            }

            if (Index == 1)
            {
                dc.DrawGeometry(null, Styles.GraphSelectedPen, GetHandle(HandleDirection.Left, UpperX));
            }
            else
            {
                dc.DrawGeometry(null, Styles.GraphBorderPen, GetHandle(HandleDirection.Left, UpperX));
            }

            if (MouseIndex == 0)
            {
                dc.DrawGeometry(null, Styles.GraphSelectedPen, GetHandle(HandleDirection.Right, LowerX));
            }
            else if (MouseIndex == 1)
            {
                dc.DrawGeometry(null, Styles.GraphSelectedPen, GetHandle(HandleDirection.Left, UpperX));
            }

            if (ShowLabels)
            {
                if (Index != -1)
                {
                    var label = string.Empty;

                    if (Index == 0)
                    {
                        label = $"{(string)TypeDescriptor.GetConverter(ValueLower).ConvertTo(ValueLower, typeof(string))}";
                    }
                    else
                    {
                        label = $"{(string)TypeDescriptor.GetConverter(ValueUpper).ConvertTo(ValueUpper, typeof(string))}";
                    }

                    if (!string.IsNullOrEmpty(label))
                    {
                        FormattedText text = new FormattedText(label, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(FontFamily, FontStyle, FontWeight, FontStretches.Normal), FontSize, Foreground, 1.0);
                        

                        Geometry geo = text.BuildGeometry(new Point(0, 0));

                        TransformGroup textTransform = new TransformGroup();

                        textTransform.Children.Add(new ScaleTransform(1, -1, 0, 0));
                        if(Index == 0)
                            textTransform.Children.Add(new TranslateTransform(LowerX - geo.Bounds.Width / 2, offset));
                        else
                            textTransform.Children.Add(new TranslateTransform(UpperX - geo.Bounds.Width / 2, offset));

                        geo.Transform = textTransform;

                        Rect bounds = geo.Bounds;
                        bounds.Inflate(2, 2);

                        //dc.DrawRectangle(Styles.GraphTint, null, bounds);
                        dc.DrawGeometry(Styles.GraphBorder, null, geo);
                    }
                }
            }
        }

        private void InitializeSlider()
        {
            SizeX = ActualWidth;
            SizeY = ActualHeight;

            MinX = HANDLE_WIDTH;
            MaxX = SizeX - HANDLE_WIDTH;

            Factor = (MaxX - MinX) / 127;

            _Lower = (int)ValueLower;
            _Upper = (int)ValueUpper;

            if (ShowLabels)
            {
                MinHeight = 30;
                MaxHeight = 30;
            }

            Update();
        }

        private void Update()
        {
            Debug.Print($"Lower = {ValueLower} | Upper = {ValueUpper}");
            _Lower = (int)ValueLower;
            _Upper = (int)ValueUpper;

            LowerX = (int)ValueLower * Factor + HANDLE_WIDTH;
            UpperX = (int)ValueUpper * Factor + HANDLE_WIDTH;

            InvalidateVisual();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            MouseIndex = -1;
            Point mouse = e.GetPosition(this);

            if (InvalidateIndex(mouse, out int index))
            {
                if(index == 0)
                {
                    MouseOffsetX = LowerX - mouse.X;
                }
                else if(index == 1)
                {
                    MouseOffsetX = UpperX - mouse.X;
                }

                Index = index;

                InvalidateVisual();
            }

            Focus();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            MouseIndex = -1;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            MouseIndex = -1;

            if (e.LeftButton == MouseButtonState.Pressed && Index != -1)
            {
                if(Index == 0)
                {
                    var x = e.GetPosition(this).X + MouseOffsetX - LowerX;

                    if (Math.Abs(x) >= Factor)
                    {
                        Lower += x / Factor;
                    }
                }
                else if(Index == 1)
                {
                    
                    var x = e.GetPosition(this).X + MouseOffsetX - UpperX;

                    if (Math.Abs(x) >= Factor)
                    {
                        Upper += x / Factor;
                    }
                }
            }
            else
            {
                if (InvalidateIndex(e.GetPosition(this), out int index))
                {
                    MouseIndex = index;
                }

                InvalidateVisual();
            }
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            Index = -1;
            InvalidateVisual();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (Index == -1)
                return;

            bool CTRL_KEY_DOWN = e.KeyboardDevice.IsKeyDown(Key.RightCtrl) || e.KeyboardDevice.IsKeyDown(Key.LeftCtrl);

            //int iFactor = CP[Index].IsInversed ? -1 : 1;

            switch (e.Key)
            {
                case Key.Left:
                    if (Index == 0)
                        Lower -= CTRL_KEY_DOWN ? 10 : 1;
                    else
                        Upper -= CTRL_KEY_DOWN ? 10 : 1;
                    break;
                case Key.Right:
                    if (Index == 0)
                        Lower += CTRL_KEY_DOWN ? 10 : 1;
                    else
                        Upper += CTRL_KEY_DOWN ? 10 : 1;
                    break;
                case Key.Home:
                    if (Index == 0)
                        Lower = 0;
                    else
                        Upper = Lower;
                    break;
                case Key.End:
                    if (Index == 0)
                        Lower = Upper;
                    else
                        Upper = 127;
                    break;

                case Key.Tab:

                    Index = Index == 0 ? 1 : 0;
                    InvalidateVisual();
                    break;

                case Key.Enter:
                case Key.Escape:

                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(this), null);
                    Index = -1;
                    InvalidateVisual();
                    break;
            }

            e.Handled = true;
        }

        private bool InvalidateIndex(Point mouse, out int index)
        {
            if (mouse.X >= 0 && mouse.X <= LowerX)
            {
                index = 0;
                return true;
            }
            else if (mouse.X >= UpperX && mouse.X <= SizeX)
            {
                index = 1;
                return true;
            }

            index = -1;
            return false;
        }
    }
}

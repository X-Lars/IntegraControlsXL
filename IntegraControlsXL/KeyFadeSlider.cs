using StylesXL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StylesXL.Extensions;
using System.Windows.Input;
using System.Diagnostics;
using System.Globalization;

namespace IntegraControlsXL
{
    public class KeyFadeSlider : Control
    {
        private const double HANDLE_WIDTH = 4;
        private double Factor = 0;
        private double SizeX;
        private double SizeY;
        private double MinX;
        private double MaxX;
        private double _ValueX;
        private bool IsInverted => Min > Max;
        private bool IsSelected = false;
        private bool IsMouseOver = false;

        private Brush FadeBrush = new LinearGradientBrush();
        
        static KeyFadeSlider()
        {
            StyleManager.Initialize();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KeyFadeSlider), new FrameworkPropertyMetadata(typeof(KeyFadeSlider)));
        }

        public KeyFadeSlider()
        {
            LayoutTransform = new ScaleTransform(1, -1);
            ClipToBounds = true;

            Loaded += (s, e) => InitializeSlider();
            StyleManager.StyleChanged += StyleChanged; ;

            MinWidth = 50;
            MinHeight = 10;
            //MaxHeight = 10;

            Height = double.NaN;
            Width = double.NaN;

            IsHitTestVisible = true;
        }

        private double ValueX
        {
            get => _ValueX;
            set
            {
                value = value < MinX ? MinX : value > MaxX ? MaxX : value;

                if (_ValueX != value)
                {
                    _ValueX = value;
                    
                    if(!IsInverted)
                    {
                        Value = (int)((value - MinX) / Factor);
                    }
                    else
                    {
                        Value = (int)((MaxX - value) / Factor);
                    }
                }
            }
        }

        private void StyleChanged(object sender, EventArgs e)
        {
            if(!IsInverted)
                FadeBrush = Styles.GraphSelectedHighlight.Fade(FadeDirection.Left);
            else
                FadeBrush = Styles.GraphSelectedHighlight.Fade(FadeDirection.Right);
            InvalidateVisual();
        }

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(int), typeof(KeyFadeSlider), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValueChanged, CoerceValue));

        private static object CoerceValue(DependencyObject d, object baseValue)
        {
            //return baseValue;
            KeyFadeSlider slider = d as KeyFadeSlider;
            int value = (int)baseValue;
            
            if (slider.IsInverted)
            {
//                Debug.Print($"Coerce: {(value < slider.Max ? slider.Max : value > slider.Min ? slider.Min : value)}");
                return value < slider.Max ? slider.Max : value > slider.Min ? slider.Min : value;
            }
            else
            {
//                Debug.Print($"Coerce: {(value < slider.Min ? slider.Min : value > slider.Max ? slider.Max : value)}");
                return value < slider.Min ? slider.Min : value > slider.Max ? slider.Max : value;
            }
        }

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Debug.Print($"Update: {e.Property.Name} = {(int)e.NewValue}");

            ((KeyFadeSlider)d).Update();
        }

        public int Min
        {
            get { return (int)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Min.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinProperty = DependencyProperty.Register(nameof(Min), typeof(int), typeof(KeyFadeSlider), new PropertyMetadata(0));



        public int Max
        {
            get { return (int)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Max.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxProperty = DependencyProperty.Register(nameof(Max), typeof(int), typeof(KeyFadeSlider), new PropertyMetadata(127));
        public bool ShowLabels
        {
            get { return (bool)GetValue(ShowLabelsProperty); }
            set { SetValue(ShowLabelsProperty, value); }
        }
        public static readonly DependencyProperty ShowLabelsProperty = DependencyProperty.Register(nameof(ShowLabels), typeof(bool), typeof(KeyFadeSlider), new PropertyMetadata(true));

        private void InitializeSlider()
        {
            SizeX = ActualWidth;
            SizeY = ActualHeight;

            MinX = HANDLE_WIDTH;
            MaxX = SizeX - HANDLE_WIDTH;
            

            if (IsInverted)
            {
                FadeBrush = Styles.GraphSelectedHighlight.Fade(FadeDirection.Right);

                Factor = (MaxX - MinX) / Min;
                
            }
            else
            {
                FadeBrush = Styles.GraphSelectedHighlight.Fade(FadeDirection.Left);

                Factor = (MaxX - MinX) / Max;
            }

            ValueX = MinX + (Value * Factor);


            if (ShowLabels)
            {
                MinHeight = 30;
                MaxHeight = 30;
            }

            //Debug.Print($"MinX: {MinX} | MaxX: {MaxX}");
            //Debug.Print($"Factor: {Factor}");
            Update();
        }

        private void Update()
        {
            if (!IsInverted)
                _ValueX = MinX + (Value * Factor);
            else
                _ValueX = MaxX - (Value * Factor);

            InvalidateVisual();
        }

        private StreamGeometry GetHandle(double x)
        {
            StreamGeometry geometry = new StreamGeometry();

            using (StreamGeometryContext ctx = geometry.Open())
            {
                var offset = ShowLabels ? 20 : 0;

                if (IsInverted)
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

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            SizeX = ActualWidth;
            SizeY = ActualHeight;

            MinX = HANDLE_WIDTH;
            MaxX = SizeX - HANDLE_WIDTH;

            if (Min > Max)
            {
                Factor = (MaxX - MinX) / Min;
            }
            else
            {
                Factor = (MaxX - MinX) / Max;
            }

            Update();
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            var offset = ShowLabels ? 20 : 0;

            // Background
            dc.DrawRectangle(System.Windows.Media.Brushes.Transparent, null, new Rect(0, 0, ActualWidth, ActualHeight));

            // Track Background
            dc.DrawRectangle(Styles.GraphBackground, null, new Rect(new Point(0 + HANDLE_WIDTH, offset), new Point(ActualWidth - HANDLE_WIDTH, ActualHeight)));

            // Track Value
            if(!IsInverted)
                dc.DrawRectangle(FadeBrush, null, new Rect(new Point(MinX, offset), new Point(ValueX, ActualHeight)));
            else
                dc.DrawRectangle(FadeBrush, null, new Rect(new Point(MaxX, offset), new Point(ValueX, ActualHeight)));

            if (IsSelected || IsMouseOver)
            {
                dc.DrawGeometry(null, Styles.GraphSelectedPen, GetHandle(ValueX));
                //dc.DrawLine(Styles.GraphSelectedPen, new Point(ValueX, offset-1), new Point(ValueX, ActualHeight + 1));
            }
            else
            {
                dc.DrawGeometry(null, Styles.GraphBorderPen, GetHandle(ValueX));
                //dc.DrawLine(Styles.GraphBorderPen, new Point(ValueX, offset-1), new Point(ValueX, ActualHeight+1));
            }

            //if (Index == 0)
            //{
            //    dc.DrawGeometry(null, Styles.GraphSelectedPen, GetHandle(HandleDirection.Right, LowerX));
            //}
            //else
            //{
            //    dc.DrawGeometry(null, Styles.GraphBorderPen, GetHandle(HandleDirection.Right, LowerX));
            //}

            //if (Index == 1)
            //{
            //    dc.DrawGeometry(null, Styles.GraphSelectedPen, GetHandle(HandleDirection.Left, UpperX));
            //}
            //else
            //{
            //    dc.DrawGeometry(null, Styles.GraphBorderPen, GetHandle(HandleDirection.Left, UpperX));
            //}

            //if (MouseIndex == 0)
            //{
            //    dc.DrawGeometry(null, Styles.GraphSelectedPen, GetHandle(HandleDirection.Right, LowerX));
            //}
            //else if (MouseIndex == 1)
            //{
            //    dc.DrawGeometry(null, Styles.GraphSelectedPen, GetHandle(HandleDirection.Left, UpperX));
            //}

            if (ShowLabels)
            {
                if (IsSelected)
                {
                    var label = $"{Value}";

                    if (!string.IsNullOrEmpty(label))
                    {
                        FormattedText text = new FormattedText(label, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(FontFamily, FontStyle, FontWeight, FontStretches.Normal), FontSize, Foreground, 1.0);


                        Geometry geo = text.BuildGeometry(new Point(0, 0));

                        TransformGroup textTransform = new TransformGroup();

                        textTransform.Children.Add(new ScaleTransform(1, -1, 0, 0));
                        textTransform.Children.Add(new TranslateTransform(ValueX - geo.Bounds.Width / 2, offset));

                        geo.Transform = textTransform;

                        Rect bounds = geo.Bounds;
                        bounds.Inflate(2, 2);

                        //dc.DrawRectangle(Styles.GraphTint, null, bounds);
                        dc.DrawGeometry(Styles.GraphBorder, null, geo);
                    }
                }
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            IsMouseOver = true;
            InvalidateVisual();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            IsMouseOver = false;
            InvalidateVisual();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            IsSelected = true;

            ValueX = e.GetPosition(this).X;

            Focus();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && IsSelected)
            {
                ValueX = e.GetPosition(this).X;
            }
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            IsSelected = false;
            InvalidateVisual();
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                ValueX += Factor;
            }
            else
            {
                ValueX -= Factor;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (!IsSelected)
                return;

            bool CTRL_KEY_DOWN = e.KeyboardDevice.IsKeyDown(Key.RightCtrl) || e.KeyboardDevice.IsKeyDown(Key.LeftCtrl);

            //int iFactor = CP[Index].IsInversed ? -1 : 1;

            switch (e.Key)
            {
                case Key.Left:
                        ValueX -= CTRL_KEY_DOWN ? Factor * 10 : Factor;
                    break;
                case Key.Right:
                    
                        ValueX += CTRL_KEY_DOWN ? Factor * 10 : Factor;
                    break;
                case Key.Home:

                    if (!IsInverted)
                        ValueX = MinX;
                    else
                        ValueX = MaxX;
                    break;
                case Key.End:
                    if (!IsInverted)
                        ValueX = MaxX;
                    else
                        ValueX = MinX;
                    break;

                case Key.Tab:
                case Key.Enter:
                case Key.Escape:

                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(this), null);
                    IsSelected = false;
                    InvalidateVisual();
                    break;
            }

            e.Handled = true;
        }
    }
}

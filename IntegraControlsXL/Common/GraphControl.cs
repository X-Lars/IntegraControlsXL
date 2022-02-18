using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using IntegraControlsXL.Extensions;
using System.Diagnostics;
using System;
using System.Globalization;
using StylesXL;

namespace IntegraControlsXL.Common
{
    public abstract class GraphControl : Control
    {
        /// <summary>
        /// Defines the radius for <see cref="ControlPoint"/> hit testing.
        /// </summary>
        public static readonly int CP_HIT_RADIUS = 5;

        /// <summary>
        /// Defines the distance for <see cref="ControlPoint"/> hit testing.
        /// </summary>
        public static readonly int CP_HIT_DISTANCE = CP_HIT_RADIUS * CP_HIT_RADIUS;

        /// <summary>
        /// The mouse X offset relative to the center of the selected <see cref="ControlPoint"/>.
        /// </summary>
        public double MouseOffsetX = 0;

        /// <summary>
        /// The mouse Y offset relative to the center of the selected <see cref="ControlPoint"/>
        /// </summary>
        public double MouseOffsetY = 0;

        /// <summary>
        /// Stores the number of X segments.
        /// </summary>
        private readonly int _SegmentsX;

        /// <summary>
        /// Stores the number of Y segments.
        /// </summary>
        private readonly int _SegmentsY;

        #region Constructor

        static GraphControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphControl), new FrameworkPropertyMetadata(typeof(GraphControl), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
            StyleManager.Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controlPoints"></param>
        /// <param name="segmentsX"></param>
        /// <param name="segmentsY"></param>
        public GraphControl(int controlPoints, int segmentsX, int segmentsY)
        {
            // Inverts the y axis of the graphs
            LayoutTransform = new ScaleTransform(1, -1);

            CP = new ControlPoint[controlPoints];

            for (int i = 0; i < controlPoints; i++)
            {
                CP[i] = new ControlPoint();
            }

            _SegmentsX = segmentsX;
            _SegmentsY = segmentsY;

            Loaded += (sender, e) => InitializeGraph();

            StyleManager.StyleChanged += (sender, e) => InvalidateVisual();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the array of control points.
        /// </summary>
        protected ControlPoint[] CP { get; private set; }

        /// <summary>
        /// Gets or sets the index specifying the selected control point.
        /// </summary>
        public int Index { get; private set; } = -1;

        /// <summary>
        /// Gets or sets the index specifying the mouse focused control point.
        /// </summary>
        public int MouseIndex { get; protected set; } = -1;

        /// <summary>
        /// Gets the grid segments collection.
        /// </summary>
        public Segments Segments { get; private set; } = new Segments();

        /// <summary>
        /// Gets the size of a single segment.
        /// </summary>
        public Size SegmentSize { get; private set; } = new Size();

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the base graph control.
        /// </summary>
        private void InitializeGraph()
        {
            InitSegments();
            Initialize();
            Update();
        }

        /// <summary>
        /// Sets the segment size X and Y by dividing the control's actual size by the number of segments.
        /// </summary>
        private void InitSegments()
        {
            Segments = new Segments(ActualWidth, ActualHeight, _SegmentsX, _SegmentsY);
            SegmentSize.X = ActualWidth  / _SegmentsX;
            SegmentSize.Y = ActualHeight / _SegmentsY;
        }

        /// <summary>
        /// Method to set initial values, invoked only once after the control is loaded.
        /// </summary>
        protected virtual void Initialize() { }

        /// <summary>
        /// Method to update <see cref="ControlPoint"/>s and other requirements before the <see cref="OnRender(DrawingContext)"/> method is invoked.
        /// </summary>
        /// <remarks>
        /// - Called when the control is loaded.<br/>
        /// - Called when a dependency property is changed that has its property change callback point to the <see cref="Update(DependencyObject, DependencyPropertyChangedEventArgs)"/> callback.<br/>
        /// - Called on the <see cref="OnMouseMove(MouseEventArgs)"/> event when a control point is selected and moved.<br/>
        /// - Called on the <see cref="OnRenderSizeChanged(SizeChangedInfo)"/> event.<br/><br/>
        /// <i>Invoke the base method when the control points are set, to re-render the control or invoke the <see cref="UIElement.InvalidateVisual"/> method.</i>
        /// </remarks>
        public virtual void Update()
        {
            InvalidateVisual();
        }

        /// <summary>
        /// Dependency property change callback to be invoked by properties associated with <see cref="ControlPoint"/>s to enable two-way binding.
        /// </summary>
        /// <param name="d">The <see cref="GraphControl"/> derived class containing the property that raised the event.</param>
        /// <param name="e">The event's associated data.</param>
        /// <remarks><i>
        /// Invokes the <see cref="Update"/> method to update and re-render the control.
        /// </i></remarks>
        protected static void Update(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GraphControl)d).Update();

            //Debug.Print($"{e.Property.Name}: {e.NewValue}");
        }

        public abstract void Render(DrawingContext dc);

        
        private bool InvalidateIndex(Point mouse, out int index)
        {
            for (int i = 0; i < CP.Length; i++)
            {
                if (mouse.Distance(CP[i]) < CP_HIT_DISTANCE)
                {
                    //Index = i;
                    index = i;
                    return true;
                }
            }

            //Index = -1;
            index = -1;
            return false;

        }

        #endregion

        #region Overrides : Control

        /// <summary>
        /// Handles the render size changed event by reinitializing the control.
        /// </summary>
        /// <param name="sizeInfo">The event's associated data.</param>
        protected sealed override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            InitializeGraph();
        }

        /// <summary>
        /// Handles rendering of the control.
        /// </summary>
        /// <param name="dc">The drawing context for rendering.</param>
        protected sealed override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            
            // Background
            dc.DrawRectangle(Styles.GraphBackground, null, new Rect(0, 0, ActualWidth, ActualHeight));

            // Grid lines
            //for (int x = 0; x < Segments.CountX; x++)
            //{
            //    for (int y = 0; y < Segments.CountY; y++)
            //    {
            //        dc.DrawRectangle(null, new Pen(Styles.GraphBorder, 0.1), Segments[x, y]);
            //    }
            //}

            Render(dc);


            if (Index != -1)
            {
                dc.DrawRectangle(Styles.GraphHighlight, null, new Rect(CP[Index].Limit.MinX, 0, CP[Index].Limit.SX, ActualHeight));
                dc.DrawEllipse(Styles.GraphSelected, null, CP[Index], CP_HIT_RADIUS, CP_HIT_RADIUS);
            }
            
            if (MouseIndex != -1)
            {
                dc.DrawEllipse(Styles.GraphHighlight, null, CP[MouseIndex], CP_HIT_RADIUS,CP_HIT_RADIUS);
            }

            if (Index != -1)
            {

                // The text to show inside the label
                FormattedText text = new FormattedText($"{CP[Index].Name}: {CP[Index].ValueX}" , CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(FontFamily, FontStyle, FontWeight, FontStretches.Normal), FontSize, Foreground, 1.0);
                
                // Convert the text to geometry to be able to get it's bounds
                Geometry geo = text.BuildGeometry(new Point(CP[Index].X, CP[Index].Y));

                // Mirror the text vertically
                TransformGroup transform = new TransformGroup();

                transform.Children.Add(new ScaleTransform(1, -1, 0, geo.Bounds.Y));
                transform.Children.Add(new TranslateTransform(geo.Bounds.Height, 0));
                if (CP[Index].X + geo.Bounds.Width > ActualWidth)
                {
                    transform.Children.Add(new TranslateTransform(-geo.Bounds.Width - geo.Bounds.Height, 0));
                }
                if (CP[Index].Y - geo.Bounds.Height < 0)
                {
                    transform.Children.Add(new TranslateTransform(0, geo.Bounds.Height));
                }
                else if(CP[Index].Y + geo.Bounds.Height > ActualHeight)
                {
                    transform.Children.Add(new TranslateTransform(0, -geo.Bounds.Height));
                }

                geo.Transform = transform;

                // Translate the text in -X so the cursor doesn't overlay
                //TranslateTransform translateX = new TranslateTransform(-geo.Bounds.Width / 2, geo.Bounds.Height);

                // Apply the transformations
                //dc.PushTransform(mirrorY);
                //dc.PushTransform(translateX);

                dc.DrawRectangle(Styles.GraphTint, null, new Rect(geo.Bounds.X , geo.Bounds.Y , geo.Bounds.Width, geo.Bounds.Height));
                dc.DrawGeometry(Styles.GraphHighlight, null, geo);
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            Cursor = Cursors.Arrow;
            Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
            MouseIndex = -1;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            MouseIndex = -1;
            Point mouse = e.GetPosition(this);

            if (InvalidateIndex(mouse, out int index))
            {
                Cursor = Cursors.None;

                MouseOffsetX = CP[index].X - mouse.X;
                MouseOffsetY = CP[index].Y - mouse.Y;

                Index = index;

                InvalidateVisual();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            MouseIndex = -1;

            if (e.LeftButton == MouseButtonState.Pressed && Index != -1)
            {
                var x = e.GetPosition(this).X + MouseOffsetX - CP[Index].X;
                var y = e.GetPosition(this).Y + MouseOffsetY - CP[Index].Y;

                if(Math.Abs(x) >= CP[Index]._FactorX)
                {
                    CP[Index].ValueX += x / CP[Index]._FactorX;
                }

                if(Math.Abs(y) >= CP[Index]._FactorY)
                {
                    CP[Index].ValueY += y / CP[Index]._FactorY;
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

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {

            if (Index == -1)
                return;

            bool CTRL_KEY_DOWN = e.KeyboardDevice.IsKeyDown(Key.RightCtrl) || e.KeyboardDevice.IsKeyDown(Key.LeftCtrl);

            switch (e.Key)
            {
                case Key.Up:    CP[Index].ValueY += CTRL_KEY_DOWN ? 10 : 1; break;
                case Key.Down:  CP[Index].ValueY -= CTRL_KEY_DOWN ? 10 : 1; break;
                case Key.Left:  CP[Index].ValueX -= CTRL_KEY_DOWN ? 10 : 1; break;
                case Key.Right: CP[Index].ValueX += CTRL_KEY_DOWN ? 10 : 1; break;

                case Key.PageDown: CP[Index].ValueY = CP[Index].MinY; break;
                case Key.PageUp:   CP[Index].ValueY = CP[Index].MaxY; break;
                case Key.Home:     CP[Index].ValueX = CP[Index].MinX; break;
                case Key.End:      CP[Index].ValueX = CP[Index].MaxX; break;


                case Key.Tab:

                    if (e.KeyboardDevice.IsKeyDown(Key.LeftShift) || e.KeyboardDevice.IsKeyDown(Key.RightShift) || e.KeyboardDevice.IsKeyToggled(Key.CapsLock))
                    {
                        // Select previous control point
                        if (Index > 0)
                            Index--;
                        else
                            Index = CP.Length - 1;
                    }
                    else
                    {
                        // Select next control point
                        if (Index < CP.Length - 1)
                            Index++;
                        else
                            Index = 0;
                    }
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

        #endregion

        

        
    }
}

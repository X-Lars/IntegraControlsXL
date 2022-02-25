using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using IntegraControlsXL.Extensions;
using System;
using System.Globalization;
using StylesXL;
using System.Diagnostics;

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
            ClipToBounds = true;

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

        /// <summary>
        /// Gets or sets wheter the control point values labels are rendered.
        /// </summary>
        public bool ShowValues { get; set; } = true;

        /// <summary>
        /// Gets or sets wheter the grid lines are rendered.
        /// </summary>
        public bool ShowGrid { get; set; } = false;

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

        public static object CoerceRound(DependencyObject d, object baseValue)
        {
            return Math.Round((double)baseValue);
        }

        /// <summary>
        /// Render routine for derived classes to draw it's background.
        /// </summary>
        /// <param name="dc"></param>
        /// <remarks><i>The second render layer, invoked directly after the graph's background rendering. </i></remarks>
        public virtual void RenderBackground(DrawingContext dc) { }

        /// <summary>
        /// Render routine for derived classes to draw it's graph.
        /// </summary>
        /// <param name="dc">The drawing context for rendering.</param>
        /// <remarks><i>The middle render layer, invoked directly after the <see cref="RenderBackground(DrawingContext)"/> method.</i></remarks>
        public abstract void RenderGraph(DrawingContext dc);

        /// <summary>
        /// Render routine for derived classes to overlay the rendered graph.
        /// </summary>
        /// <param name="dc"></param>
        /// <remarks><i>The last render layer.</i></remarks>
        public virtual void RenderOverlay(DrawingContext dc) { }

        /// <summary>
        /// Checks whether the mouse is over a control point.
        /// </summary>
        /// <param name="mouse">The current mouse position.</param>
        /// <param name="index">The index of the hovered control point.</param>
        /// <returns>True if the mouse is over a control point.</returns>
        private bool InvalidateIndex(Point mouse, out int index)
        {
            for (int i = 0; i < CP.Length; i++)
            {
                if (mouse.Distance(CP[i]) < CP_HIT_DISTANCE)
                {
                    index = i;
                    return true;
                }
            }

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
            
            // DRAW BACKGROUND
            dc.DrawRectangle(Styles.GraphBackground, null, new Rect(0, 0, ActualWidth, ActualHeight));

            // DRAW GRID
            if (ShowGrid)
            {
                for (int x = 0; x < Segments.CountX; x++)
                {
                    for (int y = 0; y < Segments.CountY; y++)
                    {
                        dc.DrawRectangle(null, Styles.GraphGridPen, Segments[x, y]);
                    }
                }
            }

            // DRAW CONTROL POINTS
            for (int i = 0; i < CP.Length; i++)
            {
                if(CP[i].IsVisble)
                    dc.DrawEllipse(Styles.GraphSelected, null, CP[i], 2, 2);
            }

            // DRAW CONSTRAINTS
            if(Index != -1 && CP[Index].IsVisble)
            {
                if(CP[Index].MinX != CP[Index].MaxX)
                {
                    dc.DrawLine(Styles.GraphConstraintPen, new Point(0, CP[Index].Y), new Point(Segments.SX, CP[Index].Y));
                }

                if (CP[Index].MinY != CP[Index].MaxY)
                {
                    dc.DrawLine(Styles.GraphConstraintPen, new Point(CP[Index].X, 0), new Point(CP[Index].X, Segments.SY));
                }
            }

            RenderBackground(dc);

            // RENDER DERIVED
            RenderGraph(dc);

            // DRAW SELECTED CONTROL POINT
            if (Index != -1 && CP[Index].IsVisble)
            {
                dc.DrawRectangle(Styles.GraphHighlight, null, new Rect(CP[Index].Limit.MinX, 0, CP[Index].Limit.SX, ActualHeight));
                dc.DrawEllipse(Styles.GraphSelected, null, CP[Index], CP_HIT_RADIUS, CP_HIT_RADIUS);
            }
            
            // DRAW MOUSE OVER CONTROL POINT
            if (MouseIndex != -1 && CP[MouseIndex].IsVisble)
            {
                dc.DrawEllipse(Styles.GraphHighlight, null, CP[MouseIndex], CP_HIT_RADIUS,CP_HIT_RADIUS);
            }

            // DRAW PROPERY VALUE(S)
            if (Index != -1 && CP[Index].IsVisble)
            {
                if (ShowValues)
                {
                    if (!string.IsNullOrEmpty(CP[Index].NameX))
                    {
                        FormattedText valueX = new FormattedText($"{CP[Index].NameX}: {CP[Index].GetValueX()}", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(FontFamily, FontStyle, FontWeight, FontStretches.Normal), FontSize, Foreground, 1.0);
                        Geometry geoX = valueX.BuildGeometry(new Point(0, 0));

                        TransformGroup transform = new TransformGroup();
                        transform.Children.Add(new ScaleTransform(1, -1, 0, 0));
                        transform.Children.Add(new TranslateTransform(CP[Index].X + geoX.Bounds.Height, CP[Index].Y + geoX.Bounds.Height));
                        geoX.Transform = transform;

                        dc.DrawGeometry(Styles.GraphTint, null, geoX);
                    }

                    if (!string.IsNullOrEmpty(CP[Index].NameY))
                    {
                        FormattedText valueY = new FormattedText($"{CP[Index].NameY}: {CP[Index].GetValueY()}", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(FontFamily, FontStyle, FontWeight, FontStretches.Normal), FontSize, Foreground, 1.0);
                        Geometry geoY = valueY.BuildGeometry(new Point(0, 0));

                        TransformGroup transform = new TransformGroup();
                        transform.Children.Add(new ScaleTransform(1, -1, 0, 0));
                        transform.Children.Add(new TranslateTransform(CP[Index].X - geoY.Bounds.Width / 2, CP[Index].Y + geoY.Bounds.Height * 3));
                        geoY.Transform = transform;

                        dc.DrawGeometry(Styles.GraphTint, null, geoY);
                    }
                }
            }

            // DRAW PROPERTY NAMES LABEL
            if(MouseIndex != -1 && CP[MouseIndex].IsVisble)
            {
                var label = string.Empty;
                var textX = CP[MouseIndex].NameX;
                var textY = CP[MouseIndex].NameY;

                if (!string.IsNullOrEmpty(textX))
                    label = $" X: {textX} ";

                if (!string.IsNullOrEmpty(textY))
                    label += $" Y: {textY} ";

                if(!string.IsNullOrEmpty(label))
                {
                    FormattedText text = new FormattedText(label, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface(FontFamily, FontStyle, FontWeight, FontStretches.Normal), FontSize, Foreground, 1.0);
                    Geometry geo = text.BuildGeometry(new Point(0, 0));

                    TransformGroup textTransform = new TransformGroup();

                    textTransform.Children.Add(new ScaleTransform(1, -1, 0, 0));
                    textTransform.Children.Add(new TranslateTransform(Segments.SX / 2 - geo.Bounds.Width / 2, geo.Bounds.Height * 2));

                    geo.Transform = textTransform;
                    
                    Rect bounds = geo.Bounds;
                    bounds.Inflate(2, 2);

                    dc.DrawRectangle(Styles.GraphTint, null, bounds);
                    dc.DrawGeometry(Styles.GraphHighlight, null, geo);
                }
            }

            RenderOverlay(dc);
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

            Focus();
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

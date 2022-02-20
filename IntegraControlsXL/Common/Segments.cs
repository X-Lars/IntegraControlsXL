using System;
using System.Diagnostics;
using System.Windows;

namespace IntegraControlsXL.Common
{
    /// <summary>
    /// Class representing a 2D rectangular dimension
    /// </summary>
    public abstract class Dimension
    {
        #region Constructor

        /// <summary>
        /// Creates and initializes a new <see cref="Dimension"/> instance.
        /// </summary>
        /// <param name="x">The x coördinate.</param>
        /// <param name="y">The y coördinate</param>
        /// <param name="sx">The width.</param>
        /// <param name="sy">The height.</param>
        public Dimension(double x, double y, double sx, double sy) 
        {
            X = x; 
            Y = y;
            
            SX = sx; 
            SY = sy; 
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the origin X coördinate.
        /// </summary>
        public double X { get; private set; }

        /// <summary>
        /// Gets or sets the orgin Y coördinate.
        /// </summary>
        public double Y { get; private set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public double SX { get; protected set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public double SY { get; protected set; }

        /// <summary>
        /// Gets the center X coördinate.
        /// </summary>
        public double CX => (X + SX) / 2;

        /// <summary>
        /// Gets the center Y coördinate.
        /// </summary>
        public double CY => (Y + SY) / 2;

        #endregion

        /// <summary>
        /// Implicitly assigns a <see cref="Dimension"/> instance to a <see cref="Rect"/>.
        /// </summary>
        /// <param name="instance">The instance to assign.</param>
        /// <remarks><i>
        /// If the instance is null an empty rectangle is returned.
        /// </i></remarks>
        public static implicit operator Rect(Dimension instance)
        {
            if (instance == null)
                return new Rect(0, 0, 0, 0);

            return new Rect(Math.Max(instance.X, 0), Math.Max(instance.Y, 0), Math.Max(instance.SX, 0), Math.Max(instance.SY, 0));
        }
    }

    /// <summary>
    /// Class representing a grid segment.
    /// </summary>
    public sealed class Segment : Dimension
    {
        public Segment(double x, double y, double sx, double sy) : base(x, y, sx, sy) { }
    }

    /// <summary>
    /// Class representing a grid of <see cref="Segment"/>s.
    /// </summary>
    public sealed class Segments : Dimension
    {
        #region Fields

        /// <summary>
        /// Stores the grid of <see cref="Segment"/>s.
        /// </summary>
        private readonly Segment[,] _Segments;

        /// <summary>
        /// Stores the size of a single <see cref="Segment"/> in pixels.
        /// </summary>
        private readonly Size _SegmentSize;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for internal use.
        /// </summary>
        internal Segments() : this(0, 0, 0, 0) { }

        /// <summary>
        /// Creates and initializes a new <see cref="Segments"/> grid.
        /// </summary>
        /// <param name="sx">The width of the grid in pixels.</param>
        /// <param name="sy">The height of the grid in pixels.</param>
        /// <param name="segmentsX">The number of horizontal segments.</param>
        /// <param name="segmentsY">The number of vertical segments.</param>
        /// <remarks><i>
        /// The rows are indexed from bottom to top.
        /// </i></remarks>
        public Segments(double sx, double sy, int segmentsX, int segmentsY) : base(0, 0, sx, sy)
        {
            double sizeX = sx / segmentsX;
            double sizeY = sy / segmentsY;

            _SegmentSize = new Size(sizeX, sizeY);
            _Segments    = new Segment[segmentsX, segmentsY];

            CountX = segmentsX;
            CountY = segmentsY;

            for (int x = 0; x < segmentsX; x++)
            {
                for (int y = 0; y < segmentsY; y++)
                {
                    _Segments[x, y] = new Segment(x * sizeX, y * sizeY, sizeX, sizeY);
                }
            }

            SX = segmentsX * _SegmentSize.X;
            SY = segmentsY * _SegmentSize.Y;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the size of a single <see cref="Segment"/> in pixels.
        /// </summary>
        public Size SegmentSize => _SegmentSize;

        /// <summary>
        /// Gets the <see cref="Segment"/> at the specified x and y index.
        /// </summary>
        /// <param name="x">The segment's X index.</param>
        /// <param name="y">The segment's Y index.</param>
        /// <returns>The <see cref="Segment"/> at the specified index.</returns>
        public Segment this[int x, int y] => _Segments[x, y];

        public int CountX { get; private set; }
        public int CountY { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a segment representing a column at the specified index.
        /// </summary>
        /// <param name="index">The <i>zero based</i> column index.</param>
        /// <returns>A segment with the dimension of the column at the specified index.</returns>
        public Segment Column(int index)
        {
            if (CountX == 0)
                return new Segment(0, 0, 0, 0);

            return new Segment(this[index, 0].X, this[index, 0].Y, this[index, 0].SX, SY);
        }

        /// <summary>
        /// Gets a segment representing a row at the specified index.
        /// </summary>
        /// <param name="index">The <i>zero based</i> row index.</param>
        /// <returns>A segment with the dimension of the row at the specified index.</returns>
        /// <remarks><i>
        /// The rows are indexed from bottom to top.
        /// </i></remarks>
        public Segment Row(int index)
        {
            if (CountY == 0)
                return new Segment(0, 0, 0, 0);

            return new Segment(this[0, index].X, this[0, index].Y, CountX * SX, this[0, index].SY);
        }

        #endregion

        public static implicit operator Rect(Segments instance)
        {
            if (instance == null)
                return new Rect();

            return new Rect(Math.Max(instance.X, 0), Math.Max(instance.Y, 0), Math.Max(instance.SX, 0), Math.Max(instance.SY, 0));
        }
    }
}

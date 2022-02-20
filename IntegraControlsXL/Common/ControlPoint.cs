using System;
using System.Diagnostics;
using System.Windows;

namespace IntegraControlsXL.Common
{
    // TODO: Linked offset / update without creating new

    /// <summary>
    /// Defines a point structure that can be constrained in movement.
    /// </summary>
    /// <remarks><i>
    /// - Implicitly assignable to a <see cref="Point"/>.<br/>
    /// </i></remarks>
    public class ControlPoint
    {
        #region Fields: Private

        /// <summary>
        /// Stores a reference to the parent object.
        /// </summary>
        private readonly GraphControl _Parent;

        /// <summary>
        /// Stores a reference to the property associated with the X value.
        /// </summary>
        private readonly DependencyProperty _PropertyX;

        /// <summary>
        /// Stores a reference to the property associated with the Y value.
        /// </summary>
        private readonly DependencyProperty _PropertyY;

        /// <summary>
        /// Stores the X value offset.
        /// </summary>
        private readonly double _OffsetX;

        /// <summary>
        /// Stores the Y value offset.
        /// </summary>
        private readonly double _OffsetY;

        /// <summary>
        /// Stores the X value.
        /// </summary>
        private double _ValueX;

        /// <summary>
        /// Stores the Y value.
        /// </summary>
        private double _ValueY;

        /// <summary>
        /// Stores the X scale factor.
        /// </summary>
        public readonly double _FactorX;

        /// <summary>
        /// Stores the Y scale factor.
        /// </summary>
        public readonly double _FactorY;

        /// <summary>
        /// Stores the X coördinate.
        /// </summary>
        private double _X = 0;

        /// <summary>
        /// Stores the Y coördinate.
        /// </summary>
        private double _Y = 0;

        #endregion

        #region Fields: Public

        /// <summary>
        /// Gets the control point's graph limits.
        /// </summary>
        private readonly Limit _Limit;

        /// <summary>
        /// Gets the minimal value of the property associated to the X axis.
        /// </summary>
        public readonly double MinX;

        /// <summary>
        /// Gets the maximal value of the property associated to the X axis.
        /// </summary>
        public readonly double MaxX;

        /// <summary>
        /// Gets the minimal value of the property associated to the Y axis.
        /// </summary>
        public readonly double MinY;

        /// <summary>
        /// Gets the maximal value of the property associated to the Y axis.
        /// </summary>
        public readonly double MaxY;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for internal use.
        /// </summary>
        internal ControlPoint() { IsVisble = false; }

        

        public ControlPoint(DependencyObject parent, DependencyProperty propertyX, DependencyProperty propertyY, double minX, double maxX, double minY, double maxY, Limit limit, double offsetX = 0, double offsetY = 0)
        {
            _Parent = (GraphControl)parent;
            
            _PropertyX = propertyX;
            _PropertyY = propertyY;

            _OffsetX = offsetX;
            _OffsetY = offsetY;

            _FactorX = limit.SX / Math.Abs(minX - maxX);
            _FactorY = limit.SY / Math.Abs(minY - maxY);

            if (propertyX != null)
                _ValueX = (double)_Parent.GetValue(propertyX);

            if (propertyY != null)
                _ValueY = (double)_Parent.GetValue(propertyY);

            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;

            _Limit = limit;

            if(limit.MinX == limit.MaxX)
            {
                X = limit.MinX;
            }
            else
            {
                X = limit.MinX + (_FactorX * _ValueX) + (_OffsetX * _FactorX);
            }

            if(limit.MinY == limit.MaxY)
            {
                Y = limit.MinY;
            }
            else
            {
                Y = limit.MinY + (_FactorY * _ValueY) + (_OffsetY * _FactorY);
            }

            //Debug.Print($"{this}");
        }

        #endregion

        #region Properties

        public string NameX => _PropertyX != null ? _PropertyX.Name : default;
        public string NameY => _PropertyY != null ? _PropertyY.Name : default;

        public bool IsVisble { get; set; } = true;
        
        public Limit Limit
        {
            get => _Limit ?? new Limit(0, 0, 0, 0);
        }

        /// <summary>
        /// Gets the value from control point's X axis.
        /// </summary>
        public double ValueX
        {
            get => _ValueX;
            set
            {
                if (_PropertyX != null)
                {
                    value = value < MinX ? MinX : value > MaxX ? MaxX : value;

                    if (_ValueX != value)
                    {
                        _Parent.SetValue(_PropertyX, (double)value);
                        _ValueX = value;
                        Debug.Print($"{_PropertyX.Name}: {value}");
                    }
                }
            }
        }

        /// <summary>
        /// Gets the value from the control point's Y axis.
        /// </summary>
        public double ValueY
        {
            get => _ValueY;
            set
            {
                if (_PropertyY != null)
                {
                    value = value < MinY ? MinY : value > MaxY ? MaxY : value;

                    if (_ValueY != value)
                    {
                        _Parent.SetValue(_PropertyY, (double)value);
                        _ValueY = value;
                        Debug.Print($"{_PropertyY.Name}: {value}");
                    }
                }
            }
        }

        /// <summary>
        /// Gets the X limit center. 
        /// </summary>
        public double CX
        {
            get { return (_Limit.MaxX - _Limit.MinX) / 2; }
        }

        /// <summary>
        /// Gets the Y limit center.
        /// </summary>
        public double CY
        {
            get { return (_Limit.MaxY - _Limit.MinY) / 2; }
        }

        /// <summary>
        /// Gets or sets the X value.
        /// </summary>
        /// <remarks><i>
        /// The associated callback is invoked on set.
        /// </i></remarks>
        public double X
        {
            get => _X;
            set
            {
                value = value < _Limit.MinX ? _Limit.MinX : value > _Limit.MaxX ? _Limit.MaxX : value;

                if (_X != value)
                {
                    _X = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Y value.
        /// </summary>
        /// <remarks><i>
        /// The associated callback is invoked on set.
        /// </i></remarks>
        public double Y
        {
            get => _Y;
            set
            {
                value = value < _Limit.MinY ? _Limit.MinY : value > _Limit.MaxY ? _Limit.MaxY : value;

                if (_Y != value)
                {
                    _Y = value;
                }
            }
        }

        #endregion
        
        #region Overloads

        /// <summary>
        /// Makes a <see cref="ControlPoint"/> implicitly assignable to a <see cref="Point"/>.
        /// </summary>
        /// <param name="instance">The instance to assign.</param>
        public static implicit operator Point(ControlPoint instance)
        {
            if (instance == null)
                return new Point(0, 0);

            return new Point(instance.X, instance.Y);
        }

        #endregion

        #region Overrides: Object

        /// <summary>
        /// Returns a string representation of the current <see cref="ControlPoint"/>.
        /// </summary>
        /// <returns>A string that represents the current <see cref="ControlPoint"/>.</returns>
        public override string ToString()
        {
            return $"X:{X}, Y:{Y} | Min X:{_Limit.MinX}, Max X:{_Limit.MaxX}, Val X: {ValueX} | Min Y:{_Limit.MinY}, Max Y:{_Limit.MaxY}, Val Y:{ValueY}";
        }

        #endregion
    }

    /// <summary>
    /// Defines a <see cref="ControlPoint"/> constrained to the X axis that can move within the specified limit.
    /// </summary>
    public class ControlPointX : ControlPoint
    {
        public ControlPointX(DependencyObject parent, DependencyProperty property, double min, double max, LimitX limit, double offset = 0) :
            base(parent, property, null, min, max, 0, 0, limit, offset)
        { }

    }

    /// <summary>
    /// Defines a <see cref="ControlPoint"/> constrained to the T axis that can move within the specified limit.
    /// </summary>
    public class ControlPointY : ControlPoint
    {
        public ControlPointY(DependencyObject parent, DependencyProperty property, double min, double max, LimitY limit, double offset = 0) :
            base(parent, null, property, 0, 0, min, max,limit, 0, offset)
        { }

    }

}

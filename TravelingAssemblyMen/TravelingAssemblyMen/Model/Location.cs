using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelingAssemblyMen.Model
{
    public class Location : IEquatable<Location>
    {
        private Position _position;

        #region CoordinateBoundaries
        const float rightHorizontalBound = 50;
        const float leftHorizontalBound = -50;
        const float upperVerticalBound = 50;
        const float lowerVerticalBound = -50;
        #endregion

        #region Constructors
        public Location(Double X, Double Y)
        {
            this._X = X;
            this._Y = Y;
        }

        public Location(PointF position) : this(position.X, position.Y) { }
        #endregion

        #region CheckedCoordinates
        private Double _X
        {
            get { return Math.Round(_position.x, 1); }
            set
            {
                if (value > rightHorizontalBound || value < leftHorizontalBound)
                {
                    throw new ArgumentOutOfRangeException("The Customers Positions are out of bounds.");
                }

                _position.x = value;
            }
        }
        private Double _Y
        {
            get { return Math.Round(_position.y, 1); }
            set
            {
                if (value > upperVerticalBound || value < lowerVerticalBound)
                {
                    throw new ArgumentOutOfRangeException("The Customers Positions are out of bounds.");
                }

                _position.y = value;
            }
        } 
        #endregion

        public Double DistanceTo(Location other)
        {
            return DistanceTo(other._X, other._Y);
        }

        public Double DistanceTo(Double x, Double y)
        {
            Double diffX = _X - x;
            Double diffY = _Y - y;

            return Math.Sqrt(Math.Pow(diffX, 2) + Math.Pow(diffY, 2));
        }

        #region Graphics
        public void Draw(Graphics graphics, Position origin, Double pixelsPerKilometer)
        {
            Draw(graphics, origin, pixelsPerKilometer, Color.Black);
        }

        public void Draw(Graphics graphics, Position origin, Double pixelsPerKilometer, Color color)
        {
            graphics.FillEllipse(new SolidBrush(color), (float)(_X * pixelsPerKilometer + origin.x - 4), (float)(_Y * pixelsPerKilometer + origin.y - 4), 8, 8);
        }

        public void DrawLineTo(Location nextCustomer, Graphics graphics, Position origin, Double pixelsPerKilometer)
        {
            Point pointThisCustomer = new Point(Convert.ToInt32(Math.Round(_X * pixelsPerKilometer + origin.x)), Convert.ToInt32(Math.Round(_Y * pixelsPerKilometer + origin.y)));
            Point pointNextCustomer = new Point(Convert.ToInt32(Math.Round(nextCustomer._X * pixelsPerKilometer + origin.x)), Convert.ToInt32(Math.Round(nextCustomer._Y * pixelsPerKilometer + origin.y)));
            
            graphics.DrawLine(new Pen(Color.Black), pointThisCustomer, pointNextCustomer);
        } 
        #endregion

        #region Equatable
        public override int GetHashCode()
        {
            return (Int32)(this._X + this._Y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Location))
            {
                return false;
            }

            Location comparate = obj as Location;

            return this._X == comparate._X && this._Y == comparate._Y;
        }

        public bool Equals(Location comparate)
        {
            return this._X == comparate._X && this._Y == comparate._Y;
        }
        #endregion
    }
}

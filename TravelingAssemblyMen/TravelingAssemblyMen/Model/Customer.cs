using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelingAssemblyMen.Model
{
    public class Customer
    {
        private PointF _position;

        #region CoordinateBoundaries
        const float rightHorizontalBound = 50;
        const float leftHorizontalBound = -50;
        const float upperVerticalBound = 50;
        const float lowerVerticalBound = -50;
        #endregion

        #region Constructors
        public Customer(float X, float Y)
        {
            this._position = new PointF();
            this._X = X;
            this._Y = Y;
        }

        public Customer(PointF position) : this(position.X, position.Y) { }
        #endregion

        #region CheckedCoordinates
        private float _X
        {
            get { return _position.X; }
            set
            {
                if (value > rightHorizontalBound || value < leftHorizontalBound)
                {
                    throw new ArgumentOutOfRangeException("The Customers Positions are out of bounds.");
                }

                _position.X = value;
            }
        }
        private float _Y
        {
            get { return _position.Y; }
            set
            {
                if (value > upperVerticalBound || value < lowerVerticalBound)
                {
                    throw new ArgumentOutOfRangeException("The Customers Positions are out of bounds.");
                }

                _position.Y = value;
            }
        } 
        #endregion

        public Double Distance(Customer other)
        {
            return Distance(other._position);
        }

        public Double Distance(PointF point)
        {
            float diffX = this._X - point.X;
            float diffY = this._Y - point.Y;

            return Math.Sqrt(Math.Pow(diffX, 2) + Math.Pow(diffY, 2));
        }

        public override int GetHashCode()
        {
            return (Int32)(this._X + this._Y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is  Customer))
            {
                return false;
            }

            Customer comparate = obj as Customer;

            return this._X == comparate._X && this._Y == comparate._Y;
        }
    }
}

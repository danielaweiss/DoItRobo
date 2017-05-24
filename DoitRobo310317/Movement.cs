using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoitRobo310317
{
    class Movement
    {
        public int X { get; set; }
        public int Y { get; set; }
        public float Angle { get; set; }

        public Movement(int X, int Y, float Angle)
        {
            this.X = X;
            this.Y = Y;
            this.Angle = Angle;
        }

        public override string ToString()
        {
            return String.Format("X {0} Y {1} Angle {2:0.00}°", this.X, this.Y, this.Angle);
        }

        //TODO Koordinatentransformation

    }
}

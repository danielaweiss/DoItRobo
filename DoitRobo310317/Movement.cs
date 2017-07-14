using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;

namespace DoitRobo310317
{
    class Movement
    {
        public string ObjectType { get; set; }
 
        public int Frame { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public float Angle { get; set; }

        public Mat objectmask { get; set; }

        public Movement(string ObjectType, int X, int Y, float Angle)
        {
            this.ObjectType = ObjectType;
            this.X = X;
            this.Y = Y;
            this.Angle = Angle;
        }

        public override string ToString()
        {
            return String.Format("{0}: X {1} Y {2} Angle {3:0.00}° Frame: {4}", this.ObjectType, this.X, this.Y, this.Angle, this.Frame);
        }

        //TODO Koordinatentransformation

    }
}

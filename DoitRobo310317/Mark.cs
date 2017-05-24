using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DoitRobo310317
{
    public class Mark
    {
        public double Cxitem { get; set; }

        public double Cyitem { get; set; }

        public int Indexitem { get; set; }

        public Point[]  Corners { get; set; }

        public static Mark operator +(Mark a, Mark b)
        {
            Mark result = new Mark();
            result.Cxitem = a.Cxitem + b.Cxitem;
            result.Cyitem = a.Cyitem + b.Cyitem;
            return result;
        }

        public static Mark operator -(Mark a, Mark b)
        {
            Mark result = new Mark();
            result.Cxitem = a.Cxitem - b.Cxitem;
            result.Cyitem = a.Cyitem - b.Cyitem;
            return result;
        }
        public static Mark operator *(Mark a, double variable)
        {
            Mark result = new Mark();
            result.Cxitem = a.Cxitem * variable;
            result.Cyitem = a.Cyitem * variable;
            return result;
        }
        public static Mark operator *(double variable, Mark a)
        {
            Mark result = new Mark();
            result.Cxitem = a.Cxitem * variable;
            result.Cyitem = a.Cyitem * variable;
            return result;
        }
        public static Mark operator +(double variable, Mark a)
        {
            Mark result = new Mark();
            result.Cxitem = a.Cxitem + variable;
            result.Cyitem = a.Cyitem + variable;
            return result;
        }
        public static Mark operator +(Mark a, double variable)
        {
            Mark result = new Mark();
            result.Cxitem = a.Cxitem + variable;
            result.Cyitem = a.Cyitem + variable;
            return result;
        }

        public override string ToString()
        {
            return "X: " + this.Cxitem + "Y: " + this.Cyitem;
        }

    }
}

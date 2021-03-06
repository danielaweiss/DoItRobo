﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;

namespace DoitRobo310317
{
    public class RobotMovement
    {
        public enum MovementTyp
        {
            REACH, GRASP, MOVE, RELEASE, ROTATE
        }

        public RobotMovement()
        {
            this.typ = new List<MovementTyp>();
            this.frameNumber = -1;
        }

        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public float a { get; set; }
        public float b { get; set; }
        public float c { get; set; }

        public bool tool { get; set; }

        public long timestamp { get; set; }

        public List<MovementTyp> typ { get; set; }
              
        public Mat imagesource { get; set; }
        public int frameNumber { get; set; }

        private static String template = 
@"<Sen Type=""DoIt"">
    <Estr>DoIt</Estr>
    <RKorr X=""{0:F4}"" Y=""{1:F4}"" Z=""{2:F4}"" A=""{3:F4}"" B=""{4:F4}"" C=""{5:F4}"" />
    <DiO>{6}</DiO>
    <IPOC>{7}</IPOC>
</Sen>";

        public String convert()
        {
            String converted = null;
            converted  = String.Format(template, x,y,z,a,b,c,tool, timestamp);
            return converted; 
        }

        

    }
}

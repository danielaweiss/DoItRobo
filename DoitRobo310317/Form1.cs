using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Speech.Recognition;
using ZXing;
using ZXing.Common;
using ZXing.Multi;
using ZXing.QrCode;
using Emgu.CV.CvEnum;

namespace DoitRobo310317
{

    public partial class Form1 : Form
    {
        // für Sprachsteuerung- allgemeine Variable
        Boolean start = false;
        //hat plötzlich nicht mehr funktioniert :o 
        //private Reader reader = new ByQuadrantReader(new QRCodeReader());
        private Reader reader = new QRCodeReader();


        private IDictionary<DecodeHintType, object> decoderhints = new Dictionary<DecodeHintType, object>();

        private SpeechRecognitionEngine SR;

        private VideoCapture video_capture = null;

        private List<Movement> movements = new List<Movement>();

        private int frameCount = 0;

        private bool live = false;

        private bool qrCodeVisible = false;

        private bool objectConture = false;

        private String objectColor = "";

        private bool gloveRed = false;

        private bool gloveGreen = false;

        private long captureCount = 0;


        Form2 classifierForm = new Form2();

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
           
            tbDebug.Text = null;
            
            decoderhints.Add(DecodeHintType.POSSIBLE_FORMATS, BarcodeFormat.QR_CODE);
            decoderhints.Add(DecodeHintType.TRY_HARDER, true);
            


            /*
            * Sprachsteuerung
            */
            SR = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("de-DE"));
            SR.SetInputToDefaultAudioDevice();
            Choices Commands = new Choices();

            Commands.Add("Manny starte die Aufnahme");
            Commands.Add("Manny beende die Aufnahme");
            //weitere Befehle hier eingeben

            GrammarBuilder GB = new GrammarBuilder(Commands); // die Befehle mit einem GrammerBuilder laden
            Grammar CommandGrammar = new Grammar(GB); // eine Grammatik über den GrammarBuilder erstellen
            SR.LoadGrammarAsync(CommandGrammar); // die Grammatik laden
            SR.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(CommandRecognized); // Funktion zur Behandlung des Ereignisses
            //SR.Enabled = true;
            SR.RecognizeAsync(RecognizeMode.Multiple);
            lbStatus.ForeColor = Color.Red;
            // Application.Idle += processFrame;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void processFrame(object sender, EventArgs e)
        {
            if(video_capture == null)
            {
                return;
            }
            // Aktuelles Bild -- Ab hier beginnt die Auswertung und die Erkennung des QR Codes
            Emgu.CV.Mat image = new Mat();
            video_capture.Retrieve(image);

            if (start == true)
            {
                captureCount++;
            }

            if (image.IsEmpty && !live && checkBoxLoop.Checked)//
            {
                video_capture = new Emgu.CV.VideoCapture(openFileDialog.FileName);
                return;
            }
            else if(image.IsEmpty)
            {
                tbDebug.Text = "Ende";
                return;
            }

            Mat inputimage = image.Clone();
            Mat outputimage = image.Clone();
            //Einmal QR Code dektieren!
            if (String.IsNullOrEmpty(objectColor))
            {
                detectQrCode(inputimage);
                imageBox1.Image = inputimage;
                return;
            }


            Movement objectMovement = TrackObject(inputimage,outputimage);
            Tuple<Movement, Movement> gloveMovement = gloveRecognized(inputimage, outputimage);

            this.imageBox1.Image = outputimage; //Damit wir immer was sehen können 

            //CvInvoke.Imshow("Test", image);

            if (qrCodeVisible & objectConture)
            {
                this.lbStatus.ForeColor = Color.Green;
            } else
            {
                this.lbStatus.ForeColor = Color.Red;
            }

            if (objectMovement != null)
            {
                CvInvoke.PutText(outputimage, "Position: X:" + objectMovement.X + " Y:" + objectMovement.Y, new System.Drawing.Point(10, 50), Emgu.CV.CvEnum.FontFace.HersheyComplex, 0.5, new Bgr(0, 0, 0).MCvScalar);
            }

            if (start)
            {
                int frameNumber = frameCount++;

                if (objectMovement != null)
                {
                    objectMovement.Frame = frameNumber;
                    this.movements.Add(objectMovement);
                }
                if (gloveMovement == null) { return; } //DANI CODE
                Movement red = gloveMovement.Item1;
                Movement green = gloveMovement.Item2;
                if(red != null)
                {
                    red.Frame = frameNumber;
                    this.movements.Add(red);
                }
                if(green != null)
                {
                    green.Frame = frameNumber;
                    this.movements.Add(green);
                }

                if (objectMovement != null && red != null && green != null)
                {
                    Movement picture = new Movement("image");
                    picture.Frame = frameNumber;
                    Mat resizedoutput= new Mat((int) outputimage.Rows/4, (int) outputimage.Cols/4, outputimage.Depth, outputimage.NumberOfChannels);
                    CvInvoke.PyrDown(outputimage, resizedoutput);
                    picture.imagesource = resizedoutput;
                    this.movements.Add(picture);
                }
            }
            
            // image.Dispose();
        }

        private Movement TrackObject(Mat inputimage, Mat outputimage)
        {
            Movement detectedMovement = new Movement("", -1, -1, -0.0f);


            if (!String.IsNullOrEmpty(objectColor))
            {
                // Colortracking
                Movement m = trackColored(inputimage, objectColor, outputimage);
                if(m != null)
                {
                    detectedMovement.X = m.X;
                    detectedMovement.Y = m.Y;
                    detectedMovement.objectmask = m.objectmask;
                }
            }

            // TODO der QR Code wird jetzt nur einmal erkannt pro Szene
            // qrCodeVisible = false;
            Emgu.CV.Mat gray = new Mat();
            CvInvoke.CvtColor(inputimage, gray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
            


            //Bild Thresholden (damit Unterschiede besser erkannt werden)
                          
            CvInvoke.AdaptiveThreshold(gray, gray, 255, AdaptiveThresholdType.MeanC, ThresholdType.BinaryInv, 25, 7);//25
            //CvInvoke.Threshold(gray, gray, 100, 170, ThresholdType.Binary);//100,170
            qrBox.Image = gray;
            

            //CvInvoke.MedianBlur(gray, gray, 5);
            //CvInvoke.GaussianBlur(gray, gray, new Size(5, 5), 5);
            Mat scaled = new Mat(gray.Rows / 2, gray.Cols / 2, Emgu.CV.CvEnum.DepthType.Cv8S, 3);
            //CvInvoke.PyrDown(gray, scaled);
           //CvInvoke.PyrUp(scaled, gray);

            Mat edges = new Mat();
            CvInvoke.Canny(gray, edges, 50, 150, 3); //3 muss bleiben, wurde ausgetestet, alles andere ist zu genau
            //CvInvoke.Erode(edges, edges, null, new Point(-1, -1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);

           // qrBox.Image = edges;
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();

            int[,] hierachy = CvInvoke.FindContourTree(edges, contours, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

            Mat contoursDebug = new Mat(inputimage.Rows, inputimage.Cols, Emgu.CV.CvEnum.DepthType.Cv32F, 3);
            CvInvoke.DrawContours(contoursDebug, contours, -1, new MCvScalar(255, 255, 255), 1);
            //qrBox.Image = contoursDebug;


            List<Mark> markerslist = new List<Mark>();

            for (int i = 0; i < contours.Size; i++)
            {
                VectorOfPoint contour = contours[i];
                if (CvInvoke.ContourArea(contour) < 100)
                {
                    continue;
                }

                var moments = CvInvoke.Moments(contour);

                if (moments.M00 > 0)
                {
                    int cx = Convert.ToInt32(moments.M10 / moments.M00);
                    int cy = Convert.ToInt32(moments.M01 / moments.M00);

                    var epsilon = 0.1 * CvInvoke.ArcLength(contour, true);
                    VectorOfPoint approx = new VectorOfPoint();
                    CvInvoke.ApproxPolyDP(contour, approx, epsilon, true);

                    if (approx.Size == 4)
                    {
                        int k = i;
                        int nestedContoursCount = 0;
                        while (hierachy[k, 2] != -1)
                        {
                            k = hierachy[k, 2];
                            nestedContoursCount++;
                        }

                        if (hierachy[k, 2] != -1)
                        {
                            nestedContoursCount++;
                        }

                        if (nestedContoursCount >= 5)
                        {
                            // Hier sind cx,cy und i wichtig ....
                            Mark marker = new Mark();
                            marker.Cxitem = cx;
                            marker.Cyitem = cy;
                            marker.Indexitem = i;

                            marker.Corners = findCorners(contours[i]);

                            markerslist.Add(marker);


                            MCvScalar color = new MCvScalar(0, 12, 255);
                            CvInvoke.DrawContours(outputimage, contours, i, color, 2);
                            CvInvoke.Circle(outputimage, new Point(cx, cy), 1, color, 2);
                        }
                    }
                }

            }




            // Hier werden alle Marker ausgewertet
            if (markerslist.Count >= 3)
            {
                Mark markerA = null;
                Mark markerB = null;
                Mark markerC = null;

                double AB = distance(markerslist[0], markerslist[1]);
                double BC = distance(markerslist[1], markerslist[2]);
                double AC = distance(markerslist[0], markerslist[2]);
                Mark cp = new Mark();
                double lineAngle = 0;
                if (AB > BC && AB > AC)
                {
                    CvInvoke.Line(outputimage, coordinatestopoints(markerslist[0]), coordinatestopoints(markerslist[1]), new Bgr(Color.Green).MCvScalar, 2);
                    lineAngle = angle(markerslist[0], markerslist[1]);
                    cp = center(markerslist[0], markerslist[1]);
                    markerA = markerslist[2];
                    markerB = markerslist[0];
                    markerC = markerslist[1];
                    //
                }
                if (AC > AB && AC > BC)
                {
                    CvInvoke.Line(outputimage, coordinatestopoints(markerslist[0]), coordinatestopoints(markerslist[2]), new Bgr(Color.Blue).MCvScalar, 2);
                    lineAngle = angle(markerslist[0], markerslist[2]);
                    cp = center(markerslist[0], markerslist[2]);
                    markerA = markerslist[1];
                    markerB = markerslist[2];
                    markerC = markerslist[0];
                }
                if (BC > AB && BC > AC)
                {
                    CvInvoke.Line(outputimage, coordinatestopoints(markerslist[1]), coordinatestopoints(markerslist[2]), new Bgr(Color.Red).MCvScalar, 2);
                    lineAngle = angle(markerslist[1], markerslist[2]);
                    cp = center(markerslist[1], markerslist[2]);
                    markerA = markerslist[0];
                    markerB = markerslist[1];
                    markerC = markerslist[2];
                }

                //Falls Markererkennung nicht richtig ist, kann das sonst nicht berechnet werden
                if (markerA == null)
                {
                   return null;
                }

                Point[] cornercandiates = new Point[12];
                Array.Copy(markerA.Corners, 0, cornercandiates, 0, markerA.Corners.Length);
                Array.Copy(markerB.Corners, 0, cornercandiates, 4, markerA.Corners.Length);
                Array.Copy(markerC.Corners, 0, cornercandiates, 8, markerA.Corners.Length);

                cornercandiates = findFarest(cornercandiates, cp, 3);
                // hier weiter machen
                for (int i = 0; i < cornercandiates.Length; i++)
                {
                    Point p = cornercandiates[i];
                    CvInvoke.Circle(outputimage, p, 7, new MCvScalar(255, 191, 0), 2, Emgu.CV.CvEnum.LineType.AntiAlias);

                }

                // 4. Punkt berechnen (ohne Eckerl)
                Point[] corners = new Point[4];
                Array.Copy(cornercandiates, 0, corners, 0, 3);

                Point cpA = findFarest(markerA.Corners, cp, 1)[0];
                Mark aPoint = new Mark();
                aPoint.Cxitem = cpA.X;
                aPoint.Cyitem = cpA.Y;

                double missingdistance = distance(aPoint, cp);

                Mark missingPointM = new Mark();
                missingPointM = cp + (aPoint - cp) * (-1);

                Point missingPointP = coordinatestopoints(missingPointM);

                CvInvoke.Circle(outputimage, missingPointP, 7, new MCvScalar(255, 191, 0), 9, Emgu.CV.CvEnum.LineType.AntiAlias);

                corners[3] = missingPointP;

                int width = 480;
                int height = 480;

                Mat dst = new Mat(width, height, Emgu.CV.CvEnum.DepthType.Cv8U, 3);
                PointF[] dstPoints = new PointF[4];
                dstPoints[0] = new PointF(0, 0);
                dstPoints[1] = new PointF(480, 0);
                dstPoints[2] = new PointF(0, 480);
                dstPoints[3] = new PointF(480, 480);

                Point[] cornerssort = sortPoints(corners);


                int offset = 15;

                //cornerssort[3] = new PointF(cornerssort[2].X, cornerssort[1].Y);

                cornerssort[0].X = cornerssort[0].X - offset;
                cornerssort[0].Y = cornerssort[0].Y - offset;

                cornerssort[1].X = cornerssort[1].X - offset;
                cornerssort[1].Y = cornerssort[1].Y + offset;

                cornerssort[2].X = cornerssort[2].X + offset;
                cornerssort[2].Y = cornerssort[2].Y - offset;

                cornerssort[3].X = cornerssort[3].X + offset;
                cornerssort[3].Y = cornerssort[3].Y + offset;




                Mat transformMat = CvInvoke.GetPerspectiveTransform(convertToPointF(cornerssort), dstPoints);
                CvInvoke.WarpPerspective(inputimage, dst, transformMat, new Size(480, 480));



                /*
                * Winkelberechnung
                */
                double h = threePointdist(markerC, markerB, markerA);
                double s = slope(markerB, markerC);


                // In welchen Quadranten befindet sich Marker A!!
                Orientation orientation = Orientation.none;
                double correction = 0;
                if (h < 0 && s < 0)
                {
                    orientation = Orientation.OL;
                    if (lineAngle < -45)
                    {
                        correction = 405;
                    }
                    else
                    {
                        correction = 45;
                    }


                }
                else if (h < 0 && s > 0)
                {
                    orientation = Orientation.OR;
                    correction = 45;
                }
                else if (h > 0 && s < 0)
                {
                    orientation = Orientation.UR;
                    correction = 225;
                }
                else if (h > 0 && s >= 0)
                {
                    orientation = Orientation.UL;
                    correction = 225;
                }


                double rotationAngle = lineAngle + correction;

                tbDebug.AppendText("\r\n Winkel korrigiert: " + Math.Round(((lineAngle + correction)), 2).ToString());
                tbDebug.AppendText("\r\n Orientation:" + orientation.ToString());
                tbDebug.AppendText("\r\n Winkel unkorrigiert: " + Math.Round((lineAngle), 2).ToString());
                tbDebug.AppendText("\r\n h: " + Math.Round(h, 2));
                tbDebug.AppendText("\r\n Steigung: " + Math.Round(s, 2));
                tbDebug.AppendText("\r\n Rotation: " + Math.Round(rotationAngle, 2));

                // für Robotersteuerung Winkel in eine Liste
                List<double> angleList = new List<double>();

                if (start == true)
                {
                    angleList.Add(lineAngle);
                }

 
                Mark cptemp = new Mark();
                cptemp.Cxitem = cp.Cxitem;
                cptemp.Cyitem = 0;

                Mat temp = inputimage.Clone();


                //CvInvoke.PutText(image,"Winkel: " + Math.Round((lineAngle),2).ToString(), new System.Drawing.Point(10, 80), Emgu.CV.CvEnum.FontFace.HersheyComplex,1.0,new Bgr(0, 0, 0).MCvScalar);
                CvInvoke.Circle(outputimage, coordinatestopoints(cp), 5, new Bgr(0, 0, 255).MCvScalar);

                this.imageBoxQr.Image = dst;

                if(detectedMovement.X < 0)
                {
                    detectedMovement.X = (int)cp.Cxitem;
                }
                if (detectedMovement.Y < 0)
                {
                    detectedMovement.Y = (int)cp.Cyitem;
                }

                detectedMovement.Angle = (float)(lineAngle+correction);

                return detectedMovement;
                
            }
            
            return null;
        }
        /*************************
         * Objekttracking ahand der Farbe
         * *************************/
        private Movement trackColored(Mat inputimage, string objectColor, Mat outputimage)
        {
            Mat work = inputimage.Clone();

            CvInvoke.MedianBlur(work, work, 15);
            CvInvoke.GaussianBlur(work, work, new Size(5, 5), 5);

            // konvertieren in HSV FUll (0-360°)
            Mat hsv = new Mat();
            CvInvoke.CvtColor(work, hsv, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);

            Color rgbColor = Color.FromArgb(Convert.ToInt32(objectColor.Substring(0, 2),16), Convert.ToInt32(objectColor.Substring(2, 2),16), Convert.ToInt32(objectColor.Substring(4, 2),16));           MCvScalar hsvColor = new MCvScalar();

            MCvScalar mask_lower = new MCvScalar(rgbColor.GetHue()/2 - 5, 140, 140); //(0,50,50);
            MCvScalar mask_upper = new MCvScalar(rgbColor.GetHue()/2 + 5, 255, 255); //(13, 255, 255);
            // MCvScalar mask_lower = new MCvScalar(13,140,140); //(0,50,50);
            // MCvScalar mask_upper = new MCvScalar(19, 255, 255); //(13, 255, 255);

            //Maske erstellen
            Mat mask = new Mat();

            CvInvoke.InRange(hsv, new ScalarArray(mask_lower), new ScalarArray(mask_upper), mask);
            BoxMaskObject.Image = mask;
            

            var moments = CvInvoke.Moments(mask,true);
            //Center of gravitiy
            if (moments.M00 > 0)
            {
                int cx = Convert.ToInt32(moments.M10 / moments.M00);
                int cy = Convert.ToInt32(moments.M01 / moments.M00);
                objectConture = true;
                CvInvoke.Circle(outputimage, new Point(cx, cy), 3, new Bgr(Color.Pink).MCvScalar, 3);
                CvInvoke.PutText(outputimage, string.Format("X:{0}, Y:{1}", cx, cy), new System.Drawing.Point(200, 50), Emgu.CV.CvEnum.FontFace.HersheyComplex, 0.5, new Bgr(Color.Blue).MCvScalar);
                Movement m = new Movement("", cx, cy, -1.0f);
                m.objectmask = mask;
                return m;
            } else
            {
                objectConture = false;
            }



            return null;
        }

        private PointF[] convertToPointF(Point[] toConvert)
        {
            PointF[] result = new PointF[toConvert.Length];
            for(int i = 0; i < toConvert.Length; i++)
            {
                result[i] = new PointF(toConvert[i].X, toConvert[i].Y);
            }
            return result;
        }

        private bool calculateMissing(Point[] cornersB, Point[] cornersC, ref Point result)
        {
            Point a1 = cornersC[1];
            Point a2 = cornersC[3];
            Point b1 = cornersB[2];
            Point b2 = cornersB[3];
            Point p = a1;
            Point q = b1;

            Point r = new Point(a2.X - a1.X, a2.Y - a1.Y);
            Point s = new Point(b2.X - b1.X, b2.Y - b1.Y);
            Point qpTemp = new Point(q.X - p.X, q.Y - p.Y);

            int rs = cross(r, s);
            if(rs <= 0)
            {
                return false;
            }
            int t = cross(qpTemp, s) / rs;

            int iX = p.X + t * r.X;
            int iY = p.Y + t * r.Y;
            result.X = iX;
            result.Y = iY;
            return true;
        }

        private int cross(Point v1, Point v2)
        {
            return (v1.X * v2.Y) - (v1.Y * v2.X);
        }

        //Distanz zwischen Marker A und Marker B berechnen
        public double distance(Mark markerA, Mark markerB)
        { 
            double tempcx = markerA.Cxitem - markerB.Cxitem;
            double tempcy = markerA.Cyitem - markerB.Cyitem;
            double squaredcx =  Math.Pow(tempcx, 2);
            double squaredcy =  Math.Pow(tempcy, 2);
            double sum = squaredcx + squaredcy;
            double result = Math.Sqrt(sum);
    
            return result;
        }
        // zwei Werte zu einem Punkt (Koordinate) machen
        public Point coordinatestopoints(Mark markerA)
        {
            Point coordinate = new Point();
            coordinate.X = Convert.ToInt16(markerA.Cxitem);
            coordinate.Y = Convert.ToInt16(markerA.Cyitem);
            return coordinate;
        }
 
        //Mittelpunkt des des QR-Codes berechnen
        public Mark center(Mark markerA, Mark markerB)
        {
            double sumA = markerA.Cxitem + markerB.Cxitem;
            double sumB = markerA.Cyitem + markerB.Cyitem;
            Mark centerMark = new Mark();
            centerMark.Cxitem = (sumA * 0.5);
            centerMark.Cyitem = (sumB * 0.5);
            return centerMark;
        }

        //Steigung
        public double slope(Mark markerL, Mark markerM)
        {
            double slope, dx, dy;
            dx = (markerL.Cxitem - markerM.Cxitem);
            dy = (markerL.Cyitem - markerM.Cyitem);
            if (dx == 0)
            {
                return 0.0;
            }
            slope = dy / dx;
            return slope;
        }


        // Winkel berechnen, wie liegt der QR-Code
        public double angle(Mark markerA, Mark markerB)
        {
            double tempcx = markerA.Cxitem - markerB.Cxitem;
            double tempcy = markerA.Cyitem - markerB.Cyitem;

            double temp = tempcy / tempcx;
            double angle = Math.Atan(temp) * 180 / Math.PI;
            return angle;
        }

        public double threePointdist(Mark markerL, Mark markerM, Mark markerJ)
        {
            double dist = 0;
            double a, b, c;
            a = -(markerM.Cyitem - markerL.Cyitem) / (markerM.Cxitem - markerL.Cxitem);
            b = 1.0;
            c = (((markerM.Cyitem - markerL.Cyitem) / (markerM.Cxitem - markerL.Cxitem)) * markerL.Cxitem) - markerL.Cyitem;

            dist = (a * markerJ.Cxitem + (b * markerJ.Cyitem) + c) / Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));

            return  dist;
        }

        //Distanz zwischen drei Punkten
        public Point[] findCorners(VectorOfPoint cnt)
        {
            RotatedRect rect = CvInvoke.MinAreaRect(cnt);
            PointF[] box = CvInvoke.BoxPoints(rect);
            Point[] boxx = new Point[4];
            for (int i = 0; i < boxx.Length; i++)
            {
                boxx[i] = new Point((int) box[i].X,(int) box[i].Y);
            }
            return boxx;            
        }

        //finde die Ecken die am weitesten vom Mittelpunkt entfernt sind
        public Point[] findFarest(Point[] points, Mark center, int count)
        {
            Point[] result = new Point[count];
            List <double> distances = new List<double>();

            for (int i = 0; i < points.Length; i++)
            {
                Point point = points[i];
                Mark pointm = new Mark();
                pointm.Cxitem = point.X;
                pointm.Cyitem = point.Y;

                distances.Add(distance(pointm, center));
            }

            for(int i = 0; i < count; i++)
            {
                int idx = distances
                .Select((value, index) => new { Value = value, Index = index })
                .Aggregate((a, b) => (a.Value > b.Value) ? a : b)
                .Index;
                distances[idx] = -1;
                result[i] = points[idx];
            }

            return result;

        }

        private Point[] sortPoints(Point[] corners)
        {
            Point[] cornersort = new Point[4];

            int[] sum = new int[4];
            int[] diff = new int[4];

            for (int i = 0; i < corners.Length; i++)
            {
                sum[i] = corners[i].X + corners[i].Y;
                diff[i] = corners[i].X - corners[i].Y;
            }

            int sumMin = int.MaxValue; //AUFPASSEN SOLLTE Bild größer werden -->anpassen
            int sumMax = 0;
            int sumMinIndex = 0;
            int sumMaxIndex = 0;
            int diffMin = 0;
            int diffMax = int.MinValue;
            int diffMinIndex = 0;
            int diffMaxIndex = 0;

            tbDebug.AppendText("Differenz " + diff[0].ToString());
            tbDebug.AppendText("Summe " + sum.ToString());

            for (int i = 0; i < corners.Length; i++)
            {
                if (sum[i] < sumMin)
                {
                    sumMin = sum[i];
                    sumMinIndex = i;
                }

                if (sum[i] > sumMax)
                {
                    sumMax = sum[i];
                    sumMaxIndex = i;
                }

                if (diff[i] < diffMin)
                {
                    diffMin = diff[i];
                    diffMinIndex = i;
                }

                if (diff[i] > diffMax)
                {
                    diffMax = diff[i];
                    diffMaxIndex = i;
                }


            }

            cornersort[0] = new Point(corners[sumMinIndex].X, corners[sumMinIndex].Y);
            cornersort[3] = new Point(corners[sumMaxIndex].X, corners[sumMaxIndex].Y);
            cornersort[1] = new Point(corners[diffMinIndex].X, corners[diffMinIndex].Y);
            cornersort[2] = new Point(corners[diffMaxIndex].X, corners[diffMaxIndex].Y);

            return cornersort;
        }


        /******************
         * Handschuhtracking
         ******************/
         /****
          * Farbraum in HSV-Color umwandlen, damit der Code robuster und weniger Licht anfällig ist
          * Masken erstellen um die Farben zu trennen - und bitweise verknüpfen, so dass nur mehr die Farben übrig bleiben! 
          * Kontouren erkennen - um Schwerpunkt zu berechnen
          ****/

        private Tuple<Movement, Movement> gloveRecognized(Mat inputimage, Mat outputimage)
        {
            Mat work = inputimage.Clone();
            work.ConvertTo(work, DepthType.Default, 1, 10); //Dani Brightness nach oben gestellt
            gloveBox.Image = work;
            Mat workViertel = new Mat((int)work.Rows / 4, (int)work.Cols / 4, work.Depth, work.NumberOfChannels);
            CvInvoke.PyrDown(work, workViertel);
           //CvInvoke.Resize(work, work, new Size((int)work.Rows / 4, (int)work.Cols / 4));
            work = workViertel;

            CvInvoke.MedianBlur(work, work, 15);
            CvInvoke.GaussianBlur(work, work,new Size(5,5),5);

            // konvertieren in HSV FUll (0-360°)
            Mat hsv = new Mat();
            CvInvoke.CvtColor(work, hsv, Emgu.CV.CvEnum.ColorConversion.Bgr2HsvFull);

            //Masken erstellen
            //Kuchenstücke von HSV (Grenzen) definieren

            MCvScalar mask_red_lower= new MCvScalar(0,50,50); //(0,50,50);
            MCvScalar mask_red_upper = new MCvScalar(5, 255, 255); //(13, 255, 255);

            MCvScalar mask_green_lower = new MCvScalar(90, 25, 50); //(90, 50, 50);
            MCvScalar mask_green_upper = new MCvScalar(135, 255, 255); //(120, 255, 255);

            //Maske erstellen
            Mat mask_red = new Mat();
            Mat mask_green= new Mat();

            CvInvoke.InRange(hsv, new ScalarArray(mask_red_lower), new ScalarArray(mask_red_upper), mask_red);
            CvInvoke.InRange(hsv, new ScalarArray(mask_green_lower), new ScalarArray(mask_green_upper), mask_green);

            //Dani
            Mat mask_red_upperRed = new Mat();
            MCvScalar mask_red_upperRed_lower = new MCvScalar(170, 50, 50); //(0,50,50);
            MCvScalar mask_red_upperRed_upper = new MCvScalar(179, 255, 255); //(13, 255, 255);
            CvInvoke.InRange(hsv, new ScalarArray(mask_red_upperRed_lower), new ScalarArray(mask_red_upperRed_upper), mask_red_upperRed);

            Mat mask_red_combined = new Mat(mask_red.Rows, mask_red.Cols, DepthType.Cv8S, 3);
            CvInvoke.BitwiseOr(mask_red, mask_red_upperRed, mask_red_combined);


            //Dani Ende

            //Kontouren erkennen
            VectorOfVectorOfPoint red_countours = new VectorOfVectorOfPoint ();
            VectorOfVectorOfPoint green_countours = new VectorOfVectorOfPoint();

            Mat hierachy_red = new Mat();
            Mat hierachy_green = new Mat();

            Mat countours = new Mat(work.Rows, work.Cols, Emgu.CV.CvEnum.DepthType.Cv32F, 3);

            Mat mask_Combined = new Mat(mask_red.Rows, mask_red.Cols, DepthType.Cv8S, 3);

            CvInvoke.BitwiseOr(mask_red_combined, mask_green, mask_Combined);

            BoxMaskGlove.Image = mask_Combined;
            
            CvInvoke.FindContours(mask_red_combined, red_countours, hierachy_red, Emgu.CV.CvEnum.RetrType.Tree,Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            CvInvoke.FindContours(mask_green, green_countours, hierachy_green, Emgu.CV.CvEnum.RetrType.Tree, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

            //Filtern, dass nur das größte Grün angezeigt wird


            //FÜR GRÜN
            int idx_largestGreen = 0;
            double Size_largestGreen = 0;
            for (int i = 0; i < green_countours.Size; i++)
            {
                double temp = CvInvoke.ContourArea(green_countours[i]);
                if (temp > Size_largestGreen)
                {
                    Size_largestGreen = temp;
                    idx_largestGreen = i;
                }

            }

            //Für ROT
            int idx_red = -1;
            double distance_redGreen = 9999999999999; // Könnte man bei Rot verbessern - auf geringster Abstand zu Grün
            if (green_countours.Size > 0)
            {
                Rectangle rect_Green = CvInvoke.BoundingRectangle(green_countours[idx_largestGreen]);
                Point centerGreen = new Point((rect_Green.X + rect_Green.Width / 2) * 2, (rect_Green.Y + rect_Green.Height / 2) * 2);
                for (int i = 0; i < red_countours.Size; i++)
                {
                    double contourSize = CvInvoke.ContourArea(red_countours[i]);
                    tbDebug.AppendText("\n\n Red IDX: " + i + "  Size: " + contourSize + " \n\n\n\n");
                    if (contourSize < 5)
                    {
                        continue;
                    }

                    Rectangle rect_Red = CvInvoke.BoundingRectangle(red_countours[i]);
                    
                    Point centerRed =new Point((rect_Red.X + rect_Red.Width / 2) * 2, (rect_Red.Y + rect_Red.Height / 2) * 2);
                    double deltax = centerRed.X - centerGreen.X;
                    double deltay = centerRed.Y - centerGreen.Y;

                    double temp = Math.Sqrt(Math.Pow(deltax,2)+Math.Pow(deltay,2));

                    if (temp < distance_redGreen)
                    {
                        distance_redGreen = temp;
                        idx_red = i;
                    }

                }
            }
            if(idx_red < 0)
            {
                return null;
            }
            Movement greenMove = null;
            Movement redMove = null;
            MCvScalar redcolor = new MCvScalar(0, 0, 255);
            MCvScalar greencolor = new MCvScalar(0, 255, 0);
            if (red_countours.Size > 0)
            {
                Rectangle rect_Red = CvInvoke.BoundingRectangle(red_countours[idx_red]);
                CvInvoke.Rectangle(countours, rect_Red, redcolor, 1, Emgu.CV.CvEnum.LineType.AntiAlias);
                CvInvoke.DrawContours(countours, red_countours, idx_red, redcolor, 2); //ROT
                redMove = new Movement("Red", (rect_Red.X+ (rect_Red.Width / 2)) * 2, (rect_Red.Y + (rect_Red.Height / 2))*2, 0.0f);
                                
                CvInvoke.Circle(outputimage, new Point(redMove.X, redMove.Y), 3, redcolor, 3);
                CvInvoke.PutText(outputimage, string.Format("X:{0}, Y:{1}", redMove.X, redMove.Y), new System.Drawing.Point(10, 80), Emgu.CV.CvEnum.FontFace.HersheyComplex, 0.5, redcolor);


            }
            if (green_countours.Size > 0)
            {
                Rectangle rect_Green = CvInvoke.BoundingRectangle(green_countours[idx_largestGreen]);
                CvInvoke.Rectangle(countours, rect_Green, greencolor, 1, Emgu.CV.CvEnum.LineType.AntiAlias);
                CvInvoke.DrawContours(countours, green_countours, idx_largestGreen, greencolor, 2); //GRÜN
                greenMove = new Movement("Green", (rect_Green.X + rect_Green.Width / 2)*2, (rect_Green.Y + rect_Green.Height / 2)*2, 0.0f);
                
                CvInvoke.Circle(outputimage, new Point(greenMove.X, greenMove.Y), 3, greencolor,3);
                CvInvoke.PutText(outputimage, string.Format("X:{0}, Y:{1}", greenMove.X, greenMove.Y), new System.Drawing.Point(10, 110), Emgu.CV.CvEnum.FontFace.HersheyComplex, 0.5, greencolor);

            }



            if (red_countours.Size > 0 && green_countours.Size > 0)
            {
                CvInvoke.Line(outputimage, new Point(redMove.X, redMove.Y), new Point(greenMove.X, greenMove.Y), new Bgr(Color.RosyBrown).MCvScalar, 2);
                float deltax = redMove.X - greenMove.X;
                float deltay = redMove.Y - greenMove.Y;

                double length = Math.Sqrt(Math.Pow(deltax, 2) + Math.Pow(deltay, 2));
                int centerx = (redMove.X + greenMove.X) / 2;
                int centery = (redMove.Y + greenMove.Y) / 2;
                CvInvoke.PutText(outputimage, string.Format("M: X:{0}, Y:{1} L:{2:F2}", centerx, centery, length), new System.Drawing.Point(10, 140), Emgu.CV.CvEnum.FontFace.HersheyComplex, 0.5, new Bgr(Color.Brown).MCvScalar);
                CvInvoke.Circle(outputimage, new Point(centerx, centery), 3, new Bgr(Color.Brown).MCvScalar, 3);
            }

            //gloveBox.Image = countours;

            

            return Tuple.Create(redMove, greenMove);
        }

        





        //Teil für die Spracherekennung - hier Ausführung der Sprachbefehle programmieren
        private void CommandRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string Command = e.Result.Text;
            lblDemo.Text = e.Result.Text;
            switch (e.Result.Text)
            {
                case "Manny starte die Aufnahme":
                    start = true;
                    //control.changeTool(2);
                    break;
                case "Manny beende die Aufnahme":
                    start = false;
                    //control.changeTool(1);
                 
                    break;
                
            }
        }

        private void buttonFile_Click(object sender, EventArgs e)
        {
            
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                live = false;
                panelMode.Hide();
                this.video_capture = new Emgu.CV.VideoCapture(openFileDialog.FileName);
                checkBoxLoop.Visible = true;
                this.video_capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps, 30);
                this.video_capture.ImageGrabbed += processFrame;
                this.video_capture.Start();
            }
        }

        private void buttonLive_Click(object sender, EventArgs e)
        {
            live = true;
            panelMode.Hide();
            this.video_capture = new Emgu.CV.VideoCapture(2);
            this.video_capture.ImageGrabbed += processFrame;
            
            //this.video_capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps, 10);
            //this.video_capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, 640);
            //this.video_capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, 480);
            this.video_capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, 960);
            this.video_capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, 540);
            this.video_capture.SetCaptureProperty(CapProp.Focus, 0);
            this.video_capture.Start();
        }

        private void buttonRecord_Click(object sender, EventArgs e)
        {
            if (!this.start)
            {
                this.start = true;
                this.buttonRecord.Text = "Stopp";
                captureCount = 0;
            } else
            {
                this.start = false;
                this.buttonRecord.Text = "Start";
              
                
            }
        }

        private void btDoSomething_Click(object sender, EventArgs e)
        {
            List<RobotMovement> translatedMovements = translateMovements(this.movements);
            
        }


        /****
         *
         *Hier werden die Movements in Roboterbewegungen und textuelle Greifen, Drehung... umgewandelt
         * 
         ****/
        private List<RobotMovement> translateMovements(List<Movement> movements)
        {
            if (classifierForm.Visible == false)
            {
                classifierForm.Show();
            }
            //classifierForm.tbClassified.Clear();
            List<RobotMovement> roboMoves = new List<RobotMovement>();

            List<List<Movement>> groupedMovements = movements
            .GroupBy(e => e.Frame)
            .Select(grp => grp.ToList())
            .OrderBy(lm => lm.First().Frame)
            .ToList();

            int processableFrames = groupedMovements.Where(e => e.Count() == 3).Count();

            lbFrameCount.Text = String.Format("{0};{1}", frameCount, processableFrames);

            for (int current = 0; current < groupedMovements.Count; current++)
            {
                List<Movement> currentMovements = groupedMovements.ElementAt(current);
                List<Movement> previousMovements = new List<Movement>();
                List<Movement> nextMovements = new List<Movement>();

                if(current > 0)
                {
                    previousMovements = groupedMovements.ElementAt(current - 1);
                }

                if (current > 0 && current+1 < groupedMovements.Count)
                {
                    nextMovements = groupedMovements.ElementAt(current + 1);
                }

                bool redVisible = false;
                bool greenVisible = false;
                bool objectVisible = false;

                Movement red = null;
                Movement green = null; 
                Movement obje = null;
                Movement image = null;
                double length = 0;
                

                foreach(Movement m in currentMovements)
                {
                    if ("Red".Equals(m.ObjectType))
                    {
                        redVisible = true;
                        red = m;
                    } else if ("Green".Equals(m.ObjectType))
                    {
                        greenVisible = true;
                        green = m;
                    } else if ("image".Equals(m.ObjectType)) {
                        image = m;
                    } else
                    {
                        objectVisible = true;
                        obje = m;
                    }
                }
             
                if (redVisible && greenVisible && objectVisible)
                {
                    // Start classification

                    // Basic movement
                    RobotMovement move = new RobotMovement();
                        // TODO Nur wenn Handschuh sich bewegt?
                    //move.typ.Add(RobotMovement.MovementTyp.MOVE);
                    move.imagesource = new Mat();
                    if(image != null)
                    {
                        move.imagesource = image.imagesource;
                    }


                    float deltax = red.X - green.X;
                    float deltay = red.Y - green.Y;
           
                    length = Math.Sqrt(Math.Pow(deltax, 2) + Math.Pow(deltay, 2));
                    float centerx = (red.X + green.X) / 2;
                    float centery = (red.Y + green.Y) / 2;
                    move.x = centerx;
                    move.y = centery;
                    move.a = obje.Angle;

                    //Erkennung von Greifer Auf/ Zu

                    //Prüfung ob Linie zwischen Rot und Grün die Objektmaske schneidet!!

                    bool intersect = intersection(red, green, obje);

                    
                    if (intersect == true)
                    {
                        bool grasp = false;
                        bool rotate = false;
                        bool release = false;
                        bool reach = false;

                        if (previousMovements.Count > 0)
                        {
                            Movement objeprev = null;
                            foreach (Movement m in previousMovements)
                            {
                                if (String.IsNullOrEmpty(m.ObjectType))
                                {
                                    objeprev = m;
                                }
                            }
                            if(objeprev != null && Math.Abs(obje.Angle - objeprev.Angle) > 1.0)//5.0
                            {
                                rotate = true;
                            }

                        }
                        double prevdistance = length;
                        // Analyse der nächsten n Frames
                        for (int i = 1; i < 2; i++)
                        {
                            if (current > 0 && current + i < groupedMovements.Count)
                            {
                                Movement redloop = null;
                                Movement greenloop = null;
                                Movement objeloop = null;

                                //aktuelles Bild + n 
                                List <Movement> furture = groupedMovements.ElementAt(current + i);

                                //Red, Green, obje herausfiltern
                               foreach (Movement m in furture)
                                {
                                    if ("Red".Equals(m.ObjectType))
                                    {
                                        redVisible = true;
                                        redloop = m;
                                    }
                                    else if ("Green".Equals(m.ObjectType))
                                    {
                                        greenVisible = true;
                                        greenloop = m;
                                    }
                                    else if (String.IsNullOrEmpty(m.ObjectType))
                                    {
                                        objectVisible = true;
                                        objeloop = m;
                                    }

                                    if(greenloop == null || redloop == null || objeloop == null)
                                    {
                                        continue;
                                    }

                                    // schauen ob immer noch Schnitt der Linien zwischen Objekt und Grün und Rot
                                    bool loopintersect = intersection(redloop, greenloop, objeloop);

                                    //Bestimmung der Distanz
                                    float deltaxloop = redloop.X - greenloop.X;
                                    float deltayloop = redloop.Y - greenloop.Y;

                                    double distance = Math.Sqrt(Math.Pow(deltaxloop, 2) + Math.Pow(deltayloop, 2));


                                    // if (loopintersect && prevdistance - distance > 0 && length - distance > 5)/0
                                    if (loopintersect && ( (prevdistance - distance) > 0 )&& length - distance > 0) //Eigentlich ist prevdist = length (daher nicht nochmal machen)
                                    {
                                        grasp = true;
                                        
                                    } else
                                    {
                                        grasp = false;
                                    }
                                    //Loslassen
                                    if (loopintersect && ((prevdistance - distance) < 0 ))
                                    {
                                        release = true; 
                                    }
                                    else
                                    {
                                        release = false;
                                    }

                                    prevdistance = distance;

                                    if (grasp)
                                    {
                                        move.typ.Add(RobotMovement.MovementTyp.GRASP);
                                    }
                                    if (rotate)
                                    {
                                        move.typ.Add(RobotMovement.MovementTyp.ROTATE);
                                    }
                                    if (release)
                                    {
                                        move.typ.Add(RobotMovement.MovementTyp.RELEASE);

                                    }
                                }

                                

                            }
                        }

                    }
                    roboMoves.Add(move);  
                }

            }

            classifierForm.UpdateMovements(roboMoves);
            
            return roboMoves;
        }
        //Prüfung ob Linie zwischen Rot und Grün die Objektmaske schneidet!!
        private bool intersection(Movement red, Movement green, Movement obje)
        {
            bool intersect = false;
            Mat objemask = obje.objectmask;

            if (objemask != null && red != null && green != null)
            {
                Mat lineMat = new Mat(objemask.Rows, objemask.Cols, DepthType.Cv8U, 1);
                Mat result = new Mat(objemask.Rows, objemask.Cols, DepthType.Cv8U, 1);
                CvInvoke.Line(lineMat, new Point(red.X, red.Y), new Point(green.X, green.Y), new MCvScalar(255));

                CvInvoke.BitwiseAnd(objemask, lineMat, result);

                return CvInvoke.CountNonZero(result) > 0;

            }            
 
            return intersect;
        }

        
        private void btNeustart_Click(object sender, EventArgs e)
        {
            panelMode.Visible = true;
            this.video_capture.Stop();
            this.video_capture.ImageGrabbed -= processFrame;
            classifierForm.tbClassified.Clear();
        }

        private void detectQrCode(Mat inputimage)
        {   if (inputimage == null && CvInvoke.CountNonZero(inputimage) > 0)
            {
                return;
            }
            /*
             * QR-CODE DEKODIEREN
             */
          
                try
                {
                // Funktiniert mit entzerrten Bild noch nicht so wirklich
                Bitmap bmp = inputimage.Bitmap;
                    BitmapLuminanceSource source = new BitmapLuminanceSource(bmp);
                    BinaryBitmap candidate = new BinaryBitmap(new HybridBinarizer(source));

                    Result result = reader.decode(candidate, decoderhints);


                    tbQR.Text = result.Text;
                    qrCodeVisible = true;

                    // TODO QRCode text auswerten
                    objectColor = "955B09";//955B09 //E6C200
 

                }
                catch (ReaderException ex)
                {

                    tbQR.Text = "";
                    
                }
        }
    }


    
}

using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.Util;
using System.Speech.Recognition;
using com.google.zxing.client;
using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.qrcode;
using com.google.zxing.multi;
using com.google.zxing.oned;

namespace DoitRobo310317
{
    
    public partial class Form1 : Form
    {
        // für Sprachsteuerung- allgemeine Variable
        Boolean start = false;

        private Reader reader = new QRCodeReader();
        private Hashtable decoderhints = new Hashtable();

        private SpeechRecognitionEngine SR;

        private VideoCapture video_capture;

        private List<Movement> movements = new List<Movement>();

        private int frameCount = 0;

        public Form1()
        {
            InitializeComponent();
            this.video_capture = new Emgu.CV.VideoCapture(2);
            tbDebug.Text = null;
            
            decoderhints.Add(DecodeHintType.POSSIBLE_FORMATS, BarcodeFormat.QR_CODE);
            decoderhints.Add(DecodeHintType.TRY_HARDER, true);





            /*
            * Sprachsteuerung
            */
            SR = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("de-DE"));
            SR.SetInputToDefaultAudioDevice();
            Choices Commands = new Choices();

            Commands.Add("Manny start");
            Commands.Add("Manny beenden");
            //weitere Befehle hier eingeben

            GrammarBuilder GB = new GrammarBuilder(Commands); // die Befehle mit einem GrammerBuilder laden
            Grammar CommandGrammar = new Grammar(GB); // eine Grammatik über den GrammarBuilder erstellen
            SR.LoadGrammarAsync(CommandGrammar); // die Grammatik laden
            SR.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(CommandRecognized); // Funktion zur Behandlung des Ereignisses
            //SR.Enabled = true;
            SR.RecognizeAsync(RecognizeMode.Multiple);

            Application.Idle += processFrame;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void processFrame(object sender, EventArgs e)
        {
            // Aktuelles Bild -- Ab hier beginnt die Auswertung und die Erkennung des QR Codes
            Emgu.CV.Mat image = new Mat();
            video_capture.Read(image);

            Movement objectMovement = TrackObject(image);
            Tuple<Movement, Movement> gloveMovement = gloveRecognized(image);

            if (start)
            {
                int frameNumber = frameCount++;

                if (objectMovement != null)
                {
                    objectMovement.Frame = frameNumber;
                    this.movements.Add(objectMovement);
                }
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
            }
            this.imageBox1.Image = image;
        }

        private Movement TrackObject(Mat image)
        {

            Mat original = image.Clone();

            Emgu.CV.Mat gray = new Mat();
            CvInvoke.CvtColor(image, gray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

            Mat edges = new Mat();
            CvInvoke.Canny(gray, edges, 100, 200, 3);
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();

            int[,] hierachy = CvInvoke.FindContourTree(edges, contours, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

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
                            CvInvoke.DrawContours(image, contours, i, color, 2);
                            CvInvoke.Circle(image, new Point(cx, cy), 1, color, 2);
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
                    CvInvoke.Line(image, coordinatestopoints(markerslist[0]), coordinatestopoints(markerslist[1]), new Bgr(Color.Green).MCvScalar, 2);
                    lineAngle = angle(markerslist[0], markerslist[1]);
                    cp = center(markerslist[0], markerslist[1]);
                    markerA = markerslist[2];
                    markerB = markerslist[0];
                    markerC = markerslist[1];
                    //
                }
                if (AC > AB && AC > BC)
                {
                    CvInvoke.Line(image, coordinatestopoints(markerslist[0]), coordinatestopoints(markerslist[2]), new Bgr(Color.Blue).MCvScalar, 2);
                    lineAngle = angle(markerslist[0], markerslist[2]);
                    cp = center(markerslist[0], markerslist[2]);
                    markerA = markerslist[1];
                    markerB = markerslist[2];
                    markerC = markerslist[0];
                }
                if (BC > AB && BC > AC)
                {
                    CvInvoke.Line(image, coordinatestopoints(markerslist[1]), coordinatestopoints(markerslist[2]), new Bgr(Color.Red).MCvScalar, 2);
                    lineAngle = angle(markerslist[1], markerslist[2]);
                    cp = center(markerslist[1], markerslist[2]);
                    markerA = markerslist[0];
                    markerB = markerslist[1];
                    markerC = markerslist[2];
                }

                //Falls Markererkennung nicht richtig ist, kann das sonst nicht berechnet werden
                if (markerA == null)
                {
                    this.imageBox1.Image = image;
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
                    CvInvoke.Circle(image, p, 7, new MCvScalar(255, 191, 0), 2, Emgu.CV.CvEnum.LineType.AntiAlias);

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

                CvInvoke.Circle(image, missingPointP, 7, new MCvScalar(255, 191, 0), 9, Emgu.CV.CvEnum.LineType.AntiAlias);

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
                CvInvoke.WarpPerspective(original, dst, transformMat, new Size(480, 480));

                /*
                 * QR-CODE DEKODIEREN
                 */
                try
                {
                    // Funktiniert mit entzerrten Bild noch nicht so wirklich
                    RGBLuminanceSource source = new RGBLuminanceSource(original.Bitmap, original.Cols, original.Rows);
                    BinaryBitmap candidate = new BinaryBitmap(new HybridBinarizer(source));
                    Result result = reader.decode(candidate, decoderhints);

                    tbQR.Text = result.Text;
                }
                catch (ReaderException ex)
                {

                    tbQR.Text = "";
                }

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

                Mat temp = image.Clone();


                //CvInvoke.PutText(image,"Winkel: " + Math.Round((lineAngle),2).ToString(), new System.Drawing.Point(10, 80), Emgu.CV.CvEnum.FontFace.HersheyComplex,1.0,new Bgr(0, 0, 0).MCvScalar);
                CvInvoke.Circle(image, coordinatestopoints(cp), 5, new Bgr(0, 0, 255).MCvScalar);
                CvInvoke.PutText(image, "Position: X:" + cp.Cxitem.ToString() + " Y:" + cp.Cyitem.ToString(), new System.Drawing.Point(10, 50), Emgu.CV.CvEnum.FontFace.HersheyComplex, 1.0, new Bgr(0, 0, 0).MCvScalar);

                this.imageBoxQr.Image = dst;

                return new Movement(tbQR.Text, (int)(cp.Cxitem), (int)cp.Cyitem, (float)rotationAngle);
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

        private Tuple<Movement, Movement> gloveRecognized(Mat image)
        {
            Mat work = image.Clone();
            CvInvoke.MedianBlur(work, work, 15);
            CvInvoke.GaussianBlur(work,work,new Size(5,5),5);

            // konvertieren in HSV FUll (0-360°)
            Mat hsv = new Mat();
            CvInvoke.CvtColor(work, hsv, Emgu.CV.CvEnum.ColorConversion.Bgr2HsvFull);

            //Masken erstellen
            //Kuchenstücke von HSV (Grenzen) definieren

            MCvScalar mask_red_lower= new MCvScalar(0,50,50);
            MCvScalar mask_red_upper = new MCvScalar(13, 255, 255);

            MCvScalar mask_green_lower = new MCvScalar(90, 50, 50);
            MCvScalar mask_green_upper = new MCvScalar(120, 255, 255);

            //Maske erstellen
            Mat mask_red = new Mat();
            Mat mask_green= new Mat();

            CvInvoke.InRange(hsv, new ScalarArray(mask_red_lower), new ScalarArray(mask_red_upper), mask_red);
            CvInvoke.InRange(hsv, new ScalarArray(mask_green_lower), new ScalarArray(mask_green_upper), mask_green);

            //Kontouren erkennen
            VectorOfVectorOfPoint red_countours = new VectorOfVectorOfPoint ();
            VectorOfVectorOfPoint green_countours = new VectorOfVectorOfPoint();

            Mat hierachy_red = new Mat();
            Mat hierachy_green = new Mat();

            Mat countours = new Mat(work.Rows, work.Cols, Emgu.CV.CvEnum.DepthType.Cv32F, 3);

            CvInvoke.FindContours(mask_red, red_countours, hierachy_red, Emgu.CV.CvEnum.RetrType.Tree,Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            CvInvoke.FindContours(mask_green, green_countours, hierachy_green, Emgu.CV.CvEnum.RetrType.Tree, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

            //Filtern, dass nur das größte angezeigt wird

            //Für ROT
            int idx_largestRed = 0;
            double Size_largestRed = 0; // Könnte man bei Rot verbessern - auf geringster Abstand zu Grün
            for (int i = 0; i < red_countours.Size; i++)
            {
               double temp= CvInvoke.ContourArea(red_countours[i]);
                if (temp > Size_largestRed)
                {
                    Size_largestRed = temp;
                    idx_largestRed = i;
                }

            }
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

            Movement greenMove = null;
            Movement redMove = null;
            if (red_countours.Size > 0)
            {
                Rectangle rect_Red = CvInvoke.BoundingRectangle(red_countours[idx_largestRed]);
                CvInvoke.Rectangle(countours, rect_Red, new MCvScalar(0, 0, 255), 1, Emgu.CV.CvEnum.LineType.AntiAlias);
                CvInvoke.DrawContours(countours, red_countours, idx_largestRed, new MCvScalar(0, 0, 255), 2); //ROT
                redMove = new Movement("Red", rect_Red.X + rect_Red.Width / 2, rect_Red.Y + rect_Red.Height / 2, 0.0f);
            }
            if (green_countours.Size > 0)
            {
                Rectangle rect_Green = CvInvoke.BoundingRectangle(green_countours[idx_largestGreen]);
                CvInvoke.Rectangle(countours, rect_Green, new MCvScalar(0, 255, 0), 1, Emgu.CV.CvEnum.LineType.AntiAlias);
                CvInvoke.DrawContours(countours, green_countours, idx_largestGreen, new MCvScalar(0, 255, 0), 2); //GRÜN
                greenMove = new Movement("Green", rect_Green.X + rect_Green.Width / 2, rect_Green.Y + rect_Green.Height / 2, 0.0f);

            }

            gloveBox.Image = countours;

            return Tuple.Create(redMove, greenMove);
        }

        





        //Teil für die Spracherekennung - hier Ausführung der Sprachbefehle programmieren
        private void CommandRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string Command = e.Result.Text;
            lblDemo.Text = e.Result.Text;
            switch (e.Result.Text)
            {
                case "Manny start":
                    start = true;
                    //control.changeTool(2);
                    break;
                case "Manny beenden":
                    start = false;
                    //control.changeTool(1);
                    this.moveMentsLB.Items.Clear();
                    foreach(Movement m in this.movements)
                    {
                        this.moveMentsLB.Items.Add(m);
                    }
                    break;
                
            }
        }

     
    }


    
}

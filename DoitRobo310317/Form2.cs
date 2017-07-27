using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoitRobo310317
{
    public partial class Form2 : Form
    {

        private List<RobotMovement> Robomoves { get; set; }

        public Form2()
        {
            InitializeComponent();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        

        private int idx = 0;

        public void UpdateMovements(List<RobotMovement> newMovements)
        {
            Robomoves = newMovements;
            idx = 0;
            updateGui();
        }

        private void updateGui()
        {
            if(Robomoves.Count == 0)
            {
                return;
            }
            imageBoxDebug.Image = Robomoves[idx].imagesource;
            tbClassified.Text = Robomoves[idx].convert();
            //Farben zurücksetzen
            lbHinlangen.ForeColor = Color.Red;
            lbGreifen.ForeColor = Color.Red;
            lbBewegen.ForeColor = Color.Red;
            lbDrehung.ForeColor = Color.Red;
            lbLoslassen.ForeColor = Color.Red;

            lbFrameNR.Text = "" + Robomoves[idx].frameNumber;

            lbWinkel.Text = String.Format("{0:F2}", Robomoves[idx].a);

            foreach (RobotMovement.MovementTyp t in Robomoves[idx].typ)
            {


                switch (t)
                {
                    case RobotMovement.MovementTyp.REACH:
                        lbHinlangen.ForeColor = Color.Green;
                        break;
                    case RobotMovement.MovementTyp.GRASP:
                        lbGreifen.ForeColor = Color.Green;
                        break;
                    case RobotMovement.MovementTyp.MOVE:
                        lbBewegen.ForeColor = Color.Green;
                        break;
                    case RobotMovement.MovementTyp.RELEASE:
                        lbLoslassen.ForeColor = Color.Green;
                        break;
                    case RobotMovement.MovementTyp.ROTATE:
                        lbDrehung.ForeColor = Color.Green;
                        break;
                }
            }
                    if (idx == Robomoves.Count - 1)
                    {
                        btNext.Enabled = false;
                        btPrev.Enabled = true;
            }
                    else if (idx == 0)
                    {
                        btPrev.Enabled = false;
                        btNext.Enabled = true;
                    }
                    else
                    {
                        btPrev.Enabled = true;
                        btNext.Enabled = true;
                    }

                    
        }

        private void btPrev_Click(object sender, EventArgs e)
        {
            if (idx > 1)
            {
                idx--;
                updateGui();
            }
        }

        private void btNext_Click(object sender, EventArgs e)
        {

            if (idx < Robomoves.Count - 1)
            {
                idx++;
                updateGui();
            }

          


        }
    }
}

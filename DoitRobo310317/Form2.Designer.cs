namespace DoitRobo310317
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tbClassified = new System.Windows.Forms.TextBox();
            this.btClose = new System.Windows.Forms.Button();
            this.lbXML = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbHinlangen = new System.Windows.Forms.Label();
            this.lbGreifen = new System.Windows.Forms.Label();
            this.lbBewegen = new System.Windows.Forms.Label();
            this.lbLoslassen = new System.Windows.Forms.Label();
            this.lbDrehung = new System.Windows.Forms.Label();
            this.lbWinkel = new System.Windows.Forms.Label();
            this.imageBoxDebug = new Emgu.CV.UI.ImageBox();
            this.btPrev = new System.Windows.Forms.Button();
            this.btNext = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lbFrameNR = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxDebug)).BeginInit();
            this.SuspendLayout();
            // 
            // tbClassified
            // 
            this.tbClassified.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbClassified.Location = new System.Drawing.Point(863, 87);
            this.tbClassified.Multiline = true;
            this.tbClassified.Name = "tbClassified";
            this.tbClassified.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbClassified.Size = new System.Drawing.Size(730, 236);
            this.tbClassified.TabIndex = 24;
            // 
            // btClose
            // 
            this.btClose.Location = new System.Drawing.Point(1403, 12);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(171, 52);
            this.btClose.TabIndex = 25;
            this.btClose.Text = "Schließen";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // lbXML
            // 
            this.lbXML.AutoSize = true;
            this.lbXML.Location = new System.Drawing.Point(858, 35);
            this.lbXML.Name = "lbXML";
            this.lbXML.Size = new System.Drawing.Size(132, 29);
            this.lbXML.TabIndex = 26;
            this.lbXML.Text = "XML String";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(866, 400);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 29);
            this.label1.TabIndex = 27;
            this.label1.Text = "Greifen";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(866, 522);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 29);
            this.label2.TabIndex = 28;
            this.label2.Text = "Loslassen";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(866, 341);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 29);
            this.label3.TabIndex = 29;
            this.label3.Text = "Hinlangen";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(866, 461);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 29);
            this.label4.TabIndex = 30;
            this.label4.Text = "Bewegen";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(866, 590);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 29);
            this.label5.TabIndex = 31;
            this.label5.Text = "Drehung";
            // 
            // lbHinlangen
            // 
            this.lbHinlangen.AutoSize = true;
            this.lbHinlangen.Location = new System.Drawing.Point(1017, 341);
            this.lbHinlangen.Name = "lbHinlangen";
            this.lbHinlangen.Size = new System.Drawing.Size(28, 29);
            this.lbHinlangen.TabIndex = 32;
            this.lbHinlangen.Text = "●";
            // 
            // lbGreifen
            // 
            this.lbGreifen.AutoSize = true;
            this.lbGreifen.Location = new System.Drawing.Point(1017, 400);
            this.lbGreifen.Name = "lbGreifen";
            this.lbGreifen.Size = new System.Drawing.Size(28, 29);
            this.lbGreifen.TabIndex = 33;
            this.lbGreifen.Text = "●";
            // 
            // lbBewegen
            // 
            this.lbBewegen.AutoSize = true;
            this.lbBewegen.Location = new System.Drawing.Point(1017, 461);
            this.lbBewegen.Name = "lbBewegen";
            this.lbBewegen.Size = new System.Drawing.Size(28, 29);
            this.lbBewegen.TabIndex = 34;
            this.lbBewegen.Text = "●";
            // 
            // lbLoslassen
            // 
            this.lbLoslassen.AutoSize = true;
            this.lbLoslassen.Location = new System.Drawing.Point(1017, 522);
            this.lbLoslassen.Name = "lbLoslassen";
            this.lbLoslassen.Size = new System.Drawing.Size(28, 29);
            this.lbLoslassen.TabIndex = 35;
            this.lbLoslassen.Text = "●";
            // 
            // lbDrehung
            // 
            this.lbDrehung.AutoSize = true;
            this.lbDrehung.Location = new System.Drawing.Point(1017, 590);
            this.lbDrehung.Name = "lbDrehung";
            this.lbDrehung.Size = new System.Drawing.Size(28, 29);
            this.lbDrehung.TabIndex = 36;
            this.lbDrehung.Text = "●";
            // 
            // lbWinkel
            // 
            this.lbWinkel.AutoSize = true;
            this.lbWinkel.Location = new System.Drawing.Point(1076, 590);
            this.lbWinkel.Name = "lbWinkel";
            this.lbWinkel.Size = new System.Drawing.Size(86, 29);
            this.lbWinkel.TabIndex = 37;
            this.lbWinkel.Text = "Winkel";
            // 
            // imageBoxDebug
            // 
            this.imageBoxDebug.Location = new System.Drawing.Point(12, 87);
            this.imageBoxDebug.Name = "imageBoxDebug";
            this.imageBoxDebug.Size = new System.Drawing.Size(828, 464);
            this.imageBoxDebug.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBoxDebug.TabIndex = 2;
            this.imageBoxDebug.TabStop = false;
            // 
            // btPrev
            // 
            this.btPrev.Location = new System.Drawing.Point(12, 578);
            this.btPrev.Name = "btPrev";
            this.btPrev.Size = new System.Drawing.Size(204, 76);
            this.btPrev.TabIndex = 38;
            this.btPrev.Text = "vorheriges";
            this.btPrev.UseVisualStyleBackColor = true;
            this.btPrev.Click += new System.EventHandler(this.btPrev_Click);
            // 
            // btNext
            // 
            this.btNext.Location = new System.Drawing.Point(636, 578);
            this.btNext.Name = "btNext";
            this.btNext.Size = new System.Drawing.Size(204, 76);
            this.btNext.TabIndex = 39;
            this.btNext.Text = "nächstes";
            this.btNext.UseVisualStyleBackColor = true;
            this.btNext.Click += new System.EventHandler(this.btNext_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 29);
            this.label6.TabIndex = 40;
            this.label6.Text = "Frame";
            // 
            // lbFrameNR
            // 
            this.lbFrameNR.AutoSize = true;
            this.lbFrameNR.Location = new System.Drawing.Point(115, 35);
            this.lbFrameNR.Name = "lbFrameNR";
            this.lbFrameNR.Size = new System.Drawing.Size(26, 29);
            this.lbFrameNR.TabIndex = 41;
            this.lbFrameNR.Text = "0";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1605, 744);
            this.ControlBox = false;
            this.Controls.Add(this.lbFrameNR);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btNext);
            this.Controls.Add(this.btPrev);
            this.Controls.Add(this.imageBoxDebug);
            this.Controls.Add(this.lbWinkel);
            this.Controls.Add(this.lbDrehung);
            this.Controls.Add(this.lbLoslassen);
            this.Controls.Add(this.lbBewegen);
            this.Controls.Add(this.lbGreifen);
            this.Controls.Add(this.lbHinlangen);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbXML);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.tbClassified);
            this.Name = "Form2";
            this.Text = "Klassifizierung";
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxDebug)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox tbClassified;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Label lbXML;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbHinlangen;
        private System.Windows.Forms.Label lbGreifen;
        private System.Windows.Forms.Label lbBewegen;
        private System.Windows.Forms.Label lbLoslassen;
        private System.Windows.Forms.Label lbDrehung;
        private System.Windows.Forms.Label lbWinkel;
        private Emgu.CV.UI.ImageBox imageBoxDebug;
        private System.Windows.Forms.Button btPrev;
        private System.Windows.Forms.Button btNext;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbFrameNR;
    }
}
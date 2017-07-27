namespace DoitRobo310317
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            this.lblDemo = new System.Windows.Forms.Label();
            this.lbDebug = new System.Windows.Forms.Label();
            this.QRCode = new System.Windows.Forms.Label();
            this.tbQR = new System.Windows.Forms.TextBox();
            this.tbDebug = new System.Windows.Forms.TextBox();
            this.btDoSomething = new System.Windows.Forms.Button();
            this.gloveBox = new Emgu.CV.UI.ImageBox();
            this.lblglove = new System.Windows.Forms.Label();
            this.panelMode = new System.Windows.Forms.Panel();
            this.buttonFile = new System.Windows.Forms.Button();
            this.buttonLive = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.checkBoxLoop = new System.Windows.Forms.CheckBox();
            this.imageBoxQr = new Emgu.CV.UI.ImageBox();
            this.BoxMaskGlove = new Emgu.CV.UI.ImageBox();
            this.BoxMaskObject = new Emgu.CV.UI.ImageBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.qrBox = new Emgu.CV.UI.ImageBox();
            this.buttonRecord = new System.Windows.Forms.Button();
            this.btNeustart = new System.Windows.Forms.Button();
            this.lbFrameCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gloveBox)).BeginInit();
            this.panelMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxQr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoxMaskGlove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoxMaskObject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qrBox)).BeginInit();
            this.SuspendLayout();
            // 
            // imageBox1
            // 
            this.imageBox1.Location = new System.Drawing.Point(12, 12);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(1566, 811);
            this.imageBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox1.TabIndex = 2;
            this.imageBox1.TabStop = false;
            // 
            // lblDemo
            // 
            this.lblDemo.AutoSize = true;
            this.lblDemo.Location = new System.Drawing.Point(425, 854);
            this.lblDemo.Name = "lblDemo";
            this.lblDemo.Size = new System.Drawing.Size(156, 29);
            this.lblDemo.TabIndex = 3;
            this.lblDemo.Text = "Sprachbefehl";
            // 
            // lbDebug
            // 
            this.lbDebug.AutoSize = true;
            this.lbDebug.Location = new System.Drawing.Point(1627, 924);
            this.lbDebug.Name = "lbDebug";
            this.lbDebug.Size = new System.Drawing.Size(136, 29);
            this.lbDebug.TabIndex = 5;
            this.lbDebug.Text = "Debuginfos";
            // 
            // QRCode
            // 
            this.QRCode.AutoSize = true;
            this.QRCode.Location = new System.Drawing.Point(1290, 924);
            this.QRCode.Name = "QRCode";
            this.QRCode.Size = new System.Drawing.Size(215, 29);
            this.QRCode.TabIndex = 7;
            this.QRCode.Text = "QR Code Ausgabe";
            // 
            // tbQR
            // 
            this.tbQR.Location = new System.Drawing.Point(1295, 975);
            this.tbQR.Multiline = true;
            this.tbQR.Name = "tbQR";
            this.tbQR.Size = new System.Drawing.Size(309, 122);
            this.tbQR.TabIndex = 8;
            // 
            // tbDebug
            // 
            this.tbDebug.Location = new System.Drawing.Point(1610, 975);
            this.tbDebug.Multiline = true;
            this.tbDebug.Name = "tbDebug";
            this.tbDebug.Size = new System.Drawing.Size(363, 604);
            this.tbDebug.TabIndex = 9;
            // 
            // btDoSomething
            // 
            this.btDoSomething.Location = new System.Drawing.Point(1594, 12);
            this.btDoSomething.Name = "btDoSomething";
            this.btDoSomething.Size = new System.Drawing.Size(223, 62);
            this.btDoSomething.TabIndex = 11;
            this.btDoSomething.Text = "klassifizieren";
            this.btDoSomething.UseVisualStyleBackColor = true;
            this.btDoSomething.Click += new System.EventHandler(this.btDoSomething_Click);
            // 
            // gloveBox
            // 
            this.gloveBox.Location = new System.Drawing.Point(812, 1309);
            this.gloveBox.Name = "gloveBox";
            this.gloveBox.Size = new System.Drawing.Size(480, 270);
            this.gloveBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.gloveBox.TabIndex = 2;
            this.gloveBox.TabStop = false;
            // 
            // lblglove
            // 
            this.lblglove.AutoSize = true;
            this.lblglove.Location = new System.Drawing.Point(804, 924);
            this.lblglove.Name = "lblglove";
            this.lblglove.Size = new System.Drawing.Size(80, 29);
            this.lblglove.TabIndex = 13;
            this.lblglove.Text = "Canny";
            // 
            // panelMode
            // 
            this.panelMode.Controls.Add(this.buttonFile);
            this.panelMode.Controls.Add(this.buttonLive);
            this.panelMode.Location = new System.Drawing.Point(12, 12);
            this.panelMode.Name = "panelMode";
            this.panelMode.Size = new System.Drawing.Size(610, 134);
            this.panelMode.TabIndex = 14;
            // 
            // buttonFile
            // 
            this.buttonFile.Location = new System.Drawing.Point(362, 27);
            this.buttonFile.Name = "buttonFile";
            this.buttonFile.Size = new System.Drawing.Size(207, 75);
            this.buttonFile.TabIndex = 1;
            this.buttonFile.Text = "Datei öffnen";
            this.buttonFile.UseVisualStyleBackColor = true;
            this.buttonFile.Click += new System.EventHandler(this.buttonFile_Click);
            // 
            // buttonLive
            // 
            this.buttonLive.Location = new System.Drawing.Point(40, 27);
            this.buttonLive.Name = "buttonLive";
            this.buttonLive.Size = new System.Drawing.Size(235, 73);
            this.buttonLive.TabIndex = 0;
            this.buttonLive.Text = "Live";
            this.buttonLive.UseVisualStyleBackColor = true;
            this.buttonLive.Click += new System.EventHandler(this.buttonLive_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "*.mp4";
            this.openFileDialog.Filter = "Video|*.mp4";
            // 
            // checkBoxLoop
            // 
            this.checkBoxLoop.AutoSize = true;
            this.checkBoxLoop.Location = new System.Drawing.Point(52, 825);
            this.checkBoxLoop.Name = "checkBoxLoop";
            this.checkBoxLoop.Size = new System.Drawing.Size(100, 33);
            this.checkBoxLoop.TabIndex = 15;
            this.checkBoxLoop.Text = "Loop";
            this.checkBoxLoop.UseVisualStyleBackColor = true;
            this.checkBoxLoop.Visible = false;
            // 
            // imageBoxQr
            // 
            this.imageBoxQr.Location = new System.Drawing.Point(8, 975);
            this.imageBoxQr.Name = "imageBoxQr";
            this.imageBoxQr.Size = new System.Drawing.Size(270, 270);
            this.imageBoxQr.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBoxQr.TabIndex = 16;
            this.imageBoxQr.TabStop = false;
            // 
            // BoxMaskGlove
            // 
            this.BoxMaskGlove.Location = new System.Drawing.Point(304, 975);
            this.BoxMaskGlove.Name = "BoxMaskGlove";
            this.BoxMaskGlove.Size = new System.Drawing.Size(480, 270);
            this.BoxMaskGlove.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.BoxMaskGlove.TabIndex = 17;
            this.BoxMaskGlove.TabStop = false;
            // 
            // BoxMaskObject
            // 
            this.BoxMaskObject.Location = new System.Drawing.Point(307, 1309);
            this.BoxMaskObject.Name = "BoxMaskObject";
            this.BoxMaskObject.Size = new System.Drawing.Size(480, 270);
            this.BoxMaskObject.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.BoxMaskObject.TabIndex = 18;
            this.BoxMaskObject.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(299, 924);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(312, 29);
            this.label2.TabIndex = 19;
            this.label2.Text = "Maske Handschuh Rot/Grün";
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(12, 829);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(28, 29);
            this.lbStatus.TabIndex = 20;
            this.lbStatus.Text = "●";
            // 
            // qrBox
            // 
            this.qrBox.Location = new System.Drawing.Point(809, 975);
            this.qrBox.Name = "qrBox";
            this.qrBox.Size = new System.Drawing.Size(480, 270);
            this.qrBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.qrBox.TabIndex = 21;
            this.qrBox.TabStop = false;
            // 
            // buttonRecord
            // 
            this.buttonRecord.Location = new System.Drawing.Point(257, 834);
            this.buttonRecord.Name = "buttonRecord";
            this.buttonRecord.Size = new System.Drawing.Size(127, 49);
            this.buttonRecord.TabIndex = 22;
            this.buttonRecord.Text = "Start";
            this.buttonRecord.UseVisualStyleBackColor = true;
            this.buttonRecord.Click += new System.EventHandler(this.buttonRecord_Click);
            // 
            // btNeustart
            // 
            this.btNeustart.Location = new System.Drawing.Point(1610, 742);
            this.btNeustart.Name = "btNeustart";
            this.btNeustart.Size = new System.Drawing.Size(207, 76);
            this.btNeustart.TabIndex = 24;
            this.btNeustart.Text = "Neustart";
            this.btNeustart.UseVisualStyleBackColor = true;
            this.btNeustart.Click += new System.EventHandler(this.btNeustart_Click);
            // 
            // lbFrameCount
            // 
            this.lbFrameCount.AutoSize = true;
            this.lbFrameCount.Location = new System.Drawing.Point(1305, 1175);
            this.lbFrameCount.Name = "lbFrameCount";
            this.lbFrameCount.Size = new System.Drawing.Size(45, 29);
            this.lbFrameCount.TabIndex = 25;
            this.lbFrameCount.Text = "0;0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 924);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 29);
            this.label1.TabIndex = 26;
            this.label1.Text = "QR-Code entzerrt";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(302, 1262);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(161, 29);
            this.label3.TabIndex = 27;
            this.label3.Text = "Maske Objekt";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1290, 1124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 29);
            this.label4.TabIndex = 28;
            this.label4.Text = "Frames";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1989, 1650);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbFrameCount);
            this.Controls.Add(this.btNeustart);
            this.Controls.Add(this.buttonRecord);
            this.Controls.Add(this.qrBox);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BoxMaskObject);
            this.Controls.Add(this.BoxMaskGlove);
            this.Controls.Add(this.imageBoxQr);
            this.Controls.Add(this.checkBoxLoop);
            this.Controls.Add(this.panelMode);
            this.Controls.Add(this.lblglove);
            this.Controls.Add(this.gloveBox);
            this.Controls.Add(this.btDoSomething);
            this.Controls.Add(this.tbDebug);
            this.Controls.Add(this.tbQR);
            this.Controls.Add(this.QRCode);
            this.Controls.Add(this.lbDebug);
            this.Controls.Add(this.lblDemo);
            this.Controls.Add(this.imageBox1);
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gloveBox)).EndInit();
            this.panelMode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxQr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoxMaskGlove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoxMaskObject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qrBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox imageBox1;
        private System.Windows.Forms.Label lblDemo;
        private System.Windows.Forms.Label lbDebug;
        private System.Windows.Forms.Label QRCode;
        private System.Windows.Forms.TextBox tbQR;
        private System.Windows.Forms.TextBox tbDebug;
        private System.Windows.Forms.Button btDoSomething;
        private Emgu.CV.UI.ImageBox gloveBox;
        private System.Windows.Forms.Label lblglove;
        private System.Windows.Forms.Panel panelMode;
        private System.Windows.Forms.Button buttonFile;
        private System.Windows.Forms.Button buttonLive;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.CheckBox checkBoxLoop;
        private Emgu.CV.UI.ImageBox imageBoxQr;
        private Emgu.CV.UI.ImageBox BoxMaskGlove;
        private Emgu.CV.UI.ImageBox BoxMaskObject;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbStatus;
        private Emgu.CV.UI.ImageBox qrBox;
        private System.Windows.Forms.Button buttonRecord;
        private System.Windows.Forms.Button btNeustart;
        private System.Windows.Forms.Label lbFrameCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}


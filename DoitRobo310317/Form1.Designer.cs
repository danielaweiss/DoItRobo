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
            this.imageBoxQr = new Emgu.CV.UI.ImageBox();
            this.QRCode = new System.Windows.Forms.Label();
            this.tbQR = new System.Windows.Forms.TextBox();
            this.tbDebug = new System.Windows.Forms.TextBox();
            this.moveMentsLB = new System.Windows.Forms.CheckedListBox();
            this.btDoSomething = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.gloveBox = new Emgu.CV.UI.ImageBox();
            this.lblglove = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxQr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gloveBox)).BeginInit();
            this.SuspendLayout();
            // 
            // imageBox1
            // 
            this.imageBox1.Location = new System.Drawing.Point(12, 12);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(1226, 870);
            this.imageBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox1.TabIndex = 2;
            this.imageBox1.TabStop = false;
            // 
            // lblDemo
            // 
            this.lblDemo.AutoSize = true;
            this.lblDemo.Location = new System.Drawing.Point(1261, 853);
            this.lblDemo.Name = "lblDemo";
            this.lblDemo.Size = new System.Drawing.Size(156, 29);
            this.lblDemo.TabIndex = 3;
            this.lblDemo.Text = "Sprachbefehl";
            // 
            // lbDebug
            // 
            this.lbDebug.AutoSize = true;
            this.lbDebug.Location = new System.Drawing.Point(1244, 16);
            this.lbDebug.Name = "lbDebug";
            this.lbDebug.Size = new System.Drawing.Size(136, 29);
            this.lbDebug.TabIndex = 5;
            this.lbDebug.Text = "Debuginfos";
            // 
            // imageBoxQr
            // 
            this.imageBoxQr.Location = new System.Drawing.Point(1244, 388);
            this.imageBoxQr.Name = "imageBoxQr";
            this.imageBoxQr.Size = new System.Drawing.Size(364, 243);
            this.imageBoxQr.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBoxQr.TabIndex = 2;
            this.imageBoxQr.TabStop = false;
            // 
            // QRCode
            // 
            this.QRCode.AutoSize = true;
            this.QRCode.Location = new System.Drawing.Point(1244, 653);
            this.QRCode.Name = "QRCode";
            this.QRCode.Size = new System.Drawing.Size(215, 29);
            this.QRCode.TabIndex = 7;
            this.QRCode.Text = "QR Code Ausgabe";
            // 
            // tbQR
            // 
            this.tbQR.Location = new System.Drawing.Point(1244, 685);
            this.tbQR.Multiline = true;
            this.tbQR.Name = "tbQR";
            this.tbQR.Size = new System.Drawing.Size(363, 133);
            this.tbQR.TabIndex = 8;
            // 
            // tbDebug
            // 
            this.tbDebug.Location = new System.Drawing.Point(1249, 58);
            this.tbDebug.Multiline = true;
            this.tbDebug.Name = "tbDebug";
            this.tbDebug.Size = new System.Drawing.Size(359, 310);
            this.tbDebug.TabIndex = 9;
            // 
            // moveMentsLB
            // 
            this.moveMentsLB.FormattingEnabled = true;
            this.moveMentsLB.Location = new System.Drawing.Point(1631, 59);
            this.moveMentsLB.Name = "moveMentsLB";
            this.moveMentsLB.Size = new System.Drawing.Size(332, 754);
            this.moveMentsLB.TabIndex = 10;
            // 
            // btDoSomething
            // 
            this.btDoSomething.Location = new System.Drawing.Point(1631, 829);
            this.btDoSomething.Name = "btDoSomething";
            this.btDoSomething.Size = new System.Drawing.Size(223, 62);
            this.btDoSomething.TabIndex = 11;
            this.btDoSomething.Text = "Tu wat";
            this.btDoSomething.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1631, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 29);
            this.label1.TabIndex = 12;
            this.label1.Text = "Movements";
            // 
            // gloveBox
            // 
            this.gloveBox.Location = new System.Drawing.Point(12, 948);
            this.gloveBox.Name = "gloveBox";
            this.gloveBox.Size = new System.Drawing.Size(1226, 870);
            this.gloveBox.TabIndex = 2;
            this.gloveBox.TabStop = false;
            // 
            // lblglove
            // 
            this.lblglove.AutoSize = true;
            this.lblglove.Location = new System.Drawing.Point(12, 898);
            this.lblglove.Name = "lblglove";
            this.lblglove.Size = new System.Drawing.Size(247, 29);
            this.lblglove.TabIndex = 13;
            this.lblglove.Text = "Handschuherkennung";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1989, 1834);
            this.Controls.Add(this.lblglove);
            this.Controls.Add(this.gloveBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btDoSomething);
            this.Controls.Add(this.moveMentsLB);
            this.Controls.Add(this.tbDebug);
            this.Controls.Add(this.tbQR);
            this.Controls.Add(this.QRCode);
            this.Controls.Add(this.imageBoxQr);
            this.Controls.Add(this.lbDebug);
            this.Controls.Add(this.lblDemo);
            this.Controls.Add(this.imageBox1);
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxQr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gloveBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox imageBox1;
        private System.Windows.Forms.Label lblDemo;
        private System.Windows.Forms.Label lbDebug;
        private Emgu.CV.UI.ImageBox imageBoxQr;
        private System.Windows.Forms.Label QRCode;
        private System.Windows.Forms.TextBox tbQR;
        private System.Windows.Forms.TextBox tbDebug;
        private System.Windows.Forms.CheckedListBox moveMentsLB;
        private System.Windows.Forms.Button btDoSomething;
        private System.Windows.Forms.Label label1;
        private Emgu.CV.UI.ImageBox gloveBox;
        private System.Windows.Forms.Label lblglove;
    }
}


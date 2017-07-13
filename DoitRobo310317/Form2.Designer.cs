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
            this.tbClassified = new System.Windows.Forms.TextBox();
            this.btClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbClassified
            // 
            this.tbClassified.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbClassified.Location = new System.Drawing.Point(21, 87);
            this.tbClassified.Multiline = true;
            this.tbClassified.Name = "tbClassified";
            this.tbClassified.Size = new System.Drawing.Size(1261, 830);
            this.tbClassified.TabIndex = 24;
            // 
            // btClose
            // 
            this.btClose.Location = new System.Drawing.Point(12, 12);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(141, 52);
            this.btClose.TabIndex = 25;
            this.btClose.Text = "Schließen";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1301, 929);
            this.ControlBox = false;
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.tbClassified);
            this.Name = "Form2";
            this.Text = "Klassifizierung";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox tbClassified;
        private System.Windows.Forms.Button btClose;
    }
}
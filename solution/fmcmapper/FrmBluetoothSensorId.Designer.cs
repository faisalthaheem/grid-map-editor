namespace fmcmapper
{
    partial class FrmBluetoothSensorId
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
            this.a0 = new System.Windows.Forms.TextBox();
            this.a1 = new System.Windows.Forms.TextBox();
            this.a3 = new System.Windows.Forms.TextBox();
            this.a2 = new System.Windows.Forms.TextBox();
            this.a5 = new System.Windows.Forms.TextBox();
            this.a4 = new System.Windows.Forms.TextBox();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // a0
            // 
            this.a0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.a0.Location = new System.Drawing.Point(13, 13);
            this.a0.MaxLength = 2;
            this.a0.Name = "a0";
            this.a0.Size = new System.Drawing.Size(27, 20);
            this.a0.TabIndex = 0;
            // 
            // a1
            // 
            this.a1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.a1.Location = new System.Drawing.Point(46, 13);
            this.a1.MaxLength = 2;
            this.a1.Name = "a1";
            this.a1.Size = new System.Drawing.Size(27, 20);
            this.a1.TabIndex = 1;
            // 
            // a3
            // 
            this.a3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.a3.Location = new System.Drawing.Point(113, 13);
            this.a3.MaxLength = 2;
            this.a3.Name = "a3";
            this.a3.Size = new System.Drawing.Size(27, 20);
            this.a3.TabIndex = 3;
            // 
            // a2
            // 
            this.a2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.a2.Location = new System.Drawing.Point(80, 13);
            this.a2.MaxLength = 2;
            this.a2.Name = "a2";
            this.a2.Size = new System.Drawing.Size(27, 20);
            this.a2.TabIndex = 2;
            // 
            // a5
            // 
            this.a5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.a5.Location = new System.Drawing.Point(180, 13);
            this.a5.MaxLength = 2;
            this.a5.Name = "a5";
            this.a5.Size = new System.Drawing.Size(27, 20);
            this.a5.TabIndex = 5;
            // 
            // a4
            // 
            this.a4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.a4.Location = new System.Drawing.Point(147, 13);
            this.a4.MaxLength = 2;
            this.a4.Name = "a4";
            this.a4.Size = new System.Drawing.Size(27, 20);
            this.a4.TabIndex = 4;
            // 
            // btnOkay
            // 
            this.btnOkay.Location = new System.Drawing.Point(259, 13);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 6;
            this.btnOkay.Text = "Ok";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(340, 13);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmBluetoothSensorId
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 48);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.a5);
            this.Controls.Add(this.a4);
            this.Controls.Add(this.a3);
            this.Controls.Add(this.a2);
            this.Controls.Add(this.a1);
            this.Controls.Add(this.a0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmBluetoothSensorId";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bluetooth Sensor";
            this.Load += new System.EventHandler(this.FrmBluetoothSensorId_Load);
            this.Shown += new System.EventHandler(this.FrmBluetoothSensorId_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox a0;
        private System.Windows.Forms.TextBox a1;
        private System.Windows.Forms.TextBox a3;
        private System.Windows.Forms.TextBox a2;
        private System.Windows.Forms.TextBox a5;
        private System.Windows.Forms.TextBox a4;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.Button btnCancel;
    }
}
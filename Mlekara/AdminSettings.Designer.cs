namespace Mlekara
{
    partial class AdminSettings
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
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCompany = new System.Windows.Forms.Button();
            this.btnProbe = new System.Windows.Forms.Button();
            this.txtDeviceName = new System.Windows.Forms.TextBox();
            this.chkDeviceActive = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbDeviceId = new System.Windows.Forms.ComboBox();
            this.btnDevice = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cmbProbeDeviceId = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbProbeId = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chkProbeActive = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtProbeName = new System.Windows.Forms.TextBox();
            this.numMarker = new System.Windows.Forms.NumericUpDown();
            this.numMax = new System.Windows.Forms.NumericUpDown();
            this.numMin = new System.Windows.Forms.NumericUpDown();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMarker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMin)).BeginInit();
            this.SuspendLayout();
            // 
            // txtCompany
            // 
            this.txtCompany.Location = new System.Drawing.Point(6, 37);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(206, 20);
            this.txtCompany.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Naziv:";
            // 
            // btnCompany
            // 
            this.btnCompany.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompany.Location = new System.Drawing.Point(137, 63);
            this.btnCompany.Name = "btnCompany";
            this.btnCompany.Size = new System.Drawing.Size(75, 23);
            this.btnCompany.TabIndex = 2;
            this.btnCompany.Text = "Save";
            this.btnCompany.UseVisualStyleBackColor = true;
            this.btnCompany.Click += new System.EventHandler(this.btnCompany_Click);
            // 
            // btnProbe
            // 
            this.btnProbe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProbe.Location = new System.Drawing.Point(137, 111);
            this.btnProbe.Name = "btnProbe";
            this.btnProbe.Size = new System.Drawing.Size(75, 23);
            this.btnProbe.TabIndex = 7;
            this.btnProbe.Text = "Save";
            this.btnProbe.UseVisualStyleBackColor = true;
            this.btnProbe.Click += new System.EventHandler(this.btnProbe_Click);
            // 
            // txtDeviceName
            // 
            this.txtDeviceName.Location = new System.Drawing.Point(6, 72);
            this.txtDeviceName.Name = "txtDeviceName";
            this.txtDeviceName.Size = new System.Drawing.Size(206, 20);
            this.txtDeviceName.TabIndex = 5;
            // 
            // chkDeviceActive
            // 
            this.chkDeviceActive.AutoSize = true;
            this.chkDeviceActive.Location = new System.Drawing.Point(6, 98);
            this.chkDeviceActive.Name = "chkDeviceActive";
            this.chkDeviceActive.Size = new System.Drawing.Size(56, 17);
            this.chkDeviceActive.TabIndex = 8;
            this.chkDeviceActive.Text = "Active";
            this.chkDeviceActive.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtCompany);
            this.groupBox1.Controls.Add(this.btnCompany);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(218, 92);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Firma";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.cmbDeviceId);
            this.groupBox2.Controls.Add(this.btnDevice);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.chkDeviceActive);
            this.groupBox2.Controls.Add(this.txtDeviceName);
            this.groupBox2.Location = new System.Drawing.Point(12, 110);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(218, 136);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Device";
            // 
            // cmbDeviceId
            // 
            this.cmbDeviceId.FormattingEnabled = true;
            this.cmbDeviceId.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cmbDeviceId.Location = new System.Drawing.Point(6, 32);
            this.cmbDeviceId.Name = "cmbDeviceId";
            this.cmbDeviceId.Size = new System.Drawing.Size(35, 21);
            this.cmbDeviceId.TabIndex = 13;
            // 
            // btnDevice
            // 
            this.btnDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDevice.Location = new System.Drawing.Point(137, 107);
            this.btnDevice.Name = "btnDevice";
            this.btnDevice.Size = new System.Drawing.Size(75, 23);
            this.btnDevice.TabIndex = 21;
            this.btnDevice.Text = "Save";
            this.btnDevice.UseVisualStyleBackColor = true;
            this.btnDevice.Click += new System.EventHandler(this.btnDevice_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(21, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "ID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Naziv:";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.cmbProbeDeviceId);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.cmbProbeId);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.chkProbeActive);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.txtProbeName);
            this.groupBox4.Controls.Add(this.numMarker);
            this.groupBox4.Controls.Add(this.btnProbe);
            this.groupBox4.Controls.Add(this.numMax);
            this.groupBox4.Controls.Add(this.numMin);
            this.groupBox4.Location = new System.Drawing.Point(12, 252);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(218, 140);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Probe";
            // 
            // cmbProbeDeviceId
            // 
            this.cmbProbeDeviceId.FormattingEnabled = true;
            this.cmbProbeDeviceId.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cmbProbeDeviceId.Location = new System.Drawing.Point(77, 32);
            this.cmbProbeDeviceId.Name = "cmbProbeDeviceId";
            this.cmbProbeDeviceId.Size = new System.Drawing.Size(35, 21);
            this.cmbProbeDeviceId.TabIndex = 28;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(74, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Device:";
            // 
            // cmbProbeId
            // 
            this.cmbProbeId.FormattingEnabled = true;
            this.cmbProbeId.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.cmbProbeId.Location = new System.Drawing.Point(6, 32);
            this.cmbProbeId.Name = "cmbProbeId";
            this.cmbProbeId.Size = new System.Drawing.Size(35, 21);
            this.cmbProbeId.TabIndex = 26;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(21, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "ID:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(85, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Marker:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 56);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "Naziv:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(44, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Max:";
            // 
            // chkProbeActive
            // 
            this.chkProbeActive.AutoSize = true;
            this.chkProbeActive.Location = new System.Drawing.Point(143, 34);
            this.chkProbeActive.Name = "chkProbeActive";
            this.chkProbeActive.Size = new System.Drawing.Size(56, 17);
            this.chkProbeActive.TabIndex = 24;
            this.chkProbeActive.Text = "Active";
            this.chkProbeActive.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Min:";
            // 
            // txtProbeName
            // 
            this.txtProbeName.Location = new System.Drawing.Point(6, 72);
            this.txtProbeName.Name = "txtProbeName";
            this.txtProbeName.Size = new System.Drawing.Size(206, 20);
            this.txtProbeName.TabIndex = 23;
            // 
            // numMarker
            // 
            this.numMarker.Location = new System.Drawing.Point(88, 111);
            this.numMarker.Name = "numMarker";
            this.numMarker.Size = new System.Drawing.Size(35, 20);
            this.numMarker.TabIndex = 17;
            this.numMarker.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numMax
            // 
            this.numMax.Location = new System.Drawing.Point(47, 111);
            this.numMax.Name = "numMax";
            this.numMax.Size = new System.Drawing.Size(35, 20);
            this.numMax.TabIndex = 16;
            this.numMax.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numMin
            // 
            this.numMin.Location = new System.Drawing.Point(6, 111);
            this.numMin.Name = "numMin";
            this.numMin.Size = new System.Drawing.Size(35, 20);
            this.numMin.TabIndex = 12;
            this.numMin.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(155, 399);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // AdminSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(242, 434);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AdminSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Admin Settings";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMarker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCompany;
        private System.Windows.Forms.Button btnProbe;
        private System.Windows.Forms.TextBox txtDeviceName;
        private System.Windows.Forms.CheckBox chkDeviceActive;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbDeviceId;
        private System.Windows.Forms.Button btnDevice;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cmbProbeId;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkProbeActive;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtProbeName;
        private System.Windows.Forms.NumericUpDown numMarker;
        private System.Windows.Forms.NumericUpDown numMax;
        private System.Windows.Forms.NumericUpDown numMin;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cmbProbeDeviceId;
        private System.Windows.Forms.Label label3;
    }
}
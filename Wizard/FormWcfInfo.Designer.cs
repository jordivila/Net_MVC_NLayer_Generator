namespace VSIX_MVC_Layered_Wizard
{
    partial class FormWcfInfo
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.radioButtonNetTcpBinding = new System.Windows.Forms.RadioButton();
            this.radioButtonBasicHttpBinding = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonNetTcpBinding
            // 
            this.radioButtonNetTcpBinding.AutoSize = true;
            this.radioButtonNetTcpBinding.Checked = true;
            this.radioButtonNetTcpBinding.Location = new System.Drawing.Point(9, 63);
            this.radioButtonNetTcpBinding.Name = "radioButtonNetTcpBinding";
            this.radioButtonNetTcpBinding.Size = new System.Drawing.Size(96, 17);
            this.radioButtonNetTcpBinding.TabIndex = 0;
            this.radioButtonNetTcpBinding.TabStop = true;
            this.radioButtonNetTcpBinding.Text = "NetTcpBinding";
            this.radioButtonNetTcpBinding.UseVisualStyleBackColor = true;
            // 
            // radioButtonBasicHttpBinding
            // 
            this.radioButtonBasicHttpBinding.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonBasicHttpBinding.AutoSize = true;
            this.radioButtonBasicHttpBinding.Location = new System.Drawing.Point(266, 63);
            this.radioButtonBasicHttpBinding.Name = "radioButtonBasicHttpBinding";
            this.radioButtonBasicHttpBinding.Size = new System.Drawing.Size(106, 17);
            this.radioButtonBasicHttpBinding.TabIndex = 1;
            this.radioButtonBasicHttpBinding.Text = "BasicHttpBinding";
            this.radioButtonBasicHttpBinding.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(220, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Backend WCF Services Configuration";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.radioButtonBasicHttpBinding);
            this.panel1.Controls.Add(this.radioButtonNetTcpBinding);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(409, 107);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(356, 40);
            this.panel2.TabIndex = 0;
            // 
            // FormWcfInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "FormWcfInfo";
            this.Size = new System.Drawing.Size(418, 115);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonNetTcpBinding;
        private System.Windows.Forms.RadioButton radioButtonBasicHttpBinding;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}

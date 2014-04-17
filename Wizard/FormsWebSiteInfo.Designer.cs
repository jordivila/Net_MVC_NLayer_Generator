namespace VSIX_MVC_Layered_Wizard
{
    partial class FormsWebSiteInfo
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.MembershipAdminEmailLabel = new System.Windows.Forms.Label();
            this.MembershipAdminEmailTextbox = new System.Windows.Forms.MaskedTextBox();
            this.MembershipAdminPasswordLabel = new System.Windows.Forms.Label();
            this.MembershipAdminPasswordTextbox = new System.Windows.Forms.MaskedTextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.label7);
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(378, 40);
            this.panel1.TabIndex = 56;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(176, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Web Site Admin Configuration";
            // 
            // MembershipAdminEmailLabel
            // 
            this.MembershipAdminEmailLabel.AutoSize = true;
            this.MembershipAdminEmailLabel.Location = new System.Drawing.Point(3, 62);
            this.MembershipAdminEmailLabel.Name = "MembershipAdminEmailLabel";
            this.MembershipAdminEmailLabel.Size = new System.Drawing.Size(147, 13);
            this.MembershipAdminEmailLabel.TabIndex = 49;
            this.MembershipAdminEmailLabel.Text = "Website Admin Email Address";
            // 
            // MembershipAdminEmailTextbox
            // 
            this.MembershipAdminEmailTextbox.AsciiOnly = true;
            this.MembershipAdminEmailTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MembershipAdminEmailTextbox.Culture = new System.Globalization.CultureInfo("");
            this.MembershipAdminEmailTextbox.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.MembershipAdminEmailTextbox.Location = new System.Drawing.Point(178, 59);
            this.MembershipAdminEmailTextbox.Name = "MembershipAdminEmailTextbox";
            this.MembershipAdminEmailTextbox.RejectInputOnFirstFailure = true;
            this.MembershipAdminEmailTextbox.ResetOnSpace = false;
            this.MembershipAdminEmailTextbox.Size = new System.Drawing.Size(188, 20);
            this.MembershipAdminEmailTextbox.TabIndex = 41;
            // 
            // MembershipAdminPasswordLabel
            // 
            this.MembershipAdminPasswordLabel.AutoSize = true;
            this.MembershipAdminPasswordLabel.Location = new System.Drawing.Point(3, 88);
            this.MembershipAdminPasswordLabel.Name = "MembershipAdminPasswordLabel";
            this.MembershipAdminPasswordLabel.Size = new System.Drawing.Size(127, 13);
            this.MembershipAdminPasswordLabel.TabIndex = 47;
            this.MembershipAdminPasswordLabel.Text = "Website Admin Password";
            // 
            // MembershipAdminPasswordTextbox
            // 
            this.MembershipAdminPasswordTextbox.AsciiOnly = true;
            this.MembershipAdminPasswordTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MembershipAdminPasswordTextbox.Culture = new System.Globalization.CultureInfo("");
            this.MembershipAdminPasswordTextbox.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.MembershipAdminPasswordTextbox.Location = new System.Drawing.Point(177, 85);
            this.MembershipAdminPasswordTextbox.Name = "MembershipAdminPasswordTextbox";
            this.MembershipAdminPasswordTextbox.RejectInputOnFirstFailure = true;
            this.MembershipAdminPasswordTextbox.ResetOnSpace = false;
            this.MembershipAdminPasswordTextbox.Size = new System.Drawing.Size(188, 20);
            this.MembershipAdminPasswordTextbox.TabIndex = 43;
            // 
            // errorProvider1
            // 
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider1.ContainerControl = this;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.MembershipAdminPasswordTextbox);
            this.panel2.Controls.Add(this.MembershipAdminPasswordLabel);
            this.panel2.Controls.Add(this.MembershipAdminEmailLabel);
            this.panel2.Controls.Add(this.MembershipAdminEmailTextbox);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(433, 118);
            this.panel2.TabIndex = 57;
            // 
            // FormsWebSiteInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Name = "FormsWebSiteInfo";
            this.Size = new System.Drawing.Size(439, 128);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label MembershipAdminEmailLabel;
        private System.Windows.Forms.MaskedTextBox MembershipAdminEmailTextbox;
        private System.Windows.Forms.Label MembershipAdminPasswordLabel;
        private System.Windows.Forms.MaskedTextBox MembershipAdminPasswordTextbox;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Panel panel2;
    }
}

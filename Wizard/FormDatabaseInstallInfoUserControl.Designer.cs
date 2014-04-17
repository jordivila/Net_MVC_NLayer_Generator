namespace VSIX_MVC_Layered_Wizard
{
    partial class FormDatabaseInstallInfoUserControl
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
            this.serverLabel = new System.Windows.Forms.Label();
            this.ServerTextBox = new System.Windows.Forms.MaskedTextBox();
            this.MembershipDBNameLabel = new System.Windows.Forms.Label();
            this.MembershipDBNameTextbox = new System.Windows.Forms.MaskedTextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.LoggingDBNameLabel = new System.Windows.Forms.Label();
            this.LoggingDBNameTextbox = new System.Windows.Forms.MaskedTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.TokenPersistenceDBNameLabel = new System.Windows.Forms.Label();
            this.TokenPersistenceDBNameTextbox = new System.Windows.Forms.MaskedTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(4, 53);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(93, 13);
            this.serverLabel.TabIndex = 31;
            this.serverLabel.Text = "SQL Server Name";
            // 
            // ServerTextBox
            // 
            this.ServerTextBox.AsciiOnly = true;
            this.ServerTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ServerTextBox.Culture = new System.Globalization.CultureInfo("");
            this.ServerTextBox.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.ServerTextBox.Location = new System.Drawing.Point(171, 50);
            this.ServerTextBox.Name = "ServerTextBox";
            this.ServerTextBox.RejectInputOnFirstFailure = true;
            this.ServerTextBox.ResetOnSpace = false;
            this.ServerTextBox.Size = new System.Drawing.Size(188, 20);
            this.ServerTextBox.TabIndex = 20;
            // 
            // MembershipDBNameLabel
            // 
            this.MembershipDBNameLabel.AutoSize = true;
            this.MembershipDBNameLabel.Location = new System.Drawing.Point(4, 79);
            this.MembershipDBNameLabel.Name = "MembershipDBNameLabel";
            this.MembershipDBNameLabel.Size = new System.Drawing.Size(144, 13);
            this.MembershipDBNameLabel.TabIndex = 30;
            this.MembershipDBNameLabel.Text = "Membership Database Name";
            // 
            // MembershipDBNameTextbox
            // 
            this.MembershipDBNameTextbox.AsciiOnly = true;
            this.MembershipDBNameTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MembershipDBNameTextbox.Culture = new System.Globalization.CultureInfo("");
            this.MembershipDBNameTextbox.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.MembershipDBNameTextbox.Location = new System.Drawing.Point(171, 76);
            this.MembershipDBNameTextbox.Name = "MembershipDBNameTextbox";
            this.MembershipDBNameTextbox.RejectInputOnFirstFailure = true;
            this.MembershipDBNameTextbox.ResetOnSpace = false;
            this.MembershipDBNameTextbox.Size = new System.Drawing.Size(188, 20);
            this.MembershipDBNameTextbox.TabIndex = 21;
            // 
            // errorProvider1
            // 
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider1.ContainerControl = this;
            // 
            // LoggingDBNameLabel
            // 
            this.LoggingDBNameLabel.AutoSize = true;
            this.LoggingDBNameLabel.Location = new System.Drawing.Point(3, 105);
            this.LoggingDBNameLabel.Name = "LoggingDBNameLabel";
            this.LoggingDBNameLabel.Size = new System.Drawing.Size(125, 13);
            this.LoggingDBNameLabel.TabIndex = 27;
            this.LoggingDBNameLabel.Text = "Logging Database Name";
            // 
            // LoggingDBNameTextbox
            // 
            this.LoggingDBNameTextbox.AsciiOnly = true;
            this.LoggingDBNameTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LoggingDBNameTextbox.Culture = new System.Globalization.CultureInfo("");
            this.LoggingDBNameTextbox.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.LoggingDBNameTextbox.Location = new System.Drawing.Point(171, 102);
            this.LoggingDBNameTextbox.Name = "LoggingDBNameTextbox";
            this.LoggingDBNameTextbox.RejectInputOnFirstFailure = true;
            this.LoggingDBNameTextbox.ResetOnSpace = false;
            this.LoggingDBNameTextbox.Size = new System.Drawing.Size(189, 20);
            this.LoggingDBNameTextbox.TabIndex = 22;
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
            this.panel1.Size = new System.Drawing.Size(356, 40);
            this.panel1.TabIndex = 33;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(259, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Database and Web Site Admin Configruation";
            // 
            // TokenPersistenceDBNameLabel
            // 
            this.TokenPersistenceDBNameLabel.AutoSize = true;
            this.TokenPersistenceDBNameLabel.Location = new System.Drawing.Point(4, 131);
            this.TokenPersistenceDBNameLabel.Name = "TokenPersistenceDBNameLabel";
            this.TokenPersistenceDBNameLabel.Size = new System.Drawing.Size(126, 13);
            this.TokenPersistenceDBNameLabel.TabIndex = 35;
            this.TokenPersistenceDBNameLabel.Text = "Tokens  Database Name";
            // 
            // TokenPersistenceDBNameTextbox
            // 
            this.TokenPersistenceDBNameTextbox.AsciiOnly = true;
            this.TokenPersistenceDBNameTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TokenPersistenceDBNameTextbox.Culture = new System.Globalization.CultureInfo("");
            this.TokenPersistenceDBNameTextbox.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.TokenPersistenceDBNameTextbox.Location = new System.Drawing.Point(171, 128);
            this.TokenPersistenceDBNameTextbox.Name = "TokenPersistenceDBNameTextbox";
            this.TokenPersistenceDBNameTextbox.RejectInputOnFirstFailure = true;
            this.TokenPersistenceDBNameTextbox.ResetOnSpace = false;
            this.TokenPersistenceDBNameTextbox.Size = new System.Drawing.Size(189, 20);
            this.TokenPersistenceDBNameTextbox.TabIndex = 23;
            // 
            // FormDatabaseInstallInfoUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TokenPersistenceDBNameLabel);
            this.Controls.Add(this.TokenPersistenceDBNameTextbox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.serverLabel);
            this.Controls.Add(this.ServerTextBox);
            this.Controls.Add(this.MembershipDBNameLabel);
            this.Controls.Add(this.MembershipDBNameTextbox);
            this.Controls.Add(this.LoggingDBNameLabel);
            this.Controls.Add(this.LoggingDBNameTextbox);
            this.Name = "FormDatabaseInstallInfoUserControl";
            this.Size = new System.Drawing.Size(415, 173);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.MaskedTextBox ServerTextBox;
        private System.Windows.Forms.Label MembershipDBNameLabel;
        private System.Windows.Forms.MaskedTextBox MembershipDBNameTextbox;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label LoggingDBNameLabel;
        private System.Windows.Forms.MaskedTextBox LoggingDBNameTextbox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label TokenPersistenceDBNameLabel;
        private System.Windows.Forms.MaskedTextBox TokenPersistenceDBNameTextbox;
    }
}

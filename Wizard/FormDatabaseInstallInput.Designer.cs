namespace Wizard
{
    partial class FormDatabaseInstallInput
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
            this.DatabaseInstallNextButton = new System.Windows.Forms.Button();
            this.DatabaseInstallCancelButton = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.LoggingDBNameLabel = new System.Windows.Forms.Label();
            this.LoggingDBNameTextbox = new System.Windows.Forms.MaskedTextBox();
            this.MembershipAdminPasswordLabel = new System.Windows.Forms.Label();
            this.MembershipAdminPasswordTextbox = new System.Windows.Forms.MaskedTextBox();
            this.MembershipAdminEmailLabel = new System.Windows.Forms.Label();
            this.MembershipAdminEmailTextbox = new System.Windows.Forms.MaskedTextBox();
            this.MembershipDBNameLabel = new System.Windows.Forms.Label();
            this.MembershipDBNameTextbox = new System.Windows.Forms.MaskedTextBox();
            this.serverLabel = new System.Windows.Forms.Label();
            this.ServerTextBox = new System.Windows.Forms.MaskedTextBox();
            this.DevButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // DatabaseInstallNextButton
            // 
            this.DatabaseInstallNextButton.Location = new System.Drawing.Point(188, 151);
            this.DatabaseInstallNextButton.Name = "DatabaseInstallNextButton";
            this.DatabaseInstallNextButton.Size = new System.Drawing.Size(75, 23);
            this.DatabaseInstallNextButton.TabIndex = 6;
            this.DatabaseInstallNextButton.Text = "Next";
            this.DatabaseInstallNextButton.UseVisualStyleBackColor = true;
            this.DatabaseInstallNextButton.Click += new System.EventHandler(this.DatabaseInstallNextButton_Click);
            // 
            // DatabaseInstallCancelButton
            // 
            this.DatabaseInstallCancelButton.CausesValidation = false;
            this.DatabaseInstallCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DatabaseInstallCancelButton.Location = new System.Drawing.Point(301, 151);
            this.DatabaseInstallCancelButton.Name = "DatabaseInstallCancelButton";
            this.DatabaseInstallCancelButton.Size = new System.Drawing.Size(75, 23);
            this.DatabaseInstallCancelButton.TabIndex = 7;
            this.DatabaseInstallCancelButton.Text = "Cancel";
            this.DatabaseInstallCancelButton.UseVisualStyleBackColor = true;
            this.DatabaseInstallCancelButton.Click += new System.EventHandler(this.DatabaseInstallCancelButton_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider1.ContainerControl = this;
            // 
            // LoggingDBNameLabel
            // 
            this.LoggingDBNameLabel.AutoSize = true;
            this.LoggingDBNameLabel.Location = new System.Drawing.Point(12, 69);
            this.LoggingDBNameLabel.Name = "LoggingDBNameLabel";
            this.LoggingDBNameLabel.Size = new System.Drawing.Size(125, 13);
            this.LoggingDBNameLabel.TabIndex = 10;
            this.LoggingDBNameLabel.Text = "Logging Database Name";
            // 
            // LoggingDBNameTextbox
            // 
            this.LoggingDBNameTextbox.AsciiOnly = true;
            this.LoggingDBNameTextbox.Culture = new System.Globalization.CultureInfo("");
            this.LoggingDBNameTextbox.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.LoggingDBNameTextbox.Location = new System.Drawing.Point(187, 66);
            this.LoggingDBNameTextbox.Name = "LoggingDBNameTextbox";
            this.LoggingDBNameTextbox.RejectInputOnFirstFailure = true;
            this.LoggingDBNameTextbox.ResetOnSpace = false;
            this.LoggingDBNameTextbox.Size = new System.Drawing.Size(189, 20);
            this.LoggingDBNameTextbox.TabIndex = 3;
            // 
            // MembershipAdminPasswordLabel
            // 
            this.MembershipAdminPasswordLabel.AutoSize = true;
            this.MembershipAdminPasswordLabel.Location = new System.Drawing.Point(13, 121);
            this.MembershipAdminPasswordLabel.Name = "MembershipAdminPasswordLabel";
            this.MembershipAdminPasswordLabel.Size = new System.Drawing.Size(127, 13);
            this.MembershipAdminPasswordLabel.TabIndex = 12;
            this.MembershipAdminPasswordLabel.Text = "Website Admin Password";
            // 
            // MembershipAdminPasswordTextbox
            // 
            this.MembershipAdminPasswordTextbox.AsciiOnly = true;
            this.MembershipAdminPasswordTextbox.Culture = new System.Globalization.CultureInfo("");
            this.MembershipAdminPasswordTextbox.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.MembershipAdminPasswordTextbox.Location = new System.Drawing.Point(187, 118);
            this.MembershipAdminPasswordTextbox.Name = "MembershipAdminPasswordTextbox";
            this.MembershipAdminPasswordTextbox.RejectInputOnFirstFailure = true;
            this.MembershipAdminPasswordTextbox.ResetOnSpace = false;
            this.MembershipAdminPasswordTextbox.Size = new System.Drawing.Size(188, 20);
            this.MembershipAdminPasswordTextbox.TabIndex = 5;
            // 
            // MembershipAdminEmailLabel
            // 
            this.MembershipAdminEmailLabel.AutoSize = true;
            this.MembershipAdminEmailLabel.Location = new System.Drawing.Point(13, 95);
            this.MembershipAdminEmailLabel.Name = "MembershipAdminEmailLabel";
            this.MembershipAdminEmailLabel.Size = new System.Drawing.Size(147, 13);
            this.MembershipAdminEmailLabel.TabIndex = 14;
            this.MembershipAdminEmailLabel.Text = "Website Admin Email Address";
            // 
            // MembershipAdminEmailTextbox
            // 
            this.MembershipAdminEmailTextbox.AsciiOnly = true;
            this.MembershipAdminEmailTextbox.Culture = new System.Globalization.CultureInfo("");
            this.MembershipAdminEmailTextbox.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.MembershipAdminEmailTextbox.Location = new System.Drawing.Point(188, 92);
            this.MembershipAdminEmailTextbox.Name = "MembershipAdminEmailTextbox";
            this.MembershipAdminEmailTextbox.RejectInputOnFirstFailure = true;
            this.MembershipAdminEmailTextbox.ResetOnSpace = false;
            this.MembershipAdminEmailTextbox.Size = new System.Drawing.Size(188, 20);
            this.MembershipAdminEmailTextbox.TabIndex = 4;
            // 
            // MembershipDBNameLabel
            // 
            this.MembershipDBNameLabel.AutoSize = true;
            this.MembershipDBNameLabel.Location = new System.Drawing.Point(12, 43);
            this.MembershipDBNameLabel.Name = "MembershipDBNameLabel";
            this.MembershipDBNameLabel.Size = new System.Drawing.Size(144, 13);
            this.MembershipDBNameLabel.TabIndex = 16;
            this.MembershipDBNameLabel.Text = "Membership Database Name";
            // 
            // MembershipDBNameTextbox
            // 
            this.MembershipDBNameTextbox.AsciiOnly = true;
            this.MembershipDBNameTextbox.Culture = new System.Globalization.CultureInfo("");
            this.MembershipDBNameTextbox.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.MembershipDBNameTextbox.Location = new System.Drawing.Point(187, 40);
            this.MembershipDBNameTextbox.Name = "MembershipDBNameTextbox";
            this.MembershipDBNameTextbox.RejectInputOnFirstFailure = true;
            this.MembershipDBNameTextbox.ResetOnSpace = false;
            this.MembershipDBNameTextbox.Size = new System.Drawing.Size(188, 20);
            this.MembershipDBNameTextbox.TabIndex = 2;
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(12, 17);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(93, 13);
            this.serverLabel.TabIndex = 18;
            this.serverLabel.Text = "SQL Server Name";
            // 
            // ServerTextBox
            // 
            this.ServerTextBox.AsciiOnly = true;
            this.ServerTextBox.Culture = new System.Globalization.CultureInfo("");
            this.ServerTextBox.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.ServerTextBox.Location = new System.Drawing.Point(187, 14);
            this.ServerTextBox.Name = "ServerTextBox";
            this.ServerTextBox.RejectInputOnFirstFailure = true;
            this.ServerTextBox.ResetOnSpace = false;
            this.ServerTextBox.Size = new System.Drawing.Size(188, 20);
            this.ServerTextBox.TabIndex = 1;
            // 
            // DevButton
            // 
            this.DevButton.Location = new System.Drawing.Point(16, 150);
            this.DevButton.Name = "DevButton";
            this.DevButton.Size = new System.Drawing.Size(75, 23);
            this.DevButton.TabIndex = 19;
            this.DevButton.Text = "Dev Data";
            this.DevButton.UseVisualStyleBackColor = true;
            this.DevButton.Click += new System.EventHandler(this.DevButton_Click);
            // 
            // FormDatabaseInstallInput
            // 
            this.AcceptButton = this.DatabaseInstallNextButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.DatabaseInstallCancelButton;
            this.ClientSize = new System.Drawing.Size(411, 186);
            this.Controls.Add(this.DevButton);
            this.Controls.Add(this.serverLabel);
            this.Controls.Add(this.ServerTextBox);
            this.Controls.Add(this.MembershipDBNameLabel);
            this.Controls.Add(this.MembershipDBNameTextbox);
            this.Controls.Add(this.MembershipAdminEmailLabel);
            this.Controls.Add(this.MembershipAdminEmailTextbox);
            this.Controls.Add(this.MembershipAdminPasswordLabel);
            this.Controls.Add(this.MembershipAdminPasswordTextbox);
            this.Controls.Add(this.LoggingDBNameLabel);
            this.Controls.Add(this.LoggingDBNameTextbox);
            this.Controls.Add(this.DatabaseInstallCancelButton);
            this.Controls.Add(this.DatabaseInstallNextButton);
            this.Name = "FormDatabaseInstallInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DatabaseInstallInputForm";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DatabaseInstallNextButton;
        private System.Windows.Forms.Button DatabaseInstallCancelButton;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.MaskedTextBox ServerTextBox;
        private System.Windows.Forms.Label MembershipDBNameLabel;
        private System.Windows.Forms.MaskedTextBox MembershipDBNameTextbox;
        private System.Windows.Forms.Label MembershipAdminEmailLabel;
        private System.Windows.Forms.MaskedTextBox MembershipAdminEmailTextbox;
        private System.Windows.Forms.Label MembershipAdminPasswordLabel;
        private System.Windows.Forms.MaskedTextBox MembershipAdminPasswordTextbox;
        private System.Windows.Forms.Label LoggingDBNameLabel;
        private System.Windows.Forms.MaskedTextBox LoggingDBNameTextbox;
        private System.Windows.Forms.Button DevButton;
    }
}
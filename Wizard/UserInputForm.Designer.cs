namespace CustomWizard
{
    partial class UserInputForm
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
            this.namespaceButton = new System.Windows.Forms.Button();
            this.namespaceContainer = new System.Windows.Forms.Panel();
            this.namespaceTextBox = new System.Windows.Forms.MaskedTextBox();
            this.namespaceLabelContainer = new System.Windows.Forms.Panel();
            this.namespaceLabel = new System.Windows.Forms.Label();
            this.namespaceContainer.SuspendLayout();
            this.namespaceLabelContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // namespaceButton
            // 
            this.namespaceButton.Location = new System.Drawing.Point(179, 112);
            this.namespaceButton.Name = "namespaceButton";
            this.namespaceButton.Size = new System.Drawing.Size(75, 23);
            this.namespaceButton.TabIndex = 1;
            this.namespaceButton.Text = "Apply";
            this.namespaceButton.UseVisualStyleBackColor = true;
            this.namespaceButton.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // namespaceContainer
            // 
            this.namespaceContainer.Controls.Add(this.namespaceTextBox);
            this.namespaceContainer.Controls.Add(this.namespaceLabelContainer);
            this.namespaceContainer.Location = new System.Drawing.Point(12, 12);
            this.namespaceContainer.Name = "namespaceContainer";
            this.namespaceContainer.Size = new System.Drawing.Size(247, 68);
            this.namespaceContainer.TabIndex = 2;
            // 
            // namespaceTextBox
            // 
            this.namespaceTextBox.AsciiOnly = true;
            this.namespaceTextBox.Culture = new System.Globalization.CultureInfo("");
            this.namespaceTextBox.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.namespaceTextBox.Location = new System.Drawing.Point(3, 45);
            this.namespaceTextBox.Name = "namespaceTextBox";
            this.namespaceTextBox.RejectInputOnFirstFailure = true;
            this.namespaceTextBox.ResetOnSpace = false;
            this.namespaceTextBox.Size = new System.Drawing.Size(239, 20);
            this.namespaceTextBox.TabIndex = 3;
            // 
            // namespaceLabelContainer
            // 
            this.namespaceLabelContainer.Controls.Add(this.namespaceLabel);
            this.namespaceLabelContainer.ForeColor = System.Drawing.Color.Black;
            this.namespaceLabelContainer.Location = new System.Drawing.Point(3, 3);
            this.namespaceLabelContainer.Name = "namespaceLabelContainer";
            this.namespaceLabelContainer.Size = new System.Drawing.Size(239, 33);
            this.namespaceLabelContainer.TabIndex = 2;
            // 
            // namespaceLabel
            // 
            this.namespaceLabel.AutoSize = true;
            this.namespaceLabel.Location = new System.Drawing.Point(3, 10);
            this.namespaceLabel.Name = "namespaceLabel";
            this.namespaceLabel.Size = new System.Drawing.Size(132, 13);
            this.namespaceLabel.TabIndex = 0;
            this.namespaceLabel.Text = "1.- Type Main Namespace";
            // 
            // UserInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 280);
            this.Controls.Add(this.namespaceContainer);
            this.Controls.Add(this.namespaceButton);
            this.Name = "UserInputForm";
            this.Text = "UserInputForm";
            this.namespaceContainer.ResumeLayout(false);
            this.namespaceContainer.PerformLayout();
            this.namespaceLabelContainer.ResumeLayout(false);
            this.namespaceLabelContainer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button namespaceButton;
        private System.Windows.Forms.Panel namespaceContainer;
        private System.Windows.Forms.Panel namespaceLabelContainer;
        private System.Windows.Forms.Label namespaceLabel;
        private System.Windows.Forms.MaskedTextBox namespaceTextBox;
    }
}
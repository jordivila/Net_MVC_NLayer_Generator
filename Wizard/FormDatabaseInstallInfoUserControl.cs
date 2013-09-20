using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace Wizard
{
    public partial class FormDatabaseInstallInfoUserControl : UserControl
    {
        public FormDatabaseInstallInfoUserControl()
        {
            InitializeComponent();

            this.ServerTextBox_Init();
            this.MembershipDBNameTextbox_Init();
            this.LoggingDBNameTextbox_Init();
        }

        private bool IsValidServerName(string value)
        {
            return Regex.IsMatch(value.ToString(), @"^[a-zA-Z0-9_]+$");
        }

        private bool IsValidDatabaseName(string value)
        {
            return Regex.IsMatch(value.ToString(), @"^[a-zA-Z0-9_ ]+$");
        }

        private bool IsValidEmailAddress(string value)
        {
            try
            {
                new MailAddress(value);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private void ErrorClear(object sender)
        {
            errorProvider1.SetError((Control)sender, string.Empty);
        }

        private void TextBoxRequiredCommonInit(MaskedTextBox textBox,
                                        string initialText,
                                        string initialError,
                                        Type validtingType,
                                        Action<object, TypeValidationEventArgs> IsValidCustomized = null)
        {
            textBox.Text = initialText;
            textBox.Mask = string.Empty;
            textBox.ValidatingType = validtingType;
            textBox.KeyDown += delegate(object sender, KeyEventArgs e) { this.ErrorClear(sender); };
            textBox.TypeValidationCompleted += delegate(object sender, TypeValidationEventArgs e)
            {
                e.Cancel = false;

                if ((!e.IsValidInput) || (string.IsNullOrEmpty(e.ReturnValue.ToString())))
                {
                    errorProvider1.SetError(textBox, initialError);
                    e.Cancel = true;
                }
                else
                {
                    if (IsValidCustomized != null)
                    {
                        IsValidCustomized(sender, e);
                    }
                }
            };
        }

        private void ServerTextBox_Init()
        {
            this.TextBoxRequiredCommonInit(this.ServerTextBox,
                                    FormDatabaseInstallInputResources.ServerName,
                                    FormDatabaseInstallInputResources.ServerNameInvalid,
                                    typeof(System.String),
                                    delegate(object sender, TypeValidationEventArgs e)
                                    {
                                        if (!this.IsValidServerName(e.ReturnValue.ToString()))
                                        {
                                            errorProvider1.SetError((Control)sender, FormDatabaseInstallInputResources.ServerNameInvalid);
                                            e.Cancel = true;
                                        }
                                    });
        }

        private void MembershipDBNameTextbox_Init()
        {
            this.TextBoxRequiredCommonInit(this.MembershipDBNameTextbox,
                                    FormDatabaseInstallInputResources.MembershipDatabaseName,
                                    FormDatabaseInstallInputResources.DatabaseInvalidName,
                                    typeof(System.String),
                                    delegate(object sender, TypeValidationEventArgs e)
                                    {
                                        if (!this.IsValidDatabaseName(e.ReturnValue.ToString()))
                                        {
                                            errorProvider1.SetError((Control)sender, FormDatabaseInstallInputResources.DatabaseInvalidName);
                                            e.Cancel = true;
                                        }
                                    });
        }

        private void LoggingDBNameTextbox_Init()
        {
            this.TextBoxRequiredCommonInit(this.LoggingDBNameTextbox,
                                    FormDatabaseInstallInputResources.LoggingDatabaseName,
                                    FormDatabaseInstallInputResources.DatabaseInvalidName,
                                    typeof(System.String),
                                    delegate(object sender, TypeValidationEventArgs e)
                                    {
                                        if (!this.IsValidDatabaseName(e.ReturnValue.ToString()))
                                        {
                                            errorProvider1.SetError((Control)sender, FormDatabaseInstallInputResources.DatabaseInvalidName);
                                            e.Cancel = true;
                                        }
                                    });
        }

        public FormDatabaseInstallInfoData ValueFake(object sender, EventArgs e)
        {
            return new FormDatabaseInstallInfoData()
            {
                CreateDatabaseAccepted = false,
                ServerName = FormDatabaseInstallInputResources.ServerName,
                MembershipDBName = FormDatabaseInstallInputResources.MembershipDatabaseName,
                LoggingDBName = FormDatabaseInstallInputResources.LoggingDatabaseName
            };
        }

        public FormDatabaseInstallInfoData Value(object sender, EventArgs e)
        {
            return new FormDatabaseInstallInfoData()
            {
                CreateDatabaseAccepted = true,
                ServerName = this.ServerTextBox.Text,
                MembershipDBName = this.MembershipDBNameTextbox.Text,
                LoggingDBName = this.LoggingDBNameTextbox.Text,
            };
        }

        public void DevButton_Click2()
        {
            this.ServerTextBox.Text = "IO_JV";
            this.MembershipDBNameTextbox.Text = "CurlyDevelopmentMembership";
            this.LoggingDBNameTextbox.Text = "CurlyDevelopmentLogging";
        }


        public void DevButton_Click(object sender, EventArgs e)
        {
            this.ServerTextBox.Text = "IO_JV";
            this.MembershipDBNameTextbox.Text = "CurlyDevelopmentMembership";
            this.LoggingDBNameTextbox.Text = "CurlyDevelopmentLogging";
        }
    }

    public class FormDatabaseInstallInfoData
    {
        public bool CreateDatabaseAccepted { get; set; }

        public string ServerName { get; set; }
        public string MembershipDBName { get; set; }
        public string LoggingDBName { get; set; }
    }

}

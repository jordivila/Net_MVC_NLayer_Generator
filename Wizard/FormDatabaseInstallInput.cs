using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Wizard
{
    public partial class FormDatabaseInstallInput : FormBase
    {
        

        public delegate void CompletedEventHandler(object sender, DatabaseInstallEventArgs e);
        public event CompletedEventHandler onCompleted;
        public event CompletedEventHandler onCancelled;

        public FormDatabaseInstallInput()
        {
            InitializeComponent();

            this.DevButton.Visible = this.IsDebugMode;

            this.ServerTextBox_Init();
            this.MembershipDBNameTextbox_Init();
            this.LoggingDBNameTextbox_Init();
            this.MembershipAdminEmailTextbox_Init();
            this.MembershipAdminPasswordTextbox_Init();
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

        private void MembershipAdminEmailTextbox_Init()
        {
            this.TextBoxRequiredCommonInit(this.MembershipAdminEmailTextbox,
                                    FormDatabaseInstallInputResources.EmailAddressValid,
                                    FormDatabaseInstallInputResources.EmailAddressInvalid,
                                    typeof(System.String),
                                    delegate(object sender, TypeValidationEventArgs e)
                                    {
                                        if (!this.IsValidEmailAddress(e.ReturnValue.ToString()))
                                        {
                                            errorProvider1.SetError((Control)sender, FormDatabaseInstallInputResources.EmailAddressInvalid);
                                            e.Cancel = true;
                                        }
                                    });
        }
        
        private void MembershipAdminPasswordTextbox_Init()
        {
            this.TextBoxRequiredCommonInit(this.MembershipAdminPasswordTextbox,
                                    FormDatabaseInstallInputResources.AdminPassword,
                                    FormDatabaseInstallInputResources.PasswordInvalid,
                                    typeof(System.String),
                                    null);
        }

        public DatabaseInstallInfo GetFake()
        {
            return new DatabaseInstallInfo()
            {
                CreateDatabaseAccepted = false,
                ServerName = FormDatabaseInstallInputResources.ServerName,
                MembershipDBName = FormDatabaseInstallInputResources.MembershipDatabaseName,
                LoggingDBName = FormDatabaseInstallInputResources.LoggingDatabaseName,
                WebSiteAdminEmailAddress = FormDatabaseInstallInputResources.EmailAddressValid,
                WebSiteAdminPassword = FormDatabaseInstallInputResources.AdminPassword
            };
        }

        private void DatabaseInstallCancelButton_Click(object sender, EventArgs e)
        {
            this.Hide();

            if (this.onCancelled != null)
            {
                this.onCancelled(this, new DatabaseInstallEventArgs()
                {
                    DBInfo = this.GetFake()
                });

            }
        }

        private void DatabaseInstallNextButton_Click(object sender, EventArgs e)
        {
            this.Hide();

            if (this.ValidateChildren(ValidationConstraints.ImmediateChildren))
            {
                if (this.onCompleted != null)
                {
                    this.onCompleted(this, new DatabaseInstallEventArgs()
                    {
                        DBInfo = new DatabaseInstallInfo()
                        {
                            CreateDatabaseAccepted = true,
                            ServerName = this.ServerTextBox.Text,
                            MembershipDBName = this.MembershipDBNameTextbox.Text,
                            LoggingDBName = this.LoggingDBNameTextbox.Text,
                            WebSiteAdminEmailAddress = this.MembershipAdminEmailTextbox.Text,
                            WebSiteAdminPassword = this.MembershipAdminPasswordTextbox.Text
                        }
                    });
                }
            }
        }

        private void DevButton_Click(object sender, EventArgs e)
        {
            this.ServerTextBox.Text = "IO_JV";
            this.MembershipDBNameTextbox.Text = "CurlyDevelopmentMembership";
            this.MembershipAdminEmailTextbox.Text = "admin@admin.com";
            this.MembershipAdminPasswordTextbox.Text = "123456";
            this.LoggingDBNameTextbox.Text = "CurlyDevelopmentLogging";
        }
    }


    public class DatabaseInstallEventArgs : EventArgs
    {
        public DatabaseInstallInfo DBInfo { get; set; }

        public DatabaseInstallEventArgs() : base() { }
    }

}

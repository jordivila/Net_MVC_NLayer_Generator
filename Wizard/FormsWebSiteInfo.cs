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
    public partial class FormsWebSiteInfo : UserControl
    {
        public FormsWebSiteInfo()
        {
            InitializeComponent();

            this.MembershipAdminEmailTextbox_Init();
            this.MembershipAdminPasswordTextbox_Init();
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

        public FormsWebSiteInfoData ValueFake(object sender, EventArgs e)
        {
            return new FormsWebSiteInfoData()
            {
                WebSiteAdminEmailAddress = FormDatabaseInstallInputResources.EmailAddressValid,
                WebSiteAdminPassword = FormDatabaseInstallInputResources.AdminPassword
            };
        }

        public FormsWebSiteInfoData Value(object sender, EventArgs e)
        {
            return new FormsWebSiteInfoData()
            {
                WebSiteAdminEmailAddress = this.MembershipAdminEmailTextbox.Text,
                WebSiteAdminPassword = this.MembershipAdminPasswordTextbox.Text
            };
        }

        public void DevButton_Click(object sender, EventArgs e)
        {
            this.MembershipAdminEmailTextbox.Text = "admin@admin.com";
            this.MembershipAdminPasswordTextbox.Text = "123456";
        }


    }

    public class FormsWebSiteInfoData
    {
        public string WebSiteAdminEmailAddress { get; set; }
        public string WebSiteAdminPassword { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wizard;

namespace Wizard
{
    public partial class FormInfo : FormBase
    {
        public delegate void CompletedEventHandler(object sender, FormInfoEventArgs e);
        public event CompletedEventHandler onCompleted;
        public event CompletedEventHandler onCancelled;

        public FormInfo()
        {
            InitializeComponent();
        }

        public void ButtonCancel_Click(object sender, EventArgs e)
        {
            if (this.onCancelled != null)
            {
                this.onCancelled(this, new FormInfoEventArgs(
                                                    this.formDatabaseInstallInfoUserControl1.Value(this, new EventArgs()),
                                                    this.formWcfInfo1.Value(this, new EventArgs()),
                                                    this.formsWebSiteInfo1.Value(this, new EventArgs()))
                    );
            }
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren(ValidationConstraints.ImmediateChildren))
            {
                if (this.onCompleted != null)
                {
                    this.onCompleted(this, new FormInfoEventArgs(
                                                    this.formDatabaseInstallInfoUserControl1.Value(this, new EventArgs()),
                                                    this.formWcfInfo1.Value(this, new EventArgs()),
                                                    this.formsWebSiteInfo1.Value(this, new EventArgs())));
                }
            }
        }

        private void ButtonDev_Click(object sender, EventArgs e)
        {
            this.formDatabaseInstallInfoUserControl1.DevButton_Click2();
            this.formsWebSiteInfo1.DevButton_Click(this, new EventArgs());
            this.formWcfInfo1.DevButton_Click(this, new EventArgs());
        }
    }

    public class FormInfoEventArgs : EventArgs
    {
        public WebSiteConfigurationCustomized WebSiteConfig { get; set; }

        public FormInfoEventArgs(FormDatabaseInstallInfoData DBInfo,
                                    FormsWcfInfoData WcfInfo,
                                    FormsWebSiteInfoData WebSiteData)
            : base()
        {
            this.WebSiteConfig = new WebSiteConfigurationCustomized(DBInfo, WcfInfo, WebSiteData);
        }
    }

    public class WebSiteConfigurationCustomized
    {
        public FormDatabaseInstallInfoData DBInfo { get; set; }
        public FormsWcfInfoData WcfInfo { get; set; }
        public FormsWebSiteInfoData WebSiteData { get; set; }

        public WebSiteConfigurationCustomized(FormDatabaseInstallInfoData DBInfo,
                                    FormsWcfInfoData WcfInfo,
                                    FormsWebSiteInfoData WebSiteData)
        {
            this.DBInfo = DBInfo;
            this.WcfInfo = WcfInfo;
            this.WebSiteData = WebSiteData;
        }
    }
}
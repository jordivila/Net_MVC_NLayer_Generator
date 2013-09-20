using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;

namespace Wizard
{
    public partial class FormWcfInfo : UserControl
    {
        public FormWcfInfo()
        {
            InitializeComponent();
        }

        public FormsWcfInfoData ValueFake(object sender, EventArgs e)
        {
            return new FormsWcfInfoData(typeof(NetTcpBinding));
        }

        public FormsWcfInfoData Value(object sender, EventArgs e)
        {
            if (this.radioButtonNetTcpBinding.Checked)
            {
                return new FormsWcfInfoData(typeof(NetTcpBinding));
            }
            else
            {
                return new FormsWcfInfoData(typeof(BasicHttpBinding));
            }
        }

        public void DevButton_Click(object sender, EventArgs e)
        {
            this.radioButtonNetTcpBinding.Checked = true;
        }

    }

    public class FormsWcfInfoData
    {
        public Type BindingSelected { get; set; }
        public bool IsNetTcpBinding { get; private set; }
        public bool IsBasicHttpBinding { get; private set; }
        public string Protocol { get; private set; }
        public string BindingTypeName { get; private set; }
        public string BindingUserRequestModelAtServer { get; private set; }
        public string BindingUserRequestModelSelected { get; private set; }
        public readonly string BindingUserRequestModelNetTcpAliasName = "UserRequestModelNetTcp";
        public readonly string BindingUserRequestModelHttpAliasName = "UserRequestModelHttp";

        public FormsWcfInfoData(Type bindingSelected)
        {
            this.BindingSelected = bindingSelected;
            this.IsNetTcpBinding = bindingSelected == typeof(NetTcpBinding);
            this.IsBasicHttpBinding = bindingSelected == typeof(BasicHttpBinding);
            this.Protocol = this.IsNetTcpBinding ? "net.tcp" : "http";
            this.BindingTypeName = this.IsNetTcpBinding ? "netTcpBinding" : "basicHttpBinding";
            this.BindingUserRequestModelSelected = this.IsNetTcpBinding ? this.BindingUserRequestModelNetTcpAliasName : this.BindingUserRequestModelHttpAliasName;
        }
    }

}

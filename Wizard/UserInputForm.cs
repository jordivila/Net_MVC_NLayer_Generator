using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CustomWizard
{
    public partial class UserInputForm : Form
    {
        private string customMessage;

        public UserInputForm()
        {
            InitializeComponent();
        }

        public string get_CustomMessage()
        {
            return customMessage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            customMessage = namespaceTextBox.Text;

            this.Dispose();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            customMessage = namespaceTextBox.Text;

            this.Dispose();
        }




    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsAddressBook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void addEntryButton_Click(object sender, EventArgs e)
        {

        }

        private void nameTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                e.Cancel = true;
                nameTextBox.Focus();
                nameErrorProvider.SetError(nameTextBox, "Please enter a name for your entry");
            }
            else
            {
                nameErrorProvider.SetError(nameTextBox, null);
            }
        }

        private void emailAddressTextBox_Validating(object sender, CancelEventArgs e)
        {
            // Checks whether the textbox input matches the Regex pattern for an email address.
            if (!Regex.IsMatch(emailAddressTextBox.Text, @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$"))
            {
                e.Cancel = true;
                emailAddressTextBox.Focus();
                emailAddressErrorProvider.SetError(emailAddressTextBox, "Please enter a valid email address");
            }
            else
            {
                emailAddressErrorProvider.SetError(emailAddressTextBox, null);
            }
        }
    }
}

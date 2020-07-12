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
            ValidateRegex(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$", emailAddressTextBox, emailAddressErrorProvider, "Please enter a valid email address", e);
        }

        private void phoneNumberTextBox_Validating(object sender, CancelEventArgs e)
        {
            ValidateRegex(@"^[0-9]{10}$", phoneNumberTextBox, phoneNumberErrorProvider, "Please enter a ten digit phone number, including area code, without dashes or spaces.", e);
        }
        private void ValidateRegex(string regexPattern, TextBox textBoxToValidate, ErrorProvider errorProvider, string errorProviderMessage, CancelEventArgs e)
        {
            // Checks whether the textbox input matches the Regex pattern.
            if (!Regex.IsMatch(textBoxToValidate.Text, regexPattern))
            {
                e.Cancel = true;
                textBoxToValidate.Focus();
                errorProvider.SetError(textBoxToValidate, errorProviderMessage);
            }
            else
            {
                errorProvider.SetError(textBoxToValidate, null);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.IO.File;

namespace WinFormsAddressBook
{
    public partial class Form1 : Form
    {
        readonly List<Entry> entryList = new List<Entry>();
        public Form1()
        {
            InitializeComponent();
        }

        #region 1 Event handlers
        #region 1.1. Click event handlers
        private void AddEntryButton_Click(object sender, EventArgs e)
        {
            if (ValidateChildren(ValidationConstraints.Enabled))
            {
                Entry entry = CreateNewEntry();
                entryList.Add(entry);
                warningLabel.ForeColor = Color.Black;
                ClearFields();
                warningLabel.Text = "Added entry successfully.";
            }
            else
            {
                warningLabel.ForeColor = Color.Red;
            }
        }
        #endregion
        #region 1.2 Validation event handlers
        private void NameTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                string warningString = "Name is required";
                e.Cancel = true;
                nameErrorProvider.SetError(nameTextBox, warningString);
                warningLabel.Text = warningString;
            }
            else
            {
                nameErrorProvider.SetError(nameTextBox, null);
            }
        }

        private void EmailAddressTextBox_Validating(object sender, CancelEventArgs e)
        {
            ValidateRegex(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$", emailAddressTextBox, emailAddressErrorProvider, "Email address is invalid", e);
        }

        private void PhoneNumberTextBox_Validating(object sender, CancelEventArgs e)
        {
            ValidateRegex(@"^[0-9]{10}$", phoneNumberTextBox, phoneNumberErrorProvider, "Phone number must be 10 numbers, no dashes.", e);
        }
        #endregion
        #region 1.3 Form close event handler
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Add entries from the entries list to the file phone-book.txt.
            AddEntries(entryList, "phone-book.txt");
            //            WriteLine("The data was stored properly. Here it is!");
            // Open phone-book.txt with Notepad.
            OpenTextFileWithNotepad("phone-book.txt");
        }

        #endregion
        #endregion

        #region 2 Helper methods
        private Entry CreateNewEntry()
        {
            return new Entry { Name = nameTextBox.Text, EmailAddress = emailAddressTextBox.Text, PhoneNumber = phoneNumberTextBox.Text };
        }
        private void ClearFields()
        {
            nameTextBox.Text = ""; emailAddressTextBox.Text = ""; phoneNumberTextBox.Text = "";
        }

        private void ValidateRegex(string regexPattern, TextBox textBoxToValidate, ErrorProvider errorProvider, string errorProviderMessage, CancelEventArgs e)
        {
            // Checks whether the textbox input matches the Regex pattern.
            if (!Regex.IsMatch(textBoxToValidate.Text, regexPattern))
            {
                e.Cancel = true;
                errorProvider.SetError(textBoxToValidate, errorProviderMessage);
                warningLabel.Text = errorProviderMessage;
            }
            else
            {
                errorProvider.SetError(textBoxToValidate, null);
            }
        }

        private static void AddEntries(List<Entry> fromEntryList, string toFile)
        {
            // Create a file with the name passed in as a string parameter, and a StreamWriter object to write to it. 
            using StreamWriter writer = CreateText(toFile);
            // With the StreamWriter, for each entry in the entry list passed in...
            fromEntryList.ForEach(entry =>
            {
                // ...get the entry number
                int entryNumber = fromEntryList.IndexOf(entry);
                // ...and write that entry's number, name, address, and phone number to the file on a single line.
                writer.WriteLine($"Entry {entryNumber + 1}:\tName: {entry.Name},\temail address: {entry.EmailAddress},\tphone number: *{entry.PhoneNumber}*");
            });
        }

        private static void OpenTextFileWithNotepad(string fileName)
        {
            Process.Start("notepad.exe", fileName);
        }
        #endregion

    }
}

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
            // If all data is valid...
            if (ValidateChildren(ValidationConstraints.Enabled))
            {
                // Create a new entry
                Entry entry = CreateNewEntry();
                // add it to our entry list
                entryList.Add(entry);
                // set the warning label color to black
                warningLabel.ForeColor = Color.Black;
                // clear the textboxes.
                ClearFields();
                // And message the user that everything was added to our list successfully
                warningLabel.Text = "Added entry successfully.";
            }
            else
            {
                // set the warning label text color to red.
                warningLabel.ForeColor = Color.Red;
            }
        }
        #endregion
        #region 1.2 Validation event handlers
        private void NameTextBox_Validating(object sender, CancelEventArgs e)
        {
            // If the name textbox is empty...
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                // set the warning string to "Name is required"
                string warningString = "Name is required";
                // cancel the button click event that calls it.
                e.Cancel = true;
                // cue the text to the error provider and warning string
                nameErrorProvider.SetError(nameTextBox, warningString);
                warningLabel.Text = warningString;
            }
            else
            {
                // set the error provider text to null if text isn't empty.
                nameErrorProvider.SetError(nameTextBox, null);
            }
        }

        private void EmailAddressTextBox_Validating(object sender, CancelEventArgs e)
        {
            // Validate text in email textbox matches regex for email address, and send an "email address is invalid message" to the email address error provider".
            ValidateRegex(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$", emailAddressTextBox, emailAddressErrorProvider, "Email address is invalid", e);
        }

        private void PhoneNumberTextBox_Validating(object sender, CancelEventArgs e)
        {
            // Validate text in phone number textbox matches 10 digits, and send a "Phone must be 10 digits, no dashes" message to the phone number error provider".
            ValidateRegex(@"^[0-9]{10}$", phoneNumberTextBox, phoneNumberErrorProvider, "Phone must be 10 digits, no dashes.", e);
        }
        #endregion
        #region 1.3 Form close event handler
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // On closing application, add entries from the entries list to the file phone-book.txt.
            AddEntries(entryList, "phone-book.txt");
            // ... and open phone-book.txt with Notepad.
            OpenTextFileWithNotepad("phone-book.txt");
        }

        #endregion
        #endregion

        #region 2 Helper methods
        // Creates a new entry object from textbox elements.
        private Entry CreateNewEntry()
        {
            return new Entry { Name = nameTextBox.Text, EmailAddress = emailAddressTextBox.Text, PhoneNumber = phoneNumberTextBox.Text };
        }
        // Clears the textbox fields.
        private void ClearFields()
        {
            nameTextBox.Text = ""; emailAddressTextBox.Text = ""; phoneNumberTextBox.Text = "";
        }

        private void ValidateRegex(string regexPattern, TextBox textBoxToValidate, ErrorProvider errorProvider, string errorProviderMessage, CancelEventArgs e)
        {
            // Checks whether the textbox input matches the Regex pattern.
            if (!Regex.IsMatch(textBoxToValidate.Text, regexPattern))
            {
                // and cancels the event if it doesn't,
                e.Cancel = true;
                // ...sending the a message to the textbox's associated error provider
                errorProvider.SetError(textBoxToValidate, errorProviderMessage);
                // ... as well as to the warning label text.
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

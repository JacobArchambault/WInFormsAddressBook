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
        List<Entry> entryList = new List<Entry>();
        public Form1()
        {
            InitializeComponent();
        }

        private void addEntryButton_Click(object sender, EventArgs e)
        {
            Entry entry = CreateNewEntry();
            entryList.Add(entry);
        }

        private Entry CreateNewEntry()
        {
            return new Entry { Name = nameTextBox.Text, EmailAddress = emailAddressTextBox.Text, PhoneNumber = phoneNumberTextBox.Text };
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Add entries from the entries list to the file phone-book.txt.
            AddEntries(entryList, "phone-book.txt");
//            WriteLine("The data was stored properly. Here it is!");
            // Open phone-book.txt with Notepad.
            OpenTextFileWithNotepad("phone-book.txt");
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
                // ...trim the phone number of dashes.
                string trimmedPhoneNumber = Regex.Replace(entry.PhoneNumber, "-", "");
                // ...and write that entry's number, name, address, and phone number trimmed of dashes to the file on a single line.
                writer.WriteLine($"Entry {entryNumber + 1}:\tName: {entry.Name},\temail address: {entry.EmailAddress},\tphone number: *{trimmedPhoneNumber}*");
            });
        }

        private static void OpenTextFileWithNotepad(string fileName)
        {
            Process.Start("notepad.exe", fileName);
        }

    }
}

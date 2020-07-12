using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
    }
}

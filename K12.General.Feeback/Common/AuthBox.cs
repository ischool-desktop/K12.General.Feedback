using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;

namespace K12.General.Feedback
{
    public partial class AuthBox : BaseForm
    {
        private const string password = "%#$!%^("; //shift + 5341569

        public AuthBox()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Program.DevelopmentMode)
            {
                DialogResult = DialogResult.OK;
                return;
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
                return;

            if (txtPassword.Text != password)
            {
                txtPassword.Focus();
                txtPassword.SelectAll();
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnOK_Click(null, null);
            }
        }
    }
}
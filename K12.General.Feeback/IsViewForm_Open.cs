using FISCA.Presentation.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace K12.General.Feedback
{
    public partial class IsViewForm_Open : BaseForm
    {
        public IsViewForm_Open(name m)
        {
            InitializeComponent();
            this.Text = m._key;
            textBoxX1.Text = m._value.Replace("\n","\r\n");
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

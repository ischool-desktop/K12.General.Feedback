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
    public partial class UrlBrowser : BaseForm
    {
        public UrlBrowser(string url)
        {
            InitializeComponent();
            webBrowser1.Navigate(url);
        }
    }
}
using System;
using System.Diagnostics;
using System.Windows.Forms;

using XML.classes;

namespace XML.forms
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();

            Text = Methods.NAME + " - О программе";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://voentorg.ua/");
        }
    }
}

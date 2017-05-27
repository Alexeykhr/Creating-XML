using System;
using System.IO;
using System.Xml.Linq;
using System.Windows.Forms;

using XML.classes;
using XML.classes.db;

namespace XML.forms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Text = "XML - Main";

            if ( ! Database.SetConnection() )
                Close();
        }

        private void CompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormShop f = new FormShop();
            f.Show(this);
        }

        private void CurrencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormList f = new FormList(false);
            f.Show(this);
        }

        private void CategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormList f = new FormList(true);
            f.Show(this);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "XML Files(*.xml) | *.xml"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                XDocument doc = XMLHelpler.CreateDoc();
                XElement shop = XMLHelpler.AddShop(doc);

                if (shop == null)
                    return;

                XMLHelpler.AddCategories(shop);
                XMLHelpler.AddCurrencies(shop);
                XMLHelpler.AddOffers(shop);

                doc.Save(dialog.FileName);
            }
        }
    }
}

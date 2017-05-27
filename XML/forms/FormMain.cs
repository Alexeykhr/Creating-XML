using System;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;

using XML.classes;
using XML.classes.db;
using XML.classes.db.offer;
using XML.classes.db.category;
using XML.classes.db.currency;

namespace XML.forms
{
    public partial class Form1 : Form
    {
        private bool isEdit = false;

        public Form1()
        {
            InitializeComponent();

            if ( ! Database.SetConnection() )
                Close();

            Text = "XML - Main";

            var offers = OfferModel.GetAll();

            if (offers != null)
            {
                foreach (var offer in offers)
                {
                    listView1.Items.Add(new ListViewItem(new[] {
                        offer.Name, offer.URL, offer.PictureURL, offer.CategoryId.ToString(),
                        offer.CurrencyId
                    }));
                }
            }
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

        private void ExportXMLToolStripMenuItem_Click(object sender, EventArgs e)
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

                MessageBox.Show("Готово");
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            int categoryId = 0;
            string currencyId = "";

            if (!isEdit)
            {
                if (!string.IsNullOrWhiteSpace(comboBox1.Text))
                    categoryId = CategoryModel.GetOne(comboBox1.Text).First().CategoryId;

                if (!string.IsNullOrWhiteSpace(comboBox2.Text))
                    currencyId = CurrencyModel.GetOne(comboBox2.Text).First().CurrencyId;

                int isInserted = OfferModel.Insert(new OfferTable {
                    Name = textBox1.Text,
                    URL = textBox2.Text,
                    PictureURL = textBox3.Text,
                    Description = textBox4.Text,
                    CategoryId = categoryId,
                    CurrencyId = currencyId,
                    IsAviable = checkBox1.Checked
                });

                MessageBox.Show("category " + categoryId + " currency " + currencyId
                    + "\nisOk = " + isInserted);
            }
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
            {
                isEdit = false;
                return;
            }

            isEdit = true;
        }
    }
}

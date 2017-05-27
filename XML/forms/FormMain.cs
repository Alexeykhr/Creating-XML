using System;
using System.Linq;
using System.Drawing;
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

            FillListView();
        }

        private void FillListView()
        {
            listView1.Columns.Add("ID");
            listView1.Columns.Add("Название");
            listView1.Columns.Add("Товар");
            listView1.Columns.Add("Картинка");
            listView1.Columns.Add("Категория");
            listView1.Columns.Add("Валюта");
            listView1.Columns.Add("Доступен");

            var offers = OfferModel.GetAll();

            if (offers != null)
            {
                foreach (var offer in offers)
                {
                    listView1.Items.Add(new ListViewItem(new[] {
                        offer.offerId.ToString(), offer.Name, offer.URL, offer.PictureURL,
                        offer.CategoryTitle,  offer.CurrencyId,
                        offer.IsAviable ? "Да" : "Нет"
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
            if (!int.TryParse(fOfferId.Text, out int offerId))
                return;

            if (string.IsNullOrWhiteSpace(fName.Text) || offerId < 1)
                return;

            if (string.IsNullOrEmpty(comboBox1.Text) || string.IsNullOrEmpty(comboBox2.Text))
                return;
            
            string category = CategoryModel.GetOne(comboBox1.Text).First().Title;
            string currencyId = CurrencyModel.GetOne(comboBox2.Text).First().CurrencyId;

            if (!isEdit)
            {
                int isInserted = OfferModel.Insert(new OfferTable {
                    offerId = offerId,
                    Name = fName.Text,
                    URL = fURL.Text,
                    PictureURL = fPictureURL.Text,
                    Description = fDescription.Text,
                    CategoryTitle = category,
                    CurrencyId = currencyId,
                    IsAviable = checkBox1.Checked
                });

                if (isInserted == 1)
                {
                    listView1.Items.Add(new ListViewItem(new[] {
                        fOfferId.Text, fName.Text, fURL.Text, fPictureURL.Text,
                        category, currencyId, checkBox1.Checked ? "Да" : "Нет"
                    }));
                }
            }
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
            {
                label10.BackColor = Color.Brown;
                isEdit = false;
                fOfferId.Text = ( OfferModel.GetCount() + 1 ).ToString();
                return;
            }

            label10.BackColor = Color.DarkSlateGray;
            isEdit = true;
        }

        private void ComboBox1_DropDown(object sender, EventArgs e)
        {
            var items = CategoryModel.GetAll();
            comboBox1.Items.Clear();

            foreach (var item in items)
            {
                comboBox1.Items.Add(item.Title);
            }
        }

        private void ComboBox2_DropDown(object sender, EventArgs e)
        {
            var items = CurrencyModel.GetAll();
            comboBox2.Items.Clear();

            foreach (var item in items)
            {
                comboBox2.Items.Add(item.CurrencyId);
            }
        }
    }
}

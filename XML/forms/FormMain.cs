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

            Text = "XML - Главная форма";
            fOfferId.Text = (OfferModel.GetCount() + 1).ToString();

            FillListView();

            var categories = CategoryModel.GetAll();
            foreach (var item in categories)
            {
                comboBox1.Items.Add(item.Title);
            }

            var currencies = CurrencyModel.GetAll();
            foreach (var item in currencies)
            {
                comboBox2.Items.Add(item.CurrencyId);
            }
        }

        private void FillListView()
        {
            listView1.Columns.Add("ID");
            listView1.Columns.Add("Название");
            listView1.Columns.Add("Цена");
            listView1.Columns.Add("URL");
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
                        offer.OfferId.ToString(), offer.Name, offer.Price.ToString(), offer.URL,
                        offer.PictureURL, offer.CategoryTitle,  offer.CurrencyId,
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

                XMLHelpler.AddCurrencies(shop);
                XMLHelpler.AddCategories(shop);
                XMLHelpler.AddOffers(shop);

                doc.Save(dialog.FileName);

                MessageBox.Show("Готово");
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(fOfferId.Text, out int offerId) || !Double.TryParse(fPrice.Text, out double price))
                return;

            if (string.IsNullOrWhiteSpace(fName.Text) || offerId < 1 || price < 1)
                return;

            if (string.IsNullOrEmpty(comboBox1.Text) || string.IsNullOrEmpty(comboBox2.Text))
                return;

            string category = CategoryModel.GetOne(comboBox1.Text).First().Title;
            string currencyId = CurrencyModel.GetOne(comboBox2.Text).First().CurrencyId;

            var offer = new OfferTable
            {
                OfferId = offerId,
                Name = fName.Text,
                Price = price,
                URL = fURL.Text,
                PictureURL = fPictureURL.Text,
                Description = fDescription.Text,
                CategoryTitle = category,
                CurrencyId = currencyId,
                IsAviable = checkBox1.Checked,
                Params = GenerateDataGrid()
            };

            if (!isEdit && OfferModel.Insert(offer) == 1)
            {
                listView1.Items.Add(new ListViewItem(new[] {
                    fOfferId.Text, fName.Text, fPrice.Text.ToString(), fURL.Text,
                    fPictureURL.Text, category, currencyId, checkBox1.Checked ? "Да" : "Нет"
                }));
            }
            else
            {
                offer.Id = OfferModel.GetOneByOfferId(offerId).First().Id;
                
                if (OfferModel.Update(offer) == 1)
                {
                    listView1.SelectedItems[0].SubItems[0].Text = fOfferId.Text;
                    listView1.SelectedItems[0].SubItems[1].Text = fName.Text;
                    listView1.SelectedItems[0].SubItems[2].Text = fPrice.Text;
                    listView1.SelectedItems[0].SubItems[3].Text = fURL.Text;
                    listView1.SelectedItems[0].SubItems[4].Text = fPictureURL.Text;
                    listView1.SelectedItems[0].SubItems[5].Text = category;
                    listView1.SelectedItems[0].SubItems[6].Text = currencyId;
                    listView1.SelectedItems[0].SubItems[7].Text = checkBox1.Checked ? "Да" : "Нет";
                }
            }
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
            {
                label10.BackColor = Color.Brown;
                isEdit = false;
                ClearPanel();
                fOfferId.Text = (OfferModel.GetCount() + 1).ToString();
                return;
            }

            label10.BackColor = Color.DarkSlateGray;
            isEdit = true;

            var item = OfferModel.GetOneByOfferId(int.Parse(listView1.SelectedItems[0].Text)).First();

            fOfferId.Text = item.OfferId.ToString();
            fName.Text = item.Name;
            fPrice.Text = item.Price.ToString();
            fURL.Text = item.URL;
            fPictureURL.Text = item.PictureURL;
            comboBox1.SelectedItem = item.CategoryTitle;
            comboBox2.Text = item.CurrencyId;
            checkBox1.Checked = item.IsAviable;
            fDescription.Text = item.Description;
            InsertDataGrid(item.Params);
        }

        private void ComboBox1_DropDown(object sender, EventArgs e)
        {
            var items = CategoryModel.GetAll();
            string save = comboBox1.Text;
            comboBox1.Items.Clear();

            foreach (var item in items)
            {
                comboBox1.Items.Add(item.Title);
            }

            comboBox1.Text = save;
        }

        private void ComboBox2_DropDown(object sender, EventArgs e)
        {
            var items = CurrencyModel.GetAll();
            string save = comboBox2.Text;
            comboBox2.Items.Clear();

            foreach (var item in items)
            {
                comboBox2.Items.Add(item.CurrencyId);
            }

            comboBox2.Text = save;
        }

        private void ClearPanel()
        {
            dataGridView1.Rows.Clear();

            foreach (Control c in panel1.Controls)
            {
                if (c.GetType() == typeof(TextBox))
                    (c as TextBox).Text = string.Empty;
                else if (c.GetType() == typeof(ComboBox))
                    (c as ComboBox).ResetText();
            }
        }

        private string GenerateDataGrid()
        {
            string sout = "";
            int len = dataGridView1.Rows.Count;

            for (int i = 0; i < len; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value == null || dataGridView1.Rows[i].Cells[1].Value == null)
                    continue;

                string one = dataGridView1.Rows[i].Cells[0].Value.ToString().Trim();
                string two = dataGridView1.Rows[i].Cells[1].Value.ToString().Trim();

                if (string.IsNullOrWhiteSpace(one) || string.IsNullOrWhiteSpace(two))
                    continue;

                sout += one + "|" + two + "|";
            }

            if (string.IsNullOrEmpty(sout))
                return "";

            return sout.Substring(0, sout.Length - 1);
        }

        private void InsertDataGrid(string value)
        {
            if (string.IsNullOrEmpty(value))
                return;

            string[] arr = value.Split('|');
            int len = arr.Length / 2;

            for (int i = 0, j = 0; i < len; i++, j += 2)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = arr[j];
                dataGridView1.Rows[i].Cells[1].Value = arr[j + 1];
            }
        }
    }
}

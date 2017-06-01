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
using XML.classes.db.parametrs;

namespace XML.forms
{
    public partial class Form1 : Form
    {
        private bool isEdit = false;

        public Form1()
        {
            InitializeComponent();

            if ( ! Database.SetConnection() )
            {
                MessageBox.Show("Ошибка при подлкючении к базе данных");
                Close();
            }

            Text = Methods.NAME + " - Главная";

            InitFillListView();
        }

        private void InitFillListView()
        {
            //ListView

            listView1.Columns.Add("ID");
            listView1.Columns.Add("Название");
            listView1.Columns.Add("Цена");
            listView1.Columns.Add("URL");
            listView1.Columns.Add("Картинки");
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

            // Panel

            fOfferId.Text = (OfferModel.GetCount() + 1).ToString();
            FillComboBoxCategories();
            FillComboBoxCurrencies();
        }

        private void FillComboBoxCategories()
        {
            string save = comboBox1.Text;

            comboBox1.Items.Clear();

            var categories = CategoryModel.GetAll();
            foreach (var item in categories)
            {
                comboBox1.Items.Add(item.Title);
            }

            if (!isEdit && categories.Count() > 0)
                comboBox1.SelectedIndex = 0;
            else
                comboBox1.Text = save;
        }

        private void FillComboBoxCurrencies()
        {
            string save = comboBox2.Text;

            comboBox2.Items.Clear();

            var currencies = CurrencyModel.GetAll();

            foreach (var item in currencies)
            {
                comboBox2.Items.Add(item.CurrencyId);
            }
            
            if (!isEdit && currencies.Count() > 0)
                comboBox2.SelectedIndex = 0;
            else
                comboBox2.Text = save;
        }

        private void CompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormShop f = new FormShop();
            f.Show(this);
        }

        private void CategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormList f = new FormList(true))
            {
                f.ShowDialog();
            }

            FillComboBoxCategories();
        }

        private void CurrencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormList f = new FormList(false))
            {
                f.ShowDialog();
            }

            FillComboBoxCurrencies();
        }

        private void ConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormParams f = new FormParams())
            {
                f.ShowDialog();
            }
        }

        private void DeleteRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
            {
                MSG("Выберите в списке удаляемый товар");
                return;
            }

            int deleted = OfferModel.DeleteObject<OfferTable>(
                OfferModel.GetOneByOfferId(GetSelectedOfferId()).First().Id
            );

            if (deleted == 1)
            {
                listView1.Items.RemoveAt(listView1.SelectedItems[0].Index);
                MSG("Товар удалён");
            }
            else
                MSG("Товар не удалился");
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
                {
                    MSG("Нужно добавить настройки компании");
                    return;
                }

                XMLHelpler.AddCurrencies(shop);
                XMLHelpler.AddCategories(shop);
                XMLHelpler.AddOffers(shop);

                doc.Save(dialog.FileName);

                MSG("Экспорт завершён");
            }
        }

        private void ImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "XML Files(*.xml) | *.xml"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // !!!!!!!!!
                XMLHelpler.ImportXML(dialog.FileName, false);

                MSG("Импорт завершён");
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            CorrectInput();

            if (!CheckDataForNewData())
                return;

            int offerId = int.Parse(fOfferId.Text);
            double price = Double.Parse(fPrice.Text);
            string category = CategoryModel.GetOne(comboBox1.Text).First().Title;
            string currencyId = CurrencyModel.GetOne(comboBox2.Text).First().CurrencyId;

            var offer = new OfferTable
            {
                OfferId = offerId,
                Name = fName.Text,
                Price = price,
                URL = fURL.Text,
                PictureURL = fPicturesURL.Text,
                Description = fDescription.Text,
                CategoryTitle = category,
                CurrencyId = currencyId,
                IsAviable = checkBox1.Checked,
                Params = GenerateParams()
            };

            if (!isEdit)
            {
                if (OfferModel.Insert(offer) == 1)
                {
                    listView1.Items.Add(new ListViewItem(new[] {
                        fOfferId.Text, fName.Text, fPrice.Text.ToString(), fURL.Text,
                        fPicturesURL.Text, category, currencyId, checkBox1.Checked ? "Да" : "Нет"
                    }));

                    MSG("Товар добавлен");
                }
                else
                    MSG("Товар не добавлен");
            }
            else
            {
                offer.Id = OfferModel.GetOneByOfferId(int.Parse(listView1.SelectedItems[0].Text)).First().Id;
                
                if (OfferModel.Update(offer) == 1)
                {
                    listView1.SelectedItems[0].SubItems[0].Text = fOfferId.Text;
                    listView1.SelectedItems[0].SubItems[1].Text = fName.Text;
                    listView1.SelectedItems[0].SubItems[2].Text = fPrice.Text;
                    listView1.SelectedItems[0].SubItems[3].Text = fURL.Text;
                    listView1.SelectedItems[0].SubItems[4].Text = fPicturesURL.Text;
                    listView1.SelectedItems[0].SubItems[5].Text = category;
                    listView1.SelectedItems[0].SubItems[6].Text = currencyId;
                    listView1.SelectedItems[0].SubItems[7].Text = checkBox1.Checked ? "Да" : "Нет";

                    MSG("Товар обновлён");
                }
                else
                    MSG("Товар не обновлён - проверьте уникальность ID товара и название");
            }
        }

        private void CorrectInput()
        {
            fName.Text = Methods.FirstCharToUpper(fName.Text).Trim();
            fPrice.Text = Methods.ReplaceDot(fPrice.Text).Trim();
            fOfferId.Text = fOfferId.Text.Trim();
            fPrice.Text = fPrice.Text.Trim();
            fURL.Text = fURL.Text.Trim();
            fDescription.Text = fDescription.Text.Trim();

            // Pictures
            string[] pictures = fPicturesURL.Text.Trim().Split('\n');
            string outPictures = "";
            foreach (string picture in pictures)
            {
                if (!string.IsNullOrWhiteSpace(picture))
                    outPictures += picture.Trim() + Environment.NewLine;
            }

            fPicturesURL.Text = outPictures.Trim();
        }

        private bool CheckDataForNewData()
        {
            if (!int.TryParse(fOfferId.Text, out int offerId))
                MSG("ID должен быть число");

            else if (offerId < 1)
                MSG("ID должен быть больше 0 и быть уникальным");

            else if (!Double.TryParse(fPrice.Text, out double price))
                MSG("Цена должна быть числом с плавающей точкой");

            else if (string.IsNullOrWhiteSpace(fName.Text))
                MSG("Заполните название товара");

            else if (string.IsNullOrEmpty(comboBox1.Text))
                MSG("Категория не выбрана");

            else if (string.IsNullOrEmpty(comboBox2.Text))
                MSG("Валюта не выбрана");

            else
                return true;

            return false;
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
            {
                label10.BackColor = Color.Brown;
                isEdit = false;
                ClearPanel();
                fOfferId.Text = (OfferModel.GetCount() + 1).ToString();
                fPrice.Text = "0";

                if (comboBox1.Items.Count > 0)
                    comboBox1.SelectedIndex = 0;

                if (comboBox2.Items.Count > 0)
                    comboBox2.SelectedIndex = 0;

                FillDataGridFromBasic();
                return;
            }

            label10.BackColor = Color.DarkSlateGray;
            isEdit = true;

            var offer = OfferModel.GetOneByOfferId(GetSelectedOfferId()).First();

            fOfferId.Text = offer.OfferId.ToString();
            fName.Text = offer.Name;
            fPrice.Text = offer.Price.ToString();
            fURL.Text = offer.URL;
            fPicturesURL.Text = offer.PictureURL;
            comboBox1.Text = offer.CategoryTitle;
            comboBox2.Text = offer.CurrencyId;
            checkBox1.Checked = offer.IsAviable;
            fDescription.Text = offer.Description;
            FillDataGridFromTable(offer.Params);
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
                else if (c.GetType() == typeof(CheckBox))
                    (c as CheckBox).Checked = true;
            }
        }

        private string GenerateParams()
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

        private void FillDataGridFromBasic()
        {
            dataGridView1.Rows.Clear();

            if (string.IsNullOrWhiteSpace(comboBox1.Text) && !isEdit)
                return;

            var parametrs = ParametrsModel.GetOneByCategoryTitle(comboBox1.Text);

            if (parametrs.Count() < 1)
                return;

            string[] arr = parametrs.First().Parametrs.Split('\n');

            foreach (var line in arr)
            {
                dataGridView1.Rows.Add(line);
            }
        }

        private void FillDataGridFromTable(string value)
        {
            dataGridView1.Rows.Clear();

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

        private int GetSelectedOfferId()
        {
            return int.Parse(listView1.SelectedItems[0].Text);
        }

        private void FOfferId_Leave(object sender, EventArgs e)
        {
            bool isInt = int.TryParse(fOfferId.Text, out int id);

            if (!isInt)
            {
                MSG("ID должен быть числом");
                return;
            }

            var offer = OfferModel.GetOneByOfferId(id);

            if (offer.Count() > 0)
                MSG("ID используется - \"" + offer.First().Name + "\"");
            else
                MSG("ID не занят");
        }

        private void FName_Leave(object sender, EventArgs e)
        {
            fName.Text = Methods.FirstCharToUpper(fName.Text).Trim();
            var offer = OfferModel.GetOneByName(fName.Text);

            if (offer.Count() > 0)
                MSG("Название товара используется - #" + offer.First().OfferId);
            else
                MSG("Название товара не занято");
        }

        private void MSG(string text)
        {
            Info.Text = text;
            Info.BackColor = Color.FromArgb(255, 192, 192);
            timer1.Enabled = true;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Info.BackColor = Color.White;
            timer1.Enabled = false;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isEdit)
                return;
            
            FillDataGridFromBasic();
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Xml.Linq;
using System.Diagnostics;
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
        private int count = 0;

        public Form1()
        {
            InitializeComponent();

            if (! Database.Connection())
            {
                MessageBox.Show("Ошибка соедиения с БД");
                return;
            }
            
            Text = Methods.NAME + " - Главная";

            Initial();
        }

        private void Initial()
        {
            //ListView
            listView1.Columns.Add("ID");
            listView1.Columns.Add("Название");
            listView1.Columns.Add("Цена");
            listView1.Columns.Add("URL");
            listView1.Columns.Add("Картинки");
            listView1.Columns.Add("Категория");
            listView1.Columns.Add("Валюта");
            listView1.Columns.Add("Продавец");
            listView1.Columns.Add("Доступен");

            var offers = OfferModel.GetAll();
            count = offers.Count();

            if (offers != null)
            {
                foreach (var offer in offers)
                {
                    listView1.Items.Add(new ListViewItem(new[] {
                        offer.OfferId.ToString(), offer.Name, offer.Price.ToString(), offer.URL.Length > 0 ? "+" : "-",
                        offer.PicturesURL.Length > 0 ? "+" : "-", offer.CategoryTitle,  offer.CurrencyId,
                        offer.Vendor, offer.IsAviable ? "Да" : "Нет"
                    }));
                }
            }

            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            // Panel
            fOfferId.Text = (count + 1).ToString();
            fPrice.Text = "0";
            FillComboBoxCategories();
            FillComboBoxCurrencies();

            // Notify
            notify.Text = Methods.NAME;
            notify.BalloonTipTitle = Methods.NAME + " - уведомление";
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
        
        private void CorrectInput()
        {
            fName.Text = Methods.FirstCharToUpper(fName.Text).Trim();
            fPrice.Text = Methods.ReplaceDot(fPrice.Text);
            fURL.Text = fURL.Text.Trim();
            fDescription.Text = Methods.FirstCharToUpper(fDescription.Text).Trim();
            fPicturesURL.Text = Methods.GetPickPictures(fPicturesURL.Text);
            fVendor.Text = Methods.FirstCharToUpper(fVendor.Text).Trim();
        }

        private bool CheckDataForNewData()
        {
            if (! int.TryParse(fOfferId.Text, out int offerId))
                MSG("ID должен быть число");

            else if (offerId < 1)
                MSG("ID должен быть больше 0 и быть уникальным");

            else if (! Double.TryParse(fPrice.Text, out double price))
                MSG("Цена введена неверно. Пример: 25 | 2 | 5.23 | 63,745");

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
                else if (c.GetType() == typeof(RichTextBox))
                    (c as RichTextBox).Text = string.Empty;
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

        private void FillDataGridDefault()
        {
            ClearPanel();
            dataGridView1.Rows.Clear();

            fOfferId.Text = (count + 1).ToString();
            fPrice.Text = "0";

            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;

            if (comboBox2.Items.Count > 0)
                comboBox2.SelectedIndex = 0;

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

        private void FillDataGridDatabase(string value)
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

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isEdit)
                return;

            dataGridView1.Rows.Clear();
            var parametrs = ParametrsModel.GetOneByCategoryTitle(comboBox1.Text);

            if (parametrs.Count() < 1)
                return;

            string[] arr = parametrs.First().Parametrs.Split('\n');

            foreach (var line in arr)
            {
                dataGridView1.Rows.Add(line);
            }
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (isEdit)
                fOfferId.ReadOnly = isEdit ? button3.Enabled : false;

            button3.Enabled = !button3.Enabled;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Database.CloseConnection();
        }

        // |--------------------------------------------------------------------------
        // | Click ToolStripMenu
        // |--------------------------------------------------------------------------
        // |

        private void CompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormShop f = new FormShop();
            f.Show(this);
        }

        private void CategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormCategory f = new FormCategory())
            {
                f.ShowDialog();
            }

            FillComboBoxCategories();
        }

        private void CurrencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormCurrency f = new FormCurrency())
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

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout f = new FormAbout();
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
                MSG("Создаём Файл..");
                XDocument doc = XMLHelpler.CreateDoc();

                MSG("Добавляем магазин..");
                XElement shop = XMLHelpler.AddShop(doc);

                if (shop == null)
                {
                    MSG("Отсутствуют настройки магазина. Меню \"Магазин\"");
                    return;
                }

                MSG("Добавляем валюты..");
                XMLHelpler.AddCurrencies(shop);

                MSG("Добавляем категории..");
                XMLHelpler.AddCategories(shop);

                MSG("Добавляем товары..");
                XMLHelpler.AddOffers(shop);

                MSG("Сохраняем файл..");
                doc.Save(dialog.FileName);

                MSG("Экспорт завершён");
                ShowNotify("Экспорт завершён");
            }
        }

        private void ImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult isOpen = MessageBox.Show("Импорт XML может занят до 2-ух минут (зависит от системы)" + Environment.NewLine
                + "После завершения программа закроется и данные обновятся" + Environment.NewLine
                + "Вы уверены, что хотите продолжить?",
                Methods.NAME, MessageBoxButtons.YesNo);

            if (isOpen == DialogResult.No)
                return;

            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "XML Files(*.xml) | *.xml"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult result = MessageBox.Show(
                    "Перезаписывать существующие данные?",
                    Methods.NAME,
                    MessageBoxButtons.YesNo
                );

                bool isComplete = XMLHelpler.ImportXML(dialog.FileName, result == DialogResult.Yes);

                if (isComplete)
                {
                    ShowNotify("Импорт завершён!\nДля продолжения - запустите программу заново.");
                    Close();
                }
                else
                {
                    ShowNotify("Произошла ошибка при импорте", 2000, ToolTipIcon.Warning);
                    MSG("Произошла ошибка при импорте. " +
                        "Проверьте файл на наличе <shop> и отсутствии символов \"&\". " +
                        "Для символа - запустите починку в меню \"XML\"");
                }
            }
        }

        private void RepairXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult isOpen = MessageBox.Show("Исходный файл будет изменён" + Environment.NewLine
                + "Вы уверены, что хотите продолжить?",
                Methods.NAME, MessageBoxButtons.YesNo);

            if (isOpen == DialogResult.No)
                return;

            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "XML Files(*.xml) | *.xml"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                XMLHelpler.Repair(dialog.FileName);
                MSG("Починка завершена. Попробуйте импортировать файл снова.", true);
            }
        }

        // |--------------------------------------------------------------------------
        // | Click Buttons
        // |--------------------------------------------------------------------------
        // |

        // Save or insert
        private void Button1_Click(object sender, EventArgs e)
        {
            CorrectInput();

            if (! CheckDataForNewData())
                return;

            var offer = new OfferTable
            {
                OfferId = int.Parse(fOfferId.Text),
                Name = fName.Text,
                Price = Double.Parse(fPrice.Text),
                URL = fURL.Text,
                PicturesURL = fPicturesURL.Text,
                Description = fDescription.Text,
                CategoryTitle = comboBox1.Text,
                CurrencyId = comboBox2.Text,
                IsAviable = checkBox1.Checked,
                Vendor = fVendor.Text,
                Params = GenerateParams()
            };

            if (! isEdit)
            {
                if (Database.Insert(offer) == 1)
                {
                    listView1.Items.Add(new ListViewItem(new[] {
                        fOfferId.Text, fName.Text, fPrice.Text.ToString(), fURL.Text.Length > 0 ? "+" : "-",
                        fPicturesURL.Text.Length > 0 ? "+" : "-", comboBox1.Text, comboBox2.Text,
                        fVendor.Text, checkBox1.Checked ? "Да" : "Нет"
                    }));

                    MSG("Товар добавлен [" + fOfferId.Text + "] - " + fName.Text, true);
                    count++;

                    FillDataGridDefault();
                }
                else
                    MSG("Товар не добавлен - проверьте уникальность ID товара и название");
            }
            else
            {
                offer.Id = OfferModel.GetOneByName(GetSelectedName()).First().Id;

                if (Database.Update(offer) == 1)
                {
                    listView1.SelectedItems[0].SubItems[0].Text = fOfferId.Text;
                    listView1.SelectedItems[0].SubItems[1].Text = fName.Text;
                    listView1.SelectedItems[0].SubItems[2].Text = fPrice.Text;
                    listView1.SelectedItems[0].SubItems[3].Text = fURL.Text.Length > 0 ? "+" : "-";
                    listView1.SelectedItems[0].SubItems[4].Text = fPicturesURL.Text.Length > 0 ? "+" : "-";
                    listView1.SelectedItems[0].SubItems[5].Text = comboBox1.Text;
                    listView1.SelectedItems[0].SubItems[6].Text = comboBox2.Text;
                    listView1.SelectedItems[0].SubItems[7].Text = fVendor.Text;
                    listView1.SelectedItems[0].SubItems[8].Text = checkBox1.Checked ? "Да" : "Нет";

                    MSG("Товар обновлён [" + fOfferId.Text + "] - " + fName.Text, true);
                }
                else
                    MSG("Товар не обновлён - проверьте уникальность ID товара и название");
            }
        }

        // Search in ListView
        private void Button2_Click(object sender, EventArgs e)
        {
            int count = listView1.Items.Count;
            int initIndex = isEdit ? GetSelectedIndex() + 1 : 0;
            bool isInt = int.TryParse(textBox1.Text, out int id);

            if (isInt)
            {
                for (int i = initIndex; i < count + initIndex; i++)
                {
                    if (string.Equals(textBox1.Text, listView1.Items[i % count].Text, StringComparison.CurrentCultureIgnoreCase))
                    {
                        listView1.Items[i % count].Selected = true;
                        MSG("Найден товар по ID [" + listView1.Items[i % count].Text + "] - "
                            + listView1.Items[i % count].SubItems[1].Text, true);
                        return;
                    }
                }

                MSG("Товар по ID не найден");
                return;
            }

            int len = textBox1.Text.Length;
            string text = textBox1.Text.Substring(0, len);

            for (int i = initIndex; i < count + initIndex; i++)
            {
                string curText = listView1.Items[i % count].SubItems[1].Text;

                if (curText.Length < len)
                    continue;

                if (string.Equals(text, curText.Substring(0, len), StringComparison.CurrentCultureIgnoreCase))
                {
                    listView1.Items[i % count].Selected = true;
                    MSG("Найден товар по названию [" + listView1.Items[i % count].Text + "] - "
                        + listView1.Items[i % count].SubItems[1].Text, true);
                    return;
                }
            }

            MSG("Товар по названию не найден");
        }

        // Delete product
        private void Button3_Click(object sender, EventArgs e)
        {
            if (!isEdit)
            {
                MSG("Выберите в списке удаляемый товар");
                return;
            }

            string name = GetSelectedName();
            int deleted = Database.DeleteObject<OfferTable>(GetSelectedOfferId());

            if (deleted == 1)
            {
                MSG("Товар удалён - " + name, true);
                listView1.Items.RemoveAt(GetSelectedIndex());
                count--;
            }
            else
                MSG("Товар не удалился");
        }

        // Copy product
        private void Button4_Click(object sender, EventArgs e)
        {
            if (!isEdit)
                return;

            string name = fName.Text;
            string price = fPrice.Text;
            string url = fURL.Text;
            string picture = fPicturesURL.Text;
            string category = comboBox1.Text;
            string currency = comboBox2.Text;
            string vendor = fVendor.Text;
            bool checkbox = checkBox1.Checked;
            string parameters = GenerateParams();
            string desc = fDescription.Text;

            listView1.SelectedItems[0].Selected = false;

            fName.Text = name;
            fPrice.Text = price;
            fURL.Text = url;
            fPicturesURL.Text = picture;
            comboBox1.Text = category;
            comboBox2.Text = currency;
            fVendor.Text = vendor;
            checkBox1.Checked = checkbox;
            fDescription.Text = desc;
            FillDataGridDatabase(parameters);

            MSG("Товар скопирован. Необходимо изменить название и id (при необходимости)", true);
        }

        // Fall focus
        private void Button5_Click(object sender, EventArgs e)
        {
            if (isEdit)
                listView1.SelectedItems[0].Selected = false;
        }

        // Go to website
        private void Label9_Click(object sender, EventArgs e)
        {
            if (Methods.IsWebSite(fURL.Text))
                Process.Start(fURL.Text);
            else
                MSG("Ссылка имеет неверный формат");
        }

        // |--------------------------------------------------------------------------
        // | Leave Inputs
        // |--------------------------------------------------------------------------
        // |

        private void FOfferId_Leave(object sender, EventArgs e)
        {
            bool isInt = int.TryParse(fOfferId.Text, out int id);

            if (!isInt)
            {
                MSG("ID должен быть числом");
                return;
            }

            if (isEdit && GetSelectedOfferId() == id)
                return;

            var offer = OfferModel.GetOneByOfferId(id);

            if (offer.Count() > 0)
                MSG("ID используется - \"" + offer.First().Name + "\"");
            else
                MSG("ID не занят", true);
        }

        private void FName_Leave(object sender, EventArgs e)
        {
            fName.Text = Methods.FirstCharToUpper(fName.Text).Trim();

            if (isEdit && GetSelectedName() == fName.Text)
                return;

            var offer = OfferModel.GetOneByName(fName.Text);

            if (offer.Count() < 1)
                MSG("Название товара не занято", true);
            else
                MSG("Название товара используется - #" + offer.First().OfferId);
        }

        private void FPrice_Leave(object sender, EventArgs e)
        {
            fPrice.Text = Methods.ReplaceDot(fPrice.Text);
            bool isDouble = Double.TryParse(fPrice.Text, out double salary);

            if (isDouble)
                MSG("Цена корректная", true);
            else
                MSG("Цена введена неверно. Пример: 25 | 2 | 5.23 | 63,745");
        }

        private void FURL_Leave(object sender, EventArgs e)
        {
            if (Methods.IsWebSite(fURL.Text))
                MSG("URL корректный", true);
            else
                MSG("URL имеет неверный формат");
        }

        // |--------------------------------------------------------------------------
        // | Notifications
        // |--------------------------------------------------------------------------
        // |

        private void MSG(string text, bool isGood = false)
        {
            Info.Text = text;
            Info.BackColor = isGood ? Color.FromArgb(40, 167, 69) : Color.FromArgb(255, 192, 192);
            timer1.Enabled = true;
        }

        private void ShowNotify(string text, int timeout = 3000, ToolTipIcon icon = ToolTipIcon.Info)
        {
            notify.BalloonTipText = text;
            notify.ShowBalloonTip(timeout);
            notify.BalloonTipIcon = icon;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Info.BackColor = Color.White;
            timer1.Enabled = false;
        }
        
        // |--------------------------------------------------------------------------
        // | ListView
        // |--------------------------------------------------------------------------
        // |

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
            {
                label10.BackColor = Color.Brown;
                isEdit = false;
                fOfferId.Text = (count + 1).ToString();
                fOfferId.ReadOnly = false;
                button1.Text = "Добавить";

                FillDataGridDefault();
                return;
            }

            label10.BackColor = Color.DarkSlateGray;
            isEdit = true;
            button1.Text = "Обновить";
            fOfferId.ReadOnly = !button3.Enabled;

            var model = OfferModel.GetOneByOfferId(GetSelectedOfferId());

            if (model.Count() < 1)
            {
                listView1.SelectedItems[0].Selected = false;
                return;
            }

            var offer = model.First();

            fOfferId.Text = offer.OfferId.ToString();
            fName.Text = offer.Name;
            fPrice.Text = offer.Price.ToString();
            fURL.Text = offer.URL;
            fPicturesURL.Text = offer.PicturesURL;
            comboBox1.Text = offer.CategoryTitle;
            comboBox2.Text = offer.CurrencyId;
            checkBox1.Checked = offer.IsAviable;
            fDescription.Text = offer.Description;
            fVendor.Text = offer.Vendor;
            FillDataGridDatabase(offer.Params);
        }

        private int GetSelectedIndex()
        {
            return listView1.SelectedItems[0].Index;
        }

        private int GetSelectedOfferId()
        {
            return int.Parse(listView1.SelectedItems[0].Text);
        }

        private string GetSelectedName()
        {
            return listView1.SelectedItems[0].SubItems[1].Text;
        }
    }
}

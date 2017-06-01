using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

using XML.classes;
using XML.classes.db.offer;
using XML.classes.db.currency;
using XML.classes.db.category;

namespace XML.forms
{
    public partial class FormList : Form
    {
        private bool isCategory = false;
        private bool isEdit = false;

        // True  - Category
        // False - Currency
        public FormList(bool isCategory)
        {
            InitializeComponent();

            this.isCategory = isCategory;

            if (isCategory)
            {
                Text = "XML - Категории";
                label2.Text = "ID";
                label3.Text = "Название";
                listView1.Columns.Add("ID");
                listView1.Columns.Add("Название");
                textBox1.Text = (CategoryModel.GetCount() + 1).ToString();
            }
            else
            {
                Text = "XML - Валюты";
                label2.Text = "Валюта";
                label3.Text = "Ставка";
                listView1.Columns.Add("Валюта");
                listView1.Columns.Add("Ставка");
            }

            FillListView();
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
            {
                label1.BackColor = Color.Brown;
                isEdit = false;
                textBox1.Text = isCategory ? (CategoryModel.GetCount() + 1).ToString() : string.Empty;
                textBox2.Text = string.Empty;
                return;
            }

            label1.BackColor = Color.DarkSlateGray;
            isEdit = true;
            textBox1.Text = listView1.SelectedItems[0].SubItems[0].Text;
            textBox2.Text = listView1.SelectedItems[0].SubItems[1].Text;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Данные отсутствуют");
                return;
            }

            textBox1.Text = isCategory ? textBox1.Text.Trim() : textBox1.Text.ToUpper().Trim();
            textBox2.Text = isCategory ? textBox2.Text.Trim() : Methods.ReplaceDot(textBox2.Text);

            if (isEdit && UpdateSelectedItem() == 1)
            {
                int selectedIndex = listView1.SelectedItems[0].Index;

                listView1.Items[selectedIndex].SubItems[0].Text = textBox1.Text;
                listView1.Items[selectedIndex].SubItems[1].Text = textBox2.Text;

                if (listView1.SelectedItems.Count > 0)
                    listView1.SelectedItems[0].Selected = false;
            }
            else if (InsertItem() == 1)
            {
                listView1.Items.Add(new ListViewItem(new[] {
                    textBox1.Text, textBox2.Text
                }));
            }
            else
                return;
            
            textBox1.Text = isCategory ? (CategoryModel.GetCount() + 1).ToString() : string.Empty;
            textBox2.Text = string.Empty;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
            {
                MessageBox.Show("Выберите строку");
                return;
            }

            if (DeleteSelectedItem() == 1)
                listView1.SelectedItems[0].Remove();
            else
                MessageBox.Show("Данные не удалились");
        }

        private void FillListView()
        {
            if (isCategory)
                FillListViewCategory();
            else
                FillListViewCurrency();
        }

        private void FillListViewCategory()
        {
            var categories = CategoryModel.GetAll();

            if (categories != null && categories.Count() > 0)
            {
                foreach (var category in categories)
                {
                    listView1.Items.Add(new ListViewItem(new[] {
                        category.CategoryId.ToString(), category.Title
                    }));
                }
            }
        }

        private void FillListViewCurrency()
        {
            var currencies = CurrencyModel.GetAll();

            if (currencies != null && currencies.Count() > 0)
            {
                foreach (var currency in currencies)
                {
                    listView1.Items.Add(new ListViewItem(new[] {
                        currency.CurrencyId, currency.Rate.ToString()
                    }));
                }
            }
        }

        private int InsertItem()
        {
            int inserted = 0;

            if (isCategory)
            {
                if (!int.TryParse(textBox1.Text, out int categoryId))
                {
                    MessageBox.Show("ID должен быть числом");
                    return 0;
                }

                inserted = InsertItemCategory(categoryId, textBox2.Text);
            }
            else
            {
                if (!Double.TryParse(textBox2.Text, out double rate))
                {
                    MessageBox.Show("Валюта должна быть числом (возможна плавающая точка)");
                    return 0;
                }

                inserted = InsertItemCurrency(textBox1.Text, rate);
            }

            if (inserted != 1)
                MessageBox.Show("Данные существуют");

            return inserted;
        }

        private int InsertItemCategory(int categoryId, string title)
        {
            return CategoryModel.Insert(new CategoryTable
            {
                CategoryId = categoryId,
                Title = title
            });
        }

        private int InsertItemCurrency(string currencyId, double rate)
        {
            return CurrencyModel.Insert(new CurrencyTable
            {
                CurrencyId = currencyId,
                Rate = rate
            });
        }

        private int UpdateSelectedItem()
        {
            if (isCategory)
            {
                if (!int.TryParse(textBox1.Text, out int categoryId))
                {
                    MessageBox.Show("ID должен быть числом");
                    return 0;
                }

                return UpdateSelectedItemCategory(categoryId, textBox2.Text);
            }

            if (!Double.TryParse(textBox2.Text, out double currencyId))
            {
                MessageBox.Show("Валюта должна быть числом (возможна плавающая точка)");
                return 0;
            }

            return UpdateSelectedItemCurrency(textBox1.Text, currencyId);
        }

        private int UpdateSelectedItemCategory(int categoryId, string title)
        {
            try
            {
                int id = CategoryModel.GetOne(
                    int.Parse(listView1.SelectedItems[0].Text)
                ).First().Id;

                return CategoryModel.Update(new CategoryTable
                {
                    Id = id,
                    CategoryId = categoryId,
                    Title = title
                });
            }
            catch { return 0; }
        }

        private int UpdateSelectedItemCurrency(string currencyId, double rate)
        {
            try
            {
                int id = CurrencyModel.GetOne(
                    listView1.SelectedItems[0].Text
                ).First().Id;

                return CurrencyModel.Update(new CurrencyTable
                {
                    Id = id,
                    CurrencyId = currencyId,
                    Rate = rate
                });
            }
            catch { return 0; }
        }

        private int DeleteSelectedItem()
        {
            return isCategory ? DeleteSelectedItemCategory() : DeleteSelectedItemCurrency();
        }

        private int DeleteSelectedItemCategory()
        {
            try
            {
                int id = CategoryModel.GetOne(
                    int.Parse(listView1.SelectedItems[0].Text)
                ).First().Id;

                var category = OfferModel.GetOneByCategoryTitle(listView1.SelectedItems[0].SubItems[1].Text);

                if (category.Count() > 0)
                {
                    MessageBox.Show("[" + category.First().Id + "] " + category.First().Name + " - использует эту категорию");
                    return 0;
                }

                return CategoryModel.DeleteObject<CategoryTable>(id);
            }
            catch { return 0; }
        }

        private int DeleteSelectedItemCurrency()
        {
            try
            {
                int id = CurrencyModel.GetOne(
                    listView1.SelectedItems[0].Text
                ).First().Id;

                var currency = OfferModel.GetOneByCurrencyId(listView1.SelectedItems[0].Text);

                if (currency.Count() > 0)
                {
                    MessageBox.Show("[" + currency.First().Id + "] " + currency.First().Name + " - использует эту валюту");
                    return 0;
                }

                return CurrencyModel.DeleteObject<CurrencyTable>(id);
            }
            catch { return 0; }
        }
    }
}

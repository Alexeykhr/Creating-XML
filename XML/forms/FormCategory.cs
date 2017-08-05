using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

using XML.classes;
using XML.classes.db;
using XML.classes.db.offer;
using XML.classes.db.category;
using XML.classes.db.parametrs;

namespace XML.forms
{
    public partial class FormCategory : Form
    {
        private bool isEdit = false;
        private int count = 0;

        public FormCategory()
        {
            InitializeComponent();

            Text = Methods.NAME + " - Категории";

            Initial();
        }

        private void Initial()
        {
            // ListView
            listView1.Columns.Add("ID");
            listView1.Columns.Add("Родительский ID");
            listView1.Columns.Add("Название");

            var categories = CategoryModel.GetAll();

            if (categories != null && categories.Count() > 0)
            {
                foreach (var category in categories)
                {
                    listView1.Items.Add(new ListViewItem(new[] {
                        category.CategoryId.ToString(), category.ParCategoryId.ToString(),
                        category.Title
                    }));
                }
            }

            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            // Other
            count = categories.Count();
            textBox1.Text = (count + 1).ToString();
            textBox3.Text = "0";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            bool check = CheckInputs();

            if (!check)
                return;

            if (isEdit && UpdateSelectedItem() == 1)
            {
                int selectedIndex = listView1.SelectedItems[0].Index;

                listView1.Items[selectedIndex].SubItems[0].Text = textBox1.Text;
                listView1.Items[selectedIndex].SubItems[1].Text = textBox3.Text;
                listView1.Items[selectedIndex].SubItems[2].Text = textBox2.Text;

                if (listView1.SelectedItems.Count > 0)
                    listView1.SelectedItems[0].Selected = false;
            }
            else if (InsertItem() == 1)
            {
                count++;
                listView1.Items.Add(new ListViewItem(new[] {
                    textBox1.Text, textBox3.Text, textBox2.Text
                }));
            }
            else
                return;
            
            textBox1.Text = (count + 1).ToString();
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (! isEdit)
            {
                MessageBox.Show("Выберите строку");
                return;
            }

            if (DeleteSelectedItem() == 1)
            {
                count--;
                listView1.SelectedItems[0].Remove();
            }
            else
                MessageBox.Show("Данные не удалились");
        }

        private void CorrectInputs()
        {
            textBox3.Text = textBox3.Text.Trim();

            if (string.IsNullOrWhiteSpace(textBox3.Text))
                textBox3.Text = "0";
        }

        private bool CheckInputs(bool isDelete = false)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
                MessageBox.Show("Нужно заполнить ID и название");

            else if (!int.TryParse(textBox1.Text, out int categoryId) || !int.TryParse(textBox3.Text, out int parCategoryId))
                MessageBox.Show("ID должен быть числом");

            else if(categoryId < 1)
                MessageBox.Show("ID должен быть больше 0");

            else if(parCategoryId < 0)
                MessageBox.Show("Родительский ID должен положительным числом");

            else if(!isDelete && parCategoryId != 0 && !CategoryModel.IsExistsCategoryId(parCategoryId))
                MessageBox.Show("Родительский ID не найден");

            else
                return true;


            return false;
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
            {
                label1.BackColor = Color.Brown;
                isEdit = false;
                textBox1.Text = (count + 1).ToString();
                textBox2.Text = string.Empty;
                textBox3.Text = "0";
                return;
            }

            label1.BackColor = Color.DarkSlateGray;
            isEdit = true;
            textBox1.Text = listView1.SelectedItems[0].SubItems[0].Text;
            textBox3.Text = listView1.SelectedItems[0].SubItems[1].Text;
            textBox2.Text = listView1.SelectedItems[0].SubItems[2].Text;
        }

        private int InsertItem()
        {
            int inserted = 0;

            inserted = Database.Insert(new CategoryTable
            {
                CategoryId = int.Parse(textBox1.Text),
                ParCategoryId = int.Parse(textBox3.Text),
                Title = textBox2.Text
            });

            if (inserted != 1)
                MessageBox.Show("Данные существуют");

            return inserted;
        }

        private int UpdateSelectedItem()
        {
            try
            {
                return Database.Update(new CategoryTable
                {
                    CategoryId = int.Parse(listView1.SelectedItems[0].Text),
                    ParCategoryId = int.Parse(textBox3.Text),
                    Title = textBox2.Text
                });
            }
            catch { return 0; }
        }

        private int DeleteSelectedItem()
        {
            try
            {
                var category = OfferModel.GetOneByCategoryTitle(listView1.SelectedItems[0].SubItems[2].Text);

                if (category.Count() > 0)
                {
                    MessageBox.Show("[" + category.First().OfferId + "] " + category.First().Name + " - использует эту категорию");
                    return 0;
                }

                // Clear submenu
                var subMenus = CategoryModel.GetAllParCategoryId(int.Parse(listView1.SelectedItems[0].Text));

                DialogResult choose = DialogResult.Yes;
                if (subMenus.Count() > 0)
                {
                    choose = MessageBox.Show("К этому родительскому ID привязаны другие категории" + Environment.NewLine
                        + "Вы уверены, что хотите продолжить?" + Environment.NewLine
                        + "Все привязанные категории будут сброшены, а окно закрыто для обновления данных",
                        Methods.NAME, MessageBoxButtons.YesNo);
                }

                if (choose == DialogResult.No)
                    return 0;

                foreach (var subMenu in subMenus)
                {
                    Database.Update(new CategoryTable {
                        CategoryId = subMenu.CategoryId,
                        Title = subMenu.Title,
                        ParCategoryId = 0
                    });
                }
                // End

                // Delete parametrs
                Database.DeleteObject<ParametrsTable>(listView1.SelectedItems[0].SubItems[2].Text);
                // End
                
                return Database.DeleteObject<CategoryTable>(listView1.SelectedItems[0].Text);
            }
            catch { return 0; }
        }
    }
}

using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

using XML.classes;
using XML.classes.db.offer;
using XML.classes.db.category;
using XML.classes.db.parametrs;

namespace XML.forms
{
    public partial class FormCategory : Form
    {
        private bool isEdit = false;

        public FormCategory()
        {
            InitializeComponent();

            Text = Methods.NAME + " - Категории";
            listView1.Columns.Add("ID");
            listView1.Columns.Add("Родительский ID");
            listView1.Columns.Add("Название");
            textBox1.Text = (CategoryModel.GetCount() + 1).ToString();
            textBox3.Text = "0";

            FillListView();
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void FillListView()
        {
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
                listView1.Items.Add(new ListViewItem(new[] {
                    textBox1.Text, textBox3.Text, textBox2.Text
                }));
            }
            else
                return;
            
            textBox1.Text = (CategoryModel.GetCount() + 1).ToString();
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
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

        private void CorrectInputs()
        {
            textBox1.Text = textBox1.Text.Trim();
            textBox2.Text = textBox2.Text.Trim();
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
                textBox1.Text = (CategoryModel.GetCount() + 1).ToString();
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

            inserted = CategoryModel.Insert(new CategoryTable
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
                int id = CategoryModel.GetOne(
                    int.Parse(listView1.SelectedItems[0].Text)
                ).First().Id;

                return CategoryModel.Update(new CategoryTable
                {
                    Id = id,
                    CategoryId = int.Parse(textBox1.Text),
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
                int id = CategoryModel.GetOne(
                    listView1.SelectedItems[0].SubItems[2].Text
                ).First().Id;

                var category = OfferModel.GetOneByCategoryTitle(listView1.SelectedItems[0].SubItems[2].Text);

                if (category.Count() > 0)
                {
                    MessageBox.Show("[" + category.First().Id + "] " + category.First().Name + " - использует эту категорию");
                    return 0;
                }

                // Clear submenu
                var subMenus = CategoryModel.GetAllParCategoryId(int.Parse(listView1.SelectedItems[0].SubItems[0].Text));

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
                    CategoryModel.Update(new CategoryTable {
                        Id = subMenu.Id,
                        CategoryId = subMenu.CategoryId,
                        Title = subMenu.Title,
                        ParCategoryId = 0
                    });
                }
                // End

                // Delete parametrs
                var parametrs = ParametrsModel.GetOneByCategoryTitle(listView1.SelectedItems[0].SubItems[2].Text);

                if (parametrs.Count() > 0)
                    ParametrsModel.DeleteObject<ParametrsTable>(parametrs.First().Id);
                // End

                if (subMenus.Count() > 0)
                {
                    CategoryModel.DeleteObject<CategoryTable>(id);
                    Close();
                    return 1;
                }
                
                return CategoryModel.DeleteObject<CategoryTable>(id);
            }
            catch { return 0; }
        }
    }
}

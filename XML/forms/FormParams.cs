using System;
using System.Linq;
using System.Windows.Forms;

using XML.classes;
using XML.classes.db;
using XML.classes.db.category;
using XML.classes.db.parametrs;

namespace XML.forms
{
    public partial class FormParams : Form
    {
        public FormParams()
        {
            InitializeComponent();

            Text = Methods.NAME + " - Параметры";

            var categories = new CategoryModel().GetAll();

            foreach (var category in categories)
            {
                listBox1.Items.Add(category.Title);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count < 1)
                return;

            var model = new ParametrsModel().GetOneByCategoryTitle(listBox1.SelectedItem.ToString());

            textBox1.Text = GenerateParametrs(textBox1.Text);

            var table = new ParametrsTable
            {
                CategoryTitle = listBox1.SelectedItem.ToString(),
                Parametrs = textBox1.Text
            };

            int saved = model.Count() > 0 ? new Database().Update(table) : new Database().Insert(table);

            if (saved == 1)
                MessageBox.Show("Сохранено");
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;

            if (listBox1.SelectedItems.Count < 1)
                return;

            var parametrs = new ParametrsModel().GetAllByCategoryTitle(listBox1.SelectedItem.ToString());

            if (parametrs.Count() < 1)
                return;

            textBox1.Text = parametrs.First().Parametrs;
        }

        private string GenerateParametrs(string text)
        {
            string[] arr = text.Split('\n');
            string outText = "";
            
            foreach (var line in arr)
            {
                if (! string.IsNullOrWhiteSpace(line))
                    outText += Methods.FirstCharToUpper(line.Trim()) + Environment.NewLine;
            }

            return outText.Trim();
        }
    }
}

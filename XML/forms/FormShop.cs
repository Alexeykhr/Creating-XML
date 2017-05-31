using System;
using System.Linq;
using System.Windows.Forms;

using XML.classes;
using XML.classes.db.shop;

namespace XML.forms
{
    public partial class FormShop : Form
    {
        public bool isNew = true;

        public FormShop()
        {
            InitializeComponent();

            Text = "XML - Магазин";

            FillForm();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (UpdateNewData())
            {
                MessageBox.Show("Данные сохранены");
                Close();
            }
        }

        private bool UpdateNewData()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Имя не указано");
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Название не указано");
                return false;
            }

            if (!Methods.IsWebSite(textBox3.Text))
            {
                MessageBox.Show("Ссылка имеет неверный формат");
                return false;
            }

            var company = new ShopTable
            {
                Id = 1,
                Name = textBox1.Text,
                Company = textBox2.Text,
                Url = textBox3.Text
            };

            int isUpdated = isNew ? ShopModel.Insert(company) : ShopModel.Update(company);

            if (isUpdated == 1)
            {
                isNew = false;
                return true;
            }

            return false;
        }

        private void FillForm()
        {
            var company = ShopModel.Get();

            if (company != null && company.Count() > 0)
            {
                var data = company.First();

                textBox1.Text = data.Name;
                textBox2.Text = data.Company;
                textBox3.Text = data.Url;

                isNew = false;
            }
        }
    }
}

using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

using XML.classes;
using XML.classes.db;
using XML.classes.db.offer;
using XML.classes.db.currency;

namespace XML.forms
{
    public partial class FormCurrency : Form
    {
        private bool isEdit = false;
        private int count = 0;
        
        public FormCurrency()
        {
            InitializeComponent();
            
            Text = Methods.NAME + " - Валюты";

            Init();
        }

        private void Init()
        {
            // ListView
            listView1.Columns.Add("Валюта");
            listView1.Columns.Add("Ставка");

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

            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            // Other
            label2.Text = "Валюта";
            label3.Text = "Ставка";
            count = currencies.Count();
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
            {
                label1.BackColor = Color.Brown;
                isEdit = false;
                textBox1.Text = string.Empty;
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

            textBox1.Text = textBox1.Text.ToUpper().Trim();
            textBox2.Text = Methods.ReplaceComma(textBox2.Text);

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
                count++;
            }
            else
                return;
            
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
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
                listView1.SelectedItems[0].Remove();
                count--;
            }
            else
                MessageBox.Show("Данные не удалились");
        }

        private int InsertItem()
        {
            int inserted = Database.Insert(new CurrencyTable
            {
                CurrencyId = textBox1.Text,
                Rate = textBox2.Text
            });

            if (inserted != 1)
                MessageBox.Show("Данные существуют");

            return inserted;
        }

        private int UpdateSelectedItem()
        {
            try
            {
                return Database.Update(new CurrencyTable
                {
                    CurrencyId = listView1.SelectedItems[0].Text,
                    Rate = textBox2.Text
                });
            }
            catch { return 0; }
        }

        private int DeleteSelectedItem()
        {
            try
            {
                var offer = OfferModel.GetOneByCurrencyId(listView1.SelectedItems[0].Text);

                if (offer.Count() > 0)
                {
                    MessageBox.Show("[" + offer.First().OfferId + "] " + offer.First().Name + " - использует эту валюту");
                    return 0;
                }

                return Database.DeleteObject<CurrencyTable>(listView1.SelectedItems[0].Text);
            }
            catch { return 0; }
        }
    }
}

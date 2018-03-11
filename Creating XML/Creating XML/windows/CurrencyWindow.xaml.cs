using System.Windows;
using Creating_XML.src.db;
using Creating_XML.src.store;
using System.Windows.Controls;
using Creating_XML.src.db.tables;
using Creating_XML.windows.dialogs;

namespace Creating_XML.windows
{
    public partial class CurrencyWindow : Window
    {
        private bool _isUpdated;

        public CurrencyWindow()
        {
            InitializeComponent();
            listViewCurrencies.ItemsSource = CurrencyStore.List;
        }

        /// <summary>
        /// Update GUI (Fill data).
        /// </summary>
        private void GUI()
        {
            listViewCurrencies.ItemsSource = null;
            listViewCurrencies.ItemsSource = CurrencyStore.Fetch();
        }

        /// <summary>
        /// Add a new record to DB.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCurrency_Click(object sender, RoutedEventArgs e)
        {
            string name = fName.Text.Trim().ToUpper(),
                rate = fRate.Text.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(rate))
                return;

            var table = new CurrencyTable { Name = name, Rate = rate };
            int result = Database.Insert(table);

            if (result == 1)
            {
                fName.Text = string.Empty;
                fRate.Text = string.Empty;
                GUI();
            }
            else
                MessageBox.Show("Валюта существует");
        }

        /// <summary>
        /// Open the dialog for Edit/Delete item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewCurrencies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = listViewCurrencies.SelectedItem as CurrencyTable;

            if (item == null)
                return;

            var dialog = new CurrencyItemDialog(item);
            dialog.ShowDialog();

            if (dialog.IsUpdated())
            {
                _isUpdated = true;
                GUI();
            }

            listViewCurrencies.SelectedItem = null;
        }

        /// <summary>
        /// Get the value is updated.
        /// </summary>
        /// <returns></returns>
        public bool IsUpdated()
        {
            return _isUpdated;
        }
    }
}

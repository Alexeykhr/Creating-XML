using System.Windows;
using Creating_XML.src.db;
using Creating_XML.src.db.tables;

namespace Creating_XML.windows.dialogs
{
    public partial class CurrencyItemDialog : Window
    {
        private CurrencyTable _item;

        private bool _isUpdated;

        public CurrencyItemDialog(CurrencyTable item)
        {
            InitializeComponent();
            fName.Text = item.Name;
            fRate.Text = item.Rate;
            _item = item;
        }

        /// <summary>
        /// Get the value updated.
        /// </summary>
        /// <returns></returns>
        public bool IsUpdated()
        {
            return _isUpdated;
        }

        /// <summary>
        /// Update Currency item in the DB.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string name = fName.Text.Trim().ToUpper(),
                rate = fRate.Text.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(rate))
                return;

            _item.Name = name;
            _item.Rate = rate;

            int result = Database.Update(_item);

            if (result == 1)
            {
                _isUpdated = true;
                Close();
            }
            else
                MessageBox.Show("Валюта не обновлена. Проверьте на уникальность.");
        }

        /// <summary>
        /// Delete Currency item in the DB.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int result = Database.Delete(_item);

            if (result == 1)
            {
                _isUpdated = true;
                Close();
            }
            else
                MessageBox.Show("Валюта не удалена.");
        }
    }
}

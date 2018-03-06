using Creating_XML.src.db;
using Creating_XML.src.db.tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
                MessageBox.Show("Валюта не обновлена. Проверьте на уникальность валюты.");
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

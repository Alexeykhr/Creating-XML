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
    public partial class CategoryItemDialog : Window
    {
        private CategoryTable _item;

        private bool _isUpdated;

        public CategoryItemDialog(CategoryTable item)
        {
            InitializeComponent();
            // FIll GUI
            _item = item;
        }

        /// <summary>
        /// Delete Category item in the DB.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var categories = Database.Query<CategoryTable>("SELECT * FROM CategoryTable WHERE ParentId = ? LIMIT 1", _item.Id);

            if (categories.Count > 0)
            {
                MessageBox.Show("Невозможно удалить. К этой категории привязаные подкатегории");
                return;
            }

            int result = Database.Delete(_item);

            
            if (result == 1)
            {
                _isUpdated = true;
                Close();
            }
            else
                MessageBox.Show("Валюта не удалена.");

        }

        /// <summary>
        /// Get the value is updated.
        /// </summary>
        /// <returns></returns>
        public bool IsUpdated()
        {
            return _isUpdated;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

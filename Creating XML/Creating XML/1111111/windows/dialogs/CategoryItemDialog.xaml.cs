using Creating_XML.src.db.tables;
using Creating_XML.src.db;
using System.Windows;

namespace Creating_XML.windows.dialogs
{
    public partial class CategoryItemDialog : Window
    {
        private CategoryTable _item;

        private bool _isUpdated;

        public CategoryItemDialog(CategoryTable item)
        {
            InitializeComponent();
            fCategory.Text = item.Name;
            _item = item;
        }

        /// <summary>
        /// Get the value is updated.
        /// </summary>
        /// <returns></returns>
        public bool IsUpdated()
        {
            return _isUpdated;
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
        /// Update Category item in the DB.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string category = fCategory.Text.Trim();

            if (string.IsNullOrWhiteSpace(category))
                return;

            _item.Name = category;

            int result = Database.Update(_item);

            if (result == 1)
            {
                _isUpdated = true;
                Close();
            }
            else
                MessageBox.Show("Категория не обновлена. Проверьте на уникальность.");
        }
    }
}

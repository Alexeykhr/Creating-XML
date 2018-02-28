using Creating_XML.src;
using Creating_XML.src.db;
using Creating_XML.src.db.tables;
using Creating_XML.src.store;
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

namespace Creating_XML.windows
{
    public partial class CategoryWindow : Window
    {
        public CategoryWindow()
        {
            InitializeComponent();
            GUI();
        }

        /// <summary>
        /// Update GUI (Fill data).
        /// </summary>
        private void GUI()
        {
            treeView.ItemsSource = null;
            treeView.ItemsSource = CategoryStore.FetchNewList();
        }

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            // TODO fParentCategory

            string category = RegexHelper.RemoveExtraSpace(fCategory.Text);

            if (string.IsNullOrEmpty(category))
                return;

            // TODO ParentId
            var table = new CategoryTable { Name = category };
            int result = Database.Insert(table);

            if (result == 1)
            {
                fCategory.Text = string.Empty;
                fParentCategory.Text = string.Empty;
                GUI();
            }
            else
                MessageBox.Show("Ошибка при добавлении. Проверьте уникальность категории.");
        }

        private void fParentCategory_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // TODO Open dialog for edit parent Category
        }
    }
}

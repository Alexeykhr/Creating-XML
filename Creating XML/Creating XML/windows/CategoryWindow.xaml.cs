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
        private CategoryTable selectedItem;

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
            string category = RegexHelper.RemoveExtraSpace(fCategory.Text);

            if (string.IsNullOrEmpty(category))
                return;
            
            var table = new CategoryTable { Name = category };

            if (selectedItem != null)
                table.ParentId = selectedItem.Id;

            int result = Database.Insert(table);

            if (result == 1)
            {
                fCategory.Text = string.Empty;
                fParentCategory.Text = string.Empty;
                selectedItem = null;
                GUI();
            }
            else
                MessageBox.Show("Ошибка при добавлении. Проверьте уникальность категории.");
        }

        private void fParentCategory_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // TODO Open dialog for edit parent Category
            MessageBox.Show("Open dialog");
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue == null)
                return;
            
            selectedItem = e.NewValue as CategoryTable;
            fParentCategory.Text = selectedItem.Name;
        }
    }
}

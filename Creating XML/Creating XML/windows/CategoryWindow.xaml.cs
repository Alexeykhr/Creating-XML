using Creating_XML.src;
using Creating_XML.src.db;
using Creating_XML.src.store;
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
using Creating_XML.windows.dialogs;

namespace Creating_XML.windows
{
    public partial class CategoryWindow : Window
    {
        private CategoryTable selectedItem;

        private bool _isUpdated;

        public CategoryWindow()
        {
            InitializeComponent();
            treeView.ItemsSource = CategoryStore.ListTree;
        }

        /// <summary>
        /// Update GUI.
        /// </summary>
        private void GUI()
        {
            treeView.ItemsSource = null;
            treeView.ItemsSource = CategoryStore.Fetch();
        }

        /// <summary>
        /// Added a new category to the DB.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            string category = RegexHelper.RemoveExtraSpace(fCategory.Text);

            if (string.IsNullOrEmpty(category))
                return;
            
            var table = new CategoryTable { Name = category };

            // If selected item => fill ParentId
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

        /// <summary>
        /// Open DialogWindow for edit/delete selected parent category.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fParentCategory_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // TODO Open dialog for edit parent Category
            //(treeView.SelectedItem as TreeViewItem).IsSelected = false;
        }

        /// <summary>
        /// Select new item parent category.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //var item = treeView.SelectedItem as CategoryTable;

            //if (item == null)
            //    return;

            //var dialog = new CategoryItemDialog(item);
            //dialog.ShowDialog();

            //if (dialog.IsUpdated())
            //{
            //    _isUpdated = true;
            //    GUI();
            //}

            //treeView.SelectedItem = null;

            if (e.NewValue == null)
                return;
            
            selectedItem = e.NewValue as CategoryTable;
            fParentCategory.Text = selectedItem.Name;
        }
    }
}

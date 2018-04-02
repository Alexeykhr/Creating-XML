using System.Windows;
using Creating_XML.src;
using Creating_XML.src.db;
using System.Windows.Input;
using Creating_XML.src.store;
using System.Windows.Controls;
using Creating_XML.src.db.tables;
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
                TreeViewClearSelected();
                fCategory.Text = string.Empty;
                fParentCategory.Text = string.Empty;
                selectedItem = null;
                _isUpdated = true;
                GUI();
            }
            else
                MessageBox.Show("Ошибка при добавлении. Проверьте уникальность категории.");
        }

        /// <summary>
        /// Clear parentCategory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fParentCategory_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewClearSelected();
            fParentCategory.Text = string.Empty;
            selectedItem = null;
        }

        /// <summary>
        /// Select new item parent category.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue == null)
                return;
            
            selectedItem = e.NewValue as CategoryTable;
            fParentCategory.Text = selectedItem.Name;
        }

        /// <summary>
        /// Open DialogWindow for edit/delete selected parent category.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditParent_Click(object sender, RoutedEventArgs e)
        {
            if (selectedItem == null)
                return;

            var dialog = new CategoryItemDialog(selectedItem);
            dialog.ShowDialog();

            if (dialog.IsUpdated())
            {
                _isUpdated = true;
                GUI();
            }
        }

        /// <summary>
        /// Set isSelected to false.
        /// </summary>
        private void TreeViewClearSelected()
        {
            if (treeView.SelectedItem == null)
                return;

            if (treeView.SelectedItem is TreeViewItem)
            {
                (treeView.SelectedItem as TreeViewItem).IsSelected = false;
            }
            else
            {
                if (treeView.ItemContainerGenerator.ContainerFromIndex(0) is TreeViewItem item)
                {
                    item.IsSelected = true;
                    item.IsSelected = false;
                }
            }
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

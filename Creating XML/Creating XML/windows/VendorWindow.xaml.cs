using System.Windows;
using Creating_XML.src.db;
using Creating_XML.src.store;
using System.Windows.Controls;
using Creating_XML.src.db.tables;
using Creating_XML.windows.dialogs;

namespace Creating_XML.windows
{
    public partial class VendorWindow : Window
    {
        private bool _isUpdated;

        public VendorWindow()
        {
            InitializeComponent();
            listBoxVendors.ItemsSource = VendorStore.List;
        }

        /// <summary>
        /// Update GUI.
        /// </summary>
        private void GUI()
        {
            listBoxVendors.ItemsSource = null;
            listBoxVendors.ItemsSource = VendorStore.Fetch();
        }

        /// <summary>
        /// Add a new vendor to DB.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddVendor_Click(object sender, RoutedEventArgs e)
        {
            var vendor = fVendor.Text.Trim();

            if (string.IsNullOrWhiteSpace(vendor))
                return;

            var table = new VendorTable { Name = vendor };
            int result = Database.Insert(table);

            if (result == 1)
            {
                fVendor.Text = string.Empty;
                GUI();
            }
            else
                MessageBox.Show("Продавец существует");
        }

        /// <summary>
        /// Open the dialog for Edit/Delete item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxVendors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = listBoxVendors.SelectedItem as VendorTable;

            if (item == null)
                return;

            var dialog = new VendorItemDialog(item);
            dialog.ShowDialog();

            if (dialog.IsUpdated())
            {
                _isUpdated = true;
                GUI();
            }

            listBoxVendors.SelectedItem = null;
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

using System.Windows;
using Creating_XML.src.db;
using Creating_XML.src.store;
using System.Windows.Controls;
using Creating_XML.src.db.tables;

namespace Creating_XML.windows
{
    public partial class VendorWindow : Window
    {
        private VendorTable selectedItem;
        
        public VendorWindow()
        {
            InitializeComponent();
            listBoxVendors.ItemsSource = VendorStore.List;
        }

        /// <summary>
        /// Update GUI (Fill data).
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
        /// Set selectedItem.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxVendors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = listBoxVendors.SelectedItem as VendorTable;
            // TODO Create new window for edit - OneEditWindow
        }
    }
}

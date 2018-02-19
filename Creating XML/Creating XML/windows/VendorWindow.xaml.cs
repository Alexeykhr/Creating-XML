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
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public VendorWindow()
        {
            InitializeComponent();
            GUI();
        }

        /// <summary>
        /// Update GUI (Fill data).
        /// </summary>
        private void GUI()
        {
            listBoxVendors.ItemsSource = null;
            listBoxVendors.ItemsSource = VendorStore.FetchNewList();
        }

        /// <summary>
        /// Add a new vendor to DB.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddVendor_Click(object sender, RoutedEventArgs e)
        {
            var text = fVendor.Text;

            if (string.IsNullOrWhiteSpace(text))
                return;

            var table = new VendorTable { Name = fVendor.Text };
            int result = Database.Insert(table);

            if (result == 1)
            {
                // TODO Snackbar
                snackbar.MessageQueue.Enqueue("Продавец добавлен");
                fVendor.Text = string.Empty;
                GUI();
            }
            else
                snackbar.MessageQueue.Enqueue("Продавец существует");
        }

        /// <summary>
        /// Set selectedItem and Flip content.
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

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

namespace Creating_XML.windows
{
    /// <summary>
    /// Логика взаимодействия для VendorWindow.xaml
    /// </summary>
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
            // Save in store
            var items = Database.List<VendorTable>().OrderBy(v => v.Name);

            listBoxVendors.ItemsSource = null;
            listBoxVendors.ItemsSource = items;
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
                GUI();
                fVendor.Text = string.Empty;
            }
            else
            {
                // TODO Show
                MessageBox.Show("Запись существует");
            }
        }

        /// <summary>
        /// Set selectedItem and Flip content.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxVendors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = listBoxVendors.SelectedItem as VendorTable;
            flipper.IsFlipped = true;
        }
    }
}

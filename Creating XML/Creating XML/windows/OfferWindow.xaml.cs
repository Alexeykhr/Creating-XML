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
    public partial class OfferWindow : Window
    {
        private CategoryTable selectedCategory;
        private CurrencyTable selectedCurrency;
        private VendorTable selectedVendor;

        public OfferWindow()
        {
            InitializeComponent();
            dataGridParams.ItemsSource = new List<OfferParametersTable>();
            fVendor.ItemsSource = VendorStore.List;
            fCurrency.ItemsSource = CurrencyStore.List;
            fCategory.ItemsSource = CategoryStore.List;
        }

        private void btnImageAdd_Click(object sender, RoutedEventArgs e)
        {
            bool isUrl = Web.IsCorrectURL(fImageUrl.Text);

            if (isUrl)
            {
                // TODO: Test
                fImage.Source = new BitmapImage(new Uri(fImageUrl.Text));
                listBoxImages.Items.Add(fImageUrl.Text);
                listBoxImages.Items.MoveCurrentToLast();
                fImageUrl.Text = string.Empty;
            }
        }

        private void btnAddOffer_Click(object sender, RoutedEventArgs e)
        {
            // TODO Test
            string name = fName.Text.Trim(),
                url = fUrl.Text.Trim(),
                description = fDescription.Text.Trim();

            int article = int.Parse(fArticle.Text),
                price = int.Parse(fPrice.Text);

            bool isAvailable = fIsAvailable.IsChecked.Value;

            var table = new OfferTable
            {
                Name = name,
                Article = article,
                Price = price,
                Url = url,
                Description = description,
                IsAvailable = isAvailable,
                VendorId = selectedVendor.Id,
                CategoryId = selectedCategory.Id,
                CurrencyId = selectedCurrency.Id
            };
            int result = Database.Insert(table);

            if (result == 1)
                MessageBox.Show("Товар добавлен");
            else
                MessageBox.Show("Ошибка при добавлении");
        }

        private void fCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCategory = e.AddedItems[0] as CategoryTable;
        }

        private void fCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCurrency = e.AddedItems[0] as CurrencyTable;
        }

        private void fVendor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedVendor = e.AddedItems[0] as VendorTable;
        }
    }
}

using System;
using System.Windows;
using Creating_XML.src;
using Creating_XML.src.db;
using Creating_XML.src.db.tables;
using Creating_XML.src.objects;
using Creating_XML.src.store;
using System.Windows.Controls;
using System.Collections.Generic;
using Creating_XML.src.db.tables;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Creating_XML.windows
{
    public partial class OfferWindow : Window
    {
        private CategoryTable selectedCategory;
        private CurrencyTable selectedCurrency;
        private VendorTable selectedVendor;

        private CategoryTable _selectedCategory;
        private CurrencyTable _selectedCurrency;
        private VendorTable _selectedVendor;

        private OfferObject _offer;

        public OfferWindow(OfferObject offer = null)
        {
            InitializeComponent();

            _offer = new OfferObject();
            _offer.Images = new List<OfferImageTable>();
            _offer.Parameters = new List<OfferParameterTable>();
            
            fVendor.ItemsSource = VendorStore.List;
            fCurrency.ItemsSource = CurrencyStore.List;
            fCategory.ItemsSource = CategoryStore.List;
        }

        private void btnImageAdd_Click(object sender, RoutedEventArgs e)
        {
            if (Web.IsCorrectURL(fImageUrl.Text))
            {
                fImage.Source = new BitmapImage(new Uri(fImageUrl.Text));
                
                _offer.Images.Add(new OfferImageTable {
                    Url = fImageUrl.Text
                });

                listBoxImages.ItemsSource = _offer.Images;

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

            var offer = new OfferTable
            {
                Name = name,
                Article = article,
                Price = price,
                Url = url,
                Description = description,
                IsAvailable = isAvailable,
                VendorId = _selectedVendor.Id,
                CategoryId = _selectedCategory.Id,
                CurrencyId = _selectedCurrency.Id
            };
            int result = Database.Insert(offer);

            if (result == 1)
            {
                foreach (var image in listBoxImages.Items)
                {
                    Database.InsertOrReplace(new OfferImageTable
                    {
                        OfferId = offer.Id,
                        Url = (image as OfferImageTable).Url
                    });
                }

                foreach (var parameter in dataGridParams.Items)
                {
                    var par = parameter as OfferParameterTable;

                    Database.InsertOrReplace(new OfferParameterTable
                    {
                        OfferId = offer.Id,
                        Name = par.Name,
                        Value = par.Value
                    });
                }

                MessageBox.Show("Товар добавлен");
            }
            else
                MessageBox.Show("Ошибка при добавлении");
        }

        private void fCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedCategory = e.AddedItems[0] as CategoryTable;
        }

        private void fCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedCurrency = e.AddedItems[0] as CurrencyTable;
        }

        private void fVendor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedVendor = e.AddedItems[0] as VendorTable;
        }
    }
}

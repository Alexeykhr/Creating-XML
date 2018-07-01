using Creating_XML.core;
using Creating_XML.src;
using Creating_XML.src.objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Creating_XML.windows
{
    public partial class MainWindow : Window
    {
        private Pagination _pagination;

        private int _lastNumber;

        /// <summary>
        /// Select file before work (open window).
        /// </summary>
        /// <see cref="OpenFileWindow()"/>
        public MainWindow()
        {
            InitializeComponent();

            if (!OpenFileWindow())
                return;

            _pagination = new Pagination(fCurrentPage, fMaxItemsOnPage, btnNextPage, btnPrevPage, GUI);

            //CategoryStore.Fetch();
            //CurrencyStore.Fetch();
            //VendorStore.Fetch();
        }

        /// <summary>
        /// OpenFileWindow for select a file and connect to the Database.
        /// </summary>
        /// <see cref="SelectFileWindow"/>
        /// <returns></returns>
        private bool OpenFileWindow()
        {
            Hide();

            var fileWindow = new SelectFileWindow();
            fileWindow.ShowDialog();

            if (fileWindow.IsOpened)
            {
                Show();
                return true;
            }

            Application.Current.Shutdown();
            return false;
        }

        /// <summary>
        /// Update GUI (Fill data).
        /// </summary>
        private bool GUI()
        {
            UpdateListView(fSearch.Text, _pagination.MaxItemsOnPageNumber, _pagination.CurrentPageNumber); // FIXME All params
            return true;
        }

        /// <summary>
        /// Query to Database and update ListView.
        /// </summary>
        private async void UpdateListView(string search, int take, int page)
        {
            //if (!Database.HasConnection())
            //    return;
            
            //var collection = await Task.Run(() =>
            //{
            //    try
            //    {
            //        return OfferModel.List(search, take, page);
            //    }
            //    catch { return null; }
            //});

            //if (collection == null)
            //{
            //    MessageBox.Show("Ошибка в Базе данных");
            //    OpenFileWindow();
            //    return;
            //}
            
            //if (collection.Count < 1 && page > 1)
            //{
            //    fCurrentPage.Text = (page - 1).ToString();
            //    _currentPage = page - 1;
            //}

            //listView.ItemsSource = collection;
            //listView.Items.Refresh();
        }

        /// <summary>
        /// Open OfferWindow for add a new offer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddOffer_Click(object sender, RoutedEventArgs e)
        {
            //var window = new OfferWindow();
            //window.ShowDialog();
            // TODO IsUpdated
        }

        /// <summary>
        /// Seach Article or Name of offer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            GUI();
        }

        /*
         * |-------------------------------------------
         * | Menu items.
         * |-------------------------------------------
         * |
         */

        /// <summary>
        /// Open window for select file.
        /// </summary>
        /// <see cref="OpenFileWindow()"/>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileWindow();
        }

        /// <summary>
        /// Close the program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Open VendorWidnow for add a new vendor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemVendor_Click(object sender, RoutedEventArgs e)
        {
            //var window = new VendorWindow();
            //window.ShowDialog();

            //if (window.IsUpdated())
            //    GUI();
        }

        /// <summary>
        /// Open CategoryWindow for add a new currency.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemCurrency_Click(object sender, RoutedEventArgs e)
        {
            //var window = new CurrencyWindow();
            //window.ShowDialog();

            //if (window.IsUpdated())
            //    GUI();
        }

        /// <summary>
        /// Open ShowWindow for config info about shop.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemShop_Click(object sender, RoutedEventArgs e)
        {
            //new ShopWindow().ShowDialog();
        }

        /// <summary>
        /// Open CategoryWindow for add a new category.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemCategory_Click(object sender, RoutedEventArgs e)
        {
            //var window = new CategoryWindow();
            //window.ShowDialog();

            //if (window.IsUpdated())
            //    GUI();
        }
    }
}

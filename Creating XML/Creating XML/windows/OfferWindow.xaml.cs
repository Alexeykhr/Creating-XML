using Creating_XML.src;
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
    /// <summary>
    /// Логика взаимодействия для OfferWindow.xaml
    /// </summary>
    public partial class OfferWindow : Window
    {
        public OfferWindow()
        {
            InitializeComponent();
            dataGridParams.ItemsSource = new List<OfferParametersTable>();
            fVendor.ItemsSource = VendorStore.List;
        }

        private void btnImageAdd_Click(object sender, RoutedEventArgs e)
        {
            bool isUrl = Web.IsCorrectURL(fImageUrl.Text);

            if (isUrl)
            {
                // TODO: For Tests
                fImage.Source = new BitmapImage(new Uri(fImageUrl.Text));
                listBoxImages.Items.Add(fImageUrl.Text);
                listBoxImages.Items.MoveCurrentToLast();
                fImageUrl.Text = string.Empty;
            }
        }
    }
}

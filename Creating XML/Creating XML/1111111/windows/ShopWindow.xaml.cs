using System.Windows;
using Creating_XML.src;

namespace Creating_XML.windows
{
    public partial class ShopWindow : Window
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ShopWindow()
        {
            InitializeComponent();
            GUI();
        }
        
        /// <summary>
        /// Update GUI (Fill data).
        /// </summary>
        private void GUI()
        {
            fName.Text = Settings.ShopName;
            fCompany.Text = Settings.ShopCompany;
            fUrl.Text = Settings.ShopUrl;
        }

        /// <summary>
        /// Sava data in Settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string name = fName.Text.Trim(),
                company = fCompany.Text.Trim(),
                url = fUrl.Text.Trim();
            
            Settings.ShopName = name;
            Settings.ShopCompany = company;

            if (string.IsNullOrWhiteSpace(url) || Web.IsCorrectURL(url))
                Settings.ShopUrl = url;

            MessageBox.Show("Сохранено");
            GUI();
        }
    }
}

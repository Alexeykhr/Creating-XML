using Creating_XML.src;
using Creating_XML.src.db;
using Creating_XML.src.db.models;
using Creating_XML.src.db.tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private int page = 1;
        private int count = 20;

        /// <summary>
        /// Select file before work (open window).
        /// </summary>
        /// <see cref="OpenFileWindow()"/>
        public MainWindow()
        {
            InitializeComponent();
            OpenFileWindow();
            UI();
        }

        /// <summary>
        /// OpenFileWindow for select a file and connect to the Database.
        /// </summary>
        /// <see cref="SelectFileWindow"/>
        private void OpenFileWindow()
        {
            if (Database.HasConnection())
                Database.CloseConnection();
            
            Hide();

            var fileWindow = new SelectFileWindow();
            fileWindow.ShowDialog();

            // If the file is not selected (the window is closed) - close the program.
            if (!fileWindow.IsOpened)
                Close();
            else
                Show();
        }

        /// <summary>
        /// Update UI (Fill data).
        /// </summary>
        private void UI()
        {
            fMaxItemsOnPage.Text = count.ToString();
            fCurrentPage.Text = page.ToString();

            var list = Database.List<OfferTable>();
            // TODO
        }

        /// <summary>
        /// After got focus = clear the text.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fCurrentPage_GotFocus(object sender, RoutedEventArgs e)
        {
            fCurrentPage.Text = string.Empty;
        }

        /// <summary>
        /// Check Input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fCurrentPage_LostFocus(object sender, RoutedEventArgs e)
        {
            bool isCorrect = int.TryParse(fCurrentPage.Text, out int page);

            if (!isCorrect || page < 1)
                page = this.page;

            fCurrentPage.Text = page.ToString();
            this.page = page;
        }

        /// <summary>
        /// Only accept numbers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fCurrentPage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
    }
}

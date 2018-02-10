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
        private int lastNumber;

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
            lastNumber = int.Parse(fCurrentPage.Text);
            fCurrentPage.Text = string.Empty;
        }

        /// <summary>
        /// Check Input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fCurrentPage_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(fCurrentPage.Text, out int result) || result < 1)
                fCurrentPage.Text = lastNumber.ToString();
        }

        /// <summary>
        /// Only accept numbers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fCurrentPage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = RegexHelper.IsOnlyNumbers(e.Text);
        }

        /// <summary>
        /// After got focus = clear the text.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fMaxItemsOnPage_GotFocus(object sender, RoutedEventArgs e)
        {
            lastNumber = int.Parse(fMaxItemsOnPage.Text);
            fMaxItemsOnPage.Text = string.Empty;
        }

        /// <summary>
        /// Check Input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fMaxItemsOnPage_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(fMaxItemsOnPage.Text, out int result) || result < 1)
                fMaxItemsOnPage.Text = lastNumber.ToString();
        }

        /// <summary>
        /// Only accept numbers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fMaxItemsOnPage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = RegexHelper.IsOnlyNumbers(e.Text);
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

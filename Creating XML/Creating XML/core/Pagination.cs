using System.Windows.Controls;
using System;

namespace Creating_XML.core
{
    class Pagination
    {
        private TextBox _currentPage;

        private TextBox _maxItemsOnPage;

        private Button _nextPage;

        private Button _prevPage;

        private Func<bool> _func;

        private int _currentPageNumber = 1;

        private int _maxItemsOnPageNumber = 20;

        private int _lastNumber;

        public Pagination(TextBox currentPage, TextBox maxItemsOnPage, Button nextPage, Button prevPage, Func<bool> func)
        {
            // Save incoming data to the class properties
            _currentPage = currentPage;
            _maxItemsOnPage = maxItemsOnPage;
            _nextPage = nextPage;
            _prevPage = prevPage;
            _func = func;

            // Initialize the handlers
            currentPage.GotFocus += CurrentPage_GotFocus;
            currentPage.LostFocus += CurrentPage_LostFocus;
            currentPage.PreviewTextInput += CurrentPage_PreviewTextInput;
            currentPage.TextChanged += CurrentPage_TextChanged;

            maxItemsOnPage.GotFocus += MaxItemsOnPage_GotFocus;
            maxItemsOnPage.LostFocus += MaxItemsOnPage_LostFocus;
            maxItemsOnPage.PreviewTextInput += MaxItemsOnPage_PreviewTextInput;
            maxItemsOnPage.TextChanged += MaxItemsOnPage_TextChanged;

            nextPage.Click += NextPage_Click;
            prevPage.Click += PrevPage_Click;

            // Set value
            _currentPageNumber = int.Parse(currentPage.Text);
            _maxItemsOnPageNumber = int.Parse(maxItemsOnPage.Text);
        }

        public int CurrentPageNumber { get => _currentPageNumber; set => _currentPageNumber = value; }

        public int MaxItemsOnPageNumber { get => _maxItemsOnPageNumber; set => _maxItemsOnPageNumber = value; }

        /* --------------------------------------------------------------------------------------------------------------- */

        private void CurrentPage_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            _lastNumber = int.Parse(_currentPage.Text);
            _currentPage.Text = string.Empty;
        }

        private void CurrentPage_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!int.TryParse(_currentPage.Text, out int result) || result < 1)
                _currentPage.Text = _lastNumber.ToString();
        }

        private void CurrentPage_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = RegexHelper.IsOnlyNumbers(e.Text);
        }

        private void CurrentPage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(_currentPage.Text, out int result) && result > 0)
            {
                _currentPageNumber = result;
                _func();
            }
        }

        /* --------------------------------------------------------------------------------------------------------------- */

        private void MaxItemsOnPage_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            _lastNumber = int.Parse(_maxItemsOnPage.Text);
            _maxItemsOnPage.Text = string.Empty;
        }

        private void MaxItemsOnPage_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!int.TryParse(_maxItemsOnPage.Text, out int result) || result < 1)
                _maxItemsOnPage.Text = _lastNumber.ToString();
        }

        private void MaxItemsOnPage_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = RegexHelper.IsOnlyNumbers(e.Text);
        }
        private void MaxItemsOnPage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(_maxItemsOnPage.Text, out int result) && result > 0)
            {
                _maxItemsOnPageNumber = result;
                _func();
            }
        }

        /* --------------------------------------------------------------------------------------------------------------- */

        private void PrevPage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_currentPageNumber > 1)
            {
                _currentPage.Text = (--_currentPageNumber).ToString();
                _func();
            }
        }

        private void NextPage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _currentPage.Text = (++_currentPageNumber).ToString();
            _func();
        }
    }
}

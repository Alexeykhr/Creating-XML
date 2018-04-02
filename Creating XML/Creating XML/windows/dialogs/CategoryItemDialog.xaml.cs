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

namespace Creating_XML.windows.dialogs
{
    /// <summary>
    /// Логика взаимодействия для CategoryItemDialog.xaml
    /// </summary>
    public partial class CategoryItemDialog : Window
    {
        private CategoryTable _item;

        private bool _isUpdated;

        public CategoryItemDialog(CategoryTable item)
        {
            InitializeComponent();
            // FIll GUI
            _item = item;
        }

        /// <summary>
        /// Get the value updated.
        /// </summary>
        /// <returns></returns>
        public bool IsUpdated()
        {
            return _isUpdated;
        }
    }
}

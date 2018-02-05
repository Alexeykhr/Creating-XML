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
    /// Логика взаимодействия для SelectFileWindow.xaml
    /// </summary>
    public partial class SelectFileWindow : Window
    {
        private bool _isOpened;
        private string _openedFileUri;

        public SelectFileWindow()
        {
            InitializeComponent();
        }

        public bool IsOpened
        {
            get { return _isOpened; }
        }

        public string OpenedFileUri
        {
            get { return _openedFileUri; }
        }
    }
}

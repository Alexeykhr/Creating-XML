using Creating_XML.src;
using Creating_XML.src.db;
using Creating_XML.src.db.models;
using Creating_XML.src.db.tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public MainWindow()
        {
            InitializeComponent();

            var fileWindow = new SelectFileWindow();
            fileWindow.ShowDialog();

            if (!fileWindow.IsOpened)
                Close();

            var list = Settings.LastFilesUri;
        }
    }
}

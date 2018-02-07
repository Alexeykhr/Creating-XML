using Creating_XML.src.db;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class SelectFileWindow : Window
    {
        private bool _isOpened;

        public SelectFileWindow()
        {
            InitializeComponent();
        }

        public bool IsOpened
        {
            get { return _isOpened; }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                FileName = "File",
                Filter = "(*.xmldb)|*.xmldb"
            };

            bool? result = sfd.ShowDialog();
            
            if (result == true)
                SelectFile(sfd.FileName, true);
        }

        private void SelectFile(string file, bool isNewProject)
        {
            try
            {
                _isOpened = true;
                Project.FileUri = file;
                Database.Connection(file);

                if (isNewProject)
                {
                    if (File.Exists(file))
                    {
                        Database.CloseConnection();
                        File.Delete(file);
                        Database.Connection(file);
                    }

                    Database.Migration();
                }

                Close();
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при работе с файлами");
            }
        }
    }
}

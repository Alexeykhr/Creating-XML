using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using Creating_XML.src;
using Creating_XML.src.db;

namespace Creating_XML.windows
{
    public partial class SelectFileWindow : Window
    {
        private const string FILTER_EXT = "(*.xmldb)|*.xmldb";

        private bool _isOpened;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SelectFileWindow()
        {
            InitializeComponent();
            DisplayRecentFiles();
        }

        /// <summary>
        /// Get property. Used in other classes.
        /// </summary>
        public bool IsOpened
        {
            get { return _isOpened; }
        }

        /// <summary>
        /// Fill ListView from Settings.
        /// </summary>
        public void DisplayRecentFiles()
        {
            listView.ItemsSource = Settings.LastFilesUri;
        }

        /// <summary>
        /// Сlick event to open file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = FILTER_EXT
            };

            bool? result = ofd.ShowDialog();

            if (result == true)
                SelectFile(ofd.FileName);
        }

        /// <summary>
        /// Сlick event to create file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                Filter = FILTER_EXT
            };

            bool? result = sfd.ShowDialog();
            
            if (result == true)
                SelectFile(sfd.FileName, true);
        }

        /// <summary>
        /// Connect to the file and close Window.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="isNewProject"></param>
        private void SelectFile(string file, bool isNewProject = false)
        {
            try
            {
                _isOpened = true;
                Project.FileUri = file;

                if (isNewProject)
                {
                    // If the file exists, delete it.
                    if (File.Exists(file))
                        File.Delete(file);

                    Database.Connection(file);
                    Database.Migration();
                }
                else
                    Database.Connection(file);

                Settings.InsertLastFile(file);

                Close();
            }
            catch
            {
                throw new Exception("Ошибка при работе с файлами");
            }
        }
    }
}

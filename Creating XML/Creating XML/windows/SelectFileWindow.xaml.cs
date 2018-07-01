using Creating_XML.src.objects;
using System.Windows.Controls;
using Creating_XML.src.db;
using Creating_XML.src;
using Microsoft.Win32;
using System.Windows;
using System.IO;
using System;

namespace Creating_XML.windows
{
    public partial class SelectFileWindow : Window
    {
        private const string FILTER_EXT = "(*.xmldb)|*.xmldb";

        private bool _isOpened;

        /// <summary>
        /// Iutput the latest files from Settings and establish a connection with the database.
        /// </summary>
        public SelectFileWindow()
        {
            InitializeComponent();
            DisplayRecentFiles();

            if (Database.HasConnection())
                Database.CloseConnection();
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
                OpenExistsProject(ofd.FileName);
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
                CreateNewProject(sfd.FileName);
        }

        /// <summary>
        /// Open the previous file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;

            if (item == null)
                return;

            OpenExistsProject((item as FileObject).Uri);
        }
        
        /// <summary>
        /// Create new project.
        /// </summary>
        /// <param name="file"></param>
        private void CreateNewProject(string file)
        {
            try
            {
                if (File.Exists(file))
                    File.Delete(file);

                Database.Connection(file);
                Database.Migration();

                SelectFile(file);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при работе с файлом");
            }
        }

        /// <summary>
        /// Open exists project.
        /// </summary>
        /// <param name="file"></param>
        private void OpenExistsProject(string file)
        {
            try
            {
                if (!File.Exists(file))
                {
                    MessageBox.Show("Файл не существует, ссылка удалена");
                    Settings.DeleteFileUri(file);
                    DisplayRecentFiles();
                    return;
                }

                Database.Connection(file);
                SelectFile(file);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при работе с файлом");
            }
        }

        /// <summary>
        /// Save choose and close this window.
        /// </summary>
        /// <param name="file"></param>
        private void SelectFile(string file)
        {
            _isOpened = true;
            Project.FileUri = file;
            Settings.InsertLastFile(file);
            Close();
        }

        /// <summary>
        /// Get property _isOpened.
        /// </summary>
        public bool IsOpened
        {
            get { return _isOpened; }
        }
    }
}

﻿using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using Creating_XML.src;
using Creating_XML.src.db;
using System.Windows.Controls;
using Creating_XML.src.objects;

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

            if (Database.HasConnection())
                Database.CloseConnection();
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
        /// Open the previous file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;

            if (item == null)
                return;

            SelectFile((item as FileObject).Uri);
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
                if (isNewProject)
                {
                    if (File.Exists(file))
                        File.Delete(file);

                    Database.Connection(file);
                    Database.Migration();
                }
                else
                {
                    if (!File.Exists(file))
                    {
                        MessageBox.Show("Файл не существует, ссылка удалена");
                        Settings.DeleteFileUri(file);
                        DisplayRecentFiles();
                        return;
                    }

                    Database.Connection(file);
                }

                _isOpened = true;
                Project.FileUri = file;
                Settings.InsertLastFile(file);

                Close();
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при работе с файлом");
            }
        }
    }
}

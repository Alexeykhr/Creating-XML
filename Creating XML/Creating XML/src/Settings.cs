using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Creating_XML.src.objects;

namespace Creating_XML.src
{
    class Settings
    {
        private const byte MAX_COUNT_FILES = 10;

        /// <summary>
        /// Display the last opened files.
        /// </summary>
        /// <see cref="windows.SelectFileWindow"/>
        public static List<FileObject> LastFilesUri
        {
            get
            {
                var list = Properties.Settings.Default.last_files_uri as List<FileObject>;

                if (list == null)
                    list = new List<FileObject>();

                return list;
            }
        }

        /// <summary>
        /// Save new list.
        /// </summary>
        /// <param name="uri"></param>
        public static void InsertLastFile(string uri)
        {
            List<FileObject> list = LastFilesUri;
            int searchIndex = list.FindIndex(x => x.Uri.Equals(uri));

            list.Insert(searchIndex > -1 ? searchIndex : 0, new FileObject
            {
                Uri = uri,
                OpenedAt = DateTime.Now
            });

            if (list.Count > MAX_COUNT_FILES)
                list.RemoveRange(MAX_COUNT_FILES, list.Count - MAX_COUNT_FILES);

            Properties.Settings.Default.last_files_uri = list;
            Properties.Settings.Default.Save();

            var newList = Properties.Settings.Default.last_files_uri;
        }
    }
}

using System;
using System.IO;
using Creating_XML.src.objects;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

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
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Properties.Settings.Default.last_files_uri)))
                {
                    try
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        return bf.Deserialize(ms) as List<FileObject>;
                    }
                    catch
                    {
                        return new List<FileObject>();
                    }
                }
            }
        }

        /// <summary>
        /// Add or update uri to the first place.
        /// </summary>
        /// <param name="uri"></param>
        public static void InsertLastFile(string uri)
        {
            List<FileObject> list = LastFilesUri;
            int searchIndex = list.FindIndex(x => x.Uri.Equals(uri));

            if (searchIndex > -1)
                list.RemoveAt(searchIndex);

            list.Insert(0, new FileObject
            {
                Uri = uri,
                OpenedAt = DateTime.Now
            });

            if (list.Count > MAX_COUNT_FILES)
                list.RemoveRange(MAX_COUNT_FILES, list.Count - MAX_COUNT_FILES);

            SaveLastFilesUri(list);
        }

        /// <summary>
        /// Save new list of FileObject.
        /// </summary>
        /// <param name="list"></param>
        private static void SaveLastFilesUri(List<FileObject> list)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, list);
                ms.Position = 0;
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                Properties.Settings.Default.last_files_uri = Convert.ToBase64String(buffer);
                Properties.Settings.Default.Save();
            }
        }
    }
}

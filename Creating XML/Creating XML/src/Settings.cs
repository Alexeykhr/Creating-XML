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
        /// Get or set ShopName.
        /// </summary>
        public static string ShopName
        {
            get
            {
                return Properties.Settings.Default.shop_name;
            }
            set
            {
                Properties.Settings.Default.shop_name = value;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Get or set ShopCompany.
        /// </summary>
        public static string ShopCompany
        {
            get
            {
                return Properties.Settings.Default.shop_company;
            }
            set
            {
                Properties.Settings.Default.shop_company = value;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Get or set ShopUrl.
        /// </summary>
        public static string ShopUrl
        {
            get
            {
                return Properties.Settings.Default.shop_url;
            }
            set
            {
                Properties.Settings.Default.shop_url = value;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Get the last opened files.
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
        /// Delete the URI file from the settings via the name.
        /// </summary>
        /// <param name="uri"></param>
        public static void DeleteFileUri(string uri)
        {
            List<FileObject> list = LastFilesUri;
            int searchIndex = list.FindIndex(x => x.Uri.Equals(uri));

            if (searchIndex == -1)
                return;

            list.RemoveAt(searchIndex);
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

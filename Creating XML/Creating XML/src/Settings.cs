using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Creating_XML.src
{
    class Settings
    {
        private const string LAST_FILES_URI = "last_files_uri";

        public static List<string> Files
        {
            get
            {
                try
                {
                    return GetStringCollection(LAST_FILES_URI).Cast<string>().ToList();
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                try
                {
                    var collection = new StringCollection();
                    collection.AddRange(value.ToArray());
                    Set(LAST_FILES_URI, collection);
                }
                catch { }
            }
        }

        public static void InsertLastProject(string fileUri)
        {
            var files = Files;
            files.Insert(0, fileUri);
            Files = files.GetRange(0, 10);
        }

        private static object Get(string name)
        {
            return Properties.Settings.Default[name];
        }

        private static StringCollection GetStringCollection(string name)
        {
            return Get(name) as StringCollection;
        }

        private static void Set(string name, object value)
        {
            Properties.Settings.Default[name] = value;
            Save();
        }

        private static void Set(string name, StringCollection collection)
        {
            Properties.Settings.Default[name] = collection;
            Save();
        }

        private static void Save()
        {
            Properties.Settings.Default.Save();
        }
    }
}

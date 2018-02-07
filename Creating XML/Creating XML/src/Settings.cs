using System.Collections.Generic;
using Creating_XML.src.objects;

namespace Creating_XML.src
{
    class Settings
    {
        private const string LAST_FILES_URI = "last_files_uri";

        /// <summary>
        /// Display the last opened files.
        /// </summary>
        /// <see cref="windows.SelectFileWindow"/>
        public static List<FileObject> LastFilesUri
        {
            get
            {
                return Properties.Settings.Default.last_files_uri;
            }
            set
            {
                Properties.Settings.Default.last_files_uri = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}

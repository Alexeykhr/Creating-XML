using Creating_XML.src.db.tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creating_XML.src.db
{
    class Project
    {
        private static string _file_uri = "";

        public static void CreateNewProject(string file)
        {
            _file_uri = file;
            Settings.InsertLastProject(file);
            Database.Migration();
        }

        public static string FileUri
        {
            get { return _file_uri; }
            set { _file_uri = value; }
        }
    }
}

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
        private static string FILE_NAME = "";
        private static string FILE_URI = "";

        public static void CreateNewProject(string file)
        {
            Database.Migration();
        }

        public static string GetCurrentFileDB()
        {
            return FILE_URI;
        }
    }
}

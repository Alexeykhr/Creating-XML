using System.IO;

namespace Creating_XML.core.db
{
    public abstract class Table {
        public static string[] GetTables()
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\tables", ".cs");

            return files;
        }
    }
}

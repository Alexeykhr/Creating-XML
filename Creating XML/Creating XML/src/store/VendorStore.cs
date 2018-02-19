using System.Linq;
using Creating_XML.src.db;
using Creating_XML.src.db.tables;
using System.Collections.Generic;

namespace Creating_XML.src.store
{
    class VendorStore
    {
        private static IEnumerable<VendorTable> _list;

        /// <summary>
        /// Set and get List of VendorTable.
        /// </summary>
        public static IEnumerable<VendorTable> List
        {
            get { return _list; }
            set { _list = value; }
        }

        /// <summary>
        /// Send query for new list.
        /// </summary>
        public static IEnumerable<VendorTable> FetchNewList()
        {
            _list = Database.List<VendorTable>().OrderBy(v => v.Name);

            return _list;
        }
    }
}

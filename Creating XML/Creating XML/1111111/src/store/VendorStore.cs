using System.Collections.Generic;
using Creating_XML.src.db.tables;
using Creating_XML.src.db;
using Creating_XML.core;
using System.Linq;

namespace Creating_XML.src.store
{
    class VendorStore : Store<VendorTable>
    {
        /// <summary>
        /// Send query for new list.
        /// </summary>
        public static IEnumerable<VendorTable> Fetch()
        {
            _list = Database.List<VendorTable>().OrderBy(v => v.Name);

            return _list;
        }
    }
}

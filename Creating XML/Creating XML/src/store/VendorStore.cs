using System.Linq;
using Creating_XML.src.db;
using Creating_XML.src.db.tables;
using System.Collections.Generic;

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

using System.Linq;
using Creating_XML.src.db;
using Creating_XML.src.db.tables;
using System.Collections.Generic;

namespace Creating_XML.src.store
{
    class CurrencyStore : Store<CurrencyTable>
    {
        /// <summary>
        /// Send query for get a new list.
        /// </summary>
        public static IEnumerable<CurrencyTable> Fetch()
        {
            _list = Database.List<CurrencyTable>().OrderBy(v => v.Name);

            return _list;
        }
    }
}

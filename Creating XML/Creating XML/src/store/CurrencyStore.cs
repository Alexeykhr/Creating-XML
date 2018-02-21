using System.Linq;
using Creating_XML.src.db;
using Creating_XML.src.db.tables;
using System.Collections.Generic;

namespace Creating_XML.src.store
{
    class CurrencyStore
    {
        private static IEnumerable<CurrencyTable> _list;

        /// <summary>
        /// Set and get List of VendorTable.
        /// </summary>
        public static IEnumerable<CurrencyTable> List
        {
            get
            {
                if (_list == null)
                    return FetchNewList();

                return _list;
            }
            set { _list = value; }
        }

        /// <summary>
        /// Send query for new list.
        /// </summary>
        public static IEnumerable<CurrencyTable> FetchNewList()
        {
            _list = Database.List<CurrencyTable>().OrderBy(v => v.Name);

            return _list;
        }
    }
}

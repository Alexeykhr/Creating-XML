using System.Linq;
using Creating_XML.src.db;
using Creating_XML.src.db.tables;
using System.Collections.Generic;

namespace Creating_XML.src.store
{
    class CategoryStore
    {
        private static IEnumerable<CategoryTable> _list;

        /// <summary>
        /// Set and get List of VendorTable.
        /// </summary>
        public static IEnumerable<CategoryTable> List
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
        public static IEnumerable<CategoryTable> FetchNewList()
        {
            _list = Database.List<CategoryTable>().OrderBy(v => v.Name);
            _list = BuildTree(_list.ToList());

            return _list;
        }

        public static List<CategoryTable> BuildTree(List<CategoryTable> items)
        {
            items.ForEach(i => i.Childrens = items.Where(ch => ch.ParentId == i.Id).ToList());

            return items.Where(i => i.ParentId == 0).ToList();
        }
    }
}

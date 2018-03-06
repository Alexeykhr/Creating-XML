using System.Linq;
using Creating_XML.src.db;
using Creating_XML.src.db.tables;
using System.Collections.Generic;

namespace Creating_XML.src.store
{
    class CategoryStore : Store<CategoryTable>
    {
        private static List<CategoryTable> _listTree;

        /// <summary>
        /// Set and get ListTree of CategoryTable.
        /// </summary>
        public static List<CategoryTable> ListTree
        {
            get { return _listTree; }
            set { _listTree = value; }
        }

        /// <summary>
        /// Send query for new list.
        /// </summary>
        public static IEnumerable<CategoryTable> Fetch(bool returnTree = true)
        {
            _list = Database.List<CategoryTable>().OrderBy(v => v.Name);
            _listTree = BuildTree(_list.ToList());

            if (returnTree)
                return _listTree;

            return _list;
        }

        /// <summary>
        /// Create tree from List<CategoryTable>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static List<CategoryTable> BuildTree(List<CategoryTable> items)
        {
            items.ForEach(i => i.Childrens = items.Where(ch => ch.ParentId == i.Id).ToList());

            return items.Where(i => i.ParentId == 0).ToList();
        }
    }
}

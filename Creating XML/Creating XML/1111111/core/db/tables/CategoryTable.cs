using System.Collections.Generic;
using SQLite;

namespace Creating_XML.core.db.tables
{
    public class CategoryTable : Table
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        [Indexed]
        public int ParentId { get; set; }

        [Unique, NotNull]
        public string Name { get; set; }

        [Ignore]
        public List<CategoryTable> Childrens { get; set; }
    }
}

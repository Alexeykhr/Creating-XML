using SQLite;
using System.Collections.Generic;

namespace Creating_XML.src.db.tables
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

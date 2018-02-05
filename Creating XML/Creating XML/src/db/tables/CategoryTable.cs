using SQLite;

namespace Creating_XML.src.db.tables
{
    class CategoryTable : Table
    {
        [AutoIncrement, PrimaryKey]
        public long Id { get; set; }

        [Indexed]
        public long ParentId { get; set; }

        [Unique, NotNull]
        public string Name { get; set; }
    }
}

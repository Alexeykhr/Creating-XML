using SQLite;

namespace Creating_XML.src.db.tables
{
    class CategoryParametersTable : Table
    {
        [AutoIncrement, PrimaryKey]
        public long Id { get; set; }

        [Indexed, NotNull]
        public long CategoryId { get; set; }
        
        [NotNull]
        public string Name { get; set; }
    }
}

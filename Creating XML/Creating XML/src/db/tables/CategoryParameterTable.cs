using SQLite;

namespace Creating_XML.src.db.tables
{
    class CategoryParameterTable : Table
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        [Indexed, NotNull]
        public int CategoryId { get; set; }
        
        [NotNull]
        public string Name { get; set; }
    }
}

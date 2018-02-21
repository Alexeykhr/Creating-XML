using SQLite;

namespace Creating_XML.src.db.tables
{
    class VendorTable : Table
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        [Unique, NotNull]
        public string Name { get; set; }
    }
}

using SQLite;

namespace Creating_XML.core.db.tables
{
    public class VendorTable : Table
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        [Unique, NotNull]
        public string Name { get; set; }
    }
}

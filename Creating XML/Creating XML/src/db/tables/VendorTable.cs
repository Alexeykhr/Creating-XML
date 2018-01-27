using SQLite;

namespace Creating_XML.src.db.tables
{
    class VendorTable
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        [Unique]
        public string Name { get; set; }
    }
}

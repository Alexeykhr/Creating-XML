using SQLite;

namespace Creating_XML.src.db.tables
{
    class CurrencyTable : Table
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        [Unique]
        public string Name { get; set; }

        public string Rate { get; set; }
    }
}

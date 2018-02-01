using SQLite;

namespace Creating_XML.src.db.tables
{
    class OfferParametersTable : Table
    {
        [AutoIncrement, PrimaryKey]
        public long Id { get; set; }

        [Indexed, NotNull]
        public long OfferId { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Value { get; set; }
    }
}

using SQLite;

namespace Creating_XML.src.db.tables
{
    class OfferImageTable : Table
    {
        [AutoIncrement, PrimaryKey]
        public long Id { get; set; }

        [Indexed, NotNull]
        public long OfferId { get; set; }
        
        [NotNull]
        public string Url { get; set; }
    }
}

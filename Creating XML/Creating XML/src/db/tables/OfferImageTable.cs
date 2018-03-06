using SQLite;

namespace Creating_XML.src.db.tables
{
    public class OfferImageTable : Table
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        [Indexed, NotNull]
        public int OfferId { get; set; }
        
        [NotNull]
        public string Url { get; set; }
    }
}

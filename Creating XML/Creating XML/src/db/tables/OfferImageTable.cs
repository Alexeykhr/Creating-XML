using SQLite;

namespace Creating_XML.src.db.tables
{
    class OfferImageTable : Table
    {
        [Indexed]
        public int OfferId { get; set; }

        public string Url { get; set; }
    }
}

using SQLite;

namespace Creating_XML.src.db.tables
{
    class OfferParametersTable : Table
    {
        [Indexed]
        public int OfferId { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}

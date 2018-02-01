using SQLite;

namespace Creating_XML.src.db.tables
{
    class OfferTable : Table
    {
        [AutoIncrement, PrimaryKey]
        public long Id { get; set; }

        public long Article { get; set; }

        [Indexed]
        public long CurrencyId { get; set; }

        [Indexed]
        public long CategoryId { get; set; }

        [Indexed]
        public long VendorId { get; set; }

        [Unique, NotNull]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public decimal Price { get; set; }

        [NotNull]
        public bool IsAvailable { get; set; }
    }
}

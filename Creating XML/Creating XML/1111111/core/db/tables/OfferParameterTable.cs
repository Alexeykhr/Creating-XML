using SQLite;

namespace Creating_XML.core.db.tables
{
    public class OfferParameterTable : Table
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        [Indexed(Name = "Parameter", Order = 1, Unique = true), NotNull]
        public int OfferId { get; set; }

        [Indexed(Name = "Parameter", Order = 2, Unique = true), NotNull]
        public string Name { get; set; }

        [Indexed(Name = "Parameter", Order = 3, Unique = true), NotNull]
        public string Value { get; set; }
    }
}

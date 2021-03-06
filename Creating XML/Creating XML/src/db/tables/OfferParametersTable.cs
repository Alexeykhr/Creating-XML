﻿using SQLite;

namespace Creating_XML.src.db.tables
{
    class OfferParametersTable : Table
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        [Indexed, NotNull]
        public int OfferId { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Value { get; set; }
    }
}

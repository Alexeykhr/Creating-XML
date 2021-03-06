﻿using SQLite;

namespace Creating_XML.src.db.tables
{
    class CurrencyTable : Table
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        [Unique, NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Rate { get; set; }
    }
}

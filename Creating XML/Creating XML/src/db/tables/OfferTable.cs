﻿using SQLite;

namespace Creating_XML.src.db.tables
{
    class OfferTable
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        
        public int Article { get; set; }

        [Indexed]
        public int CurrencyId { get; set; }

        [Indexed]
        public int CategoryId { get; set; }

        [Indexed]
        public int VendorId { get; set; }

        [Unique, NotNull]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public double Price { get; set; }

        public bool IsAvailable { get; set; }
    }
}

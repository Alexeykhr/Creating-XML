﻿using SQLite;

namespace Creating_XML.src.db.tables
{
    class CategoryTable : Table
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        [Indexed]
        public int ParentId { get; set; }

        [Unique, NotNull]
        public string Name { get; set; }
    }
}

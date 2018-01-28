using SQLite;

namespace Creating_XML.src.db.tables
{
    class CategoryParametersTable : Table
    {
        [Indexed]
        public int CategoryId { get; set; }
        
        public string Name { get; set; }
    }
}

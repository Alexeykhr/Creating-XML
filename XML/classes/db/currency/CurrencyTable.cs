using SQLite;

namespace XML.classes.db.currency
{
    class CurrencyTable
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }

        [Unique]
        public string CurrencyId { get; set; }
        
        public string Rate { get; set; }
    }
}
